using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using table;
using UnityEngine;


partial class TitleAddPropertyPanel : UIPanelBase
{

    Dictionary<uint, int> m_dicEachTypeForeverAdd = null;//永久属性  按类型分   key   typeId   value  该typeID下面的总加成
    List<uint> m_lstAllForeverAddeffectTypeId = null;       //永久属性list

    List<uint> m_lstActivateAdd = null;     //激活加成

    UIGridCreatorBase m_titleForeverGridCreator;     //永久加成
    UIGridCreatorBase m_titleActivateGridCreator;  //激活加成

    TitleManager TManager
    {
        get
        {
            return DataManager.Manager<TitleManager>();
        }
    }


    #region override

    protected override void OnInitPanelData()
    {
        base.OnInitPanelData();
        //panelBaseData.m_bool_needCMBg = false;
        //panelBaseData.m_int_topBarUIMask = (int)UIDefine.UITopBarUIMode.None;
        //panelBaseData.m_str_name = "我的队伍";
        //panelBaseData.m_em_showMode = UIDefine.UIPanelShowMode.HideNormal;
        //panelBaseData.m_em_colliderMode = UIDefine.UIPanelColliderMode.TransBg;
    }

    protected override void OnLoading()
    {
        base.OnLoading();

        InitTitlePropWidgt();
    }

    protected override void OnPrepareShow(object data)
    {
        base.OnPrepareShow(data);
    }

    protected override void OnShow(object data)
    {
        base.OnShow(data);

        InitTitlePropData();

        CreateGrids();

        m_label_allAddfightInfo.text = "总战斗力加成 " + TManager.GetAllAdd();
    }

    protected override void OnHide()
    {
        base.OnHide();

        Release();
    }

    protected override void OnPanelBaseDestory()
    {
        base.OnPanelBaseDestory();
    }

    public override void Release(bool depthRelease = true)
    {
        base.Release(depthRelease);

        if (m_titleForeverGridCreator != null)
        {
            m_titleForeverGridCreator.Release(depthRelease);
        }

        if (m_titleActivateGridCreator != null)
        {
            m_titleActivateGridCreator.Release(depthRelease);
        }

    }

    #endregion


    /// <summary>
    /// 初始化数据
    /// </summary>
    void InitTitlePropData()
    {
        //永久加成
        m_dicEachTypeForeverAdd = TManager.GetForeverAddByEachTypeDic();
        m_lstAllForeverAddeffectTypeId = new List<uint>(m_dicEachTypeForeverAdd.Keys);

        //激活加成
        m_lstActivateAdd = TManager.GetAllActivateAddList();
    }

    /// <summary>
    /// 初始化界面
    /// </summary>
    void InitTitlePropWidgt()
    {

        if (m_titleForeverGridCreator == null || m_titleActivateGridCreator == null)
        {
            //永久加成
            //UnityEngine.GameObject obj = UIManager.GetResGameObj(GridID.Uititlepropgrid) as UnityEngine.GameObject;
            m_titleForeverGridCreator = m_trans_foreverContent.GetComponent<UIGridCreatorBase>();
            if (m_titleForeverGridCreator == null)
                m_titleForeverGridCreator = m_trans_foreverContent.gameObject.AddComponent<UIGridCreatorBase>();
            m_titleForeverGridCreator.gridContentOffset = new UnityEngine.Vector2(0, 0);
            m_titleForeverGridCreator.arrageMent = UIGridCreatorBase.Arrangement.Vertical;
            m_titleForeverGridCreator.gridWidth = 250;
            m_titleForeverGridCreator.gridHeight = 40;

            m_titleForeverGridCreator.RefreshCheck();
            m_titleForeverGridCreator.Initialize<UITitlePropAddGrid>((uint)GridID.Uititlepropgrid, UIManager.OnObjsCreate, UIManager.OnObjsRelease, OnTitleForeverAddPropGridDataUpdate, OnTitlePropGridUIEvent);

            //激活加成
            m_titleActivateGridCreator = m_trans_activateContent.GetComponent<UIGridCreatorBase>();
            if (m_titleActivateGridCreator == null)
                m_titleActivateGridCreator = m_trans_activateContent.gameObject.AddComponent<UIGridCreatorBase>();
            m_titleActivateGridCreator.gridContentOffset = new UnityEngine.Vector2(0, 0);
            m_titleActivateGridCreator.arrageMent = UIGridCreatorBase.Arrangement.Vertical;
            m_titleActivateGridCreator.gridWidth = 250;
            m_titleActivateGridCreator.gridHeight = 40;

            m_titleActivateGridCreator.RefreshCheck();
            m_titleActivateGridCreator.Initialize<UITitlePropAddGrid>((uint)GridID.Uititlepropgrid, UIManager.OnObjsCreate, UIManager.OnObjsRelease, OnTitleActivateAddPropGridDataUpdate, OnTitlePropGridUIEvent);
        }

    }

    void CreateGrids()
    {
        m_titleForeverGridCreator.CreateGrids((null != m_lstAllForeverAddeffectTypeId) ? m_lstAllForeverAddeffectTypeId.Count : 0);

        m_titleActivateGridCreator.CreateGrids((null != m_lstActivateAdd) ? m_lstActivateAdd.Count : 0);
    }

    /// <summary>
    /// 跟新永久加成 grid数据
    /// </summary>
    /// <param name="data"></param>
    /// <param name="index"></param>
    private void OnTitleForeverAddPropGridDataUpdate(UIGridBase data, int index)
    {
        if (m_lstAllForeverAddeffectTypeId != null && index < m_lstAllForeverAddeffectTypeId.Count)
        {
            UITitlePropAddGrid grid = data as UITitlePropAddGrid;
            if (grid == null)
            {
                return;
            }

            grid.InitAllAddGrid(m_lstAllForeverAddeffectTypeId[index], m_dicEachTypeForeverAdd[m_lstAllForeverAddeffectTypeId[index]]);
        }
    }

    /// <summary>
    /// 跟新激活加成 grid数据
    /// </summary>
    /// <param name="data"></param>
    /// <param name="index"></param>
    private void OnTitleActivateAddPropGridDataUpdate(UIGridBase data, int index)
    {
        if (m_lstActivateAdd != null && index < m_lstActivateAdd.Count)
        {
            UITitlePropAddGrid grid = data as UITitlePropAddGrid;
            if (grid == null)
            {
                return;
            }

            grid.Init(m_lstActivateAdd[index]);
        }
    }

    /// <summary>
    /// grid点击事件
    /// </summary>
    /// <param name="eventType"></param>
    /// <param name="data"></param>
    /// <param name="param"></param>
    private void OnTitlePropGridUIEvent(UIEventType eventType, object data, object param)
    {
        if (eventType == UIEventType.Click)
        {

        }
    }



    #region click

    void onClick_Btn_close_Btn(GameObject caster)
    {
        this.HideSelf();
    }

    #endregion
}

