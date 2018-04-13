using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

//*******************************************************************************************
//	创建日期：	2018-1-31   16:15
//	文件名称：	AccumulativeRechargePanel,cs
//	创 建 人：	Lin Chuang Yuan
//	版权所有：	ZQB
//	说    明：	每日累充
//*******************************************************************************************
partial class AccumulativeRechargePanel
{
    #region property
    private UIGridCreatorBase m_dailyCreator = null;

    private UIGridCreatorBase m_dailyItemCreator = null;

    private UIGridCreatorBase m_weekCreator = null;

    List<uint> m_lstDaily = new List<uint>();

    string[] m_dailyItemArr;

    List<uint> m_lstWeek = new List<uint>();

    uint m_selectDailyId = 0;

    #endregion

    #region override

    protected override void OnInitPanelData()
    {
        base.OnInitPanelData();
    }

    void RegisterEvent(bool b)
    {
        if (b)
        {
            Engine.Utility.EventEngine.Instance().AddEventListener((int)Client.GameEventID.RECHARGEGETREWARD, EventCallback);
        }
        else
        {
            Engine.Utility.EventEngine.Instance().RemoveEventListener((int)Client.GameEventID.RECHARGEGETREWARD, EventCallback);
        }
    }

    void EventCallback(int nEventID, object param)
    {
        if (nEventID == (int)Client.GameEventID.RECHARGEGETREWARD)
        {
            CreateWeek();
        }
    }

    protected override void OnLoading()
    {
        base.OnLoading();

        m_lstDaily.Clear();
        m_lstWeek.Clear();
        m_lstDaily = GameTableManager.Instance.GetGlobalConfigList<uint>("DailyAccumulative");
        m_lstWeek = GameTableManager.Instance.GetGlobalConfigList<uint>("WeekAccumulative");

        InitWidget();
    }

    protected override void OnPrepareShow(object data)
    {
        base.OnPrepareShow(data);
    }

    protected override void OnShow(object data)
    {
        base.OnShow(data);

        RegisterEvent(true);

        CreateUI();


    }

    protected override void OnHide()
    {
        base.OnHide();

        RegisterEvent(false);

        Release();
    }

    protected override void OnPanelBaseDestory()
    {
        base.OnPanelBaseDestory();
    }

    public override void Release(bool depthRelease = true)
    {
        base.Release(depthRelease);
    }

    public override bool OnMsg(UIMsgID msgid, object param)
    {
        if (msgid == UIMsgID.eAccumulativeRecharge)
        {
            CreateWeek();
        }

        return true;
    }



    #endregion

    #region method
    void InitWidget()
    {
        //每日
        m_dailyCreator = m_trans_DailyRoot.GetComponent<UIGridCreatorBase>();
        if (m_dailyCreator == null)
        {
            m_dailyCreator = m_trans_DailyRoot.gameObject.AddComponent<UIGridCreatorBase>();
        }
        if (m_dailyCreator != null)
        {
            m_dailyCreator.gridContentOffset = new UnityEngine.Vector2(0, 0);
            m_dailyCreator.arrageMent = UIGridCreatorBase.Arrangement.Horizontal;
            m_dailyCreator.gridWidth = 155;
            m_dailyCreator.gridHeight = 50;

            m_dailyCreator.RefreshCheck();
            m_dailyCreator.Initialize<UIAccumulativeDailyGrid>(m_trans_UIAccumulativeDailyGrid.gameObject, OnGridDataUpdate, OnGridUIEvent);
        }

        //每日item
        m_dailyItemCreator = m_trans_ItemListRoot.GetComponent<UIGridCreatorBase>();
        if (m_dailyItemCreator == null)
        {
            m_dailyItemCreator = m_trans_ItemListRoot.gameObject.AddComponent<UIGridCreatorBase>();
        }
        if (m_dailyItemCreator != null)
        {
            m_dailyItemCreator.gridContentOffset = new UnityEngine.Vector2(0, 0);
            m_dailyItemCreator.arrageMent = UIGridCreatorBase.Arrangement.Horizontal;
            m_dailyItemCreator.gridWidth = 100;
            m_dailyItemCreator.gridHeight = 100;

            m_dailyItemCreator.RefreshCheck();
            m_dailyItemCreator.Initialize<UIItemRewardGrid>(m_trans_UIItemRewardGrid.gameObject, OnGridDataUpdate, OnGridUIEvent);
        }

        //每周
        m_weekCreator = m_trans_WeekRoot.GetComponent<UIGridCreatorBase>();
        if (m_weekCreator == null)
        {
            m_weekCreator = m_trans_WeekRoot.gameObject.AddComponent<UIGridCreatorBase>();
        }
        if (m_weekCreator != null)
        {
            m_weekCreator.gridContentOffset = new UnityEngine.Vector2(0, 0);
            m_weekCreator.arrageMent = UIGridCreatorBase.Arrangement.Vertical;
            m_weekCreator.gridWidth = 300;
            m_weekCreator.gridHeight = 125;

            m_weekCreator.RefreshCheck();
            m_weekCreator.Initialize<UIAccumulativeWeekGrid>(m_widget_UIAccumulativeWeekGrid.gameObject, OnGridDataUpdate, OnGridUIEvent);
        }

    }


