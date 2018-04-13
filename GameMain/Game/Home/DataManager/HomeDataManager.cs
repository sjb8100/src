using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Client;
using GameCmd;
using table;
using Engine;
using Engine.Utility;
public enum HomeDispatchEvent
{
    ChangeMineState,
    ChangeTreeState,
}
partial class HomeDataManager : BaseModuleData, IManager, ITimer
{
    /// <summary>
    /// 农场id
    /// </summary>
    public uint landID = 101;
    /// <summary>
    /// 牧场id
    /// </summary>
    public uint animalID = 201;

    public uint mineModuleID = 401;
    public uint ironTreeID = 501;

    public uint copperTreeID = 502;
    public uint sliverTreeID = 503;
    public uint goldTreeID = 504;

    /// <summary>
    /// 牧场院子ID，用于区分农场里面的土地
    /// </summary>
    public int animalYardID = 10001;

    /// <summary>
    /// 家园拥有者id
    /// </summary>
    public uint m_uHomeUserID = 0;

    private readonly uint m_uHomeTimerID = 9527;

    public class HomePosInfo
    {
        //索引
        public int index = -1;

        public int posX = 0;
        public int posZ = 0;

    }
    public void Initialize()
    {
        EventEngine.Instance().AddEventListener((int)GameEventID.SYSTEM_LOADSCENECOMPELETE, DoGameEvent);
        EventEngine.Instance().AddEventListener((int)GameEventID.PLAYER_LOGIN_SUCCESS, DoGameEvent);
        EventEngine.Instance().AddEventListener((int)GameEventID.HOMELAND_CLICKENTITY, DoGameEvent);

        TimerAxis.Instance().SetTimer(m_uHomeTimerID, 1000, this);
        InitTradeItemDic();//大管家
    }

    public void Reset(bool depthClearData = false)
    {
        plantAndAnimalRemainTimeDic.Clear();
        seedIndexDic.Clear();
        canGainLandList.Clear();
        canGainAnimalList.Clear();
        HomeScene.Instance.Clear();
        HomeSceneUIRoot.Instance.ReleaseUI();
        entityStateDic.Clear();
        alreadyReqHomePrice = false;

        CleanHomeTradeData(); //清除大管家数据
    }
    public void ClearData()
    {

    }
    public void Process(float deltaTime)
    {
        // OnMineProcess( deltaTime );
    }
    void DoGameEvent(int eventID, object param)
    {
        if (eventID == (int)GameEventID.SYSTEM_LOADSCENECOMPELETE)
        {
            Client.stLoadSceneComplete pa = (Client.stLoadSceneComplete)param;
             int homeID = GameTableManager.Instance.GetGlobalConfig<int>( "HomeSceneID" );
             if ( pa.nMapID == homeID )
            {
                IPlayer player = ClientGlobal.Instance().MainPlayer;
                if (player != null)
                {
                    //stRequestAllHomeUserCmd_C cmd = new stRequestAllHomeUserCmd_C();
                    //cmd.char_id = player.GetID();
                    //NetService.Instance.Send(cmd);
                    ReqAllUserHomeData(player.GetID());

                    ReqHomeTradePrice();
                }

            }

        }
        else if (eventID == (int)GameEventID.PLAYER_LOGIN_SUCCESS)
        {

        }
        else if (eventID == (int)GameEventID.HOMELAND_CLICKENTITY)
        {
            stClickEntity ce = (stClickEntity)param;
            IEntitySystem es = ClientGlobal.Instance().GetEntitySystem();
            if (es != null)
            {
                IEntity et = es.FindEntity(ce.uid);
                if (et != null)
                {
                    if (IsMyHome()) //只有自己家园才可以点击实体
                    {
                        if (et.GetEntityType() == EntityType.EntityType_Plant || et.GetEntityType() == EntityType.EntityType_Soil)
                        {//农场点击响应
                            OnClickPlantEntity(et);
                        }
                        else if (et.GetEntityType() == EntityType.EntityType_Animal)
                        {
                            OnClickAnimalEntity(et);
                        }
                        else if (et.GetEntityType() == EntityType.EntityType_Tree)
                        {
                            OnClickTreeEntity(et);
                        }
                        else if (et.GetEntityType() == EntityType.EntityType_Minerals)
                        {
                            OnClickMineEntity(et);
                        }

                    }
                }
            }
        }
    }

