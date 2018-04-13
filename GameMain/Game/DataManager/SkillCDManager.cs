using System;
using System.Collections.Generic;
using Client;
public class SkillCDManager : IManager
{
    Dictionary<uint, SkillCDInfo> m_dictSkillCD = null;
    List<uint> cdkeys = null;
    Dictionary<uint, table.SkillDatabase> m_dictskill;
    public void Initialize()
    {
        m_dictSkillCD = new Dictionary<uint, SkillCDInfo>();
        cdkeys = new List<uint>();
        Engine.Utility.EventEngine.Instance().AddVoteListener((int)GameVoteEventID.SKILL_CDING, VoteCallBack);
        Engine.Utility.EventEngine.Instance().AddEventListener((int)Client.GameEventID.SKILLCD_BEGIN, SkillEvent);
    }

    public void Reset(bool depthClearData = false)
    {
    }
    public void ClearData()
    {

    }
    public void Process(float deltaTime)
    {
        if (m_dictSkillCD == null)
        {
            return;
        }
        if (m_dictSkillCD.Count > 0)
        {
            for (int i = 0; i < cdkeys.Count; i++)
            {
                m_dictSkillCD[cdkeys[i]].currTime -= deltaTime;
                if (m_dictSkillCD[cdkeys[i]].currTime <= 0)
                {
                    m_dictSkillCD.Remove(cdkeys[i]);
                    Engine.Utility.Log.LogGroup("ZCX", "skill cd end {0}", cdkeys[i]);
                    //普攻不用加CD特效
                    if (cdkeys[i] != 101)
                    {
                        Engine.Utility.EventEngine.Instance().DispatchEvent((int)GameEventID.SKILLSYSTEM_CDEND, cdkeys[i]);
                    }                  
                    cdkeys.RemoveAt(i);
                    i--;
                   
                }
            }
        }
    }

    bool VoteCallBack(int nEventID, object param)
    {
        if (nEventID == (int)GameVoteEventID.SKILL_CDING)
        {
            uint skillid = (uint)param;
            if (GetSkillCD(skillid) > 0)
            {
                return true;
            }
        }
        return false;
    }
    void SkillEvent(int eventID, object param)
    {
        if (eventID == (int)Client.GameEventID.SKILLCD_BEGIN)
        {
            Client.stSkillCDChange st = (Client.stSkillCDChange)param;
      
          
            AddSkillCD(st.skillid,st.cd);
        }
    }
    //public void AddSkillCD(uint skillid)
    //{
    //    if (m_dictskill == null)
    //    {
    //        Client.IPlayer player = Client.ClientGlobal.Instance().MainPlayer;
    //        Client.ISkillPart m_skillPart = player.GetPart(Client.EntityPart.Skill) as Client.ISkillPart;
    //        m_dictskill = m_skillPart.GetCurSkills();
    //    }
    //    table.SkillDatabase skillTable = null;
    //    if (m_dictskill.ContainsKey(skillid))
    //    {
    //        skillTable =  m_dictskill[skillid];
    //    }
    //    if (skillTable == null)
    //    {
    //        skillTable = GameTableManager.Instance.GetTableItem<table.SkillDatabase>(skillid);
    //    }

    //    if (skillTable != null)
    //    {
    //        if (!m_dictSkillCD.ContainsKey(skillid))
    //        {
    //            m_dictSkillCD.Add(skillid, new SkillCDInfo());
    //            cdkeys.Add(skillid);
    //        }
    //        m_dictSkillCD[skillid].totalTime = skillTable.dwIntervalTime;
    //        m_dictSkillCD[skillid].skillid = (int)skillid;
    //        m_dictSkillCD[skillid].currTime = skillTable.dwIntervalTime;
    //    }
    //    else
    //    {
    //        table.RideSkillDes rideskill = GameTableManager.Instance.GetTableItem<table.RideSkillDes>(skillid);
    //        if (rideskill == null)
    //        {
    //            return;
    //        }
    //        if (!m_dictSkillCD.ContainsKey(skillid))
    //        {
    //            m_dictSkillCD.Add(skillid, new SkillCDInfo());
    //            cdkeys.Add(skillid);
    //        }
    //        m_dictSkillCD[skillid].totalTime = rideskill.skillCD;
    //        m_dictSkillCD[skillid].skillid = (int)skillid;
    //        m_dictSkillCD[skillid].currTime = rideskill.skillCD;
    //    }
    //}

