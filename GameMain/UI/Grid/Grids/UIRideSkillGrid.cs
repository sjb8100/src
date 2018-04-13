using System;
using System.Collections.Generic;
using UnityEngine;

class UIRideSkillGrid :UIGridBase
{
    public class SkillGridData
    {
        public RideData rideData = null;
        public uint skill = 0;
    }

    UITexture m_spriteIcon = null;
    UILabel m_labelName = null;
    UILabel m_labelCost = null;
    UILabel m_labelDes = null;
    GameObject m_goLock = null;
    GameObject m_goUnLearn = null;
    UILabel m_lableUnlockLevel = null;

    SkillGridData m_SkillGridData = null;
    public SkillGridData Data { get { return m_SkillGridData; } }

    public bool UnLearn { get; set; }
    public bool Unlock { get; set; }
    private CMResAsynSeedData<CMTexture> iuiBorderAtlas = null;

    public override void Release(bool depthRelease = true)
    {
        base.Release(depthRelease);
        if (null != iuiBorderAtlas)
        {
            iuiBorderAtlas.Release(true);
            iuiBorderAtlas = null;
        }

    }
    void Awake()
    {
        m_spriteIcon = transform.Find("icon").GetComponent<UITexture>();
        m_labelName = transform.Find("name").GetComponent<UILabel>();
        m_labelCost = transform.Find("SkillXiaohaoNum").GetComponent<UILabel>();
        m_labelDes = transform.Find("SkillDes").GetComponent<UILabel>();

        Transform t = transform.Find("unlock");
        if (t != null)
        {
            m_goLock = t.gameObject;
        }
        
        t = transform.Find("unlearn");
        if (t != null)
        {
            m_goUnLearn = t.gameObject;
        }

        if (m_goLock != null)
        {
            m_lableUnlockLevel = m_goLock.transform.Find("Label").GetComponent<UILabel>();
        }
    }

    public override void SetGridData(object data)
    {
        base.SetGridData(data);

        if (data is SkillGridData)
        {
            m_SkillGridData = (SkillGridData)data;

            SetTopUI();

            SetSkillState();
        }
    }

    private void SetSkillState()
    {
        if (m_SkillGridData.rideData.skill_ids.Contains((int)m_SkillGridData.skill))
        {
            Unlock = true;
            UnLearn = false;
            m_goLock.SetActive(false);
            m_spriteIcon.color = new Color(m_spriteIcon.color.r, m_spriteIcon.color.g, m_spriteIcon.color.b, 1f);
            m_goUnLearn.SetActive(false);
            return;
        }


        table.RideSkillData currskilldata = GameTableManager.Instance.GetTableItem<table.RideSkillData>(m_SkillGridData.rideData.baseid, m_SkillGridData.rideData.level);
        if (currskilldata != null)
        {
            bool bcanOpen = currskilldata.skillArray.Contains(m_SkillGridData.skill);
            if (bcanOpen)
            {
                m_spriteIcon.color = new Color(m_spriteIcon.color.r, m_spriteIcon.color.g, m_spriteIcon.color.b, 1f);
               
                UnLearn = true;
                m_goLock.SetActive(false);
                m_goUnLearn.SetActive(true);

                int preSkillId = 0;
                foreach (var item in currskilldata.skillArray)
                {
                    if (item == m_SkillGridData.skill)
                    {
                        if (preSkillId != 0 && !m_SkillGridData.rideData.skill_ids.Contains(preSkillId))
                        {
                            Unlock = false;
                            m_goUnLearn.GetComponentInChildren<UILabel>().text = DataManager.Manager<TextManager>().GetLocalText(LocalTextType.Ride_Skill_weijiesuo);
                            return;
                        }
                    }
                    else
                    {
                        preSkillId = (int)item;
                    }
                }
                Unlock = true;
                m_goUnLearn.GetComponentInChildren<UILabel>().text = DataManager.Manager<TextManager>().GetLocalText(LocalTextType.Ride_Skill_weilingwu); ;
                return;
            }

            int m_openSkillLevel = GetUnlockLevel();
            UnLearn = false;
            Unlock = false;
            m_goLock.SetActive(true);
            m_spriteIcon.color = new Color(m_spriteIcon.color.r, m_spriteIcon.color.g, m_spriteIcon.color.b, 51 / 255.0f);
            m_goUnLearn.SetActive(false);
            m_goLock.GetComponentInChildren<UILabel>().text = DataManager.Manager<TextManager>().GetLocalFormatText(LocalTextType.Ride_Skill_dengjijiesuo, m_openSkillLevel);
        }
        else
        {
            UnLearn = false;
            Unlock = false;
            m_goLock.SetActive(true);
            m_spriteIcon.color = new Color(m_spriteIcon.color.r, m_spriteIcon.color.g, m_spriteIcon.color.b, 51 / 255.0f);

            m_goUnLearn.SetActive(false);
            if (m_lableUnlockLevel != null)
            {
                int currLevel = m_SkillGridData.rideData.level;
                if (currLevel == 0)
                {
                    currLevel++;
                    currskilldata = GameTableManager.Instance.GetTableItem<table.RideSkillData>(m_SkillGridData.rideData.baseid, currLevel);
                }
                while (currskilldata != null)
                {
                    if (currskilldata.skillArray.Contains(m_SkillGridData.skill))
                    {
                        m_lableUnlockLevel.text = DataManager.Manager<TextManager>().GetLocalFormatText(LocalTextType.Ride_Skill_dengjijiesuo, currLevel); 
                        break;
                    }
                    currLevel++;
                    currskilldata = GameTableManager.Instance.GetTableItem<table.RideSkillData>(m_SkillGridData.rideData.baseid, currLevel);
                }
            }
        }
    }
    private void SetTopUI()
    {
        table.RideSkillDes skillDesc = GameTableManager.Instance.GetTableItem<table.RideSkillDes>(m_SkillGridData.skill);
        if (skillDesc != null)
        {
            UIManager.GetTextureAsyn(skillDesc.skillIcon, ref iuiBorderAtlas, () =>
            {
                if (null != m_spriteIcon)
                {
                    m_spriteIcon.mainTexture = null;
                }
            }, m_spriteIcon);
           

            if (m_labelName != null)
            {
                m_labelName.text = skillDesc.skillName;
            }

            if (m_labelCost != null)
            {
                m_labelCost.text = skillDesc.costrepletion.ToString();
            }

            if (m_labelDes != null)
            {
                m_labelDes.text = skillDesc.skillDesc;
            }
        }
    }
    int GetUnlockLevel()
    {
        int currLevel = m_SkillGridData.rideData.level;
        table.RideSkillData currskilldata = GameTableManager.Instance.GetTableItem<table.RideSkillData>(m_SkillGridData.rideData.baseid, currLevel);
        while (currskilldata != null)
        {
            if (currskilldata.skillArray.Contains(m_SkillGridData.skill))
            {
                break;
            }
            currLevel++;
            currskilldata = GameTableManager.Instance.GetTableItem<table.RideSkillData>(m_SkillGridData.rideData.baseid, currLevel);
        }
        return currLevel;
    }
}