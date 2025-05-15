using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;


public class GActionInterval : GAction
{
    public float elapsed = 0;

    protected bool firstTick = true;

    override public bool IsDone()
    {
        return elapsed >= duration;
    }

    override public void StartWithTarget(GameObject go)
    {
        target = go;
        elapsed = 0;
        firstTick = true;
    }

    override public void Step(float dt)
    {
        if (firstTick)
        {
            firstTick = false;
            elapsed = 0;
        }
        else
        {
            elapsed += dt;
        }

        float updateDt = Mathf.Max(0, Mathf.Min(1, elapsed / duration));
        Update(updateDt);
    }

    protected void InitWithDuration(float d)
    {
        duration = d;
        if (duration == 0)
        {
            duration = 1.192092896e-07F;
        }
    }
}

public class GActionDelayTime : GActionInterval
{
    public GActionDelayTime(float duration)
    {
        InitWithDuration(duration);
    }

    override public GAction Clone()
    {
        return new GActionDelayTime(duration);
    }
}

public class GActionMoveBy : GActionInterval
{
    protected Vector3 deltaPosition;
    protected Vector3 startPosition;
    protected Vector3 previousPosition;

    protected Transform trans;

    public GActionMoveBy()
    {
    }

    public GActionMoveBy(float duration, Vector3 _deltaPosition)
    {
        InitWithDuration(duration);
        deltaPosition = _deltaPosition;
    }

    override public GAction Clone()
    {
        return new GActionMoveBy(duration, deltaPosition);
    }

    override public void StartWithTarget(GameObject go)
    {
        base.StartWithTarget(go);
        trans = go.transform;
        previousPosition = startPosition = trans.localPosition;
    }

    override public void Update(float time)
    {
        time = EaseManager.Evaluate(ease, time, 1, 1.70158f, 0);
        if (target && trans)
        {
            Vector3 currentPos = trans.localPosition;
            Vector3 diff = currentPos - previousPosition;
            startPosition = startPosition + diff;
            Vector3 newPos = startPosition + (deltaPosition * time);
            trans.localPosition = newPos;
            previousPosition = newPos;
        }
    }
}

public class GActionMoveTo : GActionMoveBy
{
    Vector3 endPosition;

    public GActionMoveTo(float duration, Vector3 _endPosition)
    {
        InitWithDuration(duration);
        endPosition = _endPosition;
    }

    override public GAction Clone()
    {
        return new GActionMoveBy(duration, endPosition);
    }

    override public void StartWithTarget(GameObject go)
    {
        base.StartWithTarget(go);
        deltaPosition = endPosition - trans.localPosition;
    }

    override public void Update(float time)
    {
        base.Update(time);
        if (time == 1)
        {
            if (trans)
            {
                trans.localPosition = endPosition;
            }
        }
    }
}

public class GActionRotateBy : GActionInterval
{
    float startAngle;
    float deltaAngle;

    Transform trans;

    public GActionRotateBy(float duration, float _deltaAngle)
    {
        InitWithDuration(duration);
        deltaAngle = _deltaAngle;
    }

    override public GAction Clone()
    {
        return new GActionRotateBy(duration, deltaAngle);
    }

    override public void StartWithTarget(GameObject go)
    {
        base.StartWithTarget(go);
        trans = go.transform;
        startAngle = trans.localEulerAngles.z;
    }

    override public void Update(float time)
    {
        time = EaseManager.Evaluate(ease, time, 1, 1.70158f, 0);
        if (target != null)
        {
            float z = startAngle + deltaAngle * time;
            trans.localEulerAngles = new Vector3(0, 0, z);
        }
    }
}

public class GActionRotateTo : GActionInterval
{
    float startAngle;
    float endAngle;
    float deltaAngle;

    Transform trans;

    public GActionRotateTo(float duration, float destAngle)
    {
        InitWithDuration(duration);
        endAngle = destAngle;
    }

