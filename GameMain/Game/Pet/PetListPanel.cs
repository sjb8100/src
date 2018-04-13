//*************************************************************************
//	创建日期:	2016/10/27 15:02:16
//	文件名称:	PetListPanel
//   创 建 人:   zhuidanyu	
//	版权所有:	中青宝
//	说    明:	pet设置列表
//*************************************************************************
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using table;
using Client;


partial class PetListPanel
{
    PetDataManager m_petDataManager
    {
        get
        {
            return DataManager.Manager<PetDataManager>();
        }
    }
    private UIGridCreatorBase m_petSettingGridCreator;

    protected override void OnLoading()
    {
        base.OnLoading();
        m_petDataManager.ValueUpdateEvent += m_petDataManager_ValueUpdateEvent;

        if (m_widget_box1.GetComponent<UIWidget>() == null)
        {
            m_widget_box1.gameObject.AddComponent<UIWidget>();
        }
        if (m_widget_box2.GetComponent<UIWidget>() == null)
        {
            m_widget_box2.gameObject.AddComponent<UIWidget>();
        }
        UIEventListener.Get(m_widget_box1.gameObject).onClick = onClick_Btn_queding_Btn;
        UIEventListener.Get(m_widget_box2.gameObject).onClick = onClick_Btn_queding_Btn;
    }

    public void OnDestroy()
    {
        m_petDataManager.ValueUpdateEvent -= m_petDataManager_ValueUpdateEvent;
    }
    protected override void OnHide()
    {
       
        base.OnHide();
        Release();
    }
    protected override void OnPanelBaseDestory()
    {
        base.OnPanelBaseDestory();
        if (null != m_petSettingGridCreator)
        {
            UIManager.OnObjsRelease(m_petSettingGridCreator.CacheTransform, (uint)GridID.Uipetsettinggrid);
            m_petSettingGridCreator = null;
        }
    }
    void m_petDataManager_ValueUpdateEvent(object sender , ValueUpdateEventArgs e)
    {
        if ( e != null )
        {
            if ( e.key == PetDispatchEventString.RefreshQuickSetting.ToString() )
            {
                m_petSettingGridCreator.UpdateActiveGridData();
            }
        }
    }
    public override void OnColliderMaskClicked()
    {
        base.OnColliderMaskClicked();
      
        //HideSelf();
        //DataManager.Manager<UIPanelManager>().SendMsg(PanelID.PetSettingPanel, UIMsgID.eShowQuickSettingBtn, null);

    }
    protected override void OnShow(object data)
    {

        base.OnShow( data );
        InitScrollView();
    }
    void InitScrollView()
    {
        m_petSettingGridCreator = m_trans_PetScrollView.GetComponent<UIGridCreatorBase>();
        if ( m_petSettingGridCreator == null )
        {
            m_petSettingGridCreator = m_trans_PetScrollView.gameObject.AddComponent<UIGridCreatorBase>();
        }
       // GameObject obj = UIManager.GetResGameObj( GridID.Uipetsettinggrid ) as GameObject;
        m_petSettingGridCreator.arrageMent = UIGridCreatorBase.Arrangement.Horizontal;
        m_petSettingGridCreator.gridContentOffset = new Vector2( -177 , -240 );
        m_petSettingGridCreator.gridWidth = 90;
        m_petSettingGridCreator.gridHeight = 90;
        m_petSettingGridCreator.rowcolumLimit = 5;
        m_petSettingGridCreator.RefreshCheck();
        m_petSettingGridCreator.Initialize<UIPetSettingGrid>((uint)GridID.Uipetsettinggrid, UIManager.OnObjsCreate, UIManager.OnObjsRelease, OnUpdateUIGrid, OnUIGridEventDlg);
       // m_petSettingGridCreator.Initialize<UIPetSettingGrid>(obj, OnUpdateUIGrid, OnUIGridEventDlg);
        Dictionary<uint , IPet> petDic = m_petDataManager.GetPetDic();
        int count = 0;
        if ( petDic != null )
        {
            count = petDic.Keys.Count;
        }
        m_petSettingGridCreator.CreateGrids( count );
    }
    public override void Release(bool depthRelease = true)
    {
        base.Release(depthRelease);
        if(m_petSettingGridCreator != null)
        {
            m_petSettingGridCreator.Release(depthRelease);
        }

    }
    private void OnUIGridEventDlg(UIEventType eventType , object data , object param)
    {
        if ( null == data )
        {
            return;
        }
        switch ( eventType )
        {
            case UIEventType.Click:
                {
                    if ( data is UIPetSettingGrid )
                    {
                        UIPetSettingGrid gridData = data as UIPetSettingGrid;
                        uint petID = gridData.PetID;
                        if ( gridData.IsSetting() )
                        {
                            TipsManager.Instance.ShowTips( "已经设置过了" );
                        }
                        else
                        {
                            m_petDataManager.SetUserQuickList( petID );
                            m_petDataManager.SendQuickListMsg();
                        }
                    }
                }
                break;
        }
    }
    void OnUpdateUIGrid(UIGridBase grid , int index)
    {
        Dictionary<uint , IPet> petDic = m_petDataManager.GetPetDic();
        if ( petDic != null )
        {
            List<IPet> petList = petDic.Values.ToList<IPet>();
            if(petList == null)
            {
                return;
            }
            if(index <petList.Count)
            {
                IPet pet = petList[index];
                if (pet != null)
                {
                    int baseID = pet.GetProp((int)PetProp.BaseID);
                    PetDataBase pdb = GameTableManager.Instance.GetTableItem<PetDataBase>((uint)baseID);
                    if (pdb != null)
                    {

                        UIPetSettingGrid petgrid = grid as UIPetSettingGrid;
                        if (petgrid != null)
                        {
                            petgrid.SetIcon(pdb.icon);
                            List<uint> quickList = m_petDataManager.PetQuickList;
                            // foreach ( var petid in quickList )
                            {
                                if (quickList.Contains(pet.GetID()))
                                {
                                    petgrid.SetStatus(true);
                                }
                                else
                                {
                                    petgrid.SetStatus(false);
                                }
                            }
                            petgrid.SetPetID(pet.GetID());
                        }

                    }
                }
            }
           
        }
    }
    void onClick_Btn_queding_Btn(GameObject caster)
    {
        HideSelf();
        DataManager.Manager<UIPanelManager>().SendMsg(PanelID.PetSettingPanel, UIMsgID.eShowQuickSettingBtn, null);
    }
}

