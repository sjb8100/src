
using Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using table;
using UnityEngine;

class UITitleSecondTypeGrid : UIGridBase
{
    UILabel m_lblName;
    UISprite m_spSelected;//选择背景
    GameObject m_goWarning;//警告
    GameObject m_goWear;   //佩戴
    GameObject m_goActivate;    //激活
    UIButton m_btn;

    //一级页签Id;
    uint m_firstKeyId;
    public uint FirstKeyId
    {
        get
        {
            return m_firstKeyId;
        }
    }

    //称号Id  二级页签Id
    uint m_titleId;
    public uint TitleId
    {
        get
        {
            return m_titleId;
        }
    }


    protected override void OnAwake()
    {
        base.OnAwake();
        m_lblName = this.transform.Find("TitleName").GetComponent<UILabel>();
        m_spSelected = this.transform.Find("select").GetComponent<UISprite>();
        m_goWarning = this.transform.Find("mark_warning").gameObject;
        m_goWear = this.transform.Find("mark_Attached").gameObject;
        m_goActivate = this.transform.Find("mark_Use").gameObject;
        m_btn = this.transform.GetComponent<UIButton>();
    }


    public void SetGridData(uint firstKeyId, uint titleId)
    {
        this.m_firstKeyId = firstKeyId;
        this.m_titleId = titleId;

        List<TitleDataBase> tableTitleList = GameTableManager.Instance.GetTableList<TitleDataBase>();
        TitleManager titleManager = DataManager.Manager<TitleManager>();

        TitleDataBase tdb = tableTitleList.Find((d) => { return d.dwID == this.m_titleId; });

        //拥有未拥有
        if (titleManager.OwnedTitleList.Exists((d) => { return d.dwID == this.m_titleId ? true : false; }))
        {
            SetName(tdb.strName, true);
        }
        else
        {
            SetName(tdb.strName, false);
        }

        //佩戴称号  激活称号
        //限时  限次   限时限次
        if (tdb.timeliness == (uint)PropPanel.Timeliness.TimeLimit || tdb.timeliness == (uint)PropPanel.Timeliness.NumLimit || tdb.timeliness == (uint)PropPanel.Timeliness.TimeNumLimit)
        {
            GameCmd.stTitleData titleData = titleManager.OwnedTitleList.Find((d) => { return d.dwID == this.m_titleId; });
            if (titleData != null)
            {
                //有时间剩余  或次数剩余
                if (titleData.dwTime > 0 || titleData.dwCount > 0)
                {
                    SetWearMark(this.m_titleId == titleManager.WearTitleId);
                    SetActivateMark(this.m_titleId == titleManager.ActivateTitleId);
                }
                else
                {
                    SetWearMark(false);
                    SetActivateMark(false);
                }
            }
            else
            {
                SetWearMark(false);
                SetActivateMark(false);
            }
        }
        //永久称号
        else
        {
            SetWearMark(this.m_titleId == titleManager.WearTitleId);
            SetActivateMark(this.m_titleId == titleManager.ActivateTitleId);
        }
    }

    public override void Reset()
    {

    }

    /// <summary>
    /// 设置高亮状态(选中)
    /// </summary>
    /// <param name="select"></param>
    //public override void SetHightLight(bool hightLight)
    //{
    //    base.SetHightLight(hightLight);

    //    if (m_spSelected != null && m_spSelected.gameObject.activeSelf != hightLight)
    //    {
    //        m_spSelected.gameObject.SetActive(hightLight);
    //    }
    //}

    #region Set


    /// <summary>
    /// 设置是否装备
    /// </summary>
    /// <param name="equip"></param>
    public void SetEquip(bool equip)
    {

    }

    /// <summary>
    /// 设置名称
    /// </summary>
    /// <param name="name"></param>
    public void SetName(string name, bool isGet = false)
    {
        if (m_lblName != null)
        {
            string notGet = "";
            if (isGet == false)
            {
                notGet = "(未获得)";
            }
            m_lblName.text = string.Format("{0}{1}", name, notGet);
        }

    }

    /// <summary>
    /// 设置Icon
    /// </summary>
    /// <param name="spriteName"></param>
    /// <param name="atlas"></param>
    public void SetIcon(string spriteName)
    {

    }

    /// <summary>
    /// 设置选中标示
    /// </summary>
    /// <param name="select"></param>
    public void SetSelect(bool select)
    {
        if (m_spSelected != null && m_spSelected.gameObject.activeSelf != select)
        {
            m_spSelected.gameObject.SetActive(select);
        }
    }


    /// <summary>
    /// 佩戴标记 
    /// </summary>
    /// <param name="wear"></param>
    public void SetWearMark(bool wear)
    {

        if (m_goWear != null && m_goWear.activeSelf != wear)
        {
            m_goWear.SetActive(wear);
        }


    }

    /// <summary>
    /// 激活标记
    /// </summary>
    /// <param name="Activate"></param>
    public void SetActivateMark(bool activate)
    {
        if (m_goActivate != null && m_goActivate.activeSelf != activate)
        {
            m_goActivate.SetActive(activate);
        }

    }

    /// <summary>
    /// 新获得称号标记
    /// </summary>
    /// <param name="newTitle"></param>
    public void SetRedPointStatus(bool newTitle)
    {
        if (m_goWarning != null && m_goWarning.activeSelf != newTitle)
        {
            m_goWarning.SetActive(newTitle);
        }
    }



    #endregion
}



