using Microsoft.AspNetCore.Builder;

namespace API.Test
{
    public class ProgramTests
    {
        [Fact]
        public void AppConfigTest()
        {
            WebApplicationBuilder webApplicationBuilder = WebApplication.CreateBuilder(new string[] { });
            webApplicationBuilder.Environment.EnvironmentName = "Development";

            var app = AppConfig.Config(webApplicationBuilder);

            Assert.NotNull(app.Services);

        }
    }
}
