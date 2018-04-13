/************************************************************************************
 * Copyright (c) 2016  All Rights Reserved.
 * CLR版本： 4.0.30319.42000
 * 公司名称：ZQB
 * 命名空间：GameMain.Game.Muhon
 * 创建人：  wenjunhua.zqgame
 * 文件名：  MuhonPanel_Blend
 * 版本号：  V1.0.0.0
 * 创建时间：10/15/2016 12:21:56 PM
 * 描述：
 ************************************************************************************/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
partial class MuhonPanel
{
    #region property
    //需要融合的圣魂id
    private uint blendSelectMuhonId = 0;
    private Dictionary<EquipDefine.AttrIndex, UIMuhonPropertyGrid> m_dic_currentAttrs = null;
    private Dictionary<EquipDefine.AttrIndex, UIMuhonPropertyGrid> m_dic_blendAtrs = null;
    private UIItemGrowShowGrid m_blendCurGrow = null;
    private UIItemGrowShowGrid m_blendNextGrow = null;
    #endregion

    #region init
    private void InitBlendWidgets()
    {
        if (IsInitStatus(TabMode.RongHe))
        {
            return;
        }
        SetInitStatus(TabMode.RongHe, true);
        Transform clone = null;
        if (null != m_trans_BlendCurShowGrid && null == m_blendCurGrow)
        {
            m_blendCurGrow = m_trans_BlendCurShowGrid.GetComponent<UIItemGrowShowGrid>();
            if (null == m_blendCurGrow)
                m_blendCurGrow = m_trans_BlendCurShowGrid.gameObject.AddComponent<UIItemGrowShowGrid>();
        }

        if (null != m_trans_BlendNextShowGrid && null == m_blendNextGrow)
        {
            m_blendNextGrow = m_trans_BlendNextShowGrid.GetComponent<UIItemGrowShowGrid>();
            if (null == m_blendNextGrow)
                m_blendNextGrow = m_trans_BlendNextShowGrid.gameObject.AddComponent<UIItemGrowShowGrid>();
            m_blendNextGrow.RegisterUIEventDelegate((UIEventType eventType, object data, object param) =>
            {
                if (eventType == UIEventType.Click)
                {
                    OnSelectBlendMuhon();
                }
            });
        }
        m_dic_currentAttrs = new Dictionary<EquipDefine.AttrIndex, UIMuhonPropertyGrid>();
        m_dic_blendAtrs = new Dictionary<EquipDefine.AttrIndex, UIMuhonPropertyGrid>();
        UIMuhonPropertyGrid eInfoGrid = null;
        if ( null != m_grid_CurrentAdditiveAttrContent && null != m_grid_BlendAdditiveAttrContent)
        {
            for (EquipDefine.AttrIndex i = EquipDefine.AttrIndex.First;
                i <= EquipDefine.AttrIndex.Fifth; i++)
            
            {
                clone = UIManager.AddGridTransform(GridID.Uimuhonpropertygrid, m_grid_CurrentAdditiveAttrContent.transform);
                if (null != clone)
                {
                    clone.name = ((int)i).ToString();
                    eInfoGrid = clone.GetComponent<UIMuhonPropertyGrid>();
                    if (null == eInfoGrid)
                    {
                        eInfoGrid = clone.gameObject.AddComponent<UIMuhonPropertyGrid>();
                    }
                    if (null != eInfoGrid)
                    {
                        m_dic_currentAttrs.Add(i, eInfoGrid);
                    }
                }

                clone = UIManager.AddGridTransform(GridID.Uimuhonpropertygrid, m_grid_BlendAdditiveAttrContent.transform);
                if (null != clone)
                {
                    clone.name = ((int)i).ToString();
                    eInfoGrid = clone.GetComponent<UIMuhonPropertyGrid>();
                    if (null == eInfoGrid)
                    {
                        eInfoGrid = clone.gameObject.AddComponent<UIMuhonPropertyGrid>();
                    }
                    if (null != eInfoGrid)
                    {
                        m_dic_blendAtrs.Add(i, eInfoGrid);
                    }
                }
            }
            m_grid_CurrentAdditiveAttrContent.Reposition();
            m_grid_BlendAdditiveAttrContent.Reposition();
        }
    }

    /// <summary>
    /// 初始化数据
    /// </summary>
    private void InitBlendData()
    {

    }
    #endregion

    #region Op
    /// <summary>
    /// 重置Blend
    /// </summary>
    private void ResetBlend()
    {
        blendSelectMuhonId = 0;
    }

