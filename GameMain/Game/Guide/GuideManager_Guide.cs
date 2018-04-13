/************************************************************************************
 * Copyright (c) 2017  All Rights Reserved.
 * CLR版本： 4.0.30319.42000
 * 公司名称：ZQB
 * 命名空间：GameMain.Game.Guide
 * 创建人：  wenjunhua.zqgame
 * 文件名：  GuideManager_Guide
 * 版本号：  V1.0.0.0
 * 创建时间：4/1/2017 3:02:43 PM
 * 描述：
 ************************************************************************************/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

partial class GuideManager
{
    #region GUIDE(功能引导)
    //新手引导是否开启
    public const string GUIDE_ENABLE_NAME = "GuideEnable";
    public static bool IsGuideEnable
    {
        get
        {
            if (Application.isEditor)
            {
                return GameMain.Instance.OpenNewGuide;
            }
            int status = GameTableManager.Instance.GetGlobalConfig<int>(GUIDE_ENABLE_NAME);
            return (status == 1);
        }
    }
    //新手引导指向偏移
    public const string GUIDE_POINTAT_OFFSET_NAME = "GuidePointOffset";
    public static float GuidePointAtOffset
    {
        get
        {
            return GameTableManager.Instance.GetGlobalConfig<float>(GUIDE_POINTAT_OFFSET_NAME);
        }
    }

    //新手引导自动关闭delay
    public const string GUIDE_AUTO_CLOSE_DELAY_NAME = "GuideAutoCloseDelay";
    public static float GuideAutoCloseDelay
    {
        get
        {
            return GameTableManager.Instance.GetGlobalConfig<float>(GUIDE_AUTO_CLOSE_DELAY_NAME);
        }
    }

    //新手引导弹出跳过delay
    public const string GUIDE_SKIP_DELAY_NAME = "GuideShowSkipDelay";
    public static float GuideShowSkipDelay
    {
        get
        {
            return GameTableManager.Instance.GetGlobalConfig<float>(GUIDE_SKIP_DELAY_NAME);
        }
    }



    //当前正在做的强制引导
    private GuideDefine.LocalGuideGroupData m_curDoConstraintGuide = null;
    public GuideDefine.LocalGuideGroupData CurDoConstriantGuide
    {
        get
        {
            return m_curDoConstraintGuide;
        }
        set
        {
            m_curDoConstraintGuide = value;
        }
    }

    //当前正在做的非强制引导
    private Dictionary<uint,GuideDefine.LocalGuideGroupData> m_dic_curDoUnConstraintGuide;
    public Dictionary<uint,GuideDefine.LocalGuideGroupData> CurDoUnConstraintGuide
    {
        get
        {
            return m_dic_curDoUnConstraintGuide;
        }
    }

    //当前正在执行的反复触发的非强制引导
    private GuideDefine.LocalGuideGroupData m_curDoRepeatUnConstraintGuide;
    public GuideDefine.LocalGuideGroupData CurDoRepeatUnConstraintGuide
    {

        get
        {
            return m_curDoRepeatUnConstraintGuide;
        }set
        {
            m_curDoRepeatUnConstraintGuide = value;
        }
    }


    /// <summary>
    /// 是否正在执行该类型
    /// </summary>
    /// <param name="guideType"></param>
    /// <param name="guideId"></param>
    /// <returns></returns>
    public bool IsDoingGuide(GuideDefine.GuideType guideType, uint guideId = 0)
    {
        if (guideType == GuideDefine.GuideType.Constraint)
        {
            if (guideId == 0)
            {
                return null != CurDoConstriantGuide;
            }
            else
            {
                return null != CurDoConstriantGuide && CurDoConstriantGuide.CurStep != 0
                && CurDoConstriantGuide.CurStep == guideId;
            }
        }
        else if (guideType == GuideDefine.GuideType.Unconstrain)
        {
            if (guideId == 0)
            {
                return m_dic_curDoUnConstraintGuide.Count > 0;
            }
            else
            {
                GuideDefine.LocalGuideGroupData tempGroup = null;
                var iemurator = m_dic_curDoUnConstraintGuide.GetEnumerator();
                while (iemurator.MoveNext())
                {
                    tempGroup = iemurator.Current.Value;
                    if (iemurator.Current.Value.CurStep != 0
                        && iemurator.Current.Value.CurStep == guideId)
                    {
                        return true;
                    }

                }
            }
        }
        return false;
    }

    /// <summary>
    /// 是否
    /// </summary>
    /// <param name="gtype"></param>
    /// <returns></returns>
    public bool HaveUnFinishGuide(GuideDefine.GuideType gtype)
    {
        List<uint> groupids = null;
        if (m_dic_canDoGuideGroup.TryGetValue(gtype, out groupids) && groupids.Count > 0)
        {
            return true;
        }
        return false;
    }

    //引导本地数据
    Dictionary<uint, GuideDefine.LocalGuideData> m_dic_guideDatas = null;
    //本地引导组数据
    private Dictionary<uint, GuideDefine.LocalGuideGroupData> m_dic_guideGroupDatas = null;

    //已完成引导组ID
    private List<uint> m_lst_completeGroupGuide = null;
    //当前可执行的引导组
    private Dictionary<GuideDefine.GuideType, List<uint>> m_dic_canDoGuideGroup = null;
    //当前可执行的重复引导
    private uint curCanDoRepeatGroup = 0;
    //引导组数据
    private List<uint> m_lstGuideGroups = null;
    //已经触发的引导组
    private List<uint> m_lstTriggerGuideGroups = null;
    private bool m_bGuideDataReady = false;

