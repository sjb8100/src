using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using table;

public class MallDefine
{
    #region Define
    /// <summary>
    /// 商品标识
    /// </summary>
    public enum MallTagType
    {
        None = 0,
        Hot,        //热卖
        Discount,   //折扣
        VipNeed,    //vip要求
        CharacterLv, //角色等级
    }

    /// <summary>
    /// 商品购买限制
    /// </summary>
    public enum PurchaseLimitType
    {
        None = 0,
        NotEnough = 1,      //背包空间不足
        UnShelve = 2,        //商品已下架
        SoldOut = 3,        //已售罄
        Vip = 4,            //vip要求
        CharacterLv = 5,    //角色等级要求
    }

    /// <summary>
    /// 黑市刷新数据
    /// </summary>
    public class BlackMarketRefreshData
    {
        //刷新次数
        public int RefreshCount = 0;
        //刷新消耗
        public CurrencyData RefreshCost = null;
        //刷新间隔时间(单位分钟)
        public int RefreshGap = 0;
    }

    /// <summary>
    /// 商品购买限制数据类
    /// </summary>
    public class PurchaseLimitData
    {
        public PurchaseLimitType limitType = PurchaseLimitType.None;
        public string limitDes = "";
    }

    /// <summary>
    /// 商城标记数据
    /// </summary>
    public class MallTagTypeData 
    {
        public MallTagType Tag = MallTagType.None;
        public int Value = 0;

        #region operator
        public static bool operator == (MallTagTypeData t1,MallTagTypeData t2)
        {
            if (System.Object.ReferenceEquals(t1, t2))
                return true;
            if (null == t1 || null == t2 )
                return false;
            return t1.Tag == t2.Tag;
        }

        public static bool operator !=(MallTagTypeData t1, MallTagTypeData t2)
        {
            return !(t1 == t2);
        }
        #endregion
    }

    /// <summary>
    /// 商城数据
    /// </summary>
    public class MallData
    {
        #region property
        //商城id
        private int m_int_mallId = 0;
        public int MallId
        {
            get
            {
                return m_int_mallId;
            }
        }

        //<mallid>
        private Dictionary<int, MallDefine.MallTagData> m_dic_mallDatas = null;
        #endregion

        #region structmethod
        private MallData (int mallId)
        {
            m_dic_mallDatas = new Dictionary<int, MallTagData>();
            m_int_mallId = mallId;
        }
        #endregion

        #region Create
        public static MallDefine.MallData Create(int mallId)
        {
            return new MallDefine.MallData(mallId);
        }
        #endregion

        #region Op
        /// <summary>
        /// 获取数据
        /// </summary>
        /// <param name="tagType"></param>
        /// <returns></returns>
        public MallDefine.MallTagData GetMallTagData(int tagType)
        {
            MallDefine.MallTagData data = null;
            return (m_dic_mallDatas.ContainsKey(tagType)) ? m_dic_mallDatas[tagType] : null;
        }

        /// <summary>
        /// 获取商城分类id
        /// </summary>
        /// <returns></returns>
        public List<int> GetMallTagIds()
        {
            List<int> tagIds = new List<int>();
            if (m_dic_mallDatas.Count > 0)
            {
                tagIds.AddRange(m_dic_mallDatas.Keys);
                tagIds.Sort();
            }
            return tagIds;
        }
        public void Clear()
        {
            m_dic_mallDatas.Clear();
        }

        /// <summary>
        /// 添加数据
        /// </summary>
        /// <param name="database"></param>
        public void Add(table.StoreDataBase database)
        {
            if (null == database || database.storeId != MallId)
            {
                return;
            }
            int tag = (int)database.tag;
            if (!m_dic_mallDatas.ContainsKey(tag))
            {
                m_dic_mallDatas.Add(tag, MallTagData.Create(MallId, tag, database.tagName));
            }
            m_dic_mallDatas[tag].Add(database.mallItemId, (int)database.sortId);
        }
        #endregion
    }

    /// <summary>
    /// 商城排序数据
    /// </summary>
    public class MallSortData
    {
        public uint MallItemId;
        public int SortId;
    }

    /// <summary>
    /// 商城分页数据
    /// </summary>
    public class MallTagData
    {
        #region property
        //商城id
        private int m_int_mallId = 0;
        public int MallId
        {
            get
            {
                return m_int_mallId;
            }
        }
        //分页标签id
        private int m_int_tagId = 0;
        public int TagId
        {
            get
            {
                return m_int_tagId;
            }
        }
        //分页标签名称
        private string m_str_tagName;
        public string TagName
        {
            get
            {
                return m_str_tagName;
            }
        }
        //tag字典<tagId,tagName>
        private List<MallSortData> m_lst_mallTagDatas = null;

