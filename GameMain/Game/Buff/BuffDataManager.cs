//*************************************************************************
//	创建日期:	2016/11/3 17:11:39
//	文件名称:	BuffDataManager
//   创 建 人:   zhudianyu	
//	版权所有:	中青宝
//	说    明:	游戏主界面
//*************************************************************************

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Client;
using table;
using GameCmd;
using Engine;
using Engine.Utility;
using UnityEngine;
using System.Collections;


class BuffDataManager : BaseModuleData, IManager, ITimer
{
    public ArrayList MainRoleBuffList
    {
        get
        {
            return mainRoleBuffList;
        }
    }
    public ArrayList TargetBuffList
    {
        get
        {
            return targetBuffList;
        }
    }
    ArrayList mainRoleBuffList = new ArrayList();
    ArrayList targetBuffList = new ArrayList();
    private readonly uint m_nBuffTimerID = 1000;
    #region IManager
    void IManager.Initialize()
    {
        EventEngine.Instance().AddEventListener((int)GameEventID.BUFF_ADDTOTARGETBUFF, OnEvent);
        EventEngine.Instance().AddEventListener((int)GameEventID.BUFF_DELETETARGETBUFF, OnEvent);
        EventEngine.Instance().AddEventListener((int)GameEventID.BUFF_UPDATEARGETBUFF, OnEvent);
        EventEngine.Instance().AddEventListener((int)GameEventID.RECONNECT_SUCESS, OnEvent);
        EventEngine.Instance().AddEventListener((int)GameEventID.ENTITYSYSTEM_TARGETCHANGE, OnEvent);

        TimerAxis.Instance().SetTimer(m_nBuffTimerID, 500, this);
    }

    void IManager.Reset(bool depthClearData = false)
    {
        CommonData.SafeClearList(mainRoleBuffList);
        CommonData.SafeClearList(targetBuffList);
    }

    void IManager.Process(float deltaTime)
    {

    }
    public void ClearData()
    {

    }
    #endregion
    void OnEvent(int eventID, object param)
    {
        switch ((Client.GameEventID)eventID)
        {
            case GameEventID.BUFF_ADDTOTARGETBUFF:
                OnEeventAddBuff(eventID, (Client.stAddBuff)param);
                break;
            case GameEventID.BUFF_DELETETARGETBUFF:
                OnEventRemoveBuff(eventID, (Client.stRemoveBuff)param);
                break;
            case GameEventID.BUFF_UPDATEARGETBUFF:
                break;
            case GameEventID.ENTITYSYSTEM_TARGETCHANGE:
                {
                    stTargetChange tc = (stTargetChange)param;
                    IEntity en = tc.target;
                    if(en != null)
                    {
                        IBuffPart bp = en.GetPart(EntityPart.Buff) as IBuffPart;
                        if(bp != null)
                        {
                            List<stAddBuff> list = null;
                            bp.GetBuffList(out list);
                           if(list != null)
                           {
                               targetBuffList.Clear();
                               for(int i = 0;i<list.Count;i++)
                               {
                                   targetBuffList.Insert(0, list[i]);
                               }
                           }

                        }
                    }
                }
                break;
            case GameEventID.RECONNECT_SUCESS:
                {
                    stReconnectSucess reconnectSucess = (stReconnectSucess)param;
                    if (reconnectSucess.isLogin)
                    {
                        ClearAllBuff();
                    }
                }
                break;
        }
    }

