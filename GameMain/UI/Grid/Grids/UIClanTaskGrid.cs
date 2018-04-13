/************************************************************************************
 * Copyright (c) 2016  All Rights Reserved.
 * CLR版本： 4.0.30319.42000
 * 公司名称：ZQB
 * 命名空间：GameMain.UI.Grid.Grids
 * 创建人：  wenjunhua.zqgame
 * 文件名：  UIClanTaskGrid
 * 版本号：  V1.0.0.0
 * 创建时间：10/24/2016 4:23:42 PM
 * 描述：
 ************************************************************************************/

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

class UIClanTaskGrid : UIGridBase
{
    #region property
    //描述
    private UIXmlRichText m_lab_des;
    //可完成次数
    //private UILabel m_lab_times;
    //接受
    private GameObject m_obj_accept;
    //进行中
    private GameObject m_obj_inProgress;
    //提交
    private GameObject m_obj_submit;
    private UISprite m_sprite_star;

    //进度
    private UILabel m_lblCount;

    //进度条
    private UISlider m_slider;

    //奖励倍数
    private UILabel m_rewardMultiple;

    //数据
    private ClanManger.ClanQuestInfo questInfo;
    public ClanManger.ClanQuestInfo QuestInfo
    {
        get
        {
            return questInfo;
        }
    }
    UILabel m_typeLabel;
    //奖励
    //private UIGrid m_reward = null;
    private UILabel m_lab_reward = null;
    #endregion

    #region override

    protected override void OnAwake()
    {
        base.OnAwake();
        m_lab_des = CacheTransform.Find("Content/Des").GetComponent<UIXmlRichText>();
        //m_lab_times = CacheTransform.Find("Content/Times").GetComponent<UILabel>();
        //m_reward = CacheTransform.Find("Content/RewardContent/Reward").GetComponent<UIGrid>();
        m_lab_reward = CacheTransform.Find("Content/RewardContent/RewardTxt").GetComponent<UILabel>();
        m_obj_accept = CacheTransform.Find("Content/Btns/BtnAccept").gameObject;
        m_typeLabel = CacheTransform.Find("Content/IconContent/IconBg/Label").GetComponent<UILabel>();
        m_sprite_star = CacheTransform.Find("Content/star").GetComponent<UISprite>();

        m_lblCount = CacheTransform.Find("Content/Times").GetComponent<UILabel>();

        m_slider = CacheTransform.Find("Content/Times/Expslider").GetComponent<UISlider>();

        m_rewardMultiple = CacheTransform.Find("Content/IconContent/mask/Label").GetComponent<UILabel>();

        UIEventListener.Get(m_obj_accept).onClick = (obj) =>
        {
            //DataManager.Manager<ClanManger>().AcceptClanTask(questInfo.ID);
            AcceptClanTask();
        };
        m_obj_inProgress = CacheTransform.Find("Content/Btns/BtnDoing").gameObject;
        UIEventListener.Get(m_obj_inProgress).onClick = (obj) =>
        {
            //AutoMoveToTarget();
            DoClanTask();
        };
        m_obj_submit = CacheTransform.Find("Content/Btns/BtnSubmit").gameObject;
        UIEventListener.Get(m_obj_submit).onClick = (obj) =>
        {
            //DataManager.Manager<ClanManger>().FinishClanTask(questInfo.ID);
            FinishClanTask();
        };
    }

