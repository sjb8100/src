/************************************************************************************
 * Copyright (c) 2016  All Rights Reserved.
 * CLR版本： 4.0.30319.42000
 * 公司名称：ZQB
 * 命名空间：GameMain.Game.Muhon
 * 创建人：  wenjunhua.zqgame
 * 文件名：  MuhonPanel_Evolve
 * 版本号：  V1.0.0.0
 * 创建时间：10/15/2016 12:19:06 PM
 * 描述：
 ************************************************************************************/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

partial class MuhonPanel
{
    #region define
    public class MuhonStarData
    {
        private EquipDefine.AttrIndex attrIndex = EquipDefine.AttrIndex.None;
        public EquipDefine.AttrIndex AIndex
        {
            get
            {
                return attrIndex;
            }
        }
        private UIToggle starToggle = null;
        private TweenAlpha starTween = null;
        private bool isOpen = false;
        public bool Open
        {
            get
            {
                return isOpen;
            }
        }

        public void SetStatus(bool open)
        {
            isOpen = open;
            starToggle.value = open;
        }

        public void PlayStarAnim(bool start)
        {
            if (null != starTween && starTween.enabled != start)
            {
                starTween.enabled = start;
            }
            if (!start)
            {
                starToggle.value = isOpen;
                starToggle.activeSprite.alpha = (isOpen) ? 1f : 0;
            }
        }

        private  MuhonStarData( UIToggle toggle,EquipDefine.AttrIndex index)
        {
            starTween = toggle.activeSprite.GetComponent<TweenAlpha>();
            attrIndex = index;
            this.starToggle = toggle;
        }

        public Transform Ts
        {
            get
            {
                if (null != starToggle)
                {
                    return starToggle.gameObject.transform;
                }
                return null;
            }
        }

        public static MuhonStarData Create(UIToggle toggle,EquipDefine.AttrIndex index)
        {
            return new MuhonStarData(toggle, index); 
        }
    }

    #endregion

    #region property
    private UIItemShowGrid m_evolveCurGrow = null;
    
    private UIItemGrowShowGrid m_evolveMaxGrow = null;
    private Dictionary<EquipDefine.AttrIndex, MuhonStarData> m_dicStarData = null;
    private Dictionary<EquipDefine.AttrIndex, UIItemShowGrid> m_dicMuhonDeputy = null;
    private Dictionary<EquipDefine.AttrIndex, uint> m_dicSelectMuhonDeputy = null;

    private Dictionary<EquipDefine.AttrIndex, UIParticleWidget> m_dicParticleWidgets = null;
    //星星爆点组件
    private UIParticleWidget m_evolveStarBoomParticleWidget = null;
    //进化目标爆点组件
    private UIParticleWidget m_evolveTargetParticleWidget = null;
    #endregion

    #region Op

    private bool OnEvolveLogicBack()
    {
        return SetEvolvePre(false);
    }
    private bool IsSelectMuhonDeputySuccess()
    {
        bool success = false;
        if (null != Data
            && Data.EvolveNeedMuhonNum == m_dicSelectMuhonDeputy.Count)
        {
            success = true;
        }
        return success;
    }

    /// <summary>
    /// 是否选中muhonQwID作为进化副装备
    /// </summary>
    /// <param name="muhonQwID">武魂唯一ID</param>
    /// <param name="index"></param>
    /// <returns></returns>
    private bool IsChooseMuhonDeputy(uint muhonQwID,out EquipDefine.AttrIndex index)
    {
        var enumerator = m_dicSelectMuhonDeputy.GetEnumerator();
        index = EquipDefine.AttrIndex.None;
        while(enumerator.MoveNext())
        {
            if (enumerator.Current.Value == muhonQwID)
            {
                index = enumerator.Current.Key;
                return true;
            }
        }
        return false;
    }

    /// <summary>
    /// 获取可填充的空格子
    /// </summary>
    /// <param name="index"></param>
    /// <returns></returns>
    private bool TryGetEmptyDeputyMuhonIndex(out EquipDefine.AttrIndex index)
    {

        index = EquipDefine.AttrIndex.None;
        Muhon data = Data;
        if (null != data)
        {
            int needNum = data.EvolveNeedMuhonNum;
            for(EquipDefine.AttrIndex i = EquipDefine.AttrIndex.None + 1;i < EquipDefine.AttrIndex.Max;i ++)
            {
               if (((int)i) > needNum)
               {
                   break;
               }
               if (IsDeputyEvolveGridFill(i))
               {
                   index = i;
                   return true;
               }
            }
        }
        return false;
    }
    /// <summary>
    /// 进化完成
    /// </summary>
    /// <param name="muhonId"></param>
    void OnMuhonEvolution(uint muhonId)
    {
        //播放动画
        ResetEvolve();
        if (IsStatus(TabMode.JinHua))
        {
            m_bEvolveAnimEnable = true;
            PlayMuhonEvolveCompleteAnim(muhonId, () =>
            {
                ResetEvolveAnim();
                DataManager.Manager<UIPanelManager>().ShowPanel(PanelID.MuhonEvolveCompletePanel, data: muhonId);
                UpdateEvolve(Data);
            });
        }
        
    }

    #region EvolveCompleteAnim (播放进化完成动画)
    