    override public GAction Clone()
    {
        return new GActionRotateBy(duration, endAngle);
    }

    override public void StartWithTarget(GameObject go)
    {
        base.StartWithTarget(go);
        trans = go.transform;
        startAngle = trans.localEulerAngles.z;

        if (startAngle > 0)
        {
            startAngle %= 360.0f;
        }
        else
        {
            startAngle %= -360.0f;
        }

        deltaAngle = endAngle - startAngle;
        if (deltaAngle > 180)
        {
            deltaAngle -= 360;
        }

        if (deltaAngle < -180)
        {
            deltaAngle += 360;
        }
    }

    override public void Update(float time)
    {
        time = EaseManager.Evaluate(ease, time, 1, 1.70158f, 0);
        if (target != null)
        {
            float z = startAngle + deltaAngle * time;
            trans.localEulerAngles = new Vector3(0, 0, z);
        }
    }
}

public class GActionScaleTo : GActionInterval
{
    protected float startScaleX;
    protected float startScaleY;
    protected float startScaleZ;
    protected float endScaleX;
    protected float endScaleY;
    protected float endScaleZ;
    protected float deltaX;
    protected float deltaY;
    protected float deltaZ;

    protected Transform trans;

    public GActionScaleTo()
    {
    }

    public GActionScaleTo(float duration, float sx, float sy, float sz)
    {
        InitWithDuration(duration);
        endScaleX = sx;
        endScaleY = sy;
        endScaleZ = sz;
    }

    override public GAction Clone()
    {
        return new GActionScaleTo(duration, endScaleX, endScaleY, endScaleZ);
    }

    override public void StartWithTarget(GameObject go)
    {
        base.StartWithTarget(go);
        trans = go.transform;
        startScaleX = trans.localScale.x;
        startScaleY = trans.localScale.y;
        startScaleZ = trans.localScale.z;
        deltaX = endScaleX - startScaleX;
        deltaY = endScaleY - startScaleY;
        deltaZ = endScaleZ - startScaleZ;
    }

    override public void Update(float time)
    {
        time = EaseManager.Evaluate(ease, time, 1, 1.70158f, 0);
        if (target != null)
        {
            trans.localScale = new Vector3(startScaleX + deltaX * time, startScaleY + deltaY * time,
                startScaleZ + deltaZ * time);
        }
    }
}

public class GActionScaleBy : GActionScaleTo
{
    public GActionScaleBy(float duration, float sx, float sy, float sz)
    {
        InitWithDuration(duration);
        endScaleX = sx;
        endScaleY = sy;
        endScaleZ = sz;
    }

    override public void StartWithTarget(GameObject go)
    {
        base.StartWithTarget(go);
        deltaX = startScaleX * endScaleX - startScaleX;
        deltaY = startScaleY * endScaleY - startScaleY;
        deltaZ = startScaleZ * endScaleZ - startScaleZ;
    }
}

public class GActionFadeTo : GActionInterval
{
    float startOpacity;
    float endOpacity;

    SpriteRenderer sr;
    Graphic graphic;
    float deltaOpacity;

    public GActionFadeTo(float duration, float opacity)
    {
        InitWithDuration(duration);
        endOpacity = opacity;
    }

    override public GAction Clone()
    {
        return new GActionFadeTo(duration, endOpacity);
    }

    override public void StartWithTarget(GameObject go)
    {
        base.StartWithTarget(go);
        sr = go.GetComponent<SpriteRenderer>();
        if (sr != null)
        {
            startOpacity = sr.color.a;
            deltaOpacity = endOpacity - startOpacity;
        }
        else
        {
            graphic = go.GetComponent<Graphic>();
            if (graphic != null)
            {
                startOpacity = graphic.color.a;
                deltaOpacity = endOpacity - startOpacity;
            }
        }
    }

