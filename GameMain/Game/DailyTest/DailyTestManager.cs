using Client;
using GameCmd;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using table;


public class DailyTestInfo
{
    public uint id;             //id   （是地图ID）
    public string name;         //名称
    public uint expMultiple;    //经验倍数
    public uint lvMin;          //怪物等级下限
    public uint lvMax;          //怪物等级上限
    public bool isRecommend;    //是否推荐
    public uint bgId;
}

class DailyTestManager : BaseModuleData, IManager
{
    #region global
    /// <summary>
    /// 福瑞珠itemID 
    /// </summary>
    public uint FuRuiZhuItemId
    {
        get
        {
            return GameTableManager.Instance.GetGlobalConfig<uint>("FuRuiZhuItemId");
        }
    }

    /// <summary>
    /// 高倍经验的怪物总数
    /// </summary>
    public uint TotleMonsterNum
    {
        get
        {
            return GameTableManager.Instance.GetGlobalConfig<uint>("TotleMonsterNum");
        }
    }

    #endregion


    #region property

    /// <summary>
    /// 剩余高倍经验怪物数量
    /// </summary>
    uint m_monsterNum;

    public uint MonsterNum
    {
        get
        {
            return m_monsterNum;
        }
    }

    /// <summary>
    /// 当天可使用福瑞珠数量
    /// </summary>
    uint m_dailyCanUseFuRuiZhuNum;
    public uint DailyCanUseFuRuiZhuNum
    {
        get
        {
            return m_dailyCanUseFuRuiZhuNum;
        }
    }

    /// <summary>
    /// 福瑞珠加速数量
    /// </summary>
    uint m_fuRuiZhuSpeedupNum;
    public uint FuRuiZhuSpeedupNum
    {
        get
        {
            return m_fuRuiZhuSpeedupNum;
        }
    }

    List<DailyTestInfo> m_lstDailyTestInfo = new List<DailyTestInfo>();

    #endregion

    #region interface
    /// <summary>
    /// 初始化
    /// </summary>
    public void Initialize()
    {

    }
    /// <summary>
    /// 重置
    /// </summary>
    /// <param name="depthClearData">是否深度清除管理器数据</param>
    public void Reset(bool depthClearData = false)
    {

    }
    /// <summary>
    /// 每帧执行
    /// </summary>
    /// <param name="deltaTime"></param>
    public void Process(float deltaTime)
    {

    }
    /// <summary>
    /// 清理数据
    /// </summary>
    public void ClearData()
    {

    }

    #endregion

    #region method

    public List<DailyTestInfo> GetDailyTestInfoList()
    {
        m_lstDailyTestInfo.Clear();

        //有多个推荐  选取经验倍数最大的推荐
        uint tempId = 0;
        uint tempExpMultiple = 0;
        uint tempRecommendCount = 0;

        List<DailyTestDataBase> m_lstDailyTestDb = GameTableManager.Instance.GetTableList<DailyTestDataBase>();
        for (int i = 0; i < m_lstDailyTestDb.Count; i++)
        {
            DailyTestInfo dailyTestInfo = new DailyTestInfo();
            dailyTestInfo.id = m_lstDailyTestDb[i].dwID;
            dailyTestInfo.name = m_lstDailyTestDb[i].strName;
            dailyTestInfo.expMultiple = m_lstDailyTestDb[i].expMultiple;
            dailyTestInfo.lvMin = m_lstDailyTestDb[i].monsterLvMin;
            dailyTestInfo.lvMax = m_lstDailyTestDb[i].monsterLvMax;
            dailyTestInfo.bgId = m_lstDailyTestDb[i].BgTextureId;

            bool bIsRecommend = IsRecommend(m_lstDailyTestDb[i].monsterLvMin, m_lstDailyTestDb[i].monsterLvMax);
            dailyTestInfo.isRecommend = bIsRecommend;
            if (true == bIsRecommend)
            {
                if (tempExpMultiple < m_lstDailyTestDb[i].expMultiple)
                {
                    tempId = m_lstDailyTestDb[i].dwID;
                    tempExpMultiple = m_lstDailyTestDb[i].expMultiple;
                }

                tempRecommendCount++;
            }

            m_lstDailyTestInfo.Add(dailyTestInfo);
        }

        //有多个推荐  选取经验倍数最大的推荐
        if (tempRecommendCount >= 2)
        {
            for (int i = 0; i < m_lstDailyTestInfo.Count; i++)
            {
                if (tempId == m_lstDailyTestInfo[i].id)
                {
                    m_lstDailyTestInfo[i].isRecommend = true;
                }
                else
                {
                    m_lstDailyTestInfo[i].isRecommend = false;
                }
            }
        }

        m_lstDailyTestInfo.Sort(new DailyTestCompare<DailyTestInfo>());

        return m_lstDailyTestInfo;
    }

    public bool IsRecommend(uint lvMin, uint lvMax)
    {
        Client.IPlayer mainPlayer = Client.ClientGlobal.Instance().MainPlayer;
        if (mainPlayer != null)
        {
            uint lv = (uint)mainPlayer.GetProp((int)CreatureProp.Level);
            if (lv >= lvMin && lv <= lvMax)
            {
                return true;
            }
        }

        return false;
    }



    #endregion


    #region Net

    public void ReqFuruizhuInfo()
    {
        GameCmd.stfuruizhuinfoPropertyUserCmd_CS cmd = new GameCmd.stfuruizhuinfoPropertyUserCmd_CS();

        NetService.Instance.Send(cmd);
    }

    /// <summary>
    /// 每日试炼刷新
    /// </summary>
    /// <param name="cmd"></param>
    public void OnFuruizhuInfo(GameCmd.stfuruizhuinfoPropertyUserCmd_CS cmd)
    {
        this.m_monsterNum = cmd.npc_num;
        this.m_dailyCanUseFuRuiZhuNum = cmd.remain_num;
        this.m_fuRuiZhuSpeedupNum = cmd.double_num;

        //每日试炼刷新
        if (DataManager.Manager<UIPanelManager>().IsShowPanel(PanelID.DailyTestPanel))
        {
            DataManager.Manager<UIPanelManager>().SendMsg(PanelID.DailyTestPanel, UIMsgID.eUpdateDailyTest, null);
        }
    }

    #endregion


}


public class DailyTestCompare<T> : IComparer<T>
{
    public int Compare(T x, T y)
    {
        DailyTestInfo left = x as DailyTestInfo;
        DailyTestInfo right = y as DailyTestInfo;
        if (left != null && right != null)
        {
            if (left.isRecommend == true || right.isRecommend == true)
            {
                return right.isRecommend == true ? 1 : -1;
            }
            else if (left.id != right.id)
            {
                return (int)left.id - (int)right.id;
            }
            return 0;
        }

        return 0;
    }
}
