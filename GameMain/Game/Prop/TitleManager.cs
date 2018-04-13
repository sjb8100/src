using Client;
using Engine.Utility;
using GameCmd;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using table;

//*******************************************************************************************
//	创建日期：	2016-10-31   11:49
//	文件名称：	TitleManager,cs
//	创 建 人：	Lin Chuang Yuan
//	版权所有：	Lin Chuang Yuan
//	说    明：	称号系统
//*******************************************************************************************
public class TitleManager : BaseModuleData, IManager, ITimer
{

    #region property

    /// <summary>
    /// 佩戴的称号的ID
    /// </summary>
    public uint WearTitleId { set; get; }

    /// <summary>
    /// 激活的称号ID
    /// </summary>
    public uint ActivateTitleId { set; get; }

    private List<uint> m_listNewTitleId = new List<uint>();
    public List<uint> NewTitleIdList
    {
        get
        {
            return m_listNewTitleId;
        }
    }

    List<stTitleData> m_listOwnedTitle = new List<stTitleData>();   //服务器发过来的已经拥有的的称号

    Dictionary<uint, List<uint>> m_dicTitle = new Dictionary<uint, List<uint>>();         //用于显示的

    List<uint> m_lstTitleCategory = new List<uint>();              //称号类别list

    Dictionary<uint, string> m_dicTitleCategory = new Dictionary<uint, string>();//称号类别 ， 用于上面几个标签页显示

    List<TitleDataBase> m_tableTitleList = null;            //table中总的称号list

    List<TitleDataBase> m_tableVisibleTitleList = null;     //table中可见的称号list

    //Dictionary<Client.IPlayer, int> m_DictEffectTitle = new Dictionary<Client.IPlayer, int>();

    private const int TITLE_TIMERID = 2000;

    int openlv = 0;
    /// <summary>
    /// 称号  key 为5大类型 value为id的list
    /// </summary>
    Dictionary<uint, List<uint>> TitleDic
    {
        get
        {
            return m_dicTitle;
        }
    }

    /// <summary>
    /// 称号类别list
    /// </summary>
    public List<uint> TitleCategoryList
    {
        get
        {
            return m_lstTitleCategory;
        }
    }

    public Dictionary<uint, string> TitleCategoryDic
    {
        get
        {
            return m_dicTitleCategory;
        }
    }

    /// <summary>
    /// 已经拥有的list
    /// </summary>
    public List<stTitleData> OwnedTitleList
    {
        get
        {
            return m_listOwnedTitle;
        }
    }

    /// <summary>
    /// 是否显示称号
    /// </summary>
    bool m_isShowTitle = true;

    #endregion


    #region override
    public void ClearData()
    {

    }
    /// <summary>
    /// 初始化
    /// </summary>
    public void Initialize()
    {
        m_tableTitleList = GameTableManager.Instance.GetTableList<TitleDataBase>(); //表中的称号list;
        if (m_tableTitleList != null)
        {
            m_tableVisibleTitleList = m_tableTitleList.FindAll((data) => { return data.dwShow == 1; });  //可见的
        }

        m_dicTitle.Clear();
        m_lstTitleCategory.Clear();
        m_dicTitleCategory.Clear();
        List<TitleDataBase> titleList = GameTableManager.Instance.GetTableList<TitleDataBase>();
        if (titleList != null)
        {
            for (int i = 0; i < titleList.Count; i++)
            {
                if (!m_dicTitle.ContainsKey(titleList[i].type))
                {
                    List<TitleDataBase> temp = titleList.FindAll((d) => { return d.type == titleList[i].type; });
                    List<uint> titleDataList = new List<uint>();
                    for (int a = 0; a < temp.Count; a++)
                    {
                        titleDataList.Add(temp[a].dwID);
                    }

                    m_dicTitle.Add(titleList[i].type, titleDataList);     //称号数据

                    m_lstTitleCategory.Add(titleList[i].type);            //称号类别数据

                    m_dicTitleCategory.Add(titleList[i].type, titleList[i].typeName);//称号类别数据
                }
            }
        }

        TimerAxis.Instance().SetTimer(TITLE_TIMERID, 1000, this);
        openlv = GameTableManager.Instance.GetGlobalConfig<int>("TitleOpenLevel");
    }
    /// <summary>
    /// 重置
    /// </summary>
    public void Reset(bool depthClearData = false)
    {
        CLeanData();

        TimerAxis.Instance().KillTimer(TITLE_TIMERID, this);
    }
    /// <summary>
    /// 每帧执行
    /// </summary>
    /// <param name="deltaTime"></param>
    public void Process(float deltaTime)
    {

    }

