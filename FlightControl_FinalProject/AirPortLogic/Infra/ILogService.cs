using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AirPortLogic.Infra
{
    public interface ILogService
    {
        void AddLog(string logMessage);
    }
}
