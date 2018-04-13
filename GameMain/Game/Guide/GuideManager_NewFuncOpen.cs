/************************************************************************************
 * Copyright (c) 2017  All Rights Reserved.
 * CLR版本： 4.0.30319.42000
 * 公司名称：ZQB
 * 命名空间：GameMain.Game.Guide
 * 创建人：  wenjunhua.zqgame
 * 文件名：  GuideManager_NewFuncOpen
 * 版本号：  V1.0.0.0
 * 创建时间：4/1/2017 3:03:55 PM
 * 描述：
 ************************************************************************************/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

partial class GuideManager
{
    #region NewFuncOpen(新功能开启)
    //新功能提醒是否开启
    public const string NEW_FUNCTION_OPEN_ENABLE_NAME = "NewFuncOpenNoticeEnable";
    public static bool IsNewFunctionOpenNoticeEnable
    {
        get
        {
            if (Application.isEditor)
            {
                return GameMain.Instance.OpenNewFuncGuide;
            }
            int status = GameTableManager.Instance.GetGlobalConfig<int>(NEW_FUNCTION_OPEN_ENABLE_NAME);
            return (status == 1);
        }
    }

    //新功能提醒自动关闭延迟
    public const string NEW_FUNCTION_OPEN_AUTOCLOSE_DELAY_NAME = "NewFuncOpenNoticeAutoCloseDelay";
    public static int NewFuncAutoCloseDelay
    {
        get
        {
            return GameTableManager.Instance.GetGlobalConfig<int>(NEW_FUNCTION_OPEN_AUTOCLOSE_DELAY_NAME);
        }
    }
    private System.Collections.Queue m_FuncCommonNoticesQ = null;
    //已经完成的功能提示
    private List<uint> m_lst_alreadyDoFuncOpen = null;
   
    //当前可执行的提示
    private List<uint> m_lst_canDoFuncOpen = null;

    //已经关闭的
    private List<uint> m_lst_closeFuncOpen = null;
    //功能提示本地数据
    Dictionary<uint, GuideDefine.LocalFuncOpenData> m_dic_funcOpenDatas = null;

    //排序组
    Dictionary<int, List<uint>> m_dic_completeFuncSort = null;

    //当前执行的功能提示
    private GuideDefine.FuncOpenShowData m_currentFuncOpenShowData = null;
    public GuideDefine.FuncOpenShowData CurrenFuncOpenShowData
    {
        get
        {
            return m_currentFuncOpenShowData;
        }
    }
    private bool m_bNewFuncDataReady = false;
    /// <summary>
    /// 是否正在执行新功能提醒
    /// </summary>
    public bool IsDoingNewFuncOpen
    {
        get
        {

            return (null != m_currentFuncOpenShowData);
        }
    }


    public Vector3 NewFuncFlyStartPos
    {
        get;
        set;
    }

    /// <summary>
    /// 充值新功能开启
    /// </summary>
    private void ResetFuncOpen()
    {
        //新功能提醒
        m_lst_alreadyDoFuncOpen.Clear();
        m_lst_canDoFuncOpen.Clear();
        m_lst_closeFuncOpen.Clear();
        m_FuncCommonNoticesQ.Clear();
        m_currentFuncOpenShowData = null;
        m_newFuncFlyGrid = null;
        m_dic_completeFuncSort.Clear();
        m_bNewFuncDataReady = false;
    }

