using Newtonsoft.Json;
using SanctionScannerInterviewCase.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SanctionScannerInterviewCase.Services
{
    public class LoggerService : ILoggerService
    {
        public void Log(string message)
        {
            Console.WriteLine(message);
        }
        public void ExternalLog(object data)
        {
            File.WriteAllText(Environment.CurrentDirectory + @"\data.txt", JsonConvert.SerializeObject(data));
        }
    }
}
