using System;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;
using UnityEngine;



public class UtilsDataCollection
{
    private static Dictionary<string, bool> TRACE_ONCE_INSTALL = new Dictionary<string, bool>()
    {
        { "GameLauncher.Awake", true }, { "Shoot", true }
    };

    private static Dictionary<string, bool> TRACE_ONCE_INSTALL_DICT = new Dictionary<string, bool>()
    {
        { "GameLauncher.Awake", false }, { "Shoot", false }
    };

    public static void DotEvent(string eventStr, JObject paramsJsonData)
    {
        DotEvent(eventStr, paramsJsonData.ToString());
    }

    public static void DotEvent(string eventStr, string paramStr)
    {
        if (TRACE_ONCE_INSTALL.ContainsKey(eventStr))
        {
            string localSaveStr = PlayerPrefs.GetString("TRACE_ONCE_INSTALL_" + eventStr, "");

            if (TRACE_ONCE_INSTALL_DICT.TryGetValue(eventStr, out var value))
            {
                if (value)
                {
                    return;
                }
            }

            // if (string.IsNullOrEmpty(localSaveStr))
            // {
            //     PlayerPrefs.SetString("TRACE_ONCE_INSTALL_" + eventStr, Utils.DeviceConfigJson.deviceId);
            //     TRACE_ONCE_INSTALL_DICT[eventStr] = true;
            // }
            // else
            // {
            //     return;
            // }
        }

        // JObject single = new JObject
        // {
        //     // ["device_id"] = Utils.DeviceConfigJson.deviceId,
        //     // ["platform"] = Utils.DeviceConfigJson.platform,
        //     ["user_id"] = "",
        //     ["event"] = eventStr,
        //     ["param"] = paramStr,
        //     ["ts"] = new DateTimeOffset(DateTime.Now).ToUnixTimeMilliseconds()
        // };
        //
        // JObject jsonData = new JObject();
        // jsonData.(JsonType.Array);
        // jsonData.Add(single);
        // string json = JsonMapper.ToJson(jsonData);
        //
        // string url = ShellConfig.CONFIG_HTTP_ORIGIN + "dot/events";
        //
        // UtilsHttp.HttpPostJson(url, json, 5, (isSuccess, code, response) =>
        // {
        //     Debug.LogFormat("{0} Response {1} {2} {3}", ShellConfig.CONFIG_HTTP_ORIGIN + "dot/events",
        //         isSuccess, code, response);
        // });
    }
}