    /// <summary>
    /// 重置引导
    /// </summary>
    private void ResetGuide()
    {
        //新手引导
        m_lst_completeGroupGuide.Clear();
        m_curDoConstraintGuide = null;
        m_lstTriggerGuideGroups.Clear();
        m_bGuideDataReady = false;
        if (null != m_dic_guideGroupDatas && m_dic_guideGroupDatas.Count != 0)
        {
            List<uint> groupIds = new List<uint>(m_dic_guideGroupDatas.Keys);
            for(int i = 0,max = groupIds.Count;i < max;i ++)
            {
                m_dic_guideGroupDatas[groupIds[i]].Reset();
            }
        }

        if (null != m_dic_canDoGuideGroup)
        {
            m_dic_canDoGuideGroup.Clear();
        }

        if (null != CurDoUnConstraintGuide && CurDoUnConstraintGuide.Count > 0)
        {
            var iemurator = CurDoUnConstraintGuide.GetEnumerator();
            GuideDefine.LocalGuideGroupData localGroupData = null;
            while (iemurator.MoveNext())
            {
                localGroupData = iemurator.Current.Value;
                if (localGroupData.CurStep != 0)
                {
                    CancelUnconstrainGuideShow(localGroupData.CurStep);
                }
            }
            CurDoUnConstraintGuide.Clear();
        }

        if (null != m_curDoRepeatUnConstraintGuide)
        {
            if (m_curDoRepeatUnConstraintGuide.CurStep != 0)
            {
                CancelUnconstrainGuideShow(m_curDoRepeatUnConstraintGuide.CurStep);
            }
            m_curDoRepeatUnConstraintGuide = null;
        }
    }

    private void InitGuideData()
    {
        m_lst_completeGroupGuide = new List<uint>();
        m_dic_canDoGuideGroup = new Dictionary<GuideDefine.GuideType, List<uint>>();
        m_dic_guideDatas = new Dictionary<uint, GuideDefine.LocalGuideData>();
        m_dic_guideGroupDatas = new Dictionary<uint, GuideDefine.LocalGuideGroupData>();
        m_dic_curDoUnConstraintGuide = new Dictionary<uint, GuideDefine.LocalGuideGroupData>();
        m_lstGuideGroups = new List<uint>();
        m_lstTriggerGuideGroups = new List<uint>();

        List<table.GuideDataBase> guideDatas = GameTableManager.Instance.GetTableList<table.GuideDataBase>();
        if (null != guideDatas)
        {
            GuideDefine.LocalGuideData localData = null;
            GuideDefine.LocalGuideGroupData localGroupData = null;
            for (int i = 0; i < guideDatas.Count; i++)
            {
                localData = GuideDefine.LocalGuideData.Create(guideDatas[i].id);
                if (null == localData || localData.GuideGroupID == 0)
                    continue;
                if (!m_dic_guideDatas.ContainsKey(localData.ID))
                {
                    m_dic_guideDatas.Add(localData.ID, localData);
                }

                if (!m_dic_guideGroupDatas.TryGetValue(localData.GuideGroupID, out localGroupData))
                {
                    localGroupData = GuideDefine.LocalGuideGroupData.Create(localData.GuideGroupID);
                    m_dic_guideGroupDatas.Add(localData.GuideGroupID
                        , localGroupData);
                }
                localGroupData.Add(localData);

                //构造数据引导组步骤数据
                if (!m_lstGuideGroups.Contains(localData.GuideGroupID))
                {
                    m_lstGuideGroups.Add(localData.GuideGroupID);
                }
            }

            //对引导组内部步骤排序
            if (m_dic_guideGroupDatas.Count > 0)
            {
                var groupEmurator = m_dic_guideGroupDatas.GetEnumerator();
                GuideDefine.LocalGuideData leftData = null;
                GuideDefine.LocalGuideData rightData = null;
                while(groupEmurator.MoveNext())
                {
                    localGroupData = groupEmurator.Current.Value;
                    localGroupData.InitData();
                    localGroupData.GroupSteps.Sort((left, right) =>
                        {
                            if (TryGetGuideLocalData(left,out leftData) && TryGetGuideLocalData(right,out rightData))
                            {
                                return (int)leftData.GuideGroupStep - (int)rightData.GuideGroupStep;
                            }
                            return 0;
                        });
                }
            }
        }
    }

    /// <summary>
    /// 尝试获取引导的组ID
    /// </summary>
    /// <param name="guideId"></param>
    /// <param name="groupId"></param>
    /// <returns></returns>
    public bool TryGetGuideGroupID(uint guideId,out uint groupId)
    {
        groupId = 0;
        GuideDefine.LocalGuideData localData = null;
        if (TryGetGuideLocalData(guideId, out localData))
        {
            groupId = localData.GuideGroupID;
            return true;
        }
        return false;
    }

    /// <summary>
    /// 尝试获取引导本地数据
    /// </summary>
    /// <param name="guideId"></param>
    /// <param name="localData"></param>
    /// <returns></returns>
    public bool TryGetGuideLocalData(uint guideId, out GuideDefine.LocalGuideData localData)
    {
        return m_dic_guideDatas.TryGetValue(guideId, out localData);
    }

    /// <summary>
    /// 引导组是否已触发
    /// </summary>
    /// <param name="groupId"></param>
    /// <returns></returns>
    public bool IsTriggerGuideGroup(uint groupId)
    {
        return m_lstTriggerGuideGroups.Contains(groupId);
    }

