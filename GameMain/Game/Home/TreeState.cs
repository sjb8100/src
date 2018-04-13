//********************************************************************
//	创建日期:	2016-9-28   14:18
//	文件名称:	TreeStatus.cs
//  创 建 人:   刘超	
//	版权所有:	中青宝
//	说    明:	许愿树的场景状态显示
//********************************************************************
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using GameCmd;
using Client;
using table;
using Engine;
using Engine.Utility;

class TreeState : MonoBehaviour
{
    HomeDataManager homeDM
    {
        get
        {
            return DataManager.Manager<HomeDataManager>();
        }
    }
    TreeData td = DataManager.Manager<HomeDataManager>().GetTreeData();
    Transform TreeLevel;
    Transform treeTime;
    Transform helpSlider;
    Transform buyTree;
    UILabel LevelLabel;
    UILabel timeLabel;
    UILabel percent;
    UISlider slider;
    int m_nLeftTime = -1;

    void Start()
    {
        Engine.Utility.EventEngine.Instance().AddEventListener((int)Client.GameEventID.HOME_TREEBEGIN, EventCallBack);
        Engine.Utility.EventEngine.Instance().AddEventListener((int)Client.GameEventID.HOME_TREEGAIN, EventCallBack);
        Engine.Utility.EventEngine.Instance().AddEventListener((int)Client.GameEventID.HOME_BUYTREE, EventCallBack);
        Engine.Utility.EventEngine.Instance().AddEventListener((int)Client.GameEventID.HOME_HELPSUCCESS, EventCallBack);
    }
    void EventCallBack(int nEventID, object param)
    {
        td = DataManager.Manager<HomeDataManager>().GetTreeData();
        if (nEventID == (int)Client.GameEventID.HOME_TREEBEGIN)
        {
            Init();
        }

        if (nEventID == (int)Client.GameEventID.HOME_BUYTREE)
        {
            Init();
        }
        if (nEventID == (int)Client.GameEventID.HOME_HELPSUCCESS)
        {
            Init();
        }
        if (nEventID == (int)Client.GameEventID.HOME_TREEGAIN)
        {
            Init();
        }
    }

    public void OnDestroy()
    {
        Engine.Utility.EventEngine.Instance().RemoveEventListener((int)Client.GameEventID.HOME_TREEBEGIN, EventCallBack);
        Engine.Utility.EventEngine.Instance().RemoveEventListener((int)Client.GameEventID.HOME_TREEGAIN, EventCallBack);
        Engine.Utility.EventEngine.Instance().RemoveEventListener((int)Client.GameEventID.HOME_BUYTREE, EventCallBack);
        Engine.Utility.EventEngine.Instance().RemoveEventListener((int)Client.GameEventID.HOME_HELPSUCCESS, EventCallBack);
    }

    void Awake()
    {

        TreeLevel = transform.Find("TreeLevel");
        treeTime = transform.Find("Time");
        helpSlider = transform.Find("HelpSlider");
        buyTree = transform.Find("BuyTree");
        UIEventListener.Get(buyTree.gameObject).onClick = onClick_Btn_BuyTree_Btn;
        if (TreeLevel != null)
        {
            LevelLabel = TreeLevel.GetComponentInChildren<UILabel>();
            if (LevelLabel != null)
            {
                LevelLabel.text = "许愿树";
            }
        }
        if (treeTime != null)
        {
            timeLabel = treeTime.GetComponent<UILabel>();
        }
        if (helpSlider != null)
        {
            percent = helpSlider.GetComponentInChildren<UILabel>();
            slider = helpSlider.GetComponent<UISlider>();
        }
    }

    float tempTime = 0;
    void Update()
    {
        tempTime += Time.deltaTime;
        if (tempTime > 0.5f)
        {
            m_nLeftTime = (int)homeDM.TreeLeftTime;
                if (m_nLeftTime > 0)
                {

                    timeLabel.text = StringUtil.GetStringBySeconds((uint)m_nLeftTime);
                }
                if (m_nLeftTime == 0)
                {
                    timeLabel.text = "可收获";
                }

            tempTime = 0;
        }
    }

    public void Init()
    {
        gameObject.SetActive(true);
        m_nLeftTime = (int)homeDM.TreeLeftTime;
        if (homeDM.MaxTreeID == 501)
        {
            LevelLabel.text = "铁·许愿树";
        }
        if (homeDM.MaxTreeID == 502)
        {
            LevelLabel.text = "铜·许愿树";
        }
        if (homeDM.MaxTreeID == 503)
        {
            LevelLabel.text = "银·许愿树";
        }
        if (homeDM.MaxTreeID == 504)
        {
            LevelLabel.text = "金·许愿树";
        }
        if (m_nLeftTime > 0)
        {
           // TimerAxis.Instance().SetTimer(1000, 1000, this);
        }
        if (m_nLeftTime == 0)
        {
            timeLabel.text = "可收获";
           // TimerAxis.Instance().KillTimer(1000, this);
        }

        table.WishingTreeDataBase data = GameTableManager.Instance.GetTableItem<table.WishingTreeDataBase>(homeDM.MaxTreeID);
        if(data == null)
        {
            return;
        }
        percent.text = homeDM.HelpNum.ToString() + "/" + data.loveMaxNum.ToString();
        slider.value = (homeDM.HelpNum+0.00f) / data.loveMaxNum;
        timeLabel.text = StringUtil.GetStringBySeconds((uint)m_nLeftTime);
    }
    void onClick_Btn_BuyTree_Btn(GameObject caster) 
    {
        DataManager.Manager<UIPanelManager>().ShowPanel(PanelID.WishTreePanel);
    }
 
}
