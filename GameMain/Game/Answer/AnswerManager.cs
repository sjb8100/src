using Client;
using Engine;
using GameCmd;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using table;
using UnityEngine;

//*******************************************************************************************
//	创建日期：	2018-3-28   10:37
//	文件名称：	LimitAnswerManager,cs
//	创 建 人：	Lin Chuang Yuan
//	版权所有：	ZQB
//	说    明：	限时答题
//*******************************************************************************************

public class RewardItemData
{
    public uint itemId;
    public uint itemNum;
}


class AnswerManager : BaseModuleData, IManager, ICopy
{
    #region Property
    public enum eAnswerState
    {
        BeforeAnswerRule,     // 还未开始答题， > 活动开启10s
        BeforeAnswerDesc,     // 10s -- 6s  活动即将开始，请准备（手机振动效果）
        BeforeAnswerDescCD,   // 6s -- 0s   5、4、3、2、1、开始答题
        InQuestion,           // 显示题目20s -- 5s
        InQuestionCD,         // 题目最后5s --0s 要弹出倒计时
        AnswerAnswer,         // 显示答案6s
        AnswerNextCD,         // 即将进入下一题（同时显示3、2、1） 
        AnswerEnd,            // 答题结束
    }

    /// <summary>
    /// 答题状态
    /// </summary>
    eAnswerState m_answerState = eAnswerState.AnswerEnd;

    public eAnswerState AnswerState
    {
        get
        {
            return m_answerState;
        }
    }

    /// <summary>
    /// 是否在答题副本
    /// </summary>
    bool b_InAnswerCopy = false;
    public bool InAnswerCopy
    {
        get
        {
            return b_InAnswerCopy;
        }
    }

    /// <summary>
    /// 复活卡ID
    /// </summary>
    uint m_fuHuoCardId;

    public uint FuHuoCardId
    {
        get
        {
            return m_fuHuoCardId;
        }
    }

    /// <summary>
    /// 最大题目数
    /// </summary>
    uint m_maxNum;

    public uint MaxNum
    {
        get
        {
            return m_maxNum;
        }
    }

    /// <summary>
    /// 下发题目前10s
    /// </summary>
    float m_preStartTime;

    public float PreStartTime
    {
        get
        {
            return m_preStartTime;
        }
    }

    /// <summary>
    /// 答题时间20s
    /// </summary>
    float m_questionTime;

    public float QuestionTime
    {
        get
        {
            return m_questionTime;
        }
    }

    /// <summary>
    /// 当前轮人数
    /// </summary>
    uint m_playerNum;
    public uint PlayerNum
    {
        get
        {
            return m_playerNum;
        }
    }

    /// <summary>
    /// 总奖金
    /// </summary>
    uint m_allMoney;
    public uint AllMoney
    {
        get
        {
            return m_allMoney;
        }
    }

    /// <summary>
    /// 服务器通知你获得的金币
    /// </summary>
    uint m_rewardMoney;
    public uint RewardMoney
    {
        get
        {
            return m_rewardMoney;
        }
    }

    /// <summary>
    /// 可以获取答题经验的区域
    /// </summary>
    List<int> m_lstExpZone;

    /// <summary>
    /// 题目Id
    /// </summary>
    uint m_questionId;
    public uint QuestionId
    {
        get
        {
            return m_questionId;
        }
    }

    /// <summary>
    /// 当前第几题
    /// </summary>
    uint m_questionIndex;
    public uint QuestionIndex
    {
        get
        {
            return m_questionIndex;
        }
    }

    /// <summary>
    /// 服务器下发答案
    /// </summary>
    bool m_answerTrueOrFalse;

    public bool AnswerTrueOrFalse
    {
        get
        {
            return m_answerTrueOrFalse;
        }
    }

    /// <summary>
    /// 答案时间
    /// </summary>
    float m_trueOrFalseTime;

    public float TrueOrFalseTime
    {
        get
        {
            return m_trueOrFalseTime;
        }
    }

    /// <summary>
    /// 玩家答案状态 1 对，2 错 ，3 复活
    /// </summary>
    int m_playerAnswerReslut;

