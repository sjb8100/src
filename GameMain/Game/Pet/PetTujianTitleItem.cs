
//*************************************************************************
//	创建日期:	2017/5/5 星期五 14:55:01
//	文件名称:	PetTujianTitleItem
//   创 建 人:   zhudianyu	
//	版权所有:	zhudianyu
//	说    明:	
//*************************************************************************

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Engine.Utility;
class PetTujianTitleItem:UIGridBase
{
    protected override void OnAwake()
    {
        base.OnAwake();
    }
    
    public override void SetHightLight(bool hightLight)
    {
        base.SetHightLight(hightLight);
        Transform high = transform.Find("Sprite");
        if(high != null)
        {
            high.gameObject.SetActive(hightLight);
        }
    }
    public void SetText(string str)
    {
        Transform labelTrans = transform.Find("Label");
        if (labelTrans != null)
        {
   
            UILabel label = labelTrans.GetComponent<UILabel>();
            if (label != null)
            {
                label.text = str;
            }
        }
    }
}
