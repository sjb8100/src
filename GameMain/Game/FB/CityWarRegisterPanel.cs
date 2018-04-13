using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;


partial class CityWarRegisterPanel
{
    #region property
    private UIGridCreatorBase m_cityWarRegisterClanGridCreator = null;

    /// <summary>
    /// 申请列表
    /// </summary>
    List<GameCmd.CityApplyInfo> m_lstApply;

    private CMResAsynSeedData<CMTexture> m_iuiBgSeed = null;

    UILabel[] m_signClanName;

    #endregion

    #region override

    protected override void OnLoading()
    {
        base.OnLoading();
        m_signClanName = m_trans_clanNameRoot.gameObject.GetComponentsInChildren<UILabel>();

        InitWidget();

        m_trans_Tips1.gameObject.SetActive(false);
        m_trans_Tips2.gameObject.SetActive(false);
        m_trans_Tips3.gameObject.SetActive(false);
        m_trans_Tips4.gameObject.SetActive(false);
    }

    protected override void OnShow(object data)
    {
        base.OnShow(data);

        uint copyId = 0;
        if (data != null)
        {
            copyId = (uint)data;
        }

        DataManager.Manager<CityWarManager>().ReqCityWarFrameInfo(copyId);
        m_trans_TipsScrollViewRoot.gameObject.SetActive(false);
    }

    protected override void OnPanelReady()
    {
        base.OnPanelReady();

        InitUI();
    }

    protected override void OnHide()
    {
        base.OnHide();


    }

    public override bool OnMsg(UIMsgID msgid, object param)
    {
        if (msgid == UIMsgID.eCityWarFrameInfo)
        {
            InitUI();
        }
        else if (msgid == UIMsgID.eCityWarSubmitStatue)
        {
            InitUI();
        }

        return true;
    }

    protected override void OnPanelBaseDestory()
    {
        base.OnPanelBaseDestory();
    }

    public override void Release(bool depthRelease = true)
    {
        base.Release(depthRelease);

        if (m_cityWarRegisterClanGridCreator != null)
        {
            m_cityWarRegisterClanGridCreator.Release(depthRelease);
        }

        if (m_iuiBgSeed != null)
        {
            m_iuiBgSeed.Release(depthRelease);
        }

    }

    #endregion


    #region method

    void InitWidget()
    {
        //报名信息
        m_cityWarRegisterClanGridCreator = m_trans_CityWarRegisterScrollView.GetComponent<UIGridCreatorBase>();
        if (m_cityWarRegisterClanGridCreator == null)
        {
            m_cityWarRegisterClanGridCreator = m_trans_CityWarRegisterScrollView.gameObject.AddComponent<UIGridCreatorBase>();
        }

        if (m_cityWarRegisterClanGridCreator != null)
        {
            m_cityWarRegisterClanGridCreator.gridContentOffset = new UnityEngine.Vector2(0, 0);
            m_cityWarRegisterClanGridCreator.arrageMent = UIGridCreatorBase.Arrangement.Vertical;
            m_cityWarRegisterClanGridCreator.gridWidth = 700;
            m_cityWarRegisterClanGridCreator.gridHeight = 45;

            m_cityWarRegisterClanGridCreator.RefreshCheck();
            m_cityWarRegisterClanGridCreator.Initialize<UICityWarRegisterClanGrid>(m_widget_CityWarRegisterClan.gameObject, OnGridDataUpdate, OnGridUIEvent);
        }
    }

    private void OnGridDataUpdate(UIGridBase data, int index)
    {
        if (data is UICityWarRegisterClanGrid)
        {
            if (m_lstApply != null && m_lstApply.Count > index)
            {
                UICityWarRegisterClanGrid grid = data as UICityWarRegisterClanGrid;
                if (grid == null)
                {
                    return;
                }

                grid.SetRank((uint)index + 1);
                grid.SetClanName(m_lstApply[index].clan_name);
                grid.SetClanLeader(m_lstApply[index].clan_leader);
                grid.SetNum(m_lstApply[index].nums);
            }
        }
    }

    private void OnGridUIEvent(UIEventType eventType, object data, object param)
    {
        if (eventType == UIEventType.Click)
        {
            //UICityWarCityInfoGrid grid = data as UICityWarCityInfoGrid;
            //if (grid == null)
            //{
            //    return;
            //}
        }
    }


