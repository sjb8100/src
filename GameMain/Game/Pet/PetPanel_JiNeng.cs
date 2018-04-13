using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using GameCmd;
using table;
partial class PetPanel
{

    #region skillpanel
    public void InitFirstSkill()
    {
        if (CurPet != null)
        {
            List<PetSkillObj> list = CurPet.GetPetSkillList();
            if (petDataManager.SelectSkillDataBase == null)
            {

                if (list.Count > 0)
                {
                    PetSkillObj obj = list[0];
                    SkillDatabase db = GameTableManager.Instance.GetTableItem<SkillDatabase>((uint)obj.id, obj.lv);
                    if (db != null)
                    {
                        petDataManager.SetSkillDatabase(db);
                    }
                }
            }
            if (list.Count == 0)
            {
                petDataManager.SetSkillDatabase(null);
            }
        }
    }
    uint m_nSkillNeedItemID = 0;
    public void InitSkillPanel()
    {
        if (m_label_jinengpetname == null)
        {
            return;
        }
        if (CurPet != null)
        {
            m_label_jinengpetname.text = petDataManager.GetCurPetName();
            m_label_jinengpetlevel.text = petDataManager.GetCurPetLevel().ToString();
            List<PetSkillObj> list = CurPet.GetPetSkillList();


            if (petDataManager.SelectSkillDataBase != null)
            {
                m_widget_SkillDescription.gameObject.SetActive(true);

                uint skillid = petDataManager.SelectSkillDataBase.wdID;
                PetSkillObj obj = null;
                if (list.Count > 0)
                {
                    obj = list[0];
                }


                for (int i = 0; i < list.Count; i++)
                {
                    var skill = list[i];
                    if (skill.id == skillid)
                    {
                        obj = skill;
                    }
                }
                if (obj != null)
                {
                    SkillDatabase db = GameTableManager.Instance.GetTableItem<SkillDatabase>((uint)obj.id, obj.lv);
                    if (db != null)
                    {
                        petDataManager.SetSkillDatabase(db);
                    }

                }
                else
                {
                    m_widget_SkillDescription.gameObject.SetActive(false);
                    return;
                }
                m_label_Skillname.text = petDataManager.SelectSkillDataBase.strName;
                m_label_SkillLevel.text = petDataManager.SelectSkillDataBase.wdLevel.ToString();
                PetSkillType type = (PetSkillType)Enum.Parse(typeof(PetSkillType), petDataManager.SelectSkillDataBase.petType.ToString());
                m_label_SkillType.text = type.GetEnumDescription();
                m_label_NowLevel.text = petDataManager.SelectSkillDataBase.strDesc;
                m_label_skill_xiaohaogold.text = petDataManager.SelectSkillDataBase.dwMoney.ToString();
                uint nextLevel = petDataManager.SelectSkillDataBase.wdLevel + 1;
                SkillDatabase nexeDb = GameTableManager.Instance.GetTableItem<SkillDatabase>(petDataManager.SelectSkillDataBase.wdID, (int)nextLevel);
                if (nexeDb != null)
                {
                    m_label_NextLevel.text = nexeDb.strDesc;
                    m_widget_next.gameObject.SetActive(true);
                    m_label_skillfulltips.gameObject.SetActive(false);
                }
                else
                {
                    m_label_skillfulltips.gameObject.SetActive(true);
                    m_widget_next.gameObject.SetActive(false);
                    return;
                }
                string itemArray = nexeDb.needItemArray;
                string[] itemIDArray = itemArray.Split(';');
                string[] itemNum = itemIDArray[0].Split('_');
                uint itemID = uint.Parse(itemNum[0]);
                uint needNum = uint.Parse(itemNum[1]);
                m__jineng_icon.transform.DestroyChildren();
                m_nSkillNeedItemID = itemID;
                int itemCount = DataManager.Manager<ItemManager>().GetItemNumByBaseId(itemID);
                UpLevelNeedNum = needNum;
                ShowJiNengColdLabel(itemID);
                UIItem.AttachParent(m__jineng_icon.transform, itemID, (uint)itemCount, ShowJinengGetWayCallBack);
                int count = CurPet.GetPetSkillList().Count;

                ItemDataBase itemDb = GameTableManager.Instance.GetTableItem<ItemDataBase>(itemID);
                if (itemDb != null)
                {
                    m_label_jineng_name.text = itemDb.itemName;
                    m_label_jineng_number.text = StringUtil.GetNumNeedString(itemCount, needNum);

                }
            }
            else
            {
                m_widget_SkillDescription.gameObject.SetActive(false);
            }
        }
    }
    void ShowJinengGetWayCallBack(UIItemCommonGrid grid)
    {
        int itemCount = DataManager.Manager<ItemManager>().GetItemNumByBaseId(m_nSkillNeedItemID);
        if (0 == itemCount)
        {
            TipsManager.Instance.ShowItemTips(m_nSkillNeedItemID);
            DataManager.Manager<UIPanelManager>().ShowPanel(PanelID.GetWayPanel, data: grid.Data.DwObjectId);

        }
        else
        {
            TipsManager.Instance.ShowItemTips(m_nSkillNeedItemID);
        }
    }
    //技能升级的参数消耗元宝时原函数只有单价，本地缓存抵用消耗的个数
    uint UpLevelNeedNum = 1;
    bool ShowJiNengColdLabel(uint itemID)
    {
        int itemCount = DataManager.Manager<ItemManager>().GetItemNumByBaseId(itemID);
        if (itemCount <= 0)
        {
            if (skillUpAutobuy)
            {
                string needStr = "";
                if (petDataManager.GetItemNeedColdString(itemID, ref needStr, UpLevelNeedNum))
                {
                    m_label_jineng_number.gameObject.SetActive(false);
                    m_label_jinengshengji_dianjuanxiaohao.gameObject.SetActive(true);
                    m_label_jinengshengji_dianjuanxiaohao.text = needStr;
                    return true;
                }
                else
                {
                    m_label_jineng_number.gameObject.SetActive(false);
                    m_label_jinengshengji_dianjuanxiaohao.gameObject.SetActive(true);
                    m_label_jinengshengji_dianjuanxiaohao.text = needStr;
                    return false;
                }

            }
            else
            {
                m_label_jineng_number.gameObject.SetActive(true);
                m_label_jinengshengji_dianjuanxiaohao.gameObject.SetActive(false);
            }
        }
        else
        {
            m_label_jineng_number.gameObject.SetActive(true);
            m_label_jinengshengji_dianjuanxiaohao.gameObject.SetActive(false);
        }
        return true;
    }
    #endregion
    void onClick_Xuexijineng_Btn(GameObject caster)
    {
        //ReturnBackUIData[] returnUI = new ReturnBackUIData[1];
        //returnUI[0] = new ReturnBackUIData();
        //returnUI[0].panelid = PanelID.PetPanel;
        //returnUI[0].msgid = UIMsgID.eShowUI;
        //returnUI[0].param = new ReturnBackUIMsg()
        //{
        //    tabs = new int[1] { (int)PetPanelSubFlag.Jinneg },
        //    param = CurPet.GetID(),
        //};


        DataManager.Manager<UIPanelManager>().ShowPanel(PanelID.PetLearnSkill);
        //DataManager.Manager<UIPanelManager>().ShowPanel( PanelID.PetLearnSkill,returnBackUIData:returnUI);
    }

