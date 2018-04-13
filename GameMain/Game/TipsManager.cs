using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Client;
using UnityEngine;
using Engine.Utility;
using GameCmd;

public class FightPowerMsg 
{
    public int index;
    public float cd;
    public float time;
}
public class TipsManager :BaseModuleData ,ITipsManager, IManager
{
    float recieveTime = 0;
    bool update = false;
    FightPowerMsg msg = null;
     public static TipsManager Instance
     {
         get 
         {
             return  DataManager.Manager<TipsManager>();
         }
     }
    private void RegisterGlobalEvent(bool register)
    {
        if (register)
        {
            Engine.Utility.EventEngine.Instance().AddEventListener((int)GameEventID.RECONNECT_SUCESS, OnEvent);
            Engine.Utility.EventEngine.Instance().AddEventListener((int)GameEventID.NETWORK_CONNECTE_CLOSE, OnEvent);
            Engine.Utility.EventEngine.Instance().AddEventListener((int)Client.GameEventID.TIPS_EVENT, OnEvent);
        }
        else
        {
            Engine.Utility.EventEngine.Instance().RemoveEventListener((int)GameEventID.RECONNECT_SUCESS, OnEvent);
            Engine.Utility.EventEngine.Instance().RemoveEventListener((int)GameEventID.NETWORK_CONNECTE_CLOSE, OnEvent);
            Engine.Utility.EventEngine.Instance().RemoveEventListener((int)Client.GameEventID.TIPS_EVENT, OnEvent);
        }
    }
    public  void Initialize()
    {
        RegisterGlobalEvent(true);
        
    }
    public void ClearData()
    {

    }
    public void Reset(bool depthClearData = false)
    {
        recieveTime = 0;
        update = false;
    }

    public void UnInitialize()
    {
        RegisterGlobalEvent(false);
    }
    void OnEvent(int nEventid, object param)
    {
        if (nEventid == (int)Client.GameEventID.TIPS_EVENT)
        {
            stTipsEvent en = (stTipsEvent)param;
            table.LocalTextDataBase db = GameTableManager.Instance.GetTableItem<table.LocalTextDataBase>(en.errorID);
            if (db != null)
            {
                TipsManager.Instance.ShowLocalFormatTips((LocalTextType)en.errorID, en.tips);
            }
            else
            {
                if (en.errorID != 0)
                {
                    TipsManager.Instance.ShowTipsById(en.errorID);
                }
            }
        }
        else if (nEventid == (int)Client.GameEventID.RECONNECT_SUCESS) 
        {
        
        }

    }
    public void AddTips(string str, Vector3 pos, TipType type)
    {
        if (string.IsNullOrEmpty(str))
        {
            return;
        }
        ITipsManager tips = ClientGlobal.Instance().GetTipsManager();
        tips.AddTips( str , pos , type );
    }
    /// <summary>
    /// 根据本地语言的宏来显示tips
    /// </summary>
    /// <param name="localtype"></param>
    public void ShowTips(LocalTextType localtype)
    {
        string txt = DataManager.Manager<TextManager>().GetLocalText(localtype);
        ShowTips(txt);
    }

    /// <summary>
    /// 根据本地语言获取格式化tips
    /// </summary>
    /// <param name="localtype"></param>
    /// <param name="param"></param>
    public void ShowLocalFormatTips(LocalTextType localtype,params object[] param)
    {
        string txt = DataManager.Manager<TextManager>().GetLocalFormatText(localtype, param);
        ShowTips(txt);
    }

    bool m_bEnableTips = true;
    public void EnableTips(bool bEnable)
    {
        m_bEnableTips = bEnable;
    }
    public void ShowTips(string msg)
    {
        DataManager.Manager<EffectDisplayManager>().AddTips(msg);
        //if (!m_bEnableTips)
        //{
        //    return;
        //}
        //TipsPanel panel = null;

        //if (string.IsNullOrEmpty(msg))
        //{
        //    msg = "";
        //}
        //if (!DataManager.Manager<UIPanelManager>().IsShowPanel(PanelID.TipsPanel))
        //{
        //    panel = DataManager.Manager<UIPanelManager>().ShowPanel(PanelID.TipsPanel) as TipsPanel;
        //}
        //else
        //{
        //    panel = DataManager.Manager<UIPanelManager>().GetPanel<TipsPanel>(PanelID.TipsPanel);
        //}

        //if (panel != null)
        //{
        //    panel.AddTips(RichXmlHelper.RichXmlAdapt(msg));
        //}
    }

