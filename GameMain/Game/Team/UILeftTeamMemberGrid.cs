using Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using table;
using Engine.Utility;

class UILeftTeamMemberGrid : UIGridBase
{
    UITexture icon;
    UILabel name;
    UILabel level;
    UISlider hpbar;
    UILabel place;//位置
    GameObject msgBtn;
    GameObject leader;
    GameObject follow;  //跟随标记
    GameObject dead;
    GameObject offline;
    GameObject bg;
    GameObject Background;

    TeamMemberInfo teamMemberInfo;

    IEntitySystem es;

    private CMResAsynSeedData<CMTexture> iuiIconAtlas = null;

    #region nomo

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
        icon = this.transform.Find("icon_head").GetComponent<UITexture>();
        name = this.transform.Find("name").GetComponent<UILabel>();
        level = this.transform.Find("level").GetComponent<UILabel>();
        hpbar = this.transform.Find("hp").GetComponent<UISlider>();
        place = this.transform.Find("place").GetComponent<UILabel>();
        leader = this.transform.Find("mark_leader").gameObject;
        follow = this.transform.Find("mark_follow").gameObject;
        dead = this.transform.Find("status_dead").gameObject;
        offline = this.transform.Find("status_offline").gameObject;
        bg = this.transform.Find("bg").gameObject;
        Background = this.transform.Find("bg/Background").gameObject;
        msgBtn = this.transform.Find("btn_message").gameObject;

        UIEventListener.Get(bg).onPress = OnClickBg;

        UIEventListener.Get(icon.gameObject).onClick = OnClickIcon;
        UIEventListener.Get(msgBtn.gameObject).onClick = OnClickMsg;

