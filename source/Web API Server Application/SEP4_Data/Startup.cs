using System.Security.Cryptography;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using SEP4_Data.Data;

namespace SEP4_Data
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }
        
        private IConfigService _configService;
        
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
            _configService = new ConfigService();
            IPersistenceService persistenceService = new PersistenceService(_configService);
            services.AddSingleton(init => _configService);
            services.AddSingleton(init => persistenceService);
            if (_configService.ReInitializeDb)
            {
                var bytes1 = new byte[128 / 8];
                var bytes2 = new byte[128 / 8];
                using var generator = RandomNumberGenerator.Create();
                generator.GetBytes(bytes1);
                generator.GetBytes(bytes2);
                _configService.Salt = bytes1;
                _configService.JwtKey = bytes2;
                persistenceService.DropSchema();
                persistenceService.InitSchema();
            }
            ILogService logService = new LogService();
            services.AddSingleton(init => logService);
            ISampleService sampleService = new SampleService(_configService, logService, persistenceService);
            services.AddSingleton(init => sampleService);
            services.AddSwaggerGen(c => {
                c.SwaggerDoc("v1", new OpenApiInfo {Title = "Mushroom++", Version = "v1"});
                // add JWT Authentication
                var securityScheme = new OpenApiSecurityScheme
                {
                    Name = "Bearer Authentication",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.Http,
                    Scheme = "bearer",
                    BearerFormat = "JWT",
                    Reference = new OpenApiReference {
                        Id = JwtBearerDefaults.AuthenticationScheme,
                        Type = ReferenceType.SecurityScheme
                    }
                };
                c.AddSecurityDefinition(securityScheme.Reference.Id, securityScheme);
                c.AddSecurityRequirement(new OpenApiSecurityRequirement {
                    {securityScheme, System.Array.Empty<string>()}
                });
                // add Basic Authentication
                var basicSecurityScheme = new OpenApiSecurityScheme {
                    Type = SecuritySchemeType.Http,
                    Scheme = "basic",
                    Reference = new OpenApiReference {
                        Id = "BasicAuth",
                        Type = ReferenceType.SecurityScheme
                    }
                };
                c.AddSecurityDefinition(basicSecurityScheme.Reference.Id, basicSecurityScheme);
                c.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {basicSecurityScheme, System.Array.Empty<string>()}
                });
            });
        }
        
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            if (env.IsDevelopment() || _configService.Swagger)
            {
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "MushroomPP"));
            }

            app.UseRouting();

            app.UseMiddleware<JwtMiddleware>();

            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
        }
    }
}