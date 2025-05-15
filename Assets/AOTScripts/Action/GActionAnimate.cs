using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using MotionFramework.Resource;
using YooAsset;


public class AnimationParam
{
    public Sprite[] frames;
    public float interval;
}

public class FrameManager
{
    private class AtlasWrapper
    {
        
        public SubAssetsHandle Handle { private set; get; }
        public string AtlasName { private set; get; }
        private readonly Object[] _sprites;
        private bool _isRelease = false;

        public AtlasWrapper(SubAssetsHandle handle, string altasName, Object[] sprites)
        {
            Handle = handle;
            AtlasName = altasName;
            _sprites = sprites;
        }

        public Sprite GetSprite(string spriteName, bool warning)
        {
            if (_sprites == null)
                return null;
            foreach (var sp in _sprites)
            {
                if (sp.name == spriteName)
                    return sp as Sprite;
            }

            if (warning)
                Debug.LogWarning($"Not found sprite {spriteName} in altas {AtlasName}");
            return null;
        }

        public void Release()
        {
            if (_isRelease == false)
            {
                _isRelease = true;
                Handle.Release();
            }
        }
    }

    private static Dictionary<string, Sprite[]> frameMap = new Dictionary<string, Sprite[]>();

    private static Sprite GetSprite(string atlasName, string spriteName)
    {
        // //TODO
        // var handle = ResourceManager.Instance.LoadSubAssetsSync<Sprite>(atlasName);
        // if (handle.Status == EOperationStatus.Succeed)
        // {
        //     AtlasWrapper newWrapper = new AtlasWrapper(handle, atlasName, handle.AllAssetObjects);
        //     return newWrapper.GetSprite(spriteName, true);
        // }
        // else
        // {
        //     Debug.LogError($"加载图集资源失败 : {atlasName}");
        //     return null;
        // }
        return null;
    }

    public static AnimationParam GetParam(string name, string atlasName, string prefix, int[] frameIds,
        float interval)
    {
        Sprite[] frames = null;
        frameMap.TryGetValue(name, out frames);
        if (frames == null)
        {
            frames = new Sprite[frameIds.Length];
            for (int i = 0; i < frameIds.Length; i++)
            {
                int frameId = frameIds[i];
                string spriteName = prefix + (frameId < 10 ? "0" + frameId : "" + frameId);
                Sprite sprite = GetSprite(atlasName, spriteName);
                frames[i] = sprite;
            }

            frameMap[name] = frames;
        }

        AnimationParam param = new AnimationParam();
        param.frames = frames;
        param.interval = interval;
        return param;
    }

    public static AnimationParam GetParam(string name, string pkg, string prefix, int frameCount, float interval)
    {
        if (frameMap.ContainsKey(name))
        {
            return GetParam(name, null, null, null, interval);
        }
        else
        {
            var frameIds = new int[frameCount];
            for (int i = 0; i < frameCount; i++)
            {
                frameIds[i] = i + 1;
            }

            return GetParam(name, pkg, prefix, frameIds, interval);
        }
    }
}

public class GActionAnimate : GActionInterval
{
    AnimationParam param;
    SpriteRenderer sr;
    Image img;

    public GActionAnimate(AnimationParam param)
    {
        this.param = param;
        duration = param.frames.Length * param.interval;
    }

    public GActionAnimate(string name, string pkg, string prefix, int[] frameIds, float interval)
    {
        param = FrameManager.GetParam(name, pkg, prefix, frameIds, interval);
        duration = param.frames.Length * param.interval;
    }

    public GActionAnimate(string name, string pkg, string prefix, int frameCount, float interval)
    {
        param = FrameManager.GetParam(name, pkg, prefix, frameCount, interval);
        duration = param.frames.Length * param.interval;
    }

    override public GAction Clone()
    {
        var _param = new AnimationParam();
        _param.frames = param.frames;
        _param.interval = param.interval;

        return new GActionAnimate(_param);
    }

    override public void StartWithTarget(GameObject go)
    {
        base.StartWithTarget(go);
        sr = go.GetComponent<SpriteRenderer>();
        if (sr != null)
        {
            sr.sprite = param.frames[0];
        }
        else
        {
            img = go.GetComponent<Image>();
            if (img != null)
            {
                img.sprite = param.frames[0];
            }
        }
    }

    override public void Update(float time)
    {
        int len = param.frames.Length;
        int frameIndex = (int)(len * time);
        if (frameIndex >= len)
        {
            frameIndex = len - 1;
        }

        if (sr != null)
        {
            sr.sprite = param.frames[frameIndex];
        }
        else if (img != null)
        {
            img.sprite = param.frames[frameIndex];
        }
    }
}