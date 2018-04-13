
//*************************************************************************
//	创建日期:	2017/8/10 星期四 16:47:53
//	文件名称:	PetPanel_ChuanCheng
//   创 建 人:   zhudianyu	
//	版权所有:	zhudianyu
//	说    明:	
//*************************************************************************

using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using Client;
using Engine.Utility;
using table;
using GameCmd;


partial class PetPanel
{
    struct PetInheritJumpData
    {
        public uint oldPetID;
        public uint newPetID;
        public bool bInheritExp;
        public bool bInheritSkill;
        public bool bInheritXiuwei;
    }
    enum PetInheritPos
    {
        Old = 1,
        New = 2,
    }
    uint m_uOldPetBaseID = 0;
    uint m_uNewPetBaseID = 0;
    uint m_uPetOldThisID = 0;
    uint m_uPetNewThisID = 0;
    void InitInHerit()
    {
        ResetNewPet();
        ResetOldPet();
        ResetCostItem();
        petDataManager.ResetInheritData();
        ResetGou();
        UIEventListener.Get(m_sprite_ptchuanchengjingyan.gameObject).onClick = OnInheritExp;
        UIEventListener.Get(m_sprite_ptchuanchengjineng.gameObject).onClick = OnInheritSkill;
        UIEventListener.Get(m_sprite_ptchuanchengxiuwei.gameObject).onClick = OnInheritXiuWei;
        petDataManager.ClearInheritPet();
    }
    void OnJumpHerit(PetInheritJumpData data)
    {
        if (data.oldPetID == 0)
        {
            return;
        }
        InitOldPet(data.oldPetID);
        if (data.newPetID == 0)
        {
            return;
        }
        InitNewPet(data.newPetID);
        petDataManager.bInheritXiuwei = data.bInheritXiuwei;
        petDataManager.bInheritSkill = data.bInheritSkill;
        petDataManager.bInheritExp = data.bInheritExp;
        DoInheritExp();
        DoInheritXiuWei();
        DoInhertSkill();

    }
    void ResetGou()
    {
        m_sprite_inheritskillgou.gameObject.SetActive(false);
        m_sprite_inheritxiuweigou.gameObject.SetActive(false);
        m_sprite_inhertexpgou.gameObject.SetActive(false);
    }
    #region OnBtn
    string GetLocalText(LocalTextType type)
    {
        return DataManager.Manager<TextManager>().GetLocalText(type);
    }
    void OnInheritExp(GameObject go)
    {
        int lv = 0;
        if (!CheckHasOldPet())
        {
            return;
        }

     

        IPet pet = petDataManager.GetInheritPet((int)PetInheritPos.Old);
        if (pet == null)
        {
            return;
        }

        int expLv = pet.GetProp((int)CreatureProp.Level);
        int exp = pet.GetProp((int)PetProp.LevelExp);
        if (exp == 0 && expLv == 1)
        {
            TipsManager.Instance.ShowTips(LocalTextType.Pet_Next_cailiaozhenshoumeiyoujingyanchuancheng);

            return;
        }
        if (GetInheritLevel(out lv))
        {
            petDataManager.bInheritExp = !petDataManager.bInheritExp;
            DoInheritExp();
        }
        else
        {
            if (!petDataManager.bInheritExp)
            {
                TipsManager.Instance.ShowTipWindow(TipWindowType.CancelOk, GetLocalText(LocalTextType.Pet_Next_chuanchenghoumubiaozhenshouyoujingyanyichu),
              () =>
              {
                  petDataManager.bInheritExp = !petDataManager.bInheritExp;
                  DoInheritExp();
              });
            }
            else
            {
                petDataManager.bInheritExp = !petDataManager.bInheritExp;
                DoInheritExp();
            }

        }
    }

