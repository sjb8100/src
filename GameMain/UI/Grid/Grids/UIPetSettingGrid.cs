//*************************************************************************
//	创建日期:	2016/10/28 9:54:03
//	文件名称:	UIPetSettingGrid
//   创 建 人:   zhudianyu	
//	版权所有:	中青宝
//	说    明:	宠物设置grid
//*************************************************************************

using GameCmd;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Engine.Utility;

class UIPetSettingGrid : UIGridBase
{
    /// <summary>
    /// 宠物图片
    /// </summary>
    UITexture m_sprIcon;
    /// <summary>
    /// 宠物设置状态
    /// </summary>
    Transform m_transStatus;

    public uint PetID
    {
        get
        {
            return m_petID;
        }
    }
    uint m_petID = 0;
    protected override void OnAwake()
    {
        m_sprIcon = transform.Find("icon").GetComponent<UITexture>();
        if (m_sprIcon == null)
        {
            Log.Error(" m_spricon is null");
        }

        m_transStatus = transform.Find("status_set");
        m_transStatus.gameObject.SetActive(false);
        base.OnAwake();
    }

    public void SetPetID(uint petid)
    {
        m_petID = petid;
    }
    public void SetIcon(string name)
    {
        if (m_sprIcon != null)
        {
            UIManager.GetTextureAsyn(name, ref m_curIconAsynSeed, () =>
            {
                if (null != m_sprIcon)
                {
                    m_sprIcon.mainTexture = null;
                }
            }, m_sprIcon);

        }

    }
    CMResAsynSeedData<CMTexture> m_curIconAsynSeed = null;
    public override void Release(bool depthRelease = true)
    {
        base.Release(depthRelease);
        if (m_curIconAsynSeed != null)
        {
            m_curIconAsynSeed.Release(true);
            m_curIconAsynSeed = null;
        }
    }
    public bool IsSetting()
    {
        return m_transStatus.gameObject.activeSelf;
    }
    public void SetStatus(bool hasSet)
    {
        if (m_transStatus != null)
        {
            m_transStatus.gameObject.SetActive(hasSet);
        }
    }
}