    /// <summary>
    /// grid 跟新数据
    /// </summary>
    /// <param name="data"></param>
    /// <param name="index"></param>
    private void OnGridDataUpdate(UIGridBase data, int index)
    {
        if (data is UIAccumulativeDailyGrid)
        {
            if (m_lstDaily != null && index < m_lstDaily.Count)
            {
                table.RechargeCostDataBase rcDb = GameTableManager.Instance.GetTableItem<table.RechargeCostDataBase>(m_lstDaily[index]);
                if (rcDb == null)
                {
                    return;
                }

                UIAccumulativeDailyGrid grid = data as UIAccumulativeDailyGrid;
                if (grid == null)
                {
                    return;
                }

                grid.SetGridData(m_lstDaily[index]);
                grid.SetName(rcDb.title);
                grid.SetSelect(m_selectDailyId == m_lstDaily[index]);
            }
        }
        else if (data is UIItemRewardGrid)
        {
            UIItemRewardGrid grid = data as UIItemRewardGrid;
            if (grid == null)
            {
                return;
            }

            if (m_dailyItemArr != null && index < m_dailyItemArr.Length)
            {
                string[] itemData = m_dailyItemArr[index].Split('_');
                if (itemData.Length == 2)
                {
                    uint itemId;
                    uint itemNum;
                    if (uint.TryParse(itemData[0], out itemId) == false)
                    {
                        Engine.Utility.Log.Error("表格解析错误找策划！！！");
                    }
                    if (uint.TryParse(itemData[1], out itemNum) == false)
                    {
                        Engine.Utility.Log.Error("表格解析错误找策划！！！");
                    }

                    grid.SetGridData(itemId, itemNum, false, false);

                    //BaseItem baseItem = new BaseItem(itemId);
                    //grid.Reset();
                    //grid.SetGridData(itemId);
                    //grid.SetIcon(true, baseItem.Icon);
                    //grid.SetBorder(true, baseItem.BorderIcon);
                    //grid.SetNum(true, itemNum.ToString());
                    //grid.SetNotEnoughGet(false);
                }
            }
        }
        else if (data is UIAccumulativeWeekGrid)
        {
            if (m_lstWeek != null && index < m_lstWeek.Count)
            {
                table.RechargeCostDataBase rcDb = GameTableManager.Instance.GetTableItem<table.RechargeCostDataBase>(m_lstWeek[index]);
                if (rcDb == null)
                {
                    return;
                }

                UIAccumulativeWeekGrid grid = data as UIAccumulativeWeekGrid;
                if (grid == null)
                {
                    return;
                }

                grid.SetGridData(rcDb.ID);

                string[] itemData = rcDb.reward.Split('_');
                if (itemData.Length == 2)
                {
                    uint itemId;
                    uint itemNum;
                    if (uint.TryParse(itemData[0], out itemId) == false)
                    {
                        Engine.Utility.Log.Error("表格解析错误找策划！！！");
                    }
                    if (uint.TryParse(itemData[1], out itemNum) == false)
                    {
                        Engine.Utility.Log.Error("表格解析错误找策划！！！");
                    }
                    BaseItem baseItem = new BaseItem(itemId);
                    grid.SetIcon(baseItem.Icon);
                    grid.SetBorder(baseItem.BorderIcon);
                    grid.SetNum(itemNum);
                }

                List<GameCmd.WeekRechTimes> weekRechList = DataManager.Manager<ActivityManager>().WeekRechList;
                if (index >= weekRechList.Count)
                {
                    Engine.Utility.Log.Error("服务器没发数据！！！");
                    return;
                }

                string timesDes = string.Format("{0}/{1}", weekRechList[index].times, rcDb.parameter2);
                if (weekRechList[index].times < rcDb.parameter2)
                {
                    timesDes = ColorManager.GetColorString(ColorType.Red, timesDes);
                }
                else
                {
                    timesDes = ColorManager.GetColorString(ColorType.Green, timesDes);
                }
                string name = string.Format(rcDb.title, rcDb.parameter2, timesDes, rcDb.parameter);
                grid.SetName(name);

                List<uint> overIdList = DataManager.Manager<ActivityManager>().OverIDList;

                //state  1:不可领取  2:可以领取  3：已经领取
                if (false == overIdList.Contains(rcDb.ID))
                {
                    grid.SetBtnSelect(weekRechList[index].times >= rcDb.parameter2 ? 2 : 1);
                }
                else
                {
                    grid.SetBtnSelect(3);
                }

            }
        }
    }

