using Elmah.Io.Extensions.Logging;

namespace DevIO.API.Configuration
{
    public static class LoggerConfig
    {
        public static IServiceCollection AddLoggingConfiguration(this IServiceCollection services)
        {
            services.AddElmahIo(o =>
            {
                o.ApiKey = "a655fb4a6e2c4547980e8c376af30ea4";
                o.LogId = new Guid("e624981c-efd5-43d0-a807-db8e21732c6b");
            });

            //services.AddLogging(builder =>
            //{
            //    builder.AddElmahIo(o =>
            //    {
            //        o.ApiKey = "a655fb4a6e2c4547980e8c376af30ea4";
            //        o.LogId = new Guid("e624981c-efd5-43d0-a807-db8e21732c6b");
            //    });
            //    builder.AddFilter<ElmahIoLoggerProvider>(null, LogLevel.Warning);
            //});            

            return services;
        }

        public static IApplicationBuilder UseLoggingConfiguration(this IApplicationBuilder app)
        {
            app.UseElmahIo();
            return app;
        }

    }
}
