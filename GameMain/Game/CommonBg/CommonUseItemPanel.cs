using Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using table;
using UnityEngine;
//*******************************************************************************************
//	创建日期：	2016-11-29   17:58
//	文件名称：	CommomUseItemPanel,cs
//	创 建 人：	Lin Chuang Yuan
//	版权所有：	Lin Chuang Yuan
//	说    明：	使用道具通用姐main
//*******************************************************************************************


public delegate void OkDelegate(bool useDianJunAutoBuy = false, int num = 1);

public class UseItemWidnowParam
{
    public string title;
    public uint itemId;
    public OkDelegate ok;
    public Action cancel;
    public Action belowMinCount;  //低于最小设置数
    public Action overMaxCount;  //使用超过道具存量的最大数
    public string oktxt;
    public string canletxt;
    public int LeftUseTimes;    //剩余使用次数
    public bool canUseDianjuan;  //可使用元宝功能
    public bool canSetNum;   //可改变使用数量
}

partial class CommonUseItemPanel
{

    string title;
    uint itemId;
    OkDelegate ok;
    Action cancel;
    Action belowMinCount;  //低于最小设置数
    Action overMaxCount;
    string oktxt;
    string canletxt;
    int leftUseTimes;    //剩余使用次数
    public bool canUseDianjuan;  //可使用元宝功能 是否开启
    public bool canSetNum;


    int num = 1;    //要使用数量
    bool useDianJuan = false;    //使用元宝开启了的 是否勾选(和canUseDianjuan不一样)
    int itemCount;   //当前道具的存量
    int dianjuanCount;//元宝存量
    uint itemPerCostDianjuan;  //每个道具要几个元宝来换
    BaseItem baseItem;
    int availableNum = 0;//可使用数量

    private UIItemInfoGrid m_baseGrid = null;
    protected override void OnLoading()
    {
        base.OnLoading();

        if (m_trans_ItemRoot.childCount == 0)
        {
            GameObject preObj = UIManager.GetResGameObj(GridID.Uiiteminfogrid) as GameObject;
            GameObject cloneObj = NGUITools.AddChild(m_trans_ItemRoot.gameObject, preObj);
            if (null != cloneObj)
            {
                m_baseGrid = cloneObj.GetComponent<UIItemInfoGrid>();
                if (null == m_baseGrid)
                {
                    m_baseGrid = cloneObj.AddComponent<UIItemInfoGrid>();
                }
            }
        }
    }

    protected override void OnShow(object data)
    {
        base.OnShow(data);

        InitItem();
    }

    protected override void OnPanelBaseDestory()
    {
        base.OnPanelBaseDestory();

        if (null != m_baseGrid)
        {
            m_baseGrid.Release(true);
            UIManager.OnObjsRelease(m_baseGrid.CacheTransform, (uint)GridID.Uiiteminfogrid);
            m_baseGrid = null;
        }
    }

    public override void Release(bool depthRelease = true)
    {
        base.Release(depthRelease);
    }

