/************************************************************************************
 * Copyright (c) 2017  All Rights Reserved.
 * CLR版本： 4.0.30319.42000
 * 公司名称：ZQB
 * 命名空间：GameMain.Game.OfflineEarnings
 * 创建人：  wenjunhua.zqgame
 * 文件名：  OfflineManager
 * 版本号：  V1.0.0.0
 * 创建时间：5/27/2017 11:29:35 AM
 * 描述：
 ************************************************************************************/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

class OfflineManager: BaseModuleData, IManager
{
    #region property
    //是否有收益
    private bool mbHaveEarning = false;
    private bool mbGameReady = false;
    #endregion


    public bool mShowAdvertise = true;

    #region IManager Method
    public void Initialize()
    {
        mlstAwardItems = new List<BaseItem>();
        mOfflineTime = 0;
        mbHaveEarning = false;
        mbGameReady = false;
        RegisterEvent(true);
    }

    public void Reset(bool depthClearData = false)
    {
        mlstAwardItems.Clear();
        mOfflineTime = 0;
        mbHaveEarning = false;
        mbGameReady = false;
        m_uOfflineExpXS = 0;
        if (depthClearData)
        {
            mShowAdvertise = true;   
        }
    }

    public void Process(float deltaTime)
    {

    }

    public void ClearData()
    {
        mlstAwardItems.Clear();
        mOfflineTime = 0;
        m_uOfflineExpXS = 0;
        mbHaveEarning = false;
        mbGameReady = false;
    }
    #endregion

    #region Op

    public void RegisterEvent(bool register)
    {
        if (register)
        {
            //数据ready
            Engine.Utility.EventEngine.Instance().AddEventListener((int)Client.GameEventID.SYSTEM_GAME_READY, OnClientEventHandler);
            //面板焦点状态改变
            Engine.Utility.EventEngine.Instance().AddEventListener((int)Client.GameEventID.UIEVENT_PANELFOCUSSTATUSCHANGED, OnClientEventHandler);
        }
        else
        {
            Engine.Utility.EventEngine.Instance().RemoveEventListener((int)Client.GameEventID.SYSTEM_GAME_READY, OnClientEventHandler);
            //面板焦点状态改变
            Engine.Utility.EventEngine.Instance().RemoveEventListener((int)Client.GameEventID.UIEVENT_PANELFOCUSSTATUSCHANGED, OnClientEventHandler);
        }
    }


    /// <summary>
    /// 事件处理
    /// </summary>
    /// <param name="nEventID"></param>
    /// <param name="param"></param>
    private void OnClientEventHandler(int nEventID, object param)
    {
        switch (nEventID)
        {
            case (int)Client.GameEventID.SYSTEM_GAME_READY:
                {
                    mbGameReady = true;
                    if (mbGameReady && mbHaveEarning)
                    {
                        ShowOfflineEarningsGet();
                    }

                    ShowAdvertise();

                }
                break;
            /***************面板焦点状态改变********************/
            case (int)Client.GameEventID.UIEVENT_PANELFOCUSSTATUSCHANGED:
                {
                    UIPanelManager.PanelFocusData status = (UIPanelManager.PanelFocusData)param;
                    if (status.GetFocus)
                    {
                        if (status.ID == PanelID.MainPanel)
                        {
                           if (mbGameReady && mbHaveEarning)
                           {
                               ShowOfflineEarningsGet();

                               ShowAdvertise();
                           }
                        }
                    }
                }
                break;
        }
    }

    //离线经验系数
    public uint m_uOfflineExpXS = 0;
    public  uint OfflineExpXS
    {
        get
        {
            return m_uOfflineExpXS;
        }
    }


    //离线开始等级
    public const string CONST_OFFLINE_STARTLV_NAME = "OpenOffRewardLev";
    public static uint OfflineStartLv
    {
        get
        {
            return GameTableManager.Instance.GetGlobalConfig<uint>(CONST_OFFLINE_STARTLV_NAME);
        }
    }

    //离线收益最小时间
    public const string CONST_OFFLINE_MIN_TIME_NAME = "OfflineMinTime";
    public static uint OfflineMinTime
    {
        get
        {
            return GameTableManager.Instance.GetGlobalConfig<uint>(CONST_OFFLINE_MIN_TIME_NAME);
        }
    }

    //离线收益最大时间
    public const string CONST_OFFLINE_MAX_TIME_NAME = "OfflinemMaxTime";
    public static uint OfflinemMaxTime
    {
        get
        {
            return GameTableManager.Instance.GetGlobalConfig<uint>(CONST_OFFLINE_MAX_TIME_NAME);
        }
    }

    //离线奖励物品
    private List<BaseItem> mlstAwardItems = null;
    public List<BaseItem> AwardItems
    {
        get
        {
            return mlstAwardItems;
        }
    }
    //离线时间
    private uint mOfflineTime = 0;
    public uint OfflineTime
    {
        get
        {
            return mOfflineTime;
        }
    }

    public uint OfflineExpAward
    {
        get
        {
            return OfflineExpXS * mOfflineTime;
        }
    }
    public void ShowOfflineEarningsGet()
    {
        if (mbHaveEarning)
        {
            DataManager.Manager<UIPanelManager>().ShowPanel(PanelID.OfflineEarningsPanel);
            mbHaveEarning = false;
        }

    }

    public void ShowAdvertise()
    {
        if (mShowAdvertise)
        {
            DataManager.Manager<UIPanelManager>().ShowPanel(PanelID.advertisePanel);
            mShowAdvertise = false;
        }
    }

    /// <summary>
    /// 服务器下发奖励
    /// </summary>
    /// <param name="itemIds"></param>
    /// <param name="offlineTime"></param>
    public void OnSendOfflineReward(List<GameCmd.ItemSerialize> itemData, uint offlineTime,uint offlineXS)
    {
        mlstAwardItems.Clear();
        if (null != itemData && itemData.Count > 0)
        {
            BaseItem baseItem = null;
            GameCmd.ItemSerialize offlineReward = null;
            for (int i = 0; i < itemData.Count; i++)
            {
                offlineReward = itemData[i];
                baseItem = new BaseItem(offlineReward.dwObjectID,offlineReward);
                mlstAwardItems.Add(baseItem);
            }
        }
        mOfflineTime = offlineTime;
        m_uOfflineExpXS = offlineXS;
        mbHaveEarning = true;
    }


    public void GetOfflineReward(bool doubleEarning)
    {
        if (null != DataManager.Instance.Sender)
        {
            DataManager.Instance.Sender.GetOfflineRewardReq(doubleEarning);
        }
    }

    public void OnGetOfflineReward()
    {
        mbHaveEarning = false;
        mOfflineTime = 0;
        mlstAwardItems.Clear();
    }
    #endregion
}