//*******************************************************************************************
//	创建日期：	2016-9-29   21:50
//	文件名称：	HomeAnimalPanel,cs
//	创 建 人：	Lin Chuang Yuan
//	版权所有：	Lin Chuang Yuan
//	说    明：	牧场UI
//*******************************************************************************************

using Client;
using Engine.Utility;
using GameCmd;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using table;
using UnityEngine;


partial class HomeAnimalPanel : UIPanelBase
{

    protected override void OnLoading()
    {
        base.OnLoading();
        EventEngine.Instance().AddEventListener((int)GameEventID.HOMELAND_UPDATEANIMAL, DoGameEvent);
    }
    protected override void OnShow(object data)
    {
        base.OnShow(data);

        Init();
    }

    void DoGameEvent(int eventID, object param)
    {
        if (eventID == (int)GameEventID.HOMELAND_UPDATEANIMAL)
        {
            UpdateAnimalGridList();
        }
    }

    /// <summary>
    /// 初始化界面
    /// </summary>
    void Init()
    {
        if (m_grid_grid.transform.childCount > 0)
        {
            UpdateAnimalGridList();
            return;
        }

        List<LandAndFarmDataBase> landList = GameTableManager.Instance.GetTableList<LandAndFarmDataBase>();

        uint animalID = DataManager.Manager<HomeDataManager>().animalID;

        landList = landList.FindAll((LandAndFarmDataBase ld) => { return ld.dwID == animalID; });

        for (int i = 0; i < landList.Count;i++)
        {
            UnityEngine.GameObject obj = UIManager.GetResGameObj(GridID.Uihomeanimalgrid) as UnityEngine.GameObject;
            obj = Instantiate(obj);

            obj.transform.parent = m_grid_grid.transform;
            obj.transform.localPosition = Vector3.zero;
            obj.transform.localScale = Vector3.one;
            obj.transform.localRotation = Quaternion.identity;
            
            UIHomeAnimalGrid homeAnimalGrid = obj.GetComponent<UIHomeAnimalGrid>();
            if (homeAnimalGrid == null)
            {
                homeAnimalGrid = obj.AddComponent<UIHomeAnimalGrid>();
            }

            obj.SetActive(true);
            homeAnimalGrid.Init(landList[i].indexID);
        }

        m_grid_grid.Reposition();

    }

    void UpdateAnimalGridList()
    {
        if (this.gameObject.activeSelf == false)
        {
            return;
        }

        List<LandAndFarmDataBase> landList = GameTableManager.Instance.GetTableList<LandAndFarmDataBase>();

        uint animalID = DataManager.Manager<HomeDataManager>().animalID;

        landList = landList.FindAll((LandAndFarmDataBase ld) => { return ld.dwID == animalID; });

        for (int i = 0; i < m_grid_grid.transform.childCount; i++)
        {
            UIHomeAnimalGrid homeAnimalGrid = m_grid_grid.GetChild(i).GetComponent<UIHomeAnimalGrid>();
            if (homeAnimalGrid == null)
            {
                homeAnimalGrid = m_grid_grid.GetChild(i).gameObject.AddComponent<UIHomeAnimalGrid>();
            }

            if (landList[i] != null)
            {
                homeAnimalGrid.Init(landList[i].indexID);
            }
        }
    }

    void onClick_Close_Btn(GameObject caster)
    {
        HideSelf();
    }

}

