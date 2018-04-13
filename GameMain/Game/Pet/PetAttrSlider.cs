using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

class PetAttrSlider : UISlider
{
    
    //protected override void OnStart()
    //{
    //    if(value < m_initValue)
    //    {
    //        value = m_initValue;
    //        return;
    //    }
    //    base.OnStart();
    //}
    float m_initValue;
    public string propStr;
    public void InitData(float initValue,string prop)
    {
        propStr = prop;
        m_initValue = initValue;
    }
    public override void OnPan(Vector2 delta)
    {
        Engine.Utility.Log.Error( "on pan " + delta.ToString() );
        base.OnPan( delta );
    }

}