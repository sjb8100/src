//*************************************************************************
//	创建日期:	2017-5-9 14:10
//	文件名称:	LoginSceneGyr.cs
//  创 建 人:   	Chengxue.Zhao
//	版权所有:	中青宝
//	说    明:	登录场景 层级滑动控制
//*************************************************************************
using System;
using System.Collections.Generic;
using UnityEngine;

public class LoginSceneGyr : MonoBehaviour
{
    Vector2 farPosXRange = new Vector2(-0.45f,0.5f);
    Vector2 farPosYRange = new Vector2(-0.3f, 0.28f);

    Vector2 midPosXRange = new Vector2(-0.25f, 0.32f);
    Vector2 midPosYRange = new Vector2(-0.25f, 0.15f);

    Vector2 nearPosXRange = new Vector2(-0.21f, 0.33f);
    Vector2 nearPosYRange = new Vector2(-0.3f, 0.24f);

    public Transform nearTrans;
    public Transform farTrans;
    public Transform midTrans;
    Vector3 midpos;
    Vector3 farpos;
    Vector3 nearpos;

    static LoginSceneGyr _instance = null;

    public static LoginSceneGyr Instance
    {
        get { return _instance; }
    }

    public float farX = 0.5f;
    public float farY= 0.5f;
    public float midX= 1f; 
    public float midY= 1f; 
 
    void Awake() {
        _instance = this;
    }
    void Start()
    {
        nearTrans = transform.Find("Near");
        midTrans = transform.Find("Mid");
        farTrans = transform.Find("Far");
        if (midTrans != null)
        {
            midpos = midTrans.localPosition;
        }

        if (farTrans != null)
        {
            farpos = farTrans.localPosition;
        }

        if (nearTrans != null)
        {
            nearpos = nearTrans.localPosition;
        }
    }
    public Vector3 acceletation = Vector3.zero;
    public bool bmoveRight = true;
    void Update()
    {
        if (midTrans == null || farTrans == null)
        {
            return;
        }
        
        //if (Input.acceleration.x == 0 && Input.acceleration.y == 0)
        {
            if (bmoveRight)
            {
                acceletation.x += Time.deltaTime * 0.1f;
            }
            else
            {
                acceletation.x -= Time.deltaTime * 0.1f;
            }

            if (acceletation.x > 1f)
            {
                bmoveRight = false;
                acceletation.x = 1f;
            }
            else if (acceletation.x < -1f)
            {
                bmoveRight = true;
                acceletation.x = -1f;
            }
        }
        //else
        //{
        //    acceletation = Input.acceleration;
        //}
        midTrans.localPosition = GetMidPos(acceletation);

        farTrans.localPosition = GetFarPos(acceletation);

        nearTrans.localPosition = GetNearPos(acceletation);
    }

    string strfarSpeedX = "0.15";
    public float farSpeedX = 0.15f;
    string strfarSpeedY = "0.15";
    public float farSpeedY = 0.15f;

    string strmidSmooth = "15";
    public float smooth = 15f;

    string strmidSpeedX = "0.35";
    public float midSpeedX = 0.35f;
    string strmidSpeedY = "0.35";
    public float midSpeedY = 0.35f;

