using Autofac;
using Autofac.Extensions.DependencyInjection;
using AutofacSerilogIntegration;
using EFCoreCosmoDbSample.Application;
using EFCoreCosmoDbSample.Domain;
using EFCoreCosmoDbSample.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using Serilog.Core;
using System;

namespace EFCoreCosmoDbSample
{
    class Program
    {
        private static IConfiguration Configuration { get; set; }
        private static IContainer Container { get; set; }

        static void Main(string[] args)
        {
            ConfigureServices();

            var cmd = Container.Resolve<IAddPostCommand>();

            while (true)
            {
                Console.Write("Do you want to create a post? (y/n): ");
                var key = Console.ReadKey();
                Console.WriteLine("");
                if (key.KeyChar != 'y')
                {
                    break;
                }

                var post = new Post();
                Console.WriteLine("Who's the author?");
                post.Author = Console.ReadLine();
                Console.WriteLine("");
                Console.WriteLine("Give me a title:");
                post.Title = Console.ReadLine();
                Console.WriteLine("");
                Console.WriteLine("Write the first line of the post. Then you're done! :D");
                post.Content = Console.ReadLine();
                Console.WriteLine("");

                var entity = cmd.Execute(post).GetAwaiter().GetResult();
                Console.WriteLine($"The newly created post's id is {entity.Id}");
            }

            Console.WriteLine("----------- Press any key to get out -----------");
            Console.ReadKey();
        }

        static void ConfigureServices()
        {
            var configBuilder = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appsettings.Local.json", optional: true);

            Configuration = configBuilder.Build();
            Container = IocConfig();
        }

        public static IContainer IocConfig()
        {
            var serviceCollection = new ServiceCollection();
            var builder = new ContainerBuilder();

            builder.RegisterType<AddPostCommand>().As<IAddPostCommand>();

            serviceCollection.AddEntityFrameworkCosmosSql();
            serviceCollection.AddDbContext<BlogDbContext>(options =>
            {
                options.UseCosmosSql(
                    new Uri(Configuration["CosmosDb:EndpointUrl"]),
                    Configuration["CosmosDb:PrivateKey"],
                    Configuration["CosmosDb:DbName"]);
            });

            Log.Logger = CreateLogger(Configuration);
            builder.RegisterLogger();

            builder.Populate(serviceCollection);
            IContainer container = builder.Build();
            return container;
        }

        public static Logger CreateLogger(IConfiguration configuration)
        {
            return new LoggerConfiguration()
                .ReadFrom.Configuration(configuration)
                .Enrich.WithProperty("ApplicationName", typeof(Program).Assembly.GetName().Name)
                .Enrich.WithProperty("AppDomain", AppDomain.CurrentDomain)
                .Enrich.WithProperty("RuntimeVersion", Environment.Version)
                .CreateLogger();
        }
    }
}
