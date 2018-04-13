using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Client;
using UnityEngine;
using UnityEngine.Profiling;

class MapIconPoolManager
{
    private static MapIconPoolManager instance = null;
    Dictionary<string, List<GameObject>> iconPoolDic = new Dictionary<string, List<GameObject>>();
    public static MapIconPoolManager Instance
    {
        get
        {
            if (instance == null)
                instance = new MapIconPoolManager();
            return instance;
        }
    }

    StringBuilder strIconContainer = new StringBuilder();
    /// <summary>
    /// 获取mapicon
    /// </summary>
    /// <param name="type">Icontype</param>
    /// <param name="entity">要显示icon的实体</param>
    /// <param name="contaninerParentTransrom">IconContainer的父节点</param>
    /// <returns></returns>
    public GameObject GetIcon(IconType type, IEntity entity, Transform contaninerParentTransrom)
    {

        List<GameObject> gameList;
        string key = GetStringByType(type);
        if (iconPoolDic.TryGetValue(key, out gameList))
        {
            gameList = iconPoolDic[key];
            if (gameList.Count > 0)
            {
                GameObject go = gameList[0];
                gameList.RemoveAt(0);
                if (go == null)
                {

                    strIconContainer.Remove(0, strIconContainer.Length);
                    strIconContainer.Append("IconContainer/");
                    strIconContainer.Append(type.ToString());
                    Transform iconTrans = contaninerParentTransrom.Find(strIconContainer.ToString());
                    go = GameObject.Instantiate(iconTrans.gameObject);

                }
                go.name = GetIconName(type, entity);
                go.SetActive(true);
                return go;

            }
            else
            {
                GameObject go = GetIconGameObject(type, entity, contaninerParentTransrom);
                return go;
            }
        }
        else
        {
            gameList = new List<GameObject>();
            iconPoolDic.Add(key, gameList);
            return GetIconGameObject(type, entity, contaninerParentTransrom);
        }
    }
    /// <summary>
    /// 预设池最大数量
    /// </summary>
    const int MAXPOOLCOUNT = 40;
    string[] iconNameStrArray;
    public void ReturnIcon(GameObject go)
    {
        Profiler.BeginSample("ReturnIcon");


        go.SetActive(false);
        List<GameObject> gameList;
        if (iconPoolDic.TryGetValue(go.name, out gameList))
        {
            if (gameList.Count > MAXPOOLCOUNT)
            {
                GameObject.DestroyImmediate(go);
                return;
            }
            gameList.Add(go);
        }
        else
        {
            gameList = new List<GameObject>();
            iconPoolDic.Add(go.name, gameList);
            gameList.Add(go);
        }
        Profiler.EndSample();
    }
    GameObject GetIconGameObject(IconType type, IEntity entity, Transform contaninerParentTransrom)
    {
        string name = string.Format("IconContainer/{0}", type);
        Transform iconTrans = contaninerParentTransrom.Find(name);
        if (iconTrans == null)
        {
            Engine.Utility.Log.LogGroup("ZDY", "get icontrans is null typs is " + type);
            return null;
        }
        GameObject go = GameObject.Instantiate(iconTrans.gameObject);
        go.name = GetIconName(type, entity);
        go.SetActive(true);
        return go;
    }
    StringBuilder m_iconName = new StringBuilder();

    string m_duiyouStr = string.Empty;
    string m_masterStr = string.Empty;
    string m_npcStr = string.Empty;
    string m_otherPlayer = string.Empty;
    string m_pathStr = string.Empty;
    string m_petStr = string.Empty;
    string m_playerStr = string.Empty;
    string m_robotStr = string.Empty;
    /// <summary>
    /// 防止update里面 tostring 造成的gc
    /// </summary>
    /// <param name="type"></param>
    /// <returns></returns>
    public string GetStringByType(IconType type)
    {
        if (type == IconType.duiyouicon)
        {
            if (string.IsNullOrEmpty(m_duiyouStr))
            {
                m_duiyouStr = type.ToString();
            }
            return m_duiyouStr;
        }
        else if (type == IconType.mastericon)
        {
            if (string.IsNullOrEmpty(m_masterStr))
            {
                m_masterStr = type.ToString();
            }
            return m_masterStr;
        }
        else if (type == IconType.npcicon)
        {
            if (string.IsNullOrEmpty(m_npcStr))
            {
                m_npcStr = type.ToString();
            }
            return m_npcStr;
        }
        else if (type == IconType.otherplayericon)
        {
            if (string.IsNullOrEmpty(m_otherPlayer))
            {
                m_otherPlayer = type.ToString();
            }
            return m_otherPlayer;
        }
        else if (type == IconType.pathpointicon)
        {
            if (string.IsNullOrEmpty(m_pathStr))
            {
                m_pathStr = type.ToString();
            }
            return m_pathStr;

        }
        else if (type == IconType.peticon)
        {
            if (string.IsNullOrEmpty(m_petStr))
            {
                m_petStr = type.ToString();
            }
            return m_petStr;
        }
        else if (type == IconType.playericon)
        {
            if (string.IsNullOrEmpty(m_playerStr))
            {
                m_playerStr = type.ToString();
            }
            return m_playerStr;
        }
        else if (type == IconType.robot)
        {
            if (string.IsNullOrEmpty(m_robotStr))
            {
                m_robotStr = type.ToString();
            }
            return m_robotStr;
        }
        else
        {
            Engine.Utility.Log.Error("类型不存在");
        }
        return "";
    }
    public string GetIconName(IconType type, IEntity entity)
    {
        m_iconName.Remove(0, m_iconName.Length);
        m_iconName.Append(GetStringByType(type));
        return m_iconName.ToString();

    }
}