        private bool m_bool_sort = false;
        #endregion

        #region structmethod
        private MallTagData(int mallId,int tagId = 0,string tagName = "")
        {
            m_bool_sort = false;
            m_lst_mallTagDatas = new List<MallSortData>();
            this.m_int_mallId = mallId;
            this.m_int_tagId = tagId;
            this.m_str_tagName = tagName;
        }
        #endregion

        #region Create
        /// <summary>
        /// 商城分页数据
        /// </summary>
        /// <param name="mallId">商城id</param>
        /// <param name="tagId">标签di</param>
        /// <param name="tagName"></param>
        /// <returns></returns>
        public static MallTagData Create(int mallId,int tagId =0 ,string tagName = "")
        {
            return new MallTagData(mallId,tagId,tagName);
        }
        #endregion

        #region Op

        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="tagId"></param>
        /// <param name="tagName"></param>
        public void Add(uint itemId,int sortId)
        {
            MallSortData sortData = new MallSortData()
            {
                MallItemId = itemId,
                SortId = sortId,
            };
            m_lst_mallTagDatas.Add(sortData);
        }


        /// <summary>
        /// 获取商城分页id
        /// </summary>
        /// <returns></returns>
        public List<uint> GetTagIdDatas()
        {
            List<uint> tagIds = new List<uint>();
            if (m_lst_mallTagDatas.Count > 0)
            {
                if (!m_bool_sort)
                {
                    m_bool_sort = true;
                    m_lst_mallTagDatas.Sort((left, right) =>
                        {
                            return left.SortId - right.SortId;
                        });
                }
                for (int i = 0; i < m_lst_mallTagDatas.Count; i++)
                {
                    tagIds.Add(m_lst_mallTagDatas[i].MallItemId);
                }
            }
            return tagIds;
        }

        /// <summary>
        /// 清空
        /// </summary>
        public void Clear()
        {
            m_lst_mallTagDatas.Clear();
            m_bool_sort = false;
        }
        #endregion
    }

    


    /// <summary>
    /// 黑市商城数据
    /// </summary>
    public class BlackMarketLocalData
    {
        //商城id
        private uint mallId;
        public uint MallId
        {
            get
            {
                return mallId;
            }
        }

        //手动刷新剩余次数
        private uint handRefreshTimes;
        public uint HandRefreshTimes
        {
            get
            {
                return handRefreshTimes;
            }
        }
        //下次自动刷新时间戳
        private long nextAutoRefreshTimeStamp;
        public long NextAutoRefreshTimeStamp
        {
            get
            {
                return nextAutoRefreshTimeStamp;
            }
        }