    public void ShowTipsById(uint errorid, params object[] args)
    {
        table.LocalTextDataBase data = GameTableManager.Instance.GetTableItem<table.LocalTextDataBase>(errorid);
        if (data != null)
        {
            if (string.IsNullOrEmpty(data.text))
            {
                Engine.Utility.Log.Error(string.Format("错误码{0}的内容为空", errorid));
            }
            if (data.dwType != 2)
            {
                
                //if(args.Length >= 1)
                //{
                //    if ( args[0].GetType() == typeof( string ) )
                //    {
                //        name = args[0].ToString();
                //    }
                //    else
                //    {
                //        int roleID = (int)args[0];
                //        //先暂时把所有额外参数当错人名处理 后期服务器要改成字符串 add by zhudianyu
                //        IEntitySystem es = ClientGlobal.Instance().GetEntitySystem();
                //        if ( es != null )
                //        {
                //            IPlayer en = es.FindEntity<IPlayer>( (uint)roleID );
                //            if ( en != null )
                //            {
                //                name = en.GetName();
                //            }
                //        }
                //    }
             
                //}
                string strOutLog = "";
                if(args.Length == 0)
                {
                    strOutLog = string.Format( data.text , "" );
                }
                else
                {
                    strOutLog = string.Format( data.text , args ); ;
                }
          
                ShowTips(strOutLog);
            }
            else
            {
                ShowTipWindow(TipWindowType.Ok,data.text,null);
            }
            
        }
        else
        {
            ShowTips("找不到提示id：" + errorid);
        }
    }
    /// <summary>
    /// 显示提示窗口
    /// </summary>
    /// <param name="type">窗口类型</param>
    /// <param name="tipContent">提示内容</param>
    /// <param name="okCallBack">确定回调</param>
    /// <param name="cancelCallBack">取消回调</param>
    /// <param name="okstr">确定按钮上的文本</param>
    /// <param name="cancleStr">取消按钮上是文本</param>
    /// <param name="title">标题</param>
    public void ShowTipWindow(TipWindowType type, string tipContent, Action okCallBack, Action cancelCallBack = null, Action closeCallBack = null, 
        string title = "提示",
        string okstr = "确定",
        string cancleStr = "取消",
        uint _color = 0x000000)
    {

        DataManager.Manager<UIPanelManager>().ShowPanel(PanelID.PrompdPanel, panelShowAction: (pb) =>
            {
                if (null != pb && pb is PrompdPanel )
                {
                    PrompdPanel panel = pb as PrompdPanel;
                    if (panel != null)
                    {
                        panel.Init(new TipWidnowParam()
                        {
                            windowtype = type,
                            winContent = tipContent,
                            winTitle = title,
                            ok = okCallBack,
                            cancel = cancelCallBack,
                            close = closeCallBack,
                            oktxt = okstr,
                            canletxt = cancleStr,
                            color = _color,
                        });
                    }
                }
            });
        
    }

    /// <summary>
    /// 显示提示窗口
    /// </summary>
    /// <param name="type">窗口类型</param>
    /// <param name="tipContent">提示内容</param>
    /// <param name="okCallBack">确定回调</param>
    /// <param name="cancelCallBack">取消回调</param>
    /// <param name="okstr">确定按钮上的文本</param>
    /// <param name="cancleStr">取消按钮上是文本</param>
    /// <param name="title">标题</param>
    /// <param name="okCdTime">确定按钮倒计时</param>
    /// <param name="cancelCdTime">取消按钮倒计时</param>
    public void ShowTipWindow(uint okCdTime, uint cancelCdTime, TipWindowType type, string tipContent, Action okCallBack, Action cancelCallBack = null, Action closeCallBack = null, string title = "提示", string okstr = "确定", string cancleStr = "取消")
    {
        DataManager.Manager<UIPanelManager>().ShowPanel(PanelID.PrompdPanel, panelShowAction: (pb) =>
        {
            if (null != pb && pb is PrompdPanel)
            {
                PrompdPanel panel = pb as PrompdPanel;
                panel.Init(new TipWidnowParam()
                {
                    windowtype = type,
                    winContent = tipContent,
                    winTitle = title,
                    ok = okCallBack,
                    cancel = cancelCallBack,
                    close = closeCallBack,
                    oktxt = okstr,
                    canletxt = cancleStr,
                    okCDTime = okCdTime,
                    cancelCDTime = cancelCdTime
                });
            }
        });
    }

