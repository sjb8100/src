/************************************************************************************
 * Copyright (c) 2017  All Rights Reserved.
 * CLR版本： 4.0.30319.42000
 * 公司名称：ZQB
 * 命名空间：GameMain.UI.UIManager
 * 创建人：  wenjunhua.zqgame
 * 文件名：  UIPanelManager_Define
 * 版本号：  V1.0.0.0
 * 创建时间：5/9/2017 11:44:09 AM
 * 描述：
 ************************************************************************************/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

partial class UIPanelManager
{
    #region define
    /// <summary>
    /// 面板显示数据
    /// </summary>
    public class PanelShowData
    {
        public PanelID PID = PanelID.None;
        public UIPanelBase.PanelData PrePanelData = null;
        public object Data = null;
        public UIPanelBase.PanelJumpData JumpData = null;
        public bool IgnoreCache = false;
        public bool ForceClearCacheBack = false;
        public bool ForceResetPanel = true;
        public Action<UIPanelBase> PanelShowAction;
    }

    /// <summary>
    /// 面板焦点数据
    /// </summary>
    public class PanelFocusData
    {
        public PanelID ID = PanelID.None;
        public bool GetFocus = false;
    }


    /// <summary>
    /// 面板页签数据
    /// </summary>
    public class PanelTabData
    {
        /// <summary>
        /// 页签数据元
        /// </summary>
        public class PanelTabUnit
        {
            //排序ID
            public int SortIndex;
            //枚举值
            public int EnumValue;
            //名称
            public string EnumName;
            //创建的游戏对象名称
            public string ObjName;
            //位置索引
            public int PosIndex;
            //功能ID
            public int FuncID;

        }

        /// <summary>
        /// 是否可用
        /// </summary>
        public bool Enable
        {
            get
            {
                return Count > 0;
            }
        }

        private Dictionary<int, PanelTabUnit> m_dicTabUnitDatas = new Dictionary<int, PanelTabUnit>();
        public void Add(PanelTabUnit data)
        {
            if (!m_dicTabUnitDatas.ContainsKey(data.EnumValue))
            {
                m_dicTabUnitDatas.Add(data.EnumValue, data);
            }
        }

        public bool TryGetTabUnit(int enumId,out PanelTabUnit unit)
        {
            return m_dicTabUnitDatas.TryGetValue(enumId, out unit);
        }

        public List<PanelTabUnit> GetTabUnitList()
        {
            List<PanelTabUnit> datas = new List<PanelTabUnit>(m_dicTabUnitDatas.Values);
            datas.Sort((left, right) =>
            {
                return left.SortIndex - right.SortIndex;
            });
            return datas;
        }
        public int Count
        {
            get
            {
                return m_dicTabUnitDatas.Count;
            }
        }
    }

    //面板缓存类型
    public enum PanelCacheLv
    {
        None = 0,
        //低深度面板
        LowDepth = 1,
        //所有显示面板
        Dialog = 2,
        All = 3,
    }

    public class LocalPanelInfo
    {
        #region property
        //表格id
        private uint m_uint_id = 0;
        public uint ID
        {
            get
            {
                return m_uint_id;
            }
        }

        //表格数据
        public table.PanelInfoDataBase TableData
        {
            get
            {
                return GameTableManager.Instance.GetTableItem<table.PanelInfoDataBase>(ID);
            }
        }

        //顶部栏显示名称
        public string TitleName
        {
            get
            {
                return (null != TableData) ? TableData.titleName : "";
            }
        }

        //面板展开音效
        public uint SoundEffectId
        {
            get
            {
                return (null != TableData) ? TableData.showSoundEffectID : 0;
            }
        }
        public List<uint> TextIDList
        {
            get
            {
                if(TableData == null)
                {
                    return new List<uint>();
                }
                else
                {
                    return StringUtil.GetSplitStringList<uint>(TableData.tabTipsText, '|');
                }
            }
        }

        //是否有展开音效
        public bool HaveSoundEffect
        {
            get
            {
                return (SoundEffectId != 0);
            }
        }

        private uint[] dependenceRes = null;
        public uint [] DependenceRes
        {
            get
            {
                if (null == dependenceRes 
                    && null != TableData 
                    && !string.IsNullOrEmpty(TableData.dependenceRes))
                {
                    string[] dependenceArrayStr = TableData.dependenceRes.Split('|');
                    if (null != dependenceArrayStr)
                    {
                        int length = dependenceArrayStr.Length;
                        dependenceRes = new uint[length];
                        uint tempResID = 0;
                        for(int i=0;i < length;i++)
                        {
                            if (uint.TryParse(dependenceArrayStr[i],out tempResID))
                            {
                                dependenceRes[i] = tempResID;
                            }
                        }
                    }
                }
                return dependenceRes;
            }
        }