    /// <summary>
    /// grid的点击事件
    /// </summary>
    /// <param name="eventType"></param>
    /// <param name="data"></param>
    /// <param name="param"></param>
    private void OnGridUIEvent(UIEventType eventType, object data, object param)
    {
        if (eventType == UIEventType.Click)
        {
            if (data is UIAccumulativeDailyGrid)
            {
                UIAccumulativeDailyGrid grid = data as UIAccumulativeDailyGrid;
                if (grid == null)
                {
                    return;
                }

                SetSelectDailyGrid(grid.Id);
            }
            else if (data is UIItemRewardGrid)
            {

                //TipsManager.Instance.ShowItemTips(grid.id);
            }
            else if (data is UIAccumulativeWeekGrid)
            {
                UIAccumulativeWeekGrid grid = data as UIAccumulativeWeekGrid;
                if (grid == null)
                {
                    return;
                }

                if (param != null)
                {
                    int btnIndex = (int)param;

                    //领取处理
                    if (btnIndex == 1)
                    {
                        NetService.Instance.Send(new GameCmd.stRequstRechargeRewardPropertyUserCmd_CS() { id = grid.Id });
                    }

                    //itemTips
                    if (btnIndex == 2)
                    {
                        table.RechargeCostDataBase rcDb = GameTableManager.Instance.GetTableItem<table.RechargeCostDataBase>(grid.Id);
                        if (rcDb == null)
                        {
                            return;
                        }

                        string[] itemData = rcDb.reward.Split('_');
                        if (itemData.Length == 2)
                        {
                            uint itemId;
                            if (uint.TryParse(itemData[0], out itemId) == false)
                            {
                                Engine.Utility.Log.Error("表格解析错误找策划！！！");
                            }

                            TipsManager.Instance.ShowItemTips(itemId);
                        }
                    }
                }
            }
        }
    }

    void CreateUI()
    {
        //日奖励
        if (m_dailyCreator != null)
        {
            m_dailyCreator.CreateGrids(m_lstDaily.Count);
        }
        if (m_lstDaily.Count > 0)
        {
            SetSelectDailyGrid(m_lstDaily[0]);
        }

        //周奖励
        CreateWeek();

        UpdateRightLabel();
    }

    void CreateWeek()
    {
        if (m_weekCreator != null)
        {
            m_weekCreator.CreateGrids(m_lstWeek.Count);
        }
    }

    void SetSelectDailyGrid(uint dailyId)
    {
        UIAccumulativeDailyGrid grid = m_dailyCreator.GetGrid<UIAccumulativeDailyGrid>(m_lstDaily.IndexOf(this.m_selectDailyId));
        if (grid != null)
        {
            grid.SetSelect(false);
        }

        grid = m_dailyCreator.GetGrid<UIAccumulativeDailyGrid>(m_lstDaily.IndexOf(dailyId));
        if (grid != null)
        {
            grid.SetSelect(true);
        }

        this.m_selectDailyId = dailyId;

        CreateDailyItemList();

        table.RechargeCostDataBase rcDb = GameTableManager.Instance.GetTableItem<table.RechargeCostDataBase>(this.m_selectDailyId);
        if (rcDb == null)
        {
            return;
        }

        UpdateCenterLbl(rcDb.parameter);
    }

    void CreateDailyItemList()
    {
        table.RechargeCostDataBase rcDb = GameTableManager.Instance.GetTableItem<table.RechargeCostDataBase>(this.m_selectDailyId);
        if (rcDb == null)
        {
            return;
        }

        m_dailyItemArr = rcDb.reward.Split(';');
        m_dailyItemCreator.CreateGrids(m_dailyItemArr.Length);
    }

    void UpdateCenterLbl(uint targetRecharge)
    {
        uint dayRecharge = DataManager.Manager<ActivityManager>().DayAllRMB;

        string des = string.Empty;
        if (dayRecharge < targetRecharge)
        {
            des = string.Format("{0}/{1}", dayRecharge, targetRecharge);
            des = ColorManager.GetColorString(ColorType.Red, des);
            des = "今日已累计充值：" + des + "元";
        }
        else
        {
            des = ColorManager.GetColorString(ColorType.Green, "已获赠");
        }

        m_label_centerLabel.text = des;
    }

    /// <summary>
    /// 奖励重置倒计时
    /// </summary>
    void UpdateRightLabel()
    {
        TimeSpan ts = GetSundayDate(System.DateTime.Now);
        m_label_rightLabel.text = "奖励重置倒计时：" + StringUtil.GetLeftTimeStringMin2((int)ts.TotalSeconds);
    }

    public TimeSpan GetSundayDate(DateTime someDate)
    {
        int i = someDate.DayOfWeek - DayOfWeek.Sunday;
        if (i != 0) i = 7 - i;// 因为枚举原因，Sunday排在最前，相减间隔要被7减。
        int leftHours = 24 - someDate.Hour - 1;
        int leftMinute = 60 - someDate.Minute;
        int leftSecond = 60 - someDate.Second;
        TimeSpan ts = new TimeSpan(i, leftHours, leftMinute, leftSecond);
        return ts;
    }

    #endregion

    #region click

    void onClick_Btn_close_Btn(GameObject caster)
    {
        HideSelf();
    }

    void onClick_ByeBtn_Btn(GameObject caster)
    {
        uint jumpId = GameTableManager.Instance.GetGlobalConfig<uint>("AccumulativeJumpId");
        ItemManager.DoJump(jumpId);
    }

    #endregion

}

