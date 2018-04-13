/************************************************************************************
 * Copyright (c) 2018  All Rights Reserved.
 * CLR版本： 4.0.30319.42000
 * 公司名称：ZQB
 * 命名空间：GameMain.Game.FB
 * 创建人：  wenjunhua.zqgame
 * 文件名：  FBPassAwardPanel
 * 版本号：  V1.0.0.0
 * 创建时间：4/3/2018 3:00:27 PM
 * 描述：
 ************************************************************************************/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

/// <summary>
/// 通用奖励数据
/// </summary>
public class CommonAwardData
{
    //id
    private uint baseId = 0;
    public uint BaseId
    {
        get
        {
            return baseId;
        }
    }

    //数量
    private uint num = 0;
    public uint Num
    {
        get
        {
            return num;
        }
    }

    public CommonAwardData(uint bid,uint num)
    {
        this.baseId = bid;
        this.num = num;
    }
}

partial class FBPassAwardPanel
{
    #region property
    private Vector2 gridGap = Vector2.zero;
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
        List<CommonAwardData> awardData = null;
        if (null != data && data is List<CommonAwardData>)
        {
            awardData = (List<CommonAwardData>)data;
        }
        CreateAwardUI(awardData);
    }

    protected override void OnHide()
    {
        base.OnHide();
        ReleaseGrid();
    }

    public override void OnColliderMaskClicked()
    {
        base.OnColliderMaskClicked();
        HideSelf();
    }
    #endregion

    #region Op

    private void InitWidgets()
    {
        if (null != m_trans_UIAwardItemGrid)
        {
           Bounds bounds = NGUIMath.CalculateRelativeWidgetBounds(m_trans_UIAwardItemGrid,true);
           size = bounds.size;
        }
        gridGap = new Vector2(10, 0);
    }

    private List<UIAwardItemGrid> lastShowGrids = null;
    private Vector3 size = Vector3.zero;
    private void CreateAwardUI(List<CommonAwardData> awardDatas)
    {
        if (null != m_trans_AwardContent && null != m_trans_UIAwardItemGrid)
        {
            int cacheNum = (null != lastShowGrids) ? lastShowGrids.Count : 0;
            int createNum = (null != awardDatas) ? awardDatas.Count : 0;
            List<UIAwardItemGrid> tempCacheGrid = new List<UIAwardItemGrid>();
            if (null != lastShowGrids && lastShowGrids.Count > 0)
            {
                tempCacheGrid.AddRange(lastShowGrids);
            }
            
            Vector3 startPos = Vector3.zero;
            int div = createNum / 2;
            if (createNum % 2 == 0)
            {
                startPos.x = -((div - 1) * gridGap.x + 0.5f * gridGap.x + (div - 1) * size.x + 0.5f * size.x);
            }else
            {
                startPos.x = -(div * (gridGap.x + size.x));
            }

            int loopCount = Mathf.Max(cacheNum, createNum);
            UIAwardItemGrid tempAwardGrid = null;
            GameObject cloneObj = null;
            for(int i = 0;i < loopCount;i ++)
            {
                if (i < createNum)
                {
                    if (i < cacheNum)
                    {
                        tempAwardGrid = tempCacheGrid[i];
                    }else
                    {
                        cloneObj = GameObject.Instantiate(m_trans_UIAwardItemGrid.gameObject);
                        if (null == cloneObj)
                        {
                            continue;
                        }
                       tempAwardGrid = cloneObj.AddComponent<UIAwardItemGrid>();
                       tempAwardGrid.CacheTransform.parent = m_trans_AwardContent;
                       tempAwardGrid.CacheTransform.localScale = Vector3.one;
                       if (null == lastShowGrids)
                       {
                           lastShowGrids = new List<UIAwardItemGrid>();
                       }
                       lastShowGrids.Add(tempAwardGrid);
                    }

                    tempAwardGrid.CacheTransform.localPosition = startPos;
                    tempAwardGrid.SetGridData(awardDatas[i].BaseId, awardDatas[i].Num);
                    startPos.x += (gridGap.x + size.x);
                }
                else
                {
                    tempAwardGrid = tempCacheGrid[i];
                    if (null != tempAwardGrid)
                    {
                        tempAwardGrid.Release();
                        tempAwardGrid.SetVisible(false);
                    }
                }
                
            }
        }
    }


    private void ReleaseGrid()
    {
        if (null != lastShowGrids)
        {
            UIAwardItemGrid tempGrid = null;
            for(int i = 0,max = lastShowGrids.Count;i < max;i++)
            {
                tempGrid = lastShowGrids[i];
                if (null != tempGrid)
                {
                    tempGrid.Release();
                }
            }
        }
    }

    #endregion

}