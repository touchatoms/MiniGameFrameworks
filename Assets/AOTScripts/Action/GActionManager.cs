using System.Collections.Generic;
using UnityEngine;
using System;

public static class GameObjectExtension
{
    public static void RunAction(this GameObject go, GAction act)
    {
        GActionManager.Run(go, act);
    }

    public static void StopAction(this GameObject go, GAction act)
    {
        GActionManager.Stop(go, act);
    }

    public static void StopAllActions(this GameObject go)
    {
        GActionManager.StopAll(go);
    }
}


public class StopWhenDestroy : MonoBehaviour
{
    void OnDestroy()
    {
        GActionManager.StopAll(gameObject);
    }
}

public class GActionManager
{
    private static GActionManager _inst = new GActionManager();
    private Dictionary<GameObject, List<GAction>> actions = new Dictionary<GameObject, List<GAction>>();
    private List<KeyValuePair<GameObject, GAction>> cacheActions = new List<KeyValuePair<GameObject, GAction>>();

    public static GActionManager inst
    {
        get { return _inst; }
    }

    // private void Awake()
    // {
    //     inst = this;
    // }

    public static void Run(GameObject target, GAction act)
    {
        inst.cacheActions.Add(new KeyValuePair<GameObject, GAction>(target, act));
        if (target.GetComponent<StopWhenDestroy>() == null)
        {
            target.AddComponent<StopWhenDestroy>();
        }
    }

    public static void Stop(GameObject target, GAction act)
    {
        List<GAction> list = null;
        inst.actions.TryGetValue(target, out list);
        if (list != null)
        {
            if (list.Contains(act))
            {
                act.markDelete = true;
            }
        }
    }

    public static void StopAll(GameObject target)
    {
        List<GAction> list = null;
        inst.actions.TryGetValue(target, out list);
        if (list != null)
        {
            foreach (var act in list)
            {
                act.markDelete = true;
            }
        }

        foreach (var pair in inst.cacheActions)
        {
            if (pair.Key == target)
            {
                pair.Value.markDelete = true;
            }
        }
    }

    void _Run(GameObject target, GAction act)
    {
        List<GAction> list = null;
        inst.actions.TryGetValue(target, out list);
        if (list == null)
        {
            inst.actions[target] = list = new List<GAction>();
        }

        list.Add(act);
        act.StartWithTarget(target);
    }

    // 每帧执行一次，被Global.Update方法调用
    public void Update()
    {
        float deltaTime = Time.deltaTime;
        List<GameObject> finishedObj = new List<GameObject>();

        // cache actions
        if (cacheActions.Count > 0)
        {
            foreach (var kv in cacheActions)
            {
                try
                {
                    _Run(kv.Key, kv.Value);
                }
                catch (MissingReferenceException e)
                {
                    Debug.LogWarning(e.ToString());
                }
                catch (NullReferenceException e)
                {
                    Debug.LogWarning(e.ToString());
                }
            }

            cacheActions.Clear();
        }

        foreach (var kv in actions)
        {
            GameObject go = kv.Key;
            var list = kv.Value;

            // finished all
            if (go == null || go.IsDestroyed() || list.Count == 0)
            {
                finishedObj.Add(go);
                continue;
            }

            GAction finishedAction = null;
            GAction action = null;
            for (int i = list.Count - 1; i > -1; i--)
            {
                action = list[i];
                if (action.markDelete)
                {
                    finishedAction = action;
                }
                else
                {
                    try
                    {
                        action.Step(deltaTime);
                        if (action.IsDone())
                        {
                            finishedAction = action;
                        }
                    }
                    catch (Exception e)
                    {
                        Debug.LogErrorFormat("GActionManager exception: {0}\nstacktrace: {1}", e.ToString(),
                            e.StackTrace);
                    }
                }

                // finished one
                if (finishedAction != null)
                {
                    list.Remove(finishedAction);
                    finishedAction = null;
                }
            }
        }

        // finished all
        if (finishedObj.Count > 0)
        {
            foreach (var deletObj in finishedObj)
            {
                actions.Remove(deletObj);
            }
        }
    }
}