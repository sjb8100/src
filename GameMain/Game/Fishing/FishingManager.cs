using Client;
using Engine.Utility;
using GameCmd;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using table;
using UnityEngine;

/// <summary>
/// 钓鱼排行信息
/// </summary>
public class FishingRankInfo
{
    public uint rank;

    public string name;

    public string clanName;

    public uint num;

    public uint score;
}

class FishingManager : BaseModuleData, IManager
{
    #region property

    ///// <summary>
    ///// 是否在钓鱼区
    ///// </summary>
    //bool m_bIsInFinshingArea = false;

    //public bool IsInFinshingArea
    //{
    //    get
    //    {
    //        return m_bIsInFinshingArea;
    //    }
    //}

    /// <summary>
    /// 钓鱼的地图id
    /// </summary>
    private uint m_fishingMapId;
    private uint m_fishingDailyId;

    /// <summary>
    /// 是否在钓鱼
    /// </summary>
    bool m_bIsFishing = false;

    public bool IsFishing
    {
        get
        {
            return m_bIsFishing;
        }
    }

    /// <summary>
    /// 当前钓上来的鱼的fish Id
    /// </summary>
    uint m_fishId = 0;
    public uint FishId
    {
        get
        {
            return m_fishId;
        }
    }

    /// <summary>
    /// 上鱼时间(出现上鱼转盘)
    /// </summary>
    float m_fishingUpTime;

    public float FishingUpTime
    {
        set
        {
            m_fishingUpTime = value;
        }
        get
        {
            return m_fishingUpTime;
        }
    }

    /// <summary>
    /// 上鱼 fishId
    /// </summary>
    uint m_fishingUpFishId;
    public uint FishingUpFishId
    {
        get
        {
            return m_fishingUpFishId;
        }
    }

    /// <summary>
    /// 上鱼后的倒计时
    /// </summary>
    float m_fishingUpCd = 0f;
    public float FishingUpCd
    {
        set
        {
            m_fishingUpCd = value;
        }
        get
        {
            return m_fishingUpCd;
        }
    }

    /// <summary>
    /// 开始钓鱼时间
    /// </summary>
    float m_fishingTime = 0f;
    public float FishingTime
    {
        set
        {
            m_fishingTime = value;
        }
        get
        {
            return m_fishingTime;
        }
    }

    /// <summary>
    /// 排行
    /// </summary>
    uint m_fishingRank;
    public uint FishingRank
    {
        get
        {
            return m_fishingRank;
        }
    }

    /// <summary>
    /// 积分
    /// </summary>
    uint m_fishingScore;
    public uint FishingScore
    {
        get
        {
            return m_fishingScore;
        }
    }

    /// <summary>
    /// 钓鱼排行信息
    /// </summary>
    List<FishingRankInfo> m_fishingRankInfoList = new List<FishingRankInfo>();
    public List<FishingRankInfo> FishingRankInfoList
    {
        set
        {
            m_fishingRankInfoList = value;
        }
        get
        {
            return m_fishingRankInfoList;
        }
    }

    IEnumerator m_nextFishingCoroutine = null;
    #endregion


    #region interface
    public void ClearData()
    {
        this.m_bIsFishing = false;
        if (DataManager.Manager<UIPanelManager>().IsShowPanel(PanelID.FishingPanel))
        {
            DataManager.Manager<UIPanelManager>().HidePanel(PanelID.FishingPanel);
        }
    }


    /// <summary>
    /// 初始化
    /// </summary>
    public void Initialize()
    {
        m_fishingMapId = GameTableManager.Instance.GetGlobalConfig<uint>("FS_MapID");
        m_fishingDailyId = GameTableManager.Instance.GetGlobalConfig<uint>("FS_DailyId");
        RegistEvent(true);
    }


    /// <summary>
    /// 重置
    /// </summary>
    public void Reset(bool depthClearData = false)
    {

    }

    /// <summary>
    /// 每帧执行
    /// </summary>
    /// <param name="deltaTime"></param>
    public void Process(float deltaTime)
    {
        if (false == m_bIsFishing)
        {
            return;
        }

        //鱼上钩了
        if (Time.realtimeSinceStartup > m_fishingUpTime && Time.realtimeSinceStartup < m_fishingUpTime + m_fishingUpCd)
        {
            //钓鱼倒计时清0
            m_fishingTime = 0;
        }
    }

    #endregion


