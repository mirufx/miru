using System;
using System.Linq;
using Hangfire;
using Hangfire.Console;
using Hangfire.Console.Extensions;
using Hangfire.Console.Extensions.Serilog;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Miru;
using Miru.Behaviors.BelongsToUser;
using Miru.Fabrication;
using Miru.Foundation.Logging;
using Miru.Globalization;
using Miru.Hosting;
using Miru.Mailing;
using Miru.Mvc;
using Miru.Pipeline;
using Miru.Queuing;
using Miru.Scheduling;
using Miru.Scopables;
using Miru.Security;
using Miru.Sqlite;
using Miru.Userfy;
using Playground.Config;
using Playground.Database;
using Playground.Domain;
using StackExchange.Exceptional;

namespace Playground;

public class Startup
{
    public void ConfigureServices(IServiceCollection services)
    {
        services.AddMiru<Startup>()

            .AddMiruHtml<HtmlConfig>()

            .AddDefaultPipeline<Startup>()

            .AddGlobalization(defaultCulture: "en-GB", "de-DE", "en-US", "pt-BR", "pt-PT")

            .AddEfCoreSqlite<PlaygroundDbContext>()

            // user register, login, logout
            .AddUserfy<User, PlaygroundDbContext>(
                cookie: cfg =>
                {
                    cfg.Cookie.Name = App.Name;
                    cfg.Cookie.HttpOnly = true;
                    cfg.ExpireTimeSpan = TimeSpan.FromHours(2);
                    cfg.LoginPath = "/Accounts/Login";
                },
                identity: cfg =>
                {
                    cfg.SignIn.RequireConfirmedAccount = false;
                    cfg.SignIn.RequireConfirmedEmail = false;
                    cfg.SignIn.RequireConfirmedPhoneNumber = false;

                    cfg.Password.RequiredLength = 3;
                    cfg.Password.RequireUppercase = false;
                    cfg.Password.RequireNonAlphanumeric = false;
                    cfg.Password.RequireLowercase = false;

                    cfg.User.RequireUniqueEmail = true;
                })
            .AddAuthorizationRules<AuthorizationRulesConfig>()
            .AddBelongsToUser()

            .AddMailing(_ =>
            {
                _.EmailDefaults(email => email.From("noreply@skeleton.com", "Skeleton"));
            })
            .AddSmtpSender()

            // TODO: one line call .AddLiteDbQueueing() with DefaultQueueAuthorizer set by default
            .AddScoped<IQueueAuthorizer, DefaultQueueAuthorizer>()
            .AddQueuing(_ =>
            {
                _.UseLiteDb();
            })
            .AddHangfireServer()

            .AddScheduledJob<ScheduledJobConfig>();

        services
            .AddFabrication<PlaygroundFabricator>();
        
        services.AddExceptional(settings =>
        {
        });
        services.AddMvcCore(options => { options.Filters.Add(typeof(ExceptionalExceptionFilter), -9000); });

        services.AddSession();
        services.AddDistributedMemoryCache();
        services.AddMemoryCache();

        // your app services
    }

    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        // The Middlewares here are configured in order of executation
        // Here, they are organized for Miru defaults. Changing the order might break some functionality 

        app.UseExceptional();
        
        if (env.IsDevelopmentOrTest())
        {
            // app.UseDeveloperExceptionPage();
        }
        else
        {
            app.UseExceptionHandler("/Error");
            app.UseHsts();
            app.UseHttpsRedirection();
        }
            
        app.UseStaticFiles();

        app.UseRequestLocalization();
            
        app.UseRequestLogging();
        app.UseStatusCodePagesWithReExecute("/Error/{0}");
        app.UseExceptionLogging();

        app.UseRouting();
        app.UseSession();
        app.UseAuthentication();
            
        app.UseQueueDashboard();
        
        app.UseEndpoints(e =>
        {
            e.MapDefaultControllerRoute();
            e.MapRazorPages();
        });
    }
}

public class ExceptionalExceptionFilter : IExceptionFilter
{
    public void OnException(ExceptionContext context)
    {
        context.Exception.Log(context.HttpContext);
    }
}