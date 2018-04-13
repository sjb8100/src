/************************************************************************************
 * Copyright (c) 2017  All Rights Reserved.
 * CLR版本： 4.0.30319.42000
 * 公司名称：ZQB
 * 命名空间：GameMain.UI.Grid.Grids
 * 创建人：  wenjunhua.zqgame
 * 文件名：  UIUnconstraintGrid
 * 版本号：  V1.0.0.0
 * 创建时间：3/28/2017 9:58:25 AM
 * 描述：
 ************************************************************************************/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

class UIUnconstraintGrid : UIGridBase
{
    #region property
    private uint m_uint_id;
    private UIPanel m_panel;
    public uint ID
    {
        get
        {
            return m_uint_id;
        }
    }
    //引导指向根节点
    private Transform m_ts_PointBox = null;
    private Transform m_tsLeft = null;
    //描述左
    private UILabel m_lab_lefDes = null;
    //guideBG
    private Transform m_tsRight = null;
    //描述右边
    private UILabel m_lab_rightDes = null;

    //箭头
    private Transform m_ts_Arrow = null;
    //箭头动画
    private TweenPosition m_tween_Arrow = null;
    //引导背筐
    private UISprite m_sp_GuideBorder = null;
    //指示Content
    private Transform m_ts_pointContent = null;
    //高亮背景
    private UISprite m_sp_highLightBg = null;

    private Transform m_fingerEffectRoot = null;
    private Transform m_effectRoot = null;
    private UIParticleWidget m_particle = null;

    private Transform m_tsContent = null;
    #endregion

    #region overridemethod
    protected override void OnAwake()
    {
        base.OnAwake();
        m_panel = CacheTransform.GetComponent<UIPanel>();
        m_tsContent = CacheTransform.Find("Content");
        m_ts_PointBox = CacheTransform.Find("Content/UnconstrainPointBox");
        m_ts_Arrow = CacheTransform.Find("Content/UnconstrainPointBox/UnArrowContent");
        m_tween_Arrow = CacheTransform.Find("Content/UnconstrainPointBox/UnArrowContent/Arrow").GetComponent<TweenPosition>();

        m_ts_pointContent = CacheTransform.Find("Content/UnconstrainPointBox/UnPointBoxContent");

        m_sp_GuideBorder = CacheTransform.Find("Content/UnconstrainPointBox/UnPointBoxContent/BgBorder").GetComponent<UISprite>();

        m_tsLeft = CacheTransform.Find("Content/UnconstrainPointBox/UnPointBoxContent/PBLeftContent");
        m_tsRight = CacheTransform.Find("Content/UnconstrainPointBox/UnPointBoxContent/PBRightContent");

        m_lab_lefDes = CacheTransform.Find("Content/UnconstrainPointBox/UnPointBoxContent/PBLeftContent/UnGuideContentLeft").GetComponent<UILabel>();
        m_lab_rightDes = CacheTransform.Find("Content/UnconstrainPointBox/UnPointBoxContent/PBRightContent/UnGuideContentRight").GetComponent<UILabel>();

        m_sp_highLightBg = CacheTransform.Find("Content/UnconstrainPointBox/HighLightBorder").GetComponent<UISprite>();
        m_fingerEffectRoot = CacheTransform.Find("Content/UnconstrainPointBox/FingerEffectRoot");
        m_effectRoot = CacheTransform.Find("Content/UnconstrainPointBox/FingerEffectRoot/EffectRoot");
        if (null != m_effectRoot)
        {
            m_particle = m_effectRoot.GetComponent<UIParticleWidget>();
            if (null == m_particle)
            {
                m_particle = m_effectRoot.gameObject.AddComponent<UIParticleWidget>();
                if (null != m_lab_lefDes)
                    m_particle.depth = m_lab_lefDes.depth + 1;
            }
            m_v3EffectSourcePos = m_effectRoot.transform.localPosition;
        }
    }
    #endregion

