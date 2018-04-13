using UnityEngine;
using System;
using System.Collections.Generic;

class ThirdCameraController :MonoBehaviour
{
    static ThirdCameraController ms_instance;
    public static ThirdCameraController Instance
    {
        get{return ms_instance;}
    }

    void Awake()
    {
        ms_instance = this;
    }

    bool m_moving = false;
    public Transform m_targetTransfrom = null;
    public Transform TargetTransform
    {
        get { return m_targetTransfrom; }
        set
        {
            m_moving = (m_targetTransfrom != null);
            m_targetTransfrom = value;

            if (m_targetTransfrom == null)
            {
                m_moving = false;
            }
        }
    }

    public float m_dist = 15.0f;
    public float m_pitchAngle = 10.0f;
    public float m_yawAngle = -90.0f;
    public float m_height = 1.0f;

    Vector3 m_last_targetPos;

    void Update()
    {
        if (m_targetTransfrom == null)
            return;

        Vector3 targetPos = m_targetTransfrom.position;
        targetPos.y += m_height;

        //if (targetPos == m_last_targetPos)
        //    return;

        Vector3 cameraPos = targetPos;

        float r = m_dist * Mathf.Cos(m_pitchAngle * Mathf.Deg2Rad);

        cameraPos.z += Mathf.Sin((m_yawAngle) * Mathf.Deg2Rad) * r;
        cameraPos.x += Mathf.Cos((m_yawAngle) * Mathf.Deg2Rad) * r;

        cameraPos.y += m_dist * Mathf.Sin(m_pitchAngle * Mathf.Deg2Rad);

        transform.position = cameraPos;
        transform.LookAt(targetPos);

        m_last_targetPos = targetPos;
    }
}