    /// <summary>
    /// 检测引导是否可以触发
    /// </summary>
    /// <param name="checkData"></param>
    private void CheckGuide(CheckWorkFlowData checkData)
    {
        if (null == checkData || checkData.CheckTypeMask == 0)
            return;
        GuideDefine.LocalGuideGroupData tempGroup = null;
        bool newAdd = false;
        for(int i = 0,max = m_lstGuideGroups.Count;i < max;i++)
        {
            if (!TryGetGuideGroupData(m_lstGuideGroups[i],out tempGroup) || null == tempGroup)
            {
                Engine.Utility.Log.Error("GuideManager_Guide->CheckGuide faield,groupData null groupID:{0}",tempGroup.GroupId);
                continue;
            }

            if (!tempGroup.LoopTrigger &&(IsGuideGroupComplete(tempGroup.GroupId) || IsTriggerGuideGroup(tempGroup.GroupId)))
            {
                continue;
            }

            if (null == tempGroup.TriggerIds || tempGroup.TriggerIds.Count == 0)
            {
                Engine.Utility.Log.Error("GuideManager_Guide->CheckGuide faield,triggerData error GroupID:{0}", tempGroup.GroupId);
                continue;
            }
            

            bool matchCheck = false;
            table.GuideTriggerCondiDataBase tempTriggerData = null;
            if (!tempGroup.LoopTrigger || null != checkData.Param && checkData.Param is GuideDefine.RecycleTriggerGuidePassData)
            {
                for (int k = 0, triggerCount = tempGroup.TriggerIds.Count; k < triggerCount; k++)
                {
                    tempTriggerData = GameTableManager.Instance.GetTableItem<table.GuideTriggerCondiDataBase>(tempGroup.TriggerIds[k]);
                    if (null == tempTriggerData)
                        break;

                    if (GuideDefine.IsMaskMatchTriggerType(checkData.CheckTypeMask, GuideDefine.GuideTriggerType.Always)
                      || GuideDefine.IsMaskMatchTriggerType(checkData.CheckTypeMask, tempTriggerData.triggerType)
                      || tempTriggerData.triggerType == (uint)GuideDefine.GuideTriggerType.Always)
                    {
                        matchCheck = true;
                        break;
                    }
                }
            }

            bool triggerGuideGroup = true;
            if (matchCheck)
            {
                for (int j = 0, maxCount = tempGroup.TriggerIds.Count; j < maxCount; j++)
                {
                    if (!IsMatchTriggerCondition(tempGroup.TriggerIds[j], checkData.Param))
                    {
                        triggerGuideGroup = false;
                        break;
                    }
                }
            }else
            {
                triggerGuideGroup = false;
            }

            if (triggerGuideGroup)
            {
                Engine.Utility.Log.LogGroup("WJH", "TriggerGuideGroup:{0}" , tempGroup.GroupId);
                newAdd = true;

                if (tempGroup.LoopTrigger)
                {
                    if (IsGuideGroupComplete(tempGroup.GroupId))
                    {
                        m_lst_completeGroupGuide.Remove(tempGroup.GroupId);
                    }
                    ResetGuideGroup(tempGroup.GroupId);

                    if (null != CurDoRepeatUnConstraintGuide)
                    {
                        ResetGuideGroup(CurDoRepeatUnConstraintGuide.GroupId);
                    }
                }

                OnNewGuideGroupTrigger(tempGroup.GroupId);
                //非强制引导触发就发送服务器保存
                if (tempGroup.GType == GuideDefine.GuideType.Unconstrain
                    && !tempGroup.LoopTrigger)
                {
                    SendGuideComplete(tempGroup.GroupId);
                }
            }
        }

        if (newAdd)
        {
            //执行引导工作流
            Engine.Utility.EventEngine.Instance().DispatchEvent((int)Client.GameEventID.UIEVENTDOGUIDEWORKFLOW);
        }
    }

    /// <summary>
    /// 重置引导组
    /// </summary>
    /// <param name="groupID"></param>
    private void ResetGuideGroup(uint groupID)
    {
        uint curDoGuide = 0;
        if (IsDoingGroup(groupID,out curDoGuide))
        {
            if (CurDoUnConstraintGuide.ContainsKey(groupID))
            {
                CurDoUnConstraintGuide.Remove(groupID);
            }
            else if (null != CurDoRepeatUnConstraintGuide && CurDoRepeatUnConstraintGuide.GroupId == groupID)
            {
                m_curDoRepeatUnConstraintGuide = null;
            }
            CancelUnconstrainGuideShow(curDoGuide);
        }
    }

    public void RefreshPanelGuideObj(PanelID pid)
    {

    }

    /// <summary>
    ///刷新
    /// </summary>
    /// <param name="pid"></param>
    private void RefreshGuideTargetObj(PanelID pid)
    {
        GuideDefine.LocalGuideGroupData localGroupData = null;
        GuideDefine.LocalGuideData localGuideData = null;
        if (null != CurDoUnConstraintGuide && CurDoUnConstraintGuide.Count > 0)
        {
            var iemurator = CurDoUnConstraintGuide.GetEnumerator();
            while (iemurator.MoveNext())
            {
                localGroupData = iemurator.Current.Value;
                if (null == localGroupData || !localGroupData.LoopTrigger)
                    continue;
                if (localGroupData.CurStep != 0
                    && TryGetGuideLocalData(localGroupData.CurStep, out localGuideData)
                    && localGuideData.GuideTargetDependPanel == pid)
                {
                    localGuideData.GuideTargetObj = null;
                }
            }
        }

        if (null != CurDoRepeatUnConstraintGuide)
        {
            if (CurDoRepeatUnConstraintGuide.CurStep != 0
                && TryGetGuideLocalData(CurDoRepeatUnConstraintGuide.CurStep, out localGuideData)
                    && localGuideData.GuideTargetDependPanel == pid)
            {
                localGuideData.GuideTargetObj = null;
            }
        }
    }

