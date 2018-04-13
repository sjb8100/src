using Engine.Utility;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using table;
using UnityEngine;
//*******************************************************************************************
//	创建日期：	2017-2-21   10:02
//	文件名称：	NvWaPanel,cs
//	创 建 人：	Lin Chuang Yuan
//	版权所有：	ZQB
//	说    明：	
//*******************************************************************************************

partial class NvWaPanel : ITimer
{
    #region property

    int m_closeTime = 10;

    private const int NVWA_TIMERID = 1000;

    NvWaManager NWManager
    {
        get
        {
            return DataManager.Manager<NvWaManager>();
        }
    }

    uint m_cdTime = 0;

    uint maxGuard = 0;
    #endregion


    #region override

    protected override void OnLoading()
    {
        base.OnLoading();

        maxGuard = GameTableManager.Instance.GetGlobalConfig<uint>("NWMaxGuard");
    }

    protected override void OnShow(object data)
    {
        base.OnShow(data);

        InitBattle();

        //第几波      
        SetNewWave();
    }

    protected override void OnHide()
    {
        base.OnHide();
    }

    public override bool OnMsg(UIMsgID msgid, object param)
    {
        if (msgid == UIMsgID.eNvWaNewWave)
        {
            SetNewWave();
        }
        else if (msgid == UIMsgID.eNvWaDesCD)
        {
            stCopyBattleInfo info = (stCopyBattleInfo)param;
            SetDesAndCD(info);
        }
        else if (msgid == UIMsgID.eNvWaReslut)
        {
            //InitResult();
        }
        else if (msgid == UIMsgID.eNvWaExit)
        {
            NvwaExit();
        }

        return true;
    }

    protected override void OnPanelBaseDestory()
    {
        base.OnPanelBaseDestory();
    }

    #endregion


    #region method


    /// <summary>
    /// 战斗中显示
    /// </summary>
    void InitBattle()
    {
        m_trans_BattleContent.gameObject.SetActive(true);

        m_trans_DesAndCD.gameObject.SetActive(false);
    }

    /// <summary>
    /// 新的一波
    /// </summary>
    void SetNewWave()
    {
        m_trans_BattleContent.gameObject.SetActive(true);
        m_trans_DesAndCD.gameObject.SetActive(true);

        //波数
        m_trans_Round.gameObject.SetActive(true);
        if (NWManager.CopyWave > 0)
        {
            m_label_NvWaRoundNum.text = string.Format("{0}", NWManager.CopyWave);
        }
        else
        {
            m_label_NvWaRoundNum.text = "";
        }

        //下方的描述信息
        uint copyId = DataManager.Manager<ComBatCopyDataManager>().EnterCopyID;
        CopyDisplayDataBase cddb = GameTableManager.Instance.GetTableItem<CopyDisplayDataBase>(copyId, (int)NWManager.CopyWave);
        if (cddb != null)
        {
            m_label_battleDes.text = cddb.des;
        }
        else
        {
            m_label_battleDes.text = "";
        }
    }

    /// <summary>
    /// 描述和CD
    /// </summary>
    /// <param name="info"></param>
    void SetDesAndCD(stCopyBattleInfo info)
    {
        this.m_cdTime = info.cd;

        m_label_NvWaTimeNum.text = info.cd.ToString();

        TimerAxis.Instance().KillTimer(NVWA_TIMERID, this);
        TimerAxis.Instance().SetTimer(NVWA_TIMERID, 1000, this);
    }


    void NvwaExit()
    {
        TimerAxis.Instance().KillTimer(NVWA_TIMERID, this);
        HideSelf();
    }

    #endregion


    #region Timer

    public void OnTimer(uint uTimerID)
    {
        if (this.m_cdTime > 0)
        {
            this.m_cdTime--;

            if (this.m_cdTime == 0)
            {
                m_label_NvWaTimeNum.text = "";
            }
            else
            {
                m_label_NvWaTimeNum.text = this.m_cdTime.ToString();
            }
        }
    }

    #endregion
}