    /// <summary>
    /// 初始化功能开启提示
    /// </summary>
    private void InitFuncOpenData()
    {
        if (null == m_lst_alreadyDoFuncOpen)
        {
            m_lst_alreadyDoFuncOpen = new List<uint>();
        }

        if (null == m_lst_canDoFuncOpen)
        {
            m_lst_canDoFuncOpen = new List<uint>();
        }

        if (null == m_lst_closeFuncOpen)
        {
            m_lst_closeFuncOpen = new List<uint>();
        }

        if (null == m_dic_completeFuncSort)
        {
            m_dic_completeFuncSort = new Dictionary<int, List<uint>>();
        }
        m_dic_funcOpenDatas = new Dictionary<uint, GuideDefine.LocalFuncOpenData>();
        m_FuncCommonNoticesQ = new System.Collections.Queue();
        List<table.NewFUNCOpenDataBase> newFuncOpenData = GameTableManager.Instance.GetTableList<table.NewFUNCOpenDataBase>();
        if (null != newFuncOpenData)
        {
            GuideDefine.LocalFuncOpenData localData = null;
            table.NewFUNCOpenDataBase tempdb = null;
            for (int i = 0; i < newFuncOpenData.Count; i++)
            {
                tempdb = newFuncOpenData[i];
                if (null == tempdb)
                {
                    continue;
                }
                if (!m_dic_funcOpenDatas.ContainsKey(tempdb.openFuncID))
                {
                    localData = GuideDefine.LocalFuncOpenData.Create(tempdb.openFuncID);
                    m_dic_funcOpenDatas.Add(tempdb.openFuncID, localData);
                }
            }
        }
    }

    /// <summary>
    /// 尝试获取功能开启数据
    /// </summary>
    /// <param name="funcId"></param>
    /// <param name="data"></param>
    /// <returns></returns>
    public bool TryGetFuncOpenData(uint funcId,out GuideDefine.LocalFuncOpenData data)
    {
        return m_dic_funcOpenDatas.TryGetValue(funcId,out data);
    }

    /// <summary>
    ///检测是否有新功能开启
    /// </summary>
    /// <param name="triggerType">检测触发类型</param>
    private void CheckNewFuncOpen(CheckWorkFlowData checkData)
    {
        if (null == checkData || checkData.CheckTypeMask == 0)
            return;

        table.GuideTriggerCondiDataBase triggerData = null;
        foreach (KeyValuePair<uint, GuideDefine.LocalFuncOpenData> pair in m_dic_funcOpenDatas)
        {
            if ((m_lst_canDoFuncOpen.Contains(pair.Key)
                || m_lst_alreadyDoFuncOpen.Contains(pair.Key))
                && pair.Value.CloseTriggerId == 0)
            {
                continue;
            }
            if (pair.Value.AutoOpen)
            {
                if (pair.Value.CloseTriggerId != 0 
                    && IsMatchTriggerCondition(pair.Value.CloseTriggerId, checkData.Param))
                {
                     CloseNewFuncOpen(pair.Key);
                }else
                {
                    CompleteNewFuncOpen(new
                    GuideDefine.FuncOpenShowData()
                    {
                        FOT = GuideDefine.FuncOpenType.Base,
                        FuncOpenId = pair.Key,
                    });
                }
            }
            else
            {
                triggerData = GameTableManager.Instance.GetTableItem<table.GuideTriggerCondiDataBase>(pair.Value.TriggerId);
                if (null == triggerData)
                {
                    continue;
                }

                if (GuideDefine.IsMaskMatchTriggerType(checkData.CheckTypeMask, GuideDefine.GuideTriggerType.Always)
                    || GuideDefine.IsMaskMatchTriggerType(checkData.CheckTypeMask, triggerData.triggerType)
                    || triggerData.triggerType == (uint)GuideDefine.GuideTriggerType.Always)
                {
                    if (IsMatchTriggerCondition(pair.Value.TriggerId, checkData.Param))
                    {
                        if (pair.Value.CloseTriggerId == 0 || !IsMatchTriggerCondition(pair.Value.CloseTriggerId, checkData.Param))
                        {
                            AddNewFuncOpen(pair.Key);
                        }
                        else if (IsMatchTriggerCondition(pair.Value.CloseTriggerId, checkData.Param))
                        {
                            CloseNewFuncOpen(pair.Key);
                        }
                    }
                }
            }
            
        }
    }

    /// <summary>
    /// 是否新功能开启提醒完成
    /// </summary>
    /// <param name="funcId">新功能提醒ID</param>
    /// <returns></returns>
    public bool IsNewFuncOpenComplete(uint funcId)
    {
        return IsNewFuncOpen(funcId);
        //return (null != m_lst_alreadyDoFuncOpen && m_lst_alreadyDoFuncOpen.Contains(funcId));
    }