    #region event
    void RegistEvent(bool b)
    {
        if (b)
        {
            Engine.Utility.EventEngine.Instance().AddEventListener((int)GameEventID.ENTITYSYSTEM_CHANGEAREA, OnEvent);
            Engine.Utility.EventEngine.Instance().AddEventListener((int)GameEventID.SYSTEM_LOADSCENECOMPELETE, OnEvent);
        }
        else
        {
            Engine.Utility.EventEngine.Instance().RemoveEventListener((int)GameEventID.ENTITYSYSTEM_CHANGEAREA, OnEvent);
            Engine.Utility.EventEngine.Instance().RemoveEventListener((int)GameEventID.SYSTEM_LOADSCENECOMPELETE, OnEvent);
        }
    }

    private void OnEvent(int nEventID, object param)
    {
        if (nEventID == (int)GameEventID.ENTITYSYSTEM_CHANGEAREA || nEventID == (int)GameEventID.SYSTEM_LOADSCENECOMPELETE)
        {
            //地图区域判断
            if (nEventID == (int)GameEventID.ENTITYSYSTEM_CHANGEAREA)
            {
                stEntityChangeArea changeArea = (stEntityChangeArea)param;

                bool isMainPlayer = Client.ClientGlobal.Instance().IsMainPlayer(changeArea.uid);

                if (isMainPlayer)
                {
                    //初始化
                    InitFishing();

                    //if (changeArea.eType == MapAreaType.Fish)
                    //{
                    //    //  Engine.Utility.Log.Error("到达钓鱼区域了！！！");

                    //    m_bIsInFinshingArea = true;
                    //}
                    //else
                    //{
                    //    //Engine.Utility.Log.Error("离开钓鱼区域了！！！");

                    //    m_bIsInFinshingArea = false;
                    //}
                }
            }

            else if (nEventID == (int)GameEventID.SYSTEM_LOADSCENECOMPELETE)
            {
                Client.stLoadSceneComplete loadScene = (Client.stLoadSceneComplete)param;

                uint mapID = (uint)loadScene.nMapID;

                if (mapID == m_fishingMapId)
                {
                    //初始化
                    InitFishing();
                }
                //IMapSystem ms = Client.ClientGlobal.Instance().GetMapSystem();
                //if (ms == null)
                //{
                //    return;
                //}

                //IPlayer mainPlayer = Client.ClientGlobal.Instance().MainPlayer;
                //if (mainPlayer == null)
                //{
                //    return;
                //}

                //if (ms.GetAreaTypeByPos(mainPlayer.GetPos()) == MapAreaType.Fish)
                //{
                //    Engine.Utility.Log.Error("到达钓鱼区域了！！！");
                //    m_bIsInFinshingArea = true;
                //}
                //else
                //{
                //    m_bIsInFinshingArea = false;
                //}
                //}
                //else    //不在钓鱼地图
                //{
                //    m_bIsInFinshingArea = false;

            }



        }
    }

    void InitFishing()
    {
        if (DataManager.Manager<UIPanelManager>().IsShowPanel(PanelID.MainPanel))
        {
            MainPanel mainPanel = DataManager.Manager<UIPanelManager>().GetPanel<MainPanel>(PanelID.MainPanel);
            mainPanel.InitFishingUI();
        }

        if (CanFishing())
        {

        }
        else
        {
            if (true == m_bIsFishing)
            {
                ReqCloseFishing();
            }
        }
    }



    #endregion


    #region method

    /// <summary>
    /// 是否在钓鱼区域
    /// </summary>
    /// <returns></returns>
    public bool IsInFishingArea()
    {
        IMapSystem ms = Client.ClientGlobal.Instance().GetMapSystem();
        if (ms == null)
        {
            return false;
        }

        IPlayer mainPlayer = Client.ClientGlobal.Instance().MainPlayer;
        if (mainPlayer == null)
        {
            return false;
        }

        return ms.GetAreaTypeByPos(mainPlayer.GetPos()) == MapAreaType.Fish;



    }

    /// <summary>
    /// 是否在钓鱼日程内
    /// </summary>
    /// <returns></returns>
    public bool IsInFishingSchedule()
    {
        long leftTime;
        return DataManager.Manager<DailyManager>().IsTimeInSchedule(m_fishingDailyId, DateTimeHelper.Instance.Now, out leftTime);
    }

    /// <summary>
    /// 是否可以钓鱼
    /// </summary>
    /// <returns></returns>
    public bool CanFishing()
    {
        //return m_bIsInFinshingArea && IsInFishingSchedule();
        return IsInFishingArea() && IsInFishingSchedule();
    }


