/************************************************************************************
 * Copyright (c) 2016  All Rights Reserved.
 * CLR版本： 4.0.30319.42000
 * 公司名称：ZQB
 * 命名空间：GameMain.Game.Muhon
 * 创建人：  wenjunhua.zqgame
 * 文件名：  MuhonPanel_Upgrade
 * 版本号：  V1.0.0.0
 * 创建时间：10/15/2016 12:20:43 PM
 * 描述：
 ************************************************************************************/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

partial class MuhonPanel
{
    #region GlobalUIEvent
    void OnExpChange(object data)
    {
        if (status == TabMode.ShengJi)
        {
            UpdateDataUIByStatus();
        }

    }

    void OnMuhonLevelUp()
    {
        if (status == TabMode.ShengJi)
        {
            UpdateDataUIByStatus();
        }
    }
    #endregion

    #region property
    private UIItemGrowShowGrid m_promoteGrow = null;
    #endregion

    #region init
    private void InitPromoteWidgets()
    {
        if (IsInitStatus(TabMode.ShengJi))
        {
            return;
        }
        SetInitStatus(TabMode.ShengJi, true);
        if (null != m_trans_PromoteItemGrowRoot && null == m_promoteGrow)
        {
            Transform ts = UIManager.AddGridTransform(GridID.Uiitemgrowshowgrid,m_trans_PromoteItemGrowRoot);
            if (null != ts)
            {

                m_promoteGrow = ts.GetComponent<UIItemGrowShowGrid>();
                if (null == m_promoteGrow)
                    m_promoteGrow = ts.gameObject.AddComponent<UIItemGrowShowGrid>();
            }
            
        }
        if (null != m_slider_PromoteExpProgress)
        {
            SlideAnimation slideAnim = m_slider_PromoteExpProgress.GetComponent<SlideAnimation>();
            if (null == slideAnim)
                m_slider_PromoteExpProgress.gameObject.AddComponent<SlideAnimation>();
        }

        CreateMuhonExpGrid();
    }

    private bool mbCreateMuhonExp = false;
    private List<UIMuhonExpGrid> m_lst_expGrids = null;
    /// <summary>
    /// 创建圣魂经验丹格子
    /// </summary>
    private void CreateMuhonExpGrid()
    {
        if (null == m_trans_UIMuhonExpGrid)
        {
            return;
        }
        List<uint> muhonExpItemsId = emgr.MuhonExpItemIds;
        if (!mbCreateMuhonExp)
        {
            m_lst_expGrids = new List<UIMuhonExpGrid>();
            if (null == muhonExpItemsId || muhonExpItemsId.Count == 0)
            {
                Engine.Utility.Log.Error("获取圣魂经验丹id列表Error");
                return;
            }
            muhonExpItemsId.Sort();
            if (null != m_grid_PromoteExpRoot)
            {
                UIEventDelegate dlg = (eventType, data, param) =>
                {
                    switch (eventType)
                    {
                        case UIEventType.Click:
                        case UIEventType.LongPressing:
                            {
                                UIMuhonExpGrid commonGrid = data as UIMuhonExpGrid;
                                UserMuhonExp(commonGrid.BaseId);
                            }
                            break;
                    }
                };
                Transform tempTrans = null;
                UIMuhonExpGrid expGrid = null;
                GameObject cloneObj = null;
                for (int i = 0; i < muhonExpItemsId.Count; i++)
                {
                    cloneObj = NGUITools.AddChild(m_grid_PromoteExpRoot.gameObject, m_trans_UIMuhonExpGrid.gameObject);
                    if (null == cloneObj)
                        continue;
                    tempTrans = cloneObj.transform;
                    if (null == tempTrans)
                    {
                        continue;
                    }
                    tempTrans.name = i.ToString();
                    expGrid = tempTrans.GetComponent<UIMuhonExpGrid>();
                    if (null == expGrid)
                    {
                        expGrid = tempTrans.gameObject.AddComponent<UIMuhonExpGrid>();
                    }
                    expGrid.RegisterUIEventDelegate(dlg);
                    expGrid.SetGridData(muhonExpItemsId[i], 1);
                    m_lst_expGrids.Add(expGrid);
                }
                m_grid_PromoteExpRoot.Reposition();
            }

            mbCreateMuhonExp = true;
        }

        if (null != m_lst_expGrids)
        {
            for(int i =0,max = m_lst_expGrids.Count;i < max;i++)
            {
                m_lst_expGrids[i].SetGridData(muhonExpItemsId[i], 1);
            }
        }
       
        
    }