    /// <summary>
    /// 执行进化动画步骤
    /// </summary>
    /// <param name="step"></param>
    /// <param name="num"></param>
    /// <param name="complete"></param>
    private void DoMuhonEvolveSteps(EquipManager.EvolveAnimStep step, uint muhonBaseId, Action<EquipManager.EvolveAnimStep> complete)
    {
        if (!IsInitStatus(TabMode.JinHua))
            return;
        Muhon cur = new Muhon(muhonBaseId);
        if (null == cur || null == cur.Pre)
        {
            return;
        }
        Muhon pre = cur.Pre;
        UIParticleWidget tempParticleWidget = null;
        int pos = 0;
        uint effectId = emgr.GetEvloveAnimEffectIdByStep(step);
        Engine.EffectCallback onEvolveEffectComplete = (strEffectName, strEvent, param) =>
            {
                if (null != complete)
                {
                    complete.Invoke(step);
                }
            };
        int num = pre.EvolveNeedMuhonNum;
        switch (step)
        {
            case EquipManager.EvolveAnimStep.MaterialBorderBoom:
            case EquipManager.EvolveAnimStep.MaterialFlyEffect:
            case EquipManager.EvolveAnimStep.FlyToTarget:
                {
                    //星级显示
                    for (EquipDefine.AttrIndex i = EquipDefine.AttrIndex.None + 1; i < EquipDefine.AttrIndex.Max; i++)
                    {
                        if (!m_dicParticleWidgets.TryGetValue(i, out tempParticleWidget) || null == tempParticleWidget)
                        {
                            continue;
                        }
                        //tempParticleWidget.ReleaseParticle();
                        if (step == EquipManager.EvolveAnimStep.MaterialBorderBoom)
                        {
                            tempParticleWidget.cachedTransform.localPosition = Vector3.zero;
                        }
                        pos = (int)i;
                        if (pos <= num)
                        {
                            if (step == EquipManager.EvolveAnimStep.FlyToTarget)
                            {
                                TweenPosition tp = tempParticleWidget.GetComponent<TweenPosition>();
                                if (null == tp)
                                {
                                    continue;
                                }
                                tp.ResetToBeginning();
                                tp.onFinished.Clear();
                                if (pos == num)
                                {
                                    EventDelegate.Callback finish = () =>
                                        {
                                            if (null != complete)
                                            {
                                                complete.Invoke(step);
                                            }
                                        };
                                    EventDelegate d = new EventDelegate(finish);
                                    tp.onFinished.Add(d);
                                }
                                tp.Play(true);
                            }
                            else
                            {
                                if (pos != num)
                                {
                                    tempParticleWidget.AddParticle(effectId);
                                }
                                else
                                {
                                    if (step == EquipManager.EvolveAnimStep.MaterialFlyEffect)
                                    {
                                        tempParticleWidget.AddParticle(effectId, (effct) =>
                                        {
                                            if (null != complete)
                                            {
                                                complete.Invoke(step);
                                            }
                                        });
                                    }
                                    else
                                    {
                                        tempParticleWidget.AddParticle(effectId, null, endCallback: onEvolveEffectComplete);
                                    }

                                }
                            }
                        }
                    }
                }
                break;
            case EquipManager.EvolveAnimStep.TargetBoom:
                {
                    if (null != m_evolveTargetParticleWidget)
                    {

                        m_evolveTargetParticleWidget.AddParticle(effectId, endCallback: onEvolveEffectComplete);
                    }
                    else if (null != complete)
                    {
                        complete.Invoke(step);
                    }
                }
                break;
            case EquipManager.EvolveAnimStep.StarLight:
                {
                    if (null != m_evolveStarBoomParticleWidget)
                    {
                        Engine.EffectCallback onStarLightComplete = (strEffectName, strEvent, param) =>
                        {
                            if (null != complete)
                            {
                                complete.Invoke(step);
                            }
                        };
                        Transform startTran = GetStarTransByStarLv(cur.StartLevel);
                        if (null != startTran)
                        {
                            m_evolveStarBoomParticleWidget.cachedTransform.localPosition = startTran.localPosition;
                        }
                        
                        m_evolveStarBoomParticleWidget.AddParticle(effectId, endCallback: onEvolveEffectComplete);
                    }
                    else if (null != complete)
                    {
                        complete.Invoke(step);
                    }
                }
                break;
        }
    }

    private Transform GetStarTransByStarLv(uint startLv)
    {
        if (startLv == 0)
            return null;
        if (startLv >= (uint)EquipDefine.AttrIndex.First && startLv < (uint)EquipDefine.AttrIndex.Max)
        {
            EquipDefine.AttrIndex index = (EquipDefine.AttrIndex)startLv;
            MuhonStarData starData = null;
            if (m_dicStarData.TryGetValue(index,out starData))
            {
                return starData.Ts;
            }
        }
        return null;

    }

    //进化动画是否可以播放
    private bool m_bEvolveAnimEnable = false;

    public bool EvolveAnimEnable
    {
        get
        {
            return IsStatus(TabMode.JinHua) && m_bEvolveAnimEnable;
        }
    }

    public void ResetEvolveAnim()
    {
        m_bEvolveAnimEnable = false;
        //进化动画组件重置
        if (null != m_evolveStarBoomParticleWidget)
        {
            m_evolveStarBoomParticleWidget.ReleaseParticle();
        }
        if (null != m_evolveTargetParticleWidget)
        {
            m_evolveTargetParticleWidget.ReleaseParticle();
        }
        if (null != m_dicParticleWidgets)
        {
            var iemurator = m_dicParticleWidgets.GetEnumerator();
            while (iemurator.MoveNext())
            {
                if (null == iemurator.Current.Value)
                    continue;
                iemurator.Current.Value.ReleaseParticle();
            }
        }
    }

   

