using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using table;
using Client;

enum PetLineUpPos
{
    None = 0,
    Up,
    Down,
}
partial class PetEmbatteGrid : MonoBehaviour
{
    UITexture m_PetIcon;
    UISprite m_quaSpr;
    UILabel m_labelPower;
    Action<PetLineUpPos, int, uint> m_callBack;
    Transform m_transHighLight;
    Transform m_flag;
    int m_index;
    PetLineUpPos m_pos = PetLineUpPos.None;

    PetDataManager m_petData
    {
        get
        {
            return DataManager.Manager<PetDataManager>();
        }
    }

    uint m_uPetID = 0;
    void Awake()
    {
        m_PetIcon = transform.Find("Head/icon").GetComponent<UITexture>();
        if (m_PetIcon != null)
        {
            UIEventListener.Get(m_PetIcon.gameObject).onClick = OnGridClick;
            m_quaSpr = m_PetIcon.transform.Find("pingzhi_box").GetComponent<UISprite>();
        }
        m_labelPower = transform.Find("Power/powerNum").GetComponent<UILabel>();
        m_transHighLight = transform.Find("HighLight");
        m_flag = transform.Find("lineflag");
    }
    public void InitPos(PetLineUpPos pos, int index)
    {
        m_pos = pos;
        m_index = index;
    }
    public void InitPetEmbattleGrid(uint petID, Action<PetLineUpPos, int, uint> callBack = null)
    {
        m_callBack = callBack;
        m_uPetID = petID;
        SetFlag(false);
        if (m_uPetID == 0)
        {
            SetIcon("");
            SetPower("");
            SetQua(0);
        }
        else
        {
            IPet pet = m_petData.GetPetByThisID(m_uPetID);
            if (pet != null)
            {
                uint baseID = pet.PetBaseID;
                PetDataBase pdb = GameTableManager.Instance.GetTableItem<PetDataBase>(baseID);
                if (pdb != null)
                {
                    SetIcon(pdb.icon);
                    SetQua(pdb.petQuality);
                }
                int power = pet.GetProp((int)FightCreatureProp.Power);
                SetPower(power.ToString());
            }
        }
    }
    void SetQua(uint qua)
    {
        if (m_quaSpr != null)
        {
        
            UIManager.GetQualityAtlasAsyn(qua, ref m_curQualityAsynSeed, () =>
            {
                if (null != m_quaSpr)
                {
                    m_quaSpr.atlas = null;
                }
            }, m_quaSpr);
        }
    }
    CMResAsynSeedData<CMTexture> m_curIconAsynSeed = null;
    CMResAsynSeedData<CMAtlas> m_curQualityAsynSeed = null;
    void SetIcon(string iconname)
    {
        if (m_PetIcon != null)
        {
            if (string.IsNullOrEmpty(iconname))
            {
                m_PetIcon.mainTexture = null;
            }
            else
            {
                UIManager.GetTextureAsyn(iconname, ref m_curIconAsynSeed, () =>
                {
                    if (null != m_PetIcon)
                    {
                        m_PetIcon.mainTexture = null;
                    }
                }, m_PetIcon);
            }
        }
    }
    public void ReleaseGrid(bool depth = true)
    {
        if(m_curIconAsynSeed != null)
        {
            m_curIconAsynSeed.Release(depth);
            m_curIconAsynSeed = null;
        }
        if(m_curQualityAsynSeed != null)
        {
            m_curQualityAsynSeed.Release(depth);
            m_curQualityAsynSeed = null;
        }
    }
    void SetPower(string text)
    {
        if (m_labelPower != null)
        {
            m_labelPower.text = text;
        }
    }
    public void SetFlag(bool bShow)
    {
        if (m_flag != null)
        {
            m_flag.gameObject.SetActive(bShow);
        }
    }
    public void SetHighLight(bool bHigh)
    {
        if (m_transHighLight != null)
        {
            m_transHighLight.gameObject.SetActive(bHigh);
        }
    }
    void OnGridClick(GameObject go)
    {
        if (m_pos == PetLineUpPos.Up)
        {
            OnUpClick();
        }
        if (m_pos == PetLineUpPos.Down)
        {
            OnDownClick();
        }
    }
    void OnUpClick()
    {
        if (m_uPetID == 0)
        {
            return;
        }
        if (m_petData.PetQuickList.Count < 3)
        {
            m_petData.RemovelineUp(m_uPetID);
        }
        else
        {
            if (m_callBack != null)
            {
                m_callBack(m_pos, m_index, m_uPetID);
            }
        }
    }
    void OnDownClick()
    {
        if (m_uPetID == 0)
        {
            return;
        }
        if (m_petData.PetQuickList.Count < 3)
        {
            m_petData.AddLineUp(m_uPetID);
        }
        else
        {
            if (m_callBack != null)
            {
                m_callBack(m_pos, m_index, m_uPetID);
            }
        }
    }

}
