/************************************************************************************
 * Copyright (c) 2017  All Rights Reserved.
 * CLR版本： 4.0.30319.42000
 * 公司名称：ZQB
 * 命名空间：GameMain.Game.RoleBar
 * 创建人：  wenjunhua.zqgame
 * 文件名：  RoleStateBarPanel
 * 版本号：  V1.0.0.0
 * 创建时间：7/26/2017 3:44:11 PM
 * 描述：
 ************************************************************************************/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Client;
using UnityEngine;


partial class RoleStateBarPanel
{
    #region property
    private Dictionary<long, UIRoleStateBar> m_dicActiveRoleStateBar = null;
    private List<UIRoleStateBar> m_lstCacheStateBar = null;
    #endregion

    #region overridemethod
    protected override void OnLoading()
    {
        base.OnLoading();
        InitWidgets();
    }

    protected override void OnShow(object data)
    {
        base.OnShow(data);
        BuildEntityBars();
        RegisterGlobalEvent(true);
        
    }

    protected override void OnHide()
    {
        base.OnHide();
        
        RegisterGlobalEvent(false);
        m_lstChangePosIds.Clear();
        ReleaseAll();
    }

    protected override void OnPanelBaseDestory()
    {
        base.OnPanelBaseDestory();
    }
    #endregion

    #region Init
    private void InitWidgets()
    {
        if (null == m_dicActiveRoleStateBar)
        {
            m_dicActiveRoleStateBar = new Dictionary<long, UIRoleStateBar>();
            m_lstCacheStateBar = new List<UIRoleStateBar>();
        }
    }
    #endregion

    void LateUpdate()
    {
        DoLateUpdateChangePos();
    }

    #region OP

    /// <summary>
    /// 构建实体顶部栏
    /// </summary>
    private void BuildEntityBars()
    {
        StopCoroutine(BuildEntityBarsCor());
        StartCoroutine(BuildEntityBarsCor());
    }

    private System.Collections.IEnumerator BuildEntityBarsCor()
    {
        yield return 0;
        IEntitySystem entitySystem = ClientGlobal.Instance().GetEntitySystem();
        if (null != entitySystem)
        {
            List<long> entityUids = entitySystem.GetEntityUids();
            
            if (null != entityUids)
            {
                UIRoleStateBar bar = null;
                int successCreateCount = 0;
                int modSeed = (entityUids.Count % 30);
                modSeed = Mathf.Max(0, modSeed);
                for (int i = 0, max = entityUids.Count; i < max; i++)
                {
                   bar = AddRoleBar(entityUids[i]);
                   if (null != bar)
                   {
                       successCreateCount++;
                       if(modSeed != 0)
                       {
                           if (successCreateCount % modSeed == 0)
                           {
                               //等待下一帧
                               yield return null;
                           }
                       }
                    
                   }
                }
            }
        }
    }
    /// <summary>
    /// 获取一个空的RoleBar
    /// </summary>
    /// <returns></returns>
    private Transform GetRoleBarTrans()
    {
        GameObject prefab = UIManager.GetResGameObj(GridID.Uirolestatebar) as GameObject;
        if (null != m_widget_RoleStateBarRoot)
        {
            GameObject cloneObj = NGUITools.AddChild(m_widget_RoleStateBarRoot.gameObject, prefab);
            if (null != cloneObj)
            {
                return cloneObj.transform;
            }
        }
        return null;
    }

    /// <summary>
    /// 获取角色状态bar
    /// </summary>
    /// <returns></returns>
    public UIRoleStateBar GetEmptyRoleStateBar()
    {
        UIRoleStateBar bar = null;
        if (m_lstCacheStateBar.Count > 0)
        {
            bar = m_lstCacheStateBar[0];
            m_lstCacheStateBar.RemoveAt(0);
        }else
        {
            bar = UIRoleStateBar.Create(GetRoleBarTrans());
        }
        return bar;
    }

    private void UpdateRoleBarPos(long uid)
    {
        UIRoleStateBar roleBar = GetRoleBar(uid);
        if (null != roleBar)
        {
            roleBar.UpdatePositon();
        }
    }

    /// <summary>
    /// 根据UID获取RoleBar
    /// </summary>
    /// <param name="uid"></param>
    /// <returns></returns>
    private UIRoleStateBar GetRoleBar(long uid)
    {
        if (m_dicActiveRoleStateBar.ContainsKey(uid))
        {
            return m_dicActiveRoleStateBar[uid];
        }
        return null;
    }