    void InitUI()
    {
        table.CityWarDataBase cityWarDb = GameTableManager.Instance.GetTableItem<table.CityWarDataBase>(DataManager.Manager<CityWarManager>().CityWarSelectCopyId);
        if (cityWarDb == null)
        {
            return;
        }

        //titleName
        uint huaxiaCopyId =  GameTableManager.Instance.GetGlobalConfig<uint>("CityWarCopyID");
        string titleName = string.Empty;
        if (cityWarDb.CopyId == huaxiaCopyId)
        {
            titleName = "华夏争霸战";
            m_label_Registertime_label.gameObject.SetActive(false);
        }
        else
        {
            titleName = "城池争夺战";
            m_label_Registertime_label.gameObject.SetActive(true);
        }
        m_label_name.text = titleName;

        //name
        m_label_CityWar_name.text = cityWarDb.Name;

        //bg
        if (m__bgtexture != null)
        {
            UIManager.GetTextureAsyn(cityWarDb.Bg, ref m_iuiBgSeed, () => { if (m__bgtexture != null) { m__bgtexture.mainTexture = null; } }, m__bgtexture, false);
        }

        //isOpen
        m_sprite_OpenLimit.gameObject.SetActive(!DataManager.Manager<CityWarManager>().IsOpen);

        //time
        m_label_Starttime.text = cityWarDb.OpenTime;
        m_label_Registertime.text = cityWarDb.RegisterTime;

        //CityLeader 城主
        m_label_Playerdes.text = DataManager.Manager<CityWarManager>().CityWarRegisterClanLeaderName;
        m_label_ClanNamedes.text = DataManager.Manager<CityWarManager>().CityWarRegisterClanName;

        //signName
        for (int i = 0; i < m_signClanName.Length; i++)
        {
            List<string> list = DataManager.Manager<CityWarManager>().CityWarSignList;
            if (i < m_trans_ClanRoot.childCount)
            {
                Transform transf = m_trans_ClanRoot.GetChild(i);
                if (transf == null)
                {
                    return;
                }

                if (list != null && i < list.Count)
                {
                    m_signClanName[i].text = list[i];
                    transf.gameObject.SetActive(true);
                }
                else
                {
                    m_signClanName[i].text = "欢迎报名";
                    transf.gameObject.SetActive(false);
                }
            }
        }

        m_sprite_cityWarDefend.gameObject.SetActive(DataManager.Manager<CityWarManager>().HaveDefender);

        // applyList
        m_label_ApplyTitle.text = string.Format("申请{0}排名", cityWarDb.Name);
        m_lstApply = DataManager.Manager<CityWarManager>().CityWarApplyInfoList;

        if (m_cityWarRegisterClanGridCreator != null)
        {
            m_cityWarRegisterClanGridCreator.CreateGrids(m_lstApply != null ? m_lstApply.Count : 0);
        }
    }


    #endregion

    #region click
    void onClick_Close_Btn(GameObject caster)
    {
        HideSelf();
    }

    void onClick_Btn_left_Btn(GameObject caster)
    {
        DataManager.Manager<CityWarManager>().ReqOpenSubmitPanel();

        //uint itemBaseId = GameTableManager.Instance.GetGlobalConfig<uint>("CityWarStatue");
        //DataManager.Manager<UIPanelManager>().ShowPanel(PanelID.CityWarSubmitPanel, data: itemBaseId);
    }

    void onClick_Btn_right_Btn(GameObject caster)
    {
        GameCmd.stCityWarBeginClanUserCmd_CS agreeCmd = new GameCmd.stCityWarBeginClanUserCmd_CS();
        NetService.Instance.Send(agreeCmd);
    }

    void onClick_Btn_tips_Btn(GameObject caster)
    {
        m_trans_TipsScrollViewRoot.gameObject.SetActive(true);

        uint copyId = DataManager.Manager<CityWarManager>().CityWarSelectCopyId;
        switch (copyId)
        {
            case 4004:
                m_trans_Tips1.gameObject.SetActive(true);
                m_trans_Tips2.gameObject.SetActive(false);
                m_trans_Tips3.gameObject.SetActive(false);
                m_trans_Tips4.gameObject.SetActive(false);
                break;
            case 4005:
                 m_trans_Tips1.gameObject.SetActive(false);
                m_trans_Tips2.gameObject.SetActive(true);
                m_trans_Tips3.gameObject.SetActive(false);
                m_trans_Tips4.gameObject.SetActive(false);
                break;
            case 4006:
                m_trans_Tips1.gameObject.SetActive(false);
                m_trans_Tips2.gameObject.SetActive(false);
                m_trans_Tips3.gameObject.SetActive(true);
                m_trans_Tips4.gameObject.SetActive(false);
                break;
            case 4002:
                m_trans_Tips1.gameObject.SetActive(false);
                m_trans_Tips2.gameObject.SetActive(false);
                m_trans_Tips3.gameObject.SetActive(false);
                m_trans_Tips4.gameObject.SetActive(true);
                break;
        }

    }

    void onClick_TipsContentClose_Btn(GameObject caster)
    {
        if (m_trans_TipsScrollViewRoot.gameObject.activeSelf)
        {
            m_trans_TipsScrollViewRoot.gameObject.SetActive(false);
        }
    }


    #endregion
}

