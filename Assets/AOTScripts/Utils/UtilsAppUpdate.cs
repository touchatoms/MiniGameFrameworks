using System;
using Newtonsoft.Json.Linq;
using UnityEngine;


public static class UtilsAppUpdate
{
    public static string TEXT_UPDATE_FORCE = "尊敬的用户，您当前的客户端版本过低，会严重影响您的游戏体验，请点击下方按钮，将客户端更新至最新版本哦！";

    private static JObject _appVersionInfo;

    public static JObject AppVersionInfo
    {
        get { return _appVersionInfo; }
        set { _appVersionInfo = value; }
    }

    public static bool IsForceShowVersion()
    {
        var mustUpdateVersionStr = AppVersionInfo["mustUpdateVersion"].ToString();
        var forceVersionStr = AppVersionInfo["forceVersion"].ToString();

        // Version appVersion = new Version(Utils.DeviceConfigJson.version);

        var needUpdate = false;

        // if (!string.IsNullOrEmpty(mustUpdateVersionStr))
        // {
        //     Version mustUpdateVersion = new Version(AppVersionInfo["mustUpdateVersion"].ToString());
        //     needUpdate = appVersion == mustUpdateVersion;
        // }
        //
        // if (!string.IsNullOrEmpty(forceVersionStr))
        // {
        //     Version forceVersion = new Version(AppVersionInfo["forceVersion"].ToString());
        //     needUpdate = needUpdate || appVersion < forceVersion;
        // }

        return needUpdate;
    }

    public static void CheckForceShowVersion(Action callback)
    {
        // if (IsForceShowVersion())
        // {
        //     UtilsUI.Alert1ButtonNoClose(TEXT_UPDATE_FORCE, "前往更新",
        //         () => { Application.OpenURL(AppVersionInfo["updateUrl"].ToString()); });
        // }
        // else
        // {
        //     callback?.Invoke();
        // }
    }
}