    /// <summary>
    /// 是否在答题（true在答题区，false在非答题区）
    /// </summary>
    public bool IsAnswer
    {
        get
        {
            return IsInAnswerZone();
        }
    }

    /// <summary>
    /// OX 的坐标
    /// </summary>
    Vector3 XPos = Vector3.zero;
    Vector3 OPos = Vector3.zero;

    /// <summary>
    /// 跑马灯开始时间+活动结束总时间（支持多个日程用逗号分隔）
    /// </summary>
    List<uint> m_lstSchedule1 = new List<uint>();

    /// <summary>
    /// 进入变为观众的时间起、终时间（支持多个日程用逗号分隔）
    /// </summary>
    List<uint> m_lstSchedule2 = new List<uint>();

    /// <summary>
    /// 天雷特效
    /// </summary>
    IEffect effect = null;

    #endregion



    #region Interface

    public void ClearData()
    {

    }
    public void Initialize()
    {
        this.m_lstExpZone = GameTableManager.Instance.GetGlobalConfigList<int>("RightWrongExpZone");
        this.m_fuHuoCardId = GameTableManager.Instance.GetGlobalConfig<uint>("RightWrongFuHuo");
        this.m_maxNum = GameTableManager.Instance.GetGlobalConfig<uint>("RightWrongQuestionCnt");

        this.m_lstSchedule1 = GameTableManager.Instance.GetGlobalConfigList<uint>("RightWrongTime");
        this.m_lstSchedule2 = GameTableManager.Instance.GetGlobalConfigList<uint>("RightWrongGuestTime");

        List<uint> OXPos = GameTableManager.Instance.GetGlobalConfigList<uint>("RightWrongOXPos");
        if (OXPos.Count == 4)
        {
            OPos = new Vector3(OXPos[0], 3.7f, -OXPos[1]);
            XPos = new Vector3(OXPos[2], 3.7f, -OXPos[3]);
        }
        else
        {
            Engine.Utility.Log.Error("策划配置OX坐标不对！！！！");
        }

        Engine.Utility.EventEngine.Instance().AddEventListener((int)GameEventID.SYSTEM_LOADSCENECOMPELETE, OnEvent);
    }

    public void Reset(bool depthClearData = false)
    {

    }

    public void Process(float deltaTime)
    {
        UpdateTime(deltaTime);
    }

    /// <summary>
    /// 进入副本
    /// </summary>
    public void EnterCopy()
    {
        this.b_InAnswerCopy = true;

        this.m_playerAnswerReslut = 0;

        stCopyInfo info = new stCopyInfo();
        info.bShow = true;
        info.bShowBattleInfoBtn = false;
        DataManager.Manager<UIPanelManager>().SendMsg(PanelID.MainPanel, UIMsgID.eShowCopyInfo, info);
        DataManager.Manager<ComBatCopyDataManager>().CopyCDAndExitData = new CopyCDAndExitInfo { bShow = true, bShowBattleInfoBtn = false };


    }

    /// <summary>
    /// 退出副本
    /// </summary>
    public void ExitCopy()
    {
        this.b_InAnswerCopy = false;

        this.m_playerAnswerReslut = 0;

        this.m_preStartTime = 0;

        this.m_questionTime = 0;

        this.m_trueOrFalseTime = 0;

        this.m_allMoney = 0;

        this.m_playerNum = 0;

        this.m_answerState = eAnswerState.AnswerEnd;

        if (DataManager.Manager<UIPanelManager>().IsShowPanel(PanelID.MainPanel))
        {
            DataManager.Manager<UIPanelManager>().SendMsg(PanelID.MainPanel, UIMsgID.eAnswerState, null);
        }
    }

    #endregion

    #region event
    void OnEvent(int eventID, object param)
    {
        if (eventID == (int)GameEventID.SYSTEM_LOADSCENECOMPELETE)
        {
            if (IsAnswer)
            {
                this.m_answerState = eAnswerState.BeforeAnswerRule;
                //Engine.Utility.Log.Error("111---刚进答题副本，在答题区");
            }
            else
            {
                this.m_answerState = eAnswerState.AnswerEnd;
                this.m_playerAnswerReslut = 2;
                //Engine.Utility.Log.Error("222---刚进答题副本，在观众区");
            }

        }
    }
    #endregion