    override public void Update(float time)
    {
        time = EaseManager.Evaluate(ease, time, 1, 1.70158f, 0);
        if (sr != null)
        {
            sr.color = new Color(sr.color.r, sr.color.g, sr.color.b, startOpacity + deltaOpacity * time);
        }
        else if (graphic != null)
        {
            graphic.color = new Color(graphic.color.r, graphic.color.g, graphic.color.b,
                startOpacity + deltaOpacity * time);
        }
    }
}

public class GActionTintTo : GActionInterval
{
    Color startColor;
    Color endColor;

    SpriteRenderer sr;
    Color deltaColor;

    public GActionTintTo(float duration, float r, float g, float b)
    {
        InitWithDuration(duration);
        endColor = new Color(r, g, b);
    }

    override public GAction Clone()
    {
        return new GActionTintTo(duration, endColor.r, endColor.g, endColor.g);
    }

    override public void StartWithTarget(GameObject go)
    {
        base.StartWithTarget(go);
        sr = go.GetComponent<SpriteRenderer>();
        if (sr != null)
        {
            startColor = sr.color;
            deltaColor = endColor - startColor;
        }
    }

    override public void Update(float time)
    {
        if (sr != null)
        {
            sr.color = startColor + deltaColor * time;
        }
    }
}

public class GActionBlendTo : GActionInterval
{
    Dictionary<Renderer, Dictionary<Material, PropertiesValue>> MeshRenderers =
        new Dictionary<Renderer, Dictionary<Material, PropertiesValue>>();

    Dictionary<SpriteRenderer, PropertiesValue> SpriteRenderers = new Dictionary<SpriteRenderer, PropertiesValue>();

    struct PropertiesValue
    {
        public float startAlpha;
        public Color color;
        public MaterialPropertyBlock block;
    }

    float endAlpha;

    public GActionBlendTo(float duration, float a)
    {
        InitWithDuration(duration);
        endAlpha = a;
    }

    override public GAction Clone()
    {
        return new GActionBlendTo(duration, endAlpha);
    }

    override public void StartWithTarget(GameObject go)
    {
        base.StartWithTarget(go);
        Renderer[] _renderers = go.GetComponentsInChildren<Renderer>();
        foreach (Renderer _renderer in _renderers)
        {
            if (_renderer.enabled == false)
                continue;
            if (_renderer.GetType() == typeof(SpriteRenderer))
            {
                SpriteRenderer _spriteRenderer = (SpriteRenderer)_renderer;
                PropertiesValue value = new PropertiesValue();
                value.startAlpha = _spriteRenderer.color.a;
                value.color = _spriteRenderer.color;
                SpriteRenderers.Add(_spriteRenderer, value);
            }
            else
            {
                Material[] _mats = _renderer.materials;
                Dictionary<Material, PropertiesValue> mats = new Dictionary<Material, PropertiesValue>();
                _mats = _renderer.materials;
                for (int i = 0; i < _mats.Length; i++)
                {
                    if (_mats[i] == null)
                        continue;
                    if (mats.ContainsKey(_mats[i]))
                        continue;

                    PropertiesValue value = new PropertiesValue();
                    value.block = new MaterialPropertyBlock();
                    if (_mats[i].HasProperty("_Alpha"))
                    {
                        value.startAlpha = _mats[i].GetFloat("_Alpha");
                        mats.Add(_mats[i], value);
                    }
                    else if (_mats[i].HasProperty("_Color"))
                    {
                        value.color = _mats[i].GetColor("_Color");
                        value.startAlpha = value.color.a;
                        mats.Add(_mats[i], value);
                    }
                    else if (_mats[i].HasProperty("_TintColor"))
                    {
                        value.color = _mats[i].GetColor("_TintColor");
                        value.startAlpha = value.color.a;
                        mats.Add(_mats[i], value);
                    }
                    else
                    {
                        continue;
                    }

                    if (_mats[i].shader.name == "CustomStandardV2")
                    {
                        //renderQueue to Transparent
                        _mats[i].SetOverrideTag("RenderType", "Transparent");
                        _mats[i].SetFloat("_Mode", 3);
                        _mats[i].SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
                        _mats[i].SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
                        //_mats[i].SetInt("_ZWrite", 0);
                        _mats[i].DisableKeyword("_ALPHATEST_ON");
                        _mats[i].DisableKeyword("_ALPHABLEND_ON");
                        _mats[i].EnableKeyword("_ALPHAPREMULTIPLY_ON");
                        _mats[i].renderQueue = (int)UnityEngine.Rendering.RenderQueue.Transparent;
                    }
                }

                MeshRenderers.Add(_renderer, mats);
            }
        }
    }

