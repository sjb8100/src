//*************************************************************************
//	创建日期:	2016-12-29 17:48
//	文件名称:	RidePropety.cs
//  创 建 人:   	Chengxue.Zhao
//	版权所有:	中青宝
//	说    明:	坐骑属性界面
//*************************************************************************
using System;
using System.Collections.Generic;
using UnityEngine;
using table;
public partial class RidePanel
{
    private IRenerTextureObj m_RTObj = null;
    //骑术rt 正在出站的
    private IRenerTextureObj m_KnightRTObj = null;
    float rotateY = -94f;

    uint m_rideid;
    uint baseRideId = 0;
    /// <summary>
    /// 恢复饱食度
    /// </summary>
    /// <param name="caster"></param>
    void onClick_Btn_satiation_setting_Btn(GameObject caster)
    {
        if (m_currRideData != null)
        {
            DataManager.Manager<UIPanelManager>().ShowPanel(PanelID.UseItemCommonPanel,
               data: new UseItemCommonPanel.UseItemParam
               {
                   type = UseItemCommonPanel.UseItemEnum.RideFeed,
                   userId = m_currRideData.id,
                   tabs = new int[1] { (int)m_Content },
               });
        }

    }

    private void LoadingPropetyUI()
    {
        if (m__rideModel != null)
            UIEventListener.Get(m__model_bg.gameObject).onDrag = OnDragModel;
    }

    void OnDragModel(GameObject go, Vector2 delta)
    {
        if (m_RTObj != null)
        {
            rotateY += -0.5f * delta.x;
            m_RTObj.SetModelRotateY(rotateY);
        }
    }

    private void InitPropetyUI(RideData data)
    {
        m_rideid = 0;
        if (data != null)
        {
            m_rideid = data.id;

            UpdateFightState();

            if (m_label_Ride_Name != null)
            {
                m_label_Ride_Name.text = data.name; 
            }

            if (m_label_level != null) m_label_level.text = data.level.ToString();

            if (m_label_RideSpeedLabel != null) m_label_RideSpeedLabel.text ="速度+"+ data.GetSpeed().ToString() + "%";

            if (m_label_life != null) m_label_life.text = data.life.ToString();

            //m_btn_btn_diuqi.gameObject.SetActive(data.life <= 0);
            //m_btn_btn_seal.gameObject.SetActive(data.life > 0);
            if (m_label_Repletion != null) m_label_Repletion.text = data.repletion.ToString() + "/" + data.maxRepletion;


            table.RideFeedData maxFeedata = GameTableManager.Instance.GetTableItem<table.RideFeedData>(data.baseid, data.level + 1);
            if (maxFeedata != null)
            {
                table.RideFeedData feedata = GameTableManager.Instance.GetTableItem<table.RideFeedData>(data.baseid, data.level);
                if (feedata != null)
                {
                    m_label_maxLevel.enabled = false;

                   // m_btn_btn_addExp.gameObject.SetActive(true);
                   // m_label_expLabel.gameObject.SetActive(true);
                   //// m_slider_ExpSlider.gameObject.SetActive(true);
                   // m_label_expLabel.text = string.Format("{0}/{1}", data.exp, feedata.upExp);
                   // m_slider_ExpSlider.value = data.exp * 1.0f / feedata.upExp;
                }
            }
            else
            {
                m_label_maxLevel.enabled = true;
                m_btn_btn_addExp.gameObject.SetActive(false);
                m_label_expLabel.gameObject.SetActive(false);
                m_slider_ExpSlider.gameObject.SetActive(false);
            }

            if (baseRideId != data.modelid)
            {
                baseRideId = data.modelid;
                if (m_RTObj != null)
                {
                    m_RTObj.Release();
                }
                m__rideModel.gameObject.SetActive(true);
                m_RTObj = DataManager.Manager<RenderTextureManager>().CreateRenderTextureObj((int)baseRideId, 750);
                if (m_RTObj == null)
                {
                    return;
                }
                ModeDiplayDataBase modelDisp = GameTableManager.Instance.GetTableItem<ModeDiplayDataBase>(baseRideId);
                if (modelDisp == null)
                {
                    Engine.Utility.Log.Error("BOSS模型ID为{0}的模型展示数据为空", baseRideId);
                    return;
                }

                m_RTObj.SetDisplayCamera(modelDisp.pos750, modelDisp.rotate750, modelDisp.Modelrotation);
                m_RTObj.PlayModelAni(Client.EntityAction.Stand);
                UIRenderTexture rt = m__rideModel.GetComponent<UIRenderTexture>();
                if (null == rt)
                {
                    rt = m__rideModel.gameObject.AddComponent<UIRenderTexture>();
                }
                if (null != rt)
                {
                    rt.SetDepth(0);
                    rt.Initialize(m_RTObj, m_RTObj.YAngle, new UnityEngine.Vector2(750, 750));
                }
            }

           
        }
    }

    private void PropetyRelease()
    {
        if (m_RTObj != null)
        {
            m_RTObj.Release();
            m_RTObj = null;
        }
        if(m_KnightRTObj != null)
        {
            m_KnightRTObj.Release();
            m_KnightRTObj = null;
        }
        baseRideId = 0;
    }

    void onClick_Btn_addExp_Btn(GameObject caster)
    {
        if (m_currRideData != null)
        {
            DataManager.Manager<UIPanelManager>().ShowPanel(PanelID.UseItemCommonPanel,
               data: new UseItemCommonPanel.UseItemParam { 
                   type = UseItemCommonPanel.UseItemEnum.RideExp, 
                   userId = m_currRideData.id,
                   tabs = new int[1]{(int)m_Content},
               });
        }
    }

    void onClick_Btn_fight_Btn(GameObject caster)
    {
        if (m_currRideData != null)
        {
            if (m_rideMgr.Auto_Ride == m_currRideData.id)
                DataManager.Instance.Sender.RideCallBackRideUser(m_currRideData.id);
            else
                DataManager.Instance.Sender.RideFightRideUser(m_currRideData.id);
        }
    }

    void onClick_Btn_seal_Btn(GameObject caster)
    {
        if (m_currRideData != null)
        {
            if (m_currRideData.id == m_rideMgr.Auto_Ride)
            {
                TipsManager.Instance.ShowTipsById(113007);
                return;
            }

        }
        DataManager.Manager<UIPanelManager>().ShowPanel(PanelID.RideAbandonPanel, data: m_currRideData);
    }

    void onClick_Btn_diuqi_Btn(GameObject caster)
    {
        DataManager.Manager<UIPanelManager>().ShowPanel(PanelID.RideAbandonPanel, data: m_currRideData);

    }
}