    /// <summary>
    /// 播放进化完成动画
    /// </summary>
    /// <param name="completeCallback"></param>
    private void PlayMuhonEvolveCompleteAnim(uint muhonQwThisId,Action completeCallback)
    {
        if (!EvolveAnimEnable)
        {
            return;
        }
        Muhon muhon = imgr.GetBaseItemByQwThisId<Muhon>(muhonQwThisId);
        if (null == muhon)
        {
            Engine.Utility.Log.Warning("MuhonPanel_Evolve->PlayMuhonEvolutionAnim data null");
            return;
        }
        //Muhon pre = muhon.Pre;
        //if (null == pre)
        //{
        //    Engine.Utility.Log.Warning("MuhonPanel_Evolve->PlayMuhonEvolutionAnim pre muhon null muhonBaseId:{0}", muhon.BaseId);
        //    return;
        //}

        uint baseID = muhon.BaseId;
        Action<EquipManager.EvolveAnimStep> animEnd = (step) =>
        {
            if (!EvolveAnimEnable)
            {
                return;
            }
            if (null != completeCallback)
            {
                completeCallback.Invoke();
            }
        };

        Action<EquipManager.EvolveAnimStep> stepTB = (step) =>
        {
            if (!EvolveAnimEnable)
            {
                return;
            }
            DoMuhonEvolveSteps(EquipManager.EvolveAnimStep.StarLight, baseID, animEnd);
        };

        Action<EquipManager.EvolveAnimStep> stepFTT = (step) =>
        {
            if (!EvolveAnimEnable)
            {
                return;
            }
            DoMuhonEvolveSteps(EquipManager.EvolveAnimStep.TargetBoom, baseID, stepTB);
        };

        Action<EquipManager.EvolveAnimStep> stepMFE = (step) =>
        {
            if (!EvolveAnimEnable)
            {
                return;
            }
            DoMuhonEvolveSteps(EquipManager.EvolveAnimStep.FlyToTarget, baseID, stepFTT);
        };

        Action<EquipManager.EvolveAnimStep> stepMBB = (step) =>
        {
            if (!EvolveAnimEnable)
            {
                return;
            }
            DoMuhonEvolveSteps(EquipManager.EvolveAnimStep.MaterialFlyEffect, baseID, stepMFE);
        };

        DoMuhonEvolveSteps(EquipManager.EvolveAnimStep.MaterialBorderBoom, baseID, stepMBB);

    }
    #endregion

    
    //重置
    private void ResetEvolve()
    {
        if (IsInitStatus(TabMode.JinHua))
            m_dicSelectMuhonDeputy.Clear();
        ResetEvolveAnim();
    }
    private void InitEvolveWidgets()
    {
        if (IsInitStatus(TabMode.JinHua))
        {
            return;
        }
        SetInitStatus(TabMode.JinHua, true);

        if (null != m_widget_PreviewCollider)
        {
            UIEventListener.Get(m_widget_PreviewCollider.gameObject).onClick = (preViewObj) =>
                {
                    SetEvolvePre(false);
                };
        }
        
        Transform clone = null;
        if (null != m_trans_EvolveGrowRoot && null == m_evolveCurGrow)
        {
            clone = UIManager.AddGridTransform(GridID.Uiitemshowgrid, m_trans_EvolveGrowRoot);
            if (null != clone)
            {
                m_evolveCurGrow = clone.GetComponent<UIItemShowGrid>();
                if (null == m_evolveCurGrow)
                    m_evolveCurGrow = clone.gameObject.AddComponent<UIItemShowGrid>();
            }
        }
        
        m_dicStarData = new Dictionary<EquipDefine.AttrIndex, MuhonStarData>();
        m_dicMuhonDeputy = new Dictionary<EquipDefine.AttrIndex, UIItemShowGrid>();

        Transform tempTrans = null;
        UIToggle tempToggle = null;
        string indexStr = "";
        UIItemShowGrid tempShowGrid = null;

        UIEventDelegate deputyAction = (eventType,data,param)=>
            {
                if (eventType == UIEventType.Click && data is UIItemShowGrid)
                {
                    UIItemShowGrid showGrid = data as UIItemShowGrid;
                    EquipDefine.AttrIndex index = EquipDefine.AttrIndex.None;
                    if (TryGetEvolveGridAttrIndex(showGrid.CacheTransform.gameObject,out index))
                    {
                        if (null != param && param is bool)
                        {
                            OnUnloadEvolveMuhon(index);
                        }
                        else if (!IsDeputyEvolveGridFill(index))
                        {
                            OnSelectEvolveMuhon(index);
                        }
                    }
                }
            };
        m_dicParticleWidgets = new Dictionary<EquipDefine.AttrIndex, UIParticleWidget>();
        UIParticleWidget tempParticleWidget = null;
        Transform tempParticleTran = null;
        //星星爆点组件初始化
        if (null == m_evolveStarBoomParticleWidget && null != m_trans_EvolveStarLightParticle)
        {
            m_evolveStarBoomParticleWidget = m_trans_EvolveStarLightParticle.GetComponent<UIParticleWidget>();
            if (null == m_evolveStarBoomParticleWidget)
            {
                m_evolveStarBoomParticleWidget = m_trans_EvolveStarLightParticle.gameObject.AddComponent<UIParticleWidget>();
            }
            if (null != m_evolveStarBoomParticleWidget && null != m_label_EvolveEffetDepthLimitMask)
            {
                m_evolveStarBoomParticleWidget.depth = m_label_EvolveEffetDepthLimitMask.depth - 1;
            }
        }

        //进化目标组件爆点初始化
        if (null == m_evolveTargetParticleWidget && null != m_trans_EvolveTargeParticle)
        {
            m_evolveTargetParticleWidget = m_trans_EvolveTargeParticle.GetComponent<UIParticleWidget>();
            if (null == m_evolveTargetParticleWidget)
            {
                m_evolveTargetParticleWidget = m_trans_EvolveTargeParticle.gameObject.AddComponent<UIParticleWidget>();
            }
            if (null != m_evolveTargetParticleWidget && null != m_label_EvolveEffetDepthLimitMask)
            {
                m_evolveTargetParticleWidget.depth = m_label_EvolveEffetDepthLimitMask.depth - 1;
            }
        }

        for (EquipDefine.AttrIndex i = EquipDefine.AttrIndex.None + 1; i < EquipDefine.AttrIndex.Max; i++)
        {
            if (!Enum.IsDefined(typeof(EquipDefine.AttrIndex), i)) //|| i == EquipDefine.AttrIndex.Fifth)
                continue;
            indexStr = ((int)i).ToString();
            if (null != m_trans_CostEvolveStar)
            {
                tempTrans = m_trans_CostEvolveStar.Find(indexStr);
                if (null != tempTrans)
                {
                    tempToggle = tempTrans.GetComponent<UIToggle>();
                    if (null != tempToggle)
                    {
                        m_dicStarData.Add(i, MuhonStarData.Create(tempToggle, i));
                    }
                }
            }

            if (null != m_trans_CostEvolveMuhon)
            {
                if (null != tempTrans)
                {
                    tempTrans = m_trans_CostEvolveMuhon.Find(indexStr);
                    tempParticleTran = tempTrans.Find(indexStr);
                    if (null != tempParticleTran)
                    {
                        tempParticleWidget = tempParticleTran.GetComponent<UIParticleWidget>();
                        if (null == tempParticleWidget)
                        {
                            tempParticleWidget = tempParticleTran.gameObject.AddComponent<UIParticleWidget>();
                        }
                        if (null != tempParticleWidget)
                        {
                            if (null != m_label_EvolveEffetDepthLimitMask)
                            {
                                tempParticleWidget.depth = m_label_EvolveEffetDepthLimitMask.depth - 1;
                            }
                            if (!m_dicParticleWidgets.ContainsKey(i))
                            {
                                m_dicParticleWidgets.Add(i, tempParticleWidget);
                            }
                        }
                    }
                    

                    clone = UIManager.AddGridTransform(GridID.Uiitemshowgrid, tempTrans);
                    tempShowGrid = clone.GetComponent<UIItemShowGrid>();
                    if (null == tempShowGrid)
                        tempShowGrid = clone.gameObject.AddComponent<UIItemShowGrid>();
                    if (null != tempShowGrid)
                    {
                        tempShowGrid.RegisterUIEventDelegate(deputyAction);
                        m_dicMuhonDeputy.Add(i, tempShowGrid);
                    }
                    
                }
            }
        }

        if (null != m_trans_EvolveMaxGrowRoot && null == m_evolveMaxGrow)
        {
            clone = UIManager.AddGridTransform(GridID.Uiitemgrowshowgrid,m_trans_EvolveMaxGrowRoot);
            if (null != clone)
            {
                m_evolveMaxGrow = clone.GetComponent<UIItemGrowShowGrid>();
                if (null == m_evolveMaxGrow)
                    m_evolveMaxGrow = clone.gameObject.AddComponent<UIItemGrowShowGrid>();
            }
        }

        m_dicSelectMuhonDeputy = new Dictionary<EquipDefine.AttrIndex, uint>();
    }

