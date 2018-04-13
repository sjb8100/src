using UnityEngine;
using System.Collections.Generic;
using Cmd;
using Client;
using System;
public partial class PlayerOpreatePanel : UIPanelBase
{
    public enum ViewType
    {
        Normal,
        AddRemove_Contact,  //好友联系人
        AddRemove_Interact, //好友互动
        AddRemove_Enemy,    //好友仇敌
        AddRemove_Shield,   //好友屏蔽
        Clan,
    }

    public class PlayerViewInfo
    {
        public enum PlayerViewBtns
        {
            //发送消息
            SendTxt = 1,
            //查看消息
            ViewMsg = 2,
            //邀请组队
            Invite = 4,
            //申请入队
            Apply = 8,
            //添加好友
            AddFriend = 16,
            //拜访家园
            Visit = 32,
            //移除列表
            Remove = 64,
            //屏幕
            Shield = 128,
            //升降职务
            ChangeDuty = 512,
            //逐出氏族
            Expel = 1024,
        }
        public uint uid;
        public string name;
        public uint level;
        public uint teamID = 0;
        public uint teamNum;
        public uint teamMaxNum = 5;
        public bool onLine = false;
        public ViewType viewType = ViewType.Normal;
        public uint job;
        public bool isRobot = false;
        public uint sex;
        public uint clanid;

        //功能按钮显示mask（默认显示PlayerViewBtns前面6个）
        public int playerViewMask = (int)(PlayerViewBtns.SendTxt | PlayerViewBtns.ViewMsg | PlayerViewBtns.Invite
            | PlayerViewBtns.Apply | PlayerViewBtns.AddFriend | PlayerViewBtns.Visit);

        public GameCmd.RelationType RelationToRemove = GameCmd.RelationType.Relation_MAX;
    }
    PlayerViewInfo m_datainfo;
    protected override void OnInitPanelData()
    {
        base.OnInitPanelData();
    }

    protected override void OnLoading()
    {
        base.OnLoading();
        InitBtnGap();
    }

    private CMResAsynSeedData<CMTexture> m_playerAvataCASD = null;
    protected override void OnShow(object data)
    {
        base.OnShow(data);
        m_datainfo = null;
        if (data is PlayerViewInfo)
        {
            PlayerViewInfo info = (PlayerViewInfo)data;
            m_datainfo = info;
            m_label_playerLevel.text = "等级:" + info.level.ToString();
            //m_label_playerId.text = "ID: " + info.uid.ToString();
            m_label_playername.text = info.name;
            uint clanId = info.clanid;
            if (clanId != 0)
            {
                DataManager.Manager<ClanManger>().GetClanName(clanId, (namedata) =>
                {
                    string winerCityName = string.Empty;
                    string name = string.Empty;
                    if (DataManager.Manager<CityWarManager>().GetWinerOfCityWarCityName((uint)clanId, out winerCityName))
                    {
                        //name = winerCityName + namedata.ClanName;
                        name = string.Format("{0}【{1}】", winerCityName, namedata.ClanName);
                    }
                    else
                    {
                        //name = namedata.ClanName;
                        name = string.Format("【{0}】", namedata.ClanName);
                    }
                    m_label_playerClan.text = "氏族:" + name;
                });
            }
            else
            {
                m_label_playerClan.text = "氏族:无";
            }
            //m_spriteEx_icon_head.ChangeSprite(info.job);

            //icon
            table.SelectRoleDataBase sdb = table.SelectRoleDataBase.Where((GameCmd.enumProfession)info.job, (GameCmd.enmCharSex)1);
            if (sdb != null)
            {
                UIManager.GetTextureAsyn(sdb.strprofessionIcon, ref m_playerAvataCASD, () =>
                    {
                        if (null != m__icon_head)
                        {
                            m__icon_head.mainTexture = null;
                        }
                    }, m__icon_head);
            }

            //             if (info.teamNum <= 0)
            // 	        {
            //                 m_label_playerTeam.text = "无队伍";
            //             }
            //             else
            //             {
            //                 m_label_playerTeam.text = string.Format("队伍：{0}/{1}", info.teamNum, info.teamMaxNum);
            //             }

            UpdatePlayerViewBtnsStatus();
            bool isFriend = DataManager.Manager<RelationManager>().IsMyFriend(info.uid);
            m_btn_btn_addfriend.GetComponentInChildren<UILabel>().text = isFriend ? "删除好友" : "添加好友";
        }
        else
        {
            Engine.Utility.Log.Error("PlayerOpreatePanel 传入数据类型不对");
        }
    }
    public override void OnColliderMaskClicked()
    {
        base.OnColliderMaskClicked();

        SendClanMemberOpDialogCloseMsg();

        HideSelf();
    }

