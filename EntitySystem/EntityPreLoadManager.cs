
//*************************************************************************
//	创建日期:	2017/7/4 星期二 14:44:06
//	文件名称:	EntityPoolManager
//   创 建 人:   zhudianyu	
//	版权所有:	zhudianyu
//	说    明:	实体预加载管理
//*************************************************************************
using UnityEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Engine.Utility;
using Client;
using System.Collections;
namespace EntitySystem
{
    public class EntityPreLoadManager : Singleton<EntityPreLoadManager>,ITimer
    {
        Dictionary<string, Engine.IRenderObj[]> m_preLoadDic = new Dictionary<string, Engine.IRenderObj[]>();
        int m_uPreLoadNum = 1;
        bool bSettingNotPreLoad = false;
        List<uint> m_preLoadMapIDList = new List<uint>();
        public void InitPreLoadManager()
        {
            m_uPreLoadNum = GameTableManager.Instance.GetGlobalConfig<int>("PreLoadEntityNum");
            if(m_uPreLoadNum == 0)
            {
                m_uPreLoadNum = 10;
            }
            m_uPreLoadNum = 1;
            RegisterEvents(true);
            m_preLoadMapIDList = GameTableManager.Instance.GetGlobalConfigList<uint>("PreLoadMapIDList");
          //  bSettingNotPreLoad = PlayerPrefs.GetInt("ePreLoad", 0) == 1 ? true : false; 
        }

        public void ClearPreLoadManager()
        {
            RegisterEvents(false);
        }
        void RegisterEvents(bool bReg)
        {
            if(bReg)
            {
                EventEngine.Instance().AddEventListener((int)GameEventID.ENTITYSYSTEM_ENTERMAP, OnEvent);
                EventEngine.Instance().AddEventListener((int)GameEventID.SYSTEM_LOADSCENECOMPELETE, OnEvent);
                EventEngine.Instance().AddEventListener((int)GameEventID.ENTITYSYSTEM_LEAVEMAP, OnEvent);
            }
            else
            {
                EventEngine.Instance().RemoveEventListener((int)GameEventID.ENTITYSYSTEM_ENTERMAP, OnEvent);
                EventEngine.Instance().RemoveEventListener((int)GameEventID.SYSTEM_LOADSCENECOMPELETE, OnEvent);
                EventEngine.Instance().RemoveEventListener((int)GameEventID.ENTITYSYSTEM_LEAVEMAP, OnEvent);
            }
        }
        void OnEvent(int eventID,object param)
        {
            if (eventID == (int)GameEventID.SYSTEM_LOADSCENECOMPELETE)
            {
                Client.stLoadSceneComplete loadScene = (Client.stLoadSceneComplete)param;
             
                uint mapID =(uint)loadScene.nMapID;
                //PreLoadEntity(mapID);
            }
            else if(eventID == (int)GameEventID.ENTITYSYSTEM_LEAVEMAP)
            {
                ClearPreLoadEntity();
            }
            else if(eventID == (int)GameEventID.ENTITYSYSTEM_ENTERMAP)
            {
                uint mapID = (uint)param;
                PreLoadEntity(mapID);
            }

        }
        //小怪预载列表
        List<uint> preLoadMonsterList = new List<uint>();

