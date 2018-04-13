using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using table;
using UnityEngine;

partial class DailyAnswerPanel
{
    #region property
    DailyDataBase m_dailyDb;

    List<UIDailyAnswerAnswerGrid> m_lstGrid = new List<UIDailyAnswerAnswerGrid>();

    #endregion


    #region override

    protected override void OnInitPanelData()
    {
        base.OnInitPanelData();
    }

    protected override void OnLoading()
    {
        base.OnLoading();

        for (int i = 0; i < m_grid_Grid.transform.childCount; i++)
        {
            Transform ts = m_grid_Grid.transform.GetChild(i);
            UIDailyAnswerAnswerGrid grid = ts.gameObject.GetComponent<UIDailyAnswerAnswerGrid>();
            if (grid == null)
            {
                grid = ts.gameObject.AddComponent<UIDailyAnswerAnswerGrid>();
            }

            grid.ReSetRightOrWrong();
            grid.SetSelect(false);
        }

        uint dailyId = DataManager.Manager<DailyAnswerManager>().DailyAnswerDailyId;
        m_dailyDb = GameTableManager.Instance.GetTableItem<DailyDataBase>(dailyId);
    }

    protected override void OnPrepareShow(object data)
    {
        base.OnPrepareShow(data);
    }

    protected override void OnShow(object data)
    {
        //请求新题目
        DataManager.Manager<DailyAnswerManager>().ReqNewQuestion();

        ShowUI();
    }

    public override bool OnMsg(UIMsgID msgid, object param)
    {
        if (msgid == UIMsgID.eDailyAnswerNewQ) //每日答题 新题
        {
            //正确率
            UpdateCorrectRate();

            //经验金币
            UpdateCoinAndExpReward();

            //第几题
            UpdateQuestionIndexLabel();

            //题目和答案
            UpdateQuestionAndAnswer();

            //背景
            UpdateBg();

            //宝箱奖励
            UpdateReawrd();
        }
        else if (msgid == UIMsgID.eDailyAnswerReward)  //每日答题 奖励领取
        {
            //奖励大礼包
            UpdateReawrd();
        }
        else if (msgid == UIMsgID.eDailyAnswerEachReward) //每题奖励
        {
            //经验金币
            UpdateCoinAndExpReward();
        }

        return true;
    }

    protected override void OnHide()
    {
        Release();
    }

    protected override void OnPanelBaseDestory()
    {
        base.OnPanelBaseDestory();
    }

    public override bool ExecuteReturnLogic()
    {
        if (CheckShowGiftbagGetPanel())
        {
            ShowGiftbagGetPanel();
            return true;
        }

        return false;
    }


    public override void Release(bool depthRelease = true)
    {
        base.Release(depthRelease);
    }

    #endregion


    #region mono

    float temptime = 0;
    void Update()
    {
        temptime += Time.deltaTime;
        if (temptime > 0.95f)
        {
            UpdateLeftTime();

            temptime = 0;
        }
    }

    #endregion


    #region method
    void ShowUI()
    {
        //正确率
        UpdateCorrectRate();

        //经验金币
        UpdateCoinAndExpReward();

        //第几题
        UpdateQuestionIndexLabel();

        //题目和答案
        UpdateQuestionAndAnswer();

        //奖励大礼包
        UpdateReawrd();

        //背景
        UpdateBg();

        ResetGridState();
    }

    /// <summary>
    /// 剩余时间
    /// </summary>
    void UpdateLeftTime()
    {
        if (m_dailyDb != null)
        {
            long leftSeconds = 0;
            if (DataManager.Manager<DailyManager>().UpdateDataLeftTime(m_dailyDb, out leftSeconds))
            {
                string leftTimeStr = StringUtil.GetStringBySeconds((uint)leftSeconds);

                string str = string.Format("剩余时间：{0}", leftTimeStr);

                m_label_lasttime_label.text = str;
            }
        }

    }

    /// <summary>
    /// 正确率
    /// </summary>
    void UpdateCorrectRate()
    {
        //打对数
        uint correctNum = DataManager.Manager<DailyAnswerManager>().CorrectNum;
        uint alreadyAnswerNum = DataManager.Manager<DailyAnswerManager>().AlreadyAnswerNum;

        m_label_right_label.text = string.Format("正确率：{0}/{1}", correctNum, alreadyAnswerNum);
    }

