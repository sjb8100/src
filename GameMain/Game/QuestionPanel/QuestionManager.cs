using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common;
using UnityEngine;
using GameCmd;
using table;
using Client;


public class QuestionManager : BaseModuleData, IManager
{
    private RewardStatus questionStatus = RewardStatus.Reward_Lock;
    public RewardStatus QuestionStatus
    {
        set 
        {
            questionStatus = value;
        }
        get 
        {
            return questionStatus;
        }   
    }
    private RewardStatus feedBackStatus = RewardStatus.Reward_Lock;
    public RewardStatus FeedBackStatus
    {
        set
        {
            feedBackStatus = value;
        }
        get
        {
            return feedBackStatus;
        }
    }
    private uint feedRewardLeftTime = 0;
    public uint FeedRewardLeftTime
    {
        set
        {
            feedRewardLeftTime = value;
        }
        get
        {
            return feedRewardLeftTime;
        }
    }
    public bool canGetFeed = false;
    private Dictionary<uint, stSendRewardQuestionPropertyUserCmd_S.QuestionInfo> questDic = new Dictionary<uint, stSendRewardQuestionPropertyUserCmd_S.QuestionInfo>();
    public Dictionary<uint, stSendRewardQuestionPropertyUserCmd_S.QuestionInfo> QuestDic
    {
        set 
        {
            questDic = value;
        }
        get 
        {
            return questDic;
        }
    }
    public void RecieveQuestionData(List<stSendRewardQuestionPropertyUserCmd_S.QuestionInfo> quests, RewardStatus feed, uint feedNum) 
    {
        canGetFeed = false;
        feedBackStatus = feed;
        feedRewardLeftTime = feedNum;
        bool showRed = false;
        for (int i = 0; i < quests.Count; i++ )
        {
            if (questDic.ContainsKey(quests[i].id))
            {
                questDic[quests[i].id] = quests[i];
            }
            else 
            {
                questDic.Add(quests[i].id, quests[i]);
            }
        }
        if (feedBackStatus == RewardStatus.Reward_Open)
        {
            showRed = true;
            if (feedBackStatus == RewardStatus.Reward_Open)
            {
                canGetFeed = true;
            }
            else 
            {
                canGetFeed = false;
            }
        }
        else 
        {
            showRed = false;
        }
        stShowMainPanelRedPoint st = new stShowMainPanelRedPoint()
        {
            modelID = (int)WarningEnum.Question,
            direction = (int)WarningDirection.None,
            bShowRed = showRed,
        };
        Engine.Utility.EventEngine.Instance().DispatchEvent((int)Client.GameEventID.MAINPANEL_SHOWREDWARING, st);
        Engine.Utility.EventEngine.Instance().DispatchEvent((int)Client.GameEventID.REFRESHQUESTIONPANEL, null);
    }

    public void Reset(bool depthClearData = false) 
    {
    
    }
    public void ClearData()
    {

    }
    public void Initialize()
    {

    }
    public void Process(float deltime)
    {
    
    }
}