    /// <summary>
    /// 添加新功能开启到缓存列表
    /// </summary>
    /// <param name="funcId"></param>
    private void AddNewFuncOpen(uint funcId)
    {
        GuideDefine.FuncOpenShowData showData = new GuideDefine.FuncOpenShowData()
        {
            FOT = GuideDefine.FuncOpenType.Base,
            FuncOpenId = funcId,
        };
        Engine.Utility.EventEngine.Instance().DispatchEvent((int)Client.GameEventID.UIEVENTNEWFUNCOPENADD, showData);
    }

    /// <summary>
    /// 添加新功能提醒到缓存
    /// </summary>
    /// <param name="showData"></param>
    private void OnNewFuncOpenAdd(GuideDefine.FuncOpenShowData showData)
    {
        if (null != showData)
        {
            bool add = false;
            if (showData.FOT == GuideDefine.FuncOpenType.Base)
            {
                GuideDefine.LocalFuncOpenData localdata = m_dic_funcOpenDatas[showData.FuncOpenId];
                if (IsNeedIgnoreByCondi(localdata.IgnoreCondi))
                {
                    CompleteNewFuncOpen(showData);
                }
                else if (!m_lst_canDoFuncOpen.Contains(showData.FuncOpenId))
                {
                    //1、添加到新功能提醒列表
                    m_lst_canDoFuncOpen.Add(showData.FuncOpenId);
                    //2、根据优先级排序
                    m_lst_canDoFuncOpen.Sort((left, right) =>
                    {
                        return GuideDefine.LocalFuncOpenData.CompareFuncOpenPriority(left, right);
                    });
                    add = true;
                }

            }
            else if (showData.FOT == GuideDefine.FuncOpenType.Skill)
            {
                int count = m_FuncCommonNoticesQ.Count + 1;
                m_FuncCommonNoticesQ.Enqueue(showData);
                add = true;
            }

            if (add)
            {
                //执行下一个工作流
                Engine.Utility.EventEngine.Instance().DispatchEvent((int)Client.GameEventID.UIEVENTDOGUIDEWORKFLOW);
            }
        }
    }

    /// <summary>
    /// 新功能关闭
    /// </summary>
    /// <param name="funcOpenId"></param>
    private void CloseNewFuncOpen(uint funcOpenId)
    {
        GuideDefine.LocalFuncOpenData data = null;
        if (TryGetFuncOpenData(funcOpenId,out data))
        {
            bool needRefresh = false;
            if (m_lst_alreadyDoFuncOpen.Contains(funcOpenId))
            {
                needRefresh = true;
                m_lst_alreadyDoFuncOpen.Remove(funcOpenId);
            }
                
            if (m_lst_canDoFuncOpen.Contains(funcOpenId))
            {
                needRefresh = true;
                m_lst_canDoFuncOpen.Remove(funcOpenId);
            }

            if (m_lst_closeFuncOpen.Contains(funcOpenId))
            {
                needRefresh = true;
                m_lst_closeFuncOpen.Add(funcOpenId);
            }
                
            List<uint> sortGroupIDs = null;
            if (m_dic_completeFuncSort.TryGetValue(data.SortGroup, out sortGroupIDs) && sortGroupIDs.Contains(funcOpenId))
            {
                needRefresh = true;
                sortGroupIDs.Remove(funcOpenId);
            }

            RefreshNewFuncUIStatus(funcOpenId);
            if (data.NeedSort)
            {
                if (needRefresh)
                {
                    DoRefreshNewFuncOpenStaus(data.SortGroup);
                }
            }
        }
    }

