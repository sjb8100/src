using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using table;
using UnityEngine;
partial class GodWeapenPanel 
{
    WelfareManager m_mgr = DataManager.Manager<WelfareManager>();
    List<ArtifactDataBase> list = null;
    List<UILabel> m_lst_labels = new List<UILabel>();
    List<uint> m_lst_GwRecord = new List<uint>();

    CMResAsynSeedData<CMAtlas> m_playerAvataCASD = null;
    List<UIItemRewardGrid> m_lst_award = null;

    IRenerTextureObj m_RTObj = null;
    protected override void OnLoading()
    {
        base.OnLoading();
        list = GameTableManager.Instance.GetTableList<ArtifactDataBase>();

        AddCreator(m_trans_rewardRoot);
    }
    protected override void OnShow(object data)
    {
        base.OnShow(data);
        m_mgr.ValueUpdateEvent += OnUpdateAreifact;
        RefreshUI();
        ShowModel();
        //UIEventListener.Get(m__backGround.gameObject).onClick = OnClickTexture;

    }
    void ShowModel() 
    {
         int job = MainPlayerHelper.GetMainPlayerJob();

        List<uint> list = GameTableManager.Instance.GetGlobalConfigList<uint>("ArtifactReward", job.ToString());
        if (list.Count == 2)
        {
            uint modelID = list[1];
            ShowModelDataBase data = GameTableManager.Instance.GetTableItem<ShowModelDataBase>(modelID);
            if (data == null)
            {
                return;
            }
            if (m_RTObj != null)
            {
                m_RTObj.Release();
            }
            m_RTObj = DataManager.Manager<RenderTextureManager>().CreateRenderTextureObj((int)modelID, 800);
            if (m_RTObj == null)
            {
                return;
            }
            // 0 1.52  0    7   45   0    5 
            m_RTObj.SetCamera(new Vector3(0, data.quanOffsetY * 0.01f, 0), Vector3.zero, -data.quanDistance * 0.01f);
            if (m__Model != null)
            {
                m__Model.mainTexture = m_RTObj.GetTexture();
                m__Model.MakePixelPerfect();

            }
        }

    }

    void OnUpdateAreifact(object obj, ValueUpdateEventArgs value)
    {
        if (value.key.Equals("OnUpdateArticfact"))
        {
            RefreshUI();
        }
    }

    void RefreshUI() 
    {
        m_lst_GwRecord = m_mgr.GodWeapenRecord;
        if (m_mgr.IsGodWeapenActivate)
        {
            m_btn_FinalRewardBtn.gameObject.SetActive(false);
            m_trans_rewardRoot.gameObject.SetActive(false);
        }
        else
        {
            if (m_mgr.CanJiHuoGodWeapen)
            {
                m_btn_FinalRewardBtn.gameObject.SetActive(true);
                m_trans_rewardRoot.gameObject.SetActive(false);
            }
            else 
            {
                m_btn_FinalRewardBtn.gameObject.SetActive(false);
                m_trans_rewardRoot.gameObject.SetActive(true);
                int level = MainPlayerHelper.GetPlayerLevel();
                int textureCount = m__backGround.transform.childCount;
                int labelCount = m_trans_labelRoot.transform.childCount;
               m_lst_UIItemRewardDatas.Clear();
                for (int i = 0; i < list.Count; i++)
                {
                    bool isOk = level >= list[i].open_level && DataManager.Manager<TaskDataManager>().CheckTaskFinished(list[i].taskid);
                    if (i <= textureCount && i <= labelCount)
                    {
                        Transform textureTrans = m__backGround.transform.GetChild(i);
                        if (textureTrans != null)
                        {
                            textureTrans.gameObject.SetActive(isOk);
                        }
                        Transform labelTrans = m_trans_labelRoot.transform.GetChild(i);
                        if (labelTrans != null)
                        {
                            UILabel label = labelTrans.GetComponent<UILabel>();
                            label.text = isOk ? string.Format("[FFFFC8]{0}[-]", list[i].decrib)
                                              : ColorManager.GetColorString(ColorType.White, list[i].decrib);
                        }
                    }
                    m_lst_UIItemRewardDatas.Add(new UIItemRewardData()
                    {
                        itemID = list[i].reward,
                        num = 1,
                        blockVisible =!isOk,
                        hasGot = m_lst_GwRecord.Contains(list[i].ID),
                        GodWeapenType = true,
                        DataID = list[i].ID,
                    });
                }  
                m_ctor_UIItemRewardCreator.CreateGrids(m_lst_UIItemRewardDatas.Count);
            }
            
        }
            
    }

        #region UIItemRewardGridCreator
    UIGridCreatorBase m_ctor_UIItemRewardCreator;
    List<UIItemRewardData> m_lst_UIItemRewardDatas = new List<UIItemRewardData>();
    void AddCreator(Transform parent)
    {
        if (parent != null)
        {
            m_ctor_UIItemRewardCreator = parent.GetComponent<UIGridCreatorBase>();
            if (m_ctor_UIItemRewardCreator == null)
            {
                m_ctor_UIItemRewardCreator = parent.gameObject.AddComponent<UIGridCreatorBase>();
            }
            m_ctor_UIItemRewardCreator.arrageMent = UIGridCreatorBase.Arrangement.Horizontal;
            m_ctor_UIItemRewardCreator.gridWidth = 90;
            m_ctor_UIItemRewardCreator.gridHeight = 90;
            m_ctor_UIItemRewardCreator.RefreshCheck();
            m_ctor_UIItemRewardCreator.Initialize<UIItemRewardGrid>(m_trans_UIItemRewardGrid.gameObject, OnUpdateGridData, null);
        }
    }
    void OnUpdateGridData(UIGridBase grid, int index)
    {
        if (grid is UIItemRewardGrid)
        {
            UIItemRewardGrid itemShow = grid as UIItemRewardGrid;
            if (itemShow != null)
            {
                if (index < m_lst_UIItemRewardDatas.Count)
                {
                    UIItemRewardData data = m_lst_UIItemRewardDatas[index];
                    uint itemID = data.itemID;
                    itemShow.SetGridData(itemID, 1, false, false, false, null, data.blockVisible, data.hasGot, true, data.DataID);
                }
            }
        }
    }
    #endregion


    protected override void OnHide()
    {
        base.OnHide();
        Release();
    }
    protected override void OnPanelBaseDestory()
    {
        base.OnPanelBaseDestory();
        Release();

    }
    public override void Release(bool depthRelease = true)
    {
        base.Release(depthRelease);
        m_mgr.ValueUpdateEvent -= OnUpdateAreifact;
        if (m_playerAvataCASD != null)
        {
            m_playerAvataCASD.Release(depthRelease);
            m_playerAvataCASD = null;
        }
        if (m_RTObj != null)
        {
            m_RTObj.Release();
        }
    }
    void onClick_Preview_btn_Btn(GameObject caster)
    {
        int job = MainPlayerHelper.GetMainPlayerJob();

        List<uint> list = GameTableManager.Instance.GetGlobalConfigList<uint>("ArtifactReward", job.ToString());
        if (list.Count == 2)
        {
            uint modelID = list[1];
            DataManager.Manager<UIPanelManager>().ShowPanel(PanelID.ShowModelPanel, data: (uint)modelID);
        }
    }
    void onClick_Close_Btn(GameObject caster)
    {
        HideSelf();
    }

    void onClick_FinalRewardBtn_Btn(GameObject caster)
    {
        if (m_mgr.CanJiHuoGodWeapen)
        {
            NetService.Instance.Send(new GameCmd.stActivateArtifactDataUserCmd_CS());
        }          
    }
}

