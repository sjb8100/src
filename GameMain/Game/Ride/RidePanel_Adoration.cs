using System;
using System.Collections.Generic;
using UnityEngine;

partial class RidePanel : UIPanelBase
{
    public enum TransExpType
    {
        None,
        Normal = 1,
        Perfect = 2,
    }
    RideData m_LeftRideData = null;
    RideData m_RightRideData = null;

    void OnTransExp(string strBtnName)
    {
        if (strBtnName.Equals("wmchuancheng"))
        {
            RefreshCostUI(TransExpType.Perfect);
        }
        else if (strBtnName.Equals("ptchuancheng"))
        {
            RefreshCostUI(TransExpType.Normal);
        }
    }

    void RefreshCostUI(TransExpType ttype)
    {
        if (ttype != TransExpType.None)
        {
            int newLevel = 0;
            if (m_LeftRideData != null)
            {
                if (m_RightRideData != null)
                {
                    newLevel = CaculateLevel(m_LeftRideData, m_RightRideData, ttype);
                }
                
                m_label_PTxiaohao.text = GetCost(m_LeftRideData, TransExpType.Normal).ToString();
                m_label_WMxiaohao.text = GetCost(m_LeftRideData, TransExpType.Perfect).ToString();
            }
            else
            {
                GetOldRideUIGo().SetActive(false);
                m_label_PTxiaohao.text = "0";
                m_label_WMxiaohao.text = "0";
            }

            if (m_RightRideData != null)
            {
                SetRightNewLevel();
            }
            else if (m_RightRideData == null)
            {
                GetNewRideUIGo().SetActive(false);
            }
        }
    }

    public void InitAdoration()
    {
        ResetUIRideGridAdoration();

        ResetAdoration();
        m_label_transExpNormal.text = string.Format(LangTalkData.GetTextById(60001), GameTableManager.Instance.GetGlobalConfig<int>("PassOnRateRide", ((int)TransExpType.Normal * 10).ToString()));
        m_label_transExpPerfect.text = string.Format(LangTalkData.GetTextById(60001), GameTableManager.Instance.GetGlobalConfig<int>("PassOnRateRide", ((int)TransExpType.Perfect * 10).ToString()));
    }

    private void ResetUIRideGridAdoration()
    {
        if (m_UIGridCreatorBase != null)
        {
            List<UIRideGrid> lstGrids = m_UIGridCreatorBase.GetGrids<UIRideGrid>(true);
            for (int i = 0; i < lstGrids.Count; i++)
            {
                lstGrids[i].TransExpSelect = -1;
                lstGrids[i].SetAdirationState(false);
                lstGrids[i].SetSufferingState(false);
            }
        }
    }
    void ResetAdoration()
    {
        m_RightRideData = null;
        m_LeftRideData = null;
        GetNewRideUIGo().SetActive(false);
        GetOldRideUIGo().SetActive(false);
        m_widget_new_select.gameObject.SetActive(false);
        m_widget_old_select.gameObject.SetActive(true);
    }
    private CMResAsynSeedData<CMTexture> iconOldAtlas = null;
    private CMResAsynSeedData<CMTexture> iconNewdAtlas = null;