    /// <summary>
    /// 抛竿
    /// </summary>
    public void StartFishing(uint uid)
    {
        //播放钓鱼动画
        if (m_bIsFishing)
        {
            //钓鱼引导倒计时
            uint m_fishingCD = GameTableManager.Instance.GetGlobalConfig<uint>("FS_CD");
            m_fishingTime = Time.realtimeSinceStartup + m_fishingCD;  //加10s
        }
    }

    public void PlayStartFishingAni(uint uid)
    {
        //抛竿动画
        PlayAnimation(uid, EntityAction.FishingStart, 1);

        //延时0.6s钓鱼动画
        Engine.CorotinueInstance.Instance.StartCoroutine(DelayPlayFishingAni(uid));
    }

    public void PlayAnimation(uint uid, string entityAction, int loop)
    {
        DataManager.Manager<RideManager>().TryUnRide((obj) =>
        {
            PlayAni(uid, entityAction, loop);
        },
        null);
    }

    public void PlayAni(uint uid, string entityAction, int loop)
    {
        IEntitySystem es = ClientGlobal.Instance().GetEntitySystem();
        if (es == null)
        {
            return;
        }

        IPlayer player = es.FindPlayer(uid);
        if (player == null)
        {
            return;
        }

        //处理
        PlayAni anim_param = new PlayAni();
        anim_param.strAcionName = entityAction;
        anim_param.fSpeed = 1;
        anim_param.nStartFrame = 0;
        anim_param.nLoop = loop;
        anim_param.fBlendTime = 0.2f;
        player.SendMessage(EntityMessage.EntityCommand_PlayAni, anim_param);
    }

    //播完抛竿动作0.6s后，马上播放钓鱼动作
    IEnumerator DelayPlayFishingAni(uint uid)
    {
        yield return new WaitForSeconds(0.6f);
        PlayAni(uid, EntityAction.Fishing, -1);
    }


    //收杆
    public void FinishFishing(uint addIndex)
    {
        //发送收杆加成
        ReqPointerAddition(addIndex);
    }

    /// <summary>
    /// 关闭钓鱼
    /// </summary>
    public void CloseFishing(uint uid)
    {
        PlayAni(uid, EntityAction.Stand, -1);
    }

    /// <summary>
    /// 下次钓鱼
    /// </summary>
    public void NextFishing()
    {
        m_nextFishingCoroutine = ReqNextFishing();
        Engine.CorotinueInstance.Instance.StartCoroutine(m_nextFishingCoroutine);
    }

    IEnumerator ReqNextFishing()
    {
        yield return new WaitForSeconds(2f);
        ReqOpenFishing();
    }
    #endregion


    #region  Net

    /// <summary>
    /// 打开钓鱼
    /// </summary>
    public void ReqOpenFishing()
    {
        if (CanFishing())
        {
            stRequestOpenFishingPropertyUserCmd_CS cmd = new stRequestOpenFishingPropertyUserCmd_CS();

            NetService.Instance.Send(cmd);
        }
    }

    /// <summary>
    /// 打开钓鱼
    /// </summary>
    /// <param name="msg"></param>
    public void OnOpenFishing(stRequestOpenFishingPropertyUserCmd_CS msg)
    {
        this.m_bIsFishing = true;

        DataManager.Manager<UIPanelManager>().ShowPanel(PanelID.FishingPanel);

        //开始钓鱼
        StartFishing(Client.ClientGlobal.Instance().MainPlayer.GetID());
    }

    /// <summary>
    /// 关闭钓鱼
    /// </summary>
    public void ReqCloseFishing()
    {
        stRequesClosetFishingPropertyUserCmd_C cmd = new stRequesClosetFishingPropertyUserCmd_C();
        NetService.Instance.Send(cmd);
    }


    /// <summary>
    /// 关闭钓鱼
    /// </summary>
    /// <param name="msg"></param>
    public void OnCloseFishing(stClosetFishingPropertyUserCmd_S msg)
    {
        this.m_bIsFishing = false;

        if (m_nextFishingCoroutine != null)
        {
            Engine.CorotinueInstance.Instance.StopCoroutine(m_nextFishingCoroutine);
        }

        DataManager.Manager<UIPanelManager>().HidePanel(PanelID.FishingPanel);


        //钓鱼时间结束
        if (msg.flag == 1)
        {
            //弹出排行榜
            DataManager.Manager<UIPanelManager>().ShowPanel(PanelID.FishingRankPanel);

            CloseAllPlayerFishing();

            TipsManager.Instance.ShowTips("钓鱼活动结束");
        }
    }