    public void AddCommonSkillCD(uint skillid)
    {
        if (m_dictskill == null)
        {
            Client.IPlayer player = Client.ClientGlobal.Instance().MainPlayer;
            Client.ISkillPart m_skillPart = player.GetPart(Client.EntityPart.Skill) as Client.ISkillPart;
            m_dictskill = m_skillPart.GetCurSkills();
        }
        table.SkillDatabase skillTable = null;
        if (m_dictskill.ContainsKey(skillid))
        {
            skillTable = m_dictskill[skillid];

            if (!m_dictSkillCD.ContainsKey(skillid))
            {
                m_dictSkillCD.Add(skillid, new SkillCDInfo());
                cdkeys.Add(skillid);
            }
            m_dictSkillCD[skillid].totalTime = skillTable.dwCommonCDTime;
            m_dictSkillCD[skillid].skillid = (int)skillid;
            m_dictSkillCD[skillid].currTime = skillTable.dwCommonCDTime;
        }
    }

    public void AddSkillCD(uint skillid,int cd)
    {
      
        if (m_dictskill == null)
        {
            Client.IPlayer player = Client.ClientGlobal.Instance().MainPlayer;
            Client.ISkillPart m_skillPart = player.GetPart(Client.EntityPart.Skill) as Client.ISkillPart;
            m_dictskill = m_skillPart.GetCurSkills();
        }
        table.SkillDatabase skillTable = null;
        if (m_dictskill.ContainsKey(skillid))
        {
            skillTable = m_dictskill[skillid];
        }
        if (skillTable == null)
        {
            skillTable = GameTableManager.Instance.GetTableItem<table.SkillDatabase>(skillid);
        }

        if (skillTable != null)
        {
            if (!m_dictSkillCD.ContainsKey(skillid))
            {
                m_dictSkillCD.Add(skillid, new SkillCDInfo());
                cdkeys.Add(skillid);
            }
            if(cd == -1)
            {
                m_dictSkillCD[skillid].totalTime = skillTable.dwIntervalTime;
            }
            else
            {
                m_dictSkillCD[skillid].totalTime = cd;
            }
            m_dictSkillCD[skillid].skillid = (int)skillid;
            m_dictSkillCD[skillid].currTime = m_dictSkillCD[skillid].totalTime;
        }
        else
        {
            table.RideSkillDes rideskill = GameTableManager.Instance.GetTableItem<table.RideSkillDes>(skillid);
            if (rideskill == null)
            {
                return;
            }
            if (!m_dictSkillCD.ContainsKey(skillid))
            {
                m_dictSkillCD.Add(skillid, new SkillCDInfo());
                cdkeys.Add(skillid);
            }
            m_dictSkillCD[skillid].totalTime = rideskill.skillCD;
            m_dictSkillCD[skillid].skillid = (int)skillid;
            m_dictSkillCD[skillid].currTime = rideskill.skillCD;
        }
    }

    public SkillCDInfo GetSkillCDBySkillId(uint skillid)
    {
        if (m_dictSkillCD.ContainsKey(skillid))
        {
            return m_dictSkillCD[skillid];
        }
        return null;
    }

    /// <summary>
    /// including ride skill
    /// </summary>
    /// <param name="skillid"></param>
    /// <returns></returns>
    public float GetSkillCD(uint skillid)
    {
        if (m_dictSkillCD.ContainsKey(skillid))
        {
            return m_dictSkillCD[skillid].currTime;
        }
        return  0;
    }
}
