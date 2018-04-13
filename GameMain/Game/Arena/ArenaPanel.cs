using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using GameCmd;
using System.Collections;
using Client;


partial class ArenaPanel : UIPanelBase
{

    #region property

    GameObject[] RivalCard;
    GameObject[] TopCard;
    Coroutine challengIntervalCoroutine;

    private CMResAsynSeedData<CMTexture> iuiIconAtlas = null;

    public ArenaManager AManager
    {
        get
        {
            return DataManager.Manager<ArenaManager>();
        }
    }

    public TextManager Tmger
    {
        get
        {
            return DataManager.Manager<TextManager>();
        }
    }

    #endregion


    #region override

    //在窗口第一次加载时，调用
    protected override void OnLoading()
    {
        RivalCard = new GameObject[3] { m_widget_player_01.gameObject, m_widget_player_02.gameObject, m_widget_player_03.gameObject };
        TopCard = new GameObject[3] { m_trans_Top_01.gameObject, m_trans_Top_02.gameObject, m_trans_Top_03.gameObject };
    }

    protected override void OnPrepareShow(object data)
    {
        base.OnPrepareShow(data);
    }

    /// <summary>
    /// 界面显示回调
    /// </summary>
    /// <param name="data"></param>
    protected override void OnShow(object data)
    {
        base.OnShow(data);

        AManager.ReqOpenArena();

        //UpdateArenaMain();
    }

    protected override void OnHide()
    {
        base.OnHide();
        ReleaseAtlas();
    }


    protected override void OnJump(UIPanelBase.PanelJumpData jumpData)
    {
        base.OnJump(jumpData);
        //if (null == jumpData)
        //{
        //    jumpData = new PanelJumpData();
        //}
        //int firstTabData = -1;
        //int secondTabData = -1;
        //if (null != jumpData.Tabs && jumpData.Tabs.Length > 0)
        //{
        //    firstTabData = jumpData.Tabs[0];

        //}
        //else
        //{
        //    firstTabData = (int)PropPanelMode.Prop;
        //}
        //UIFrameManager.Instance.OnCilckTogglePanel(this.PanelId, 1, firstTabData);
    }

    public override bool OnTogglePanel(int tabType, int pageid)
    {
        return base.OnTogglePanel(tabType, pageid);
    }

    public override UIPanelBase.PanelData GetPanelData()
    {
        return base.GetPanelData();
    }

    public override bool OnMsg(UIMsgID msgid, object param)
    {
        if (msgid == UIMsgID.eArenaMainData)
        {
            UpdateArenaMain();
        }
        else if (msgid == UIMsgID.eArenaTopThree)
        {
            //UpdateTopThree();//前三名  top3   (改为点击按钮后显示)
        }
        else if (msgid == UIMsgID.eArenaRivalThree)
        {
            UpdateRivalThree(); //挑战的3个对手
        }
        else if (msgid == UIMsgID.eArenaTimesUpdate)
        {
            ChallengeTimesUpdate();// 刷新挑战次数
        }
        else if (msgid == UIMsgID.eArenaCDUpdate)
        {
            CDUpdate();// 刷新挑战CD
        }

        return true;
    }

    #endregion


    #region method
    /// <summary>
    /// 武斗场主界面
    /// </summary>
    void UpdateArenaMain()
    {
        if (challengIntervalCoroutine != null) StopCoroutine(challengIntervalCoroutine);

        AManager.ChallengeTarget = null;//清除挑战对手

        //前三名  top3
        //UpdateTopThree();

        //挑战的3个对手
        //UpdateRivalThree();



        //排行
        m_label_MyRanking.text = AManager.Rank.ToString();

        IPlayer player = Client.ClientGlobal.Instance().MainPlayer;
        if (player != null)
        {
            //icon
            int profession = player.GetProp((int)PlayerProp.Job);
            table.SelectRoleDataBase sdb = table.SelectRoleDataBase.Where((GameCmd.enumProfession)profession, (GameCmd.enmCharSex)1);
            if (sdb != null)
            {
                UIManager.GetTextureAsyn(sdb.strprofessionIcon, ref iuiIconAtlas,
                    () =>
                    {
                        if (m__MainPlayerIcon != null) { m__MainPlayerIcon.mainTexture = null; }
                    },
                m__MainPlayerIcon,false);
            }

            //name
            m_label_MainPlayerName.text = player.GetName();

            //level
            m_label_MainPlayerLevel.text = player.GetProp((int)CreatureProp.Level).ToString();

            //战斗力
            m_label_MyFighting.text = player.GetProp((int)FightCreatureProp.Power).ToString();
        }

        //刷新挑战次数
        ChallengeTimesUpdate();

        //刷新CD
        CDUpdate();

        m_trans_gridContent.localPosition = new Vector3(0, m_trans_gridContent.localPosition.y, m_trans_gridContent.localPosition.z);
        m_btn_leftArrow.gameObject.SetActive(true);
        m_btn_rightArrow.gameObject.SetActive(false);

        //UICenterOnChild centerOnChild = m_grid_grid.GetComponent<UICenterOnChild>();
        //if (centerOnChild == null)
        //{
        //    centerOnChild = m_grid_grid.gameObject.AddComponent<UICenterOnChild>();
        //}
        //centerOnChild.onCenter = OnCenterOnChildCallback; //控制左右箭头
    }

