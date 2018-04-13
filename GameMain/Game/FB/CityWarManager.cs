using Client;
using Engine.Utility;
using GameCmd;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


//class CityWarClan
//{
//    public uint clanId;         //氏族Id
//    public string clanName;       //氏族名称
//}

class CityWarTotem
{
    public uint clanId;
    public uint hp;
    public UnityEngine.Vector2 pos;
    public string iconName;
    public uint npcBaseId;   //图腾的npcId
    public string clanName;
}

class CityWarCityInfo
{
    public uint copyId;
    public string name;
    public string bg;
    public string openTime;         //开启时间
    public string registerTime;     //报名时间

}

class CityWarManager : IManager, ICopy
{

    private const string CONST_COTYWARTOTEMPOS_NAME = "CityWarTotemPos";//图腾位置配置

    private const string CONST_COTYWARTOTEM_NAME = "CityWarTotem";//图腾配置  用于寻找npc;


    private bool m_bEnterCityWar = false;

    public bool EnterCityWar
    {
        get
        {
            return m_bEnterCityWar;
        }
    }

    //华夏城主氏族ID
    private uint m_winerClanId = 0;
    public uint WinerClanId
    {
        get
        {
            return m_winerClanId;
        }
    }

    private List<WinerData> m_lstWinerClanId = new List<WinerData>();


    /// <summary>
    /// 参加城战的所有氏族Id
    /// </summary>
    private List<uint> m_lstCityWarClanId = new List<uint>();
    public List<uint> CityWarClanIdList
    {
        get
        {
            return m_lstCityWarClanId;
        }
    }

    /// <summary>
    /// 参加城战的所有氏族Name
    /// </summary>
    private List<string> m_lstCityWarClanName = new List<string>();
    public List<string> CityWarClanNameList
    {
        get
        {
            return m_lstCityWarClanName;
        }
    }

    private List<uint> m_lstCityWarTotemBaseId = new List<uint>();

    public List<uint> CityWarTotemBaseIdList
    {
        get
        {
            return m_lstCityWarTotemBaseId;
        }
    }

    private List<uint> m_lstCityWarTotemClanId = new List<uint>();

    public List<uint> CityWarTotemClanIdList
    {
        get
        {
            return m_lstCityWarTotemClanId;
        }
    }

    /// <summary>
    /// 击杀敌人数
    /// </summary>
    private uint m_CityWarKillNum = 0;
    public uint CityWarKillNum
    {
        get
        {
            return m_CityWarKillNum;
        }
    }

    /// <summary>
    /// 死亡数
    /// </summary>
    private uint m_CityWarDeathNum = 0;
    public uint CityWarDeathNum
    {
        get
        {
            return m_CityWarDeathNum;
        }
    }

    private uint m_cityWarRank = 0;
    public uint CityWarRank
    {
        get
        {
            return m_cityWarRank;
        }
    }

    private List<CityWarHero> m_lstCityWarHero = new List<CityWarHero>();
    public List<CityWarHero> CityWarHeroList
    {
        get
        {
            return m_lstCityWarHero;
        }
    }

    private List<CityWarTotem> m_lstCityWarTotem = new List<CityWarTotem>();
    public List<CityWarTotem> CityWarTotemList
    {
        get
        {
            return m_lstCityWarTotem;
        }
    }

    /// <summary>
    /// 城战报名氏族
    /// </summary>
    private List<string> m_lstCityWarSignUpClan = new List<string>();
    public List<string> CityWarSignUpClanList
    {
        get
        {
            return m_lstCityWarSignUpClan;
        }
    }

    /// <summary>
    /// 华夏城战副本ID
    /// </summary>
    uint m_cityWarHuaXiaCopyId;
    public uint CityWarHuaXiaCopyId
    {
        get
        {
            return m_cityWarHuaXiaCopyId;
        }
    }

    /// <summary>
    /// 当前选择的城战副本
    /// </summary>
    uint m_cityWarSelectCopyId = 0;
    public uint CityWarSelectCopyId
    {
        set
        {
            m_cityWarSelectCopyId = value;
        }
        get
        {
            return m_cityWarSelectCopyId;
        }
    }

