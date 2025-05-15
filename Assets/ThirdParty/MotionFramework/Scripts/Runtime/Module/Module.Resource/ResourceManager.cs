﻿//--------------------------------------------------
// Motion Framework
// Copyright©2018-2021 何冠峰
// Licensed under the MIT license
//--------------------------------------------------
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using YooAsset;

namespace MotionFramework.Resource
{
	/// <summary>
	/// 资源管理器
	/// </summary>
	public sealed class ResourceManager : ModuleSingleton<ResourceManager>, IModule
	{
		private InitializeParameters _createParameters;
		private ResourcePackage _defaultPackage;
		private string _locationRoot;

		void IModule.OnCreate(System.Object param)
		{
			YooAssets.Initialize();
		}
		void IModule.OnUpdate()
		{
		}
		void IModule.OnDestroy()
		{
			ForceUnloadAllAssets("");
			DestroySingleton();
		}
		void IModule.OnGUI()
		{
			//ConsoleGUI.Lable($"[{nameof(PatchManager)}] Run Mode : {_runMode}");
			//ConsoleGUI.Lable($"[{nameof(PatchManager)}] Dwonloader : {DownloadSystem.GetDownloaderTotalCount()}");
			//ConsoleGUI.Lable($"[{nameof(ResourceManager)}] Bundle count : {AssetSystem.GetLoaderCount()}");
			//ConsoleGUI.Lable($"[{nameof(ResourceManager)}] Asset loader count : {AssetSystem.GetProviderCount()}");
		}
		void IModule.OnReset()
		{
			YooAssets.Destroy();
		}
		/// <summary>
		/// 获取package
		/// </summary>
		public ResourcePackage GetPackage(string packageName)
		{
			return YooAssets.GetPackage(packageName);
		}

		/// <summary>
		/// 是否需要从远端更新下载
		/// </summary>
		/// <param name="location">资源的定位地址</param>
		public bool IsNeedDownloadFromRemote(string location)
		{
			return YooAssets.IsNeedDownloadFromRemote(location);
		}

		/// <summary>
		/// 获取资源对象信息列表
		/// </summary>
		public AssetInfo[] GetBundleInfo(string[] tags)
		{
			return YooAssets.GetAssetInfos(tags);
		}
		

		/// <summary>
		/// 资源回收（卸载引用计数为零的资源）
		/// </summary>
		public void UnloadUnusedAssets(string packName = "DefaultPackage")
		{
			var pack = YooAssets.GetPackage(packName);
			if (pack != null)
			{
				// TODO
				// pack.UnloadUnusedAssets();
			}
		}

		/// <summary>
		/// 强制回收所有资源
		/// </summary>
		public void ForceUnloadAllAssets(string packName = "DefaultPackage")
		{
			var pack = YooAssets.GetPackage(packName);
			if (pack != null)
			{
				// TODO
				// pack.ForceUnloadAllAssets();
			}
		}
		/// <summary>
		/// 释放资源对象
		/// </summary>
		public void Release(AssetHandle handle)
		{
			handle.Release();
		}

		#region 场景加载接口
		/// <summary>
		/// 异步加载场景
		/// </summary>
		public SceneHandle LoadSceneAsync(string location, LoadSceneMode sceneMode = LoadSceneMode.Single, bool activeOnLoad = true,LocalPhysicsMode physicsMode = LocalPhysicsMode.None, uint priority = 100)
		{
			location = _locationRoot + location;
			return YooAssets.LoadSceneAsync(location, sceneMode, LocalPhysicsMode.None, activeOnLoad, priority);
		}
		#endregion

		#region 资源加载接口
		/// <summary>
		/// 同步加载资源对象
		/// </summary>
		/// <param name="location">资源对象相对路径</param>
		public AssetHandle LoadAssetSync<TObject>(string location) where TObject : UnityEngine.Object
		{
			location = _locationRoot + location;
			return YooAssets.LoadAssetSync<TObject>(location);
		}

