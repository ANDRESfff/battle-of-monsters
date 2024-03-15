using API.Extensions;

namespace API
{
    public static class AppConfig
    {
        public static WebApplication Config(WebApplicationBuilder builder)
        {
            builder.Services.ConfigureCors();
            builder.Services.AddApplicationServices();
            builder.Services.ConfigureServices(builder.Configuration);
            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            WebApplication app = builder.Build();
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }
            app.UseCors("CorsPolicy");
            app.UseAuthorization();
            app.MapControllers();
            return app;
        }
    }
}