        //npc预载列表
        List<uint> preLoadNpcList = new List<uint>();
        void PreLoadEntity(uint mapID)
        {
            if(m_preLoadMapIDList.Count == 0)
            {
                return;
            }
            if(!m_preLoadMapIDList.Contains(mapID))
            {
                return;
            }
            //bSettingNotPreLoad = PlayerPrefs.GetInt("ePreLoad", 0) == 1 ? true : false; 
            if(bSettingNotPreLoad)
            {
                return;
            }
            IMapSystem ms = EntitySystem.m_ClientGlobal.GetMapSystem();
            if(ms == null)
            {
                return;
            }

            preLoadMonsterList.Clear();
            preLoadNpcList.Clear();
            Dictionary<int, List<NPCInfo>> m_dic = ms.GetMapXmlInfo();
            var iter = m_dic.GetEnumerator();
            while(iter.MoveNext())
            {
                var dic = iter.Current;
                table.NpcDataBase npcData = GameTableManager.Instance.GetTableItem<table.NpcDataBase>((uint)dic.Key);
                if(npcData != null)
                {//只预载普通怪
                    if (npcData.dwMonsterType == 1 && npcData.dwType == 0)
                    {
                        if(!preLoadMonsterList.Contains(npcData.dwModelSet))
                        {
                            preLoadMonsterList.Add(npcData.dwModelSet);
                        }
                    }
                    else if (npcData.dwMonsterType == 0 && npcData.dwType == 2)
                    {//功能npc
                        if (!preLoadNpcList.Contains(npcData.dwModelSet))
                        {
                            preLoadNpcList.Add(npcData.dwModelSet);
                        }
                    }
                }
            }
            if(TimerAxis.Instance().IsExist(m_uTimerID,this))
            {
                TimerAxis.Instance().KillTimer(m_uTimerID, this);
            }
            TimerAxis.Instance().SetTimer(m_uTimerID, 100,this);
            LoadEntity();
        
        }
        uint m_uTimerID = 1000;
        public void OnTimer(uint uTimerID)
        {
            if(m_uTimerID == uTimerID)
            {
                if(preLoadNpcList.Count > 0)
                {
                    uint resID = preLoadNpcList[0];
                    table.ResourceDataBase rdb = GameTableManager.Instance.GetTableItem<table.ResourceDataBase>(resID);
                    if (rdb != null)
                    {
                        Engine.IRenderObj[] objList;
                        Log.LogGroup("ZDY", "preload npc {0} num is {1}", rdb.strPath, m_uPreLoadNum);
                        Engine.RareEngine.Instance().CacheRenderObj(rdb.strPath, 1, out objList);
                        if (!m_preLoadDic.ContainsKey(rdb.strPath))
                        {
                            m_preLoadDic.Add(rdb.strPath, objList);
                        }
                    }
                    preLoadNpcList.RemoveAt(0);
                }
                if (preLoadMonsterList.Count > 0)
                {
                    uint resID = preLoadMonsterList[0];
                    table.ResourceDataBase rdb = GameTableManager.Instance.GetTableItem<table.ResourceDataBase>(resID);
                    if (rdb != null)
                    {
                        Engine.IRenderObj[] objList;
                        Log.LogGroup("ZDY", "preload entity {0} num is {1}", rdb.strPath, m_uPreLoadNum);
                        Engine.RareEngine.Instance().CacheRenderObj(rdb.strPath, m_uPreLoadNum, out objList);
                        if (!m_preLoadDic.ContainsKey(rdb.strPath))
                        {
                            m_preLoadDic.Add(rdb.strPath, objList);
                        }
                    }
                    preLoadMonsterList.RemoveAt(0);
                }

                if(preLoadNpcList.Count == 0 && preLoadMonsterList.Count == 0)
                {
                    TimerAxis.Instance().KillTimer(m_uTimerID, this);
                }
            }
         
        }
        void LoadEntity()
        {
            for (int i = 0; i < preLoadNpcList.Count; i++)
            {
                // yield return null;
                uint resID = preLoadNpcList[i];
                table.ResourceDataBase rdb = GameTableManager.Instance.GetTableItem<table.ResourceDataBase>(resID);
                if (rdb != null)
                {
                    Engine.IRenderObj[] objList;
                    Log.LogGroup("ZDY", "preload npc {0} num is {1}", rdb.strPath, m_uPreLoadNum);
                    Engine.RareEngine.Instance().CacheRenderObj(rdb.strPath, 1, out objList);
                    if (!m_preLoadDic.ContainsKey(rdb.strPath))
                    {
                        m_preLoadDic.Add(rdb.strPath, objList);
                    }
                }
            }
            for (int i = 0; i < preLoadMonsterList.Count; i++)
            {
                //yield return null;
                uint resID = preLoadMonsterList[i];
                table.ResourceDataBase rdb = GameTableManager.Instance.GetTableItem<table.ResourceDataBase>(resID);
                if (rdb != null)
                {
                    Engine.IRenderObj[] objList;
                    Log.LogGroup("ZDY", "preload entity {0} num is {1}", rdb.strPath, m_uPreLoadNum);
                    Engine.RareEngine.Instance().CacheRenderObj(rdb.strPath, m_uPreLoadNum, out objList);
                    if (!m_preLoadDic.ContainsKey(rdb.strPath))
                    {
                        m_preLoadDic.Add(rdb.strPath, objList);
                    }
                }
            }

     
        }
        void ClearPreLoadEntity()
        {
            if(m_preLoadDic != null)
            {
                var iter = m_preLoadDic.GetEnumerator();
                while(iter.MoveNext())
                {
                    var objArray = iter.Current.Value;
                    for(int i = 0;i<objArray.Length;i++)
                    {
                        Engine.IRenderObj obj = objArray[i];
                        if(obj != null)
                        {
                            Engine.IRenderSystem rs = Engine.RareEngine.Instance().GetRenderSystem();
                            if(rs != null)
                            {
                                if(rs.CanRemovePreLoad(obj.GetID()))
                                {
                                    rs.RemoveRenderObj(obj);
                                }
                                
                            }
                        }
                    }
                    objArray = null;
                }
                m_preLoadDic.Clear();
            }
        }

     
    }
}