    public void Init(UseItemWidnowParam param)
    {
        this.title = param.title;
        this.itemId = param.itemId;
        this.ok = param.ok;
        this.cancel = param.cancel;
        this.belowMinCount = param.belowMinCount;
        this.overMaxCount = param.overMaxCount;
        this.oktxt = param.oktxt;
        this.canletxt = param.canletxt;
        this.leftUseTimes = param.LeftUseTimes;
        this.canSetNum = param.canSetNum;
        this.canUseDianjuan = param.canUseDianjuan;

        //初始为默认状态
        num = 1;
        useDianJuan = false;
        SetZidongbuzuFalse();
        this.baseItem = new BaseItem(this.itemId);
        if (baseItem == null)
        {
            return;
        }

        itemCount = DataManager.Manager<ItemManager>().GetItemNumByBaseId(this.itemId);//道具存量
        dianjuanCount = MainPlayerHelper.GetMoneyNumByType(ClientMoneyType.YuanBao);   //元宝存量
        PointConsumeDataBase pcDb = GameTableManager.Instance.GetTableItem<PointConsumeDataBase>(this.itemId);
        if (pcDb != null)
        {
            itemPerCostDianjuan = pcDb.buyPrice; //每个道具要几个元宝来换
        }
        else
        {
            Engine.Utility.Log.Info("元宝代消耗表格无数据");
        }

        //初始化item
        InitItem();

        m_label_TitleLabel.text = this.title;
        m_label_Name.text = baseItem.Name;
        m_label_Description.text = baseItem.Des;
        m_btn_Btn_Use.GetComponentInChildren<UILabel>().text = this.oktxt;
        m_btn_Btn_Canel.GetComponentInChildren<UILabel>().text = this.canletxt;
        m_label_UnitNum.text = num.ToString();

        //每日使用次数限制
        if (this.leftUseTimes == -1)   //-1 无使用次数限制
        {
            m_trans_Times.gameObject.SetActive(false);
            this.availableNum = itemCount;
        }
        else
        {
            m_trans_Times.gameObject.SetActive(true);
            this.availableNum = Mathf.Min(this.leftUseTimes, itemCount);
            if (this.leftUseTimes == 0)
            {
                m_label_TimesNum.color = Color.red;
            }

            if (this.leftUseTimes > 0)
            {
                m_label_TimesNum.color = Color.green;
            }

            m_label_TimesNum.text = string.Format("{0}/{1}", this.leftUseTimes, baseItem.DailyUseNum);
        }

        //设置-+功能
        if (this.canSetNum)
        {
            m_btn_Btn_Less.gameObject.SetActive(true);
            m_btn_Btn_Add.gameObject.SetActive(true);
        }
        else
        {
            m_btn_Btn_Less.gameObject.SetActive(false);
            m_btn_Btn_Add.gameObject.SetActive(false);
        }

        //使用元宝功能
        if (this.canUseDianjuan)
        {
            m_trans_buzu.gameObject.SetActive(true);
        }
        else
        {
            m_trans_buzu.gameObject.SetActive(false);
            m_trans_UseCost.gameObject.SetActive(false);
        }
    }

    /// <summary>
    /// 初始化Item  icon信息
    /// </summary>
    void InitItem()
    {
        if (this.itemId == 0)
        {
            return;
        }

        if (m_baseGrid != null)
        {
            if (this.baseItem == null)
            {
                this.baseItem = new BaseItem(this.itemId);
            }

            m_baseGrid.Reset();
            m_baseGrid.SetBorder(true, this.baseItem.BorderIcon);
            m_baseGrid.SetIcon(true, this.baseItem.Icon);
            this.itemCount = DataManager.Manager<ItemManager>().GetItemNumByBaseId(this.itemId);//道具存量
            m_baseGrid.SetNum(true, this.itemCount.ToString());

            //获取途径
            if (itemCount < 1)
            {
                m_baseGrid.SetNotEnoughGet(true);
                m_baseGrid.RegisterUIEventDelegate(UIItemInfoEventDelegate);
            }
            else
            {
                m_baseGrid.SetNotEnoughGet(false);
                m_baseGrid.UnRegisterUIEventDelegate();
            }
        }
    }

    /// <summary>
    /// 点击弹出获取item面板
    /// </summary>
    /// <param name="eventType"></param>
    /// <param name="data"></param>
    /// <param name="param"></param>
    void UIItemInfoEventDelegate(UIEventType eventType, object data, object param)
    {
        if (eventType == UIEventType.Click)
        {
            DataManager.Manager<UIPanelManager>().ShowPanel(PanelID.GetWayPanel, data: this.itemId);
        }
    }