    #endregion


    #region method

    /// <summary>
    /// 清除数据
    /// </summary>
    void CLeanData()
    {
        m_listNewTitleId.Clear();
        m_listOwnedTitle.Clear();    //服务器发过来的已经拥有的的称号
    }

    /// <summary>
    /// 清空并初始化title Dic
    /// </summary>
    void CleanAndInitTitleDic()
    {
        m_dicTitle.Clear();

        List<TitleDataBase> titleList = GameTableManager.Instance.GetTableList<TitleDataBase>();
        if (titleList != null)
        {
            for (int i = 0; i < titleList.Count; i++)
            {
                if (!m_dicTitle.ContainsKey(titleList[i].type))
                {
                    List<uint> titleDataList = new List<uint>();

                    m_dicTitle.Add(titleList[i].type, titleDataList);     //称号数据
                }
            }
        }
    }

    /// <summary>
    /// 通过类型取对应的已获得称号List
    /// </summary>
    /// <param name="type">标签页上面的类型</param>
    /// <returns></returns>
    public List<uint> GetOwnTitleListByType(uint type)
    {
        List<uint> list = m_dicTitle[type];

        List<uint> ownList = new List<uint>();
        for (int i = 0; i < list.Count; i++)
        {
            if (m_listOwnedTitle.Exists((data) => { return data.dwID == list[i] ? true : false; }))
            {
                ownList.Add(list[i]);
            }
        }

        return ownList;
    }

    /// <summary>
    /// 永久加成Id list
    /// </summary>
    public List<uint> GetForeverAddList(uint titleId)
    {
        List<uint> foreverAddList = new List<uint>();
        TitleDataBase titleDataBase = GameTableManager.Instance.GetTableItem<TitleDataBase>(titleId);
        if (titleDataBase != null)
        {
            string addStr = titleDataBase.per_effect;

            if (addStr == "0")
            {
                return foreverAddList;
            }

            string[] addStrArr = addStr.Split('_');
            for (int i = 0; i < addStrArr.Length; i++)
            {
                uint addEffectId = uint.Parse(addStrArr[i]);
                foreverAddList.Add(addEffectId);
            }
        }
        return foreverAddList;
    }

    /// <summary>
    /// 永久加成战斗力
    /// </summary>
    /// <param name="titleId"></param>
    /// <returns></returns>
    public uint GetForeverAddFight(uint titleId)
    {
        uint fight = 0;

        List<uint> list = GetForeverAddList(titleId);
        for (int i = 0; i < list.Count; i++)
        {
            StateDataBase db = GameTableManager.Instance.GetTableItem<StateDataBase>(list[i]);

            if (db == null)
            {
                Engine.Utility.Log.Error("取不到表格数据！！！");
                continue;
            }

            fight += (uint)db.fightPower;
        }
        return fight;
    }


    /// <summary>
    /// 激活加成effectIdList
    /// </summary>
    public List<uint> GetActivateAddList(uint titleId)
    {
        List<uint> activateAddList = new List<uint>();
        TitleDataBase titleDataBase = GameTableManager.Instance.GetTableItem<TitleDataBase>(titleId);
        if (titleDataBase != null)
        {
            string addStr = titleDataBase.act_effect;

            if (addStr == "0")
            {
                return activateAddList;
            }

            string[] addStrArr = addStr.Split('_');
            for (int i = 0; i < addStrArr.Length; i++)
            {
                uint addEffectId = uint.Parse(addStrArr[i]);
                activateAddList.Add(addEffectId);
            }
        }
        return activateAddList;
    }