    /// <summary>
    /// 依赖面板关闭清空相应的循环触发引导（正在执行，或者等待执行的）
    /// </summary>
    /// <param name="id"></param>
    private void ClearRecycleTriggerGuideByPanel(PanelID pid)
    {
        //1、等待列表移除
        uint tempID = 0;
        List<uint> waitDoList = null;
        List<uint> removeList = null;
        List<uint> stepList = null;
        GuideDefine.LocalGuideGroupData localGroupData = null;
        GuideDefine.LocalGuideData localGuideData = null;
        if (m_dic_canDoGuideGroup.TryGetValue(GuideDefine.GuideType.Unconstrain, out waitDoList)
            && waitDoList.Count > 0)
        {
            for(int i = 0,max = waitDoList.Count;i < max;i++)
            {
                tempID = waitDoList[i];
                if (!TryGetGuideGroupData(tempID,out localGroupData) || !localGroupData.LoopTrigger)
                {
                    continue;
                }
                stepList = localGroupData.GroupSteps;
                if (null == stepList || stepList.Count == 0)
                    continue;

                for(int j = 0,maxj = stepList.Count;j < maxj;j++)
                {
                    tempID = stepList[j];
                    if (!TryGetGuideLocalData(tempID,out localGuideData))
                    {
                        continue;
                    }

                    if (localGuideData.GuideTargetDependPanel == pid)
                    {
                        if (null == removeList)
                            removeList = new List<uint>();
                        if (!removeList.Contains(waitDoList[i]))
                            removeList.Add(waitDoList[i]);
                    }

                }
            }
        }
        if (null != removeList)
        {
            for (int i = 0, max = removeList.Count; i < max; i++)
            {
                tempID = removeList[i];
                waitDoList.Remove(tempID);
            }
        }

        //2、进行中引导
        if (null != CurDoUnConstraintGuide && CurDoUnConstraintGuide.Count > 0)
        {
            var iemurator = CurDoUnConstraintGuide.GetEnumerator();
            while(iemurator.MoveNext())
            {
                localGroupData = iemurator.Current.Value;
                if (null == localGroupData || !localGroupData.LoopTrigger)
                    continue;
                if (localGroupData.CurStep != 0
                    && TryGetGuideLocalData(localGroupData.CurStep, out localGuideData)
                    && localGuideData.GuideTargetDependPanel == pid
                    && (//localGroupData.CurStep != localGroupData.FirstStep || 
                    pid != PanelID.MainPanel))
                {
                    if (null == removeList)
                        removeList = new List<uint>();
                    if (!removeList.Contains(localGroupData.GroupId))
                        removeList.Add(localGroupData.GroupId);
                }
            }
        }
        if (null != removeList)
        {
            for (int i = 0, max = removeList.Count; i < max; i++)
            {
                tempID = removeList[i];
                ResetGuideGroup(tempID);
            }
        }

        //3、重复触发引导
        if (null != CurDoRepeatUnConstraintGuide && CurDoRepeatUnConstraintGuide.CurStep != 0)
        {
            if (CurDoRepeatUnConstraintGuide.CurStep != 0
                    && TryGetGuideLocalData(CurDoRepeatUnConstraintGuide.CurStep, out localGuideData)
                    && localGuideData.GuideTargetDependPanel == pid
                    && (//localGroupData.CurStep != localGroupData.FirstStep || 
                    pid != PanelID.MainPanel))
            {
                ResetGuideGroup(CurDoRepeatUnConstraintGuide.GroupId);
            }
        }

    }

    /// <summary>
    /// 引导组是否满足忽略条件
    /// </summary>
    /// <param name="groupId"></param>
    /// <returns></returns>
    public bool IsNeedIgnoreGroup(uint groupId)
    {
        bool ignore = true;
        GuideDefine.LocalGuideGroupData tempGroup = null;
        if (TryGetGuideGroupData(groupId, out tempGroup)
            && null != tempGroup.IgnoreTriggerIds
            && tempGroup.IgnoreTriggerIds.Count > 0)
        {
            for (int i = 0, maxCount = tempGroup.IgnoreTriggerIds.Count; i < maxCount; i++)
            {
                if (!IsNeedIgnoreByCondi(tempGroup.IgnoreTriggerIds[i]))
                {
                    ignore = false;
                    break;
                }
            }
        }else
        {
            ignore = false;
        }
        
        return ignore;
    }

    /// <summary>
    /// 是否满足条件忽略
    /// </summary>
    /// <param name="ignoreLv"></param>
    /// <returns></returns>
    public bool IsNeedIgnoreByCondi(uint ignoreTriggerId)
    {
        return IsMatchTriggerCondition(ignoreTriggerId);
    }

