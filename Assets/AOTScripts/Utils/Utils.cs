using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using MotionFramework;
using UnityEngine;
using UnityEngine.Networking;
using YooAsset;
using Object = UnityEngine.Object;
using SceneManager = UnityEngine.SceneManagement.SceneManager;
using Newtonsoft.Json;

public static class Utils
{
    public static string DeviceJsonTemplate = @"
        {
            ""deviceId"": ""{deviceId}"",
            ""idfa"": ""idfa"",
            ""idfv"": ""idfv"",

            ""deviceManufacturer"": ""OPPO"",
            ""language"": ""zh"",
            ""country"": ""CN"",
            ""deviceModel"": ""PCAM00"",
            ""packageName"": ""com.ggqp.fishcq"",
            ""channel"": ""qhby"",
            ""carrier"": """",
            ""osVersion"": ""12.1"",
            ""version"": ""1.0.0"",
            ""displayName"": ""捕鱼传奇"",
            ""oaid"": ""oaid"",
            ""imei"": ""imei"",
            ""trackingIoDeviceId"": ""trackingIoDeviceId"",
            ""androidId"": ""androidId"",
            ""platform"": ""{platform}"",
            ""loginTypes"": ""visitor,"",
            ""payTypes"": ""h5"",
            ""ips"": ""101.201.83.125:8002"",
            ""cdn"": ""http://app.qigame1024.com:8080/unity/"",
            ""urlOrigin"": ""http://101.201.83.125:9100/"",
            ""--------platform(只能为 windows mac ios android)"": """",
            ""--------deviceModel"": ""设备型号"",
            ""--------packageName"": ""包名"",
            ""--------channel"": ""渠道名"",
            ""--------mobileType"": ""设备类型 android / ios "",
            ""--------deviceManufacturer"": ""设备制造商"",
            ""--------version"": ""App Version"",
            ""--------osVersion"": ""操作系统版本"",
            ""--------carrier"": ""网络类型"",
            ""--------idfa"": ""advertisingIdentifier 同一台手机统一"",
            ""--------idfv"": ""identifierForVendor 同一台手机 同一个厂商统一，不同厂商不同"",
            ""--------deviceId"": ""设备ID"",
            ""--------displayName"": ""app display name"",
            ""--------trackingIoDeviceId"": ""热云deviceId"",
            ""--------ips"": ""socket 地址  "",
            ""--------cdn"": ""cdn 地址"",
            ""--------urlOrigin"": ""http 链接源"",
            ""--------loginTypes"": ""登录类型"",
            ""--------payTypes"": ""支付类型"",
            ""--------loginTypes"": ""登录类型字符串已逗号隔开""
        }";


    public static string ManifestFileName = "PackageManifest";

    public static string ManifestFolderName = "ManifestFiles";


    public const string StreamingAssetsBuildinFolder = "BuildinFiles";

    public static string DeviceInfo = "";
    public static string ReporterUrl = "";

    public static void LogSystemInfo()
    {
        UtilsLog.LogRelease("##########Application.dataPath:" + Application.dataPath);
        UtilsLog.LogRelease("##########Application.streamingAssetsPath:" + Application.streamingAssetsPath);
        UtilsLog.LogRelease("##########Application.persistentDataPath:" + Application.persistentDataPath);
        UtilsLog.LogRelease("##########Application.temporaryCachePath:" + Application.temporaryCachePath);
        UtilsLog.LogRelease("##########Application.platform:" + Application.platform);
    }

    public static string GetPersistentRootPath()
    {
        string _sandboxPath = "";
#if UNITY_EDITOR
        // 注意：为了方便调试查看，编辑器下把存储目录放到项目里
        if (string.IsNullOrEmpty(_sandboxPath))
        {
            string directory = Path.GetDirectoryName(UnityEngine.Application.dataPath);
            string projectPath = GetRegularPath(directory);
            _sandboxPath = string.Format("{0}/Sandbox", projectPath);
        }

        return _sandboxPath;
#else
            if (string.IsNullOrEmpty(_sandboxPath))
            {
                _sandboxPath = string.Format("{0}/Sandbox", UnityEngine.Application.persistentDataPath);
            }

            return _sandboxPath;
#endif
    }

