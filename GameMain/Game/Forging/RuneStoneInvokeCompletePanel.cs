/************************************************************************************
 * Copyright (c) 2016  All Rights Reserved.
 * CLR版本： 4.0.30319.42000
 * 公司名称：ZQB
 * 命名空间：GameMain.Game.Forging
 * 创建人：  wenjunhua.zqgame
 * 文件名：  RuneStoneInvokeCompletePanel
 * 版本号：  V1.0.0.0
 * 创建时间：11/15/2016 5:12:14 PM
 * 描述：
 ************************************************************************************/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

partial class RuneStoneInvokeCompletePanel
{
    #region property
    //符石id
    private uint m_uint_runeStoneBaseId = 0;
    private UIItemShowGrid m_infoGrid = null;
    #endregion

    #region overridemethod

    protected override void OnLoading()
    {
        base.OnLoading();
        if (null == m_infoGrid && null != m_trans_InfoGridRoot)
        {
            GameObject preObj = UIManager.GetResGameObj(GridID.Uiitemshowgrid) as GameObject;
            if (null != preObj)
            {
                GameObject cloneObj = NGUITools.AddChild(m_trans_InfoGridRoot.gameObject, preObj);
                if (null != cloneObj)
                {
                    m_infoGrid = cloneObj.GetComponent<UIItemShowGrid>();
                    if (null == m_infoGrid)
                    {
                        m_infoGrid = cloneObj.AddComponent<UIItemShowGrid>();
                    }
                    if (null != m_infoGrid && !m_infoGrid.Visible)
                    {
                        m_infoGrid.SetVisible(true);
                    }
                }
            }
        }
    }

    protected override void OnShow(object data)
    {
        base.OnShow(data);
        if (null == data || !(data is uint))
        {
            return;
        }
        m_uint_runeStoneBaseId = (uint) data;
        RuneStone rs = DataManager.Manager<ItemManager>()
                    .GetTempBaseItemByBaseID<RuneStone>(m_uint_runeStoneBaseId, ItemDefine.ItemDataType.RuneStone);
        if (null != m_infoGrid)
        {
            m_infoGrid.SetGridData(rs.BaseId);

            if (null != m_label_RsName)
            {
                m_label_RsName.color = Color.white;
                string txtName = ColorManager.GetColorString(ColorType.JZRY_Txt_Black, "恭喜获得：{0}");
                m_label_RsName.text = string.Format(txtName , rs.Name);
            }
        }
    }

    #endregion

    #region UIEvent
    void onClick_Close_Btn(GameObject caster)
    {
        HideSelf();
    }

    void onClick_Confirm_Btn(GameObject caster)
    {
        HideSelf();
    }
    #endregion
}