    /// <summary>
    /// 报名的城主氏族名
    /// </summary>
    string m_cityWarRegisterClanName;
    public string CityWarRegisterClanName
    {
        get
        {
            return m_cityWarRegisterClanName;
        }
    }

    List<GameCmd.CityApplyInfo> m_lstCityWarApplyInfo;
    public List<GameCmd.CityApplyInfo> CityWarApplyInfoList
    {
        get
        {
            return m_lstCityWarApplyInfo;
        }
    }

    List<string> m_lstCityWarSign;

    public List<string> CityWarSignList
    {
        get
        {
            return m_lstCityWarSign;
        }
    }

    /// <summary>
    /// 报名的城主名
    /// </summary>
    string m_cityWarRegisterClanLeaderName;
    public string CityWarRegisterClanLeaderName
    {
        get
        {
            return m_cityWarRegisterClanLeaderName;
        }
    }

    bool m_bIsOpen;
    public bool IsOpen
    {
        get
        {
            return m_bIsOpen;
        }
    }

    bool m_haveDefender;
    public bool HaveDefender
    {
        get
        {
            return m_haveDefender;
        }
    }
    /// <summary>
    /// 初始化
    /// </summary>
    public void Initialize()
    {
        Engine.Utility.EventEngine.Instance().AddEventListener((int)GameEventID.ENTITYSYSTEM_CREATEENTITY, DoGameEvent);
        m_cityWarHuaXiaCopyId = GameTableManager.Instance.GetGlobalConfig<uint>("CityWarCopyID");
    }
    public void ClearData()
    {

    }
    /// <summary>
    /// 重置
    /// </summary>
    public void Reset(bool depthClearData = false)
    {
        ResetCityWar();
    }

    /// <summary>
    /// 每帧执行
    /// </summary>
    /// <param name="deltaTime"></param>
    public void Process(float deltaTime)
    {

    }

    //进入副本
    public void EnterCopy()
    {
        m_bEnterCityWar = true;

        stCopyInfo info = new stCopyInfo();
        info.bShow = true;
        info.bShowBattleInfoBtn = true;
        DataManager.Manager<UIPanelManager>().SendMsg(PanelID.MainPanel, UIMsgID.eShowCopyInfo, info);
        DataManager.Manager<ComBatCopyDataManager>().CopyCDAndExitData = new CopyCDAndExitInfo { bShow = true, bShowBattleInfoBtn = true };

        DataManager.Manager<UIPanelManager>().ShowPanel(PanelID.CityWarFightingPanel);
    }

    //退出副本
    public void ExitCopy()
    {
        m_bEnterCityWar = false;

        DataManager.Manager<UIPanelManager>().HidePanel(PanelID.CityWarFightingPanel);
    }


    /// <summary>
    /// 重置城战数据
    /// </summary>
    public void ResetCityWar()
    {
        CityWarClanIdList.Clear();
        CityWarClanNameList.Clear();
        m_lstCityWarHero.Clear();
        m_lstCityWarTotem.Clear();
        m_CityWarKillNum = 0;
        m_CityWarDeathNum = 0;
        m_cityWarRank = 0;
        m_winerClanId = 0;
        m_bEnterCityWar = false;
    }