    override public void Update(float time)
    {
        foreach (var item in MeshRenderers)
        {
            int index = 0;
            Renderer renderer = item.Key;
            if (!renderer)
            {
                continue;
            }

            Dictionary<Material, PropertiesValue> mats = item.Value;
            foreach (var item2 in mats)
            {
                MaterialPropertyBlock block = item2.Value.block;
                Material mat = item2.Key;
                PropertiesValue value = item2.Value;
                renderer.GetPropertyBlock(block, index);
                float startAlpha = value.startAlpha;
                float alpha = startAlpha + (endAlpha - startAlpha) * time;
                if (mat.HasProperty("_Alpha"))
                {
                    block.SetFloat("_Alpha", alpha);
                    //Debug.Log( 1  + "  mat.GetFloatAlpha : " + mat.GetFloat("_Alpha") + "    alpha :" + alpha);
                    //mat.SetFloat("_Alpha", alpha);
                }
                else if (mat.HasProperty("_Color"))
                {
                    value.color.a = alpha;
                    block.SetColor("_Color", value.color);
                    //Debug.Log(2 + "  mat.GetColor : " + mat.GetColor("_Color") + "    value.color :" + value.color);
                    //mat.SetColor("_TintColor", value.color);
                }
                else if (mat.HasProperty("_TintColor"))
                {
                    value.color.a = alpha;
                    block.SetColor("_TintColor", value.color);
                    //mat.SetColor("_TintColor", value.color);
                }

                renderer.SetPropertyBlock(block, index);
                index++;
            }
        }

        foreach (var item in SpriteRenderers)
        {
            SpriteRenderer renderer = item.Key;
            if (renderer == null)
                continue;

            float startAlpha = item.Value.startAlpha;
            float alpha = startAlpha + (endAlpha - startAlpha) * time;
            Color color = item.Value.color;
            color.a = alpha;
            renderer.color = color;
        }
    }
}


public class GActionFlash : GActionInterval
{
    struct hitEffectInfo
    {
        public Color hitColor;
        public float hitMultiple;
        public float srcMultipleValue;
        public Color srcColor;
        public int MatIndex;
        public Renderer Renderer;
        public MaterialPropertyBlock MaterialPropertyBlock;
    }

    Dictionary<Material, hitEffectInfo> matDic = new Dictionary<Material, hitEffectInfo>();
    Renderer renderer;

    public GActionFlash()
    {
        InitWithDuration(0.4f);
    }

    override public GAction Clone()
    {
        return new GActionFlash();
    }

    override public void StartWithTarget(GameObject go)
    {
        base.StartWithTarget(go);
        renderer = go.GetComponent<Renderer>();
        if (renderer == null)
        {
            //Debug.LogWarning("GActionFlash :renderer == null");
            return;
        }

        for (int i = 0; i < renderer.materials.Length; i++)
        {
            Material item = renderer.materials[i];
            if (!item.HasProperty("_HitColor"))
            {
                //Debug.LogWarning("!item.HasProperty(_HitColor):" + item.shader.name);
                continue;
            }

            if (!item.HasProperty("_HitMultiple"))
            {
                //Debug.LogWarning("!item.HasProperty(_HitMultiple)");
                continue;
            }

            hitEffectInfo hitEffectInfo = new hitEffectInfo();
            hitEffectInfo.MatIndex = i;
            hitEffectInfo.Renderer = renderer;
            hitEffectInfo.MaterialPropertyBlock = new MaterialPropertyBlock();
            hitEffectInfo.hitColor = item.GetColor("_HitColor");
            hitEffectInfo.srcColor = item.GetColor("_OverlayColor");
            hitEffectInfo.hitMultiple = item.GetFloat("_HitMultiple");
            if (item.GetFloat("_HitColorChannel") == 0)
                hitEffectInfo.srcMultipleValue = 0;
            else
                hitEffectInfo.srcMultipleValue = 1;
            matDic.Add(item, hitEffectInfo);
        }
    }

