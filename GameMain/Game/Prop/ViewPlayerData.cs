using System;
using System.Collections.Generic;
using GameCmd;
using table;
public class ViewPlayerData
{
    public GameCmd.ViewRoleData viewRoleData;
    public GameCmd.RideUserData rideData;
    public GameCmd.PetUserData petdata;
    public List<GameCmd.ItemSerialize> itemList = new List<GameCmd.ItemSerialize>();
    public List<GameCmd.GemInlayList> gem_data = new List<GameCmd.GemInlayList>();
    public List<GameCmd.SuitData> suit_data = new List<GameCmd.SuitData>();
    public List<GameCmd.StrengthList> strengthList = new List<GameCmd.StrengthList>();
    public uint god_level;
    public uint power;
    public string username;
    public uint userid;
    public string clan_name;
    public uint user_level;
    public uint job;
    public uint sex;
    static ViewPlayerData retData = null;
    public static ViewPlayerData GetViewPlayerData()
    {
        return retData;
    }
    public static ViewPlayerData BuildViewData(uint uid, string name, uint job, int level, uint sex = 0)
    {
        retData = new ViewPlayerData();
        retData.job = job;
        retData.sex = sex;
        retData.userid = uid;
        retData.username = name;
        table.RobotDataBase data = GameTableManager.Instance.GetTableItem<table.RobotDataBase>(job, level);
        if (data != null)
        {
            retData.power = data.power;

            retData.user_level = data.dwLevel;

            retData.viewRoleData = new GameCmd.ViewRoleData();
            retData.viewRoleData.maxhp = (uint)(data.hp * 1.15f);
            retData.viewRoleData.maxsp = (uint)(data.mp * 1.15f);

            retData.viewRoleData.pdam_min = (uint)(data.pdam * 0.8f);
            retData.viewRoleData.pdam_max = (uint)(data.pdam * 1.15f);

            retData.viewRoleData.mdam_min = (uint)(data.mdam * 0.8f);
            retData.viewRoleData.mdam_max = (uint)(data.mdam * 1.15f);

            retData.viewRoleData.pdef_min = (uint)(data.mdef * 0.8f);
            retData.viewRoleData.pdef_max = (uint)(data.mdef * 1.15f);

            retData.viewRoleData.liliang = data.liliang;
            retData.viewRoleData.minjie = data.minjie;
            retData.viewRoleData.zhili = data.zhili;
            retData.viewRoleData.tizhi = data.tizhi;
            retData.viewRoleData.jingshen = data.jingshen;
            retData.viewRoleData.esdam = data.esdam;
            retData.viewRoleData.ssdam = data.ssdam;
            retData.viewRoleData.lsdam = data.lsdam;
            retData.viewRoleData.vsdam = data.vsdam;
            retData.viewRoleData.lucky = data.lucky;
            retData.viewRoleData.mlucky = data.mlucky;
            retData.viewRoleData.criti_ratio = data.criti_ratio;
            retData.viewRoleData.cure = data.cure;
            retData.viewRoleData.phit = data.phit;
            retData.viewRoleData.hide_per = data.hide_per;
            retData.viewRoleData.pabs = data.pabs;
            retData.viewRoleData.mabs = data.mabs;
            retData.viewRoleData.harm_add_per = data.harm_add_per;
            retData.viewRoleData.harm_sub_per = data.harm_sub_per;

            List<uint> equipList = new List<uint>();
            /*1;    //  头盔
            2;    //  护肩
            3;    //  上衣
            4;    //  护腿
            5;    //  戒指
            7;    //  盾牌
            8;    //  武器
            9;    //  鞋子
            10;   //  护腕
            11;   //  腰带
            12;   //  披风
            13;   //  项链*/
            equipList.Add(data.Hat);
            equipList.Add(data.Shoulder);
            equipList.Add(data.Coat);
            equipList.Add(data.Leg);
            equipList.Add(data.Adornl_1);
            equipList.Add(data.Adornl_2);
            equipList.Add(data.Shield);
            equipList.Add(data.Equip);
            equipList.Add(data.Shoes);
            equipList.Add(data.Cuff);
            equipList.Add(data.Belf);
            equipList.Add(data.Necklace);

            GameCmd.ItemSerialize item = null;
            for (int i = 0; i < equipList.Count; i++)
            {
                if (equipList[i] == 0)
                {
                    continue;
                }
                table.ItemDataBase itemdb = GameTableManager.Instance.GetTableItem<table.ItemDataBase>(equipList[i]);
                if (itemdb == null)
                {
                    continue;
                }
                //GameCmd.eItemAttribute
                item = new GameCmd.ItemSerialize();
                item.dwObjectID = equipList[i];

                GameCmd.EquipPos[] pos = EquipDefine.GetEquipPosByEquipType((GameCmd.EquipType)itemdb.subType);
                if (pos.Length == 2)
                {
                    uint loc = 0;
                    if (i == 4)
                    {
                        loc = ItemDefine.TransformLocal2ServerLocation(GameCmd.PACKAGETYPE.PACKAGETYPE_EQUIP, new UnityEngine.Vector2() { x = 0, y = (int)pos[0] });
                    }
                    else if (i == 5)
                    {
                        loc = ItemDefine.TransformLocal2ServerLocation(GameCmd.PACKAGETYPE.PACKAGETYPE_EQUIP, new UnityEngine.Vector2() { x = 0, y = (int)pos[1] });
                    }

                    item.pos = new GameCmd.tItemLocation() { loc = loc };
                }
                else if (pos.Length == 1)
                {
                    uint loc = ItemDefine.TransformLocal2ServerLocation(GameCmd.PACKAGETYPE.PACKAGETYPE_EQUIP, new UnityEngine.Vector2() { x = 0, y = (int)pos[0] });
                    item.pos = new GameCmd.tItemLocation() { loc = loc };
                }
                item.dwNum = 1;
                item.numbers.Add(new GameCmd.PairNumber() { id = (int)GameCmd.eItemAttribute.Item_Attribute_Bind, value = itemdb.bindMask });
                item.numbers.Add(new GameCmd.PairNumber() { id = (int)GameCmd.eItemAttribute.Item_Attribute_Grade, value = itemdb.grade });

                table.EquipDataBase equipdb = GameTableManager.Instance.GetTableItem<table.EquipDataBase>(itemdb.itemID);
                if (equipdb == null)
                {
                    continue;
                }
                item.numbers.Add(new GameCmd.PairNumber() { id = (int)GameCmd.eItemAttribute.Item_Attribute_Dur, value = equipdb.maxDurable });
                item.numbers.Add(new GameCmd.PairNumber() { id = (int)GameCmd.eItemAttribute.Item_Attribute_HoleNum, value = 0 });
                item.numbers.Add(new GameCmd.PairNumber() { id = (int)GameCmd.eItemAttribute.Item_Attribute_FightPower, value = equipdb.fightPower });

                retData.itemList.Add(item);

                if (equipdb.act_show != 0)
                {
                    table.SuitDataBase suitDb = GameTableManager.Instance.GetTableItem<table.SuitDataBase>(equipdb.act_show, 1);
                    if (suitDb != null)
                    {
                        retData.suit_data.Add(new GameCmd.SuitData() { baseid = suitDb.base_id, suit_type = (GameCmd.EquipSuitType)suitDb.type });
                    }
                }
            }
        }

        return retData;
    }
  