    void DoGameEvent(int eventID, object param)
    {
        if (eventID == (int)GameEventID.ENTITYSYSTEM_CREATEENTITY)
        {
            if (m_bEnterCityWar == false)
            {
                return;
            }

            Client.stCreateEntity npcEntity = (Client.stCreateEntity)param;

            IEntitySystem es = ClientGlobal.Instance().GetEntitySystem();
            if (es == null)
            {
                return;
            }

            IEntity npc = es.FindEntity(npcEntity.uid);
            if (npc == null)
            {
                return;
            }

            int npcBaseId = npc.GetProp((int)EntityProp.BaseID);

            // 1、设置城战图腾氏族ID
            CityWarTotem cityWarTotem = m_lstCityWarTotem.Find((d) => { return d.npcBaseId == npcBaseId; });
            if (cityWarTotem != null)
            {
                //npc.SetProp((int)Client.CreatureProp.ClanId, (int)cityWarTotem.clanId);

                uint clanId = cityWarTotem.clanId;
                int clanIdLow = (int)(clanId & 0x0000ffff);
                int clanIdHigh = (int)(clanId >> 16);
                npc.SetProp((int)Client.CreatureProp.ClanIdLow, clanIdLow);
                npc.SetProp((int)Client.CreatureProp.ClanIdHigh, clanIdHigh);
            }

            // 2、采集物
            table.NpcDataBase npctable = GameTableManager.Instance.GetTableItem<table.NpcDataBase>((uint)npcBaseId);
            if (npctable != null && npctable.dwType == (uint)GameCmd.enumNpcType.NPC_TYPE_COLLECT_PLANT)//采集物
            {
                CampNpcOnTrigger callback = new CampNpcOnTrigger();
                npc.SetCallback(callback);
            }

            // 城战打印
            //UnityEngine.Debug.LogError("------npctable.dwID = " + npcBaseId);
            //List<CityWarTotem> list = DataManager.Manager<CityWarManager>().CityWarTotemList;
            //for (int i = 0; i < list.Count; i++)
            //{
            //    UnityEngine.Debug.LogError(">>>>>>totemNpcId = " + list[i].totemNpcId + " clanId = " + list[i].clanId);
            //}
        }
    }

    #region Net


    /// <summary>
    /// 城战报名成功
    /// </summary>
    /// <param name="msg"></param>
    public void OnCityWarSignSuccess(stCityWarSignClanUserCmd_S cmd)
    {
        TipsManager.Instance.ShowTips(LocalTextType.CityWar_huaxiachengzhanbaomingchenggong);//华夏城战报名成功
    }

    /// <summary>
    /// 报名列表通知客户端
    /// </summary>
    /// <param name="cmd"></param>
    public void OnCityWarSignInfo(stCityWarSignInfoClanUserCmd_S cmd)
    {
        //城战报名信息
        m_lstCityWarSignUpClan = cmd.clans_name;  //所有氏族名字

        //if (DataManager.Manager<UIPanelManager>().IsShowPanel(PanelID.CityWarMessagePanel))
        //{
        //    DataManager.Manager<UIPanelManager>().SendMsg(PanelID.CityWarMessagePanel, UIMsgID.eCityWarSignUpClan, null);
        //}
    }

    /// <summary>
    /// 城战开始 tips
    /// </summary>
    /// <param name="cmd"></param>
    public void OnCityWarBeginNotice(GameCmd.stCityWarBeginClanUserCmd_CS cmd)
    {
        //同意参加
        Action agree = delegate
        {
            MainPlayStop();

            stCityWarBeginClanUserCmd_CS agreeCmd = new stCityWarBeginClanUserCmd_CS();
            NetService.Instance.Send(agreeCmd);
        };

        Action refuse = delegate
        {

        };

        List<table.CityWarDataBase> cityWarList = GameTableManager.Instance.GetTableList<table.CityWarDataBase>();
        table.CityWarDataBase db = cityWarList.Find((d) => { return d.CopyId == cmd.city_war_id; });

        if (db == null)
        {
            return;
        }

        string des = string.Format("{0}即将开始，是否进入参加", db.Name);
        TipsManager.Instance.ShowTipWindow(0, 15, Client.TipWindowType.CancelOk, des, agree, refuse, title: db.Name);
    }

    void MainPlayStop()
    {
        Client.IPlayer player = Client.ClientGlobal.Instance().MainPlayer;
        if (player != null)
        {
            player.SendMessage(Client.EntityMessage.EntityCommand_StopMove, player.GetPos());
            Engine.Utility.EventEngine.Instance().DispatchEvent((int)Client.GameEventID.ROBOTCOMBAT_SEARCHPATH, false);//关闭自动寻路中
        }
        Controller.CmdManager.Instance().Clear();//清除寻路
    }