    private CMResAsynSeedData<CMAtlas> borderOldAtlas = null;
    private CMResAsynSeedData<CMAtlas> borderNewAtlas = null;
    public int OnSelectRide(UIRideGrid grid)
    {
        RideData rideData = grid.RideData;
         if (m_RightRideData != null && grid.TransExpSelect == 2)
        {
            OnUnselectRight(); return 0;
        }else if (m_LeftRideData != null && grid.TransExpSelect == 1)
        {
            OnUnselectLeft(); return 0;
        }
        else if (m_LeftRideData == null)
        {
            if (rideData.id == DataManager.Manager<RideManager>().Auto_Ride)
            {
                TipsManager.Instance.ShowTips(LocalTextType.Ride_Inherit_chuzhanzhongdezuoqiwufachuancheng);
                return 0;
            }

            if (m_RightRideData != null && m_RightRideData.id == rideData.id)
            {
                TipsManager.Instance.ShowTips(LocalTextType.Ride_Inherit_bunengxuanzetongyigezuoqijinxingchuancheng);

                return 0;
            }
            m_LeftRideData = rideData;
            GetOldRideUIGo().SetActive(true);
            
            m_widget_old_select.gameObject.SetActive(false);
            m_widget_new_select.gameObject.SetActive(m_RightRideData == null);
            m_label_Old_level_Before.text = rideData.level.ToString();
            m_label_Old_level_After.text = "0";
            m_label_Old_name.text = rideData.name;
            m_label_Old_speed_Before.text = rideData.GetSpeed().ToString() + "%";
            m_label_Old_speed_After.text = RideData.GetSpeedById_Level(rideData.baseid,0) + "%";

            if (m__Old_icon != null)
            {
                UIManager.GetTextureAsyn(rideData.icon
                    , ref iconOldAtlas, () =>
                    {
                        if (null != m__Old_icon)
                        {
                            m__Icon.mainTexture = null;
                        }
                    }, m__Old_icon
                    , true);


                UISprite border = m__Old_icon.cachedTransform.parent.Find("IconBox").GetComponent<UISprite>();
                if (border != null)
                {
                    UIManager.GetAtlasAsyn(rideData.QualityBorderIcon
                        , ref borderOldAtlas
                        ,() =>
                        {
                            if (null != border)
                            {
                                border.atlas = null;
                            }
                        }
                    , border
                    , true);
                }
            }


            TransExpType ttype = TransExpType.None;
            if (m_toggle_ptchuancheng.value)
            {
                ttype = TransExpType.Normal;
            }
            else if (m_toggle_wmchuancheng.value)
            {
                ttype = TransExpType.Perfect;
            }

            RefreshCostUI(ttype);

            return 1;
        }
        else if(m_RightRideData == null)
        {
            if (m_LeftRideData != null && m_LeftRideData.id == rideData.id)
            {
                TipsManager.Instance.ShowTips(LocalTextType.Ride_Inherit_bunengxuanzetongyigezuoqijinxingchuancheng);
                return 0;
            }
            m_RightRideData = rideData;
            GetNewRideUIGo().SetActive(true);
            m_widget_new_select.gameObject.SetActive(false);
            m_label_New_level_Before.text = m_RightRideData.level.ToString();
            m_label_New_name.text = rideData.name;
            m_label_New_speed_Before.text = m_RightRideData.GetSpeed().ToString() + "%";

            if (m__New_icon != null)
            {
                UIManager.GetTextureAsyn(rideData.icon
                    , ref iconNewdAtlas, () =>
                    {
                        if (null != m__New_icon)
                        {
                            m__New_icon.mainTexture = null;
                        }
                    }, m__New_icon, true);


                UISprite border = m__New_icon.cachedTransform.parent.Find("IconBox").GetComponent<UISprite>();
                if (border != null)
                {
                    UIManager.GetAtlasAsyn(rideData.QualityBorderIcon
                        , ref borderNewAtlas, () =>
                        {
                            if (null != border)
                            {
                                border.atlas = null;
                            }
                        }, border, true);
                }
            }
            SetRightNewLevel();

            return 2;
        }
         return 0;
    }

    void SetRightNewLevel()
    {
        if (m_LeftRideData != null && m_RightRideData != null)
        {
            TransExpType ttype = TransExpType.None;
            if (m_toggle_ptchuancheng.value)
            {
                ttype = TransExpType.Normal;
            }
            else if (m_toggle_wmchuancheng.value)
            {
                ttype = TransExpType.Perfect;
            }
            
            int level = CaculateLevel(m_LeftRideData, m_RightRideData, ttype);
            m_label_New_level_After.text = level.ToString();         
            m_label_New_level_After.color = level == m_RightRideData.level ? Color.white : Color.green;
            m_label_New_speed_After.text = RideData.GetSpeedById_Level(m_RightRideData.baseid, level).ToString() + "%";
            m_label_New_speed_After.color = level == m_RightRideData.level ? Color.white : Color.green;
        }
        else
        {
            m_label_New_level_After.text = "0";
            m_label_New_speed_After.text = "0";
        }
    }

    public void OnUnselectLeft()
    {
        m_LeftRideData = null;
        GetOldRideUIGo().SetActive(false);
        m_widget_old_select.gameObject.SetActive(true);
        m_widget_new_select.gameObject.SetActive(false);

        if (m_UIGridCreatorBase != null)
        {
            List<UIRideGrid> lstGrids = m_UIGridCreatorBase.GetGrids<UIRideGrid>(true);
            for (int i = 0; i < lstGrids.Count; i++)
            {
                if (lstGrids[i].TransExpSelect == 1)
                {
                    lstGrids[i].TransExpSelect = -1;
                    lstGrids[i].SetSufferingState(false);
                }
            }
        }
        m_label_WMxiaohao.text = "0";
        m_label_PTxiaohao.text = "0";

        SetRightNewLevel();
    }

