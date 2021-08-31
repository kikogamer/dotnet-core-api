using DEVIO.Api.Extensions;
using Elmah.Io.AspNetCore;
using Elmah.Io.AspNetCore.HealthChecks;
using Elmah.Io.Extensions.Logging;
using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace DEVIO.Api.Configuration
{
    public static class LoggerConfig
    {
        public static IServiceCollection AddLoggingConfiguration(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddElmahIo(o =>
            {
                o.ApiKey = "2d2721ed866e404797bfbcbcd72b6b6c";
                o.LogId = new Guid("58222650-576f-45b8-998b-0fe3d5940d31");
            });

            // configuração para pegar logs injetados manualmente
            //services.AddLogging(builder =>
            //{
            //    builder.AddElmahIo(o =>
            //        {
            //            o.ApiKey = "2d2721ed866e404797bfbcbcd72b6b6c";
            //            o.LogId = new Guid("58222650-576f-45b8-998b-0fe3d5940d31");
            //        });
            //    builder.AddFilter<ElmahIoLoggerProvider>(null, LogLevel.Warning);
            //});

            services.AddHealthChecks()
                .AddElmahIoPublisher(apiKey: "2d2721ed866e404797bfbcbcd72b6b6c", new Guid("58222650-576f-45b8-998b-0fe3d5940d31"))
                .AddCheck("Produtos", new SqlServerHealthCheck(configuration.GetConnectionString("DefaultConnection")))
                .AddSqlServer(configuration.GetConnectionString("DefaultConnection"), name: "BancoSQL");

            services.AddHealthChecksUI();

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

            app.UseHealthChecksUI(options =>
            {
                options.UIPath = "/api/hc-ui";
                options.ResourcesPath = "/ui/resources";
            });

            return app;
        }
    }
}
