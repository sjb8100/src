/************************************************************************************
 * Copyright (c) 2016  All Rights Reserved.
 * CLR版本： 4.0.30319.42000
 * 公司名称：ZQB
 * 命名空间：GameMain.Game.Item
 * 创建人：  wenjunhua.zqgame
 * 文件名：  ComposeManager
 * 版本号：  V1.0.0.0
 * 创建时间：11/4/2016 9:57:20 AM
 * 描述：
 ************************************************************************************/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

//分类数据
public class CategoryTypeData
{
    public enum CategoryType
    {
        First = 1,
        Second = 2,
    }
    //分类id
    public uint m_uint_categoryId = 0;
    //分类名称
    public string m_str_categoryName = "";

    private List<uint> m_list_categroyDatas = new List<uint>();

    public CategoryTypeData(uint id, string name)
    {
        this.m_uint_categoryId = id;
        this.m_str_categoryName = name;
    }

    //添加数据
    public bool Add(uint id)
    {
        if (!m_list_categroyDatas.Contains(id))
        {
            m_list_categroyDatas.Add(id);
            return true;
        }
        return false;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public bool Contains(uint id)
    {
        return m_list_categroyDatas.Contains(id);
    }


    /// <summary>
    /// 获取数据列表
    /// </summary>
    /// <returns></returns>
    public List<uint> GetDatas()
    {
        List<uint> datas = new List<uint>();
        if (null != m_list_categroyDatas
            && m_list_categroyDatas.Count != 0)
        {
            datas.AddRange(m_list_categroyDatas);
        }
        datas.Sort();
        return datas;
    }

    public uint GetDataByIndex(int index)
    {
        uint id = 0;
        List<uint> datas = GetDatas();
        if (datas.Count > index)
        {
            id = datas[index];
        }
        return id;
    }

    public int IndexOf(uint id)
    {
        int index = 0;
        List<uint> datas = GetDatas();
        return datas.IndexOf(id);
    }

    public int Count
    {
        get
        {
            return m_list_categroyDatas.Count;
        }
    }

}
class ComposeManager : BaseModuleData, IManager
{
    #region property
    //一级分类数据
    private Dictionary<uint, CategoryTypeData> m_dic_fTypeDatas = null;
    //二级分类数据
    private Dictionary<uint, CategoryTypeData> m_dic_sTypeDatas = null;
    #endregion