    /// <summary>
    /// 激活加成战斗力
    /// </summary>
    /// <param name="titleId"></param>
    /// <returns></returns>
    public uint GetActivateAddFight(uint titleId)
    {
        uint fight = 0;

        List<uint> list = GetActivateAddList(titleId);
        for (int i = 0; i < list.Count; i++)
        {
            StateDataBase db = GameTableManager.Instance.GetTableItem<StateDataBase>(list[i]);

            if (db == null)
            {
                Engine.Utility.Log.Error("取不到表格数据！！！");
                continue;
            }

            fight += (uint)db.fightPower;
        }
        return fight;
    }


    /// <summary>
    /// 总加成
    /// </summary>
    public uint GetAllAdd()
    {
        uint allAdd = 0;

        //基础累加加成
        for (int i = 0; i < m_listOwnedTitle.Count; i++)
        {
            TitleDataBase titleDataBase = GameTableManager.Instance.GetTableItem<TitleDataBase>(m_listOwnedTitle[i].dwID);

            if (titleDataBase == null)
            {
                Engine.Utility.Log.Error("取不到表格数据！！！");
                continue;
            }
            if (titleDataBase.timeliness == 0 || titleDataBase.timeliness == 2 || m_listOwnedTitle[i].dwTime > 0)//永久，限次，限时时间还没用完
            {
                uint fight = GetForeverAddFight(m_listOwnedTitle[i].dwID);
                allAdd += fight;
            }

        }

        //激活加成
        uint activateAdd = GetActivateAddFight(this.ActivateTitleId);
        allAdd += activateAdd;

        return allAdd;
    }

    /// <summary>
    /// 分项目总加成
    /// </summary>
    /// <returns>  </returns>
    public List<uint> GetAllForeverAddeffectIdList()
    {
        List<uint> allForeverAddList = new List<uint>();
        for (int i = 0; i < m_listOwnedTitle.Count; i++)
        {
            TitleDataBase titleDataBase = GameTableManager.Instance.GetTableItem<TitleDataBase>(m_listOwnedTitle[i].dwID);
            if (titleDataBase == null)
            {
                Engine.Utility.Log.Error("取不到表格数据！！！");
                continue;
            }

            if (titleDataBase.timeliness == 0 || titleDataBase.timeliness == 2 || m_listOwnedTitle[i].dwTime > 0)//永久，限次，限时时间还没用完
            {
                List<uint> list = GetForeverAddList(m_listOwnedTitle[i].dwID);

                for (int j = 0; j < list.Count; j++)
                {
                    allForeverAddList.Add(list[j]);
                }
            }
        }

        return allForeverAddList;
    }


    public Dictionary<uint, int> GetForeverAddByEachTypeDic()
    {
        Dictionary<uint, int> dic = new Dictionary<uint, int>();
        List<uint> allForeverAddeffectIdList = GetAllForeverAddeffectIdList();

        for (int i = 0; i < allForeverAddeffectIdList.Count; i++)
        {
            StateDataBase db = GameTableManager.Instance.GetTableItem<StateDataBase>(allForeverAddeffectIdList[i]);

            if (dic.ContainsKey(db.typeid))
            {
                dic[db.typeid] += db.param1;
            }
            else
            {
                dic[db.typeid] = db.param1;
            }
        }

        return dic;
    }

    /// <summary>
    /// 已经获得的激活加成
    /// </summary>
    /// <returns></returns>
    public List<uint> GetAllActivateAddList()
    {
        return GetActivateAddList(this.ActivateTitleId);
    }

    /// <summary>
    /// 用于设置是否显示场景中的称号
    /// </summary>
    /// <param name="b"></param>
    public void SetIsShowTitle(bool b)
    {
        m_isShowTitle = b;
    }