    public void ClearAllBuff()
    {

        for (int i = 0; i < mainRoleBuffList.Count;i++ )
        {
            var buff = mainRoleBuffList[i];
            Client.stAddBuff st = (Client.stAddBuff)buff;
            RemoveBuffByClient(st);
        }

        for (int i = 0; i < targetBuffList.Count;i++ )
        {
            var tarbuff = targetBuffList[i];
            Client.stAddBuff st = (Client.stAddBuff)tarbuff;
            RemoveBuffByClient(st);
        }
        CommonData.SafeClearList(mainRoleBuffList);
        CommonData.SafeClearList(targetBuffList);
    }
    void OnEeventAddBuff(int eventID, Client.stAddBuff state)
    {
        BuffDataBase db = GameTableManager.Instance.GetTableItem<BuffDataBase>(state.buffid);
        if (db != null)
        {
            if (db.dwShield == 1)
            {//不显示的直接跳过
                return;
            }
        }
        if (ClientGlobal.Instance().IsMainPlayer(state.uid))
        {
            mainRoleBuffList.Insert(0, state);
            stShowBuffInfo info = new stShowBuffInfo();
            info.IsMainRole = true;
            DataManager.Manager<UIPanelManager>().SendMsg(PanelID.MainPanel, UIMsgID.stShowBuff, info);

        }
        else
        {
            Client.IEntity target = ClientGlobal.Instance().GetControllerSystem().GetActiveCtrl().GetCurTarget();
            if (target != null)
            {
                if (target.GetUID() == state.uid)
                {
                    targetBuffList.Insert(0, state);
                    stShowBuffInfo info = new stShowBuffInfo();
                    info.IsMainRole = false;
                    DataManager.Manager<UIPanelManager>().SendMsg(PanelID.MainPanel, UIMsgID.stShowBuff, info);
                }
            }

        }
    }

    void OnEventRemoveBuff(int eventID, stRemoveBuff param)
    {
        if (ClientGlobal.Instance().IsMainPlayer(param.uid))
        {
            for (int i = 0; i < mainRoleBuffList.Count; i++)
            {
                Client.stAddBuff st = (Client.stAddBuff)mainRoleBuffList[i];
                if (st.buffThisID == param.buffThisID)
                {
                    mainRoleBuffList.RemoveAt(i);
                    break;
                }
            }
            stShowBuffInfo info = new stShowBuffInfo();
            info.IsMainRole = true;
            DataManager.Manager<UIPanelManager>().SendMsg(PanelID.MainPanel, UIMsgID.stShowBuff, info);
        }
        else
        {

            for (int i = 0; i < targetBuffList.Count; i++)
            {
                Client.stAddBuff st = (Client.stAddBuff)targetBuffList[i];
                if (st.buffThisID == param.buffThisID && st.uid == param.uid)
                {
                    targetBuffList.RemoveAt(i);
                    break;
                }
            }
            stShowBuffInfo info = new stShowBuffInfo();
            info.IsMainRole = false;
            DataManager.Manager<UIPanelManager>().SendMsg(PanelID.MainPanel, UIMsgID.stShowBuff, info);
        }

    }

    public void OnTimer(uint uTimerID)
    {
        for (int i = 0; i < mainRoleBuffList.Count; i++)
        {
            Client.stAddBuff st = (Client.stAddBuff)mainRoleBuffList[i];
            if(st.lTime > 0)
            {
                st.lTime -= 500;
                mainRoleBuffList[i] = st;
                if (st.lTime <= 0)
                {
                    RemoveBuffByClient(st);
                }
            }

        }

        for (int j = 0; j < targetBuffList.Count; j++)
        {
            Client.stAddBuff st = (Client.stAddBuff)targetBuffList[j];
            if(st.lTime > 0)
            {
                st.lTime -= 500;
                targetBuffList[j] = st;
                if(st.lTime <= 0)
                {
                    RemoveBuffByClient(st);
                }
            }
        
        }
    }
    void RemoveBuffByClient(Client.stAddBuff st)
    {
        stRemoveBuff sb = new stRemoveBuff();
        sb.uid = st.uid;
        sb.buffid = st.buffid;
        sb.buffThisID = st.buffThisID;
        BuffDataBase bdb = GameTableManager.Instance.GetTableItem<BuffDataBase>(st.buffid);
        if(bdb != null)
        {
            if(bdb.forever == 1)
            {
                return;
            }
        }
   
        OnEventRemoveBuff(0, sb);
    }
}