    /// <summary>
    /// 城战基本信息 进入副本时下发一次	
    /// </summary>
    /// <param name="cmd"></param>
    public void OnCityWarBaseInfo(GameCmd.stCityWarBaseInfoClanUserCmd_S cmd)
    {
        m_bEnterCityWar = true;//

        m_lstCityWarClanId = cmd.clans_id;
        m_lstCityWarClanName = cmd.clans_name;
        m_lstCityWarTotemBaseId = cmd.towers_base_id;
        m_lstCityWarTotemClanId = cmd.tower_clan_id;

        //图腾
        m_lstCityWarTotem = new List<CityWarTotem>();
        for (int i = 0; i < m_lstCityWarTotemBaseId.Count; i++)
        {
            CityWarTotem cityWarTotem = new CityWarTotem();
            cityWarTotem.npcBaseId = m_lstCityWarTotemBaseId[i];
            cityWarTotem.clanId = m_lstCityWarTotemClanId[i];

            m_lstCityWarTotem.Add(cityWarTotem);

            //设置实体clanID;
            IEntitySystem es = Client.ClientGlobal.Instance().GetEntitySystem();
            if (es != null)
            {
                INPC npc = es.FindNPCByBaseId((int)cityWarTotem.npcBaseId);
                if (npc != null)
                {
                    uint clanId = cityWarTotem.clanId;
                    int clanIdLow = (int)(clanId & 0x0000ffff);
                    int clanIdHigh = (int)(clanId >> 16);
                    npc.SetProp((int)Client.CreatureProp.ClanIdLow, clanIdLow);
                    npc.SetProp((int)Client.CreatureProp.ClanIdHigh, clanIdHigh);

                    //npc.SetProp((int)Client.CreatureProp.ClanId, (int)cityWarTotem.clanId);
                }
            }
        }

        //氏族名称
        for (int i = 0; i < m_lstCityWarTotem.Count; i++)
        {
            if (m_lstCityWarTotem[i].clanId == 0)
            {
                continue;
            }

            int clanIndex = m_lstCityWarClanId.IndexOf(m_lstCityWarTotem[i].clanId);
            string clanName = clanIndex < m_lstCityWarClanName.Count ? m_lstCityWarClanName[clanIndex] : "";
            m_lstCityWarTotem[i].clanName = clanName;
        }

        if (DataManager.Manager<UIPanelManager>().IsShowPanel(PanelID.CityWarFightingPanel))
        {
            DataManager.Manager<UIPanelManager>().SendMsg(PanelID.CityWarFightingPanel, UIMsgID.eCityWarInfoUpdate, null);
        }
    }

    /// <summary>
    /// 自己的击杀信息
    /// </summary>
    /// <param name="cmd"></param>
    public void OnCityWarSelfInfo(stCityWarSelfInfoClanUserCmd_S cmd)
    {
        m_CityWarKillNum = cmd.kill_cnt;
        m_CityWarDeathNum = cmd.die_cnt;

        if (DataManager.Manager<UIPanelManager>().IsShowPanel(PanelID.CityWarPanel))
        {
            DataManager.Manager<UIPanelManager>().SendMsg(PanelID.CityWarPanel, UIMsgID.eCityWarSelfInfoUpdate, null);
        }
    }


    public void ReqCityWarInfo(bool openOrClose)
    {
        stCityWarInfoClanUserCmd_C cmd = new stCityWarInfoClanUserCmd_C();
        cmd.open_or_close = openOrClose;  //true  为打开   false为关闭
        NetService.Instance.Send(cmd);
    }