    override public void Update(float time)
    {
        foreach (var item in matDic)
        {
            float alpha = 0;
            Material mat = item.Key;
            hitEffectInfo info = item.Value;
            MaterialPropertyBlock block = info.MaterialPropertyBlock;
            Color colorValue = Color.red;
            float multipleValueDst = 1;
            if (time < 0.5f)
            {
                alpha = time / 0.5f;
                colorValue = Color.Lerp(info.srcColor, info.hitColor, alpha);
                multipleValueDst = Mathf.Lerp(info.srcMultipleValue, info.hitMultiple, alpha);
            }
            else if (time >= 0.5f)
            {
                alpha = (time - 0.5f) / 0.5f;
                colorValue = Color.Lerp(info.hitColor, Color.white, alpha);
                multipleValueDst = Mathf.Lerp(info.hitMultiple, info.srcMultipleValue, alpha);
            }

            info.Renderer.GetPropertyBlock(block, info.MatIndex);
            block.SetColor("_OverlayColor", colorValue);
            block.SetFloat("_OverlayMultiple", multipleValueDst);
            info.Renderer.SetPropertyBlock(block, info.MatIndex);
        }
    }
}

public class GActionSequence : GActionInterval
{
    GAction[] actions = new GAction[2];
    float split;
    int last;

    public static GActionSequence CreateWithTwoActions(GAction actionOne, GAction actionTwo)
    {
        GActionSequence ret = new GActionSequence();
        ret.InitWithDuration(actionOne.duration + actionTwo.duration);
        ret.actions[0] = actionOne;
        ret.actions[1] = actionTwo;
        return ret;
    }

    public static GActionSequence Create(params GAction[] values)
    {
        if (values.Length == 1)
        {
            return CreateWithTwoActions(values[0], new GAction());
        }
        else
        {
            GAction prev = values[0];
            for (int i = 1; i < values.Length; i++)
            {
                prev = CreateWithTwoActions(prev, values[i]);
            }

            return (GActionSequence)prev;
        }
    }

    override public GAction Clone()
    {
        return CreateWithTwoActions(actions[0].Clone(), actions[1].Clone());
    }

    override public void StartWithTarget(GameObject go)
    {
        base.StartWithTarget(go);
        split = actions[0].duration / duration;
        last = -1;
    }

    override public void Stop()
    {
        if (last != -1)
        {
            actions[last].Stop();
        }

        base.Stop();
    }

    override public void Update(float time)
    {
        int found = 0;
        float new_t = 0.0f;

        if (time < split)
        {
            // action[0]
            found = 0;
            if (split != 0)
                new_t = time / split;
            else
                new_t = 1;
        }
        else
        {
            // action[1]
            found = 1;
            if (split == 1)
                new_t = 1;
            else
                new_t = (time - split) / (1 - split);
        }

        if (found == 1)
        {
            if (last == -1)
            {
                actions[0].StartWithTarget(target);
                actions[0].Update(1.0f);
                actions[0].Stop();
            }
            else if (last == 0)
            {
                actions[0].Update(1.0f);
                actions[0].Stop();
            }
        }
        else if (found == 0 && last == 1)
        {
            actions[1].Update(0);
            actions[1].Stop();
        }

        if (found == last && actions[found].IsDone())
        {
            return;
        }

        if (target == null)
        {
            return;
        }

        if (found != last)
        {
            actions[found].StartWithTarget(target);
        }

        actions[found].Update(new_t);
        last = found;
    }
}

