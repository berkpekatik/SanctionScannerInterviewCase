using Microsoft.Extensions.DependencyInjection;
using SanctionScannerInterviewCase.Interfaces;
using SanctionScannerInterviewCase.Service;
using SanctionScannerInterviewCase.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SanctionScannerInterviewCase.App_Start
{
    public class Startup
    {
        public static IServiceProvider ConfigureService()
        {
            var provider = new ServiceCollection()
            .AddSingleton<IWebService, WebService>()
            .AddSingleton<IDataProccesorService, DataProccesorService>()
            .AddSingleton<ILoggerService, LoggerService>()
            .BuildServiceProvider();
            return provider;
        }
    }

}
