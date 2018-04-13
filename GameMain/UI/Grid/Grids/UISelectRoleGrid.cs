/************************************************************************************
 * Copyright (c) 2017  All Rights Reserved.
 * CLR版本： 4.0.30319.42000
 * 公司名称：ZQB
 * 命名空间：GameMain.UI.Grid.Grids
 * 创建人：  wenjunhua.zqgame
 * 文件名：  UISelectRoleGrid
 * 版本号：  V1.0.0.0
 * 创建时间：7/17/2017 10:30:47 AM
 * 描述：
 ************************************************************************************/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using GameCmd;
using Common;
using Client;

class UISelectRoleGrid : UIGridBase
{
    #region property
    private Transform m_tsNone = null;

    private Transform m_tsCharacterInfo = null;
    private UISpriteEx m_spCIcon = null;
    private Transform m_tsSelectMask = null;
    private UILabel m_labName = null;
    private UILabel m_labLv = null;
    private UISpriteEx m_spSelectBg = null;
    private int m_iIndex = 0;
    private TweenScale m_ts = null;

    private UILabel m_labLeftTime = null;

    public int Index
    {
        get
        {
            return m_iIndex;
        }
    }

    private uint m_uID = 0;
    public uint ID
    {
        get
        {
            return m_uID;
        }
    }

    public bool Empty
    {
        get
        {
            return (m_uID == 0);
        }
    }

    private BoxCollider m_collider = null;
    private GameCmd.SelectUserInfo m_info
    {
        set;
        get;
    }

    private GameObject m_deleteBtn = null;
    #endregion
    #region overridemethod
    protected override void OnAwake()
    {
        base.OnAwake();
        m_ts = CacheTransform.GetComponent<TweenScale>();
        m_collider = CacheTransform.GetComponent<BoxCollider>();
        m_tsNone = CacheTransform.Find("Content/None");
        UIEventListener.Get(m_tsNone.gameObject).onClick = (obj) =>
        {
            InvokeUIDlg(UIEventType.Click, this,null);
        };
        m_tsCharacterInfo = CacheTransform.Find("Content/Info");
        m_spCIcon = CacheTransform.Find("Content/Info/Icon").GetComponent<UISpriteEx>();
        m_spSelectBg = CacheTransform.Find("Content/Info/Bg").GetComponent<UISpriteEx>();
        m_tsSelectMask = CacheTransform.Find("Content/Info/SelectMask");
        m_labLv = CacheTransform.Find("Content/Info/Lv").GetComponent<UILabel>();
        m_labName = CacheTransform.Find("Content/Info/Name").GetComponent<UILabel>();
        m_labLeftTime = CacheTransform.Find("Content/Info/DeleteLeftTime").GetComponent<UILabel>();
        SetTriggerEffect(false);
        m_deleteBtn = CacheTransform.Find("Content/Info/SelectMask/DeleteRoleBtn").gameObject;
        if (m_deleteBtn != null)
        {
            UIEventListener.Get(m_deleteBtn).onClick = onClick_DeleteRoleBtn_Btn;
        }
       
    }
    #endregion

