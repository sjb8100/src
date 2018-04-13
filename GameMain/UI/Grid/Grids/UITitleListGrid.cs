using Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using table;
using UnityEngine;

class UITitleListGrid : UIGridBase
{
    UILabel m_lblName;
    UISprite m_spSelected;//选择背景
    GameObject m_goWarning;//警告
    GameObject m_goWear;   //穿
    GameObject m_goUse;    //使用、激活
    UISprite bg;
    UIButton m_btn;

    public uint m_titleId;
    protected override void OnAwake()
    {
        base.OnAwake();
        m_lblName = this.transform.Find("TitleName").GetComponent<UILabel>();
        bg = this.transform.Find("bg").GetComponent<UISprite>();
        m_spSelected = this.transform.Find("select").GetComponent<UISprite>();
        m_goWarning = this.transform.Find("mark_warning").gameObject;
        m_goWear = this.transform.Find("mark_Attached").gameObject;
        m_goUse = this.transform.Find("mark_Use").gameObject;
        m_btn = this.transform.GetComponent<UIButton>();
    }


    public override void SetGridData(object data)
    {
        base.SetGridData(data);
        this.m_titleId = (uint)data;

        List<TitleDataBase> tableTitleList = GameTableManager.Instance.GetTableList<TitleDataBase>();
        TitleManager titleManager = DataManager.Manager<TitleManager>();

        TitleDataBase tdb = tableTitleList.Find((d) => { return d.dwID == this.m_titleId; });
        m_lblName.text = tdb.strName;

        if (this.m_titleId == titleManager.WearTitleId)//穿的
        {
            m_goWear.SetActive(true);
        }
        else
        {
            m_goWear.SetActive(false);
        }

        if (this.m_titleId == titleManager.ActivateTitleId) //激活的
        {
            m_goUse.SetActive(true);
        }
        else
        {
            m_goUse.SetActive(false);
        }
    }

    public override void Reset()
    {

    }

    /// <summary>
    /// 设置高亮状态
    /// </summary>
    /// <param name="select"></param>
    public override void SetHightLight(bool hightLight)
    {
        base.SetHightLight(hightLight);
    }

    #region Set
    /// <summary>
    /// 设置背景
    /// </summary>
    /// <param name="spriteName"></param>
    public void SetBg(string spriteName)
    {
        UIManager.GetAtlasAsyn(spriteName, ref m_playerAvataCASD, () =>
        {
            if (null != bg)
            {
                bg.atlas = null;
            }
        }, bg);
      
    }
    CMResAsynSeedData<CMAtlas> m_playerAvataCASD = null;
    public override void Release(bool depthRelease = true)
    {
        base.Release(depthRelease);
        if (m_playerAvataCASD != null)
        {
            m_playerAvataCASD.Release(depthRelease);
            m_playerAvataCASD = null;
        }
    }

    protected override void OnUIBaseDestroy()
    {
        base.OnUIBaseDestroy();
        Release();
    }

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
                notGet = "（未获得）";
            }
            m_lblName.text = string.Format("{0}{1}", name, notGet);
        }
        
    }

    ///// <summary>
    ///// 未获得
    ///// </summary>
    ///// <param name="b"></param>
    //public void SetDidNotGet(bool b)
    //{
    //    if (m_btn != null)
    //    {
    //        string notGet = "";
    //        if (b == false)
    //        {
    //              notGet = "（未获得）";
    //        } 
    //        m_lblName.text = string.Format("{0}{1}",name ,notGet);
    //    }
        
    //}

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
        if (m_goUse != null && m_goUse.activeSelf != activate)
        {
            m_goUse.SetActive(activate);
        }

    }

    /// <summary>
    /// 新获得称号标记
    /// </summary>
    /// <param name="newTitle"></param>
    public void SetNewMark(bool newTitle)
    {
        if (m_goWarning != null && m_goWarning.activeSelf != newTitle)
        {
            m_goWarning.SetActive(newTitle);
        }
    }



    #endregion
}

