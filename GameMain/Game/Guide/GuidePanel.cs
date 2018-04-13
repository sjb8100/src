/************************************************************************************
 * Copyright (c) 2017  All Rights Reserved.
 * CLR版本： 4.0.30319.42000
 * 公司名称：ZQB
 * 命名空间：GameMain.Game.Guide
 * 创建人：  wenjunhua.zqgame
 * 文件名：  GuidePanel
 * 版本号：  V1.0.0.0
 * 创建时间：2/28/2017 5:07:11 PM
 * 描述：
 ************************************************************************************/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

partial class GuidePanel
{
    #region define
    /// <summary>
    /// 引导样式数据
    /// </summary>
    public class GuideStyleData
    {
        //箭头旋转角度
        public float ArrowRotateAngle = 0;
        //目标挂接点
        public Transform TargetRoot;
        //箭头在右边
        public bool ArrowRight = false;

        public Vector3 TargetPos = Vector3.zero;
    }

    /// <summary>
    /// 引导显示帮组数据
    /// </summary>
    public class GuideShowHelpData
    {
        public GuideDefine.GuideType GType;
        public GuideDefine.LocalGuideData LocalData;

        public Dictionary<GuideDefine.GuideStyle, Transform> StyleRoot = new Dictionary<GuideDefine.GuideStyle, Transform>();
        public Transform GuideRoot;
        public Transform AttachRoot;
        public Transform PointBoxRoot;
        public GameObject CloneTarget;
    }
    #endregion

    #region property
    private GuideShowHelpData m_helpData = null;
    //引导数据是否准备好
    public bool IsDataReady
    {
        get
        {
            return (null != m_helpData && null != m_helpData.LocalData);
        }
    }
    #endregion

    #region overridemethod
    protected override void OnLoading()
    {
        base.OnLoading();
        InitWidgets();
    }

    protected override void OnPrepareShow(object data)
    {
        base.OnPrepareShow(data);
    }

    protected override void OnShow(object data)
    {
        base.OnShow(data);
        if (null != data && data is GuideDefine.LocalGuideData)
        {
            GuideDefine.LocalGuideData localData = data as GuideDefine.LocalGuideData;
            if (localData.GType != GuideDefine.GuideType.Constraint)
            {
                Engine.Utility.Log.Error("GuidePanel Show Failed,localData.GType != GuideDefine.GuideType.Constraint");
                return;
            }
            if (IsDataReady)
            {
                CompleteGuide();
            }
            m_helpData.LocalData = localData;
            m_float_sinceShow = 0;
            ShowSkip(false);
            StartCoroutine(DelayOp(localData.DelayTime));
        }
    }

    protected override void OnHide()
    {
        base.OnHide();
        Engine.Utility.EventEngine.Instance().DispatchEvent((int)Client.GameEventID.UIEVENTGUIDEHIDE,null);
    }
    protected override void OnPanelBaseDestory()
    {
        base.OnPanelBaseDestory();
    }

    public override void OnColliderMaskClicked()
    {
        base.OnColliderMaskClicked();
    }
    #endregion

    #region Init

    /// <summary>
    /// 初始化组件
    /// </summary>
    private void InitWidgets()
    {
        if (null == m_helpData)
        {
            m_helpData = new GuideShowHelpData();
        }
        m_helpData.GuideRoot = m_trans_Constraint;
        m_helpData.PointBoxRoot = m_trans_ConstraintPointBox;
        m_helpData.AttachRoot = m_trans_ConstraintAttachRoot;
        if (null != m_trans_ConstraintDirection)
        {
            Transform dir = null;
            for (GuideDefine.GuideStyle j = GuideDefine.GuideStyle.Center; j < GuideDefine.GuideStyle.Down; j++)
            {
                dir = m_trans_ConstraintDirection.Find(j.ToString());
                if (null != dir)
                {
                    m_helpData.StyleRoot.Add(j, dir);
                }
            }
        }
        m_float_animAddMaxTime = (float)Mathf.Max(GuideManager.GuideShowSkipDelay, GuideManager.GuideAutoCloseDelay);
    }

