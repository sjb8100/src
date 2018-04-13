using Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using table;
using UnityEngine;

public class MyTeamGridData
{
    public TeamMemberInfo teamMemberInfo;
    public Vector3 pos;
}

class UIMyTeamGrid : UIGridBase
{
    GameObject mark_Leader;
    UILabel level;
    UILabel name;
    UISpriteEx mark_job;
    GameObject bg_role;
    UIButton btn_agree;
    UITexture icon;

    GameObject btn_memberInfo;
    GameObject btn_addMember;
    GameObject m_lblMatching;

    //GameObject rtGo;
    //IRenerTextureObj rtObj;

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
        mark_Leader = transform.Find("member_info/mark_leader").gameObject;
        level = transform.Find("member_info/level").GetComponent<UILabel>();
        name = transform.Find("member_info/name").GetComponent<UILabel>();
        icon = transform.Find("member_info/avatarRoot/HeadIcon").GetComponent<UITexture>();
        mark_job = transform.Find("member_info/mark_job").GetComponent<UISpriteEx>();
        //rtGo = transform.Find("member_info/avatarRoot/CharacterRenderTexture").gameObject;

        btn_memberInfo = transform.Find("member_info").gameObject;
        btn_addMember = transform.Find("add_member").gameObject;
        m_lblMatching = transform.Find("add_member/Matching").gameObject;

        UIEventListener.Get(btn_memberInfo).onClick = OnClickMemberInfo;
        UIEventListener.Get(btn_addMember).onClick = OnClickAddMember;
    }

    public override void SetGridData(object data)
    {
        base.SetGridData(data);

        this.teamMemberInfo = data as TeamMemberInfo;
    }

    public void ShowMember(bool show)
    {
        if (show)
        {
            btn_memberInfo.gameObject.SetActive(true);
            btn_addMember.gameObject.SetActive(false);
        }
        else
        {
            btn_memberInfo.gameObject.SetActive(false);
            btn_addMember.gameObject.SetActive(true);
        }

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
            //icon.ChangeSprite(job);

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

    public void SetLv(uint lv)
    {
        if (level != null)
        {
            if (Client.ClientGlobal.Instance().IsMainPlayer(this.teamMemberInfo.id))
            {
                IPlayer player = Client.ClientGlobal.Instance().MainPlayer;
                if (player != null)
                {

                    lv = (uint)player.GetProp((int)CreatureProp.Level);
                }
            }
            level.text = string.Format("等级 {0}", lv);
        }

    }

    public void SetJob(uint job)
    {
        if (mark_job != null)
        {
            mark_job.ChangeSprite(job);
        }
    }

    public void SetLeaderMark(uint id)
    {
        if (IsLeader(id))
        {
            SetLeaderMark(true);
        }
        else
        {
            SetLeaderMark(false);
        }
    }

    public void SetLeaderMark(bool leader)
    {
        if (mark_Leader != null && mark_Leader.gameObject.activeSelf != leader)
        {
            mark_Leader.gameObject.SetActive(leader);
        }
    }

    public void SetMatchState(bool b)
    {
        if (m_lblMatching != null)
        {
            m_lblMatching.SetActive(b);
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

    bool IsLeader(uint id)
    {
        return DataManager.Manager<TeamDataManager>().IsLeader(id);
    }

    void OnClickMemberInfo(GameObject o)
    {
        uint MainPlayerID = ClientGlobal.Instance().MainPlayer.GetID();
        if (MainPlayerID != teamMemberInfo.id)
        {
            uint btnMemberInfo = 1;
            InvokeUIDlg(UIEventType.Click, this, btnMemberInfo);
        }
    }

    /// <summary>
    /// 添加队员   
    /// </summary>
    /// <param name="o"></param>
    void OnClickAddMember(GameObject o)
    {
        uint btnAddMember = 2;
        InvokeUIDlg(UIEventType.Click, this, btnAddMember);
    }
}

