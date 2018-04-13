using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

class UITeamMemberGrid : UIGridBase
{
    UISprite markLeader;
    UILabel nameLbl;
    UILabel LvLbl;
    UISprite markJob;
    UIButton btnAgree;

    TeamMemberInfo memberInfo;

    #region override
    protected override void OnAwake()
    {
        base.OnAwake();
        markLeader = this.transform.Find("mark_leader").GetComponent<UISprite>();
        this.transform.Find("bg_name").gameObject.SetActive(true);
        nameLbl = this.transform.Find("bg_name").GetComponentInChildren<UILabel>();
        LvLbl = this.transform.Find("level").GetComponent<UILabel>();
        markJob = this.transform.Find("mark_job").GetComponent<UISprite>();
        btnAgree = this.transform.Find("btn_agree").GetComponent<UIButton>();

        UIEventListener.Get(btnAgree.gameObject).onClick = BtnAgree;

    }

    public override void SetGridData(object data)
    {
        memberInfo = data as TeamMemberInfo;
        if(memberInfo != null)
        {
            markLeader.spriteName = "";
            nameLbl.text = memberInfo.name;
            LvLbl.text = memberInfo.lv.ToString();
            markJob.spriteName = "";
        }
    }

    void BtnAgree(GameObject obj)
    {

    }


    #endregion



}

