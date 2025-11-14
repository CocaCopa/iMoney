using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;

namespace CocaCopa.Logger {
    [Flags]
    public enum LogFiltering {
        None = 0, Messages = 1, Warnings = 2, Errors = 4
    }

    public enum LogColor {
        Default, White, Red, Green, Yellow, Blue, Magenta, Orange
    }

    public enum KeywordMode {
        Include, Exclude
    }

    /// <summary>
    /// Zero-dependency, static logger. No MonoBehaviour. No Core coupling.
    /// </summary>
    public static class CustomDebug {
        // Defaults; overridden by Initialize(...) or by optional LoggerSettings (Resources)
        static LogFiltering _filter = LogFiltering.Messages | LogFiltering.Warnings | LogFiltering.Errors;
        static KeywordMode _mode = KeywordMode.Include;
        static List<string> _keywords;

        // Strip color tags in player builds to avoid log junk
#if !UNITY_EDITOR
        static readonly Regex Strip = new Regex(@"<\/?color.*?>", RegexOptions.Compiled);
#endif

        // One-time static ctor tries to load optional settings
        static CustomDebug() {
            var s = Resources.Load<LoggerSettings>("LoggerSettings");
            if (s != null)
                Initialize(s.filter, s.mode, s.keywords != null ? new List<string>(s.keywords) : null);
        }

        /// <summary>Programmatic initialization (e.g., at app bootstrap).</summary>
        public static void Initialize(LogFiltering filter, KeywordMode mode, List<string> keywords = null) {
            _filter = filter;
            _mode = mode;
            _keywords = keywords ?? new List<string>(0);
        }

        public static void Log(object msg, LogColor c = LogColor.Default) => Print(LogType.Message, msg, c);
        public static void LogWarning(object msg, LogColor c = LogColor.Default) => Print(LogType.Warning, msg, c);
        public static void LogError(object msg, LogColor c = LogColor.Default) => Print(LogType.Error, msg, c);

        enum LogType {
            Message, Warning, Error
        }

        static void Print(LogType t, object message, LogColor color) {
            if (!TypeAllowed(t) || !KeywordAllowed(message))
                return;

            var text = Colorize(message, color);

            switch (t) {
                case LogType.Message: Debug.Log(text); break;
                case LogType.Warning: Debug.LogWarning(text); break;
                case LogType.Error: Debug.LogError(text); break;
            }
        }

        static bool TypeAllowed(LogType t) => t switch {
            LogType.Message => (_filter & LogFiltering.Messages) != 0,
            LogType.Warning => (_filter & LogFiltering.Warnings) != 0,
            LogType.Error => (_filter & LogFiltering.Errors) != 0,
            _ => false
        };

        static bool KeywordAllowed(object msgObj) {
            if (_keywords == null || _keywords.Count == 0)
                return true;

            var msg = msgObj?.ToString() ?? string.Empty;
            if (msg.Length == 0) return true;

            bool any = false;
            for (int i = 0; i < _keywords.Count; i++) {
                var k = _keywords[i];
                if (string.IsNullOrWhiteSpace(k)) continue;
                if (msg.IndexOf(k.Trim(), StringComparison.OrdinalIgnoreCase) >= 0) {
                    any = true; break;
                }
            }
            return _mode == KeywordMode.Include ? any : !any;
        }

        static string Colorize(object m, LogColor c) {
            var text = m?.ToString() ?? "null";
#if UNITY_EDITOR
            string open = c switch {
                LogColor.White => "<color=white>",
                LogColor.Red => "<color=red>",
                LogColor.Green => "<color=green>",
                LogColor.Yellow => "<color=yellow>",
                LogColor.Blue => "<color=cyan>",
                LogColor.Magenta => "<color=#FF00FF>",
                LogColor.Orange => "<color=orange>",
                _ => ""
            };
            return open.Length > 0 ? open + text + "</color>" : text;
#else
            return Strip.Replace(text, "");
#endif
        }
    }
}
