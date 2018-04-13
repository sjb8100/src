using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GameCmd;
using Engine;
using Engine.Utility;
using Client;
using table;

public class NobleParam
{
   public uint time;
   public uint freeWenqian;
}
partial class Mall_HuangLingManager : BaseModuleData, IManager
{
    public void BuyNobleRequest(uint id) 
    {
        if (UnityEngine.Application.isEditor)
        {
            NetService.Instance.Send(new stBuyNoblePropertyUserCmd_CS() { noble_id = id });
        }else
        {
            table.NobleDataBase nobleDB = GameTableManager.Instance.GetTableItem<table.NobleDataBase>(id);
            if (null != nobleDB)
            {
                DataManager.Manager<RechargeManager>().DoRecharge(nobleDB.rechargeID);
            }
        }
        
        
        
    }
    public void GetNobleDailyMoney(uint id ) 
    {
        NetService.Instance.Send(new stNobleGiveDayFreeCoinPropertyUserCmd_CS() { noble_id = id });
    }
    public uint remain_time = 0;
    public uint Remain_Time 
    {
        get
        {
            return remain_time;
        }
        set 
        {
            remain_time = value;
        }
    }

    public uint nobleID = 1;
    public uint NobleID
    {
        get 
        {
            return nobleID;
        }
        set
        {
            nobleID = value;
        }
    }
    public uint hunt_time = 0;
    public uint Hunt_time
    {
        get
        {
            return hunt_time;
        }
        set
        {
            hunt_time = value;
        }
    }
    private Dictionary<uint,NobleParam> nobleDic;
    public Dictionary<uint, NobleParam> NobleDic 
    {
        set 
        {
            nobleDic = value;          
        }
        get 
        { 
            return nobleDic;
        }
    
    
    }
    public void OnBuyNobel(stBuyNoblePropertyUserCmd_CS cmd) 
    {
        Remain_Time = cmd.protected_time;
        NobleDataBase table = GameTableManager.Instance.GetTableItem<NobleDataBase>(cmd.noble_id);
        TipsManager.Instance.ShowTips(string.Format(DataManager.Manager<TextManager>().GetLocalFormatText(LocalTextType.Noble_Commond_1, table.name)));
        if (NobleDic.ContainsKey(cmd.noble_id))
        {
            NobleDic[cmd.noble_id].time = cmd.protected_time;
        }
        else 
        {
            NobleDic.Add(cmd.noble_id, new NobleParam
            {
                time = cmd.protected_time,
                freeWenqian = 0,
            }
              );
        }
        if (NobleID < cmd.noble_id)
        {
            NobleID = cmd.noble_id;
        }
        stNobleTempIndex index = new stNobleTempIndex() { nobleID = cmd.noble_id };
        Engine.Utility.EventEngine.Instance().DispatchEvent((int)Client.GameEventID.BUYNOBLESUCCESS, index);
        HasNobleWarning();
    }
    public void GetNobleMoneySuccess(stNobleGiveDayFreeCoinPropertyUserCmd_CS  cmd) 
    {
     
        if (NobleDic.ContainsKey(cmd.noble_id))
        {
            NobleDic[cmd.noble_id].freeWenqian = cmd.Coin_num;
        }
        else
        {
            NobleDic.Add(cmd.noble_id, new NobleParam
            {
                freeWenqian = cmd.Coin_num,             
            }
              );
        }
        if (NobleID < cmd.noble_id)
        {
            NobleID = cmd.noble_id;
        }
        stNobleTempIndex index = new stNobleTempIndex() { nobleID = cmd.noble_id };
        Engine.Utility.EventEngine.Instance().DispatchEvent((int)Client.GameEventID.GETNOBLEMONEYSUCCESS, index);
        HasNobleWarning();
    }
    public void OnRecieveMsg(stNobleInfoPropertyUserCmd_S cmd) 
    {
        List<NobleDataBase> tb = GameTableManager.Instance.GetTableList<NobleDataBase>();
        List<uint> protected_time = cmd.protected_time;
        List<uint> givefreewenqian = cmd.givefreewenqian;
        NobleDic = new Dictionary<uint, NobleParam>();
        for (int i = 0; i < protected_time.Count;i++ )
        {
            if (protected_time[i]!=0)
            {
                NobleID = (uint)i+2;
                NobleDic.Add(NobleID, new NobleParam
                {
                    time = protected_time[i],
                    freeWenqian = givefreewenqian[i],
                }
                );
            }
        }
    }


