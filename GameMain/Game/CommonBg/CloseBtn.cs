//*************************************************************************
//	创建日期:	2016/11/2 17:15:43
//	文件名称:	CloseBtn
//   创 建 人:   zhudianyu	
//	版权所有:	中青宝
//	说    明:	游戏主界面
//*************************************************************************

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

class CloseBtn:MonoBehaviour
{
    void Awake()
    {
        UIEventListener.Get( gameObject ).onClick = OnHidePanel;
      
    }
    void OnHidePanel(GameObject go)
    {
       UIPanelBase panel = transform.GetComponentInParent<UIPanelBase>();
        if(panel != null)
        {
            panel.HideSelf();
        }

    }
}

