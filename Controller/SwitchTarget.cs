using System;
using System.Collections;
using System.Collections.Generic;
using Client;
using UnityEngine;

namespace Controller
{
    #region
    class MonsterDistanceCondition : IFindCondition<INPC>
    {
        Vector3 m_playerPos = Vector3.zero;
        //Camera m_maincamera = null;
        float m_dis = 0;
        public MonsterDistanceCondition(Vector3 playerPos, float dis)
        {
            m_playerPos = playerPos;
            m_dis = dis;
        }

        public bool Conform(INPC e)
        {
            if (!e.CanSelect())
            {
                return false;
            }
            if (e.IsMonster())
            {
                Vector3 distance = m_playerPos - e.GetPos();
                if (distance.sqrMagnitude <= m_dis*m_dis)
                {
                    return true;
                }
                //Vector3 pos = m_maincamera.WorldToViewportPoint(e.GetPos());
                //bool isVisible = (m_maincamera.orthographic || pos.z > 0f) && (pos.x > 0f && pos.x < 1f && pos.y > 0f && pos.y < 1f);
                //return isVisible;
            }
            return false;
        }
    }

    class PlayerDistanceCondition : IFindCondition<IPlayer>
    {
        Vector3 m_playerPos = Vector3.zero;
       // Camera m_maincamera = null;
        PLAYERPKMODEL m_pkmodel;
        IPlayer m_mainPlayer;
        Client.IControllerHelper m_controllerhelper;
        float m_dis = 0;
        public PlayerDistanceCondition(PLAYERPKMODEL pkmodel, Vector3 playerPos, float dis)
        {
            m_playerPos = playerPos;
            m_dis = dis;
            m_pkmodel = pkmodel;
            m_mainPlayer = ControllerSystem.m_ClientGlobal.MainPlayer;
            m_controllerhelper = ControllerSystem.m_ClientGlobal.GetControllerSystem().GetControllerHelper();
        }

        public bool Conform(IPlayer e)
        {
            if (m_pkmodel == PLAYERPKMODEL.PKMODE_M_TEAM)
            {
                if (m_controllerhelper != null && m_controllerhelper.IsSameTeam(e))
                {
                    return false;
                }
            }
            else if (m_pkmodel == PLAYERPKMODEL.PKMODE_M_FAMILY)
            {
                if (m_controllerhelper != null && m_controllerhelper.IsSameFamily(e))
                {
                    return false;
                }
            }
            else if (m_pkmodel == PLAYERPKMODEL.PKMODE_M_CAMP)
            {
                if (m_controllerhelper != null && !m_controllerhelper.IsEnemyCamp(e))
                {
                    return false;
                }
            }
            else if (m_pkmodel == PLAYERPKMODEL.PKMODE_M_JUSTICE)
            {
                //6)	善恶模式：白名、黄名玩家可攻击灰名、红名玩家以及怪物，灰名、红名玩家可以攻击白名、黄名玩家以及怪物。
                if (m_mainPlayer != null)
                {
                    GameCmd.enumGoodNess mainPlayerGoodNess = (GameCmd.enumGoodNess)m_mainPlayer.GetProp((int)PlayerProp.GoodNess);
                    GameCmd.enumGoodNess targetGoodNess = (GameCmd.enumGoodNess)e.GetProp((int)PlayerProp.GoodNess);
                    //GoodNess_Badman Color.yellow;
                    //GoodNess_Normal Color.white;
                    //GoodNess_Rioter  Color.gray;
                    //GoodNess_Evil Color.red;
                    if (mainPlayerGoodNess == GameCmd.enumGoodNess.GoodNess_Normal || GameCmd.enumGoodNess.GoodNess_Badman == mainPlayerGoodNess)
                    {
                        if (targetGoodNess != GameCmd.enumGoodNess.GoodNess_Rioter && GameCmd.enumGoodNess.GoodNess_Evil != targetGoodNess)
                        {
                            return false;
                        }
                    }
                    else if (mainPlayerGoodNess == GameCmd.enumGoodNess.GoodNess_Rioter || GameCmd.enumGoodNess.GoodNess_Evil == mainPlayerGoodNess)
                    {
                        if (targetGoodNess != GameCmd.enumGoodNess.GoodNess_Normal && GameCmd.enumGoodNess.GoodNess_Badman != targetGoodNess)
                        {
                            return false;
                        }
                    }
                }
            }

            if (e.IsDead())
            {
                return false;
            }
            Vector3 distance = m_playerPos - e.GetPos();
            if (distance.sqrMagnitude <= m_dis * m_dis)
            {
                return true;
            }
            //Vector3 pos = m_maincamera.WorldToViewportPoint(e.GetNode().GetWorldPosition());
            //bool isVisible = (m_maincamera.orthographic || pos.z > 0f) && (pos.x > 0f && pos.x < 1f && pos.y > 0f && pos.y < 1f);
            //return isVisible;
            return true;
        }
    }