    /// <summary>
    /// 前三名  top3
    /// </summary>
    void UpdateTopThree()
    {
        List<TopUserData> topList = AManager.TopUserList;
        for (int i = 0; i < topList.Count; i++)
        {
            ArenaTopCardGrid topGrid = TopCard[i].GetComponent<ArenaTopCardGrid>();
            if (topGrid == null)
            {
                topGrid = TopCard[i].AddComponent<ArenaTopCardGrid>();
            }

            topGrid.SetGridData(topList[i]);
            topGrid.SetModel(topList[i].suit_data, (int)topList[i].job, topList[i].wdface); //设置模型
        }
    }

    /// <summary>
    /// 可挑战的三个对手
    /// </summary>
    void UpdateRivalThree()
    {
        List<OppuserData> rivalList = AManager.RivalList;

        for (int i = 0; i < rivalList.Count; i++)
        {
            ArenaRivalCardGrid rivalGrid = RivalCard[i].GetComponent<ArenaRivalCardGrid>();
            if (rivalGrid == null)
                rivalGrid = RivalCard[i].AddComponent<ArenaRivalCardGrid>();
            rivalGrid.SetGridData(rivalList[i]);
            rivalGrid.SetModel(rivalList[i].suit_data, (int)rivalList[i].job, rivalList[i].face);//设置模型及外观

            rivalGrid.RegisterUIEventDelegate(OnRivalGridUIEvent);
        }
    }

    /// <summary>
    /// 点击事件
    /// </summary>
    /// <param name="eventType"></param>
    /// <param name="data"></param>
    /// <param name="param"></param>
    private void OnRivalGridUIEvent(UIEventType eventType, object data, object param)
    {
        if (eventType == UIEventType.Click)
        {
            ArenaRivalCardGrid grid = data as ArenaRivalCardGrid;
            if (grid != null)
            {
                AManager.ChallengeTarget = grid.rivalInfo;

                if (AManager.ChallengeTimes == 0)
                {
                    CheckAndReqRefreshChallengeTimes();
                    return;
                }

                if (AManager.CD > 0)
                {
                    CheckAndReqClearCD();
                    return;
                }

                //发起挑战
                DataManager.Manager<ArenaManager>().ReqChallengeInvite(grid.rivalInfo.id, grid.rivalInfo.name, grid.rivalInfo.rank, grid.rivalInfo.online_state);
            }
        }
    }

    /// <summary>
    /// 刷新挑战CD
    /// </summary>
    void CDUpdate()
    {
        if (AManager.CD > 0 && AManager.ChallengeTimes != 0)
        {
            m_btn_NowChalleng.gameObject.SetActive(true);
            m_label_NowChalleng_price.gameObject.SetActive(true);
            m_label_Challenge_Time.gameObject.SetActive(true);
            m_label_NowChalleng_price.text = string.Format("{0}", AManager.GetClearCDCost());//暂定20
            challengIntervalCoroutine = StartCoroutine(ArenaChallengInterval((int)AManager.CD));              //暂定10分钟
        }
        else
        {
            m_btn_NowChalleng.gameObject.SetActive(false);
            m_label_NowChalleng_price.gameObject.SetActive(false);
            m_label_Challenge_Time.gameObject.SetActive(false);
        }
    }

    /// <summary>
    /// 刷新挑战次数
    /// </summary>
    void ChallengeTimesUpdate()
    {
        m_label_number_challenge.text = AManager.ChallengeTimes.ToString() + "/" + AManager.MaxTimes.ToString();
        if (DataManager.Manager<ArenaManager>().ChallengeTimes == 0)
        {
            m_btn_add_challenge.gameObject.SetActive(true);
        }
        else
        {
            m_btn_add_challenge.gameObject.SetActive(false);
        }
    }