    /// <summary>
    /// 点击圣魂融合选择
    /// </summary>
    private void OnSelectBlendMuhon()
    {
        if (DataManager.Manager<UIPanelManager>().IsShowPanel(PanelID.ItemChoosePanel))
            return;
        Muhon itemData = Data;
        if (null == itemData)
        {
            TipsManager.Instance.ShowTips(LocalTextType.Local_TXT_Soul_ChooseBlendMuhonTips);
            return;
        }

        if (!emgr.IsItemHaveAdditiveAttrOrGemHole(itemData.QWThisID, false))
        {
            TipsManager.Instance.ShowTips(LocalTextType.Local_TXT_Soul_ActiveTips);
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
            Muhon tempData = null;

            for (int i = 0; i < muhonList.Count; i++)
            {
                if (itemData.IsMatchBlend(muhonList[i],false))
                {
                    data.Add(muhonList[i]);
                }
            }
        }
        if (data.Count == 0)
        {
            TipsManager.Instance.ShowTips(string.Format("背包中未找到消耗所需{0}{1}"
                , ColorManager.GetNGUIColorOfType(ColorType.Red), itemData.Name));
            return;
        }

        data.Sort((left,right) =>
            {
                Muhon leftM = imgr.GetBaseItemByQwThisId<Muhon>(left);
                Muhon rightM = imgr.GetBaseItemByQwThisId<Muhon>(right);
                if (rightM.IsActive)
                {
                    if (leftM.IsActive)
                    {
                        return rightM.Level - leftM.Level;
                    }
                    return 1;
                }else if (leftM.IsActive)
                {
                    return -1;
                }else
                {
                    return rightM.Level - leftM.Level;
                }
                return 0;
            });

        UILabel desLabel = null;
        Action<UILabel> desLabAction = (des) =>
        {
            desLabel = des;
            if (null != desLabel)
                desLabel.text = DataManager.Manager<TextManager>()
                    .GetLocalText(LocalTextType.Local_TXT_Soul_ChooseActiveMuhonNotice);
        };
        uint tempSelectMuhonId = blendSelectMuhonId;
        UIWeaponSoulInfoSelectGrid tempSelectWeaponSoulGrid = null;
        UIEventDelegate eventDlg = (eventTye, grid, param) =>
        {
            if (eventTye == UIEventType.Click)
            {
                UIWeaponSoulInfoSelectGrid tempGrid = grid as UIWeaponSoulInfoSelectGrid;
                if (tempSelectMuhonId == tempGrid.QWThisId)
                {
                    tempSelectMuhonId = 0;
                    tempGrid.SetHightLight(false);
                }
                else if (itemData.IsMatchBlend(tempGrid.QWThisId))
                {
                    tempSelectMuhonId = tempGrid.QWThisId;
                    if (null != tempSelectWeaponSoulGrid)
                        tempSelectWeaponSoulGrid.SetHightLight(false);
                    tempGrid.SetHightLight(true);
                    tempSelectWeaponSoulGrid = tempGrid;
                }else
                {
                    TipsManager.Instance.ShowTips(LocalTextType.Local_TXT_Soul_UnactiveNotice);
                }
            }
        };

        ItemChoosePanel.ItemChooseShowData showData = new ItemChoosePanel.ItemChooseShowData()
        {
            createNum = data.Count,
            title = "选择圣魂",
            cloneTemp = UIManager.GetResGameObj(GridID.Uiweaponsoulinfoselectgrid) as GameObject,
            onChooseCallback = () =>
            {
                if (tempSelectMuhonId != blendSelectMuhonId)
                {
                    blendSelectMuhonId = tempSelectMuhonId;
                    UpdateBlend(Data);
                }
            },
            gridCreateCallback = (obj, index) =>
            {
                if (index >= data.Count)
                {
                    Engine.Utility.Log.Error("OnSelectBlendMuhon error,index out of range");
                }
                else
                {
                    Muhon tempItemData = imgr.GetBaseItemByQwThisId<Muhon>(data[index]);
                    if (null == tempItemData)
                    {
                        Engine.Utility.Log.Error("OnSelectBlendMuhon error,get itemData qwThisId:{0}", data[index]);
                    }
                    else
                    {
                        UIWeaponSoulInfoSelectGrid weaponsoulInfoGrid = obj.GetComponent<UIWeaponSoulInfoSelectGrid>();
                        if (null == weaponsoulInfoGrid)
                            weaponsoulInfoGrid = obj.AddComponent<UIWeaponSoulInfoSelectGrid>();
                        weaponsoulInfoGrid.RegisterUIEventDelegate(eventDlg);
                        if (!itemData.IsMatchBlend(tempSelectMuhonId))
                        {
                            tempSelectMuhonId = 0;
                        }
                        bool select = (data[index] == tempSelectMuhonId);
                        weaponsoulInfoGrid.SetGridViewData(tempItemData.QWThisID, select, itemData.IsMatchBlend(data[index]),true);
                        if (select)
                        {
                            tempSelectWeaponSoulGrid = weaponsoulInfoGrid;
                            tempSelectMuhonId = data[index];
                        }
                    }
                }

            },
            desPassCallback = desLabAction,
        };
        DataManager.Manager<UIPanelManager>().ShowPanel(PanelID.ItemChoosePanel, data: showData);
    }