    protected override void OnHide()
    {
        base.OnHide();
        Release();
    }

    public override void Release(bool depthRelease = true)
    {
        base.Release(depthRelease);
        if (null != m_playerAvataCASD)
        {
            m_playerAvataCASD.Release(true);
            m_playerAvataCASD = null;
        }
        m_datainfo = null;
    }

    /// <summary>
    /// 是否功能按钮可见
    /// </summary>
    /// <returns></returns>
    private bool IsPlayerViewBtnsCanSee(PlayerViewInfo.PlayerViewBtns btn)
    {
        if (null != m_datainfo
            && (m_datainfo.playerViewMask & (int)btn) != 0)
        {
            return true;
        }
        return false;
    }

    //屏蔽按钮文本
    private UILabel m_lab_shield = null;
    private Vector2 m_v2_btnGap = Vector2.zero;
    private Vector3 m_v3_btnStartPos = Vector3.zero;
    /// <summary>
    /// 初始化按钮间隔
    /// </summary>
    private void InitBtnGap()
    {
        if (null != m_btn_btn_sendmessage && null != m_btn_btn_checkmessage && null != m_btn_btn_pinvite)
        {
            m_v3_btnStartPos = m_btn_btn_sendmessage.transform.localPosition;
            m_v2_btnGap.x = m_btn_btn_checkmessage.transform.localPosition.x - m_btn_btn_sendmessage.transform.localPosition.x;
            m_v2_btnGap.y = m_btn_btn_pinvite.transform.localPosition.y - m_btn_btn_checkmessage.transform.localPosition.y;
        }
    }

