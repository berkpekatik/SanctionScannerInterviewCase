﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SanctionScannerInterviewCase.Interfaces
{
    public interface ILoggerService
    {
        void Log(string message);
        void ExternalLog(object data);
    }
}
