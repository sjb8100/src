using Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using table;
using UnityEngine;


class UIApplyListGrid : UIGridBase
{
    GameObject mark_Leader;
    UILabel level;
    UILabel name;
    UITexture icon;
    UISpriteEx mark_job;
    GameObject bg_role;
    UIButton btn_agree;
    UIButton btn_refuse;

    //IRenerTextureObj rtObj;
    //GameObject rtGo;

    public TeamMemberInfo teamMemberInfo;

    CMResAsynSeedData<CMTexture> m_iconSeedData = null;

    public override void Release(bool depthRelease = true)
    {
        base.Release(depthRelease);
        if (null != m_iconSeedData)
        {
            m_iconSeedData.Release(true);
            m_iconSeedData = null;
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
        //mark_Leader = transform.Find("mark_leader").gameObject;
        //mark_Leader.SetActive(false);
        level = transform.Find("level").GetComponent<UILabel>();
        name = transform.Find("name").GetComponent<UILabel>();
        icon = transform.Find("avatarRoot/HeadIcon").GetComponent<UITexture>();
        mark_job = transform.Find("mark_job").GetComponent<UISpriteEx>();
        //rtGo = transform.Find("avatarRoot/CharacterRenderTexture").gameObject;

        bg_role = transform.Find("bg_role").gameObject;
        btn_agree = transform.Find("btn_agree").GetComponent<UIButton>();
        btn_refuse = transform.Find("btn_refuse").GetComponent<UIButton>();

        UIEventListener.Get(btn_agree.gameObject).onClick = OnClickAgreeBtn;
        UIEventListener.Get(btn_refuse.gameObject).onClick = OnClickRefuse;
    }


    public override void SetGridData(object data)
    {
        base.SetGridData(data);
        this.teamMemberInfo = data as TeamMemberInfo;
    }

    public void SetName(string namestr)
    {
        if (name != null)
        {
            name.text = namestr;
        }
    }

    public void SetIcon(uint job)
    {
        if (icon != null)
        {
            // icon.ChangeSprite(job);

            SelectRoleDataBase sdb = table.SelectRoleDataBase.Where((GameCmd.enumProfession)job, (GameCmd.enmCharSex)1);
            if (sdb != null)
            {
                UIManager.GetTextureAsyn(sdb.jobBigIcon, ref m_iconSeedData, () =>
                {
                    if (icon != null)
                    {
                        icon.mainTexture = null;
                    }
                }, icon);
            }
        }
    }

    public void SetLevel(uint lv)
    {
        if (level != null)
        {
            level.text = lv.ToString();
        }
    }

    public void setJob(uint job)
    {
        if (mark_job != null)
        {
            mark_job.ChangeSprite(job);
        }
    }


    //public void SetModel(TeamMemberInfo teamMemberInfo)
    //{
    //    if (rtObj == null)
    //    {
    //        rtObj = DataManager.Manager<RenderTextureManager>().CreateRenderTextureObj(teamMemberInfo.suitdData, (int)teamMemberInfo.job, (int)GameUtil.FaceToSex(teamMemberInfo.faceId), 512);
    //        if (rtObj == null)
    //        {
    //            return;
    //        }

    //        UIRenderTexture rt = rtGo.GetComponent<UIRenderTexture>();
    //        if (rt == null)
    //            rt = rtGo.AddComponent<UIRenderTexture>();
    //        rtObj.SetModelRotateY(180);
    //        rtObj.SetModelScale(0.7f);
    //        rtObj.SetCamera(new Vector3(0, 1f, 0f), new Vector3(15, 0, 0), 4f);
    //        rt.SetDepth(3);
    //        rt.Initialize(rtObj, 180f, new Vector2(205f, 420f));
    //    }
    //}


    //同意
    void OnClickAgreeBtn(GameObject o)
    {
        uint AgreeBtn = 1;
        InvokeUIDlg(UIEventType.Click, this, AgreeBtn);
    }

    //拒绝
    void OnClickRefuse(GameObject o)
    {
        uint refuseBtn = 2;
        InvokeUIDlg(UIEventType.Click, this, refuseBtn);
    }

}