    /// <summary>
    /// 关闭所有人钓鱼
    /// </summary>
    void CloseAllPlayerFishing()
    {
        //换成武器
        Dictionary<uint, uint> dic1 = DataManager.Manager<SuitDataManager>().PlayerSuitDic;
        Dictionary<uint, uint>.Enumerator etr1 = dic1.GetEnumerator();
        while (etr1.MoveNext())
        {
            uint uid = etr1.Current.Key;
            uint weaponId = etr1.Current.Value;

            DataManager.Manager<SuitDataManager>().RebackWeaponSuit(uid);

            PlayAnimation(uid, EntityAction.Stand, -1);
        }

        //关闭鱼线
        Dictionary<uint, Engine.IRenderObj> dic2 = DataManager.Manager<SuitDataManager>().PlayerFishingDic;
        Dictionary<uint, Engine.IRenderObj>.Enumerator etr2 = dic2.GetEnumerator();
        while (etr2.MoveNext())
        {
            uint uid = etr1.Current.Key;

            CloseFishingLine(uid);
        }

        //清协程
        Engine.CorotinueInstance.Instance.StopAllCoroutines();

        //清鱼竿 鱼线数据
        DataManager.Manager<SuitDataManager>().CleanFisingData();
    }

    /// <summary>
    /// 客户端指针加成(收杆的时候发);
    /// </summary>
    public void ReqPointerAddition(uint addIndex)
    {
        stLeaePropertyUserCmd_C cmd = new stLeaePropertyUserCmd_C();
        cmd.index = addIndex;
        NetService.Instance.Send(cmd);
    }

    /// <summary>
    /// 甩杆时告诉客户端几秒出现条什么鱼
    /// </summary>
    /// <param name="msg"></param>
    public void OnGetOneFish(stGetOneFihsPropertyUserCmd_S msg)
    {
        this.m_fishingUpTime = Time.realtimeSinceStartup + msg.time;
        this.m_fishingUpFishId = msg.kind;

        FishingDataBase fishingDb = GameTableManager.Instance.GetTableItem<FishingDataBase>(this.m_fishingUpFishId);
        if (fishingDb != null)
        {
            this.m_fishingUpCd = fishingDb.upTime;
        }
        else
        {
            //没钓到鱼(10s + 2s后自动开始第二次钓鱼)
            this.m_fishingUpCd = 0;

            Engine.Utility.Log.Error(" fishId == 0 出错了！！！ ");
        }

        if (DataManager.Manager<UIPanelManager>().IsShowPanel(PanelID.FishingPanel))
        {
            DataManager.Manager<UIPanelManager>().SendMsg(PanelID.FishingPanel, UIMsgID.eFishingGetOne, null);
        }
    }

    /// <summary>
    /// 告诉客户端成功钓上的鱼
    /// </summary>
    /// <param name="msg"></param>
    public void OnSucessFishing(stSucessFishingPropertyUserCmd_S msg)
    {
        this.m_fishId = msg.fishId;

        //延时0.7s显示钓到鱼的UI
        Engine.CorotinueInstance.Instance.StartCoroutine(DelayToShowSucessFish());
    }

    /// <summary>
    /// 延时0.7s显示钓到鱼的UI
    /// </summary>
    /// <returns></returns>
    IEnumerator DelayToShowSucessFish()
    {
        yield return new WaitForSeconds(0.9f);
        if (DataManager.Manager<UIPanelManager>().IsShowPanel(PanelID.FishingPanel))
        {
            DataManager.Manager<UIPanelManager>().SendMsg(PanelID.FishingPanel, UIMsgID.eFishingSuccess, null);
        }
    }


    /// <summary>
    /// 钓鱼排行
    /// </summary>
    public void ReqFishRanking()
    {
        stRequestOrderListRelationUserCmd_C cmd = new stRequestOrderListRelationUserCmd_C();
        cmd.type = OrderListType.OrderListType_FishCoin;
        NetService.Instance.Send(cmd);
    }

