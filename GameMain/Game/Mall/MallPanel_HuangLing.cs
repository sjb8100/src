using UnityEngine;
using Client;
using GameCmd;
using table;
using Engine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Engine.Utility;
using Common;
 partial class MallPanel:UIPanelBase
{
     List<uint> m_lst_nobleID = new List<uint>();
     int curSelectIndex = 0;
    void CreateNobleGrids() 
    {
        List<NobleDataBase> noble = GameTableManager.Instance.GetTableList<NobleDataBase>();
        for(int i = 0 ; i < noble.Count;i++)
        {
           if(!m_lst_nobleID.Contains(noble[i].dwID) && noble[i].invaild ==1)
           {
               m_lst_nobleID.Add(noble[i].dwID);
           }
        }
        m_ctor_NobleContentRoot.CreateGrids(m_lst_nobleID.Count);
    }
    void RefreshNobleGrid(uint nobleID) 
    {
        if (m_lst_nobleID.Contains(nobleID))
        {
            m_ctor_NobleContentRoot.UpdateData(m_lst_nobleID.IndexOf(nobleID));
        }
    }
    private void OnNobleGridDataUpdate(UIGridBase data, int index) 
    {
        if(data is UINobleGrid)
        {
            UINobleGrid nb = data as UINobleGrid;
            if (index < m_lst_nobleID.Count)
            {
                NobleDataBase tab = GameTableManager.Instance.GetTableItem<NobleDataBase>(m_lst_nobleID[index]);
                if (tab != null)
                {
                    nb.SetGridData(m_lst_nobleID[index]);
                    nb.SetNobleGridData(tab);
                    nb.SetBtn(DataManager.Manager<Mall_HuangLingManager>().NobleDic, tab);
                }
            }
        }
    }
    private void OnNobleGridEvent(UIEventType eventType, object data, object param)
    {
        switch (eventType)
        {
            case UIEventType.Click:
                if (data is UINobleGrid)
                {
                    UINobleGrid nb = data as UINobleGrid;
                    if (isShowDetail)
                    {
                        NobleDataBase table = GameTableManager.Instance.GetTableItem<NobleDataBase>(nb.nobleID);
                        if (table != null)
                        {
                            m_label_TextContent.text = table.des;
                            m_sprite_prerogative.GetComponent<UISprite>().height = (int)table.BgHeight;
                        }
                      
                    }
                }
                break;
        }
    }
    private bool isShowDetail = false;
    void onClick_TipBtn_Btn(GameObject caster)
    {
        if (!isShowDetail)
        {
            m_sprite_prerogative.gameObject.SetActive(true);
            if (curSelectIndex < m_lst_nobleID.Count)
            {
                NobleDataBase data = GameTableManager.Instance.GetTableItem<NobleDataBase>(m_lst_nobleID[curSelectIndex]);
                if (data != null)
                {
                    m_label_TextContent.text = data.des;
                    m_sprite_prerogative.GetComponent<UISprite>().height = (int)data.BgHeight;
                    isShowDetail = true;
                }
            }                 
        }
        else 
        {
            m_sprite_prerogative.gameObject.SetActive(false);
            isShowDetail = false;
        }
      
    }
    void onClick_Container_Btn(GameObject caster)
    {
        if (isShowDetail)
        {
            m_sprite_prerogative.gameObject.SetActive(false);
            isShowDetail = false;
        }
    }
}