    /// <summary>
    /// 获取沙盒内包裹的版本文件的路径
    /// </summary>
    public static string GetCachePackageVersionFilePath(string packageName)
    {
        string fileName = GetPackageVersionFileName(packageName);

        string versionpath = $"{ManifestFolderName}/{fileName}";

        string root = GetPersistentRootPath();
        return string.Format("{0}/{1}", root, versionpath);
    }

    private static string GetRegularPath(string path)
    {
        return path.Replace('\\', '/').Replace("\\", "/"); //替换为Linux路径格式
    }

    /// <summary>
    /// 获取基于流文件夹的加载路径
    /// </summary>
    public static string MakeStreamingLoadPath(string path)
    {
        string _buildinPath = string.Format("{0}/{1}", UnityEngine.Application.streamingAssetsPath,
            StreamingAssetsBuildinFolder);
        return string.Format("{0}/{1}", _buildinPath, path);
    }

    /// <summary>
    /// 获取包裹的版本文件完整名称
    /// </summary>
    public static string GetPackageVersionFileName(string packageName)
    {
        return $"{ManifestFileName}_{packageName}.version";
    }

    public static int MatchNum(string str)
    {
        Regex r = new Regex(@"\d+", RegexOptions.IgnoreCase);
        Match m = r.Match(str);
        int number = int.Parse(m.Value);
        return number;
    }

    public static string GetPackageVersion()
    {
#if UNITY_EDITOR
        string path = Path.Combine(Application.dataPath, "../Game/Scripts/Config/Config.cs");
        UtilsLog.LogFormatRelease("Utils.GetPackageVersion UNITY_EDITOR:{0}", path);
        string[] strs = File.ReadAllLines(path);
        foreach (string str in strs)
        {
            if (str.IndexOf("CONFIG_GAME_BUILD = ") >= 0)
            {
                return MatchNum(str).ToString();
            }
        }

        return "0";
#else
            string versionCachefilePath = GetCachePackageVersionFilePath("DefaultPackage");
            UtilsLog.LogFormatRelease("Utils.GetPackageVersion versionCachefilePath:{0}", versionCachefilePath);

            if (File.Exists(versionCachefilePath))
            {
                string PackageVersion = File.ReadAllText(versionCachefilePath, Encoding.UTF8);
                return PackageVersion;
            }
            else
            {
                string versionName = GetPackageVersionFileName("DefaultPackage");
                string _buildinPath =
                    string.Format("{0}/{1}", Application.streamingAssetsPath, StreamingAssetsBuildinFolder);
                string streamingAssetFilePath = string.Format("{0}/{1}", _buildinPath, versionName);
                UtilsLog.LogFormatRelease("Utils.GetPackageVersion streamingAssetFilePath:{0}", streamingAssetFilePath);

                return ReadFileContent(streamingAssetFilePath);
            }
#endif
    }


    public static bool IsMacAddressValid(string macAddr)
    {
        return !string.IsNullOrEmpty(macAddr) && !macAddr.StartsWith("02:00:00");
    }

    public static string ReadFileContent(string filePath)
    {
        if (Application.platform == RuntimePlatform.Android)
        {
            filePath = filePath.StartsWith("jar:file") ? filePath : "jar:file://" + filePath;
            UnityWebRequest request = UnityWebRequest.Get(filePath);
            request.SendWebRequest();
            while (!request.isDone)
            {
            }

            return request.downloadHandler.text;
        }
        else
        {
            return File.ReadAllText(filePath, Encoding.UTF8);
        }
    }

    public static string GetInAppVersion()
    {
        if (StreamingAssetsHelper.FileExists(Application.streamingAssetsPath + "/BuildAppName"))
        {
            string info = ReadFileContent(Application.streamingAssetsPath + "/BuildAppName");
            var infoArr = info.Split('_');
            if (infoArr.Length == 2)
            {
                return $"({infoArr[1]})";
            }
            else
            {
                return "(E)";
            }
        }

        return "";
    }

