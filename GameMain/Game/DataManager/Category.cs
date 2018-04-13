/************************************************************************************
 * Copyright (c) 2017  All Rights Reserved.
 * CLR版本： 4.0.30319.42000
 * 公司名称：ZQB
 * 命名空间：GameMain.Game.DataManager
 * 创建人：  wenjunhua.zqgame
 * 文件名：  Category
 * 版本号：  V1.0.0.0
 * 创建时间：8/15/2017 4:05:00 PM
 * 描述：
 ************************************************************************************/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public class Category
{
    #region property
    private uint m_uId;
    public uint Id
    {
        get
        {
            return m_uId;
        }
    }

    private string m_strName = "";
    public string Name
    {
        get
        {
            return m_strName;
        }
    }

    private Dictionary<uint,int> m_dicCategory = null;
    private List<Category> m_lstChildCategoryData;
    public List<Category> ChildCategoryData
    {
        get
        {
            return m_lstChildCategoryData;
        }
    }

    private List<uint> m_lstDatas = null;
    public List<uint> Datas
    {
        get
        {
            return m_lstDatas;
        }
    }

    #endregion

    #region Op

    public bool TryGetCategory(uint id,out Category category)
    {
        category = null;
        int index = 0;
        if (null != m_dicCategory 
            && m_dicCategory.TryGetValue(id,out index)
            && ChildCategoryData.Count > index)
        {
            category = ChildCategoryData[index];
            return true;
        }
        return false;
    }

    public void AddCategory(Category data)
    {
        if (null == m_dicCategory)
        {
            m_dicCategory = new Dictionary<uint, int>();
        }

        if (m_dicCategory.ContainsKey(data.Id))
        {
            return;
        }

        if (null == m_lstChildCategoryData)
        {
            m_lstChildCategoryData = new List<Category>();
        }
        m_lstChildCategoryData.Add(data);
        m_dicCategory.Add(data.Id, m_lstChildCategoryData.Count - 1);
    }

    public void AddData(uint data)
    {
        if (null == m_lstDatas)
        {
            m_lstDatas = new List<uint>();
        }
        if (!m_lstDatas.Contains(data))
        {
            m_lstDatas.Add(data);
        }
    }

    public void SortCategoryData(Comparison<Category> compareDlg,bool includeChild = true)
    {
        if (null != m_lstChildCategoryData)
        {
            if (null != compareDlg)
                m_lstChildCategoryData.Sort(compareDlg);
            m_dicCategory.Clear();
            for(int i = 0,max = m_lstChildCategoryData.Count ;i < max;i++)
            {
                m_dicCategory.Add(m_lstChildCategoryData[i].Id, i);
                if (includeChild)
                    m_lstChildCategoryData[i].SortCategoryData(compareDlg);
            }
        }
        
    }

    public void SortData(Comparison<uint> compareDlg = null,bool includeChild = true)
    {
        if (null != m_lstDatas)
        {
            if (null != compareDlg)
                m_lstDatas.Sort(compareDlg);
        }
        if (null != m_lstChildCategoryData)
        {
            for (int i = 0, max = m_lstChildCategoryData.Count; i < max; i++)
            {
                if (includeChild)
                    m_lstChildCategoryData[i].SortData(compareDlg);
            }
        }
        
    }

    private Category(uint id, string name)
    {
        this.m_uId = id;
        this.m_strName = name;
    }

    private Category()
    {

    }

    public static Category Create()
    {
        return new Category();
    }

    public static Category Create(uint id, string name)
    {
        return new Category(id, name);
    }
    #endregion


}