    /// <summary>
    /// 跟新城战信息
    /// </summary>
    /// <param name="cmd"></param>
    public void OnCityWarInfo(GameCmd.stCityWarInfoClanUserCmd_S cmd)
    {

        m_cityWarRank = cmd.rank;
        m_lstCityWarHero = cmd.heros;

        m_lstCityWarTotem.Clear();
        for (int i = 0; i < cmd.towers.Count; i++)
        {
            CityWarTotem cityWarTotem = new CityWarTotem();

            cityWarTotem.clanId = cmd.towers[i].clan_id;
            cityWarTotem.hp = (uint)cmd.towers[i].hp;
            cityWarTotem.iconName = GetTotemIconName(i);

            //图腾位置
            List<uint> pointXYList = GameTableManager.Instance.GetGlobalConfigList<uint>(CONST_COTYWARTOTEMPOS_NAME, (i + 1).ToString());
            if (pointXYList != null)
            {
                if (pointXYList.Count == 2)
                {
                    cityWarTotem.pos = new UnityEngine.Vector2((float)pointXYList[0], (float)pointXYList[1]);
                }
                else
                {
                    Engine.Utility.Log.Error("全局配置表配置错误！！！！！！");
                }
            }

            //图腾NPC
            List<uint> totemNpcIdList = GameTableManager.Instance.GetGlobalConfigList<uint>(CONST_COTYWARTOTEM_NAME, (i + 1).ToString());
            if (cmd.towers[i].clan_id != 0) //非中立
            {
                int clanIndex = m_lstCityWarClanId.IndexOf(cmd.towers[i].clan_id);  //由第几个阵营 获得当前图腾的npc
                cityWarTotem.npcBaseId = totemNpcIdList[clanIndex];
                string clanName = clanIndex < m_lstCityWarClanName.Count ? m_lstCityWarClanName[clanIndex] : "";
                cityWarTotem.clanName = clanName;
            }
            else     //中立
            {
                cityWarTotem.npcBaseId = totemNpcIdList[totemNpcIdList.Count - 1];//最后一个默认为中立的npc
                cityWarTotem.clanName = string.Empty;
            }

            m_lstCityWarTotem.Add(cityWarTotem);
        }

        if (DataManager.Manager<UIPanelManager>().IsShowPanel(PanelID.CityWarPanel))
        {
            DataManager.Manager<UIPanelManager>().SendMsg(PanelID.CityWarPanel, UIMsgID.eCityWarInfoUpdate, null);
        }

        if (DataManager.Manager<UIPanelManager>().IsShowPanel(PanelID.CityWarFightingPanel))
        {
            DataManager.Manager<UIPanelManager>().SendMsg(PanelID.CityWarFightingPanel, UIMsgID.eCityWarInfoUpdate, null);
        }
    }

    /// <summary>
    /// 城战召集召集氏族
    /// </summary>
    /// <param name="msg"></param>
    public void OCityWarCallClan(stCityWarCallClanClanUserCmd_CS cmd)
    {

    }

    /// <summary>
    /// 城战
    /// </summary>
    /// <param name="msg"></param>
    public void OnCityWarCallTeam(stCityWarCallTeamClanUserCmd_CS cmd)
    {

    }

