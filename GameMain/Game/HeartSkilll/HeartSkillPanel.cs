using Client;
using GameCmd;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using table;
using UnityEngine;


partial class LearnSkillPanel
{
    #region property

    int m_selectItem = 0;//选中的心法;默认选中第一个

    List<HeartSkill> m_heartSkillList = null;

    /// <summary>
    /// 重置心法道具ID
    /// </summary>
    uint m_resetSkillItemId;

    /// <summary>
    /// 加心法点道具ID
    /// </summary>
    uint m_addPointItemId;

    HeartSkillManager HSManager
    {
        get
        {
            return DataManager.Manager<HeartSkillManager>();
        }
    }

    #endregion


    #region method

    /// <summary>
    /// 点心法按钮
    /// </summary>
    void OnClickHeartSkillBtn()
    {
        int lvLimit = GameTableManager.Instance.GetGlobalConfig<int>("GodOpenLevel");
        Client.IPlayer mainPlayer = Client.ClientGlobal.Instance().MainPlayer;
        int playerLv = mainPlayer.GetProp((int)Client.CreatureProp.Level);

        if (playerLv >= lvLimit)
        {
            m_trans_LearnSkillContent.gameObject.SetActive(false);
            m_trans_HeartSkillContent.gameObject.SetActive(true);

            InitHeartSkillUI();  //初始化心法界面
        }
        else
        {
            string des = string.Format("需要玩家等级达到{0}级才开放心法", lvLimit);
            TipsManager.Instance.ShowTips(des);
        }
    }


    /// <summary>
    /// 初始化心法姐main
    /// </summary>
    void InitHeartSkillUI()
    {
        if (m_trans_HeartSkillContent.gameObject.activeSelf == true)
        {
            m_heartSkillList = HSManager.GetHeartSkillList();
            InitHeartSkillGridList();   //grid
            InitHeartSkillUpgradeInfo(); //对应grid的信息
            UpdateTopUI();   //神魔等级，心法点
            UpdateRedPoint();
        }
    }

    /// <summary>
    /// 初始化左侧的grid
    /// </summary>
    void InitHeartSkillGridList()
    {
        if (m_heartSkillList != null)
        {
            for (int i = 0; i < m_grid_heartSkillGridContent.transform.childCount; i++)
            {
                GameObject gridGo = m_grid_heartSkillGridContent.transform.GetChild(i).gameObject;
                UIHeartSkillGrid grid = gridGo.GetComponent<UIHeartSkillGrid>();
                if (grid == null)
                {
                    grid = gridGo.AddComponent<UIHeartSkillGrid>();
                }

                grid.SetGridData(m_heartSkillList[i]);
                bool enableUpgrade = HSManager.IsEnableUpgrade(m_heartSkillList[i]);
                grid.SetRedPoint(enableUpgrade);
                if (m_selectItem == i)
                {
                    SetSelectItem(m_selectItem);
                }

                grid.RegisterUIEventDelegate(OnSelectCallback);
            }
        }

    }

    /// <summary>
    /// grid点击事件
    /// </summary>
    /// <param name="eventType"></param>
    /// <param name="data"></param>
    /// <param name="param"></param>
    void OnSelectCallback(UIEventType eventType, object data, object param)
    {
        if (eventType == UIEventType.Click)
        {
            UIHeartSkillGrid grid = data as UIHeartSkillGrid;
            if (grid == null)
            {
                return;
            }

            HeartSkillDataBase db = GameTableManager.Instance.GetTableItem<HeartSkillDataBase>(grid.m_heartSkill.skill_id, (int)grid.m_heartSkill.level);
            if (db != null)
            {
                SetSelectItem((int)db.orderId - 1);
            }
        }
    }

    /// <summary>
    /// 设置选中
    /// </summary>
    /// <param name="item"></param>
    void SetSelectItem(int item)
    {

        Transform gridTransf = m_grid_heartSkillGridContent.transform.GetChild(m_selectItem);
        if (gridTransf != null)
        {
            UIHeartSkillGrid grid = gridTransf.GetComponent<UIHeartSkillGrid>();
            if (grid != null)
            {
                grid.SetSelect(false);
            }
        }

        gridTransf = m_grid_heartSkillGridContent.transform.GetChild(item);
        if (gridTransf != null)
        {
            UIHeartSkillGrid grid = gridTransf.GetComponent<UIHeartSkillGrid>();
            if (grid != null)
            {
                grid.SetSelect(true);
            }
        }

        m_selectItem = item;

        InitHeartSkillUpgradeInfo();
    }

    /// <summary>
    /// 设置等级
    /// </summary>
    /// <param name="item"></param>
    void SetItemLv(int item)
    {

        Transform gridTransf = m_grid_heartSkillGridContent.transform.GetChild(item);
        if (gridTransf != null)
        {
            UIHeartSkillGrid grid = gridTransf.GetComponent<UIHeartSkillGrid>();
            HeartSkillDataBase db = GameTableManager.Instance.GetTableItem<HeartSkillDataBase>(m_heartSkillList[item].skill_id, (int)m_heartSkillList[item].level);
            if (grid != null && db != null)
            {
                grid.SetLv(db.lv);
            }
        }

    }

