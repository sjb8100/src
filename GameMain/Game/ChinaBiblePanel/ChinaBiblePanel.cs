using System;
using Common;
using table;
using UnityEngine;
using System.Collections.Generic;
partial class ChinaBiblePanel:UIPanelBase
{
    public class FeedBackNotice 
    {
        public int index;
        public string title;
        public string content;
    }
    private UISecondTabCreatorBase mSecondTabCreator = null;
    Dictionary<uint, List<BibleDataBase>> bibleDic = new Dictionary<uint, List<BibleDataBase>>();
    Dictionary<uint, List<uint>> m_dic = new Dictionary<uint, List<uint>>();
    private List<uint> mlstFirstTabIds = null;
    List<BibleDataBase> temp = null;
    private uint m_uint_activeFType = 0;
    private uint m_uint_activeStype = 0;
    uint selectSecondsKey = 0;
    UIXmlRichText m_UIXmlRichText = null;
    List<FeedBackNotice> m_lstNotice = new List<FeedBackNotice>();
    protected override void OnLoading()
    {
        base.OnLoading();
        List<BibleDataBase> list = GameTableManager.Instance.GetTableList<BibleDataBase>();

        for (int i = 0; i < list.Count; i++)
        {
            if (!bibleDic.ContainsKey(list[i].firID))
            {
                temp = new List<BibleDataBase>();
                temp.Add(list[i]);
                bibleDic.Add(list[i].firID, temp);
            }
            else
            {
                bibleDic[list[i].firID].Add(list[i]);
            }
        }
        if (m_scrollview_TypeScrollView != null)
        {

            if (mSecondTabCreator == null)
            {
                mSecondTabCreator = m_scrollview_TypeScrollView.gameObject.AddComponent<UISecondTabCreatorBase>();
            }
            if (null != mSecondTabCreator)
            {
//                 GameObject cloneFTemp = UIManager.GetResGameObj(GridID.Uictrtypegrid) as GameObject;
//                 GameObject cloneSTemp = UIManager.GetResGameObj(GridID.Uisecondtypegrid) as GameObject;
                mSecondTabCreator.Initialize<UISecondTypeGrid>(m_trans_UICtrTypeGrid.gameObject, m_widget_UISecondTypeGrid.gameObject
                    , onUpdateGrid, OnUpdateSecondTabGrid, OnGridUIEvent);
            }
            List<int> secondTabsNums = new List<int>();
            if (null == mlstFirstTabIds)
            {
                mlstFirstTabIds = new List<uint>();
            }
            if (m_dic == null)
            {
                m_dic = new Dictionary<uint, List<uint>>();
            }
            mlstFirstTabIds.Clear();
            m_dic.Clear();
            foreach (var i in bibleDic)
            {
                mlstFirstTabIds.Add(i.Key);
                secondTabsNums.Add(i.Value.Count);
                for (int a = 0; a < i.Value.Count; a++)
                {
                    if (m_dic.ContainsKey(i.Key))
                    {
                        m_dic[i.Key].Add(i.Value[a].secID);
                    }
                    else
                    {
                        List<uint> li = new List<uint>();
                        li.Add(i.Value[a].secID);
                        m_dic.Add(i.Key, li);
                    }
                }
            }
            if (null != mSecondTabCreator)
            {
                mSecondTabCreator.CreateGrids(secondTabsNums);
            }
        }
        ParseFromFile();
    }
    public override void Release(bool depthRelease = true)
    {
        base.Release(depthRelease);
        if (mSecondTabCreator != null)
        {
            mSecondTabCreator.Release(depthRelease);
        }
        if (bibleDic != null)
        {
            bibleDic.Clear();
        }
        if (m_dic != null)
        {
            m_dic.Clear();
        }
        if (mlstFirstTabIds != null)
        {
            mlstFirstTabIds.Clear();
        }
        if (temp != null)
        {
            temp.Clear();
        }
        if (m_UIXmlRichText != null)
        {
            m_UIXmlRichText = null;
        }
        if (m_lstNotice != null)
        {
            m_lstNotice.Clear();
        }
    }
    protected override void OnPanelBaseDestory()
    {
        base.OnPanelBaseDestory();

        Release();
    }
    protected  override void OnShow(object data) 
    {
        base.OnShow(data);
    }
    protected override void OnHide()
    {
        base.OnHide();
    }
    public void onUpdateGrid(UIGridBase grid, int index)
    {
        if (grid is UICtrTypeGrid)
       {
           UICtrTypeGrid tab = grid as UICtrTypeGrid;
           List<BibleDataBase> d = bibleDic[mlstFirstTabIds[index]];
           tab.SetData(mlstFirstTabIds[index], d[0].firName, d.Count);
           tab.SetGridData((uint)mlstFirstTabIds[index]);

       }
    }
    private void OnUpdateSecondTabGrid(UIGridBase grid, object id, int index)
    {
        if (grid is UISecondTypeGrid)
        {
            UISecondTypeGrid sGrid = grid as UISecondTypeGrid;
            sGrid.SetRedPoint(false);
            List<BibleDataBase> list = bibleDic[(uint)id];
            sGrid.SetData(list[index].secID, list[index].secName, m_uint_activeStype == m_dic[m_uint_activeFType][index]);
        }
    }
    public void OnGridUIEvent(UIEventType eventType , object data,object param) 
    {
       switch(eventType)
       {
           case UIEventType.Click: 
               {
                   if (data is UISecondTypeGrid)
                   {
                       UISecondTypeGrid sec = data as UISecondTypeGrid;
                       SetSelectSecondType(sec.Data);
                   }
         
                   if (data is UICtrTypeGrid)
                   {
                       UICtrTypeGrid tabGrid = data as UICtrTypeGrid;
                       SetSelectFirstType((uint)tabGrid.ID);
                   }
               }
               break;
       }
    }
    private void SetSelectFirstType(uint type, bool force = false)
    {
        if (null == mSecondTabCreator)
        {
            return;
        }
        if (m_uint_activeFType == type && !force)
        {
            mSecondTabCreator.DoToggle(mlstFirstTabIds.IndexOf(m_uint_activeFType), true, true);
            return;
        }
        m_uint_activeFType = type;
        mSecondTabCreator.Open(mlstFirstTabIds.IndexOf(m_uint_activeFType), true);
        selectSecondsKey = m_dic[m_uint_activeFType][0];
        SetSelectSecondType(selectSecondsKey, true);
    }
private void SetSelectSecondType(uint type, bool force = false)
{
        if (null == mSecondTabCreator)
        {
            return;
        }
        if (m_uint_activeStype == type && !force)

            return;
        UISecondTypeGrid sGrid = null;
        if ( m_uint_activeFType != 0)
        {
            sGrid = mSecondTabCreator.GetGrid<UISecondTypeGrid>(mlstFirstTabIds.IndexOf(m_uint_activeFType), m_dic[m_uint_activeFType].IndexOf(m_uint_activeStype));
            if (null != sGrid)
            {
                sGrid.SetHightLight(false);
            }
        }

        m_uint_activeStype = type;
        if ( m_uint_activeFType != 0)
        {
            sGrid = mSecondTabCreator.GetGrid<UISecondTypeGrid>(mlstFirstTabIds.IndexOf(m_uint_activeFType), m_dic[m_uint_activeFType].IndexOf(m_uint_activeStype));
            if (null != sGrid)
            {
                sGrid.SetHightLight(true);
            }
        }
        m_UIXmlRichText.Clear();
        BibleDataBase table = GameTableManager.Instance.GetTableItem<BibleDataBase>(m_uint_activeFType, (int)m_uint_activeStype);
        if (table == null)
        {
            Engine.Utility.Log.Warning("华夏宝典表格大类{0}-小类{1}数据为空！", m_uint_activeFType, m_uint_activeStype);
            return;
        }
        string content = table.content;
        m_UIXmlRichText.AddXml(RichXmlHelper.RichXmlAdapt(content));
    
}

