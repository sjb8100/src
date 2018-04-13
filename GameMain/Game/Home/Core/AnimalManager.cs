using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Client;

// 小动物移动
class AnimalMove
{
    public IAnimal m_Animal = null;

    private float m_fIdleEndTime = 0;

    public enum State
    {
        Idle = 0,
        Run,
    }

    public State state = State.Idle; // 0 休闲 1 行走

    public AnimalMove(IAnimal animal)
    {
        // 注册事件
        m_Animal = animal;
        m_fIdleEndTime = Time.realtimeSinceStartup;

        // 关注移动停止事件
        Engine.Utility.EventEngine.Instance().AddEventListener((int)Client.GameEventID.ENTITYSYSTEM_ENTITYSTOPMOVE, OnContrllerEvent);
    }

    public void Destroy()
    {
        
    }

    public void Update()
    {
        if (m_Animal == null)
        {
            return;
        }

        switch(state)
        {
            case State.Idle:
                {
                    if(Time.realtimeSinceStartup>=m_fIdleEndTime)
                    {
                        // 进入Run状态
                        Vector3 pos = AnimalManager.GetRandomPos();
                        Move move = new Move();
                        move.m_target = pos;
                        move.strRunAct = EntityAction.Run;
                        move.m_ignoreStand = false;
                        m_Animal.SendMessage(EntityMessage.EntityCommand_MoveTo, pos);

                        state = State.Run;
                    }
                    break;
                }
            case State.Run:
                {
                    break;
                }
            default:
                {
                    break;
                }
        }
    }

    public void OnContrllerEvent(int nEventID, object param)
    {
        if (m_Animal==null)
        {
            return;
        }
        if(nEventID==(int)Client.GameEventID.ENTITYSYSTEM_ENTITYSTOPMOVE)
        {
            stEntityStopMove stopMove = (stEntityStopMove)param;
            {
                if(stopMove.uid == m_Animal.GetUID())
                {
                    this.state = State.Idle;
                    this.m_fIdleEndTime = Time.realtimeSinceStartup + Engine.Utility.MathLib.Instance().RandomRange(3.0f, 10.0f);
                }
            }
        }
    }
}

class AnimalManager : Engine.Utility.ITimer
{
    private static uint ANIMAL_TIME = 0;
    // 牧场位置
    private static Vector3 m_PasturePos = Vector3.zero;
    // 牧场范围
    private static float m_fRange = 10.0f;
    // 动物列表
    private Dictionary<int, AnimalMove> m_Animals = new Dictionary<int, AnimalMove>();

    public void Init()
    {
        // 随机位置
        float x = GameTableManager.Instance.GetGlobalConfig<float>("PasturePosX");
        float y = GameTableManager.Instance.GetGlobalConfig<float>("PasturePosY");
        m_PasturePos = new Vector3(x,0,y);
        m_fRange = GameTableManager.Instance.GetGlobalConfig<float>("PastureRange");

        Engine.Utility.TimerAxis.Instance().SetTimer(AnimalManager.ANIMAL_TIME, 1000, this, Engine.Utility.TimerAxis.INFINITY_CALL, "家园 AnimalManager");
    }

    public void Clear()
    {
        m_Animals.Clear();

        Engine.Utility.TimerAxis.Instance().KillTimer(AnimalManager.ANIMAL_TIME, this);
    }

    public static Vector3 GetRandomPos()
    {
        Vector3 pos = new Vector3(Engine.Utility.MathLib.Instance().RandomRange(0, m_fRange), 0.0f, 0.0f);
        Quaternion rot = new Quaternion();
        rot.eulerAngles = new Vector3(0, Engine.Utility.MathLib.Instance().RandomRange(0, 360.0f), 0);
        pos = rot * pos + m_PasturePos;
        pos.z = -pos.z;
        return pos;
    }

    public void AddAnimal(IAnimal animal)
    {
        if(animal==null)
        {
            return;
        }

        AnimalMove move = new AnimalMove(animal);
        // 设置移动属性
        animal.SetProp((int)WorldObjProp.MoveSpeed, ClientGlobal.Instance().MainPlayer.GetProp((int)WorldObjProp.MoveSpeed));
        move.m_Animal = animal;

        move.state = AnimalMove.State.Idle;

        animal.SendMessage(EntityMessage.EntityCommand_SetPos, GetRandomPos());
        m_Animals.Add((int)move.m_Animal.GetID(), move);
    }

    public void RemoveAnimal(IAnimal animal)
    {
        if(m_Animals==null || animal==null)
        {
            return;
        }

        m_Animals.Remove((int)animal.GetID());
    }

    //-------------------------------------------------------------------------------------------------------
    public void OnTimer(uint nTimerID)
    {
        Dictionary<int, AnimalMove>.Enumerator iter = m_Animals.GetEnumerator();
        while (iter.MoveNext())
        {
            if (iter.Current.Value != null)
            {
                iter.Current.Value.Update();
            }
        }
    }
}

