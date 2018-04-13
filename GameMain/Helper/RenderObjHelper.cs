using System;
using System.Collections.Generic;

public class RenderObjHelper
{
    public static void BeginDissolveEffect(Engine.IRenderObj obj)
    {
        if (obj == null)
            return;

        UnityEngine.GameObject gameObject = obj.GetNode().GetTransForm().gameObject;

        DissolveScript[] component = gameObject.GetComponentsInChildren<DissolveScript>(true);
        for (int i = 0; i < component.Length; i++)
        {
            component[i].BeginDissolve();
        }

    }

    public static void EndDissolveEffect(Engine.IRenderObj obj)
    {
        if (obj == null)
            return;

        //UnityEngine.GameObject gameObject = obj.GetNode().GetTransForm().gameObject;
        if (obj.GetNode() == null)
        {
            return;
        }

        if (obj.GetNode().GetTransForm() == null)
        {
            return;
        }

        UnityEngine.GameObject gameObject = obj.GetNode().GetTransForm().gameObject;
        if (gameObject == null)
        {
            return;
        }

        DissolveScript[] component = gameObject.GetComponentsInChildren<DissolveScript>(true);
        for (int i = 0; i < component.Length; i++)
        {
            component[i].EndDissolve();
        }

    }
}

