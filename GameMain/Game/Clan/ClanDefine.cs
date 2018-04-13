/************************************************************************************
 * Copyright (c) 2016  All Rights Reserved.
 * CLR版本： 4.0.30319.42000
 * 公司名称：ZQB
 * 命名空间：GameMain.Game.Clan
 * 创建人：  wenjunhua.zqgame
 * 文件名：  ClanDefine
 * 版本号：  V1.0.0.0
 * 创建时间：10/31/2016 9:16:12 AM
 * 描述：
 ************************************************************************************/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GameCmd;
class ClanDefine
{
    #region Define
    //族贡图标名
    public const string CONST_ICON_ZG_NAME = "tubiao_huobi_zugong";
    //资金图标名
    public const string CONST_ICON_ZJ_NAME = "tubiao_huobi_zijin";

    //最大公告字数
    public const int CONST_CLAN_GG_MAX_WORDS = 50;

    //名称获取回调
    public delegate void OnClanNameGet(ClanNameData data);
    //名称获取
    public class ClanNameData
    {
        //氏族ID
        public uint ClanID;
        //氏族名称
        public string ClanName;
    }
    /// <summary>
    /// 氏族权限
    /// </summary>
    public enum ClanDutyRight
    {
        None = 0,
        AgreeApply,         //同意氏族申请
        Appointment,       //任免
        BroadCaseGG,        //发送公告
        BroadCastMsg,       //群发消息
        Expel,              //驱逐
        Max,
    }
    /// <summary>
    /// 职位权限本地数据结构
    /// </summary>
    public class LocalClanDutyDB
    {
        #region property
        //职位权限表格数据
        private table.ClanDutyPermDataBase m_db;
        //同意申请
        public bool CanAgreeApply
        {
            get
            {
                return (null != m_db && m_db.agreeApply == 1) ? true : false;
            }
        }
        //是否可以任免
        public bool CanAppointment
        {
            get
            {
                return (null != m_db && m_db.appointment == 1) ? true : false;
            }
        }
        //是否能发布公告
        public bool CanBroadCaseGG
        {
            get
            {
                return (null != m_db && m_db.broadcastGG == 1) ? true : false;
            }
        }

        //是否能群发消息
        public bool CanBroadCastMsg
        {
            get
            {
                return (null != m_db && m_db.broadcastMsg == 1) ? true : false;
            }
        }

        //驱逐
        public bool CanExpel
        {
            get
            {
                return (null != m_db && m_db.expel == 1) ? true : false;
            }
        }
        //职阶
        public GameCmd.enumClanDuty Duty
        {
            get
            {
                return (null != m_db && m_db.job != 0) ? (GameCmd.enumClanDuty)m_db.job : GameCmd.enumClanDuty.CD_None;
            }
        }
        //职阶名称
        public string Name
        {
            get
            {
                return DataManager.Manager<ClanManger>().GetNameByClanDuty(Duty);
            }
        }
        #endregion

        #region structmethod
        public LocalClanDutyDB(table.ClanDutyPermDataBase db)
        {
            this.m_db = db;
        }
        #endregion

        #region Op



        /// <summary>
        /// 获取可以任免的职阶列表
        /// </summary>
        /// <returns></returns>
        public List<GameCmd.enumClanDuty> GetAppointmentDutys()
        {
            List<GameCmd.enumClanDuty> apList = new List<GameCmd.enumClanDuty>();
            if (CanAppointment)
            {
                Array eV = Enum.GetValues(typeof(GameCmd.enumClanDuty));
            }
            return apList;
        }
        #endregion

    }

    public class LocalClanDonateDB
    {
        #region property
        private table.ClanDonateDataBase m_db;
        //id
        public uint ID
        {
            get
            {
                return (null != m_db) ? m_db.id : 0;
            }
        }
        //捐献类型
        public uint DonateType
        {
            get
            {
                return (null != m_db) ? m_db.donateType : 0;
            }
        }
        //名称
        public string Name
        {
            get
            {
                return (null != m_db) ? m_db.donateName : "";
            }
        }
        //捐献数量
        public uint DonateNum
        {
            get
            {
                return (null != m_db) ? m_db.donateNum : 0;
            }
        }
        //声望
        public uint SW
        {
            get
            {
                return (null != m_db) ? m_db.reputation : 0;
            }
        }
        //族贡
        public uint ZG
        {
            get
            {
                return (null != m_db) ? m_db.clanSp : 0;
            }
        }
        //资金
        public uint ZJ
        {
            get
            {
                return (null != m_db) ? m_db.money : 0;
            }
        }
        //剩余捐献次数
        private uint m_donateTimes;
        public uint LeftTimes
        {
            get
            {
                return Math.Max(0, (null != m_db) ? m_db.times - m_donateTimes : 0);
            }
        }