    /// <summary>
    /// 添加到已完成功能提示列表
    /// </summary>
    /// <param name="funcId"></param>
    private void CompleteNewFuncOpen(GuideDefine.FuncOpenShowData showData)
    {
        if (null == showData)
        {
            return;
        }
        if (showData.FOT == GuideDefine.FuncOpenType.Base)
        {
            //1、从当前新功能开启列表中移除
            if (null != m_lst_canDoFuncOpen && m_lst_canDoFuncOpen.Contains(showData.FuncOpenId))
            {
                m_lst_canDoFuncOpen.Remove(showData.FuncOpenId);
            }

            bool add = false;
            //2、并添加到已完成提醒功能列表
            if (null != m_lst_alreadyDoFuncOpen && !m_lst_alreadyDoFuncOpen.Contains(showData.FuncOpenId))
            {
                m_lst_alreadyDoFuncOpen.Add(showData.FuncOpenId);
                add = true;
            }

            GuideDefine.LocalFuncOpenData openData = null;
            if (TryGetFuncOpenData(showData.FuncOpenId, out openData))
            {
                if (openData.NeedSort)
                {
                    List<uint> sortIds = null;
                    bool needRefreshUI = false;
                    if (!m_dic_completeFuncSort.TryGetValue(openData.SortGroup, out sortIds))
                    {
                        sortIds = new List<uint>();
                        sortIds.Add(showData.FuncOpenId);
                        m_dic_completeFuncSort.Add(openData.SortGroup, sortIds);
                        needRefreshUI = true;
                    }
                    else if (!sortIds.Contains(showData.FuncOpenId))
                    {
                        sortIds.Add(showData.FuncOpenId);
                        sortIds.Sort((left, right) =>
                        {
                            GuideDefine.LocalFuncOpenData leftData = null;
                            GuideDefine.LocalFuncOpenData rightData = null;
                            if (TryGetFuncOpenData(left, out leftData) && TryGetFuncOpenData(right, out rightData))
                            {
                                return leftData.SortID - rightData.SortID;
                            }
                            return 0;
                        });
                        needRefreshUI = true;
                    }
                    if (needRefreshUI)
                    {
                        DoRefreshNewFuncOpenStaus(openData.SortGroup);
                    }
                }
                else if (null != openData.CreateFuncObj && !openData.CreateFuncObj.activeSelf)
                {
                    openData.CreateFuncObj.SetActive(true);
                }
            }

            if (add)
            {
                Engine.Utility.EventEngine.Instance().DispatchEvent((int)Client.GameEventID.UIEVENTNEWFUNCOPEN, showData.FuncOpenId);
                //发送已完成消息
                SendNewFuncOpenComplete(showData.FuncOpenId);
            }
        }
        else if (showData.FOT == GuideDefine.FuncOpenType.Skill)
        {
            if (null != m_FuncCommonNoticesQ && m_FuncCommonNoticesQ.Count != 0)
            {
                GuideDefine.FuncOpenShowData cacheShowData = (GuideDefine.FuncOpenShowData)m_FuncCommonNoticesQ.Peek();
                if (cacheShowData.FuncOpenId == showData.FuncOpenId)
                {
                    //取出第一个
                    m_FuncCommonNoticesQ.Dequeue();
                }
            }
        }
        m_currentFuncOpenShowData = null;
        Engine.Utility.EventEngine.Instance().DispatchEvent((int)Client.GameEventID.UIEVENTNEWFUNCCOMPLETE, showData);
    }

    /// <summary>
    /// 刷新新功能UI状态
    /// </summary>
    /// <param name="funcId"></param>
    private void RefreshNewFuncUIStatus(uint funcId)
    {
        GuideDefine.LocalFuncOpenData localData = null;

        if (null != m_dic_funcOpenDatas && m_dic_funcOpenDatas.TryGetValue(funcId, out localData))
        {
            PanelID dependPanelId = localData.FuncCreateObjPathData.DependPanelId;
            if (dependPanelId == PanelID.None || !DataManager.Manager<UIPanelManager>().IsShowPanel(dependPanelId))
            {
                //Engine.Utility.Log.Error("GuideManger->RefreshNewFuncUIStatus faield,relative panel panelId:{0}"
                //    , localData.FuncCreateObjPathData.DependPanelId);
                return;
            }
            GameObject obj = localData.CreateFuncObj;
            if (null != obj)
            {
                bool funcOpen = IsNewFuncOpen(funcId);
                if (funcOpen && localData.NeedSort)
                {
                    obj.transform.localPosition = GetNewFuncObjTargetPos(funcId);
                }
                if (obj.activeSelf != funcOpen)
                    obj.SetActive(funcOpen);
            }
        }
        else
        {
            Engine.Utility.Log.Error("GuideManger->RefreshNewFuncUIStatus faield,can't found funcId:{0}", funcId);
        }
    }