    /// <summary>
    /// 删除头顶bar
    /// </summary>
    /// <param name="uid"></param>
    private void RemoveRoleBar(long uid)
    {
        Release(uid);
    }

    /// 添加实体头顶标识
    /// </summary>
    /// <param name="uid"></param>
    private UIRoleStateBar AddRoleBar(long uid)
    {
        IEntity entity = RoleStateBarManager.GetEntity(uid);
        if (null == entity)
        {
            Engine.Utility.Log.Error("RoleStateBarPanel GetEntity Failed UID = " + uid);
            return null;
        }
        return AddRoleBar(entity);
    }

    /// <summary>
    /// 添加实体头顶标识
    /// </summary>
    /// <param name="entity"></param>
    private UIRoleStateBar AddRoleBar(IEntity entity)
    {
        if (null == entity || m_dicActiveRoleStateBar.ContainsKey(entity.GetUID()))
        {
            return null;
        }
        UIRoleStateBar rolebar = GetEmptyRoleStateBar();
        if (null != rolebar)
        {
            if (Application.isEditor)
            {
                if (entity.GetEntityType() == EntityType.EntityType_Player)
                {
                    rolebar.Tran.name = entity.GetName() + entity.GetID().ToString();
                }
                else
                {
                    rolebar.Tran.name = entity.GetName() + "_" + entity.GetID().ToString();
                }
            }
            rolebar.SetVisible(true);
            rolebar.SetData(entity.GetUID());
            m_dicActiveRoleStateBar.Add(entity.GetUID(), rolebar);
            //LateUpdate刷新位置
            LateUpdateChangePos(entity.GetUID());
        }
        return rolebar;
    }

    private void UpdateHeadStaus(long uid,HeadStatusType type,bool adJustHp = true)
    {
        if (m_dicActiveRoleStateBar.ContainsKey(uid))
        {
            m_dicActiveRoleStateBar[uid].UpdateHeadStatus(type, adJustHp);
        }
    }

    private void UpdateHeadStaus(IEntity entity,HeadStatusType type)
    {
        UpdateHeadStaus(entity.GetUID(), type);
    }

    private List<long> m_lstChangePosIds = new List<long>();
    private void DoLateUpdateChangePos()
    {
        if (m_lstChangePosIds.Count > 0)
        {
            UIRoleStateBar roleStateBar = null;
            for (int i = 0, max = m_lstChangePosIds.Count; i < max; i++)
            {
                if (!m_dicActiveRoleStateBar.TryGetValue(m_lstChangePosIds[i],out roleStateBar))
                {
                    continue;
                }
                roleStateBar.UpdatePositon();
            }
            m_lstChangePosIds.Clear();
        }
    }
    /// <summary>
    /// 释放状态栏
    /// </summary>
    /// <param name="stateBar"></param>
    private void Release(UIRoleStateBar stateBar)
    {
        if (null == stateBar)
            return;
        Release(stateBar.UID);
    }

    /// <summary>
    /// 释放状态栏
    /// </summary>
    /// <param name="uid"></param>
    private void Release(long uid)
    {
        UIRoleStateBar roleStateBar = null;
        if (m_dicActiveRoleStateBar.ContainsKey(uid))
        {
            roleStateBar = m_dicActiveRoleStateBar[uid];
            m_dicActiveRoleStateBar.Remove(uid);
        }

        if (null != roleStateBar && !m_lstCacheStateBar.Contains(roleStateBar))
        {
            roleStateBar.SetVisible(false);
            roleStateBar.Tran.name = "cache";
            m_lstCacheStateBar.Add(roleStateBar);
        }
    }

    /// <summary>
    /// 释放所有
    /// </summary>
    private void ReleaseAll()
    {
        if (null != m_dicActiveRoleStateBar)
        {   List<long> keys = new List<long>();
            keys.AddRange(m_dicActiveRoleStateBar.Keys);
            while (keys.Count > 0)
            {
                Release(keys[0]);
                keys.RemoveAt(0);
            }
        }
    }

    void SetBarVisible(long uid, bool bShow, float alp = 1.0f)
    { 
        UIRoleStateBar roleStateBar = null;
        if (m_dicActiveRoleStateBar.ContainsKey(uid))
        {
            roleStateBar = m_dicActiveRoleStateBar[uid];
            if (roleStateBar !=  null)
            {
                UIWidget widgt = roleStateBar.widget;
                if (widgt != null)
                {
                    if (bShow)
                    {
                        widgt.alpha = alp;
                    }
                    else
                    {
                        widgt.alpha = 0f;
                    }
                }
              
            }
        }
    }
    #endregion
}