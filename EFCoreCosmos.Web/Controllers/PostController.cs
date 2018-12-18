using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EFCoreCosmos.Application.Post.Commands;
using EFCoreCosmos.Application.Post.Models;
using EFCoreCosmos.Application.Post.Queries;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EFCoreCosmos.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PostController : ControllerBase
    {
        [HttpGet("GetAll")]
        public async Task<IEnumerable<PostModel>> GetAll([FromServices] IGetPostsQuery query)
        {
            return await query.Execute();
        }

        [HttpPost()]
        public async Task<PostModel> Post([FromServices] IAddPostCommand cmd,[FromBody] PostModel model)
        {
            return await cmd.Execute(model);
        }
    }
}