//*************************************************************************
//	创建日期:	2017-4-5 14:03
//	文件名称:	UISkillGrid.cs
//  创 建 人:   	Chengxue.Zhao
//	版权所有:	中青宝
//	说    明:	查看别人信息 技能icon
//*************************************************************************
using System;
using System.Collections.Generic;
using UnityEngine;

public class UISkillGridData
{
    public int skillid;
    public int skillType; //1 宠物 2 坐骑
    public int level;
} 

public class UISkillGrid : UIGridBase

{
    
    UITexture m_spriteIcon = null;
    UILabel m_labelName = null;
    UISkillGridData m_data = null;
    public UISkillGridData Data
    {
        get
        {
            return m_data;
        }
    }
   protected override  void OnAwake()
    {
        base.OnAwake();
        m_labelName = CacheTransform.Find("Name").GetComponent<UILabel>();
        m_spriteIcon = CacheTransform.Find("icon").GetComponent<UITexture>();
    }

    public override void SetGridData(object data)
    {
        base.SetGridData(data);
        if (data is UISkillGridData)
        {
            m_data = (UISkillGridData)data;
            OnShowUI();
        }
    }
    CMResAsynSeedData<CMTexture> m_playerAvataCASD = null;
    public override void Release(bool depthRelease = true)
    {
        base.Release(depthRelease);
        if (m_playerAvataCASD != null)
        {
            m_playerAvataCASD.Release(depthRelease);
            m_playerAvataCASD = null;
        }
    }
    public void ClearData() 
    {
//         m_labelName.gameObject.SetActive(true);
//         m_labelName.text = "技能";
        m_spriteIcon.gameObject.SetActive(false);
    }
    void OnShowUI()
    {
        if (m_data == null)
        {
            return;
        }
        m_labelName.gameObject.SetActive(false);
        m_spriteIcon.gameObject.SetActive(true);

        if (m_data.skillType == 1)
        {
            table.SkillDatabase skilldata = GameTableManager.Instance.GetTableItem<table.SkillDatabase>((uint)m_data.skillid);
            if (skilldata != null)
            {
                UIManager.GetTextureAsyn(skilldata.iconPath, ref m_playerAvataCASD, () =>
                {
                    if (null != m_spriteIcon)
                    {
                        m_spriteIcon.mainTexture = null;
                    }
                }, m_spriteIcon);
           
            }

        }else if (m_data.skillType == 2)
        {
            table.RideSkillDes rideSkill = GameTableManager.Instance.GetTableItem<table.RideSkillDes>((uint)m_data.skillid);
            if (rideSkill != null)
            {
                UIManager.GetTextureAsyn(rideSkill.skillIcon, ref m_playerAvataCASD, () =>
                {
                    if (null != m_spriteIcon)
                    {
                        m_spriteIcon.mainTexture = null;
                    }
                }, m_spriteIcon);
              
            }
        }
    }
}