    class CountryDistanceCondition : IFindCondition<IPlayer>
    {
        Vector3 m_playerPos = Vector3.zero;
        //Camera m_maincamera = null;
        int m_country = 0;
        float m_dis = 0;
        public CountryDistanceCondition(Vector3 playerPos, float dic, int country)
        {
            m_playerPos = playerPos;
            //m_maincamera = cam;
            m_country = country;
            m_dis = dic;
        }

        public bool Conform(IPlayer e)
        {
            if (e.IsDead())
            {
                return false;
            }
            Vector3 distance = m_playerPos - e.GetPos();
            if (distance.sqrMagnitude <= m_dis*m_dis)
            {
                return true;
            }
            //if (m_country != e.GetProp((int)PlayerProp.Country) )
            //{
            //    Vector3 pos = m_maincamera.WorldToViewportPoint(e.GetNode().GetWorldPosition());
            //   return (m_maincamera.orthographic || pos.z > 0f) && (pos.x > 0f && pos.x < 1f && pos.y > 0f && pos.y < 1f);
            //}
            return false;
        }
    }
    #endregion

    class SwitchTarget : ISwitchTarget, Engine.Utility.ITimer
    {
        const int TIMER_ID = 1230;
        IEntity m_currEntity = null;
        public const int SQRMagnitude = 400;
        int m_currIndex = -1;
        List<IEntity> m_lstEntity = new List<IEntity>();
        Vector3 playerPos;
        Camera camera = null;
        bool m_bSetTimer = false;
        public SwitchTarget()
        {
            Engine.Utility.EventEngine.Instance().AddEventListener((int)GameEventID.ENTITYSYSTEM_TARGETCHANGE, OnEvent);
            Engine.Utility.EventEngine.Instance().AddEventListener((int)GameEventID.ENTITYSYSTEM_ENTITYDEAD, OnEvent);
            Engine.Utility.EventEngine.Instance().AddEventListener((int)GameEventID.ENTITYSYSTEM_REMOVEENTITY, OnEvent);
        }

        public void Clear()
        {
            m_lstEntity.Clear();
        }

        public void Relese()
        {
            Clear();
            Engine.Utility.EventEngine.Instance().RemoveEventListener((int)GameEventID.ENTITYSYSTEM_TARGETCHANGE, OnEvent);
            Engine.Utility.EventEngine.Instance().RemoveEventListener((int)GameEventID.ENTITYSYSTEM_ENTITYDEAD, OnEvent);
            Engine.Utility.EventEngine.Instance().RemoveEventListener((int)GameEventID.ENTITYSYSTEM_REMOVEENTITY, OnEvent);
        }
        public void OnTimer(uint uTimerID)
        {
            if (m_currEntity == null)
            {
                return;
            }

            if (CheckEntityVisible(m_currEntity) == false)
            {
                ReleseTarget();
            }
        }


