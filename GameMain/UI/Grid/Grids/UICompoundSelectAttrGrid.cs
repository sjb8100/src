/************************************************************************************
 * Copyright (c) 2017  All Rights Reserved.
 * CLR版本： 4.0.30319.42000
 * 公司名称：ZQB
 * 命名空间：GameMain.UI.Grid.Grids
 * 创建人：  wenjunhua.zqgame
 * 文件名：  UICompoundSelectAttrGrid
 * 版本号：  V1.0.0.0
 * 创建时间：8/9/2017 3:25:23 PM
 * 描述：
 ************************************************************************************/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

class UICompoundSelectAttrGrid : UIItemInfoGridBase
{
    #region property
    private UIToggle m_toggle = null;

    private Transform selectMask = null;

    private Transform m_tsOpen = null;
    private Transform m_tsClose = null;
    private TweenRotation m_openAnim = null;

    private Transform m_tsCostRoot = null;
    private Transform m_tsFreeGet = null;

    private Transform m_tsCostMoney = null;
    private UISprite m_moneyIcon = null;

    private Transform m_tsCostProp = null;
    private UISprite m_propBorder = null;
    private UITexture m_propIcon = null;

    private UILabel m_costNum = null;

    private AttrTransData[] m_TransDatas = null;
    private TweenPosition m_idleAnim = null;
    public uint Index
    {
        get
        {
            return (null != compoundSelectData) ? compoundSelectData.Index : 0;
        }
    }

    public bool IsSelect
    {
        get
        {
            return (null != m_toggle) ? m_toggle.value : false;
        }
    }
    EquipManager.CompoudSelectAttrData compoundSelectData = null;
    #endregion

    #region overridemethod
    protected override void OnAwake()
    {
        base.OnAwake();
        Transform tempTrans = null;
        m_toggle = CacheTransform.Find("Content/OpenContent/Toggle").GetComponent<UIToggle>();
        selectMask = CacheTransform.Find("Content/OpenContent/SelectMask");
        if (null != m_toggle)
        {
            m_toggle.onChange.Add(new EventDelegate(()=>
                {
                    if (IsSelect && !callEvent)
                    {
                        callEvent = true;
                    }
                    InvokeCallBack();
                    if (null != selectMask && selectMask.gameObject.activeSelf != IsSelect)
                    {
                        selectMask.gameObject.SetActive(IsSelect);
                    }
                }));
        }

        m_tsOpen = CacheTransform.Find("Content/OpenContent");
        m_tsClose = CacheTransform.Find("Content/CloseContent");
        if (null != m_tsClose)
        {
            m_openAnim = m_tsClose.GetComponent<TweenRotation>();
            if (null != m_openAnim)
            {
                m_openAnim.AddOnFinished(() =>
                    {
                        OnAnimFinish();
                    });
            }
        }

        m_tsFreeGet = CacheTransform.Find("Content/CloseContent/FreeGet");
        m_tsCostRoot = CacheTransform.Find("Content/CloseContent/CostRoot");

        m_tsCostMoney = CacheTransform.Find("Content/CloseContent/CostRoot/Money");
        tempTrans = CacheTransform.Find("Content/CloseContent/CostRoot/Money/Icon");
        if (null != tempTrans)
        {
            m_moneyIcon = tempTrans.GetComponent<UISprite>();
        }

        m_tsCostProp = CacheTransform.Find("Content/CloseContent/CostRoot/Prop");
        tempTrans = CacheTransform.Find("Content/CloseContent/CostRoot/Prop/Icon");
        if (null != tempTrans)
        {
            m_propIcon = tempTrans.GetComponent<UITexture>();
        }

        tempTrans = CacheTransform.Find("Content/CloseContent/CostRoot/Prop/Broder");
        if (null != tempTrans)
        {
            m_propBorder = tempTrans.GetComponent<UISprite>();
        }

        tempTrans = CacheTransform.Find("Content/CloseContent/CostRoot/Num");
        if (null != tempTrans)
        {
            m_costNum = tempTrans.GetComponent<UILabel>();
        }

        tempTrans = CacheTransform.Find("Content/OpenContent/AttrRoot");
        if (null != tempTrans)
        {
            int size = 5;
            m_TransDatas = new AttrTransData[size];
            Transform tempTs = null;
            StringBuilder builder = new StringBuilder();
            AttrTransData tempData = null;
            for (int i = 0; i < size; i++)
            {
                tempData = new AttrTransData();
                builder.Remove(0, builder.Length);
                builder.Append(i + 1);
                tempData.Root = tempTrans.Find(builder.ToString());

                builder.Remove(0, builder.Length);
                builder.Append(i + 1);
                builder.Append("/Content/Grade/Grade");
                tempTs = tempTrans.Find(builder.ToString());

                if (null != tempTs)
                {
                    tempData.Grade = tempTs.GetComponent<UILabel>();
                }

                builder.Remove(0, builder.Length);
                builder.Append(i + 1);
                builder.Append("/Content/Des");
                tempTs = tempTrans.Find(builder.ToString());
                if (null != tempTs)
                {
                    tempData.Des = tempTs.GetComponent<UILabel>();
                }
                m_TransDatas[i] = tempData;
            }
        }

        InitItemInfoGrid(CacheTransform.Find("Content/OpenContent/InfoGridRoot/InfoGrid"), false);
    }
    //public override void OnClick()
    //{
    //    SetSelect(!IsSelect, false);
    //    base.OnClick();
    //}