    /// <summary>
    /// 获取已开启功能按钮的目标位置
    /// </summary>
    /// <param name="newFuncId"></param>
    /// <returns></returns>
    private Vector3 GetNewFuncObjTargetPos(uint newFuncId)
    {
        Vector3 pos = Vector3.zero;
        GuideDefine.LocalFuncOpenData openData = null;
        int funcIndex = 0;
        if (TryGetFuncOpenData(newFuncId, out openData) 
            && TryGetFunctionSortIndex(openData.SortGroup, newFuncId, out funcIndex))
        {
            pos += funcIndex * openData.FuncObjGap;
        }
        return pos;
    }

    private bool TryGetFunctionSortIndex(int groupId,uint funcID,out int index)
    {
        index = 0;
        List<uint> groupDatas = null;
        if (null != m_dic_completeFuncSort && m_dic_completeFuncSort.TryGetValue(groupId, out groupDatas) && groupDatas.Contains(funcID))
        {
            index = groupDatas.IndexOf(funcID);
            return true;
        }
        return false;
    }

    private void DoRefreshNewFuncOpenStaus(int sortGroup)
    {
        List<uint> groupDatas = null;
        if (!m_dic_completeFuncSort.TryGetValue(sortGroup, out groupDatas))
        {
            return;
        }

        List<uint> removeData =null;
        for(int i = 0,max = groupDatas.Count;i < max;i ++)
        {
            if (!IsNewFuncOpen(groupDatas[i]))
            {
                if (null == removeData)
                {
                    removeData = new List<uint>();
                }
                removeData.Add(groupDatas[i]);
            }
        }

        if (null != removeData)
        {
            for (int j = 0, maxj = removeData.Count; j < maxj; j++)
            {
                if (groupDatas.Contains(removeData[j]))
                {
                    groupDatas.Remove(removeData[j]);
                }
            }
        }

        for (int i = 0, max = groupDatas.Count; i < max;i++ )
        {
            RefreshNewFuncUIStatus(groupDatas[i]);
        }
    }

    /// <summary>
    /// 刷新新功能状态
    /// </summary>
    /// <param name="panelId">是否显示刷新单个panel</param>
    private void DoRefreshNewFuncOpenStaus(PanelID panelId = PanelID.None)
    {
        bool matchPanel = (panelId != PanelID.None);
        if (null != m_dic_funcOpenDatas)
        {
            var enumerator = m_dic_funcOpenDatas.GetEnumerator();
            while(enumerator.MoveNext())
            {
                if (matchPanel && enumerator.Current.Value.FuncCreateObjPathData.DependPanelId != panelId)
                {
                    continue;
                }
                RefreshNewFuncUIStatus(enumerator.Current.Key);
            }
        }
    }



    /// <summary>
    /// 读取新功能开启提示
    /// </summary>
    /// <param name="funcId"></param>
    private void OnNewFuncOpenRead(GuideDefine.FuncOpenShowData showData)
    {
        if (null == showData)
        {
            Engine.Utility.Log.Error("GuideManager->OnNewFuncOpenRead showdata null!");
            return;
        }
        //if (showData.FOT == GuideDefine.FuncOpenType.Base)
        //{
        //    CompleteNewFuncOpen(showData);
        //}else if (showData.FOT == GuideDefine.FuncOpenType.Skill)
        //{
        //    if (null != m_FuncCommonNoticesQ && null != m_FuncCommonNoticesQ.Peek())
        //    {
        //        GuideDefine.FuncOpenShowData cacheShowData = (GuideDefine.FuncOpenShowData)m_FuncCommonNoticesQ.Peek();
        //        if (cacheShowData.FuncOpenId ==  showData.FuncOpenId)
        //        {
        //            //取出第一个
        //            m_FuncCommonNoticesQ.Dequeue();
        //        }
        //    }
        //}
        DoNewFuncIconFlyEffect(showData);
    }