    /// <summary>
    /// 左侧的奖励大礼包
    /// </summary>
    void UpdateReawrd()
    {
        if (DataManager.Manager<DailyAnswerManager>().IsReceivedReward)
        {
            //不可领
            m__BoxClose.gameObject.SetActive(false);

            //可领
            m_btn_BoxOpen.gameObject.SetActive(false);

            //已经领
            m__BoxAlreadyOpen.gameObject.SetActive(true);
        }
        else
        {
            m__BoxAlreadyOpen.gameObject.SetActive(false);

            uint CorrectNum = DataManager.Manager<DailyAnswerManager>().CorrectNum;
            uint DailyAnswerRewardLimit = DataManager.Manager<DailyAnswerManager>().DailyAnswerRewardLimit;

            m_label_bottomtitle_label.text = string.Format("答对{0}题可以领取", DailyAnswerRewardLimit);

            //达到领取奖励条件
            if (CorrectNum >= DailyAnswerRewardLimit)
            {
                m_btn_BoxOpen.gameObject.SetActive(true);
                m__BoxClose.gameObject.SetActive(false);
                PlayEffect();
            }
            else
            {
                m_btn_BoxOpen.gameObject.SetActive(false);
                m__BoxClose.gameObject.SetActive(true);
            }
        }
    }

    /// <summary>
    /// 宝箱领取提示特效
    /// </summary>
    void PlayEffect()
    {
        //特效
        UIParticleWidget wight = m_btn_BoxOpen.GetComponent<UIParticleWidget>();
        if (null == wight)
        {
            wight = m_btn_BoxOpen.gameObject.AddComponent<UIParticleWidget>();
            wight.depth = 100;
        }

        if (wight != null)
        {
            wight.ReleaseParticle();
            wight.AddParticle(50035);
        }
    }

    void UpdateBg()
    {
        uint AlreadyAnswerNum = DataManager.Manager<DailyAnswerManager>().AlreadyAnswerNum;
        uint DailyAnswerMax = DataManager.Manager<DailyAnswerManager>().DailyAnswerMax;
        if (AlreadyAnswerNum == DailyAnswerMax)
        {
            m_trans_complated.gameObject.SetActive(true);
            m_trans_right.gameObject.SetActive(false);
        }
        else
        {
            m_trans_complated.gameObject.SetActive(false);
            m_trans_right.gameObject.SetActive(true);
        }
    }

    /// <summary>
    /// 经验和金币奖励
    /// </summary>
    void UpdateCoinAndExpReward()
    {
        m_label_exp_label.text = string.Format("获得经验：{0}", DataManager.Manager<DailyAnswerManager>().ExpReward);
        m_label_gold_label.text = string.Format("获得金币：{0}", DataManager.Manager<DailyAnswerManager>().CoinReward);
    }

    /// <summary>
    /// 第 3/10 道题目
    /// </summary>
    void UpdateQuestionIndexLabel()
    {
        uint alreadyAnswerNum = DataManager.Manager<DailyAnswerManager>().AlreadyAnswerNum;
        uint dailyAnswerMax = DataManager.Manager<DailyAnswerManager>().DailyAnswerMax;

        m_label_questNum_label.text = string.Format("第{0}/{1}题", alreadyAnswerNum + 1, dailyAnswerMax);
    }

    void UpdateQuestionAndAnswer()
    {
        DailyAnswerInfo dailyAnswerInfo = DataManager.Manager<DailyAnswerManager>().GetDailyAnswerInfo();

        if (dailyAnswerInfo != null)
        {
            // 题 目
            m_label_question_label.text = dailyAnswerInfo.question;

            //答案

            m_lstGrid.Clear();
            for (int i = 0; i < m_grid_Grid.transform.childCount; i++)
            {
                Transform ts = m_grid_Grid.transform.GetChild(i);
                UIDailyAnswerAnswerGrid grid = ts.gameObject.GetComponent<UIDailyAnswerAnswerGrid>();
                if (grid == null)
                {
                    grid = ts.gameObject.AddComponent<UIDailyAnswerAnswerGrid>();
                }
                if (dailyAnswerInfo.answer != null && i < dailyAnswerInfo.answer.Count)
                {
                    grid.SetGridData(dailyAnswerInfo.answer[i]);

                    grid.SetAnswerDes(dailyAnswerInfo.answer[i].answerDes);

                    grid.RegisterUIEventDelegate(OnGridEventDlg);

                    m_lstGrid.Add(grid);
                }


            }

        }
    }

    private void OnGridEventDlg(UIEventType eventType, object data, object param)
    {
        if (eventType == UIEventType.Click)
        {
            UIDailyAnswerAnswerGrid grid = data as UIDailyAnswerAnswerGrid;
            if (grid != null)
            {
                if (DataManager.Manager<DailyAnswerManager>().CanAnswer)
                {
                    SetGrid(grid);

                    StartCoroutine(WaitForNextQuestion(grid.m_data.index));

                    //只能选一次答案，直到下道题下来，才可以回答
                    DataManager.Manager<DailyAnswerManager>().CanAnswer = false;
                }
            }
        }
    }