        //总次数
        public uint TotalTimes
        {
            get
            {
                return (null != m_db) ? m_db.times : 0;
            }
        }


        #endregion
        public LocalClanDonateDB(table.ClanDonateDataBase db, uint donateTimes)
        {
            this.m_db = db;
            this.m_donateTimes = donateTimes;
        }
    }

    public class LocalClanMemberDB
    {
        #region Property
        //成员数量字典
        private Dictionary<GameCmd.enumClanDuty, uint> m_dic_clanDutyMembers = new Dictionary<GameCmd.enumClanDuty, uint>();
        //表格数据
        private table.ClanMemberDataBase m_db;
        //等级
        public uint Lv
        {
            get
            {
                return (null != m_db) ? m_db.lv : 0;
            }
        }
        //成员上限
        public uint MaxMember
        {
            get
            {
                return (null != m_db) ? m_db.memberNum : 0;
            }
        }
        public uint CostZiJin
        {
            get
            {
                return (null != m_db) ? m_db.costfund : 0;
            }
        }
        #endregion

        #region Structmethod
        public LocalClanMemberDB(table.ClanMemberDataBase db)
        {
            this.m_db = db;
            if (null != m_db)
            {
                string[] dutyArray = db.duty.Split(new char[] { '|' });
                string[] dutyNumArray = db.dutyNum.Split(new char[] { '|' });
                if (null != dutyArray && null != dutyNumArray
                    && dutyArray.Length == dutyNumArray.Length)
                {
                    GameCmd.enumClanDuty dutyEnum;
                    uint dutyMemberNum = 0;
                    for (int i = 0; i < dutyArray.Length; i++)
                    {
                        dutyEnum = (GameCmd.enumClanDuty)int.Parse(dutyArray[i].Trim());
                        dutyMemberNum = uint.Parse(dutyNumArray[i].Trim());
                        if (!m_dic_clanDutyMembers.ContainsKey(dutyEnum))
                        {
                            m_dic_clanDutyMembers.Add(dutyEnum, dutyMemberNum);
                        }
                    }

                }
            }
        }
        #endregion

        #region Op
        /// <summary>
        /// 获取该职位的人数限制
        /// </summary>
        /// <param name="duty"></param>
        /// <returns></returns>
        public uint GetMemberCountOfDuty(GameCmd.enumClanDuty duty)
        {
            return (m_dic_clanDutyMembers.ContainsKey(duty) ? m_dic_clanDutyMembers[duty] : 0);
        }
        #endregion
    }

    /// <summary>
    /// 本地氏族数据结构
    /// </summary>
    public class LocalClanInfo
    {
        #region property
        //id
        public uint Id
        {
            get
            {
                return (null != m_serverInfo) ? m_serverInfo.id : 0;
            }
        }

        //创建时间
        public uint CreateTime
        {
            get
            {
                return (null != m_serverInfo) ? m_serverInfo.create_time : 0;
            }
        }

        //创建者
        public uint Creator
        {
            get
            {
                return (null != m_serverInfo) ? m_serverInfo.creator : 0;
            }
        }
        //是否为正式氏族
        public bool IsFormal
        {
            get
            {
                return (null != m_serverInfo && (m_serverInfo.formal == 1)) ? true : false;
            }
        }

        //氏族等级
        public uint Lv
        {
            get
            {
                return (null != m_serverInfo) ? m_serverInfo.level : 0;
            }
        }

        //氏族名称
        public string Name
        {
            get
            {
                return (null != m_serverInfo) ? m_serverInfo.name : "";
            }
        }

        //氏族公告
        public string GG
        {
            get
            {
                return (null != m_serverInfo) ? m_serverInfo.broadcast_msg : "";
            }
        }

        //成员数量
        public int MemberCount
        {
            get
            {
                int count = 0;
                if (null != m_serverInfo)
                {
                    count = (int)m_serverInfo.member_size;
                }           
                return count;
            }
        }

        /// <summary>
        /// 或在线玩家
        /// </summary>
        public int OnLineMemberCount
        {
            get
            {
                int count = 0;
                if (null != m_serverInfo
                    && null != m_serverInfo.memberlist
                    && null != m_serverInfo.memberlist.member)
                {
                    foreach (GameCmd.stClanMemberInfo memberInfo in m_serverInfo.memberlist.member)
                    {
                        if (memberInfo.is_online == 1)
                        {
                            count++;
                        }
                    }
                }
                return count;
            }
        }

