using UnityEngine;


public enum EaseType
{
    Linear,
    SineIn,
    SineOut,
    SineInOut,
    QuadIn,
    QuadOut,
    QuadInOut,
    CubicIn,
    CubicOut,
    CubicInOut,
    QuartIn,
    QuartOut,
    QuartInOut,
    QuintIn,
    QuintOut,
    QuintInOut,
    ExpoIn,
    ExpoOut,
    ExpoInOut,
    CircIn,
    CircOut,
    CircInOut,
    ElasticIn,
    ElasticOut,
    ElasticInOut,
    BackIn,
    BackOut,
    BackInOut,
    BounceIn,
    BounceOut,
    BounceInOut,
    Custom
}

public class GAction
{
    public delegate void CallBack();

    public bool markDelete = false;
    public float duration = 0;

    protected GameObject target;
    protected EaseType ease = EaseType.Linear;

    virtual public GAction Clone()
    {
        return null;
    }

    virtual public bool IsDone()
    {
        return true;
    }

    virtual public void StartWithTarget(GameObject go)
    {
        target = go;
    }

    virtual public void Step(float dt)
    {
    }

    virtual public void Stop()
    {
        target = null;
    }

    virtual public void Update(float time)
    {
    }

    virtual public GAction Ease(EaseType ease)
    {
        this.ease = ease;
        return this;
    }

    /// static methods
    public static GActionShow Show()
    {
        return new GActionShow();
    }

    public static GActionHide Hide()
    {
        return new GActionHide();
    }

    public static GActionRemoveSelf RemoveSelf()
    {
        return new GActionRemoveSelf();
    }

    public static GActionFlipX FlipX()
    {
        return new GActionFlipX();
    }

    public static GActionFlipY FlipY()
    {
        return new GActionFlipY();
    }

    public static GActionCallFunc CallFunc(CallBack cb)
    {
        return new GActionCallFunc(cb);
    }

    public static GActionDelayTime DelayTime(float interval)
    {
        return new GActionDelayTime(interval);
    }

    public static GActionMoveBy MoveBy(float duration, Vector2 deltaPosition)
    {
        return new GActionMoveBy(duration, deltaPosition);
    }

    public static GActionMoveTo MoveTo(float duration, Vector3 endPosition)
    {
        return new GActionMoveTo(duration, endPosition);
    }

    public static GActionRotateBy RotateBy(float duration, float deltaAngle)
    {
        return new GActionRotateBy(duration, deltaAngle);
    }

    public static GActionRotateTo RotateTo(float duration, float destAngle)
    {
        return new GActionRotateTo(duration, destAngle);
    }

    public static GActionScaleTo ScaleTo(float duration, float sx, float sy, float sz = 1)
    {
        return new GActionScaleTo(duration, sx, sy, sz);
    }

    public static GActionScaleBy ScaleBy(float duration, float sx, float sy, float sz = 1)
    {
        return new GActionScaleBy(duration, sx, sy, sz);
    }

    public static GActionFadeTo FadeTo(float duration, float alpha)
    {
        return new GActionFadeTo(duration, alpha);
    }

    public static GActionTintTo TintTo(float duration, float r, float g, float b)
    {
        return new GActionTintTo(duration, r, g, b);
    }

    public static GActionBlendTo BlendTo(float duration, float a)
    {
        return new GActionBlendTo(duration, a);
    }

    public static GActionFlash Flash()
    {
        return new GActionFlash();
    }

    public static GActionSequence Sequence(params GAction[] arr)
    {
        return GActionSequence.Create(arr);
    }

    public static GActionSpawn Spawn(params GAction[] arr)
    {
        return GActionSpawn.Create(arr);
    }

    public static GActionRepeat Repeat(GAction act, uint times)
    {
        return new GActionRepeat(act, times);
    }

    public static GActionRepeatForever RepeatForever(GActionInterval act)
    {
        return new GActionRepeatForever(act);
    }

    public static GActionAnimate Animate(string name, string atlasName, string prefix, int[] frameIds,
        float interval)
    {
        return new GActionAnimate(name, atlasName, prefix, frameIds, interval);
    }

    public static GActionAnimate Animate(string name, string atlasName, string prefix, int frameCount,
        float interval)
    {
        return new GActionAnimate(name, atlasName, prefix, frameCount, interval);
    }

    //added by qzb
    public static GActionBezierTo BezierTo(float duration, Vector3 p0, Vector3 p1, Vector3 p2, Vector3 p3)
    {
        return new GActionBezierTo(duration, p0, p1, p2, p3);
    }

    public static GActionCanvasGroupAlphaFadeTo CanvasGroupAlphaFadeTo(float duration, float alpha)
    {
        return new GActionCanvasGroupAlphaFadeTo(duration, alpha);
    }
}