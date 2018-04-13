
//*************************************************************************
//	创建日期:	2017/8/10 星期四 15:21:08
//	文件名称:	PetLineUPItemInfo
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

class PetLineUPItemInfo : MonoBehaviour
{
    UITexture m_PetIcon;
    UISprite m_hpSpr;
    UILabel m_petName;
    UILabel m_petLv;
    UILabel m_cdLabel;
    Transform m_transStatus;
    Transform m_transFight;
    Transform m_transDead;
    uint m_petID = 0;
    PetDataManager m_petData
    {
        get
        {
            return DataManager.Manager<PetDataManager>();
        }
    }
    void Awake()
    {
        m_PetIcon = transform.Find("icon").GetComponent<UITexture>();
        m_hpSpr = transform.Find("hpslider").GetComponent<UISprite>();
        m_petName = transform.Find("name").GetComponent<UILabel>();
        m_petLv = transform.Find("level").GetComponent<UILabel>();
        m_transFight = transform.Find("status/embattled");
        m_transDead = transform.Find("status/dead");
        m_cdLabel = transform.Find("cd").GetComponent<UILabel>();
      
      UIEventListener.Get(this.gameObject).onClick = OnFightClick;
        
    }
    public void InitLineUpItem(uint petID)
    {
        if(petID != 0)
        {
            m_petID = petID;
          
            IPet pet = m_petData.GetPetByThisID(petID);
            if(pet != null)
            {
                SetHP();
                PetDataBase pdb = GameTableManager.Instance.GetTableItem<PetDataBase>(pet.PetBaseID);
                if(pdb != null)
                {
                    SetIcon(pdb.icon);
                }

                SetPetName(m_petData.GetPetName(pet));
                SetLevel(m_petData.GetPetLvelStr(petID));
            }
            else
            {
                SetHP();
                SetIcon("");
                SetPetName("");
                SetLevel("");
            }
        }
        else
        {
            SetHP();
            SetIcon("");
            SetPetName("");
            SetLevel("");
        }
        RefreshStatus();
    }
    void RefreshStatus()
    {
        if (m_petID == m_petData.CurFightingPet && m_petID != 0)
        {
            ShowFight(true);
        }
        else
        {
            ShowFight(false);
        }
        if(m_petData.GetRelieveCDTime(m_petID) > 0)
        {
            ShowDead(true);
        }
        else
        {
            ShowDead(false);
        }
    }
    void OnEnable()
    {
        RegisterEvents(true);
    }

    public void OnDisable()
    {
        RegisterEvents(false);
    }
    void OnEvent(int eventID, object param)
    {
        if (eventID == (int)Client.GameEventID.ENTITYSYSTEM_HPUPDATE)
        {
            stPropUpdate prop = (stPropUpdate)param;
            if (prop.nPropIndex != (int)CreatureProp.Hp)
                return;
            SetHP(prop.uid);
        }
    }
    void RegisterEvents(bool bReg)
    {
        if(bReg)
        {
            m_petData.ValueUpdateEvent += m_petData_ValueUpdateEvent;
            Engine.Utility.EventEngine.Instance().AddEventListener((int)Client.GameEventID.ENTITYSYSTEM_HPUPDATE, OnEvent);
        }
        else
        {
            m_petData.ValueUpdateEvent -= m_petData_ValueUpdateEvent;

            Engine.Utility.EventEngine.Instance().RemoveEventListener((int)Client.GameEventID.ENTITYSYSTEM_HPUPDATE, OnEvent);
        }
    }

    void m_petData_ValueUpdateEvent(object sender, ValueUpdateEventArgs e)
    {
        if(e.key == PetDispatchEventString.ChangeFight.ToString())
        {
            RefreshStatus();
        }
       
    }
    void SetIcon(string iconname)
    {
        iconname = UIManager.GetIconName(iconname, true);
        if (m_PetIcon != null)
        {
            if (string.IsNullOrEmpty(iconname))
            {
                //m_PetIcon.spriteName = "";
                m_PetIcon.mainTexture = null;
            }
            else
            {
               // DataManager.Manager<UIManager>().SetSpriteDynamicIcon(m_PetIcon, iconname,false,true);
                UIManager.GetTextureAsyn(iconname, ref m_curIconAsynSeed, () =>
                {
                    if (null != m_PetIcon)
                    {
                        m_PetIcon.mainTexture = null;
                    }
                }, m_PetIcon, false);
            }
        }
    }
    CMResAsynSeedData<CMTexture> m_curIconAsynSeed = null;
    public void Release(bool depthRelease = true)
    {
        if (m_curIconAsynSeed != null)
        {
            m_curIconAsynSeed.Release(depthRelease);
            m_curIconAsynSeed = null;
        }
    }
    void ShowFight(bool bShow)
    {
        if(m_transFight != null)
        {
            m_transFight.gameObject.SetActive(bShow);
        }
    }
    void ShowDead(bool bShow)
    {
        if(m_transDead != null)
        {
            m_transDead.gameObject.SetActive(bShow);
        }
    }
    void SetLevel(string lv)
    {
        if(m_petLv != null)
        {
            m_petLv.text = lv;
        }
    }
    void SetPetName(string petName)
    {
        if(m_petName != null)
        {
            m_petName.text = petName;
        }
    }
    int m_nFrameNum = 0;
    void Update()
    {
        m_nFrameNum++;
        if (m_nFrameNum > 100000)
        {
            m_nFrameNum = 0;
        }
        if (m_nFrameNum % 20 != 0)
        {
            return;
        }
        if(m_cdLabel == null )
        {
            return;
        }
        if(m_petID == 0)
        {
            return;
        }
        int time = m_petData.GetRelieveCDTime(m_petID);
        if(time > 0)
        {
            if(m_cdLabel.gameObject.activeSelf)
            {
                m_cdLabel.text = time.ToString();
            }
            else
            {
                m_cdLabel.text = time.ToString();
                m_cdLabel.gameObject.SetActive(true);
            }
        }
        else
        {
            if(m_cdLabel.gameObject.activeSelf)
            {
                m_cdLabel.gameObject.SetActive(false);
            }
        }
    }
    void SetHP(long uid = 0)
    {
        if(m_petID == 0)
        {
            m_hpSpr.fillAmount = 1;
            return;
        }
        INPC npc = m_petData.GetNpcByPetID(m_petID);
        if (npc != null)
        {
            if (uid == 0 || uid == npc.GetUID())
            {
                int curHP = npc.GetProp((int)CreatureProp.Hp);
                int maxHP = npc.GetProp((int)CreatureProp.MaxHp);
                if (maxHP != 0)
                {
                    float v = curHP * 1.0f / maxHP;
                    m_hpSpr.fillAmount = v;
                }
            }

        }
    }
    void OnFightClick(GameObject go)
    {
       if(m_petID == 0)
       {
           return;
       }
        IPet pet = m_petData.GetPetByThisID(m_petID);
        if (pet != null)
        {
            int life = pet.GetProp((int)PetProp.Life);
            if (life <= 0)
            {
                TipsManager.Instance.ShowLocalFormatTips(LocalTextType.Pet_Age_zhanhunshoumingbuzuwufachuzhan);
                //ShowTips(108017);
                return;
            }
            int cd = m_petData.GetPetFightCDTime(pet.GetID());
            if (cd > 0)
            {
                TipsManager.Instance.ShowTipsById(108020, cd);
                return;
            }
            stUseFightPetUserCmd_CS cmd = new stUseFightPetUserCmd_CS();
            cmd.id = pet.GetID();
            NetService.Instance.Send(cmd);
        }
    }
}
