using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

class UIEquipPropertyGrid :UIGridBase
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
    #endregion
    protected override void OnAwake()
    {
        base.OnAwake();
        property = CacheTransform.Find("Content/Property").GetComponent<UILabel>();
        gradeCotent = CacheTransform.Find("Content/GradeContent");
        if (null != CacheTransform.Find("Content/GradeContent/Grade"))
            propertyGrade = CacheTransform.Find("Content/GradeContent/Grade").GetComponent<UILabel>();
        if (null != CacheTransform.Find("Content/Mark"))
            mark = CacheTransform.Find("Content/Mark");
    }

    #region OverrideMethod
    #endregion

    #region Set
    public void SetProperty(string propertyString)
    {
        if (null != property)
            property.text = propertyString;
    }

    /// <summary>
    /// 设置格子View
    /// </summary>
    /// <param name="txt"></param>
    /// <param name="needMask"></param>
    /// <param name="needGrade"></param>
    /// <param name="grade"></param>
    public void SetGridView(string txt,bool needMask ,bool needGrade,uint grade = 1)
    {
        if (null != gradeCotent)
        {
            if (gradeCotent.gameObject.activeSelf != needGrade)
                gradeCotent.gameObject.SetActive(needGrade);
            if (needGrade)
            {
                if (null != propertyGrade)
                    propertyGrade.text = "" + grade;
            }
        }
        if (null != mark)
        {
            if (mark.gameObject.activeSelf != needMask)
                mark.gameObject.SetActive(needMask);
        }

        if (null != property)
        {
            property.text = txt;
        }
    }

    #endregion
}