using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using Multiverse.Server.Persistence;
using System;
using System.Linq;

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
                        OnMessageReceived = (context) =>
                        {
                            if (context.Request.Headers["Authorization"].Count == 0)
                            {
                                var authorizationCookie = context.Request.Cookies["Authorization"];
                                if (authorizationCookie != null && authorizationCookie.Length > 0)
                                    context.Token = authorizationCookie;
                            }
                            return System.Threading.Tasks.Task.CompletedTask;
                        }
                    };
                    options.TokenValidationParameters.ValidateIssuerSigningKey = true;
                    options.TokenValidationParameters.IssuerSigningKey = new Microsoft.IdentityModel.Tokens.SymmetricSecurityKey(System.Text.Encoding.ASCII.GetBytes(Configuration["Authentication:JwtSecret"]));
                    options.TokenValidationParameters.ValidateIssuer = false;
                    options.TokenValidationParameters.ValidateAudience = false;
                    options.TokenValidationParameters.ClockSkew = TimeSpan.Zero;
                });

            services.AddControllers();

            services.AddSingleton<Microsoft.AspNetCore.Identity.PasswordHasher<Authorization.User>>();

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Multiverse.Server", Version = "v1" });
            });

            //services.AddGrpc();

            services.AddTransient(s => SessionFactoryCreator.CreateConfiguration(Configuration.GetConnectionString("Default")));

            services.AddSingleton(s => SessionFactoryCreator.Create(s.GetRequiredService<NHibernate.Cfg.Configuration>()));

#pragma warning disable DF0001 // Marks undisposed anonymous objects from method invocations.
            services.AddTransient(s => s.GetRequiredService<NHibernate.ISessionFactory>().OpenSession());
#pragma warning restore DF0001 // Marks undisposed anonymous objects from method invocations.

            services.AddSingleton<IRepositoryFactoryFactory>(s => new Multiverse.Persistence.NHibernate.NHibernateRepositoryFactoryFactory(() => s.GetRequiredService<NHibernate.Cfg.Configuration>()));


            var allowedWorldsString = Configuration["Multiverse:AllowedWorlds"];
            if (string.IsNullOrEmpty(allowedWorldsString))
                throw new Exception("Missing configuration variable Multiverse:AllowedWorlds");
            var allowedWorlds = new AllowedRunningWorlds();
            allowedWorlds.AddRange(allowedWorldsString.Split(',').Select(x => int.Parse(x)));
            services.AddSingleton(allowedWorlds);

            services.AddSingleton(s =>
            {
                var universesString = Configuration["Multiverse:Universes"];
                if (string.IsNullOrEmpty(universesString))
                    throw new Exception("Missing configuration variable Multiverse:Universes");

#pragma warning disable DF0001 // Marks undisposed anonymous objects from method invocations. IRepositoryFactoryFactory is global singleton, it should not be disposed.
                var registrations = new UniverseRegistrations(s.GetRequiredService<IRepositoryFactoryFactory>());
#pragma warning restore DF0001 // Marks undisposed anonymous objects from method invocations.
                foreach (var universeImplementationPath in universesString.Split(';'))
                    registrations.RegisterFromAssembly(universeImplementationPath);
                return registrations;
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

            //app.UseHttpsRedirection();

            app.UseCors(policy =>
            {
                policy.WithOrigins("https://localhost:20013").AllowAnyMethod().AllowAnyHeader();
            });

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
