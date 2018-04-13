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
class UIWelfareOtherGrid : UIGridBase
{
    UILabel m_lableNamel = null;
    UILabel m_lableProcess = null;
    Transform m_transSrollview = null;
    GameObject m_goGetReward = null;
    UILabel m_lab_TitleName2 = null;
    GameObject m_goReceived = null;
    GameObject m_goNoMatch = null;
    UISprite redPoint = null;
    WelfareBaseData m_WelfareData = null;

    Transform m_trans_UIItemRewardGrid;
    UIGridCreatorBase m_ctor;
    void Awake()
    {
        m_lableNamel = transform.Find("Tittle/TittleName").GetComponent<UILabel>();
        m_lab_TitleName2 = transform.Find("Tittle/TittleName2").GetComponent<UILabel>();
        m_transSrollview = transform.Find("offset/ItemRoot");
        m_goGetReward = transform.Find("Btn_Receive").gameObject;
        m_goReceived = transform.Find("Status_Received").gameObject;
        redPoint = transform.Find("Btn_Receive/warning").GetComponent<UISprite>();
        m_lableProcess = transform.Find("process").GetComponent<UILabel>();
        m_goNoMatch = transform.Find("Status_NoMatch").gameObject;
        if (m_goGetReward != null)
        {
            UIEventListener.Get(m_goGetReward).onClick = OnGetReward;
        }
        if (m_lableProcess != null)
        {
            m_lableProcess.text = "";
        }

        m_trans_UIItemRewardGrid = transform.Find("UIItemRewardGrid");

        AddCreator(m_transSrollview);
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
                else if (m_WelfareData.welfareType == WelfareType.RushLevel)
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
            else if (m_WelfareData.DataType == 2)
            {
                DataManager.Instance.Sender.RequestGetServerOpenReward(m_WelfareData.id);
            }


        }
    }

    void RefreshItems()
    {
        list.Clear();
        for (int i = 0; i < m_WelfareData.lstWelfareItems.Count; i++)
        {
            list.Add(new UIItemRewardData()
            {
                itemID = m_WelfareData.lstWelfareItems[i].itemid,
                num = m_WelfareData.lstWelfareItems[i].itemNum,
            });
        }
        m_ctor.CreateGrids(list.Count);
    }

    public override void SetGridData(object data)
    {
        base.SetGridData(data);

        if (data is WelfareBaseData)
        {
            m_WelfareData = (WelfareBaseData)data;
            if (m_lab_TitleName2 != null)
            {
                bool IsRushLv = m_WelfareData.welfareType == WelfareType.RushLevel;
                m_lab_TitleName2.gameObject.SetActive(IsRushLv);
                if (IsRushLv)
                {
                    m_lab_TitleName2.text = m_WelfareData.process.ToString();
                }
            }
            if (m_lableNamel != null)
            {
                m_lableNamel.text = m_WelfareData.title;
            }
            if (m_lableProcess != null)
            {
                string msg = "";
                bool match = false;
                m_lableProcess.text = "";
                if (m_WelfareData.state != QuickLevState.QuickLevState_HaveGet)
                {
                    if (m_WelfareData.DataType == 2)
                    {
                        msg = string.Format("{0}/{1}", m_WelfareData.process, m_WelfareData.total); ;
                        SevenDayWelfare wdata = m_WelfareData as SevenDayWelfare;
                        if (wdata.nType == 7)
                        {
                            if (m_WelfareData.process == 0)
                            {
                                msg = string.Format("{0}/{1}", "1000+", m_WelfareData.total);
                                match = false;
                            }
                            else
                            {
                                match = m_WelfareData.process <= m_WelfareData.total;
                            }

                        }
                        else
                        {
                            match = m_WelfareData.process >= m_WelfareData.total;
                        }
                    }
                    else if (m_WelfareData.DataType == 1)
                    {
                        WelfareData wdata = m_WelfareData as WelfareData;
                        if (wdata.type == WelfareType.OnLine || wdata.type == WelfareType.RoleLevel || wdata.type == WelfareType.OpenSever || wdata.type == WelfareType.SevenDay)
                        {
                            match = m_WelfareData.process >= m_WelfareData.total;
                            msg = string.Format("{0}/{1}", m_WelfareData.process, m_WelfareData.total);
                        }
                        else if (wdata.type == WelfareType.FriendInvite)
                        {
                            if (wdata.inviteType == InviteType.Invited)
                            {
                                bool hadIvited = DataManager.Manager<WelfareManager>().HadBeenInvited;
                                msg = string.Format("{0}/{1}", m_WelfareData.process, m_WelfareData.total);
                                match = m_WelfareData.process >= m_WelfareData.total && hadIvited;
                            }
                            else
                            {
                                msg = string.Format("{0}/{1}", m_WelfareData.process, m_WelfareData.total);
                                match = m_WelfareData.process >= m_WelfareData.total;
                            }
                        }
                        else if (wdata.type == WelfareType.RushLevel)
                        {
                            msg = string.Format("{0}/{1}", MainPlayerHelper.GetPlayerLevel(), wdata.param2);
                            match = MainPlayerHelper.GetPlayerLevel() >= wdata.param2;
                        }

                    }
                }
                m_lableProcess.text = match ? ColorManager.GetColorString(ColorType.JZRY_Green, msg) : ColorManager.GetColorString(ColorType.JZRY_Txt_Red, msg);
            }
            SetButtonState();
            RefreshItems();
            return;
        }
    }

    private void SetButtonState()
    {
        if (m_goGetReward == null || m_goReceived == null)
        {
            return;
        }

        bool enable = m_WelfareData.state != QuickLevState.QuickLevState_HaveGet && m_WelfareData.state != QuickLevState.QuickLevState_UnGet;
        m_goGetReward.SetActive(enable);
        if (enable)
        {
            bool value = m_WelfareData.state == QuickLevState.QuickLevState_CanGet;
            m_goGetReward.GetComponent<UIButton>().isEnabled = value;
            redPoint.gameObject.SetActive(value);
        }
        m_goReceived.SetActive(m_WelfareData.state == QuickLevState.QuickLevState_HaveGet);
        m_goNoMatch.SetActive(m_WelfareData.state == QuickLevState.QuickLevState_UnGet);
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
                    itemShow.SetGridData(itemID, num, false, false);
                    itemShow.gameObject.SetActive(true);
                }
            }
        }
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