    #region Method

    void UpdateTime(float deltaTime)
    {
        if (this.b_InAnswerCopy == false)
        {
            return;
        }

        //答题前
        if (this.m_preStartTime > 0)
        {
            this.m_preStartTime -= deltaTime;
        }
        if (this.m_preStartTime > 6f)
        {
            if (this.m_answerState != eAnswerState.BeforeAnswerDesc)
            {
                this.m_answerState = eAnswerState.BeforeAnswerDesc;

                if (DataManager.Manager<UIPanelManager>().IsShowPanel(PanelID.MainPanel))
                {
                    DataManager.Manager<UIPanelManager>().SendMsg(PanelID.MainPanel, UIMsgID.eAnswerState, null);
                }
            }
        }

        if (this.m_preStartTime > 0 && this.m_preStartTime <= 6f)
        {
            if (this.m_answerState != eAnswerState.BeforeAnswerDescCD)
            {
                this.m_answerState = eAnswerState.BeforeAnswerDescCD;
                //调用手机震动
                Vibrate();
                if (DataManager.Manager<UIPanelManager>().IsShowPanel(PanelID.MainPanel))
                {
                    DataManager.Manager<UIPanelManager>().SendMsg(PanelID.MainPanel, UIMsgID.eAnswerState, null);
                }

            }
        }

        //出题目了
        if (this.m_questionTime > 0)
        {
            this.m_questionTime -= deltaTime;
        }
        if (this.m_questionTime > 5 && this.m_questionTime < 20)
        {
            if (this.m_answerState != eAnswerState.InQuestion)
            {
                this.m_answerState = eAnswerState.InQuestion;

                if (DataManager.Manager<UIPanelManager>().IsShowPanel(PanelID.MainPanel))
                {
                    DataManager.Manager<UIPanelManager>().SendMsg(PanelID.MainPanel, UIMsgID.eAnswerState, null);
                }
            }
        }

        if (this.m_questionTime > 0 && this.m_questionTime <= 5)
        {
            if (this.m_answerState != eAnswerState.InQuestionCD)
            {
                this.m_answerState = eAnswerState.InQuestionCD;

                if (DataManager.Manager<UIPanelManager>().IsShowPanel(PanelID.MainPanel))
                {
                    DataManager.Manager<UIPanelManager>().SendMsg(PanelID.MainPanel, UIMsgID.eAnswerState, null);
                }

                //倒计时CD
                DataManager.Manager<UIPanelManager>().ShowPanel(PanelID.AnswerCDPanel);
            }
        }

        //答案显示3s
        if (this.m_trueOrFalseTime > 0)
        {
            this.m_trueOrFalseTime -= deltaTime;
        }
        if (this.m_trueOrFalseTime > 3 && this.m_trueOrFalseTime < 9)
        {
            if (this.m_answerState != eAnswerState.AnswerAnswer)
            {
                this.m_answerState = eAnswerState.AnswerAnswer;

                if (DataManager.Manager<UIPanelManager>().IsShowPanel(PanelID.MainPanel))
                {
                    DataManager.Manager<UIPanelManager>().SendMsg(PanelID.MainPanel, UIMsgID.eAnswerState, null);
                }
            }
        }

        //对或者复活
        if (this.m_trueOrFalseTime > 0 && this.m_trueOrFalseTime <= 3)
        {
            //最后一题
            if (this.QuestionIndex == 20)
            {
                return;
            }

            //对
            if (m_playerAnswerReslut == 1)
            {
                if (this.m_answerState != eAnswerState.AnswerNextCD)
                {
                    this.m_answerState = eAnswerState.AnswerNextCD;
                    //TipsManager.Instance.ShowTips("恭喜你回答正确");

                    if (DataManager.Manager<UIPanelManager>().IsShowPanel(PanelID.MainPanel))
                    {
                        DataManager.Manager<UIPanelManager>().SendMsg(PanelID.MainPanel, UIMsgID.eAnswerState, null);
                    }
                }
            }
            //复活
            if (m_playerAnswerReslut == 3)
            {
                if (this.m_answerState != eAnswerState.AnswerNextCD)
                {
                    this.m_answerState = eAnswerState.AnswerNextCD;
                    //TipsManager.Instance.ShowTips("回答错误，已自动使用复活");

                    if (DataManager.Manager<UIPanelManager>().IsShowPanel(PanelID.MainPanel))
                    {
                        DataManager.Manager<UIPanelManager>().SendMsg(PanelID.MainPanel, UIMsgID.eAnswerState, null);
                    }
                }
            }

            //观众玩家(错或者刚进)
            if (m_playerAnswerReslut == 2 || m_playerAnswerReslut == 0)
            {
                if (this.m_answerState != eAnswerState.AnswerNextCD)
                {
                    this.m_answerState = eAnswerState.AnswerNextCD;

                    if (DataManager.Manager<UIPanelManager>().IsShowPanel(PanelID.MainPanel))
                    {
                        DataManager.Manager<UIPanelManager>().SendMsg(PanelID.MainPanel, UIMsgID.eAnswerState, null);
                    }
                }
            }

        }

        if (m_playerAnswerReslut == 2)//错
        {
            //if (this.m_trueOrFalseTime <= 0)
            //{
            //    if (this.m_answerState != eAnswerState.AnswerEnd)
            //    {
            //        //this.m_answerState = eAnswerState.AnswerEnd;
            //        //TipsManager.Instance.ShowTips("回答错误，你已进入观众席");
            //    }
            //}
        }
    }