    /// <summary>
    /// 钓鱼排行
    /// </summary>
    /// <param name="msg"></param>
    public void OnFishRankingList(stRequestFishRankingListRelationUserCmd_CS msg)
    {
        this.m_fishingRank = msg.self_rank;
        this.m_fishingScore = msg.self_yiju;

        m_fishingRankInfoList.Clear();
        for (int i = 0; i < msg.data.Count; i++)
        {
            FishingRankInfo info = new FishingRankInfo();
            info.rank = msg.data[i].rank;
            info.name = msg.data[i].name;
            info.clanName = msg.data[i].clanname;
            info.num = msg.data[i].num;
            info.score = msg.data[i].yiju;

            m_fishingRankInfoList.Add(info);
        }

        if (DataManager.Manager<UIPanelManager>().IsShowPanel(PanelID.FishingPanel))
        {
            DataManager.Manager<UIPanelManager>().SendMsg(PanelID.FishingPanel, UIMsgID.eFishingMyRank, null);
        }

        if (DataManager.Manager<UIPanelManager>().IsShowPanel(PanelID.FishingRankPanel))
        {
            DataManager.Manager<UIPanelManager>().SendMsg(PanelID.FishingRankPanel, UIMsgID.eFishingRank, null);
        }
    }

    /// <summary>
    /// 9屏同步钓鱼状态
    /// </summary>
    /// <param name="cmd"></param>
    public void OnFishingStateToNine(stFishStateToNinePropertyUserCmd_S cmd)
    {
        IEntitySystem es = ClientGlobal.Instance().GetEntitySystem();
        if (es == null)
        {
            return;
        }

        IPlayer player = es.FindPlayer(cmd.uid);
        if (player == null)
        {
            return;
        }

        // 1 ：进入钓鱼  2 ：退出钓鱼
        if (cmd.type == 1)
        {
            //调整人物朝向
            AdjustPlayerFoward(player);

            //武器换成鱼竿
            DataManager.Manager<SuitDataManager>().OnFishingRodSuit(player);
        }
        else if (cmd.type == 2)
        {
            //鱼竿换回武器
            DataManager.Manager<SuitDataManager>().RebackWeaponSuitAndCleanData(player.GetID());

            //播放站立动作
            PlayAnimation(cmd.uid, EntityAction.Stand, -1);

            //关闭画鱼线
            CloseFishingLine(cmd.uid);
        }
    }




    /// <summary>
    /// 9屏同步钓鱼动作
    /// </summary>
    /// <param name="cmd"></param>
    public void OnFishingAniToNine(stFishToNinePropertyUserCmd_S cmd)
    {
        IEntitySystem es = ClientGlobal.Instance().GetEntitySystem();
        if (es == null)
        {
            return;
        }

        IPlayer player = es.FindPlayer(cmd.uid);
        if (player == null)
        {
            return;
        }

        //不是钓鱼的装备  换成钓鱼的装备
        if (false == DataManager.Manager<SuitDataManager>().IsFishingSuit(cmd.uid))
        {
            //调整人物朝向
            AdjustPlayerFoward(player);

            //武器换成鱼竿
            DataManager.Manager<SuitDataManager>().OnFishingRodSuit(player);
        }


        // 1 ：为抛竿  3 ：为收杆
        if (cmd.type == 1)
        {
            PlayStartFishingAni(cmd.uid);

            //鱼竿动画
            PlayFishingRodAni(cmd.uid, "Angling_Start");

            //关闭画鱼线
            CloseFishingLine(cmd.uid);

            Engine.CorotinueInstance.Instance.StartCoroutine(DrawFishingLine(cmd.uid));
        }
        else if (cmd.type == 3)
        {
            PlayAnimation(cmd.uid, EntityAction.FishingEnd, 1);

            //鱼竿动画
            PlayFishingRodAni(cmd.uid, "Angling_End");

            //关闭画鱼线
            CloseFishingLine(cmd.uid);

            //收杆动作完成后 回收鱼竿 播放idle
            Engine.CorotinueInstance.Instance.StartCoroutine(DelayToFishngStand(cmd.uid));
        }
    }

    /// <summary>
    /// 播放鱼竿动画
    /// </summary>
    /// <param name="uid"></param>
    /// <param name="aniName"></param>
    void PlayFishingRodAni(uint uid, string aniName)
    {
        Dictionary<uint, Engine.IRenderObj> playerFishingDic = DataManager.Manager<SuitDataManager>().PlayerFishingDic;

        Engine.IRenderObj yuganObj;
        if (playerFishingDic.TryGetValue(uid, out yuganObj))
        {
            Engine.Node node = yuganObj.GetNode();
            if (node == null)
            {
                return;
            }

            Transform transf = node.GetTransForm();
            if (transf == null)
            {
                return;
            }

            Animation[] anis = transf.gameObject.GetComponentsInChildren<Animation>();

            if (anis.Length > 0)
            {
                Animation animation = anis[0];
                animation.Play(aniName);
            }
        }
    }