    public override void SetGridData(object data)
    {
        base.SetGridData(data);
        if (null == data || !(data is ClanManger.ClanQuestInfo))
        {
            Engine.Utility.Log.Error("UIClanTaskGrid->SetGridData null");
            return;
        }
        this.questInfo = data as ClanManger.ClanQuestInfo;
        if (null != m_lab_des)
        {
            //             m_lab_des.Clear();
            //             m_lab_des.fontSize = 24;
            //             m_lab_des.AddXml(RichXmlHelper.RichXmlAdapt(questInfo.Des));
            //            Debug.Log(questInfo.Des);
        }
        if (null != m_typeLabel)
        {
            m_typeLabel.text = this.questInfo.TaskName;
            //if (questInfo.taskChildType == GameCmd.TaskChildType.TaskChildType_Collection)
            //{
            //    m_typeLabel.text = "采集物品";
            //}
            //else if (questInfo.taskChildType == GameCmd.TaskChildType.TaskChildType_SubmitItem)
            //{
            //    m_typeLabel.text = "上交物品";
            //}
            //else if (questInfo.taskChildType == GameCmd.TaskChildType.TaskChildType_KillMaster)
            //{
            //    m_typeLabel.text = "击杀怪物";
            //}
        }
        bool cansee = false;
        cansee = (questInfo.TaskStatus == GameCmd.TaskProcess.TaskProcess_None);
        /*if (null != m_lab_times)
        {           
            if (m_lab_times.gameObject.activeSelf != cansee)
            {
                m_lab_times.gameObject.SetActive(cansee);
            }
            if (cansee)
            {
                ColorType c = ColorType.Green;
                if (questInfo.LeftTimes == 0)
                {
                    c = ColorType.Red;
                }
                m_lab_times.text = DataManager.Manager<TextManager>().GetLocalFormatText(
                    LocalTextType.Local_TXT_TodayCompleteLimit, ColorManager.GetColorString(c, questInfo.LeftTimes.ToString()));
            }
            if (m_sprite_star != null)
            {
                CreateEffect();
                m_sprite_star.fillAmount = (5 - questInfo.LeftTimes) * 1f / 5;
            }
          
        }*/

        if (null != m_obj_accept)
        {
            cansee = (questInfo.TaskStatus == GameCmd.TaskProcess.TaskProcess_None);
            m_obj_accept.SetActive(cansee);
            if (cansee)
            {
                m_obj_accept.GetComponent<UIButton>().isEnabled = (questInfo.LeftTimes > 0);
            }
        }
        if (null != m_obj_inProgress)
        {
            cansee = (questInfo.TaskStatus == GameCmd.TaskProcess.TaskProcess_Doing);
            m_obj_inProgress.SetActive(cansee);
        }
        if (null != m_obj_submit)
        {
            cansee = (questInfo.TaskStatus == GameCmd.TaskProcess.TaskProcess_CanDone);
            m_obj_submit.SetActive(cansee);
        }
        if (null != m_lab_reward)
        {
            string rewardformat = "{0}:+{1}";
            uint num = 0;
            string rewardName = "";
            StringBuilder builder = new StringBuilder();
            //文钱
            if (questInfo.AwardMoney > 0)
            {
                rewardName = "绑元";
                num = questInfo.AwardMoney;
                builder.Append(string.Format(rewardformat, rewardName, num * this.questInfo.RewardMultiple));
            }
            //金币
            if (questInfo.AwardGold > 0)
            {
                if (!string.IsNullOrEmpty(builder.ToString()))
                    builder.Append("   ");
                rewardName = "金币";
                num = questInfo.AwardGold;
                builder.Append(string.Format(rewardformat, rewardName, num * this.questInfo.RewardMultiple));
            }
            //声望
            if (questInfo.AwardRep > 0)
            {
                if (!string.IsNullOrEmpty(builder.ToString()))
                    builder.Append("   ");
                rewardName = "声望";
                num = questInfo.AwardRep;
                builder.Append(string.Format(rewardformat, rewardName, num * this.questInfo.RewardMultiple));
            }
            //族贡
            if (questInfo.AwardZG > 0)
            {
                if (!string.IsNullOrEmpty(builder.ToString()))
                    builder.Append("   ");
                rewardName = "建设度";
                num = questInfo.AwardZG;
                builder.Append(string.Format(rewardformat, rewardName, num * this.questInfo.RewardMultiple));
            }
            //资金
            if (questInfo.AwardZJ > 0)
            {
                if (!string.IsNullOrEmpty(builder.ToString()))
                    builder.Append("   ");
                rewardName = "资金";
                num = questInfo.AwardZJ;
                builder.Append(string.Format(rewardformat, rewardName, num * this.questInfo.RewardMultiple));
            }

            //经验
            if (questInfo.AwardExp > 0)
            {
                if (!string.IsNullOrEmpty(builder.ToString()))
                    builder.Append("   ");
                rewardName = "经验";
                num = questInfo.AwardExp;
                builder.Append(string.Format(rewardformat, rewardName, num * this.questInfo.RewardMultiple));
            }

            //神魔经验
            if (questInfo.AwardDevExp > 0)
            {
                if (!string.IsNullOrEmpty(builder.ToString()))
                    builder.Append("   ");
                rewardName = "神魔经验";
                num = questInfo.AwardDevExp;
                builder.Append(string.Format(rewardformat, rewardName, num * this.questInfo.RewardMultiple));
            }

            //物品
            if (null != questInfo && null != questInfo.AwardItems
                && questInfo.AwardItems.Count > 0)
            {
                BaseItem baseItem = null;
                for (int i = 0; i < questInfo.AwardItems.Count; i++)
                {
                    if (!string.IsNullOrEmpty(builder.ToString()))
                        builder.Append("   ");
                    baseItem = DataManager.Manager<ItemManager>().GetTempBaseItemByBaseID<BaseItem>(questInfo.AwardItems[i]);
                    num = (null != questInfo.AwardNums && questInfo.AwardNums.Count > i) ? questInfo.AwardNums[i] : 0;
                    rewardName = baseItem.LocalName;
                    builder.Append(string.Format(rewardformat, rewardName, num * this.questInfo.RewardMultiple));
                }
            }
            m_lab_reward.text = builder.ToString();
        }

        string countStr = string.Format("{0}/{1}", this.questInfo.FinishNum, this.questInfo.ClanTaskStepNum);  //this.questInfo
        SetCount(countStr);

        float slider = this.questInfo.FinishNum / (float)this.questInfo.ClanTaskStepNum;
        SetSlider(slider);

        SetRewardMultiple(this.questInfo.RewardMultiple);

        m_sprite_star.fillAmount = questInfo.Star * 1f / 5;    //改为难度

        SetBtn();
    }