    float m_float_sinceShow = 0;
    float m_float_animAddMaxTime = 0;
    void Update()
    {
        if (m_float_sinceShow < m_float_animAddMaxTime)
        {
            m_float_sinceShow += Time.deltaTime;
            if (m_float_sinceShow >= GuideManager.GuideShowSkipDelay)
            {
                ShowSkip(true);
            }

            if (m_float_sinceShow >= GuideManager.GuideAutoCloseDelay)
            {

                CompleteGuide(true);
                HideSelf();
            }
        }
    }


    private void SilentComplete()
    {
        CompleteGuide(true);
        HideSelf();
    }

    #endregion


    #region Op

    /// <summary>
    /// 显示隐藏引导
    /// </summary>
    /// <param name="show"></param>
    public void ShowSkip(bool show)
    {
        if (null != m_btn_Skip && m_btn_Skip.gameObject.activeSelf != show)
        {
            m_btn_Skip.gameObject.SetActive(show);
        }
    }

    /// <summary>
    /// 显示引导
    /// </summary>
    /// <param name="data"></param>
    private void ShowGuide()
    {
        Transform ts = null;
        if (IsDataReady)
        {
            GuideDefine.LocalGuideData data = m_helpData.LocalData;
            if (null == m_helpData.LocalData.GuideTargetObj)
            {
                SilentComplete();
            }
            if (!m_helpData.GuideRoot.gameObject.activeSelf)
                m_helpData.GuideRoot.gameObject.SetActive(true);

            GuideStyleData gstyleData = GuidePanel.GetGuideStyleData(m_helpData);
            Vector3 angle = Vector3.zero;
            angle.z = gstyleData.ArrowRotateAngle;
            
            UILabel desLab = null;
            if (null != m_trans_ArrowContent)
            {
                m_trans_ArrowContent.localEulerAngles = angle;
            }
            if (null != m_trans_PointBoxContent)
            {
                m_trans_PointBoxContent.parent = gstyleData.TargetRoot;
                m_trans_PointBoxContent.localPosition = Vector3.zero;
            }

            if (null != m_trans_PBLeftContent)
            {
                if (m_trans_PBLeftContent.gameObject.activeSelf == gstyleData.ArrowRight)
                {
                    m_trans_PBLeftContent.gameObject.SetActive(!gstyleData.ArrowRight);

                }
                if (!gstyleData.ArrowRight)
                {
                    desLab = m_label_GuideLeftContent;
                }
            }

            if (null != m_trans_PBRightContent)
            {
                if (m_trans_PBRightContent.gameObject.activeSelf != gstyleData.ArrowRight)
                {
                    m_trans_PBRightContent.gameObject.SetActive(gstyleData.ArrowRight);

                }
                if (gstyleData.ArrowRight)
                {
                    desLab = m_label_GuideRightContent;
                }
            }
            if (null != desLab)
            {
                desLab.text = data.Des;
            }

            if (null != data.GuideTargetObj && null != m_helpData.AttachRoot)
            {
                //备份目标对象
                GameObject cloneTarget = GameObject.Instantiate(data.GuideTargetObj);
                cloneTarget.name = data.GuideTargetObj.name;
                cloneTarget.transform.parent = m_helpData.AttachRoot;
                cloneTarget.transform.localScale = data.GuideTargetObj.transform.localScale;
                cloneTarget.transform.localPosition = data.GuideTargetObj.transform.localPosition;
                cloneTarget.transform.localRotation = data.GuideTargetObj.transform.localRotation;
                cloneTarget.transform.position = data.GuideTargetObj.transform.position;
                m_helpData.CloneTarget = cloneTarget;

                //删除UIPlayerTween
                UIPlayTween[] tws = cloneTarget.GetComponents<UIPlayTween>();
                int length = (null != tws) ? tws.Length:0;
                if (length > 0)
                {
                    for(int i = 0; i < length ;i ++)
                    {
                        GameObject.Destroy(tws[i]);
                    }
                }

                GuideTrigger trigger = cloneTarget.GetComponent<GuideTrigger>();
                if (null == trigger)
                {
                    trigger = cloneTarget.AddComponent<GuideTrigger>();
                }
                trigger.InitTrigger(data.ID, GuideTriggerDlg);

                //如果没有事件触发，需要手动添加碰撞器触发引导事件
                //if (data.TriggerEventType == GuideDefine.GuideTriggerEventType.None)
                {
                    BoxCollider boxCollider = cloneTarget.GetComponent<BoxCollider>();
                    if (null == boxCollider)
                    {
                        boxCollider = cloneTarget.AddComponent<BoxCollider>();
                        boxCollider.size = data.GuideTargetObjLocalBounds.size;
                        boxCollider.center = data.GuideTargetObjLocalBounds.center;
                    }
                    if (!boxCollider.enabled)
                    {
                        boxCollider.enabled = true;
                    }
                }
            }

            if (null != m_helpData.PointBoxRoot)
            {
                if (!m_helpData.PointBoxRoot.gameObject.activeSelf)
                    m_helpData.PointBoxRoot.gameObject.SetActive(true);
                m_helpData.PointBoxRoot.localPosition = gstyleData.TargetPos;
            }

            ShowGuideDynamicMask(m_helpData.LocalData.GuideTargetWorldPos, 1);
            if (null != m_trans_ConstraintAnimContent)
            {
               TweenScale tween = m_trans_ConstraintAnimContent.GetComponent<TweenScale>();
                if (null != tween)
                {
                    tween.ResetToBeginning();
                    tween.enabled = true;
                }
            }
            SetVisible(false);
            SetVisible(true);
        }
    }