    /// <summary>
    /// 初始化数据
    /// </summary>
    private void InitPromoteData()
    {
        CreateMuhonExpGrid();
    }
    #endregion

    #region Op
    /// <summary>
    /// 使用圣魂经验丹
    /// <param name="baseId"></param>
    /// </summary>
    private void UserMuhonExp(uint baseId)
    {
        Muhon itemData = Data;
        if (null != itemData)
        {
            int num = imgr.GetItemNumByBaseId(baseId);
            if (num > 0)
            {
                if (itemData.IsMaxLv)
                {
                    TipsManager.Instance.ShowTips("已升至最高等级");
                    return;
                }
                emgr.AddWeaponSoulExp(itemData.QWThisID, baseId,1);
            }
            else
            {
                TipsManager.Instance.ShowItemTips(baseId);
                DataManager.Manager<UIPanelManager>().ShowPanel(PanelID.GetWayPanel, data: baseId);
            }
        }
    }
    private void UpdatePromote(Muhon data)
    {
        if (null == data)
            return;
        if (null == data.LocalUpgradeDataBase)
        {
            Engine.Utility.Log.Error(CLASS_NAME + "->upGradeDataBase null");
            return;
        }

        //刷新圣魂升级信息
        if (null != m_promoteGrow)
        {
            m_promoteGrow.SetGridData(data.QWThisID);
        }
        
        //刷新等级经验信息
        if (null != m_label_Level)
        {
            //当前等级：{0}/{1}   {2}需求等级：{3}
            ColorType cTye = ColorType.Green;
            if (DataManager.Instance.PlayerLv < data.BaseData.useLevel)
            {
                cTye = ColorType.Red;
            }
            string color = ColorManager.GetNGUIColorOfType(ColorType.Gray);
            m_label_Level.text = string.Format("{0}/{1}",data.Level,data.MaxLv);
                //= DataManager.Manager<TextManager>().GetLocalFormatText(LocalTextType.Local_TXT_Soul_Level
                //, data.Level, data.MaxLv, ColorUtil.GetNGUIColorOfType(cTye), data.BaseData.useLevel);
        }
        float expPercentage = (data.IsMaxLv) ? 1f : (float)data.Exp / data.UpgradeExp;
        if (null != m_label_PromoteExpPencentage)
            m_label_PromoteExpPencentage.text = string.Format("{0}%", (expPercentage * 100).ToString("f1"));
        if (null != m_slider_PromoteExpProgress)
        {
            SlideAnimation slideAnim = m_slider_PromoteExpProgress.GetComponent<SlideAnimation>();
            if (null != slideAnim)
                slideAnim.DoSlideAnim(slideAnim.value, expPercentage);
        }
        //刷新基础属性预览
        List<EquipDefine.EquipBasePropertyData> basePropertyList = emgr.GetWeaponSoulBasePropertyData(data.BaseId, data.Level);
        int count = (null != basePropertyList) ? basePropertyList.Count : 0;

        table.WeaponSoulUpgradeDataBase nextUpgradeDataBase = null;
        List<EquipDefine.EquipBasePropertyData> nexBasePropertyList = null;
        if (!data.IsMaxLv)
        {
            nexBasePropertyList = emgr.GetWeaponSoulBasePropertyData(data.BaseId, data.Level + 1);
        }

        if (null != m_trans_BasePropertyLevel)
        {
            m_trans_BasePropertyLevel.Find("Content/Name").GetComponent<UILabel>().text = "等级";
            m_trans_BasePropertyLevel.Find("Content/Current").GetComponent<UILabel>().text = data.Level + "";
            m_trans_BasePropertyLevel.Find("Content/Target").GetComponent<UILabel>().text = (data.IsMaxLv) ? "满级" : "" + (data.Level + 1);
        }
        EquipDefine.EquipBasePropertyData temp = null;
        if (null != m_trans_BaseProperty1)
        {
            if (count >= 1)
            {
                temp = basePropertyList[0];
            }

            m_trans_BaseProperty1.Find("Content/Name").GetComponent<UILabel>().text = (null != temp) ? temp.Name : "无";
            m_trans_BaseProperty1.Find("Content/Current").GetComponent<UILabel>().text = (null != temp) ? temp.ToString() : "无";
            m_trans_BaseProperty1.Find("Content/Target").GetComponent<UILabel>().text = (!data.IsMaxLv && null != nexBasePropertyList && nexBasePropertyList.Count >= 1) ? nexBasePropertyList[0].ToString() : "满级";
        }
        if (null != m_trans_BaseProperty2)
        {
            if (count >= 2)
            {
                temp = basePropertyList[1];
            }

            m_trans_BaseProperty2.Find("Content/Name").GetComponent<UILabel>().text = (null != temp) ? temp.Name : "无";
            m_trans_BaseProperty2.Find("Content/Current").GetComponent<UILabel>().text = (null != temp) ? temp.ToString() : "无";
            m_trans_BaseProperty2.Find("Content/Target").GetComponent<UILabel>().text = (!data.IsMaxLv && null != nexBasePropertyList && nexBasePropertyList.Count >= 2) ? nexBasePropertyList[1].ToString() : "满级";
        }
        if (null != m_trans_BaseProperty3)
        {
            if (count >= 3)
            {
                temp = basePropertyList[2];
            }

            m_trans_BaseProperty3.Find("Content/Name").GetComponent<UILabel>().text = (null != temp) ? temp.Name : "无";
            m_trans_BaseProperty3.Find("Content/Current").GetComponent<UILabel>().text = (null != temp) ? temp.ToString() : "无";
            m_trans_BaseProperty3.Find("Content/Target").GetComponent<UILabel>().text = (!data.IsMaxLv && null != nexBasePropertyList && nexBasePropertyList.Count >= 3) ? nexBasePropertyList[2].ToString() : "满级";
        }
        if (null != m_trans_BaseProperty4)
        {
            if (count >= 4)
            {
                temp = basePropertyList[3];
            }

            m_trans_BaseProperty4.Find("Content/Name").GetComponent<UILabel>().text = (null != temp) ? temp.Name : "无";
            m_trans_BaseProperty4.Find("Content/Current").GetComponent<UILabel>().text = (null != temp) ? temp.ToString() : "无";
            m_trans_BaseProperty4.Find("Content/Target").GetComponent<UILabel>().text = (!data.IsMaxLv && null != nexBasePropertyList && nexBasePropertyList.Count >= 4) ? nexBasePropertyList[3].ToString() : "满级";
        }
    }

    private void ReleaseUpgrade(bool depthClear =true)
    {
        if (null != m_promoteGrow)
        {
            m_promoteGrow.Release(depthClear);
            if (depthClear)
            {
                UIManager.OnObjsRelease(m_promoteGrow.CacheTransform, (uint)GridID.Uiitemgrowshowgrid);
                m_promoteGrow = null;
            }
        }
        
        if (null != m_lst_expGrids)
        {
            if (depthClear)
            {
                for (int i = 0, max = m_lst_expGrids.Count; i < max; i++)
                {
                    m_lst_expGrids[i].Release(depthClear);
                }
                m_lst_expGrids.Clear();
                m_lst_expGrids = null;
            }
        }
    }
    #endregion

}