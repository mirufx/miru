using System;
using Hangfire;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Miru;
using Miru.Behaviors.BelongsToUser;
using Miru.Foundation.Hosting;
using Miru.Foundation.Logging;
using Miru.Mailing;
using Miru.Mvc;
using Miru.Pipeline;
using Miru.Queuing;
using Miru.Sqlite;
using Miru.Userfy;
using Serilog.Events;
using Supportreon.Config;
using Supportreon.Database;
using Supportreon.Domain;

namespace Supportreon
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMiru<Startup>()
                .AddSerilogConfig(_ =>
                {
                    _.EntityFrameworkSql(LogEventLevel.Information);
                    _.Authentication(LogEventLevel.Information);
                })
                
                .AddDefaultPipeline<Startup>()
                
                .AddEfCoreSqlite<SupportreonDbContext>()
                
                .AddMailing(_ =>
                {
                    _.EmailDefaults(email => email.From("noreply@Supportreon.com", "Supportreon"));
                })
                .AddSenderStorage()

                .AddQueuing(_ => 
                {
                    _.UseLiteDb();
                })
                .AddHangfireServer()
            
                // user register, login, logout
                .AddUserfy<User, SupportreonDbContext>(
                    cookie: cfg =>
                    {
                        cfg.Cookie.Name = App.Name;
                        cfg.Cookie.HttpOnly = true;
                        cfg.ExpireTimeSpan = TimeSpan.FromHours(2);
                        cfg.LoginPath = "/Accounts/Login";
                    },
                    options: cfg =>
                    {
                        cfg.SignIn.RequireConfirmedAccount = false;
                        
                        cfg.Password.RequiredLength = 3;
                        cfg.Password.RequireUppercase = false;
                        cfg.Password.RequireNonAlphanumeric = false;
                        cfg.Password.RequireLowercase = false;
                        
                        cfg.User.RequireUniqueEmail = true;
                    })
                    .AddAuthorizationRules<AuthorizationRulesConfig>()
                    .AddBelongsToUser<User>();
            
            services.AddSession();
            services.AddDistributedMemoryCache();
            services.AddMemoryCache();

            // your app services
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            // The Middlewares here are configured in order of executation
            // Here, they are organized for Miru defaults. Changing the order might break some functionality 

            if (env.IsDevelopmentOrTest())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                app.UseHsts();
                app.UseHttpsRedirection();
            }
            
            app.UseStaticFiles();
            
            app.UseRequestLogging();
            app.UseStatusCodePagesWithReExecute("/Error/{0}");
            app.UseExceptionLogging();

            app.UseHangfireDashboard();
            app.UseRouting();
            app.UseSession();
            app.UseAuthentication();
            
            app.UseEndpoints(e =>
            {
                e.MapDefaultControllerRoute();
                e.MapRazorPages();
            });
        }
    }
}
