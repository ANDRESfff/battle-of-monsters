using System.Diagnostics.CodeAnalysis;

namespace API
{
    [ExcludeFromCodeCoverage]
    public class Program
    {
        public static void Main(string[] args)
        {
            WebApplicationBuilder builder = WebApplication.CreateBuilder(args);
            var app = AppConfig.Config(builder);
            app.Run();
        }
    }
}
