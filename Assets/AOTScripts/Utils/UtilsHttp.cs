using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using System.IO;

using Newtonsoft.Json.Linq;


public partial class UtilsHttp
    {
        static Dictionary<string, Texture2D> textures = new Dictionary<string, Texture2D>();

        static Dictionary<Image, Coroutine> imageCoroutineDict = new Dictionary<Image, Coroutine>();
        private static string HttpImgPath = $"{Application.persistentDataPath}/HttpImg/";
        public static void HttpImg(Image img, string url)
        {
            if (ShellConfig.DEBUG > 1) Debug.LogFormat("HTTP IMAGE:{0}", url);
            if (textures.TryGetValue(url, out var tex))
            {
                img.sprite = Sprite.Create(tex, new Rect(0, 0, tex.width, tex.height), Vector2.zero);
            }
            else
            {
                if (imageCoroutineDict.TryGetValue(img, out var value))
                {
                    if (ShellConfig.DEBUG > 1) Debug.LogFormat("HTTP IMAGE StopCoroutine:{0}", url);
                    MotionFramework.MotionEngine.StopCoroutine(value);
                    imageCoroutineDict.Remove(img);
                }
                Coroutine coroutine = MotionFramework.MotionEngine.StartCoroutine(HttpImgCoroutine(img, url));
                if (coroutine != null) imageCoroutineDict.Add(img, coroutine);
            }
        }

        public static void HttpGet(string url, int timeout, Action<int, string> cb = null)
        {
            if (ShellConfig.DEBUG > 1) Debug.LogFormat("HTTP GET:{0}", url);

            MotionFramework.MotionEngine.StartCoroutine(HttpGetCoroutine(url, timeout, cb));
        }

        public static void HttpGet2(string url, int timeout, Action<int, string> cb = null)
        {
            if (ShellConfig.DEBUG > 1) Debug.LogFormat("HTTP GET:{0}", url);

            MotionFramework.MotionEngine.StartCoroutine(HttpGetCoroutine2(url, timeout, cb));
        }

        public static void HttpPost(string url, string data, int timeout, Action<int, string> cb)
        {
            if (ShellConfig.DEBUG > 1) Debug.LogFormat("HTTP POST:{0} {1}", url, data);

            List<IMultipartFormSection> formData = new List<IMultipartFormSection>();
            formData.Add(new MultipartFormDataSection(data));
            MotionFramework.MotionEngine.StartCoroutine(HttpPostCoroutine(url, formData, timeout, cb));
        }

        public static void HttpPost(string url, SortedDictionary<string, string> paramsDict, int timeout,
            Action<int, string> cb)
        {
            if (ShellConfig.DEBUG > 1) Debug.LogFormat("HTTP POST:{0}", url);

            WWWForm form = new WWWForm();
            foreach (KeyValuePair<string, string> kvp in paramsDict)
            {
                form.AddField(kvp.Key, kvp.Value);
            }

            form.AddField("code", GetUrlParamsCode(paramsDict));
            MotionFramework.MotionEngine.StartCoroutine(HttpPostCoroutine(url, form, timeout, cb));
        }

        public static void HttpPostJson(string url, string json, int timeout, Action<bool, int, string> cb)
        {
            if (ShellConfig.DEBUG > 1) Debug.LogFormat("HTTP POST:{0} \n {1}", url, json);

            MotionFramework.MotionEngine.StartCoroutine(HttpPostJsonCoroutine(url, json, timeout, cb));
        }

        public static void HttpPostJsonGame(string url, string json, int timeout, Action<bool, int, JObject> cb)
        {
            if (ShellConfig.DEBUG > 1) Debug.LogFormat("HTTP HttpPostJsonGame >>>>>>>>>> Send: {0} \n {1}", url, json);

            MotionFramework.MotionEngine.StartCoroutine(HttpPostJsonCoroutineGame(url, json, timeout, cb));
        }


        public static string GetUrlParams(SortedDictionary<string, string> urlParamsDict)
        {
            return GetUrlParamsWithNicai("code", urlParamsDict);
        }

        public static string GetUrlParamsCode(SortedDictionary<string, string> urlParamsDict)
        {
            string paramStrForCode = "";
            foreach (KeyValuePair<string, string> kvp in urlParamsDict)
            {
                if (paramStrForCode.Length > 0)
                {
                    paramStrForCode += "&";
                }

                paramStrForCode = paramStrForCode + kvp.Key + "=" + kvp.Value;
            }

            string nicai = DesNicaiAndBase64(paramStrForCode);
            string code = GetMd5(nicai);
            return code;
        }

        public static string GetUrlParamsWithNicai(string nicaiKey, SortedDictionary<string, string> urlParamsDict)
        {
            string code = GetUrlParamsCode(urlParamsDict);
            string urlParams = "";
            foreach (KeyValuePair<string, string> kvp in urlParamsDict)
            {
                if (urlParams.Length > 0)
                {
                    urlParams += "&";
                }

                urlParams = urlParams + kvp.Key + "=" + WebUtility.UrlEncode(kvp.Value);
            }

            urlParams = urlParams + "&" + nicaiKey + "=" + code;
            return urlParams;
        }

        public static string GetUrlParamsWithoutNicai(SortedDictionary<string, string> urlParamsDict)
        {
            string paramStrForCode = "";
            foreach (KeyValuePair<string, string> kvp in urlParamsDict)
            {
                if (paramStrForCode.Length > 0)
                {
                    paramStrForCode += "&";
                }

                paramStrForCode = paramStrForCode + kvp.Key + "=" + kvp.Value;
            }

            string urlParams = "";
            foreach (KeyValuePair<string, string> kvp in urlParamsDict)
            {
                if (urlParams.Length > 0)
                {
                    urlParams += "&";
                }

                urlParams = urlParams + kvp.Key + "=" + WebUtility.UrlEncode(kvp.Value);
            }

            return urlParams;
        }

        public static string AdjustUrl(string url)
        {
            return url.EndsWith("/") ? url : (url + "/");
        }

        static IEnumerator HttpImgCoroutine(Image img, string url)
        {
            // Fixed windows editor crash
            if (string.IsNullOrEmpty(url))
            {
                Debug
                    .LogWarning("HttpImgCoroutine : URL is empty or null.");
                yield break;
            }
            var downloadedFile = $"{HttpImgPath}{GetMd5(url)}";
            var requestFile = File.Exists(downloadedFile) ? "file://"+downloadedFile : url;
            using (UnityWebRequest uwr = UnityWebRequestTexture.GetTexture(requestFile))
            {
                yield return uwr.SendWebRequest();
                if (uwr.result == UnityWebRequest.Result.ConnectionError ||
                    uwr.result == UnityWebRequest.Result.ProtocolError)
                {
                    if (ShellConfig.DEBUG > 1) Debug.LogWarning(uwr.error);
                }
                else
                {
                    var tex = DownloadHandlerTexture.GetContent(uwr);
                    if (!img.IsDestroyed())
                    {
                        img.sprite = Sprite.Create(tex, new Rect(0, 0, tex.width, tex.height), Vector2.zero);
                    }
                    
                    textures[url] = tex;
                    if (!File.Exists(downloadedFile))
                    {
                        if (!Directory.Exists(HttpImgPath)) Directory.CreateDirectory(HttpImgPath);
                        try
                        {
                            File.WriteAllBytes(downloadedFile, uwr.downloadHandler.data);
                        }
                        catch (Exception e)
                        {
                            File.Delete(downloadedFile);
                            Debug.LogWarning($"Save http image failed {e}");
                        }
                    }
                    
                }
            
                if (imageCoroutineDict.ContainsKey(img))
                {
                    imageCoroutineDict.Remove(img);
                }
            }
        }

        public static void HttpImgPngToNative(string url)
        {
            if (ShellConfig.DEBUG > 1) Debug.LogFormat("HTTP IMAGE:{0}", url);


            MotionFramework.MotionEngine.StartCoroutine(HttpImgCoroutine(url));
        }

        static IEnumerator HttpImgCoroutine(string url)
        {
            // Fixed windows editor crash
            if (string.IsNullOrEmpty(url))
            {
                if (ShellConfig.DEBUG > 1) Debug.LogWarning("HttpImgCoroutine : URL is empty or null.");
                yield break;
            }

            using (UnityWebRequest uwr = UnityWebRequestTexture.GetTexture(url))
            {
                yield return uwr.SendWebRequest();
                if (uwr.result == UnityWebRequest.Result.ConnectionError ||
                    uwr.result == UnityWebRequest.Result.ProtocolError)
                {
                    if (ShellConfig.DEBUG > 1) Debug.LogWarning(uwr.error);
                }
                else
                {
                    var tex = DownloadHandlerTexture.GetContent(uwr);
                    byte[] bytes = tex.EncodeToPNG();
                    UnityEngine.Object.Destroy(tex);

                    File.WriteAllBytes(Application.persistentDataPath + "/beauty_share.png", bytes);
                }
            }
        }

        static IEnumerator HttpGetCoroutine(string url, int timeout, Action<int, string> cb)
        {
            using (UnityWebRequest uwr = UnityWebRequest.Get(url))
            {
                uwr.timeout = timeout;
                yield return uwr.SendWebRequest();
                if (uwr.result == UnityWebRequest.Result.ConnectionError ||
                    uwr.result == UnityWebRequest.Result.ProtocolError)
                {
                    if (ShellConfig.DEBUG > 1) Debug.LogFormat("HTTP ERROR:{0}", uwr.error);

                    cb(-1, uwr.error);
                }
                else
                {
                    if (ShellConfig.DEBUG > 1) Debug.LogFormat("HTTP RES:{0}", uwr.downloadHandler.text);

                    cb(0, uwr.downloadHandler.text);
                }
            }
        }

        static IEnumerator HttpGetCoroutine2(string url, int timeout, Action<int, string> cb)
        {
            using (UnityWebRequest uwr = UnityWebRequest.Get(url))
            {
                uwr.timeout = timeout;
                yield return uwr.SendWebRequest();
                if (uwr.result == UnityWebRequest.Result.ConnectionError)
                {
                    cb(-1, uwr.error);
                }
                else if (uwr.result == UnityWebRequest.Result.ProtocolError)
                {
                    cb(-2, uwr.error);
                }
                else
                {
                    if (ShellConfig.DEBUG > 1) Debug.LogFormat("HTTP RES:{0}", uwr.downloadHandler.text);

                    cb(0, uwr.downloadHandler.text);
                }
            }
        }

        static IEnumerator HttpPostCoroutine(string serverURL, WWWForm form, int timeout, Action<int, string> cb = null)
        {
            using (UnityWebRequest uwr = UnityWebRequest.Post(serverURL, form))
            {
                uwr.timeout = timeout;
                yield return uwr.SendWebRequest();
                if (uwr.result == UnityWebRequest.Result.ConnectionError ||
                    uwr.result == UnityWebRequest.Result.ProtocolError)
                {
                    cb?.Invoke(-1, uwr.error);
                }
                else
                {
                    if (ShellConfig.DEBUG > 1) Debug.LogFormat("HTTP RES:{0}", uwr.downloadHandler.text);

                    cb?.Invoke(0, uwr.downloadHandler.text);
                }
            }
        }

        static IEnumerator HttpPostCoroutine(string serverURL, List<IMultipartFormSection> lstformData, int timeout,
            Action<int, string> cb = null)
        {
            using (UnityWebRequest uwr = UnityWebRequest.Post(serverURL, lstformData))
            {
                uwr.timeout = timeout;
                yield return uwr.SendWebRequest();
                if (uwr.result == UnityWebRequest.Result.ConnectionError ||
                    uwr.result == UnityWebRequest.Result.ProtocolError)
                {
                    cb?.Invoke(-1, uwr.error);
                }
                else
                {
                    if (ShellConfig.DEBUG > 1) Debug.LogFormat("HTTP RES:{0}", uwr.downloadHandler.text);

                    cb?.Invoke(0, uwr.downloadHandler.text);
                }
            }
        }

        static IEnumerator HttpPostJsonCoroutine(string serverURL, string jsonStr, int timeout,
            Action<bool, int, string> cb = null)
        {
            if (ShellConfig.DEBUG > 1) Debug.LogFormat("UtilsHttp:HttpPostJsonCoroutine Send>>>>>>>>>>>>:{0}", jsonStr);

            using (UnityWebRequest uwr = new UnityWebRequest(serverURL, UnityWebRequest.kHttpVerbPOST))
            {
                uwr.timeout = timeout;
                uwr.SetRequestHeader("Content-Type", "application/json;charset=utf-8");

                uwr.uploadHandler = new UploadHandlerRaw(Utils.EncodeHttpText(jsonStr));
                uwr.downloadHandler = new DownloadHandlerBuffer();
                yield return uwr.SendWebRequest();
                if (uwr.result == UnityWebRequest.Result.ConnectionError ||
                    uwr.result == UnityWebRequest.Result.ProtocolError)
                {
                    if (ShellConfig.DEBUG > 1) Debug.LogFormat("UtilsHttp:HttpPostJsonCoroutine Error:{0}", uwr.error);

                    cb?.Invoke(false, -1, uwr.error);
                }
                else
                {
                    if (ShellConfig.DEBUG > 1)
                        Debug.LogFormat("UtilsHttp:HttpPostJsonCoroutine Success:{0}",
                            uwr.downloadHandler == null ? " " : uwr.downloadHandler.text);

                    cb?.Invoke(true, 0, uwr.downloadHandler.text);
                }
            }
        }

        static IEnumerator HttpPostJsonCoroutineGame(string serverURL, string jsonStr, int timeout,
            Action<bool, int, JObject> cb = null)
        {
            using (UnityWebRequest uwr = new UnityWebRequest(serverURL, UnityWebRequest.kHttpVerbPOST))
            {
                uwr.timeout = timeout;
                uwr.SetRequestHeader("Content-Type", "application/json;charset=utf-8");

                uwr.uploadHandler = new UploadHandlerRaw(Utils.EncodeHttpText(jsonStr));
                uwr.downloadHandler = new DownloadHandlerBuffer();
                yield return uwr.SendWebRequest();
                if (uwr.result == UnityWebRequest.Result.ConnectionError ||
                    uwr.result == UnityWebRequest.Result.ProtocolError)
                {
                    if (ShellConfig.DEBUG > 1)
                        Debug.LogFormat("UtilsHttp:HttpPostJsonCoroutine <<<<<<<<<<<Receive ERROR:{0}", uwr.error);

                    cb?.Invoke(false, -1, JObject.Parse(uwr.error));
                }
                else
                {
                    var response = uwr.downloadHandler.text;
                    var responseDict = JObject.Parse(Utils.DecodeHttpText(response));
                    var responseCode = (int)responseDict["code"];


                    if (ShellConfig.DEBUG > 1)
                        Debug.LogFormat("UtilsHttp:HttpPostJsonCoroutine <<<<<<<<<<<Receive:\n{0}<<<<<<<<<<{1}\n{2}",
                            serverURL,
                            responseCode, responseDict.ToString());


                    cb?.Invoke(responseCode == 100, responseCode, responseDict);
                }
            }
        }
    }