    public void HideTipWindow()
    {
        DataManager.Manager<UIPanelManager>().HidePanel(PanelID.PrompdPanel);
    }
    #region ItemTips(物品Tips)

    /// <summary>
    /// 显示物品tips
    /// </summary>
    /// <param name="id">baseId 或者 唯一id</param>
    /// <param name="fireObj">触发Tips的UI对象用于定位tips位置</param>
    /// <param name="needCompare">是否需要对比（针对装备）</param>
    public void ShowItemTips(uint id, UnityEngine.GameObject fireObj = null, bool needCompare = false)
    {
        BaseItem baseItem = DataManager.Manager<ItemManager>().GetBaseItemByQwThisId(id);
        if (null == baseItem)
        {
            baseItem = DataManager.Manager<ItemManager>().GetTempBaseItemByBaseID<BaseItem>(id);
        }
        ShowItemTips(baseItem, fireObj, needCompare);
    }

    public void ShowItemTips(BaseItem baseItem, GameObject fileObj = null, bool needCompare = false)
    {
        if (baseItem.IsEquip)
        {
            NetService.Instance.Send(new stRequestDurabilityPropertyUserCmd_C() { qwThisID = baseItem .QWThisID});
        }
        DataManager.Manager<UIPanelManager>().ShowItemTips(baseItem, fileObj, needCompare);
    }

   /// <summary>
   /// 使用道具
   /// </summary>
   /// <param name="title">抬头</param>
   /// <param name="itemId">物品Id</param>
   /// <param name="okDel">确认回调</param>
   /// <param name="cancelDel">取消回调</param>
   /// <param name="belowDel">超过最小设置数量</param>
   /// <param name="overDel">超过背包中道具存量</param>
   /// <param name="okStr">确认按钮文字</param>
   /// <param name="cancelStr">取消按钮文字</param>
   /// <param name="leftUseTimes">剩余使用次数</param>
   /// <param name="setNum">是否可以设置使用数量，默认不可设置道具使用数量</param>
   /// <param name="useDianjuan">是否可使用元宝兑换，默认可以使用元宝兑换道具</param>
    public void ShowUseItemWindow(string title, uint itemId,OkDelegate okDel, Action cancelDel ,Action belowDel = null,Action overDel = null,string okStr = "使用",string cancelStr = "取消",int leftUseTimes = -1,bool setNum = false ,bool canUseDianjuan = false)
    {
        UseItemWidnowParam param = new UseItemWidnowParam { title = title, itemId = itemId, ok = okDel
            , cancel = cancelDel,belowMinCount = belowDel
            ,overMaxCount = overDel, oktxt = okStr
            , canletxt = cancelStr,LeftUseTimes = leftUseTimes 
            ,canSetNum = setNum,canUseDianjuan = canUseDianjuan};

        DataManager.Manager<UIPanelManager>().ShowPanel(PanelID.CommonUseItemPanel, panelShowAction: (pb) =>
        {
            if (null != pb && pb is CommonUseItemPanel)
            {
                CommonUseItemPanel panel = pb as CommonUseItemPanel;
                panel.Init(param);
            }
        });
    }

    #endregion

    public void ShowFightPowerChangeTips(bool value,int newPower,int prePower)
    {
        DataManager.Manager<UIPanelManager>().ShowPanel(PanelID.TipsPanel, panelShowAction: (pb) =>
        {
            if (null != pb && pb is TipsPanel)
            {
                TipsPanel panel = pb as TipsPanel;
                if (panel != null)
                {
                    msg = new FightPowerMsg()
                    {
                        time = UnityEngine.Time.realtimeSinceStartup,
                        cd = 1.5f,
                    };
                    panel.ShowFightPowerChangeTips(value, newPower, prePower);
                }
            }
        });
    }
    /// <summary>
    /// 显示消耗单个道具tips
    /// </summary>
    /// <param name="cp"></param>
    public void ShowSingelConsumPanel(CommonSingleParam cp)
    {
        DataManager.Manager<UIPanelManager>().ShowPanel(PanelID.CommonSingleUseItemPanel, data: cp);
    }

    public void Process(float deltaTime)
    {
        if (msg == null)
        {
            return;
        }
        if (msg != null)
        {
            msg.cd -= deltaTime;

        }
        if (!DataManager.IsFrameCountRemainderEq0())
        {
            //限制调用次数
            return;
        }

        if (msg.cd <= 0)
        {
            DataManager.Manager<UIPanelManager>().SendMsg(PanelID.TipsPanel, UIMsgID.eFightPowerChange, null);
            msg = null;
        }
    }
}