    public static ViewPlayerData BuildViewData(GameCmd.stViewRoleReturnPropertyUserCmd_S cmd)
    {
        retData = new ViewPlayerData();
        retData.userid = cmd.userid;
        retData.user_level = cmd.user_level;
        retData.userid = cmd.userid;
        retData.username = cmd.username;
        retData.sex = cmd.sex;
        retData.power = cmd.power;
        retData.job = cmd.job;
        retData.clan_name = cmd.clan_name;

        retData.gem_data = cmd.gem_data;
        retData.viewRoleData = cmd.user_data;
        retData.suit_data = cmd.suit_data;
        retData.itemList = cmd.itemList;
        retData.strengthList = cmd.strength_data;

        Engine.PackageIn pack = new Engine.PackageIn(cmd.pet_data);
        retData.petdata = GameCmd.PetUserData.Deserialize(pack);

        pack = new Engine.PackageIn(cmd.ride_data);
        retData.rideData = GameCmd.RideUserData.Deserialize(pack);

        return retData;
    }

    uint activeStrengthenSuitLv = 0;
    public uint ActiveStrengthenSuitLv 
    {
        get 
        {
            List<GridStrengthenSuitDataBase> StrengthenDataBase = GameTableManager.Instance.GetTableList<GridStrengthenSuitDataBase>();

            for (int i = 0; i < StrengthenDataBase.Count; i++)
            {
                if (StrengthenDataBase[i].job == retData.job)
                {
                    uint matchNum = 0; 
                    for (int j = 0; j < strengthList.Count; j++)
                    {
                        if (strengthList[j].level >= StrengthenDataBase[i].triggerStrengLv)
                        {
                            matchNum++;
                        }
                    }
                    if (matchNum >= StrengthenDataBase[i].triggerPosNum)
                    {
                        activeStrengthenSuitLv = StrengthenDataBase[i].suitlv;
                    }

                }
            }
            return activeStrengthenSuitLv;
        }
    }

    uint colorLv = 0;
    public uint ActiveColorSuitLv 
    {
        get
        {
            List<ColorSuitDataBase> colors = GameTableManager.Instance.GetTableList<ColorSuitDataBase>();
            List<BaseItem> items = new List<BaseItem>();       
            for (int i = 0; i < itemList.Count; i++)
            {
                BaseItem baseItem = new BaseItem(itemList[i].dwObjectID, itemList[i]);
                if (baseItem != null)
                {
                    if (baseItem.EquipColor >= EquipDefine.EquipColor.Orange)
                    {
                        items.Add(baseItem);
                    }
                }
            }
            for (int j = 0; j < colors.Count; j++)
            {
                uint count = 0;
                for (int x = 0; x < items.Count; x++)
                {
                    if (items[x].Grade >= colors[j].grade)
                    {
                        count++;
                    }
                }
                if (count >= colors[j].equip_num)
                {
                    colorLv = colors[j].level;
                }
            }
            return colorLv;
        }
    }

    uint activeStoneLv = 0;
    public uint  ActiveStoneSuitLv 
    {
       get
       {
           uint stoneLv = 0;
           List<GemSuitDataBase> gems = GameTableManager.Instance.GetTableList<GemSuitDataBase>();
           for (int i = 0; i < gem_data.Count; i++)
           {
               uint itemID = gem_data[i].base_id;
               table.ItemDataBase item = GameTableManager.Instance.GetTableItem<table.ItemDataBase>(itemID);
               if (item != null)
               {
                   stoneLv += item.grade;
               }
           }
           for (int j = 0; j < gems.Count; j++)
           {
               if (stoneLv >= gems[j].gem_all_level)
              {
                  activeStoneLv = gems[j].level;
              }
           }
           return activeStoneLv;
       }
    }
           
          
}