    /// <summary>
    /// 延迟执行部分
    /// </summary>
    /// <param name="delayTime"></param>
    /// <returns></returns>
    private System.Collections.IEnumerator DelayOp(float delayTime)
    {
        yield return new WaitForSeconds(delayTime);
        ShowGuide();
    }


    /// <summary>
    /// 延迟到本帧结束执行
    /// </summary>
    /// <returns></returns>
    private System.Collections.IEnumerator EndOfFrameDoGuideComplete()
    {
        yield return new WaitForEndOfFrame();
        yield return new WaitForEndOfFrame();
        CompleteGuide();
    }

    /// <summary>
    /// 完成单个类型引导
    /// </summary>
    /// <param name="gType"></param>
    private void CompleteGuide(bool skip = false)
    {
        if (IsDataReady)
        {
            uint guideId = m_helpData.LocalData.ID;
            if (null != m_helpData.GuideRoot && m_helpData.GuideRoot.gameObject.activeSelf)
            {
                m_helpData.GuideRoot.gameObject.SetActive(false);
            }
            if (null != m_helpData.CloneTarget)
            {
                GameObject.Destroy(m_helpData.CloneTarget);
            }
            if (null != m_helpData.PointBoxRoot && m_helpData.PointBoxRoot.gameObject.activeSelf)
            {
                m_helpData.PointBoxRoot.gameObject.SetActive(false);
            }
            ResetGuideDynamicMask();
            HideGuideDynamicMask();
            uint groupId = m_helpData.LocalData.GuideGroupID;
            m_helpData.LocalData = null;
            if (!skip)
            {
                Engine.Utility.EventEngine.Instance().DispatchEvent((int)Client.GameEventID.UIEVENTGUIDECOMPLETE, guideId);
            }
            else 
            {
                //跳过当前引导组
                Engine.Utility.EventEngine.Instance().DispatchEvent((int)Client.GameEventID.UIEVENTGUIDESKIP, groupId);
            }
            
        }
    }

