using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using FYP2021.Models;
using Rotativa.AspNetCore;

namespace FYP2021
{
    public class Startup
    {

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public IConfiguration Configuration { get; }

        // For report to be in PDF/Printing
        public Startup(IConfiguration configuration,
            Microsoft.AspNetCore.Hosting.IHostingEnvironment env)
        {
            Configuration = configuration;
            RotativaConfiguration.Setup(env);
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc(options => options.EnableEndpointRouting = false);
            services.AddDbContext<AppDbContext>(
                  options =>
                     options
                        .UseSqlServer(
                             Configuration.GetConnectionString("DefaultConnection")));
            // adding authentication handler for Account using authentication scheme "AdminAccount"
            services
               .AddAuthentication("AdminAccount")
               .AddCookie("AdminAccount",
                   options =>
                   {
                       options.LoginPath = "/AdminAccount/Login/";
                       options.AccessDeniedPath = "/AdminAccount/Forbidden/";
                   });


            // adding authentication handler for Account using authentication scheme "StudentAccount"
            services
               .AddAuthentication("StudentAccount")
               .AddCookie("StudentAccount",
                   options =>
                   {
                       options.LoginPath = "/StudentAccount/Login/";
                       options.AccessDeniedPath = "/StudentAccount/Forbidden/";
                   });

        }


        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseStaticFiles();
            app.UseAuthentication();
            app.UseMvc(
               routes =>
               {
                   routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
               });

        }
    }
}
