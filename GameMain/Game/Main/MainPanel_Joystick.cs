/************************************************************************************
 * Copyright (c) 2017  All Rights Reserved.
 * CLR版本： 4.0.30319.42000
 * 公司名称：ZQB
 * 命名空间：GameMain.Game.Main
 * 创建人：  wenjunhua.zqgame
 * 文件名：  MainPanel_Joystick
 * 版本号：  V1.0.0.0
 * 创建时间：7/10/2017 2:10:45 PM
 * 描述：
 ************************************************************************************/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Client;
using UnityEngine;
using Engine.Utility;

partial class MainPanel
{
    #region property
    private Vector2 m_LastJoystickBgScreenPos = Vector2.zero;
    private float m_fMaxDis = 60.0f; // 根据底图大小
    private Vector2 m_LastTouchPos = Vector2.zero;

    public delegate void OnJoystickChange(float fAngle);

    public OnJoystickChange OnJoystickChangeEvent = null;

    Vector3 bg_pos_offset;
    bool m_bpress = false;
    bool m_bJoystickStable = false;

    bool m_bSkillLongPress = false;
    //忽略拖动
    bool m_bIgnore = false;
    #endregion

    #region Init
    private void InitJoystick()
    {
        UIEventListener.Get(m_widget_MainCollider.gameObject).onPress = OnActive;
        UIEventListener.Get(m_widget_BottomCollider.gameObject).onPress = OnClickCollider;
        //UIEventListener.Get(m_widget_leftCollider.gameObject).onPress = OnClickCollider;

        UIEventListener.Get(m_widget_MainCollider.gameObject).onDrag = OnThumbDrag;
        UIEventListener.Get(m_widget_BottomCollider.gameObject).onDrag = OnThumbDrag;
        //UIEventListener.Get(m_widget_leftCollider.gameObject).onDrag = OnThumbDrag;

        m_bJoystickStable = ClientGlobal.Instance().gameOption.GetInt("Operate", "SettingFixedRocker", 1) == 1;

        SetMainCollider();
        Engine.Utility.EventEngine.Instance().AddEventListener((int)Client.GameEventID.ENTITYSYSTEM_LEAVEMAP, OnEvent);
        Engine.Utility.EventEngine.Instance().AddEventListener((int)Client.GameEventID.SKLL_LONGPRESS, OnEvent);
    }
    #endregion

    #region Op

    void SetMainCollider()
    {
        Vector3 pos = m_widget_MainCollider.transform.localPosition;
        if ( m_bJoystickStable )
        {
            pos.x = 4;
            m_widget_MainCollider.transform.localPosition = pos;
            m_widget_MainCollider.width = 400;
            //m_widget_MainCollider.depth = 12;
        }
        else
        {
            pos.x = 59;
            m_widget_MainCollider.transform.localPosition = pos;
            m_widget_MainCollider.width = 315;
            //m_widget_MainCollider.depth = 0;
        }
    }
    private void SetThumbDrag()
    {
        float fDis = Vector2.Distance( m_LastJoystickBgScreenPos , m_LastTouchPos );
        if ( fDis > m_fMaxDis )
        {
            Vector2 dir = ( new Vector2( m_LastTouchPos.x , m_LastTouchPos.y ) - m_LastJoystickBgScreenPos ).normalized;

            Vector2 pos = dir * ( fDis - m_fMaxDis );

            m_LastJoystickBgScreenPos += pos;

            m_sprite_joystickBg.transform.position = UICamera.currentCamera.ScreenToWorldPoint( new Vector3( m_LastJoystickBgScreenPos.x , m_LastJoystickBgScreenPos.y , 0 ) );
        }

        OnThumbDrag( null , Vector2.zero );
    }