    bool skillUpAutobuy = false;
    void onClick_Jineng_Sprite_Btn(GameObject caster)
    {
        skillUpAutobuy = !skillUpAutobuy;
        Transform spr = caster.transform.Find( "Sprite" );
        if(spr != null)
        {
            spr.gameObject.SetActive( skillUpAutobuy );
        }
        ShowJiNengColdLabel( m_nSkillNeedItemID );
    }

    void onClick_Skill_shengji_Btn(GameObject caster)
    {
        if(petDataManager.SelectSkillDataBase != null && CurPet != null)
        {
            int itemCount = DataManager.Manager<ItemManager>().GetItemNumByBaseId( m_nSkillNeedItemID );
            if(skillUpAutobuy)
            {
                if ( !ShowJiNengColdLabel( m_nSkillNeedItemID ) )
                {
                    TipsManager.Instance.ShowTipsById( 5 );
                    return ;
                }
            }
            else
            {
                if(itemCount <= 0)
                {
                    TipsManager.Instance.ShowTipsById( 6 );
                    return;
                }
            }
            stUpSkillPetUserCmd_CS cmd = new stUpSkillPetUserCmd_CS();
            cmd.id = CurPet.GetID();
            cmd.skill = (int)petDataManager.SelectSkillDataBase.wdID;
            cmd.auto_buy = skillUpAutobuy;
            NetService.Instance.Send( cmd );
        }

    }

    void onClick_Shengji_Btn(GameObject caster)
    {

    }

}