     public  void HasNobleWarning()
    {
        int num = 0;
        foreach (var i in NobleDic)
        {
            if (i.Value.freeWenqian == 0)
            {
                num++;
            }
        }
         m_boolHasNobleWarning = num > 0;

        stShowMainPanelRedPoint st = new stShowMainPanelRedPoint()
        {
            modelID = (int)WarningEnum.Noble,
            direction = (int)WarningDirection.None,
            bShowRed = m_boolHasNobleWarning,
        };
        Engine.Utility.EventEngine.Instance().DispatchEvent((int)Client.GameEventID.MAINPANEL_SHOWREDWARING, st);
    }
    private bool m_boolHasNobleWarning = false;
    public bool M_boolHasNobleWarning
    {
        get 
        {
             return m_boolHasNobleWarning;
        }
     
    }
    //stNobleInfoPropertyUserCmd_S
    public List<uint> alreadyFirstRecharge = new List<uint>();
    public List<uint> AlreadyFirstRecharge
    {
        get
        {
            return alreadyFirstRecharge;
        }
        set
        {
            alreadyFirstRecharge = value;
        }
    }
    public void OnFirstRechargeList(stAllRechTypeListPropertyUserCmd_S cmd) 
    {
        AlreadyFirstRecharge = cmd.userrech;
    }

    public TimeSpan GetLeftTime(uint timeStamp)
    {
        TimeSpan ts = new TimeSpan(0, 0, (int)timeStamp);

        return ts;
    }

    #region recharge（充值）
    /// <summary>
    /// 显示充值面板
    /// </summary>
    /// <param name="des"></param>
    /// <param name="title"></param>
    /// <param name="confirmTxt"></param>
    /// <param name="cancelTxt"></param>
    public void ShowRecharge(string des,string title = "",string confirmTxt = "",string cancelTxt = "")
    {
        TipsManager.Instance.ShowTipWindow(Client.TipWindowType.CancelOk
                , des
                , () =>
                {
                    UIPanelBase.PanelJumpData jumpData = new UIPanelBase.PanelJumpData();
                    jumpData.Tabs = new int[1];
                    jumpData.Tabs[0] = (int)MallPanel.TabMode.ChongZhi;
                    DataManager.Manager<UIPanelManager>().ShowPanel(PanelID.MallPanel, jumpData: jumpData);
                    if (DataManager.Manager<UIPanelManager>().IsShowPanel(PanelID.ExchangeMoneyPanel))
                    {
                        DataManager.Manager<UIPanelManager>().HidePanel(PanelID.ExchangeMoneyPanel);
                    }
                }
                , null,null
                , title, confirmTxt, cancelTxt);
    }
    public void ShowExchange(string des,ClientMoneyType nType, string title = "", string confirmTxt = "", string cancelTxt = "")
    {
        TipsManager.Instance.ShowTipWindow(Client.TipWindowType.CancelOk
                , des
                , () =>
                {
                    DataManager.Manager<UIPanelManager>().ShowPanel(PanelID.ExchangeMoneyPanel, data: nType);
                }
                , null,null
                , title, confirmTxt, cancelTxt);
    }

    /// <summary>
    /// 显示充值面板
    /// </summary>
    /// <param name="errorNo"></param>
    /// <param name="title"></param>
    /// <param name="confirmTxt"></param>
    /// <param name="cancelTxt"></param>
    public void ShowRecharge(uint errorNo,string title = "",string confirmTxt = "",string cancelTxt = "")
    {
        ShowRecharge(DataManager.Manager<TextManager>().GetLocalText((int)errorNo)
            , title, confirmTxt, cancelTxt);
    }
    public void ShowExchange(uint errorNo,ClientMoneyType nType, string title = "", string confirmTxt = "", string cancelTxt = "")
    {
        ShowExchange(DataManager.Manager<TextManager>().GetLocalText((int)errorNo)
           ,nType , title, confirmTxt, cancelTxt);
    }
    #endregion

    public void Reset(bool depthClearData = false)
    {
        remain_time = 0;
        nobleID = 1;
        hunt_time = 0;
        alreadyFirstRecharge.Clear();
    }
    public void Process(float deltime)
    { }
    public void Initialize()
    {
       
    }

    public void ClearData()
    {

    }
}