        /// <summary>
        /// 刷新间隔
        /// </summary>
        private uint autoRefreshGapTime;
        public uint AutoRefreshGapTime
        {
            get
            {
                return autoRefreshGapTime;
            }
        }
        //商城数据项列表
        private List<GameCmd.DynaStorePosInfo> infos = new List<GameCmd.DynaStorePosInfo>();
        public List<GameCmd.DynaStorePosInfo> Infos
        {
            get
            {
                return infos;
            }
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="info"></param>
        public BlackMarketLocalData(GameCmd.DynaStoreInfo info)
        {
            UpdateData(info);
        }

        /// <summary>
        /// 更新数据
        /// </summary>
        /// <param name="info"></param>
        public void UpdateData(GameCmd.DynaStoreInfo info)
        {
            mallId = info.store_id;
            nextAutoRefreshTimeStamp = info.refresh_time;
            autoRefreshGapTime = info.interval_time;
            handRefreshTimes = info.hand_times;
            infos.Clear();
            infos.AddRange(info.pos_info);
            Sort();
        }


        /// <summary>
        /// 是否包含该物品信息
        /// </summary>
        /// <param name="mallItemId"></param>
        /// <returns></returns>
        public bool Contains(uint mallItemId)
        {
            if (null != infos && infos.Count > 0)
            {
                foreach(GameCmd.DynaStorePosInfo info in infos)
                {
                    if (info.baseid == mallItemId)
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        /// <summary>
        /// 尝试获取服务端下发的物品信息
        /// </summary>
        /// <param name="pos"></param>
        /// <param name="info"></param>
        /// <returns></returns>
        public bool TryGetMallItemPosInfo(uint pos,out GameCmd.DynaStorePosInfo info)
        {
            info = null;
            if (null != infos && infos.Count > 0)
            {
                foreach (GameCmd.DynaStorePosInfo inf in infos)
                {
                    if (inf.pos == pos)
                    {
                        info = inf;
                        return true;
                    }
                }
            }
            return false;
        }

        /// <summary>
        /// 是否该商品售罄
        /// </summary>
        /// <param name="mallItemId"></param>
        /// <returns></returns>
        public bool IsSoldOut(uint pos)
        {
            GameCmd.DynaStorePosInfo info = null;
            if (TryGetMallItemPosInfo(pos, out info))
            {
                return (info.buy_flag == 1) ? true : false;
            }
            return true;
        }

        /// <summary>
        /// 设置售罄状态
        /// </summary>
        /// <param name="pos"></param>
        /// <param name="soldOut"></param>
        public void SetSoldOut(uint pos,bool soldOut)
        {
            GameCmd.DynaStorePosInfo info = null;
            if (TryGetMallItemPosInfo(pos, out info))
            {
                info.buy_flag = (uint)((soldOut) ? 1 : 0);
            }
        }

        /// <summary>
        /// 获取商品列表
        /// </summary>
        /// <returns></returns>
        public List<uint> GetMallItemList()
        {
            List<uint> mallItemList = new List<uint>();
            if (null != infos && infos.Count > 0)
            {
                foreach(GameCmd.DynaStorePosInfo info in infos)
                {
                    mallItemList.Add(info.baseid);
                }
            }
            return mallItemList;
        }

        /// <summary>
        /// 获取商品位置列表
        /// </summary>
        /// <returns></returns>
        public List<uint> GetMallItemPosList()
        {
            List<uint> mallItemPosList = new List<uint>();
            if (null != infos && infos.Count > 0)
            {
                foreach (GameCmd.DynaStorePosInfo info in infos)
                {
                    mallItemPosList.Add(info.pos);
                }
            }
            return mallItemPosList;
        }

        /// <summary>
        /// 排序
        /// </summary>
        public void Sort()
        {
            if (null != infos && infos.Count >1)
            {
                infos.Sort((left, right) =>
                    {
                        return (int)left.pos - (int)right.pos;
                    });
            }
        }
    }

    /// <summary>
    /// 货币数据
    /// </summary>
    public class CurrencyData
    {
        private uint num;
        public uint Num
        {
            get
            {
                return num;
            }
            set
            {
                num = value;
            }
        }
        private GameCmd.MoneyType cType = GameCmd.MoneyType.MoneyType_Invalid;
        public GameCmd.MoneyType CType
        {
            get
            {
                return cType;
            }
            set
            {
                cType = value;
            }
        }

        private ColorType colorType = ColorType.JZRY_Txt_White;
        public ColorType ClorType
        {
            get
            {
                return colorType;
            }
        }

        public CurrencyData(GameCmd.MoneyType cType, uint num,ColorType clType = ColorType.JZRY_Txt_White)
        {
            this.num = num;
            this.cType = cType;
            this.colorType = clType;
        }


    }

    public class MallLocalData
    {
        #region property
        //商品id
        private uint mallItemId;
        //商品标记改变回调
        private Action<uint> m_dlg_onLocalItemTagChanged = null;
        //商品标记
        private MallTagTypeData m_tag = null;
        //商品标记
        public MallDefine.MallTagTypeData Tag
        {
            get
            {
                CheckTagStatus(true);
                return m_tag;
            }
        }
        #endregion
        
        public static MallLocalData Create(uint mallItemId,Action<uint> onLocalItemTagChanged = null)
        {
            MallLocalData data = new MallLocalData();
            return (data.UpdateData(mallItemId, onLocalItemTagChanged)) ? data : null;
        }
        
        /// <summary>
        /// 根据商城id更新商品本地数据类
        /// </summary>
        /// <param name="mallItemId"></param>
        private bool UpdateData(uint mallItemId, Action<uint> onLocalItemTagChanged)
        {
            bool success = false;

            m_localMall = GameTableManager.Instance.GetTableItem<table.StoreDataBase>(mallItemId);
            
            if (null != m_localMall)
            {
                BaseItem local = DataManager.Manager<ItemManager>().GetTempBaseItemByBaseID<BaseItem>(m_localMall.itemId);
                if (null != local)
                {
                    success = true;
                    this.mallItemId = mallItemId;
                    this.m_dlg_onLocalItemTagChanged = onLocalItemTagChanged;
                    StructScheduleInfo();
                    CheckTagStatus(false);
                }
            }
            return success;
        }

        /// <summary>
        /// 刷新购买次数
        /// </summary>
        /// <param name="num"></param>
        public void RefreshPurchaseNum(int num)
        {
            this.m_int_alreadyBuyNum = num;
        }
        public BaseItem LocalItem
        {
            get
            {
                if ( null != LocalMall)
                {
                    return DataManager.Manager<ItemManager>().GetTempBaseItemByBaseID<BaseItem>(LocalMall.itemId);
                }
                return null;
            }
        }
        private StoreDataBase m_localMall = null;
        /// <summary>
        /// 商城属性
        /// </summary>
        public StoreDataBase LocalMall
        {
            get
            {
                if (null == m_localMall)
                {
                    return GameTableManager.Instance.GetTableItem<StoreDataBase>(mallItemId);
                }
                return m_localMall;
                
            }
        }

        /// <summary>
        /// 商品id
        /// </summary>
        public uint MallId
        {
            get
            {
                return (null != LocalMall) ? LocalMall.mallItemId : 0;
            }
        }

        //已经购买次数
        private int m_int_alreadyBuyNum = 0;
        public int AlreadyBuyNum
        {
            get
            {
                return m_int_alreadyBuyNum;
            }
        }

        /// <summary>
        /// 剩余购买次数
        /// </summary>
        public int LeftTimes
        {
            get
            {
                return Math.Max(0, (IsDayNumLimit) ? (DayMaxNum - AlreadyBuyNum) : 0);
            }
        }

        //单次最大购买数量
        public int MaxPurchaseNum
        {
            get
            {
                if (null == LocalItem || null == LocalMall)
                {
                    return 0;
                }
                int maxPurchaseNum = (int)LocalMall.pileNum * LeftTimes;
                if (!IsDayNumLimit || IsDayNumLimit && maxPurchaseNum > LocalItem.MaxPileNum)
                {
                    maxPurchaseNum = (int)(LocalItem.MaxPileNum / LocalMall.pileNum);
                }else
                {
                    maxPurchaseNum = LeftTimes;
                }
                return maxPurchaseNum;
            }
        }

        /// <summary>
        /// 是否有每日限制
        /// </summary>
        public bool IsDayNumLimit
        {
            get
            {
                return (DayMaxNum != 0) ? true : false;
            }
        }

        /// <summary>
        /// 单日最大购买次数
        /// </summary>
        public int DayMaxNum
        {
            get
            {
                return (null != LocalMall) ? (int)LocalMall.dayMaxnum : 0;
            }
        }

        /// <summary>
        /// 是否开启打折
        /// </summary>
        public bool IsOpenDiscount
        {
            get
            {
                return (null != LocalMall && LocalMall.isOff== 1) ? true : false;
            }
        }

        /// <summary>
        /// 当前是否处于打折状态
        /// </summary>
        public bool IsInDiscount
        {
            get
            {
                long leftSeconds = 0;
                if (IsOpenDiscount 
                    && (!HasSchedule || IsTimeInSchedule(DateTimeHelper.Instance.Now, out leftSeconds)))
                {
                    return true;
                }
                return false;
            }
        }

        /// <summary>
        /// 是否有日程信息
        /// </summary>
        public bool HasSchedule
        {
            get
            {
                return (null != LocalMall && !string.IsNullOrEmpty(LocalMall.ScheduleId));
            }
        }

        /// <summary>
        /// 是否下架
        /// </summary>
        public bool IsUnshelve
        {
            get
            {
                return (null != LocalMall && LocalMall.isOut == 1) ? true : false;
            }
        }

        /// <summary>
        /// 排序id
        /// </summary>
        public uint SortId
        {
            get
            {
                return (null != LocalMall) ? LocalMall.sortId : 0;
            }
        }

        /// <summary>
        /// 检测标记状态是否改变
        /// </summary>
        /// <param name="needSendEvent"></param>
        /// <returns></returns>
        public void CheckTagStatus(bool needSendEvent = true)
        {
            MallTagTypeData data = GetTagTypeData();
            if (m_tag != data)
            {
                m_tag = data;
                if (null != m_dlg_onLocalItemTagChanged && needSendEvent)
                {
                    m_dlg_onLocalItemTagChanged.Invoke(mallItemId);
                }
            }
        }

        /// <summary>
        /// 获取tag数据
        /// </summary>
        /// <returns></returns>
        private MallTagTypeData GetTagTypeData()
        {
            MallTagTypeData data = new MallTagTypeData();
            if (null != LocalMall)
            {
                if (IsInDiscount)
                {
                    data.Tag = MallTagType.Discount;
                    data.Value = (int)LocalMall.offPrice;
                }
                else if (LocalMall.isHot != 0)
                {
                    data.Tag = MallTagType.Hot;
                    data.Value = (int)LocalMall.offPrice;
                }
                else if (LocalMall.vipLev != 0)
                {
                    data.Tag = MallTagType.VipNeed;
                    data.Value = (int)LocalMall.vipLev;
                }
                else if (LocalMall.lev != 0)
                {
                    data.Tag = MallTagType.CharacterLv;
                    data.Value = (int)LocalMall.lev;
                }
            }
            return data;
        }

        /// <summary>
        /// 构建日程信息
        /// </summary>
        public void StructScheduleInfo()
        {
            scheduleInfos.Clear();
            table.StoreDataBase store = LocalMall;
            if (null != store && !string.IsNullOrEmpty(store.ScheduleId))
            {
                string[] scheduleIdArray = store.ScheduleId.Split(new char[] { '_' });
                if (null != scheduleIdArray)
                {
                    ScheduleDefine.ScheduleLocalData scheduleTemp = null;
                    for(int i = 0;i < scheduleIdArray.Length;i++)
                    {
                        if (string.IsNullOrEmpty(scheduleIdArray[i]))
                            continue;
                        scheduleTemp = new ScheduleDefine.ScheduleLocalData(uint.Parse(scheduleIdArray[i].Trim()));
                        scheduleInfos.Add(scheduleTemp);
                    }
                }
            }
        }

        /// <summary>
        /// 日程信息
        /// </summary>
        private List<ScheduleDefine.ScheduleLocalData> scheduleInfos = new List<ScheduleDefine.ScheduleLocalData>();
        public List<ScheduleDefine.ScheduleLocalData> ScheduleInfos
        {
           get
            {
                return scheduleInfos;
            }
        }

        /// <summary>
        /// 是否时间seconds在当前商品日程内
        /// </summary>
        /// <param name="seconds">距离1970.1.1.0.0.0</param>
        /// <param name="leftSeconds"> 距离日程结束还剩到少时间</param>
        /// <returns></returns>
        public bool IsTimeInSchedule(long seconds,out long leftSeconds)
        {
            return IsTimeInSchedule(DateTimeHelper.TransformServerTime2DateTime(seconds), out leftSeconds);
        }

        /// <summary>
        /// 是否时间seconds在当前商品日程内
        /// </summary>
        /// <param name="date">当前时间</param>
        /// <param name="leftSeconds">如果在日程内，返回最大剩余时间
        /// ，如果不在日程内返回下一个日程最小等待时间</param>
        /// <returns></returns>
        public bool IsTimeInSchedule(System.DateTime date,out long leftSeconds)
        {
            bool inSchedule = false;
            leftSeconds = 0;
            long scheduleLeftTime = 0;
            long nextScheduleTime = 0;
            if (null != scheduleInfos && scheduleInfos.Count > 0)
            {
                for (int i = 0; i < scheduleInfos.Count;i++ )
                {
                    if (scheduleInfos[i].IsInSchedule(date,out leftSeconds))
                    {
                        inSchedule = true; 
                        if (leftSeconds > scheduleLeftTime)
                        {
                            scheduleLeftTime = leftSeconds;
                        }
                    }else if (leftSeconds > 0 && leftSeconds > nextScheduleTime)
                    {
                        nextScheduleTime = leftSeconds;
                    }
                }
            }
            leftSeconds = (inSchedule) ? scheduleLeftTime : nextScheduleTime;
            return inSchedule;
        }
    }
    #endregion

    #region static method

    /// <summary>
    /// 获取打折描述
    /// </summary>
    /// <param name="txt">价格文本</param>
    /// <param name="addBlank">是否首尾添加空格</param>
    /// <param name="ct">文本颜色</param>
    /// <returns></returns>
    public static string GetDiscountString(string txt,bool addBlank = true,ColorType ct = ColorType.JZRY_Txt_Red)
    {
        StringBuilder builder = new StringBuilder();
        builder.Append("[s]");
        if (addBlank)
        {
            builder.Append(" ");
        }
        builder.Append(txt);
        if (addBlank)
        {
            builder.Append(" ");
        }
        builder.Append("[/s]");
        return ColorManager.GetColorString(ct, builder.ToString());
    }
    /// <summary>
    /// 根据货币类型获取图标
    /// </summary>
    /// <param name="cType"></param>
    /// <returns></returns>
    public static string GetCurrencyIconNameByType(GameCmd.MoneyType cType)
    {
        string iconName = "";
        CurrencyIconData data = CurrencyIconData.GetCurrencyIconByMoneyType((ClientMoneyType)cType);
        if (data != null)
        {
            iconName = data.smallIconName;
        }    
        return iconName;
    }
    #endregion


}