using System.Collections.Generic;
using CocaCopa.Logger.API;
using CocaCopa.Logger.Runtime;
using CocaCopa.Logger.SPI;
using UnityEngine;

namespace CocaCopa.Logger.Unity {
    internal class LoggerInstaller {
        private static LoggerRules loggerRules;

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
        static void Bootstrap() {
            var s = Resources.Load<LoggerSettings>("LoggerSettings") ?? LoggerSettings.Default;
            ILogBridge bridge = new LogBridge();
            loggerRules = new LoggerRules(bridge, s.filter, s.mode, s.keywords != null ? new List<string>(s.keywords) : null);
            Log.WireLogger(loggerRules);
        }
    }
}
