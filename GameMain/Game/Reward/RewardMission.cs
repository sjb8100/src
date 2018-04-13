//*************************************************************************
//	创建日期:	2016-10-10 16:23
//	文件名称:	RewardMission.cs
//  创 建 人:   	Chengxue.Zhao
//	版权所有:	中青宝
//	说    明:	我的悬赏任务条条 废弃
//*************************************************************************
using System;
using System.Collections.Generic;
using UnityEngine;

class RewardMission: MonoBehaviour,Engine.Utility.ITimer
{
    GameObject m_GoNoRelease_status = null;//没有发布
    GameObject m_GoReleasing_status = null;//已经发布
    GameObject m_GoReleasing = null;
    GameObject m_GoReleaseEnd = null;
    UISprite m_spriteIcon = null;
    UILabel m_lableName = null;
    UILabel m_lableExp = null;
    UILabel m_lableTime = null;
    UILabel m_lableReleasing = null;
    GameObject m_btnGiveUp = null;
    GameObject m_GoNoAccess_status = null;

    RewardPanel m_parent;

    const int TIMER_ID = 1003;
    bool m_bsetTimer = false;
    RewardMisssionInfo m_dataInfo;
    void Awake()
    {
        m_GoNoRelease_status = transform.Find("NoRelease_status").gameObject;
        m_GoReleasing_status = transform.Find("Releasing_status").gameObject;

        if (m_GoNoRelease_status != null)
        {
            GameObject releaseBtn = m_GoNoRelease_status.transform.Find("btn_GoRelease").gameObject;
            if (releaseBtn != null)
            {
                UIEventListener.Get(releaseBtn).onClick = OnGoRelease;
            }
        }

        if (m_GoReleasing_status != null)
        {
            m_GoReleasing = m_GoReleasing_status.transform.Find("Releasing").gameObject;
            m_GoReleaseEnd = m_GoReleasing_status.transform.Find("Releasend").gameObject;

            m_spriteIcon = m_GoReleasing_status.transform.Find("ReleasInfo/icon").GetComponent<UISprite>();
            m_lableName = m_GoReleasing_status.transform.Find("ReleasInfo/name").GetComponent<UILabel>();
            m_lableExp = m_GoReleasing_status.transform.Find("ReleasInfo/exp").GetComponent<UILabel>();
         
            if (m_GoReleasing != null)
            {
                m_lableTime = m_GoReleasing.transform.Find("time").GetComponent<UILabel>();
                //
                m_lableReleasing = m_GoReleasing.transform.Find("Releasinglable").GetComponent<UILabel>();
                m_btnGiveUp = m_GoReleasing.transform.Find("btn_GiveUp").gameObject;
                if (m_btnGiveUp != null)
                {
                    UIEventListener.Get(m_btnGiveUp).onClick = OnGiveUp;
                }
            }

            if (m_GoReleaseEnd != null)
            {
                GameObject getRewardBtn = m_GoReleaseEnd.transform.Find("btn_Receive").gameObject;
                if (getRewardBtn != null)
                {
                    UIEventListener.Get(getRewardBtn).onClick = OnGetReward;
                }
            }
        }

        m_GoNoAccess_status = transform.Find("NoAccess_status").gameObject;
        if (m_GoNoAccess_status != null)
        {
            GameObject getTaskBtn = m_GoNoAccess_status.transform.Find("btn_GoAccess").gameObject;
            if (getTaskBtn != null)
            {
                UIEventListener.Get(getTaskBtn).onClick = OnGetTask;
            }
        }
    }

    /// <summary>
    /// 发布悬赏
    /// </summary>
    /// <param name="go"></param>
    void OnGoRelease(GameObject go)
    {
        if (m_parent != null)
        {
           // m_parent.GoToRelease();
        }
    }

    void OnGetReward(GameObject go)
    {
        if (m_dataInfo != null)
        {
            if (m_dataInfo.nType == 1)
            {
                NetService.Instance.Send(new GameCmd.stPublicTokenTaskFinishScriptUserCmd_C()
                {
                    tokentaskid = m_dataInfo.id,
                    userid = Client.ClientGlobal.Instance().MainPlayer.GetID(),
                });
            }
            else
            {
                NetService.Instance.Send(new GameCmd.stAcceptTokenTaskFinishScriptUserCmd_C()
                {
                    tokentaskid = m_dataInfo.id,
                    //userid = Client.ClientGlobal.Instance().MainPlayer.GetID(),
                });
            }
        }
    }

    void OnGetTask(GameObject go)
    {
        if (m_parent != null)
        {
           // m_parent.GoToReceive();
        }
    }

