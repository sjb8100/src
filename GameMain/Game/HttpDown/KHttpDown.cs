using System;
using System.Collections.Generic;
using UnityEngine;

public class KHttpDown
{
    public bool SceneFileExists(uint mapid)
    {
        if (KDownloadInstance.Instance().IsSmallPackage() == false)
            return true;

        table.MapDataBase mapDB = GameTableManager.Instance.GetTableItem<table.MapDataBase>(mapid);
        if (mapDB == null)
        {
            Engine.Utility.Log.Error("MapSystem:找不到地图配置数据{0}", mapid);
            return false;
        }

        table.ResourceDataBase resDB = GameTableManager.Instance.GetTableItem<table.ResourceDataBase>(mapDB.dwResPath);
        if (resDB == null)
        {
            Engine.Utility.Log.Error("MapSystem:找不到地图资源路径配置{0}", mapDB.dwResPath);
            return false;
        }

        string strMapName = resDB.strPath.ToLower();

        if (Application.platform == RuntimePlatform.Android)
        {
            string strPath = System.IO.Path.Combine(Application.persistentDataPath + "/assets/", strMapName);
            bool exists = System.IO.File.Exists(strPath);
            if (exists == false)
            {
                strPath = System.IO.Path.Combine(Application.streamingAssetsPath, strMapName);
                exists = System.IO.File.Exists(strPath);
            }
            return exists;

        }
        else if (Application.platform == RuntimePlatform.IPhonePlayer)
        {
            string strPath = Application.persistentDataPath + "/" + strMapName;
            bool exists = System.IO.File.Exists(strPath);
            if (exists == false)
            {
                strPath = System.IO.Path.Combine(Application.streamingAssetsPath, strMapName);
                exists = System.IO.File.Exists(strPath);
            }
            return exists;
        }

        return true;
    }


    private static KHttpDown s_Instance = null;

    public static KHttpDown Instance()
    {
        if (s_Instance == null)
        {
            s_Instance = new KHttpDown();
        }
        return s_Instance;
    }
}


