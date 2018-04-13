using GameCmd;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using table;
using UnityEngine;


class UIHeartSkillGrid : UIGridBase
{
    UILabel m_lblName;
    UILabel m_lblLv;
    UITexture m_spIcon;
    GameObject m_goChoose;
    GameObject m_goMask;
    GameObject m_redPoint;

    public HeartSkill m_heartSkill;

    private CMResAsynSeedData<CMTexture> iuiIconAtlas = null;

    public override void Release(bool depthRelease = true)
    {
        base.Release(depthRelease);
        if (null != iuiIconAtlas)
        {
            iuiIconAtlas.Release(true);
            iuiIconAtlas = null;
        }
    }

    protected override void OnUIBaseDestroy()
    {
        base.OnUIBaseDestroy();
        Release();
    }

    protected override void OnAwake()
    {
        base.OnAwake();
        m_lblName = this.transform.Find("name").GetComponent<UILabel>();
        m_lblLv = this.transform.Find("level").GetComponent<UILabel>();
        m_spIcon = this.transform.Find("select").GetComponent<UITexture>();
        m_goChoose = this.transform.Find("choose").gameObject;
        m_goMask = this.transform.Find("mask").gameObject;
        m_redPoint = this.transform.Find("redPoint").gameObject;
    }

    public override void SetGridData(object data)
    {
        base.SetGridData(data);
        m_heartSkill = data as HeartSkill;
        if (m_heartSkill == null)
        {
            return;
        }

        HeartSkillDataBase db = GameTableManager.Instance.GetTableItem<HeartSkillDataBase>(m_heartSkill.skill_id, (int)m_heartSkill.level);
        if (db != null)
        {
            SetIcon(db.icon);
            SetName(db.name);
            SetLv(db.lv);
            bool isLock = db.lv == 0 ? true : false;
            SetMask(isLock);
        }
    }

    public void SetIcon(string iconName)
    {
        if (m_spIcon != null)
        {
            UIManager.GetTextureAsyn(iconName, ref iuiIconAtlas, SetAtlasNull, m_spIcon, false);
        }
    }

    void SetAtlasNull()
    {
        if (null != m_spIcon)
        {
            m_spIcon.mainTexture = null;
        }
    }

    public void SetName(string name)
    {
        if (m_lblName != null)
        {
            m_lblName.text = name;
        }
    }

    public void SetLv(uint level)
    {
        if (m_lblLv != null)
        {
            m_lblLv.text = string.Format("{0}级", level);
        }
    }

    public void SetSelect(bool b)
    {
        if (m_goChoose != null && m_goChoose.activeSelf != b)
        {
            m_goChoose.SetActive(b);
        }
    }

    public void SetMask(bool b)
    {
        if (m_goMask != null && m_goMask.activeSelf != b)
        {
            m_goMask.SetActive(b);
        }
    }

    public void SetRedPoint(bool b)
    {
        if (m_redPoint != null && m_redPoint.activeSelf != b)
        {
            m_redPoint.SetActive(b);
        }
    }

}
