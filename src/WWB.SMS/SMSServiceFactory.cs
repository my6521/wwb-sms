using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Reflection;
using WWB.SMS.Exceptions;
using WWB.SMS.Models;

namespace WWB.SMS
{
    public class SMSServiceFactory : ISMSServiceFactory
    {
        private readonly ILoggerFactory logger;
        private readonly IOptionsMonitor<SMSOptions> optionsMonitor;
        private static ConcurrentDictionary<string, ISMSService> _smsServiceDic = new ConcurrentDictionary<string, ISMSService>();
        private const string ASSEMBLY = "WWB.SMS.{0}";

        public SMSServiceFactory(ILoggerFactory logger, IOptionsMonitor<SMSOptions> optionsMonitor)
        {
            this.optionsMonitor = optionsMonitor ?? throw new ArgumentNullException();
            this.optionsMonitor = optionsMonitor ?? throw new ArgumentNullException();

            this.logger = logger;
            this.optionsMonitor = optionsMonitor;
        }

        public ISMSService Create()
        {
            return Create(DefaultOptionName.Name);
        }

        public ISMSService Create(string name)
        {
            if (string.IsNullOrEmpty(name))
                throw new ArgumentException($"Name can not null.");
            if (_smsServiceDic.ContainsKey(name))
            {
                return _smsServiceDic[name];
            }

            var options = optionsMonitor.Get(name);
            if (options == null)
                throw new ArgumentException($"Cannot get option by name '{name}'.");
            if (string.IsNullOrEmpty(options.Provider))
                throw new ArgumentNullException(nameof(options.Provider), "Provider can not null.");

            var type = GetType(options.Provider);
            var ossService = (ISMSService)Activator.CreateInstance(type, new object[] { options });
            if (ossService == null)
                throw new ArgumentNullException("ossService is null.");

            _smsServiceDic[name] = ossService;

            return ossService;
        }

        private Type GetType(string name)
        {
            var assembly = Assembly.Load(string.Format(ASSEMBLY, name));
            var type = assembly.GetTypes().Where(type => typeof(ISMSService).IsAssignableFrom(type) && type.IsClass && !type.IsAbstract).FirstOrDefault();
            if (type == null)
                throw new SMSException("type is not found.");

            return type;
        }
    }
}