using System;
using System.Collections.Generic;
using Client;
using Engine;
using Engine.Utility;
using System.Security;
using Mono.Xml;

public class FirstFightMgr
{
    private static FirstFightMgr s_Instance = null;
    public static FirstFightMgr Instance()
    {
        if (FirstFightMgr.s_Instance == null) { FirstFightMgr.s_Instance = new FirstFightMgr(); }
        return FirstFightMgr.s_Instance;
    }


    private float m_fPFS = 1f / 16f;

    private float m_fElapseTime = 0f;

    public uint m_nGameLoop = 0;

    private IEntity mainPlayer = null;

    // ai
    public KAIManager m_AIManager = new KAIManager();
    public KSkillManager m_SkillManager = new KSkillManager();

    public KObjectIndex<KNpc> m_NpcSet = new KObjectIndex<KNpc>();
    public KObjectIndex<KPlayer> m_PlayerSet = new KObjectIndex<KPlayer>();


    private KScene m_pCurScne = null;



    public FirstFightMgr()
    {


    }

    public int Init()
    {
        m_AIManager.Init();
        EventEngine.Instance().AddEventListener((int)GameEventID.SYSTEM_LOADSCENECOMPELETE, DoGameEvent);

        m_NpcSet.SetPrefix(0x01);


        return 1;
    }

    public void UnInit()
    {

    }

    public void FrameMove(float dt)
    {
        m_fElapseTime += dt;

        while (m_fElapseTime >= m_fPFS)
        {
            m_nGameLoop++;
            m_fElapseTime -= m_fPFS;

            //Test111();

            FrameMoveImpl();
        }
    }


    private void FrameMoveImpl()
    {
        //Dictionary<uint, KNpc>.Enumerator it = m_NpcSet.m_ObjectIndex.GetEnumerator();
        //while (it.MoveNext())
        //{
        //    KNpc pNpc = it.Current.Value;
        //    if (pNpc != null)
        //    {
        //        pNpc.Activate();
        //    }
        //}
        if (m_pCurScne != null)
        {
            m_pCurScne.Activate();
        }


    }

    void DoGameEvent(int eventID, object param)
    {
        if (eventID == (int)GameEventID.SYSTEM_LOADSCENECOMPELETE)
        {
            Client.stLoadSceneComplete pa = (Client.stLoadSceneComplete)param;
            if (pa.nMapID == 15)
            {
                IPlayer player = ClientGlobal.Instance().MainPlayer;
                if (player != null)
                {
                    CreatePlayer();
                    CameraFollow.Instance.Update(0f);


                    //LoadNpc();
                }
            }
        }
    }



