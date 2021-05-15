using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using Projektarbete_WebApi_EJ_JA.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Projektarbete_WebApi_EJ_JA.Models;
using System.IO;
using Microsoft.AspNetCore.Mvc.Versioning;
using Swashbuckle.AspNetCore.SwaggerGen;
using Microsoft.Extensions.Options;
using Swashbuckle.Swagger;
using IDocumentFilter = Swashbuckle.Swagger.IDocumentFilter;
using IOperationFilter = Swashbuckle.Swagger.IOperationFilter;
using Projektarbete_WebApi_EJ_JA.options;

namespace Projektarbete_WebApi_EJ_JA
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }


        private static void configureSwaggerGen(SwaggerGenOptions options)
        {
            addSwaggerDocs(options);

            options.OperationFilter<RemoveVersionFromParameter>();
            options.DocumentFilter<ReplaceVersionWithExactValueInPath>();

            options.DocInclusionPredicate((version, desc) =>
            {
                if (!desc.TryGetMethodInfo(out var methodInfo))
                    return false;

                var versions = methodInfo
                   .DeclaringType?
               .GetCustomAttributes(true)
               .OfType<ApiVersionAttribute>()
               .SelectMany(attr => attr.Versions);

                var maps = methodInfo
                   .GetCustomAttributes(true)
               .OfType<MapToApiVersionAttribute>()
               .SelectMany(attr => attr.Versions)
               .ToList();

                return versions?.Any(v => $"v{v}" == version) == true
                         && (!maps.Any() || maps.Any(v => $"v{v}" == version));
            });
        }


        private static void addSwaggerDocs(SwaggerGenOptions options)
        {
            options.SwaggerDoc("v1", new OpenApiInfo
            {
                Version = "v1",
                Title = "Projektarbete_WebApi_EJ_JA",
                Description = "Projektarbete_WebApi_EJ_JA V2",
            });

            options.SwaggerDoc("v2", new OpenApiInfo
            {
                Version = "v2",
                Title = "Projektarbete_WebApi_EJ_JA",
                Description = "Projektarbete_WebApi_EJ_JA V2",
            });
        }


        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<UserDbContext>(options => 
            options.UseSqlServer(@"Server=(localdb)\mssqllocaldb;Database=GeoMessages"));

            services.AddControllers();

            services.AddApiVersioning(o =>
            {
                o.AssumeDefaultVersionWhenUnspecified = true;
                o.DefaultApiVersion = new ApiVersion(1, 0);
            });


            services.AddSwaggerGen(configureSwaggerGen);


            services.AddSwaggerGen(c =>
            {
     
                var xmlPath = Path.Combine(AppContext.BaseDirectory, "Documentation.xml");
                c.IncludeXmlComments(xmlPath);

                c.EnableAnnotations();

                c.AddSecurityDefinition("basic", new OpenApiSecurityScheme
                {
                    Name = "Authorization",
                    Type = SecuritySchemeType.Http,
                    Scheme = "basic",
                    In = ParameterLocation.Header,
                    Description = "Basic Authorization header using the Bearer scheme."
                });




                c.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "basic"
                            }
                        },
                        new string[] {}
                    }
                });
            });

            services.AddDbContext<UserDbContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));

            services.AddIdentityCore<User>()
                .AddEntityFrameworkStores<UserDbContext>();

            services.AddAuthentication("BasicAuthentication")
               .AddScheme<AuthenticationSchemeOptions, BasicAuthenticationHandler>("BasicAuthentication", null);

            
        }


        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c =>
                {
                    c.SwaggerEndpoint($"/swagger/v1/swagger.json", "Projektarbete_WebApi_EJ_JA v1");
                    c.SwaggerEndpoint($"/swagger/v2/swagger.json", "Projektarbete_WebApi_EJ_JA v2");
                    c.DisplayOperationId();
                    c.DisplayRequestDuration();
                });
            };


            app.UseHttpsRedirection();
            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }


    }
}
