using System;
using System.Collections.Generic;

class NpcAscription : Singleton<NpcAscription>
{
    public bool IsBelongOther(uint owernuserid,uint owernteamid,uint ownerclanid)
    {
        bool belongOther = false;
        if (owernuserid != 0)
        {
            if (owernuserid != Client.ClientGlobal.Instance().MainPlayer.GetID())
            {
                belongOther = true;
            }
        }
        if (!belongOther && (owernteamid != 0 || ownerclanid != 0))
        {
            if (DataManager.Manager<TeamDataManager>().IsJoinTeam == true)
            {
                if (DataManager.Manager<TeamDataManager>().TeamId != owernteamid)
                {
                    belongOther = true;
                }
            }
            else if (DataManager.Manager<ClanManger>().IsJoinClan == true)
            {
                if (DataManager.Manager<ClanManger>().ClanId != ownerclanid)
                {
                    belongOther = true;
                }
            }
            else
            {
                belongOther = true;
            }
        }

        return belongOther;
    }

    public void UpdateBelong(Client.INPC npc, uint owernuserid, uint owernteamid,uint ownerclanid,string ownerName)
    {
        bool preBelong = npc.BelongOther;

        bool nowBelong = IsBelongOther(owernuserid, owernteamid, ownerclanid);

        npc.TeamID = owernteamid;
        npc.OwnerID = owernuserid;
        if (preBelong != nowBelong)
        {
            Client.stEntityChangename e = new Client.stEntityChangename();
            e.uid = npc.GetUID();
            Engine.Utility.EventEngine.Instance().DispatchEvent((int)Client.GameEventID.ENTITYSYSTEM_CHANGENAME, e);
        }
        stRefreshNPCBelongParam param = new stRefreshNPCBelongParam()
        {
            npcid = npc.GetUID(),
            teamid = owernteamid,
            ownerid = owernuserid,
            clanid = ownerclanid,
            ownerName = ownerName,
        };
        OnBelongChanged(param);
        
        DataManager.Manager<UIPanelManager>().SendMsg(PanelID.MainPanel, UIMsgID.eRefreshNpcBelong, param);
    }

    Dictionary<long,stRefreshNPCBelongParam> m_dic_NpcBelongData = new Dictionary<long,stRefreshNPCBelongParam>(); 
//     public stRefreshNPCBelongParam NpcBelongData 
//     {
//         get 
//         {
//             return npcBelongData;
//         }
//     }
    public void OnBelongChanged(stRefreshNPCBelongParam param)
    {
        if (m_dic_NpcBelongData.Count >= 300)
        {
            m_dic_NpcBelongData.Clear();
        }
        if (m_dic_NpcBelongData.ContainsKey(param.npcid))
        {
            m_dic_NpcBelongData[param.npcid] = param;
        }
        else
        {
            m_dic_NpcBelongData.Add(param.npcid, param);
        }
    }
    public stRefreshNPCBelongParam GetNpcBelongByNpcID(long npcuid) 
    {
        return m_dic_NpcBelongData.ContainsKey(npcuid) ? m_dic_NpcBelongData[npcuid]:new stRefreshNPCBelongParam();
    }

    public void OnRemoveBelongData(long npcuid) 
    {
        if (m_dic_NpcBelongData.ContainsKey(npcuid))
       {
           m_dic_NpcBelongData.Remove(npcuid);
       }
    }
}