    //获得当前大页签下面的数据 key: 小类 value: 里面的称号 id list
    public Dictionary<uint, List<uint>> GetTitleTypeDic(uint typeId)
    {
        Dictionary<uint, List<uint>> titleTypeDic = new Dictionary<uint, List<uint>>();//1、key 小类   2、value 里面的称号 id list

        List<uint> list;
        if (m_dicTitle.TryGetValue(typeId, out list))
        {
            //1、排序
            //OrderTitleIdList(ref list);
            list.Sort(TitleSort);

            //2、添加到字典
            for (int i = 0; i < list.Count; i++)
            {
                TitleDataBase tdb = m_tableTitleList.Find((data) => { return data.dwID == list[i]; });
                if (titleTypeDic.ContainsKey(tdb.secondType) == false)
                {
                    List<uint> titleIdList = new List<uint>();
                    titleIdList.Add(tdb.dwID);
                    titleTypeDic.Add(tdb.secondType, titleIdList);
                }
                else
                {
                    titleTypeDic[tdb.secondType].Add(tdb.dwID);
                }
            }
        }

        return titleTypeDic;
    }

    public List<uint> GetTitleTypeList(uint typeId)
    {
        List<uint> list = new List<uint>();
        if (m_dicTitle.TryGetValue(typeId, out list))
        {
            return list;
        }
        return list;
    }

    //对称号Id排序
    void OrderTitleIdList(ref List<uint> titleIdList)
    {
        List<uint> copyTitleIdList = new List<uint>();//拷贝数据
        for (int i = 0; i < titleIdList.Count; i++)
        {
            copyTitleIdList.Add(titleIdList[i]);
        }

        titleIdList.Clear();

        stTitleData td1 = m_listOwnedTitle.Find((data) => { return data.dwID == ActivateTitleId; });// 1、激活

        if (td1 != null && copyTitleIdList.Contains(td1.dwID))
        {
            if (titleIdList.Contains(td1.dwID) == false)
            {
                titleIdList.Add(td1.dwID);
            }
        }

        stTitleData td2 = m_listOwnedTitle.Find((data) => { return data.dwID == WearTitleId; });    // 2、佩戴
        if (td2 != null && copyTitleIdList.Contains(td2.dwID))
        {
            if (titleIdList.Contains(td2.dwID) == false)
            {
                titleIdList.Add(td2.dwID);
            }
        }

        for (int i = 0; i < m_listOwnedTitle.Count; i++)
        {
            if (m_listOwnedTitle[i].dwID != ActivateTitleId && m_listOwnedTitle[i].dwID != WearTitleId && copyTitleIdList.Contains(m_listOwnedTitle[i].dwID))//3、拥有的
            {
                if (titleIdList.Contains(m_listOwnedTitle[i].dwID) == false)
                {
                    titleIdList.Add(m_listOwnedTitle[i].dwID);
                }
            }
        }

        for (int i = 0; i < copyTitleIdList.Count; i++)  //4、未拥有的
        {
            if (titleIdList.Contains(copyTitleIdList[i]) == false)
            {
                titleIdList.Add(copyTitleIdList[i]);
            }
        }
    }

    /// <summary>
    /// 称号排序
    /// </summary>
    /// <param name="left"></param>
    /// <param name="right"></param>
    /// <returns></returns>
    int TitleSort(uint leftTitleId, uint rightTitleId)
    {
        //1、激活的
        if (leftTitleId == ActivateTitleId || rightTitleId == ActivateTitleId)
        {
            return rightTitleId == ActivateTitleId ? 1 : -1;
        }

        //2、佩戴的
        else if (leftTitleId == WearTitleId || rightTitleId == WearTitleId)
        {
            return rightTitleId == WearTitleId ? 1 : -1;
        }

        //3、拥有的
        else if (true == m_listOwnedTitle.Exists((d) => { return d.dwID == leftTitleId; }) || true == m_listOwnedTitle.Exists((d) => { return d.dwID == rightTitleId; }))
        {
            return true == m_listOwnedTitle.Exists((d) => { return d.dwID == rightTitleId; }) ? 1 : -1;
        }

        //4、未拥有的
        else
        {
            return leftTitleId > rightTitleId ? 1 : -1;
        }


        //if (true == m_listOwnedTitle.Exists((d) => { return d.dwID == leftTitleId; }) || true == m_listOwnedTitle.Exists((d) => { return d.dwID == rightTitleId; }))
        //{
        //    //1、激活的
        //    if (leftTitleId == ActivateTitleId || rightTitleId == ActivateTitleId)
        //    {
        //        return rightTitleId == ActivateTitleId ? 1 : -1;
        //    }

        //    //2、佩戴的
        //    else if (leftTitleId == WearTitleId || rightTitleId == WearTitleId)
        //    {
        //        return rightTitleId == WearTitleId ? 1 : -1;
        //    }

        //    //3、拥有的
        //    else if (true == m_listOwnedTitle.Exists((d) => { return d.dwID == leftTitleId; }) || true == m_listOwnedTitle.Exists((d) => { return d.dwID == rightTitleId; }))
        //    {
        //        return true == m_listOwnedTitle.Exists((d) => { return d.dwID == rightTitleId; }) ? 1 : -1;
        //    }
        //    else 
        //    {
        //        return leftTitleId > rightTitleId ? 1 : -1;
        //    }
        //}
        ////4、未拥有的
        //else
        //{
        //    return leftTitleId > rightTitleId ? 1 : -1;
        //}
    }