    public static EPlayMode GetPlayMode()
    {
        string playMode = "";
        switch (Application.platform)
        {
            case RuntimePlatform.WindowsEditor:
            case RuntimePlatform.OSXEditor:
                playMode = PlayerPrefs.GetString("DebugPlayMode", "");
                UtilsLog.LogRelease("GetPlayMode:WindowsEditor+OSXEditor:" + playMode);

                if (playMode == "OfflinePlayMode")
                {
                    return EPlayMode.OfflinePlayMode;
                }
                else if (playMode == "HostPlayMode")
                {
                    return EPlayMode.HostPlayMode;
                }
                else
                {
                    return EPlayMode.EditorSimulateMode;
                }
            case RuntimePlatform.Android:
            case RuntimePlatform.IPhonePlayer:
            {
                string cdnHttpOrigin = ShellConfig.CDN_HTTP_ORIGIN;
                playMode = PlayerPrefs.GetString("DebugPlayMode", "");

                string debugUrlOrigin = PlayerPrefs.GetString("DebugUrlOrigin", "");

                UtilsLog.LogRelease($"GetPlayMode:Android+IPhonePlayer: {cdnHttpOrigin}-{playMode}");

                if (playMode == "OfflinePlayMode" ||
                    (string.IsNullOrEmpty(cdnHttpOrigin) && string.IsNullOrEmpty(debugUrlOrigin)))
                {
                    return EPlayMode.OfflinePlayMode;
                }
                else
                {
                    return EPlayMode.HostPlayMode;
                }
            }
            default:
            {
                UtilsLog.LogRelease("GetPlayMode default: " + EPlayMode.HostPlayMode);
                return EPlayMode.HostPlayMode;
            }
        }
    }

    //发送报错信息到飞书群
    public static void SetErrorReport(bool isOpen)
    {
        if (isOpen)
        {
            if (ShellConfig.DEBUG > 0)
            {
                Application.logMessageReceivedThreaded -= CaptureLogThread;
                Application.logMessageReceivedThreaded += CaptureLogThread;
                ReporterUrl = "https://open.feishu.cn/open-apis/bot/v2/hook/42767a07-bf2a-4072-ab95-b2bc4e902bbc";
            }
            else
            {
                Application.logMessageReceivedThreaded -= CaptureLogThreadRelease;
                Application.logMessageReceivedThreaded += CaptureLogThreadRelease;
                ReporterUrl = "https://open.feishu.cn/open-apis/bot/v2/hook/eb9027a7-f151-4095-b667-f4dc37507535";
            }

            AppDomain.CurrentDomain.UnhandledException -= CaptureUnhandledLog;
            AppDomain.CurrentDomain.UnhandledException += CaptureUnhandledLog;
        }
        else
        {
            if (ShellConfig.DEBUG > 0)
            {
                Application.logMessageReceivedThreaded -= CaptureLogThread;
            }
            else
            {
                Application.logMessageReceivedThreaded -= CaptureLogThreadRelease;
            }

            AppDomain.CurrentDomain.UnhandledException -= CaptureUnhandledLog;
        }

        lock (errorLogSet)
        {
            errorLogSet.Clear();
        }
    }

    private static void CaptureUnhandledLog(object sender, UnhandledExceptionEventArgs args)
    {
        if (args == null || args.ExceptionObject == null)
        {
            return;
        }

        try
        {
            if (args.ExceptionObject.GetType() != typeof(System.Exception))
            {
                return;
            }
        }
        catch
        {
            return;
        }

        var exception = (System.Exception)args.ExceptionObject;
        CaptureLogThread(exception.Message, exception.StackTrace, LogType.Exception);
    }

    public static bool HasReporter;

    public static void LoadReporter()
    {
        if (!HasReporter && ShellConfig.DEBUG > 1)
        {
            var go = (GameObject)Resources.Load("Prefabs/Reporter");
            Object.Instantiate(go);
            HasReporter = true;
        }
    }

    static HashSet<string> errorLogSet = new HashSet<string>();

    //发送报错信息到飞书群
    public static void CaptureLogThreadRelease(string condition, string stacktrace, LogType type)
    {
        if (type == LogType.Exception)
        {
            var str = "";
            bool isSend = false;
            lock (errorLogSet)
            {
                if (!errorLogSet.Contains(stacktrace))
                {
                    // TODO Game.userId
                    str =
                        $"Online_Device: {DeviceInfo} UserId {"Game.userId.ToString()"} Scene: {SceneManager.GetActiveScene().name} error: {condition} \n------------------------------\n stack:{stacktrace}";
                    errorLogSet.Add(stacktrace);
                    isSend = true;
                }
            }

            if (isSend)
            {
                MotionEngine.StartCoroutine(SendToLark(str));
            }
        }
    }

