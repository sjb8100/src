using System;
using System.Collections.Generic;

class PKModeData : Singleton<PKModeData>
{
    public GameCmd.enumPKMODE MainPlayerPkMode
    {
        get
        {
            Client.IPlayer player = Client.ClientGlobal.Instance().MainPlayer;
            if (player != null)
            {
                int currMode = player.GetProp((int)Client.PlayerProp.PkMode);

                return (GameCmd.enumPKMODE)currMode;
            }

            return GameCmd.enumPKMODE.PKMODE_M_NONE;
        }
    }

    public GameCmd.enumPKMODE GetEntityPkMode(uint uid)
    {
        Client.IEntitySystem es = Client.ClientGlobal.Instance().GetEntitySystem();
        if (es == null)
        {
            Engine.Utility.Log.Error(" EntitySystem is NULL");
            return GameCmd.enumPKMODE.PKMODE_M_NONE;
        }

        Client.IPlayer player = es.FindPlayer(uid);
        if (player != null)
        {
            int currMode = player.GetProp((int)Client.PlayerProp.PkMode);

            return (GameCmd.enumPKMODE)currMode;
        }
        return GameCmd.enumPKMODE.PKMODE_M_NONE;
    }

    public int SetPkMode(GameCmd.enumPKMODE mode)
    {
        if (MainPlayerPkMode == mode)
        {
            return 0;
        }

        if (mode == GameCmd.enumPKMODE.PKMODE_M_TEAM)
        {
            if (DataManager.Manager<TeamDataManager>().IsJoinTeam == false)
            {
                TipsManager.Instance.ShowTips(LocalTextType.PK_Commond_1);
                return 1;
            }
        }
        else if (mode == GameCmd.enumPKMODE.PKMODE_M_FAMILY)
        {
            if (DataManager.Manager<ClanManger>().IsJoinClan == false)
            {
                TipsManager.Instance.ShowTips(LocalTextType.PK_Commond_2);
                return 2;
            }
        }
        else if (mode == GameCmd.enumPKMODE.PKMODE_M_CAMP)
        {

            if (DataManager.Manager<ClanManger>().IsJoinClan)
            {
                if (!DataManager.Manager<ClanManger>().IsClanInDeclareWar())
                {
                    TipsManager.Instance.ShowTips(LocalTextType.PK_Commond_3);
                    return 3;
                }
            }
        }
        GameCmd.stSetPKModePropertyUserCmd_CS cmd = new GameCmd.stSetPKModePropertyUserCmd_CS();
        cmd.pkmode = (uint)mode;
        NetService.Instance.Send(cmd);
        return 0;
    }

    string GetPkModeDes(PLAYERPKMODEL pkmodel)
    {
        return pkmodel.GetEnumDescription();
    }
}
