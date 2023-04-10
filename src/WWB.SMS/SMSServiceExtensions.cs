﻿using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System;
using System.Linq;
using WWB.SMS.Models;

namespace WWB.SMS
{
    public static class SMSServiceExtensions
    {
        public static IServiceCollection AddOSSService(this IServiceCollection services, string key)
        {
            return services.AddOSSService(DefaultOptionName.Name, key);
        }

        public static IServiceCollection AddOSSService(this IServiceCollection services, string name, string key)
        {
            using (ServiceProvider provider = services.BuildServiceProvider())
            {
                var configuration = provider.GetRequiredService<IConfiguration>();
                if (configuration == null)
                {
                    throw new ArgumentNullException(nameof(IConfiguration));
                }
                var section = configuration.GetSection(key);
                if (!section.Exists())
                {
                    throw new Exception($"Config file not exist '{key}' section.");
                }
                var options = section.Get<SMSOptions>();
                if (options == null)
                {
                    throw new Exception($"Get OSS option from config file failed.");
                }
                return services.AddOSSService(name, o =>
                {
                    o.AccessKeyId = options.AccessKeyId;
                    o.AccessKeySecret = options.AccessKeySecret;
                });
            }
        }

        public static IServiceCollection AddOSSService(this IServiceCollection services, Action<SMSOptions> option)
        {
            return services.AddOSSService(DefaultOptionName.Name, option);
        }

        public static IServiceCollection AddOSSService(this IServiceCollection services, string name, Action<SMSOptions> option)
        {
            if (string.IsNullOrEmpty(name))
            {
                name = DefaultOptionName.Name;
            }
            services.Configure(name, option);

            //对于IOSSServiceFactory只需要注入一次
            if (!services.Any(p => p.ServiceType == typeof(ISMSServiceFactory)))
            {
                services.TryAddSingleton<ISMSServiceFactory, SMSServiceFactory>();
            }
            //
            services.TryAddScoped(sp => sp.GetRequiredService<ISMSServiceFactory>().Create(name));

            return services;
        }
    }
}