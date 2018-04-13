using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GameCmd;
using UnityEngine;
using table;



class UIDailyPushGrid : UIGridBase
{
    UILabel name;
    UILabel JinXingZhong;
    UILabel WeiKaiQi;
    UISprite icon;

    public uint dailyID   
    {
        set;
        get;
    }
    protected override void OnAwake()
    {
        base.OnAwake();
        name =CacheTransform.Find("name").GetComponent<UILabel>();
        JinXingZhong = CacheTransform.Find("JinXingZhong").GetComponent<UILabel>();
        WeiKaiQi = CacheTransform.Find("WeiKaiQi").GetComponent<UILabel>();
        icon = CacheTransform.Find("icon").GetComponent<UISprite>();
    }
    CMResAsynSeedData<CMAtlas> m_playerAvataCASD = null;
    public override void Release(bool depthRelease = true)
    {
        base.Release(depthRelease);
        if (m_playerAvataCASD != null)
        {
            m_playerAvataCASD.Release(depthRelease);
            m_playerAvataCASD = null;
        }
    }
    public override void SetGridData(object data)
    {
        base.SetGridData(data);
        if (data != null && data is DailyPushData)
        {
            DailyPushData pushData = data as DailyPushData;

            dailyID = pushData.dailyID;
            name.text = pushData.dailyName;
            if (pushData.curState == DailyPushMsgState.PreShow)
            {
                JinXingZhong.gameObject.SetActive(false);
                WeiKaiQi.gameObject.SetActive(true);
                WeiKaiQi.text = DataManager.Manager<DailyManager>().GetCloserScheduleTimeByID(dailyID);
            }
            else if (pushData.curState == DailyPushMsgState.Showing)
            {
                JinXingZhong.gameObject.SetActive(true);
                WeiKaiQi.gameObject.SetActive(false);
            }

            UIManager.GetAtlasAsyn(pushData.iconName, ref m_playerAvataCASD, () =>
            {
                if (null != icon)
                {
                    icon.atlas = null;
                }
            }, icon,false);
        }
       
    }

}
