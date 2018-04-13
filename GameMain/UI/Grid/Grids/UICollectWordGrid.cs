//*************************************************************************
//	创建日期:	2016-11-18 15:54
//	文件名称:	UIWelfareOtherGrid.cs
//  创 建 人:   	Chengxue.Zhao
//	版权所有:	中青宝
//	说    明:	福利格子
//*************************************************************************
using System;
using System.Collections.Generic;
using UnityEngine;
using GameCmd;
class UICollectWordGrid : UIGridBase
{
    UILabel m_lableProcess = null;
    Transform m_transItemRoot = null;
    UISprite m_spriteAddIcon = null;
    UISprite m_spriteEqualsIcon = null;
    GameObject m_goReceived = null;
    Transform m_redPoint = null;
    Transform m_trans_UIItemRewardGrid; 
    UIGridCreatorBase m_ctor;

    WelfareData m_WelfareData;
    void Awake()
    {
        m_transItemRoot = transform.Find("ItemRoot");
        m_lableProcess = transform.Find("process").GetComponent<UILabel>();
        m_trans_UIItemRewardGrid = transform.Find("UIItemRewardGrid");
        m_goReceived = transform.Find("Btn_Receive").gameObject;
        m_redPoint = transform.Find("Btn_Receive/warning");
        if (m_goReceived != null)
        {
            UIEventListener.Get(m_goReceived).onClick = OnGetReward;
        }
        if (m_lableProcess != null)
        {
            m_lableProcess.text = "";
        }
        AddCreator(m_transItemRoot);
    }

    void OnGetReward(GameObject go)
    {
        if (m_WelfareData == null)
        {
            return;
        }
        if (m_WelfareData.state == QuickLevState.QuickLevState_CanGet)
        {
            if (m_WelfareData.DataType == 1)//福利
            {
               
                if (m_WelfareData.welfareType == WelfareType.FriendInvite)
                {
                    DataManager.Instance.Sender.ReqGetInviteReward(m_WelfareData.id);
                }
                else if(m_WelfareData.welfareType == WelfareType.RushLevel)
                {
                    NetService.Instance.Send(new stGetQuickLevRewardDataUserCmd_CS() { id = m_WelfareData.id });
                }
                else                
                {
                    List<uint> lstids = new List<uint>();
                    lstids.Add(m_WelfareData.id);
                    DataManager.Instance.Sender.ReqGetReward(ref lstids);
                }
                
            }




        }
    }

    void RefreshItems()
    {
        list.Clear();
        for (int i = 0; i < m_WelfareData.collectWords.Count; i++)
        {
            string icon = i == m_WelfareData.collectWords.Count - 1 ? "tubiao_deng_he" : "tubiao_jia_he";
            list.Add(new UIItemRewardData()
            {
                itemID = m_WelfareData.collectWords[i].itemid,
                num = m_WelfareData.collectWords[i].itemNum,
                name = m_WelfareData.collectWords[i].name,
                additionalIconName =icon, 
                showAdditional = true,
            });
        }
        for (int i = 0; i < m_WelfareData.lstWelfareItems.Count; i++)
        {
            list.Add(new UIItemRewardData()
            {
                itemID = m_WelfareData.lstWelfareItems[i].itemid,
                num = m_WelfareData.lstWelfareItems[i].itemNum,
                showAdditional = false,
            });
        }
        m_ctor.CreateGrids(list.Count);
    }

    public override void SetGridData(object data)
    {
        base.SetGridData(data);

        if (data is WelfareData)
        {
            m_WelfareData = (WelfareData)data;                       
            if (m_lableProcess != null)
            {
                string msg = "";
                bool match = false;
                m_lableProcess.text = "";
                int process =m_WelfareData.process;
                if (m_WelfareData.collectType == CollectType.Day)
                {
                    msg = string.Format("{0} {1}/{2}", "每日限制次数", process, m_WelfareData.param2);
                }
                else if (m_WelfareData.collectType == CollectType.All)
                {
                    msg = string.Format("{0} {1}/{2}", "总限制次数", process, m_WelfareData.param2);
                }
                match = process < m_WelfareData.param2;
                m_lableProcess.text = match ? ColorManager.GetColorString(ColorType.JZRY_Green, msg) : ColorManager.GetColorString(ColorType.JZRY_Txt_Red, msg);
            }
            SetButtonState();
            RefreshItems();
            return;
        }
    }

    private void SetButtonState()
    {
        if (m_goReceived == null)
        {
            return;
        }
        bool value = m_WelfareData.state == QuickLevState.QuickLevState_CanGet;
        m_goReceived.GetComponent<UIButton>().isEnabled = value;

        m_redPoint.gameObject.SetActive(value);
    }


    List<UIItemRewardData> list = new List<UIItemRewardData>();
    void AddCreator(Transform parent) 
    {
        if (parent != null)
       {
           m_ctor = parent.GetComponent<UIGridCreatorBase>();
           if (m_ctor == null)
            {
                m_ctor = parent.gameObject.AddComponent<UIGridCreatorBase>();
                m_ctor.arrageMent = UIGridCreatorBase.Arrangement.Horizontal;
                m_ctor.gridWidth = 150;
                m_ctor.gridHeight = 90;
            }       
           m_ctor.RefreshCheck();
           m_ctor.Initialize<UIItemRewardGrid>(m_trans_UIItemRewardGrid.gameObject, OnUpdateGridData,null);
       }
    }
    void OnUpdateGridData(UIGridBase grid, int index)
    {
        if (grid is UIItemRewardGrid)
        {
            UIItemRewardGrid itemShow = grid as UIItemRewardGrid;
            if (itemShow != null)
            {
                if(index < list.Count)
                {
                    UIItemRewardData data = list[index];
                    uint itemID = data.itemID;
                    uint num = data.num;
                    itemShow.gameObject.SetActive(true);
                    itemShow.SetGridData(itemID, num, true, false, !data.showAdditional);
                    itemShow.SetName(true,data.name);
                  
                    SetAdditionalIcon(itemShow, data.additionalIconName, data.showAdditional);                  
                }                            
            }
        }
    }
    public void SetAdditionalIcon(UIItemRewardGrid grid, string iconName, bool showIcon)
    {
        Transform icon = grid.transform.Find("addIcon");
        if (icon == null)
        {
            return;
        }
        icon.gameObject.SetActive(showIcon);
        UISprite AdditionalIcon = icon.GetComponent<UISprite>();
        if (AdditionalIcon == null)
        {
            return;
        }
        AdditionalIcon.spriteName = iconName;
    }
    public override void Release(bool depthRelease = true)
    {
        base.Release(depthRelease);

        if (m_WelfareData != null)
        {
            m_WelfareData = null;
        }

  
    }
}