//*************************************************************************
//	创建日期:	2017-4-1 15:35
//	文件名称:	ViewRidePanel.cs
//  创 建 人:   	Chengxue.Zhao
//	版权所有:	中青宝
//	说    明:	其他玩家 坐骑查看界面
//*************************************************************************
using System;
using System.Collections.Generic;
using UnityEngine;
using table;
partial class ViewPlayerPanel : UIPanelBase
{

    void OnCreateRideGrid()
    {
        if (m_ctor_Ridescrollview != null)
        {
            m_ctor_Ridescrollview.RefreshCheck();
            m_ctor_Ridescrollview.Initialize<UIPetRideGrid>(m_trans_UIPetRideGrid.gameObject, OnRidtGridDataUpdate, OnRideGridUIEvent);
        }
      
    }

    void InitRideSkill()
    {
        if (m_ctor_RideSkill != null)
        {
            m_ctor_RideSkill.RefreshCheck();
            m_ctor_RideSkill.Initialize<UISkillGrid>(m_trans_UISkillGrid.gameObject,OnRidtGridDataUpdate, OnRideSkillGridUIEvent);
        }
      
    }
    void OnRidtGridDataUpdate(UIGridBase data, int index)
    {
        if (data is UIPetRideGrid)
        {
            UIPetRideGrid grid = data as UIPetRideGrid;
            if (m_lstRideData != null && index < m_lstRideData.Count)
            {
                grid.SetGridData(m_lstRideData[index]);
            }
        }
        if (data is UISkillGrid )
        {
            UISkillGrid grid = data as UISkillGrid;
            if (index < m_lst_RideSkill.Count )
            {
                UISkillGridData para = new UISkillGridData()
                {
                    skillid = m_lst_RideSkill[index],
                    skillType = 2
                };
                grid.SetGridData(para);
            }
          
        }
      
    }
    List<int> m_lst_RideSkill = new List<int>();
    void OnShowRideSkill(List<int> skillids)
    {
        m_lst_RideSkill.Clear();
        m_lst_RideSkill.AddRange(skillids);
        m_ctor_RideSkill.CreateGrids(skillids.Count);
    }

    void OnRideSkillGridUIEvent(UIEventType eventType, object data, object param)
    {
        switch (eventType)
        {
            case UIEventType.Click:
                UISkillGrid grid = data as UISkillGrid;
                if (grid != null)
                {
                    if (grid.Data != null && grid.Data.skillid != 0)
                    {
                        DataManager.Manager<UIPanelManager>().ShowPanel(PanelID.PlayerSkillTipsPanel, panelShowAction: (pb) =>
                        {
                            if (null != pb && pb is PlayerSkillTipsPanel)
                            {
                                PlayerSkillTipsPanel panel = pb as PlayerSkillTipsPanel;
                                if (panel != null)
                                {
                                    table.RideSkillDes skilldb = GameTableManager.Instance.GetTableItem<table.RideSkillDes>((uint)grid.Data.skillid);
                                    panel.ShowUI(PlayerSkillTipsPanel.SkillTipsType.Ride, skilldb, grid.Data.level);
                                    panel.InitParentTransform(grid.transform, new Vector2(0, 80));
                                }
                            }
                        });
                    }                   
                }
                break;
        }
    }
    void OnRideGridUIEvent(UIEventType eventType, object data, object param)
    {
        switch (eventType)
        {
            case UIEventType.Click:
                UIPetRideGrid grid = data as UIPetRideGrid;
                if (grid != null)
                {
                    if (grid.RideData != null)
                    {
                        OnShowRideUI(grid.RideData);
                    }                 
                    m_ctor_Ridescrollview.SetSelect(grid);
                }
                break;
        }
    }

    void OnShowRideUI(RideData data)
    {
        m_label_level.text = DataManager.Manager<RideManager>().GetRideQualityStr(data.quality);
        m_label_Ride_Name.text = data.name;
        m_label_speed.text = data.GetSpeed().ToString() + "%";
        if (data.skill_ids != null)
        {
            OnShowRideSkill(data.skill_ids);
        }       
        if (m_nModelId == data.modelid)
        {
            return;
        }
        if (m_RTObj_Ride != null)
        {
            m_RTObj_Ride.Release();
        }
        m_nModelId = (int)data.modelid;
        m_RTObj_Ride = DataManager.Manager<RenderTextureManager>().CreateRenderTextureObj(m_nModelId, 512);
        if (m_RTObj_Ride == null)
        {
            return;
        }
        table.RideDataBase rideData = GameTableManager.Instance.GetTableItem<table.RideDataBase>(data.baseid);
        if (rideData == null)
        {
            return;
        }
        ModeDiplayDataBase modelDisp = GameTableManager.Instance.GetTableItem<ModeDiplayDataBase>(rideData.viewresid);
        if (modelDisp == null)
        {
            Engine.Utility.Log.Error("坐骑模型ID为{0}的模型展示数据为空", rideData.viewresid);
            return;
        }   
        m_RTObj_Ride.SetDisplayCamera(modelDisp.pos512, modelDisp.rotate512,modelDisp.Modelrotation);
        m_RTObj_Ride.PlayModelAni(Client.EntityAction.Stand);
//         //设置人物旋转
//         m_RTObj_Ride.SetModelRotateY(rotateY);
//         m_RTObj_Ride.SetModelScale(data.modelScale);
//         //人物
        UIRenderTexture rt = m__rideModel.GetComponent<UIRenderTexture>();
        if (null == rt)
        {
            rt = m__rideModel.gameObject.AddComponent<UIRenderTexture>();
        }
        if (null != rt)
        {
            rt.SetDepth(0);
            rt.Initialize(m_RTObj_Ride, m_RTObj_Ride.YAngle, new Vector2(512, 512));
        }

       
    }

    void AddRide(GameCmd.RideData data)
    {
        table.RideDataBase tabledata = GameTableManager.Instance.GetTableItem<table.RideDataBase>(data.base_id);
        if (tabledata != null)
        {
            m_lstRideData.Add(new RideData()
            {
                id = data.id,
               // level = data.level,
               // life = data.life,
                //exp = data.exp,
                //fight_power = data.fight_power,
               // repletion = data.repletion,
                //skill_ids = data.skill_list,
                baseid = data.base_id,
                name = tabledata.name,
                icon = tabledata.icon.ToString(),
                modelid = tabledata.resid,
                spellTime = tabledata.spellTime,
                quality = tabledata.quality,
                maxRepletion = tabledata.maxRepletion,
                subLife = tabledata.subLife,
                modelScale = tabledata.modelScale * 0.01f,
            });
        }
        else
        {
            Engine.Utility.Log.Error("Not Found ride data id:{0}", data.id);
        }
    }
}
