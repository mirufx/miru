using System;
using Microsoft.Extensions.DependencyInjection;
using OpenQA.Selenium;
using OpenQA.Selenium.Remote;

namespace Miru.PageTesting;

public static class ServiceSeleniumExtensions
{
    public static void AddSelenium(this IServiceCollection services, Func<WebDriver> driverFactory)
    {
        services.AddSingleton(driverFactory());
    }
        
    public static IServiceCollection AddSelenium(this IServiceCollection services, Func<IServiceProvider, WebDriver> driverFactory)
    {
        return services.AddSingleton(driverFactory);
    }
}