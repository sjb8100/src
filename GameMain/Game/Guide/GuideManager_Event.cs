/************************************************************************************
 * Copyright (c) 2017  All Rights Reserved.
 * CLR版本： 4.0.30319.42000
 * 公司名称：ZQB
 * 命名空间：GameMain.Game.Guide
 * 创建人：  wenjunhua.zqgame
 * 文件名：  GuideManager_Event
 * 版本号：  V1.0.0.0
 * 创建时间：4/1/2017 3:00:57 PM
 * 描述：
 ************************************************************************************/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Client;
partial class GuideManager
{
    #region define
    public class CheckWorkFlowData
    {
        public int CheckTypeMask = 0;
        public object Param = null;
    }
    #endregion

    #region IGlobalEvent
    public void RegisterGlobalEvent(bool register)
    {
        if (register)
        {
            //数据ready
            Engine.Utility.EventEngine.Instance().AddEventListener((int)Client.GameEventID.SYSTEM_GAME_READY, GlobalEventHandler);

            //等级上升
            Engine.Utility.EventEngine.Instance().AddEventListener((int)Client.GameEventID.ENTITYSYSTEM_LEVELUP, GlobalEventHandler);
            //Engine.Utility.EventEngine.Instance().AddEventListener((int)Client.GameEventID.PLAYER_LOGIN_SUCCESS, GlobalEventHandler);
            Engine.Utility.EventEngine.Instance().AddEventListener((int)Client.GameEventID.TITLE_NEWTITLE, GlobalEventHandler);
            Engine.Utility.EventEngine.Instance().AddEventListener((int)Client.GameEventID.MAINPANEL_SHOWREDWARING, GlobalEventHandler);
            //七天福利状态
            Engine.Utility.EventEngine.Instance().AddEventListener((int)Client.GameEventID.SEVENDAYOPENSTATUS, GlobalEventHandler);
            //神兵领取状态
            Engine.Utility.EventEngine.Instance().AddEventListener((int)Client.GameEventID.GODWEAPENSTATUS, GlobalEventHandler);
            //开服豪礼状态
            Engine.Utility.EventEngine.Instance().AddEventListener((int)Client.GameEventID.OPENSERVERGIFTSTATUS, GlobalEventHandler);
            //首充相关
            Engine.Utility.EventEngine.Instance().AddEventListener((int)Client.GameEventID.FIRSTRECHARGESTATUS, GlobalEventHandler);
            //内测返利领取状态
            Engine.Utility.EventEngine.Instance().AddEventListener((int)Client.GameEventID.RETREWARDSTATUS, GlobalEventHandler);
            //问卷状态
            Engine.Utility.EventEngine.Instance().AddEventListener((int)Client.GameEventID.QUESTIONSTATUS, GlobalEventHandler);
            //任务相关
            Engine.Utility.EventEngine.Instance().AddEventListener((int)Client.GameEventID.TASK_ACCEPT, GlobalEventHandler);
            Engine.Utility.EventEngine.Instance().AddEventListener((int)Client.GameEventID.TASK_DONING, GlobalEventHandler);
            Engine.Utility.EventEngine.Instance().AddEventListener((int)Client.GameEventID.TASK_CANSUBMIT, GlobalEventHandler);
            Engine.Utility.EventEngine.Instance().AddEventListener((int)Client.GameEventID.TASK_DONE, GlobalEventHandler);
            Engine.Utility.EventEngine.Instance().AddEventListener((int)Client.GameEventID.UIEVENTGUIDERECYCLETRIGGER, GlobalEventHandler);
            

            //面板焦点状态改变
            Engine.Utility.EventEngine.Instance().AddEventListener((int)Client.GameEventID.UIEVENT_PANELFOCUSSTATUSCHANGED, GlobalEventHandler);
            Engine.Utility.EventEngine.Instance().AddEventListener((int)Client.GameEventID.UIEVENT_PANELSTATUS, GlobalEventHandler);
            //新功能开启事件
            Engine.Utility.EventEngine.Instance().AddEventListener((int)Client.GameEventID.UIEVENTNEWFUNCOPENREAD, GlobalEventHandler);
            Engine.Utility.EventEngine.Instance().AddEventListener((int)Client.GameEventID.UIEVENTNEWFUNCOPENADD, GlobalEventHandler);
            Engine.Utility.EventEngine.Instance().AddEventListener((int)Client.GameEventID.UIEVENTNEWFUNCCOMPLETE, GlobalEventHandler);

            //新手引导事件
            Engine.Utility.EventEngine.Instance().AddEventListener((int)Client.GameEventID.UIEVENTGUIDECOMPLETE, GlobalEventHandler);
            Engine.Utility.EventEngine.Instance().AddEventListener((int)Client.GameEventID.UIEVENTGAMEOBJMOVESTATUSCHANGED, GlobalEventHandler);
            Engine.Utility.EventEngine.Instance().AddEventListener((int)Client.GameEventID.UIEVENTGUIDESKIP, GlobalEventHandler);

            Engine.Utility.EventEngine.Instance().AddEventListener((int)Client.GameEventID.UIEVENTDOGUIDEWORKFLOWCHECK, GlobalEventHandler);
            Engine.Utility.EventEngine.Instance().AddEventListener((int)Client.GameEventID.UIEVENTGUIDEWORKFLOWCHECKCOMPLETE, GlobalEventHandler);
            Engine.Utility.EventEngine.Instance().AddEventListener((int)Client.GameEventID.UIEVENTDOGUIDEWORKFLOW, GlobalEventHandler);

            Engine.Utility.EventEngine.Instance().AddEventListener((int)Client.GameEventID.UIEVENT_UPDATEITEM, GlobalEventHandler);

            //章节完成
            Engine.Utility.EventEngine.Instance().AddEventListener((int)Client.GameEventID.CHAPTER_EFFECT_END, GlobalEventHandler);
            //断线重连
            Engine.Utility.EventEngine.Instance().AddEventListener((int)Client.GameEventID.RECONNECT_SUCESS, GlobalEventHandler);

            //UI界面状态改变，需要刷引导目标对象
            Engine.Utility.EventEngine.Instance().AddEventListener((int)Client.GameEventID.UIEVENTUISTATECHANGED, GlobalEventHandler);
        }
        else
        {
            Engine.Utility.EventEngine.Instance().RemoveEventListener((int)Client.GameEventID.SYSTEM_GAME_READY, GlobalEventHandler);

            Engine.Utility.EventEngine.Instance().RemoveEventListener((int)Client.GameEventID.ENTITYSYSTEM_LEVELUP, GlobalEventHandler);
            //Engine.Utility.EventEngine.Instance().RemoveEventListener((int)Client.GameEventID.PLAYER_LOGIN_SUCCESS, GlobalEventHandler);
            Engine.Utility.EventEngine.Instance().RemoveEventListener((int)Client.GameEventID.TITLE_NEWTITLE, GlobalEventHandler);
            Engine.Utility.EventEngine.Instance().RemoveEventListener((int)Client.GameEventID.MAINPANEL_SHOWREDWARING, GlobalEventHandler);
            Engine.Utility.EventEngine.Instance().RemoveEventListener((int)Client.GameEventID.UIEVENT_PANELSTATUS, GlobalEventHandler);
            //七天福利状态
            Engine.Utility.EventEngine.Instance().RemoveEventListener((int)Client.GameEventID.SEVENDAYOPENSTATUS, GlobalEventHandler);
            Engine.Utility.EventEngine.Instance().RemoveEventListener((int)Client.GameEventID.GODWEAPENSTATUS, GlobalEventHandler);
            Engine.Utility.EventEngine.Instance().RemoveEventListener((int)Client.GameEventID.RETREWARDSTATUS, GlobalEventHandler);
            Engine.Utility.EventEngine.Instance().RemoveEventListener((int)Client.GameEventID.QUESTIONSTATUS, GlobalEventHandler);
            //首充状态
            Engine.Utility.EventEngine.Instance().RemoveEventListener((int)Client.GameEventID.FIRSTRECHARGESTATUS, GlobalEventHandler);
            //开服豪礼相关
            Engine.Utility.EventEngine.Instance().RemoveEventListener((int)Client.GameEventID.OPENSERVERGIFTSTATUS, GlobalEventHandler);
            //任务相关
            Engine.Utility.EventEngine.Instance().RemoveEventListener((int)Client.GameEventID.TASK_ACCEPT, GlobalEventHandler);
            Engine.Utility.EventEngine.Instance().RemoveEventListener((int)Client.GameEventID.TASK_DONING, GlobalEventHandler);
            Engine.Utility.EventEngine.Instance().RemoveEventListener((int)Client.GameEventID.TASK_CANSUBMIT, GlobalEventHandler);
            Engine.Utility.EventEngine.Instance().RemoveEventListener((int)Client.GameEventID.TASK_DONE, GlobalEventHandler);
            Engine.Utility.EventEngine.Instance().RemoveEventListener((int)Client.GameEventID.UIEVENTGUIDERECYCLETRIGGER, GlobalEventHandler);


            //面板焦点状态改变
            Engine.Utility.EventEngine.Instance().RemoveEventListener((int)Client.GameEventID.UIEVENT_PANELFOCUSSTATUSCHANGED, GlobalEventHandler);

            //新功能开启事件
            Engine.Utility.EventEngine.Instance().RemoveEventListener((int)Client.GameEventID.UIEVENTNEWFUNCOPENADD, GlobalEventHandler);
            Engine.Utility.EventEngine.Instance().RemoveEventListener((int)Client.GameEventID.UIEVENTNEWFUNCOPENREAD, GlobalEventHandler);
            Engine.Utility.EventEngine.Instance().RemoveEventListener((int)Client.GameEventID.UIEVENTNEWFUNCCOMPLETE, GlobalEventHandler);

            //新手引导事件
            Engine.Utility.EventEngine.Instance().RemoveEventListener((int)Client.GameEventID.UIEVENTGUIDECOMPLETE, GlobalEventHandler);
            Engine.Utility.EventEngine.Instance().RemoveEventListener((int)Client.GameEventID.UIEVENTGAMEOBJMOVESTATUSCHANGED, GlobalEventHandler);
            Engine.Utility.EventEngine.Instance().RemoveEventListener((int)Client.GameEventID.UIEVENTGUIDESKIP, GlobalEventHandler);

            Engine.Utility.EventEngine.Instance().RemoveEventListener((int)Client.GameEventID.UIEVENTDOGUIDEWORKFLOWCHECK, GlobalEventHandler);
            Engine.Utility.EventEngine.Instance().RemoveEventListener((int)Client.GameEventID.UIEVENTGUIDEWORKFLOWCHECKCOMPLETE, GlobalEventHandler);
            Engine.Utility.EventEngine.Instance().RemoveEventListener((int)Client.GameEventID.UIEVENTDOGUIDEWORKFLOW, GlobalEventHandler);

            Engine.Utility.EventEngine.Instance().RemoveEventListener((int)Client.GameEventID.UIEVENT_UPDATEITEM, GlobalEventHandler);

            //章节完成
            Engine.Utility.EventEngine.Instance().RemoveEventListener((int)Client.GameEventID.CHAPTER_EFFECT_END, GlobalEventHandler);
            //断线重连
            Engine.Utility.EventEngine.Instance().RemoveEventListener((int)Client.GameEventID.RECONNECT_SUCESS, GlobalEventHandler);

            //UI界面状态改变，需要刷引导目标对象
            Engine.Utility.EventEngine.Instance().RemoveEventListener((int)Client.GameEventID.UIEVENTUISTATECHANGED, GlobalEventHandler);
        }
    }

