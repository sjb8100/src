
//********************************************************************
//	创建日期:	2016-11-28   15:07
//	文件名称:	UIHuntingListGrid.cs
//  创 建 人:   刘超	
//	版权所有:	中青宝
//	说    明:	狩猎左侧List
//********************************************************************
using System;
using table;
using System.Collections.Generic;
using System.Linq;
using GameCmd;
using UnityEngine;
class UIHuntingListGrid : UIGridBase
{
    UISprite bg;
    UITexture icon;
    UILabel name;
    UILabel level;
    UILabel m_lab_bossState;
    UISprite warning;
    UILabel time;
    UILabel label;
    HuntingDataBase huntingdata;
    Dictionary<uint, BossRefreshInfo> boss_dic =DataManager.Manager<HuntingManager>().boss_dic;

    Transform timeContent;
    private UIToggle m_tg_hightlight;

    public uint MonsterID { get; set; }
    protected override void  OnAwake()
    {
       base.OnAwake();
       bg      = CacheTransform.Find("Bg").GetComponent<UISprite>();
       icon    = CacheTransform.Find("IconContent/Icon").GetComponent<UITexture>();
       name    = CacheTransform.Find("Name").GetComponent<UILabel>();
       level   = CacheTransform.Find("Level").GetComponent<UILabel>();
       warning = CacheTransform.Find("Warning").GetComponent<UISprite>();

       m_lab_bossState = CacheTransform.Find("BossState").GetComponent<UILabel>();

       m_tg_hightlight = CacheTransform.Find("Toggle").GetComponent<UIToggle>();
    }

    private CMResAsynSeedData<CMTexture> m_iconCASD = null;
    public override void SetGridData(object data)
    {
        base.SetGridData(data);
        if( null == data )
        {
            return;
        }
        this.huntingdata = data as HuntingDataBase;
        MonsterID = huntingdata.ID;
        level.text = huntingdata.level.ToString();
        if (null != icon)
        {
            UIManager.GetTextureAsyn(huntingdata.icon, ref m_iconCASD, () =>
                {
                    if (null != icon)
                    {
                        icon.mainTexture = null;
                    }
                }, icon);
        }
        //刷新时间
        uint boss_index = huntingdata.ID;
        if (boss_dic.Count >0)
        {
            string msg = "";
            TextManager tMgr = DataManager.Manager<TextManager>();
            if (boss_dic.ContainsKey(boss_index))
            {
                BossRefreshInfo info = boss_dic[boss_index];
                if (info.boss_state ==(uint)BossState.BossState_Live)
                {
                    msg = tMgr.GetLocalText(LocalTextType.HuntingBoss_YiShuaXin);
                }
                //击杀中
                else if (info.boss_state == (uint)BossState.BossState_Attacted)
                {
                    msg = tMgr.GetLocalText(LocalTextType.HuntingBoss_GongJiZhong);
                }
                else if (info.boss_state == (uint)BossState.BossState_Die)
                {
                    DateTime dt = SetTime(info.remaintime);
                    msg = tMgr.GetLocalFormatText(LocalTextType.HuntingBoss_ShuaXinShiJian,string.Format("{0:d2}", dt.Hour) + ":" + string.Format("{0:d2}", dt.Minute));
                }
                else if (info.boss_state == (uint)BossState.BossState_Rest)
                {
                    msg = tMgr.GetLocalText(LocalTextType.HuntingBoss_YiXiuXi);
                }
              
                NpcDataBase monster = GameTableManager.Instance.GetTableItem<NpcDataBase>(info.boss_npc_id);
                if (monster != null)
                {
                    name.text = monster.strName;
                }
                else
                {
                    Engine.Utility.Log.Error("NPC表格中找不到ID为{0}的怪物,请确认表格或者重新打表", info.boss_npc_id);
                }

                m_lab_bossState.text = msg;
            }
        }
    }

    DateTime SetTime(uint time)
    {
        DateTime dtStart = TimeZone.CurrentTimeZone.ToLocalTime(new DateTime(1970, 1, 1));
        long lTime = long.Parse(time.ToString() + "0000000");
        TimeSpan toNow = new TimeSpan(lTime);
        return dtStart.Add(toNow);
    }

    public override void SetHightLight(bool hightLight)
    {
        base.SetHightLight(hightLight);
        if (null != m_tg_hightlight && m_tg_hightlight.value != hightLight)
        {
            m_tg_hightlight.value = hightLight;
        }
    }


    public override void Release(bool depthRelease = true)
    {
        base.Release(depthRelease);
        if (null != m_iconCASD)
        {
            m_iconCASD.Release(true);
        }
    }

}