    void OnGiveUp(GameObject go)
    {
        if (m_dataInfo != null)
        {
            NetService.Instance.Send(new GameCmd.stAcceptTokenTaskGiveupScriptUserCmd_C()
            {
                tokentaskid = m_dataInfo.id,
                //userid = Client.ClientGlobal.Instance().MainPlayer.GetID(),
            });
        }
    }

    public void InitUI(RewardMisssionInfo dataInfo,int ntype,RewardPanel parent) 
    {
        m_parent = parent;
        m_dataInfo = dataInfo;
        if (dataInfo == null)
        {
            if (m_GoNoAccess_status != null)
            {
                m_GoNoAccess_status.SetActive(ntype == 2); 
            }

            if (m_GoNoRelease_status != null)
            {
                m_GoNoRelease_status.SetActive(ntype == 1);
            }

            if (m_GoReleasing_status != null)
            {
                m_GoReleasing_status.SetActive(false);
            }
            return;
        }


        if (m_spriteIcon != null)
        {
            m_spriteIcon.spriteName = dataInfo.strIcon;
        }

        if (m_lableExp != null)
        {
            m_lableExp.text = dataInfo.nExp.ToString();
        }
        if (m_lableName != null)
        {
            m_lableName.text = dataInfo.strName;
        }

        ntype = (int)dataInfo.nType;
        
        Debug.Log("nType :" + ntype);
        if (m_dataInfo.nleftTime > 0)
        {
            m_lableTime.text = GetLeftTime((int)m_dataInfo.nleftTime);
        }
        if (!m_bsetTimer)
        {
            m_bsetTimer = true;
            Engine.Utility.TimerAxis.Instance().SetTimer(TIMER_ID, 1000, this);
        }
        if (m_GoNoAccess_status != null)
        {
            m_GoNoAccess_status.SetActive(false);
        }

        if (m_GoNoRelease_status != null)
        {
            m_GoNoRelease_status.SetActive(false);
        }

        if (m_GoReleasing_status != null)
        {
            m_GoReleasing_status.SetActive(true);
        }


        if (ntype == 1)
        {
            bool active = dataInfo.nstate == (int)GameCmd.TokenTaskState.TOKEN_STATE_PUBLISH || dataInfo.nstate == (int)GameCmd.TokenTaskState.TOKEN_STATE_ACCEPT;
            if (m_GoReleasing != null)
            {
                m_GoReleasing.SetActive(active);
                if (m_lableReleasing != null)
                {
                    m_lableReleasing.enabled = active;
                    if (dataInfo.nstate == (int)GameCmd.TokenTaskState.TOKEN_STATE_ACCEPT)
                    {
                        m_lableReleasing.text = "被接取";
                    }
                    else if (dataInfo.nstate == (int)GameCmd.TokenTaskState.TOKEN_STATE_PUBLISH)
                    {
                        m_lableReleasing.text = "发布中";
                    }
                }
            }
            if (m_GoReleaseEnd != null )
            {
                m_GoReleaseEnd.SetActive(dataInfo.nstate == 3);
            }
        }
        else
        {
            if (m_GoReleasing != null)
            {
                m_GoReleasing.SetActive(dataInfo.nstate == (int)GameCmd.TokenTaskState.TOKEN_STATE_ACCEPT);
            }
            if (m_GoReleaseEnd != null)
            {
                m_GoReleaseEnd.SetActive(dataInfo.nstate == (int)GameCmd.TokenTaskState.TOKEN_STATE_FINISH);
            }
            if (m_lableReleasing != null)
            {
                m_lableReleasing.enabled = false;
            }
            if (m_btnGiveUp != null)
            {
                m_btnGiveUp.SetActive(dataInfo.nstate != 3);
            }
        }
    }

    void OnDisable()
    {
        if (m_bsetTimer)
        {
            m_bsetTimer = false;
            Engine.Utility.TimerAxis.Instance().KillTimer(TIMER_ID, this);
        }
    }

    public void OnTimer(uint uTimerID)
    {
        if (uTimerID == TIMER_ID)
        {
            if (m_dataInfo != null )
            {
                if (m_dataInfo.nleftTime > 0)
                {
                    m_lableTime.text = GetLeftTime((int)m_dataInfo.nleftTime);
                }else
                {
                    m_bsetTimer = false;
                    Engine.Utility.TimerAxis.Instance().KillTimer(TIMER_ID, this);
                }
            }
        }
    }

    string GetLeftTime(int time)
    {
        string strTime = "";
        int h = (int)(time / 3600.0f);
        strTime = h > 0 ? h.ToString("00") : "00";
        int m = (int)((time - 3600 * h) / 60.0f);
        strTime += ":" + (m > 0 ? m.ToString("00") : "00");
        int s = (int)(time - 3600 * h - 60 * m);
        strTime += ":" + (s > 0 ? s.ToString("00") : "00");
        return strTime;
    }
}