    /// <summary>
    /// 初始化数据
    /// </summary>
    public void InitEvolveData()
    {

    }

    private void RemoveEvolveDeputyItem(uint id)
    {
        EquipDefine.AttrIndex index = EquipDefine.AttrIndex.None;
        if (null != m_dicSelectMuhonDeputy && IsChooseMuhonDeputy(id,out index))
        {
            m_dicSelectMuhonDeputy.Remove(index);
        }
    }

    /// <summary>
    /// 尝试获取格子对象对应的索引
    /// </summary>
    /// <param name="obj"></param>
    /// <param name="index"></param>
    /// <returns></returns>
    private bool TryGetEvolveGridAttrIndex(GameObject obj,out EquipDefine.AttrIndex index)
    {
        index = EquipDefine.AttrIndex.None;
        if (null == obj)
            return false;
        Transform parent = obj.transform.parent;
        int attrIndex = 0;
        if (null != parent && int.TryParse(parent.name,out attrIndex))
        {
            index = (EquipDefine.AttrIndex)attrIndex;
            return true;
        }
        return false;
    }

    /// <summary>
    /// 是否副武魂格子已经填充数据
    /// </summary>
    /// <param name="index"></param>
    /// <returns></returns>
    private bool IsDeputyEvolveGridFill(EquipDefine.AttrIndex index)
    {
        return m_dicSelectMuhonDeputy.ContainsKey(index);
    }
    /// <summary>
    /// 卸载副武魂
    /// </summary>
    /// <param name="index"></param>
    private void OnUnloadEvolveMuhon(EquipDefine.AttrIndex index)
    {
        if (m_dicSelectMuhonDeputy.ContainsKey(index))
        {
            m_dicSelectMuhonDeputy.Remove(index);
            UIItemShowGrid showGrid = null;
            if (m_dicMuhonDeputy.TryGetValue(index,out showGrid))
            {
                showGrid.SetGridData(0, false);
            }
            UpdateEvolveSelectStarAnim();
        }
    }

