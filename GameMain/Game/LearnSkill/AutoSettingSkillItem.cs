
//*************************************************************************
//	创建日期:	2017/9/28 星期四 11:23:15
//	文件名称:	AutoSettingSkillItem
//   创 建 人:   zhudianyu	
//	版权所有:	zhudianyu
//	说    明:	
//*************************************************************************

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using table;
class AutoSettingSkillItem : MonoBehaviour
{
    Transform m_transLock;
    Transform m_transLabel;
    Transform m_transSkillIcon;
    Transform m_transChoose;
    SkillDatabase m_db;
    uint m_skillID = 0;
    bool m_bInitControl = false;

    LearnSkillDataManager m_skillData
    {
        get
        {
            return DataManager.Manager<LearnSkillDataManager>();
        }
    }
    void Awake()
    {
        InitConrol();
    }
    void InitConrol()
    {
        if (!m_bInitControl)
        {
            m_transLock = transform.Find("lock");
            m_transLabel = transform.Find("name");
            m_transSkillIcon = transform.Find("skilIcon");
            m_transChoose = transform.Find("choosebox");
            UIEventListener.Get(m_transChoose.gameObject).onClick = OnChooseClick;
            m_bInitControl = true;
        }

    }
    void OnEnable()
    {
        m_skillData.ValueUpdateEvent += m_skillData_ValueUpdateEvent;
    }

    void m_skillData_ValueUpdateEvent(object sender, ValueUpdateEventArgs e)
    {
      if(e != null)
      {
          if(e.key ==  LearnSkillDispatchEvents.SkillAutoFightSet.ToString())
          {
              bool bSet = m_skillData.IsSkillAutoSet(m_skillID);
              SetChoose(bSet);
          }
          else if(e.key == LearnSkillDispatchEvents.SkillLevelUP.ToString())
          {
              bool bLock = m_skillData.IsSkillUnLock(m_skillID);
              ShowLock(!bLock);
          }
      }
    }
    void OnDisable()
    {
        m_skillData.ValueUpdateEvent -= m_skillData_ValueUpdateEvent;
    }
    public void InitSkillInfo(SkillDatabase db)
    {
        InitConrol();
        m_db = db;
        if (db == null)
        {
            return;
        }
        m_skillID = db.wdID;
        SetName(db.strName);
        SetIcon(db.iconPath);
        bool bSet = m_skillData.IsSkillAutoSet(m_skillID);
        SetChoose(bSet);
        bool bLock = m_skillData.IsSkillUnLock(m_skillID);
        ShowLock(!bLock);
    }
    void ShowLock(bool bShow)
    {
        if (m_transLock != null)
        {
            m_transLock.gameObject.SetActive(bShow);
        }
    }
    void SetName(string skillName)
    {
        if (m_transLabel != null)
        {
            UILabel la = m_transLabel.GetComponent<UILabel>();
            if (la != null)
            {
                la.text = skillName;
            }
        }
    }
    void SetIcon(string icon)
    {
        if (m_transSkillIcon != null)
        {
            UITexture spr = m_transSkillIcon.GetComponent<UITexture>();
            if (spr != null)
            {
                UIManager.GetTextureAsyn(DataManager.Manager<UIManager>().GetResIDByFileName(false, icon, false), ref iuiIconAtlas, () =>
                {
                    if (null != spr)
                    {
                        spr.mainTexture = null;
                    }
                }, spr, false);
            }
        }
    }
    private CMResAsynSeedData<CMTexture> iuiIconAtlas = null;
    public void ReleaseAutoSetting()
    {
        if(iuiIconAtlas != null)
        {
            iuiIconAtlas.Release();
            iuiIconAtlas = null;
        }
    }
    void SetChoose(bool bChoose)
    {
        if (m_transChoose != null)
        {
            Transform mark = m_transChoose.Find("choosemark");
            if (mark != null)
            {
                mark.gameObject.SetActive(bChoose);
            }
        }
    }
    void OnChooseClick(GameObject go)
    {
        bool bLock = m_skillData.IsSkillUnLock(m_skillID);
        if(!bLock)
        {
            return;
        }
        if(m_skillID == 0)
        {
            return;
        }
        bool bSet = m_skillData.IsSkillAutoSet(m_skillID);
        SetChoose(!bSet);
        m_skillData.SetAutoSkill(m_skillID, !bSet);
    }
}
