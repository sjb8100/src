/************************************************************************************
 * Copyright (c) 2016  All Rights Reserved.
 * CLR版本： 4.0.30319.42000
 * 公司名称：ZQB
 * 命名空间：GameMain.UI.Grid.Grids
 * 创建人：  wenjunhua.zqgame
 * 文件名：  UIClanMemberGrid
 * 版本号：  V1.0.0.0
 * 创建时间：10/24/2016 4:22:46 PM
 * 描述：
 ************************************************************************************/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

class UIClanMemberGrid : UIGridBase
{
    #region property
    private uint  m_uint_userId;
    public uint UserId
    {
        get
        {
            return m_uint_userId;
        }
    }
    //职业
    private UISprite m_pro;
    //自己标识
    private UISprite m_sp_selfMask;
    //名称
    private UILabel m_name;
    //等级
    private UILabel m_lv;
    //声望
    private UILabel m_honor;
    //职务
    private UILabel m_duty;
    //入族时间
    private UILabel m_joinTime;
    //离线时间
    private UILabel m_outlineTime;
    //背景
    private UIToggle m_tg_hightlight = null;

    UISprite background1 = null;
    UISprite background2 = null;
    #endregion

    #region overridemethod
    protected override void OnAwake()
    {
        base.OnAwake();
        m_sp_selfMask = CacheTransform.Find("Content/Mask").GetComponent<UISprite>();
        m_pro = CacheTransform.Find("Content/Prof").GetComponent<UISprite>();
        m_name = CacheTransform.Find("Content/Name").GetComponent<UILabel>();
        m_lv = CacheTransform.Find("Content/Lv").GetComponent<UILabel>();
        m_honor = CacheTransform.Find("Content/Honor").GetComponent<UILabel>();
        m_duty = CacheTransform.Find("Content/Duty").GetComponent<UILabel>();
        m_joinTime = CacheTransform.Find("Content/JoinTime").GetComponent<UILabel>();
        m_outlineTime = CacheTransform.Find("Content/OutLineTime").GetComponent<UILabel>();
        m_tg_hightlight = CacheTransform.Find("Content/Toggle").GetComponent<UIToggle>();
        background1 = CacheTransform.Find("Content/Toggle/Background").GetComponent<UISprite>();
        background2 = CacheTransform.Find("Content/Toggle/Background2").GetComponent<UISprite>();
    }

    public override void SetGridData(object data)
    {
        base.SetGridData(data);
        if (null != data)
        {
            GameCmd.stClanMemberInfo m_data = (GameCmd.stClanMemberInfo)data;
            m_uint_userId = m_data.id;
            if (null != m_pro)
            {
                m_pro.spriteName = DataManager.GetIconByProfession(m_data.job);
            }
            if (null != m_name)
            {
                m_name.text = m_data.name + "";
            }
            if (null != m_lv)
            {
                m_lv.text =m_data.level + "";
            }
            if (null != m_honor)
            {
                //m_honor.text = string.Format("{0}/{1}/{2}",m_data.credit,m_data.credit_7day,m_data.credit_total);
                m_honor.text = m_data.credit_total.ToString();
            }
            if (null != m_duty)
            {
                m_duty.text = DataManager.Manager<ClanManger>().GetNameByClanDuty(m_data.duty);
            }
            if (null != m_joinTime)
            {
//                 DateTime date = new DateTime(1970, 1, 1, 0, 0, 0);
//                 date = date.AddSeconds(m_data.join_time);
//                 m_joinTime.text = date.ToString("yyyy-MM-dd");
                m_joinTime.text = m_data.fight.ToString();
            }
            if (null != m_outlineTime)
            {
                if (m_data.is_online == 1)
                {
                    m_outlineTime.text = "在线";

                }else
                {
                    DateTime date = new DateTime(1970, 1, 1, 0, 0, 0);
                    date = date.AddSeconds(m_data.offline_time);
                    m_outlineTime.text = date.ToString("yyyy-MM-dd"); ;
                }
            }
            MaskSelf();
        }
    }
    
    private bool IsSelf()
    {
        return DataManager.Instance.UserId == m_uint_userId;
    }
    private void MaskSelf()
    {
        bool isself = IsSelf();
        if (null != m_sp_selfMask && m_sp_selfMask.gameObject.activeSelf != isself)
        {
            m_sp_selfMask.gameObject.SetActive(IsSelf());
        }
//         if (null != m_tg_hightlight && m_tg_hightlight.gameObject.activeSelf == isself)
//         {
//             m_tg_hightlight.gameObject.SetActive(!isself);
//         }
    }
    public override void SetHightLight(bool hightLight)
    {
        base.SetHightLight(hightLight);
        if (!IsSelf() && null != m_tg_hightlight && m_tg_hightlight.value != hightLight)
        {
            m_tg_hightlight.value = hightLight;
        }
    }

    public void SetBackGround(int index) 
    {
        if (background1 != null && background2 != null)
        {
            bool value = index % 2 == 0;
            background1.gameObject.SetActive(value);
            background2.gameObject.SetActive(!value);
        }
    }
    #endregion
}