    /// <summary>
    /// 关闭元宝自动补足
    /// </summary>
    void SetZidongbuzuFalse()
    {
        if (this.m_btn_zidongbuzu.GetComponent<UIToggle>().value)
        {
            this.m_btn_zidongbuzu.GetComponent<UIToggle>().isChecked = false;
            m_trans_UseCost.gameObject.SetActive(false);
        }
    }

    /// <summary>
    /// 设置元宝UI
    /// </summary>
    void SetDianjuanUI()
    {
        if (this.canUseDianjuan == false)
        {
            return;
        }

        if (this.m_btn_zidongbuzu.GetComponent<UIToggle>().value)
        {
            m_trans_UseCost.gameObject.SetActive(true);

            uint cost = (uint)this.num * itemPerCostDianjuan;
            if (cost > dianjuanCount)
            {
                m_label_UseCostNum.color = Color.red;
            }
            else
            {
                m_label_UseCostNum.color = Color.white;
            }

            m_label_UseCostNum.text = cost.ToString();
        }
        else
        {
            m_trans_UseCost.gameObject.SetActive(false);
        }
    }

    void SetItemNum(int inputNum)
    {
        this.num = inputNum;

        //如果少于一个
        if (this.num < 1)
        {
            this.num = 1;
            if (this.belowMinCount != null)
            {
                this.belowMinCount.Invoke();
            }
        }

        //大于可使用数
        if (this.num > this.availableNum)
        {
            this.num = this.availableNum;
            if (this.overMaxCount != null)
            {
                this.overMaxCount.Invoke();
            }

            this.num = this.num < 1 ? 1 : this.num;
        }

        m_label_UnitNum.text = num.ToString();

        SetDianjuanUI();
    }

    #region click

    /// <summary>
    /// 减
    /// </summary>
    /// <param name="caster"></param>
    void onClick_Btn_Less_Btn(GameObject caster)
    {
        if (num > 0)
        {
            num--;
        }

        SetItemNum(num);
    }

    /// <summary>
    /// 加
    /// </summary>
    /// <param name="caster"></param>
    void onClick_Btn_Add_Btn(GameObject caster)
    {
        this.num++;

        SetItemNum(this.num);
    }

    /// <summary>
    /// 使用元宝自动补足
    /// </summary>
    /// <param name="caster"></param>
    void onClick_Zidongbuzu_Btn(GameObject caster)
    {
        SetDianjuanUI();
    }

    /// <summary>
    /// 使用按钮
    /// </summary>
    /// <param name="caster"></param>
    void onClick_Btn_Use_Btn(GameObject caster)
    {
        this.useDianJuan = this.m_btn_zidongbuzu.GetComponent<UIToggle>().value;

        if (ok != null)
        {
            ok.Invoke(this.useDianJuan, this.num);
        }
        HideSelf();
    }

    /// <summary>
    /// 取消
    /// </summary>
    /// <param name="caster"></param>
    void onClick_Btn_Canel_Btn(GameObject caster)
    {
        if (cancel != null)
        {
            cancel.Invoke();
        }

        HideSelf();
    }


    void onClick_InputBtnArea_Btn(GameObject caster)
    {
        if (false == this.canSetNum)
        {
            return;
        }

        UIPanelManager mgr = DataManager.Manager<UIPanelManager>();
        if (mgr.IsShowPanel(PanelID.HandInputPanel))
        {
            mgr.HidePanel(PanelID.HandInputPanel);
        }
        else
        {

            mgr.ShowPanel(PanelID.HandInputPanel, data: new HandInputPanel.HandInputInitData()
            {
                maxInputNum = (uint)this.availableNum,
                onInputValue = SetItemNum,
                showLocalOffsetPosition = new Vector3(425, 0, 0),
            });
        }
    }

    void onClick_Btn_Max_Btn(GameObject caster)
    {
        SetItemNum(this.availableNum);
    }

    void onClick_Btn_close_Btn(GameObject caster)
    {
        HideSelf();
    }

    #endregion

}

