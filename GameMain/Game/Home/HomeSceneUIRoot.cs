using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Client;
using GameCmd;
using table;
using Engine;
using Engine.Utility;

class HomeSceneUIRoot : SingletonMono<HomeSceneUIRoot>
{
    HomeDataManager homeDM
    {
        get
        {
            return DataManager.Manager<HomeDataManager>();
        }
    }
    Camera uiCamera;
    Camera mainCamera;
    IEntitySystem es;
    Dictionary<long, GameObject> uiDic = new Dictionary<long, GameObject>();
    public bool disableIfInvisible = true;

    List<GameObject> uiList = new List<GameObject>();
    List<GameObject> uiAnimalList = new List<GameObject>();
    List<GameObject> uiTreeList = new List<GameObject>();
    public void Init()
    {
        this.transform.parent = Util.UICameraObj.transform;
        transform.localPosition = Vector3.zero;
        transform.localScale = Vector3.one;
        transform.localRotation = Quaternion.identity;
    }

    public void ShowPlantSceneUI()
    {
        gameObject.SetActive(true);
        uiCamera = Util.UICameraObj.GetComponent<Camera>();
        mainCamera = Util.MainCameraObj.GetComponent<Camera>();
        es = ClientGlobal.Instance().GetEntitySystem();
        if (es == null)
            return;

        foreach (var info in homeDM.EntityStateDic)
        {
            IEntity en = es.FindEntity(info.Key);
            if (en != null)
            {
                Vector3 enpos = en.GetPos();
                enpos.y = 0;
                enpos = mainCamera.WorldToViewportPoint(enpos);
                enpos = uiCamera.ViewportToWorldPoint(enpos);

                ShowFarmUI(info, enpos);
                ShowAnimalUI(info, enpos);
                ShowTreeUI(info, enpos);
            }
        }
    }


