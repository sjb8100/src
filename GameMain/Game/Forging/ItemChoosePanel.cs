using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;


partial class ItemChoosePanel
{
    #region define
    public class ItemChooseShowData
    {
        //克隆模板
        public GameObject cloneTemp;
        //title
        public string title;
        //生成列表数据
        public int createNum;
        //选择按钮回调
        public Action onChooseCallback;
        //关闭按钮回调
        public Action onCloseCallback;
        //格子生成回调
        public Action<GameObject, int> gridCreateCallback;
        //选择面板描述改变回调
        public Action<UILabel> desPassCallback;
        //null Tips
        public string nullTips = "";
    }
    #endregion
    #region property
    //面板显示数据
    private ItemChooseShowData data;
    #endregion
    #region overridemethod
    protected override void OnLoading()
    {
        base.OnLoading();
    }

    protected override void OnPrepareShow(object data)
    {
        base.OnPrepareShow(data);
        ClearPreGrid();
    }

    protected override void OnShow(object data)
    {
        base.OnShow(data);
        if (null == data)
        {
            Engine.Utility.Log.Error("Show ItemChoosePanel Error showdata null");
            return;
        }
        this.data = data as ItemChooseShowData;
        CreateItemChooseUI();
    }

    protected override void OnPanelBaseDestory()
    {
        base.OnPanelBaseDestory();
        
    }
    #endregion

    #region Op

    /// <summary>
    /// 清空上一个
    /// </summary>
    private void ClearPreGrid()
    {
        if (null != m_grid_GridContent)
        {
            m_grid_GridContent.transform.DestroyChildren();
        }
    }
    /// <summary>
    /// 生成物品选择UI
    /// </summary>
    private void CreateItemChooseUI()
    {
        if (null == m_grid_GridContent || null == data || null == data.cloneTemp)
            return;
        if (null != m_label_Title)
            m_label_Title.text = data.title;
        bool isNull = (data.createNum == 0) ? true :false;
        if (null != m_label_NullTips)
        {
            if (m_label_NullTips.gameObject.activeSelf != isNull)
            {
                m_label_NullTips.gameObject.SetActive(isNull);
            }
            if (isNull)
            {
                m_label_NullTips.text = data.nullTips;
            }
        }
        if (!isNull)
        {
            GameObject cloneObj = null;
            UIDragScrollView dragScrollVeiw = null;
            for (int i = 0; i < data.createNum; i++)
            {
                cloneObj = NGUITools.AddChild(m_grid_GridContent.gameObject, data.cloneTemp);
                if (null == cloneObj)
                    continue;
                dragScrollVeiw = cloneObj.GetComponent<UIDragScrollView>();
                if (null == dragScrollVeiw)
                    dragScrollVeiw = cloneObj.AddComponent<UIDragScrollView>();
                if (null != data.gridCreateCallback)
                {
                    data.gridCreateCallback.Invoke(cloneObj, i);
                }
            }
            if (null != data.desPassCallback)
            {
                data.desPassCallback.Invoke(m_label_Des);
            }
            m_grid_GridContent.Reposition();
            if (null != m_scrollview_SelectScrollView)
                m_scrollview_SelectScrollView.ResetPosition();
        }
    }
    #endregion

    #region UIEvent
    void onClick_Close_Btn(GameObject caster)
    {

        if (null != data && null != data.onCloseCallback)
            data.onCloseCallback.Invoke();

        HideSelf();
    }

    void onClick_SelectBtn_Btn(GameObject caster)
    {
       
        if (null != data && null != data.onChooseCallback)
            data.onChooseCallback.Invoke();

        HideSelf();
    }

    #endregion
}