    /// <summary>
    /// 选择圣魂操作
    /// </summary>
    private void OnSelectEvolveMuhon(EquipDefine.AttrIndex attrIndex)
    {
        if (DataManager.Manager<UIPanelManager>().IsShowPanel(PanelID.ItemChoosePanel))
            return;

        Muhon currentData = Data;
        if (null == currentData)
        {
            TipsManager.Instance.ShowTips(LocalTextType.Local_TXT_Soul_ChooseEvolveMuhonTips);
            return;
        }

        if (!currentData.IsMaxLv)
        {
            TipsManager.Instance.ShowTips(LocalTextType.Local_TXT_Soul_MuhonNeedMaxLvTips);
            return;
        }
        //筛选满足条件的圣魂
        List<uint> data = new List<uint>();
        List<uint> filterTypes = new List<uint>();
        filterTypes.Add((uint)GameCmd.EquipType.EquipType_SoulOne);
        List<uint> muhonList = imgr.GetItemByType(GameCmd.ItemBaseType.ItemBaseType_Equip, filterTypes, GameCmd.PACKAGETYPE.PACKAGETYPE_MAIN);
        //过滤
        if (null != muhonList && muhonList.Count != 0)
        {
            for (int i = 0; i < muhonList.Count; i++)
            {
                if (currentData.IsMatchEvolve(muhonList[i], false))
                {
                    data.Add(muhonList[i]);
                }
            }
        }
        if (data.Count == 0)
        {
            TipsManager.Instance.ShowTips(tmgr.GetLocalFormatText(LocalTextType.Soul_Star_1
                ,tmgr.GetMuhonStarName(currentData.StartLevel)));
            return;
        }

        data.Sort((left, right) =>
        {
            Muhon leftM = imgr.GetBaseItemByQwThisId<Muhon>(left);
            Muhon rightM = imgr.GetBaseItemByQwThisId<Muhon>(right);
            if (currentData.IsMatchEvolve(rightM, true))
            {
                if (currentData.IsMatchEvolve(leftM, true))
                {
                    return rightM.Level - leftM.Level;
                }
                return 1;
            }
            else if (currentData.IsMatchEvolve(leftM, true))
            {
                return -1;
            }
            else
            {
                return rightM.Level - leftM.Level;
            }
            return 0;
        });

        
        

        Dictionary<EquipDefine.AttrIndex, uint> curSelectDeputyMuhon = new Dictionary<EquipDefine.AttrIndex, uint>();
        Dictionary<uint, EquipDefine.AttrIndex> curSelectDeputyMuhonRS = new Dictionary<uint, EquipDefine.AttrIndex>();
        List<uint> selectDeputyIds = new List<uint>();
        var enumerator = m_dicSelectMuhonDeputy.GetEnumerator();
        while(enumerator.MoveNext())
        {
            curSelectDeputyMuhon.Add( enumerator.Current.Key,enumerator.Current.Value);
            curSelectDeputyMuhonRS.Add(enumerator.Current.Value, enumerator.Current.Key);
        }

        UILabel desLabel = null;
        bool needRefreshLabel = false;
        Action<UILabel> desLabAction = (des) =>
        {
            desLabel = des;
            if (null != desLabel)
                desLabel.text = tmgr.GetLocalFormatText(LocalTextType.Local_TXT_Soul_ChooseNum,
                    curSelectDeputyMuhon.Count, currentData.EvolveNeedMuhonNum);
        };

        UIEventDelegate eventDlg = (eventTye, grid, param) =>
        {
            if (eventTye == UIEventType.Click)
            {
                UIWeaponSoulInfoSelectGrid tempGrid = grid as UIWeaponSoulInfoSelectGrid;
                if (curSelectDeputyMuhonRS.ContainsKey(tempGrid.QWThisId))
                {
                    curSelectDeputyMuhon.Remove(curSelectDeputyMuhonRS[tempGrid.QWThisId]);
                    curSelectDeputyMuhonRS.Remove(tempGrid.QWThisId);
                    tempGrid.SetHightLight(false);
                }
                else if (currentData.IsMatchEvolve(tempGrid.QWThisId))
                {
                    needRefreshLabel = true;
                    if (curSelectDeputyMuhon.Count < currentData.EvolveNeedMuhonNum)
                    {
                        if (!curSelectDeputyMuhon.ContainsKey(attrIndex))
                        {
                            curSelectDeputyMuhonRS.Add( tempGrid.QWThisId,attrIndex);
                            curSelectDeputyMuhon.Add(attrIndex, tempGrid.QWThisId);
                        }else
                        {
                            for (EquipDefine.AttrIndex i = EquipDefine.AttrIndex.None + 1; i < EquipDefine.AttrIndex.Max; i++)
                            {
                                if (!Enum.IsDefined(typeof(EquipDefine.AttrIndex), i))
                                {
                                    continue;
                                }
                                if (((int) i) > currentData.EvolveNeedMuhonNum)
                                {
                                    break;
                                }
                                if (curSelectDeputyMuhon.ContainsKey(i))
                                {
                                    continue;
                                }

                                curSelectDeputyMuhonRS.Add(tempGrid.QWThisId, i);
                                curSelectDeputyMuhon.Add(i, tempGrid.QWThisId);
                                break;

                            }
                        }
                        tempGrid.SetHightLight(true);
                    }
                }else
                {

                    Muhon selectMuhon = imgr.GetBaseItemByQwThisId<Muhon>(tempGrid.QWThisId);
                    if (null != selectedMuhonId)
                    {
                        string txt = "";
                        if (selectMuhon.IsActive && !selectMuhon.IsMaxLv)
                        {
                            txt = tmgr.GetLocalText(LocalTextType.Local_TXT_Soul_NotMatchLvTips);
                        }
                        else if (!selectMuhon.IsActive && selectMuhon.IsMaxLv)
                        {
                            txt = tmgr.GetLocalText(LocalTextType.Local_TXT_Soul_UnactiveNotice);
                        }
                        else if (!selectMuhon.IsActive && !selectMuhon.IsMaxLv)
                        {
                            txt = tmgr.GetLocalText(LocalTextType.Local_TXT_Soul_NotMatchLvActiveTips);
                        }
                        TipsManager.Instance.ShowTips(txt);
                    }
                }

                if (null != desLabel && needRefreshLabel)
                {
                    desLabel.text = tmgr.GetLocalFormatText(LocalTextType.Local_TXT_Soul_ChooseNum,
                        curSelectDeputyMuhon.Count, currentData.EvolveNeedMuhonNum);
                }

            }
        };


        ItemChoosePanel.ItemChooseShowData showData = new ItemChoosePanel.ItemChooseShowData()
        {
            createNum = data.Count,
            title = tmgr.GetLocalText(LocalTextType.Local_TXT_Soul_ChooseMuhon),
            cloneTemp = UIManager.GetResGameObj(GridID.Uiweaponsoulinfoselectgrid) as GameObject,
            onChooseCallback = () =>
            {
                var enumeratorSelect = curSelectDeputyMuhon.GetEnumerator();
                m_dicSelectMuhonDeputy.Clear();
                while (enumeratorSelect.MoveNext())
                {
                    m_dicSelectMuhonDeputy.Add(enumeratorSelect.Current.Key, enumeratorSelect.Current.Value);
                }
                UpdateEvolve(Data);
            },
            gridCreateCallback = (obj, index) =>
            {
                if (index >= data.Count)
                {
                    Engine.Utility.Log.Error("OnSelectInvokeEquip error,index out of range");
                }
                else
                {
                    Muhon itemData = imgr.GetBaseItemByQwThisId<Muhon>(data[index]);
                    if (null == itemData)
                    {
                        Engine.Utility.Log.Error("OnSelectInvokeEquip error,get itemData qwThisId:{0}", data[index]);
                    }
                    else
                    {
                        UIWeaponSoulInfoSelectGrid weaponsoulGrid = obj.GetComponent<UIWeaponSoulInfoSelectGrid>();
                        if (null == weaponsoulGrid)
                            weaponsoulGrid = obj.AddComponent<UIWeaponSoulInfoSelectGrid>();
                        weaponsoulGrid.RegisterUIEventDelegate(eventDlg);
                        weaponsoulGrid.SetGridViewData(itemData.QWThisID, curSelectDeputyMuhonRS.ContainsKey(data[index])
                            , currentData.IsMatchEvolve(data[index]), false);
                    }
                }

            },
            desPassCallback = desLabAction,
        };
        DataManager.Manager<UIPanelManager>().ShowPanel(PanelID.ItemChoosePanel, data: showData);
    }

