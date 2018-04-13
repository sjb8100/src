//*************************************************************************
//	创建日期:	2016-12-30 11:38
//	文件名称:	CommonUseItemPanel.cs
//  创 建 人:   	zhudianyu
//	版权所有:	中青宝
//	说    明:	通用使用物品界面（坐骑，其他）
using System;
using System.Collections.Generic;
using UnityEngine;
using Client;
using table;
using Common;
using GameCmd;
public partial class UseItemCommonPanel : UIPanelBase
{
    PetDataManager petDataManager
    {
        get
        {
            return DataManager.Manager<PetDataManager>();
        }
    }
    public IPet CurPet
    {
        get
        {
            return petDataManager.CurPet;
        }
    }
    void InitPetData(UseItemEnum type)
    {
        if (CurPet == null)
            return;
        UpdateSlider();
        m_lstUseItemData.Clear();

        string str = "ExpItem";
        if (type == UseItemEnum.PetLife)
        {
            str = "LifeItem";
            m_label_title.text = CommonData.GetLocalString(" 珍兽寿命");
        }
        else if (type == UseItemEnum.PetExp)
        {
            str = "ExpItem";
            m_label_title.text = petDataManager.GetCurPetLevelStr();
        }
        else
        {
            return;
        }

        List<string> itemList = GameTableManager.Instance.GetGlobalConfigKeyList(str);

        PetDataBase pdb = GameTableManager.Instance.GetTableItem<PetDataBase>(CurPet.PetBaseID);
        if (pdb == null)
        {
            return;
        }
        uint carryLevel = (uint)petDataManager.GetCurPetLevel();
        #region  从lua里面拿出来的数据无序 要先排序
        List<uint> tempList = new List<uint>();
        foreach (var strID in itemList)
        {
            uint id = uint.Parse(strID);
            tempList.Add(id);
        }
        tempList.Sort((x1, x2) =>
        {
            if (x1 > x2)
                return 1;
            else if (x1 < x2)
                return -1;
            else
                return 0;
        });

        List<uint> itemIDList = new List<uint>();
        #endregion
        for (int i = 0; i < tempList.Count; i++)
        {
            uint id = tempList[i];
            itemIDList.Add(id);
            //if (type == UseItemEnum.PetLife)
            //{
            //    int firstLevel = GameTableManager.Instance.GetGlobalConfig<int>("PetFirstLevel");
            //    int secondLevel = GameTableManager.Instance.GetGlobalConfig<int>("PetSecondLevel");
            //    if (carryLevel < firstLevel)
            //    {
            //        if (i == 0 || i == 3)
            //        {
            //            itemIDList.Add(id);
            //        }
            //    }
            //    else if (carryLevel > secondLevel)
            //    {
            //        if (i == 2 || i == 5)
            //        {
            //            itemIDList.Add(id);
            //        }
            //    }
            //    else
            //    {
            //        if (i == 1 || i == 4)
            //        {
            //            itemIDList.Add(id);
            //        }
            //    }
            //}
            //else
            //{
            //    itemIDList.Add(id);
            //}

        }

        itemIDList.Sort((x1, x2) =>
        {
            if (x1 > x2)
                return 1;
            else if (x1 < x2)
                return -1;
            else
                return 0;
        });
        if (type == UseItemEnum.PetExp)
        {
            List<uint> hasList = new List<uint>();
            List<uint> noList = new List<uint>();
            for (int i = 0; i < itemIDList.Count; i++)
            {
                uint id = itemIDList[i];
                int count = DataManager.Manager<ItemManager>().GetItemNumByBaseId(id);
                if (count != 0)
                {
                    hasList.Add(id);
                }
                else
                {
                    noList.Add(id);
                }
            }
            itemIDList.Clear();
            itemIDList.AddRange(hasList);
            itemIDList.AddRange(noList);
        }

        for (int i = 0; i < itemIDList.Count; i++)
        {
            UseItemData itemdata = new UseItemData();
            itemdata.parent = this;
            itemdata.itemid = (itemIDList[i]);
            itemdata.useNum = (uint)1;
            m_lstUseItemData.Add(itemdata);
        }
    }
    void PetDataManager_ValueUpdateEvent(object sender, ValueUpdateEventArgs e)
    {
        if (e != null)
        {
            if (e.key == PetDispatchEventString.PetRefreshProp.ToString())
            {
                RefreshPetSliderData();
            }
        }
    }
    bool bHasDanyao(uint itemID, uint willUseNum)
    {
        int itemCount = DataManager.Manager<ItemManager>().GetItemNumByBaseId(itemID);
        if (itemCount < willUseNum)
        {
            return false;
        }
        return true;
    }
    bool IsCanUpLevel()
    {
        uint itemID = m_currUICommonUseItemGrid.Data.itemid;
        if (m_UseItemParam.type == UseItemEnum.PetExp)
        {
            int level = ClientGlobal.Instance().MainPlayer.GetProp((int)CreatureProp.Level);
            int yinhunLevel = CurPet.GetProp((int)PetProp.YinHunLevel);
            int totalLevel = level + yinhunLevel;
            if(m_nCurPetLevel >= totalLevel)
            {
                return false;
            }
            int addexp = GameTableManager.Instance.GetGlobalConfig<int>("ExpItem", itemID.ToString());
            m_nCurPetExp = m_nCurPetExp + addexp;
           PetUpGradeDataBase db = GameTableManager.Instance.GetTableItem<PetUpGradeDataBase>((uint)m_nCurPetLevel, (int)CurPet.PetBaseID);
            if(db == null)
            {
                Engine.Utility.Log.Error("level is error " + m_nCurPetLevel);
                return false;
            }
            uint needExp = db.upGradeExp;
            while (m_nCurPetExp >= needExp)
            {
                int petlevel = m_nCurPetLevel + 1;
              
                if (petlevel >= totalLevel)
                {
                    OnPetUse(m_currUICommonUseItemGrid);
                    return false;
                }
                else
                {
                    m_nCurPetLevel = petlevel;
               
                }
                m_nCurPetExp -= (int)needExp;
                needExp = GameTableManager.Instance.GetTableItem<PetUpGradeDataBase>((uint)m_nCurPetLevel, (int)CurPet.PetBaseID).upGradeExp;
            }
       
        }
        return true;
    }
    bool Check(uint itemID,uint willUseNum)
    {
        if (!bHasDanyao(itemID,willUseNum))
        {
            TipsManager.Instance.ShowTipsById(6);
            return false;
        }
        if (m_UseItemParam.type == UseItemEnum.PetExp)
        {
            if (!IsCanUpLevel())
            {
                TipsManager.Instance.ShowTips(LocalTextType.Pet_Rank_zhanhundengjiyimanzanshiwufajixushengji);
                return false;
            }

        }
        if (m_UseItemParam.type == UseItemEnum.PetLife)
        {
//             int addLife = GameTableManager.Instance.GetGlobalConfig<int>("LifeItem", itemID.ToString());
//             m_nCurPetLife += addLife;
            int life = CurPet.GetProp((int)PetProp.Life);
            if (CurPet != null)
            {
                PetDataBase pdb = GameTableManager.Instance.GetTableItem<PetDataBase>(CurPet.PetBaseID);
                if (pdb != null)
                {
                    int maxLife = (int)pdb.life;
                    if (life >= maxLife)
                    {
                        TipsManager.Instance.ShowTips(LocalTextType.Pet_Age_zhanhunshoumingyiman);
                        //TipsManager.Instance.ShowTipsById(108016);
                        return false;
                    }
                }

            }
        }

        return true;
    }
    void OnPetUseSingeItem(UICommonUseItemGrid grid)
    {
         uint itemID = grid.Data.itemid;
        if(Check(itemID,1))
        {
            m_uWillUseItemNum = 1;
            OnPetUse(grid);
        }
    }
    void OnPetUse(UICommonUseItemGrid grid)
    {
        if (CurPet == null)
            return;

        uint itemID = grid.Data.itemid;
   
        if (m_UseItemParam.type == UseItemEnum.PetExp)
        {
            stAddExpPetUserCmd_C cmd = new stAddExpPetUserCmd_C();
            cmd.id = CurPet.GetID();
            cmd.item = itemID;
            cmd.num = m_uWillUseItemNum;
            NetService.Instance.Send(cmd);
        }
        if (m_UseItemParam.type == UseItemEnum.PetLife)
        {
            stAddLifePetUserCmd_C cmd = new stAddLifePetUserCmd_C();
            cmd.id = CurPet.GetID();
            cmd.item = itemID;
            cmd.num = m_uWillUseItemNum;
            NetService.Instance.Send(cmd);
        }


    }
    void RefreshPetSliderData()
    {
        if (CurPet == null)
        {
            return;
        }
        int level = CurPet.GetProp((int)CreatureProp.Level);
        int curLife = CurPet.GetProp((int)PetProp.Life);
        int curExp = CurPet.GetProp((int)PetProp.LevelExp);
        m_label_title.text = petDataManager.GetCurPetLevelStr();
        uint totalExp = GameTableManager.Instance.GetTableItem<PetUpGradeDataBase>((uint)level, (int)CurPet.PetBaseID).upGradeExp;
        PetDataBase db = petDataManager.GetPetDataBase(CurPet.PetBaseID);
        if (db != null)
        {
            uint life = db.life;
            if (m_UseItemParam.type == UseItemEnum.PetLife)
            {
                ShowSlider(curLife, (int)life);
            }
        }
        if (m_UseItemParam.type == UseItemEnum.PetExp)
        {
            ShowSlider(curExp, (int)totalExp);
        }
 
    }

