using Client;
using GameCmd;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using table;

class HeartSkillManager : BaseModuleData, IManager
{
    #region property

    /// <summary>
    /// 心法点
    /// </summary>
    private uint m_heartSkillPoint = 0;

    public uint HeartSkillPoint
    {
        get
        {
            return m_heartSkillPoint;
        }
    }

    /// <summary>
    /// 神魔等级
    /// </summary>
    private uint m_ghostLv = 0;
    public uint GhostLv
    {
        get
        {
            return m_ghostLv;
        }
    }

    /// <summary>
    /// 神魔经验
    /// </summary>
    private uint m_ghostExp = 0;
    public uint GhostExp
    {
        get
        {
            return m_ghostExp;
        }
    }

    /// <summary>
    /// 剩余免费重置次数
    /// </summary>
    private uint m_freeReset = 0;
    public uint FreeReset
    {
        get
        {
            return m_freeReset;
        }
    }

    /// <summary>
    /// 已经拥有的心法
    /// </summary>
    List<GameCmd.HeartSkill> m_ownedHeartSkillList = new List<GameCmd.HeartSkill>();

    public List<GameCmd.HeartSkill> OwnedHeartSkillList
    {
        get
        {
            return m_ownedHeartSkillList;
        }
    }
    #endregion


    #region IManager
    public void Initialize()
    {

    }
    public void Reset(bool depthClearData = false)
    {
        CleanData();
    }

    public void Process(float deltaTime)
    {

    }
    public void ClearData()
    {

    }
    #endregion


    #region method

    /// <summary>
    /// 清除心法数据
    /// </summary>
    void CleanData()
    {
        m_heartSkillPoint = 0;
        m_ghostLv = 0;
        m_ghostExp = 0;
        m_freeReset = 0;
        m_ownedHeartSkillList.Clear();
    }


    public List<GameCmd.HeartSkill> GetHeartSkillList()
    {
        List<HeartSkill> heartSkillList = new List<HeartSkill>();
        List<uint> list = GetHeartSkillIdList();
        for (int i = 0; i < list.Count; i++)
        {
            //此处要判断， 综合服务器数据和客户端数据
            GameCmd.HeartSkill heartSkill = m_ownedHeartSkillList.Find((data) => { return list[i] == data.skill_id; });

            if (heartSkill != null)
            {
                heartSkillList.Add(heartSkill);            //服务器获得的,已经解锁的
            }
            else
            {
                HeartSkillDataBase db = GameTableManager.Instance.GetTableItem<HeartSkillDataBase>(list[i], 0); //还未解锁的
                heartSkillList.Add(new HeartSkill { skill_id = list[i], level = db.lv });
            }
        }
        return heartSkillList;
    }

    /// <summary>
    /// 获得玩家的心法ID list
    /// </summary>
    /// <returns></returns>
    List<uint> GetHeartSkillIdList()
    {
        //int lvLimit = GameTableManager.Instance.GetClientGlobalConst<int>("HeartSkill", "HeartSkillUnlockLv");
        int lvLimit = GameTableManager.Instance.GetGlobalConfig<int>("GodOpenLevel"); //100级开放心法
        Client.IPlayer mainPlayer = Client.ClientGlobal.Instance().MainPlayer;
        int playerLv = mainPlayer.GetProp((int)Client.CreatureProp.Level);
        int playerJob = mainPlayer.GetProp((int)Client.PlayerProp.Job);

        List<HeartSkillDataBase> heartSkillList = GameTableManager.Instance.GetTableList<HeartSkillDataBase>();
        heartSkillList = heartSkillList.FindAll((d) => { return d.profession == playerJob; });//职业

        heartSkillList.Sort((x, y) => x.orderId.CompareTo(y.orderId));

        //heartSkillList = new List<HeartSkillDataBase>(orderList);

        List<uint> idList = new List<uint>();
        if (playerLv >= lvLimit)
        {
            for (int i = 0; i < heartSkillList.Count; i++)
            {
                if (idList.Contains(heartSkillList[i].id) == false)
                {
                    idList.Add(heartSkillList[i].id);
                    //Engine.Utility.Log.LogGroup(GameDefine.LogGroup.User_LCY, "--->>> 顺序为： {0}", heartSkillList[i].orderId);
                }
            }
        }

        return idList;
    }

