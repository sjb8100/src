/************************************************************************************
 * Copyright (c) 2016  All Rights Reserved.
 * CLR版本： 4.0.30319.42000
 * 公司名称：ZQB
 * 命名空间：GameMain.UI.Grid.Grids
 * 创建人：  wenjunhua.zqgame
 * 文件名：  UIFashionGrid
 * 版本号：  V1.0.0.0
 * 创建时间：11/29/2016 5:37:27 PM
 * 描述：
 ************************************************************************************/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using table;
class UIFashionGrid : UIGridBase
{
    #region define
    enum FashionGridFunction
    {
        None = 0,
        Wear,
        Unload,
        Discard,
        Preview,
        Buy,
    }
    #endregion

    #region property
    //名称
    private UILabel m_lab_name;
    //图标
    private UITexture m_sp_icon;
    //背景
    private UISprite m_sp_bg;
    private UISprite m_high_bg;
    //装备标记
    private GameObject m_obj_equipMark;
    //购买按钮
    private GameObject m_obj_buy;
    private UILocalText m_lt_btnName;

    UILabel m_label_countdown;
    Transform m_trans_priceContent;
    UILabel m_label_price;
    UISprite m_sprMoney;
    //id
    private uint m_uint_id;
    public uint Data
    {
        get
        {
            return m_uint_id;
        }
    }
    //按钮功能
    private FashionGridFunction m_em_f = FashionGridFunction.None;
    //
    SuitDataBase m_suitDataBase = null;
    ClientSuitData m_CurSuitData;

    public uint SuitBaseID = 0;
    #endregion

    #region overridemethod
    protected override void OnAwake()
    {
        base.OnAwake();

        m_lab_name = CacheTransform.Find("Content/FashionName").GetComponent<UILabel>();
        m_sp_bg = CacheTransform.Find("Content/Bg").GetComponent<UISprite>();
        m_high_bg =  CacheTransform.Find("Content/hightbg").GetComponent<UISprite>();
        if(m_high_bg != null)
        {
            m_high_bg.gameObject.SetActive(false);
        }
        m_sp_icon = CacheTransform.Find("Content/IconContent/IconBg/Icon").GetComponent<UITexture>();
        m_obj_equipMark = CacheTransform.Find("Content/SignEquipped").gameObject;
        m_trans_priceContent = CacheTransform.Find("Content/Time/pricecontent").transform;

        if (m_trans_priceContent != null)
        {
            m_label_price = m_trans_priceContent.Find("pricelabel").GetComponent<UILabel>();
            m_sprMoney = m_trans_priceContent.GetComponent<UISprite>();
        }
        m_label_countdown = CacheTransform.Find("Content/Time/ContDown").GetComponent<UILabel>();
        UIEventListener.Get(this.gameObject).onClick = (obj) =>
            {
                OnBtnClicked();
            };
    }
    private CMResAsynSeedData<CMTexture> m_iconCASD = null;
    public override void SetGridData(object data)
    {
        base.SetGridData(data);
        if (data != null && data is ClientSuitData)
        {
            m_CurSuitData = (ClientSuitData)data;
            SuitBaseID = m_CurSuitData.suitBaseID;
            m_suitDataBase = GameTableManager.Instance.GetTableItem<SuitDataBase>(m_CurSuitData.suitBaseID, 1);
            if (m_suitDataBase == null)
            {
                Engine.Utility.Log.Error("m_suitDataBase is null id is " + m_CurSuitData.suitBaseID);
                return;
            }
            if (null != m_lab_name)
            {
                m_lab_name.text = m_suitDataBase.name;
            }
            if (m_sp_icon != null)
            {
                UIManager.GetTextureAsyn(m_suitDataBase.icon, ref m_iconCASD, () =>
                    {
                        if (null != m_sp_icon)
                        {
                            m_sp_icon.mainTexture = null;
                        }
                    }, m_sp_icon);
            }
            if (m_CurSuitData.suitState == SuitState.NoBuy)
            {
                SetPrice();
                m_trans_priceContent.gameObject.SetActive(true);
                m_label_countdown.gameObject.SetActive(false);
            }
            else
            {
                m_trans_priceContent.gameObject.SetActive(false);

                m_label_countdown.gameObject.SetActive(true);
                SetCountDown(m_CurSuitData.leftTime);
            }
          
             ShowActiveSign(m_CurSuitData.suitState);
          
        }

    }
    void ShowActiveSign(SuitState st)
    {
        if (m_obj_equipMark != null)
        {
            m_obj_equipMark.SetActive(true);
            UISprite spr = m_obj_equipMark.GetComponent<UISprite>();
            if(spr == null)
            {
                return;
            }
            if(st == SuitState.Active)
            {
                spr.spriteName = "jiaobiao_yijihuo";
            }
            else if(st == SuitState.Equip)
            {
                spr.spriteName = "jiaobiao_yizhuangbei2";
            }
            else if (st == SuitState.HasBuy)
            {
                spr.spriteName = "jiaobiao_yiyongyou";
            }
            else if(st == SuitState.NoEffect)
            {
                spr.spriteName = "jiaobiao_yishixiao";
            }
            else if (st == SuitState.Show)
            {
                spr.spriteName = "jiaobiao_weijihuo";
            }
            else 
            {
                m_obj_equipMark.SetActive(false);
            }
        }
    }
    void SetPrice()
    {
        string moneyStr = m_suitDataBase.buyPrice;
        List<uint> typeMoney = StringUtil.GetSplitStringList<uint>(moneyStr, '_');
        if (typeMoney.Count == 2)
        {
            int type = (int)typeMoney[0];
            uint cost = typeMoney[1];

            m_sprMoney.spriteName = MainPlayerHelper.GetMoneyIconByType(type);
            m_label_price.text = cost.ToString();
        }
    }
    void SetCountDown(uint leftTime)
    {
        long time = leftTime - DateTimeHelper.Instance.Now;
        if (leftTime == 0)
        {
            time = 0;
        }
        if (m_label_countdown != null)
        {
            m_label_countdown.text = DataManager.Manager<SuitDataManager>().GetLeftTimeStringMin((int)time);
        }
    }

