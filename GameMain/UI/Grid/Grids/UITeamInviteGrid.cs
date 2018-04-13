using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using table;
using UnityEngine;


class UITeamInviteGrid : UIGridBase
{
    UITexture m_spIcon;
    UILabel m_lblLv;
    UILabel m_lblName;
    UIButton m_btnInvite;
    UILabel m_lblInvite;

    public TeamDataManager.People m_people;

    private CMResAsynSeedData<CMTexture> iuiIconAtlas = null;

    TeamDataManager TDManager
    {
        get
        {
            return DataManager.Manager<TeamDataManager>();
        }
    }


    #region overridemethod

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
        m_spIcon = this.transform.Find("icon").GetComponent<UITexture>();
        m_lblLv = this.transform.Find("level").GetComponent<UILabel>();
        m_lblName = this.transform.Find("name").GetComponent<UILabel>();
        m_btnInvite = this.transform.Find("btn_invite").GetComponent<UIButton>();
        m_lblInvite = this.transform.Find("btn_invite/Label").GetComponent<UILabel>();

        UIEventListener.Get(m_btnInvite.gameObject).onClick = OnClickInvite;
    }

    public override void SetGridData(object data)
    {
        base.SetGridData(data);

        this.m_people = data as TeamDataManager.People;

        m_lblLv.text = string.Format("等级{0}", this.m_people.lv);
        m_lblName.text = this.m_people.name;
        if (this.m_people.alreadyInvite)
        {
            m_btnInvite.isEnabled = false;
            m_lblInvite.text = "已邀请";
        }
        else
        {
            m_btnInvite.isEnabled = true;
            m_lblInvite.text = "邀请";
        }
    }

    #endregion

    public void SetIcon(uint job)
    {
        if (m_spIcon != null)
        {
            m_spIcon.gameObject.SetActive(true);
            //m_spIcon.ChangeSprite(job);

            SelectRoleDataBase sdb = table.SelectRoleDataBase.Where((GameCmd.enumProfession)job, (GameCmd.enmCharSex)1);
            if (sdb != null)
            {
                //DataManager.Manager<UIManager>().SetSpriteDynamicIcon(m_spIcon, sdb.strprofessionIcon, false);

                UIManager.GetTextureAsyn(sdb.strprofessionIcon, ref iuiIconAtlas, () =>
                {
                    if (m_spIcon != null)
                    {
                        m_spIcon.mainTexture = null;
                    }
                 }, m_spIcon);
            }
        }
    }

    void SetAtlasNull()
    {
        if (m_spIcon != null)
        {
            m_spIcon.mainTexture = null;
        }
    }

    void OnClickInvite(GameObject go)
    {
        int btnIndex = 1;

        InvokeUIDlg(UIEventType.Click, this, btnIndex);

        m_btnInvite.isEnabled = false;
        m_lblInvite.text = "已邀请";
    }

}