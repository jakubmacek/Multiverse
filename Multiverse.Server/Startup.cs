using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using Multiverse.Server.Persistence;
using System;

namespace Multiverse.Server
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.Configure<Authorization.AuthenticationSettings>(Configuration.GetSection("Authentication"));

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, options =>
                {
                    options.Events = new JwtBearerEvents()
                    {
                        OnMessageReceived = async (context) =>
                        {
                            if (context.Request.Headers["Authorization"].Count == 0)
                            {
                                var authorizationCookie = context.Request.Cookies["Authorization"];
                                if (authorizationCookie != null && authorizationCookie.Length > 0)
                                    context.Token = authorizationCookie;
                            }
                        }
                    };
                    options.TokenValidationParameters.ValidateIssuerSigningKey = true;
                    options.TokenValidationParameters.IssuerSigningKey = new Microsoft.IdentityModel.Tokens.SymmetricSecurityKey(System.Text.Encoding.ASCII.GetBytes(Configuration["Authentication:JwtSecret"]));
                    options.TokenValidationParameters.ValidateIssuer = false;
                    options.TokenValidationParameters.ValidateAudience = false;
                    options.TokenValidationParameters.ClockSkew = TimeSpan.Zero;
                });

            services.AddControllers();

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Multiverse.Server", Version = "v1" });
            });

            //services.AddGrpc();

            services.AddTransient(s => SessionFactoryCreator.CreateConfiguration(Configuration.GetConnectionString("Default")));

            services.AddSingleton(s => SessionFactoryCreator.Create(s.GetRequiredService<NHibernate.Cfg.Configuration>()));

            services.AddTransient(s => s.GetRequiredService<NHibernate.ISessionFactory>().OpenSession());

            services.AddSingleton<IRepositoryFactoryFactory>(s => new Multiverse.Persistence.NHibernate.NHibernateRepositoryFactoryFactory(() => s.GetRequiredService<NHibernate.Cfg.Configuration>()));

            services.AddSingleton(s =>
            {
                var registrations = new UniverseRegistrations(s.GetRequiredService<IRepositoryFactoryFactory>());
                registrations.RegisterFromAssembly(@"X:\Dropbox\Projects\Multiverse\Multiverse.SimpleUniverse\bin\Debug\net5.0\Multiverse.SimpleUniverse.dll"); // TODO read from configuration / environment / command line
                return registrations;
            });

            services.AddSingleton(s =>
            {
                var allowedWorlds = new AllowedRunningWorlds();
                allowedWorlds.Add(1); // TODO read from configuration / environment / command line
                return allowedWorlds;
            });

            services.AddSingleton<RunningWorlds>();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Multiverse.Server v1"));
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();

                //endpoints.MapGrpcService<GreeterService>();

                //endpoints.MapGet("/", async context =>
                //{
                //    await context.Response.WriteAsync("Communication with gRPC endpoints must be made through a gRPC client. To learn how to create a client, visit: https://go.microsoft.com/fwlink/?linkid=2086909");
                //});
            });
        }

        public static void Main(string[] args)
        {
#pragma warning disable DF0001 // Marks undisposed anonymous objects from method invocations.
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                })
                .Build()
                .Run();
#pragma warning restore DF0001 // Marks undisposed anonymous objects from method invocations.
        }
    }
}