    private IEntity CreateMainPlayer(string strName, int nJob, int nSex, EntityViewProp[] propList, bool bMainHost = false)
    {
        EntityCreateData data = new EntityCreateData();
        data.ID = 1;
        data.strName = strName;

        int speed = 0;
        Client.IPlayer player = ClientGlobal.Instance().MainPlayer;
        if (player != null)
        {
            speed = player.GetProp((int)WorldObjProp.MoveSpeed);
        }


        data.PropList = new EntityAttr[(int)PuppetProp.End - (int)EntityProp.Begin];
        int index = 0;
        data.PropList[index++] = new EntityAttr((int)PuppetProp.Job, 1);
        data.PropList[index++] = new EntityAttr((int)PuppetProp.Sex, 1);
        data.PropList[index++] = new EntityAttr((int)EntityProp.BaseID, 0);
        data.PropList[index++] = new EntityAttr((int)WorldObjProp.MoveSpeed, speed);
        data.ViewList = propList;

        IEntitySystem es = ClientGlobal.Instance().GetEntitySystem();
        if (es == null)
        {
            Engine.Utility.Log.Error("严重错误：EntitySystem is null!");
            return null;
        }

        IPuppet entity = es.CreateEntity(EntityType.EntityType_Puppet, data) as IPuppet;
        if (entity == null)
        {
            Engine.Utility.Log.Error("AddEntity:创建对象失败!");
            return null;
        }

        if (bMainHost)
        {
            IControllerSystem cs = ClientGlobal.Instance().GetControllerSystem();
            if (cs == null)
            {
                return null;
            }

            cs.GetActiveCtrl().SetHost(entity);
            CameraFollow.Instance.target = entity;



        }
        return entity;

    }
    private void CreatePlayer()
    {
        IPlayer player = ClientGlobal.Instance().MainPlayer;
        if (player != null)
        {
            int job = player.GetProp((int)PlayerProp.Job);
            int sex = player.GetProp((int)PlayerProp.Sex);
            int index = 0;
            EntityViewProp[] propList = new EntityViewProp[(int)Client.EquipPos.EquipPos_Max];
            propList[index++] = new EntityViewProp((int)Client.EquipPos.EquipPos_Body, 0);
            propList[index++] = new EntityViewProp((int)Client.EquipPos.EquipPos_Weapon, (int)0);

            mainPlayer = CreateMainPlayer(player.GetName(), job, sex, propList, true);
            if (mainPlayer != null)
            {
                mainPlayer.SendMessage(EntityMessage.EntityCommand_SetPos, new UnityEngine.Vector3(100, 0f, -100));

                PlayAni anim_param = new PlayAni();
                anim_param.strAcionName = EntityAction.Stand;
                anim_param.fSpeed = 1;
                anim_param.nStartFrame = 0;
                anim_param.nLoop = -1;
                anim_param.fBlendTime = 0.1f;
                mainPlayer.SendMessage(EntityMessage.EntityCommand_PlayAni, anim_param);
            }


        }


        KPlayer pPlayer = NewPlayer(0);
        pPlayer.m_pEntity = mainPlayer;

        AddPlayer(pPlayer, m_pCurScne);

    }


    KNpc npc1 = null;
    KNpc npc2 = null;
    KNpc npc3 = null;
    KNpc npc4 = null;

    public void LoadNpc()
    {
        npc1 = NewNpc();
        AddNpc(npc1, m_pCurScne);

        npc1.SetDisplayData(13811);
        npc1.SetPosition(new UnityEngine.Vector3(100, 0, -100));


        //npc 技能设置
        npc1.m_pTemplate.dwSkillIDList[0] = 10005;
        npc1.m_pTemplate.nSkillCastInterval[0] = 16;// 间隔一秒



        npc1.m_AIVM.Setup(npc1, 1);

        //-----------------------------------------------------------------------------
        npc2 = NewNpc();
        AddNpc(npc2, m_pCurScne);


        npc2.m_pTemplate.dwSkillIDList[0] = 10005;
        npc2.m_pTemplate.nSkillCastInterval[0] = 24;// 间隔一秒


        npc2.SetDisplayData(13811);
        npc2.SetPosition(new UnityEngine.Vector3(100, 0, -90));

        npc2.m_AIVM.Setup(npc2, 1);
        //-----------------------------------------------------------------------------

        npc3 = NewNpc();
        AddNpc(npc3, m_pCurScne);


        npc3.m_pTemplate.dwSkillIDList[0] = 10001;
        npc3.m_pTemplate.nSkillCastInterval[0] = 18;// 间隔一秒


        npc3.SetDisplayData(10001);
        npc3.SetPosition(new UnityEngine.Vector3(90, 0, -90));

        npc3.m_AIVM.Setup(npc3, 1);

        //-----------------------------------------------------------------------------




    }


    bool bdaxia = true;
    public void LoadNpc_1()
    {
        if (bdaxia)
        {
            KNpc npc3 = NewNpc();
            AddNpc(npc3, m_pCurScne);

            npc3.m_pTemplate.dwSkillIDList[0] = 10131;
            npc3.m_pTemplate.nSkillCastInterval[0] = 6 * 16;// 间隔一秒

            npc3.SetDisplayData(23005);

            Random ran = new Random();
            int RandKey = ran.Next(85, 100);

            npc3.SetPosition(new UnityEngine.Vector3(RandKey, 0, -RandKey));

            npc3.m_AIVM.Setup(npc3, 1);

            bdaxia = false;
        }
        else
        {
            KNpc npc1 = NewNpc();
            AddNpc(npc1, m_pCurScne);

            npc1.SetDisplayData(13811);


            Random ran = new Random();
            int RandKey = ran.Next(85, 100);

            npc1.SetPosition(new UnityEngine.Vector3(RandKey, 0, -RandKey));


            //npc 技能设置
            npc1.m_pTemplate.dwSkillIDList[0] = 10005;
            npc1.m_pTemplate.nSkillCastInterval[0] = 16;// 间隔一秒


            npc1.m_AIVM.Setup(npc1, 1);

            bdaxia = true;
        }

    }