    public override void Release(bool depthRelease = true)
    {
        base.Release(depthRelease);
        if (null != m_baseGrid)
        {
            m_baseGrid.Release(false);
        }

        if (null != m_moneyIconCASD)
        {
            m_moneyIconCASD.Release(true);
        }

        if (null != m_propBorderCASD)
        {
            m_propBorderCASD.Release(true);
        }

        if (null != m_propIconCASD)
        {
            m_propIconCASD.Release(true);
        }

        compoundSelectData = null;
    }
    #endregion

    #region OP
    /// <summary>
    /// 设置数据
    /// </summary>
    /// <param name="open"></param>
    /// <param name="data"></param>
    /// <param name="select"></param>
    /// <param name="freeGet"></param>
    public void SetData(bool open,EquipManager.CompoudSelectAttrData data = null,bool select = false,bool freeGet = true)
    {
        m_bSelect = open && select;
        compoundSelectData = data;
        if (open)
        {
            OpenCard(false);
        }else
        {
            if (null != m_openAnim)
            {
                m_openAnim.ResetToBeginning();
            }
            UpdateCloseCostStatus(freeGet);
        }
    }

    private void SetTransformState(bool open)
    {
        if (null != m_tsClose && m_tsClose.gameObject.activeSelf == open)
        {
            m_tsClose.gameObject.SetActive(!open);
        }
        if (null != m_tsOpen && m_tsOpen.gameObject.activeSelf != open)
        {
            m_tsOpen.gameObject.SetActive(open);
        }
    }

    private void SetOpenData()
    {
        if (null == compoundSelectData)
        {
            return;
        }
        SetTransformState(true);
        ResetInfoGrid();
        BaseItem baseItem = DataManager.Manager<ItemManager>().GetTempBaseItemByBaseID<BaseItem>(compoundSelectData.BaseID);
        int attrNum = (null != compoundSelectData.Attrs) ? compoundSelectData.Attrs.Count : 0;
        SetIcon(true, baseItem.Icon);
        SetBorder(true, ItemDefine.GetItemBorderIcon((uint)attrNum));
        if (compoundSelectData.IsBind)
        {
            SetBindMask(true);
        }
        if (null != m_TransDatas)
        {
            AttrTransData temData = null;
            GameCmd.PairNumber pair = null;
            EquipManager emgr = DataManager.Manager<EquipManager>();

            for (int i = 0, max = m_TransDatas.Length; i < max; i++)
            {
                temData = m_TransDatas[i];
                if (temData.Root == null)
                    continue;
                if (null != compoundSelectData.Attrs && compoundSelectData.Attrs.Count > i)
                {
                    if (!temData.Root.gameObject.activeSelf)
                    {
                        temData.Root.gameObject.SetActive(true);
                    }
                    pair = compoundSelectData.Attrs[i];
                    if (null != temData.Grade)
                    {
                        temData.Grade.text = emgr.GetAttrGrade(compoundSelectData.Attrs[i]).ToString();
                    }

                    if (null != temData.Des)
                    {
                        temData.Des.text = emgr.GetAttrDes(compoundSelectData.Attrs[i]);
                    }
                }
                else if (temData.Root.gameObject.activeSelf)
                {
                    temData.Root.gameObject.SetActive(false);
                }
            }
        }
        SetSelect(m_bSelect);
    }