    IEnumerator WaitForSetGridRightOrWorng(UIDailyAnswerAnswerGrid grid)
    {
        yield return new WaitForSeconds(1f);

        grid.SetRightOrWrong(grid.m_data.index == 1);

        for (int i = 0; i < m_lstGrid.Count; i++)
        {
            if (grid.m_data.index != 1)
            {

                if (m_lstGrid[i].m_data.index == 1)
                {
                    m_lstGrid[i].SetRightOrWrong(true);
                }
            }
        }

        DataManager.Manager<DailyAnswerManager>().AnswerIsCorrect(grid.m_data.index);
    }



    void SetGrid(UIDailyAnswerAnswerGrid grid)
    {
        //选中
        SetGridSelect(grid);

        //对错
        StartCoroutine(WaitForSetGridRightOrWorng(grid));
    }

    void SetGridSelect(UIDailyAnswerAnswerGrid grid)
    {
        for (int i = 0; i < m_lstGrid.Count; i++)
        {
            //选中
            if (grid.m_data.index == m_lstGrid[i].m_data.index)
            {
                m_lstGrid[i].SetSelect(true);
            }
            else
            {
                m_lstGrid[i].SetSelect(false);
            }
        }
    }



    void ResetGridState()
    {
        for (int i = 0; i < m_lstGrid.Count; i++)
        {
            m_lstGrid[i].SetSelect(false);

            m_lstGrid[i].ReSetRightOrWrong();
        }
    }


    IEnumerator WaitForNextQuestion(uint index)
    {
        yield return new WaitForSeconds(3f);

        ResetGridState();

        DataManager.Manager<DailyAnswerManager>().ReqCommitAnswer(index);
    }

    #endregion

    #region click

    void onClick_BoxOpen_Btn(GameObject caster)
    {
        ShowGiftbagGetPanel();
    }

    void ShowGiftbagGetPanel()
    {
        uint CorrectNum = DataManager.Manager<DailyAnswerManager>().CorrectNum;
        uint DailyAnswerRewardLimit = DataManager.Manager<DailyAnswerManager>().DailyAnswerRewardLimit;
        bool isReceivedReward = DataManager.Manager<DailyAnswerManager>().IsReceivedReward;

        if (false == isReceivedReward)
        {
            List<uint> itemList = GameTableManager.Instance.GetGlobalConfigList<uint>("DailyAnswerRewardItem");

            GiftbagParam param = new GiftbagParam();
            param.firstTitle = "学富五车礼包";
            param.secondTitle = string.Format("答对{0}道题可领取", DailyAnswerRewardLimit);// "答对5道题可领取";

            if (CorrectNum < DailyAnswerRewardLimit)
            {
                param.canGetState = 0;
            }
            else
            {
                param.canGetState = isReceivedReward ? (uint)2 : (uint)1;
            }

            param.giftbagDic = new Dictionary<uint, uint>();
            for (int i = 0; i < itemList.Count; i++)
            {
                param.giftbagDic.Add(itemList[i], 1);
            }

            param.getGiftbagEvent = ReqAnswerReward;

            DataManager.Manager<UIPanelManager>().ShowPanel(PanelID.GiftbagGetPanel, data: param);
        }
        else
        {
            TipsManager.Instance.ShowTips("礼包已经领取");
        }
    }

    void ReqAnswerReward()
    {
        DataManager.Manager<DailyAnswerManager>().ReqAnswerReward();
    }

    #endregion

    bool CheckShowGiftbagGetPanel()
    {
        uint correctNum = DataManager.Manager<DailyAnswerManager>().CorrectNum;
        uint dailyAnswerRewardLimit = DataManager.Manager<DailyAnswerManager>().DailyAnswerRewardLimit;
        bool isReceivedReward = DataManager.Manager<DailyAnswerManager>().IsReceivedReward;
        uint alreadyAnswerNum = DataManager.Manager<DailyAnswerManager>().AlreadyAnswerNum;
        uint dailyAnswerMax = DataManager.Manager<DailyAnswerManager>().DailyAnswerMax;

        if (correctNum >= dailyAnswerRewardLimit && alreadyAnswerNum >= dailyAnswerMax && false == isReceivedReward)
        {
            return true;
            //ShowGiftbagGetPanel();
        }
        else
        {
            return false;
        }
    }

}