    public void LeaveScene()
    {

    }

    private void HideUI()
    {

    }

    public int LoadFightConfig(string strConfig)
    {
        int nResult = 0;

        byte[] bytes = Engine.Utility.FileUtils.Instance().ReadFile(strConfig);
        if (bytes == null) { goto Exit0; }

        string xml = System.Text.Encoding.UTF8.GetString(bytes);
        bytes = null;
        SecurityElement xmlRoot = XmlParser.Parser(xml);
        if (xmlRoot == null) { goto Exit0; }

        //foreach (SecurityElement child_TimelineContainers in xmlRoot.Children)
        //{
        //    if (child_TimelineContainers.Tag == "TimelineContainers")
        //    {
        //        string strName = child_TimelineContainers.Attribute("Name");

        //    }
        //}

        nResult = 1;
    Exit0:
        return nResult;
    }

    public KScene NewClientScene(uint uMapID)
    {
        KScene pSecne = null;

        pSecne = new KScene();

        pSecne.Enter();

        m_pCurScne = pSecne;
        return pSecne;
    }

    public KPlayer NewPlayer(uint uPlayerID)
    {
        int nRetCode = 0;
        KPlayer pPlayer = new KPlayer();

        nRetCode = m_PlayerSet.Register(pPlayer, 0);
        if (nRetCode == 0) { goto Exit0; }

        nRetCode = pPlayer.Init();
        if (nRetCode == 0) { goto Exit0; }



    Exit0:
        return pPlayer;
    }

    public int AddPlayer(KPlayer pPlayer, KScene pScene)
    {
        pScene.AddPlayer(pPlayer);
        return 1;
    }


    public KNpc NewNpc()
    {
        int nRetCode = 0;
        bool bNpcRegFlag = false;

        KNpc pResult = null;
        KNpc pNpc = new KNpc();

        nRetCode = FirstFightMgr.Instance().m_NpcSet.Register(pNpc, 0);
        if (nRetCode == 0) { goto Exit0; }

        bNpcRegFlag = true;

        nRetCode = pNpc.Init();
        if (nRetCode == 0) { goto Exit0; }

        pResult = pNpc;

    Exit0:
        if (pResult == null)
        {
            if (bNpcRegFlag)
            {
                FirstFightMgr.Instance().m_NpcSet.Unregister(pNpc);
                bNpcRegFlag = false;
            }
        }
        return pResult;

    }

    public int AddNpc(KNpc pNpc, KScene pScene)
    {
        pScene.AddNpc(pNpc);

        return 1;
    }

    //坚持
    private int RemoveNpc(KNpc pNpc)
    {
        int nResult = 0;
        int nRetCode = 0;

        if (pNpc == null)
        {
            goto Exit0;
        }
        if (pNpc.m_pScene == null)
        {
            goto Exit0;
        }

        pNpc.DestroyDisplayData();

        nRetCode = pNpc.m_pScene.RemoveNpc(pNpc);
        if (nRetCode == 0)
        {
            goto Exit0;
        }

        nResult = 1;
    Exit0:
        return nResult;

    }

    // 这里是否需要ref 参数
    public int DeleteNpc(KNpc pNpc)
    {
        if (pNpc == null)
        {
            //error
        }

        if (pNpc.m_pScene != null)
        {
            RemoveNpc(pNpc);
        }

        pNpc.UnInit();

        FirstFightMgr.Instance().m_NpcSet.Unregister(pNpc);

        pNpc = null;

        return 1;

    }

}