    /// <summary>
    /// 新引导组触发
    /// </summary>
    /// <param name="groupID"></param>
    private void OnNewGuideGroupTrigger(uint groupID)
    {
        GuideDefine.LocalGuideGroupData tempGroup = null;
        if (TryGetGuideGroupData(groupID, out tempGroup))
        {
            if (tempGroup.LoopTrigger)
            {
                curCanDoRepeatGroup = tempGroup.GroupId;
            }
            else if (!IsTriggerGuideGroup(groupID))
            {
                m_lstTriggerGuideGroups.Add(groupID);
                //添加组
                List<uint> groups = null;
                if (!m_dic_canDoGuideGroup.TryGetValue(tempGroup.GType, out groups))
                {
                    groups = new List<uint>();
                    m_dic_canDoGuideGroup.Add(tempGroup.GType, groups);
                }
                if (!groups.Contains(tempGroup.GroupId))
                {
                    groups.Add(tempGroup.GroupId);
                    //排序
                    groups.Sort((left, right) =>
                    {
                        GuideDefine.LocalGuideGroupData lleft = null;
                        GuideDefine.LocalGuideGroupData lright = null;
                        if (TryGetGuideGroupData(left, out lleft) && TryGetGuideGroupData(right, out lright))
                        {
                            return lleft.Priority - lright.Priority;
                        }
                        return 0;
                    });

                }
            }
        }
        
    }

    /// <summary>
    /// 执行非强制引导
    /// </summary>
    private void DoUnconstrainGuide()
    {
        //新功能开启优先于功能引导
        if (IsDoingNewFuncOpen)
        {
            return;
        }
        //如果还有下一个新功能开启
        if (IsNextNewFuncOpenReady())
        {
            return;
        }
        //如果正在执行强制引导
        //存在一个连续的强制引导
        if (HaveUnFinishGuide(GuideDefine.GuideType.Constraint)
            || IsDoingGuide(GuideDefine.GuideType.Constraint)
            || IsNextConstraitGuideReady())
        {
            return;
        }

        GuideDefine.LocalGuideGroupData localGroupData = null;
        //1、等待执行列表
        List<uint> canDoGuideGroups = TakeCanDoUnConstraitGuideGroup();
        if (null != canDoGuideGroups && canDoGuideGroups.Count > 0)
        {
            for (int i = 0; i < canDoGuideGroups.Count; i++)
            {
                if (!TryGetGuideGroupData(canDoGuideGroups[i], out localGroupData) 
                    || !IsGuideUIStatusReady(localGroupData.FirstStep))
                {
                    continue;
                }

                DoGuideGroup(canDoGuideGroups[i]);
            }
        }

        //2、执行正在进行的引导
        if (CurDoUnConstraintGuide.Count > 0)
        {
            List<uint> groupIds = new List<uint>();
            groupIds.AddRange(CurDoUnConstraintGuide.Keys);
            for(int i =0,max = groupIds.Count;i < max;i++)
            {
                localGroupData = CurDoUnConstraintGuide[groupIds[i]];
                if (localGroupData.CurStep == 0)
                {
                    DoGuideGroup(localGroupData.GroupId);
                }
            }
        }

        //3、执行正在执行的重复引导
        if (curCanDoRepeatGroup != 0)
        {
            if (!IsDoingGroup(curCanDoRepeatGroup)
                && !IsGuideGroupComplete(curCanDoRepeatGroup)
                && !IsNeedIgnoreGroup(curCanDoRepeatGroup))
            {
                if (TryGetGuideGroupData(curCanDoRepeatGroup, out localGroupData)
                    && IsGuideUIStatusReady(localGroupData.FirstStep))
                {
                    DoGuideGroup(curCanDoRepeatGroup);
                }
            }
        }else if (null != CurDoRepeatUnConstraintGuide)
        {
            if (CurDoRepeatUnConstraintGuide.CurStep == 0)
            {
                DoGuideGroup(CurDoRepeatUnConstraintGuide.GroupId);
            }
        }
    }



    /// <summary>
    /// 执行下一引导
    /// </summary>
    private bool DoNextConstrainGuide()
    {
        bool success = false;
        //新功能开启优先于功能引导
        if (IsDoingNewFuncOpen)
        {
            return success;
        }
        //如果还有下一个新功能开启
        if (IsNextNewFuncOpenReady())
        {
            return false;
        }
        //如果正在执行强制引导
        if (IsDoingGuide(GuideDefine.GuideType.Constraint))
        {
            return false;
        }

        uint groupID = 0;
        if (IsNextConstraitGuideReady(out groupID))
        {
            DoGuideGroup(groupID);
        }
        return success;
    }

    /// <summary>
    /// 是否正在执行引导组
    /// </summary>
    /// <param name="guideGroup"></param>
    /// <returns></returns>
    public bool IsDoingGroup(uint guideGroup)
    {
        uint guideId = 0;
        return IsDoingGroup(guideGroup, out guideId);
    }

    /// <summary>
    /// 是否当前正在执行引导组
    /// </summary>
    /// <param name="guideGroup"></param>
    /// <param name="guideId"></param>
    /// <returns></returns>
    public bool IsDoingGroup(uint guideGroup, out uint guideId)
    {
        guideId = 0;
        GuideDefine.LocalGuideGroupData groupData = null;
        if (TryGetGuideGroupData(guideGroup,out groupData))
        {
            if (groupData.GType == GuideDefine.GuideType.Unconstrain)
            {
                if (!groupData.LoopTrigger)
                {
                    if (null != CurDoUnConstraintGuide && CurDoUnConstraintGuide.Count != 0)
                    {
                        var iemurator = CurDoUnConstraintGuide.GetEnumerator();
                        GuideDefine.LocalGuideGroupData tempGroup = null;
                        while (iemurator.MoveNext())
                        {
                            tempGroup = iemurator.Current.Value;
                            if (tempGroup.GroupId == guideGroup)
                            {
                                guideId = tempGroup.CurStep;
                                return true;
                            }
                        }
                    }
                }
                else if (null != CurDoRepeatUnConstraintGuide 
                    && CurDoRepeatUnConstraintGuide.GroupId == guideGroup)
                {
                    guideId = CurDoRepeatUnConstraintGuide.CurStep;
                    return true;
                }
                
            }else if (groupData.GType == GuideDefine.GuideType.Constraint)
            {
                if (null != CurDoConstriantGuide
                    && CurDoConstriantGuide.GroupId == guideGroup)
                {
                    guideId = CurDoConstriantGuide.CurStep;
                    return true;
                }
            }
        }
        
        return false;
    }