    #region IManager Method
    public void Initialize()
    {
        if (null == m_dic_fTypeDatas)
        {
            m_dic_fTypeDatas = new Dictionary<uint, CategoryTypeData>();
        }
        m_dic_fTypeDatas.Clear();
        if (null == m_dic_sTypeDatas)
        {
            m_dic_sTypeDatas = new Dictionary<uint, CategoryTypeData>();
        }
        m_dic_sTypeDatas.Clear();

        //构建合成数据
        List<table.ComposeDataBase> composeDatas 
            = GameTableManager.Instance.GetTableList<table.ComposeDataBase>();
        if (null == composeDatas)
        {
            Engine.Utility.Log.Error("ComposeManager Initialize failed,datas error");
            return;
        }
        uint buildKey = 0;
        foreach (table.ComposeDataBase data in composeDatas)
        {
            buildKey = BuildComposeSecondsKey(data.fType, data.sType);
            //1级页
            if (!m_dic_fTypeDatas.ContainsKey(data.fType))
            {
                m_dic_fTypeDatas.Add(data.fType, new CategoryTypeData(data.fType,data.fTypeName));
            }
            if (!m_dic_fTypeDatas[data.fType].Contains(data.sType))
            {
                m_dic_fTypeDatas[data.fType].Add(buildKey);
            }
            //2级页
            if (!m_dic_sTypeDatas.ContainsKey(buildKey))
            {
                m_dic_sTypeDatas.Add(buildKey, new CategoryTypeData(buildKey, data.sTypeName));
            }
            if (data.needShow == 0)
            {
                //表格中不显示
                continue;
            }
            if (!m_dic_sTypeDatas[buildKey].Contains(data.id))
            {
                m_dic_sTypeDatas[buildKey].Add(data.id);
            }
        }
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
    #region Compose

    /// <summary>
    /// 获取所有可以合成的物品id
    /// </summary>
    /// <returns></returns>
    public List<uint> GetCanComposeItemIds()
    {
        List<uint> canComposeDatas = new List<uint>();
        if (null != m_dic_sTypeDatas)
        {
            List<uint> datas = null;
            foreach(CategoryTypeData ctd in m_dic_sTypeDatas.Values)
            {
                datas = ctd.GetDatas();
                if (null == datas || datas.Count == 0)
                {
                    continue;
                }

                for(int i = 0;i < datas.Count;i++)
                {
                    if (IsItemCanCompose(datas[i])
                        && !canComposeDatas.Contains(datas[i]))
                    {
                        canComposeDatas.Add(datas[i]);
                    }
                }
            }
        }
        canComposeDatas.Sort();
        return canComposeDatas;
    }

    /// <summary>
    /// 物品是否满足合成条件
    /// </summary>
    /// <param name="itemId">合成物品</param>
    /// <param name="needReduceCost">是否有需要减少的合成材料</param>
    /// <param name="reduceItemBaseId">需要减少的合成材料id</param>
    /// <param name="reduceItemNum">需要减少合成材料数量</param>
    /// <returns></returns>
    public bool IsItemCanCompose(uint itemId,bool needReduceCost = false,uint reduceItemBaseId = 0,uint reduceItemNum = 0)
    {
        table.ComposeDataBase db = GameTableManager.Instance.GetTableItem<table.ComposeDataBase>(itemId);
        if (null != db)
        {
            int currencyHoldNum = DataManager.Instance.GetCurrencyNumByType((GameCmd.MoneyType)db.moneyType);
            int composeNum = 0;
            int tempComposeNum = 0;
            ItemManager img = DataManager.Manager<ItemManager>();
            uint holdNum = 0;
            //材料1
            if (db.costItem1 != 0 && db.costNum1 > 0)
            {
                holdNum = (uint)img.GetItemNumByBaseId(db.costItem1);
                if (needReduceCost && (reduceItemBaseId == db.costItem1))
                {
                    holdNum += reduceItemNum;
                }
                tempComposeNum = (int)(holdNum / db.costNum1);
                composeNum = tempComposeNum;
            }
            //材料2
            if (db.costItem2 != 0 && db.costNum2 > 0)
            {
                holdNum = (uint)img.GetItemNumByBaseId(db.costItem2);
                if (needReduceCost && (reduceItemBaseId == db.costItem2))
                {
                    holdNum += reduceItemNum;
                }
                tempComposeNum = (int)(holdNum / db.costNum2);
                if (tempComposeNum < composeNum)
                {
                    composeNum = tempComposeNum;
                }
            }
            //材料3
            if (db.costItem3 != 0 && db.costNum3 > 0)
            {
                holdNum = (uint)img.GetItemNumByBaseId(db.costItem3);
                if (needReduceCost && (reduceItemBaseId == db.costItem3))
                {
                    holdNum += reduceItemNum;
                }
                tempComposeNum = (int)(holdNum / db.costNum3);
                if (tempComposeNum < composeNum)
                {
                    composeNum = tempComposeNum;
                }
            }
            //材料4
            if (db.costItem4 != 0 && db.costNum4 > 0)
            {
                holdNum = (uint)img.GetItemNumByBaseId(db.costItem4);
                if (needReduceCost && (reduceItemBaseId == db.costItem4))
                {
                    holdNum += reduceItemNum;
                }
                tempComposeNum = (int)(holdNum / db.costNum4);
                if (tempComposeNum < composeNum)
                {
                    composeNum = tempComposeNum;
                }
            }
            //材料5
            if (db.costItem5 != 0 && db.costNum5 > 0)
            {
                holdNum = (uint)img.GetItemNumByBaseId(db.costItem5);
                if (needReduceCost && (reduceItemBaseId == db.costItem5))
                {
                    holdNum += reduceItemNum;
                }
                tempComposeNum = (int)(holdNum / db.costNum5);
                if (tempComposeNum < composeNum)
                {
                    composeNum = tempComposeNum;
                }
            }
            if (composeNum > 0 
                && currencyHoldNum >= db.costMoney)
            {
                return true;
            }
        }
        return false;
    }

    /// <summary>
    /// 根据分类获取合成列表
    /// </summary>
    /// <param name="ftype">1级分类</param>
    /// <param name="stype">2级分类</param>
    /// <returns></returns>
    public List<uint> GetComposeDatas(uint ftype,uint stype)
    {
        uint buildKey = BuildComposeSecondsKey(ftype, stype);
        return GetTypeDatas(CategoryTypeData.CategoryType.Second,buildKey);
    }

    /// <summary>
    /// 获取对应分类下的数据
    /// </summary>
    /// <param name="type">分类类型</param>
    /// <param name="typeId">分类id</param>
    /// <returns></returns>
    public List<uint> GetTypeDatas(CategoryTypeData.CategoryType type, uint typeId)
    {
        List<uint> datas = new List<uint>();
        CategoryTypeData cTypeData = GetCategoryTypeDataByType(type,typeId);
        if (null != cTypeData)
        {
            datas.AddRange(cTypeData.GetDatas());
        }
        datas.Sort();
        return datas;
    }

    /// <summary>
    /// 获取对应分类下的数据
    /// </summary>
    /// <param name="type">分类类型</param>
    /// <param name="typeId">分类id</param>
    /// <returns></returns>
    public CategoryTypeData GetCategoryTypeDataByType(CategoryTypeData.CategoryType type,uint typeId)
    {
        CategoryTypeData ctData = null;
        if (type == CategoryTypeData.CategoryType.First
            && m_dic_fTypeDatas.TryGetValue(typeId, out ctData))
        {
            return ctData;
        }
        else if (type == CategoryTypeData.CategoryType.Second
            && m_dic_sTypeDatas.TryGetValue(typeId, out ctData))
        {
            return ctData;
        }
        return null;
    }

    /// <summary>
    /// 获取一级分类下的数据
    /// </summary>
    /// <returns></returns>
    public List<CategoryTypeData> GetCategoryTypeDatasByFirstType()
    {
        List<CategoryTypeData> datas = new List<CategoryTypeData>();
        if (m_dic_fTypeDatas.Count != 0)
        {
            datas.AddRange(m_dic_fTypeDatas.Values);
        }
        datas.Add(new CategoryTypeData(0, "可合成"));
        return datas;
    }

    /// <summary>
    /// 根据当前id获取下一级合成id
    /// </summary>
    /// <param name="curComposeId"></param>
    /// <returns></returns>
    public uint GetNextComposeId(uint curComposeId)
    {
        uint nextId = 0;
        table.ComposeDataBase curDB
            = GameTableManager.Instance.GetTableItem<table.ComposeDataBase>(curComposeId);
        if (null != curDB)
        {
            nextId = curDB.nextId;
        }
        return nextId;
    }

    /// <summary>
    /// 是否为合成最高级
    /// </summary>
    /// <param name="composeId"></param>
    /// <returns></returns>
    public bool IsHighestComposeId(uint composeId)
    {
        table.ComposeDataBase curDB
            = GameTableManager.Instance.GetTableItem<table.ComposeDataBase>(composeId);
        if (null != curDB)
        {
            return (curDB.nextId == 0) ? true : false;
        }
        return true;
    }
    /// <summary>
    /// 生成合成分类列表key
    /// </summary>
    /// <param name="fType">一级分类</param>
    /// <param name="sType">二级分类</param>
    /// <returns></returns>
    public uint BuildComposeSecondsKey(uint fType, uint sType)
    {
        return (fType << 16) | sType;
    }
     
    /// <summary>
    /// 进行合成
    /// </summary>
    /// <param name="composeId">合成id</param>
    /// <param name="composeAll">全部合成</param>
    public void DoCompose(uint composeId,bool composeAll = false)
    {
        if (null != DataManager.Instance.Sender)
        {
            DataManager.Instance.Sender.ItemComposeReq(composeId, (uint)((composeAll) ? 1 : 0));
        }
    }

    /// <summary>
    /// 镶嵌宝石合成
    /// </summary>
    /// <param name="gemBaseId">宝石baseid</param>
    /// <param name="pos">镶嵌位置</param>
    /// <param name="equipGridIndexType"></param>
    public void DoInlayGemCompose(uint gemBaseId,GameCmd.EquipPos pos,EquipManager.EquipGridIndexType equipGridIndexType)
    {
        if (null != DataManager.Instance.Sender)
        {
            DataManager.Instance.Sender.ItemComposeReq(gemBaseId,2 ,pos,(uint) equipGridIndexType);
        }
    }

    /// <summary>
    /// 合成响应
    /// </summary>
    /// <param name="itemid"></param>
    /// <param name="num"></param>
    /// <param name="composeType"></param>
    public void OnCompose(uint itemid,uint num, uint composeType)
    {
        BaseItem bItem = DataManager.Manager<ItemManager>().GetTempBaseItemByBaseID<BaseItem>(itemid);
        switch(composeType)
        {
            case 0:
            case 1:
                //TipsManager.Instance.ShowTips(string.Format("成功获得{0}x{1}", bItem.Name, num));
                break;
            case 2:
                //镶嵌宝石合成
                Engine.Utility.EventEngine.Instance().DispatchEvent((int)Client.GameEventID.UIEVENTGEMINLAYCHANGED);
                break;
        }
    }
    #endregion
}