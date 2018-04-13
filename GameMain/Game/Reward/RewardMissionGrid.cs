//*************************************************************************
//	创建日期:	2017-2-9 14:23
//	文件名称:	RewardMissionGrid.cs
//  创 建 人:   	Chengxue.Zhao
//	版权所有:	中青宝
//	说    明:	悬赏发布任务详情
//*************************************************************************
using System;
using System.Collections.Generic;
using UnityEngine;

class RewardMissionGrid :MonoBehaviour
{
    UISprite m_spriteIcon = null;
    UILabel m_labelName = null;
    UILabel m_labelExp = null;
    GameObject m_goProcessing = null;
    GameObject m_goDone = null;
    GameObject m_goGet = null;

    RewardMisssionInfo m_missionInfo = null;
    void Awake()
    {
        m_spriteIcon = transform.Find("icon").GetComponent<UISprite>();
        m_labelName = transform.Find("name").GetComponent<UILabel>();
        m_labelExp = transform.Find("expLabel").GetComponent<UILabel>();

        m_goProcessing = transform.Find("processing").gameObject;

        m_goDone = transform.Find("done").gameObject;

        m_goGet = transform.Find("getbtn").gameObject;
        UIEventListener.Get(m_goGet).onClick = (go) =>
        {
            if (m_missionInfo != null)
            {
                NetService.Instance.Send(new GameCmd.stPublicTokenTaskFinishScriptUserCmd_C()
                {
                    tokentaskid = m_missionInfo.id,
                    userid = Client.ClientGlobal.Instance().MainPlayer.GetID(),
                });
            }
        };
    }

    public void Init(RewardMisssionInfo rInfo)
    {
        m_missionInfo = rInfo;
        if (m_spriteIcon != null)
        {
            m_spriteIcon.spriteName = rInfo.strIcon;
        }

        if (m_labelName != null)
        {
            m_labelName.text = rInfo.strName;
        }

        if (m_labelExp != null)
        {
            table.QuestDataBase taskdb = GameTableManager.Instance.GetTableItem<table.QuestDataBase>(rInfo.ntaskid);
            if (taskdb != null)
            {
                m_labelExp.text = taskdb.dwRewardExp.ToString();
            }
        }

        if (m_goGet != null)
        {
            m_goGet.gameObject.SetActive(rInfo.nstate == (int)GameCmd.TokenTaskState.TOKEN_STATE_FINISH);
        }
        if (m_goDone != null)
        {
            m_goDone.SetActive(rInfo.nstate == (int)GameCmd.TokenTaskState.TOKEN_STATE_ACCEPT);
        }
        if (m_goProcessing != null)
        {
            m_goProcessing.SetActive(rInfo.nstate == (int)GameCmd.TokenTaskState.TOKEN_STATE_PUBLISH);
        }
    }
}