using GameCmd;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using table;



public class DailyAnswerInfo
{
    public string question;
    public List<DailyAnswerAnswer> answer;
}

public class DailyAnswerAnswer
{
    public uint index;
    public string answerDes;
}

class DailyAnswerManager : BaseModuleData, IManager
{

    /// <summary>
    /// 已经回答到第几道题目
    /// </summary>
    uint m_alreadyAnswerNum;

    public uint AlreadyAnswerNum
    {
        get
        {
            return m_alreadyAnswerNum;
        }
    }


    /// <summary>
    /// 当前题目ID
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
    /// 答对数量
    /// </summary>
    uint m_correctNum;
    public uint CorrectNum
    {
        get
        {
            return m_correctNum;
        }
    }

    /// <summary>
    /// 经验奖励
    /// </summary>
    uint m_expReward;
    public uint ExpReward
    {
        get
        {
            return m_expReward;
        }
    }

    /// <summary>
    /// 金币奖励
    /// </summary>
    uint m_coinReward;
    public uint CoinReward
    {
        get
        {
            return m_coinReward;
        }
    }

    /// <summary>
    /// 答案
    /// </summary>
    List<DailyAnswerAnswer> m_lstDailyAnswerAnswer = new List<DailyAnswerAnswer>();

    /// <summary>
    /// 日常Id
    /// </summary>
    /// 
    uint m_dailyAnswerDailyId;

    public uint DailyAnswerDailyId
    {
        get
        {
            return m_dailyAnswerDailyId;
        }
    }

    /// <summary>
    /// 每日答题上限
    /// </summary>
    /// 
    uint m_dailyAnswerMax;

    public uint DailyAnswerMax
    {
        get
        {
            return m_dailyAnswerMax;
        }
    }


    /// <summary>
    /// 答对几道题才可领取奖励
    /// </summary>
    /// 

    uint m_dailyAnswerRewardLimit;

    public uint DailyAnswerRewardLimit
    {
        get
        {
            return m_dailyAnswerRewardLimit;
        }
    }


    /// <summary>
    /// 是否已经领取奖励
    /// </summary>
    bool m_isReceivedReward;

    public bool IsReceivedReward
    {
        get
        {
            return m_isReceivedReward;
        }
    }

    //只有新题目下来才可以回答，每道题目只有一次机会（只能选一次）。
    public bool CanAnswer = false;

    #region interface
    /// <summary>
    /// 初始化
    /// </summary>
    public void Initialize()
    {
        m_dailyAnswerDailyId = GameTableManager.Instance.GetGlobalConfig<uint>("DailyAnswerDailyID");

        m_dailyAnswerMax = GameTableManager.Instance.GetGlobalConfig<uint>("DailyAnswerMax");

        m_dailyAnswerRewardLimit = GameTableManager.Instance.GetGlobalConfig<uint>("DailyAnswerRewardLimit");
    }

    /// <summary>
    /// 重置
    /// </summary>
    /// <param name="depthClearData">是否深度清除管理器数据</param>
    public void Reset(bool depthClearData = false)
    {

    }

    public void Process(float deltaTime)
    {

    }

    public void ClearData()
    {

    }

    #endregion


    #region method

    public DailyAnswerInfo GetDailyAnswerInfo()
    {
        DailyAnswerInfo dailyAnswerInfo = null;

        DailyAnswerDatabase DailyAnswerDb = GameTableManager.Instance.GetTableItem<DailyAnswerDatabase>(m_questionId);

        if (DailyAnswerDb != null)
        {
            dailyAnswerInfo = new DailyAnswerInfo();
            dailyAnswerInfo.question = DailyAnswerDb.question;

            List<DailyAnswerAnswer> answerList = GetDailyAnswerAnswer(DailyAnswerDb);
            if (answerList == null)
            {
                return null;
            }
            else
            {
                dailyAnswerInfo.answer = answerList;
            }
        }

        return dailyAnswerInfo;
    }


    List<DailyAnswerAnswer> GetDailyAnswerAnswer(DailyAnswerDatabase DailyAnswerDb)
    {
        m_lstDailyAnswerAnswer.Clear();

        DailyAnswerAnswer question1 = new DailyAnswerAnswer();
        question1.index = 1;    //只有index == 1 才是正确答案
        question1.answerDes = DailyAnswerDb.answer1;

        DailyAnswerAnswer question2 = new DailyAnswerAnswer();
        question2.index = 2;
        question2.answerDes = DailyAnswerDb.answer2;

        DailyAnswerAnswer question3 = new DailyAnswerAnswer();
        question3.index = 3;
        question3.answerDes = DailyAnswerDb.answer3;

        DailyAnswerAnswer question4 = new DailyAnswerAnswer();
        question4.index = 4;
        question4.answerDes = DailyAnswerDb.answer4;

        m_lstDailyAnswerAnswer.Add(question1);
        m_lstDailyAnswerAnswer.Add(question2);
        m_lstDailyAnswerAnswer.Add(question3);
        m_lstDailyAnswerAnswer.Add(question4);

        //随机答案
        m_lstDailyAnswerAnswer.Sort(ListRandom);

        return m_lstDailyAnswerAnswer;
    }

