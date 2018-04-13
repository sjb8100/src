using Client;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using table;
using UnityEngine;

/// <summary>
/// 城战部分
/// </summary>
public partial class MainPanel
{

    /// <summary>
    /// 初始化城战
    /// </summary>

    long m_lCloseNpcid = 0;
    void InitFBWidget()
    {
        m_label_FBTimeLabel.text = string.Empty;
    }

    /// <summary>
    /// 是否显示副本退出及倒计时
    /// </summary>
    void ShowFBWidgetUI()
    {
        //退出倒计时和战况
        CopyCDAndExitInfo copyCDAndExitInfo = DataManager.Manager<ComBatCopyDataManager>().CopyCDAndExitData;
        if (copyCDAndExitInfo != null)
        {
            ShowFBWidgetUI(copyCDAndExitInfo.bShow, copyCDAndExitInfo.bShowBattleInfoBtn);
        }
        else
        {
            ShowFBWidgetUI(false, false);
        }

        //鼓舞和世界聚宝倒计时
        uint copyId = DataManager.Manager<ComBatCopyDataManager>().EnterCopyID;
        if (DataManager.Manager<ComBatCopyDataManager>().IsWorldJuBao(copyId))
        {
            m_trans_FBBtn_JuBao.gameObject.SetActive(true);
            float jubaoCd = DataManager.Manager<ComBatCopyDataManager>().worldJuBaoCD;
            if (jubaoCd > 0)
            {
                TimeSpan ts = new TimeSpan(0, 0, (int)jubaoCd);
                m_trans_FBBtn_JuBaoStartCD.gameObject.SetActive(true);
                m_label_FBBtn_JuBaoStartCDTxt.text = string.Format("{0:D2}:{1:D2}", ts.Minutes, ts.Seconds);
            }
            else
            {
                m_trans_FBBtn_JuBaoStartCD.gameObject.SetActive(false);
            }
        }
        else
        {
            m_trans_FBBtn_JuBao.gameObject.SetActive(false);
        }
    }

    /// <summary>
    /// 是否显示副本退出及倒计时
    /// </summary>
    void ShowFBWidgetUI(bool show, bool showBattleInfoBtn)
    {
        if (show)
        {
            RegisterEvent(true);
            //打开根节点
            m_trans_FB.gameObject.SetActive(true);

            //初始化时间(氏族领地不显示时间)
            uint clanTerritoryCopyId = GameTableManager.Instance.GetGlobalConfig<uint>("ClanTerritoryCopyID");
            uint copyId = DataManager.Manager<ComBatCopyDataManager>().EnterCopyID;
            if (copyId == clanTerritoryCopyId)
            {
                m_label_FBTimeLabel.gameObject.SetActive(false);
            }
            else
            {
                m_label_FBTimeLabel.gameObject.SetActive(true);
            }

            //初始化按钮
            if (showBattleInfoBtn)
            {
                m_btn_FBBtn_Info.gameObject.SetActive(true);
            }
            else
            {
                m_btn_FBBtn_Info.gameObject.SetActive(false);
            }
        }
        else
        {
            RegisterEvent(false);
            m_trans_FB.gameObject.SetActive(false);

        }

    }

    void RegisterEvent(bool register)
    {
        if (register)
        {
            Engine.Utility.EventEngine.Instance().AddEventListener((int)Client.GameEventID.CAMP_ADDCOLLECTNPC, OnEvent);
        }
        else
        {
            Engine.Utility.EventEngine.Instance().RemoveEventListener((int)Client.GameEventID.CAMP_ADDCOLLECTNPC, OnEvent);
        }
    }

    float m_fbTempTime = 0f;
    /// <summary>
    /// 更新副本时间显示
    /// </summary>
    void UpdateFBTime()
    {
        if (false == DataManager.Manager<ComBatCopyDataManager>().IsEnterCopy)
        {
            return;
        }

        this.m_fbTempTime += Time.deltaTime;
        if (this.m_fbTempTime > 0.95f)
        {
            float copyCD = DataManager.Manager<ComBatCopyDataManager>().CopyCountDown;

            //副本倒计时
            UpdateFBTimeLabel(copyCD);

            //世界聚宝开始倒计时
            UpdateJuBaoStartCd();

            this.m_fbTempTime = 0;
        }
    }

    //更新时间显示
    void UpdateFBTimeLabel(float time)
    {
        TimeSpan ts = new TimeSpan(0, 0, (int)time);

        if (DataManager.Manager<ArenaManager>().EnterArena)  //武斗场
        {
            if (DataManager.Manager<ArenaManager>().StartBattle)
            {
                m_label_FBTimeLabel.text = string.Format("{0}:{1:D2}", ts.Minutes, ts.Seconds);
            }
            else
            {
                m_label_FBTimeLabel.text = string.Empty;
            }
        }
        else //非武斗场
        {
            m_label_FBTimeLabel.text = string.Format("{0}:{1:D2}", ts.Minutes, ts.Seconds);
        }
    }

    //采集
    void UpdateFBPickBtn(object param)
    {
        if (DataManager.Manager<ComBatCopyDataManager>().IsEnterCopy)
        {
            if (param == null)
            {
                return;
            }

            stCampCollectNpc npc = (stCampCollectNpc)param;

            //npc在傍边
            bool isNpcExist = false;
            IEntitySystem es = ClientGlobal.Instance().GetEntitySystem();
            if (es != null)
            {
                if (null != es.FindEntity(npc.npcid))
                {
                    isNpcExist = true;
                }
            }


            if (npc.enter && isNpcExist)
            {
                m_btn_FBPickBtn.gameObject.SetActive(true);
                m_lCloseNpcid = npc.npcid;
            }
            else
            {
                m_lCloseNpcid = 0;
                m_btn_FBPickBtn.gameObject.SetActive(false);
            }
        }
        else
        {
            m_btn_FBPickBtn.gameObject.SetActive(false);
        }
    }