        //互斥面板
        public List<PanelID> MutexPanels
        {
            get
            {
                if (null != TableData && !string.IsNullOrEmpty(TableData.mutexPanelStr))
                {
                    List<PanelID> pIds = new List<PanelID>();
                    string[] panels = TableData.mutexPanelStr.Split(new char[] { '|' });
                    if (null != panels && panels.Length > 0)
                    {
                        string panelName = "";
                        for (int i = 0; i < panels.Length; i++)
                        {
                            if (string.IsNullOrEmpty(panels[i]))
                            {
                                continue;
                            }
                            panelName = panels[i].Trim();

                            if (Enum.IsDefined(typeof(PanelID), panelName))
                            {
                                pIds.Add((PanelID)Enum.Parse(typeof(PanelID), panelName));
                            }
                        }
                        return pIds;
                    }
                }
                return null;
            }
        }

        public string PanelEnumName
        {
            get
            {
                return (null != TableData) ? CMResourceDefine.LocalResourceData.Create(TableData.resID).ResName : "";
            }
        }

        /// <summary>
        /// 顶部栏元素
        /// </summary>
        public List<int> TopBarProperty
        {
            get
            {
                List<int> topBarProperty = new List<int>();
                if (null != TableData && NeedTopBar && !string.IsNullOrEmpty(TableData.titleElement))
                {
                    string[] elements = TableData.titleElement.Split(new char[] { '|' });
                    int parseInt = 0;
                    if (null != elements)
                    {
                        for (int i = 0; i < elements.Length; i++)
                        {
                            if (!int.TryParse(elements[i], out parseInt))
                            {
                                continue;
                            }
                            topBarProperty.Insert(0, parseInt);
                        }
                    }
                }
                return topBarProperty;
            }
        }

        //是否需要计算焦点
        public bool IsNeedCalculateFocus
        {
            get
            {
                return (null != TableData && TableData.caculateFocus == 0);
            }
        }

        private PanelID pid = PanelID.None;
        //面板ID
        public PanelID PID
        {
            get
            {
                if (pid == PanelID.None)
                {
                    table.PanelInfoDataBase tableData = TableData;
                    if (null != tableData
                        && !string.IsNullOrEmpty(tableData.panelIDEnumName)
                        && Enum.IsDefined(typeof(PanelID), tableData.panelIDEnumName))
                    {
                        pid = (PanelID)Enum.Parse(typeof(PanelID), tableData.panelIDEnumName);
                    }
                }
                return pid;
            }
        }

        //面板类型
        public PanelType PType
        {
            get
            {
                return (null != TableData) ? (PanelType)TableData.panelType : PanelType.PopUp;
            }
        }

        //是否忽略缓存
        public bool IgnoreCache
        {
            get
            {
                return (null != TableData && TableData.ignoreCache == 1);
            }
        }

        //面板显示时候缓存显示到堆栈的等级
        public PanelCacheLv CacheLv
        {
            get
            {
                return (null != TableData && TableData.cacheLv != 0) ? (PanelCacheLv)TableData.cacheLv : PanelCacheLv.None;
            }
        }

        

        //是否定义为首个面板
        public bool IsStartPanel
        {
            get
            {
                return (null != TableData && TableData.isStart == 1);
            }
        }


        //预制路径
        public string PrefabPath
        {
            get
            {
                return (null != TableData) ? CMResourceDefine.LocalResourceData.Create(TableData.resID).ResRelativePath : "";
            }
        }

        /// <summary>
        /// 是否需要公共背景
        /// </summary>
        public bool NeedBg
        {
            get
            {
                return (null != TableData && TableData.needBg == 1);
            }
        }

        /// <summary>
        /// 遮罩模式
        /// </summary>
        public UIDefine.UIPanelColliderMode CollideMode
        {
            get
            {
                return (null != TableData) ? (UIDefine.UIPanelColliderMode)TableData.collideMaskType : UIDefine.UIPanelColliderMode.None;
            }
        }

        /// <summary>
        /// 是否需要顶部栏
        /// </summary>
        public bool NeedTopBar
        {
            get
            {
                return (null != TableData && TableData.needTitle == 1);
            }
        }

