using Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using table;


class GrowUpManager : BaseModuleData, IManager
{
    #region property

    List<GrowUpFightPowerDabaBase> m_lstGrowUpFightPowerDabaBase;
    List<GrowUpFightPowerLevelDabaBase> m_lstGrowUpFightPowerLevelDabaBase;
    List<GrowUpDabaBase> m_lstGrowUpDabaBase;
    List<GrowUpRecommendFightPowerDabaBase> m_lstGrowUpRecommendFightPowerDabaBase;

    /// <summary>
    /// 玩法数据
    /// </summary>
    Dictionary<uint, Dictionary<uint, List<uint>>> m_dicGrowUp = new Dictionary<uint, Dictionary<uint, List<uint>>>();

    public Dictionary<uint, Dictionary<uint, List<uint>>> GrowUpDic
    {
        get
        {
            return m_dicGrowUp;
        }
    }
    #endregion


    #region interface

    public void Initialize()
    {
        m_lstGrowUpFightPowerDabaBase = GameTableManager.Instance.GetTableList<GrowUpFightPowerDabaBase>();
        m_lstGrowUpFightPowerLevelDabaBase = GameTableManager.Instance.GetTableList<GrowUpFightPowerLevelDabaBase>();
        m_lstGrowUpDabaBase = GameTableManager.Instance.GetTableList<GrowUpDabaBase>();
        m_lstGrowUpRecommendFightPowerDabaBase = GameTableManager.Instance.GetTableList<GrowUpRecommendFightPowerDabaBase>();

        InitGrowUpDic();
    }
    public void Reset(bool depthClearData = false)
    {

    }

    public void Process(float deltaTime)
    {

    }
    public void ClearData()
    {

    }

    #endregion


    #region method

    /// <summary>
    /// 玩法分类数据初始化
    /// </summary>
    void InitGrowUpDic()
    {
        m_dicGrowUp.Clear();
        for (int i = 0; i < m_lstGrowUpDabaBase.Count; i++)
        {
            //不含大类  全部新建数据  同时把这个数据加入
            if (m_dicGrowUp.ContainsKey(m_lstGrowUpDabaBase[i].Type) == false)
            {
                List<uint> idList = new List<uint>();
                idList.Add(m_lstGrowUpDabaBase[i].dwID);

                Dictionary<uint, List<uint>> secondDic = new Dictionary<uint, List<uint>>();
                secondDic.Add(m_lstGrowUpDabaBase[i].IndexType, idList);

                m_dicGrowUp.Add(m_lstGrowUpDabaBase[i].Type, secondDic);
            }
            else //包含大类
            {
                Dictionary<uint, List<uint>> secondDic = m_dicGrowUp[m_lstGrowUpDabaBase[i].Type];

                //包含大类不包含小类  新建数据  并加入当前数据
                if (secondDic.ContainsKey(m_lstGrowUpDabaBase[i].IndexType) == false)
                {
                    List<uint> idList = new List<uint>();
                    idList.Add(m_lstGrowUpDabaBase[i].dwID);

                    secondDic.Add(m_lstGrowUpDabaBase[i].IndexType, idList);
                }
                else
                {
                    //不包含大类并且不包含小类  取出数据即可
                    List<uint> idList = secondDic[m_lstGrowUpDabaBase[i].IndexType];
                    if (idList.Contains(m_lstGrowUpDabaBase[i].dwID) == false)
                    {
                        idList.Add(m_lstGrowUpDabaBase[i].dwID);
                    }
                    else
                    {
                        Engine.Utility.Log.Error("ID数据重复！！！");
                    }
                }
            }

        }//for 循环结束

        Engine.Utility.Log.Info("数据整理完成");
    }

    /// <summary>
    /// 玩法分类  通过key获取数据
    /// </summary>
    /// <param name="key">大类</param>
    /// <param name="secondKey">小类</param>
    /// <param name="idList"> idList</param>
    /// <returns></returns>
    public bool TryGetGrowUpIdListByKeyAndSecondkey(uint key, uint secondKey, out List<uint> idList)
    {
        Dictionary<uint, List<uint>> secondDic;
        if (m_dicGrowUp.TryGetValue(key, out secondDic))
        {
            if (secondDic.TryGetValue(secondKey, out idList))
            {
                return true;
            }
            else
            {
                idList = null;
                return false;
            }
        }
        else
        {
            idList = null;
            return false;
        }

        idList = null;
        return false;
    }