    /// <summary>
    /// 事件处理
    /// </summary>
    /// <param name="nEventID"></param>
    /// <param name="param"></param>
    public void GlobalEventHandler(int nEventID, object param)
    {
        GuideDefine.GuideTriggerType triggerType = GuideDefine.GuideTriggerType.Invalide;
        bool playerLogin = false;
        switch (nEventID)
        {
            case (int)Client.GameEventID.ENTITYSYSTEM_LEVELUP:
                {
                    triggerType = GuideDefine.GuideTriggerType.Level;
                    CheckTabFuncOpen(true);
                }
                break;
            case (int) (int)Client.GameEventID.CHAPTER_EFFECT_END:
                {
                    triggerType = GuideDefine.GuideTriggerType.ChapterEnd;
                }
                break;
            case (int)Client.GameEventID.TASK_ACCEPT:
            case (int)Client.GameEventID.TASK_DONING:
            case (int)Client.GameEventID.TASK_CANSUBMIT:
            case (int)Client.GameEventID.TASK_DONE:
            case (int) Client.GameEventID.UIEVENTGUIDERECYCLETRIGGER:
                {
                    triggerType = GuideDefine.GuideTriggerType.Task;
                };
                break;
            case (int) Client.GameEventID.SEVENDAYOPENSTATUS:
            case (int)Client.GameEventID.GODWEAPENSTATUS:
            case (int)Client.GameEventID.OPENSERVERGIFTSTATUS:
            case (int)Client.GameEventID.FIRSTRECHARGESTATUS:
            case (int)Client.GameEventID.RETREWARDSTATUS:
            case (int)Client.GameEventID.QUESTIONSTATUS:
                {
                    triggerType = GuideDefine.GuideTriggerType.Condition;
                }
                break;
            case (int)Client.GameEventID.TITLE_NEWTITLE:           //称号获得
            case (int)Client.GameEventID.MAINPANEL_SHOWREDWARING:         //日常
                {
                    stShowMainPanelRedPoint st = (stShowMainPanelRedPoint)param;
                    WarningDirection direction = (WarningDirection)st.direction;
                    WarningEnum model = (WarningEnum)st.modelID;
                    if (model == WarningEnum.Daily && st.bShowRed)
                    {
                        triggerType = GuideDefine.GuideTriggerType.Condition;
                    }                  
                }
                break;

            case (int)Client.GameEventID.UIEVENTDOGUIDEWORKFLOWCHECK:
                {
                    CheckWorkFlowData checkData = new CheckWorkFlowData
                    {
                        CheckTypeMask = GuideDefine.GetTriggerMaskByType(GuideDefine.GuideTriggerType.Always),
                    };
                    if (null != param && param is CheckWorkFlowData)
                    {
                        checkData = (CheckWorkFlowData)param;
                    }
                    CheckWorkFlow(checkData);
                }
                break;
            case (int)Client.GameEventID.SYSTEM_GAME_READY:
            //case (int)Client.GameEventID.PLAYER_LOGIN_SUCCESS:
                {
                    triggerType = GuideDefine.GuideTriggerType.Always;
                    playerLogin = true;
                    CheckTabFuncOpen(false);
                }
                break;
            case (int)Client.GameEventID.RECONNECT_SUCESS:
                {
                    //断线重连
                    if (null != param && param is Client.stReconnectSucess)
                    {
                        Client.stReconnectSucess reconnect = (Client.stReconnectSucess)param;
                    }
                    triggerType = GuideDefine.GuideTriggerType.Always;
                    playerLogin = true;
                }
                break;
            case (int)Client.GameEventID.UIEVENTGUIDEWORKFLOWCHECKCOMPLETE:
                {
                    //设置数据状态
                    m_bool_workFlowReady = true;
                    //执行下一个工作流
                    Engine.Utility.EventEngine.Instance().DispatchEvent((int)Client.GameEventID.UIEVENTDOGUIDEWORKFLOW);
                }
                break;

            case (int)Client.GameEventID.UIEVENTDOGUIDEWORKFLOW:
                {
                    DoWorkFlow();
                }
                break;
            case (int) Client.GameEventID.UIEVENT_PANELSTATUS:
                {
                    if (null != param && param is UIPanelBase.PanelStatusData)
                    {
                        UIPanelBase.PanelStatusData status = (UIPanelBase.PanelStatusData)param;
                        if (status.Status == UIPanelBase.PanelStatus.Show)
                        {
                            DoRefreshNewFuncOpenStaus(status.ID);
                        }else if (status.Status == UIPanelBase.PanelStatus.Hide)
                        {
                            ClearRecycleTriggerGuideByPanel(status.ID);
                        }
                    }
                    
                }
                break;
            /***************面板焦点状态改变********************/
            case (int)Client.GameEventID.UIEVENT_PANELFOCUSSTATUSCHANGED:
                {
                    UIPanelManager.PanelFocusData status = (UIPanelManager.PanelFocusData)param;
                    if (status.GetFocus)
                    {
                        //任务提交面板显示检测工作流（条件触发）
                        Engine.Utility.EventEngine.Instance().DispatchEvent((int)Client.GameEventID.UIEVENTDOGUIDEWORKFLOWCHECK
                            , new CheckWorkFlowData()
                        {
                            CheckTypeMask = (1 << (int)GuideDefine.GuideTriggerType.Condition),
                        });

                        //刷新新功能开启状态
                        //DoRefreshNewFuncOpenStaus(status.ID);
                        //执行下一个工作流
                        Engine.Utility.EventEngine.Instance().DispatchEvent((int)Client.GameEventID.UIEVENTDOGUIDEWORKFLOW);
                    }
                }
                break;
            case (int)Client.GameEventID.UIEVENTGAMEOBJMOVESTATUSCHANGED:
                {
                    if (null != param && param is UIDefine.GameObjMoveData)
                    {
                        UIDefine.GameObjMoveData moveData = param as UIDefine.GameObjMoveData;
                        //Engine.Utility.Log.LogGroup("WJH", "MOveStatus status:{0} child:{1} ", moveData.Status, moveData.Objs.Count);
                        bool adjustTime = false;
                        bool matchTime = false;
                        switch(moveData.Status)
                        {
                            case UIDefine.GameObjMoveStatus.MoveToInvisible:
                                {
                                    adjustTime = true;
                                    matchTime = false;
                                }
                                break;
                            case UIDefine.GameObjMoveStatus.Invisible:
                            case UIDefine.GameObjMoveStatus.Visible:
                                {
                                    adjustTime = true;
                                    matchTime = true;
                                }
                                break;
                            case UIDefine.GameObjMoveStatus.MoveToVisible:
                                {

                                }
                                break;
                        }

                        AdjustWorkFlowDoTime(adjustTime,matchTime);
                        AdjustUnGuideRefreshDoTime(adjustTime, matchTime);
                    }
                    
                    
                }
                break;
            /**********新手引导*************/
            case (int)Client.GameEventID.UIEVENTGUIDECOMPLETE:
                {
                    //完成一个引导
                    CompleteGuide((uint)param);
                    //执行下一个工作流
                    Engine.Utility.EventEngine.Instance().DispatchEvent((int)Client.GameEventID.UIEVENTDOGUIDEWORKFLOW);
                }
                break;
            case (int)Client.GameEventID.UIEVENTGUIDESKIP:
                {
                    //跳过静默完成当前引导
                    SilentCompleteGuideGroup((uint)param);
                    //执行下一个工作流
                    Engine.Utility.EventEngine.Instance().DispatchEvent((int)Client.GameEventID.UIEVENTDOGUIDEWORKFLOW);
                }
                break;

            /**********新功能开启*************/
            case (int)Client.GameEventID.UIEVENTNEWFUNCOPENREAD:
                {
                    OnNewFuncOpenRead((GuideDefine.FuncOpenShowData)param);
                }
                break;
            case (int)Client.GameEventID.UIEVENTNEWFUNCOPENADD:
                {
                    OnNewFuncOpenAdd((GuideDefine.FuncOpenShowData)param);
                }
                break;
            case (int)Client.GameEventID.UIEVENTNEWFUNCCOMPLETE:
                {
                    //执行下一个工作流
                    Engine.Utility.EventEngine.Instance().DispatchEvent((int)Client.GameEventID.UIEVENTDOGUIDEWORKFLOW);
                }
                break;

            //物品变更检测工作流（条件触发）
            case (int)Client.GameEventID.UIEVENT_UPDATEITEM:
                {
                    if (null != param && param is ItemDefine.UpdateItemPassData)
                    {
                        ItemDefine.UpdateItemPassData passData = param as ItemDefine.UpdateItemPassData;
                        if (passData.UpdateType == ItemDefine.UpdateItemType.Add
                            || (passData.UpdateType == ItemDefine.UpdateItemType.Update && passData.ChangeNum > 0))
                        {
                            GuideDefine.GetTriggerMaskByType(GuideDefine.GuideTriggerType.Condition);
                            //任务提交面板显示检测工作流（条件触发）
                            Engine.Utility.EventEngine.Instance().DispatchEvent((int)Client.GameEventID.UIEVENTDOGUIDEWORKFLOWCHECK
                                , new CheckWorkFlowData()
                                {
                                    CheckTypeMask = (GuideDefine.GetTriggerMaskByType(GuideDefine.GuideTriggerType.Condition)
                                    | GuideDefine.GetTriggerMaskByType(GuideDefine.GuideTriggerType.ItemGet)),
                                    Param = passData.BaseId,
                                });
                        }
                    }
                }
                break;

            case (int)Client.GameEventID.UIEVENTUISTATECHANGED:
                {
                    if (null != param && param is PanelID)
                    {
                        PanelID pid = (PanelID) param;
                        RefreshGuideTargetObj(pid);
                    }
                }
                break;

        }

        if (triggerType != GuideDefine.GuideTriggerType.Invalide)
        {
            //检测工作流
            Engine.Utility.EventEngine.Instance().DispatchEvent((int)Client.GameEventID.UIEVENTDOGUIDEWORKFLOWCHECK, new CheckWorkFlowData()
                {
                    CheckTypeMask = GuideDefine.GetTriggerMaskByType(triggerType),
                    Param = param,
                });
        }

        if (playerLogin)
        {
            //数据流准备好
            Engine.Utility.EventEngine.Instance().DispatchEvent((int)Client.GameEventID.UIEVENTGUIDEWORKFLOWCHECKCOMPLETE);
        }

    }
    #endregion

