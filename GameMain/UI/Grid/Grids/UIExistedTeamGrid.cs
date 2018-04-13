using Client;
using GameCmd;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using table;
using UnityEngine;


class UIExistedTeamGrid : UIGridBase
{
    UILabel m_lblTargetName;
    UILabel m_lblName;
    UILabel m_lblLv;
    UITexture m_spIcon;
    GameObject applyBtn;
    GameObject memberRoot;

    public ConvenientTeamInfo m_teamInfo;

    private CMResAsynSeedData<CMTexture> iuiIconAtlas = null;

    public override void Release(bool depthRelease = true)
    {
        base.Release(depthRelease);
        if (null != iuiIconAtlas)
        {
            iuiIconAtlas.Release(true);
            iuiIconAtlas = null;
        }
    }

    protected override void OnUIBaseDestroy()
    {
        base.OnUIBaseDestroy();
        Release();
    }

    protected override void OnAwake()
    {
        base.OnAwake();
        m_lblTargetName = this.transform.Find("target_name").GetComponent<UILabel>();
        m_lblName = this.transform.Find("name").GetComponent<UILabel>();
        m_lblLv = this.transform.Find("level").GetComponent<UILabel>();
        m_spIcon = this.transform.Find("icon_head").GetComponent<UITexture>();
        applyBtn = this.transform.Find("btn_apply").gameObject;
        memberRoot = this.transform.Find("memberRoot").gameObject;

        UIEventListener.Get(applyBtn).onClick = OnClickApplyBtn;
        UIEventListener.Get(m_spIcon.gameObject).onClick = OnClickIconBtn;
    }

    public override void SetGridData(object data)
    {
        base.SetGridData(data);
        this.m_teamInfo = data as ConvenientTeamInfo;
        SetMember(this.m_teamInfo.memDataList);
    }



    public override void Reset()
    {
        base.Reset();
    }

    void SetMember(List<ConvenientTeamInfoMemData> memData)
    {
        for (int i = 0; i < memberRoot.transform.childCount; i++)
        {
            Transform memberTransf = memberRoot.transform.GetChild(i);
            if (i < memData.Count)
            {
                memberTransf.gameObject.SetActive(true);


                SetIcon(memData[i].job);

                UILabel levelLbl = memberTransf.GetComponentInChildren<UILabel>();
                if (levelLbl != null)
                {
                    levelLbl.text = string.Format("{0}级", memData[i].level);
                }

            }
            else
            {
                memberTransf.gameObject.SetActive(false);
            }
        }
    }

    public void SetTargetName(string name)
    {
        if (m_lblTargetName != null)
        {
            m_lblTargetName.text = name;
        }
    }

    public void SetName(string name)
    {
        if (m_lblName != null)
        {
            m_lblName.text = name;
        }
    }

    public void SetIcon(uint job)
    {
        if (m_spIcon != null)
        {
            SelectRoleDataBase sdb = table.SelectRoleDataBase.Where((GameCmd.enumProfession)job, (GameCmd.enmCharSex)1);
            if (sdb != null)
            {
                UIManager.GetTextureAsyn(sdb.strprofessionIcon, ref iuiIconAtlas, () => { if (m_spIcon != null) { m_spIcon.mainTexture = null; } }, m_spIcon,false);
            }
        }
    }

    public void SetLv(uint lv)
    {
        if (m_lblLv != null)
        {
            m_lblLv.text = lv.ToString();
        }
    }

    public void SetBtnState()
    {

    }

    //申请入队
    public void OnClickApplyBtn(GameObject obj)
    {
        int btnIndex = 1;
        object param = btnIndex;
        InvokeUIDlg(UIEventType.Click, this, param);
    }

    /// <summary>
    /// 点Icon
    /// </summary>
    /// <param name="obj"></param>
    public void OnClickIconBtn(GameObject obj)
    {
        int btnIndex = 2;
        object param = btnIndex;
        InvokeUIDlg(UIEventType.Click, this, param);
    }


    //string GetJobIconByJodId(uint jobId)
    //{
    //    string IconName = "tubiao_zhanshi"; 
    //    switch(jobId)
    //    {
    //        case 1: IconName ="tubiao_zhanshi"; break;
    //        case 2: IconName = "tubiao_huanshi"; break;
    //        case 3: IconName = "tubiao_fashi"; break;
    //        case 4: IconName = "tubiao_anwu"; break;
    //    }

    //    return IconName;
    //}

    //string GetMemberIconByJobId(uint jobId)
    //{
    //    string IconName = "touxiang_zheng_zhanshi"; 
    //    switch(jobId)
    //    {
    //        case 1: IconName = "touxiang_zheng_zhanshi"; break;
    //        case 2: IconName = "touxiang_zheng_huanshi"; break;
    //        case 3: IconName = "touxiang_zheng_fashi"; break;
    //        case 4: IconName = "touxiang_zheng_anwu"; break;
    //    }

    //    return IconName;
    //}

}