    /// <summary>
    /// 获得当前可用九洲之力 List数据
    /// </summary>
    /// <returns></returns>
    public List<GrowUpFightPowerDabaBase> GetGrowUpFightPowerGridList()
    {
        IPlayer mainPlayer = Client.ClientGlobal.Instance().MainPlayer;
        if (mainPlayer == null)
        {
            return null;
        }

        int lv = mainPlayer.GetProp((int)CreatureProp.Level);

        List<GrowUpFightPowerDabaBase> idDataList = m_lstGrowUpFightPowerDabaBase.FindAll((data) => { return lv >= (int)data.LvLimit; });

        return idDataList;
    }

    public bool TryGetFightPowerDatabaseById(uint id, out uint[] data)
    {
        IPlayer mainPlayer = Client.ClientGlobal.Instance().MainPlayer;
        if (mainPlayer == null)
        {
            data = null;
            return false;
        }

        int lv = mainPlayer.GetProp((int)CreatureProp.Level);
        GrowUpFightPowerLevelDabaBase db = m_lstGrowUpFightPowerLevelDabaBase.Find((d) => { return lv <= d.MaxLv && lv >= d.MinLv; });
        if (db == null)
        {
            data = null;
            return false;
        }

        string dataStr = string.Empty;
        switch (id)
        {
            case 1: dataStr = db.Skill; break;
            case 2: dataStr = db.Equip; break;
            case 3: dataStr = db.Strengthen; break;
            case 4: dataStr = db.Gem; break;
            case 5: dataStr = db.Pet; break;
            case 6: dataStr = db.Muhon; break;
            case 7: dataStr = db.ClanSkill; break;
        }

        if (dataStr == "0")
        {
            data = null;
            return false;
        }

        string[] strArr = dataStr.Split('_');
        data = new uint[strArr.Length];

        for (int i = 0; i < strArr.Length; i++)
        {
            if (!uint.TryParse(strArr[i], out data[i]))
            {
                Engine.Utility.Log.Error("--->>> 表格数据出错 ,没取到数据！");
            }
        }

        return true;
    }

    /// <summary>
    /// 获得战力进度条value  通过九洲之力的id
    /// </summary>
    /// <param name="id">九洲之力的id</param>
    /// <returns></returns>
    public float GetFightPowerSliderValueById(uint id)
    {
        float sliderValue = 0;
        uint[] data;
        if (TryGetFightPowerDatabaseById(id, out data))
        {
            switch (id)
            {
                case 1://技能
                    {
                        if (data.Length == 1)
                        {
                            sliderValue = GetUseSkillLvSliderValue(data[0]);
                        }
                    };
                    break;

                case 2://装备
                    {
                        if (data.Length == 2)
                        {
                            sliderValue = GetUseEquipSliderValue(data[0], data[1]);
                        }
                    };
                    break;

                case 3://强化
                    {
                        if (data.Length == 1)
                        {
                            sliderValue = GetStrengthenSliderValue(data[0]);
                        }
                    };
                    break;

                case 4://宝石
                    {
                        if (data.Length == 2)
                        {
                            sliderValue = GetGemSliderValue(data[0], data[1]);
                        }
                    };
                    break;

                case 5:  //珍兽
                    {
                        if (data.Length == 1)
                        {
                            sliderValue = GetFightingPetSliderValue(data[0]);
                        }

                    };
                    break;

                case 6:  //圣魂
                    {
                        if (data.Length == 1)
                        {
                            sliderValue = GetMuhonSliderValue(data[0]);
                        }
                    };
                    break;

                case 7:  //氏族技能
                    {
                        if (data.Length == 1)
                        {
                            sliderValue = GetClanSkillSliderValue(data[0]);
                        }
                    };
                    break;
            }


        }

        return sliderValue;
    }