    #region TriggerData
    /// <summary>
    /// 是否triggerId满足触发条件
    /// </summary>
    /// <param name="triggerId"></param>
    /// <returns></returns>
    public bool IsMatchTriggerCondition(uint triggerId,object param = null)
    {
        table.GuideTriggerCondiDataBase
            db = GameTableManager.Instance.GetTableItem<table.GuideTriggerCondiDataBase>(triggerId);
        bool isMatch = false;
        if (null != db)
        {
            switch (db.triggerType)
            {
                case (uint)GuideDefine.GuideTriggerType.Always:
                    isMatch = true;
                    break;
                case (uint)GuideDefine.GuideTriggerType.Level:
                    {
                        if (DataManager.Instance.PlayerLv >= db.triggerParamType)
                        {
                            isMatch = true;
                        }
                    }
                    break;
                case (uint) GuideDefine.GuideTriggerType.ChapterEnd:
                    {
                        if (null != param
                            && param is uint
                            && db.triggerParamType == ((uint)param))
                        {
                            isMatch = true;
                        }
                    }
                    break;
                case (uint)GuideDefine.GuideTriggerType.Task:
                    {
                        bool canCheck = false;
                        if (null != param && param is GuideDefine.RecycleTriggerGuidePassData)
                        {
                            GuideDefine.RecycleTriggerGuidePassData passData = (GuideDefine.RecycleTriggerGuidePassData)param;
                            if (db.triggerParamType == passData.TaskID)
                            {
                                canCheck = true;
                            }
                        }else
                        {
                            canCheck = true;
                        }
                        if (canCheck)
                        {
                            GuideDefine.TaskSubTriggerType taskSubTriggerType = GetTaskStatus(db.triggerParamType);
                            if ((int)taskSubTriggerType == db.taskTriggerStatus)
                            {
                                isMatch = true;
                            }
                        }
                        
                    }
                    break;
                case (uint)GuideDefine.GuideTriggerType.Condition:
                    {
                        isMatch = CheckGuideConditionMatch((GuideDefine.ConditonSubTriggerType)db.triggerParamType,db.panelTriggerID);
                    }
                    break;

                case (uint) GuideDefine.GuideTriggerType.ItemGet:
                    {
                        int num = DataManager.Manager<ItemManager>().GetItemNumByBaseId(db.triggerParamType);
                        isMatch = (num > 0);
                    }
                    break;
            }
        }
        return isMatch;
    }