        void OnEvent(int evenId, object param)
        {
            if (evenId == (int)GameEventID.ENTITYSYSTEM_ENTITYDEAD)
            {
                if (m_currEntity == null)
                {
                    return;
                }
                stEntityDead ed =  (stEntityDead)param;
                IEntity e = ControllerSystem.m_ClientGlobal.GetEntitySystem().FindEntity(ed.uid);
                if (e != null && e.GetUID() == m_currEntity.GetUID())
                {
                    Engine.RareEngine.Instance().Root.GetComponent<MonoBehaviour>().StartCoroutine(WaitFoeOneSec(m_currEntity));
                }
            }
            else if (evenId == (int)GameEventID.ENTITYSYSTEM_REMOVEENTITY)
            {
                if (m_currEntity == null)
                {
                    return;
                }
                Client.stRemoveEntity removeEntiy = (Client.stRemoveEntity)param;
                if (removeEntiy.uid == m_currEntity.GetUID())
                {
                    ReleseTarget();
                }
            }
            else if (evenId == (int)GameEventID.ENTITYSYSTEM_TARGETCHANGE)
            {
                stTargetChange tc = (stTargetChange)param;
                if (tc.target != null)
                {
                    m_currEntity = tc.target;
                    SetTimer();
                }
            }
        }

        void ReleseTarget()
        {
            ControllerSystem.m_ClientGlobal.GetControllerSystem().GetActiveCtrl().UpdateTarget(null);
            m_currEntity = null;
            KillTimer();
            m_bSetTimer = false;
        }

        void SetTimer()
        {
            if (m_bSetTimer )
            {
                return;
            }
            Engine.Utility.TimerAxis.Instance().SetTimer(TIMER_ID, 100, this, Engine.Utility.TimerAxis.INFINITY_CALL, "Switch target");
            m_bSetTimer = true;
        }
        void KillTimer()
        {
            Engine.Utility.TimerAxis.Instance().KillTimer(TIMER_ID, this);
        }

        IEnumerator WaitFoeOneSec(IEntity e)
        {
            yield return new WaitForSeconds(1.5f);
            if (e != null && m_currEntity != null && e.GetUID() == m_currEntity.GetUID())
            {
                ReleseTarget();
            }
        }

        bool CheckEntityVisible(IEntity e)
        {
            if (e != null)
            {
                uint range = GameTableManager.Instance.GetGlobalConfig<uint>("Switch_Target_Range");
                Vector3 distance = ControllerSystem.m_ClientGlobal.MainPlayer.GetPos() - e.GetPos();
                if (distance.sqrMagnitude > range * range)
                {
                    return false;
                }
                else 
                {
                    return true;
                }
                //GetCamera();
                //Engine.Node node = e.GetNode();
                //if (node != null)
                //{
                //    Vector3 pos = camera.WorldToViewportPoint(node.GetWorldPosition());
                //    return (camera.orthographic || pos.z > 0f) && (pos.x > 0f && pos.x < 1f && pos.y > 0f && pos.y < 1f);
                //}
                //else
                //{
                //    Engine.Utility.Log.Error("{0}node is null",e.GetObjName());
                //}
            }
            return false;
        }

        void GetCamera()
        {
            if (camera == null)
            {
                string strCameraName = "MainCamera";
                Engine.ICamera cam = Engine.RareEngine.Instance().GetRenderSystem().GetCamera(ref strCameraName);
                camera = cam.GetNode().GetTransForm().GetComponent<Camera>();
            }
        }
        public void Switch()
        {
            GetCamera();

            if (m_lstEntity.Count > 0)
            {
                m_currIndex++;
                bool found = false;
                while (m_lstEntity.Count > m_currIndex && !found)
                {
                    IEntity temp = m_lstEntity[m_currIndex];
                    if (temp == null)//被删除
                    {
                        m_currIndex++;
                    }
                    else
                    {
                        if (CheckEntityVisible(temp))// 有可能会移动到视野外
                        {
                            found = true;
                            m_currEntity = temp;
                            break;
                        }
                        else
                        {
                            m_currIndex++;
                        }
                    }
                }

                if (!found)
                {
                    FindTargetList();
                    if (m_lstEntity.Count > 0)
                    {
                        m_currIndex = 0;
                        IEntity temp = m_lstEntity[m_currIndex];
                        if (m_currEntity != null && m_currEntity.GetUID() == temp.GetUID() && m_lstEntity.Count != 1)
                        {
                            m_currIndex++;
                            m_currEntity = temp;
                        }
                        else
                        {
                            m_currEntity = temp;
                        }             
                    }
                }
            }
            else
            {
                FindTargetList();
                if (m_lstEntity.Count > 0)
                {
                    m_currIndex = 0;
                    m_currEntity = m_lstEntity[m_currIndex];
                    IEntity currtarget = ControllerSystem.m_ClientGlobal.GetControllerSystem().GetActiveCtrl().GetCurTarget();
                    if ((currtarget != null && currtarget.GetUID() == m_currEntity.GetUID()))
                    {
                        m_currIndex++;
                        if (m_lstEntity.Count > m_currIndex)
                        {
                            m_currEntity = m_lstEntity[m_currIndex];
                        }
                    }
                }
            }

            if (m_currEntity != null)
            {
                ControllerSystem.m_ClientGlobal.GetControllerSystem().GetActiveCtrl().UpdateTarget(m_currEntity);
                SetTimer();
            }        

            Engine.Utility.EventEngine.Instance().DispatchEvent((int)GameEventID.SKILLSYSYTEM_TAB, m_currEntity);
        }