    /// <summary>
    /// list 随机
    /// </summary>
    /// <param name="left"></param>
    /// <param name="right"></param>
    /// <returns></returns>
    int ListRandom(DailyAnswerAnswer left, DailyAnswerAnswer right)
    {
        return (new Random()).Next(-1, 1);
    }

    /// <summary>
    /// 只有index == 1 才是正确答案
    /// </summary>
    /// <param name="index"></param>
    /// <returns></returns>
    public bool AnswerIsCorrect(uint index)
    {
        if (index == 1)
        {
            if (this.m_correctNum < m_dailyAnswerMax)
            {
                this.m_correctNum++;
            }
            else
            {
                Engine.Utility.Log.Error("超过题目上限 ！！！");
            }

        }
        return index == 1;
    }


    #endregion

    #region Net


    public void OnAnswerData(stAnswerDataDataUserCmd_S cmd)
    {
        this.m_questionId = cmd.cur_qid;

        this.m_alreadyAnswerNum = cmd.answer_num;

        this.m_correctNum = cmd.correct_num;

        this.m_coinReward = cmd.money;

        this.m_expReward = cmd.exp;

        this.m_isReceivedReward = cmd.have_reward;
    }

    /// <summary>
    /// 请求新题目
    /// </summary>
    public void ReqNewQuestion()
    {
        if (m_alreadyAnswerNum < m_dailyAnswerMax)
        {
            stNewQuestionDataUserCmd_CS cmd = new stNewQuestionDataUserCmd_CS();
            NetService.Instance.Send(cmd);
        }
    }

    public void OnNewQuestionData(stNewQuestionDataUserCmd_CS cmd)
    {
        this.m_questionId = cmd.qid;

        this.CanAnswer = true;

        if (DataManager.Manager<UIPanelManager>().IsShowPanel(PanelID.DailyAnswerPanel))
        {
            DataManager.Manager<UIPanelManager>().SendMsg(PanelID.DailyAnswerPanel, UIMsgID.eDailyAnswerNewQ, null);
        }
    }

    /// <summary>
    /// 提交答案
    /// </summary>
    /// <param name="qId">题目</param>
    /// <param name="index">答案</param>
    public void ReqCommitAnswer(uint index)
    {
        stCommitAnswerQDataUserCmd_CS cmd = new stCommitAnswerQDataUserCmd_CS();
        cmd.question_id = this.m_questionId;
        cmd.index = index;
        NetService.Instance.Send(cmd);
    }

    public void OnCommitAnswer(stCommitAnswerQDataUserCmd_CS cmd)
    {
        this.m_alreadyAnswerNum++;

        if (DataManager.Manager<UIPanelManager>().IsShowPanel(PanelID.DailyAnswerPanel))
        {
            DataManager.Manager<UIPanelManager>().SendMsg(PanelID.DailyAnswerPanel, UIMsgID.eDailyAnswerNewQ, null);
        }
    }

    public void ReqAnswerReward()
    {
        stGetAnswerRewardDataUserCmd_CS cmd = new stGetAnswerRewardDataUserCmd_CS();
        NetService.Instance.Send(cmd);
    }

    /// <summary>
    /// 奖励礼包
    /// </summary>
    /// <param name="cmd"></param>
    public void OnGetAnswerReward(stGetAnswerRewardDataUserCmd_CS cmd)
    {
        this.m_isReceivedReward = true;

        if (DataManager.Manager<UIPanelManager>().IsShowPanel(PanelID.DailyAnswerPanel))
        {
            DataManager.Manager<UIPanelManager>().SendMsg(PanelID.DailyAnswerPanel, UIMsgID.eDailyAnswerReward, null);
        }
    }

    /// <summary>
    /// 金币和经验奖励
    /// </summary>
    /// <param name="cmd"></param>
    public void OnGetCoinAndExpReward(stRefreshAnswerDataUserCmd_S cmd)
    {
        this.m_coinReward = cmd.money;
        this.m_expReward = cmd.exp;
    }

    #endregion

}