    /// <summary>
    /// 检测当前条件触发是否满足
    /// </summary>
    /// <param name="conditionType"></param>
    /// <returns></returns>
    private bool CheckGuideConditionMatch(GuideDefine.ConditonSubTriggerType conditionType,object param = null)
    {
        bool isMatch = false;
        switch (conditionType)
        {
            case GuideDefine.ConditonSubTriggerType.CST_None:
                isMatch = false;
                break;
            case GuideDefine.ConditonSubTriggerType.CST_FGActiveReward:
                {
                    List<uint> rewardList = DataManager.Manager<DailyManager>().CanGetBoxList;
                    if (null != rewardList && rewardList.Count > 0)
                    {
                        isMatch = true;
                    }
                }
                break;
            case GuideDefine.ConditonSubTriggerType.CST_FGGem1_3:
                {
                    List<uint> subType = new List<uint>();
                    subType.Add((int)ItemDefine.ItemMaterialSubType.Gem);
                    List<BaseItem> lifeGems = DataManager.Manager<ItemManager>().GetBaseItemByType(GameCmd.ItemBaseType.ItemBaseType_Material
                        ,subType, GameCmd.PACKAGETYPE.PACKAGETYPE_MAIN);
                    if (null != lifeGems)
                    {
                        Gem gem = null;
                        for(int i = 0; i < lifeGems.Count;i ++)
                        {
                            if (lifeGems[i].Num == 0
                                || !(lifeGems[i] is Gem))
                            {
                                continue;
                            }
                            gem = lifeGems[i] as Gem;
                            if (gem.Type != GameCmd.GemType.GemType_UpgradeLife)
                            {
                                continue;
                            }
                            if (gem.BaseData.grade >= 1 && gem.BaseData.grade <= 3)
                            {
                                isMatch = true;
                                break;
                            }
                        }
                    }
                }
                break;
            case GuideDefine.ConditonSubTriggerType.CST_FGMuhon0:
                {
                    List<uint> subType = new List<uint>();
                    subType.Add((int)GameCmd.EquipType.EquipType_SoulOne);
                    List<BaseItem> muhons = DataManager.Manager<ItemManager>().GetBaseItemByType(GameCmd.ItemBaseType.ItemBaseType_Equip
                        , subType, GameCmd.PACKAGETYPE.PACKAGETYPE_MAIN);
                    if (null != muhons)
                    {
                        Muhon muhon = null;
                        for (int i = 0; i < muhons.Count; i++)
                        {
                            if (muhons[i].Num == 0
                                || !(muhons[i] is Muhon))
                            {
                                continue;
                            }
                            muhon = muhons[i] as Muhon;
                            if (muhon.StartLevel == 0)
                            {
                                isMatch = true;
                                break;
                            }
                        }
                    }
                }
                break;
            case GuideDefine.ConditonSubTriggerType.CST_FGSkillBook:
                {
                    List<uint> subType = new List<uint>();
                    subType.Add((int)ItemDefine.ItemMaterialSubType.SkillBook);
                    List<BaseItem> skillBooks = DataManager.Manager<ItemManager>().GetBaseItemByType(GameCmd.ItemBaseType.ItemBaseType_Material
                        , subType, GameCmd.PACKAGETYPE.PACKAGETYPE_MAIN);
                    if (null != skillBooks)
                    {
                        BaseItem book = null;
                        for (int i = 0; i < skillBooks.Count; i++)
                        {
                            if (skillBooks[i].Num == 0)
                            {
                                continue;
                            }
                            isMatch = true;
                            break;
                        }
                    }
                    //List<Client.IPet> petList = DataManager.Manager<PetDataManager>().GetSortPetList();
                    //if (null != petList)
                    //{
                    //    for (int i = 0; i < petList.Count; i++)
                    //    {
                    //        if (petList[i].GetPetSkillList().Count > 0)
                    //        {
                    //            isMatch = true;
                    //            break;
                    //        }
                    //    }
                    //}
                }
                break;
            case GuideDefine.ConditonSubTriggerType.CST_FGTitle:
                {
                    isMatch = DataManager.Manager<TitleManager>().OwnedTitleList.Count > 0;
                }
                break;
            case GuideDefine.ConditonSubTriggerType.CST_FOPanel:
                {
                    if (null != param && param is string)
                    {
                        string panelIdStr = (string)param;
                        try
                        {
                            PanelID pid = (PanelID)Enum.Parse(typeof(PanelID), panelIdStr, true);
                            isMatch = DataManager.Manager<UIPanelManager>().IsShowPanel(pid);
                        }
                        catch(Exception e)
                        {
                            Engine.Utility.Log.Error("GuideManager_Event->CheckGuideConditionMatch parse panel failed error = {0},pid = {1}",e.ToString(), panelIdStr);
                        }
                    }
                }
                break;
            case GuideDefine.ConditonSubTriggerType.CST_FGJoinClan:
                {
                    isMatch = DataManager.Manager<ClanManger>().IsJoinFormal;
                }
                break;
            case GuideDefine.ConditonSubTriggerType.CST_7TianClose:
                {
                    isMatch = !DataManager.Manager<WelfareManager>().SevenDayOpenState;
                }
                break;
            case GuideDefine.ConditonSubTriggerType.CST_ShenBinClose:
                {
                    isMatch = DataManager.Manager<WelfareManager>().IsGodWeapenActivate;
                }
                break;
            case GuideDefine.ConditonSubTriggerType.CST_OpenServerGiftClose:
                {
                    isMatch = DataManager.Manager<WelfareManager>().IsOpenServerGiftFinished;
                }
                break;
            case GuideDefine.ConditonSubTriggerType.CST_FirstRechargeRewardClose:
                {
                    isMatch = DataManager.Manager<ActivityManager>().HadGotFirstRechargeReward;
                }
                break;
            case GuideDefine.ConditonSubTriggerType.CST_RechargeRewRetClose:
                {
                    isMatch = DataManager.Manager<ActivityManager>().RechargeRewRetClose;
                }
                break;
            case GuideDefine.ConditonSubTriggerType.CST_QuestionClose:
                {
                    isMatch = DataManager.Manager<FunctionShieldManager>().IsQuestionClose;
                }
                break;
        }
        return isMatch;
    }

