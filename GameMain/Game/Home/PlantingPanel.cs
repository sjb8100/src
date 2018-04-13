using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Client;
using GameCmd;
using table;
using Engine;
using Engine.Utility;
partial class PlantingPanel : UIPanelBase
{
    private EntityType m_entityType;

    HomeDataManager homeDM
    {
        get
        {
            return DataManager.Manager<HomeDataManager>();
        }
    }
    List<SeedAndCubDataBase> list = new List<SeedAndCubDataBase>();
    protected override void OnLoading()
    {
        base.OnLoading();
    }
    protected override void OnShow(object data)
    {
        base.OnShow(data);
        this.m_entityType = (EntityType)data;

        UIEventListener.Get(m_widget_close.gameObject).onClick = OnClose;
        m_sprite_SeedItem.gameObject.SetActive(false);
        m_grid_seedgrid.arrangement = UIGrid.Arrangement.Vertical;
        InitScrollView();
    }
    void InitScrollView()
    {
        list =GameTableManager.Instance.GetTableList<SeedAndCubDataBase>();
        if (list != null)
        {
            if (this.m_entityType == EntityType.EntityType_Plant)//Ö²Îï
            {
                list = list.FindAll((x) => { return x.type == 0; });
            }
            
            if (this.m_entityType == EntityType.EntityType_Animal)
            {
                list = list.FindAll((x) => { return x.type == 1; });//¶¯ÎïÓ×áÌ
            }
            
            foreach (var db in list)
            {
                Transform itemTrans = m_grid_seedgrid.transform.Find(db.itemID.ToString());
                if (itemTrans == null)
                {
                    GameObject seedItem = NGUITools.AddChild(m_grid_seedgrid.gameObject, m_sprite_SeedItem.gameObject);
                    if (seedItem != null)
                    {
                        seedItem.name = db.itemID.ToString();
                        itemTrans = seedItem.transform;
                    }

                }
                itemTrans.gameObject.SetActive(true);
                SeedItem seed = itemTrans.GetComponent<SeedItem>();
                if (seed == null)
                {
                    seed = itemTrans.gameObject.AddComponent<SeedItem>();
                }
                if (seed != null)
                {
                    seed.InitSeedItem(db);
                }
            }
        }
    }
    void OnClose(GameObject go)
    {
        HideSelf();
    }

    void onClick_BtnAutoplay_Btn(GameObject caster)
    {
        homeDM.bAutoPlant = !homeDM.bAutoPlant;
        //int landUnlockNum = homeDM.LandUnlockNum;
       
        //for (int i = 0; i < landUnlockNum;i++ )
        //{
        //    SeedItem s = new SeedItem();
        //    foreach(var m in list)
        //    {
        //        GameObject seed = m_grid_seedgrid.transform.Find(m.itemID.ToString()).gameObject;
        //        s.OnClickSeedItem(seed);
        //        return;
        //    }
           
        //}
    }


}