    #region Set
    private Vector3 m_v3EffectSourcePos = Vector3.zero;
    private bool m_bAddEffect = false;
    private Vector3 m_sourcePos = Vector3.zero;
    /// <summary>
    /// 设置非强制引导数据
    /// </summary>
    /// <param name="panelDepath"></param>
    /// <param name="id"></param>
    public void SetData(int panelDepth,uint id,UIEventDelegate dlg)
    {
        Init();
        if (!Visible)
        {
            SetVisible(true);
        }
        this.m_uint_id = id;
        GuideDefine.LocalGuideData localGuideData = null;
        SetDepth(panelDepth);
        if (DataManager.Manager<GuideManager>().TryGetGuideLocalData(ID, out localGuideData))
        {
            //设置位置
            SetPos(out m_v3ContentOffset);
            //显示内容
            UILabel tempLab = null;
            RefreshHumanImage(ref tempLab);
            if (null != tempLab)
            {
                tempLab.text = localGuideData.Des;
            }

            if (null != localGuideData.GuideTargetObj)
            {
                m_sourcePos = localGuideData.GuideTargetObj.transform.position;
                GuideTrigger trigger = localGuideData.GuideTargetObj.GetComponent<GuideTrigger>();
                if (null == trigger)
                {
                    trigger = localGuideData.GuideTargetObj.AddComponent<GuideTrigger>();
                    trigger.InitTrigger(localGuideData.ID, dlg);
                }else
                {
                    trigger.AddTriggerId(localGuideData.ID);
                }

                //如果没有事件触发，需要手动添加碰撞器触发引导事件
                if (localGuideData.TriggerEventType == GuideDefine.GuideTriggerEventType.None)
                {
                    BoxCollider boxCollider = localGuideData.GuideTargetObj.GetComponent<BoxCollider>();
                    if (null == boxCollider)
                    {
                        boxCollider = localGuideData.GuideTargetObj.AddComponent<BoxCollider>();
                        boxCollider.size = localGuideData.GuideTargetObjLocalBounds.size;
                        boxCollider.center = localGuideData.GuideTargetObjLocalBounds.center;
                    }
                } 
            }

            
            if (null != m_particle)
            {
                m_effectRoot.transform.localPosition = Vector3.zero;
                m_particle.ReleaseParticle();
                m_particle.AddParticle(50042, endCallback: OnParticleDispalyEffectComplete);
            }
            //if (null != m_particle && !m_bAddEffect)
            //{
            //    m_particle.AddParticle(50041);
            //    m_bAddEffect = true;
            //}
            SetVisible(false);
            SetVisible(true);
        }
    }

    private void OnParticleDispalyEffectComplete(string strEffectName, string strEvent, object param)
    {
        if (null != m_particle)
        {
            m_effectRoot.transform.localPosition = m_v3EffectSourcePos;
            m_particle.ReleaseParticle();
            m_particle.AddParticle(50041);
        }
    }

    public void CheckTriggerData(uint guideId,UIEventDelegate dlg)
    {
        GuideDefine.LocalGuideData localGuideData = null;
        if (DataManager.Manager<GuideManager>().TryGetGuideLocalData(ID, out localGuideData))
        {
            if (null != localGuideData.GuideTargetObj)
            {
                GuideTrigger trigger = localGuideData.GuideTargetObj.GetComponent<GuideTrigger>();
                if (null == trigger)
                {
                    trigger = localGuideData.GuideTargetObj.AddComponent<GuideTrigger>();
                    trigger.InitTrigger(localGuideData.ID, dlg);
                }
                else
                {
                    trigger.AddTriggerId(localGuideData.ID);
                }

                //如果没有事件触发，需要手动添加碰撞器触发引导事件
                if (localGuideData.TriggerEventType == GuideDefine.GuideTriggerEventType.None)
                {
                    BoxCollider boxCollider = localGuideData.GuideTargetObj.GetComponent<BoxCollider>();
                    if (null == boxCollider)
                    {
                        boxCollider = localGuideData.GuideTargetObj.AddComponent<BoxCollider>();
                        boxCollider.size = localGuideData.GuideTargetObjLocalBounds.size;
                        boxCollider.center = localGuideData.GuideTargetObjLocalBounds.center;
                    }
                }
            }
        }
        
    }

