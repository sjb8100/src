using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using table;
using UnityEngine;
using GameCmd;
using Client;
partial class ExchangeMoneyPanel:UIPanelBase
{

    private int purchaseNum = 1;
    //兑换文钱单次最大元宝输入量
    int MaxYuanBaoToWenQian = GameTableManager.Instance.GetGlobalConfig<int>("MaxYuanBaoToWenQian");
    ///兑换金币单次最大元宝输入量
    int MaxYuanBaoToGold = GameTableManager.Instance.GetGlobalConfig<int>("MaxYuanBaoToGold");
    //兑换银两单次最大元宝输入量
    int MaxYuanBaoToYinLiang = GameTableManager.Instance.GetGlobalConfig<int>("MaxYuanBaoToYinLiang");
     //单次文钱兑换金币最大额度
    int MaxWenQianToGold = GameTableManager.Instance.GetGlobalConfig<int>("MaxWenQianToGold");
    


    int MaxInputNum = 0;
    int MinInputNum = 0;


    //元宝文钱兑换比例
    int UnitYuanBaoToWenQian = GameTableManager.Instance.GetGlobalConfig<int>("UnitYuanBaoToWenQian");
    //元宝金币兑换比例
    int UnitYuanBaoToJinBi = GameTableManager.Instance.GetGlobalConfig<int>("UnitYuanBaoToJinBi");
    //元宝银两兑换比例
    int UnitYuanBaoToYinLiang = GameTableManager.Instance.GetGlobalConfig<int>("UnitYuanBaoToYinLiang");
    //文钱金币兑换比例
    int UnitWenQianToJinBi = GameTableManager.Instance.GetGlobalConfig<int>("UnitWenQianToJinBi");
    //银两金币兑换比例
    int UnitYinLiangToJinBi = GameTableManager.Instance.GetGlobalConfig<int>("UnitYinLiangToJinBi");


    //每日元宝兑换金币限额
    int YuanBaoToJinBiDayLimit = GameTableManager.Instance.GetGlobalConfig<int>("YuanBaoToJinBiDayLimit");
    //每日元宝兑换文钱限额
    int YuanBaoToWenQianDayLimit = GameTableManager.Instance.GetGlobalConfig<int>("YuanBaoToWenQianDayLimit");
    //每日文钱兑换金币最大额度
    int WenQianToJinBiDayLimit = GameTableManager.Instance.GetGlobalConfig<int>("WenQianToJinBiDayLimit");
    //每日银两兑换金币最大额度
    int YinLiangToJinBiDayLimit = GameTableManager.Instance.GetGlobalConfig<int>("YinLiangToJinBiDayLimit");
    int totalNum = 0;
    protected override void OnLoading()
    {
        base.OnLoading();
    }
    //消耗货币类型
    ClientMoneyType CostMoneyType = ClientMoneyType.Invalid;
    //想兑换的货币类型
    ClientMoneyType ToMoneyType = ClientMoneyType.Invalid;
    CMResAsynSeedData<CMAtlas> m_priceAsynSeed = null;
    CMResAsynSeedData<CMAtlas> m_priceAsynSeed_2 = null;
    CMResAsynSeedData<CMAtlas> m_priceAsynSeed_3 = null;

    protected override void OnShow(object data)
    {
        base.OnShow(data);
        if (data != null)
        {
            CostMoneyType = ClientMoneyType.YuanBao;
            if (data is ClientMoneyType)
            {
                ShowByMoneyType((ClientMoneyType)data);
            }
        }
       
    }
    protected override void OnHide()
    {
        base.OnHide();
    }
    protected override void OnPanelBaseDestory()
    {
        base.OnPanelBaseDestory();
    }

    TextManager tm
    {
        get 
        {
            return DataManager.Manager<TextManager>();
        }
    
    }
     public void ShowByMoneyType(ClientMoneyType type) 
    { 
        m_trans_ChangeGoldRoot.gameObject.SetActive(type == ClientMoneyType.Gold);
        MaxInputNum = type == ClientMoneyType.Gold ? MaxYuanBaoToGold : MaxYuanBaoToYinLiang;
        CurrencyIconData iconData = CurrencyIconData.GetCurrencyIconByMoneyType(type);
        if (iconData != null)
        {
            UIManager.GetAtlasAsyn(iconData.smallIconName, ref m_priceAsynSeed_2, () =>
            {
                if (null != m_sprite_MoneyIcon)
                {
                    m_sprite_MoneyIcon.atlas = null;
                }
            }, m_sprite_MoneyIcon);
        }
        ToMoneyType = type;

        if (type == ClientMoneyType.YinLiang)
        {
            m_label_name.text = "兑换银两";
            CostMoneyType = ClientMoneyType.YuanBao;
        }
        else if (type == ClientMoneyType.Wenqian)
        {
            m_label_name.text = "兑换绑元";
            CostMoneyType = ClientMoneyType.YuanBao;
        }
        else
        {
            m_label_name.text = "兑换金币";
            CostMoneyType = ClientMoneyType.YuanBao;
        }

        CostByMoneyType(CostMoneyType);
    }

