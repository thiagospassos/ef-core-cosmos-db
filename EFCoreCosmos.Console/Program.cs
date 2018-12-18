using Autofac;
using Autofac.Extensions.DependencyInjection;
using AutofacSerilogIntegration;
using EFCoreCosmos.Application.Post.Commands;
using EFCoreCosmos.Application.Post.Models;
using EFCoreCosmos.Application.Post.Queries;
using EFCoreCosmos.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using Serilog.Core;
using ServiceStack.Text;
using System;
using IContainer = Autofac.IContainer;

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
            var query = Container.Resolve<IGetPostsQuery>();

            while (true)
            {
                Console.Write("Do you want to create a post? (y/n): ");
                var key = Console.ReadKey();
                Console.WriteLine("");
                if (key.KeyChar != 'y')
                {
                    break;
                }

                var post = new PostModel();
                Console.WriteLine("Who's the author?");
                post.Author = Console.ReadLine();
                Console.WriteLine("What's the category?");
                post.Category = Console.ReadLine();
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

            Console.Write(query.Execute().Dump());

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

            builder.RegisterAssemblyTypes(typeof(AddPostCommand).Assembly)
                .Where(x => x.Name.EndsWith("Command") || x.Name.EndsWith("Query"))
                .AsImplementedInterfaces();

            serviceCollection.AddEntityFrameworkCosmos();
            serviceCollection.AddDbContext<BlogDbContext>(options =>
            {
                options.UseCosmos(
                    Configuration["CosmosDb:EndpointUrl"],
                    Configuration["CosmosDb:PrivateKey"],
                    Configuration["CosmosDb:DbName"]);
            });

            Log.Logger = CreateLogger(Configuration);
            builder.RegisterLogger();

            builder.Populate(serviceCollection);
            IContainer container = builder.Build();

            container.Resolve<BlogDbContext>().Database.EnsureCreated();

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
