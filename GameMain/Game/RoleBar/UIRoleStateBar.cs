/************************************************************************************
 * Copyright (c) 2017  All Rights Reserved.
 * CLR版本： 4.0.30319.42000
 * 公司名称：ZQB
 * 命名空间：GameMain.Game.RoleBar
 * 创建人：  wenjunhua.zqgame
 * 文件名：  UIRoleStateBarcs
 * 版本号：  V1.0.0.0
 * 创建时间：7/26/2017 3:55:53 PM
 * 描述：
 ************************************************************************************/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Client;

class UIRoleStateBar
{
    #region property
    private Transform m_tsTran = null;
    public Transform Tran
    {
        get
        {
            return m_tsTran;
        }
    }
    private long m_lUID = 0;
    public long UID
    {
        get
        {
            return m_lUID;
        }
    }
    //名称
    private UILabel m_labName;
    //头顶标识
    private UISprite m_spHeadMask;
    //称号
    private UILabel m_labTitle;
    private UIParticleWidget m_particleWidget;
    private UISprite m_particleRelativeWidget;
    private Engine.IEffect m_titleEffect;

    //血条
    private UISlider m_hpSlider;
    private UISprite m_spHpbg;
    private UISprite m_spHpForg;

    //氏族名称
    private UILabel m_labClanName;
    //采集
    private UILabel m_labCollectTips;
    private float offsetY = 0.5f;//头顶与模型脚下坐标的差值 以后根据配置读取
    public string name;
    public UIWidget widget;
    //任务状态
    private UISprite m_spTaskStatus;
    private int m_iGetNameSeed = 0;
    private Vector3 m_v3StartPos = Vector3.zero;

    private Transform m_tsCampMask = null;
    private Transform m_tsGodMask = null;
    private Transform m_tsDemonMask = null;
    Transform m_bossTalk = null;
    UILabel m_bossText = null;

    #endregion

    #region StructMethod
    private UIRoleStateBar(Transform ts, long uid)
    {
        InitWidget(ts);
        if (uid != 0)
        {
            SetData(uid);
        }
    }
    #endregion

    #region Init
    /// <summary>
    /// 创建一个UIRoleStateBar组件
    /// </summary>
    /// <param name="ts"></param>
    /// <param name="uid"></param>
    /// <returns></returns>
    public static UIRoleStateBar Create(Transform ts, long uid = 0)
    {
        return new UIRoleStateBar(ts, uid);
    }

    /// <summary>
    /// 初始化
    /// </summary>
    /// <param name="ts"></param>
    private void InitWidget(Transform ts)
    {
        m_tsTran = ts;
        if(widget == null)
        {
            widget = ts.GetComponent<UIWidget>();
        }
        for (HeadStatusType i = HeadStatusType.None + 1; i < HeadStatusType.Max; i++)
        {
            Transform child = m_tsTran.Find(i.ToString());
            if (child != null)
            {
                switch (i)
                {
                    case HeadStatusType.Name:
                        {
                            m_labName = child.GetComponent<UILabel>();
                        }
                        break;
                    case HeadStatusType.HeadMaskIcon:
                        {
                            m_spHeadMask = child.GetComponent<UISprite>();
                        }
                        break;
                    case HeadStatusType.Hp:
                        {
                            m_hpSlider = child.GetComponent<UISlider>();
                            m_spHpbg = child.Find("bg").GetComponent<UISprite>();
                            m_spHpForg = child.GetComponent<UISprite>();
                        }
                        break;
                    case HeadStatusType.Title:
                        {
                            m_labTitle = child.Find("TtitleTxt").GetComponent<UILabel>();
                            Transform pts = child.Find("ParticleEffect");
                            if (null != pts)
                            {
                                m_particleWidget = pts.GetComponent<UIParticleWidget>();
                                if (null == m_particleWidget)
                                {
                                    m_particleWidget = pts.gameObject.AddComponent<UIParticleWidget>();

                                }
                                if (null != m_particleWidget)
                                {
                                    m_particleRelativeWidget = m_particleWidget.GetComponent<UISprite>();
                                }
                            }
                        }
                        break;
                    case HeadStatusType.Clan:
                        {
                            m_labClanName = child.GetComponent<UILabel>();
                        }
                        break;
                    case HeadStatusType.Collect:
                        {
                            m_labCollectTips = child.GetComponent<UILabel>();
                        }
                        break;
                    case HeadStatusType.TaskStatus:
                        {
                            m_spTaskStatus = child.GetComponent<UISprite>();
                        }
                        break;

                    case HeadStatusType.CampMask:
                        {
                            m_tsCampMask = child;
                            if (null != m_tsCampMask)
                            {
                                m_tsGodMask = m_tsCampMask.Find("God");
                                m_tsDemonMask = m_tsCampMask.Find("Demon");
                            }
                        }
                        break;
                    case HeadStatusType.BossSay:
                        {
                            m_bossTalk = child;
                            if (m_bossTalk != null)
                            {
                                m_bossText = m_bossTalk.Find("text").GetComponent<UILabel>();
                            }
                        }
                        break;

                    default:
                        break;
                }
            }
        }
        if (null != m_hpSlider)
        {
            m_v3StartPos = m_hpSlider.cachedTransform.localPosition;
        }
    }

