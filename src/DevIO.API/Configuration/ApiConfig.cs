﻿using Microsoft.AspNetCore.Mvc;

namespace DevIO.API.Configuration
{
    public static class ApiConfig
    {
        public static IServiceCollection AddApiConfig(this IServiceCollection services)
        {
            // Add services to the container.
            services.AddControllers();

            services.AddMvc();

            services.AddApiVersioning(options =>
            {
                options.AssumeDefaultVersionWhenUnspecified = true;
                options.DefaultApiVersion = new ApiVersion(1, 0);
                options.ReportApiVersions = true;   
            });

            services.AddVersionedApiExplorer(options =>
            {
                options.GroupNameFormat = "'v'VVV";
                options.SubstituteApiVersionInUrl = true;
            });

            services.Configure<ApiBehaviorOptions>(options =>
            {
                options.SuppressModelStateInvalidFilter = true;
            });

            services.AddEndpointsApiExplorer();

            services.AddCors(options =>
            {
                options.AddPolicy("Development",
                    builder =>
                        builder
                        .AllowAnyOrigin()
                        .AllowAnyMethod()
                        .AllowAnyHeader());

                options.AddPolicy("Production",
                   builder =>
                       builder
                           .WithMethods("GET")
                           .WithOrigins("http://desenvolvedor.io")
                           .SetIsOriginAllowedToAllowWildcardSubdomains()
                           //.WithHeaders(HeaderNames.ContentType, "x-custom-header")
                           .AllowAnyHeader());
            });




            return services;
        }

        public static IApplicationBuilder UseApiConfig(this IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseCors("Development");                
            }
            else
            {
                app.UseCors("Development");// Usar apenas nas demos => Configuração Ideal: 
                app.UseHsts();
            }

            app.UseAuthentication();

            app.UseHttpsRedirection();

            app.UseAuthorization();            

            return app;
        }
    }
}