    /// <summary>
    /// 设置遮罩mask
    /// </summary>
    /// <param name="item"></param>
    void SetItemMask(int item)
    {

        Transform gridTransf = m_grid_heartSkillGridContent.transform.GetChild(item);
        if (gridTransf != null)
        {
            UIHeartSkillGrid grid = gridTransf.GetComponent<UIHeartSkillGrid>();
            HeartSkillDataBase db = GameTableManager.Instance.GetTableItem<HeartSkillDataBase>(m_heartSkillList[item].skill_id, (int)m_heartSkillList[item].level);
            if (grid != null)
            {
                bool isLock = db.lv == 0 ? true : false;
                grid.SetMask(isLock);
            }
        }

    }

    /// <summary>
    /// 跟新心法item信息
    /// </summary>
    /// <param name="item"></param>
    void UpdateHeartSkillItemInfo(GameCmd.HeartSkill heartSkill)
    {
        HeartSkillDataBase db = GameTableManager.Instance.GetTableItem<HeartSkillDataBase>(heartSkill.skill_id, (int)heartSkill.level);
        SetItemLv((int)db.orderId - 1);
        SetItemMask((int)db.orderId - 1);
    }

    /// <summary>
    /// 心法升级信息
    /// </summary>
    void InitHeartSkillUpgradeInfo()
    {
        IPlayer mainPlayer = ClientGlobal.Instance().MainPlayer;
        if (mainPlayer == null)
        {
            return;
        }

        HeartSkillDataBase db = GameTableManager.Instance.GetTableItem<HeartSkillDataBase>(m_heartSkillList[m_selectItem].skill_id, (int)m_heartSkillList[m_selectItem].level);

        if (db != null)
        {
            if (0 <= db.lv && db.lv <= db.maxLv)  //解锁
            {
                //当前等级
                m_widget_HeartSkill_unlock.gameObject.SetActive(true);//打开当前
                m_label_HeartSkill_unlock_name_now.text = db.name;
                m_label_HeartSkill_unlock_level_now.text = string.Format("{0}级", db.lv);
                m_label_HeartSkill_unlock_describe_now.text = db.des;
                if (db.lv == db.maxLv)  //当前等级为最大等级
                {
                    m_widget_HeartSkill_max.gameObject.SetActive(true);
                    m_widget_HeartSkill_NextLevel.gameObject.SetActive(false);
                }
                else
                {
                    m_widget_HeartSkill_max.gameObject.SetActive(false);
                    m_widget_HeartSkill_NextLevel.gameObject.SetActive(true);
                }
                m_widget_HeartSkill__lock.gameObject.SetActive(false);



                //升级到下一等级需要的信息
                HeartSkillDataBase nextDb = GameTableManager.Instance.GetTableItem<HeartSkillDataBase>(m_heartSkillList[m_selectItem].skill_id, (int)m_heartSkillList[m_selectItem].level + 1);
                if (nextDb != null)
                {
                    m_widget_HeartSkill_NextLevel.gameObject.SetActive(true);//打开下一级

                    m_label_HeartSkill_unlock_describe_nextlevel.text = nextDb.des;   //下一等级描述

                    //解锁神魔等级
                    m_label_HeartSkill_unlock_locklevel_nextlevel.text = string.Format("{0}级", nextDb.needPlayerLv);



                    if (mainPlayer.GetProp((int)CreatureProp.Level) >= nextDb.needPlayerLv)
                    {
                        m_label_HeartSkill_unlock_locklevel_nextlevel.color = new Color(28, 40, 50, 255); // Color.white;
                    }
                    else
                    {
                        m_label_HeartSkill_unlock_locklevel_nextlevel.color = Color.red;
                    }


                    //消耗 心法点 金币
                    m_label_HeartSkill_consume.text = nextDb.costHeartSkillPoint.ToString();   //消耗心法点
                    m_label_HeartSkill_unlock_xiaohao_Num.text = nextDb.need_money.ToString(); //消耗金币
                    if (HSManager.HeartSkillPoint >= nextDb.costHeartSkillPoint)//心法点不足  字体红色
                    {
                        m_label_HeartSkill_consume.color = new Color(28, 40, 50, 255); // Color.white;
                    }
                    else
                    {
                        m_label_HeartSkill_consume.color = Color.red;
                    }

                    if (mainPlayer.GetProp((int)PlayerProp.Coupon) >= nextDb.need_money)   //金币不足 字体红色
                    {
                        m_label_HeartSkill_unlock_xiaohao_Num.color = new Color(28, 40, 50, 255); // Color.white;
                    }
                    else
                    {
                        m_label_HeartSkill_unlock_xiaohao_Num.color = Color.red;
                    }

                    //前置技能
                    List<HeartSkill> preHeartSkillList = HSManager.GetPreHeartSkill(nextDb.pre_skill);
                    string preSkillString = "";
                    for (int i = 0; i < preHeartSkillList.Count; i++)
                    {
                        HeartSkillDataBase preSkillDb = GameTableManager.Instance.GetTableItem<HeartSkillDataBase>(preHeartSkillList[i].skill_id, (int)preHeartSkillList[i].level);
                        if (preSkillDb != null)
                        {
                            string preSkill = string.Format("{0}等级{1}", preSkillDb.name, preSkillDb.lv);
                            bool isExists = HSManager.OwnedHeartSkillList.Exists((d) => { return d.skill_id == preHeartSkillList[i].skill_id && d.level >= preHeartSkillList[i].level; });//前置技能学没学
                            if (isExists)
                            {
                                preSkill = string.Format("[1C2832]{0}[-]  ", preSkill);
                            }
                            else
                            {
                                preSkill = string.Format("[ff0000]{0}[-]  ", preSkill);
                            }

                            preSkillString += preSkill;
                        }
                    }
                    m_label_HeartSkill_unlock_locklevel_PreSkills.text = preSkillString;//
                }
                else
                {
                    m_widget_HeartSkill_NextLevel.gameObject.SetActive(false);//关闭下一级
                }
            }
        }


    }