    private UINewFuncFlyGrid m_newFuncFlyGrid = null;
    public void DoNewFuncIconFlyEffect(GuideDefine.FuncOpenShowData showData)
    {
        CompleteNewFuncOpen(showData);
        Action<GuideDefine.FuncOpenShowData> onComplete = (data) =>
        {
            //执行下一个工作流
            Engine.Utility.EventEngine.Instance().DispatchEvent((int)Client.GameEventID.UIEVENTDOGUIDEWORKFLOW);
        };

        if (null == m_newFuncFlyGrid)
        {
            Transform flyTs = UIManager.GetObj(GridID.Uinewfuncflygrid);
            Util.AddChildToTarget(UIRootHelper.Instance.StretchTransRoot, flyTs);
            GameObject gObj = flyTs.gameObject;
            m_newFuncFlyGrid = gObj.GetComponent<UINewFuncFlyGrid>();
            if (null == m_newFuncFlyGrid)
            {
                m_newFuncFlyGrid = gObj.gameObject.AddComponent<UINewFuncFlyGrid>();
            }
            NGUITools.MarkParentAsChanged(gObj);
        }
        if (null != m_newFuncFlyGrid)
        {
            m_newFuncFlyGrid.StartFlyToTarget(showData, NewFuncFlyStartPos, onComplete);
        }
        else
        {
            onComplete.Invoke(showData);
        }


    }

    /// <summary>
    /// 是否该功能开启
    /// </summary>
    /// <param name="funcId"></param>
    /// <returns></returns>
    public bool IsNewFuncOpen(uint funcId)
    {
        GuideDefine.LocalFuncOpenData localOpenData = null;
        if (!TryGetFuncOpenData(funcId,out localOpenData))
        {
            return false;
        }

        if (null != m_lst_alreadyDoFuncOpen 
            && m_lst_alreadyDoFuncOpen.Contains(funcId) 
            && !IsMatchTriggerCondition(localOpenData.CloseTriggerId))
        {
            return true;
        }

        return false;
    }

    /// <summary>
    /// 执行新功能开启提示
    /// </summary>
    /// <param name="funcId"></param>
    private bool DoNewFuncOpen(GuideDefine.FuncOpenShowData showData)
    {
        m_currentFuncOpenShowData = showData;
        DataManager.Manager<UIPanelManager>().ShowPanel(PanelID.NewFuncOpenPanel, data: m_currentFuncOpenShowData);
        return true;
    }

    /// <summary>
    /// 是否有下一个功能开启引导
    /// </summary>
    /// <returns></returns>
    private bool HaveNextNewFuncOpen()
    {
        GuideDefine.FuncOpenShowData newData = null;
        if (TakeNextNewFuncOpen(out newData))
        {
            return true;
        }
        return false;
    }

    /// <summary>
    /// 是否新功能开启UI准备好
    /// </summary>
    /// <param name="isSkillOpen">是否为技能</param>
    /// <param name="newFuncOpenId"></param>
    /// <returns></returns>
    public bool IsNewFuncUIStatusReady(GuideDefine.FuncOpenShowData showData)
    {
        PanelID targetPanelId = PanelID.None;
        if (showData.FOT == GuideDefine.FuncOpenType.Skill)
        {
            targetPanelId = PanelID.MainPanel;
        }
        else if (showData.FOT == GuideDefine.FuncOpenType.Base)
        {
            GuideDefine.LocalFuncOpenData localdata = GuideDefine.LocalFuncOpenData.Create(showData.FuncOpenId);
            if (null != localdata)
            {
                targetPanelId = localdata.FlyToTargetDependPanel;
            }
        }
        if (targetPanelId == PanelID.None)
        {
            return false;
        }
        return DataManager.Manager<UIPanelManager>().IsPanelFocus(targetPanelId);
    }

    /// <summary>
    /// 执行下一个新功能开启
    /// </summary>
    private bool DoNextNewFuncOpen()
    {
        if (IsDoingNewFuncOpen || IsDoingGuide(GuideDefine.GuideType.Constraint))
        {
            return false;
        }
        GuideDefine.FuncOpenShowData newData = null;
        if (IsNextNewFuncOpenReady(out newData))
        {
            if (GuideManager.IsNewFunctionOpenNoticeEnable)
            {
                return DoNewFuncOpen(newData);
            }
            else
            {
                CompleteNewFuncOpen(newData);
                return false;
            }

        }
        return false;
    }