        private void FindTargetList()
        {
            m_lstEntity.Clear();
            playerPos = ControllerSystem.m_ClientGlobal.MainPlayer.GetPos();
            uint range = GameTableManager.Instance.GetGlobalConfig<uint>("Switch_Target_Range");
            Camera c = camera;
            int pkmodel = ControllerSystem.m_ClientGlobal.MainPlayer.GetProp((int)PlayerProp.PkMode);
            PLAYERPKMODEL model = (PLAYERPKMODEL)pkmodel;
            IMapSystem ms = ControllerSystem.m_ClientGlobal.GetMapSystem();
            if (ms == null)
            {
                return ;
            }
            MapAreaType areaType = ms.GetAreaTypeByPos(ControllerSystem.m_ClientGlobal.MainPlayer.GetPos());
            if (areaType == MapAreaType.Safe)
            {
                FindEntity<INPC>(new MonsterDistanceCondition(playerPos, range));
            }
            else
            {
                if (model == PLAYERPKMODEL.PKMODE_M_NORMAL)//和平 0默认和平
                {
                    FindEntity<INPC>(new MonsterDistanceCondition(playerPos, range));
                }
                else
                {
                    FindEntity<IPlayer>(new PlayerDistanceCondition(model, playerPos, range));
                }
            }


            //if (model == PLAYERPKMODEL.PKMODE_M_NORMAL)//和平 0默认和平
            //{
            //    FindEntity<INPC>(new MonsterDistanceCondition(playerPos, c));
            //}
            //else if (model == PLAYERPKMODEL.PKMODE_M_FREEDOM)//自由pk
            //{
            //    FindEntity<IPlayer>(new PlayerDistanceCondition(playerPos, c));
            //    FindEntity<INPC>(new MonsterDistanceCondition(playerPos, c));
            //}
            //else if (model == PLAYERPKMODEL.PKMODE_M_CAMP)//阵营战 不能攻击友方 demo 阶段只判断是不是一个阵营即可
            //{
            //    FindEntity<IPlayer>(new CountryDistanceCondition(playerPos, c, pkmodel));
            //    FindEntity<INPC>(new MonsterDistanceCondition(playerPos, c));
            //}
            m_lstEntity.Sort(Sort);
        }

        void FindEntity<T>(IFindCondition<T> condition) where T : IEntity
        {
            List<T> tempList = new List<T>();
            ControllerSystem.m_ClientGlobal.GetEntitySystem().FindEntityRange<T>(condition, ref tempList);
            for (int i = 0; i < tempList.Count; ++i)
            {
                m_lstEntity.Add(tempList[i]);
            }
            tempList.Clear();
        }

        int Sort(IEntity a, IEntity b)
        {
            float alength = Vector3.SqrMagnitude(playerPos - a.GetPos());
            float blength = Vector3.SqrMagnitude(playerPos - b.GetPos());
            if (alength > blength)
            {
                return 1;
            }
            else if (alength  < blength)
            {
                return -1;
            }
            return 0;
        }

    }
}