    bool ShowFarmUI(KeyValuePair<long, HomeDataManager.HomeEntityInfo> info, Vector3 enpos)
    {
        HomeDataManager.HomeEntityInfo ei = info.Value;
        if (ei.type == EntityType.EntityType_Soil)//土地
        {
            IEntity en = es.FindEntity(info.Key);
            if (en != null)
            {
                if (uiDic.ContainsKey(en.GetUID()))
                {
                    GameObject go = uiDic[en.GetUID()];
                    if (go != null)
                    {
                        RefreshFarmState(go, ei);
                        return false;
                    }
                }
                GameObject ui = GetUIGameObejct(enpos);
                if (ui)
                {
                    RefreshFarmState(ui, ei);
                    uiDic.Add(en.GetUID(), ui);
                }

            }
        }
        return true;
    }
    void RetrunUIGameObject(GameObject ui)
    {
        if (ui != null)
        {
            uiList.Add(ui);
        }
    }
    GameObject GetUIGameObejct(Vector3 enpos)
    {
        if (uiList.Count > 0)
        {
            GameObject ui = uiList[0];
            uiList.RemoveAt(0);
            FarmState fs = ui.GetComponent<FarmState>();
            if (fs == null)
            {
                ui.AddComponent<FarmState>();
            }

            return ui;
        }
        List<string> deplist = null;
        UnityEngine.Object farm = UIManager.GetResGameObj("panel/home/frampanel/framstatus.unity3d", "assets/ui/panel/home/frampanel/framstatus.prefab");
        if (farm != null)
        {
            GameObject go = GameObject.Instantiate(farm) as GameObject;
            if (go != null)
            {
                go.transform.parent = this.transform;
                go.transform.position = enpos;
                go.transform.localScale = Vector3.one;
                go.transform.localRotation = Quaternion.identity;
                return go;
            }
        }
        return null;
    }
    void RefreshFarmState(GameObject go, HomeDataManager.HomeEntityInfo ei)
    {
        FarmState fs = go.GetComponent<FarmState>();

        if (fs == null)
        {
            fs = go.AddComponent<FarmState>();
            go.SetActive(false);
        }
        if (fs != null)
        {
            fs.InitIndex(ei.index);
        }
        if (ei.state == (int)HomeDataManager.LandState.Idle)
        {
            fs.gameObject.SetActive(false);
            if (homeDM.CanGetLeftTime(ei.index))
            {
                fs.Init(homeDM.GetLeftTimeByIndex(ei.index), ei.index);
            }
            else
            {
                fs.ShowUIByState(HomeDataManager.CreatureSmallState.None);
            }
        }
        else if (ei.state == (int)HomeDataManager.LandState.LockCanBuy)
        {
            if (ei.index == homeDM.LandUnlockNum + 1)
            {
                fs.gameObject.SetActive(true);
                fs.ShowLandState();
            }
        }
        else if (ei.state == (int)HomeDataManager.LandState.CanGain)
        {
            fs.ShowUIByState(HomeDataManager.CreatureSmallState.CanGain);
        }
        else if (ei.state == (int)HomeDataManager.LandState.Growing)
        {
            fs.ShowUIByState(HomeDataManager.CreatureSmallState.Seeding);
        }


    }
    public void ReleaseUI()
    {
        foreach (var dic in uiDic)
        {
            long uid = dic.Key;

            if (homeDM.EntityStateDic.ContainsKey(uid))
            {
                 HomeDataManager.HomeEntityInfo info = homeDM.EntityStateDic[uid];
                if(info.type == EntityType.EntityType_Soil)
                {
                    RetrunUIGameObject(dic.Value);
                }
                else if (info.type == EntityType.EntityType_Animal)
                {
                    RetrunUIAnimalGameObject(dic.Value);
                }
                else if (info.type == EntityType.EntityType_Tree)
                {
                    RetrunUITreeGameObject(dic.Value);
                }
            }
           
            
        }
        uiDic.Clear();
        gameObject.SetActive(false);
    }
    void Update()
    {
        if (es == null)
            return;
        foreach (var dic in uiDic)
        {

            IEntity en = es.FindEntity(dic.Key);
            if (en != null)
            {
                GameObject item = dic.Value;
                Vector3 enpos = en.GetPos();

                enpos = mainCamera.WorldToViewportPoint(enpos);
                bool isVisible = (mainCamera.orthographic || enpos.z > 0f) && (!disableIfInvisible || (enpos.x > 0f && enpos.x < 1f && enpos.y > 0f && enpos.y < 1f));
                if (item.gameObject.activeSelf != isVisible)
                {
                    //  item.SetActive( isVisible );
                }
                Vector3 pos = uiCamera.ViewportToWorldPoint(enpos);

                item.transform.position = pos;
                pos = item.transform.localPosition;
                pos.x = Mathf.FloorToInt(pos.x);
                pos.y = Mathf.FloorToInt(pos.y) + 100;
                pos.z = 0f;
                item.transform.localPosition = pos;
            }
        }
    }
    #region   许愿树
    bool ShowTreeUI(KeyValuePair<long, HomeDataManager.HomeEntityInfo> info, Vector3 enpos)
    {
        HomeDataManager.HomeEntityInfo ei = info.Value;
        if (ei.type == EntityType.EntityType_Tree)//许愿树
        {
            IEntity en = es.FindEntity(info.Key);
            if (en != null)
            {
                if (uiDic.ContainsKey(en.GetUID()))
                {
                    GameObject go = uiDic[en.GetUID()];
                    if (go != null)
                    {
                        RefreshTreeState(go,ei);
                        return false;
                    }
                }
                GameObject ui = GetUITreeGameObejct(enpos);
                if (ui)
                {
                    RefreshTreeState(ui,ei);
                    uiDic.Add(en.GetUID(), ui);
                }

            }
        }
        return true;
    }

