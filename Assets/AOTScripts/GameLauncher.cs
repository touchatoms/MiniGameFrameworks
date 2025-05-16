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
using EventGroup = UniFramework.Event.EventGroup;
using IEventMessage = UniFramework.Event.IEventMessage;

public class GameLauncher : MonoBehaviour
{
    public EPlayMode PlayMode = EPlayMode.HostPlayMode;
    private PatchOperation operation = null;
    private readonly EventGroup _eventGroup = new EventGroup();

    private void Awake()
    {
        Debug.Log("Application.persistentDataPath:" + Application.persistentDataPath);
        Debug.Log("Application.streamingAssetsPath:" + Application.streamingAssetsPath);
        Debug.Log("Application.dataPath:" + Application.dataPath);
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

        _eventGroup.AddListener<PatchEventDefine.InitializeFailed>(OnHandleEventMessage);
        _eventGroup.AddListener<PatchEventDefine.PatchStepsChange>(OnHandleEventMessage);
        _eventGroup.AddListener<PatchEventDefine.FoundUpdateFiles>(OnHandleEventMessage);
        _eventGroup.AddListener<PatchEventDefine.DownloadUpdate>(OnHandleEventMessage);
        _eventGroup.AddListener<PatchEventDefine.PackageVersionRequestFailed>(OnHandleEventMessage);
        _eventGroup.AddListener<PatchEventDefine.PackageManifestUpdateFailed>(OnHandleEventMessage);
        _eventGroup.AddListener<PatchEventDefine.WebFileDownloadFailed>(OnHandleEventMessage);

        operation = new PatchOperation("DefaultPackage", EPlayMode.HostPlayMode);
        YooAssets.StartOperation(operation);
    }

    /// <summary>
    /// 接收事件
    /// </summary>
    private void OnHandleEventMessage(IEventMessage message)
    {
        if (message is PatchEventDefine.InitializeFailed)
        {
        }
        else if (message is PatchEventDefine.PatchStepsChange)
        {
        }
        else if (message is PatchEventDefine.FoundUpdateFiles)
        {
        }
        else if (message is PatchEventDefine.DownloadUpdate)
        {
        }
        else if (message is PatchEventDefine.PackageVersionRequestFailed)
        {
        }
        else if (message is PatchEventDefine.PackageManifestUpdateFailed)
        {
        }
        else if (message is PatchEventDefine.WebFileDownloadFailed)
        {
        }
        else
        {
            throw new System.NotImplementedException($"{message.GetType()}");
        }

        Debug.Log("message:" + message.GetType());
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
    }
}