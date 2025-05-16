using UnityEngine;


public static class ShellConfig
{
    
    // 0 连接在线服务器, 1-在线确认服务器  2- 连接测试服务器, 3-本地, 4-审核
    public static int DEBUG = 0;

    public static string CDN_HTTP_ORIGIN = "http://192.168.0.189:8080/StandaloneOSX/DefaultPackage/2/";

    public static string CONFIG_HTTP_ORIGIN = "";

    public static string HALL_AND_FIRST_ROOM_DOWNLOAD_TAGS =
        "buildin;buildinSpine;buildinFont;buildinPrefab;buildinOgg;fishMain;";

    public static bool ShowDebugNode = false;

    public static bool DEBUG_LOG = false;

    public static void Init()
    {
        // Editor 模式下 默认使用 测试服务器
        if (Application.platform == RuntimePlatform.WindowsEditor ||
            Application.platform == RuntimePlatform.OSXEditor)
        {
            DEBUG = 2;
            DEBUG_LOG = true;
        }

        if (DEBUG == 0)
        {
            CONFIG_HTTP_ORIGIN = "https://ff3d.xgame1024.com/";
        }
        else if (DEBUG == 1)
        {
            CONFIG_HTTP_ORIGIN = "https://ff3dconfirm.xgame1024.com/";
        }
        else if (DEBUG == 2)
        {
            CONFIG_HTTP_ORIGIN = "http://101.201.83.125:9100/";
        }
        else if (DEBUG == 3)
        {
            CONFIG_HTTP_ORIGIN = "http://192.168.110.32:9100/";
        }
        else if (DEBUG == 4)
        {
            CONFIG_HTTP_ORIGIN = "https://ff3daaudit.xgame1024.com/";
        }
    }
}