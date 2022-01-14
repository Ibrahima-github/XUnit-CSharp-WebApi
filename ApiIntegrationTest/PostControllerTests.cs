using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Reflection;
using WebAPI.Controllers;
using WebAPI.Models;
using Xunit;

namespace ApiIntegrationTest
{
    public class PostControllerTests
    {
        protected readonly IConfiguration _configuration;
        protected readonly IWebHostEnvironment _env;
        [Fact]
        public void PostsController_PostsFromDatabase()
        {
            DbContextOptionsBuilder<BlogDBContext> optionsBuilder = new();
            optionsBuilder.UseInMemoryDatabase(MethodBase.GetCurrentMethod().Name);

            using(BlogDBContext ctx = new BlogDBContext(optionsBuilder.Options))
            {
                ctx.Add(new Post()
                {
                    AdsImageFileName = "anonymous.png",
                    AdsLink = "https://www.crypto.com",
                    AdsTitle = "Affiliation",
                    Category = "Informatique",
                    PostDescription = "Ceci est un post",
                    PostName = "Test post",
                    PostYoutubeHref = "https://www.youtube.com",
                    PostId = 3,
                });
                ctx.SaveChanges();
            }
            IActionResult result;
            using(BlogDBContext ctx = new BlogDBContext(optionsBuilder.Options))
            {
                result = new PostsController(_configuration, _env, ctx).Get();

            }
            var okResult = Assert.IsType<OkObjectResult>(result);

            var posts = Assert.IsType<List<Post>>(okResult.Value);

            var post = Assert.Single(posts);

            Assert.NotNull(posts);

            Assert.Equal(1, post.PostId);
            Assert.Equal("Test post", post.PostName);
            Assert.Equal("Informatique", post.Category);
            Assert.Equal("Ceci est un post", post.PostDescription);
            Assert.Equal("https://www.youtube.com", post.PostYoutubeHref);
            Assert.Equal("Affiliation", post.AdsTitle);
            Assert.Equal("anonymous.png", post.AdsImageFileName);
            Assert.Equal("https://www.crypto.com", post.AdsLink);
        }
    }
}