public class GActionSpawn : GActionInterval
{
    public GAction _one;
    public GAction _two;

    public static GActionSpawn CreateWithTwoActions(GAction actionOne, GAction actionTwo)
    {
        GActionSpawn ret = new GActionSpawn();

        float d1 = actionOne.duration;
        float d2 = actionTwo.duration;
        ret.InitWithDuration(Mathf.Max(d1, d2));

        ret._one = actionOne;
        ret._two = actionTwo;

        if (d1 > d2)
        {
            ret._two = GActionSequence.CreateWithTwoActions(actionTwo, new GActionDelayTime(d1 - d2));
        }
        else if (d1 < d2)
        {
            ret._one = GActionSequence.CreateWithTwoActions(actionOne, new GActionDelayTime(d2 - d1));
        }

        return ret;
    }

    public static GActionSpawn Create(params GAction[] values)
    {
        if (values.Length == 1)
        {
            return CreateWithTwoActions(values[0], new GAction());
        }
        else
        {
            GAction prev = values[0];
            for (int i = 1; i < values.Length; i++)
            {
                prev = CreateWithTwoActions(prev, values[i]);
            }

            return (GActionSpawn)prev;
        }
    }

    override public GAction Clone()
    {
        return CreateWithTwoActions(_one.Clone(), _two.Clone());
    }

    override public void StartWithTarget(GameObject go)
    {
        base.StartWithTarget(go);
        _one.StartWithTarget(go);
        _two.StartWithTarget(go);
    }

    override public void Stop()
    {
        _one.Stop();
        _two.Stop();
        base.Stop();
    }

    override public void Update(float time)
    {
        if (_one != null)
        {
            _one.Update(time);
        }

        if (target == null)
        {
            return;
        }

        if (_two != null)
        {
            _two.Update(time);
        }
    }
}

public class GActionRepeat : GActionInterval
{
    public uint _times;
    public uint _total;
    public float _nextDt;
    public bool _actionInstant;
    GAction _innerAction;

    public GActionRepeat(GAction action, uint times)
    {
        InitWithDuration(action.duration * times);
        _times = times;
        _innerAction = action;

        _actionInstant = (action is GActionInstant);
        if (_actionInstant)
        {
            _times -= 1;
        }

        _total = 0;
    }

    override public GAction Clone()
    {
        return new GActionRepeat(_innerAction.Clone(), _times);
    }

    override public bool IsDone()
    {
        return _total == _times;
    }

    override public void StartWithTarget(GameObject go)
    {
        _total = 0;
        _nextDt = _innerAction.duration / duration;
        base.StartWithTarget(go);
        _innerAction.StartWithTarget(go);
    }

    override public void Stop()
    {
        _innerAction.Stop();
        base.Stop();
    }

    override public void Update(float time)
    {
        if (time >= _nextDt)
        {
            while (time > _nextDt && _total < _times)
            {
                _innerAction.Update(1.0f);
                _total++;

                _innerAction.Stop();
                _innerAction.StartWithTarget(target);
                _nextDt = _innerAction.duration / duration * (_total + 1);
            }

            if (time >= 1.0f && _total < _times)
            {
                _total++;
            }

            if (!_actionInstant)
            {
                if (_total == _times)
                {
                    _innerAction.Update(1);
                    _innerAction.Stop();
                }
                else
                {
                    _innerAction.Update(time - (_nextDt - _innerAction.duration / duration));
                }
            }
        }
        else
        {
            _innerAction.Update((time * _times) % 1.0f);
        }
    }
}

public class GActionRepeatForever : GActionInterval
{
    public GActionInterval _innerAction;