    /// <summary>
    /// 是否在答题区域
    /// </summary>
    /// <returns></returns>
    public bool IsInAnswerZone()
    {
        IPlayer mainPlayer = Client.ClientGlobal.Instance().MainPlayer;
        if (mainPlayer == null)
        {
            return false;
        }

        if (m_lstExpZone.Count != 4)
        {
            Engine.Utility.Log.Error("答题区域数据配置错误！！！");
        }

        Vector3 pos = mainPlayer.GetPos();
        float x = pos.x;
        float z = -pos.z;

        if (x < m_lstExpZone[0])
        {
            return false;
        }
        else if (z > m_lstExpZone[1])
        {
            return false;
        }
        else if (x > m_lstExpZone[2])
        {
            return false;
        }
        else if (z < m_lstExpZone[3])
        {
            return false;
        }

        return true;
    }

    /// <summary>
    /// 调用手机震动
    /// </summary>
    void Vibrate()
    {
        Handheld.Vibrate();
    }

    /// <summary>
    /// 获取复活卡数量
    /// </summary>
    /// <returns></returns>
    public int GetFuHuoNum()
    {
        return DataManager.Manager<ItemManager>().GetItemNumByBaseId(this.m_fuHuoCardId);//道具存量   
    }

    public long GetPreStartLeftTime()
    {
        uint scheduleId = 0;

        for (int i = 0; i < m_lstSchedule1.Count; i++)
        {
            long templeftTime;
            if (DataManager.Manager<DailyManager>().InSchedule(m_lstSchedule1[i], out templeftTime))
            {
                scheduleId = m_lstSchedule1[i];
            }
        }

        long leftTime = 0;
        if (scheduleId != 0)
        {
            DataManager.Manager<DailyManager>().InSchedule(scheduleId, out leftTime);
        }

        long totleSeconds = GetTotleSeconds();

        return leftTime - totleSeconds > 0 ? leftTime - totleSeconds : 0;
    }

    long GetTotleSeconds()
    {
        uint scheduleId = m_lstSchedule2.Count > 0 ? m_lstSchedule2[0] : 0;

        if (scheduleId != 0)
        {
            return DataManager.Manager<DailyManager>().GetScheduleTotleSecondsByID(scheduleId);
        }
        return 0;
    }

