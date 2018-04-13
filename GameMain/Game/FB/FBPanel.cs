//*************************************************************************
//	创建日期:	2016/10/18 17:22:10
//	文件名称:	FBPanel
//   创 建 人:   zhuidanyu	
//	版权所有:	中青宝
//	说    明:	副本界面
//*************************************************************************


using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using Client;
using Engine.Utility;
using table;
using GameCmd;
using DG.Tweening;
using Vector2 = UnityEngine.Vector2;
public struct FBUIShowData
{
    public uint copyID;
}
partial class FBPanel
{
    Vector3 m_initVecPos = new Vector3(-52, -22, 0);
    Vector3 m_endPos = Vector3.zero;
    public float m_fAniTime = 0.3f;

    Transform m_widget_Panel;
    Transform m_trans_lingpai;
    //从跳转界面传入的id
    uint m_uGotoCopyID = 0;
    float m_scrollDelta = 0;
    float m_fScrollInitX = 365;
    float itemWidth = 260f;
    public float m_fbackSpeed = 5000;
    public float m_fbackTime = 0.2f;

    bool bShowInfo = false;
    int m_nItemIndex = 0;
    bool IsTweening = false;
    string m_clickName = string.Empty;
    ComBatCopyDataManager dataManager
    {
        get
        {
            return DataManager.Manager<ComBatCopyDataManager>();
        }
    }
    protected override void OnLoading()
    {
        base.OnLoading();
        m_widget_Panel = m_widget_FBCard.transform.Find("Panel");
        m_trans_lingpai = m_widget_FBCard.transform.Find("lingpai");
        m_scrollDelta = -m_grid_FbGrid.transform.localPosition.x - m_scrollview_FbScrollView.transform.localPosition.x;
        m_fScrollInitX = m_scrollview_FbScrollView.transform.localPosition.x;
        itemWidth = m_grid_FbGrid.cellWidth;
    }
    protected override void OnInitPanelData()
    {
        base.OnInitPanelData();
    }
    protected override void OnShow(object data)
    {
        base.OnShow(data);
    }
    public override bool OnTogglePanel(int tabType, int pageid)
    {
        base.OnTogglePanel(tabType, pageid);
        return true;

    }
    void InitCopyNum()
    {
        List<CopyDataBase> copyList = dataManager.GetCopyListByCopyByCopyID(m_uGotoCopyID);
        if (copyList == null)
        {
            m_label_frequency_Plot.text = "";
            return;
        }
        string frequenceLabel = "今日已完成次数:";
        if (copyList.Count == 1)
        {
            CopyDataBase db = copyList[0];
            CopyInfo info = dataManager.GetCopyInfoById(db.copyId);

            uint useNum;
            uint maxNum;
            if (DataManager.Manager<ComBatCopyDataManager>().IsCampCopy(db.copyId))
            {
                useNum = DataManager.Manager<CampCombatManager>().EnterCampTimes;
                maxNum = DataManager.Manager<CampCombatManager>().CampCopyMaxNum;
            }
            else
            {
                if (info != null)
                {
                    useNum = info.CopyUseNum;
                    maxNum = info.MaxCopyNum;
                }
                else
                {
                    useNum = 0;
                    maxNum = db.numMax;
                }
            }

            string desStr = useNum + "/" + maxNum;
            m_label_frequency_Plot.text = desStr;

            //if (info != null)
            //{
            //    uint num = info.CopyUseNum;
            //    string str = num + "/" + info.MaxCopyNum;
            //    m_label_frequency_Plot.text = str;
            //}
            //else
            //{
            //    m_label_frequency_Plot.text = 0 + "/" + db.numMax;
            //}
            if (db.forbitWhat == 1)
            {
                frequenceLabel = "今日已收益次数:";
            }
        }
        if (copyList.Count > 1)
        {
            CopyDataBase temp = copyList[0];
            if (temp == null)
            {
                return;
            }
            int num = 0;
            int maxNum = (int)temp.numMax;
            for (int i = 0; i < copyList.Count; i++)
            {
                CopyDataBase db = copyList[i];
                CopyInfo copyInfo = dataManager.GetCopyInfoById(db.copyId);
                if (copyInfo != null)
                {
                    num = (int)copyInfo.CopyUseNum;
                    maxNum = (int)copyInfo.MaxCopyNum;
                    if (db.forbitWhat == 1)
                    {
                        frequenceLabel = "今日已收益次数:";
                    }
                    break;
                }

            }
            string str = num + "/" + maxNum;
            m_label_frequency_Plot.text = str;
        }
        m_label_frequency_label.text = frequenceLabel;
    }
    protected override void OnDisable()
    {
        base.OnDisable();
    }

