using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameCmd;
using Engine.Utility;

/// <summary>
/// 任务奖励的物品信息
/// </summary>
public class MissionRewardItemInfo
{
    //baseId
    public uint itemBaseId;

    //数量
    public uint num;
}

partial class MissionMessagePanel : UIPanelBase, ITimer
{
    List<UIItemShow> m_lstItem = new List<UIItemShow>();
    string m_strStep;
    uint m_nTaskid;

    int m_nEffectNum = 0;

    //奖励物品
    UIGridCreatorBase m_rewardItemGridCreator;

    //奖励物品信息
    List<MissionRewardItemInfo> m_lstRewardItemInfo;

    /// <summary>
    /// 任务完成按钮倒计时
    /// </summary>
    private const uint MISSIONCANDONE_TIMER = 1000;

    private int missionCanDoneBtnCd = 10;

    int CONST_MissionCanDoneBtnCd = GameTableManager.Instance.GetGlobalConfig<int>("TaskAwardCountdown");


    protected override void OnLoading()
    {
        base.OnLoading();
        InitWidget();
    }

    void InitItems()
    {
        UIItemShow itemShow = null;
        m_trans_itemRoot.parent.GetComponent<UIScrollView>().ResetPosition();
        m_trans_itemRoot.DestroyChildren();
        for (int i = 0; i < 5; i++)
        {
            itemShow = GetItem();
            itemShow.transform.localPosition = new UnityEngine.Vector3(i * 90, 0, 0);
        }
    }

    UIItemShow GetItem()
    {
        UnityEngine.Object obj = UIManager.GetResGameObj(GridID.Uiitemshow);
        if (obj == null)
        {
            return null;
        }
        GameObject go = NGUITools.AddChild(m_trans_itemRoot.gameObject, obj as GameObject);
        UIItemShow itemShow = go.AddComponent<UIItemShow>();
        itemShow.gameObject.SetActive(false);
        m_lstItem.Add(itemShow);
        return itemShow;
    }
    protected override void OnShow(object data)
    {
        base.OnShow(data);

        if (data is LangTalkData)
        {
            LangTalkData currDialogData = (LangTalkData)data;
            m_nTaskid = currDialogData.nTaskId;
            m_strStep = currDialogData.strStep;

            ShowUI();
        }
        else if (data is uint)
        {
            m_nTaskid = (uint)data;

            ShowUI();
        }
    }

    protected override void OnHide()
    {
        base.OnHide();

        if (TimerAxis.Instance().IsExist(MISSIONCANDONE_TIMER, this))
        {
            TimerAxis.Instance().KillTimer(MISSIONCANDONE_TIMER, this);
            m_label_btn_bottom_Label.text = "完成";
        }

        Release();
    }

    protected override void OnPanelBaseDestory()
    {
        base.OnPanelBaseDestory();
    }

    public override void Release(bool depthRelease = true)
    {
        base.Release(depthRelease);

        if (m_rewardItemGridCreator != null)
        {
            m_rewardItemGridCreator.Release(depthRelease);
        }

    }

    public void InitWidget()
    {
        if (m_trans_rewardItemScrollView != null)
        {
            m_rewardItemGridCreator = m_trans_rewardItemScrollView.gameObject.GetComponent<UIGridCreatorBase>();
            if (m_rewardItemGridCreator == null)
            {
                m_rewardItemGridCreator = m_trans_rewardItemScrollView.gameObject.AddComponent<UIGridCreatorBase>();
            }

            UnityEngine.GameObject obj = UIManager.GetResGameObj(GridID.Uiitemshow) as UnityEngine.GameObject;
            m_rewardItemGridCreator.gridContentOffset = new UnityEngine.Vector2(0, 0);
            m_rewardItemGridCreator.arrageMent = UIGridCreatorBase.Arrangement.Horizontal;
            m_rewardItemGridCreator.gridWidth = 90;
            m_rewardItemGridCreator.gridHeight = 120;

            m_rewardItemGridCreator.RefreshCheck();
            m_rewardItemGridCreator.Initialize<UIItemShow>((uint)GridID.Uiitemshow, UIManager.OnObjsCreate, UIManager.OnObjsRelease, OnGridDataUpdate, OnGridUIEvent);
        }
    }

