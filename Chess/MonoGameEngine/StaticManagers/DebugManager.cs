using System.Collections.Generic;

namespace MonoGameEngine
{
    class DebugManager
    {
        // Debug Mode Logic
        public static bool DebugMode { get; set; } = false;

        public static void ToggleDebugMode()
        {
            DebugMode = !DebugMode;
        }


        // Debug Logs Logic
        private static List<string> _debugLogs = new List<string>();

        public static void Log(string str)
        {
            _debugLogs.Add(str);
        }

        // Clears list of logs and returns it as an array
        public static string[] DumpLogs()
        {
            string[] logs = _debugLogs.ToArray();
            _debugLogs.Clear();
            return logs;
        }
    }
}
