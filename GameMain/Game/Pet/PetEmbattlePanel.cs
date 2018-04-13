using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using table;
using Client;
using GameCmd;

partial class PetEmbattlePanel : UIPanelBase
{
    //布阵上面三个位置字典
    public Dictionary<int, PetEmbatteGrid> m_upDic = new Dictionary<int, PetEmbatteGrid>();
    //布阵上面8个位置字典
    public Dictionary<int, PetEmbatteGrid> m_downDic = new Dictionary<int, PetEmbatteGrid>();
    PetDataManager m_PetData
    {
        get
        {
            return DataManager.Manager<PetDataManager>();
        }
    }
    uint m_uDownSelectPetID = 0;
    protected override void OnLoading()
    {
        base.OnLoading();
        int n = m_grid_UpRoot.transform.childCount;
        for (int i = 0; i < n; i++)
        {
            Transform trans = m_grid_UpRoot.transform.GetChild(i);
            GameObject go = NGUITools.AddChild(trans.gameObject, m_trans_PetEmbatteGrid.gameObject);
            go.SetActive(true);
            PetEmbatteGrid grid = go.AddComponent<PetEmbatteGrid>();
            int index = 0;
            if (int.TryParse(trans.name, out index))
            {
                if (!m_upDic.ContainsKey(index))
                {
                    m_upDic.Add(index, grid);
                }
            }
        }
        int m = m_grid_DownRoot.transform.childCount;
        for (int i = 0; i < m; i++)
        {
            Transform trans = m_grid_DownRoot.transform.GetChild(i);
            GameObject go = NGUITools.AddChild(trans.gameObject, m_trans_PetEmbatteGrid.gameObject);
            go.SetActive(true);
            PetEmbatteGrid grid = go.AddComponent<PetEmbatteGrid>();

            int index = 0;
            if (int.TryParse(trans.name, out index))
            {
                if (!m_downDic.ContainsKey(index))
                {
                    m_downDic.Add(index, grid);
                }
            }
        }
        m_trans_PetEmbatteGrid.gameObject.SetActive(false);
        InitUIPos();

    }
    void InitUIPos()
    {
        var iter = m_upDic.GetEnumerator();
        while (iter.MoveNext())
        {
            var item = iter.Current;
            item.Value.InitPos(PetLineUpPos.Up, item.Key);
        }

        iter = m_downDic.GetEnumerator();
        while (iter.MoveNext())
        {
            var item = iter.Current;
            item.Value.InitPos(PetLineUpPos.Down, item.Key);
        }
    }
    void InitLineUpData()
    {
        m_uDownSelectPetID = 0;
        var iter = m_upDic.GetEnumerator();
        while (iter.MoveNext())
        {
            var item = iter.Current;
            //key是从1开始的
            int index = item.Key - 1;
            if (index < m_PetData.PetQuickList.Count)
            {
                uint petID = m_PetData.PetQuickList[index];
                item.Value.InitPetEmbattleGrid(petID, OnGridClick);
            }
            else
            {
                item.Value.InitPetEmbattleGrid(0, OnGridClick);
            }
        }

        iter = m_downDic.GetEnumerator();
        while (iter.MoveNext())
        {
            var item = iter.Current;
            int index = item.Key - 1;
            if (index < m_PetData.GetNOLineUPPetList().Count)
            {
                uint petID = m_PetData.GetNOLineUPPetList()[index];
                item.Value.InitPetEmbattleGrid(petID, OnGridClick);
            }
            else
            {
                item.Value.InitPetEmbattleGrid(0, OnGridClick);
            }
            item.Value.SetHighLight(false);
        }
    }
    void SetAutofight(bool bAtuofight)
    {
        if (m_sprite_gouSprite != null)
        {
            m_sprite_gouSprite.gameObject.SetActive(bAtuofight);
            m_sprite_gouSprite.alpha = 1;
        }
    }
    void RegisterEvent(bool bReg)
    {
        if (bReg)
        {
            m_PetData.ValueUpdateEvent += m_PetData_ValueUpdateEvent;
        }
        else
        {
            m_PetData.ValueUpdateEvent -= m_PetData_ValueUpdateEvent;
        }
    }

    void m_PetData_ValueUpdateEvent(object sender, ValueUpdateEventArgs e)
    {
        if (e.key == PetDispatchEventString.RefreshQuickSetting.ToString())
        {
            InitLineUpData();
        }
        else if (e.key == PetDispatchEventString.RefreshLineUPAutoFight.ToString())
        {
            SetAutofight(m_PetData.bFirstLineUPPetFight);
        }
    }
    protected override void OnShow(object data)
    {
        base.OnShow(data);
        RegisterEvent(true);
        InitLineUpData();
        SetAutofight(m_PetData.bFirstLineUPPetFight);
    }


    void OnGridClick(PetLineUpPos pos, int index, uint petID)
    {
        if (pos == PetLineUpPos.Down)
        {
            m_uDownSelectPetID = petID;
            var iter = m_upDic.GetEnumerator();
            while (iter.MoveNext())
            {
                var item = iter.Current;

                item.Value.SetFlag(true);
            }
            iter = m_downDic.GetEnumerator();
            while (iter.MoveNext())
            {
                var item = iter.Current;
                if (index == item.Key)
                {
                    item.Value.SetHighLight(true);
                }
                else
                {
                    item.Value.SetHighLight(false);
                }
            }
        }
        if (pos == PetLineUpPos.Up)
        {
            if (m_uDownSelectPetID == 0)
            {
                m_PetData.RemovelineUp(petID);
            }
            else
            {
                m_PetData.ReplaceLineUP(m_uDownSelectPetID, petID);
            }
        }
    }
    protected override void OnDisable()
    {
        base.OnDisable();
        RegisterEvent(false);
    }
    protected override void OnJump(UIPanelBase.PanelJumpData jumpData)
    {
        base.OnJump(jumpData);
    }
    void onClick_Btn_Close_Btn(GameObject caster)
    {
        HideSelf();
    }
    void onClick_GuiyuanCommon_zidongbuzu_Btn(GameObject caster)
    {
        bool bfight = !m_PetData.bFirstLineUPPetFight;
        stFirstForceFightPetUserCmd_CS cmd = new stFirstForceFightPetUserCmd_CS();
        cmd.force_fight = bfight;
        NetService.Instance.Send(cmd);
    }
    public override void Release(bool depthRelease = true)
    {

        base.Release(depthRelease);
        var iter = m_upDic.GetEnumerator();
        while (iter.MoveNext())
        {
            var item = iter.Current;
            item.Value.ReleaseGrid();
        }
        iter = m_downDic.GetEnumerator();
        while (iter.MoveNext())
        {
            var item = iter.Current;
            item.Value.ReleaseGrid();
        }
    }
    protected override void OnHide()
    {
        base.OnHide();
        Release();
    }
    protected override void OnPanelBaseDestory()
    {
        base.OnPanelBaseDestory();
        m_downDic.Clear();
        m_upDic.Clear();
    }
}