        //氏族资金
        public uint Money
        {
            get
            {
                return (null != m_serverInfo) ? m_serverInfo.money : 0;
            }
            set
            {
                if (null != m_serverInfo && value >= 0)
                {
                    m_serverInfo.money = value;
                }
            }
        }

        //战斗力
        public uint Fight
        {
            get
            {
                uint fight = 0;
                if (MemberCount > 0)
                {
                    foreach (GameCmd.stClanMemberInfo mInfo in m_serverInfo.memberlist.member)
                    {
                        fight += mInfo.fight;
                    }
                }

                return fight;
            }
        }

        //族长
        public uint ShaikhId
        {
            get
            {
                return (null != m_serverInfo) ? m_serverInfo.leader : 0;
            }
        }
        //成员列表
        public GameCmd.stMemberList MemberList
        {
            get
            {
                return (null != m_serverInfo) ? m_serverInfo.memberlist : null;
            }
        }
        //总族贡
        public uint TotalZG
        {
            get
            {
                return (null != m_serverInfo) ? m_serverInfo.contribution : 0;
            }
        }

        //7日族贡
        public uint SevenDayZG
        {
            get
            {
                return (null != m_serverInfo) ? m_serverInfo.contribution_7day : 0;
            }
        }

        //每日消耗族贡
        public uint DayCostZG
        {
            get
            {
                return (null != m_serverInfo) ? m_serverInfo.contribution_cost : 0;
            }
        }

        #endregion

        #region Skill

        /// <summary>
        /// 尝试获取氏族技能研发等级
        /// </summary>
        /// <param name="skillId"></param>
        /// <param name="devLv"></param>
        /// <returns></returns>
        public bool TryGetSkillDevLv(uint skillId, out uint devLv)
        {
            devLv = 0;
            if (null != m_serverInfo
                && null != m_serverInfo.skill_level)
            {
                foreach (GameCmd.PairNumber pair in m_serverInfo.skill_level)
                {
                    if (pair.id != skillId)
                        continue;
                    devLv = pair.value;
                    return true;
                }
            }
            return false;
        }

        #endregion

        /// <summary>
        /// 更新成员列表
        /// </summary>
        /// <param name="memberList"></param>
        /// <returns></returns>
        public bool UpdateMemberInfos(GameCmd.stMemberList memberList)
        {
            bool success = false;
            if (null != m_serverInfo)
            {
                m_serverInfo.memberlist = memberList;
                success = true;
            }
            return success;
        }

        /// <summary>
        /// 更新公告
        /// </summary>
        /// <param name="content"></param>
        public void UpdateGG(string content)
        {
            if (null != m_serverInfo)
            {
                m_serverInfo.broadcast_msg = content;
            }
        }

        /// <summary>
        /// 获取成员列表
        /// </summary>
        /// <returns></returns>
        public List<GameCmd.stClanMemberInfo> GetMemberInfos()
        {
            List<GameCmd.stClanMemberInfo> members = new List<GameCmd.stClanMemberInfo>();
            if (null != m_serverInfo
                && null != m_serverInfo.memberlist
                && null != m_serverInfo.memberlist.member)
            {
                members.AddRange(m_serverInfo.memberlist.member);             
            }

            Dictionary<uint, GameCmd.stClanMemberInfo> m_dic = new Dictionary<uint, GameCmd.stClanMemberInfo>();
            //测试bug  后面要删除的
            for (int i = 0; i < members.Count; i ++  )
            {
                if (m_dic.ContainsKey(members[i].id))
                {
                    Engine.Utility.Log.Error("id:{0}的玩家信息重复",members[i].id);
                }
                else 
                {
                    m_dic.Add(members[i].id, members[i]);
                }
            }
            return members;
        }

        /// <summary>
        /// 获取氏族成员数据
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public GameCmd.stClanMemberInfo GetMemberInfo(uint userId)
        {
            GameCmd.stClanMemberInfo member = null;
            if (null != m_serverInfo
                && null != m_serverInfo.memberlist
                && null != m_serverInfo.memberlist.member)
            {
                foreach (GameCmd.stClanMemberInfo info in m_serverInfo.memberlist.member)
                {
                    if (null == info)
                    {
                        continue;
                    }
                    if (info.id == userId)
                    {
                        member = info;
                        break;
                    }
                }
            }
            return member;
        }

        //服务端下发的氏族数据
        private GameCmd.stClanInfo m_serverInfo;
        public LocalClanInfo(GameCmd.stClanInfo info)
        {
            m_serverInfo = info;
        }

