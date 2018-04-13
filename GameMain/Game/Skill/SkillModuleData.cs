using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Engine.Utility;
using table;
using Client;
using Engine;
class SkillModuleData : BaseModuleData, IManager
{
    Queue<stSkillCommond> m_commondQue = new Queue<stSkillCommond>();

    bool m_bSkillLongPress = false;
    #region IManager Method
    public void Initialize()
    {
        RegisterGlobalEvent(true);
    }

    public void Reset(bool depthClearData = false)
    {

    }

    public void Process(float deltaTime)
    {

    }

    public void ClearData()
    {

    }
    #endregion
    void DoInsertSkill()
    {
        if (m_commondQue.Count > 0)
        {

            //技能
            IPlayer mp = MainPlayerHelper.GetMainPlayer();
            if (mp != null)
            {
                ISkillPart sp = mp.GetPart(EntityPart.Skill) as ISkillPart;
                if (sp != null)
                {

                    stSkillCommond st = m_commondQue.Dequeue();
                    if (st.type == 0)
                    {
                        OnUseSkill(st.skillID);
                    }

                }
            }
        }
    }
    bool IsCommonSkill(uint skillID)
    {
        SkillDatabase db = GameTableManager.Instance.GetTableItem<SkillDatabase>(skillID);
        if (db == null)
        {
            return false;
        }
        return db.isnormal;
    }
    public void RegisterGlobalEvent(bool register)
    {
        if (register)
        {
            EventEngine.Instance().AddEventListener((int)GameEventID.SKILLSYSTEM_ADDSKILLCMD, SkillEvent);
            EventEngine.Instance().AddEventListener((int)GameEventID.SKILLSYSTEM_CLEARSKILLCMD, SkillEvent);
            EventEngine.Instance().AddEventListener((int)GameEventID.SKLL_LONGPRESS, SkillEvent);
            EventEngine.Instance().AddEventListener((int)GameEventID.ENTITYSYSTEM_ENTITYBEGINMOVE, SkillEvent);
            EventEngine.Instance().AddEventListener((int)GameEventID.ROBOTCOMBAT_START, SkillEvent);
            EventEngine.Instance().AddEventListener((int)GameEventID.SKILLSYSTEM_STIFFTIMEOVER, SkillEvent);

            EventEngine.Instance().AddVoteListener((int)GameVoteEventID.SKILL_CANUSE, SkillVoteCallBack);
          //  EventEngine.Instance().AddVoteListener((int)GameVoteEventID.ENTITYSYSTEM_VOTE_ENTITYMOVE, SkillVoteCallBack);
        }
        else
        {
            EventEngine.Instance().RemoveEventListener((int)GameEventID.SKILLSYSTEM_ADDSKILLCMD, SkillEvent);
            EventEngine.Instance().RemoveEventListener((int)GameEventID.SKILLSYSTEM_CLEARSKILLCMD, SkillEvent);
            EventEngine.Instance().RemoveEventListener((int)GameEventID.SKLL_LONGPRESS, SkillEvent);
            EventEngine.Instance().RemoveEventListener((int)GameEventID.ENTITYSYSTEM_ENTITYBEGINMOVE, SkillEvent);
            EventEngine.Instance().RemoveEventListener((int)GameEventID.ROBOTCOMBAT_START, SkillEvent);
            EventEngine.Instance().RemoveEventListener((int)GameEventID.SKILLSYSTEM_STIFFTIMEOVER, SkillEvent);

            EventEngine.Instance().RemoveAllVoteListener((int)GameVoteEventID.SKILL_CANUSE);
          //  EventEngine.Instance().RemoveAllVoteListener((int)GameVoteEventID.ENTITYSYSTEM_VOTE_ENTITYMOVE);
        }
    }
    bool SkillVoteCallBack(int eventID,object param)
    {
        if(eventID == (int)GameVoteEventID.SKILL_CANUSE)
        {
            if(NetService.Instance.BCheckingTime)
            {
                Log.Error("正在对时，不能使用技能");
                return false;
            }
        }
        else if(eventID == (int)GameVoteEventID.ENTITYSYSTEM_VOTE_ENTITYMOVE)
        {
            if (NetService.Instance.BCheckingTime)
            {
                Log.Error("正在对时，不能移动");
                return false;
            }
        }
        return true;
    }
    void SkillEvent(int eventID, object param)
    {
        if (eventID == (int)GameEventID.SKILLSYSTEM_ADDSKILLCMD)
        {
            stSkillCommond st = (stSkillCommond)param;
            AddCommondQueue(st);
        }
        else if (eventID == (int)GameEventID.SKILLSYSTEM_CLEARSKILLCMD)
        {
            ClearCommondQueue();
        }
        else if (eventID == (int)GameEventID.SKLL_LONGPRESS)
        {
            stSkillLongPress st = (stSkillLongPress)param;
            m_bSkillLongPress = st.bLongPress;
        }
        else if (eventID == (int)GameEventID.ENTITYSYSTEM_ENTITYBEGINMOVE)
        {
            stEntityBeginMove move = (stEntityBeginMove)param;
            if (move.uid == MainPlayerHelper.GetPlayerUID())
            {
                ClearCommondQueue();
            }
        }
        else if (eventID == (int)GameEventID.ROBOTCOMBAT_START)
        {
            stSkillLongPress longPress = new stSkillLongPress();
            longPress.bLongPress = false;
            EventEngine.Instance().DispatchEvent((int)GameEventID.SKLL_LONGPRESS, longPress);
        }
        else if (eventID == (int)GameEventID.SKILLSYSTEM_STIFFTIMEOVER)
        {
            DoInsertSkill();
        }
    }
    public void AddCommondQueue(stSkillCommond st)
    {
        if (!m_commondQue.Contains(st))
        {
            m_commondQue.Enqueue(st);
        }
    }
    public void ClearCommondQueue()
    {
        m_commondQue.Clear();
    }
    void AddSkillCommond(uint uSkillID)
    {
        if (IsCommonSkill(uSkillID))
        {
            return;
        }
        if (m_bSkillLongPress)
        {
            //点击其他技能 解除长按
            stSkillLongPress longPress = new stSkillLongPress();
            longPress.bLongPress = false;
            EventEngine.Instance().DispatchEvent((int)GameEventID.SKLL_LONGPRESS, longPress);


            EventEngine.Instance().DispatchEvent((int)GameEventID.SKILLSYSTEM_CLEARSKILLCMD, null);


            stSkillCommond cmd = new stSkillCommond();
            cmd.type = 0;
            cmd.skillID = uSkillID;
            EventEngine.Instance().DispatchEvent((int)GameEventID.SKILLSYSTEM_ADDSKILLCMD, cmd);


            stSkillCommond st = new stSkillCommond();
            st.type = 0;
            st.skillID = DataManager.Manager<LearnSkillDataManager>().GetCommonSkillIDByJob();
            EventEngine.Instance().DispatchEvent((int)GameEventID.SKILLSYSTEM_ADDSKILLCMD, st);

        }
    }
    public void OnUseSkill(uint uSkillID)
    {
        if (uSkillID == 0)
        {
            return;
        }
        IControllerSystem cs = ClientGlobal.Instance().GetControllerSystem();
        if (cs == null)
        {
            Engine.Utility.Log.Error("ExecuteCmd: ControllerSystem is null");
            return;
        }

        ICombatRobot combot = cs.GetCombatRobot();
        if (combot.Status == CombatRobotStatus.RUNNING)
        {
            //解除普攻连击
            stSkillLongPress longPress = new stSkillLongPress();
            longPress.bLongPress = false;
            EventEngine.Instance().DispatchEvent((int)GameEventID.SKLL_LONGPRESS, longPress);
            combot.InsertSkill(uSkillID);

            return;
        }
        bool canuse = EventEngine.Instance().DispatchVote((int)GameVoteEventID.SKILL_CANUSE, uSkillID);
        if (canuse)
        {
            table.SkillDatabase db = GameTableManager.Instance.GetTableItem<table.SkillDatabase>(uSkillID, 1);
            if (db != null)
            {
                IControllerSystem ctrl = ClientGlobal.Instance().GetControllerSystem();
                if (ctrl != null)
                {
                    MessageCode code = MessageCode.MessageCode_ButtonX;

                    ctrl.OnMessage(code, uSkillID);
                }

            }
        }
        else
        {
            IPlayer mainPlayer = MainPlayerHelper.GetMainPlayer();
            if(mainPlayer != null)
            {
                ISkillPart skillPart = mainPlayer.GetPart(EntityPart.Skill) as ISkillPart;
                if(skillPart != null)
                {
                    Client.stTipsEvent en = skillPart.GetSkillNotUseReason(uSkillID);
                    if(en.errorID != 0)
                    {
                        EventEngine.Instance().DispatchEvent((int)GameEventID.TIPS_EVENT, en);
                    }
                }
            }
           
            AddSkillCommond(uSkillID);
        }
    }

}