    /// <summary>
    /// 图腾改变状态时 发客户端
    /// </summary>
    /// <param name="cmd"></param>
    public void OnCityWarTowerChange(stCityWarTowerChangeClanUserCmd_S cmd)
    {
        //图腾NPC
        List<uint> totemNpcIdList = GameTableManager.Instance.GetGlobalConfigList<uint>(CONST_COTYWARTOTEM_NAME, (cmd.tower_idx).ToString());

        if (totemNpcIdList == null)
        {
            return;
        }

        if (cmd.tower_idx - 1 > m_lstCityWarTotem.Count)
        {
            return;
        }

        CityWarTotem cityWarTotem = m_lstCityWarTotem[(int)cmd.tower_idx - 1];

        //图腾氏族ID
        cityWarTotem.clanId = cmd.clan_id;
        //图腾npc ID
        if (cmd.clan_id != 0)
        {
            int clanIndex = m_lstCityWarClanId.IndexOf(cmd.clan_id);
            cityWarTotem.npcBaseId = totemNpcIdList[clanIndex];          //图腾对应的npc
            string clanName = clanIndex < m_lstCityWarClanName.Count ? m_lstCityWarClanName[clanIndex] : "";
            cityWarTotem.clanName = clanName;
        }
        else
        {
            cityWarTotem.npcBaseId = totemNpcIdList[totemNpcIdList.Count - 1];  //最后一个默认为中立的npc
            cityWarTotem.clanName = string.Empty;
        }

        //设置实体clanID;
        IEntitySystem es = Client.ClientGlobal.Instance().GetEntitySystem();
        if (es != null)
        {
            INPC npc = es.FindNPCByBaseId((int)cityWarTotem.npcBaseId);
            if (npc != null)
            {
                uint clanId = cityWarTotem.clanId;
                int clanIdLow = (int)(clanId & 0x0000ffff);
                int clanIdHigh = (int)(clanId >> 16);
                npc.SetProp((int)Client.CreatureProp.ClanIdLow, clanIdLow);
                npc.SetProp((int)Client.CreatureProp.ClanIdHigh, clanIdHigh);

                //npc.SetProp((int)Client.CreatureProp.ClanId, (int)cityWarTotem.clanId);
            }
        }

        EventEngine.Instance().DispatchEvent((int)GameEventID.CITYWARTOTEMCLANNAMECHANGE, cityWarTotem.npcBaseId);
    }

    /// <summary>
    /// 城战胜利者通知 登录时 改变时
    /// </summary>
    /// <param name="msg"></param>
    public void OnCityWarWinerClanId(stCityWarWinerClanUserCmd_S msg)
    {
        this.m_winerClanId = msg.clan_id;

        this.m_lstWinerClanId = msg.data;

        EventEngine.Instance().DispatchEvent((int)GameEventID.CITYWARWINERCLANID, this.m_winerClanId);
    }

    /// <summary>
    /// 通知弹出结算界面  10秒后自动退出
    /// </summary>
    /// <param name="cmd"></param>
    public void OnCityWarFinish(stCityWarFinishClanUserCmd_S cmd)
    {
        if (DataManager.Manager<UIPanelManager>().IsShowPanel(PanelID.CityWarPanel) == false)
        {
            DataManager.Manager<UIPanelManager>().ShowPanel(PanelID.CityWarPanel, data: true);
        }

        // DataManager.Manager<UIPanelManager>().SendMsg(PanelID.CityWarPanel, UIMsgID.eCityWarFinish, null);
    }

    /// <summary>
    /// 请求城池信息
    /// </summary>
    /// <param name="copyId"></param>
    public void ReqCityWarFrameInfo(uint copyId)
    {
        stCityWarFrameInfoClanUserCmd_CS cmd = new stCityWarFrameInfoClanUserCmd_CS();
        cmd.city_war_id = copyId;
        NetService.Instance.Send(cmd);
    }

    /// <summary>
    /// 城池信息
    /// </summary>
    /// <param name="cmd"></param>
    public void OnCityWarFrameInfo(stCityWarFrameInfoClanUserCmd_CS cmd)
    {
        this.m_cityWarSelectCopyId = cmd.city_war_id;
        this.m_cityWarRegisterClanName = cmd.clan_name;
        this.m_cityWarRegisterClanLeaderName = cmd.city_master;
        this.m_lstCityWarApplyInfo = cmd.info;
        this.m_lstCityWarSign = cmd.sign_info;
        this.m_bIsOpen = cmd.have_open_war;
        this.m_haveDefender = cmd.have_defender;

        if (DataManager.Manager<UIPanelManager>().IsShowPanel(PanelID.CityWarRegisterPanel))
        {
            DataManager.Manager<UIPanelManager>().SendMsg(PanelID.CityWarRegisterPanel, UIMsgID.eCityWarFrameInfo, null);
        }
    }

    public void ReqCityWarSubmitStatue(uint num)
    {
        stSubmitStatueClanUserCmd_CS cmd = new stSubmitStatueClanUserCmd_CS();

        cmd.city_war_id = this.CityWarSelectCopyId;
        cmd.nums = num;

        NetService.Instance.Send(cmd);
    }