    void UpdateJuBaoStartCd()
    {
        uint copyId = DataManager.Manager<ComBatCopyDataManager>().EnterCopyID;
        if (DataManager.Manager<ComBatCopyDataManager>().IsWorldJuBao(copyId))
        {
            if (DataManager.Manager<JvBaoBossWorldManager>().IsWaitBossAppear)
            {
                TimeSpan ts = new TimeSpan(0, 0, (int)DataManager.Manager<JvBaoBossWorldManager>().WorldBossSpawnLeftTime);
                m_trans_FBBtn_JuBaoStartCD.gameObject.SetActive(true);
                m_label_FBBtn_JuBaoStartCDTxt.text = string.Format("{0:D2}:{1:D2}", ts.Minutes, ts.Seconds);
            }
            else
            {
                m_trans_FBBtn_JuBaoStartCD.gameObject.SetActive(false);
            }
        }
    }

    //关闭UI
    void CloseFBUI()
    {
        m_trans_FB.gameObject.SetActive(false);
    }

    //----------------------------副本引导-----------------------------------------------------------------------------------------------------------------------------
    /// <summary>
    /// 副本引导UI
    /// </summary>
    void ShowFBGuideUI()
    {
        ComBatCopyDataManager copyMgr = DataManager.Manager<ComBatCopyDataManager>();
        if (false == copyMgr.IsEnterCopy)
        {
            this.m_widget_copyGuide.gameObject.SetActive(false);
            return;
        }

        CopyDataBase copyDb = GameTableManager.Instance.GetTableItem<CopyDataBase>(copyMgr.EnterCopyID);
        if (copyDb == null)
        {
            return;
        }

        if (true == DataManager.Manager<ComBatCopyDataManager>().m_haveShowCopyGuide)
        {
            return;
        }
        DataManager.Manager<ComBatCopyDataManager>().m_haveShowCopyGuide = true;

        CopyTargetGuideDataBase ctGuideDb = GameTableManager.Instance.GetTableItem<CopyTargetGuideDataBase>(copyDb.guideId);
        if (ctGuideDb == null)
        {
            return;
        }

        this.m_widget_copyGuide.gameObject.SetActive(true);
        m_label_copyGuideDes.text = ctGuideDb.guideStr;
        StartCoroutine(DelayToCloseCopyGuide(ctGuideDb.time));
    }

    /// <summary>
    /// 延时关闭引导
    /// </summary>
    /// <param name="delay"></param>
    /// <returns></returns>
    IEnumerator DelayToCloseCopyGuide(float delay)
    {
        yield return new WaitForSeconds(delay);
        if (true == this.m_widget_copyGuide.gameObject.activeSelf)
        {
            this.m_widget_copyGuide.gameObject.SetActive(false);
        }
    }

    #region click

    /// <summary>
    /// 离开副本
    /// </summary>
    /// <param name="caster"></param>
    void onClick_FBBtn_exit_Btn(GameObject caster)
    {
        TipsManager.Instance.ShowTipWindow(TipWindowType.CancelOk, "即将离开副本，是否继续？", () =>
        {
            DataManager.Manager<ComBatCopyDataManager>().ReqExitCopy();
        }, okstr: "确定", cancleStr: "取消", title: "离开副本");
    }

    /// <summary>
    /// 战况
    /// </summary>
    /// <param name="caster"></param>
    void onClick_FBBtn_Info_Btn(GameObject caster)
    {
        //打开战况
        ComBatCopyDataManager copyMgr = DataManager.Manager<ComBatCopyDataManager>();

        //1、 城战战况
        List<CityWarDataBase> cityWarList = GameTableManager.Instance.GetTableList<CityWarDataBase>();

        if (cityWarList != null && cityWarList.Exists((d) => { return d.CopyId == copyMgr.EnterCopyID; }))
        {
            DataManager.Manager<UIPanelManager>().ShowPanel(PanelID.CityWarPanel);
        }

        //2、聚宝战况
        if (copyMgr.IsWorldJuBao(copyMgr.EnterCopyID))
        {
            DataManager.Manager<UIPanelManager>().ShowPanel(PanelID.JvBaoBossDamRankPanel);
        }
    }

    void onClick_FBPickBtn_Btn(GameObject caster)
    {
        if (DataManager.Manager<SliderDataManager>().IsReadingSlider)
        {
            return;
        }
        IEntity npc = ClientGlobal.Instance().GetEntitySystem().FindEntity(m_lCloseNpcid);
        if (npc != null)
        {
            bool ride = DataManager.Manager<RideManager>().IsRide;
            if (ride)
            {
                ClientGlobal.Instance().netService.Send(new GameCmd.stDownRideUserCmd_C() { });

            }

            NetService.Instance.Send(new GameCmd.stClickNpcScriptUserCmd_C()
            {
                dwNpcTempID = npc.GetID(),
            });
        }
        else
        {
            m_btn_FBPickBtn.gameObject.SetActive(false);
        }
    }

    /// <summary>
    /// 关闭副本引导
    /// </summary>
    /// <param name="caster"></param>
    void onClick_CopyGuideBg_Btn(GameObject caster)
    {
        if (true == m_widget_copyGuide.gameObject.activeSelf)
        {
            m_widget_copyGuide.gameObject.SetActive(false);
        }
    }

    /// <summary>
    /// 世界聚宝，鼓舞按钮
    /// </summary>
    /// <param name="caster"></param>
    void onClick_FBBtn_guwu_Btn(GameObject caster)
    {
        DataManager.Manager<UIPanelManager>().ShowPanel(PanelID.InspirePanel);
    }

    #endregion

}

