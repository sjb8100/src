using GameCmd;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;


class ArenaTopCardGrid : UIGridBase
{
    UILabel m_lblName;      //名
    UILabel m_lblLv;
    UILabel m_lblFighting;  //战斗力

    IRenerTextureObj rtObj;
    GameObject rtGo;

    GameObject detailBtnGo;

    TopUserData m_topdata;
    /// <summary>
    /// UIBase Awake
    /// </summary>
    protected override void OnAwake()
    {
        base.OnAwake();

        m_lblName = this.transform.Find("PlayerName").GetComponent<UILabel>();
        m_lblLv = this.transform.Find("level").GetComponent<UILabel>();
        m_lblFighting = this.transform.Find("Fighting/FightingNum").GetComponent<UILabel>();
        rtGo = this.transform.Find("avatarRoot/CharacterRenderTexture").gameObject;
        detailBtnGo = this.transform.Find("Btn_detail").gameObject;

        UIEventListener.Get(detailBtnGo).onClick = OnDetailBtnClick;
    }

    public override void SetGridData(object data)
    {
        base.SetGridData(data);

        m_topdata = data as TopUserData;

        m_lblName.text = m_topdata.name;
        m_lblLv.text = string.Format("{0}级", m_topdata.level);
        m_lblFighting.text = m_topdata.fight_power.ToString();

    }

    public override void Reset()
    {
        base.Reset();
    }

    public void SetModel(List<SuitData> suitData, int job, uint faceId)
    {
        int sex = 0;

        sex = (int)GameUtil.FaceToSex(faceId);

        if (suitData == null)//机器人无时装数据
        {
            suitData = new List<SuitData>();
            SuitData sd = new SuitData { baseid = 0, suit_type = EquipSuitType.Unknow_Type };
            suitData.Add(sd);
        }

        if (rtObj != null)
        {
            rtObj.Release();
            rtObj = null;
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

    //void OnDisable()
    //{
    //    if (rtObj != null)
    //    {
    //        rtObj.Enable(false);
    //        rtObj.Release();
    //    }
    //}

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

    void OnDetailBtnClick(GameObject go)
    {
        NetService.Instance.Send(new GameCmd.stRequestViewRolePropertyUserCmd_C()
        {
            zoneid = 0,
            dwUserid = m_topdata.id,
            mycharid = m_topdata.id,
        });
    }
}

