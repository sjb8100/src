/************************************************************************************
 * Copyright (c) 2016  All Rights Reserved.
 * CLR版本： 4.0.30319.42000
 * 公司名称：ZQB
 * 命名空间：GameMain.UI.Grid.Grids
 * 创建人：  wenjunhua.zqgame
 * 文件名：  UIClanDeclareWarGrid
 * 版本号：  V1.0.0.0
 * 创建时间：12/23/2016 10:01:14 AM
 * 描述：
 ************************************************************************************/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

class UIClanDeclareWarGrid : UIGridBase
{
    #region
    private UILabel m_lab_name;
    private UILabel m_lab_lv;
    private UILabel state;
    private uint m_uint_id;
    private GameObject declareBtn;
    UILabel time;
    UISprite select;
    UISprite one;
    UISprite two;
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
    public int Index { set; get; }
    private long m_l_endTime = 0;
    #endregion

    protected override void OnAwake()
    {
        base.OnAwake();
        m_lab_name = CacheTransform.Find("Content/Name").GetComponent<UILabel>();
        m_lab_lv = CacheTransform.Find("Content/Level").GetComponent<UILabel>();
        state = CacheTransform.Find("Content/State").GetComponent<UILabel>();
        time = CacheTransform.Find("Content/Time").GetComponent<UILabel>();
        select = CacheTransform.Find("Content/select").GetComponent<UISprite>();
        one = CacheTransform.Find("Content/Bg/one").GetComponent<UISprite>();
        two = CacheTransform.Find("Content/Bg/two").GetComponent<UISprite>(); 
    }
    public override void SetGridData(object data)
    {
        base.SetGridData(data);
        if (null == data || !(data is GameCmd.stWarClanInfo))
        {
            return;
        }
//        m_f_refreshGap = 0;
        GameCmd.stWarClanInfo info = data as GameCmd.stWarClanInfo;
        m_uint_id = info.clanid;
        Name = info.clanname;
        if (null != m_lab_name)
        {
            m_lab_name.text = info.clanname;
        }
//         //宣战还是迎战
        if (null != state)
        {
            state.text = info.is_warstart ? "宣战" : "迎战";
        }
        if (null != m_lab_lv)
        {
            m_lab_lv.text = info.clanlevel.ToString();
        }
        if(null != time)
        {
            time.text = "";
        }  
         m_l_endTime = info.endtime;
         if (null != time)
        {
            long now = DateTimeHelper.Instance.Now;
            long seconds = 0;
            ScheduleDefine.ScheduleUnit.IsInCycleDateTme(now, info.endtime, now
                , out seconds);
            time.text = DateTimeHelper.ParseTimeSeconds((int)seconds);
        }
        
    }
    public void SetSelect(bool enable) 
    {
        if (select != null)
        {
            select.gameObject.SetActive(enable);
        }
    }
    public void SetIndex(int index) 
    {
        Index = index;
        if(one == null || two == null)
        {
            return;
        }
        bool value = index % 2 == 0;
        one.gameObject.SetActive(value);
        two.gameObject.SetActive(!value);
    }
    private float m_f_refreshGap = 0;
    void Update()
    {
        if (Id != 0)
        {
            m_f_refreshGap -= Time.deltaTime;
            if (null != time && m_f_refreshGap <= 0)
            {
                m_f_refreshGap = 1f;
                long now = DateTimeHelper.Instance.Now;
                long seconds = 0;
                ScheduleDefine.ScheduleUnit.IsInCycleDateTme(now, m_l_endTime, now
                    , out seconds);
                time.text = DateTimeHelper.ParseTimeSeconds((int)seconds);
            }

        }
    }
}