    private Transform GetWidget(HeadStatusType headType)
    {
        Transform widget = null;
        switch (headType)
        {
            case HeadStatusType.Name:
                {
                    widget = m_labName.cachedTransform;
                }
                break;
            case HeadStatusType.HeadMaskIcon:
                {
                    widget = m_spHeadMask.cachedTransform;
                }
                break;
            case HeadStatusType.Hp:
                {
                    widget = m_hpSlider.cachedTransform;
                }
                break;
            case HeadStatusType.Title:
                {
                    widget = m_labTitle.cachedTransform.parent;
                }
                break;
            case HeadStatusType.Clan:
                {
                    widget = m_labClanName.cachedTransform;
                }
                break;
            case HeadStatusType.Collect:
                {
                    widget = m_labCollectTips.cachedTransform;
                }
                break;
            case HeadStatusType.TaskStatus:
                {
                    widget = m_spTaskStatus.cachedTransform;
                }
                break;
            case HeadStatusType.CampMask:
                {
                    widget = m_tsCampMask;
                }
                break;
            case HeadStatusType.BossSay:
                {
                    widget = m_bossTalk;
                }
                break;
            default:
                break;
        }
        return widget;
    }

    /// <summary>
    /// 填充数据
    /// </summary>
    /// <param name="id"></param>
    public void SetData(long id)
    {
        m_iGetNameSeed = 0;
        m_lUID = id;
        if (!Visible)
        {
            SetVisible(true);
        }
        UpdateBarUI();
        //UpdatePositon();
    }
    #endregion

    #region Op
    private void AdjustStatusPos()
    {
        if (Visible && null != m_hpSlider)
        {
            int Gap = GameTableManager.Instance.GetGlobalConfig<int>("HeadTitleGap");
            float gap = Gap * 1.0f;
            Vector3 pos = m_v3StartPos;
            Transform widget = null;
            Bounds widgetsBounds;
            int enableCount = 0;
            for (HeadStatusType i = HeadStatusType.None + 1, max = HeadStatusType.Max; i < max; i++)
            {
                if (i == HeadStatusType.CampMask)
                    continue;
                widget = GetWidget(i);
                if (null == widget || !widget.gameObject.activeSelf)
                {
                    continue;
                }

                widgetsBounds = NGUIMath.CalculateRelativeWidgetBounds(widget, false);
                if (enableCount != 0)
                {
                    pos.y += widgetsBounds.size.y * 0.5f;
                    if (i == HeadStatusType.HeadMaskIcon)
                    {
                        pos.y -= gap;
                    }
                }
                widget.localPosition = pos;
                pos.y += widgetsBounds.size.y * 0.5f;
                pos.y += gap;

                enableCount++;
            }
        }
    }
    /// <summary>
    /// 刷新任务
    /// </summary>
    public void UpdateTaskUI(bool enable, string taskStatusIcon = "")
    {
        Transform taskWidget = GetWidget(HeadStatusType.TaskStatus);
        if (null != taskWidget)
        {
            if (taskWidget.gameObject.activeSelf != enable)
            {
                taskWidget.gameObject.SetActive(enable);
            }
            if (enable)
            {
                if (null != m_spTaskStatus)
                {
                    m_spTaskStatus.spriteName = taskStatusIcon;
                    m_spTaskStatus.MakePixelPerfect();
                }
            }
            AdjustStatusPos();
        }
    }
    /// <summary>
    /// 更新UI
    /// </summary>
    public void UpdateBarUI()
    {
        for (HeadStatusType i = HeadStatusType.None + 1, max = HeadStatusType.Max; i < max; i++)
        {
            UpdateHeadStatus(i, false);
        }
        //
        AdjustStatusPos();
    }