    void OnClickCollider(GameObject go , bool press)
    {
        //if (m_bSkillLongPress)
        //{
        //    return;
        //}
        if ( m_bJoystickStable )
        {
            if ( !press )
            {
                ResetJoystick();
            }
            return;
        }

        m_bpress = press;
        if ( !press )
        {
            ResetJoystick();
        }
        else
        {
            //按下摇杆的时候清除技能执行的命令
            Controller.CmdManager.Instance().Clear();
            StopMove();
            m_LastTouchPos = UICamera.currentTouch.pos;
            m_sprite_joystickThumb.transform.position = UICamera.currentCamera.ScreenToWorldPoint( new Vector3( m_LastTouchPos.x , m_LastTouchPos.y , 0 ) );
            Vector3 pp = UICamera.currentCamera.WorldToScreenPoint( m_sprite_joystickBg.transform.position );
            m_LastJoystickBgScreenPos = new Vector2( pp.x , pp.y );

            SetThumbDrag();
        }
    }

    void StopMove()
    {
        //移动摇杆打断自动寻路，角色会滑步
        Client.IPlayer player = Client.ClientGlobal.Instance().MainPlayer;
        if ( player == null )
        {
            return;
        }
        bool moving = (bool)player.SendMessage( Client.EntityMessage.EntityCommand_IsMove , null );
        if ( moving )
        {
            player.SendMessage( Client.EntityMessage.EntityCommand_StopMove , player.GetPos() );
        }

        Engine.Utility.EventEngine.Instance().DispatchEvent((int) (int)Client.GameEventID.JOYSTICK_PRESS , null );
    }

    void OnActive(GameObject obj , bool press)
    {
        m_bpress = press;
      //  if ( m_bSkillLongPress )
        //{
        //    if ( !press )
        //    {
        //        ResetJoystick();
        //    }
        //    return;
        //}

        if ( !press )
        {
            ResetJoystick();

        }
        else
        {
            //按下摇杆的时候清除技能执行的命令
            Controller.CmdManager.Instance().Clear();
            StopMove();
            m_widget_joystick.alpha = 1.0f;
            m_LastTouchPos = UICamera.currentTouch.pos;
            if ( m_bJoystickStable )
            {
                m_sprite_joystickThumb.transform.position = UICamera.currentCamera.ScreenToWorldPoint( new Vector3( m_LastTouchPos.x , m_LastTouchPos.y , 0 ) );
                m_sprite_joystickBg.transform.position = m_sprite_joystickThumb.transform.position;
                Vector3 pp = UICamera.currentCamera.WorldToScreenPoint( m_sprite_joystickBg.transform.position );
                m_LastJoystickBgScreenPos = new Vector2( pp.x , pp.y );
                //SetThumbDrag();
            }
            else
            {
                m_widget_joystick.transform.position = UICamera.currentCamera.ScreenToWorldPoint( new Vector3( m_LastTouchPos.x , m_LastTouchPos.y , 0 ) );
                m_LastJoystickBgScreenPos = new Vector2( m_LastTouchPos.x , m_LastTouchPos.y );
            }
        }


    }

    private void ShowJoystick()
    {
        ResetJoystick();
    }

    private void ResetJoystick()
    {
        m_widget_joystick.alpha = 0.5f;
        m_widget_joystick.gameObject.transform.localPosition = m_trans_joystickPos.localPosition;
        m_sprite_joystickThumb.transform.localPosition = Vector3.zero;
        m_sprite_joystickBg.transform.localPosition = Vector3.zero;
        m_sprite_joystickThumb.GetComponent<BoxCollider>().enabled = false;

        Client.IControllerSystem controllerSys = Client.ClientGlobal.Instance().GetControllerSystem();
        if ( controllerSys != null )
        {
            Client.IController ctrl = controllerSys.GetActiveCtrl();
            if ( ctrl != null )
            {
                ctrl.OnMessage( Engine.MessageCode.MessageCode_JoystickEnd );
            }
        }
        Engine.Utility.EventEngine.Instance().DispatchEvent((int) (int)Client.GameEventID.JOYSTICK_UNPRESS , null );
        if(MainPlayerHelper.GetMainPlayer() != null)
        {
            ISkillPart part = MainPlayerHelper.GetMainPlayer().GetPart(EntityPart.Skill) as ISkillPart;
            if (part != null)
            {
                int st = part.GetCurSkillState();
                if (st == (int)SkillState.None || st == (int)SkillState.Prepare)
                {
                    stForbiddenJoystick info = new stForbiddenJoystick();
                    info.playerID = MainPlayerHelper.GetPlayerUID();
                    info.bFobidden = false;
                    EventEngine.Instance().DispatchEvent((int)GameEventID.SKILL_FORBIDDENJOYSTICK, info);
                }
            }
        }
   
    }

