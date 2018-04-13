using UnityEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using table;
using Engine;
using Client;
using GameCmd;
using System.Text;
using Common;

partial class DailyPanel
{

    List<uint> dailyIDs = new List<uint>();

    enum CalendarDay
    {
        sun = 0,
        mon =1,
        tues =2,
        wedn =3,
        thur =4,
        fri=5,
        satu=6,
}
    bool todayIsMatch = false;
    void InitCalendarCreators() 
    {

        GameObject resObj = m_trans_UIDailyCalendarGrid.gameObject;
        if (resObj == null)
        {
            return;
        }
        CalendarDay today = (CalendarDay)(DateTime.Now.DayOfWeek);
        if (m_ctor_Monday != null)
        {
            m_ctor_Monday.Initialize<UIDailyCalendarGrid>(resObj, OnUpdateMallGridData, OnDailyTabGridUIEvent);
            dailyIDs = DataManager.Manager<DailyManager>().GetDailyActivityIDs((uint)CalendarDay.mon);
            todayIsMatch = today == CalendarDay.mon;
            m_ctor_Monday.CreateGrids(dailyIDs.Count);
        }

        if (m_ctor_Tuesday != null)
        {
            m_ctor_Tuesday.Initialize<UIDailyCalendarGrid>(resObj, OnUpdateMallGridData, OnDailyTabGridUIEvent);
            dailyIDs = DataManager.Manager<DailyManager>().GetDailyActivityIDs((uint)CalendarDay.tues);
            todayIsMatch = today == CalendarDay.tues;
            m_ctor_Tuesday.CreateGrids(dailyIDs.Count);
        }

        if (m_ctor_Wednesday != null)
        {
            m_ctor_Wednesday.Initialize<UIDailyCalendarGrid>(resObj, OnUpdateMallGridData, OnDailyTabGridUIEvent);
            dailyIDs = DataManager.Manager<DailyManager>().GetDailyActivityIDs((uint)CalendarDay.wedn);
            todayIsMatch = today == CalendarDay.wedn;
            m_ctor_Wednesday.CreateGrids(dailyIDs.Count);
        }

        if (m_ctor_Thursday != null)
        {
            m_ctor_Thursday.Initialize<UIDailyCalendarGrid>(resObj, OnUpdateMallGridData, OnDailyTabGridUIEvent);
            dailyIDs = DataManager.Manager<DailyManager>().GetDailyActivityIDs((uint)CalendarDay.thur);
            todayIsMatch = today == CalendarDay.thur;
            m_ctor_Thursday.CreateGrids(dailyIDs.Count);
        }

        if (m_ctor_Friday != null)
        {
            m_ctor_Friday.Initialize<UIDailyCalendarGrid>(resObj, OnUpdateMallGridData, OnDailyTabGridUIEvent);
            dailyIDs = DataManager.Manager<DailyManager>().GetDailyActivityIDs((uint)CalendarDay.fri);
            todayIsMatch = today == CalendarDay.fri;
            m_ctor_Friday.CreateGrids(dailyIDs.Count);
        }

        if (m_ctor_Saturday != null)
        {
            m_ctor_Saturday.Initialize<UIDailyCalendarGrid>(resObj, OnUpdateMallGridData, OnDailyTabGridUIEvent);
            dailyIDs = DataManager.Manager<DailyManager>().GetDailyActivityIDs((uint)CalendarDay.satu);
            todayIsMatch = today == CalendarDay.satu;
            m_ctor_Saturday.CreateGrids(dailyIDs.Count);
        }

        if (m_ctor_Sunday != null)
        {
            m_ctor_Sunday.Initialize<UIDailyCalendarGrid>(resObj, OnUpdateMallGridData, OnDailyTabGridUIEvent);
            dailyIDs = DataManager.Manager<DailyManager>().GetDailyActivityIDs((uint)CalendarDay.sun);
            todayIsMatch = today == CalendarDay.sun;
            m_ctor_Sunday.CreateGrids(dailyIDs.Count);
        }

        m_trans_DetailContent.gameObject.SetActive(false);
    }
    CMResAsynSeedData<CMAtlas> m_playerAvataCASD = null;
    /// <summary>
    /// 显示日常的详细信息
    /// </summary>
    /// <param name="index">索引</param>
    void ShowCalendarInfo(uint dailyID)
    {
        m_trans_DetailContent.gameObject.SetActive(true);
        curCalendarData = GameTableManager.Instance.GetTableItem<DailyDataBase>(dailyID);
        if (curCalendarData == null)
        {
            return;
        }
        
        UIManager.GetAtlasAsyn(curCalendarData.icon, ref m_playerAvataCASD, () =>
        {
            if (null != m_sprite_CalendarIcon)
            {
                m_sprite_CalendarIcon.atlas = null;
            }
        }, m_sprite_CalendarIcon);
        m_label_huodongmingcheng.text = curCalendarData.name;
        if (dm.ActiveDic.ContainsKey(curCalendarData.id))
        {
            if (curCalendarData.MaxTimes == 0)
            {
                m_label_cishu.text = "不限";
            }
            else
            {
                m_label_cishu.text = string.Format("{0}/{1}", dm.ActiveDic[curCalendarData.id].time, curCalendarData.MaxTimes);
            }
            m_label_huoyuezhi.text = string.Format("{0}/{1}", dm.ActiveDic[curCalendarData.id].liveness_num, curCalendarData.MaxActive);
            m_label_xingshi.text = string.Format("{0}; 等级>={1}", curCalendarData.isTeam == 0 ? "单人" : "组队", curCalendarData.minLevel == 0 ? 1 : curCalendarData.minLevel);
            m_label_shijian.text = curCalendarData.activityTime;
            //m_label_shijian.text = DataManager.Manager<DailyManager>().GetActiveAndEndTimeByID(curCalendarData).ToString();
            m_label_miaoshu.text = curCalendarData.description;

            if (dm.ActiveDic[curCalendarData.id].time >= curCalendarData.MaxTimes && curCalendarData.MaxTimes != 0)
            {
                m_btn_JoinBtn.isEnabled = false;
                m_btn_JoinBtn.GetComponentInChildren<UILabel>().text = tm.GetLocalText(LocalTextType.Local_TXT_Notice_Daily_Finish);
            }
            else
            {
                IPlayer player = MainPlayerHelper.GetMainPlayer();
                if (player != null && player.GetProp((int)CreatureProp.Level) < curCalendarData.minLevel)
                {
                    SetDetailOpenBtn(false, curCalendarData.minLevel);
                }
                else
                {
                    SetBtnSchedule(curCalendarData);
                }
            }
        }
        AddCreator(m_trans_CalendarRewardGrid.transform);
        m_lst_UIItemRewardDatas.Clear();
        string[] items = curCalendarData.awardItem.Split(';');
      
        for (int i = 0; i < items.Length; i++)
        {
            uint itemID = uint.Parse(items[i]);
            m_lst_UIItemRewardDatas.Add(new UIItemRewardData()
            {
                itemID = itemID,
                num = 1,
            });
        }
        m_ctor_UIItemRewardCreator.CreateGrids(m_lst_UIItemRewardDatas.Count);
        //m_grid_CalendarRewardGrid.repositionNow = true;
    }

    DailyDataBase curCalendarData = null;

    void onClick_JoinBtn_Btn(GameObject caster)
    {
        ExecuteGoto(curCalendarData);
    }
    void onClick_ColliderBg_Btn(GameObject caster)
    {
        m_trans_DetailContent.gameObject.SetActive(false);
    }
}
