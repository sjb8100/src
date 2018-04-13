using Client;
using Engine.Utility;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;



partial class ArenaBattlePanel : ITimer
{

    int m_startCDTime = 3;  //开始倒计时

    uint m_battleCDTime = 0; //战斗CD 读表

    int m_resultCDTime = 10;//结算倒计时 

    private const int ARENAATART_TIMERID = 2000;

    private const int ARENARESULT_TIMERID = 4000;

    TweenScale m_tweenScale;
    ArenaManager AManager
    {
        get
        {
            return DataManager.Manager<ArenaManager>();
        }
    }

    #region override

    //在窗口第一次加载时，调用
    protected override void OnLoading()
    {
        UIEventListener.Get(m_sprite_bg.gameObject).onClick = OnClickClose;

        m_tweenScale = m_label_startTime.GetComponent<TweenScale>();
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

        Engine.Utility.EventEngine.Instance().DispatchEvent((int)Client.GameEventID.MAINBTN_ONTOGGLE, new Client.stMainBtnSet() { isShow = false, pos = 2 });//默认关闭上面的按钮

        if (data != null)
        {
            ArenaBattleState state = (ArenaBattleState)data;
            if (state == ArenaBattleState.eArenaBattleInit)
            {
                InitArenaBattleUI();
            }
            else if (state == ArenaBattleState.eArenaStartBattleCD)
            {
                ShowStart();
            }
            else if (state == ArenaBattleState.eArenaBattleResult)
            {
                ShowResult();
            }
            else if (state == ArenaBattleState.eArenaExit)
            {
                ExitArena();
            }

        }


    }

    protected override void OnHide()
    {
        base.OnHide();
        m_widget_mask.gameObject.SetActive(false);
        TimerAxis.Instance().KillTimer(ARENAATART_TIMERID, this);
        TimerAxis.Instance().KillTimer(ARENARESULT_TIMERID, this);
    }

    protected override void OnPanelBaseDestory()
    {
        
        base.OnPanelBaseDestory();
    }

    public override bool OnMsg(UIMsgID msgid, object param)
    {
        return true;
    }

    float tempTime = 0;
    void Update()
    {
        tempTime += Time.deltaTime;
        if (tempTime > 0.95f)
        {
            if (AManager.StartBattle)
            {
                EndShowCDTimeUI();
            }

            tempTime = 0;
        }

    }

    #endregion

    void InitArenaBattleUI()
    {
        m_trans_start.gameObject.SetActive(false);
        m_trans_result.gameObject.SetActive(false);
        m_widget_mask.gameObject.SetActive(true);
    }

    /// <summary>
    /// 开始倒计时3、2、1 、开始
    /// </summary>
    void ShowStart()
    {
        m_trans_start.gameObject.SetActive(true);
        m_trans_result.gameObject.SetActive(false);
        m_widget_mask.gameObject.SetActive(true);

        m_startCDTime = 3;

        m_label_startTime.text = m_startCDTime.ToString();
        m_tweenScale.ResetToBeginning();
        m_tweenScale.from = new Vector3(1f, 1f, 1f);
        m_tweenScale.to = new Vector3(0.1f, 0.1f, 0.1f);
        m_tweenScale.duration = 1.25f;
        m_tweenScale.PlayForward();

        TimerAxis.Instance().KillTimer(ARENAATART_TIMERID, this);
        TimerAxis.Instance().SetTimer(ARENAATART_TIMERID, 1250, this);
    }

    /// <summary>
    /// 关闭3、2、1、开始界面
    /// </summary>
    void EndShowCDTimeUI()
    {
        m_trans_start.gameObject.SetActive(false);
        m_widget_mask.gameObject.SetActive(false);
        //m_trans_result.gameObject.SetActive(false);

    }

    /// <summary>
    /// 战斗结果
    /// </summary>
    void ShowResult()
    {
        m_trans_start.gameObject.SetActive(false);
        m_trans_result.gameObject.SetActive(true);

        ArenaBattleResult battleResult = AManager.ArenaBattleResult;
        if (battleResult.result == 1)//胜
        {
            m_sprite_win.gameObject.SetActive(true);
            m_sprite_defeat.gameObject.SetActive(false);

            m_sprite_add.gameObject.SetActive(true);
            m_sprite_less.gameObject.SetActive(false);
        }
        else
        {
            m_sprite_win.gameObject.SetActive(false);
            m_sprite_defeat.gameObject.SetActive(true);

            m_sprite_add.gameObject.SetActive(false);
            m_sprite_less.gameObject.SetActive(true);
        }

        m_label_Rank_Change.text = battleResult.changeRank.ToString();
        m_label_integral.text = battleResult.score.ToString();


        m_resultCDTime = 10;//结算倒计时 
        m_label_close_time.text = m_resultCDTime.ToString();
        TimerAxis.Instance().KillTimer(ARENARESULT_TIMERID, this);
        TimerAxis.Instance().SetTimer(ARENARESULT_TIMERID, 1000, this);
    }

    void ExitArena()
    {
        TimerAxis.Instance().KillTimer(ARENAATART_TIMERID, this);
        TimerAxis.Instance().KillTimer(ARENARESULT_TIMERID, this);

        HideSelf();
    }

    #region click

    /// <summary>
    /// 点击退出武斗场
    /// </summary>
    /// <param name="o"></param>
    void OnClickClose(GameObject o)
    {
        if (this.gameObject.activeSelf == false) return;

        //向服务器发送退出武斗场消息
        DataManager.Manager<ComBatCopyDataManager>().ReqExitCopy();
    }

    #endregion


    #region ITimer

    public void OnTimer(uint uTimerID)
    {
        //3、2、1、开始
        if (uTimerID == ARENAATART_TIMERID)
        {
            m_startCDTime--;
            if (m_startCDTime >= 0)
            {
                m_label_startTime.text = m_startCDTime.ToString();

                m_tweenScale.ResetToBeginning();

                m_tweenScale.from = new Vector3(1f, 1f, 1f);
                m_tweenScale.to = new Vector3(0.1f, 0.1f, 0.1f);
                m_tweenScale.duration = 1.25f;

                m_tweenScale.PlayForward();
            }


            if (m_startCDTime < 0)
            {
                //结束3、2、1 正式开始战斗
                EndShowCDTimeUI();

                TimerAxis.Instance().KillTimer(ARENAATART_TIMERID, this);
            }
        }

        //结算倒计时
        if (uTimerID == ARENARESULT_TIMERID)
        {
            if (m_resultCDTime > 0)
            {
                m_resultCDTime--;

                m_label_close_time.text = m_resultCDTime.ToString();
            }

            if (m_resultCDTime == 0)
            {
                TimerAxis.Instance().KillTimer(ARENARESULT_TIMERID, this);
                OnClickClose(m_sprite_bg.gameObject);
            }
        }
    }
    #endregion
}