    private void OnThumbDrag(GameObject obj , Vector2 delta)
    {
        if ( m_bIgnore )
        {
            return;
        }

      
        if (MainPlayerHelper.GetMainPlayer().IsDead())
        {
            return;
        }

        Vector2 touchPos = UICamera.currentTouch.pos;

        float fDis = Vector2.Distance( m_LastJoystickBgScreenPos , touchPos );
        if ( m_bJoystickStable )
        {
            Vector2 touchDelta2 = touchPos - m_LastJoystickBgScreenPos;
            m_sprite_joystickThumb.transform.position = UICamera.currentCamera.ScreenToWorldPoint( new Vector3( touchPos.x , touchPos.y , 0 ) );
            if ( fDis < m_fMaxDis )
            {
                Vector2 pos = m_LastJoystickBgScreenPos + touchDelta2;
                Vector3 pp = UICamera.currentCamera.ScreenToWorldPoint( new Vector3( pos.x , pos.y , 0 ) );
                m_sprite_joystickThumb.transform.position = new Vector3( pp.x , pp.y , 0 );
            }
            else
            {
                Vector2 dt = touchDelta2.normalized;
                Vector2 pos = m_LastJoystickBgScreenPos + dt * m_fMaxDis;
                Vector3 pp = UICamera.currentCamera.ScreenToWorldPoint( new Vector3( pos.x , pos.y , 0 ) );
                m_sprite_joystickThumb.transform.position = new Vector3( pp.x , pp.y , 0 );
            }
        }
        else
        {
            m_sprite_joystickThumb.transform.position = UICamera.currentCamera.ScreenToWorldPoint( new Vector3( touchPos.x , touchPos.y , 0 ) );
            if ( fDis > m_fMaxDis )
            {
                Vector2 dir = ( new Vector2( touchPos.x , touchPos.y ) - m_LastJoystickBgScreenPos ).normalized;

                Vector2 pos = dir * ( fDis - m_fMaxDis );

                m_LastJoystickBgScreenPos += pos;

                m_sprite_joystickBg.transform.position = UICamera.currentCamera.ScreenToWorldPoint( new Vector3( m_LastJoystickBgScreenPos.x , m_LastJoystickBgScreenPos.y , 0 ) );
            }
        }

        m_LastTouchPos = touchPos;
        //if (m_bSkillLongPress)
        //{
        //    return;
        //}
        
        if ( fDis < 10 )
        {
            return;
        }
        Vector2 touchDelta = touchPos - m_LastJoystickBgScreenPos;

        // 根据 touchDelta 计算角度 垂直向上
        float fAngle = Vector2.Angle( Vector2.up , touchDelta );
        if ( touchDelta.x < 0 )
        {
            fAngle = -fAngle;
        }
        fAngle += CameraFollow.Instance.YAngle;
        Client.IControllerSystem controllerSys = Client.ClientGlobal.Instance().GetControllerSystem();
        if ( controllerSys != null )
        {
            Client.IController ctrl = controllerSys.GetActiveCtrl();
            if ( ctrl != null )
            {
                ctrl.OnMessage( Engine.MessageCode.MessageCode_JoystickChanging , (object)fAngle );
                if(m_bSkillLongPress)
                {
                    //点击摇杆 解除长按
                    Controller.CmdManager.Instance().Clear();
                    stSkillLongPress longPress = new stSkillLongPress();
                    longPress.bLongPress = false;
                    EventEngine.Instance().DispatchEvent((int)GameEventID.SKLL_LONGPRESS, longPress);
                }
        
            }
        }
    }

    #endregion
}