    /// <summary>
    /// 获取任务当前状态
    /// </summary>
    /// <param name="taskId"></param>
    /// <returns></returns>
    private GuideDefine.TaskSubTriggerType GetTaskStatus(uint taskId)
    {
        GuideDefine.TaskSubTriggerType status = GuideDefine.TaskSubTriggerType.Invalid;
        QuestTraceInfo quest = QuestTranceManager.Instance.GetQuestTraceInfo(taskId);
        if (null != quest && quest.Received)
        {
            switch (quest.GetTaskProcess())
            {
                case GameCmd.TaskProcess.TaskProcess_Doing:
                    status = GuideDefine.TaskSubTriggerType.Doing;
                    break;
                case GameCmd.TaskProcess.TaskProcess_CanDone:
                    status = GuideDefine.TaskSubTriggerType.CanSubmit;
                    break;

                case GameCmd.TaskProcess.TaskProcess_Done:
                    status = GuideDefine.TaskSubTriggerType.Complete;
                    break;
            }
        }
        else
        {
            List<QuestTraceInfo> canAcceptTasks = new List<QuestTraceInfo>();
            DataManager.Manager<TaskDataManager>().GetCanReceiveQuest(ref canAcceptTasks);
            if (null != canAcceptTasks)
            {
                for (int i = 0; i < canAcceptTasks.Count; i++)
                {
                    if (canAcceptTasks[i].taskId == taskId)
                    {
                        status = GuideDefine.TaskSubTriggerType.CanAccept;
                        break;
                    }
                }
            }
        }
        return status;
    }

    #endregion
}