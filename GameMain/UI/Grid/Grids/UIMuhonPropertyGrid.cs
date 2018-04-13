/************************************************************************************
 * Copyright (c) 2016  All Rights Reserved.
 * CLR版本： 4.0.30319.42000
 * 公司名称：ZQB
 * 命名空间：GameMain.UI.Grid.Grids
 * 创建人：  wenjunhua.zqgame
 * 文件名：  UIMuhonPropertyGrid
 * 版本号：  V1.0.0.0
 * 创建时间：12/28/2016 4:57:42 PM
 * 描述：
 ************************************************************************************/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

class UIMuhonPropertyGrid : UIGridBase
{
    #region Property
    //属性
    private UILabel property;
    //档次
    private Transform gradeCotent;
    //档次
    private UILabel propertyGrade;
    //选择标记
    private Transform mark;
    //对
    private Transform m_ts_markRight;
    //错
    private Transform m_ts_markError;
    //关闭
    private Transform mtsClose = null;
    //
    private Transform mtsOpen = null;
    //未开启描述
    private UILabel m_labCloseDes = null;
    //Infos
    private Transform mTsInfos = null;
    #endregion
    protected override void OnAwake()
    {
        base.OnAwake();
        property = CacheTransform.Find("Content/Open/Infos/Property").GetComponent<UILabel>();
        gradeCotent = CacheTransform.Find("Content/Open/Infos/GradeContent");
        propertyGrade = CacheTransform.Find("Content/Open/Infos/GradeContent/Grade").GetComponent<UILabel>();
        mark = CacheTransform.Find("Content/Open/Infos/Mark");
        m_ts_markRight = CacheTransform.Find("Content/Open/Infos/Mark/Proper");
        m_ts_markError = CacheTransform.Find("Content/Open/Infos/Mark/Error");
        mTsInfos = CacheTransform.Find("Content/Open/Infos");
        mtsOpen = CacheTransform.Find("Content/Open");
        mtsClose = CacheTransform.Find("Content/Close");
        m_labCloseDes = CacheTransform.Find("Content/Close/LockDes").GetComponent<UILabel>();
    }

    #region OverrideMethod
    #endregion

    #region Set

    /// <summary>
    /// 设置格子View
    /// </summary>
    /// <param name="txt"></param>
    /// <param name="needMask"></param>
    /// <param name="needGrade"></param>
    /// <param name="grade"></param>
    public void SetGridView(bool isOpen,bool isNone = true,string lockDes = ""
        ,string txt = "", bool needMask = false, uint grade = 1, bool check = false)
    {
        if (null != mtsClose && mtsClose.gameObject.activeSelf == isOpen)
        {
            mtsClose.gameObject.SetActive(!isOpen);
        }

        if (null != mtsOpen && mtsOpen.gameObject.activeSelf != isOpen)
        {
            mtsOpen.gameObject.SetActive(isOpen);
        }

        if (isOpen)
        {
            if (null != mTsInfos && mTsInfos.gameObject.activeSelf == isNone)
            {
                mTsInfos.gameObject.SetActive(!isNone);
            }

            if (!isNone)
            {
                if (null != propertyGrade)
                    propertyGrade.text = grade.ToString();

                if (null != mark)
                {
                    if (mark.gameObject.activeSelf != needMask)
                        mark.gameObject.SetActive(needMask);
                    if (needMask)
                    {
                        if (null != m_ts_markRight && m_ts_markRight.gameObject.activeSelf != check)
                        {
                            m_ts_markRight.gameObject.SetActive(check);
                        }
                        if (null != m_ts_markError && m_ts_markError.gameObject.activeSelf == check)
                        {
                            m_ts_markError.gameObject.SetActive(!check);
                        }
                    }
                }

                if (null != property)
                {
                    property.text = txt;
                }
            }
        }if (null != m_labCloseDes)
        {
            m_labCloseDes.text = lockDes;
        }
    }

    #endregion
}