    private bool IsRightHumanImageMode()
    {
        bool isRightHumanImage = false;
        GuideDefine.LocalGuideData localGuideData = null;
        if (!DataManager.Manager<GuideManager>().TryGetGuideLocalData(ID, out localGuideData))
        {
            return isRightHumanImage;
        }

        switch (localGuideData.GStyle)
        {
            case GuideDefine.GuideStyle.Left:
            case GuideDefine.GuideStyle.LeftUp:
            case GuideDefine.GuideStyle.Up:
            case GuideDefine.GuideStyle.LeftDown:
            case GuideDefine.GuideStyle.Down:
                {
                    isRightHumanImage = true;
                }
                break;
            case GuideDefine.GuideStyle.Right:
            case GuideDefine.GuideStyle.RightDown:
            case GuideDefine.GuideStyle.RightUp:
                {
                    isRightHumanImage = false;
                }
                break;
        }
        return isRightHumanImage;
    }

    private void RefreshHumanImage(ref UILabel guideDes)
    {
        bool isRightHumanImage = IsRightHumanImageMode();
        if (null != m_tsLeft && m_tsLeft.gameObject.activeSelf != isRightHumanImage)
        {
            m_tsLeft.gameObject.SetActive(isRightHumanImage);
        }
        if (null != m_tsRight && m_tsRight.gameObject.activeSelf == isRightHumanImage)
        {
            m_tsRight.gameObject.SetActive(!isRightHumanImage);
        }

        guideDes = (isRightHumanImage) ? m_lab_lefDes : m_lab_rightDes;
    }

    private Vector2 m_v2BroderOffset = new Vector2(20, 20);
    private Vector3 m_v3ContentOffset = Vector3.zero;

