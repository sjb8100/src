//********************************************************************
//	创建日期:	2016-10-19   17:57
//	文件名称:	RankManager.cs
//  创 建 人:   刘超	
//	版权所有:	中青宝
//	说    明:	排行榜数据管理
//********************************************************************
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common;
using UnityEngine;
using GameCmd;
using table;


public class RankManager : BaseModuleData,IManager
{
    public void ClearData()
    {

    }
    public void Initialize()
    {
    //    Engine.Utility.EventEngine.Instance().AddEventListener((int)Client.GameEventID.PLAYER_LOGIN_SUCCESS, OnPlayerLoginSuccess);
    }

    public OrderListType orderType = OrderListType.OrderListType_None;
    public OrderListType OrderType
    {
        get { return orderType; }
        set {  orderType=value; }
    }

  public List<GameCmd.stAnswerOrderListRelationUserCmd_S.Data> m_RankList = new List<GameCmd.stAnswerOrderListRelationUserCmd_S.Data>() ;
  public List<GameCmd.stAnswerOrderListRelationUserCmd_S.Data> M_RankList
    {
        get { return m_RankList; }
        set { m_RankList = value; }
    }

  public uint self_rank = 0;
  public uint SelfRank
   {
              get { return self_rank; }
              set { self_rank = value; }
    }

  public string self_name = "";
  public string SelfName
  {
      get { return self_name; }
      set { self_name = value; }
  }

  public string self_type  = "";
  public string SelfType
  {
      get { return self_type; }
      set { self_type = value; }
  }
  public string self_clan = "";
  public string SelfClan
  {
      get { return self_clan; }
      set { self_clan = value; }
  }
  public uint self_yiju = 0;
  public uint SelfYiju
  {
      get { return self_yiju; }
      set { self_yiju = value; }
  }

  public bool isRobot = false;
  public bool IsRobot 
  {
      get { return isRobot; }
      set { isRobot = value; }
  }
    public void SetRankData(GameCmd.stAnswerOrderListRelationUserCmd_S cmd) 
    {
        OrderType = cmd.type;
        SelfName = cmd.self_name;
        SelfRank = cmd.self_rank;
        SelfType = cmd.self_type;
        SelfClan = cmd.self_clan;
        SelfYiju = cmd.self_yiju;     
        M_RankList= cmd.data;
        Client.stRankType rt = new Client.stRankType();
        rt.rankId = (uint)OrderType;
        Engine.Utility.EventEngine.Instance().DispatchEvent((int)Client.GameEventID.RANKDATAREFRESH, rt);
    }

    Dictionary<uint, List<RankTypeDataBase>> rankDic = new Dictionary<uint, List<RankTypeDataBase>>();
    public Dictionary<uint, List<RankTypeDataBase>> GetRankDataByFirstType() 
    {
        List<RankTypeDataBase> rankTypes = GameTableManager.Instance.GetTableList<RankTypeDataBase>();
        foreach (var t in rankTypes)
        {
            if (rankDic.ContainsKey(t.mainID))
            {
                rankDic[t.mainID].Add(t);
            }
            else 
            {
                List<RankTypeDataBase> temp = new List<RankTypeDataBase>();
                temp.Add(t);
                rankDic.Add(t.mainID, temp);
            }
        }
        return rankDic;
    }



    #region  
    public void Reset(bool depthClearData = false) 
   {
       orderType = OrderListType.OrderListType_None;
       m_RankList.Clear();
       self_rank = 0;
       self_name = "";
       self_type = "";
       self_clan = "";
       self_yiju = 0;
       isRobot = false;
   
   
   }
   public void Process(float deltime) 
   { }

    #endregion
}
