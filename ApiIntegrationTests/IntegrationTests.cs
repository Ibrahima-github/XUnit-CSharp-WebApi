using Microsoft.AspNetCore.Mvc.Testing;
using System;
using System.Net.Http;
using WebAPI;
using Xunit;

namespace ApiIntegrationTests
{
    public class IntegrationTests
    {
        protected readonly HttpClient TestClient;

        protected  IntegrationTests()
        {
            var appFactory = new WebApplicationFactory<Startup>();
            TestClient = appFactory.CreateClient();
        }
    }
}
