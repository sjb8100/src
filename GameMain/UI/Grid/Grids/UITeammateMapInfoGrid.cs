
//*************************************************************************
//	创建日期:	2017/11/6 星期一 14:52:23
//	文件名称:	UITeammateMapInfoGrid
//   创 建 人:   zhudianyu	
//	版权所有:	zhudianyu
//	说    明:	
//*************************************************************************

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

class UITeammateMapInfoGrid : UIGridBase
{
    UITexture m_roleIcon = null;
    UILabel m_roleName;
    private CMResAsynSeedData<CMTexture> iuiIconAtlas = null;
    protected override void OnAwake()
    {
        base.OnAwake();
        m_roleIcon = transform.Find("Icon").GetComponent<UITexture>();
        m_roleName = transform.Find("name").GetComponent<UILabel>();
    }

    public void SetItemInfo(string texName,string roleName)
    {
        if(m_roleIcon != null)
        {
            UIManager.GetTextureAsyn(DataManager.Manager<UIManager>().GetResIDByFileName(false, texName, false), ref iuiIconAtlas, () =>
            {
                if (null != m_roleIcon)
                {
                    m_roleIcon.mainTexture = null;
                }
            }, m_roleIcon, false);
        }
        if(m_roleName != null)
        {
            m_roleName.text = roleName;
        }
      
    }
    public override void Release(bool depthRelease = true)
    {
        base.Release(depthRelease);
        if(iuiIconAtlas != null)
        {
            iuiIconAtlas.Release();
        }
    }
}