        public uint ResID
        {
            get
            {
                return (null != TableData) ? TableData.resID : 0;
            }
        }
        private PanelTabData tabData = null;
        /// <summary>
        /// 面板页签数据
        /// </summary>
        public PanelTabData PanelTaData
        {
            get
            {
                if (null != tabData)
                {
                    return tabData;
                }
                table.PanelInfoDataBase tableData = TableData;
                tabData = new PanelTabData();
                if (null != tableData && tableData.tabNum > 0 && !string.IsNullOrEmpty(tableData.tabEnumValue))
                {
                    string[] enumValueArray = tableData.tabEnumValue.Split(new char[] { '|' });
                    if (null != enumValueArray && tableData.tabNum == enumValueArray.Length)
                    {
                        string[] enumNameArray = null;
                        if (!string.IsNullOrEmpty(tableData.tabName))
                        {
                            enumNameArray = tableData.tabName.Split(new char[] { '|' });
                        }

                        string[] tabPosIndexArray = null;
                        if (!string.IsNullOrEmpty(tableData.tabPosIndexValue))
                        {
                            tabPosIndexArray = tableData.tabPosIndexValue.Split(new char[] { '|' });
                        }

                        string[] enumObjNameArray = null;
                        if (!string.IsNullOrEmpty(tableData.tabObjName))
                        {
                            enumObjNameArray = tableData.tabObjName.Split(new char[] { '|' });
                        }

                        string[] tabFuncIDArray = null;
                        if (!string.IsNullOrEmpty(tableData.tabFuncIdValue))
                        {
                            tabFuncIDArray = tableData.tabFuncIdValue.Split(new char[] { '|' });
                        }
                        PanelTabData.PanelTabUnit tabUnit = null;

                        for (int i = 0; i < enumValueArray.Length; i++)
                        {
                            tabUnit = new PanelTabData.PanelTabUnit();
                            tabUnit.SortIndex = i;
                            if (!int.TryParse(enumValueArray[i], out tabUnit.EnumValue))
                            {
                                continue;
                            }
                            if (null != enumNameArray && enumNameArray.Length > i)
                            {
                                tabUnit.EnumName = enumNameArray[i];
                            }
                            if (null != enumObjNameArray && enumObjNameArray.Length > i)
                            {
                                tabUnit.ObjName = enumObjNameArray[i];
                            }

                            if (null != tabFuncIDArray && tabFuncIDArray.Length > i)
                            {
                                tabUnit.FuncID = int.Parse(tabFuncIDArray[i].Trim());
                            }

                            if (null != tabPosIndexArray && tabPosIndexArray.Length == enumValueArray.Length)
                            {
                                tabUnit.PosIndex = int.Parse(tabPosIndexArray[i].Trim());
                            }
                            else
                            {
                                tabUnit.PosIndex = i;
                            }
                            tabData.Add(tabUnit);
                        }
                    }
                }
                return tabData;
            }
        }

        //显示当前面板时，需要关闭的界面类型标识
        public int HidePanelMask
        {
            get
            {
                int mask = 0;
                table.PanelInfoDataBase tableData = TableData;
                if (null != tableData)
                {
                    if (tableData.mainMask == 1)
                        mask |= (1 << ((int)PanelType.Main));
                    if (tableData.normalMask == 1)
                        mask |= (1 << ((int)PanelType.Normal));
                    if (tableData.smartPopMask == 1)
                        mask |= (1 << ((int)PanelType.SmartPopUp));
                    if (tableData.popMask == 1)
                        mask |= (1 << ((int)PanelType.PopUp));
                    if (tableData.fixedMask == 1)
                        mask |= (1 << ((int)PanelType.Fixed));
                    if (tableData.commbgMask == 1)
                        mask |= (1 << ((int)PanelType.CommonBG));
                    if (tableData.mainMask == 1)
                        mask |= (1 << ((int)PanelType.MarQueen));
                    if (tableData.guideMask == 1)
                        mask |= (1 << ((int)PanelType.Guide));
                    if (tableData.newFuncMask == 1)
                        mask |= (1 << ((int)PanelType.NewFuncOpen));
                    if (tableData.loadMask == 1)
                        mask |= (1 << ((int)PanelType.Loading));
                    if (tableData.reconnectMask == 1)
                        mask |= (1 << ((int)PanelType.Reconnect));
                }
                return mask;
            }

        }

        /// <summary>
        /// pType是否需要在当前面板显示时关闭
        /// </summary>
        /// <param name="pType"></param>
        /// <returns></returns>
        public bool IsMatchHideType(PanelType pType)
        {
            return (HidePanelMask & (1 << ((int)pType))) != 0;
        }

        /// <summary>
        /// 当前面板是否可以缓存pid
        /// </summary>
        /// <param name="pType"></param>
        /// <returns></returns>
        public bool CanCachePanel(PanelID pid)
        {
            PanelCacheLv cacheLv = CacheLv;
            if (cacheLv == PanelCacheLv.None || IsStartPanel)
            {
                return false;
            }


            bool canCache = false;
            LocalPanelInfo localP2 = null;
            if (DataManager.Manager<UIPanelManager>().TryGetLocalPanelInfo(pid,out localP2) && !localP2.IgnoreCache)
            {
                if (cacheLv == PanelCacheLv.All)
                {
                    canCache =  true;
                }else
                {
                    PanelRootType r1 = UIRootHelper.Instance.GetRootTypePanelType(PType);
                    PanelRootType r2 = UIRootHelper.Instance.GetRootTypePanelType(localP2.PType);
                    canCache = (r1 >= r2);
                    if (!canCache && CacheLv == PanelCacheLv.Dialog 
                        && r2 == PanelRootType.PopUp)
                    {
                        canCache = true;
                    }
                }
                
            }
            return canCache;
        }

        #endregion

        #region Create

        

        private LocalPanelInfo(uint tableid)
        {
            this.m_uint_id = tableid;
        }

        public static LocalPanelInfo Create(uint tableid)
        {
            return new LocalPanelInfo(tableid);
        }
        #endregion
    }
    #endregion
}