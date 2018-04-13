using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GameCmd;
using Engine;
using Engine.Utility;
using Client;
using table;
public enum HomeMineState
{
    [System.ComponentModel.Description( "未解锁" )]
    Lock,
    [System.ComponentModel.Description( "可以探矿" )]
    CanFind ,//可以探矿
    [System.ComponentModel.Description( "开采中" )]
    Mining ,//开采中
    [System.ComponentModel.Description( "可以收获" )]
    CanGain ,//可以收获
}
partial class HomeDataManager
{
    //开矿数据
    MineData mineData;
    #region 属性
    MinePanel mineUI
    {
        get
        {
            return DataManager.Manager<UIPanelManager>().GetPanel( PanelID.MinePanel ) as MinePanel;
        }
    }
    /// <summary>
    /// 要开采的矿石id
    /// </summary>
    uint stoneID = 0;
    public uint StoneID
    {
        get
        {
            return stoneID;
        }
        set
        {
            stoneID = value;
        }
    }
    /// <summary>
    /// 预计收获的矿石数量
    /// </summary>
    uint stoneNum = 0;
    public uint GainStoneNum
    {
        get
        {
            return stoneNum;
        }
        set
        {
            stoneNum = value;
        }
    }
    /// <summary>  
    /// 开采消耗的罗盘id
    /// </summary>
    uint comPassID = 0;
    public uint ComPassID
    {
        get
        {
            return comPassID;
        }
        set
        {
            comPassID = value;
        }
    }
    bool minevipIsOpen = false;
    public bool MineVIPIsOpen
    {
        get
        {
            return minevipIsOpen;
        }
        set
        {
            minevipIsOpen = value;
        }
    }
    /// <summary>
    /// 是否vip矿洞
    /// </summary>
    bool ismineVip = false;
    public bool IsMineVIP
    {
        set
        {
            ismineVip = value;
            RefreshMineData( mineData );
        }
        get
        {
            return ismineVip;
        }
    }
    /// <summary>
    /// 采矿状态
    /// </summary>
    HomeMineState mineState = HomeMineState.CanFind;
    public HomeMineState MineState
    {
        get
        {
            return mineState;
        }
        set
        {
            if(mineState != value )
            {
                mineState = value;
                DispatchValueUpdateEvent(new ValueUpdateEventArgs()
                    {
                        key = HomeDispatchEvent.ChangeMineState.ToString(),
                    });
            }
        

        }
    }
    uint digBeginTime = 0;//开始挖掘时间 0表示没有挖掘
    public uint DigBeginTime
    {
        get
        {
            return digBeginTime;
        }
        set
        {
            digBeginTime = value;
            if(digBeginTime == 0)
            {
                MineState = HomeMineState.CanFind;
            }
        }
    }
    uint digFactor = 1;//挖掘倍数
    public uint DigFactor
    {
        get
        {
            return digFactor;
        }
        set
        {
            digFactor = value;
        }
    }
    uint mineLeftTime = 0;//挖矿剩余时间
    public uint MineLeftTime
    {
        get
        {
            return mineLeftTime;
        }
        set
        {
            mineLeftTime = value;
            if(mineLeftTime != 0)
            {
                MineState = HomeMineState.Mining;
            }
            else
            {
                MineState = HomeMineState.CanGain;
            }
        }
    }
    uint robbedNum = 0;//被抢的矿数量
    public uint RobbedNum
    {
        get
        {
            return robbedNum;
        }
        set
        {
            robbedNum = value;
        }
    }
    uint robbedTimes = 0;//被抢几次
    public uint RobbedTimes
    {
        get
        {
            return robbedTimes;
        }
        set
        {
            robbedTimes = value;
        }
    }
    /// <summary>
    /// 收获次数
    /// </summary>
    uint minegainTime = 0;
    public uint MineGainTime
    {
        set
        {
            minegainTime = value;
        }
        get
        {
            return minegainTime;
        }
    }