        /// <summary>
        /// 更新信息
        /// </summary>
        /// <param name="info"></param>
        public void UpdateInfo(GameCmd.stClanInfo info)
        {
            m_serverInfo = info;
        }

        /// <summary>
        /// 移除成员
        /// </summary>
        /// <param name="userId"></param>
        public void RemoveMember(uint userId)
        {
            if (null != m_serverInfo
                && null != m_serverInfo.memberlist
                && null != m_serverInfo.memberlist.member)
            {
                GameCmd.stClanMemberInfo removeMember = null;
                foreach (GameCmd.stClanMemberInfo info in m_serverInfo.memberlist.member)
                {
                    if (null == info)
                    {
                        continue;
                    }
                    if (info.id == userId)
                    {
                        removeMember = info;
                        break;
                    }
                }
                m_serverInfo.memberlist.member.Remove(removeMember);
            }
        }

        /// <summary>
        /// 添加成员
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="member"></param>
        public void AddMember(GameCmd.stClanMemberInfo member)
        {
            if (null != m_serverInfo
                && null != m_serverInfo.memberlist
                && null != m_serverInfo.memberlist.member)
            {
                GameCmd.stClanMemberInfo addMember = null;
                foreach (GameCmd.stClanMemberInfo info in m_serverInfo.memberlist.member)
                {
                    if (null == info)
                    {
                        continue;
                    }
                    if (info.id == member.id)
                    {
                        addMember = info;
                        break;
                    }
                }

                if (null != addMember)
                {
                    m_serverInfo.memberlist.member.Remove(addMember);
                }
                if (null != member)
                {
                    m_serverInfo.memberlist.member.Add(member);
                }
            }

        }

        /// <summary>
        /// 更新成员数据
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="member"></param>
        public void UpdateMember(GameCmd.stClanMemberInfo member)
        {
            if (null != m_serverInfo
                && null != m_serverInfo.memberlist
                && null != m_serverInfo.memberlist.member)
            {
                GameCmd.stClanMemberInfo updateMember = null;
                foreach (GameCmd.stClanMemberInfo info in m_serverInfo.memberlist.member)
                {
                    if (null == info)
                    {
                        continue;
                    }
                    if (info.id == member.id)
                    {
                        updateMember = info;
                        break;
                    }
                }

                if (null != updateMember)
                {
                    m_serverInfo.memberlist.member.Remove(updateMember);
                }
                if (null != member)
                {
                    m_serverInfo.memberlist.member.Add(member);
                }
            }
        }
    }

    #endregion

    #region staticmethod
    /// <summary>
    /// 是否当前职阶拥有权限right
    /// </summary>
    /// <param name="duty"></param>
    /// <param name="right"></param>
    /// <returns></returns>
    public static bool IsClanDutyHaveRight(GameCmd.enumClanDuty duty, ClanDutyRight right)
    {
        bool have = false;
        table.ClanDutyPermDataBase db
            = GameTableManager.Instance.GetTableItem<table.ClanDutyPermDataBase>((uint)duty);
        if (null != db)
        {
            switch (right)
            {
                case ClanDutyRight.AgreeApply:
                    have = (db.agreeApply == 1);
                    break;
                case ClanDutyRight.Appointment:
                    have = (db.appointment == 1);
                    break;
                case ClanDutyRight.BroadCaseGG:
                    have = (db.broadcastGG == 1);
                    break;
                case ClanDutyRight.BroadCastMsg:
                    have = (db.broadcastMsg == 1);
                    break;
                case ClanDutyRight.Expel:
                    have = (db.expel == 1);
                    break;
            }
        }
        return have;
    }
    #endregion



      public   static  int ClanMemberSort(stClanMemberInfo thisOne,stClanMemberInfo another) 
        {
            if (null == thisOne || null == another)
            {
                return 0;
            }
            //1-在线优先  2-职位优先  3-等级优先  4-id升序
            if (thisOne.is_online !=another.is_online)
            {
                return (int)(another.is_online - thisOne.is_online);
            }
            else if (thisOne.is_online == another.is_online)
            {
                // 0 -无  1 -族长 2- 副族长 7 - 成员
                if (thisOne.duty != another.duty)
                {
                    return (int)(thisOne.duty - another.duty);
                }
                else if (thisOne.duty == another.duty)
                {
                    if (thisOne.level != another.level)
                    {  
                        return (int)(another.level - thisOne.level);
                    }
                    else if (thisOne.level == another.level)
                    {
                        if (thisOne.id != another.id)
                        {
                            return (int)(thisOne.id - another.id);
                        }
                    }
                }
            }
            return 0;
        }
}
    