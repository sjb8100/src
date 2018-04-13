
//*************************************************************************
//	创建日期:	2017/2/7 星期二 14:32:02
//	文件名称:	PetQuickInfo
//   创 建 人:   zhudianyu	
//	版权所有:	zhudianyu
//	说    明:	
//*************************************************************************

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Client;
using table;
using GameCmd;

class PetQuickInfo : MonoBehaviour
{
    Transform m_transHpSlider;
    Transform m_transIcon;

    UISprite m_hpSlider;
    UITexture m_sprIcon;
    UILabel m_levelLabel;
    Transform m_levelBgTrans;
    UISprite m_spriteBg;

    Transform m_tranRedPoint;
    PetDataManager m_petData
    {
        get
        {
            return DataManager.Manager<PetDataManager>();
        }
    }
    void Awake()
    {
        //  Transform bg = transform.Find("InfoBg");
        Transform bg = transform;
        if (bg != null)
        {
            m_transHpSlider = bg.Find("hpslider");
            if (m_transHpSlider != null)
            {
                m_hpSlider = m_transHpSlider.GetComponent<UISprite>();

            }
            m_transIcon = bg.Find("peticon");
            if (m_transIcon != null)
            {
                m_sprIcon = m_transIcon.GetComponent<UITexture>();
            }
            Transform levelTrans = bg.Find("petlevel");
            if (levelTrans != null)
            {
                m_levelLabel = levelTrans.GetComponent<UILabel>();
            }
            Transform bgspr = bg.Find("peticonbg");
            if (bgspr)
            {
                m_spriteBg = bgspr.GetComponent<UISprite>();
            }
            m_levelBgTrans = bg.Find("levelbg");
            UIEventListener.Get(bg.gameObject).onClick = ShowPetSetting;
            m_tranRedPoint = bg.Find("redpoing");
        }
    }
    void Start()
    {
        if (m_hpSlider != null)
        {
            m_hpSlider.fillAmount = 1;
        }
        m_petData.ValueUpdateEvent += m_petData_ValueUpdateEvent;
        Init();

    }

    void ShowPetSetting(GameObject go)
    {
        DataManager.Manager<UIPanelManager>().ShowPanel(PanelID.PetPanel);
    }
    public void OnDestroy()
    {
        m_petData.ValueUpdateEvent -= m_petData_ValueUpdateEvent;
    }

