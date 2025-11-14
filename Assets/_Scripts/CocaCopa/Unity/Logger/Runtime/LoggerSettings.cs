using System.Collections.Generic;
using UnityEngine;

namespace CocaCopa.Logger {
    [CreateAssetMenu(fileName = "LoggerSettings", menuName = "Megatetra/Logger Settings", order = 10)]
    public class LoggerSettings : ScriptableObject {
        public LogFiltering filter = LogFiltering.Messages | LogFiltering.Warnings | LogFiltering.Errors;
        public KeywordMode mode = KeywordMode.Include;
        public List<string> keywords = new List<string>();
    }
}