    void OnInheritSkill(GameObject go)
    {
        if (!CheckHasOldPet())
        {
            return;
        }

      

        IPet pet = petDataManager.GetInheritPet((int)PetInheritPos.Old);
        if (pet == null)
        {
            Engine.Utility.Log.Error("pet 为 null !!!");
            return;
        }

        int exp = pet.GetPetSkillList().Count;
        if (exp == 0)
        {
            TipsManager.Instance.ShowTips(LocalTextType.Pet_Next_cailiaozhenshoumeiyoujinengchuancheng);
            return;
        }
        if (!petDataManager.bInheritSkill)
        {
            IPet newpet = petDataManager.GetInheritPet((int)PetInheritPos.New);
            int skillNum = newpet.GetPetSkillList().Count;
            if(skillNum > 0)
            {
                TipsManager.Instance.ShowTipWindow(TipWindowType.CancelOk, GetLocalText(LocalTextType.Pet_Next_mubiaozhenshoudejinengjiangtihuancailiaozhenshou), () =>
                {
                    petDataManager.bInheritSkill = !petDataManager.bInheritSkill;
                    DoInhertSkill();
                });
            }
            else
            {
                petDataManager.bInheritSkill = !petDataManager.bInheritSkill;
                DoInhertSkill();
            }
        
        }
        else
        {
            petDataManager.bInheritSkill = !petDataManager.bInheritSkill;
            DoInhertSkill();
        }

    }

    void OnInheritXiuWei(GameObject go)
    {
        if (!CheckHasOldPet())
        {
            return;
        }

       

        IPet pet = petDataManager.GetInheritPet((int)PetInheritPos.Old);
        if (pet == null)
        {
            Engine.Utility.Log.Error("pet 为 null !!!");
            return;
        }

        int yinLv = pet.GetProp((int)PetProp.YinHunLevel);
        int exp = pet.GetProp((int)PetProp.YinHunExp);
        if (exp == 0 && yinLv == 0)
        {
            TipsManager.Instance.ShowTips(LocalTextType.Pet_Next_cailiaozhenshoudexiuweiyouyichu);
            return;
        }
        if (!petDataManager.bInheritXiuwei)
        {
            int yinhunLv = 0;
            if (GetInheritXiuWeiLevel(out yinhunLv))
            {
                petDataManager.bInheritXiuwei = !petDataManager.bInheritXiuwei;
                DoInheritXiuWei();
            }
            else
            {
                TipsManager.Instance.ShowTipWindow(TipWindowType.CancelOk,
                    GetLocalText(LocalTextType.Pet_Next_chuanchenghoumubiaozhenshouyouxiuweiyichu), () =>
                    {
                        petDataManager.bInheritXiuwei = !petDataManager.bInheritXiuwei;
                        DoInheritXiuWei();
                    });
            }

        }
        else
        {
            petDataManager.bInheritXiuwei = !petDataManager.bInheritXiuwei;
            DoInheritXiuWei();
        }


    }
    #endregion

