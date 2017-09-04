using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AirPortLogic.Infra;

namespace AirPortLogic.Service
{
    class ConsoleLoggerService : ILogService
    {
        private bool _invalid;

        public void AddLog(string logMessage)
        {
            if(!_invalid)
            try
            {
                Console.WriteLine(logMessage);
            }
            catch
            {
                _invalid = true;
            }
        }
    }
}
