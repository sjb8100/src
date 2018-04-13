//*************************************************************************
//	创建日期:	2016/10/24 9:49:48
//	文件名称:	FBConfirmPanel
//   创 建 人:   zhuidanyu	
//	版权所有:	中青宝
//	说    明:	副本确认界面
//*************************************************************************

using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using Client;
using Engine.Utility;
using table;
using GameCmd;
using DG.Tweening;
using Vector2 = UnityEngine.Vector2;


partial class FBConfirmPanel
{

    ComBatCopyDataManager CopyDataManager
    {
        get
        {
            return DataManager.Manager<ComBatCopyDataManager>();
        }
    }

    List<TeamMemberInfo> m_teamMemberList = new List<TeamMemberInfo>();

    CopyDataBase m_db = null;
    protected override void OnLoading()
    {
        base.OnLoading();
        CopyDataManager.ValueUpdateEvent += CopyDataManager_ValueUpdateEvent;
        m_slider_Countdownslider.value = 1;
    }

    void CopyDataManager_ValueUpdateEvent(object sender, ValueUpdateEventArgs e)
    {
        if (e != null)
        {
            if (e.key == CopyDispatchEvent.RefreshStatus.ToString())
            {
                RefreshUI();
            }
        }
    }
    protected override void OnDisable()
    {
        base.OnDisable();
    }
    protected override void OnShow(object data)
    {
        base.OnShow(data);
        m_db = GameTableManager.Instance.GetTableItem<CopyDataBase>(CopyDataManager.EnterCopyID);

        if (m_db == null)
        {
            return;
        }

        m_teamMemberList.Clear();
        List<TeamMemberInfo> tempList = DataManager.Manager<TeamDataManager>().TeamMemberList;
        foreach (var info in tempList)
        {
            if (!DataManager.Manager<TeamDataManager>().IsLeader(info.id))
            {
                m_teamMemberList.Add(info);
            }
            else
            {
                m_teamMemberList.Insert(0, info);
            }
        }
        if (DataManager.Manager<TeamDataManager>().MainPlayerIsLeader())
        {
            m_btn_btn_queding.isEnabled = false;
        }
        else
        {
            m_btn_btn_queding.isEnabled = true;
        }
        m_label_FB_name.text = m_db.copyName;
        RefreshUI();
        bContinue = true;
        m_slider_Countdownslider.value = 1;



    }
    bool bContinue = true;
    void Update()
    {
        if (CopyDataManager == null)
        {
            return;
        }

        if (!bContinue)
        {
            return;
        }
        m_label_slidertime.text = CopyDataManager.EnterCopyCountDown.ToString();
        uint total = GameTableManager.Instance.GetGlobalConfig<uint>("EnterCopyCountdown");
        if (total != 0)
        {
            float sliderValue = CopyDataManager.EnterCopyCountDown * 1.0f / total;
            if (m_slider_Countdownslider.value > sliderValue)
            {
                m_slider_Countdownslider.value -= Time.deltaTime;
            }
            if (CopyDataManager.EnterCopyCountDown == 0)
            {
                bContinue = false;
                if (DataManager.Manager<TeamDataManager>().MainPlayerIsLeader())
                {
                    //stRequestEnterCopyUserCmd_C cmd = new stRequestEnterCopyUserCmd_C();
                    //cmd.copy_base_id = m_db.copyId;
                    //NetService.Instance.Send( cmd );
                }
                else
                {
                    stAnsTeamCopyUserCmd_CS cmd = new stAnsTeamCopyUserCmd_CS();
                    cmd.ans = true;
                    cmd.copy_base_id = CopyDataManager.EnterCopyID;
                    NetService.Instance.Send(cmd);
                }

                HideSelf();
            }
        }
    }
    void RefreshUI()
    {
        foreach (var dic in CopyDataManager.m_dicTeammateStatus)
        {
            if (!dic.Value)
            {
                HideSelf();
                return;
            }
        }
        if (m_trans_Member == null)
        {
            Log.Error("m_trans_Member is null");
            return;
        }
        Transform childTrans = m_trans_Member.Find("GameObject");
        if (childTrans == null)
        {
            Log.Error("childTrans is null");
            return;
        }
        for (int i = 0; i < childTrans.childCount; i++)
        {
            string childName = "Member_" + (i + 1);
            Transform memItem = m_trans_Member.Find("GameObject/" + childName);
            if (memItem != null)
            {
                if (i < m_teamMemberList.Count)
                {
                    TeamMemberInfo info = m_teamMemberList[i];
                    RefeshMemInfo(info, memItem);
                }
                else
                {
                    RefeshMemInfo(null, memItem);
                }
            }
        }
    }


    void RefeshMemInfo(TeamMemberInfo info, Transform memItem)
    {
        FBConfirmItemInfo fb = memItem.GetComponent<FBConfirmItemInfo>();
        if (fb == null)
        {
            fb = memItem.gameObject.AddComponent<FBConfirmItemInfo>();
        }
        fb.Refresh(info);

    }

    protected override void OnHide()
    {
        base.OnHide();
        Release();
    }

    public override void Release(bool depthRelease = true)
    {
        base.Release(depthRelease);
        Transform childTrans = m_trans_Member.Find("GameObject");
        if (childTrans == null)
        {
            Log.Error("childTrans is null");
            return;
        }
        FBConfirmItemInfo[] arr = childTrans.GetComponentsInChildren<FBConfirmItemInfo>();
        foreach (var info in arr)
        {
            info.Release(depthRelease);
        }

    }
    void onClick_Close_Btn(GameObject caster)
    {
        HideSelf();
    }

    void onClick_Btn_queding_Btn(GameObject caster)
    {
        stAnsTeamCopyUserCmd_CS cmd = new stAnsTeamCopyUserCmd_CS();
        cmd.ans = true;
        cmd.copy_base_id = CopyDataManager.EnterCopyID;
        NetService.Instance.Send(cmd);

        m_btn_btn_queding.isEnabled = false;
    }

    void onClick_Btn_quxiao_Btn(GameObject caster)
    {
        stAnsTeamCopyUserCmd_CS cmd = new stAnsTeamCopyUserCmd_CS();
        cmd.ans = false;
        cmd.copy_base_id = CopyDataManager.EnterCopyID;
        NetService.Instance.Send(cmd);
        HideSelf();
    }

}

