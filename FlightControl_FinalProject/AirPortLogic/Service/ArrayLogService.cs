using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AirPortLogic.Infra;
using System.Timers;

namespace AirPortLogic.Service
{
    public class ArrayLogService : ILogService
    {
        static public string[] Logs { get; private set; }
        private static Timer _refreshTimer;

        private List<string> _lastLogs;

        public ArrayLogService()
        {
            Logs = new string[0];
            InitTimer();
            _lastLogs = new List<string>();
        }

        private void InitTimer()
        {
            _refreshTimer = new Timer(2000);
            _refreshTimer.Elapsed += _refreshTimer_Elapsed;
        }

        private void _refreshTimer_Elapsed(object sender, ElapsedEventArgs e)
        {
            Logs = _lastLogs.ToArray();
            _lastLogs.Clear();
        }

        public void AddLog(string logMessage)
        {
            _lastLogs.Add(logMessage);
        }
    }
}
