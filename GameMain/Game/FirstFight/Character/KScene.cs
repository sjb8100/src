using System;
using System.Collections.Generic;
using Client;
using UnityEngine;

public class KScene
{
    public List<KNpc> m_NpcList = new List<KNpc>();
    public List<KPlayer> m_PlayerList = new List<KPlayer>();

    private static uint s_uSceneid = 15;  // 战斗场景id

    public void Enter()
    {
        IMapSystem mapSys = ClientGlobal.Instance().GetMapSystem();
        if (mapSys == null)
        {
            return;
        }

        mapSys.EnterMap(s_uSceneid, Vector3.zero);

        IControllerSystem cs = ClientGlobal.Instance().GetControllerSystem();
        if (cs == null)
        {
            return;
        }

        Client.IPlayer mainPlayer = ClientGlobal.Instance().MainPlayer;
        if (mainPlayer != null)
        {
            mainPlayer.SendMessage(EntityMessage.EntityCommand_SetVisible, false);
        }

    }

    public void Activate()
    {
        for (int i = 0; i < m_NpcList.Count; ++i)
        {
            KNpc pNpc = m_NpcList[i];
            pNpc.Activate();
        }



    }

    public int RemoveNpc(KNpc pNpc)
    {
        int nRetCode = 0;

        pNpc.m_pScene = null;

        m_NpcList.Remove(pNpc);

        nRetCode = 1;

        return nRetCode;
    }


    public void AddPlayer(KPlayer pPlayer)
    {
        m_PlayerList.Add(pPlayer);

        pPlayer.m_pScene = this;

    }

    public void AddNpc(KNpc pNpc)
    {
        m_NpcList.Add(pNpc);

        pNpc.m_pScene = this;
    }


    public bool TraverseRangePlayer<T>(ref T Func)
    {
        return TraversePlayer(ref Func);
    }
    public bool TraverseRangeNpc<T>(ref T Func)
    {
        return TraverseNpc(ref Func);
    }

    private bool TraversePlayer<T>(ref T Func)
    {
        bool bResult = false;
        bool bRetCode = false;

        KSearchTactic rTactic = Func as KSearchTactic;

        if (rTactic == null)
        {
            goto Exit0;
        }

        for (int i = 0; i < m_PlayerList.Count; ++i)
        {
            KPlayer pPlayer = m_PlayerList[i];

            bRetCode = rTactic.OperatorSearch(pPlayer);
            if (bRetCode == false) { goto Exit0; }

        }

        bResult = true;
    Exit0:
        return bResult;

    }

    private bool TraverseNpc<T>(ref T Func)
    {
        bool bResult = false;
        bool bRetCode = false;

        KSearchTactic rTactic = Func as KSearchTactic;

        if (rTactic == null)
        {
            goto Exit0;
        }

        for (int i = 0; i < m_NpcList.Count; ++i)
        {
            KNpc pNpc = m_NpcList[i];

            bRetCode = rTactic.OperatorSearch(pNpc);
            if (bRetCode == false) { goto Exit0; }

        }

        bResult = true;
    Exit0:
        return bResult;
    }

}