    private CMResAsynSeedData<CMAtlas> m_headCASD = null;
    private CMResAsynSeedData<CMAtlas> m_taskCASD = null;
    /// <summary>
    /// 更新顶部栏状态
    /// </summary>
    public void UpdateHeadStatus(HeadStatusType type, bool adjustPos = true)
    {
        RoleStateBarManager mgr = DataManager.Manager<RoleStateBarManager>();
        IEntity entity = RoleStateBarManager.GetEntity(UID);
        if (null == entity)
        {
            return;
        }
        Transform widget = GetWidget(type);

        if (null != widget)
        {
            QuestTraceInfo traceInfo = null;
            bool visible = false;
            if (type != HeadStatusType.TaskStatus)
            {
                visible = RoleStateBarManager.IsEntityHeadStatusTypeEnable(entity, type);
            }
            else
            {
                if (RoleStateBarManager.TryGetNPCTraceInfo(entity, out traceInfo))
                {
                    visible = true;
                }
            }
            if (widget.gameObject.activeSelf != visible)
            {
                widget.gameObject.SetActive(visible);
            }
            if (type == HeadStatusType.Title
                && null != m_particleWidget)
            {
                m_particleWidget.ReleaseParticle();
            }
            if (visible)
            {
                switch (type)
                {
                    case HeadStatusType.Hp:
                        if (null != m_hpSlider && null != m_spHpbg && null != m_spHpForg)
                        {
                            float hpPer = entity.GetProp((int)CreatureProp.Hp) / (float)entity.GetProp((int)CreatureProp.MaxHp);
                            EntityHpSprite hpSp = RoleStateBarManager.GetEntityHpSpData(entity);
                            if (hpSp != null)
                            {
                                m_spHpbg.spriteName = hpSp.bgSpriteName;
                                m_spHpbg.MakePixelPerfect();
                                m_spHpForg.spriteName = hpSp.spriteName;
                                m_spHpForg.MakePixelPerfect();
                                m_hpSlider.gameObject.SetActive(hpSp.bShow);
                                m_hpSlider.value = hpPer;
                            }
                        }
                        break;
                    case HeadStatusType.Name:
                        if (null != m_labName)
                        {
                            m_labName.text = ColorManager.GetColorString(mgr.GetNameColor(entity), entity.GetName());
                        }
                        break;
                    case HeadStatusType.Clan:
                        if (null != m_labClanName)
                        {
                            Action<string, int> clanNameDlg = (clanName, getNameSeed) =>
                                {
                                    if (getNameSeed >= m_iGetNameSeed)
                                    {
                                        //保证先后
                                        //string name = DataManager.Manager<TextManager>().GetLocalFormatText(LocalTextType.Clan_Commond_shizumingzi, clanName);
                                        //labelClan.fontSize = data.m_nFontSize;
                                        m_labClanName.text = ColorManager.GetColorString(RoleStateBarManager.GetClanNameColor(entity), clanName);
                                    }
                                };
                            RoleStateBarManager.GetRoleBarClanName(entity, clanNameDlg, ++m_iGetNameSeed);
                        }
                        break;
                    case HeadStatusType.Title:
                        if (null != m_labTitle)
                        {
                            table.TitleDataBase titleDb = RoleStateBarManager.GetTitleText(entity);
                            if (null != titleDb)
                            {
                                bool visibleTxt = (titleDb.UIState == 0);
                                if (null != m_labTitle)
                                {
                                    if (m_labTitle.gameObject.activeSelf != visibleTxt)
                                    {
                                        m_labTitle.gameObject.SetActive(visibleTxt);
                                    }
                                    m_labTitle.text = titleDb.SceneTextUI;
                                }

                                if (null != m_particleWidget)
                                {
                                    if (m_particleWidget.gameObject.activeSelf == visibleTxt)
                                    {
                                        m_particleWidget.gameObject.SetActive(!visibleTxt);
                                    }

                                    m_particleWidget.AddParticle(titleDb.FxUI, m_particleRelativeWidget);
                                }
                            }

                        }
                        break;
                    case HeadStatusType.Collect:
                        if (null != m_labCollectTips)
                        {
                            m_labCollectTips.color = ColorManager.GetColor32OfType(ColorType.JSXT_CaiJiWu);
                        }
                        break;
                    case HeadStatusType.HeadMaskIcon:
                        if (null != m_spHeadMask)
                        {
                            table.NpcHeadMaskDataBase npcmaskDB = RoleStateBarManager.GetNPCHeadMaskDB(entity);
                            string iconName = (null != npcmaskDB) ? npcmaskDB.headMaskIcon : "";
                            UIManager.GetAtlasAsyn(iconName, ref m_headCASD, () =>
                                {
                                    if (null != m_spHeadMask)
                                    {
                                        m_spHeadMask.atlas = null;
                                    }
                                }, m_spHeadMask);
                        }
                        break;
                    case HeadStatusType.TaskStatus:
                        if (null != m_spTaskStatus && null != traceInfo)
                        {
                            long uid = 0;
                            string icon = "";
                            if (RoleStateBarManager.TryGetQuestStatusIcon(traceInfo, out uid, out icon)
                                && uid == UID)
                            {
                                UIManager.GetAtlasAsyn(icon, ref m_taskCASD, () =>
                                {
                                    if (null != m_spTaskStatus)
                                    {
                                        m_spTaskStatus.atlas = null;
                                    }
                                }, m_spTaskStatus);
                            }
                            else
                            {
                                widget.gameObject.SetActive(false);
                            }
                        }
                        break;
                    case HeadStatusType.CampMask:
                        {
                            bool isGod = true;
                            int camp = entity.GetProp((int)Client.CreatureProp.Camp);
                            if (camp == (int)GameCmd.eCamp.CF_Red)
                            {
                                isGod = false;
                            }
                            if (null != m_tsDemonMask && m_tsDemonMask.gameObject.activeSelf == isGod)
                            {
                                m_tsDemonMask.gameObject.SetActive(!isGod);
                            }

                            if (null != m_tsGodMask && m_tsGodMask.gameObject.activeSelf != isGod)
                            {
                                m_tsGodMask.gameObject.SetActive(isGod);
                            }
                        }
                        break;
                    case HeadStatusType.BossSay:
                        {
                         
                           
                            uint npcID = DataManager.Manager<RoleStateBarManager>().GetTalkingBossID();

                            table.BossTalkDataBase db = GameTableManager.Instance.GetTableItem<table.BossTalkDataBase>(npcID);
                            if (db != null)
                            {
                                uint textID = db.textID;
                                table.LangTextDataBase ldb = GameTableManager.Instance.GetTableItem<table.LangTextDataBase>(textID);
                                if (ldb != null)
                                {
                                    m_bossText.text = ldb.strText;
                                }
                                m_bossTalk.gameObject.SetActive(visible);
                            }
                            else
                            {
                                m_bossTalk.gameObject.SetActive(false);
                            }
                        }
                        break;
                }
            }

            if (adjustPos)
            {
                AdjustStatusPos();
            }
        }
    }