    /// <summary>
    /// 是否下一个引导准备好了
    /// </summary>
    /// <param name="gType"></param>
    /// <param name="guideId"></param>
    /// <returns></returns>
    private bool IsNextConstraitGuideReady(out uint guideId)
    {
        if (TakeNextConstraitGuide(out guideId) && IsGuideUIStatusReady(guideId))
        {
            return true;
        }
        return false;
    }

    /// <summary>
    /// 是否下一个引导准备好了
    /// </summary>
    /// <param name="gType"></param>
    /// <returns></returns>
    private bool IsNextConstraitGuideReady()
    {
        uint guideId = 0;
        return IsNextConstraitGuideReady(out guideId);
    }

    List<uint> tempCanDoResult = null;
    List<uint> tempCanDoCacheData = null;
    private List<uint> TakeCanDoUnConstraitGuideGroup()
    {
        if (null == tempCanDoResult || tempCanDoResult.Count > 20)
        {
            tempCanDoResult = new List<uint>();
        }
        tempCanDoResult.Clear();

        if (null == tempCanDoCacheData || tempCanDoCacheData.Count > 20)
        {
            tempCanDoCacheData = new List<uint>();
        }
        tempCanDoCacheData.Clear();

        List<uint> groups = null;
        GuideDefine.GuideType gType = GuideDefine.GuideType.Unconstrain;
        if (m_dic_canDoGuideGroup.TryGetValue(gType, out groups)
            && groups.Count > 0)
        {
            tempCanDoCacheData = new List<uint>();
            tempCanDoCacheData.AddRange(groups);
            for (int i = 0; i < tempCanDoCacheData.Count; i++)
            {
                if (IsDoingGroup(tempCanDoCacheData[i])
                    || IsGuideGroupComplete(tempCanDoCacheData[i]))
                {
                    groups.Remove(tempCanDoCacheData[i]);
                    continue;
                }
                if (!IsNeedIgnoreGroup(tempCanDoCacheData[i]))
                {
                    if (!tempCanDoResult.Contains(tempCanDoCacheData[i]))
                    {
                        tempCanDoResult.Add(tempCanDoCacheData[i]);
                    }
                }else
                {
                    SilentCompleteGuideGroup(tempCanDoCacheData[i]);
                }
            }
        }

        
        return tempCanDoResult;

    }

    /// <summary>
    /// 获取下一个强制引导
    /// </summary>
    /// <param name="guideId"></param>
    /// <returns></returns>
    private bool TakeNextConstraitGuide(out uint guideId)
    {
        guideId = 0;
        List<uint> guideIds = null;
        List<uint> groups = null;
        bool isMatch = false;
        GuideDefine.GuideType gType = GuideDefine.GuideType.Constraint;
        if (m_dic_canDoGuideGroup.TryGetValue(gType, out groups) && groups.Count > 0)
        {
            GuideDefine.LocalGuideGroupData groupData = null;
            GuideDefine.LocalGuideData localData = null;
            uint tempGuideId = 0;
            if (!IsDoingGuide(gType))
            {
                do
                {
                    isMatch = false;
                    if (IsGuideGroupComplete(groups[0]))
                    {
                        SilentCompleteGuideGroup(groups[0]);
                    }
                    else
                    {
                        if (TryGetGuideGroupData(groups[0], out groupData)
                        && groupData.TryGetNextStep(out guideId)
                        && TryGetGuideLocalData(guideId, out localData)
                        && (localData.IsStartStep || localData.TryGetPreStep(out tempGuideId) && IsGuideStepComplete(tempGuideId)))
                        {
                            isMatch = true;
                        }

                        if (isMatch && IsNeedIgnoreGroup(groups[0]))
                        {
                            CompleteGuide(guideId);
                            isMatch = false;
                        }

                    }

                } while (groups.Count > 0 && !isMatch);
            }

        }
        return isMatch;
    }

    /// <summary>
    /// 取消非强制引导显示
    /// </summary>
    /// <param name="guideID"></param>
    private void CancelUnconstrainGuideShow(uint guideID)
    {
        GuideDefine.LocalGuideData localData = null;
        if (TryGetGuideLocalData(guideID,out localData)
            && localData.GType == GuideDefine.GuideType.Unconstrain)
        {
            Engine.Utility.EventEngine.Instance().DispatchEvent((int)Client.GameEventID.UIEVENTGUIDERESET, guideID);
        }
    }

    /// <summary>
    /// 取消引导组
    /// </summary>
    /// <param name="guideGroup"></param>
    private void CancelUnconstrainGuideGroupShow(uint guideGroup)
    {

    }