    #region dobtn
    void DoInheritExp()
    {

        m_sprite_inhertexpgou.gameObject.SetActive(petDataManager.bInheritExp);

        RefreshInfo();
    }
    void DoInhertSkill()
    {

        m_sprite_inheritskillgou.gameObject.SetActive(petDataManager.bInheritSkill);


        RefreshInfo();
    }
    void DoInheritXiuWei()
    {

        m_sprite_inheritxiuweigou.gameObject.SetActive(petDataManager.bInheritXiuwei);

        if (petDataManager.bInheritXiuwei)
        {

            InitNewPet(m_uPetNewThisID);
        }

        RefreshInfo();
    }
    PetInHeritDataBase GetInHeritData(uint petBaseID)
    {
        PetDataBase pdb = GameTableManager.Instance.GetTableItem<PetDataBase>(petBaseID);
        if (pdb == null)
        {
            Log.Error("petdb is null id is {0}", petBaseID);
            return null;
        }
        PetInHeritDataBase db = GameTableManager.Instance.GetTableItem<PetInHeritDataBase>(pdb.petQuality);
        if (db == null)
        {
            Log.Error("PetInHeritDataBase is null id is {0}", pdb.petQuality);
            return null;
        }
        return db;
    }
    void CalCulate()
    {
        PetInHeritDataBase db = GetInHeritData(m_uNewPetBaseID);
        if (db == null)
        {
            return;
        }
        petDataManager.InheritCostItemNum = 0;
        petDataManager.InHeritCostMoney = 0;
        if (petDataManager.bInheritXiuwei)
        {
            petDataManager.InheritCostItemNum += (int)db.yinhunInheritNum;
            petDataManager.InHeritCostMoney += (int)db.yinhunInheritMoney;

        }
        if (petDataManager.bInheritSkill)
        {
            petDataManager.InheritCostItemNum += (int)db.skillInheritNum;
            petDataManager.InHeritCostMoney += (int)db.skillInheritMoney;
        }
        if (petDataManager.bInheritExp)
        {
            petDataManager.InheritCostItemNum += (int)db.expInheritNum;
            petDataManager.InHeritCostMoney += (int)db.expInheritMoney;
        }
    }
    CMResAsynSeedData<CMAtlas> m_curInHeritIconAsynSeed = null;
    CMResAsynSeedData<CMTexture> m_inheritNewIconAsynSeed = null;
    CMResAsynSeedData<CMAtlas> m_inheritNewQuaAsynSeed = null;
    CMResAsynSeedData<CMTexture> m_inheritOldIconAsynSeed = null;
    CMResAsynSeedData<CMAtlas> m_inheritOldQuaAsynSeed = null;
    void OnInheritUIRelease()
    {
        if(null != m_curInHeritIconAsynSeed)
        {
            m_curInHeritIconAsynSeed.Release();
            m_curInHeritIconAsynSeed = null;
        }
        if (null != m_inheritNewIconAsynSeed)
        {
            m_inheritNewIconAsynSeed.Release();
            m_inheritNewIconAsynSeed = null;
        }
        if (null != m_inheritNewQuaAsynSeed)
        {
            m_inheritNewQuaAsynSeed.Release();
            m_inheritNewQuaAsynSeed = null;
        }
        if (null != m_inheritOldIconAsynSeed)
        {
            m_inheritOldIconAsynSeed.Release();
            m_inheritOldIconAsynSeed = null;
        }
        if (null != m_inheritOldQuaAsynSeed)
        {
            m_inheritOldQuaAsynSeed.Release();
            m_inheritOldQuaAsynSeed = null;
        }
    }
    void RefreshInfo()
    {
        CalCulate();
        PetInHeritDataBase db = GetInHeritData(m_uNewPetBaseID);
        if (db == null)
        {
            return;
        }
        uint costItem = db.costItem;
        int num = DataManager.Manager<ItemManager>().GetItemNumByBaseId(costItem);
        m_label_ChuanChengCommon_number.text = StringUtil.GetNumNeedString(num, petDataManager.InheritCostItemNum);
        UIItem.AttachParent(m_sprite_ChuanChengCommon_icon_di.transform, costItem, 0, InheritGetWayCallBack, true, (uint)petDataManager.InheritCostItemNum);
       // DataManager.Manager<UIManager>().SetSpriteDynamicIcon(m_sprite_inheritMoneySpr, MainPlayerHelper.GetMoneyIconByType((int)db.moneyType));
        UIManager.GetAtlasAsyn(MainPlayerHelper.GetMoneyIconByType((int)db.moneyType), ref m_curInHeritIconAsynSeed, () =>
        {
            if (null != m_sprite_inheritMoneySpr)
            {
                m_sprite_inheritMoneySpr.atlas = null;
            }
        }, m_sprite_inheritMoneySpr, false);

        m_label_ChuanChengCommon_xiaohaogold.text = petDataManager.InHeritCostMoney.ToString();
        if (MainPlayerHelper.IsHasEnoughMoney(db.moneyType, (uint)petDataManager.InHeritCostMoney, false))
        {
            m_label_ChuanChengCommon_xiaohaogold.color = Color.black;
        }
        else
        {
            m_label_ChuanChengCommon_xiaohaogold.color = Color.red;
        }
    }
    #endregion
    bool CheckHasOldPet()
    {
        if (!petDataManager.ContainInheritPos((int)PetInheritPos.Old))
        {
            TipsManager.Instance.ShowTips(LocalTextType.Pet_Commond_meiyouzhenshou);
            return false;
        }
        if (!petDataManager.ContainInheritPos((int)PetInheritPos.New))
        {
            TipsManager.Instance.ShowTips(LocalTextType.Pet_Commond_meiyouzhenshou);
            return false;
        }
        return true;
    }
    void OnInheritChangePet()
    {
        if (petDataManager.CurPet == null)
        {
            return;
        }
        if (m_nRightToggleIndex != (int)TabMode.ChuanCheng)
        {
            return;
        }
        if (!m_trans_ChuanChengContent.gameObject.activeSelf)
        {
            return;
        }
        uint petID = petDataManager.CurPet.GetID();

        if (petDataManager.ContainInheritPet(petID))
        {
            return;
        }
        if (!petDataManager.ContainInheritPos((int)PetInheritPos.Old))
        {

            InitOldPet(petID);
            return;
        }
        if (!petDataManager.ContainInheritPos((int)PetInheritPos.New))
        {


            InitNewPet(petID);
            return;
        }
    }
   
