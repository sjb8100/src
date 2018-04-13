using UnityEngine;
using System;
using System.Collections.Generic;
using table;
using Cmd;
using Client;

partial class PetTujianItem
{
    private PetDataBase _db;
    bool bCotain = false;

    PetDataManager petDataManager
    {
        get
        {
            return DataManager.Manager<PetDataManager>();
        }
    }
    protected override void OnStart()
    {
        base.OnStart();
   
    }
    void OnValueUpdateEventArgs(object obj, ValueUpdateEventArgs v)
    {
        if (v != null)
        {
            if (v.key == PetDispatchEventString.AddPet.ToString())
            {
                UpdateData(_db);
            }
        }

    }

    protected override void OnEnable()
    {
        base.OnEnable();
        RegisterGlobalEvent(true);
    }

    protected override void OnDisable()
    {
        base.OnDisable();
        RegisterGlobalEvent(false);
    }

    private void RegisterGlobalEvent(bool register)
    {
        if (register)
        {
            Engine.Utility.EventEngine.Instance().AddEventListener((int)Client.GameEventID.UIEVENT_UPDATEITEM, OnGlobalUIEventHandler);
            petDataManager.ValueUpdateEvent += OnValueUpdateEventArgs;
        }
        else
        {
            petDataManager.ValueUpdateEvent -= OnValueUpdateEventArgs;
            Engine.Utility.EventEngine.Instance().RemoveEventListener((int)Client.GameEventID.UIEVENT_UPDATEITEM, OnGlobalUIEventHandler);
        }
    }

    private void OnGlobalUIEventHandler(int nEventID, object param)
    {
        switch(nEventID)
        {
            case (int)Client.GameEventID.UIEVENT_UPDATEITEM:
                {
                    if (null != param && (param is ItemDefine.UpdateItemPassData))
                    {
                        ItemDefine.UpdateItemPassData passData = param as ItemDefine.UpdateItemPassData;
                        if (null != _db && _db.fragmentID == passData.BaseId)
                        {
                            UpdatePetFragments();
                        }
                    }
                }
                break;
        }
    }

    private void UpdatePetFragments()
    {
        if (null == _db)
        {
            return;
        }
        int itemCount = DataManager.Manager<ItemManager>().GetItemNumByBaseId(_db.fragmentID);
        uint needNum = _db.fragmentNum;
        string str = itemCount + "/" + needNum;
        if (itemCount >= needNum)
        {
            str = StringUtil.GetNumNeedString(itemCount, needNum);
            m_label_Percent.color = Color.green;
        }
        else
        {
            m_label_Percent.color = Color.white;
        }
        float scale = itemCount * 1.0f / needNum;
        m_label_Percent.text = str;

        bool visible = (itemCount >= needNum);
        if (null != m_sprite_useable_mark && m_sprite_useable_mark.gameObject.activeSelf != visible)
        {
            m_sprite_useable_mark.gameObject.SetActive(visible);
        }
        m_slider_suipian_slider.value = scale;

        PetDataManager dm = DataManager.Manager<PetDataManager>();
        Dictionary<uint, IPet> petDic = dm.GetPetDic();
        bCotain = false;
        var iter = petDic.GetEnumerator();
        while(iter.MoveNext())
        {
            var pet = iter.Current;
            if (pet.Value.PetBaseID == _db.petID)
            {
                bCotain = true;
                break;
            }
        }

        if (dm.HasPossessPetList.Contains(_db.petID))
        {
            bCotain = true;
        }

        visible = !bCotain && (itemCount < needNum);
        if (null != m_sprite_nohave && m_sprite_nohave.gameObject.activeSelf != visible)
        {
            m_sprite_nohave.gameObject.SetActive(visible);
        }
    }

    public void UpdateData(PetDataBase db)
    {
        _db = db;
        if (db != null)
        {
            if (m_label_tujian_name == null)
                return;
            m_label_tujian_name.text = db.petName;
            m_label_xiedai_level.text = db.carryLevel.ToString();

            UpdatePetFragments();

            if (db.petQuality == 8)
            {
                m_sprite_shenshou_sign.gameObject.SetActive(true);
            }
            else
            {
                m_sprite_shenshou_sign.gameObject.SetActive(false);
            }
           // DataManager.Manager<UIManager>().SetSpriteDynamicIcon(m_sprite_icon, db.icon);
            UIManager.GetTextureAsyn(db.icon, ref m_curIconAsynSeed, () =>
            {
                if (null != m__icon)
                {
                    m__icon.mainTexture = null;
                }
            }, m__icon, false);
            //string bgSpriteName = ItemDefine.GetItemBorderIcon(db.flag);
            //DataManager.Manager<UIManager>().SetSpriteDynamicIcon(m_sprite_pingzhi_box, bgSpriteName);

            UIManager.GetQualityAtlasAsyn(db.flag, ref m_curQualityAsynSeed, () =>
            {
                if (null != m_sprite_pingzhi_box)
                {
                    m_sprite_pingzhi_box.atlas = null;
                }
            }, m_sprite_pingzhi_box);
            
            UIEventListener.Get(this.gameObject).onClick = OnClickBg;
        }
    }
    CMResAsynSeedData<CMTexture> m_curIconAsynSeed = null;
    CMResAsynSeedData<CMAtlas> m_curQualityAsynSeed = null;
    public override void Release(bool depthRelease = true)
    {
        base.Release(depthRelease);
        if (m_curIconAsynSeed != null)
        {
            m_curIconAsynSeed.Release(depthRelease);
            m_curIconAsynSeed = null;
        }
        if (m_curQualityAsynSeed != null)
        {
            m_curQualityAsynSeed.Release(depthRelease);
            m_curQualityAsynSeed = null;
        }
    }
    void OnClickBg(GameObject caster)
    {
        DataManager.Manager<UIPanelManager>().ShowPanel(PanelID.PetMarkPanel, panelShowAction: (pb) =>
        {
            if (null != pb && pb is PetMarkPanel)
            {
                PetMarkPanel panel = pb as PetMarkPanel;
                panel.InitPetDataBase(_db);
            }
        });
    }


}