    void AddEffect(Vector3 pos)
    {
        uint resID = 50054;
        ResourceDataBase resDB = GameTableManager.Instance.GetTableItem<ResourceDataBase>(resID);
        if (resDB == null)
        {
            Engine.Utility.Log.Error("EffectViewFactory:找不到特效资源路径配置{0}");
            return;
        }
        string path = resDB.strPath;

        Engine.IRenderSystem rs = Engine.RareEngine.Instance().GetRenderSystem();
        if (rs == null)
        {
            return;
        }

        if (effect != null)
        {
            effect.Release();
        }
        rs.CreateEffect(ref path, ref effect, (obj) =>
        {
            if (obj == null)
            {
                return;
            }
            if (obj.GetNodeTransForm() == null)
            {
                return;
            }
            obj.GetNodeTransForm().localPosition = pos;
            obj.GetNodeTransForm().localRotation = Quaternion.identity;
            obj.GetNodeTransForm().localScale = Vector3.one;

            //Engine.Utility.Log.Error("--->>特效坐标：" + mainPlayer.GetPos());
            //DoPlay(obj.GetNodeTransForm().gameObject, position, rotation, scale);
        });
    }

    #endregion


    #region Net

    /// <summary>
    /// 玩家主动进入答题副本 //邀请玩家进入答题副本
    /// </summary>
    /// <param name="cmd"></param>
    public void OnAnswerEnter(stRightWrongEnterCopyUserCmd_CS cmd)
    {
        if (false == InAnswerCopy)
        {
            Action yes = delegate
            {
                ReqEnterAnswerCopy();
            };

            string des = "百万答题即将开启，是否参与？";
            TipsManager.Instance.ShowTipWindow(10, 0, Client.TipWindowType.YesNO, des, yes, null, okstr: "立即参与", cancleStr: "取消");
        }
    }


    public void ReqEnterAnswerCopy()
    {
        stRightWrongEnterCopyUserCmd_CS sendCmd = new stRightWrongEnterCopyUserCmd_CS();
        NetService.Instance.Send(sendCmd);
    }

    /// <summary>
    /// 答题开始前10s
    /// </summary>
    /// <param name="cmd"></param>
    public void OnAnswerPreStart(stRightWrongPreStartCopyUserCmd_S cmd)
    {
        this.m_preStartTime = 10f;
    }


    /// <summary>
    /// 下发题目
    /// </summary>
    /// <param name="cmd"></param>
    public void OnAnswerQuestion(stRightWrongQuestionCopyUserCmd_S cmd)
    {
        this.m_questionId = cmd.id;
        this.m_questionIndex = cmd.no;

        this.m_questionTime = 20f;
        this.m_preStartTime = 0;        //清时间
        this.m_trueOrFalseTime = 0;     //清时间
    }

    /// <summary>
    /// 下发答案
    /// </summary>
    /// <param name="cmd"></param>
    public void OnAnswerAns(stRightWrongAnsCopyUserCmd_S cmd)
    {
        this.m_answerTrueOrFalse = cmd.ans;

        if (this.m_playerAnswerReslut == 2)
        {
            this.m_trueOrFalseTime = 9f;

            this.m_questionTime = 0;
        }

        Engine.CorotinueInstance.Instance.StartCoroutine(DelayToShowTips(this.m_answerTrueOrFalse));

        if (DataManager.Manager<UIPanelManager>().IsShowPanel(PanelID.MainPanel))
        {
            DataManager.Manager<UIPanelManager>().SendMsg(PanelID.MainPanel, UIMsgID.eAnswerState, null);
        }
    }

    /// <summary>
    /// 下发个人结果 1 对 2 错 3 复活
    /// </summary>
    /// <param name="cmd"></param>
    public void OnAnswerResult(stRightWrongResultCopyUserCmd_S cmd)
    {
        this.m_playerAnswerReslut = cmd.result;

        if (cmd.result == 1)      //对
        {
            this.m_trueOrFalseTime = 9f;
            TipsManager.Instance.ShowTips("恭喜你回答正确");
        }
        else if (cmd.result == 2) //错
        {
            this.m_trueOrFalseTime = 9f;
            TipsManager.Instance.ShowTips("回答错误，你已进入观众席");
        }
        else if (cmd.result == 3) //复活
        {
            this.m_trueOrFalseTime = 9f;
            TipsManager.Instance.ShowTips("回答错误，已自动使用复活");
        }



        this.m_questionTime = 0;  //清时间
    }

