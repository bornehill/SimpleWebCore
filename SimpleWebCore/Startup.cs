using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using SimpleWebCore.Data;
using SimpleWebCore.Data.Entities;
using SimpleWebCore.Services;

namespace SimpleWebCore
{
    public class Startup
    {
        private readonly IConfiguration _configuration;
        private readonly IHostingEnvironment _env;

        public Startup(IConfiguration configuration, IHostingEnvironment env) {
            _configuration = configuration;
            _env = env;
        }
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddIdentity<StoreUser, IdentityRole>(cfg => {
                cfg.User.RequireUniqueEmail = true;
            })
            .AddEntityFrameworkStores<SimpleWebCoreContext>();

            services.AddAuthentication()
                .AddCookie()
                .AddJwtBearer(cfg => {
                    cfg.TokenValidationParameters = new TokenValidationParameters()
                    {
                        ValidIssuer = _configuration["Tokens:Issuer"],
                        ValidAudience = _configuration["Tokens:Audience"],
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Tokens:Key"]))
                    };
                });

            services.AddDbContext<SimpleWebCoreContext>(cfg => { cfg.UseSqlServer(_configuration.GetConnectionString("SimpleWebCoreConnection")); });
            services.AddAutoMapper();

            services.AddTransient<IMailService, NullMailService>();

            services.AddTransient<SimpleSeeder>();

            services.AddScoped<ISimpleWebCoreRepository, SimpleWebCoreRepository>();
            
            services.AddMvc(opt => {
                if (_env.IsProduction()) {
                    opt.Filters.Add(new RequireHttpsAttribute());
                }
            })
            .AddJsonOptions(opt => opt.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            //I comment becuse is not necesari
            //if (env.IsDevelopment())
            //{
            //    app.UseDeveloperExceptionPage();
            //}

            //app.Run(async (context) =>
            //{
            //    //this print this msg in all url
            //    //await context.Response.WriteAsync("Hello World!");

            //    //this print this page in all url
            //    await context.Response.WriteAsync("<html><body><h1>My simple web</h1></body></html>");
            //});

            //use default files in wwwroot
            //app.UseDefaultFiles();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else {
                app.UseExceptionHandler("/error");
            }

            //service files in wwwroot
            app.UseStaticFiles();

            app.UseAuthentication();

            app.UseMvc(cfg => {
                cfg.MapRoute("Default", "{controller}/{action}/{id?}", new { controller = "App", Action = "Index" });
            });

            if (env.IsDevelopment()) {
                //Seed the database
                using (var scope = app.ApplicationServices.CreateScope()) {
                    var seeder = scope.ServiceProvider.GetService<SimpleSeeder>();
                    seeder.Seed().Wait();
                }
            }
        }
    }
}