    /// <summary>
    /// 是否下一个新功能开启准备好了
    /// </summary>
    /// <returns></returns>
    private bool IsNextNewFuncOpenReady()
    {
        GuideDefine.FuncOpenShowData newData = null;
        return IsNextNewFuncOpenReady(out newData);
    }

    /// <summary>
    /// 是否下一个新功能开启准备好了
    /// </summary>
    /// <param name="data"></param>
    /// <returns></returns>
    private bool IsNextNewFuncOpenReady(out GuideDefine.FuncOpenShowData data)
    {
        if (TakeNextNewFuncOpen(out data) && IsNewFuncUIStatusReady(data))
        {
            return true;
        }
        return false;
    }

    /// <summary>
    /// 获取下一个新功能开启ID
    /// </summary>
    /// <param name="funcId"></param>
    /// <returns></returns>
    public bool TakeNextNewFuncOpen(out GuideDefine.FuncOpenShowData showData)
    {
        showData = null;
        if (null != m_FuncCommonNoticesQ
            && m_FuncCommonNoticesQ.Count != 0
            && null != m_FuncCommonNoticesQ.Peek())
        {
            showData = (GuideDefine.FuncOpenShowData)m_FuncCommonNoticesQ.Peek();
            return true;
        }
        else if (null != m_lst_canDoFuncOpen && m_lst_canDoFuncOpen.Count > 0)
        {
            GuideDefine.LocalFuncOpenData db = null;
            bool match = false;
            do
            {
                db = GuideDefine.LocalFuncOpenData.Create(m_lst_canDoFuncOpen[0]);
                showData = db.ShowData;
                if (!IsNeedIgnoreByCondi(db.IgnoreCondi)
                    && !m_lst_alreadyDoFuncOpen.Contains(db.FuncOpenId))
                {
                    match = true;
                }
                else
                {
                    CompleteNewFuncOpen(showData);
                }

            } while (!match && m_lst_canDoFuncOpen.Count > 0);

            if (match)
                return true;
        }

        return false;
    }

    /// <summary>
    /// 获取已经激活功能
    /// </summary>
    /// <returns></returns>
    public List<uint> GetActiveFuncId()
    {
        List<uint> activefuncId = new List<uint>();
        if (null != m_lst_alreadyDoFuncOpen)
        {
            GuideDefine.LocalFuncOpenData localData = null;
            for (int i = 0; i < m_lst_alreadyDoFuncOpen.Count; i++)
            {
                if (!m_dic_funcOpenDatas.TryGetValue(m_lst_alreadyDoFuncOpen[i], out localData))
                {
                    continue;
                }
                if (!activefuncId.Contains(localData.FuncOpenId))
                {
                    activefuncId.Add(localData.FuncOpenId);
                }
            }
        }
        return activefuncId;
    }

    /// <summary>
    /// 发送完成新功能提醒请求
    /// </summary>
    /// <param name="funcID"></param>
    public void SendNewFuncOpenComplete(uint funcID)
    {
        if (null != DataManager.Instance.Sender)
        {
            DataManager.Instance.Sender.SendNewFuncNoticeCompleteReq(funcID);
        }
    }

    /// <summary>
    /// 服务器下发已完成新功能ID
    /// </summary>
    /// <param name="completeIds"></param>
    public void OnNewFuncOpenInfoGet(List<uint> completeIds)
    {
        m_lst_alreadyDoFuncOpen.Clear();
        if (null != completeIds)
        {
            uint id = 0;
            GuideDefine.FuncOpenShowData showData = new GuideDefine.FuncOpenShowData()
            {
                FOT = GuideDefine.FuncOpenType.Base,
            };
            
            for (int i = 0; i < completeIds.Count; i++)
            {
                id = completeIds[i];
                if (m_lst_alreadyDoFuncOpen.Contains(id))
                {
                    continue;
                }
                showData.FuncOpenId = id;
                CompleteNewFuncOpen(showData);
            }
        }
        m_bNewFuncDataReady = true;
    }
    #endregion
}