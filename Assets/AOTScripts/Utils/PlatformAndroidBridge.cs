using System;
using System.Runtime.InteropServices;
using MotionFramework.Resource;
using Unity.VisualScripting;
using UnityEngine;

using YooAsset;

public class PlatformAndroidBridge
{
#if UNITY_ANDROID
        public static string GetDeviceInfoJson()
        {
            AndroidJavaClass jc = new AndroidJavaClass("com.peanuts.game.util.AppBridge");
            return jc.CallStatic<string>("start");
        }

        public static void RequestPermission(Action callback)
        {
            if (Application.platform == RuntimePlatform.Android)
            {
                AndroidJavaClass jc = new AndroidJavaClass("com.peanuts.game.util.AppBridge");
                jc.CallStatic("requestPermission");
            }
            else
            {
                callback?.Invoke();
            }
        }

        public static void GetPermissionInfo()
        {
            AndroidJavaClass jc = new AndroidJavaClass("com.peanuts.game.util.AppBridge");
            string permissionInfo = jc.CallStatic<string>("getPermissionInfo");
            UtilsLog.LogRelease("GetPermissionInfo: >>>> " + permissionInfo);
            DeviceConfigShell json = JsonMapper.ToObject<DeviceConfigShell>(permissionInfo);
            if (!string.IsNullOrEmpty(json.macAddr))
            {
                Utils.DeviceConfigJson.macAddr = json.macAddr;
            }
            else if (!string.IsNullOrEmpty(json.imei))
            {
                Utils.DeviceConfigJson.macAddr = json.macAddr;
            }
            else if (!string.IsNullOrEmpty(json.oaid))
            {
                Utils.DeviceConfigJson.oaid = json.oaid;
            }
            else if (!string.IsNullOrEmpty(json.idfa))
            {
                Utils.DeviceConfigJson.idfa = json.idfa;
            }
        }

        public static void GetOAID(string pemString)
        {
            if (Application.platform == RuntimePlatform.Android)
            {
                AndroidJavaClass jc = new AndroidJavaClass("com.peanuts.game.util.AppBridge");
                jc.CallStatic("getOAID", pemString);
            }
        }

        public static long GetFreeDiskSpace()
        {
            AndroidJavaClass jc = new AndroidJavaClass("com.peanuts.game.util.AppBridge");
            return jc.CallStatic<long>("GetFreeDiskSpace");
        }
#endif
}