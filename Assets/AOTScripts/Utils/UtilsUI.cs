using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Events;


public static class UtilsUI
{
    // public static void SetLoadingProgress(float progress)
    // {
    //     GameObject uiLoadingPrefas = GameObject.Find("UICanvas/UILoading");
    //     if (uiLoadingPrefas != null)
    //     {
    //         uiLoadingPrefas.GetComponent<UILoading>().SetProgress(progress);
    //     }
    // }
    //
    // public static float GetLoadingProgress()
    // {
    //     GameObject uiLoadingPrefas = GameObject.Find("UICanvas/UILoading");
    //     if (uiLoadingPrefas != null)
    //     {
    //         return uiLoadingPrefas.GetComponent<UILoading>().GetProgress();
    //     }
    //
    //     return 0;
    // }
    //
    // public static void AddLoadingProgress(float progress)
    // {
    //     GameObject uiLoadingPrefas = GameObject.Find("UICanvas/UILoading");
    //     if (uiLoadingPrefas != null)
    //     {
    //         uiLoadingPrefas.GetComponent<UILoading>().AddProgress(progress);
    //     }
    // }
    //
    // public static void HideProgressLoading()
    // {
    //     GameObject uiLoadingPrefas = GameObject.Find("UICanvas/UILoading");
    //     if (uiLoadingPrefas != null)
    //     {
    //         uiLoadingPrefas.SetActive(false);
    //     }
    // }
    //
    // public static void SetInfoText(string textInfo)
    // {
    //     GameObject uiLoadingPrefas = GameObject.Find("UICanvas/UILoading");
    //     if (uiLoadingPrefas != null)
    //     {
    //         uiLoadingPrefas.GetComponent<UILoading>().SetInfoText(textInfo);
    //     }
    // }
    //
    // public static void ShowLoadingProgress()
    // {
    //     GameObject uiLoadingPrefas = GameObject.Find("UICanvas/UILoading");
    //
    //     if (uiLoadingPrefas != null)
    //     {
    //         GameObject progress =
    //             uiLoadingPrefas.transform.Find("image_bottom_bg/image_progress_slider").gameObject;
    //
    //         progress.SetActive(true);
    //     }
    // }
    //
    // public static void HideLoadingProgress()
    // {
    //     GameObject uiLoadingPrefas = GameObject.Find("UICanvas/UILoading");
    //     if (uiLoadingPrefas != null)
    //     {
    //         GameObject progress = uiLoadingPrefas.transform.Find("image_progress_slider").gameObject;
    //         progress.SetActive(false);
    //     }
    // }
    //
    // public static void Alert1Button(string content, System.Action confirmAction)
    // {
    //     GameObject UICanvas = GameObject.Find("UICanvas");
    //     GameObject alertShellGB = UICanvas.transform.Find("AlertShell").gameObject;
    //
    //     if (alertShellGB != null)
    //     {
    //         alertShellGB.SetActive(true);
    //         var messageBox = alertShellGB.GetComponent<AlertShell>();
    //         messageBox.Show1Button(content, confirmAction);
    //     }
    // }
    //
    // public static void Alert1Button(string content, string confirmButtonText, System.Action confirmAction)
    // {
    //     GameObject UICanvas = GameObject.Find("UICanvas");
    //     GameObject alertShellGB = UICanvas.transform.Find("AlertShell").gameObject;
    //
    //     if (alertShellGB != null)
    //     {
    //         alertShellGB.SetActive(true);
    //         var messageBox = alertShellGB.GetComponent<AlertShell>();
    //         messageBox.Show1Button(content, confirmButtonText, confirmAction);
    //     }
    // }
    //
    // public static void Alert2Button(string content, System.Action confirmAction)
    // {
    //     GameObject UICanvas = GameObject.Find("UICanvas");
    //     GameObject alertShellGB = UICanvas.transform.Find("AlertShell").gameObject;
    //
    //     if (alertShellGB != null)
    //     {
    //         alertShellGB.SetActive(true);
    //         var messageBox = alertShellGB.GetComponent<AlertShell>();
    //         messageBox.Show2Button(content, confirmAction);
    //     }
    // }
    //
    // public static void Alert2Button(string content, System.Action cancelAction, System.Action confirmAction)
    // {
    //     GameObject UICanvas = GameObject.Find("UICanvas");
    //     GameObject alertShellGB = UICanvas.transform.Find("AlertShell").gameObject;
    //
    //     if (alertShellGB != null)
    //     {
    //         alertShellGB.SetActive(true);
    //         var messageBox = alertShellGB.GetComponent<AlertShell>();
    //         messageBox.Show2Button(content, cancelAction, confirmAction);
    //     }
    // }
    //
    // public static void Alert2ButtonNoClose(string content, System.Action cancelAction, System.Action confirmAction)
    // {
    //     GameObject UICanvas = GameObject.Find("UICanvas");
    //     GameObject alertShellGB = UICanvas.transform.Find("AlertShell").gameObject;
    //
    //     if (alertShellGB != null)
    //     {
    //         alertShellGB.SetActive(true);
    //         var messageBox = alertShellGB.GetComponent<AlertShell>();
    //         messageBox.Show2Button(content, cancelAction, confirmAction);
    //         messageBox.SetButtonClosed(false);
    //     }
    // }
    //
    // public static void Alert1ButtonNoClose(string content, string confirmButtonText, System.Action confirmAction)
    // {
    //     GameObject UICanvas = GameObject.Find("UICanvas");
    //     GameObject alertShellGB = UICanvas.transform.Find("AlertShell").gameObject;
    //
    //     if (alertShellGB != null)
    //     {
    //         alertShellGB.SetActive(true);
    //         var messageBox = alertShellGB.GetComponent<AlertShell>();
    //         messageBox.Show1Button(content, confirmButtonText, confirmAction);
    //         messageBox.SetButtonClosed(false);
    //     }
    // }
    //
    // public static void CloseAlert()
    // {
    //     GameObject UICanvas = GameObject.Find("UICanvas");
    //     GameObject alertShellGB = UICanvas.transform.Find("AlertShell").gameObject;
    //     alertShellGB.SetActive(true);
    // }
    //
    // public static void ShowUIDebugPlayModeWin(System.Action cancelAction, System.Action confirmAction,
    //     System.Action loadFailAction = null)
    // {
    //     GameObject UICanvas = GameObject.Find("UICanvas");
    //     GameObject alertShellGB = UICanvas.transform.Find("UIDebugPlayModeWin")?.gameObject;
    //
    //     if (alertShellGB != null)
    //     {
    //         alertShellGB.SetActive(true);
    //         var messageBox = alertShellGB.GetComponent<UIDebugPlayModeWin>();
    //         messageBox.Show(cancelAction, confirmAction);
    //     }
    //     else
    //     {
    //         loadFailAction?.Invoke();
    //     }
    // }
    //
    // public static void Toast(string content)
    // {
    //     GameObject UICanvas = GameObject.Find("UICanvas");
    //     GameObject toastGb = UICanvas.transform.Find("Toast").gameObject;
    //
    //     if (toastGb != null)
    //     {
    //         toastGb.SetActive(true);
    //         var messageBox = toastGb.GetComponent<Toast>();
    //         messageBox.Show(content);
    //     }
    // }
}