    /// <summary>
    /// 执行引导组
    /// </summary>
    /// <param name="groupId"></param>
    /// <returns></returns>
    private bool DoGuideGroup(uint groupId)
    {
        bool success = false;
        if (GuideManager.IsGuideEnable)
        {
            GuideDefine.LocalGuideGroupData localGroupData = null;
            if (TryGetGuideGroupData(groupId,out localGroupData))
            {
                uint curDoGuideID = 0;
                if (!IsDoingGroup(groupId,out curDoGuideID))
                {
                    localGroupData.Reset();
                    if (localGroupData.GType == GuideDefine.GuideType.Constraint)
                    {
                        m_curDoConstraintGuide = localGroupData;
                    }else if (localGroupData.GType == GuideDefine.GuideType.Unconstrain)
                    {
                        if (!localGroupData.LoopTrigger)
                            m_dic_curDoUnConstraintGuide.Add(groupId, localGroupData);
                        else
                        {
                            if (curCanDoRepeatGroup != 0)
                                curCanDoRepeatGroup = 0;
                            m_curDoRepeatUnConstraintGuide = localGroupData;
                        }
                    }
                }

                if (localGroupData.CurStep == 0)
                {
                    if (localGroupData.TryGetNextStep(out curDoGuideID))
                    {
                        GuideDefine.LocalGuideData localData = null;
                        if (CheckGuideDataReady(curDoGuideID) && TryGetGuideLocalData(curDoGuideID, out localData))
                        {
                            localData.GuideTargetObj = null;
                            localGroupData.MoveToNextStep();
                            if (localData.JumpID != 0)
                            {
                                ItemManager.DoJump(localData.JumpID);
                            }
                            if (localData.GDType != GuideDefine.GuideGUIDependType.None)
                            {
                                Engine.Utility.EventEngine.Instance().DispatchEvent((int)Client.GameEventID.UIEVENTGUIDEITEMFOCUS, new GuideDefine.GuideItemFocusData()
                                {
                                    DependType = localData.GDType,
                                    Data = localData.GDParam[0],
                                });
                            }
                            if (localGroupData.GType == GuideDefine.GuideType.Unconstrain)
                            {
                                DataManager.Manager<UIPanelManager>().ShowPanel(PanelID.GuideUnconstrainPanel, data: localData);
                            }
                            else if (localGroupData.GType == GuideDefine.GuideType.Constraint)
                            {
                                DataManager.Manager<UIPanelManager>().ShowPanel(PanelID.GuidePanel, data: localData);
                            }
                            Engine.Utility.Log.LogGroup("WJH", "DoGuide:" + localData.ID);
                            Engine.Utility.EventEngine.Instance().DispatchEvent((int)Client.GameEventID.UIEVENTGUIDESHOWOUT);
                        }
                    }
                    else
                    {
                        SilentCompleteGuideGroup(groupId);
                    }
                }
                
            }else
            {
                SilentCompleteGuideGroup(groupId);
            }
        }
        else
        {
            SilentCompleteGuideGroup(groupId);
        }
        return success;
    }

    /// <summary>
    /// 检测引导数据是否准备好(一般是指目标对象)
    /// </summary>
    /// <param name="guideId"></param>
    /// <returns></returns>
    private bool CheckGuideDataReady(uint guideId)
    {
        GuideDefine.LocalGuideData guideData = null;
        bool ready = true;
        if (TryGetGuideLocalData(guideId, out guideData))
        {
            //1、目标依赖面板为空 2、目标对象不存在 3、依赖数据为空
            if (guideData.GuideTargetDependPanel == PanelID.None
                || (guideData.GDType == GuideDefine.GuideGUIDependType.None && guideData.GuideTargetObj == null)
                || (guideData.GDType != GuideDefine.GuideGUIDependType.None && guideData.GDParam.Count == 0))
            {
                ready = false;
            }

            //数据检测
            if (ready)
            {
                if (guideData.GuideCheckType == GuideDefine.GuideCheckDataType.Type
                    && guideData.GuideCheckItemType != GameCmd.ItemBaseType.ItemBaseType_None)
                {
                    List<uint> itemIds =  DataManager.Manager<ItemManager>().GetItemByType(guideData.GuideCheckItemType);
                    if (null == itemIds || itemIds.Count == 0)
                    {
                        ready = false;
                    }
                }
            }
        }
        return ready;
    }

    /// <summary>
    /// 静默完成引导组
    /// </summary>
    /// <param name="guideGroup">引导组</param>
    private void SilentCompleteGuideGroup(uint guideGroup)
    {
        GuideDefine.LocalGuideGroupData groupData = null;
        if (TryGetGuideGroupData(guideGroup, out groupData))
        {
            for (int i = 0; i < groupData.GroupSteps.Count; i++)
            {
                //完成当前组
                CompleteGuide(groupData.GroupSteps[i]);
            }
        }
        else
        {
            Engine.Utility.Log.Error("GuideManager->SilentCompleteGuideGroup failed,get groupdata error ,groupId:{0}", guideGroup);
        }
    }

