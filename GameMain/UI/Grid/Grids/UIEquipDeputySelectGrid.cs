/************************************************************************************
 * Copyright (c) 2017  All Rights Reserved.
 * CLR版本： 4.0.30319.42000
 * 公司名称：ZQB
 * 命名空间：GameMain.UI.Grid.Grids
 * 创建人：  wenjunhua.zqgame
 * 文件名：  UIEquipDeputySelectGrid
 * 版本号：  V1.0.0.0
 * 创建时间：2/20/2017 10:04:22 AM
 * 描述：
 ************************************************************************************/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
class AttrTransData
{
    public UILabel Grade = null;

    public UILabel Des = null;

    public Transform Root = null;
}

class UIEquipDeputySelectGrid : UIItemInfoGridBase
{
    #region property
    private UILabel m_lab_name;
    private Transform m_ts_infos;
    private Transform m_ts_unload;
    private Transform m_ts_none;
    private Transform mtsInfoRoot;
    private uint m_uint_qwThisId;
    private Transform m_ts_AttrRoot;
    private AttrTransData[] m_TransDatas = null;
    public uint QWThisID
    {
        get
        {
            return m_uint_qwThisId;
        }
    }
    private int m_int_index;
    public int Index
    {
        get
        {
            return m_int_index;
        }
    }
    #endregion

    #region overridemethod
    protected override void OnAwake()
    {
        base.OnAwake();
        m_lab_name = CacheTransform.Find("Content/Infos/Name").GetComponent<UILabel>();
        if (null != m_lab_name)
        {
            m_lab_name.color = ColorManager.GetColor32OfType(ColorType.White);
        }
        m_ts_infos = CacheTransform.Find("Content/Infos");
        m_ts_none = CacheTransform.Find("Content/None");
        m_ts_unload = CacheTransform.Find("Content/Infos/Unload");
        InitItemInfoGrid(CacheTransform.Find("Content/Infos/InfoGridRoot/InfoGrid"));
        mtsInfoRoot = CacheTransform.Find("Content/Infos/InfoGridRoot");
        if (null != m_ts_unload)
        {
            UIEventListener.Get(m_ts_unload.gameObject).onClick = (obj) =>
                {
                    InvokeUIDlg(UIEventType.Click, this, true);
                };
        }
        m_ts_AttrRoot = CacheTransform.Find("Content/Infos/AttrRoot");
        if (null != m_ts_AttrRoot)
        {
            int size = 5;
            m_TransDatas = new AttrTransData[size];
            Transform tempTs = null;
            StringBuilder builder = new StringBuilder();
            AttrTransData tempData = null;
            for(int i=0;i < size;i++)
            {
                tempData = new AttrTransData();
                builder.Remove(0, builder.Length);
                builder.Append(i + 1);
                tempData.Root = m_ts_AttrRoot.Find(builder.ToString());

                builder.Remove(0,builder.Length);
                builder.Append(i + 1);
                builder.Append("/Content/Grade/Grade");
                tempTs = m_ts_AttrRoot.Find(builder.ToString());
                
                if (null != tempTs)
                {
                    tempData.Grade = tempTs.GetComponent<UILabel>();
                }

                builder.Remove(0, builder.Length);
                builder.Append(i + 1);
                builder.Append("/Content/Des");
                tempTs = m_ts_AttrRoot.Find(builder.ToString());
                if (null != tempTs)
                {
                    tempData.Des = tempTs.GetComponent<UILabel>();
                }
                m_TransDatas[i] = tempData;
            }
        }
    }
    public override void Release(bool depthRelease = true)
    {
        base.Release(depthRelease);
        if (null != m_baseGrid)
        {
            m_baseGrid.Release(false);
        }
    }
    #endregion

    #region Set

    public void SetIndex(int index)
    {
        this.m_int_index = index;
    }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="none"></param>
    /// <param name="qwThisId"></param>
    /// <param name="index"></param>
    public void SetGridViewData(bool none,uint qwThisId = 0)
    {
        ResetInfoGrid();
        this.m_uint_qwThisId = qwThisId;
        if (null != m_ts_none && m_ts_none.gameObject.activeSelf != none)
        {
            m_ts_none.gameObject.SetActive(none);
        }

        if (null != m_ts_infos && m_ts_infos.gameObject.activeSelf == none)
        {
            m_ts_infos.gameObject.SetActive(!none);
        }
        if (!none)
        {
            ResetInfoGrid();
            Equip equip = DataManager.Manager<ItemManager>().GetBaseItemByQwThisId<Equip>(qwThisId);
            if (null != equip)
            {
                SetBorder(true, equip.BorderIcon);
                SetIcon(true, equip.Icon);
                SetBindMask(equip.IsBind);
            }
            if (null != m_lab_name)
            {
                m_lab_name.text = equip.Name;
            }
            if (null != m_TransDatas)
            {
                EquipManager emgr = DataManager.Manager<EquipManager>();
                GameCmd.PairNumber pair = null;
                AttrTransData tempTransData = null;
                List<GameCmd.PairNumber> pairs = equip.GetAdditiveAttr();
                for(int i = 0,max = m_TransDatas.Length; i < max;i++)
                {
                    tempTransData = m_TransDatas[i];
                    if (null == tempTransData.Root)
                        continue;

                    if (null != pairs && pairs.Count > i)
                    {
                        if (!tempTransData.Root.gameObject.activeSelf)
                        {
                            tempTransData.Root.gameObject.SetActive(true);
                        }
                        pair = pairs[i];
                        if (null != tempTransData.Grade)
                        {
                            tempTransData.Grade.text = emgr.GetAttrGrade(pair).ToString();
                        }

                        if (null != tempTransData.Des)
                        {
                            tempTransData.Des.text = emgr.GetAttrDes(pair);
                        }
                    }
                    else if (tempTransData.Root.gameObject.activeSelf)
                    {
                        tempTransData.Root.gameObject.SetActive(false);
                    }
                }
            }
        }
    }
    #endregion

    #region UIEventCallBack
    protected override void InfoGridUIEventDlg(UIEventType eventType, object data, object param)
    {
        switch (eventType)
        {
            case UIEventType.Click:
                {
                    if (data is UIItemInfoGrid)
                    {
                        if (m_uint_qwThisId == 0)
                        {
                            base.InfoGridUIEventDlg(eventType, data, param);
                        }
                        else
                        {
                            Equip equip = DataManager.Manager<ItemManager>().GetBaseItemByQwThisId<Equip>(m_uint_qwThisId);
                            if (null != equip)
                            {
                                TipsManager.Instance.ShowItemTips(m_uint_qwThisId);
                            }
                        }
                    }
                }
                break;
        }

    }
    #endregion

}