		public AssetHandle LoadAssetSync(System.Type type, string location)
		{
			location = _locationRoot + location;
			return YooAssets.LoadAssetSync(location, type);
		}

		/// <summary>
		/// 同步加载子资源对象集合
		/// </summary>
		/// <param name="location">资源对象相对路径</param>
		public SubAssetsHandle LoadSubAssetsSync<TObject>(string location) where TObject : UnityEngine.Object
		{
			location = _locationRoot + location;
			return YooAssets.LoadSubAssetsSync<TObject>(location);
		}

		public SubAssetsHandle LoadSubAssetsSync(System.Type type, string location)
		{
			location = _locationRoot + location;
			return YooAssets.LoadSubAssetsSync(location, type);
		}

		/// <summary>
		/// 异步加载资源对象
		/// </summary>
		/// <param name="location">资源对象相对路径</param>
		public AssetHandle LoadAssetAsync<TObject>(string location) where TObject : UnityEngine.Object
		{
			location = _locationRoot + location;
			return YooAssets.LoadAssetAsync<TObject>(location);
		}
		public AssetHandle LoadAssetAsync(System.Type type, string location)
		{
			location = _locationRoot + location;
			return YooAssets.LoadAssetAsync(location, type);
		}

		/// <summary>
		/// 异步加载子资源对象集合
		/// </summary>
		/// <param name="location">资源对象相对路径</param>
		public SubAssetsHandle LoadSubAssetsAsync<TObject>(string location) where TObject : UnityEngine.Object
		{
			location = _locationRoot + location;
			return YooAssets.LoadSubAssetsAsync<TObject>(location);
		}
		public SubAssetsHandle LoadSubAssetsAsync(System.Type type, string location)
		{
			location = _locationRoot + location;
			return YooAssets.LoadSubAssetsAsync(location, type);
		}
		#endregion

		#region 资源下载接口
		/// <summary>
		/// 创建补丁下载器
		/// </summary>
		/// <param name="dlcTag">DLC标记</param>
		/// <param name="fileLoadingMaxNumber">同时下载的最大文件数</param>
		/// <param name="failedTryAgain">下载失败的重试次数</param>
		public DownloaderOperation CreateDLCDownloader(string dlcTag, int fileLoadingMaxNumber, int failedTryAgain)
		{
			return YooAssets.CreateResourceDownloader(dlcTag, fileLoadingMaxNumber, failedTryAgain);
		}

		/// <summary>
		/// 创建补丁下载器
		/// </summary>
		/// <param name="dlcTags">DLC标记列表</param>
		/// <param name="fileLoadingMaxNumber">同时下载的最大文件数</param>
		/// <param name="failedTryAgain">下载失败的重试次数</param>
		public DownloaderOperation CreateDLCDownloader(string[] dlcTags, int fileLoadingMaxNumber, int failedTryAgain)
		{
			return YooAssets.CreateResourceDownloader(dlcTags, fileLoadingMaxNumber, failedTryAgain);
		}

		/// <summary>
		/// 创建补丁下载器
		/// </summary>
		/// <param name="locations">资源列表</param>
		/// <param name="fileLoadingMaxNumber">同时下载的最大文件数</param>
		/// <param name="failedTryAgain">下载失败的重试次数</param>
		public DownloaderOperation CreateBundleDownloader(string[] locations, int fileLoadingMaxNumber, int failedTryAgain)
		{
			return YooAssets.CreateBundleDownloader(locations, fileLoadingMaxNumber, failedTryAgain);
		}
		#endregion

		#region 沙盒相关
		/// <summary>
		/// 清空沙盒目录
		/// 注意：可以使用该方法修复我们本地的客户端
		/// </summary>
		public void ClearSandbox()
		{
			YooAssets.ClearSandbox();
		}

		/// <summary>
		/// 获取沙盒文件夹的路径
		/// </summary>
		public static string GetSandboxRoot()
		{
			return YooAssets.GetSandboxRoot();
		}
		#endregion
	}
}