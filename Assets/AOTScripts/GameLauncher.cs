using System;
using System.Collections;
using MotionFramework;
using MotionFramework.Audio;
using MotionFramework.Config;
using MotionFramework.Event;
using MotionFramework.Pool;
using MotionFramework.Resource;
using MotionFramework.Scene;
using MotionFramework.Tween;
using MotionFramework.Window;
using UniFramework.Event;
using UnityEngine;
using YooAsset;

public class GameLauncher : MonoBehaviour
{
    public EPlayMode PlayMode = EPlayMode.EditorSimulateMode;

    private void Awake()
    {
        // 游戏管理器
        GameManager.Instance.Behaviour = this;

        // 初始化事件系统
        UniEvent.Initalize();
        // 创建事件管理器
        MotionEngine.CreateModule<EventManager>();

        // 创建补间管理器
        MotionEngine.CreateModule<TweenManager>();
        
        MotionEngine.CreateModule<ResourceManager>();
        
        // 创建对象池管理器
        var poolCreateParam = new GameObjectPoolManager.CreateParameters();
        poolCreateParam.DefaultDestroyTime = 5f;
        MotionEngine.CreateModule<GameObjectPoolManager>(poolCreateParam);

        // 创建音频管理器
        MotionEngine.CreateModule<AudioManager>();

        // 创建配置表管理器
        MotionEngine.CreateModule<ConfigManager>();

        // 创建场景管理器
        MotionEngine.CreateModule<SceneManager>();

        // 创建窗口管理器
        MotionEngine.CreateModule<WindowManager>();

        var operation = new PatchOperation("DefaultPackage", PlayMode);
        YooAssets.StartOperation(operation);
    }

    IEnumerator StartOperation()
    {
        // 开始补丁更新流程
        var operation = new PatchOperation("DefaultPackage", PlayMode);
        YooAssets.StartOperation(operation);
        yield return operation;
    }

    /// <summary>
    /// 远端资源地址查询服务类
    /// </summary>
    private class RemoteServices : IRemoteServices
    {
        private readonly string _defaultHostServer;
        private readonly string _fallbackHostServer;

        public RemoteServices(string defaultHostServer, string fallbackHostServer)
        {
            _defaultHostServer = defaultHostServer;
            _fallbackHostServer = fallbackHostServer;
        }

        string IRemoteServices.GetRemoteMainURL(string fileName)
        {
            return $"{_defaultHostServer}/{fileName}";
        }

        string IRemoteServices.GetRemoteFallbackURL(string fileName)
        {
            return $"{_fallbackHostServer}/{fileName}";
        }
    }
    
    void HandleMotionFrameworkLog(ELogLevel logLevel, string log)
    {
        if (logLevel == ELogLevel.Log)
        {
#if FISH_DEBUG
                UtilsLog.LogRelease(log);
#endif
        }
        else if (logLevel == ELogLevel.Error)
        {
            UtilsLog.LogErrorRelease(log);
        }
        else if (logLevel == ELogLevel.Warning)
        {
            UtilsLog.LogWaringRelease(log);
        }
        else if (logLevel == ELogLevel.Exception)
        {
            UtilsLog.LogErrorRelease(log);
        }
        else
        {
            throw new NotImplementedException($"{logLevel}");
        }
    }

    void Start()
    {
        Debug.Log("GameLauncher::Start called");

//         // Editor环境下，HotUpdate.dll.bytes已经被自动加载，不需要加载，重复加载反而会出问题。
// #if !UNITY_EDITOR
//         Assembly hotUpdateAss =
//  Assembly.Load(File.ReadAllBytes($"{Application.streamingAssetsPath}/HotUpdate.dll.bytes"));
// #else
//         // Editor下无需加载，直接查找获得HotUpdate程序集
//         Assembly hotUpdateAss =
//             System.AppDomain.CurrentDomain.GetAssemblies().First(a => a.GetName().Name == "HotUpdate");
// #endif
//
//         Type type = hotUpdateAss.GetType("Hello");
//         type.GetMethod("Run")?.Invoke(null, null);
    }
}