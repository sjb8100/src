using System;
using System.Collections.Generic;

public class KSceneObject : KBaseObject
{
    //public float m_fX;
    //public float m_fY;
    //public float m_fZ;

    public KScene m_pScene = null;
    public float m_fTouchRange = 0f;

    public virtual int Init()
    {
        return 1;
    }

    public virtual void UnInit()
    {

    }

    public virtual void GetAbsoluteCoordinate(ref float pfX, ref float pfZ)
    {

    }
}