    void OnMuhonBlend(uint muhonQwThisId)
    {
        blendSelectMuhonId = 0;
        UpdateBlend(Data);
        DataManager.Manager<UIPanelManager>().ShowPanel(PanelID.MuhonBlendCompletePanel, data: muhonQwThisId);
    }

    /// <summary>
    /// 设置融合辅助
    /// </summary>
    private void SetBlendAssist()
    {
        Muhon data = Data;
        if (null == data)
            return;

        SetCommonCost(data.BlenNeedItemId, data.BlenNeedItemNum, data.BlendCost, GameCmd.MoneyType.MoneyType_Gold
            , data.BlenNeedItemNum * DataManager.Manager<MallManager>().GetDQPriceByItem(data.BlenNeedItemId), GameCmd.MoneyType.MoneyType_Coin);
    }

    private void UpdateBlend(Muhon data)
    {
        if (null == data)
            return;
        Muhon blendData = (blendSelectMuhonId != 0) ? imgr.GetBaseItemByQwThisId<Muhon>(blendSelectMuhonId) : null;
        //刷新圣魂升级信息
        if (null != m_blendCurGrow)
        {
            m_blendCurGrow.SetGridData(data.QWThisID);
        }
        if (null != m_blendNextGrow)
        {
            m_blendNextGrow.SetGridData((null != blendData) ? blendData.QWThisID : 0);
        }
        bool choose = (blendSelectMuhonId != 0 && imgr.ExistItem(blendSelectMuhonId)) ? true : false;
        int attrOpenNum = (data.StartLevel == 0) ? 1 : (int)data.StartLevel;

        //<id,是否为原属性>
        Dictionary<uint, bool> retainAdditiveAttrDic = new Dictionary<uint, bool>();
        //Cur Additive Attr
        Dictionary<uint, GameCmd.PairNumber> curAddtiveAttrsDic = new Dictionary<uint, GameCmd.PairNumber>();
        foreach (GameCmd.PairNumber pair in data.GetAdditiveAttr())
        {
            curAddtiveAttrsDic.Add(pair.id, pair);
        }
        //Blend Additive Attr
        Dictionary<uint, GameCmd.PairNumber> blendAddtiveAttrsDic = new Dictionary<uint, GameCmd.PairNumber>();
        if (null != blendData)
        {
            foreach (GameCmd.PairNumber pair in blendData.GetAdditiveAttr())
            {
                blendAddtiveAttrsDic.Add(pair.id, pair);
            }
        }

        if (null != m_btn_BlendUnload
                    && m_btn_BlendUnload.gameObject.activeSelf != choose)
        {
            m_btn_BlendUnload.gameObject.SetActive(choose);
        }

        if (choose)
        {
            bool inCur = true;
            foreach (GameCmd.PairNumber pairCur in curAddtiveAttrsDic.Values)
            {
                if (retainAdditiveAttrDic.Count >= attrOpenNum)
                {
                    continue;
                }
                inCur = true;
                if (blendAddtiveAttrsDic.ContainsKey(pairCur.id)
                    && blendAddtiveAttrsDic[pairCur.id].value > pairCur.value)
                {
                    inCur = false;
                }
                retainAdditiveAttrDic.Add(pairCur.id, inCur);

            }
            foreach (GameCmd.PairNumber pairBlend in blendAddtiveAttrsDic.Values)
            {
                if (retainAdditiveAttrDic.Count >= attrOpenNum)
                {
                    continue;
                }
                if (!retainAdditiveAttrDic.ContainsKey(pairBlend.id))
                    retainAdditiveAttrDic.Add(pairBlend.id, false);
            }
        }

        List<GameCmd.PairNumber> curTempAttrList
            = new List<GameCmd.PairNumber>(curAddtiveAttrsDic.Values);
        List<GameCmd.PairNumber> blendTempAttrList 
            = new List<GameCmd.PairNumber>(blendAddtiveAttrsDic.Values);
        string attrDes;
        bool chooseAttr = false;
        uint attrGrade = 0;
        UIMuhonPropertyGrid muhonGrid = null;
        GameCmd.PairNumber pairNum = null;
        int index = 0;
        
        for (EquipDefine.AttrIndex i = EquipDefine.AttrIndex.First; 
            i <= EquipDefine.AttrIndex.Fifth;i++ )
        {
            index = (int)i - 1;
            //填充当前圣魂属性
            if (m_dic_currentAttrs.TryGetValue(i, out muhonGrid))
            {
                if ((index + 1) <= attrOpenNum)
                {
                    if (curTempAttrList.Count > index)
                    {
                        pairNum = curTempAttrList[index];
                        attrDes = emgr.GetAttrDes(pairNum);
                        chooseAttr = (retainAdditiveAttrDic.ContainsKey(pairNum.id) && retainAdditiveAttrDic[pairNum.id]) ? true : false;
                        attrGrade = emgr.GetAttrGrade(pairNum);
                        muhonGrid.SetGridView(true, false, txt: attrDes, needMask :choose, grade: attrGrade, check: chooseAttr);
                    }
                    else
                    {
                        muhonGrid.SetGridView(true);
                    }
                }else
                {
                    muhonGrid.SetGridView(false, lockDes: tmgr.GetMuhonAttrLockDes(i));
                }
                
            }

            //填充融合圣魂属性
            if (m_dic_blendAtrs.TryGetValue(i,out muhonGrid))
            {
                if ((index + 1) <= attrOpenNum)
                {
                    if (blendTempAttrList.Count > index)
                    {
                        pairNum = blendTempAttrList[index];
                        attrDes = emgr.GetAttrDes(pairNum);
                        chooseAttr = (retainAdditiveAttrDic.ContainsKey(pairNum.id)
                            && !retainAdditiveAttrDic[pairNum.id]) ? true : false;
                        attrGrade = emgr.GetAttrGrade(pairNum);
                        muhonGrid.SetGridView(true, false, txt: attrDes, needMask: choose, grade: attrGrade, check: chooseAttr);
                    }
                    else
                    {
                        muhonGrid.SetGridView(true);
                    }
                }
                else
                {
                    muhonGrid.SetGridView(false, lockDes: tmgr.GetMuhonAttrLockDes(i));
                }
            }
        }

        //显示Tips
        if (null != m_trans_BlendTips && m_trans_BlendTips.gameObject.activeSelf != choose)
        {
            m_trans_BlendTips.gameObject.SetActive(choose);
        }
        //辅助
        SetBlendAssist();
    }
    #endregion