    private void UpdateExchangeBtnVisbile()
    {
        bool visible = true;
     //   if (dataManager.IsSingleShowCard(m_uGotoCopyID))
        {
            CopyDataBase showDb = GameTableManager.Instance.GetTableItem<CopyDataBase>(m_uGotoCopyID);
            if (null == showDb || showDb.copyFlag != (uint)CopyFlag.Juqing)
            {
                visible = false;
            }
        }
        if (null != m_btn_EquipRechange && m_btn_EquipRechange.gameObject.activeSelf != visible)
        {
            m_btn_EquipRechange.gameObject.SetActive(visible);
        }
    }

    bool ShowSingleCard()
    {
        if (dataManager.IsSingleShowCard(m_uGotoCopyID))
        {
            m_scrollview_FbScrollView.gameObject.SetActive(false);
            m_widget_Panel.gameObject.SetActive(true);
            m_widget_FBCard.gameObject.SetActive(true);

            FBCard fb = m_widget_FBCard.gameObject.GetComponent<FBCard>();
            if (fb == null)
            {
                fb = m_widget_FBCard.gameObject.AddComponent<FBCard>();
            }
            //fb.DestroyScrollChild();
            CopyDataBase showDb = GameTableManager.Instance.GetTableItem<CopyDataBase>(m_uGotoCopyID);

            if (showDb != null)
            {
                fb.InitByCopyData(showDb);
            }
            UIEventListener.Get(m_widget_FBCard.gameObject).onClick = null;
            return true;
        }
        return false;
    }
    void InitFbScroll()
    {
        UpdateExchangeBtnVisbile();
        if (ShowSingleCard())
        {
            return;
        }
        m_grid_FbGrid.transform.DestroyChildren();
        m_scrollview_FbScrollView.gameObject.SetActive(true);
        float y = m_scrollview_FbScrollView.transform.localPosition.y;
        m_scrollview_FbScrollView.transform.localPosition = new Vector3(m_fScrollInitX, y, 0);
        m_scrollview_FbScrollView.panel.clipOffset = Vector2.zero;

        List<CopyDataBase> copyList = dataManager.GetCopyListByCopyByCopyID(m_uGotoCopyID);
        if (copyList == null)
        {
            return;
        }
        int chidlCount = m_grid_FbGrid.transform.childCount;
        if (chidlCount > copyList.Count)
        {
            for (int i = copyList.Count; i < chidlCount; i++)
            {
                string name = string.Format("{0:D2}", i);

                Transform itemTrans = m_grid_FbGrid.transform.Find(name);
                if (itemTrans != null)
                {
                    itemTrans.gameObject.SetActive(false);
                }
            }
        }
        for (int i = 0; i < copyList.Count; i++)
        {
            string name = string.Format("{0:D2}", i);
            GameObject item = null;

            Transform itemTrans = m_grid_FbGrid.transform.Find(name);
            if (itemTrans == null)
            {
                item = NGUITools.AddChild(m_grid_FbGrid.gameObject, m_widget_FBCard.gameObject);
                item.SetActive(true);
                item.name = name;
                Vector3 localPos = new Vector3(i * itemWidth, 0, 0);
                item.transform.localPosition = localPos;
            }
            else
            {
                item = itemTrans.gameObject;
            }
            item.gameObject.SetActive(true);
            Transform subPanel = item.transform.Find("Panel");
            if (subPanel != null)
            {
                subPanel.gameObject.SetActive(true);
                subPanel.DOScaleX(0, 0);
            }
            CopyDataBase cdb = copyList[i];
            FBCard fb = item.GetComponent<FBCard>();
            if (fb == null)
            {
                fb = item.AddComponent<FBCard>();

            }
            fb.DestroyScrollChild();
            fb.InitByCopyData(cdb);

            UIEventListener.Get(item).onClick = OnItemClick;
        }

        m_grid_FbGrid.Reposition();

    }
    void OnShowItemPanel(string itemName)
    {
        if (string.IsNullOrEmpty(itemName))
        {
            return;
        }
        Transform itemTrans = m_grid_FbGrid.transform.Find(itemName);

        if (itemTrans == null)
        {
            return;
        }
        GameObject go = itemTrans.gameObject;
        int index = 0;
        if (int.TryParse(itemName, out index))
        {
            m_nItemIndex = index;
        }

        if (IsTweening)
        {
            return;
        }
        IsTweening = true;

        List<CopyDataBase> copyList = dataManager.GetCopyListByCopyByCopyID(m_uGotoCopyID);
        if (index < copyList.Count)
        {
            CopyDataBase db = copyList[index];
            if (db != null)
            {
                FBCard fb = go.GetComponent<FBCard>();
                if (fb == null)
                {
                    fb = m_widget_FBCard.gameObject.AddComponent<FBCard>();
                }

                fb.InitByCopyData(db);
            }
        }
        UIDragScrollView drag = go.GetComponent<UIDragScrollView>();
        InitCopyNum();
        Transform subPanel = go.transform.Find("Panel");
        if (!bShowInfo)
        {
            drag.enabled = false;

            subPanel.DOScaleX(0, 0);
            m_endPos = GetScrollItemPosOnParent(go.transform.localPosition, index);
            subPanel.DOScaleX(1, m_fbackTime).OnComplete(() =>
            {
                bShowInfo = true;
                IsTweening = false;
            });
            Log.LogGroup("ZDY", " pos is {0}", m_endPos);
            for (int i = 0; i <= index; i++)
            {
                string itemname = string.Format("{0:D2}", i);
                Transform item = m_grid_FbGrid.transform.Find(itemname);
                if (item != null)
                {
                    float offset = item.localPosition.x - m_endPos.x;
                    item.DOLocalMoveX(offset, m_fbackTime);

                }

            }
            int totoalCount = m_grid_FbGrid.transform.childCount;
            for (int j = index + 1; j < totoalCount; j++)
            {
                string itemname = string.Format("{0:D2}", j);
                Transform item = m_grid_FbGrid.transform.Find(itemname);
                if (item != null)
                {
                    float offset = item.localPosition.x + ((3 * (itemWidth + 12)) - m_endPos.x);
                    item.DOLocalMoveX(offset, m_fbackTime);

                }
            }

        }
        else
        {
            subPanel.DOScaleX(0, m_fbackTime).OnComplete(() =>
            {
                bShowInfo = false;
                IsTweening = false;
                drag.enabled = true;
            });
            for (int i = 0; i <= index; i++)
            {
                string itemname = string.Format("{0:D2}", i);
                Transform item = m_grid_FbGrid.transform.Find(itemname);
                if (item != null)
                {
                    float offset = item.localPosition.x + m_endPos.x;
                    item.DOLocalMoveX(offset, m_fbackTime);
                }

            }
            int totoalCount = m_grid_FbGrid.transform.childCount;
            for (int j = index + 1; j < totoalCount; j++)
            {
                string itemname = string.Format("{0:D2}", j);
                Transform item = m_grid_FbGrid.transform.Find(itemname);
                if (item != null)
                {
                    float offset = item.localPosition.x - ((3 * (12 + itemWidth)) - m_endPos.x);
                    item.DOLocalMoveX(offset, m_fbackTime);
                }
            }
        }
    }
    void OnItemClick(GameObject go)
    {
        m_clickName = go.name;
        OnShowItemPanel(go.name);
    }
    Vector3 GetScrollItemPosOnParent(Vector3 localPosition, int index)
    {

        float delta = index * itemWidth;// -offset.x;
        float gridOffset = -m_grid_FbGrid.transform.localPosition.x - m_scrollview_FbScrollView.transform.localPosition.x - m_scrollDelta;
        delta = delta - gridOffset;

        float x = delta;
        float y = m_scrollview_FbScrollView.transform.localPosition.y;
        return new Vector3(x, y, 0);
    }
    void RefreshUIByFlag()
    {

        InitFbScroll();
        InitCopyNum();
    }
    protected override void OnJump(UIPanelBase.PanelJumpData jumpData)
    {
        if (null == jumpData)
        {
            jumpData = new PanelJumpData();
        }
        int firstTabData = -1;
        int secondTabData = -1;
        if (null != jumpData.Tabs && jumpData.Tabs.Length > 0)
        {
            firstTabData = jumpData.Tabs[0];

        }
        else
        {
            firstTabData = 1;
        }
        if (jumpData.Param != null && jumpData.Param is uint)
        {
            m_uGotoCopyID = (uint)jumpData.Param;
            m_clickName = (string)jumpData.ExtParam;
            m_widget_Panel.gameObject.SetActive(false);
            m_widget_FBCard.gameObject.SetActive(false);
            IsTweening = false;
            bShowInfo = false;
            IsTweening = false;

            InitFbScroll();

            InitCopyNum();
            OnShowItemPanel(m_clickName);
        }
        else
        {
            Engine.Utility.Log.Error("该副本跳转途径参数3配置有误");
        }

        //        UIFrameManager.Instance.OnCilckTogglePanel(this.PanelId, 1, firstTabData);
    }
    public override UIPanelBase.PanelData GetPanelData()
    {
        PanelData pd = base.GetPanelData();
        pd.JumpData = new PanelJumpData();
        pd.JumpData.Param = m_uGotoCopyID;
        pd.JumpData.ExtParam = m_clickName;
        pd.Data = false;

        return pd;
    }

    void onClick_EquipRechange_Btn(GameObject caster)
    {
        DataManager.Manager<UIPanelManager>().ShowPanel(PanelID.EquipExchangePanel);
    }

    public override void Release(bool depthRelease = true)
    {
        base.Release(depthRelease);
    }


}