    /// <summary>
    /// 获取引导样式数据
    /// </summary>
    /// <param name="localData"></param>
    /// <returns></returns>
    public static GuideStyleData GetGuideStyleData(GuideShowHelpData helpData)
    {
        GuideStyleData styleData = new GuideStyleData();
        Vector3 targetPos = Vector3.zero;
        GuideDefine.LocalGuideData localData = helpData.LocalData;
        if (null == helpData.GuideRoot)
        {
            return styleData;
        }
        Matrix4x4 localMatrix = helpData.GuideRoot.worldToLocalMatrix;

        Bounds bounds = localData.GuideTargetObjWorldBounds;
        float offset = GuideManager.GuidePointAtOffset;
        //转化为本地坐标
        Vector3 min = localMatrix.MultiplyPoint3x4(bounds.min);
        Vector3 max = localMatrix.MultiplyPoint3x4(bounds.max);
        bounds = new Bounds(min, Vector3.zero);
        bounds.Encapsulate(max);
        if (helpData.StyleRoot.ContainsKey(localData.GStyle))
        {
            switch (localData.GStyle)
            {
                case GuideDefine.GuideStyle.LeftUp:
                    styleData.ArrowRotateAngle = 0;
                    styleData.ArrowRight = false;
                    targetPos.x = (bounds.max.x + bounds.min.x) / 2f;
                    targetPos.y = bounds.min.y - offset;
                    break;
                case GuideDefine.GuideStyle.Up:
                    styleData.ArrowRotateAngle = 0;
                    styleData.ArrowRight = false;
                    targetPos.x = (bounds.max.x + bounds.min.x) / 2f;
                    targetPos.y = bounds.min.y - offset;
                    break;
                case GuideDefine.GuideStyle.RightUp:
                    styleData.ArrowRotateAngle = 0;
                    styleData.ArrowRight = true;
                    targetPos.x = (bounds.max.x + bounds.min.x) / 2f;
                    targetPos.y = bounds.min.y - offset;
                    break;
                case GuideDefine.GuideStyle.Down:
                    styleData.ArrowRight = false;
                    styleData.ArrowRotateAngle = -180;
                    targetPos.x = (bounds.max.x + bounds.min.x) / 2f;
                    targetPos.y = bounds.max.y + offset;
                    break;
                case GuideDefine.GuideStyle.LeftDown:
                    styleData.ArrowRight = false;
                    styleData.ArrowRotateAngle = -180;
                    targetPos.x = (bounds.max.x + bounds.min.x) / 2f;
                    targetPos.y = bounds.max.y + offset;
                    break;
                case GuideDefine.GuideStyle.RightDown:
                    styleData.ArrowRight = true;
                    styleData.ArrowRotateAngle = -180;
                    targetPos.x = (bounds.max.x + bounds.min.x) / 2f;
                    targetPos.y = bounds.max.y + offset;
                    break;
                case GuideDefine.GuideStyle.Left:
                    styleData.ArrowRight = false;
                    styleData.ArrowRotateAngle = -270;
                    targetPos.x = bounds.max.x + offset;
                    targetPos.y = (bounds.max.y + bounds.min.y) / 2f;
                    break;
                case GuideDefine.GuideStyle.Right:
                    styleData.ArrowRight = true;
                    styleData.ArrowRotateAngle = -90;
                    targetPos.y = (bounds.max.y + bounds.min.y) / 2f;
                    break;
            }
            styleData.TargetRoot = helpData.StyleRoot[localData.GStyle];
            styleData.TargetPos = targetPos;
        }
        return styleData;
    }

    #endregion

    #region DynamicMask