    private void SetPos(out Vector3 rootOffset)
    {
        rootOffset = Vector3.zero;
        GuideDefine.LocalGuideData localGuideData = null;
        if (!DataManager.Manager<GuideManager>().TryGetGuideLocalData(ID, out localGuideData))
        {
            return;
        }
        GameObject targetObj = localGuideData.GuideTargetObj;
        
        if (null != targetObj && null != m_ts_PointBox)
        {
            m_ts_PointBox.localPosition = Vector3.zero;
            Bounds targetBounds = NGUIMath.CalculateRelativeWidgetBounds(m_ts_PointBox, targetObj.transform, false);
            Vector3 tempPos = Vector3.zero;
            if (null != m_fingerEffectRoot)
            {
                tempPos.x = targetBounds.center.x;
                tempPos.y = targetBounds.center.y;
                m_fingerEffectRoot.transform.localPosition = tempPos;
            }
            if (null != m_sp_highLightBg)
            {
                m_sp_highLightBg.width = Mathf.CeilToInt(targetBounds.size.x + m_v2BroderOffset.x);
                m_sp_highLightBg.height = Mathf.CeilToInt(targetBounds.size.y + m_v2BroderOffset.y);
                tempPos.x = targetBounds.center.x;
                tempPos.y = targetBounds.center.y;
                m_sp_highLightBg.transform.localPosition = tempPos;
                if (null != m_ts_Arrow && null != m_tween_Arrow)
                {
                    Vector3 rotateAngle = Vector3.zero;
                    m_ts_Arrow.transform.localEulerAngles = rotateAngle;
                    m_tween_Arrow.enabled = false;
                    m_tween_Arrow.ResetToBeginning();
                    Bounds arrowBounds = NGUIMath.CalculateRelativeWidgetBounds(m_ts_PointBox, m_ts_Arrow, false);
                    tempPos.z = 0;
                    switch (localGuideData.GStyle)
                    {
                        case GuideDefine.GuideStyle.Left:
                            {
                                tempPos.x = m_sp_highLightBg.transform.localPosition.x + 0.5f * (m_sp_highLightBg.width + arrowBounds.size.y);
                                tempPos.y = m_sp_highLightBg.transform.localPosition.y;
                                rotateAngle.z = 90;
                            }
                            break;
                        case GuideDefine.GuideStyle.Right:
                            {
                                tempPos.x = m_sp_highLightBg.transform.localPosition.x - 0.5f * (m_sp_highLightBg.width + arrowBounds.size.y);
                                tempPos.y = m_sp_highLightBg.transform.localPosition.y;
                                rotateAngle.z = 270;
                            }
                            break;
                        case GuideDefine.GuideStyle.LeftUp:
                        case GuideDefine.GuideStyle.Up:
                        case GuideDefine.GuideStyle.RightUp:
                            {
                                tempPos.x = m_sp_highLightBg.transform.localPosition.x;
                                tempPos.y = m_sp_highLightBg.transform.localPosition.y - 0.5f * (arrowBounds.size.y + m_sp_highLightBg.height);
                                rotateAngle.z = 0;
                            }
                            break;
                        case GuideDefine.GuideStyle.LeftDown:
                        case GuideDefine.GuideStyle.RightDown:
                        case GuideDefine.GuideStyle.Down:
                            {
                                tempPos.x = m_sp_highLightBg.transform.localPosition.x;
                                tempPos.y = m_sp_highLightBg.transform.localPosition.y + 0.5f * (arrowBounds.size.y + m_sp_highLightBg.height);
                                rotateAngle.z = 180;
                            }
                            break;

                    }
                    m_ts_Arrow.transform.localPosition = tempPos;
                    m_ts_Arrow.transform.localEulerAngles = rotateAngle;

                    if (null != m_ts_pointContent && null != m_sp_GuideBorder)
                    {
                        arrowBounds = NGUIMath.CalculateRelativeWidgetBounds(m_ts_PointBox, m_ts_Arrow, false);
                        Bounds pointBounds = NGUIMath.CalculateRelativeWidgetBounds(m_ts_PointBox, m_sp_GuideBorder.transform, false);
                        Vector3 pointPos = Vector3.zero;
                        Vector3 tempv = Vector3.zero;
                        switch (localGuideData.GStyle)
                        {
                            case GuideDefine.GuideStyle.LeftUp:
                                {
                                    pointPos = arrowBounds.min;
                                    pointPos.z = 0;
                                    tempv = pointBounds.max;
                                    tempv.x = pointBounds.min.x;
                                    tempv.z = 0;
                                    tempPos = m_sp_GuideBorder.transform.localPosition + (pointPos - tempv);
                                }
                                break;
                            case GuideDefine.GuideStyle.Left:
                                {
                                    pointPos.x = arrowBounds.max.x;
                                    pointPos.y = 0.5f * (arrowBounds.max.y + arrowBounds.min.y);
                                    pointPos.z = 0;
                                    tempv.x = pointBounds.min.x;
                                    tempv.y = 0.5f * (pointBounds.max.y + pointBounds.min.y);
                                    tempv.z = 0;
                                    tempPos = m_sp_GuideBorder.transform.localPosition + (pointPos - tempv);
                                }
                                break;
                            case GuideDefine.GuideStyle.Right:
                                {
                                    pointPos.x = arrowBounds.min.x;
                                    pointPos.y = 0.5f * (arrowBounds.max.y + arrowBounds.min.y);
                                    pointPos.z = 0;
                                    tempv.x = pointBounds.max.x;
                                    tempv.y = 0.5f * (pointBounds.max.y + pointBounds.min.y);
                                    tempv.z = 0;
                                    tempPos = m_sp_GuideBorder.transform.localPosition + (pointPos - tempv);
                                }
                                break;
                            case GuideDefine.GuideStyle.Up:
                                {
                                    pointPos.x = 0.5f * (arrowBounds.min.x + arrowBounds.max.x);
                                    pointPos.y = arrowBounds.min.y;
                                    pointPos.z = 0;
                                    tempv.x = 0.5f * (pointBounds.min.x + pointBounds.max.x);
                                    tempv.y = pointBounds.max.y;
                                    tempv.z = 0;
                                    tempPos = m_sp_GuideBorder.transform.localPosition + (pointPos - tempv);
                                }
                                break;
                            case GuideDefine.GuideStyle.RightUp:
                                {
                                    pointPos.x = arrowBounds.max.x;
                                    pointPos.y = arrowBounds.min.y;
                                    pointPos.z = 0;
                                    tempv.x = pointBounds.max.x;
                                    tempv.y = pointBounds.max.y;
                                    tempv.z = 0;
                                    tempPos = m_sp_GuideBorder.transform.localPosition + (pointPos - tempv);
                                }
                                break;
                            case GuideDefine.GuideStyle.LeftDown:
                                {
                                    pointPos.x = arrowBounds.min.x;
                                    pointPos.y = arrowBounds.max.y;
                                    pointPos.z = 0;
                                    tempv.x = pointBounds.min.x;
                                    tempv.y = pointBounds.min.y;
                                    tempv.z = 0;
                                    tempPos = m_sp_GuideBorder.transform.localPosition + (pointPos - tempv);
                                }
                                break;
                            case GuideDefine.GuideStyle.RightDown:
                                {
                                    pointPos.x = arrowBounds.max.x;
                                    pointPos.y = arrowBounds.max.y;
                                    pointPos.z = 0;
                                    tempv.x = pointBounds.max.x;
                                    tempv.y = pointBounds.min.y;
                                    tempv.z = 0;
                                    tempPos = m_sp_GuideBorder.transform.localPosition + (pointPos - tempv);
                                }
                                break;
                            case GuideDefine.GuideStyle.Down:
                                {
                                    pointPos.x = 0.5f * (arrowBounds.min.x + arrowBounds.max.x);
                                    pointPos.y = arrowBounds.max.y;
                                    pointPos.z = 0;
                                    tempv.x = 0.5f * (pointBounds.min.x + pointBounds.max.x);
                                    tempv.y = pointBounds.min.y;
                                    tempv.z = 0;
                                    tempPos = m_sp_GuideBorder.transform.localPosition + (pointPos - tempv);
                                }
                                break;
                        }

                        m_ts_pointContent.transform.localPosition = m_ts_pointContent.transform.localPosition + tempPos;
                    }

                    m_tween_Arrow.enabled = true;
                }

            }

            Bounds targetTempBounds = NGUIMath.CalculateRelativeWidgetBounds(m_tsContent, targetObj.transform, false);

            rootOffset = targetBounds.center -m_ts_PointBox.localPosition;
        }
    }