    /// <summary>
    /// 定时取消等待
    /// </summary>
    //void HideArenaWaitingPanel()
    //{
    //    DataManager.Manager<UIPanelManager>().HidePanel(PanelID.ArenaWaitingPanel);
    //}


    IEnumerator ArenaChallengInterval(int time)
    {
        for (int i = time; i > 0; i--)
        {
            TimeSpan ts = new TimeSpan(0, 0, i);

            m_label_Challenge_Time.text = string.Format("{0}:{1:D2}", ts.Minutes, ts.Seconds);
            yield return new WaitForSeconds(1);
        }
        m_btn_NowChalleng.gameObject.SetActive(false);
        m_label_NowChalleng_price.gameObject.SetActive(false);
        m_label_Challenge_Time.gameObject.SetActive(false);
    }



    /// <summary>
    /// 那个child在中间回调
    /// </summary>
    /// <param name="go"></param>
    //void OnCenterOnChildCallback(GameObject go)
    //{
    //    if (go == m_widget_challengeThreeContent.gameObject)
    //    {
    //        m_sprite_leftArrow.gameObject.SetActive(false);
    //        m_sprite_rightArrow.gameObject.SetActive(true);
    //    }

    //    if (go == m_widget_TopThreeContent.gameObject)
    //    {
    //        m_sprite_leftArrow.gameObject.SetActive(true);
    //        m_sprite_rightArrow.gameObject.SetActive(false);
    //    }

    //}

    /// <summary>
    /// 清挑战次数
    /// </summary>
    void CheckAndReqRefreshChallengeTimes()
    {
        uint cost = AManager.GetResetChallengeTimesCost();

        Action yes = delegate
        {
            if (MainPlayerHelper.IsHasEnoughMoney(ClientMoneyType.YuanBao, cost))
            {
                //向服务器请求
                AManager.ReqRefreshChallengeTimes();
            }
            else
            {
                //元宝不足去充值
                GotoRecharge();
            }
        };

        string des = Tmger.GetLocalFormatText(LocalTextType.Arena_Commond_7, Tmger.GetCurrencyNameByType((GameCmd.MoneyType)ClientMoneyType.YuanBao), "x" + cost);//消耗{0}元宝重置全部挑战次数

        TipsManager.Instance.ShowTipWindow(TipWindowType.YesNO, des, yes, null, title: Tmger.GetLocalText(LocalTextType.Local_TXT_Tips), okstr: Tmger.GetLocalText(LocalTextType.Local_TXT_Confirm), cancleStr: Tmger.GetLocalText(LocalTextType.Local_TXT_Cancel));
    }

    /// <summary>
    /// 元宝不足去充值
    /// </summary>
    void GotoRecharge()
    {
        //去充值
        Action gotoRecharge = delegate
        {
            UIPanelBase.PanelJumpData jumpData = new PanelJumpData();
            jumpData.Tabs = new int[1];
            jumpData.Tabs[0] = (int)MallPanel.TabMode.ChongZhi;//充值
            DataManager.Manager<UIPanelManager>().ShowPanel(PanelID.MallPanel, jumpData: jumpData);
        };

        //元宝不足处理  Clan_Commond_yuanbaobuzu
        TipsManager.Instance.ShowTipWindow(TipWindowType.YesNO, Tmger.GetLocalText(LocalTextType.Clan_Commond_yuanbaobuzu), gotoRecharge, null, title: Tmger.GetLocalText(LocalTextType.Local_TXT_Tips), okstr: "去充值", cancleStr: Tmger.GetLocalText(LocalTextType.Local_TXT_Cancel));
    }

    /// <summary>
    /// 清挑战Cd
    /// </summary>
    void CheckAndReqClearCD()
    {
        uint cost = AManager.GetClearCDCost();

        Action yes = delegate
        {
            if (MainPlayerHelper.IsHasEnoughMoney(ClientMoneyType.YuanBao, cost))
            {
                //向服务器请求
                AManager.ReqRefreshCD();
            }
            else
            {
                //元宝不足去充值
                GotoRecharge();
            }
        };

        string des = Tmger.GetLocalFormatText(LocalTextType.Arena_Commond_6, Tmger.GetCurrencyNameByType((GameCmd.MoneyType)ClientMoneyType.YuanBao), "x" + cost);//消耗{0}元宝消除冷却时间
        TipsManager.Instance.ShowTipWindow(TipWindowType.YesNO, des, yes, null, title: Tmger.GetLocalText(LocalTextType.Local_TXT_Tips), okstr: Tmger.GetLocalText(LocalTextType.Local_TXT_Confirm), cancleStr: Tmger.GetLocalText(LocalTextType.Local_TXT_Cancel));
    }