    private void OnGridDataUpdate(UIGridBase gridData, int index)
    {
        if (m_lstRewardItemInfo != null && m_lstRewardItemInfo.Count > index)
        {
            UIItemShow grid = gridData as UIItemShow;
            if (gridData == null)
            {
                return;
            }

            grid.SetGridData(m_lstRewardItemInfo[index]);
        }

    }

    private void OnGridUIEvent(UIEventType eventType, object data, object param)
    {

    }

    //     void ClearItems()
    //     {
    //         foreach (UIItemShow item in m_lstItem)
    //         {
    //             item.Release();
    //         }
    //         m_lstItem.Clear();
    //     }

    void ShowUI()
    {
        table.QuestDataBase quest = GameTableManager.Instance.GetTableItem<table.QuestDataBase>(m_nTaskid);
        if (quest == null)
        {
            return;
        }

        m_widget_demonTask.alpha = (TaskType)quest.dwType == TaskType.TaskType_Demons ? 1f : 0f;
        m_widget_normalTask.alpha = (TaskType)quest.dwType == TaskType.TaskType_Demons ? 0f : 1f;

        m_label_title.text = quest.strName;


        QuestTraceInfo taskInfo = QuestTranceManager.GetInstance().GetQuestTraceInfo(m_nTaskid);

        if (quest.dwType != (uint)GameCmd.TaskType.TaskType_Demons)
        {
            ShowNormalTaskUI(quest, taskInfo);
        }
        else
        {
            ShowDemonsTaskUI(quest, taskInfo);
        }
    }

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

    void ShowDemonsTaskUI(table.QuestDataBase quest, QuestTraceInfo taskInfo)
    {
        if (quest == null)
        {
            return;
        }

        SetTaskDesc(ref quest);

        SetTargetLabel(taskInfo, ref quest, ref m_label_demontargetLabel);

        SetDemonsStar();

        if (taskInfo != null)
        {
            SetBottomBtnLable(taskInfo);
            if (taskInfo.exp == 0)
            {
                //请求任务奖励
                NetService.Instance.Send(new GameCmd.stRequestTaskRewardScriptUserCmd_C() { task_id = m_nTaskid });
            }
            else
            {
                m_label_labledemonexp.text = taskInfo.exp.ToString();
            }
        }
        else
        {
            NetService.Instance.Send(new GameCmd.stRequestTaskRewardScriptUserCmd_C() { task_id = m_nTaskid });
            m_label_btn_bottom_Label.text = "接取";
        }
    }

    IEnumerator ShowEffect(int start, int add)
    {
        m_sprite_star.fillAmount = start * 1f / 5;
        yield return null;

        while (add > 0)
        {
            //Transform effect = CreateEffect(1);
            //effect.parent = m_sprite_star.transform;
            //effect.localPosition = new UnityEngine.Vector3(m_sprite_star.width / 5f * (start + 1), 0, 0);
            //effect.gameObject.SetActive(true);

            PlayStarEffect(new UnityEngine.Vector3(m_sprite_star.width / 5f * (start + 1), 0, 0));
            yield return new WaitForSeconds(0.2f);
            start++;
            m_sprite_star.fillAmount = start / 5f;
            add--;
        }
    }