    /// <summary>
    /// 收杆后 延时到idle 状态
    /// </summary>
    /// <returns></returns>
    IEnumerator DelayToFishngStand(uint uid)
    {
        yield return new WaitForSeconds(1f);

        //换成武器
        DataManager.Manager<SuitDataManager>().RebackWeaponSuitAndCleanData(uid);

        //站立动作
        PlayAnimation(uid, EntityAction.Stand, -1);

        //关闭画鱼线
        CloseFishingLine(uid);
    }


    /// <summary>
    /// 画鱼线
    /// </summary>
    /// <param name="uid"></param>
    /// <returns></returns>
    IEnumerator DrawFishingLine(uint uid)
    {
        yield return new WaitForSeconds(0.9f);

        Dictionary<uint, Engine.IRenderObj> playerFishingDic = DataManager.Manager<SuitDataManager>().PlayerFishingDic;

        Engine.IRenderObj yuganObj;
        if (playerFishingDic.TryGetValue(uid, out yuganObj))
        {
            Engine.Node node = yuganObj.GetNode();
            if (node != null)
            {
                Transform transf = node.GetTransForm();
                if (transf != null)
                {
                    LineRenderer[] lrs = transf.gameObject.GetComponentsInChildren<LineRenderer>();

                    if (lrs.Length > 0)
                    {
                        LineRenderer lr = lrs[0];
                        lr.enabled = true;

                        Vector3 startPos = lr.gameObject.transform.position;
                        Vector3 endPos = new Vector3(startPos.x, startPos.y - 8, startPos.z);

                        lr.SetPosition(0, startPos);
                        lr.SetPosition(1, endPos);
                    }
                    else
                    {
                        Engine.Utility.Log.Error("没有鱼竿顶点 ，鱼线会出错！！！ 找罗宇辉");
                    }
                }
            }

        }
    }

    /// <summary>
    /// 关闭画鱼线
    /// </summary>
    /// <param name="uid"></param>
    void CloseFishingLine(uint uid)
    {
        Dictionary<uint, Engine.IRenderObj> playerFishingDic = DataManager.Manager<SuitDataManager>().PlayerFishingDic;

        Engine.IRenderObj yuganObj;
        if (playerFishingDic.TryGetValue(uid, out yuganObj))
        {
            Engine.Node node = yuganObj.GetNode();
            if (node != null)
            {
                Transform transf = node.GetTransForm();
                if (transf != null)
                {
                    LineRenderer[] lrs = transf.gameObject.GetComponentsInChildren<LineRenderer>();

                    if (lrs.Length > 0)
                    {
                        LineRenderer lr = lrs[0];
                        lr.enabled = false;
                    }
                }
            }
        }
    }

    /// <summary>
    /// 调整玩家钓鱼朝向
    /// </summary>
    void AdjustPlayerFoward(IPlayer player)
    {
        List<string> keyList = GameTableManager.Instance.GetGlobalConfigKeyList("FS_POS");
        Vector3 playerPos = player.GetPos();
        float tempDistance = 0;
        Vector3 tempPos = Vector3.zero;

        List<uint> tempList = GameTableManager.Instance.GetGlobalConfigList<uint>("FS_POS", "1");
        if (tempList != null && tempList.Count == 2)
        {
            tempPos = new Vector3(tempList[0], playerPos.y, -tempList[1]);
            tempDistance = Vector3.Distance(playerPos, tempPos);
        }

        for (int i = 0; i < keyList.Count; i++)
        {
            List<uint> posList = GameTableManager.Instance.GetGlobalConfigList<uint>("FS_POS", keyList[i]);
            if (posList.Count == 2)
            {
                Vector3 pos = new Vector3(posList[0], playerPos.y, -posList[1]);
                float d = Vector3.Distance(pos, playerPos);

                if (d < tempDistance)
                {
                    tempDistance = d;
                    tempPos = pos;
                    Engine.Utility.Log.Error("---> tempPos = " + tempPos);
                }
            }
        }

        player.SendMessage(EntityMessage.EntityCommand_LookTarget, tempPos);
    }

    #endregion
}