    public void OnUnselectRight()
    {
        m_RightRideData = null;
        GetNewRideUIGo().SetActive(false);
        m_widget_new_select.gameObject.SetActive(m_LeftRideData != null);

        if (m_UIGridCreatorBase != null)
        {
            List<UIRideGrid> lstGrids = m_UIGridCreatorBase.GetGrids<UIRideGrid>(true);
            for (int i = 0; i < lstGrids.Count; i++)
            {
                if (lstGrids[i].TransExpSelect == 2)
                {
                    lstGrids[i].TransExpSelect = -1;

                    lstGrids[i].SetAdirationState(false);
                }
            }
        }
        SetRightNewLevel();
    }

    int CaculateLevel(RideData left, RideData right, TransExpType type)
    {
        int rate = GameTableManager.Instance.GetGlobalConfig<int>("PassOnRateRide", ((int)type * 10).ToString());

        int level = right.level;

        int leftRideExp = GetRideAllExp(left);
        if (leftRideExp <= 0)
        {
            return level;
        }
        int leftExp = Mathf.CeilToInt(leftRideExp * rate * 0.01f) + right.exp;
        
        table.RideFeedData feeddata = GameTableManager.Instance.GetTableItem<table.RideFeedData>(right.baseid, level);
        while (feeddata != null)
        {
            leftExp -= (int)feeddata.upExp;
            if (leftExp >= 0)
            {
                level++;
                if (leftExp > 0)
                {
                    feeddata = GameTableManager.Instance.GetTableItem<table.RideFeedData>(right.baseid, level);
                    if (feeddata == null)
                    {
                        level--;
                    }
                }
                else
                {
                    feeddata = null;
                }
            }
            else
            {
                feeddata = null;
            }
        }
        return level;
    }

    int GetCost(RideData leftRideData, TransExpType type)
    {
        int cost = 0;
        int rate = GameTableManager.Instance.GetGlobalConfig<int>("PassOnCostRide", ((int)type * 10).ToString());
        int exp = GetRideAllExp(leftRideData);
        cost = Mathf.CeilToInt(exp * 1f / rate);
        return cost; 
    }

    int GetRideAllExp(RideData ridedata)
    {
        int exp = ridedata.exp;
        int level = ridedata.level - 1;
        while (level >= 0)
        {
            table.RideFeedData feeddata = GameTableManager.Instance.GetTableItem<table.RideFeedData>(ridedata.baseid, level);
            if (feeddata != null)
            {
                exp += (int)feeddata.upExp;
            }
            level--;
        }
        return exp;
    }
    void onClick_Btn_Old_delete_Btn(GameObject caster)
    {
        OnUnselectLeft();
    }

    void onClick_Btn_New_delete_Btn(GameObject caster)
    {
        OnUnselectRight();
    }

    void onClick_Btn_Adoration_Btn(GameObject caster)
    {
    //    if (m_LeftRideData == null)
    //    {
    //        TipsManager.Instance.ShowTips(LocalTextType.Ride_Inherit_qingxuanzejinxingchuanchengdezuoqi);
    //        return;
    //    }
    //    if (m_LeftRideData == null)
    //    {
    //        TipsManager.Instance.ShowTips(LocalTextType.Ride_Inherit_qingxuanzejinxingchuanchengdezuoqi);
    //        return;
    //    }
    //    if (m_RightRideData == null)
    //    {
    //        TipsManager.Instance.ShowTips(LocalTextType.Ride_Inherit_qingxuanzejichengdezuoqi);

    //        return;
    //    }
    //    if (m_LeftRideData != null && m_RightRideData != null)
    //    {
    //        if (m_LeftRideData.id == DataManager.Manager<RideManager>().UsingRide)
    //        {
    //          //  TipsManager.Instance.ShowTipsById();
    //        }

    //        if (m_toggle_ptchuancheng.value)
    //        {
    //            DataManager.Instance.Sender.RideExpPass(m_LeftRideData.id, m_RightRideData.id, GameCmd.PassType.NORMAL_PASS);
    //        }else if (m_toggle_wmchuancheng.value)
    //        {
    //            DataManager.Instance.Sender.RideExpPass(m_LeftRideData.id, m_RightRideData.id, GameCmd.PassType.PERFECT_PASS);
    //        }
    //    }
    }
}
