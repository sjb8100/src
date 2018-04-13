/************************************************************************************
 * Copyright (c) 2016  All Rights Reserved.
 * CLR版本： 4.0.30319.42000
 * 公司名称：ZQB
 * 命名空间：GameMain.UI.Grid.Grids
 * 创建人：  wenjunhua.zqgame
 * 文件名：  UIClanDecareWaRivalryGrid
 * 版本号：  V1.0.0.0
 * 创建时间：12/23/2016 10:01:02 AM
 * 描述：
 ************************************************************************************/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

class UIClanDecareWaRivalryGrid : UIGridBase
{
    #region property
    private UILabel m_lab_name;
//     private UISprite m_sp_icon;
//     private UILabel m_lab_time;
//     private UILabel m_lab_member;
    private UILabel m_lab_level;
    private UILabel m_lab_Num;
    private uint m_uint_id;

    private GameObject declareBtn;
    public uint Id
    {
        get
        {
            return m_uint_id;
        }
    }
    public string Name
    {
        get;
        set;
    }
    #endregion

    #region overridemethod
    protected override void OnAwake()
    {
        base.OnAwake();
        m_lab_name = CacheTransform.Find("Content/Name").GetComponent<UILabel>();
        m_lab_level = CacheTransform.Find("Content/Level").GetComponent<UILabel>();
        m_lab_Num = CacheTransform.Find("Content/Num").GetComponent<UILabel>();
        declareBtn = CacheTransform.Find("Content/BtnDeclareWar").gameObject;
        UIEventListener.Get(declareBtn).onClick = (obj) =>
        {
            DataManager.Manager<UIPanelManager>().ShowPanel(PanelID.ClanDeclareWarCostPanel
                , data: new ClanDeclareWarCostPanel.ClanDeclareWarCostData()
                {
                    ClanId = Id,
                    ClanName = Name,
                });
        };
    }
    public override void SetGridData(object data)
    {
        base.SetGridData(data);
        if (null == data || !(data is GameCmd.stWarClanInfo))
        {
            return;
        }

        GameCmd.stWarClanInfo info = data as GameCmd.stWarClanInfo;
        m_uint_id = info.clanid;
        Name = info.clanname;
        if (null != m_lab_name)
        {
            m_lab_name.text = info.clanname;
        }
        if (null != m_lab_Num)
        {
            m_lab_Num.text = string.Format("{0}/{1}", info.membernumonline, info.membernum);
        }
        if (null != m_lab_level)
        {
            m_lab_level.text = info.clanlevel.ToString();
        }
    }
    #endregion
}