    public void OnHomeData(stDataHomeUserCmd_S cmd)
    {

        plantAndAnimalRemainTimeDic.Clear();
        seedIndexDic.Clear();
        canGainLandList.Clear();
        canGainAnimalList.Clear();
        HomeScene.Instance.Clear();
        HomeSceneUIRoot.Instance.ReleaseUI();
        entityStateDic.Clear();

        m_uHomeUserID = cmd.char_id;
        InitAnimalData(cmd.data.pasture);
        InitFarmData(cmd.data.farm);
        RefreshMineData(cmd.data.mine);
        RefreshTreeData(cmd.data.tree);
        SetVipIsOpen(cmd.data.mine.vip_open);
        InitItemBagData(cmd.data.item_bag);
        OnEnterScene();
    }
    /// <summary>
    /// 是否有充足的道具
    /// </summary>
    /// <param name="itemID"></param>
    /// <param name="needNum"></param>
    /// <returns></returns>
    public bool HasEnoughItem(uint itemID, int needNum = 1)
    {
        int count = DataManager.Manager<ItemManager>().GetItemNumByBaseId(itemID);
        if (count < needNum)
        {
            TipsManager.Instance.ShowTipsById(6);
            return false;
        }
        return true;
    }

    public bool HasEnoughSeedAndCub(uint itemID, uint needNum = 1)
    {
        HomeItemData seedAndCubData = HomeItemDataList.Find((HomeItemData data) => { return data.base_id == itemID; });
        if (seedAndCubData != null)
        {
            return true;
        }
        TipsManager.Instance.ShowTipsById(6);
        return false;
    }

    /// <summary>
    /// 判断是否需要有足够的元宝
    /// </summary>
    /// <param name="needCount"></param>
    /// <returns></returns>
    public bool HasEnoughDianJuan(int needCount)
    {
        IPlayer player = ClientGlobal.Instance().MainPlayer;
        if (player != null)
        {
            int count = player.GetProp((int)PlayerProp.Coupon);
            if (count >= needCount)
                return true;
        }
        TipsManager.Instance.ShowTipsById(5);
        return false;
    }

    /// <summary>
    /// 获取各个模块的物件的在场景中的坐标信息
    /// </summary>
    /// <param name="modleID">模块id</param>
    /// <returns></returns>
    public List<HomePosInfo> GetPosListByModuleID(uint modleID)
    {
        List<HomePosInfo> list = new List<HomePosInfo>();
        HomeLandViewDatabase db = GameTableManager.Instance.GetTableItem<HomeLandViewDatabase>(modleID);
        if (db != null)
        {
            string[] posArray = db.posStr.Split(';');
            foreach (var info in posArray)
            {
                string[] infoArray = info.Split('_');
                if (infoArray.Length != 3)
                {
                    Log.Error("data does not meet the requirements");
                    return null;
                }
                HomePosInfo pi = new HomePosInfo();
                if (!int.TryParse(infoArray[0], out pi.index))
                {
                    Log.Error("int parse error");
                }
                if (!int.TryParse(infoArray[1], out pi.posX))
                {
                    Log.Error("int parse error");
                }
                if (!int.TryParse(infoArray[2], out pi.posZ))
                {
                    Log.Error("int parse error");
                }
                list.Add(pi);
            }

        }
        return list;
    }

    public HomePosInfo GetPosInfoByIndex(uint modelID, int index)
    {

        List<HomePosInfo> list = GetPosListByModuleID(modelID);
        foreach (var info in list)
        {
            if (info.index == index)
            {

                return info;
            }
        }
        return null;
    }

    public bool IsMyHome()
    {
        bool isSelf = false;
        IPlayer player = ClientGlobal.Instance().MainPlayer;
        if (player != null)
        {
            isSelf = player.GetID() == m_uHomeUserID ? true : false;
        }
        return isSelf;
    }
}