    void InitNewPet(uint petID)
    {
        m_uPetNewThisID = petID;
        petDataManager.AddInhertPet((int)PetInheritPos.New, petID);
        IPet pet = petDataManager.GetPetByThisID(petID);
        if (pet != null)
        {
            uint baseID = pet.PetBaseID;
            m_uNewPetBaseID = baseID;
            PetDataBase pdb = GameTableManager.Instance.GetTableItem<PetDataBase>(baseID);
            if (pdb != null)
            {
                m_label_New_name.text = petDataManager.GetPetName(pet);
               // DataManager.Manager<UIManager>().SetSpriteDynamicIcon(m_sprite_New_icon, pdb.icon);
                UIManager.GetTextureAsyn(pdb.icon, ref m_inheritNewIconAsynSeed, () =>
                {
                    if (null != m__New_icon)
                    {
                        m__New_icon.mainTexture = null;
                    }
                }, m__New_icon, false);

                string bgSpriteName = ItemDefine.GetItemFangBorderIconByItemID(pdb.ItemID);
                //DataManager.Manager<UIManager>().SetSpriteDynamicIcon(m_sprite_NewIconBox, bgSpriteName);
                UIManager.GetAtlasAsyn(bgSpriteName, ref m_inheritNewQuaAsynSeed, () =>
                {
                    if (null != m_sprite_NewIconBox)
                    {
                        m_sprite_NewIconBox.atlas = null;
                    }
                }, m_sprite_NewIconBox, false);
                int NewLv = pet.GetProp((int)CreatureProp.Level);
                m_label_New_level_Before.text = NewLv.ToString();
                int lv = 0;
                GetInheritLevel(out lv);
                m_label_New_level_After.text = lv.ToString();
                IPet oldpet = petDataManager.GetInheritPet((int)PetInheritPos.Old);
                if (oldpet != null)
                {
                    m_label_New_skill_After.text = oldpet.GetPetSkillList().Count.ToString();
                }
                m_label_New_skill_Before.text = pet.GetPetSkillList().Count.ToString();
                m_label_New_xiuwei_Before.text = pet.GetProp((int)PetProp.YinHunLevel).ToString();
                int yinhunLv = 0;
                GetInheritXiuWeiLevel(out yinhunLv);
                m_label_New_xiuwei_After.text = yinhunLv.ToString();
              
                m_btn_btn_New_delete.gameObject.SetActive(true);

            }
        }

    }
    bool GetInheritXiuWeiLevel(out int level)
    {
        level = 0;
        IPet oldpet = petDataManager.GetInheritPet((int)PetInheritPos.Old);
        if (oldpet == null)
        {
            return true;
        }
        IPet newpet = petDataManager.GetInheritPet((int)PetInheritPos.New);
        if (newpet == null)
        {
            return true;
        }
        int oldPetLv = oldpet.GetProp((int)PetProp.YinHunLevel);
        uint oldExp = (uint)oldpet.GetProp((int)PetProp.YinHunExp);

        for (int i = 0; i < oldPetLv; i++)
        {

            PetYinHunDataBase udb = GameTableManager.Instance.GetTableItem<PetYinHunDataBase>((uint)i);
            if (udb != null)
            {
                oldExp += udb.lingQiMax;
            }
        }

        int newPetLv = newpet.GetProp((int)PetProp.YinHunLevel);
        uint newExp = (uint)newpet.GetProp((int)PetProp.YinHunExp);
        newExp += oldExp;
        level = newPetLv;
        while (newExp > 0)
        {
            PetYinHunDataBase udb = GameTableManager.Instance.GetTableItem<PetYinHunDataBase>((uint)level);
            if (udb != null)
            {
                if (newExp >= udb.lingQiMax)
                {
                    level += 1;
                    newExp -= udb.lingQiMax;
                }
                else
                {
                    break;
                }
            }
            else
            {
                level -= 1;
                return false;
            }
        }
        return true;
    }
    bool GetInheritLevel(out int level)
    {
        level = 0;
        IPet oldpet = petDataManager.GetInheritPet((int)PetInheritPos.Old);
        if (oldpet == null)
        {
            return true;
        }
        IPet newpet = petDataManager.GetInheritPet((int)PetInheritPos.New);
        if (newpet == null)
        {
            return true;
        }
        int oldPetLv = oldpet.GetProp((int)CreatureProp.Level);
        uint oldExp = (uint)oldpet.GetProp((int)PetProp.LevelExp);

        for (int i = 1; i < oldPetLv; i++)
        {
            PetUpGradeDataBase udb = GameTableManager.Instance.GetTableItem<PetUpGradeDataBase>((uint)i, (int)oldpet.PetBaseID);
            if (udb != null)
            {
                oldExp += udb.upGradeExp;
            }
        }

        int newPetLv = newpet.GetProp((int)CreatureProp.Level);
        uint newExp = (uint)newpet.GetProp((int)PetProp.LevelExp);
        newExp += oldExp;
        level = newPetLv;
        int playerLv = MainPlayerHelper.GetPlayerLevel();
        int newYinLv = newpet.GetProp((int)PetProp.YinHunLevel);
        if (petDataManager.bInheritXiuwei)
        {
            GetInheritXiuWeiLevel(out newYinLv);
        }
        int maxLv = playerLv + newYinLv;
        maxLv = maxLv > level ? maxLv : level;
        int tempLv = level;
        while (newExp > 0)
        {
            PetUpGradeDataBase udb = GameTableManager.Instance.GetTableItem<PetUpGradeDataBase>((uint)tempLv, (int)newpet.PetBaseID);
            if (udb != null)
            {
                if (newExp >= udb.upGradeExp)
                {
                    tempLv += 1;
                    newExp -= udb.upGradeExp;
                }
                else
                {
                    break;
                }
            }
            else
            {
                tempLv -= 1;

                level = tempLv > maxLv ? maxLv : tempLv;
                return false;
            }
        }

        level = tempLv > maxLv ? maxLv : tempLv;
        return true;
    }
    void ResetNewPet()
    {
        m_label_New_name.text = string.Empty;
       // m_sprite_New_icon.spriteName = "";
        m__New_icon.mainTexture = null;
       // DataManager.Manager<UIManager>().SetSpriteDynamicIcon(m_sprite_NewIconBox, 1);
        UIManager.GetQualityAtlasAsyn(1, ref m_inheritNewQuaAsynSeed, () =>
        {
            if (null != m_sprite_NewIconBox)
            {
                m_sprite_NewIconBox.atlas = null;
            }
        }, m_sprite_NewIconBox, false);
        m_btn_btn_New_delete.gameObject.SetActive(false);
        m_label_New_level_Before.text = string.Empty;
        m_label_New_level_After.text = string.Empty;
        m_label_New_skill_Before.text = string.Empty;
        m_label_New_skill_After.text = string.Empty;
        m_label_New_xiuwei_Before.text = string.Empty;
        m_label_New_xiuwei_After.text = string.Empty;
    }
  
