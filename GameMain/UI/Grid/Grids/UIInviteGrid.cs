/************************************************************************************
 * Copyright (c) 2016  All Rights Reserved.
 * CLR版本： 4.0.30319.42000
 * 公司名称：ZQB
 * 命名空间：GameMain.UI.Grid.Grids
 * 创建人：  wenjunhua.zqgame
 * 文件名：  UIInviteGrid
 * 版本号：  V1.0.0.0
 * 创建时间：10/31/2016 2:32:55 PM
 * 描述：
 ************************************************************************************/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

class UIInviteGrid : UIGridBase
{
    #region property
    //图标
    private UISprite m_sp_spIcon;
    //等级
    private UILabel m_lab_lblLv;
    //名称
    private UILabel m_lab_lblName;
    //邀请
    private GameObject m_obj_btnInvite;

    //用户id
    private uint m_uint_userId;
    public uint UserId
    {
        get
        {
            return m_uint_userId;
        }
    }
    #endregion

    #region overridemethod
    protected override void OnAwake()
    {
        base.OnAwake();
        m_sp_spIcon = this.transform.Find("icon").GetComponent<UISprite>();
        m_lab_lblLv = this.transform.Find("level").GetComponent<UILabel>();
        m_lab_lblName = this.transform.Find("name").GetComponent<UILabel>();
        m_obj_btnInvite = this.transform.Find("btn_invite").gameObject;
        if (null != m_obj_btnInvite)
        {
            UIEventListener.Get(m_obj_btnInvite).onClick = (obj) =>
                {
                    OnBtnClick();
                };
        }
    }
    #endregion

    #region Set
    public void SetData(uint userId,string name,string iconName,uint lv)
    {
        m_uint_userId = userId;
        if (null != m_lab_lblName)
        {
            m_lab_lblName.text = name;
        }
        if (null != m_lab_lblLv)
        {
            m_lab_lblLv.text = lv + "";
        }
    }

    /// <summary>
    /// 点击按钮
    /// </summary>
    private void OnBtnClick()
    {
        InvokeUIDlg(UIEventType.Click, this, true);
    }
    #endregion
}