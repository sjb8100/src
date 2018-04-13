/************************************************************************************
 * Copyright (c) 2017  All Rights Reserved.
 * CLR版本： 4.0.30319.42000
 * 公司名称：ZQB
 * 命名空间：GameMain.UI.Grid.Grids
 * 创建人：  wenjunhua.zqgame
 * 文件名：  UIExchangeGrid
 * 版本号：  V1.0.0.0
 * 创建时间：8/14/2017 8:27:10 PM
 * 描述：
 ************************************************************************************/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
class UIExchangeGrid : UIItemInfoGridBase
{
    #region property
    //价格
    private Transform costContent;
    private UILabel cost;
    private UITexture costIcon;

    //名称
    private UILabel name;
    private UIToggle m_tg_hightlight;
    //商品id
    private uint exchangedID;
    public uint ExchangedID
    {
        get
        {
            return exchangedID;
        }
    }
    #endregion

    #region overridemethod
    protected override void OnAwake()
    {
        base.OnAwake();
        name = CacheTransform.Find("Content/Name").GetComponent<UILabel>();
        name.color = ColorManager.GetColor32OfType(ColorType.White);
        m_tg_hightlight = CacheTransform.Find("Content/Toggle").GetComponent<UIToggle>();

        costContent = CacheTransform.Find("Content/Cost");
        cost = CacheTransform.Find("Content/Cost/Cost").GetComponent<UILabel>();
        costIcon = CacheTransform.Find("Content/Cost/CostIcon").GetComponent<UITexture>();

        InitItemInfoGrid(CacheTransform.Find("Content/InfoGridRoot/InfoGrid"));
        //SetTriggerEffect(true, (int)UIBase.UITriggerEffectType.Scale);
    }
    CMResAsynSeedData<CMTexture> m_exchangeCASD = null;
    public override void SetGridData(object data)
    {
        base.SetGridData(data);
        ResetInfoGrid();
        if (null == data)
            return;
        this.exchangedID = (uint)data;
        EquipDefine.LocalExchangeDB db = DataManager.Manager<EquipManager>().GetExchangeLocalDB(exchangedID);
        if (null == db)
        {
            return;
        }

        BaseItem tempItem = DataManager.Manager<ItemManager>().GetTempBaseItemByBaseID<BaseItem>(db.TargetID);
        SetIcon(true, tempItem.Icon);
        if (db.TargetNum > 1)
        {
            SetNum(true, db.TargetNum.ToString());
        }
        SetName(tempItem.Name);
        SetBorder(true, tempItem.BorderIcon);
        
        tempItem.UpdateData(db.CostID);
        UIManager.GetTextureAsyn(tempItem.Icon, ref m_exchangeCASD, () =>
            {
                if (null != costIcon)
                {
                    costIcon.mainTexture = null;
                }
            }, costIcon, false);
        if (null != cost)
        {
            cost.text = db.CostNum.ToString();
        }
    }

    public override void SetHightLight(bool hightLight)
    {
        base.SetHightLight(hightLight);
        if (null != m_tg_hightlight && m_tg_hightlight.value != hightLight)
        {
            m_tg_hightlight.value = hightLight;
        }
    }
    public override void Release(bool depthRelease = true)
    {
        base.Release(depthRelease);
        if (null != m_baseGrid)
        {
            m_baseGrid.Release(false);
        }
        if (null != m_exchangeCASD)
        {
            m_exchangeCASD.Release();
            m_exchangeCASD = null;
        }
    }
    #endregion


    #region Set

    private void SetName(string name)
    {
        if (null != this.name)
        {
            this.name.text = name;
        }
    }

    #endregion

    #region UIEventCallBack
    protected override void InfoGridUIEventDlg(UIEventType eventType, object data, object param)
    {
        switch (eventType)
        {
            case UIEventType.Click:
                {
                    if (data is UIItemInfoGrid && exchangedID != 0)
                    {
                        EquipDefine.LocalExchangeDB db = DataManager.Manager<EquipManager>().GetExchangeLocalDB(exchangedID);
                        if (null == db)
                        {
                            return;
                        }
                        base.InfoGridUIEventDlg(eventType, this, param);
                        TipsManager.Instance.ShowItemTips(db.TargetID);
                    }
                }
                break;
        }
    }
    #endregion
}