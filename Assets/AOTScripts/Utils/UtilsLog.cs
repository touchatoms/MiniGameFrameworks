using Newtonsoft.Json.Linq;

public static class UtilsLog
{
    public static bool DeviceLogEnabled = false;

    public static void LogRelease(string content)
    {
        UnityEngine.Debug.Log("[ShellRelease] " + content);
    }

    public static void LogFormatRelease(string format, params object[] args)
    {
        if (args != null)
        {
            for (int i = 0; i < args.Length; i++)
            {
                format = format.Replace("{" + i.ToString() + "}", args[i]?.ToString());
            }
        }

        UnityEngine.Debug.Log("[Shell] " + format);
    }

    public static void LogWaringRelease(string content)
    {
        UnityEngine.Debug.LogWarning("[ShellRelease] " + content);
    }

    public static void LogErrorRelease(string content)
    {
        UnityEngine.Debug.LogError("[ShellRelease] " + content);
    }

    public static void Log(string content)
    {
        if (ShellConfig.DEBUG_LOG || DeviceLogEnabled) UnityEngine.Debug.Log("[Shell] " + content);
    }

    public static void Log(JObject jsonData)
    {
        if (ShellConfig.DEBUG_LOG || DeviceLogEnabled) UnityEngine.Debug.Log("[Shell] " + jsonData.ToString());
    }

    public static void LogFormat(string format, params object[] args)
    {
        if (ShellConfig.DEBUG_LOG || DeviceLogEnabled)
        {
            if (args != null)
            {
                for (int i = 0; i < args.Length; i++)
                {
                    format = format.Replace("{" + i.ToString() + "}", args[i]?.ToString());
                }
            }

            UnityEngine.Debug.Log("[Shell] " + format);
        }
    }

    public static void Info(string content)
    {
        if (ShellConfig.DEBUG_LOG || DeviceLogEnabled) UnityEngine.Debug.Log("[Shell] " + content);
    }

    public static void Warning(string content)
    {
        if (ShellConfig.DEBUG_LOG || DeviceLogEnabled) UnityEngine.Debug.LogWarning("[Shell] " + content);
    }

    public static void Error(string content)
    {
        if (ShellConfig.DEBUG_LOG || DeviceLogEnabled) UnityEngine.Debug.LogError("[Shell] " + content);
    }
}