    /// <summary>
    /// 获得前置技能list
    /// </summary>
    /// <param name="preSkill"></param>
    /// <returns></returns>
    public List<HeartSkill> GetPreHeartSkill(string preSkill)
    {
        List<HeartSkill> list = new List<HeartSkill>();
        bool b1 = preSkill.Contains(";");
        bool b2 = preSkill.Contains("_");

        string[] arr1 = preSkill.Split(';');

        for (int i = 0; i < arr1.Length; i++)
        {
            string[] arr2 = arr1[i].Split('_');

            if (arr2.Length != 2)
            {
                return list;
            }

            uint skillId;
            uint lv;
            if (!uint.TryParse(arr2[0], out skillId))
            {
                Engine.Utility.Log.Error("--->>> 表格数据出错 ,没取到数据！");
            }
            if (!uint.TryParse(arr2[1], out lv))
            {
                Engine.Utility.Log.Error("--->>> 表格数据出错 ,没取到数据！");
            }

            HeartSkill hs = new HeartSkill { skill_id = skillId, level = lv };
            list.Add(hs);
        }

        return list;
    }

    /// <summary>
    /// 心法是否可升级
    /// </summary>
    public bool IsEnableUpgrade(HeartSkill heartSkill)
    {
        bool isEnableUpgrade = true;

        //下一等级心法数据
        HeartSkillDataBase nextDb = GameTableManager.Instance.GetTableItem<HeartSkillDataBase>(heartSkill.skill_id, (int)heartSkill.level + 1);
        if (nextDb == null)
        {
            isEnableUpgrade = false;
            return isEnableUpgrade;
        }

        //神魔等级
        IPlayer player = Client.ClientGlobal.Instance().MainPlayer;
        if (player != null)
        {
            if (player.GetProp((int)CreatureProp.Level) < nextDb.needPlayerLv)
            {
                isEnableUpgrade = false;
            }
        }


        //心法点
        if (m_heartSkillPoint < nextDb.costHeartSkillPoint)
        {
            isEnableUpgrade = false;
        }

        //前置技能
        List<HeartSkill> preSkillList = GetPreHeartSkill(nextDb.pre_skill);

        for (int i = 0; i < preSkillList.Count; i++)
        {
            if (false == m_ownedHeartSkillList.Exists((data) => { return data.skill_id == preSkillList[i].skill_id && data.level == preSkillList[i].level; }))
            {
                isEnableUpgrade = false;
            }
        }

        //消耗金币
        if (Client.ClientGlobal.Instance().MainPlayer.GetProp((int)Client.PlayerProp.Coupon) < nextDb.need_money)   //金币不足 字体红色
        {
            isEnableUpgrade = false;
        }

        //是否最高等级
        HeartSkillDataBase db = GameTableManager.Instance.GetTableItem<HeartSkillDataBase>(heartSkill.skill_id, (int)heartSkill.level);
        if (db != null)
        {
            if (heartSkill.level == db.maxLv)
            {
                isEnableUpgrade = false;
            }
        }

        return isEnableUpgrade;
    }

    /// <summary>
    /// 玩家有心法可以升级
    /// </summary>
    /// <returns></returns>
    public bool HaveHeartSkillEnableUpgrade()
    {
        //未到开放等级
        if (false == IsHeartSkillOpen())
        {
            return false;
        }

        bool isEnableUpgrade = false;

        List<GameCmd.HeartSkill> playerHeartSkillList = GetHeartSkillList();

        for (int i = 0; i < playerHeartSkillList.Count; i++)
        {
            if (IsEnableUpgrade(playerHeartSkillList[i]))
            {
                isEnableUpgrade = true;
            }
        }

        return isEnableUpgrade;
    }