        es = Client.ClientGlobal.Instance().GetEntitySystem();
    }

    float tempTime = 0f;
    void Update()
    {
        tempTime += Time.deltaTime;
        if (tempTime > 1.0f)
        {
            if (this.teamMemberInfo == null)
            {
                Engine.Utility.Log.Error("队伍成员数据为null!!!");
                return;
            }           

            TeamMemberInfo info = DataManager.Manager<TeamDataManager>().GetTeamMember(this.teamMemberInfo.id);
            if (info == null)
            {
                return;
            }

            //更新数据
            this.teamMemberInfo = info;

            //更新队友属性
            UpdateTeamMemberProp();

            //清零
            tempTime = 0;
        }
    }

    #endregion

    #region method

    public override void SetGridData(object data)
    {
        base.SetGridData(data);

        this.teamMemberInfo = data as TeamMemberInfo;
    }



    /// <summary>
    /// 跟新队友属性
    /// </summary>
    void UpdateTeamMemberProp()
    {
        if (es == null)
        {
            return;
        }

        IPlayer player = es.FindPlayer(this.teamMemberInfo.id);

        //在九屏
        if (player != null)
        {
            if (false == player.IsDead())
            {
                //死亡标识
                SetDeadMark(false);

                //地图标识
                SetMapName(false);

                //血量
                float hpValue = (player.GetProp((int)CreatureProp.Hp) + 0.0f) / (player.GetProp((int)CreatureProp.MaxHp));
                SetHp(true, hpValue);
            }
            else
            {
                SetDeadMark(true);

                SetHp(true, 0);
            }

            //等级
            SetLv((uint)player.GetProp((int)CreatureProp.Level));//同步等级

            //在线标识
            SetOfflineMark(false);
        }

        //不在九屏
        else
        {
            SetDeadMark(false);

            SetHp(false);

            //设置地图名(分线)
            SetMapName(true , this.teamMemberInfo.lineId);

            SetLv(this.teamMemberInfo.lv);

            //在线标识
            SetOfflineMark(!this.teamMemberInfo.onLine);
        }

        //队长标识
        bool isLeader = DataManager.Manager<TeamDataManager>().IsLeader(this.teamMemberInfo.id);
        SetLeaderMark(isLeader);

        //跟随标识
        SetFollowMark(this.teamMemberInfo.isFollow);
    }

    public void SetIcon(uint profession)
    {
        if (icon != null)
        {
            SelectRoleDataBase sdb = table.SelectRoleDataBase.Where((GameCmd.enumProfession)profession, (GameCmd.enmCharSex)1);
            if (sdb != null)
            {
                //DataManager.Manager<UIManager>().SetSpriteDynamicIcon(icon, sdb.strprofessionIcon, false);

                UIManager.GetTextureAsyn(sdb.strprofessionIcon, ref iuiIconAtlas, () => { if (icon != null) { icon.mainTexture = null; } }, icon,false);

            }
        }
    }

    public void SetName(string nameStr)
    {
        if (name != null)
        {
            name.text = nameStr;
        }
    }

    public void SetLv(uint lv)
    {
        if (level != null)
        {
            level.text = string.Format("{0}级", lv);
        }
    }

    public void SetHp(bool b, float hpValue = 0f)
    {
        if (hpbar != null)
        {
            hpbar.gameObject.SetActive(b);
            if (b)
            {
                hpbar.value = hpValue;
            }
        }
    }

    /// <summary>
    /// 设置地图名称
    /// </summary>
    /// <param name="b">是否显示地图名称</param>
    /// <param name="lineId">分线</param>
    public void SetMapName(bool b,uint lineId = 1)
    {
        if (place != null)
        {
            if (b)
            {
                place.gameObject.SetActive(true);
                MapDataBase mapDataBase = GameTableManager.Instance.GetTableItem<MapDataBase>(this.teamMemberInfo.mapId);
                if (mapDataBase != null)
                {
                    string mapName = string.Format("{0}({1}线)", mapDataBase.strName,lineId);
                    place.text = mapName;
                }
            }
            else
            {
                place.gameObject.SetActive(false);
            }
        }

    }

    /// <summary>
    /// 离线标识
    /// </summary>
    /// <param name="b"></param>
    public void SetOfflineMark(bool b)
    {
        if (offline != null && offline.activeSelf != b)
        {
            offline.SetActive(b);
        }
    }

    /// <summary>
    /// 跟随标识
    /// </summary>
    /// <param name="isFollow"></param>
    public void SetFollowMark(bool isFollow)
    {
        if (follow != null && follow.activeSelf != isFollow)
        {
            follow.SetActive(isFollow);
        }
    }

    /// <summary>
    /// 死亡标识
    /// </summary>
    /// <param name="b"></param>
    public void SetDeadMark(bool b)
    {
        if (dead != null && dead.activeSelf != b)
        {
            dead.SetActive(b);
        }
    }

    /// <summary>
    /// 队长标识
    /// </summary>
    /// <param name="b"></param>
    public void SetLeaderMark(bool b)
    {
        if (leader != null && leader.activeSelf != b)
        {
            leader.SetActive(b);
        }
    }

    #endregion

    #region OnClick

    void OnClickBg(GameObject go, bool state)
    {
        if (Background != null)
        {
            if (state)
            {
                Background.gameObject.SetActive(true);
            }
            else
            {
                Background.gameObject.SetActive(false);
            }
        }
    }

    void OnClickIcon(GameObject o)
    {

    }

    void OnClickMsg(GameObject o)
    {
        // 发送事件

        //显示切换目标
        es = Client.ClientGlobal.Instance().GetEntitySystem();
        if (es != null)
        {
            IPlayer m_curTarget = es.FindPlayer(this.teamMemberInfo.id);
            if (m_curTarget != null)
            {
                Client.stTargetChange targetChange = new Client.stTargetChange();
                targetChange.target = m_curTarget;
                Engine.Utility.EventEngine.Instance().DispatchEvent((int)GameEventID.ENTITYSYSTEM_TARGETCHANGE, targetChange);
            }
        }

        //左侧最远操作界面
        if (DataManager.Manager<UIPanelManager>().IsShowPanel(PanelID.MissionAndTeamPanel))
        {
            stTeamMemberBtn teamMemberBtn = new stTeamMemberBtn();
            teamMemberBtn.id = this.teamMemberInfo.id;
            teamMemberBtn.name = this.teamMemberInfo.name;
            teamMemberBtn.pos_y = this.transform.position.y;

            DataManager.Manager<UIPanelManager>().SendMsg(PanelID.MissionAndTeamPanel, UIMsgID.eTeamMemberBtn, teamMemberBtn);
        }
    }

    #endregion
}

