/************************************************************************************
 * Copyright (c) 2016  All Rights Reserved.
 * CLR版本： 4.0.30319.42000
 * 公司名称：ZQB
 * 命名空间：GameMain.UI.Grid.Grids
 * 创建人：  wenjunhua.zqgame
 * 文件名：  UIClanDonateGrid
 * 版本号：  V1.0.0.0
 * 创建时间：10/24/2016 4:24:25 PM
 * 描述：
 ************************************************************************************/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
class UIClanDonateGrid : UIGridBase
{
    #region property
    //名称
    private UILabel m_lab_name;
    //声望
    private UILabel m_lab_sw;
    //族贡
    private UILabel m_lab_zg;
    //资金
    private UILabel m_lab_zj;
    //剩余次数
    private UILabel m_lab_leftTimes;
    //捐赠货币图标
    private UISprite m_sp_donateIcon;
    //捐赠数量
    private UILabel m_lab_donateNum;
    //捐赠图标
    private UISprite m_icon;
    //数据
    private ClanDefine.LocalClanDonateDB m_data;
    private Transform headIcon;
    public ClanDefine.LocalClanDonateDB Data
    {
        get
        {
            return m_data;
        }
    }
    //捐献
    private GameObject m_obj_btnDonate;

    Transform m_trans_UIItemRewardGrid; 
     UIGridCreatorBase m_ctor;
    #endregion

    #region overridemethod
    protected override void OnAwake()
    {
        base.OnAwake();

        m_lab_name = CacheTransform.Find("Content/Title/TitleText").GetComponent<UILabel>();
        m_lab_sw = CacheTransform.Find("Content/Get/GetSW/Value").GetComponent<UILabel>();
        m_lab_zg = CacheTransform.Find("Content/Get/GetZG/Value").GetComponent<UILabel>();
        m_lab_zj = CacheTransform.Find("Content/Get/GetZJ/Value").GetComponent<UILabel>();
        m_lab_leftTimes = CacheTransform.Find("Content/Bottom/LeftTimes").GetComponent<UILabel>();
        m_sp_donateIcon = CacheTransform.Find("Content/Bottom/Currency/Content/Icon").GetComponent<UISprite>();
        m_lab_donateNum = CacheTransform.Find("Content/Bottom/Currency/Content/Num").GetComponent<UILabel>();
        m_icon = CacheTransform.Find("Content/BG/BG").GetComponent<UISprite>();
        m_obj_btnDonate = CacheTransform.Find("Content/Bottom/Donate").gameObject;
        headIcon = CacheTransform.Find("Content/headIcon");
        if (null != m_obj_btnDonate)
        {
            UIEventListener.Get(m_obj_btnDonate).onClick = (obj) =>
                {
                    if (null != donateAction && null != m_data)
                    {
                        donateAction.Invoke(m_data.ID);
                    }
                };
        }
        m_trans_UIItemRewardGrid = CacheTransform.Find("Content/UIItemRewardGrid");
        AddCreator(headIcon);
    }

    public override void SetGridData(object data)
    {
        base.SetGridData(data);
        if (null != data)
        {
            this.m_data = data as ClanDefine.LocalClanDonateDB;
            if (null != m_lab_name)
            {
                m_lab_name.text = m_data.Name;
            }
            if (null != m_lab_sw)
            {
                m_lab_sw.text = m_data.SW + "";
            }
            if (null != m_lab_zg)
            {
                m_lab_zg.text = m_data.ZG + "";
            }
            if (null != m_lab_zj)
            {
                m_lab_zj.text = m_data.ZJ + "";
            }
            if (null != m_lab_leftTimes)
            {
                string leftText = (m_data.LeftTimes > 0) ? ColorManager.GetColorString(ColorType.Green, m_data.LeftTimes.ToString())
                    : ColorManager.GetColorString(ColorType.Red, m_data.LeftTimes.ToString());
                leftText += "/" + m_data.TotalTimes;
                m_lab_leftTimes.text = string.Format("({0})"
                    , leftText);
            }
            if (null != m_icon)
            {
                
            }
            string iconName = MallDefine.GetCurrencyIconNameByType((GameCmd.MoneyType)m_data.DonateType);
            UIManager.GetAtlasAsyn(iconName, ref m_playerAvataCASD, () =>
            {
                if (null != m_sp_donateIcon)
                {
                    m_sp_donateIcon.atlas = null;
                }
            }, m_sp_donateIcon);
          
            if (null != m_lab_donateNum)
            {
                m_lab_donateNum.text = m_data.DonateNum + "";
            }
            if(null != headIcon)
            {
               list.Clear();
                CurrencyIconData curr = CurrencyIconData.GetCurrencyIconByMoneyType((ClientMoneyType)m_data.DonateType);
                if(curr != null)
                {
                     list.Add(new UIItemRewardData() 
                   {
                       itemID = curr.itemID,
                       num = 0,
                   });
                }
                  m_ctor.CreateGrids(list.Count);
            }
        }
    }
    CMResAsynSeedData<CMAtlas> m_playerAvataCASD = null;

    List<UIItemRewardData> list = new List<UIItemRewardData>();
    void AddCreator(Transform parent)
    {
        if (parent != null)
        {
            m_ctor = parent.GetComponent<UIGridCreatorBase>();
            if (m_ctor == null)
            {
                m_ctor = parent.gameObject.AddComponent<UIGridCreatorBase>();
            }
            m_ctor.arrageMent = UIGridCreatorBase.Arrangement.Horizontal;
            m_ctor.gridWidth = 90;
            m_ctor.gridHeight = 90;
            m_ctor.RefreshCheck();
            m_ctor.Initialize<UIItemRewardGrid>(m_trans_UIItemRewardGrid.gameObject, OnUpdateGridData, null);
        }
    }
    void OnUpdateGridData(UIGridBase grid, int index)
    {
        if (grid is UIItemRewardGrid)
        {
            UIItemRewardGrid itemShow = grid as UIItemRewardGrid;
            if (itemShow != null)
            {
                if (index < list.Count)
                {
                    UIItemRewardData data = list[index];
                    uint itemID = data.itemID;
                    uint num = data.num;
                    itemShow.SetGridData(itemID, num, false, false, false);
                }
            }
        }
    }

    public override void Release(bool depthRelease = true)
    {
        base.Release(depthRelease);
        if (m_playerAvataCASD != null)
        {
            m_playerAvataCASD.Release(depthRelease);
            m_playerAvataCASD = null;
        }
    }
    private Action<uint> donateAction = null;
    public void RegisterDonateAction(Action<uint> donateAction)
    {
        if (null == this.donateAction)
        {
            this.donateAction = donateAction;
        }
    }
    #endregion
}