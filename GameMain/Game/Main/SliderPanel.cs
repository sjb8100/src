using System;
using System.Collections;
using System.Collections.Generic;
using Client;
using Engine.Utility;
using UnityEngine;

public partial class SliderPanel : UIPanelBase
{

    UISprite m_sprite = null;


    SliderDataManager m_SliderData;


    bool bShow = false;
    protected override void OnLoading()
    {
        base.OnLoading();
        m_SliderData = DataManager.Manager<SliderDataManager>();
        m_SliderData.ValueUpdateEvent += m_SliderData_ValueUpdateEvent;

        m_sprite = m_slider_SkillProgressBar.GetComponent<UISprite>();
        HideSprite();
    }

    void m_SliderData_ValueUpdateEvent(object sender, ValueUpdateEventArgs e)
    {
        if (e.key == SliderDataEnum.ShowProcess.ToString())
        {
         
        }
        else if (e.key == SliderDataEnum.Begin.ToString())
        {
            if (m_slider_SkillProgressBar != null)
            {
                m_slider_SkillProgressBar.Set(0);
            }
            bShow = true;
            ShowSprite();
        }
        else if (e.key == SliderDataEnum.End.ToString())
        {
            bShow = false;
            HideSprite();
            if (e.oldValue !=  null)
            {
                GameCmd.UninterruptActionType type = (GameCmd.UninterruptActionType)e.oldValue;
                if (type == GameCmd.UninterruptActionType.UninterruptActionType_CangBaoTuCJ)
                {
                    //重置一下实体  因为藏宝图的锄头需要在挖宝打断时重新置回默认的物品模型
                    DataManager.Manager<SuitDataManager>().RebackWeaponSuit();
//                     IPlayer treasurer = MainPlayerHelper.GetMainPlayer();
//                     if (treasurer != null)
//                     {
//                         ChangeEquip ce = new ChangeEquip();
//                         ce.nSuitID = 0;
//                         ce.pos = Client.EquipPos.EquipPos_Weapon;
//                         ce.nStateType = treasurer.GetProp((int)PlayerProp.SkillStatus);
//                         treasurer.SendMessage(EntityMessage.EntityCommand_ChangeEquip, ce);
//                     }
                }
            }
            HideSelf();
         
          
           
        }

    }
    void Update()
    {
        if(bShow)
        {
            if (m_slider_SkillProgressBar != null)
            {
                m_slider_SkillProgressBar.Set(m_SliderData.SliderProcess);
            }
        }
    }
    //public static void SetSkillProgressBar(float value)
    //{
    //    m_slider_SkillProgressBar.Set(value);
    //}

    void ShowSprite()
    {
        if (m_sprite != null)
        {
            m_sprite.alpha = 1;
        }
    }
    void HideSprite()
    {
        if (m_sprite != null)
        {
            m_sprite.alpha = 0;
        }
       
    }
    protected override void OnShow(object data)
    {
        base.OnShow(data);
    }

    protected override void OnHide()
    {
        base.OnHide();
    }

    protected override void OnPanelBaseDestory()
    {

        m_SliderData.ValueUpdateEvent -= m_SliderData_ValueUpdateEvent;
        // Engine.Utility.EventEngine.Instance().RemoveEventListener((int)GameEventID.ENTITYSYSTEM_ENTITYBEGINMOVE, MoveCancel);
        base.OnPanelBaseDestory();
    }

   
    

}