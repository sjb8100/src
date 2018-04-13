using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

// 漫游Camera
public class CameraCtrl
{
    private Engine.ICamera m_Camera = null;

    public Engine.ICamera camera
    {
        set { m_Camera = value; }
    }
    // 按键处理
    public void OnMessage(Engine.MessageCode code, object param1 = null, object param2 = null, object param3 = null)
    {
        if( m_Camera == null )
        {
            return;
        }

        // 只在windows下有效
        if (code == Engine.MessageCode.MessageCode_KeyDown)
        {
            KeyCode kc = (KeyCode)param1;
            OnKeyDown(kc);
        }

        if (code == Engine.MessageCode.MessageCode_Key)
        {
            KeyCode kc = (KeyCode)param1;
            OnKeyPress(kc);
            Engine.Utility.Log.Trace("KeyCode:{0}", kc.ToString());
        }

        if( code == Engine.MessageCode.MessageCode_KeyUp )
        {
            KeyCode kc = (KeyCode)param1;
            OnKeyUp(kc);
        }
    }

    private void OnKeyDown(KeyCode kc)
    {
        Engine.Node camNode = m_Camera.GetNode();
        Vector3 pos = camNode.GetLocalPosition();
        
        switch (kc)
        {
            case KeyCode.UpArrow:
                {
                    pos.y += 1;
                    camNode.SetLocalPosition(pos);
                    break;
                }
            case KeyCode.DownArrow:
                {
                    pos.y -= 1;
                    camNode.SetLocalPosition(pos);
                    break;
                }
            case KeyCode.LeftArrow:
                {
                    pos.x -= 1;
                    camNode.SetLocalPosition(pos);
                    break;
                }
            case KeyCode.RightArrow:
                {
                    pos.x += 1;
                    camNode.SetLocalPosition(pos);
                    break;
                }
        }
    }

    private void OnKeyUp(KeyCode kc)
    {

    }

    private void OnKeyPress(KeyCode kc)
    {
        Engine.Node camNode = m_Camera.GetNode();
        Vector3 pos = camNode.GetLocalPosition();
        //KeyCode kc = (KeyCode)param1;
        switch (kc)
        {
            case KeyCode.UpArrow:
                {
                    pos.y += 1;
                    camNode.SetLocalPosition(pos);
                    break;
                }
            case KeyCode.DownArrow:
                {
                    pos.y -= 1;
                    camNode.SetLocalPosition(pos);
                    break;
                }
            case KeyCode.LeftArrow:
                {
                    pos.x -= 1;
                    camNode.SetLocalPosition(pos);
                    break;
                }
            case KeyCode.RightArrow:
                {
                    pos.x += 1;
                    camNode.SetLocalPosition(pos);
                    break;
                }
        }
    }
    // 更新
    public void Update(float dt)
    {

    }
}
