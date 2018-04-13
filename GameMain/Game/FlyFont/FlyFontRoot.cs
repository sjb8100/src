
//*************************************************************************
//	创建日期:	2017/10/12 星期四 10:19:34
//	文件名称:	FlyFontRoot
//   创 建 人:   zhudianyu	
//	版权所有:	zhudianyu
//	说    明:	
//*************************************************************************

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

class FlyFontRoot:MonoBehaviour
{
    FlyFont m_ft = null;
    void Awake()
    {

    }
    public void InitFlyFont(FlyFont ft)
    {
        m_ft = ft;
    }
    public FlyFont GetFlyFont()
    {
        return m_ft;
    }
}
