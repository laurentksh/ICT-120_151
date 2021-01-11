using ICT_151.Authentication;
using ICT_151.Data;
using ICT_151.Repositories;
using ICT_151.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ICT_151
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "ICT-151",
                    Version = "v1",
                });
                c.OperationFilter<OptionalHttpHeaderOperationFilter>(SessionTokenAuthOptions.TokenHeaderName, "Token Header OF");
                c.AddSecurityDefinition(SessionTokenAuthOptions.DefaultSchemeName, new OpenApiSecurityScheme()
                {
                    Type = SecuritySchemeType.ApiKey,
                    In = ParameterLocation.Header,
                    Name = SessionTokenAuthOptions.TokenHeaderName,
                    Description = @"Session token, 64-length randomly generated string (a-zA-Z0-9+@*#%&/\|()=?^-_.,:;���$�<>)",
                });
                c.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference()
                            {
                                Id = SessionTokenAuthOptions.TokenHeaderName,
                                Type = ReferenceType.SecurityScheme
                            }
                        },
                        Array.Empty<string>()
                    }
                });
            });

            services.AddDbContext<ApplicationDbContext>(x => x.UseSqlite("DataSource=AppDb.db"), ServiceLifetime.Scoped);

            services
                .AddAuthentication(options =>
                {
                    options.DefaultAuthenticateScheme = SessionTokenAuthOptions.DefaultSchemeName;
                    options.DefaultChallengeScheme = SessionTokenAuthOptions.DefaultSchemeName;
                })
                .AddScheme<SessionTokenAuthOptions, SessionTokenAuthenticationHandler>(SessionTokenAuthOptions.DefaultSchemeName, "Session Token Authentication", opts =>
            {
                opts.Validate();
            });

            services.AddAuthorization();

            services.AddTransient<IUserService, UserService>();
            services.AddTransient<IUserRepository, UserRepository>();
            services.AddSingleton<IExceptionHandlerService, DefaultExceptionHandlerService>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment()) {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "ICT-151 v1"));
            }

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