    //发送报错信息到飞书群
    public static void CaptureLogThread(string condition, string stacktrace, LogType type)
    {
        if (type == LogType.Exception || (type == LogType.Error && condition.Contains("Exception")))
        {
            var str = "";
            bool isSend = false;
            lock (errorLogSet)
            {
                if (!errorLogSet.Contains(stacktrace))
                {
                    // TODO Game.userId
                    str =
                        $"Device: {DeviceInfo} UserId {"Game.userId.ToString()"} Scene: {SceneManager.GetActiveScene().name} error: {condition} \n------------------------------\n stack:{stacktrace}";
                    errorLogSet.Add(stacktrace);
                    isSend = true;
                }
            }

            if (isSend)
            {
                MotionEngine.StartCoroutine(SendToLark(str));
            }
        }
    }

    //发送报错信息到飞书群
    static IEnumerator SendToLark(string str)
    {
        if (string.IsNullOrEmpty(ReporterUrl))
        {
            yield return null;
        }

        using (UnityWebRequest req = new UnityWebRequest(
                   ReporterUrl,
                   UnityWebRequest.kHttpVerbPOST))
        {
            var data = new { msg_type = "text", content = new { text = str } };
            string jsonStr = JsonConvert.ToString(data);
            req.uploadHandler = new UploadHandlerRaw(Encoding.UTF8.GetBytes(jsonStr));
            req.SetRequestHeader("Content-Type", "application/json");
            yield return req.SendWebRequest();
        }
    }
    //
    // public static void UpdateShellConfig()
    // {
    //     if (!string.IsNullOrEmpty(DeviceConfigJson.envDebug))
    //     {
    //         try
    //         {
    //             UtilsLog.LogRelease($"GameLauncher.Awake evnDebug: {DeviceConfigJson.envDebug}");
    //             ShellConfig.DEBUG = int.Parse(DeviceConfigJson.envDebug);
    //         }
    //         catch (Exception e)
    //         {
    //             UtilsLog.LogErrorRelease($"GameLauncher.Awake evnDebug exception: {e.Message}");
    //         }
    //     }
    //
    //     if (!string.IsNullOrEmpty(DeviceConfigJson.playModeDebug))
    //     {
    //         try
    //         {
    //             UtilsLog.LogRelease($"GameLauncher.Awake playModeDebug: {DeviceConfigJson.playModeDebug}");
    //             ShellConfig.ShowDebugNode = DeviceConfigJson.playModeDebug == "1";
    //         }
    //         catch (Exception e)
    //         {
    //             UtilsLog.LogErrorRelease($"GameLauncher.Awake playModeDebug  exception: {e.Message}");
    //         }
    //     }
    // }

    public static void InitBugly()
    {
#if UNITY_ANDROID || UNITY_IOS
            if(ShellConfig.DEBUG > 1) BuglyAgent.ConfigDebugMode(true);
#endif

#if UNITY_IOS
            BuglyAgent.InitWithAppId("3a740ea419"); //已经在oc中init了
#elif UNITY_ANDROID
            // BuglyAgent.InitWithAppId ("55e8da51ab");//已经在java中init了
#endif

#if UNITY_ANDROID || UNITY_IOS
            BuglyAgent.EnableExceptionHandler();
            BuglyAgent.SetUserId(DeviceConfigJson.deviceId); // 使用 deviceId 替代 userId
#endif
    }

    public static void PostDeviceInfo()
    {
        // JsonData uploadInfo = GetUpLoadInfo();
        // Dictionary<string, string> postParams = new Dictionary<string, string>(StringComparer.Ordinal)
        // {
        //     { "deviceId", DeviceConfigJson.deviceId },
        //     { "version", DeviceConfigJson.version },
        //     { "channel", DeviceConfigJson.channel },
        //     { "platform", DeviceConfigJson.platform },
        //     { "gameBuild", GetPackageVersion() },
        //     { "deviceInfo", uploadInfo.ToJson() },
        // };
        //
        // string json = JsonMapper.ToJson(postParams);
        //
        // string url = $"{ShellConfig.CONFIG_HTTP_ORIGIN}deviceApp";
        //
        // UtilsHttp.HttpPostJsonGame(url, json, 5, (isSuccess, code, response) => { });
    }

