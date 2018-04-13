using System;
using System.Collections.Generic;
using UnityEngine;
using System.Text;



public class ShakePhone : MonoBehaviour
 {
    const uint TIMER_ID = 2001;
    float old_y = 0;
    float new_y = 0;
    float dis_y = 0;
    Vector3 vec = Vector3.zero;

    void Update() 
    {
        new_y = Input.acceleration.y;
        dis_y = new_y - old_y;
        old_y = new_y;
    }
    void OnGUI() 
    {
        if (dis_y > 2)
        {
            Handheld.Vibrate();
            Engine.Utility.EventEngine.Instance().DispatchEvent((int)Client.GameEventID.SHAKEINGPHONEMSG, null);          
        }
    }
 }