    /// <summary>
    /// UI中用到的排序
    /// </summary>
    /// <param name="titleId"></param>
    /// <param name="dic"></param>
    public void OrderTitleIdDic(uint titleId, ref Dictionary<uint, List<uint>> dic)
    {
        if (dic == null)
        {
            return;
        }

        TitleDataBase tdb = m_tableTitleList.Find((data) => { return data.dwID == titleId; });
        if (tdb != null)
        {
            Dictionary<uint, List<uint>>.Enumerator enumerator = dic.GetEnumerator();
            while (enumerator.MoveNext())
            {
                List<uint> list;
                if (dic.TryGetValue(enumerator.Current.Key, out list))
                {
                    //排序
                    //OrderTitleIdList(ref list);
                    list.Sort(TitleSort);
                }
            }
        }
    }


    /// <summary>
    /// 有新的称号
    /// </summary>
    /// <returns></returns>
    public bool HaveNewTitle()
    {
        int lv = MainPlayerHelper.GetPlayerLevel();    
        bool have =m_listNewTitleId.Count > 0 && lv >= openlv;
        return have;
    }

    /// <summary>
    /// 是否是新获得称号
    /// </summary>
    /// <returns></returns>
    public bool IsNewTitle(uint titleId)
    {
        return m_listNewTitleId.Exists((data) => { return data == titleId; });
    }

    #endregion


    #region Net
    /// <summary>
    /// 称号列表
    /// </summary>
    /// <param name="cmd"></param>
    public void OnAddAllTitleList(stGetAllTitlePropertyUserCmd_S cmd)
    {
        this.m_listOwnedTitle = cmd.data;
        this.WearTitleId = cmd.select;
        this.ActivateTitleId = cmd.activate;
    }

    /// <summary>
    /// 新获得称号
    /// </summary>
    public void OnAddNewTitle(stAddTitlePropertyUserCmd_S cmd)
    {
        uint newTitleId = cmd.data.dwID;
        if (this.m_listNewTitleId.Contains(cmd.data.dwID) == false)
        {
            this.m_listNewTitleId.Add(cmd.data.dwID);
        }

        stTitleData titleData = m_listOwnedTitle.Find((data) => { return data.dwID == cmd.data.dwID ? true : false; });

        if (titleData != null)
        {
            titleData.dwCount = cmd.data.dwCount;
            titleData.dwTime = cmd.data.dwTime;
        }
        else
        {
            m_listOwnedTitle.Add(new stTitleData { dwID = cmd.data.dwID, dwCount = cmd.data.dwCount, dwTime = cmd.data.dwTime });
        }

        object param = newTitleId;
        stShowMainPanelRedPoint st = new stShowMainPanelRedPoint()
        {
            modelID = (int)WarningEnum.TITLE_NEWTITLE,
            direction = (int)WarningDirection.Left,
            bShowRed = HaveNewTitle(),
        };
        EventEngine.Instance().DispatchEvent((int)GameEventID.MAINPANEL_SHOWREDWARING, st);

        SendToMe(cmd.data.dwID); //系统消息获得称号
    }