    string strnearSpeedX = "0.33";
    public float nearSpeedX = 0.33f;
    string strnearSpeedY = "0.3";
    public float nearSpeedY = 0.3f;

/*
    void OnGUI()
    {
        if (!Application.isEditor)
        {
            GUILayout.Label("farSpeedX", GUILayout.Height(50));
            strfarSpeedX = GUI.TextField(new Rect(100, 0, 100, 50), strfarSpeedX);
            if (!string.IsNullOrEmpty(strfarSpeedX))
            {
                farSpeedX = float.Parse(strfarSpeedX);
            }

            GUILayout.Label("farSpeedY", GUILayout.Height(50));
            strfarSpeedY = GUI.TextField(new Rect(100, 50, 100, 50), strfarSpeedY);
            if (!string.IsNullOrEmpty(strfarSpeedY))
            {
                farSpeedY = float.Parse(strfarSpeedY);
            }


            GUILayout.Label("midSpeedX", GUILayout.Height(50));
            strmidSpeedX = GUI.TextField(new Rect(100, 100, 100, 50), strmidSpeedX);
            if (!string.IsNullOrEmpty(strmidSpeedX))
            {
                midSpeedX = float.Parse(strmidSpeedX);
            }

            GUILayout.Label("midSpeedY", GUILayout.Height(50));
            strmidSpeedY = GUI.TextField(new Rect(100, 150, 100, 50), strmidSpeedY);
            if (!string.IsNullOrEmpty(strmidSpeedY))
            {
                midSpeedY = float.Parse(strmidSpeedY);
            }


            GUILayout.Label("nearSpeedX", GUILayout.Height(50));
            strnearSpeedX = GUI.TextField(new Rect(100, 200, 100, 50), strnearSpeedX);
            if (!string.IsNullOrEmpty(strnearSpeedX))
            {
                nearSpeedX = float.Parse(strnearSpeedX);
            }

            GUILayout.Label("nearSpeedY", GUILayout.Height(50));
            strnearSpeedY = GUI.TextField(new Rect(100, 250, 100, 50), strnearSpeedY);
            if (!string.IsNullOrEmpty(strnearSpeedY))
            {
                nearSpeedY = float.Parse(strnearSpeedY);
            }

            GUILayout.Label("smooth", GUILayout.Height(50));
            strmidSmooth = GUI.TextField(new Rect(100, 300, 100, 50), strmidSmooth);
            if (!string.IsNullOrEmpty(strmidSmooth))
            {
                smooth = float.Parse(strmidSmooth);
            }
        }
    }

    */
    Vector3 GetMidPos(Vector3 acceletation)
    {

        Vector3 pos = new Vector3(midpos.x + acceletation.x * midSpeedX, midpos.y - acceletation.y * midSpeedY, midpos.z);

        if (pos.x < midPosXRange.x)
        {
            pos.x = midPosXRange.x;
        }
        else if (pos.x > midPosXRange.y)
        {
            pos.x = midPosXRange.y;
        }

        if (pos.y < midPosYRange.x)
        {
            pos.y = midPosYRange.x;
        }
        else if (pos.y > midPosYRange.y)
        {
            pos.y = midPosYRange.y;
        }

        return Vector3.Lerp(midTrans.localPosition, pos, Time.deltaTime * smooth);
    }

    Vector3 GetFarPos(Vector3 acceletation)
    {
        Vector3 pos = new Vector3(farpos.x + acceletation.x * farSpeedX, farpos.y - acceletation.y * farSpeedY, farpos.z);

        if (pos.x < farPosXRange.x)
        {
            pos.x = farPosXRange.x;
        }
        else if (pos.x > farPosXRange.y)
        {
            pos.x = farPosXRange.y;
        }

        if (pos.y < farPosYRange.x)
        {
            pos.y = farPosYRange.x;
        }
        else if (pos.y > farPosYRange.y)
        {
            pos.y = farPosYRange.y;
        }

        return Vector3.Lerp(farTrans.localPosition, pos, Time.deltaTime * smooth);
    }

    Vector3 GetNearPos(Vector3 acceletation)
    {
        Vector3 pos = new Vector3(nearpos.x + acceletation.x * nearSpeedX, nearpos.y - acceletation.y * nearSpeedY, nearpos.z);

        if (pos.x < nearPosXRange.x)
        {
            pos.x = nearPosXRange.x;
        }
        else if (pos.x > nearPosXRange.y)
        {
            pos.x = nearPosXRange.y;
        }

        if (pos.y < nearPosYRange.x)
        {
            pos.y = nearPosYRange.x;
        }
        else if (pos.y > nearPosYRange.y)
        {
            pos.y = nearPosYRange.y;
        }

        return Vector3.Lerp(nearTrans.localPosition, pos, Time.deltaTime * smooth);
    }
}