     void CostByMoneyType(ClientMoneyType type) 
     {
         CostMoneyType = type;
         UpdatePurchaseNum(1);       
         int haveYuanBaoNum = MainPlayerHelper.GetMoneyNumByType(ClientMoneyType.YuanBao);
         int haveWenQianNum = MainPlayerHelper.GetMoneyNumByType(ClientMoneyType.Wenqian);
         int haveYinLiangNum = MainPlayerHelper.GetMoneyNumByType(ClientMoneyType.YinLiang);  
         CurrencyIconData iconData = CurrencyIconData.GetCurrencyIconByMoneyType(type);
         if (iconData != null)
         {
             UIManager.GetAtlasAsyn(iconData.smallIconName, ref m_priceAsynSeed, () =>
             {
                 if (null != m_sprite_CostIcon)
                 {
                     m_sprite_CostIcon.atlas = null;
                 }
             }, m_sprite_CostIcon);
         }
         m_btn_YuanBao.GetComponent<UIToggle>().value = type == ClientMoneyType.YuanBao;
         m_btn_YinLiang.GetComponent<UIToggle>().value = type == ClientMoneyType.YinLiang;    
         
     }

    #region  NGUIBTNS
     void onClick_YuanBao_Btn(GameObject caster)
     {
         CostByMoneyType(ClientMoneyType.YuanBao);
     }

     void onClick_YinLiang_Btn(GameObject caster)
     {
         CostByMoneyType(ClientMoneyType.YinLiang);
     }

    void onClick_Close_Btn(GameObject caster)
    {
        this.HideSelf();
    }

    void onClick_Quxiao_Btn(GameObject caster)
    {
        this.HideSelf();
    }

    void onClick_Queding_Btn(GameObject caster)
    {
        if (isNumber(m_label_UnitNum.text))
        {
            NetService.Instance.Send(new stRechargeCoinPropertyUserCmd_CS() 
            { recharge_type = (MoneyType)CostMoneyType, 
              recharge_num = (uint)purchaseNum,
              target_type = (MoneyType)ToMoneyType,
            });
        }
        else 
        {
            TipsManager.Instance.ShowTips("请输入元宝数量");
        }
       
    }

    void onClick_InputBtnArea_Btn(GameObject caster)
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
                maxInputNum = (uint)MaxInputNum,
                onInputValue = onClick_HandInputBtnConfirm_Btn,
                showLocalOffsetPosition = new Vector3(425, 0, 0),
            });
        }
    }

    void onClick_HandInputBtnConfirm_Btn(int num)
    {
        num = Math.Min(MaxInputNum, Math.Max(1, num));
        UpdatePurchaseNum(num);
    }

 
    public override void ResetPanel()
    {
        base.ResetPanel();
        purchaseNum = 1;
    }
    public void UpdatePurchaseNum(int num)
    {
        purchaseNum = num;
        purchaseNum = Math.Max(1, purchaseNum);
        m_label_UnitNum.text = purchaseNum.ToString();
        m_label_CostNum.text = purchaseNum.ToString();
        if (CostMoneyType == ClientMoneyType.Wenqian)
        {
            totalNum = purchaseNum * UnitWenQianToJinBi;
        }
        else if (CostMoneyType == ClientMoneyType.YinLiang)
        {
            totalNum = purchaseNum * UnitYinLiangToJinBi;
        }
        else
        {
            if (ToMoneyType == ClientMoneyType.YinLiang)
            {
                totalNum = purchaseNum * UnitYuanBaoToYinLiang;
            }
            else if (ToMoneyType == ClientMoneyType.Wenqian)
            {
                totalNum = purchaseNum * UnitYuanBaoToWenQian;
            }
            else
            {
                totalNum = purchaseNum * UnitYuanBaoToJinBi;
            }
              
        }
        m_label_MoneyNum.text = totalNum.ToString();
    }
    public bool isNumber(string text)
    {
        if (string.IsNullOrEmpty(text))
        {
            return false;
        }
        foreach (char c in text)
        {
            if (!char.IsDigit(c))
            {
                return false;
            }
        }
        return true;
    }
    #endregion

    void onClick_Btn_Less_Btn(GameObject caster)
    {
        int tempNum = int.Parse(m_label_UnitNum.text);
        if (tempNum > MinInputNum)
        {
            tempNum -= 1;
        }
        else
        {
            tempNum = MinInputNum;
        }
        UpdatePurchaseNum(tempNum);
    }

    void onClick_Btn_Add_Btn(GameObject caster)
    {
        int tempNum = int.Parse(m_label_UnitNum.text);
        if (tempNum < MaxInputNum)
        {
            tempNum += 1;
        }
        else
        {
            tempNum = MaxInputNum;
        }
        UpdatePurchaseNum(tempNum);
    }

    void onClick_Btn_Max_Btn(GameObject caster)
    {
        UpdatePurchaseNum(MaxInputNum);
    }

    protected override void OnJump(UIPanelBase.PanelJumpData jumpData)
    {
        if (null == jumpData)
        {
            jumpData = new PanelJumpData();
        }
        if (null != jumpData.Param && jumpData.Param is uint)
        {
            uint type = (uint)jumpData.Param;
            ToMoneyType = (ClientMoneyType)type;
            ShowByMoneyType(ToMoneyType);
        }
    }

}

