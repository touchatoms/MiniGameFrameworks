using System;
using UnityEngine;


    public static class PlatformManager
    {
        public static void RequestPermission(Action callback)
        {
#if UNITY_ANDROID
            PlatformAndroidBridge.RequestPermission(callback);
#else
            callback?.Invoke();
#endif
        }

        public static void GetOAID(string pemString)
        {
#if UNITY_ANDROID
            PlatformAndroidBridge.GetOAID(pemString);
#endif
        }

        public static void GetPermissionInfo()
        {
#if UNITY_ANDROID
            PlatformAndroidBridge.GetPermissionInfo();
#endif
#if UNITY_IPHONE
            PlatformIOSBridge.GetPermissionInfo();
#endif
        }
    }

