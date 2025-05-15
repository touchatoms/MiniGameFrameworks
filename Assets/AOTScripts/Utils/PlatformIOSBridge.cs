using System;
using System.Runtime.InteropServices;
using UnityEngine;


public static class PlatformIOSBridge
{
#if UNITY_IPHONE
        [DllImport("__Internal")]
        private static extern string IsWXAppInstall();

        [DllImport("__Internal")]
        private static extern string GetDeviceInfo();

        [DllImport("__Internal")]
        private static extern string GetIDFA();

        [DllImport("__Internal")]
        private static extern string GetUA();

        [DllImport("__Internal")]
        private static extern void EnterReGenScene(string loginJsonStr);

        [DllImport("__Internal")]
        private static extern void Login(string loginJsonStr);

        [DllImport("__Internal")]
        private static extern void AutoLogin(string loginJsonStr);

        [DllImport("__Internal")]
        private static extern void OpenPay(string payJsonStr);

        [DllImport("__Internal")]
        private static extern void BeforeOpenPay(string payJsonStr);

        [DllImport("__Internal")]
        private static extern void LoadUrl(string jsonStr);

        [DllImport("__Internal")]
        private static extern void LoginFailNotify(string payJsonStr);

        [DllImport("__Internal")]
        private static extern void LoginSuccessNotify(string payJsonStr);


        [DllImport("__Internal")]
        private static extern void HandleValidateAppStoreOrderResponse(string payJsonStr);

        [DllImport("__Internal")]
        private static extern void UpdateAvatar(string payJsonStr);
#endif
    public static string IsWxInstalled()
    {
#if UNITY_IPHONE
            return IsWXAppInstall();
#else
        return "false";
#endif
    }

    public static string GetUAProxy()
    {
#if UNITY_IPHONE
            return GetUA();
#else
        return "";
#endif
    }

    public static string GetIDFAProxy()
    {
#if UNITY_IPHONE
            return GetIDFA();
#else
        return "";
#endif
    }

    public static string GetDeviceInfoProxy()
    {
#if UNITY_IPHONE
            return GetDeviceInfo();
#else
        return "";
#endif
    }

    public static void LoginProxy(string loginJsonStr)
    {
#if UNITY_IPHONE
            Login(loginJsonStr);
#endif
    }

    public static void EnterReGenSceneProxy(string jsonStr)
    {
#if UNITY_IPHONE
            EnterReGenScene(jsonStr);
#endif
    }

    public static void AutoLoginProxy(string loginJsonStr)
    {
#if UNITY_IPHONE
            AutoLogin(loginJsonStr);
#endif
    }

    public static void OpenPayProxy(string payJsonStr)
    {
#if UNITY_IPHONE
            OpenPay(payJsonStr);
#endif
    }

    public static void BeforeOpenPayProxy(string payJsonStr)
    {
#if UNITY_IPHONE
            BeforeOpenPay(payJsonStr);
#endif
    }

    public static void LoginFailNotifyProxy(string payJsonStr)
    {
#if UNITY_IPHONE
            LoginFailNotify(payJsonStr);
#endif
    }

    public static void LoginSuccessNotifyProxy(string jsonStr)
    {
#if UNITY_IPHONE
            LoginSuccessNotify(jsonStr);
#endif
    }

    public static void HandleValidateAppStoreOrderResponseProxy(string jsonStr)
    {
#if UNITY_IPHONE
            HandleValidateAppStoreOrderResponse(jsonStr);
#endif
    }

    public static void UpdateAvatarProxy(string jsonStr)
    {
#if UNITY_IPHONE
            UpdateAvatar(jsonStr);
#endif
    }

    public static void LoadUrlProxy(string jsonStr)
    {
#if UNITY_IPHONE
            LoadUrl(jsonStr);
#endif
    }

    public static void RequestPermission(Action callback)
    {
        callback?.Invoke();
    }

    public static void GetOAID(string pemString)
    {
    }


    public static long GetFreeDiskSpace()
    {
        return -1;
    }
}