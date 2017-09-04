using AirPortLogic.Infra;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AirPortLogic.Service
{
    class DebugLogService : ILogService
    {
        public void AddLog(string logMessage)
        {
            Debug.WriteLine(logMessage);
        }
    }
}
