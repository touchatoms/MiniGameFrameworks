using UnityEngine;


public class GActionInstant : GAction
{
    override public void Step(float dt)
    {
        Update(1);
    }
}

public class GActionShow : GActionInstant
{
    override public GAction Clone()
    {
        return new GActionShow();
    }

    override public void Update(float time)
    {
        target.SetActive(true);
    }
}

public class GActionHide : GActionInstant
{
    override public GAction Clone()
    {
        return new GActionHide();
    }

    override public void Update(float time)
    {
        target.SetActive(false);
    }
}

public class GActionRemoveSelf : GActionInstant
{
    override public GAction Clone()
    {
        return new GActionRemoveSelf();
    }

    override public void Update(float time)
    {
        Object.Destroy(target);
    }
}

public class GActionFlipX : GActionInstant
{
    override public GAction Clone()
    {
        return new GActionFlipX();
    }

    override public void Update(float time)
    {
        SpriteRenderer sr = target.GetComponent<SpriteRenderer>();
        if (sr != null)
        {
            sr.flipX = !sr.flipX;
        }
    }
}

public class GActionFlipY : GActionInstant
{
    override public GAction Clone()
    {
        return new GActionFlipY();
    }

    override public void Update(float time)
    {
        SpriteRenderer sr = target.GetComponent<SpriteRenderer>();
        if (sr != null)
        {
            sr.flipY = !sr.flipY;
        }
    }
}

public class GActionCallFunc : GActionInstant
{
    CallBack _function;

    public GActionCallFunc(CallBack cb)
    {
        _function = cb;
    }

    override public GAction Clone()
    {
        return new GActionCallFunc(_function);
    }

    override public void Update(float time)
    {
        _function();
    }
}