    /// <summary>
    /// 根据mask切换按钮是否可视
    /// </summary>
    private void UpdatePlayerViewBtnsStatus()
    {
        List<Transform> sortBtns = new List<Transform>();
        bool active = IsPlayerViewBtnsCanSee(PlayerViewInfo.PlayerViewBtns.SendTxt);
        if (null != m_btn_btn_sendmessage)
        {
            if (m_btn_btn_sendmessage.gameObject.activeSelf != active)
            {
                m_btn_btn_sendmessage.gameObject.SetActive(active);
            }

            if (active)
            {
                sortBtns.Add(m_btn_btn_sendmessage.transform);
            }
        }
        active = IsPlayerViewBtnsCanSee(PlayerViewInfo.PlayerViewBtns.ViewMsg);
        if (null != m_btn_btn_checkmessage)
        {
            if (m_btn_btn_checkmessage.gameObject.activeSelf != active)
            {
                m_btn_btn_checkmessage.gameObject.SetActive(active);
            }
            if (active)
            {
                sortBtns.Add(m_btn_btn_checkmessage.transform);
            }
        }

        active = IsPlayerViewBtnsCanSee(PlayerViewInfo.PlayerViewBtns.Invite);
        if (null != m_btn_btn_pinvite)
        {
            if (m_btn_btn_pinvite.gameObject.activeSelf != active)
            {
                m_btn_btn_pinvite.gameObject.SetActive(active);
            }

            if (active)
            {
                sortBtns.Add(m_btn_btn_pinvite.transform);
            }
            
        }

        active = IsPlayerViewBtnsCanSee(PlayerViewInfo.PlayerViewBtns.Apply);
        if (null != m_btn_btn_apply)
        {
            if (m_btn_btn_apply.gameObject.activeSelf != active)
            {
                m_btn_btn_apply.gameObject.SetActive(active);
            }

            if (active)
            {
                sortBtns.Add(m_btn_btn_apply.transform);
            }
        }

        active = IsPlayerViewBtnsCanSee(PlayerViewInfo.PlayerViewBtns.AddFriend);
        if (null != m_btn_btn_addfriend)
        {
            if (m_btn_btn_addfriend.gameObject.activeSelf != active)
            {
                m_btn_btn_addfriend.gameObject.SetActive(active);
            }

            if (active)
            {
                sortBtns.Add(m_btn_btn_addfriend.transform);
            }
        }

        active = IsPlayerViewBtnsCanSee(PlayerViewInfo.PlayerViewBtns.Visit);
        if (null != m_btn_btn_visit)
        {
            if (m_btn_btn_visit.gameObject.activeSelf != active)
            {
                m_btn_btn_visit.gameObject.SetActive(active);
            }

            if (active)
            {
                sortBtns.Add(m_btn_btn_visit.transform);
            }
        }

        active = IsPlayerViewBtnsCanSee(PlayerViewInfo.PlayerViewBtns.Shield);
        if (null != m_btn_btn_shield)
        {
            if (m_btn_btn_shield.gameObject.activeSelf != active)
            {
                m_btn_btn_shield.gameObject.SetActive(active);
            }

            if (active)
            {
                sortBtns.Add(m_btn_btn_shield.transform);
                if (null == m_lab_shield && null != m_btn_btn_shield)
                {
                    Transform shieldLabTs = m_btn_btn_shield.transform.Find("Label");
                    if (null != shieldLabTs)
                    {
                        m_lab_shield = shieldLabTs.GetComponent<UILabel>();
                    }
                }
                if (null != m_lab_shield && null != m_datainfo)
                {
                    bool isBlack = DataManager.Manager<RelationManager>().IsMyBlack(m_datainfo.uid);
                    TextManager tmgr = DataManager.Manager<TextManager>();
                    string targetTxt = isBlack ? tmgr.GetLocalText(LocalTextType.Talk_System_RmoveBlackTxt)
                        : tmgr.GetLocalText(LocalTextType.Talk_System_AddBlackTxt);
                    m_lab_shield.text = targetTxt;
                }
            }
        }

        active = IsPlayerViewBtnsCanSee(PlayerViewInfo.PlayerViewBtns.Remove);
        if (null != m_btn_btn_remove)
        {
            if (m_btn_btn_remove.gameObject.activeSelf != active)
            {
                m_btn_btn_remove.gameObject.SetActive(active);
            }

            if (active)
            {
                sortBtns.Add(m_btn_btn_remove.transform);
            }
        }

        active = IsPlayerViewBtnsCanSee(PlayerViewInfo.PlayerViewBtns.ChangeDuty);
        if (null != m_btn_btn_changeDuty)
        {
            if (m_btn_btn_changeDuty.gameObject.activeSelf != active)
            {
                m_btn_btn_changeDuty.gameObject.SetActive(active);
            }

            if (active)
            {
                sortBtns.Add(m_btn_btn_changeDuty.transform);
            }
        }

        active = IsPlayerViewBtnsCanSee(PlayerViewInfo.PlayerViewBtns.Expel);
        if (null != m_btn_btn_expel)
        {
            if (m_btn_btn_expel.gameObject.activeSelf != active)
            {
                m_btn_btn_expel.gameObject.SetActive(active);
            }

            if (active)
            {
                sortBtns.Add(m_btn_btn_expel.transform);
            }
        }
        if (null != sortBtns)
        {
            Vector3 targetPos = Vector3.zero;
            Transform tempTrans = null;
            int count = 1;
            for (int i = 0, max = sortBtns.Count; i < max;i++ )
            {
                tempTrans = sortBtns[i];
                if (null == tempTrans)
                {
                    continue;
                }

                targetPos = m_v3_btnStartPos;
                bool needReduce = false;
                if (count %2 == 0)
                {
                    targetPos.x = targetPos.x + m_v2_btnGap.x;
                    needReduce = true;
                }

                {
                    int gapCount = count / 2;
                    if (gapCount > 0 && needReduce)
                    {
                        gapCount--;
                    }
                    targetPos.y = targetPos.y + gapCount * m_v2_btnGap.y;
                }
                
                
                tempTrans.localPosition = targetPos;
                count++;
            }

            if (sortBtns.Count > 6)
            {
                m_sprite_sprite_bg.height = 380;
            }
            else
            {
                m_sprite_sprite_bg.height = 320;
            }
        }

    }