    public override void OnGridClicked()
    {
        base.OnGridClicked();
        //AutoMoveToTarget();
    }

    /// <summary>
    /// 计数
    /// </summary>
    /// <param name="countStr"></param>
    void SetCount(string countStr)
    {
        if (m_lblCount != null)
        {
            m_lblCount.text = countStr;
        }
    }

    /// <summary>
    /// 进度条
    /// </summary>
    /// <param name="value"></param>
    void SetSlider(float value)
    {
        if (m_slider != null)
        {
            m_slider.value = value;
        }
    }

    /// <summary>
    /// 奖励倍数
    /// </summary>
    void SetRewardMultiple(uint num)
    {
        if (m_rewardMultiple != null)
        {
            m_rewardMultiple.text = string.Format("{0}倍", num);
        }
    }

    void SetBtn()
    {
        uint maxTimes = DataManager.Manager<ClanManger>().ClanTaskMaxTimes;
        uint todayFinishTimes = DataManager.Manager<ClanManger>().TodayFinishTimes;

        UIButton btn = m_obj_accept.GetComponent<UIButton>();
        if (btn != null)
        {
            btn.isEnabled = todayFinishTimes < maxTimes;
        }
    }

    private void AppendRewardTxt(string name, int num)
    {

    }
    /// <summary>
    /// 自动寻路
    /// </summary>
    public void AutoMoveToTarget()
    {
        if (null != questInfo
            && questInfo.TaskStatus == GameCmd.TaskProcess.TaskProcess_Doing)
        {

            Engine.Utility.EventEngine.Instance().DispatchEvent((int)Client.GameEventID.TASK_DONING,
                    new Client.stDoingTask() { taskid = questInfo.ID });
            //Engine.Utility.Log.Info(" url :" + m_lab_des.UrlStr);
        }
    }
    #endregion


    Transform CreateEffect(int num = 1)
    {
        Engine.IEffect m_effect = null;
        for (int i = 0; i < num; i++)
        {
            Engine.IRenderSystem rs = Engine.RareEngine.Instance().GetRenderSystem();
            if (rs != null)
            {
                table.ResourceDataBase rd = GameTableManager.Instance.GetTableItem<table.ResourceDataBase>(50007);
                if (rd == null)
                {
                    return null;
                }

                string path = rd.strPath;

                bool success = rs.CreateEffect(ref path, ref m_effect, OnCreateEffectEvent, Engine.TaskPriority.TaskPriority_Immediate);
                if (success)
                {
                    m_effect.GetNode().SetScale(Vector3.one);
                    m_effect.GetNode().GetTransForm().SetChildLayer(LayerMask.NameToLayer("UI"));
                }
            }
        }

        if (m_effect != null)
        {
            return m_effect.GetNodeTransForm();
        }

        return null;
    }
    void OnCreateEffectEvent(Engine.IEffect effect)
    {
        if (effect == null)
        {
            return;
        }
        effect.GetNode().GetTransForm().SetChildLayer(LayerMask.NameToLayer("UI"));

        ParticleSystem[] particle = effect.GetNode().GetTransForm().GetComponentsInChildren<ParticleSystem>();
        foreach (var item in particle)
        {
            Renderer render = item.GetComponent<Renderer>();
            if (render != null)
            {
                render.material.renderQueue = 3320;
            }
        }
    }

    /// <summary>
    /// 接取任务
    /// </summary>
    void AcceptClanTask()
    {
        DataManager.Manager<ClanManger>().AcceptClanTask(questInfo.ID);
    }

    /// <summary>
    /// 做任务
    /// </summary>
    void DoClanTask()
    {
        AutoMoveToTarget();

        //DataManager.Manager<UIPanelManager>().HidePanel(PanelID.ClanTaskPanel);
        //DataManager.Manager<UIPanelManager>().HidePanel(PanelID.DailyPanel);
    }

    void FinishClanTask()
    {
        DataManager.Manager<ClanManger>().FinishClanTask(questInfo.ID);
    }


    #region Get
    //private UIRewardGrid GetRewardGrid()
    //{
    //    if (null == m_reward)
    //    {
    //        return null;
    //    }
    //    GameObject obj = UIManager.GetResGameObj(GridID.Uirewardgrid) as GameObject;
    //    UIRewardGrid rewardGrid = null;
    //    GameObject clone = null;
    //    clone = NGUITools.AddChild(m_reward.gameObject, obj);
    //    if (null != clone)
    //    {
    //        rewardGrid = clone.GetComponent<UIRewardGrid>();
    //        if (null == rewardGrid)
    //        {
    //            rewardGrid = clone.AddComponent<UIRewardGrid>();
    //        }
    //        m_reward.AddChild(rewardGrid.CacheTransform);
    //    }
    //    return rewardGrid;
    //}
    #endregion
}