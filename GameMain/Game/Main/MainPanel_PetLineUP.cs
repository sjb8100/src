
//*************************************************************************
//	创建日期:	2017/8/10 星期四 15:14:23
//	文件名称:	MainPanel_PetLineUP
//   创 建 人:   zhudianyu	
//	版权所有:	zhudianyu
//	说    明:	
//*************************************************************************

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Engine.Utility;
using UnityEngine;
using Engine;
using Client;
using System.Collections;
using GameCmd;
using table;
using DG.Tweening;


partial class MainPanel
{
    void InitPetLineUP()
    {
 
        if(m_sprite_petquickclose != null)
        {
            UIEventListener.Get(m_sprite_petquickclose.gameObject).onClick = OnClosePetQuick;
        }
        if (m_sprite_petquicksetting != null)
        {
            UIEventListener.Get(m_sprite_petquicksetting.gameObject).onClick = OnPetQuickSetting;
        }
        if (m_trans_petquicklisting != null)
        {
            m_trans_petquicklisting.gameObject.SetActive(false);
           
        }
    }
    void InitLineupData()
    {
        PetDataManager pm = DataManager.Manager<PetDataManager>();
        if (m_trans_petquicklisting != null)
        {
      
            Transform gridTrans = m_trans_petquicklisting.Find("grid");
            for (int i = 0; i < gridTrans.childCount; i++)
            {
                Transform item = gridTrans.GetChild(i);
                PetLineUPItemInfo info = item.GetComponent<PetLineUPItemInfo>();
                if (info == null)
                {
                    info = item.gameObject.AddComponent<PetLineUPItemInfo>();
                }
                if (i < pm.PetQuickList.Count)
                {
                    info.InitLineUpItem(pm.PetQuickList[i]);
                }
                else
                {
                    info.InitLineUpItem(0);
                }
            }
        }
    }
    void OnPetQuickSetting(GameObject go)
    {
        if (m_trans_petquicklisting != null)
        {
            m_trans_petquicklisting.gameObject.SetActive(true);
            InitLineupData();
        }
    }
    void OnClosePetQuick(GameObject go)
    {
        if(m_trans_petquicklisting != null)
        {
            m_trans_petquicklisting.gameObject.SetActive(false);
        }
    }
    void SetPetRoleBtn(MainBtn petRole)
    {

        if (petRole != null)
        {
            m_petInfo = petRole.GetComponent<PetQuickInfo>();
            if (m_petInfo == null)
            {
                m_petInfo = petRole.gameObject.AddComponent<PetQuickInfo>();
            }
        }
        ShowPetQuickInfo(m_bShowPetQuick);
    }
    //宠物快捷信息设置按钮
    bool m_bShowPetQuick = true;
    void ShowPetQuickInfo(bool bShow)
    {
        MainBtn petRole = GetMainBtnByType(MainBtnDef.BtnType.BTNPETROLE);
        if (petRole != null)
        {
            petRole.gameObject.SetActive(bShow);
        }
        if(!bShow)
        {
            m_petInfo.Release();
        }

    }
}
