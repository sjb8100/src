using Client;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using DG.Tweening;

// 镜头跟随
class CameraFollow : Singleton<CameraFollow>, Engine.ICameraCtrl
{
    public static float Cam_Near = 3.0f;
    public static float Cam_Far = 12.0f;
    public static float Cam_XRotMax = 38.0f;
    public static float Cam_XRotMin = 25.0f;

    private float m_fProgress = 1.0f;

    private Engine.ICamera m_Camera = null;
    private Client.IEntity m_Target = null;
    private Vector3 m_lastPos = Vector3.zero;

    private float m_fYOffset = 1.15f;

    private Vector3 m_Offset = new Vector3(1.0f,0.8f,-1.0f); // 镜头偏离目标的方向
    private float m_fAngleX = 0.0f;         // 镜头相对目标X轴旋转角度 
    private float m_fAngleY = 0.0f;         // 镜头相对目标Y轴旋转角度 
    private float m_fDis = 15.2f;           // 镜头离目标的距离

    private float m_fNearDis = 0.003f;      // 距离小于此值时，直接放回跟随点
    private float m_fTweenTime = 0.8f;      // 缓动时间

    private bool m_bTween = true;          // 是否处于缓动状态
    private Vector3 m_TweenSpeed = Vector3.zero;   // 缓动速度

    // Cameara Rotate
    private bool m_bDrag = false;
    private Vector2 m_vStartPos = Vector2.zero;
    private Vector3 m_vStartAngle = Vector3.zero;
    private Vector3 m_vStartCamPos = Vector3.zero;

    // 镜头震动
    bool m_bShake = true;

    /// <summary>
    /// 是否开启震屏
    /// </summary>
    public bool  BEnableShake
    {
        set
        {
            m_bShake = value;
        }
    }
    public Engine.ICamera camera
    {
        set { m_Camera = value; }
    }

    public Client.IEntity target
    {
        set { m_Target = value; }
    }

    public float YAngle
    {
        get { return m_fAngleY; }
    }

    public float Progress
    {
        get { return m_fProgress; }
    }

    private Transform m_AudioListenerTrans = null;

    public CameraFollow()
    {
        //m_Offset.Normalize(); // 方向做归一化
        //m_Offset *= m_fDis;
    }

    public void SetCameraOffset(float fAngleX, float fAngleY, float fDis)
    {
        m_fAngleX = fAngleX;
        m_fAngleY = fAngleY;
        m_fDis = fDis;

        // 计算镜头偏移
        ClacCameraOffset();

        if(m_Target==null)
        {
            Engine.Utility.Log.Error("Camera Target is null!");
            return;
        }

        if (m_Camera == null)
        {
            Engine.Utility.Log.Error("Camera is null!");
            return;
        }

        Vector3 pos = m_Target.GetPos();
        m_lastPos = pos;

        pos.y += m_fYOffset; // 将目标上移
        Vector3 camNewPos = pos + m_Offset;
        m_Camera.LookAt(camNewPos, pos, Vector3.up);

        if (m_AudioListenerTrans != null && m_Target != null)
        {
            m_AudioListenerTrans.position = m_Target.GetPos();// GetNode().GetWorldPosition();
        }
    }

    public float GetCamDis() { return m_fDis; }

    public void SetCameraFarDis(float fFarDis)
    {
        Cam_Far = fFarDis;
        SetCameraOffset(m_fProgress);
    }

    public void SetCameraOffset(float fProgress)
    {
        m_fAngleX = Cam_XRotMin + (Cam_XRotMax - Cam_XRotMin) * fProgress;
        m_fDis = Cam_Near + (Cam_Far - Cam_Near) * fProgress;

        m_fProgress = fProgress;

        // 计算镜头偏移
        ClacCameraOffset();

        if (m_Target == null)
        {
            return;
        }

        if (m_Camera == null)
        {
            return;
        }

        Vector3 pos = m_Target.GetPos();
        m_lastPos = pos;

        pos.y += m_fYOffset; // 将目标上移
        Vector3 camNewPos = pos + m_Offset;
        m_Camera.LookAt(camNewPos, pos, Vector3.up);

        if (m_AudioListenerTrans != null && m_Target != null)
        {
            m_AudioListenerTrans.position = m_Target.GetNode().GetWorldPosition();
        }
    }

    public void SetCameraDis(float fDis)
    {
        m_fDis = fDis;

        // 计算镜头偏移
        ClacCameraOffset();

        Vector3 pos = Vector3.zero;
        if (m_Target != null)
        {
            pos = m_Target.GetPos();
        }

        m_lastPos = pos;

        pos.y += m_fYOffset; // 将目标上移
        Vector3 camNewPos = pos + m_Offset;
        m_Camera.LookAt(camNewPos, pos, Vector3.up);
    }

