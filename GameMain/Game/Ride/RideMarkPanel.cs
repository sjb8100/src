using System;
using System.Collections.Generic;
using UnityEngine;
using table;
//坐骑详细
partial class RideMarkPanel : UIPanelBase
{
    private IRenerTextureObj m_RTObj = null;
    float rotateY = 50f;
    protected override void OnLoading()
    {
        base.OnLoading();
        if (m__RideModel != null)
            UIEventListener.Get(m__RideModel.gameObject).onDrag = OnDragModel;
    }
    void OnDragModel(GameObject go, Vector2 delta)
    {
        if (m_RTObj != null)
        {
            rotateY += -0.5f * delta.x;
            m_RTObj.SetModelRotateY(rotateY);
        }
    }
    protected override void OnShow(object data)
    {
        base.OnShow(data);

        if (data is table.RideDataBase)
        {
            table.RideDataBase database = data as table.RideDataBase;

           // m_label_name.text = "坐骑详细";
            m_label_rarity.text = DataManager.Manager<RideManager>().GetRideQualityStr(database.quality);
            m_label_showname.text = database.name;
            table.RideFeedData feeddata = GameTableManager.Instance.GetTableItem<table.RideFeedData>(database.rideID, 0);
            if (feeddata != null)
            {
                float value = (feeddata.speed / 100.0f) ;
                m_label_speed.text = value.ToString() + "%";
            }
            m_label_getway.text = database.getWay;

            if (m_RTObj != null)
            {
                m_RTObj.Release();
            }

            m_RTObj = DataManager.Manager<RenderTextureManager>().CreateRenderTextureObj((int)database.viewresid, 512);
            if (m_RTObj == null)
            {
                return;
            }
            ModeDiplayDataBase modelDisp = GameTableManager.Instance.GetTableItem<ModeDiplayDataBase>(database.viewresid);
            if (modelDisp == null)
            {
                Engine.Utility.Log.Error("BOSS模型ID为{0}的模型展示数据为空", database.viewresid);
                return;
            }

            m_RTObj.SetDisplayCamera(modelDisp.pos512, modelDisp.rotate512,modelDisp.Modelrotation);
            m_RTObj.PlayModelAni(Client.EntityAction.Stand);
            UIRenderTexture rt = m__RideModel.GetComponent<UIRenderTexture>();
            if (null == rt)
            {
                rt = m__RideModel.gameObject.AddComponent<UIRenderTexture>();
            }
            if (null != rt)
            {
                rt.SetDepth(0);
                rt.Initialize(m_RTObj, m_RTObj.YAngle, new UnityEngine.Vector2(512, 512));
            }
        }
    }

    void onClick_Close_Btn(GameObject caster)
    {
        this.HideSelf();
    }

    protected override void OnHide()
    {
        base.OnHide();
        if (m_RTObj != null)
        {
            m_RTObj.Release();
            m_RTObj = null;
        }
    }
}