    public void OnDisable()
    {
        Engine.Utility.EventEngine.Instance().RemoveEventListener((int)Client.GameEventID.ENTITYSYSTEM_HPUPDATE, OnEvent);
        Engine.Utility.EventEngine.Instance().RemoveEventListener((int)Client.GameEventID.UIEVENT_UPDATEITEM, OnGlobalUIEventHandler);
    }
    private void OnGlobalUIEventHandler(int eventType, object data)
    {
        switch (eventType)
        {

            case (int)Client.GameEventID.UIEVENT_UPDATEITEM:
                OnUpdateItem((ItemDefine.UpdateItemPassData)data);

                break;
        }
    }
    void OnUpdateItem(ItemDefine.UpdateItemPassData passData)
    {
        if (null == passData)
        {
            return;
        }
        uint qwThisId = passData.QWThisId;
        BaseItem bi = DataManager.Manager<ItemManager>().GetBaseItemByQwThisId(qwThisId);
        if (bi != null)
        {
            if(bi.BaseData != null)
            {
                if (bi.BaseData.subType == (uint)ItemDefine.ItemMaterialSubType.PetChips || bi.BaseData.subType == (uint)ItemDefine.ItemMaterialSubType.RideChips)
                {
                    ShowRed();
                }
            }
        
        }

    }
    public void OnEnable()
    {
        Engine.Utility.EventEngine.Instance().AddEventListener((int)Client.GameEventID.ENTITYSYSTEM_HPUPDATE, OnEvent);
        Engine.Utility.EventEngine.Instance().AddEventListener((int)Client.GameEventID.UIEVENT_UPDATEITEM, OnGlobalUIEventHandler);
        ShowRed();
    }
    void ShowRed()
    {
        if (m_petData == null)
        {
            Engine.Utility.Log.Error("m_petData is null");
            return;
        }
        bool bShow = false;
        if (m_petData.CurFightingPet != 0)
        {
            //已经出战
            bool hasLeftPoint = false;
            if (m_petData.CurPet == null)
            {
                return;
            }

            int maxPoint = m_petData.CurPet.GetProp((int)PetProp.MaxPoint);
            PetTalent attrPoint = m_petData.CurPet.GetAttrTalent();
            if (attrPoint == null)
            {
                return;
            }
            uint usePint = attrPoint.jingshen + attrPoint.liliang + attrPoint.minjie + attrPoint.zhili + attrPoint.tizhi;
            int leftPoint = maxPoint - (int)usePint;
            if (leftPoint == 0)
            {
                hasLeftPoint = false;
            }
            else
            {
                hasLeftPoint = true;
            }
            bShow = DataManager.Manager<PetDataManager>().IsHasPetFight && hasLeftPoint;
        }
        else
        {
            if (m_petData.GetPetDic() != null && m_petData.GetPetDic().Count > 0)
            {
                bShow = true;
            }
        }
        if (m_petData.GetCanComposePetList().Count > 0)
        {//有可合成的宠物 显示红点
            bShow = true;
        }
        if (m_tranRedPoint != null)
        {
            m_tranRedPoint.gameObject.SetActive(bShow);
        }
    }
    void OnEvent(int eventID, object param)
    {
        if (eventID == (int)Client.GameEventID.ENTITYSYSTEM_HPUPDATE)
        {
            stPropUpdate prop = (stPropUpdate)param;
            if (prop.nPropIndex != (int)CreatureProp.Hp)
                return;
            SetPetHP(prop.uid);
        }
    }
    void SetPetHP(long uid = 0)
    {
        if (m_petData.CurFightingPet != 0)
        {
            INPC npc = m_petData.GetNpcByPetID(m_petData.CurFightingPet);
            if (npc != null)
            {
                if (uid == 0 || uid == npc.GetUID())
                {
                    int curHP = npc.GetProp((int)CreatureProp.Hp);
                    int maxHP = npc.GetProp((int)CreatureProp.MaxHp);
                    if (maxHP != 0)
                    {
                        float v = curHP * 1.0f / maxHP;
                        m_hpSlider.fillAmount = v;
                    }
                }

            }
            else
            {
                m_hpSlider.fillAmount = 1;
            }
        }
    }
    void m_petData_ValueUpdateEvent(object sender, ValueUpdateEventArgs e)
    {
        if (e != null)
        {
            if (e.key == PetDispatchEventString.ChangeFight.ToString())
            {
                Init();
            }
            if (e.key == PetDispatchEventString.DeletePet.ToString())
            {
                Init();
            }
            if (e.key == PetDispatchEventString.AddPet.ToString())
            {
                Init();
            }
            else if (e.key == PetDispatchEventString.PetRefreshProp.ToString())
            {
                SetPetLevel();
            }
        }
    }
    CMResAsynSeedData<CMTexture> m_curIconAsynSeed = null;
    CMResAsynSeedData<CMAtlas> m_curBgAsynSeed = null;
    void SetPetIcon()
    {
        string spName = "";
        string borderName = "";
        if (m_petData.CurFightingPet != 0)
        {
            IPet pet = m_petData.GetPetByThisID(m_petData.CurFightingPet);
            if (pet != null)
            {
                uint baseID = pet.PetBaseID;
                PetDataBase pdb = GameTableManager.Instance.GetTableItem<PetDataBase>(baseID);
                if (pdb != null)
                {
                    spName = pdb.icon;
                    borderName = ItemDefine.GetItemBgYuanBorderIconByItemID(pdb.ItemID);              
                }
            }
            m_hpSlider.gameObject.SetActive(true);
        }
        else
        {
            m_hpSlider.gameObject.SetActive(false);
        }

        if (m_sprIcon != null)
        {
            spName = UIManager.BuildCircleIconName(spName);
            UIManager.GetTextureAsyn(spName, ref m_curIconAsynSeed, () =>
            {
                if (null != m_sprIcon)
                {
                    m_sprIcon.mainTexture = null;
                }
            }, m_sprIcon, false);
        }
        if (m_spriteBg != null)
        {         
            UIManager.GetAtlasAsyn(borderName, ref m_curBgAsynSeed, () =>
            {
                if (null != m_spriteBg)
                {
                    m_spriteBg.atlas = null;
                }
            }, m_spriteBg, false);
            m_spriteBg.gameObject.SetActive(!string.IsNullOrEmpty(spName));
        }
    }
    void SetPetLevel()
    {

        if (m_petData.CurFightingPet != 0)
        {
            IPet pet = m_petData.GetPetByThisID(m_petData.CurFightingPet);
            if (pet != null)
            {
                m_levelLabel.gameObject.SetActive(true);
                m_levelBgTrans.gameObject.SetActive(true);
                m_levelLabel.text = pet.GetProp((int)CreatureProp.Level).ToString();
            }
        }
        else
        {
            m_levelLabel.gameObject.SetActive(false);
            m_levelBgTrans.gameObject.SetActive(false);
        }
    }
    public void Init()
    {
        SetPetIcon();
        SetPetHP();
        SetPetLevel();
        ShowRed();
    }
    public void Release(bool depthRelease = true)
    {
        if (m_curIconAsynSeed != null)
        {
            m_curIconAsynSeed.Release(depthRelease);
            m_curIconAsynSeed = null;
        }
        if (m_curBgAsynSeed != null)
        {
            m_curBgAsynSeed.Release(depthRelease);
            m_curBgAsynSeed = null;
        }
    }
}