    void ParseFromFile() 
    {
        m_UIXmlRichText = m_widget_root.GetComponent<UIXmlRichText>();
        UnityEngine.Object prefab = UIManager.GetResGameObj("prefab/grid/richtextlabel.unity3d", "Assets/UI/prefab/Grid/RichTextLabel.prefab");
        m_UIXmlRichText.protoLabel = prefab as GameObject;
        m_UIXmlRichText.fontSize = 24;
        string strFilePath = "notice.json";
        Engine.JsonNode root = Engine.RareJson.ParseJsonFile(strFilePath);
        if(root == null)
        {
            Engine.Utility.Log.Warning("华夏宝典解析{0}文件失败！", strFilePath);
            return;
        }
        Engine.JsonArray noticeArray = (Engine.JsonArray)root["notice"];
        for (int i = 0, imax = noticeArray.Count; i < imax;i++ )
        {
            Engine.JsonObject noticeObj = (Engine.JsonObject)noticeArray[i];
            if(noticeObj == null)
            {
                continue;
            }
            FeedBackNotice notice = new FeedBackNotice();
            notice.index = int.Parse(noticeObj["index"]);
            notice.title = noticeObj["title"];
            notice.content = noticeObj["content"];
            m_lstNotice.Add(notice);

        }
    }

    void onClick_CloseBtn_Btn(GameObject caster)
    {
        HideSelf();
    }


}