    /// <summary>
    /// 进化辅助
    /// </summary>
    private void SetEvolveAssit()
    {
        Muhon data = Data;
        if (null == data || data.IsMaxStarLv)
            return;
        SetCommonCost(data.EvolveNeedItemId, data.EvolveNeedItemNum,data.EvolveCost, GameCmd.MoneyType.MoneyType_Gold
            , data.EvolveNeedItemNum * DataManager.Manager<MallManager>().GetDQPriceByItem(data.EvolveNeedItemId)
            , GameCmd.MoneyType.MoneyType_Coin);

    }

    private void UpdateEvolveSelectStarAnim()
    {
        if (null != Data)
        {
            MuhonStarData starData = null;
            //选择完成动画
            EquipDefine.AttrIndex nextAttrIndex = (EquipDefine.AttrIndex)(Data.StartLevel + 1);
            for (EquipDefine.AttrIndex i = EquipDefine.AttrIndex.None + 1; i < EquipDefine.AttrIndex.Max;i ++ )
            {
                if (m_dicStarData.TryGetValue(i, out starData))
                {
                    if (i < nextAttrIndex)
                    {
                        starData.SetStatus(true);
                    }
                    else
                    {
                        starData.SetStatus(false);
                    }
                    if (i == nextAttrIndex)
                    {
                        starData.PlayStarAnim(IsSelectMuhonDeputySuccess());
                    }else
                    {
                        starData.PlayStarAnim(false);
                    }
                    
                }
            }
        }
        
    }