    #region Set
    public void SetData(int index, GameCmd.SelectUserInfo info = null)
    {
        m_iIndex = index;
        m_uID = (null != info) ? info.id : 0;
        m_info = info;
        if (null != m_collider && m_collider.enabled == Empty)
        {
            m_collider.enabled = !Empty;
        }
        if (null != m_tsNone && null != m_tsNone.GetComponent<Collider>())
        {
            m_tsNone.GetComponent<Collider>().enabled = Empty;
        }
        //if (null != m_tsNone)
        //{
        //    if (m_tsNone.gameObject.activeSelf != Empty)
        //        m_tsNone.gameObject.SetActive(Empty);
        //    if (null != m_collider && m_collider.enabled)
        //    {
        //        m_collider.enabled = false;
        //    }
        //}
        if (m_labLeftTime != null)
        {
            if (info != null)
            {
                bool inDelete = info.del == 1;
                m_labLeftTime.gameObject.SetActive(inDelete);
                if (inDelete)
                {

                    m_labLeftTime.text = DataManager.Manager<TextManager>().GetLocalFormatText(LocalTextType.DeleteRole_Tips_5, GetTime(info.delleveltime));
                }
            }


        }
        if (null != m_tsCharacterInfo)
        {
            if (m_tsCharacterInfo.gameObject.activeSelf == Empty)
            {
                m_tsCharacterInfo.gameObject.SetActive(!Empty);
            }
            if (!Empty)
            {
                if (null != m_labLv)
                {
                    m_labLv.text = string.Format("{0}级", info.level);
                }

                if (null != m_labName)
                {
                    m_labName.text = info.name;
                }
                if (null != m_spCIcon)
                {
                    if (m_spCIcon.mSpriteCount == 2)
                    {
                        m_spCIcon.mSpriteList = new string[2];

                        string strIcon = ChooseRolePanel.GetSpriteName(info.type);
                        m_spCIcon.mSpriteList[0] = strIcon;
                        m_spCIcon.mSpriteList[1] = strIcon + "_hui";
                        m_spCIcon.Reset();
                    }
                }
            }
        }
        SetSelect(false, false);
    }

    int GetTime(uint leftseconds)
    {
        int time = Mathf.CeilToInt(leftseconds * 1.0f / 3600);
        return time;
    }
    public void SetSelect(bool select, bool needAnim = true)
    {
        if (null != m_tsSelectMask && m_tsSelectMask.gameObject.activeSelf != select)
        {
            m_tsSelectMask.gameObject.SetActive(select);
        }
        if (null != m_ts)
        {
            if (needAnim)
            {
                m_ts.Play(select);
            }
            else
            {
                m_ts.gameObject.transform.localScale = (select) ? m_ts.to : m_ts.from;
            }
        }

        if (null != m_spCIcon)
        {
            m_spCIcon.ChangeSprite((select) ? 1 : 2);
        }
        if (null != m_spSelectBg)
        {
            m_spSelectBg.ChangeSprite((select) ? 1 : 2);
        }
    }
    #endregion
    void onClick_DeleteRoleBtn_Btn(GameObject caster)
    {
        GameCmd.SelectUserInfo info =m_info;
        if (info == null)
        {
            return;
        }
        string des = "";
        uint limitLv = GameTableManager.Instance.GetGlobalConfig<uint>("DelCharStayMinLevel");
        bool matchLv = info.level >= limitLv;
        bool matchNum = DataManager.Manager<LoginDataManager>().RoleList.Count > 1;
        TextManager Tmger = DataManager.Manager<TextManager>();
        //一个角色不能删除
        if (!matchNum)
        {
            des = Tmger.GetLocalText(LocalTextType.DeleteRole_Tips_4);
            TipsManager.Instance.ShowTipWindow(TipWindowType.Ok, des, null);
            return;
        }
        if (info.del == 1)
        {
            des = Tmger.GetLocalText(LocalTextType.Login_Server_InDelete);
            TipsManager.Instance.ShowTipWindow(TipWindowType.Ok, des, null);
            return;
        }
        //小于30  不可恢复
        if (!matchLv)
        {
            des = Tmger.GetLocalFormatText(LocalTextType.DeleteRole_Tips_1, info.name);
        }
        else
        {
            uint remainHours = GameTableManager.Instance.GetGlobalConfig<uint>("DelCharStayTime");
            des = Tmger.GetLocalFormatText(LocalTextType.DeleteRole_Tips_2, info.name, remainHours, remainHours);
        }

        Action yes = delegate
        {
            stDeleteSelectUserCmd cmd = new stDeleteSelectUserCmd() { charid = info.id };
            NetService.Instance.Send(cmd);

        };
        TipsManager.Instance.ShowTipWindow(TipWindowType.YesNO, des, yes, null, title: Tmger.GetLocalText(LocalTextType.Local_TXT_Tips), okstr: Tmger.GetLocalText(LocalTextType.Local_TXT_Confirm), cancleStr: Tmger.GetLocalText(LocalTextType.Local_TXT_Cancel));
    }
}