    /// <summary>
    /// 发送氏族成员操作界面关闭消息
    /// </summary>
    private void SendClanMemberOpDialogCloseMsg()
    {
        if (null != m_datainfo && m_datainfo.viewType == ViewType.Clan)
        {
            Engine.Utility.EventEngine.Instance().DispatchEvent((int)Client.GameEventID.UIEVENTCLANMEMBEROPDIALOGCLOSED);
        }
    }

    /// <summary>
    /// 发送消息
    /// </summary>
    /// <param name="caster"></param>
    void onClick_Btn_sendmessage_Btn(GameObject caster)
    {
        if (m_datainfo == null)
        {
            return;
        }

        if (!string.IsNullOrEmpty(m_datainfo.name))
        {
            RoleRelation data = new RoleRelation() { uid = m_datainfo.uid, name = m_datainfo.name, isRobot = m_datainfo.isRobot };
            DataManager.Manager<UIPanelManager>().ShowPanel(PanelID.FriendPanel);
            DataManager.Manager<UIPanelManager>().SendMsg(PanelID.FriendPanel, UIMsgID.eChatWithPlayer, data);
        }
        else
        {
            TipsManager.Instance.ShowTips("该玩家不在线！");
        }

        this.HideSelf();
    }

    /// <summary>
    /// 查看玩家详情
    /// </summary>
    /// <param name="caster"></param>
    void onClick_Btn_checkmessage_Btn(GameObject caster)
    {
        if (m_datainfo != null)
        {
            if (m_datainfo.isRobot)
            {
                ViewPlayerData viewdata = ViewPlayerData.BuildViewData(m_datainfo.uid, m_datainfo.name, m_datainfo.job, (int)m_datainfo.level, m_datainfo.sex);
                if (viewdata != null)
                {
                    DataManager.Manager<UIPanelManager>().ShowPanel(PanelID.ViewPlayerPanel, data: viewdata);
                }
                return;
            }
            NetService.Instance.Send(new GameCmd.stRequestViewRolePropertyUserCmd_C()
            {
                zoneid = 0,
                dwUserid = m_datainfo.uid,
                mycharid = m_datainfo.uid,
            });

        }

        this.HideSelf();
    }

    /// <summary>
    /// 邀请组队
    /// </summary>
    /// <param name="caster"></param>
    void onClick_Btn_pinvite_Btn(GameObject caster)
    {
        if (m_datainfo != null)
        {
            if (m_datainfo.isRobot)
            {
                return;
            }
            DataManager.Manager<TeamDataManager>().ReqInviteTeam(m_datainfo.uid, m_datainfo.name);
        }
        this.HideSelf();
    }

    /// <summary>
    /// 申请入队
    /// </summary>
    /// <param name="caster"></param>
    void onClick_Btn_apply_Btn(GameObject caster)
    {
        //uint myId = ClientGlobal.Instance().MainPlayer.GetID();
        if (m_datainfo != null)
        {
            DataManager.Manager<TeamDataManager>().ReqJoinTeam(m_datainfo.uid);
        }
        else
        {
            Engine.Utility.Log.Error("m_datainfo 为 null !!! ");
        }

        this.HideSelf();
    }

