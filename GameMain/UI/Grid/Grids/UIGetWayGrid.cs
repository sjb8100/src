//********************************************************************
//	创建日期:	2016-12-21   15:37
//	文件名称:	UIGetWayGrid.cs
//  创 建 人:   刘超	
//	版权所有:	中青宝
//	说    明:	道具获取Grid
//********************************************************************
using UnityEngine;
using System;
using table;
using System.Collections.Generic;
using System.Linq;
using GameCmd;

class UIGetWayGrid : UIGridBase
{
    private UILabel name;
    private UILabel Des;
    private UISprite btn_go;
    private UILabel Label;
    private UISprite tuijian;
    private UISprite icon;
    private uint wayIndex;
    public uint WayIndex
    {
        get 
        {
            return wayIndex;
        }
    }
    ItemGetDataBase itemGetData = null;
    public delegate void OnClickItemGetGrid(ItemGetDataBase itemGetData, uint wayIndex);
    public OnClickItemGetGrid onClickItemGetGrid;
    protected override void OnAwake()
    {
        base.OnAwake();
        name = CacheTransform.Find("name").GetComponent<UILabel>();
        Des = CacheTransform.Find("Des").GetComponent<UILabel>();
        btn_go = CacheTransform.Find("btn_go").GetComponent<UISprite>();
        icon = CacheTransform.Find("icon").GetComponent<UISprite>();
        tuijian = CacheTransform.Find("tuijian").GetComponent<UISprite>();
        UIEventListener.Get(btn_go.gameObject).onClick = OnExecuteGo;
    }

    public override void SetGridData(object data)
    {
 	     base.SetGridData(data);
        if(null == data)
        {
           return;
        }
     
    }
  
    public void SetWayData(ItemGetDataBase data )
    {
       this.wayIndex = (uint)data.ID;
       itemGetData = data;
       name.text = data.wayName;
       JumpWayDataBase jumpWayDB = GameTableManager.Instance.GetTableItem<JumpWayDataBase>(data.jumpID);
       UIManager.GetAtlasAsyn(data.icon, ref m_playerAvataCASD, () =>
       {
           if (null != icon)
           {
               icon.atlas = null;
           }
       }, icon);

       if (jumpWayDB.jumpTypeID == 4)
       {
           btn_go.gameObject.SetActive(false);
       }
       else
       {
           btn_go.gameObject.SetActive(true);
       }
    }

    CMResAsynSeedData<CMAtlas> m_playerAvataCASD = null;
    public override void Release(bool depthRelease = true)
    {
        base.Release(depthRelease);
        if (m_playerAvataCASD != null)
        {
            m_playerAvataCASD.Release(depthRelease);
            m_playerAvataCASD = null;
        }
    }

    void OnExecuteGo(GameObject caster) 
    { 
           onClickItemGetGrid(itemGetData,wayIndex);
    }
}