    public override void SetHightLight(bool hightLight)
    {
        base.SetHightLight(hightLight);
     

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
    #endregion

    #region Op
    /// <summary>
    /// 格子点击
    /// </summary>
    private void OnBtnClicked()
    {
        if (m_suitDataBase == null)
        {
            Engine.Utility.Log.Error("m_suitdatabase is null");
            return;
        }
        SuitDataManager dm = DataManager.Manager<SuitDataManager>();
     
        DataManager.Manager<UIPanelManager>().ShowPanel(PanelID.FashionTips, data: m_suitDataBase);
        dm.CurSuitDataBase = m_suitDataBase;
        dm.SendChangeRenderObj(m_suitDataBase.base_id, (int)dm.CurSuitType,MainPlayerHelper.GetPlayerID());

    }

    /// <summary>
    /// 设置格子按钮功能
    /// </summary>
    private void SetGridFuc()
    {
        LocalTextType key = LocalTextType.LocalText_None;
        switch (m_em_f)
        {
            case FashionGridFunction.Wear:
                key = LocalTextType.Local_TXT_Wear;
                break;
            case FashionGridFunction.Discard:
                key = LocalTextType.Local_TXT_Discard;
                break;
            case FashionGridFunction.Unload:
                key = LocalTextType.Local_TXT_Unload;
                break;
            case FashionGridFunction.Buy:
                key = LocalTextType.Local_TXT_Buy;
                break;
            default:

                break;
        }

    }

    /// <summary>
    /// 设置选中
    /// </summary>
    /// <param name="select"></param>
    public void SetSelect(bool select)
    {
        if (m_high_bg != null)
        {
            m_high_bg.gameObject.SetActive(select);
        }
    }
    #endregion
}