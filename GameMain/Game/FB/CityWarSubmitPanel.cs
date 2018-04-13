using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;


partial class CityWarSubmitPanel
{
    #region property

    private UIItemInfoGrid m_baseGrid = null;

    uint m_itemBaseId;

    BaseItem baseItem;

    int itemCount;

    uint inputNum;

    #endregion


    #region override

    protected override void OnLoading()
    {
        base.OnLoading();

        InitWidget();


    }

    protected override void OnShow(object data)
    {
        base.OnShow(data);

        if (data != null)
        {
            this.m_itemBaseId = (uint)data;
        }

        //初始化UI
        InitUI();

        RegisterGlobalEvent(true);
    }

    protected override void OnHide()
    {
        base.OnHide();

        RegisterGlobalEvent(false);
    }

    public override bool OnMsg(UIMsgID msgid, object param)
    {

        return true;
    }

    protected override void OnPanelBaseDestory()
    {
        base.OnPanelBaseDestory();


    }

    public override void Release(bool depthRelease = true)
    {
        base.Release(depthRelease);

        if (m_baseGrid != null)
        {
            m_baseGrid.Release(true);
            UIManager.OnObjsRelease(m_baseGrid.CacheTransform, (uint)GridID.Uiiteminfogrid);
            m_baseGrid = null;
        }
    }

    public void RegisterGlobalEvent(bool register)
    {
        if (register)
        {
            Engine.Utility.EventEngine.Instance().AddEventListener((int)Client.GameEventID.UIEVENT_UPDATEITEM, OnGlobalUIItemUpdata);

        }
        else
        {
            Engine.Utility.EventEngine.Instance().RemoveEventListener((int)Client.GameEventID.UIEVENT_UPDATEITEM, OnGlobalUIItemUpdata);

        }
    }

    private void OnGlobalUIItemUpdata(int eventType, object data)
    {
        if (eventType == (int)Client.GameEventID.UIEVENT_UPDATEITEM)
        {
            itemCount = DataManager.Manager<ItemManager>().GetItemNumByBaseId(this.m_itemBaseId);//道具存量
            m_label_ItemNum.text = this.itemCount.ToString();
        }
    }

    #endregion

    #region method

    void InitWidget()
    {
        if (m_trans_IconRoot.childCount == 0)
        {
            GameObject preObj = UIManager.GetResGameObj(GridID.Uiiteminfogrid) as GameObject;
            GameObject cloneObj = NGUITools.AddChild(m_trans_IconRoot.gameObject, preObj);
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

    void InitUI()
    {
        //item
        this.baseItem = new BaseItem(this.m_itemBaseId);
        //count
        itemCount = DataManager.Manager<ItemManager>().GetItemNumByBaseId(this.m_itemBaseId);//道具存量

        InitItem();

        //name
        m_label_ItemNameLabel.text = this.baseItem.Name;
        m_label_name.text = string.Format("上缴{0}", this.baseItem.Name);

        m_label_ItemNum.text = this.itemCount.ToString();

        this.inputNum = 1;
        m_label_takeOutNum.text = this.inputNum.ToString();
        //UpdateInputNum();
    }

    void InitItem()
    {

        if (m_baseGrid != null)
        {
            m_baseGrid.Reset();
            m_baseGrid.SetBorder(true, this.baseItem.BorderIcon);
            m_baseGrid.SetIcon(true, this.baseItem.Icon);
            //m_baseGrid.SetNum(true, this.itemCount.ToString());

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
            DataManager.Manager<UIPanelManager>().ShowPanel(PanelID.GetWayPanel, data: this.m_itemBaseId);
        }
    }

    void OnCloseInput()
    {
        if (inputNum == 0)
        {
            OnHandInput(1);
        }
    }

    private void OnHandInput(int num)
    {
        this.inputNum = (uint)num;

        UpdateInputNum();
    }

    void UpdateInputNum()
    {
        if (this.inputNum >= 0 && this.inputNum <= this.itemCount)
        {
            m_label_takeOutNum.text = this.inputNum.ToString();
        }
    }

    #endregion

    #region click

    void onClick_Btn_queding_Btn(GameObject caster)
    {
        DataManager.Manager<CityWarManager>().ReqCityWarSubmitStatue(this.inputNum);
    }

    void onClick_BtnAdd_Btn(GameObject caster)
    {
        if (this.inputNum <this.itemCount)
        {
            this.inputNum++;
            UpdateInputNum();
        }
    }

    void onClick_BtnRemove_Btn(GameObject caster)
    {
        if (this.inputNum > 1)
        {
            this.inputNum--;
            UpdateInputNum();
        }
    }

    void onClick_BtnMax_Btn(GameObject caster)
    {
        this.inputNum = (uint)this.itemCount;
        UpdateInputNum();
    }

    void onClick_TakeOutNumInput_Btn(GameObject caster)
    {
        UIPanelManager mgr = DataManager.Manager<UIPanelManager>();
        if (mgr.IsShowPanel(PanelID.HandInputPanel))
        {
            mgr.HidePanel(PanelID.HandInputPanel);
        }
        else
        {
            mgr.ShowPanel(PanelID.HandInputPanel, data: new HandInputPanel.HandInputInitData()
            {
                maxInputNum = (uint)itemCount,
                onInputValue = OnHandInput,
                onClose = OnCloseInput,
                showLocalOffsetPosition = new Vector3(350, 0, 0),
            });
        }
    }

    void onClick_Close_Btn(GameObject caster)
    {
        HideSelf();
    }

    #endregion
}

