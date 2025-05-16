using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using HybridCLR;
using MotionFramework.Resource;
using MotionFramework.Scene;
using UnityEngine;
using UniFramework.Machine;
using UnityEngine.Networking;

internal class FsmStartGame : IStateNode
{
    private PatchOperation _owner;

    void IStateNode.OnCreate(StateMachine machine)
    {
        _owner = machine.Owner as PatchOperation;
    }

    void IStateNode.OnEnter()
    {
        PatchEventDefine.PatchStepsChange.SendEventMessage("开始游戏！");
        _owner.SetFinish();
        StartGame();
    }

    void OnFinish(int progress)
    {
    }

    void OnProgress(int progress)
    {
    }

    private TextAsset LoadGameDataAsset()
    {
        string location = "Assets/UpdateDlls/HotUpdate.dll";
        return LoadAsset(location);
    }

    /// <summary>
    /// 同步加载程序集文件
    /// </summary>
    private TextAsset LoadAsset(string location)
    {
        var handle = ResourceManager.Instance.LoadAssetSync<TextAsset>(location + ".bytes");
        if (handle.Status != YooAsset.EOperationStatus.Succeed)
        {
            Debug.LogError($"game data load failed!");
        }

        Debug.Log($"game data load:" + handle.AssetObject);
        return handle.AssetObject as TextAsset;
    }

    void IStateNode.OnUpdate()
    {
    }

    void IStateNode.OnExit()
    {
    }


    #region download assets

    private static Dictionary<string, byte[]> s_assetDatas = new Dictionary<string, byte[]>();

    public static byte[] ReadBytesFromStreamingAssets(string dllName)
    {
        return s_assetDatas[dllName];
    }

    private string GetWebRequestPath(string asset)
    {
        var path = $"{Application.streamingAssetsPath}/{asset}";
        if (!path.Contains("://"))
        {
            path = "file://" + path;
        }

        return path;
    }

    #endregion
    
    IEnumerator DownLoadAssets(Action onDownloadComplete)
    {
        var assets = new List<string>
        {
            "UpdateScripts.dll.bytes",
        };

        foreach (var asset in assets)
        {
            string dllPath = GetWebRequestPath(asset);
            Debug.Log($"start download asset:{dllPath}");
            UnityWebRequest www = UnityWebRequest.Get(dllPath);
            yield return www.SendWebRequest();

#if UNITY_2020_1_OR_NEWER
            if (www.result != UnityWebRequest.Result.Success)
            {
                Debug.Log(www.error);
            }
#else
            if (www.isHttpError || www.isNetworkError)
            {
                Debug.Log(www.error);
            }
#endif
            else
            {
                // Or retrieve results as binary data
                byte[] assetData = www.downloadHandler.data;
                Debug.Log($"dll:{asset}  size:{assetData.Length}");
                s_assetDatas[asset] = assetData;
            }
        }

        onDownloadComplete();
    }

    private static Assembly _hotUpdateAss;


    void StartGame()
    {
#if !UNITY_EDITOR
        Assembly _hotUpdateAss = Assembly.Load(ReadBytesFromStreamingAssets("UpdateScripts.dll.bytes"));
#else
        Assembly _hotUpdateAss =
            System.AppDomain.CurrentDomain.GetAssemblies().First(a => a.GetName().Name == "UpdateScripts");
#endif
        Type entryType = _hotUpdateAss.GetType("Entry");
        entryType.GetMethod("Start").Invoke(null, null);

        // ResourceManager.Instance.LoadSceneAsync()
        
        SceneManager.Instance.ChangeMainScene("Assets/UpdateRes/Scenes/HotUpdateLauncherScene");
    }
    
}