    /// <summary>
    /// 上缴雕像
    /// </summary>
    /// <param name="cmd"></param>

    public void OnCityWarSubmitStatue(stSubmitStatueClanUserCmd_CS cmd)
    {
        TipsManager.Instance.ShowTips("上交道具成功");

        //if (DataManager.Manager<UIPanelManager>().IsShowPanel(PanelID.CityWarSubmitPanel))
        //{
        //    DataManager.Manager<UIPanelManager>().SendMsg(PanelID.CityWarSubmitPanel, UIMsgID.eCityWarSubmitStatue,null);
        //}
    }

    public void ReqOpenSubmitPanel()
    {
        stOpenSubmitWindowClanUserCmd_CS cmd = new stOpenSubmitWindowClanUserCmd_CS();
        cmd.city_war_id = this.m_cityWarSelectCopyId;
        NetService.Instance.Send(cmd);
    }

    public void OnOpenSubmitPanel(stOpenSubmitWindowClanUserCmd_CS cmd)
    {
        uint itemBaseId = GameTableManager.Instance.GetGlobalConfig<uint>("CityWarStatue");
        DataManager.Manager<UIPanelManager>().ShowPanel(PanelID.CityWarSubmitPanel, data: itemBaseId);
    }

    #endregion

    /// <summary>
    /// 获取图腾氏族名称
    /// </summary>
    /// <param name="npcBaseId">图腾npcId</param>
    /// <param name="clanName"></param>
    /// <returns></returns>
    public bool GetCityWarTotemClanName(uint npcBaseId, out CityWarTotem cityWarTotemInfo)
    {
        cityWarTotemInfo = null;
        if (m_bEnterCityWar == false)
        {
            return false;
        }

        CityWarTotem cityWarTotem = m_lstCityWarTotem.Find((data) => { return data.npcBaseId == npcBaseId; });
        if (cityWarTotem != null)
        {
            if (cityWarTotem.clanId == 0)
            {
                return false;
            }

            cityWarTotemInfo = cityWarTotem;

            return true;
        }
        return false;
    }



    /// <summary>
    /// 获取
    /// </summary>
    /// <param name="clanId">传入的氏族ID </param>
    /// <param name="winerCityName">氏族后缀 华夏城</param>
    /// <returns></returns>
    public bool GetWinerOfCityWarCityName(uint clanId, out string winerCityName)
    {
        winerCityName = string.Empty;

        if (clanId == 0)
        {
            return false;
        }

        WinerData wd = m_lstWinerClanId.Find((d) => { return d.clan_id == clanId; });
        if (wd == null)
        {
            return false;
        }

        //城池名称
        table.CityWarDataBase cwdb = GameTableManager.Instance.GetTableItem<table.CityWarDataBase>(wd.copy_id);
        if (cwdb == null)
        {
            return false;
        }

        winerCityName = string.Format(cwdb.CityName + "·");
        return true;
    }


    /// <summary>
    /// 城 金 木 水 火 土
    /// </summary>
    /// <param name="index"></param>
    /// <returns></returns>
    string GetTotemIconName(int index)
    {
        string iconName = string.Empty;
        switch (index)
        {
            case 0: iconName = "tubiao_cheng"; break;
            case 1: iconName = "tubiao_jinqi"; break;
            case 2: iconName = "tubiao_mu"; break;
            case 3: iconName = "tubiao_shui"; break;
            case 4: iconName = "tubiao_huo"; break;
            case 5: iconName = "tubiao_tu"; break;
        }

        return iconName;
    }

    /// <summary>
    /// 除华夏城的城战
    /// </summary>
    /// <returns></returns>
    public List<table.CityWarDataBase> GetCityWarCopyListWithoutHuaXia()
    {
        List<table.CityWarDataBase> list = GameTableManager.Instance.GetTableList<table.CityWarDataBase>();
        return list.FindAll((d) => { return d.CopyId != m_cityWarHuaXiaCopyId; });
    }

}