    public void Release()
    {
        if (null != m_headCASD)
        {
            m_headCASD.Release(true);
            m_headCASD = null;
        }

        if (null != m_taskCASD)
        {
            m_taskCASD.Release(true);
            m_taskCASD = null;
        }

        if (null != m_particleWidget)
        {
            m_particleWidget.ReleaseParticle();
        }
    }

    /// <summary>
    /// 设置是否可见
    /// </summary>
    /// <param name="visible"></param>
    public void SetVisible(bool visible)
    {
        if (null != m_tsTran)
        {
            m_tsTran.gameObject.SetActive(visible);
        }

        if (!visible)
        {
            Release();
        }
    }

    /// <summary>
    /// 是否可见
    /// </summary>
    public bool Visible
    {
        get
        {
            return m_tsTran.gameObject.activeSelf;
        }
    }

    /// <summary>
    /// 更新位置
    /// </summary>
    public void UpdatePositon()
    {
        if (!Visible)
        {
            return;
        }
        if (UID == 0)
        {
            return;
        }
        Client.IEntity entity = RoleStateBarManager.GetEntity(UID);
        if (entity == null)
        {
            return;
        }

        RoleStateBarManager mgr = DataManager.Manager<RoleStateBarManager>();
        Vector3 pos = Vector3.zero;
        if (entity.GetEntityType() == EntityType.EntityType_Player)
        {
            bool bRide = (bool)entity.SendMessage(Client.EntityMessage.EntityCommond_IsRide, null);
            if (bRide)
            {
                pos = mgr.GetNodeWorldPos(UID, "MountName", offsetY);
            }
            else
            {
                pos = mgr.GetNodeWorldPos(UID, "Name", offsetY);
            }
        }
        else
        {
            pos = mgr.GetNodeWorldPos(UID, "Name", offsetY);
        }

        Camera mainCamera = Util.MainCameraObj.GetComponent<Camera>();
        Camera uiCamera = Util.UICameraObj.GetComponent<Camera>();
        if (null == uiCamera || null == mainCamera)
        {
            return;
        }
        pos = mainCamera.WorldToViewportPoint(pos);
        //bool isVisible = (mainCamera.orthographic || pos.z > 0f) && (/*!disableIfInvisible || */(pos.x > 0f && pos.x < 1f && pos.y > 0f && pos.y < 1f));
        //if (Visible != isVisible)
        //{
        //    SetVisible(isVisible);
        //    if (Visible)
        //    {
        //        AdjustStatusPos();
        //    }
        //}
        pos = uiCamera.ViewportToWorldPoint(pos);
        m_tsTran.position = pos;
        pos = m_tsTran.localPosition;
        pos.x = Mathf.FloorToInt(pos.x);
        pos.y = Mathf.FloorToInt(pos.y);
        pos.z = 0f;
        m_tsTran.localPosition = pos;
    }
    #endregion
}