    uint vipGainTime = 0;
    public uint VipGainTime
    {
        set
        {
            vipGainTime = value;
        }
        get
        {
            return vipGainTime;
        }
    }
    HoleData normalHole;
    HoleData vipHole;
    #endregion
    void OnClickMineEntity(IEntity et)
    {
        DataManager.Manager<UIPanelManager>().ShowPanel( PanelID.MinePanel );
    }
    void SetVipIsOpen(bool isOpen)
    {
        MineVIPIsOpen = isOpen;
    }
    void RefreshMineData(MineData data)
    {
        mineData = data;
        if(normalHole == null)
        {
            normalHole = data.normal_hole;
        }
        if(vipHole == null)
        {
            vipHole = data.vip_hole;
        }
        if ( IsMineVIP )
        {
            RefreshUIByHoleData( vipHole );
        }
        else
        {
            RefreshUIByHoleData( normalHole );
        }
    }
    void RefreshUIByHoleData(HoleData data)
    {
        if(data != null)
        {
            StoneID = (uint)data.mine_base_id;
            ComPassID = (uint)data.item;
            GainStoneNum = (uint)data.mine_num;
            MineLeftTime = (uint)data.cost_time;
            DigBeginTime = (uint)data.dig_begin;
            DigFactor = (uint)data.dig_num;
            RobbedNum = (uint)data.robbed_num;
            RobbedTimes = (uint)data.be_rob_num;
            if(mineUI != null)
            {
                mineUI.RefreshUI();
            }
        }
    }
    public MineData GetMineData()
    {
        if ( mineData == null )
        {
            Log.Error( "mine data is null" );
        }
        return mineData;
    }
    public void OnGetMineAtOnce(stImmediMineHomeUserCmd_CS cmd)
    {
        if(cmd.is_vip)
        {
            VipGainTime = cmd.fast_gain;
            vipHole.cost_time = 0;
        }
        else
        {
            MineGainTime = cmd.fast_gain;
            normalHole.cost_time = 0;
        }
       
        MineLeftTime = 0;
        mineState = HomeMineState.CanGain;
        TipsManager.Instance.ShowTips(DataManager.Manager<TextManager>().GetLocalText(LocalTextType.Local_TXT_Notice_HomeMineCanGain));
        if(mineUI != null)
        {
            mineUI.RefreshUI();
        }
    }
    public void OnDigMineSuccess(stHoleDataHomeUserCmd_S cmd)
    {
        if(cmd.is_vip)
        {
            vipHole = cmd.hole;
        }
        else
        {
            normalHole = cmd.hole;
        }
        MineState = HomeMineState.Mining;
        RefreshUIByHoleData( cmd.hole );
      
    }
    public void OnFindMine(stFindMineHomeUserCmd_CS cmd)
    {
        if ( mineUI != null )
        {
            if(cmd.is_vip)
            {
                vipHole.mine_base_id = (uint)cmd.mine_base_id;
                vipHole.mine_num = (uint)cmd.mine_num;
                vipHole.item = (uint)cmd.item;
            }
            else
            {
                normalHole.mine_base_id = (uint)cmd.mine_base_id;
                normalHole.mine_num = (uint)cmd.mine_num;
                normalHole.item = (uint)cmd.item;
            }
            stoneID = (uint)cmd.mine_base_id;
            stoneNum = (uint)cmd.mine_num;
            comPassID = (uint)cmd.item;
            if ( stoneNum != 0 )
            {
                TipsManager.Instance.ShowTipsById( 114515 );
            }
            mineUI.RefreshStone( cmd );
        }
    }
    public void OnGainMine(stGainMineHomeUserCmd_CS cmd)
    {
        ItemDataBase db = GameTableManager.Instance.GetTableItem<ItemDataBase>((uint)cmd.mine_base_id);
        if(db != null)
        {
            TipsManager.Instance.ShowTipsById( 114520 , db.itemName , cmd.mine_num );
            GainStoneNum = 0;//刷新ui需求
            StoneID = 0;
            DigBeginTime = 0;
            MineState = HomeMineState.CanFind;
        }
    }
    public void OpenVipHole(stUnlockMineHomeUserCmd_CS cmd)
    {
        SetVipIsOpen( true );
        IsMineVIP = true;
    }
    public void OnRobReport(stNewRobReportHomeUserCmd_S cmd)
    {
        //被抢的数量只是本次的
    }
    public string GetMineStateStr()
    {
        return MineState.GetEnumDescription();
    }
    float temp = 0;
  

    void OnMineProcess()
    {
        if (DigBeginTime == 0)
        {
            mineState = HomeMineState.CanFind;
            return;
        }
        if (mineLeftTime > 0)
        {

             mineLeftTime -= 1;
     
            if (mineLeftTime <= 0)
            {
                mineLeftTime = 0;
                MineState = HomeMineState.CanGain;

            }
            else
            {
                mineState = HomeMineState.Mining;
            }
        }
        else
        {
            mineState = HomeMineState.CanGain;
        }

    }
}