    /// <summary>
    /// 隐藏动态遮罩
    /// </summary>
    private void HideGuideDynamicMask()
    {
        ResetGuideDynamicMask();
        if (null != m_trans_DynamicMaskContent 
            && m_trans_DynamicMaskContent.gameObject.activeSelf)
        {
            m_trans_DynamicMaskContent.gameObject.SetActive(false);
        }
    }
    /// <summary>
    /// 显示或者隐藏遮罩
    /// </summary>
    /// <param name="show"></param>
    /// <param name="targetLocalPos"></param>
    private void ShowGuideDynamicMask(Vector3 targetPos,float scale = 1)
    {
        ResetGuideDynamicMask();
        if (null != m_trans_DynamicMaskContent)
        {
            if (!m_trans_DynamicMaskContent.gameObject.activeSelf)
            {
                m_trans_DynamicMaskContent.gameObject.SetActive(true);
            }
            m_trans_DynamicMaskContent.transform.position = targetPos;
            m_trans_DynamicMaskContent.transform.localScale = new Vector3(scale,scale,1);
        }

    }
    private bool m_bool_playDynamicMask = false;
    /// <summary>
    /// 播放动态雅黑动画
    /// </summary>
    /// <param name="targetLocalPos"></param>
    /// <param name="sourceScale"></param>
    /// <param name="targetScale"></param>
    private void PlayGuideDynamicMask(Vector3 targetPos, float sourceScale, float targetScale,bool force = false)
    {
        if (null != m_trans_DynamicMaskContent && !m_bool_playDynamicMask)
        {
            ShowGuideDynamicMask(targetPos, sourceScale);
            TweenScale ts = m_trans_DynamicMaskContent.GetComponent<TweenScale>();
            if (null != ts)
            {
                if (!ts.enabled || ts.enabled && force)
                {
                    if (ts.enabled)
                    {
                        ts.enabled = false;
                    }
                    EventDelegate.Callback animComplete = () =>
                        {
                            m_bool_playDynamicMask = false;
                        };
                    if (ts.onFinished.Count == 0)
                    {
                        ts.AddOnFinished(animComplete);
                    }
                    ts.from = new Vector3(sourceScale, sourceScale, 1);
                    ts.to = new Vector3(targetScale, targetScale, 1);
                    ts.ResetToBeginning();
                    ts.enabled = true;
                    m_bool_playDynamicMask = true;
                }
            }
        }
    }

    /// <summary>
    /// 重置引导动态遮罩
    /// </summary>
    private void ResetGuideDynamicMask()
    {
        if (null != m_trans_DynamicMaskContent)
        {
            m_trans_DynamicMaskContent.localPosition = Vector3.zero;
            TweenScale ts = m_trans_DynamicMaskContent.GetComponent<TweenScale>();
            if (null != ts)
            {
                ts.enabled = false;
                ts.ResetToBeginning();
            }
            m_bool_playDynamicMask = false;
            m_trans_DynamicMaskContent.localPosition = Vector3.zero;
            m_trans_DynamicMaskContent.localScale = Vector3.one;
        }
    }
    #endregion


    #region UIEvent
    /// <summary>
    /// 引导UI事件回调
    /// </summary>
    /// <param name="eventType">事件类型</param>
    /// <param name="data"></param>
    /// <param name="param"></param>
    private void GuideTriggerDlg(UIEventType eventType,object data,object param)
    {
        switch(eventType)
        {
            case UIEventType.Click:
                if (null != m_helpData && null != m_helpData.LocalData)
                {
                    if (data is GuideTrigger)
                    {
                        GuideTrigger gTrigger = data as GuideTrigger;
                        if (null != gTrigger && null != gTrigger.TriggerIds)
                        {
                           
                            if (null != m_helpData.LocalData.GuideTargetObj)
                            {
                                GuideTriggerData gtData = m_helpData.LocalData.GuideTargetObj.GetComponent<GuideTriggerData>();
                                if (null == gtData)
                                {
                                    gtData = m_helpData.LocalData.GuideTargetObj.AddComponent<GuideTriggerData>();
                                    gtData.IsGuideTrigger = true;
                                }
                                m_helpData.LocalData.GuideTargetObj.SendMessage("OnClick",SendMessageOptions.DontRequireReceiver);
                                if (null != gtData)
                                {
                                    GameObject.Destroy(gtData);
                                }
                            }
                            List<uint> triggerIds = new List<uint>();
                            triggerIds.AddRange(gTrigger.TriggerIds);
                            for (int i = 0; i < triggerIds.Count; i++)
                            {
                                if (m_helpData.LocalData.ID == triggerIds[i])
                                {
                                    CompleteGuide();
                                }
                            }

                            HideSelf();
                        }
                    }
                }
                
                break;
        }
    }
    void onClick_BtnMask_Btn(GameObject caster)     
    {
        if (!m_bool_playDynamicMask)
        {
            if (IsDataReady)
            {
                PlayGuideDynamicMask(m_helpData.LocalData.GuideTargetWorldPos, 30, 1);
            }
        }
    }

    void onClick_Skip_Btn( GameObject caster )
    {
        CompleteGuide(true);
        HideSelf();
    }
    #endregion
}