    bool callEvent = true;
    private void InvokeCallBack()
    {
        if (callEvent)
        {
            InvokeUIDlg(UIEventType.Click, this, null);
        }

        if (!callEvent)
            callEvent = true;
        
    }

    private void OnAnimFinish()
    {
        SetOpenData();
    }

    public void OpenCard(bool needAnim = true)
    {
       if (needAnim && null != m_openAnim)
       {
           m_openAnim.ResetToBeginning();
           m_openAnim.PlayForward();
       }
       else
       {
           SetOpenData();
       }
    }

    private CMResAsynSeedData<CMTexture> m_propIconCASD = null;
    private CMResAsynSeedData<CMAtlas> m_propBorderCASD = null;
    private CMResAsynSeedData<CMAtlas> m_moneyIconCASD = null;
    public void UpdateCloseCostStatus(bool freeGet)
    {
        if (null == compoundSelectData)
        {
            return;
        }
        SetTransformState(false);
        if (null != m_tsCostRoot)
        {
            if (m_tsCostRoot.gameObject.activeSelf == freeGet)
            {
                m_tsCostRoot.gameObject.SetActive(!freeGet);
            }
            if (!freeGet)
            {
                int holdNum = DataManager.Manager<ItemManager>().GetItemNumByBaseId(compoundSelectData.CostItemID);
                bool isEnough = holdNum >= compoundSelectData.CostItemNum;
                string numString = "";
                if (null != m_tsCostProp)
                {
                    if (m_tsCostProp.gameObject.activeSelf != isEnough)
                    {
                        m_tsCostProp.gameObject.SetActive(isEnough);
                    }
                    if (isEnough)
                    {
                        BaseItem baseItem = DataManager.Manager<ItemManager>().GetTempBaseItemByBaseID<BaseItem>(compoundSelectData.CostItemID);
                        if (null != m_propBorder)
                        {
                            UIManager.GetAtlasAsyn(baseItem.BorderIcon, ref m_propBorderCASD, () =>
                            {
                                if (null != m_propBorder)
                                {
                                    m_propBorder.atlas = null;
                                }
                            }, m_propBorder, false);
                        }

                        if (null != m_propIcon)
                        {
                            uint resID = DataManager.Manager<UIManager>().GetResIDByFileName(false,baseItem.Icon);
                            UIManager.GetTextureAsyn(resID, ref m_propIconCASD, () =>
                            {
                                if (null != m_propIcon)
                                {
                                    m_propIcon.mainTexture = null;
                                }
                            }, m_propIcon, false);
                        }
                        numString = string.Format("x{0}", compoundSelectData.CostItemNum);
                    }
                }

                if (null != m_tsCostMoney)
                {
                    if (m_tsCostMoney.gameObject.activeSelf == isEnough)
                    {
                        m_tsCostMoney.gameObject.SetActive(!isEnough);
                    }

                    if (!isEnough)
                    {
                        if (null != m_moneyIcon)
                        {
                            m_moneyIconCASD = new CMResAsynSeedData<CMAtlas>(()=>
                            {
                                if (null != m_moneyIcon)
                                {
                                    m_moneyIcon.atlas = null;
                                }
                            });
                            string iconName = MallDefine.GetCurrencyIconNameByType(compoundSelectData.ReplaceCostMoneyType);
                            UIManager.GetAtlasAsyn(iconName, ref m_moneyIconCASD, () =>
                            {
                                if (null != m_moneyIcon)
                                {
                                    m_moneyIcon.atlas = null;
                                }
                            }, m_moneyIcon, false);
                        }
                        numString = compoundSelectData.ReplaceCostMoneyNum.ToString();
                    }
                }

                if (null != m_costNum)
                {
                    m_costNum.text = numString;
                }
            }
        }

        if (null != m_tsFreeGet && m_tsFreeGet.gameObject.activeSelf != freeGet)
        {
            m_tsFreeGet.gameObject.SetActive(freeGet);
        }
    }

    private bool m_bSelect = false;
    public void SetSelect(bool select)
    {
        m_bSelect = select;
        callEvent = select;

        if (null != m_toggle && m_toggle.value != select)
        {
            m_toggle.value = select;
        }

        if (null != selectMask && selectMask.gameObject.activeSelf != select)
        {
            selectMask.gameObject.SetActive(select);
        }
    }

    #endregion
}