    private void UpdateEvolve(Muhon data)
    {
        if (null == data)
            return;
        bool isMaxStarLv = data.IsMaxStarLv;
        if (null != m_trans_EvolveInfos 
            && m_trans_EvolveInfos.gameObject.activeSelf == isMaxStarLv)
        {
            m_trans_EvolveInfos.gameObject.SetActive(!isMaxStarLv);
        }
        if (!isMaxStarLv)
        {
            if (null != m_evolveCurGrow)
            {
                m_evolveCurGrow.SetGridData(data.QWThisID);
            }
            MuhonStarData starData = null;

            UIItemShowGrid showGrid = null;
            //星级显示
             for (EquipDefine.AttrIndex i = EquipDefine.AttrIndex.None + 1; i < EquipDefine.AttrIndex.Max; i++)
             {
                 if (m_dicStarData.TryGetValue(i,out starData))
                 {
                     starData.SetStatus(data.StartLevel >= (int)i);
                 }

                 if (m_dicMuhonDeputy.TryGetValue(i, out showGrid) && null != showGrid)
                 {
                     if ((int)i > data.EvolveNeedMuhonNum)
                     {
                         showGrid.SetVisible(false);
                     }else 
                     {
                         if (!showGrid.Visible)
                         {
                             showGrid.SetVisible(true);
                         }

                         if (IsDeputyEvolveGridFill(i))
                         {
                             showGrid.SetGridData(m_dicSelectMuhonDeputy[i], true);
                         }else
                         {
                             showGrid.SetGridData(0);
                         }
                     }
                 }
             }
             //辅助物品


            UpdateEvolveSelectStarAnim();
            Muhon next = data.Next;
            //预览
            //星级
            if (null != m_slider_EvolveCurStarLv)
            {
                m_slider_EvolveCurStarLv.value = data.StartLevel / 5f;
            }
            if (null != m_slider_EvolveNextStarLv)
            {
                m_slider_EvolveNextStarLv.value = next.StartLevel / 5f;
            }
            //等级
            if (null != m_label_EvolveCurLv)
            {
                m_label_EvolveCurLv.text = tmgr.GetLocalFormatText(LocalTextType.Local_Txt_Set_4, data.MaxLv);
            }
            if (null != m_label_EvolveNextLv)
            {
                m_label_EvolveNextLv.text = tmgr.GetLocalFormatText(LocalTextType.Local_Txt_Set_4, next.MaxLv);
            }
            int attrCount = data.AdditionAttrCount;
            //附加属性
            if (null != m_label_EvolveCurAttrNum)
            {
                m_label_EvolveCurAttrNum.text = tmgr.GetLocalFormatText(LocalTextType.Local_TXT_Soul_Num, data.MuhonAttrUpLimit);
            }
            if (null != m_label_EvolveNextAttrNum)
            {
                m_label_EvolveNextAttrNum.text = tmgr.GetLocalFormatText(LocalTextType.Local_TXT_Soul_Num, data.Next.MuhonAttrUpLimit);
            }
            ColorType color = ColorType.JZRY_Green;
            if (null != m_label_EvolveMuhonLv)
            {
                if (data.Level != data.MaxLv)
                {
                    color = ColorType.JZRY_Txt_NotMatchRed;
                }else
                {
                    color = ColorType.JZRY_Green;
                }

                m_label_EvolveMuhonLv.text = tmgr.GetLocalFormatText(LocalTextType.Local_TXT_Soul_ColorLv,
                     ColorManager.GetNGUIColorOfType(ColorType.JZRY_Txt_Black)
                     , ColorManager.GetNGUIColorOfType(color), data.Level, data.MaxLv);
            }

            //角色等级限制
            if (null != m_label_EvolvePlayerLvLmit)
            {
                if (DataManager.Instance.PlayerLv < data.EvolveNeedPlayerLv)
                {
                    color = ColorType.JZRY_Txt_NotMatchRed;
                }
                else
                {
                    color = ColorType.JZRY_Green;
                }
                m_label_EvolvePlayerLvLmit.text = string.Format("{0}圣魂升星需达{1}主角{2}级"
                    , tmgr.GetMuhonStarName(data.StartLevel)
                    , ColorManager.GetNGUIColorOfType(color)
                    , data.EvolveNeedPlayerLv);
            }

            //辅助道具
            SetEvolveAssit();
        }

        //最大星级
        if (null != m_trans_EvolveMax 
            && m_trans_EvolveMax.gameObject.activeSelf != isMaxStarLv)
        {
            m_trans_EvolveMax.gameObject.SetActive(isMaxStarLv);
        }
        if (isMaxStarLv)
        {
            //刷新圣魂升级信息
            if (null != m_evolveMaxGrow)
            {
                m_evolveMaxGrow.SetGridData(data.QWThisID);
            }
            List<EquipDefine.EquipBasePropertyData> baseProperyList = emgr.GetWeaponSoulBasePropertyData(data.BaseId, data.Level);
            int countCur = (null != baseProperyList) ? baseProperyList.Count : 0;
            if (countCur == 0)
            {
                Engine.Utility.Log.Error("进化表格数据错误");
                return;
            }
            if (null != m_sprite_EvolveAttrTitle)
            {
                m_sprite_EvolveAttrTitle.transform.Find("Value").GetComponent<UILabel>().text
                    = tmgr.GetLocalFormatText(LocalTextType.Local_TXT_Soul_Num, data.MuhonAttrUpLimit);
            }
            EquipDefine.EquipBasePropertyData temp = null;
            if (null != m_sprite_EvolveAttr1)
            {
                if (countCur >= 1)
                {
                    temp = baseProperyList[0];
                    m_sprite_EvolveAttr1.transform.Find("Name").GetComponent<UILabel>().text = temp.Name;
                    m_sprite_EvolveAttr1.transform.Find("Value").GetComponent<UILabel>().text = temp.ToString();
                }
                else if (m_sprite_EvolveAttr1.gameObject.activeSelf)
                    m_sprite_EvolveAttr1.gameObject.SetActive(false);



            }
            if (null != m_sprite_EvolveAttr2)
            {
                if (countCur >= 2)
                {
                    temp = baseProperyList[1];
                    m_sprite_EvolveAttr2.transform.Find("Name").GetComponent<UILabel>().text = temp.Name;
                    m_sprite_EvolveAttr2.transform.Find("Value").GetComponent<UILabel>().text = temp.ToString();
                }
                else if (m_sprite_EvolveAttr2.gameObject.activeSelf)
                    m_sprite_EvolveAttr2.gameObject.SetActive(false);


            }
            if (null != m_sprite_EvolveAttr3)
            {
                if (countCur >= 3)
                {
                    temp = baseProperyList[2];
                    m_sprite_EvolveAttr3.transform.Find("Name").GetComponent<UILabel>().text = temp.Name;
                    m_sprite_EvolveAttr3.transform.Find("Value").GetComponent<UILabel>().text = temp.ToString();
                }
                else if (m_sprite_EvolveAttr3.gameObject.activeSelf)
                    m_sprite_EvolveAttr3.gameObject.SetActive(false);


            }
            if (null != m_sprite_EvolveAttr4)
            {
                if (countCur >= 4)
                {
                    temp = baseProperyList[3];
                    m_sprite_EvolveAttr4.transform.Find("Name").GetComponent<UILabel>().text = temp.Name;
                    m_sprite_EvolveAttr4.transform.Find("Value").GetComponent<UILabel>().text = temp.ToString();
                }
                else if (m_sprite_EvolveAttr4.gameObject.activeSelf)
                    m_sprite_EvolveAttr4.gameObject.SetActive(false);
            }
        }
    }