    private void SetDemonsStar(bool showEffect = false)
    {
        StarTaskData stardata = DataManager.Manager<TaskDataManager>().GetStarTask(m_nTaskid);
        if (stardata != null)
        {
            if (showEffect)
            {
                int start = (int)(m_sprite_star.fillAmount * 5);
                int addstartNum = (int)stardata.star - start;
                StartCoroutine(ShowEffect(start, addstartNum));
            }
            else
            {
                CreateEffect(5 - (int)stardata.star);

                m_sprite_star.fillAmount = stardata.star * 1f / 5;
            }

            int money = 0;//金币
            int coin = 0; //元宝
            int cold = 0; //文钱

            //const int moneyRefresTime = 3;
            GetStarTaskCost(stardata.star, ref money, ref coin, ref cold);

            Transform t = m_label_lablerefreshmoney.transform.Find("Sprite");
            if (t != null)
            {
                UISpriteEx se = t.GetComponent<UISpriteEx>();
                if (se != null)
                {
                    //se.ChangeSprite(stardata.all_refresh >= moneyRefresTime ? 2 : 1);
                    se.ChangeSprite(1);
                }
            }

            //m_label_lablerefreshmoney.text = stardata.all_refresh >= moneyRefresTime ? coin.ToString() : money.ToString();
            //刷新超过三次才要钱
            //if (stardata.all_refresh >= GameTableManager.Instance.GetGlobalConfig<uint>("StarGoldRefresh"))
            //{
            //    m_label_lablerefreshmoney.gameObject.SetActive(true);
            //    m_label_lablerefreshmoney.text = money.ToString();
            //}
            //else 
            //{
            //    m_label_lablerefreshmoney.gameObject.SetActive(false);
            //}
            int leftgold = MainPlayerHelper.GetMoneyNumByType(ClientMoneyType.Gold);
            m_label_remainmoney.gameObject.SetActive(true);
            m_label_remainmoney.text = leftgold.ToString();
            m_label_lablerefreshmoney.text = money.ToString();
            m_label_lableRefreshCoin.text = cold.ToString();

            t = m_btn_btn_refresh.transform.Find("Label");
            if (t != null)
            {
                UILabel lable = t.GetComponent<UILabel>();
                if (lable != null)
                {
                    lable.text = "刷新";
                    //if (stardata.all_refresh >= 3)
                    //{
                    //    lable.text = "刷新";
                    //}
                    //else
                    //{
                    //    lable.text = string.Format("刷新({0})", moneyRefresTime - stardata.all_refresh);
                    //}
                }
            }
        }
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
    /// 星星特效
    /// </summary>
    /// <param name="num"></param>
    /// <param name="pos"></param>
    void PlayStarEffect(Vector3 pos)
    {
        m_sprite_star.gameObject.SetActive(true);
        //特效
        UIParticleWidget wight = m_sprite_star.GetComponent<UIParticleWidget>();
        if (null == wight)
        {
            wight = m_sprite_star.gameObject.AddComponent<UIParticleWidget>();
            wight.depth = 100;
        }

        if (wight != null)
        {
            wight.ReleaseParticle();
            starEffectPos = pos;
            wight.AddParticle(50007, StarEffectCallBack);
        }
    }

    Vector3 starEffectPos = Vector3.zero;
    void StarEffectCallBack(Engine.IEffect effect) 
    {
       Transform tf = effect.GetNodeTransForm();
        if (tf == null)
        {
            return;
        }

        tf.localPosition = starEffectPos;
    }

    void ShowNormalTaskUI(table.QuestDataBase quest, QuestTraceInfo taskInfo)
    {
        if (quest == null)
        {
            return;
        }
        SetTaskDesc(ref quest);


        SetTargetLabel(taskInfo, ref quest, ref m_label_normaltargetLabel);


        if (taskInfo != null)
        {
            SetBottomBtnLable(taskInfo);

            if (taskInfo.exp == 0)
            {
                //请求任务奖励
                NetService.Instance.Send(new GameCmd.stRequestTaskRewardScriptUserCmd_C() { task_id = m_nTaskid });
            }
            else
            {
                m_lstRewardItemInfo = GetRewardInfoList(taskInfo.exp, taskInfo.money, taskInfo.gold, taskInfo.Items, taskInfo.ItemNum);
                m_rewardItemGridCreator.CreateGrids(m_lstRewardItemInfo != null ? m_lstRewardItemInfo.Count : 0);
                //SetNormalReward(taskInfo.exp,taskInfo.money,taskInfo.gold,taskInfo.Items,taskInfo.ItemNum);
                return;
            }

            if (taskInfo.state != 0)
            {
                m_trans_DemonFresh.gameObject.SetActive(taskInfo.state != taskInfo.operate);
            }
            else
            {
                m_trans_DemonFresh.gameObject.SetActive(true);
            }
        }
        else
        {
            NetService.Instance.Send(new GameCmd.stRequestTaskRewardScriptUserCmd_C() { task_id = m_nTaskid });
            m_label_btn_bottom_Label.text = "接取";
            m_trans_DemonFresh.gameObject.SetActive(true);
        }
    }

    List<MissionRewardItemInfo> GetRewardInfoList(uint exp, uint money, uint gold, List<uint> itemIds, List<uint> itemNums)
    {
        List<MissionRewardItemInfo> rewardItemInfoList = new List<MissionRewardItemInfo>();

        //经验
        if (exp > 0)
        {
            MissionRewardItemInfo item = new MissionRewardItemInfo() { itemBaseId = MainPlayerHelper.ExpID, num = exp };
            rewardItemInfoList.Add(item);
        }

        //文钱
        if (money > 0)
        {
            MissionRewardItemInfo item = new MissionRewardItemInfo() { itemBaseId = MainPlayerHelper.MoneyTicketID, num = money };
            rewardItemInfoList.Add(item);
        }

        //金币
        if (gold > 0)
        {
            MissionRewardItemInfo item = new MissionRewardItemInfo() { itemBaseId = MainPlayerHelper.GoldID, num = gold };
            rewardItemInfoList.Add(item);
        }

        //其他奖励
        for (int i = 0; i < itemIds.Count; i++)
        {
            if (i < itemNums.Count)
            {
                MissionRewardItemInfo item = new MissionRewardItemInfo() { itemBaseId = itemIds[i], num = itemNums[i] };
                rewardItemInfoList.Add(item);
            }
        }

        return rewardItemInfoList;
    }
    /*
    /// <summary>
    /// 
    /// </summary>
    /// <param name="exp"></param>
    /// <param name="money">文钱</param>
    /// <param name="gold">金币</param>
    /// <param name="itemIds"></param>
    /// <param name="itemNums"></param>
    private void SetNormalReward(uint exp,uint money,uint gold,List<uint> itemIds,List<uint> itemNums)
    {

        UIItemShow itemShow = null;
        int index = 0;
        if (exp > 0)
        {
            itemShow = index >= m_lstItem.Count ? GetItem() : m_lstItem[index];
            itemShow.gameObject.SetActive(true);
            itemShow.ShowWithItemIdNum(MainPlayerHelper.ExpID, exp);
            itemShow.transform.localPosition = new UnityEngine.Vector3(index * 90, 0, 0);
            index++;
        }
        if (money > 0)//文钱
        {
            itemShow = index >= m_lstItem.Count ? GetItem() : m_lstItem[index];
            itemShow.gameObject.SetActive(true);
            itemShow.ShowWithItemIdNum(MainPlayerHelper.MoneyTicketID, money);
            itemShow.transform.localPosition = new UnityEngine.Vector3(index * 90, 0, 0);
            index++;
        }
        if (gold > 0)//金币
        {
            itemShow = index >= m_lstItem.Count ? GetItem() : m_lstItem[index];
            itemShow.gameObject.SetActive(true);
            itemShow.ShowWithItemIdNum(MainPlayerHelper.GoldID, gold);
            itemShow.transform.localPosition = new UnityEngine.Vector3(index * 90, 0, 0);
            index++;
        }

        for (int i = 0; i < itemIds.Count && i < itemNums.Count; ++i)
        {
            uint itembaseid = itemIds[i];
            uint itemNum = itemNums[i];

            itemShow = index >= m_lstItem.Count ? GetItem() : m_lstItem[index];
            itemShow.gameObject.SetActive(true);

            itemShow.ShowWithItemIdNum(itembaseid, itemNum);
            itemShow.transform.localPosition = new UnityEngine.Vector3(index * 90, 0, 0);
            index++;
        }

        for (int i = index; i < m_lstItem.Count; i++)
        {
            m_lstItem[i].gameObject.SetActive(false);
        }
        m_trans_itemRoot.parent.GetComponent<UIScrollView>().ResetPosition();
    }
    */
    private void SetTargetLabel(QuestTraceInfo taskInfo, ref table.QuestDataBase quest, ref UILabel targetLable)
    {
        if (taskInfo != null)
        {
            if (taskInfo.taskSubType == TaskSubType.Collection ||
            taskInfo.taskSubType == TaskSubType.KillMonster ||
            taskInfo.taskSubType == TaskSubType.KillMonsterCollect ||
            taskInfo.taskSubType == TaskSubType.DeliverItem)
            {
                GameCmd.TaskProcess process = taskInfo.GetTaskProcess();
                if (process == TaskProcess.TaskProcess_CanDone)
                {
                    targetLable.text = string.Format("{0}[00ff00]({1}/{2})", quest.dwStrTarget, taskInfo.operate, taskInfo.state);
                }
                else if (process == TaskProcess.TaskProcess_Doing)
                {
                    targetLable.text = string.Format("{0}[ff0000]({1}/{2})", quest.dwStrTarget, taskInfo.operate, taskInfo.state);
                }
                else
                {
                    targetLable.text = quest.dwStrTarget;
                }
            }
            else
            {
                targetLable.text = quest.dwStrTarget;
            }
        }
        else
        {
            targetLable.text = quest.dwStrTarget;
        }
    }
    private void SetTaskDesc(ref table.QuestDataBase quest)
    {
        m_label_RichText.text = quest.dwDesc;
    }

    private void SetBottomBtnLable(QuestTraceInfo taskInfo)
    {
        TaskProcess process = taskInfo.GetTaskProcess();
        if (process == TaskProcess.TaskProcess_None)
        {
            m_label_btn_bottom_Label.text = "接取";
        }
        else if (process == TaskProcess.TaskProcess_Doing)
        {
            m_label_btn_bottom_Label.text = "放弃";
        }
        else if (process == TaskProcess.TaskProcess_CanDone)
        {
            //开启自动完成
            if (taskInfo.QuestTable.dwAutoCanDone)
            {
                TimerAxis.Instance().KillTimer(MISSIONCANDONE_TIMER, this);
                TimerAxis.Instance().SetTimer(MISSIONCANDONE_TIMER, 1000, this);

                missionCanDoneBtnCd = CONST_MissionCanDoneBtnCd;
                string btnDes = string.Format("完成({0})", missionCanDoneBtnCd);
                m_label_btn_bottom_Label.text = btnDes;
            }
            else
            {
                if (TimerAxis.Instance().IsExist(MISSIONCANDONE_TIMER, this))
                {
                    TimerAxis.Instance().KillTimer(MISSIONCANDONE_TIMER, this);
                }
                m_label_btn_bottom_Label.text = "完成";
            }
            UIParticleWidget p = m_btn_btn_bottom.GetComponent<UIParticleWidget>();
            if (p == null)
            {
                p = m_btn_btn_bottom.gameObject.AddComponent<UIParticleWidget>();
                p.depth = 20;
            }
            if (p != null)
            {
                p.SetDimensions(260, 54);
                p.ReleaseParticle();
                p.AddRoundParticle();
            }          
        }
    }

    void GetStarTaskCost(uint star, ref int money, ref int coin, ref int cold)
    {
        table.StarQuestDataBase db = GameTableManager.Instance.GetTableItem<table.StarQuestDataBase>(star);
        if (db != null)
        {
            int level = MainPlayerHelper.GetPlayerLevel();
            money = Mathf.CeilToInt((db.money * level * float.Parse(db.strCostRate))/10000);
            coin = Mathf.CeilToInt((db.coin * level * float.Parse(db.strCostRate))/10000);
            cold = Mathf.CeilToInt((db.fiveStarCost * level * float.Parse(db.strCostRate))/10000);
        }
    }


    void onClick_Close_Btn(GameObject caster)
    {
       

        if (TimerAxis.Instance().IsExist(MISSIONCANDONE_TIMER, this))
        {
            TimerAxis.Instance().KillTimer(MISSIONCANDONE_TIMER, this);
            m_label_btn_bottom_Label.text = "完成";
        }

        this.HideSelf();
    }



    void onClick_Btn_bottom_Btn(GameObject caster)
    {
        this.HideSelf();
        QuestTraceInfo taskInfo = QuestTranceManager.GetInstance().GetQuestTraceInfo(m_nTaskid);
        if (taskInfo == null)
        {
            Engine.Utility.Log.Error(" No Found taskindo id {0}", m_nTaskid);
            return;
        }

        if (taskInfo.Received)
        {
            TaskProcess process = taskInfo.GetTaskProcess();
            if (taskInfo.taskSubType == TaskSubType.Guild)
            {
                if (process == TaskProcess.TaskProcess_CanDone)
                {
                    if (CanPutInKanpsack())
                    {
                        Protocol.Instance.RequestFinishTask(m_nTaskid);
                    }
                }
            }
            else
            {
                if (process == TaskProcess.TaskProcess_Doing)
                {
                    Protocol.Instance.RequestDelTask(m_nTaskid);
                }
                else if (process == TaskProcess.TaskProcess_CanDone)
                {
                    if (CanPutInKanpsack())
                    {
                        Protocol.Instance.RequestDialogSelect(1, m_strStep);
                    }
                }
            }
        }
        else
        {
            Protocol.Instance.RequestDialogSelect(1, m_strStep);
        }
    }

    /// <summary>
    /// 检查背包空间是否不足
    /// </summary>
    /// <returns></returns>
    bool CanPutInKanpsack()
    {
        if (this.m_lstRewardItemInfo == null)
        {
            return true;
        }

        for (int i = 0; i < this.m_lstRewardItemInfo.Count; i++)
        {
            if (m_lstRewardItemInfo[i].itemBaseId == MainPlayerHelper.ExpID || m_lstRewardItemInfo[i].itemBaseId == MainPlayerHelper.CoinID ||
            MainPlayerHelper.GoldID == m_lstRewardItemInfo[i].itemBaseId)
            {
                continue;
            }

            bool b = DataManager.Manager<KnapsackManager>().CanPutInKanpsack(m_lstRewardItemInfo[i].itemBaseId, m_lstRewardItemInfo[i].num);
            if (b)
            {
                continue;
            }
            else
            {
                //背包空间不足，不可提交
                TipsManager.Instance.ShowTips(LocalTextType.Task_Commond_2);
                return false;
            }
        }
        return true;
    }

    void onClick_Btn_refresh_Btn(GameObject caster)
    {
        if (m_nTaskid != 0)
        {
            StarTaskData stardata = DataManager.Manager<TaskDataManager>().GetStarTask(m_nTaskid);
            if (stardata != null)
            {
                //if (stardata.star >= 5)
                //{
                //    TipsManager.Instance.ShowTips("任务已是最高星级");
                //    return;
                //}
                NetService.Instance.Send(
                new stRefreshStarScriptUserCmd_C()
                {
                    id = m_nTaskid,
                    gold_or_money = true,  //改为消耗金币
                    //gold_or_money = stardata.all_refresh < 3,
                });
            }
        }
    }

    void onClick_Btn_FiveStar_Btn(GameObject caster)
    {
        if (m_nTaskid != 0)
        {
            //StarTaskData stardata = DataManager.Manager<TaskDataManager>().GetStarTask(m_nTaskid);
            //if (stardata != null)
            //{
            //    if (stardata.star >= 5)
            //    {
            //        TipsManager.Instance.ShowTips("任务已是最高星级");
            //        return;
            //    }
            //}
            NetService.Instance.Send(new stMaxStarScriptUserCmd_C() { id = m_nTaskid });
        }
    }

    public override bool OnMsg(UIMsgID msgid, object param)
    {
        switch (msgid)
        {
            case UIMsgID.eRefreshTaskDesc:
                GameCmd.stTaskRewardScriptUserCmd_S cmd = (GameCmd.stTaskRewardScriptUserCmd_S)param;
                if (m_nTaskid == cmd.task_id)
                {
                    if (m_widget_normalTask.alpha == 1f)
                    {
                        m_lstRewardItemInfo = GetRewardInfoList(cmd.exp, cmd.money, cmd.gold, cmd.item_base_id, cmd.item_num);
                        m_rewardItemGridCreator.CreateGrids(m_lstRewardItemInfo != null ? m_lstRewardItemInfo.Count : 0);
                        //SetNormalReward(cmd.exp, cmd.money,cmd.gold, cmd.item_base_id, cmd.item_num);
                    }
                    else if (m_widget_demonTask.alpha == 1f)
                    {
                        m_label_labledemonexp.text = cmd.exp.ToString();
                    }
                }
                break;
            case UIMsgID.eRefreshStarTask:
                uint taskid = (uint)param;
                if (m_nTaskid == taskid)
                {
                    if (m_widget_demonTask.alpha == 1f)
                    {
                        SetDemonsStar(true);
                        //SetStarNum(taskid);
                    }
                }
                break;
            default:
                break;
        }
        return true;
    }

    void CanDoneBtn()
    {
        QuestTraceInfo taskInfo = QuestTranceManager.GetInstance().GetQuestTraceInfo(m_nTaskid);
        if (taskInfo == null)
        {
            Engine.Utility.Log.Error(" No Found taskindo id {0}", m_nTaskid);
            return;
        }

        TaskProcess process = taskInfo.GetTaskProcess();

        if (process == TaskProcess.TaskProcess_CanDone)
        {
            onClick_Btn_bottom_Btn(null);
        }
    }


    public void OnTimer(uint uTimerID)
    {
        if (uTimerID == MISSIONCANDONE_TIMER)
        {
            missionCanDoneBtnCd--;
            if (missionCanDoneBtnCd > 0)
            {
                m_label_btn_bottom_Label.text = string.Format("完成({0})", missionCanDoneBtnCd);
            }
            else
            {
                if (TimerAxis.Instance().IsExist(MISSIONCANDONE_TIMER, this))
                {
                    TimerAxis.Instance().KillTimer(MISSIONCANDONE_TIMER, this);
                    m_label_btn_bottom_Label.text = "完成";

                    CanDoneBtn();
                }
            }

        }
    }
}