    private static byte[] _decompressBuffer = new byte[64 * 1024];

    public static string Base64Encode(string plainText)
    {
        var plainTextBytes = System.Text.Encoding.UTF8.GetBytes(plainText);
        return Base64Encode(plainTextBytes);
    }

    public static string Base64Encode(byte[] bytes)
    {
        return System.Convert.ToBase64String(bytes);
    }

    public static byte[] Base64Decode(string base64EncodedData)
    {
        var base64EncodedBytes = System.Convert.FromBase64String(base64EncodedData);
        return base64EncodedBytes;
    }

    // Deflate解压
    public static byte[] Decompress(byte[] input)
    {
        // using (ZlibStream decompressor = new ZlibStream(new MemoryStream(input),
        //            Ionic.Zlib.CompressionMode.Decompress,
        //            Ionic.Zlib.CompressionLevel.Default))
        // {
        //     using (MemoryStream output = new MemoryStream())
        //     {
        //         var read = 0;
        //         while ((read = decompressor.Read(_decompressBuffer, 0, _decompressBuffer.Length)) > 0)
        //         {
        //             output.Write(_decompressBuffer, 0, read);
        //         }
        //
        //         return output.ToArray();
        //     }
        // }
        return null;
    }

    // Deflate压缩
    public static byte[] Compress(byte[] data)
    {
        // using (var input = new MemoryStream(data))
        // {
        //     using (var output = new MemoryStream())
        //     {
        //         using (var zlibStream = new ZlibStream(output, Ionic.Zlib.CompressionMode.Compress))
        //         {
        //             zlibStream.FlushMode = FlushType.Finish;
        //             input.CopyTo(zlibStream);
        //             input.Flush();
        //         }
        //
        //         return output.ToArray();
        //     }
        // }
        return null;
    }

    public static byte[] Compress(string inString)
    {
        return Compress(Encoding.UTF8.GetBytes(inString));
    }

    public static string DecodeHttpText(string response)
    {
        var bytes = Base64Decode(response);
        SwapXOR.Encrypt(bytes, false);
        bytes = Decompress(bytes);
        return Encoding.UTF8.GetString(bytes);
    }

    public static byte[] EncodeHttpText(string text)
    {
        var bodyRaw = Encoding.UTF8.GetBytes(text);
        bodyRaw = Compress(bodyRaw);
        SwapXOR.Encrypt(bodyRaw, true);
        return Encoding.UTF8.GetBytes(Base64Encode(bodyRaw));
    }

    public static string EncodeHttpTextStr(string text)
    {
        var bodyRaw = Encoding.UTF8.GetBytes(text);
        bodyRaw = Compress(bodyRaw);
        SwapXOR.Encrypt(bodyRaw, true);
        return Base64Encode(bodyRaw);
    }

    public static byte[] UnpackMsg(byte[] packet, bool isCompressed)
    {
        SwapXOR.Encrypt(packet, false);
        if (isCompressed)
        {
            packet = Decompress(packet);
        }

        return packet;
    }

    public static byte[] PackMsg(byte[] msgBytes, int msgType)
    {
        SwapXOR.Encrypt(msgBytes, true);
        byte[] package = new byte[msgBytes.Length + 4 + 2];

        byte[] lenBytes = BitConverter.GetBytes(msgBytes.Length + 2);
        Array.Reverse(lenBytes);

        byte[] typeBytes = BitConverter.GetBytes((Int16)msgType);
        Array.Reverse(typeBytes);

        Buffer.BlockCopy(lenBytes, 0, package, 0, 4);
        Buffer.BlockCopy(typeBytes, 0, package, 4, 2);
        Buffer.BlockCopy(msgBytes, 0, package, 6, (int)msgBytes.Length);
        return package;
    }

    public static long GetFreeDiskSpace()
    {
#if UNITY_ANDROID
            return PlatformAndroidBridge.GetFreeDiskSpace();
#endif
#if UNITY_IPHONE
            return PlatformIOSBridge.GetFreeDiskSpace();
#endif
        return -1;
    }
}