    /// <summary>
    /// 添加,删除好友
    /// </summary>
    /// <param name="caster"></param>
    void onClick_Btn_addfriend_Btn(GameObject caster)
    {
        if (m_datainfo == null)
        {
            return;
        }
        if (m_datainfo.isRobot)
        {
            return;
        }
        bool isFriend = DataManager.Manager<RelationManager>().IsMyFriend(m_datainfo.uid);
        if (!isFriend)
        {
            if (Client.ClientGlobal.Instance().IsMainPlayer(m_datainfo.uid))
            {
                TipsManager.Instance.ShowTipsById(510);
                return;
            }
            List<RoleRelation> list;
            if (DataManager.Manager<RelationManager>().GetRelationListByType(GameCmd.RelationType.Relation_Friend, out list))
            {
                uint maxNum = GameTableManager.Instance.GetGlobalConfig<uint>("MAX_FRIEND_SIZE");

                if (list.Count >= maxNum)
                {
                    TipsManager.Instance.ShowTipsById(503);
                    return;
                }
            }

            DataManager.Instance.Sender.RequestAddRelation(GameCmd.RelationType.Relation_Friend, m_datainfo.uid);
        }
        else
        {
            DataManager.Instance.Sender.RequestKickRelationUser(GameCmd.RelationType.Relation_Friend, m_datainfo.uid);
        }

        this.HideSelf();
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="caster"></param>
    void onClick_Btn_shield_Btn(GameObject caster)
    {
        if (m_datainfo == null)
        {
            return;
        }
        DataManager.Manager<RelationManager>().ShiledPlayer(m_datainfo.uid);
        this.HideSelf();
    }

    /// <summary>
    /// 移除列表
    /// </summary>
    /// <param name="caster"></param>
    void onClick_Btn_remove_Btn(GameObject caster)
    {
        //TODO 从最近联系人中移除
        if (m_datainfo == null)
        {
            return;
        }
        if (m_datainfo.RelationToRemove != GameCmd.RelationType.Relation_MAX)
        {
            DataManager.Instance.Sender.RequestKickRelationUser(m_datainfo.RelationToRemove, m_datainfo.uid);
        }

        this.HideSelf();
    }

    void onClick_BtnClose_Btn(GameObject caster)
    {
        SendClanMemberOpDialogCloseMsg();
        this.HideSelf();
    }

    /// <summary>
    /// 访问别人家园
    /// </summary>
    /// <param name="caster"></param>
    void onClick_Btn_visit_Btn(GameObject caster)
    {
        TipsManager.Instance.ShowTips("还未开放，敬请期待");
        return;

        if (m_datainfo == null)
        {
            return;
        }

        if (m_datainfo.isRobot)
        {
            TipsManager.Instance.ShowTips("无法进入该玩家家园");
            return;
        }

        this.HideSelf();
        // HomeScene.Instance.Enter();
    }

    void onClick_Btn_changeDuty_Btn(GameObject caster)
    {
        if (null == m_datainfo)
        {
            TipsManager.Instance.ShowTips("数据错误");
        }
        else
        {
            uint fzzLeft = (uint)DataManager.Manager<ClanManger>().GetClanLeftMemberNum(GameCmd.enumClanDuty.CD_Deputy);
            //uint normalLeft = (uint) DataManager.Manager<ClanManger>().GetClanLeftMemberNum(GameCmd.enumClanDuty.CD_Member);
            LiftingClanDutyPanel.LiftClanDutyData liftData = new LiftingClanDutyPanel.LiftClanDutyData()
            {
                desUserId = m_datainfo.uid,
                leftFZZ = fzzLeft,
            };
            DataManager.Manager<UIPanelManager>().ShowPanel(PanelID.LiftingClanDutyPanel, data: liftData);
        }
        HideSelf();
    }

    void onClick_Btn_expel_Btn(GameObject caster)
    {
        if (null == m_datainfo)
        {
            TipsManager.Instance.ShowTips("数据错误");
        }
        else
        {
            DataManager.Manager<ClanManger>().ExpelFromClan(m_datainfo.uid);
        }
        HideSelf();

    }

}
