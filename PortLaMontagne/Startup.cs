using AspNetCore.ReCaptcha;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using PortLaMontagne.Data;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using MimeKit;
using PortLaMontagne.Models;
using PortLaMontagne.Services;

namespace PortLaMontagne
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
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlite(
                    Configuration.GetConnectionString("DefaultConnection")));
            services.AddDatabaseDeveloperPageExceptionFilter();

            services.AddDefaultIdentity<ApplicationUser>(options => options.SignIn.RequireConfirmedAccount = true)
                .AddRoles<IdentityRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>();
                
            services.AddControllersWithViews();

            services.AddRazorPages().AddRazorRuntimeCompilation();
            
            //FLASHER
            services.AddFlashes();
            services.AddMvc();
            
            //RECAPCHA
            services.AddReCaptcha(Configuration.GetSection("ReCaptcha"));
            
            //SERVICES 
            services.AddTransient<MailerService>();
            services.AddTransient<MimeMessage>();
            services.AddTransient<UploadFile>();



        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseMigrationsEndPoint();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                //LAYOUT
                endpoints.MapControllerRoute(name: "Layout",
                    pattern: "layout/weathermap",
                    defaults: new { controller = "Layout", action = "WeatherMap"});
                
                // //ARTICLE
                // endpoints.MapControllerRoute(name: "Article",
                //     pattern: "articles",
                //     defaults: new { controller = "Article", action = "Index"});
                // endpoints.MapControllerRoute(name: "Article",
                //     pattern: "articles/{slug}",
                //     defaults: new { controller = "Article", action = "Details"});
                // endpoints.MapControllerRoute(name: "Article",
                //     pattern: "articles/create",
                //     defaults: new { controller = "Article", action = "Create"});
                
                //CONTACT
                endpoints.MapControllerRoute(name: "Contact",
                    pattern: "contact",
                    defaults: new { controller = "Contact", action = "Index"});
                
                //DEFAULT
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
                endpoints.MapRazorPages();
            });
        }
    }
}