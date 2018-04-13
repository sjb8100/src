using System;
using System.Collections.Generic;
using UnityEngine;

public class MapAreaRoot : MonoBehaviour
{
    public int width;
    public int height;
    public string map;
    public void Init(uint w,uint h,string mapFile)
    {
        width = (int)w;
        height = (int)h;
        map = mapFile;
        Engine.Utility.Log.Trace("MapAreaRoot init {0}", mapFile);
    }
    void Update()
    {
        MapSystem.MapAreaDisplay.Instance.Update();
    }
}
