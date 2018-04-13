using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GameCmd;
using UnityEngine;
using table;



class UIOpenServerGrid : UIGridBase
{

    UILabel m_lab_title;
    UISprite m_spr_icon;
    UISprite m_spr_mask;
    UILabel m_lab_content1;
    UILabel m_lab_content2;
    UISprite m_btn_getNow;
    UISprite m_spr_getTomorrow;
    UITexture m_spr_select;
    Transform particle;
    public uint CurrentDay
    {
        set;
        get;
    }

    Transform m_trans_UIItemRewardGrid;
    UIGridCreatorBase m_ctor;
    protected override void OnAwake()
    {
        base.OnAwake();
        m_lab_title = CacheTransform.Find("title").GetComponent<UILabel>();
        m_lab_content1 = CacheTransform.Find("content1").GetComponent<UILabel>();
        m_lab_content2 = CacheTransform.Find("content2").GetComponent<UILabel>();

        m_spr_icon = CacheTransform.Find("icon").GetComponent<UISprite>();
        m_spr_mask = CacheTransform.Find("mark").GetComponent<UISprite>();
        m_btn_getNow = CacheTransform.Find("GetNow").GetComponent<UISprite>();
        m_spr_getTomorrow = CacheTransform.Find("GetTomorrow").GetComponent<UISprite>();
        m_spr_select = CacheTransform.Find("select").GetComponent<UITexture>();
        particle  = CacheTransform.Find("Particle");
        UIEventListener.Get(m_btn_getNow.gameObject).onClick = OnClickBtnGetNow;

        m_trans_UIItemRewardGrid = CacheTransform.Find("UIItemRewardGrid");
        AddCreator(m_spr_icon.transform);
      
    }
    CMResAsynSeedData<CMAtlas> m_playerAvataCASD= null;

    public override void SetGridData(object data)
    {
        base.SetGridData(data);
        if (data != null && data is uint )
        {
            CurrentDay = (uint)data;
            OpenServerDataBase tableData = GameTableManager.Instance.GetTableItem<OpenServerDataBase>(CurrentDay);
            if (tableData != null)
            {
                m_lab_title.text = tableData.name;
                m_lab_content1.text = tableData.desc1;
                m_lab_content2.text = tableData.desc2;
                list.Clear();
                list.Add(new UIItemRewardData() 
                {
                    itemID = tableData.reward_id,
                    num = 0,
                });
                m_ctor.CreateGrids(list.Count);
                SetSeclect(false);
            }                   
        }      
    }


    public void SetSeclect(bool value) 
    {
        List<uint> openList = DataManager.Manager<WelfareManager>().OpenPackageList;
        bool hasGot = openList.Contains(CurrentDay); 
        bool isCanGet = CurrentDay <= DataManager.Manager<WelfareManager>().NewServerLoginTimes;
        bool isTomorrow = (CurrentDay) == DataManager.Manager<WelfareManager>().NewServerLoginTimes + 1;
        m_spr_mask.gameObject.SetActive(hasGot);
        m_btn_getNow.gameObject.SetActive(!hasGot && isCanGet && value);
        m_spr_getTomorrow.gameObject.SetActive(!hasGot && isTomorrow && value);
        CacheTransform.localScale = value ? new Vector3(1.1f, 1.1f, 1f) : Vector3.one;

        PlayEffect(!hasGot && isCanGet && value);

    }
    void PlayEffect(bool show)
    {
        if (particle != null)
        {
            UIParticleWidget p = particle.GetComponent<UIParticleWidget>();
            if (p == null)
            {
                p = particle.gameObject.AddComponent<UIParticleWidget>();
                p.depth = 20;
            }
            if (p != null)
            {

                p.SetDimensions(1,1);
                p.ReleaseParticle();
                if (show)
                {
                    p.AddParticle(50036);
                }
            }
        }
    }

    void OnClickBtnGetNow(GameObject go) 
    {
        NetService.Instance.Send(new stOpenPackageSignDataUserCmd_CS() { id = CurrentDay });
    }

    public override void Release(bool depthRelease = true)
    {
        base.Release(depthRelease);
        if (m_playerAvataCASD != null)
        {
            m_playerAvataCASD.Release(depthRelease);
            m_playerAvataCASD = null;
        }
    }
    List<UIItemRewardData> list = new List<UIItemRewardData>();
    void AddCreator(Transform parent)
    {
        if (parent != null)
        {
            m_ctor = parent.GetComponent<UIGridCreatorBase>();
            if (m_ctor == null)
            {
                m_ctor = parent.gameObject.AddComponent<UIGridCreatorBase>();
            }
            m_ctor.arrageMent = UIGridCreatorBase.Arrangement.Horizontal;
            m_ctor.gridWidth = 90;
            m_ctor.gridHeight = 90;
            m_ctor.RefreshCheck();
            m_ctor.Initialize<UIItemRewardGrid>(m_trans_UIItemRewardGrid.gameObject, OnUpdateGridData, null);
        }
    }
    void OnUpdateGridData(UIGridBase grid, int index)
    {
        if (grid is UIItemRewardGrid)
        {
            UIItemRewardGrid itemShow = grid as UIItemRewardGrid;
            if (itemShow != null)
            {
                if (index < list.Count)
                {
                    UIItemRewardData data = list[index];
                    uint itemID = data.itemID;
                    uint num = data.num;
                    itemShow.SetGridData(itemID, num, false, false, false);
                    itemShow.SetBgAndBorder(false);
                }
            }
        }
    }
//    void CreatObj(Transform trans, uint itemID, uint num, int i)
//     {       
//         if (null == m_trans_UIItemRewardGrid)
//         {
//             return;
//         }
//         if (null == m_lst_award)
//         {
//             m_lst_award = new List<UIItemRewardGrid>();
//         }
//         GameObject cloneObj = NGUITools.AddChild(trans.gameObject, m_trans_UIItemRewardGrid.gameObject);
//         UIItemRewardGrid itemShow = cloneObj.transform.GetComponent<UIItemRewardGrid>();
//         if (itemShow == null)
//         {
//             itemShow =  cloneObj.AddComponent<UIItemRewardGrid>();
//         }
// 
//         itemShow.MarkAsParentChanged();
//         itemShow.gameObject.SetActive(true);
//         itemShow.SetGridData(itemID, num, false, false, false);
//         itemShow.SetBgAndBorder(false);
//         m_lst_award.Add(itemShow);
//     }
}