    #region UIEvent

    void onClick_BlendBtn_Btn(GameObject caster)
    {
        if (!imgr.ExistItem(selectedMuhonId))
        {
            TipsManager.Instance.ShowTips(LocalTextType.Local_TXT_Soul_InvalidMuhonReChoose);
            return;
        }

        if (!imgr.ExistItem(blendSelectMuhonId))
        {
            TipsManager.Instance.ShowTips(LocalTextType.Local_TXT_Soul_ChooseBlendMuhonTips);
            return;
        }

        Muhon data = Data;
        if (null == data)
            return;
        int assistNum = imgr.GetItemNumByBaseId(data.BlenNeedItemId);
        if (assistNum < 1 && !m_bool_autoUseDQ)
        {
            TipsManager.Instance.ShowTips("辅助道具不足,无法进行融合");
            return;
        }
        emgr.BlendWeaponSoul(selectedMuhonId, blendSelectMuhonId, m_bool_autoUseDQ);
    }

    void onClick_BlendUnload_Btn(GameObject caster)
    {
        if (blendSelectMuhonId != 0)
        {
            blendSelectMuhonId = 0;
            UpdateBlend(Data);
        }
    }

    private void ReleaseBlend(bool depthClear = true)
    {
        uint resID = (uint)GridID.Uiitemgrowshowgrid;
        if (null != m_blendCurGrow)
        {
            m_blendCurGrow.Release(depthClear);
            if (depthClear)
            {
                UIManager.OnObjsRelease(m_blendCurGrow.CacheTransform, resID);
            }
        }

        if (null != m_blendNextGrow)
        {
            m_blendNextGrow.Release(depthClear);
            if (depthClear)
            {
                UIManager.OnObjsRelease(m_blendNextGrow.CacheTransform, resID);
            }
        }

        if (depthClear)
        {
            resID = (uint)GridID.Uimuhonpropertygrid;
            UIMuhonPropertyGrid pg = null;
            for (EquipDefine.AttrIndex i = EquipDefine.AttrIndex.First;
                i <= EquipDefine.AttrIndex.Fifth; i++)
            {
                if (null != m_dic_currentAttrs)
                {
                    if (m_dic_currentAttrs.TryGetValue(i, out pg))
                    {
                        UIManager.OnObjsRelease(pg.CacheTransform, resID);
                    }
                }

                if (null != m_dic_blendAtrs)
                {
                    if (m_dic_blendAtrs.TryGetValue(i, out pg))
                    {
                        UIManager.OnObjsRelease(pg.CacheTransform, resID);
                    }
                }
            }

            if (null != m_dic_currentAttrs)
            {
                m_dic_currentAttrs.Clear();
            }

            if (null != m_dic_blendAtrs)
            {
                m_dic_blendAtrs.Clear();
            }
        }
    }
    #endregion
}