    IEnumerator DelayToShowTips(bool b)
    {
        yield return new WaitForSeconds(3f);

        if (b)
        {
            AddEffect(XPos);
        }
        else
        {
            AddEffect(OPos);
        }

    }

    /// <summary>
    /// 当前第几题 和题目id 和题目已经开始几秒了
    /// </summary>
    /// <param name="cmd"></param>
    public void OnAnswerCurInfo(stRightWrongCurInfoCopyUserCmd_S cmd)
    {
        this.m_questionId = (uint)cmd.question_id;
        this.m_questionIndex = cmd.no;
        this.m_playerNum = (uint)cmd.player_cnt;

        if (cmd.sec > 0 && cmd.sec <= 20)
        {
            this.m_questionTime = 20 - cmd.sec;
            this.m_trueOrFalseTime = 0;
        }
        else if (cmd.sec > 20 && cmd.sec <= 29)
        {
            this.m_questionTime = 0;
            this.m_trueOrFalseTime = 29 - cmd.sec;
        }

        if (DataManager.Manager<UIPanelManager>().IsShowPanel(PanelID.MissionAndTeamPanel))
        {
            DataManager.Manager<UIPanelManager>().SendMsg(PanelID.MissionAndTeamPanel, UIMsgID.eAnswerCurInfo, null);
        }
    }

    /// <summary>
    /// 当前轮参与人数 总奖金 每轮结束后广播
    /// </summary>
    /// <param name="cmd"></param>
    public void OnAnswerPlayer(stRightWrongPlayerCopyUserCmd_S cmd)
    {
        this.m_playerNum = cmd.player_cnt;
        this.m_allMoney = cmd.money;

        if (DataManager.Manager<UIPanelManager>().IsShowPanel(PanelID.MissionAndTeamPanel))
        {
            DataManager.Manager<UIPanelManager>().SendMsg(PanelID.MissionAndTeamPanel, UIMsgID.eAnswerCurInfo, null);
        }
    }

    /// <summary>
    /// 获取奖励
    /// </summary>
    /// <param name="cmd"></param>
    public void OnAnswerReward(stRightWrongRewardCopyUserCmd_CS cmd)
    {
        this.m_rewardMoney = cmd.money;

        //奖励
        RewardItemData itemData = new RewardItemData();
        itemData.itemId = MainPlayerHelper.GoldID;
        itemData.itemNum = this.m_rewardMoney;

        List<RewardItemData> list = new List<RewardItemData>();
        list.Add(itemData);

        if (cmd.all_right == false)       //未通关
        {
            DataManager.Manager<UIPanelManager>().ShowPanel(PanelID.AnswerBreakPanel, data: list);
        }
        else //通关
        {
            DataManager.Manager<UIPanelManager>().ShowPanel(PanelID.AnswerSuccessPanel, data: itemData);
        }

        //答题结束
        this.m_answerState = eAnswerState.AnswerEnd;
        this.m_questionTime = 0;                  //清时间
        this.m_trueOrFalseTime = 0;               //清时间
    }

    /// <summary>
    /// 活动结束奖励 n秒后强制提出副本
    /// </summary>
    /// <param name="cmd"></param>
    public void OnAnswerFinish(stRightWrongFinishCopyUserCmd_S cmd)
    {
        Action yes = delegate
        {
            DataManager.Manager<ComBatCopyDataManager>().ReqExitCopy();
        };

        string des = "本轮答题已经结束，即将返回主场景";
        TipsManager.Instance.ShowTipWindow(10, 0, Client.TipWindowType.YesNO, des, yes, null, okstr: "立即返回", cancleStr: "取消");
    }

    #endregion

}

