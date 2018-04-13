using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using table;
using GameCmd;
class PetSkillItem : UIGridBase
{
    SkillDatabase db;
    public SkillDatabase SkillData
    {
        set
        {
            db = value;
            RefreshItem();
        }
        get
        {
            return db;
        }
    }
    Transform mengban;
    Transform learn;
    Transform numTrans;
    protected override void OnAwake()
    {
        base.OnAwake();
        mengban = transform.Find("mengban");
        learn = transform.Find("learned");
        numTrans = transform.Find("Num");
    }
 
    void  OnEnable()
    {
        pataDataManager.ValueUpdateEvent += OnValueUpdateEventArgs;
    }
    void OnDisable()
    {
        pataDataManager.ValueUpdateEvent -= OnValueUpdateEventArgs;
    }
    void OnValueUpdateEventArgs(object obj, ValueUpdateEventArgs v)
    {
        RefreshItem();
    }
    public PetDataManager pataDataManager
    {
        get
        {
            return DataManager.Manager<PetDataManager>();
        }
    }
    CMResAsynSeedData<CMTexture> m_curIconAsynSeed = null;
    public void SetIcon(SkillDatabase db)
    {
        SkillData = db;
        string iconName = string.Empty;
        if(db != null)
        {
            iconName = db.iconPath;
        }
        Transform iconTrans = transform.Find("skillIcon");
        if(iconTrans == null)
        {
            iconTrans = transform.Find("icon");
        }
        if (iconTrans != null)
        {
            iconTrans.gameObject.SetActive(true);
            UITexture spr = iconTrans.GetComponent<UITexture>();
            if (spr != null)
            {
                UIManager.GetTextureAsyn(iconName, ref m_curIconAsynSeed, () =>
                {
                    if (null != spr)
                    {
                        spr.mainTexture = null;
                    }
                }, spr, false);
            }
        }
    }
    public override void Release(bool depthRelease = true)
    {
        base.Release(depthRelease);
        if(m_curIconAsynSeed != null)
        {
            m_curIconAsynSeed.Release();
            m_curIconAsynSeed = null;
        }
    }
    public override void SetHightLight(bool hightLight)
    {
        base.SetHightLight(hightLight);
        Transform spr = transform.Find("Sprite");
        if (spr != null)
        {
            spr.gameObject.SetActive(hightLight);
        }
    }

    void RefreshItem()
    {
        bool hasLearn = false;
        if (pataDataManager.CurPet != null)
        {
            List<PetSkillObj> objList = pataDataManager.CurPet.GetPetSkillList();
            for (int i = 0; i < objList.Count; i++)
            {
                var obj = objList[i];
                if(db != null)
                {
                    if (obj.id == db.wdID)
                    {
                        hasLearn = true;
                    }
                }
            }
        }


        if (learn != null)
        {
            if (hasLearn)
            {
                learn.gameObject.SetActive(true);
            }
            else
            {
                learn.gameObject.SetActive(false);
            }
        }
        int needNum = 0;
        uint itemID = 0;
        if(db != null)
        {
            itemID = pataDataManager.GetSkillBookID(db.wdID, out needNum);
        }
        int itemCount = DataManager.Manager<ItemManager>().GetItemNumByBaseId(itemID);
    
        if (numTrans == null)
            return;
      
        if (mengban == null)
            return;
        UILabel numLabel = numTrans.GetComponent<UILabel>();
        if (numLabel == null)
            return;
        if (itemCount == 0)
        {
            mengban.gameObject.SetActive(true);
            numLabel.text = ColorManager.GetColorString(ColorType.Red, itemCount.ToString());
        }
        else
        {
            numLabel.text = ColorManager.GetColorString(ColorType.White, itemCount.ToString());
            mengban.gameObject.SetActive(false);
        }

    }
}