    private void ReleaseEvolve(bool depthClear = true)
    {
        uint resID = 0;
        if (null != m_evolveCurGrow)
        {
            resID = (uint)GridID.Uiitemshowgrid;
            m_evolveCurGrow.Release(depthClear);
            if (depthClear)
            {
                UIManager.OnObjsRelease(m_evolveCurGrow.CacheTransform, resID);
            }
        }

        if (null != m_evolveMaxGrow)
        {
            resID = (uint)GridID.Uiitemgrowshowgrid;
            m_evolveMaxGrow.Release(depthClear);
            if (depthClear)
            {
                UIManager.OnObjsRelease(m_evolveMaxGrow.CacheTransform, resID);
            }
        }

        if (depthClear && null != m_dicMuhonDeputy)
        {
            UIItemShowGrid tempGrid = null;
            resID = (uint)GridID.Uiitemshowgrid;
            for (EquipDefine.AttrIndex i = EquipDefine.AttrIndex.None + 1; i < EquipDefine.AttrIndex.Max; i++)
            {
                if (m_dicMuhonDeputy.TryGetValue(i,out tempGrid))
                {
                    tempGrid.Release(true);
                    UIManager.OnObjsRelease(tempGrid.CacheTransform, resID);
                }
            }
            m_dicMuhonDeputy.Clear();
        }

        ResetEvolveAnim();
        
    }
    #endregion

    #region UIEvent
    void onClick_EvolveBtn_Btn(GameObject caster)
    {
        Muhon data = Data;
        if (!imgr.ExistItem(selectedMuhonId))
        {
            TipsManager.Instance.ShowTips(LocalTextType.Local_TXT_Soul_InvalidMuhonTips);
            return;
        }
        if (m_dicSelectMuhonDeputy.Count != data.EvolveNeedMuhonNum)
        {
            TipsManager.Instance.ShowTips(LocalTextType.Local_TXT_Soul_ChooseEvolveMuhonTips);
            return;
        }
        int assistNum = imgr.GetItemNumByBaseId(data.EvolveNeedItemId);
        if (assistNum < 1 && !m_bool_autoUseDQ)
        {
            TipsManager.Instance.ShowTips("辅助道具不足");
            return;
        }

        List<uint> ids = new List<uint>();
        ids.AddRange(m_dicSelectMuhonDeputy.Values);

        emgr.EvolutionWeaponSoul(selectedMuhonId, ids, m_bool_autoUseDQ);

    }

    void onClick_BtnEvolvePre_Btn(GameObject caster )
    {
        ToggleEvolvePre();
    }

    bool SetEvolvePre(bool enable)
    {
        if (null != m_trans_EvolvePreview
            && m_trans_EvolvePreview.gameObject.activeSelf != enable)
        {
            m_trans_EvolvePreview.gameObject.SetActive(enable);
            return true;
        }
        return false;
    }

    void ToggleEvolvePre()
    {
        if (null != m_trans_EvolvePreview)
        {
            m_trans_EvolvePreview.gameObject.SetActive(!m_trans_EvolvePreview.gameObject.activeSelf);
        }
    }



    #endregion
}