    /// <summary>
    /// 技能
    /// </summary>
    /// <param name="requireLv"></param>
    /// <returns></returns>
    float GetUseSkillLvSliderValue(uint requireLv)
    {
        float sliderValue = 0;

        uint useSkillLvSum = DataManager.Manager<LearnSkillDataManager>().GetUseSkillLvSum();
        if (requireLv > 0)
        {
            sliderValue = (float)useSkillLvSum / requireLv;

            sliderValue = sliderValue > 1 ? 1.0f : sliderValue;
        }

        return sliderValue;
    }

    /// <summary>
    /// 装备
    /// </summary>
    /// <param name="num">数量</param>
    /// <param name="dangCi">档次</param>
    float GetUseEquipSliderValue(uint requireNum, uint requireDangCi)
    {
        float sliderValue = 0;

        //现在身上穿的装备
        List<BaseEquip> baseEquipList = DataManager.Manager<EquipManager>().GetEquipsByPackageType(GameCmd.PACKAGETYPE.PACKAGETYPE_EQUIP);

        uint count = 0;
        for (int i = 0; i < baseEquipList.Count; i++)
        {
            if (baseEquipList[i].Grade >= requireDangCi)
            {
                count++;
            }
        }

        if (requireNum > 0)
        {
            sliderValue = (float)count / requireNum;

            sliderValue = sliderValue > 1 ? 1.0f : sliderValue;
        }

        return sliderValue;
    }

    /// <summary>
    /// 变强
    /// </summary>
    /// <param name="requireStrengthenLvSum"></param>
    /// <returns></returns>
    float GetStrengthenSliderValue(uint requireStrengthenLvSum)
    {
        float sliderValue = 0;

        uint strengthenLvSum = 0;
        Dictionary<GameCmd.EquipPos, uint> dic = DataManager.Manager<EquipManager>().StrengthenGridDic;
        if (dic != null)
        {
            Dictionary<GameCmd.EquipPos, uint>.Enumerator etr = dic.GetEnumerator();
            while (etr.MoveNext())
            {
                strengthenLvSum += etr.Current.Value;
            }
        }

        if (requireStrengthenLvSum > 0)
        {
            sliderValue = (float)strengthenLvSum / requireStrengthenLvSum;

            sliderValue = sliderValue > 1 ? 1.0f : sliderValue;
        }

        return sliderValue;
    }

    /// <summary>
    /// 宝石
    /// </summary>
    float GetGemSliderValue(uint requireNum, uint requireGemLv)
    {
        float sliderValue = 0;

        uint count = 0;

        Dictionary<int, uint> gemInlayData = DataManager.Manager<EquipManager>().GemInlayData;
        Dictionary<int, uint>.Enumerator enumerator = gemInlayData.GetEnumerator();
        while (enumerator.MoveNext())
        {
            uint gemBaseId = enumerator.Current.Value;
            BaseItem baseItem = DataManager.Manager<ItemManager>().GetTempBaseItemByBaseID<BaseItem>(gemBaseId);
            if (baseItem.Grade >= requireGemLv)
            {
                count++;
            }
        }

        if (requireNum > 0)
        {
            sliderValue = (float)count / requireNum;
            sliderValue = sliderValue > 1 ? 1.0f : sliderValue;
        }
        return sliderValue;
    }

    /// <summary>
    /// 珍兽
    /// </summary>
    /// <param name="requireFightingPetLv"></param>
    /// <returns></returns>
    float GetFightingPetSliderValue(uint requireFightingPetPower)
    {
        float sliderValue = 0;
        uint fightingPetThisId = DataManager.Manager<PetDataManager>().CurFightingPet;
        IPet pet = DataManager.Manager<PetDataManager>().GetPetByThisID(fightingPetThisId);
        if (pet != null && requireFightingPetPower > 0)
        {
            int power = pet.GetProp((int)FightCreatureProp.Power);
            sliderValue = (float)power / requireFightingPetPower;
            sliderValue = sliderValue > 1 ? 1.0f : sliderValue;
        }

        return sliderValue;
    }