    /// <summary>
    /// 根据玩家等级判断是否开放心法
    /// </summary>
    /// <returns></returns>
    public bool IsHeartSkillOpen()
    {
        int lvLimit = GameTableManager.Instance.GetGlobalConfig<int>("GodOpenLevel");
        Client.IPlayer mainPlayer = Client.ClientGlobal.Instance().MainPlayer;
        int playerLv = mainPlayer.GetProp((int)Client.CreatureProp.Level);
        if (playerLv >= lvLimit)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    #endregion


    #region net

    /// <summary>
    /// 心法list
    /// </summary>
    /// <param name="cmd"></param>
    public void OnHeartSkillList(stSendGodDataUserCmd_S cmd)
    {
        m_ghostLv = cmd.level;     //神魔等级
        m_ghostExp = cmd.exp;      //神魔经验
        m_heartSkillPoint = cmd.point; //心法点
        m_freeReset = cmd.free_reset;  //

        m_ownedHeartSkillList = cmd.skill;
    }

    /// <summary>
    /// 刷新心法数据
    /// </summary>
    /// <param name="cmd"></param>
    public void OnRefreshHeartSkillData(stRefreshGodDataUserCmd_S cmd)
    {
        m_ghostLv = cmd.level;     //神魔等级
        m_ghostExp = cmd.exp;      //神魔经验
        m_heartSkillPoint = cmd.point; //心法点
        m_freeReset = cmd.free_reset;

        Engine.Utility.EventEngine.Instance().DispatchEvent((int)Client.GameEventID.HEARTSKILLGODDATA, null);
    }

    /// <summary>
    /// 升级心法数据
    /// </summary>
    /// <param name="cmd"></param>

    public void ReqUpgradeHeartSkill(uint id)
    {
        stUpSkillGodDataUserCmd_CS cmd = new stUpSkillGodDataUserCmd_CS();
        cmd.skill_id = id;
        NetService.Instance.Send(cmd);
    }

    /// <summary>
    /// 升级心法数据
    /// </summary>
    /// <param name="cmd"></param>
    public void OnUpgradeHeartSkill(stUpSkillGodDataUserCmd_CS cmd)
    {
        HeartSkill heartSkill = m_ownedHeartSkillList.Find((data) => { return data.skill_id == cmd.skill_id; });
        if (heartSkill != null)
        {
            heartSkill.level = cmd.level;
        }
        else
        {
            heartSkill = new HeartSkill { skill_id = cmd.skill_id, level = cmd.level };
            m_ownedHeartSkillList.Add(heartSkill);
        }

        Engine.Utility.EventEngine.Instance().DispatchEvent((int)Client.GameEventID.HEARTSKILLUPGRADE, heartSkill);
    }

    /// <summary>
    /// 重置所有心法
    /// </summary>
    /// <param name="cmd"></param>
    public void ReqResetHeartSkill(uint itemId)
    {
        stResetSkillGodDataUserCmd_CS cmd = new stResetSkillGodDataUserCmd_CS();
        cmd.item_id = itemId;
        //cmd.auto_buy = autoBuy;

        NetService.Instance.Send(cmd);
    }

    /// <summary>
    /// 重置心法
    /// </summary>
    /// <param name="cmd"></param>
    public void OnResetHeartSkill(stResetSkillGodDataUserCmd_CS cmd)
    {
        //成功重置
        m_ownedHeartSkillList.Clear();

        this.m_freeReset = cmd.free_reset;

        Engine.Utility.EventEngine.Instance().DispatchEvent((int)Client.GameEventID.HEARTSKILLRESET, null);
    }

    /// <summary>
    /// 增加心法点
    /// </summary>
    public void ReqAddHeartSkillPoint(uint itemId, uint num)
    {
        stAddPointUseItemGodDataUserCmd_CS cmd = new stAddPointUseItemGodDataUserCmd_CS();
        cmd.item_id = itemId;
        // cmd.auto_buy = autoBuy;
        cmd.item_num = num;
        NetService.Instance.Send(cmd);
    }

    /// <summary>
    /// 增加心法点返回
    /// </summary>
    public void OnAddHeartSkillPoint(stAddPointUseItemGodDataUserCmd_CS cmd)
    {
        //TipsManager.Instance.ShowTips("");
    }

    #endregion

}