    /// <summary>
    /// 神魔等级  心法点
    /// </summary>
    void UpdateTopUI()
    {
        m_label_GhostLevelNum.text = string.Format("{0}级", HSManager.GhostLv);   //神魔等级
        m_label_HeartPointNum.text = HSManager.HeartSkillPoint.ToString();        //心法点
    }

    /// <summary>
    /// 使用心法丸
    /// </summary>
    /// <param name="useDianJun"></param>
    /// <param name="num"></param>
    void UseHeartPill(bool useDianJun, int num)
    {
        IPlayer player = Client.ClientGlobal.Instance().MainPlayer;
        List<BaseItem> itemdataList = DataManager.Manager<ItemManager>().GetItemByBaseId(m_addPointItemId);

        DataManager.Manager<HeartSkillManager>().ReqAddHeartSkillPoint(m_addPointItemId, (uint)num);

    }

    /// <summary>
    /// 使用洗髓丸
    /// </summary>
    /// <param name="useDianJun"></param>
    /// <param name="num"></param>
    void UseWashPill(bool useDianJun, int num)
    {
        IPlayer player = Client.ClientGlobal.Instance().MainPlayer;
        List<BaseItem> itemdataList = DataManager.Manager<ItemManager>().GetItemByBaseId(m_resetSkillItemId);

        DataManager.Manager<HeartSkillManager>().ReqResetHeartSkill(m_resetSkillItemId);

    }

    #endregion


    #region click

    /// <summary>
    /// 心法升级
    /// </summary>
    /// <param name="caster"></param>
    void onClick_HeartSkill_btn_shengji_Btn(GameObject caster)
    {
        DataManager.Manager<HeartSkillManager>().ReqUpgradeHeartSkill(m_heartSkillList[m_selectItem].skill_id);
    }

    void onClick_Btn_HeartPointAdd_Btn(GameObject caster)
    {
        //弹出使用心法丸界面
        string title = "使用心法丸";

        Action overDelegate = delegate
        {
            TipsManager.Instance.ShowTips(LocalTextType.Skill_Commond_xinfawanshuliangbuzu);//心法丸数量不足
        };

        TipsManager.Instance.ShowUseItemWindow(title, m_addPointItemId, okDel: UseHeartPill, cancelDel: null, belowDel: null, overDel: overDelegate, setNum: true);
    }


    /// <summary>
    /// 重置所有心法
    /// </summary>
    /// <param name="cmd"></param>
    /// 
    void onClick_Btn_Reset_Btn(GameObject caster)
    {
        if (HSManager.OwnedHeartSkillList.Count > 0)
        {
            //还可以免费重置
            if (HSManager.FreeReset > 0)
            {
                Action ok = delegate { DataManager.Manager<HeartSkillManager>().ReqResetHeartSkill(m_resetSkillItemId); };

                //当前可以免费重置心法{0}次
                string des = DataManager.Manager<TextManager>().GetLocalFormatText(LocalTextType.HeartSkill_Commond_mianfeichongzhixingfa, HSManager.FreeReset);

                TipsManager.Instance.ShowTipWindow(TipWindowType.CancelOk, des, ok, null, null);
            }
            //要使用道具重置
            else
            {
                //使用洗髓丸界面
                string title = "使用洗髓丸";

                TipsManager.Instance.ShowUseItemWindow(title, m_resetSkillItemId, UseWashPill, null, setNum: false);
            }
        }
        else
        {
            TipsManager.Instance.ShowTips(LocalTextType.Skill_Commond_dangqianbuxuyaochongzhi);//当前不需要重置
        }
    }
}


    #endregion