    void InitOldPet(uint petID)
    {
        m_uPetOldThisID = petID;
        petDataManager.AddInhertPet((int)PetInheritPos.Old, petID);
        IPet pet = petDataManager.GetPetByThisID(petID);
        if (pet != null)
        {
            uint baseID = pet.PetBaseID;
            m_uOldPetBaseID = baseID;
            PetDataBase pdb = GameTableManager.Instance.GetTableItem<PetDataBase>(baseID);
            if (pdb != null)
            {
                m_label_Old_name.text = petDataManager.GetPetName(pet);
                //DataManager.Manager<UIManager>().SetSpriteDynamicIcon(m_sprite_Old_icon, pdb.icon);
                UIManager.GetTextureAsyn(pdb.icon, ref m_inheritOldIconAsynSeed, () =>
                {
                    if (null != m__Old_icon)
                    {
                        m__Old_icon.mainTexture = null;
                    }
                }, m__Old_icon, false);

                string bgSpriteName = ItemDefine.GetItemFangBorderIconByItemID(pdb.ItemID);
                //DataManager.Manager<UIManager>().SetSpriteDynamicIcon(m_sprite_OldIconBox, bgSpriteName);
                UIManager.GetAtlasAsyn(bgSpriteName, ref m_inheritOldQuaAsynSeed, () =>
                {
                    if (null != m_sprite_OldIconBox)
                    {
                        m_sprite_OldIconBox.atlas = null;
                    }
                }, m_sprite_OldIconBox, false);
                int oldLv = pet.GetProp((int)CreatureProp.Level);
                m_label_Old_level_Before.text = oldLv.ToString();
                m_label_Old_level_After.text = "0";
                m_label_Old_skill_Before.text = pet.GetPetSkillList().Count.ToString();
                m_label_Old_skill_After.text = "0";
                m_label_Old_xiuwei_Before.text = pet.GetProp((int)PetProp.YinHunLevel).ToString();
                m_label_Old_xiuwei_After.text = "0";
                PetInHeritDataBase idb = GetInHeritData(m_uNewPetBaseID);
                if (idb != null)
                {
                    UIItem.AttachParent(m_sprite_ChuanChengCommon_icon_di.transform, idb.costItem, 0, InheritGetWayCallBack);
                    ItemDataBase db = GameTableManager.Instance.GetTableItem<ItemDataBase>(idb.costItem);
                    if (db != null)
                    {
                        m_label_ChuanChengCommon_name.text = db.itemName;
                    }
                    m_label_ChuanChengCommon_number.text = "0/0";
                }
                m_btn_btn_Old_delete.gameObject.SetActive(true);
            }
        }
    }
    void ResetCostItem()
    {
        m_sprite_ChuanChengCommon_icon_di.spriteName = "";
        m_label_ChuanChengCommon_name.text = "";
        m_label_ChuanChengCommon_number.text = "";
    }
    void InheritGetWayCallBack(UIItemCommonGrid grid)
    {
        int itemCount = DataManager.Manager<ItemManager>().GetItemNumByBaseId(grid.Data.DwObjectId);
        if (itemCount < grid.Data.NeedNum)
        {
            TipsManager.Instance.ShowItemTips(grid.Data.DwObjectId);
            DataManager.Manager<UIPanelManager>().ShowPanel(PanelID.GetWayPanel, data: grid.Data.DwObjectId);

        }
        else
        {
            TipsManager.Instance.ShowItemTips(grid.Data.DwObjectId);
        }
    }
    void ResetOldPet()
    {
        m_label_Old_name.text = string.Empty;
        m__Old_icon.mainTexture = null;
        //m_sprite_Old_icon.spriteName = "";
        //DataManager.Manager<UIManager>().SetSpriteDynamicIcon(m_sprite_OldIconBox, 1);
        UIManager.GetQualityAtlasAsyn(1, ref m_inheritOldQuaAsynSeed, () =>
        {
            if (null != m_sprite_OldIconBox)
            {
                m_sprite_OldIconBox.atlas = null;
            }
        }, m_sprite_OldIconBox);
        m_btn_btn_Old_delete.gameObject.SetActive(false);
        m_label_Old_level_Before.text = string.Empty;
        m_label_Old_level_After.text = string.Empty;
        m_label_Old_skill_Before.text = string.Empty;
        m_label_Old_skill_After.text = string.Empty;
        m_label_Old_xiuwei_Before.text = string.Empty;
        m_label_Old_xiuwei_After.text = string.Empty;
    }
    #region btns
    void onClick_Btn_Old_delete_Btn(GameObject caster)
    {
        petDataManager.RemoveInheritPet((int)PetInheritPos.Old);
        ResetOldPet();
        ResetCostItem();
        petDataManager.ResetInheritData();
        ResetGou();
    }