    public void OnMessage(Engine.MessageCode code, object param1 = null, object param2 = null, object param3 = null)
    {
        if (m_AudioListenerTrans != null && m_Target != null)
        {

            m_AudioListenerTrans.position = m_Target.GetPos();
        }

       // Engine.Utility.Log.Error("CameraFollow MessageCode{0}", code.ToString());

        // 处理多点触摸改变镜头距离
        switch(code)
        {
            case Engine.MessageCode.MessageCode_MultipleMove:   // 多点触摸滑动
                {
                    float fDistance = (float)param1;
                    m_fProgress += -fDistance*0.1f/100f;
                    if(m_fProgress<0.0f)
                    {
                        m_fProgress = 0.0f;
                    }
                    if(m_fProgress>1.0f)
                    {
                        m_fProgress = 1.0f;
                    }
                    SetCameraOffset(m_fProgress);
                    break;
                }
            default:
                {
                    break;
                }
        }
    }

    // MoveUpdate
    // 与场景做碰撞检测，将碰到的TerrainObj置为半透 场景本身遮挡则不处理
    public void Update(float dt)
    {
        if(m_Target==null || m_Camera==null)
        {
            return;
        }

        Vector3 pos = m_Target.GetPos();
        
       // if (!pos.Equals(m_lastPos))
        //不用euals 每帧28b的gc 
        if(pos != m_lastPos)
        {
            //if (!m_bTween)
            //{
            //    // 启用跟随
            //    m_bTween = true;
            //}
            m_lastPos = pos;

            pos.y += m_fYOffset; // 将目标上移
            Vector3 camNewPos = pos + m_Offset;
            m_Camera.LookAt(camNewPos, pos, Vector3.up);
        }



        //if (m_bTween)
        //{
        //    pos.y += 0.35f; // 将目标上移
        //    Vector3 camNewPos = pos + m_Offset;

        //    Vector3 camPos = m_Camera.GetNode().GetWorldPosition();
        //    float fDis = Vector3.Distance(camPos, camNewPos);
        //    // 计算缓动
        //    Vector3 newPos = CamTween(ref camPos, ref camNewPos, fDis, dt);
        //    if (Vector3.Distance(camNewPos, newPos) < m_fNearDis)
        //    {
        //        camPos = camNewPos;
        //        m_bTween = false;
        //    }
        //    else
        //    {
        //        camPos = newPos;
        //    }

        //    m_Camera.LookAt(camPos, pos, Vector3.up);
        //}
    }

    //-------------------------------------------------------------------------------------------------------
    /**
    @brief 开始振动
    @param fAmplitude 振幅
    @param fFrequency 频率
    @param fCycle 周期
    @param fTime 持续时间
    */
    public void StartShake(float fAmplitude, float fFrequency, float fCycle, float fTime)
    {
        if(m_bShake)
        {
            if(m_Camera == null)
            {
                return;
            }
            Camera cam = m_Camera.GetNode().GetTransForm().GetComponent<Camera>();
            if(cam != null)
            {
                cam.DOShakePosition(fTime, fAmplitude, (int)fFrequency, 40);
            }
        }
    }

    // 停止振动
    public void StopShake()
    {
        if (m_Camera == null)
        {
            return;
        }
        Camera cam = m_Camera.GetNode().GetTransForm().GetComponent<Camera>();
        if (cam != null)
        {
            cam.DOKill();
        }
        
    }

    ////////////////////////////////////////////////////////////////////////////////////////////////////////////

    private void ClacCameraOffset()
    {
        Quaternion rot = new Quaternion();
        rot.eulerAngles = new Vector3(m_fAngleX,m_fAngleY,0.0f);
        Matrix4x4 mat = new Matrix4x4();
        mat.SetTRS(Vector3.zero, rot, Vector3.one);
        m_Offset = -mat.GetColumn(2)*m_fDis;
    }

    /// <param name="pos"></param>
    /// <param name="newPos"></param>
    /// <param name="fDis"></param>
    /// <param name="dt"></param>
    /// <returns></returns>
    private Vector3 CamTween(ref Vector3 pos, ref Vector3 newPos, float fDis, float dt)
    {
        Vector3 dir = newPos - pos;
        dir.Normalize();
        Vector3 speed = dir * (fDis / m_fTweenTime);
        return pos + speed * dt;
    }
    //-----------------------------------------------------------------------------------------------

    public AudioListener CreateAudioListener()
    {
        AudioListener listener = null;
        if (m_AudioListenerTrans != null)
        {
            return m_AudioListenerTrans.GetComponent<AudioListener>();
        }

        if (m_Camera != null)
        {
            GameObject go = new GameObject("AudioListener");
            m_AudioListenerTrans = go.transform;
            m_AudioListenerTrans.parent = m_Camera.GetNode().GetTransForm();
            listener = go.AddComponent<AudioListener>();
        }

        return listener;
    }
}