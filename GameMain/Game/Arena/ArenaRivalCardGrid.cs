using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using GameCmd;
using Client;

class ArenaRivalCardGrid : UIGridBase
{
    UISprite job;
    UILabel lv;
    UILabel name;
    UILabel rank;
    UILabel combat;
    GameObject online;
    GameObject challengeBtn;

    IRenerTextureObj rtObj;
    GameObject rtGo;

    public OppuserData rivalInfo;
    protected override void OnAwake()
    {
        base.OnAwake();
        job = this.transform.Find("job").GetComponent<UISprite>();
        lv = this.transform.Find("level").GetComponent<UILabel>();
        name = this.transform.Find("name").GetComponent<UILabel>();
        rank = this.transform.Find("rank").GetComponent<UILabel>();
        combat = this.transform.Find("fighting").GetComponent<UILabel>();
        challengeBtn = this.transform.Find("btn_challenge").gameObject;
        online = this.transform.Find("mark_online").gameObject;
        rtGo = this.transform.Find("avatarRoot/CharacterRenderTexture").gameObject;

        UIEventListener.Get(challengeBtn).onClick = OnClickChallenge;
    }

    public override void SetGridData(object data)
    {
        base.SetGridData(data);
        this.rivalInfo = data as OppuserData;

        job.spriteName = "";
        lv.text = string.Format("{0}级", rivalInfo.level);
        name.text = rivalInfo.name.ToString();
        rank.text = rivalInfo.rank.ToString();
        combat.text = rivalInfo.fight_power.ToString();


        if (this.rivalInfo.online_state)
        {
            online.gameObject.SetActive(true);
        }
        else
        {
            online.gameObject.SetActive(false);
        }

        if (ClientGlobal.Instance().IsMainPlayer(this.rivalInfo.id))//当是玩家自己时，不能点击自己
        {
            challengeBtn.gameObject.SetActive(false);
        }
        else
        {
            challengeBtn.gameObject.SetActive(true);
        }
    }

    public void SetModel(List<SuitData> suitData, int job, uint faceId)
    {
        int sex = 0;

        sex = (int)GameUtil.FaceToSex(faceId);

        rtGo.SetActive(true);

        if (suitData == null)//机器人无时装数据
        {
            suitData = new List<SuitData>();
            SuitData sd = new SuitData { baseid = 0, suit_type = EquipSuitType.Unknow_Type };
            suitData.Add(sd);
        }

        if (rtObj != null)
        {
            rtObj.Release();
        }
        rtObj = DataManager.Manager<RenderTextureManager>().CreateRenderTextureObj(suitData, job, sex, 700);
        if (rtObj == null)
        {
            return;
        }

        UIRenderTexture rt = rtGo.GetComponent<UIRenderTexture>();
        if (rt == null)
            rt = rtGo.AddComponent<UIRenderTexture>();
        rtObj.SetModelRotateY(180);
        rtObj.SetModelScale(1f);
        rtObj.SetCamera(new Vector3(0, 1f, 0f), new Vector3(15, 0, 0), 4f);
        rt.SetDepth(3);
        rt.Initialize(rtObj, 180f, new UnityEngine.Vector2(600f, 600f));
        rtObj.PlayModelAni(Client.EntityAction.Stand);
    }


    protected override void OnDisable()
    {
        base.OnDisable();

        if (rtObj != null)
        {
            rtObj.Enable(false);
            rtObj.Release();
            rtObj = null;
        }
    }

    public void OnClickChallenge(GameObject o)
    {
        InvokeUIDlg(UIEventType.Click, this, null);
    }
}