    void RefreshPetSliderByClientData()
    {
        if (CurPet == null)
        {
            return;
        }
        if(m_currUICommonUseItemGrid == null ||m_currUICommonUseItemGrid.Data == null)
        {
            return;
        }
        uint itemID = m_currUICommonUseItemGrid.Data.itemid;
        if (m_UseItemParam.type == UseItemEnum.PetExp)
        {
          
            //int addexp = GameTableManager.Instance.GetClientGlobalConst<int>("ExpItem", itemID.ToString());
            //m_nCurPetExp = m_nCurPetExp + addexp;
            uint needExp = GameTableManager.Instance.GetTableItem<PetUpGradeDataBase>((uint)m_nCurPetLevel, (int)CurPet.PetBaseID).upGradeExp;
            //while(m_nCurPetExp >= needExp)
            //{
            //    m_nCurPetLevel += 1;
            //    m_nCurPetExp -= (int)needExp;
            //    needExp = GameTableManager.Instance.GetTableItem<PetUpGradeDataBase>((uint)m_nCurPetLevel, (int)CurPet.PetBaseID).upGradeExp;
            //}
            m_label_title.text = m_nCurPetLevel + CommonData.GetLocalString("级");

            ShowSlider(m_nCurPetExp, (int)needExp);
        }
        else if(m_UseItemParam.type == UseItemEnum.PetLife)
        {
           // int addLife = GameTableManager.Instance.GetClientGlobalConst<int>("LifeItem", itemID.ToString());
           // m_nCurPetLife += addLife;
            PetDataBase db = petDataManager.GetPetDataBase(CurPet.PetBaseID);
            if (db != null)
            {
                uint life = db.life;

                ShowSlider(m_nCurPetLife, (int)life);
                
            }
        }
    }
}