    public GActionRepeatForever(GActionInterval action)
    {
        _innerAction = action;
    }

    override public GAction Clone()
    {
        return new GActionRepeatForever((GActionInterval)_innerAction.Clone());
    }

    override public bool IsDone()
    {
        return false;
    }

    override public void StartWithTarget(GameObject go)
    {
        base.StartWithTarget(go);
        _innerAction.StartWithTarget(go);
    }

    override public void Step(float dt)
    {
        _innerAction.Step(dt);
        if (_innerAction.IsDone())
        {
            float diff = _innerAction.elapsed - _innerAction.duration;
            if (diff > _innerAction.duration)
                diff %= _innerAction.duration;
            _innerAction.StartWithTarget(target);
            _innerAction.Step(0.0f);
            _innerAction.Step(diff);
        }
    }
}

//added by qzb
public class GActionBezierTo : GActionInterval
{
    private Vector3 endPosition;
    private Vector3 startPosition;
    private Vector3 control1;
    private Vector3 control2;

    private Transform trans;

    public GActionBezierTo(float duration, Vector3 p0, Vector3 p1, Vector3 p2, Vector3 p3)
    {
        InitWithDuration(duration);
        startPosition = p0;
        control1 = p1;
        control2 = p2;
        endPosition = p3;
    }

    override public GAction Clone()
    {
        return new GActionBezierTo(duration, startPosition, control1, control2, endPosition);
    }

    override public void StartWithTarget(GameObject go)
    {
        base.StartWithTarget(go);
        trans = go.transform;
    }

    override public void Update(float time)
    {
        time = EaseManager.Evaluate(ease, time, 1, 1.70158f, 0);
        if (target != null)
        {
            Vector3 pos = Bezier(startPosition, control1, control2, endPosition, time);
            trans.localPosition = pos;
        }
    }

    // 三阶贝塞尔曲线,t取值范围0到1
    private static Vector3 Bezier(Vector3 p0, Vector3 p1, Vector3 p2, Vector3 p3, float t)
    {
        Vector3 result;
        Vector3 p0p1 = (1 - t) * p0 + t * p1;
        Vector3 p1p2 = (1 - t) * p1 + t * p2;
        Vector3 p2p3 = (1 - t) * p2 + t * p3;
        Vector3 p0p1p2 = (1 - t) * p0p1 + t * p1p2;
        Vector3 p1p2p3 = (1 - t) * p1p2 + t * p2p3;
        result = (1 - t) * p0p1p2 + t * p1p2p3;
        return result;
    }
}

public class GActionCanvasGroupAlphaFadeTo : GActionInterval
{
    float startOpacity;
    float endOpacity;

    CanvasGroup cg;
    Graphic graphic;
    float deltaOpacity;

    public GActionCanvasGroupAlphaFadeTo(float duration, float opacity)
    {
        InitWithDuration(duration);
        endOpacity = opacity;
    }

    override public GAction Clone()
    {
        return new GActionCanvasGroupAlphaFadeTo(duration, endOpacity);
    }

    override public void StartWithTarget(GameObject go)
    {
        base.StartWithTarget(go);
        cg = go.GetComponent<CanvasGroup>();
        if (cg != null)
        {
            startOpacity = cg.alpha;
            deltaOpacity = endOpacity - startOpacity;
        }
        else
        {
            graphic = go.GetComponent<Graphic>();
            if (graphic != null)
            {
                startOpacity = graphic.color.a;
                deltaOpacity = endOpacity - startOpacity;
            }
        }
    }

    override public void Update(float time)
    {
        time = EaseManager.Evaluate(ease, time, 1, 1.70158f, 0);
        if (cg != null)
        {
            cg.alpha = startOpacity + deltaOpacity * time;
        }
        else if (graphic != null)
        {
            graphic.color = new Color(graphic.color.r, graphic.color.g, graphic.color.b,
                startOpacity + deltaOpacity * time);
        }
    }
}