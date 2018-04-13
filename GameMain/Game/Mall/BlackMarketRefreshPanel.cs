/************************************************************************************
 * Copyright (c) 2016  All Rights Reserved.
 * CLR版本： 4.0.30319.42000
 * 公司名称：ZQB
 * 命名空间：GameMain.Game.Mall
 * 创建人：  wenjunhua.zqgame
 * 文件名：  BlackMarketRefreshPanel
 * 版本号：  V1.0.0.0
 * 创建时间：12/20/2016 9:20:01 AM
 * 描述：
 ************************************************************************************/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
partial class BlackMarketRefreshPanel
{
    #region define
    public class BlackMarkeRefreshData
    {
        public int TotalTimes = 0;
        public int LeftTimes = 0;
        public MallDefine.CurrencyData Data;
    }
    #endregion
    #region property

    #endregion
    #region overridemethod

    protected override void OnLoading()
    {
        base.OnLoading();
        if (null != m_trans_RefreshCost)
        {
            UICurrencyGrid cGrid = m_trans_RefreshCost.GetComponent<UICurrencyGrid>();
            if (null == cGrid)
            {
                m_trans_RefreshCost.gameObject.AddComponent<UICurrencyGrid>();
            }
        }
    }

    protected override void OnShow(object data)
    {
        base.OnShow(data);
        if (null != data && data is BlackMarkeRefreshData)
        {
            BlackMarkeRefreshData rData = data as BlackMarkeRefreshData;
            if (null != m_trans_RefreshCost)
            {
                UICurrencyGrid cGrid = m_trans_RefreshCost.GetComponent<UICurrencyGrid>();
                if (null != cGrid)
                {
                    cGrid.SetGridData(new UICurrencyGrid.UICurrencyGridData(
                    MallDefine.GetCurrencyIconNameByType(rData.Data.CType)
                    , rData.Data.Num));
                }

                if (null != m_label_RefreshTimes)
                {
                    string text = "{0}/{1}";
                    m_label_RefreshTimes.text = string.Format(text
                        ,(rData.LeftTimes > 0) ?

                        ColorManager.GetColorString(ColorType.Green, rData.LeftTimes.ToString())
                        : ColorManager.GetColorString(ColorType.Red, rData.LeftTimes.ToString())
                        ,rData.TotalTimes);
                }
            }
        }
    }
    #endregion

    #region Op

    #endregion

    #region UIEvent
    void onClick_Close_Btn(GameObject caster)
    {
        HideSelf();
    }

    void onClick_BtnRefresh_Btn(GameObject caster)
    {
        HideSelf();
        DataManager.Manager<MallManager>().RefreshMall();
    }

    void onClick_BtnCancel_Btn(GameObject caster)
    {
        HideSelf();
    }
    #endregion
}