    /// <summary>
    /// 圣魂
    /// </summary>
    /// <param name="requireSumFightPower"></param>
    /// <returns></returns>
    float GetMuhonSliderValue(uint requireSumFightPower)
    {
        float sliderValue = 0;

        uint muhonSum = 0;

        List<uint> thisIdList = DataManager.Manager<EquipManager>().GetEquipByEquipType(GameCmd.EquipType.EquipType_SoulOne, GameCmd.PACKAGETYPE.PACKAGETYPE_EQUIP);
        for (int i = 0; i < thisIdList.Count; i++)
        {
            Muhon baseItem = DataManager.Manager<ItemManager>().GetBaseItemByQwThisId<Muhon>(thisIdList[i]);
            if (baseItem != null)
            {
                muhonSum += baseItem.Power;
            }
        }

        if (requireSumFightPower > 0)
        {
            sliderValue = (float)muhonSum / requireSumFightPower;
            sliderValue = sliderValue > 1 ? 1.0f : sliderValue;
        }

        return sliderValue;
    }


    /// <summary>
    /// 氏族技能
    /// </summary>
    /// <param name="requireClanSkillSum"></param>
    /// <returns></returns>
    float GetClanSkillSliderValue(uint requireClanSkillSum)
    {
        float sliderValue = 0;

        uint clanSkillLvSum = 0;

        List<uint> clanSkillIdTableList = DataManager.Manager<ClanManger>().GetClanSkillDatas();
        for (int i = 0; i < clanSkillIdTableList.Count; i++)
        {
            uint clanSkillLv = DataManager.Manager<ClanManger>().GetClanSkillDevLv(clanSkillIdTableList[i]);
            clanSkillLvSum += clanSkillLv;
        }

        if (requireClanSkillSum > 0)
        {
            sliderValue = (float)clanSkillLvSum / requireClanSkillSum;
            sliderValue = sliderValue > 1 ? 1.0f : sliderValue;
        }

        return sliderValue;
    }

    /// <summary>
    /// 推荐战力
    /// </summary>
    /// <returns></returns>
    public uint GetRecommendFightPower()
    {
        IPlayer mainPlayer = Client.ClientGlobal.Instance().MainPlayer;
        if (mainPlayer == null)
        {
            Engine.Utility.Log.Error("出错啦！mainPlayer 没找到");
            return 0;
        }

        int playerLv = mainPlayer.GetProp((int)CreatureProp.Level);

        GrowUpRecommendFightPowerDabaBase db = GameTableManager.Instance.GetTableItem<GrowUpRecommendFightPowerDabaBase>((uint)playerLv);
        if (db != null)
        {
            return db.FightPower;
        }
        else
        {
            return 0;
        }
    }

    /// <summary>
    /// 获得系统评分
    /// </summary>
    public string GetSystemScore()
    {
        string recommend = string.Empty;

        uint recommendFightPower = GetRecommendFightPower();
        if (recommendFightPower == 0)
        {
            return recommend;
        }

        IPlayer player = Client.ClientGlobal.Instance().MainPlayer;
        if (player != null)
        {
            int mainPlayerFightPower = player.GetProp((int)FightCreatureProp.Power);

            float value = (float)mainPlayerFightPower / recommendFightPower;

            if (value >= 0.9f)
            {
                recommend = "s";
            }
            else if (value >= 0.8f && value < 0.9f)
            {
                recommend = "a";
            }
            else if (value >= 0.7f && value < 0.8f)
            {
                recommend = "b";
            }
            else if (value < 0.7f)
            {
                recommend = "c";
            }
        }

        return recommend;
    }

    /// <summary>
    /// 战力里面  获得最佳推荐id
    /// </summary>
    /// <returns></returns>
    public uint GetBestRecommendId()
    {
        List<GrowUpFightPowerDabaBase> list = GetGrowUpFightPowerGridList();

        uint bestRecommendId = list.Count > 0 ? list[0].dwID : 0;

        float tempValue = GetFightPowerSliderValueById(bestRecommendId);

        for (int i = 0; i < list.Count; i++)
        {
            if (GetFightPowerSliderValueById(list[i].dwID) < tempValue)
            {
                tempValue = GetFightPowerSliderValueById(list[i].dwID);
                bestRecommendId = list[i].dwID;
            }
        }

        return bestRecommendId;
    }

    #endregion
}