    #endregion


    #region click

    void onClick_Close_Btn(GameObject caster)
    {
        this.HideSelf();
    }


    /// <summary>
    /// 设置武斗场技能
    /// </summary>
    /// <param name="caster"></param>
    void onClick_Btn_setskill_Btn(GameObject caster)
    {
        DataManager.Manager<ArenaSetSkillManager>().ReqArenaSetSkillList();
    }

    /// <summary>
    /// 战报
    /// </summary>
    /// <param name="caster"></param>
    void onClick_Btn_report_Btn(GameObject caster)
    {
        AManager.ReqBattlelog();
    }

    /// <summary>
    /// 排行榜
    /// </summary>
    /// <param name="caster"></param>
    void onClick_Btn_ranklist_Btn(GameObject caster)
    {
        ReturnBackUIData[] returnData = new ReturnBackUIData[1];
        returnData[0] = new ReturnBackUIData();
        returnData[0].msgid = UIMsgID.eNone;
        returnData[0].panelid = PanelID.ArenaPanel;
        returnData[0].param = null;

        string type = "OrderListType_Simulation";
        //DataManager.Manager<UIPanelManager>().ShowPanel(PanelID.RankPanel, data: type, returnBackUIData: returnData);
        DataManager.Manager<UIPanelManager>().ShowPanel(PanelID.RankPanel, data: type);
    }

    /// <summary>
    /// 积分商城
    /// </summary>
    /// <param name="caster"></param>
    void onClick_Btn_store_Btn(GameObject caster)
    {
        // DataManager.Manager<MallManager>().SetBlackMarkeActiveStore(GameCmd.CommonStore.CommonStore_Five);

        ReturnBackUIData[] returnData = new ReturnBackUIData[1];
        returnData[0] = new ReturnBackUIData();
        returnData[0].msgid = UIMsgID.eNone;
        returnData[0].panelid = PanelID.ArenaPanel;
        returnData[0].param = null;

        UIPanelBase.PanelJumpData jumpData = new PanelJumpData();
        jumpData.Tabs = new int[1];
        jumpData.Tabs[0] = 3;//积分商城
        DataManager.Manager<UIPanelManager>().ShowPanel(PanelID.BlackMarketPanel, jumpData: jumpData);
    }

    /// <summary>
    /// 换一组
    /// </summary>
    /// <param name="caster"></param>
    void onClick_Btn_change_Btn(GameObject caster)
    {
        AManager.ReqArenaRivalThree();
    }

    void onClick_Add_challenge_Btn(GameObject caster)
    {
        AManager.ChallengeTarget = null;
        CheckAndReqRefreshChallengeTimes();
    }


    //立即挑战
    void onClick_NowChalleng_Btn(GameObject caster)
    {
        AManager.ChallengeTarget = null;
        CheckAndReqClearCD();
    }

    /// <summary>
    /// 查看奖励
    /// </summary>
    /// <param name="caster"></param>
    void onClick_CheckRewardBtn_Btn(GameObject caster)
    {
        DataManager.Manager<UIPanelManager>().ShowPanel(PanelID.ArenaCheckRewardPanel);
    }

    void onClick_LeftArrow_Btn(GameObject caster)
    {
        m_btn_leftArrow.gameObject.SetActive(false);
        m_btn_rightArrow.gameObject.SetActive(true);

        bool show = m_trans_gridContent.transform.localPosition.x == 0;
        Vector3 pos;
        if (show)
        {
            pos = new Vector3(850, m_trans_gridContent.transform.localPosition.y, m_trans_gridContent.transform.localPosition.z);
            TweenPosition.Begin(m_trans_gridContent.gameObject, 0.5f, pos);
        }

        UpdateTopThree();//前三名  top3
    }

    void onClick_RightArrow_Btn(GameObject caster)
    {
        m_btn_leftArrow.gameObject.SetActive(true);
        m_btn_rightArrow.gameObject.SetActive(false);

        bool show = m_trans_gridContent.transform.localPosition.x == 850;
        Vector3 pos;
        if (show)
        {
            pos = new Vector3(0, m_trans_gridContent.transform.localPosition.y, m_trans_gridContent.transform.localPosition.z);
            TweenPosition.Begin(m_trans_gridContent.gameObject, 0.5f, pos);
        }
    }

    void ReleaseAtlas()
    {
        if (null != iuiIconAtlas)
        {
            iuiIconAtlas.Release(true);
            iuiIconAtlas = null;
        }
    }



    #endregion

}

