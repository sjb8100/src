using UnityEngine;
using System.Collections;
using table;
using Client;

partial class BuyPanel
{
    public delegate void OnBuyBtnClick(int num);

    int minNum = 0;
    int maxNum = 0;

    public class HandInputInitData
    {
        public uint item_id;
        public int price;
        public uint moneyType;
        public int min;
        public int max;
        public OnBuyBtnClick onBuyBtnClick = null;
    }
    HandInputInitData inputInitData = null;

    protected override void OnLoading()
    {
        UIEventListener.Get(m_label_UnitNum.gameObject).onClick = OnNumClick;
        UIEventListener.Get(m_widget_ContainerBox.gameObject).onClick = OnContainerBoxClick;
    }

    protected override void OnPrepareShow(object data)
    {
        base.OnPrepareShow(data);
        inputInitData = data as HandInputInitData;
        UpdateBuyInfo();
    }

    int buyNum = 1;
    int BuyNum
    {
        get
        {
            return buyNum;
        }
        set
        {
            buyNum = value;
            m_label_UnitNum.text = buyNum.ToString();
            if (inputInitData != null)
            {
                int totalMoney =MainPlayerHelper.GetMoneyNumByType((ClientMoneyType)inputInitData.moneyType);
                int buyMoney = buyNum * inputInitData.price;
                m_label_TotalPriceNum.text =  buyMoney.ToString();
                m_label_TotalPriceNum.color = (buyMoney > totalMoney) ? new Color(198 / 255.0f, 28 / 255.0f, 28 / 255.0f) : new Color(9 / 255.0f, 127 / 255.0f, 29 / 255.0f);
            }
        }
    }

    void onClick_Btn_Less_Btn(GameObject caster)
    {
        int tempNum = int.Parse(m_label_UnitNum.text);
        if (tempNum > minNum)
        {
            tempNum -= 1;
        }
        else
        {
            tempNum = minNum;
        }
        BuyNum = tempNum;
    }

    void OnContainerBoxClick(GameObject caster)
    {
        DataManager.Manager<UIPanelManager>().HidePanel(PanelID.BuyPanel);
    }

    void onClick_Btn_Add_Btn(GameObject caster)
    {
        int tempNum = int.Parse(m_label_UnitNum.text);
        if (tempNum < maxNum)
        {
            tempNum += 1;
        }
        else
        {
            tempNum = maxNum;
        }
        BuyNum = tempNum;
    }

    void onClick_Btn_Max_Btn(GameObject caster)
    {
        BuyNum = maxNum;
    }

    void onClick_Btn_Buy_Btn(GameObject caster)
    {
        if (inputInitData != null)
        {
            if (inputInitData.onBuyBtnClick != null)
            {
                int num = int.Parse(m_label_UnitNum.text);
                inputInitData.onBuyBtnClick.Invoke(num);
            }
        }
    }

    void onClick_Btn_Canel_Btn(GameObject caster)
    {
        DataManager.Manager<UIPanelManager>().HidePanel(PanelID.BuyPanel);
    }

    void OnNumClick(GameObject caster)
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
                maxInputNum = (uint)maxNum,
                onInputValue = OnNumConfirm,
                showLocalOffsetPosition = new Vector3(322, -40, 0),
            });
        }
    }

    void OnNumConfirm(int num)
    {
        int tempNum = num;
        if (num > maxNum)
        {
            tempNum = maxNum;
        }
        if (num < minNum)
        {
            tempNum = minNum;
        }

        BuyNum = tempNum;
    }
    private CMResAsynSeedData<CMTexture> m_iconCASD = null;
    public void UpdateBuyInfo()
    {
        if (inputInitData == null)
        {
            return;
        }
        uint item_id = inputInitData.item_id;
        int price = inputInitData.price;
        int min = inputInitData.min;
        int max = inputInitData.max;
        if (min > max)
        {
            return;
        }
        minNum = min;
        maxNum = max;
        m_label_UnitNum.text = min.ToString();
        m_label_TotalPriceNum.text = (min * price).ToString();
        m_label_SellNum.text = max.ToString();

        ItemDataBase itemData = GameTableManager.Instance.GetTableItem<ItemDataBase>(item_id);
        if (itemData != null)
        {
            UIManager.GetTextureAsyn(itemData.itemIcon, ref m_iconCASD, () =>
            {
                if (null != m__Icon)
                {
                    m__Icon.mainTexture = null;
                }
            }, m__Icon);

            m_label_Name.text = itemData.itemName;
            m_label_UnitPrice.text = price.ToString();
        }
        CurrencyIconData data = CurrencyIconData.GetCurrencyIconByMoneyType((ClientMoneyType)inputInitData.moneyType);
        if(data != null)
        {
            m_sprite_moneyIcon.spriteName = data.smallIconName;
            m_sprite_moneyIcon2.spriteName = data.smallIconName;
        }
        
    }

    protected override void OnHide()
    {
        base.OnHide();
        Release();
    }

    public override void Release(bool depthRelease = true)
    {
        base.Release(depthRelease);
        if (null != m_iconCASD)
        {
            m_iconCASD.Release(true);
            m_iconCASD = null;
        }
    }
}
