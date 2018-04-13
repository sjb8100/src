using System;
using System.Collections.Generic;
using UnityEngine;

public partial class RidePanel
{
    private UIGridCreatorBase m_skillCreatorBase = null;
    List<UIRideSkillGrid.SkillGridData> m_lstSkills = new List<UIRideSkillGrid.SkillGridData>();

    bool m_bInitGrid = false;
    void InitSkillContent()
    {

    }

    void InitSkillGrids()
    {   
        m_skillCreatorBase = m_trans_SkillScrollview.GetComponent<UIGridCreatorBase>();
        if (m_skillCreatorBase == null)
            m_skillCreatorBase = m_trans_SkillScrollview.gameObject.AddComponent<UIGridCreatorBase>();
        m_skillCreatorBase.gridContentOffset = new UnityEngine.Vector2(-185, -10);
        m_skillCreatorBase.arrageMent = UIGridCreatorBase.Arrangement.Horizontal;
        m_skillCreatorBase.gridWidth = 373;
        m_skillCreatorBase.gridHeight = 143;
        m_skillCreatorBase.rowcolumLimit = 2;
        m_skillCreatorBase.RefreshCheck();
        m_skillCreatorBase.Initialize<UIRideSkillGrid>(m_trans_UIRideSkillGrid.gameObject, OnRideSkillGridDataUpdate, OnRideSkillGridUIEvent);
    }

    void OnRideSkillGridDataUpdate(UIGridBase data, int index)
    {
        if (m_lstSkills != null && index < m_lstSkills.Count)
        {
            data.SetGridData(m_lstSkills[index]);
        }
    }

    void OnRideSkillGridUIEvent(UIEventType eventType, object data, object param)
    {
        switch (eventType)
        {
            case UIEventType.Click:
                UIRideSkillGrid grid = data as UIRideSkillGrid;
                if (grid != null)
                {
                    if (grid.Unlock && grid.UnLearn)
                    {
                        DataManager.Manager<UIPanelManager>().ShowPanel(PanelID.SawySkillPanel,
                            data: new SawySkillPanel.LearnSkillInfo() { rideid = grid.Data.rideData.id, skillid = grid.Data.skill });
                    }
                    else if (!grid.Unlock && grid.UnLearn)
                    {
                        TipsManager.Instance.ShowTips(LocalTextType.Ride_Skill_xuexiqianzhijienengjiesuo);
                    }
                }
                break;
        }
    }

    private CMResAsynSeedData<CMTexture> iconAtlas = null;

    private CMResAsynSeedData<CMAtlas> borderAtlas = null;

    public void InitSkillUI(RideData data)
    {
        if (data == null)
        {
            return;
        }

        if (!m_bInitGrid)
        {
            InitSkillGrids();
            m_bInitGrid = true;
        }
        m_label_Skill_RideLevel.text = data.level.ToString();
        m_label_Skill_RideName.text = data.name;
       // m_sprite_Icon.spriteName = data.icon;

        //
        if (m__Icon != null)
        {
            string iconName = UIManager.GetIconName(data.icon);
            UIManager.GetTextureAsyn(iconName
                , ref iconAtlas, () =>
                {
                    if (null != m__Icon)
                    {
                        m__Icon.mainTexture = null;
                    }
                }, m__Icon, true);


            UISprite border = m__Icon.cachedTransform.parent.Find("IconBox").GetComponent<UISprite>();
            if (border != null)
            {
                UIManager.GetAtlasAsyn(data.QualityBorderIcon
                    , ref borderAtlas, () =>
                    {
                        if (null != border)
                        {
                            border.atlas = null;
                        }
                    }, border, true);
            }
        }
        

        List<table.RideSkillData> skillList = GameTableManager.Instance.GetTableList<table.RideSkillData>();

        uint rideLevel = 0;
        int index = 0;
        for (int i = 0,imax = skillList.Count; i < imax; i++)
        {
            if (skillList[i].rideId != data.baseid)
            {
                continue;
            }

            if (skillList[i].rideLevel > rideLevel)
            {
                rideLevel = skillList[i].rideLevel;
                index = i;
            }
        }
        m_lstSkills.Clear();
        if (index != 0)
        {
            List<uint> lstSkill = skillList[index].skillArray;
            for (int i = 0; i < lstSkill.Count; i++)
            {
                UIRideSkillGrid.SkillGridData skilldata = new UIRideSkillGrid.SkillGridData()
                {
                    rideData = data,
                    skill = lstSkill[i],
                };
                m_lstSkills.Add(skilldata);
            }
        }

        if (m_skillCreatorBase != null)
        {
            m_skillCreatorBase.CreateGrids(m_lstSkills.Count);
        }

    }
}