    /// <summary>
    /// 完成引导
    /// </summary>
    /// <param name="guideId"></param>
    private void CompleteGuide(uint guideId)
    {
     //   Engine.Utility.Log.LogGroup("WJH", "CompleteGuide:" + guideId);
        GuideDefine.LocalGuideGroupData groupdata = null;
        uint groupID = 0;
        if (TryGetGuideGroupID(guideId,out groupID) && TryGetGuideGroupData(groupID,out groupdata))
        {
            bool complete = false;
            if (groupdata.GType == GuideDefine.GuideType.Constraint)
            {
                if (null != CurDoConstriantGuide && CurDoConstriantGuide.CurStep == guideId)
                {
                    CurDoConstriantGuide.LastestDoStep = guideId;
                    CurDoConstriantGuide.EndCur();
                    if (CurDoConstriantGuide.CurStep == CurDoConstriantGuide.LastStepGuideID)
                    {
                        complete = true;
                        CurDoConstriantGuide = null;
                    }
                }
            }
            else if (groupdata.GType == GuideDefine.GuideType.Unconstrain)
            {
                if (CurDoUnConstraintGuide.ContainsKey(groupID))
                {
                    if (CurDoUnConstraintGuide[groupID].CurStep == guideId)
                    {
                        CurDoUnConstraintGuide[groupID].LastestDoStep = guideId;
                        CurDoUnConstraintGuide[groupID].EndCur();
                        if (CurDoUnConstraintGuide[groupID].LastestDoStep == CurDoUnConstraintGuide[groupID].LastStepGuideID)
                        {
                            complete = true;
                            CurDoUnConstraintGuide.Remove(groupID);
                        }
                    }
                }else if (null != CurDoRepeatUnConstraintGuide && CurDoRepeatUnConstraintGuide.CurStep == guideId)
                {
                    CurDoRepeatUnConstraintGuide.LastestDoStep = guideId;
                    CurDoRepeatUnConstraintGuide.EndCur();
                    if (CurDoRepeatUnConstraintGuide.LastestDoStep == CurDoRepeatUnConstraintGuide.LastStepGuideID)
                    {
                        complete = true;
                        CurDoRepeatUnConstraintGuide = null;
                    }
                }
            }

            //如果组最后一步发送消息到服务器保存已完成组
            if (complete && !m_lst_completeGroupGuide.Contains(groupdata.GroupId))
            {
                m_lst_completeGroupGuide.Add(groupdata.GroupId);
                //非循环引导
                if (!groupdata.LoopTrigger)
                    SendGuideComplete(groupdata.GroupId);
            }
        }
    }

    /// <summary>
    /// 是否引导UI准备好(获取了焦点)
    /// </summary>
    /// <param name="guideId"></param>
    /// <returns></returns>
    public bool IsGuideUIStatusReady(uint guideId)
    {
        GuideDefine.LocalGuideData localdata = null;
        
        bool ready = false;
        if (TryGetGuideLocalData(guideId, out localdata)
            && DataManager.Manager<UIPanelManager>().IsPanelFocus(localdata.GuideTargetDependPanel)
            && ((localdata.GDType != GuideDefine.GuideGUIDependType.None && localdata.GDParam.Count > 0)
            || (localdata.GDType == GuideDefine.GuideGUIDependType.None && UIManager.IsObjVisibleByCamera(localdata.GuideTargetObj))))
        {
            ready = true;
        }
        return ready;
    }





    /// <summary>
    /// 是否引导已完成
    /// </summary>
    /// <param name="guideId"></param>
    /// <returns></returns>
    public bool IsGuideStepComplete(uint guideId)
    {
        GuideDefine.LocalGuideGroupData localGroupData = null;
        uint groupId = 0;
        if (TryGetGuideGroupID(guideId, out groupId) && TryGetGuideGroupData(groupId,out localGroupData))
        {
            if (IsGuideGroupComplete(guideId))
            {
                return true;
            }
            if (localGroupData.GType == GuideDefine.GuideType.Unconstrain)
            {
                if (null != CurDoConstriantGuide && CurDoConstriantGuide.GroupId == guideId)
                {
                    return CurDoConstriantGuide.IsCompleteStep(guideId);
                }
            }else if (localGroupData.GType == GuideDefine.GuideType.Constraint)
            {
                if (CurDoUnConstraintGuide.ContainsKey(localGroupData.GroupId))
                {
                    return CurDoUnConstraintGuide[localGroupData.GroupId].IsCompleteStep(guideId);
                }
            }
        }
        return false;
    }

    /// <summary>
    /// 是否完成引导组
    /// </summary>
    /// <param name="groupId"></param>
    /// <returns></returns>
    public bool IsGuideGroupComplete(uint groupId)
    {
        return m_lst_completeGroupGuide.Contains(groupId);
    }
    /// <summary>
    /// 发送引导完成
    /// </summary>
    /// <param name="groupID">已完成引导组</param>
    private void SendGuideComplete(uint groupID)
    {
        if (null != DataManager.Instance.Sender)
        {
            DataManager.Instance.Sender.SendGuideCompleteReq(groupID);
        }
    }

    /// <summary>
    /// 服务器下发已完成引导ID
    /// </summary>
    /// <param name="completeGroupIds"></param>
    public void OnGuideInfoGet(List<uint> completeGroupIds)
    {
        m_bGuideDataReady = true;
        if (null == completeGroupIds || completeGroupIds.Count == 0)
        {
            return;
        }
        for (int i = 0; i < completeGroupIds.Count; i++)
        {
            if (IsGuideGroupComplete(completeGroupIds[i]))
            {
                continue;
            }
            m_lst_completeGroupGuide.Add(completeGroupIds[i]);
        }
    }

    /// <summary>
    /// 获取引导组本地数据
    /// </summary>
    /// <param name="guideGroupId">引导组ID</param>
    /// <returns></returns>
    public bool TryGetGuideGroupData(uint guideGroupId, out GuideDefine.LocalGuideGroupData groupData)
    {
        return m_dic_guideGroupDatas.TryGetValue(guideGroupId, out groupData);
    }

    /// <summary>
    /// 是否当前正在展示引导
    /// </summary>
    /// <returns></returns>
    public bool IsShowGuildUI()
    {
        if (null != CurDoConstriantGuide)
        {
            return true;
        }

        if (IsDoingGuide(GuideDefine.GuideType.Unconstrain))
        {
            return true;
        }
        return false;
    }

    #endregion
}