    public void RefreshPos()
    {
        GuideDefine.LocalGuideData localGuideData = null;
        if (!DataManager.Manager<GuideManager>().TryGetGuideLocalData(ID, out localGuideData) || !localGuideData.RefreshPosInTime)
        {
            return;
        }
        GameObject targetObj = localGuideData.GuideTargetObj;
        if (null != targetObj && null != m_ts_PointBox)
        {
            Vector3 curentPos = targetObj.transform.position;
            float dis = Vector3.Distance(curentPos, m_sourcePos);
            if ( dis <= Mathf.Epsilon)
                return;
            m_sourcePos = curentPos;

            Bounds targetTempBounds = NGUIMath.CalculateRelativeWidgetBounds(m_tsContent, targetObj.transform, false);

            m_ts_PointBox.localPosition = targetTempBounds.center - m_v3ContentOffset;

            //Bounds targetBounds = NGUIMath.CalculateRelativeWidgetBounds(m_tsContent, m_ts_PointBox.transform, false);
            //m_ts_PointBox.localPosition = targetBounds.center - m_v3ContentOffset;
        }
    }
    /// <summary>
    /// 设置深度
    /// </summary>
    /// <param name="panelDepth"></param>
    public void SetDepth(int panelDepth)
    {
        if (null != m_panel)
        {
            m_panel.depth = panelDepth;
        }
    }

    public override void Release(bool depthRelease = true)
    {
        base.Release(depthRelease);
        if (null != m_particle)
        {
            m_particle.ReleaseParticle();
        }
        m_v3ContentOffset = Vector3.zero;

        m_bAddEffect = false;
        m_sourcePos = Vector3.zero;
    }
    #endregion
}