    void onClick_Btn_New_delete_Btn(GameObject caster)
    {
        petDataManager.RemoveInheritPet((int)PetInheritPos.New);
        ResetNewPet();
        ResetCostItem();
        petDataManager.ResetInheritData();
        ResetGou();
    }

    void onClick_ChuanChengCommon_zidongbuzu_Btn(GameObject caster)
    {

    }

    void onClick_KaishiChuanCheng_Btn(GameObject caster)
    {
        if (!petDataManager.bInheritExp && !petDataManager.bInheritSkill && !petDataManager.bInheritXiuwei)
        {
            TipsManager.Instance.ShowTips(LocalTextType.Pet_Commond_qinggouxuanyaochuanchengxuanxiang);
            return;
        }
        IPet oldpet = petDataManager.GetInheritPet((int)PetInheritPos.Old);
        if (oldpet == null)
        {
            return;
        }
        IPet newpet = petDataManager.GetInheritPet((int)PetInheritPos.New);
        if (newpet == null)
        {
            return;
        }
        if (newpet.GetInheritNum() >= 1)
        {
            TipsManager.Instance.ShowTips(LocalTextType.Pet_Next_zhenshouzhinengchuanchengyici);
            return;
        }

        PetInHeritDataBase db = GetInHeritData(m_uNewPetBaseID);
        if (db == null)
        {
            return;
        }
        uint costItem = db.costItem;
        int num = DataManager.Manager<ItemManager>().GetItemNumByBaseId(costItem);
        if (petDataManager.InheritCostItemNum > num)
        {
            ItemDataBase itemdb = GameTableManager.Instance.GetTableItem<ItemDataBase>(costItem);
            if (itemdb != null)
            {
                TipsManager.Instance.ShowLocalFormatTips(LocalTextType.Pet_Next_chuanchengxiaohaodedaojubuzu, itemdb.itemName);
            }
            return;
        }
        if (!MainPlayerHelper.IsHasEnoughMoney(db.moneyType, (uint)petDataManager.InHeritCostMoney, false))
        {
            DataManager.Manager<Mall_HuangLingManager>().ShowExchange((uint)ErrorEnum.NotEnoughGold, ClientMoneyType.Gold, "提示", "去兑换", "取消");
           // TipsManager.Instance.ShowTips(LocalTextType.Pet_Next_chuanchengxiaohaodejingbibuzu);
            return;
        }
        PetDataBase newPdb = GameTableManager.Instance.GetTableItem<PetDataBase>(m_uNewPetBaseID);
        PetDataBase oldPdb = GameTableManager.Instance.GetTableItem<PetDataBase>(m_uOldPetBaseID);
        if (newPdb != null && oldPdb != null)
        {
            if (newPdb.petQuality < oldPdb.petQuality)
            {
                TipsManager.Instance.ShowTips(LocalTextType.Pet_Next_mubiaozhenshoudepinzhixudayudengyucailiaozhenshou);
                return;
            }
        }
        stInheritPetUserCmd_CS cmd = new stInheritPetUserCmd_CS();
        cmd.src_id = oldpet.GetID();
        cmd.des_id = newpet.GetID();
        cmd.exp = petDataManager.bInheritExp;
        cmd.skill = petDataManager.bInheritSkill;
        cmd.yh = petDataManager.bInheritXiuwei;
        NetService.Instance.Send(cmd);

    }
    #endregion
}