    void RefreshTreeState(GameObject go, HomeDataManager.HomeEntityInfo ei)
    {
        TreeState ts = go.GetComponent<TreeState>();

        if (ts == null)
        {
            ts = go.AddComponent<TreeState>();
            go.SetActive(false);
        }
        TreeData td = DataManager.Manager<HomeDataManager>().GetTreeData();
        if (td != null)
        {
            ts.Init();
        }
    }

    GameObject GetUITreeGameObejct(Vector3 enpos)
    {
        if (uiTreeList.Count > 0)
        {
            GameObject ui = uiTreeList[0];
            uiTreeList.RemoveAt(0);
            TreeState ts = ui.GetComponent<TreeState>();
            if(ts == null)
            {
                ts = ui.AddComponent<TreeState>();
            }

            return ui;
        }
        //List<string> deplist = null;
        UnityEngine.Object tree =  UIManager.GetResGameObj("panel/home/wishtreepanel/treestatus.unity3d", "assets/ui/panel/home/wishtreepanel/treestatus.prefab");
        if (tree != null)
        {
            GameObject go = GameObject.Instantiate(tree) as GameObject;
            if (go != null)
            {
                go.transform.parent = this.transform;
                go.transform.position = enpos;
                go.transform.localScale = Vector3.one;
                go.transform.localRotation = Quaternion.identity;
                return go;
            }
        }
        return null;
    }

    void RetrunUITreeGameObject(GameObject ui)
    {
        if (ui != null)
        {
            uiTreeList.Add(ui);
        }
    }


    #endregion

    #region     Animal 
    bool ShowAnimalUI(KeyValuePair<long, HomeDataManager.HomeEntityInfo> info, Vector3 enpos) 
    {
        HomeDataManager.HomeEntityInfo ei = info.Value;
        if (ei.type == EntityType.EntityType_Animal)//动物
        {
            IEntity en = es.FindEntity(info.Key);
            if (en != null)
            {
                if (uiDic.ContainsKey(en.GetUID()))
                {
                    GameObject go = uiDic[en.GetUID()];
                    if (go != null)
                    {
                        RefreshAnimalState(go, ei);
                        return false;
                    }
                }
                GameObject ui = GetAnimaUIGameObejct(enpos);
                if (ui)
                {
                    RefreshAnimalState(ui, ei);
                    uiDic.Add(en.GetUID(), ui);
                }

            }
        }
        return true;
    }

    void RefreshAnimalState(GameObject go, HomeDataManager.HomeEntityInfo ei)
    {
        AnimalState fs = go.GetComponent<AnimalState>();

        if (fs == null)
        {
            fs = go.AddComponent<AnimalState>();
            go.SetActive(false);
        }
        if (fs != null)
        {
            fs.InitIndex(ei.index);
        }

        fs.gameObject.SetActive(true);
        if (homeDM.CanGetLeftTime(ei.index))
        {
            fs.Init(homeDM.GetLeftTimeByIndex(ei.index), ei.index ,ei.state);
        }
    }

    GameObject GetAnimaUIGameObejct(Vector3 enpos)
    {
        if (uiAnimalList.Count > 0)
        {
            GameObject ui = uiAnimalList[0]; 
            uiAnimalList.RemoveAt(0);
            AnimalState fs = ui.GetComponent<AnimalState>();
            if (fs == null)
            {
                ui.AddComponent<AnimalState>();
            }

            return ui;
        }
        List<string> deplist = null;
        UnityEngine.Object farm = UIManager.GetResGameObj("panel/home/animalpanel/animalstatus.unity3d", "assets/ui/panel/home/animalpanel/animalstatus.prefab");
        
        if (farm != null)
        {
            GameObject go = GameObject.Instantiate(farm) as GameObject;
            if (go != null)
            {
                go.transform.parent = this.transform;
                go.transform.position = enpos;
                go.transform.localScale = Vector3.one;
                go.transform.localRotation = Quaternion.identity;
                return go;
            }
        }
        return null;
    }

    void RetrunUIAnimalGameObject(GameObject ui)
    {
        if (ui != null)
        {
            uiAnimalList.Add(ui);
        }
    }

#endregion

}
