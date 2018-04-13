using UnityEngine;
using System.Collections;

//输入操作相关
public class InputUtil
{
    static bool m_downing1 = false;
    static bool m_downing0 = false;

    static bool m_moving = false;
    static Vector3 m_mousePos;
    static float m_lastDownTime = 0.0f;
    static bool m_double = false;

    static Vector3 m_downPos = Vector3.zero;
    static float m_slideDist = 0.0f;
    static Vector3 m_slideDir = Vector3.zero;
    static float m_slideSpeed = 0.0f;
    static float m_downTimeLen = 0.0f;

    static Vector3 m_deltaPos = Vector3.zero;

    static public Vector3 DeltaPos
    {
        get { return m_deltaPos; }
    }

    static public bool IsDouble
    {
        get { return m_double; }
    }

    static public bool IsMoving
    {
        get { return m_moving; }
    }

    static public bool IsTouchDowning
    {
        get { return m_downing0; }
    }

    static public float SlideDist
    {
        get { return m_slideDist; }
    }

    static public Vector3 SlideDir
    {
        get { return m_slideDir; }
    }

    static public float SlideSpeed
    {
        get { return m_slideSpeed; }
    }

    static public float DownTimeLen
    {
        get { return m_downTimeLen; }
    }


   //static public float GetDowningTime()
    //{
    //    return Time.time - m_lastDownTime;
    //}

    //两指都按下去了
    static public bool IsTowTouchDown
    {
        get {
            if (Application.isEditor)
                return m_downing1 && m_downing0;
            else
                return Input.touchCount >= 2;
        }
    }

    //static int m_touch_num = 0;

    static public void Update()
    {
        bool lastUp = false;

        if (HasTouchDown(0))
        {
            lastUp = true;

            m_deltaPos = Vector3.zero;

            m_downing0 = true;
            m_moving = false;

            m_slideDist = 0.0f;

            m_downPos = GeTouchPos();

            if (Time.time - m_lastDownTime < 0.5f)
            {
                m_double = true;
            }
            else
            {
                m_double = false;
            }

            m_lastDownTime = Time.time;
        }

        if (HasTouchUp(0))
        {
            m_downing0 = false;
            m_moving = false;
            m_deltaPos = Vector3.zero;
        }

        if (Application.isEditor)
        {
            if (HasTouchDown(1))
            {
                //Log.Error("btn 2 down");
                m_downing1 = true;
            }

            if (HasTouchUp(1))
            {
                m_downing1 = false;
            }
        }

        if (m_downing0)
        {
            Vector3 pos = GeTouchPos();
            if (lastUp == false && pos.Equals(m_mousePos) == false)
            {
                m_deltaPos = pos - m_mousePos;
                m_moving = true;
            }
            else
            {
                m_deltaPos = Vector3.zero;
                m_moving = false;
            }

            m_mousePos = pos;
        }
    }

    static public float get_slide_dist()
    {
        return (m_mousePos - m_downPos).magnitude;
    }

    static public float get_slide_ratio()
    {
       return get_slide_dist() / Screen.width;
    }

    static public bool IsClicked(float offset)
    {
        if(HasTouchUp())
        {
            //if (Time.time - m_lastDownTime < 0.5f)
            {
                //Log.Info("slide dist: " + m_slideDist);

                if (m_slideDist < offset)
                {
                    return true;
                }

            }
        }

        return false;
    }

    public static float ms_last_touch_down_ui_time = 0.0f;

    static public bool HasTouchDown(int index=0, bool though_ui = false)
    {
        if (though_ui == false && UICamera.lastHit.collider != null)
        {
            ms_last_touch_down_ui_time = Time.time;
            return false;
        }

        if (Application.isEditor)
        {
            return Input.GetMouseButtonDown(index);
        }
        else
        {
            if (Input.touchCount > index)
            {
                //Log.Info("Input.GetTouch(0).phase: " + Input.GetTouch(0).phase);
                return Input.GetTouch(index).phase == TouchPhase.Began;
            }
            else
            {
                //Log.Info("touch false");
                return false;
            }
        }
    }

    static public bool HasTouchUp(int index=0)
    {
        bool bRet = false;

        if (Application.isEditor)
        {
            bRet = Input.GetMouseButtonUp(index);
        }
        else
        {
            if (Input.touchCount > index)
            {
                bRet = Input.GetTouch(index).phase == TouchPhase.Ended;
            }

        }

        if (bRet)
        {
            m_slideDir = GeTouchPos() - m_downPos;
            m_slideDist = m_slideDir.magnitude;
            m_slideDir.z = m_slideDir.y;
            m_slideDir.y = 0;
            m_slideDir.Normalize();

            m_downTimeLen = Time.time - m_lastDownTime;

            m_slideSpeed = m_slideDist / m_downTimeLen;


            //Log.Info("down pos: " + m_downPos);
            //Log.Info("up pos: " + GeTouchPos());

            //Log.Info("slide dist: " + m_slideDist);
            //Log.Info("slide speed: " + m_slideSpeed);
        }

        return bRet;
    }

    static public Vector3 GeTouchPos()
    {
        if (Application.isEditor)
        {
            return Input.mousePosition;
        }
        else
        {
            if (Input.touchCount > 0)
            {
                return Input.GetTouch(0).position;
            }
            else
            {
                return Vector3.zero;
            }
        }
    }

    public static bool CheckWalkablePos(ref Vector3 pos)
    {
        //Ray r = Util.UICamera.ScreenPointToRay(InputUtil.GeTouchPos());
        //if (Physics.Raycast(r, 10000.0f, Util.UICamera.cullingMask))
        if (UICamera.lastHit.collider != null)
        {
            //Log.Info("hoveredObject: " + UICamera.lastHit.collider.name);
            return false;
        }

        if (Application.isEditor == false)
        {
            if (Input.touchCount == 0)
                return false;
        }

        RaycastHit hit;

        Ray r = Camera.main.ScreenPointToRay(InputUtil.GeTouchPos());

        if (Physics.Raycast(r, out hit, 50.0f, 1 <<LayerUtil.walkable))
        {
            //Debug.Log("hit:" + hit.collider.gameObject.name);
            pos = hit.point;
            return true;
        }

        return false;
    }
}