    /// <summary>
    /// 用于系统消息
    /// </summary>
    void SendToMe(uint titleId)
    {
        TitleDataBase titleDb = GameTableManager.Instance.GetTableItem<TitleDataBase>(titleId);
        if (titleDb == null)
        {
            return;
        }

        string message = string.Format("恭喜您获得了{0}称号", titleDb.strName);
        Pmd.stCommonChatUserPmd_CS sendCmd = new Pmd.stCommonChatUserPmd_CS();
        sendCmd.szInfo = message;
        sendCmd.byChatType = (uint)GameCmd.CHATTYPE.CHAT_SYS;
        sendCmd.dwOPDes = (uint)Client.ClientGlobal.Instance().MainPlayer.GetID();
        NetService.Instance.SendToMe(sendCmd);
    }


    /// <summary>
    /// 用户请求佩戴某个title
    /// </summary>
    /// <param name="cmd"></param>
    public void ReqSelectTitle(uint titleId)
    {
        stSelectTitlePropertyUserCmd_CS cmd = new stSelectTitlePropertyUserCmd_CS();
        cmd.dwUserID = ClientGlobal.Instance().MainPlayer.GetID();
        cmd.wdTitleID = titleId;
        NetService.Instance.Send(cmd);
    }

    public void OnSelectTitle(stSelectTitlePropertyUserCmd_CS cmd)
    {
        //如果是玩家自己
        if (Client.ClientGlobal.Instance().IsMainPlayer(cmd.dwUserID))
        {
            this.WearTitleId = cmd.wdTitleID;

            TitleDataBase titleDataBase = GameTableManager.Instance.GetTableItem<TitleDataBase>(this.WearTitleId);
            if (titleDataBase != null)
            {
                TipsManager.Instance.ShowLocalFormatTips(LocalTextType.Title_Commond_peidaichenghao, titleDataBase.strName);//佩戴称号｛0｝
            }

        }


        Client.IEntitySystem es = Client.ClientGlobal.Instance().GetEntitySystem();
        if (es != null)
        {
            //设置人物身上称号
            Client.IPlayer player = es.FindPlayer(cmd.dwUserID);
            if (player != null)
            {
                player.SetProp((int)PlayerProp.TitleId, (int)cmd.wdTitleID);
            }
        }


        //处理文字特效
        Client.stTitleWear data = new Client.stTitleWear { uid = cmd.dwUserID, titleId = cmd.wdTitleID };
        EventEngine.Instance().DispatchEvent((int)GameEventID.TITLE_WEAR, data);//抛出现在佩戴的称号
    }

    /// <summary>
    /// 用户请求激活某个title
    /// </summary>
    /// <param name="titleId"></param>
    public void ReqActivateTitle(uint titleId)
    {
        stActivateTitlePropertyUserCmd_CS cmd = new stActivateTitlePropertyUserCmd_CS();
        cmd.dwUserID = ClientGlobal.Instance().MainPlayer.GetID();
        cmd.wdTitleID = titleId;
        NetService.Instance.Send(cmd);
    }

    public void OnActivateTitle(stActivateTitlePropertyUserCmd_CS cmd)
    {
        this.ActivateTitleId = cmd.wdTitleID;

        if (this.ActivateTitleId == 0)
        {
            //取消激活
        }
        else
        {
            //激活称号
            TitleDataBase titleDataBase = GameTableManager.Instance.GetTableItem<TitleDataBase>(this.ActivateTitleId);
            if (titleDataBase != null)
            {
                TipsManager.Instance.ShowLocalFormatTips(LocalTextType.Title_Commond_jihuochenghao, titleDataBase.strName);//激活称号｛0｝
            }
        }

        object data = this.ActivateTitleId;
        EventEngine.Instance().DispatchEvent((int)GameEventID.TITLE_ACTIVATE, data);//抛出激活的称号
    }

