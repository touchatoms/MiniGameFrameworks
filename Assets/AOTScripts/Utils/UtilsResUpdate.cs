using System.Collections.Generic;
using Newtonsoft.Json.Linq;


public class UtilsResUpdate
{
    private static int _scriptVersion = 0;
    private static int _grayScriptDevice = 0;
    private static string _scriptUrl = "";
    private static string _hallAndFirstRoomDownloadTags = "";

    private static int _type = 0;
    private static int _jump = 0;
    private static string _webRoot = "";
    public static List<string> _socketList = new List<string>();

    // "scriptInfo": {
    //     "scriptVersion": 1, //脚本版本号，如果当前设备指向灰度资源，即为灰度资源版本号
    //     "grayScriptDevice": 1, //是否灰度脚本设备
    //     "scriptUrl": "", //脚本资源url
    // }

    // "serverInfo": { //客户端应当进入的环境
    //     "type": 1, //环境类型,0=在线，1=在线确认，2=审核，3=测试，4=本地
    //     "webRoot": "http://123.56.22.113", //web服务root url
    //     "socket":["123.56.22.113"] //网关地址列表
    // }
    public static string ScriptUrl
    {
        get => _scriptUrl;
        set => _scriptUrl = value;
    }

    public static string WebRoot
    {
        get => _webRoot;
        set => _webRoot = value;
    }

    public static int ScriptVersion
    {
        get => _scriptVersion;
        set => _scriptVersion = value;
    }

    public static int GrayScriptDevice
    {
        get => _grayScriptDevice;
        set => _grayScriptDevice = value;
    }

    public static int Jump
    {
        get => _jump;
        set => _jump = value;
    }

    public static void SetScriptInfo(JObject scriptInfo)
    {
        int scriptVersion = (int)scriptInfo["scriptVersion"];
        int grayScriptDevice = (int)scriptInfo["grayScriptDevice"];
        string scriptUrl = (string)scriptInfo["scriptUrl"];
        string hallAndFirstRoomDownloadTags = (string)scriptInfo["hallAndFirstRoomDownloadTags"];

        _scriptVersion = scriptVersion;
        _grayScriptDevice = grayScriptDevice;
        _scriptUrl = scriptUrl;
        _hallAndFirstRoomDownloadTags = hallAndFirstRoomDownloadTags;
        if (!string.IsNullOrEmpty(_scriptUrl))
        {
            ShellConfig.CDN_HTTP_ORIGIN = _scriptUrl;
        }

        if (!string.IsNullOrEmpty(_hallAndFirstRoomDownloadTags))
        {
            ShellConfig.HALL_AND_FIRST_ROOM_DOWNLOAD_TAGS = _hallAndFirstRoomDownloadTags;
        }
    }

    public static void SetServerInfo(JObject serverInfo)
    {
        int type = (int)serverInfo["type"];
        int jump = (int)serverInfo["jump"];
        string webRoot = (string)serverInfo["webRoot"];

        _jump = jump;
        _type = type;
        _webRoot = webRoot;

        if (jump == 1 && !string.IsNullOrEmpty(_webRoot))
        {
            ShellConfig.CONFIG_HTTP_ORIGIN = _webRoot;
        }
    }

    public static string GetDebugInfo()
    {
        string debugInfo = "";

        if (GrayScriptDevice == 1)
        {
            if (Jump == 1)
            {
                debugInfo = $"DEBUG: 灰度服务器: {WebRoot};";
            }

            if (string.IsNullOrEmpty(debugInfo))
            {
                debugInfo = $"DEBUG:灰度CDN: {ScriptUrl}";
            }
            else
            {
                debugInfo += $"##灰度CDN: {ScriptUrl};";
            }
        }

        return debugInfo;
    }
}

