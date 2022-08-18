using DevIO.API.Extensions;
using Elmah.Io.Extensions.Logging;
using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;

namespace DevIO.API.Configuration
{
    public static class LoggerConfig
    {
        public static IServiceCollection AddLoggingConfiguration(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddElmahIo(o =>
            {
                o.ApiKey = "a655fb4a6e2c4547980e8c376af30ea4";
                o.LogId = new Guid("e624981c-efd5-43d0-a807-db8e21732c6b");
            });

            services.AddLogging(builder =>
            {
                builder.AddElmahIo(o =>
                {
                    o.ApiKey = "a655fb4a6e2c4547980e8c376af30ea4";
                    o.LogId = new Guid("e624981c-efd5-43d0-a807-db8e21732c6b");
                });
                builder.AddFilter<ElmahIoLoggerProvider>(null, LogLevel.Warning);
            });

            services.AddHealthChecks()
                .AddElmahIoPublisher(options =>
                {
                    options.ApiKey = "a655fb4a6e2c4547980e8c376af30ea4";
                    options.LogId = new Guid("e624981c-efd5-43d0-a807-db8e21732c6b");
                    options.HeartbeatId = "API Fornecedores";
                })
                .AddCheck("Produtos", new SqlServerHealthCheck(configuration.GetConnectionString("DefaultConnection")))
                .AddSqlServer(configuration.GetConnectionString("DefaultConnection"), name: "BancoSQL");

            //services.AddHealthChecksUI()
            //    .AddSqlServerStorage(configuration.GetConnectionString("DefaultConnection"));

            return services;
        }

        public static IApplicationBuilder UseLoggingConfiguration(this IApplicationBuilder app)
        {
            app.UseElmahIo();

            app.UseHealthChecks("/api/hc", new HealthCheckOptions()
            {
                Predicate = _ => true,
                ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
            });

            //app.UseHealthChecksUI(options =>
            //{
            //    options.UIPath = "/api/hc-ui";

            //    options.ResourcesPath = "/api/hc-ui-resources";

            //    options.UseRelativeApiPath = false;
            //    options.UseRelativeResourcesPath = false;
            //    options.UseRelativeWebhookPath = false;
            //});



            return app;
        }

    }
}