    /// <summary>
    /// 增加称号使用次数
    /// </summary>
    public void ReqAddTitleUseTimes(uint titleId, uint itemId)
    {
        stAddCountTitlePropertyUserCmd_C cmd = new stAddCountTitlePropertyUserCmd_C();
        cmd.wdTitleID = titleId;
        //cmd.itemId = itemId;
        //cmd.autoBuy = autoBuy;

        NetService.Instance.Send(cmd);
    }

    /// <summary>
    /// 增加称号使用次数
    /// </summary>
    /// <param name="cmd"></param>
    public void OnGetTitleUseTimes(stSetCountTitlePropertyUserCmd_S cmd)
    {
        stTitleData titleData = m_listOwnedTitle.Find((data) => { return data.dwID == cmd.wdTitleID; });
        if (titleData != null)
        {
            titleData.dwCount = cmd.dwCount;

            //更新玩家身上称号属性
            CleanMainPlayerTitleProp(titleData);
        }

        object titleIdData = cmd.wdTitleID;
        EventEngine.Instance().DispatchEvent((int)GameEventID.TITLE_USETIMES, titleIdData);//抛出激活的称号
    }

    /// <summary>
    /// 更新玩家称号属性
    /// </summary>
    /// <param name="titleData"></param>
    void CleanMainPlayerTitleProp(stTitleData titleData)
    {
        IPlayer mainPlayer = Client.ClientGlobal.Instance().MainPlayer;
        if (mainPlayer != null)
        {
            int mainPlayerTitleId = mainPlayer.GetProp((int)PlayerProp.TitleId);
            //玩家称号使用时间和次数都为0  此时称号ID置0
            if ((uint)mainPlayerTitleId == titleData.dwID)
            {
                if (titleData.dwCount == 0 && titleData.dwTime == 0)
                {
                    mainPlayer.SetProp((int)PlayerProp.TitleId, 0);
                }
            }
        }
    }


    /// <summary>
    /// 删除称号
    /// </summary>
    public void OnDeleteTitle(stDeleteTitlePropertyUserCmd_CS cmd)
    {
        stTitleData titleData = m_listOwnedTitle.Find((data) => { return data.dwID == cmd.wdTitleID; });
        if (titleData != null)
        {
            m_listOwnedTitle.Remove(titleData);
        }

        object titleIdData = cmd.wdTitleID;
        EventEngine.Instance().DispatchEvent((int)GameEventID.TITLE_DELETE, titleIdData);//抛出删除的称号 
        isDelete = true;
    }

    #endregion


    #region Timer

    /// <summary>
    /// 倒计时
    /// </summary>
    /// <param name="uTimerID"></param>
    bool isDelete = false;
    uint timeOutTitleId = 0; //时间到的id
    public void OnTimer(uint uTimerID)
    {
        //stTitleData titleData = m_listOwnedTitle.Find((data) => { return data.dwID == timeOutTitleId; });//删除掉时间到的title
        //if (titleData != null)
        //{
        //    m_listOwnedTitle.Remove(titleData);
        //    object idData = titleData.dwID;
        //    EventEngine.Instance().DispatchEvent((int)GameEventID.TITLE_TIMEOUT, idData);//抛出倒计时为0的ID
        //}

        for (int i = 0; i < m_listOwnedTitle.Count; i++)
        {
            if (isDelete)
            {
                isDelete = false;
                return;
            }

            if (m_listOwnedTitle[i] != null && m_listOwnedTitle[i].dwTime > 0)
            {
                m_listOwnedTitle[i].dwTime--;
                if (m_listOwnedTitle[i].dwTime == 0)
                {
                    timeOutTitleId = m_listOwnedTitle[i].dwID;//暂存时间到的titleId

                    uint titleId = m_listOwnedTitle[i].dwID;

                    if (titleId == WearTitleId)
                    {
                        WearTitleId = 0;
                    }
                    if (titleId == ActivateTitleId)
                    {
                        ActivateTitleId = 0;
                    }

                    //更新玩家身上称号属性
                    CleanMainPlayerTitleProp(m_listOwnedTitle[i]);

                    object idData = titleId;
                    EventEngine.Instance().DispatchEvent((int)GameEventID.TITLE_TIMEOUT, idData);//抛出倒计时为0的ID
                }
            }
        }

    }
    #endregion
}

