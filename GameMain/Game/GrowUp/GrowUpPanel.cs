using Client;
//*******************************************************************************************
//	创建日期：	2017-5-12   17:24
//	文件名称：	growuppanel,cs
//	创 建 人：	Lin Chuang Yuan
//	版权所有：	ZQB
//	说    明：	我要变强
//*******************************************************************************************
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using table;
using UnityEngine;


partial class GrowUpPanel : UIPanelBase
{

    #region property
    UISecondTabCreatorBase m_SecondTabCreator;                  //左侧二级页签

    UIGridCreatorBase m_GrowUpFightPowerGridCreator;            //九洲之力

    UIGridCreatorBase m_GrowUpGridCreator;                      //活动玩法

    Dictionary<uint, Dictionary<uint, List<uint>>> m_dicGrowUp; // 玩法数据

    List<GrowUpDabaBase> m_lstGrowUpDabaBase; //玩法table数据

    uint m_selectFirstKeyId = 0;
    uint m_selectSecondKeyId = 0;

    List<GrowUpFightPowerDabaBase> m_lstGrowUpFightPowerDB;

    uint m_bestRecommendId;  //推荐前往 Id
    #endregion


    #region override


    protected override void OnLoading()
    {
        base.OnLoading();

        InitData();

        InitWidget();
    }

    protected override void OnPrepareShow(object data)
    {
        base.OnPrepareShow(data);
    }

    protected override void OnShow(object data)
    {
        //战力评分
        InitTopInfo();

        //九洲之力
        CreateGrowUpFightPowerGrid();

        //左侧活动玩法grid
        CreateLeftGrid();

    }

    protected override void OnHide()
    {
        base.OnHide();

        Release();
    }

    protected override void OnPanelBaseDestory()
    {
        base.OnPanelBaseDestory();
    }

    public override void Release(bool depthRelease = true)
    {
        base.Release(depthRelease);

        if (m_GrowUpFightPowerGridCreator != null)
        {
            m_GrowUpFightPowerGridCreator.Release(depthRelease);
        }

        if (m_GrowUpGridCreator != null)
        {
            m_GrowUpGridCreator.Release(depthRelease);
        }
    }


    protected override void OnJump(UIPanelBase.PanelJumpData jumpData)
    {
        base.OnJump(jumpData);
        if (null == jumpData)
        {
            jumpData = new PanelJumpData();
        }

        int firstTabData = -1;
        int secondTabData = -1;

        //有跳转数据
        if (null != jumpData.Tabs && jumpData.Tabs.Length > 0)
        {
            firstTabData = jumpData.Tabs[0];
            secondTabData = jumpData.Tabs[1];

            if (firstTabData == 0 && secondTabData == 0)
            {
                //战力评分
                InitTopInfo();

                //九洲之力
                CreateGrowUpFightPowerGrid();

                //左侧活动玩法grid
                CreateLeftGrid();

                return;
            }

            CreateLeftGrid();
            SetSelectCtrTypeGrid((uint)firstTabData);
            SetSelectSecondTypeGrid((uint)firstTabData, (uint)secondTabData);
            CreateGrowUpGrid();
        }
        else //无跳转数据，跳到默认
        {

        }
    }

    public override UIPanelBase.PanelData GetPanelData()
    {
        PanelData pd = base.GetPanelData();
        pd.JumpData = new PanelJumpData();
        pd.JumpData.Tabs = new int[2];
        pd.JumpData.Tabs[0] = (int)m_selectFirstKeyId;
        pd.JumpData.Tabs[1] = (int)m_selectSecondKeyId;
        return pd;
    }


    public override bool OnTogglePanel(int tabType, int pageid)
    {
        return base.OnTogglePanel(tabType, pageid);
    }

    #endregion


    #region method

    void InitData()
    {
        m_dicGrowUp = DataManager.Manager<GrowUpManager>().GrowUpDic;
        m_lstGrowUpDabaBase = GameTableManager.Instance.GetTableList<GrowUpDabaBase>();
    }

    /// <summary>
    /// 顶部战力 推荐战力
    /// </summary>
    void InitTopInfo()
    {
        IPlayer player = Client.ClientGlobal.Instance().MainPlayer;
        if (player == null)
        {
            return;
        }

        //我的战力
        m_label_MypowerNum.text = player.GetProp((int)FightCreatureProp.Power).ToString();

        //{30}级推荐战力：
        int playerLv = player.GetProp((int)CreatureProp.Level);
        string recommendPowerDes = string.Format("{0}级推荐战力：", playerLv);
        m_label_Recommendpower.text = recommendPowerDes;

        //推荐战力值
        m_label_RecommendpowerNum.text = DataManager.Manager<GrowUpManager>().GetRecommendFightPower().ToString();

        m_trans_GoUpWight.gameObject.SetActive(false);
        m_sprite_ScoreNum.gameObject.SetActive(false);
    }

    void InitWidget()
    {
        //左侧页签
        if (m_scrollview_growleftscrollview != null)
        {

            m_SecondTabCreator = m_scrollview_growleftscrollview.GetComponent<UISecondTabCreatorBase>();

            if (m_SecondTabCreator == null)
            {
                m_SecondTabCreator = m_scrollview_growleftscrollview.gameObject.AddComponent<UISecondTabCreatorBase>();

                GameObject cloneFTemp = UIManager.GetResGameObj(GridID.Uictrtypegrid) as GameObject;
                GameObject cloneSTemp = UIManager.GetResGameObj(GridID.Uigrowupsecondtypegrid) as GameObject;

                m_SecondTabCreator.Initialize<UIGrowUpSecondTypeGrid>(cloneFTemp, cloneSTemp /*m_trans_UIGrowUpSecondTypeGrid.gameObject*/, OnUpdateGrowUpCtrTypeGrid, OnUpdateGrowUpSecondTypeGrid, OnGrowUpTypeGridUIEventDlg);
            }
        }

        //战力
        if (m_trans_growFightPowerrightscrollview != null)
        {
            m_trans_growFightPowerrightscrollview.gameObject.SetActive(true);
            m_GrowUpFightPowerGridCreator = m_trans_growFightPowerrightscrollview.gameObject.GetComponent<UIGridCreatorBase>();
            if (m_GrowUpFightPowerGridCreator == null)
            {
                m_GrowUpFightPowerGridCreator = m_trans_growFightPowerrightscrollview.gameObject.AddComponent<UIGridCreatorBase>();
            }

            //UnityEngine.GameObject obj = UIManager.GetResGameObj(GridID.Uigrowupfightpowergrid) as UnityEngine.GameObject;
            m_GrowUpFightPowerGridCreator.gridContentOffset = new UnityEngine.Vector2(0, 0);
            m_GrowUpFightPowerGridCreator.arrageMent = UIGridCreatorBase.Arrangement.Vertical;
            m_GrowUpFightPowerGridCreator.gridWidth = 850;
            m_GrowUpFightPowerGridCreator.gridHeight = 101;

            m_GrowUpFightPowerGridCreator.RefreshCheck();
            m_GrowUpFightPowerGridCreator.Initialize<UIGrowUpFightPowerGrid>(m_sprite_UIGrowUpFightPowergrid.gameObject, OnGridDataUpdate, OnGridUIEvent);
        }

        //活动玩法
        if (m_trans_growrightscrollview != null)
        {
            m_trans_growrightscrollview.gameObject.SetActive(true);
            m_GrowUpGridCreator = m_trans_growrightscrollview.gameObject.GetComponent<UIGridCreatorBase>();
            if (m_GrowUpGridCreator == null)
            {
                m_GrowUpGridCreator = m_trans_growrightscrollview.gameObject.AddComponent<UIGridCreatorBase>();
            }

            //UnityEngine.GameObject obj = UIManager.GetResGameObj(GridID.Uigrowupgrid) as UnityEngine.GameObject;
            m_GrowUpGridCreator.gridContentOffset = new UnityEngine.Vector2(0, 0);
            m_GrowUpGridCreator.arrageMent = UIGridCreatorBase.Arrangement.Vertical;
            m_GrowUpGridCreator.gridWidth = 850;
            m_GrowUpGridCreator.gridHeight = 101;

            m_GrowUpGridCreator.RefreshCheck();
            m_GrowUpGridCreator.Initialize<UIGrowUpGrid>(m_sprite_UIGrowUpGrid.gameObject, OnGridDataUpdate, OnGridUIEvent);
        }

        m_label_btn_analysis_label.text = "马上分析";
    }


    private void OnUpdateGrowUpCtrTypeGrid(UIGridBase gridBase, int index)
    {
        if (m_dicGrowUp != null && m_dicGrowUp.Keys.Count > index)
        {
            UICtrTypeGrid grid = gridBase as UICtrTypeGrid;
            if (grid == null)
            {
                return;
            }

            List<uint> keyIdList = m_dicGrowUp.Keys.ToList<uint>();
            uint keyId = keyIdList[index];

            Dictionary<uint, List<uint>> secondKeyDic;
            if (m_dicGrowUp.TryGetValue(keyId, out secondKeyDic))
            {
                GrowUpDabaBase growUpDabaBase = m_lstGrowUpDabaBase.Find((data) => { return data.Type == keyId; });
                if (growUpDabaBase != null)
                {
                    grid.SetData(keyId, growUpDabaBase.TypeName, secondKeyDic.Keys.Count);
                }

            }
        }
    }


    private void OnUpdateGrowUpSecondTypeGrid(UIGridBase gridBase, object id, int index)
    {
        UIGrowUpSecondTypeGrid grid = gridBase as UIGrowUpSecondTypeGrid;
        if (grid == null)
        {
            return;
        }

        Dictionary<uint, List<uint>> secondKeyDic;
        if (m_dicGrowUp.TryGetValue((uint)id, out secondKeyDic))
        {
            List<uint> secondKeyIdList = secondKeyDic.Keys.ToList<uint>();
            if (secondKeyIdList.Count > index)
            {
                uint secondKeyId = secondKeyIdList[index];

                grid.SetData((uint)id, secondKeyId);
                grid.SetSelect((uint)id == m_selectFirstKeyId && secondKeyId == m_selectSecondKeyId);

                List<uint> idList;
                if (DataManager.Manager<GrowUpManager>().TryGetGrowUpIdListByKeyAndSecondkey((uint)id, secondKeyId, out idList))
                {
                    if (idList.Count > 0)
                    {
                        GrowUpDabaBase growUpDabaBase = GameTableManager.Instance.GetTableItem<GrowUpDabaBase>(idList[0]);

                        if (growUpDabaBase == null)
                        {
                            return;
                        }
                        grid.SetName(growUpDabaBase.IndexTypeName);
                    }
                }
            }

        }
    }

    private void OnGrowUpTypeGridUIEventDlg(UIEventType eventType, object data, object param)
    {
        if (eventType == UIEventType.Click)
        {
            if (data is UICtrTypeGrid)
            {
                UICtrTypeGrid grid = data as UICtrTypeGrid;
                if (grid == null)
                {
                    return;
                }

                SetSelectCtrTypeGrid((uint)grid.ID);

                //默认选中二级grid的第一个
                Dictionary<uint, List<uint>> secondKeyDic;
                if (m_dicGrowUp.TryGetValue((uint)grid.ID, out secondKeyDic))
                {
                    List<uint> secondKeyIdList = secondKeyDic.Keys.ToList<uint>();
                    if (secondKeyIdList.Count > 0)
                    {
                        uint secondKeyId = secondKeyIdList[0];
                        SetSelectSecondTypeGrid((uint)grid.ID, secondKeyId);

                        CreateGrowUpGrid();
                    }
                }
            }

            if (data is UIGrowUpSecondTypeGrid)
            {
                UIGrowUpSecondTypeGrid grid = data as UIGrowUpSecondTypeGrid;
                if (grid == null)
                {
                    return;
                }

                SetSelectSecondTypeGrid(grid.FirstKeyId, grid.SecondKeyId);

                CreateGrowUpGrid();
            }
        }
    }

    void SetSelectCtrTypeGrid(uint id)
    {
        List<uint> keyIdList = m_dicGrowUp.Keys.ToList<uint>();

        if (id == m_selectFirstKeyId)
        {
            if (m_SecondTabCreator.IsOpen(keyIdList.IndexOf(m_selectFirstKeyId)))
            {
                m_SecondTabCreator.Close(keyIdList.IndexOf(m_selectFirstKeyId));
                return;
            }
        }

        if (keyIdList.Contains(m_selectFirstKeyId))
        {
            m_SecondTabCreator.Close(keyIdList.IndexOf(m_selectFirstKeyId));
        }

        if (keyIdList.Contains(id))
        {
            m_SecondTabCreator.Open(keyIdList.IndexOf(id));
        }

        //把之前选中的二级页签取消掉
        UIGrowUpSecondTypeGrid grid = GetSecondTypeGrid(this.m_selectFirstKeyId, this.m_selectSecondKeyId);
        if (grid != null)
        {
            grid.SetSelect(false);
        }

        this.m_selectFirstKeyId = id;
    }

    void SetSelectSecondTypeGrid(uint firstKeyId, uint secondKeyId)
    {
        UIGrowUpSecondTypeGrid grid = GetSecondTypeGrid(this.m_selectFirstKeyId, this.m_selectSecondKeyId);
        if (grid != null)
        {
            grid.SetSelect(false);
        }

        grid = GetSecondTypeGrid(firstKeyId, secondKeyId);
        if (grid != null)
        {
            grid.SetSelect(true);
        }

        this.m_selectSecondKeyId = secondKeyId;
    }

    UIGrowUpSecondTypeGrid GetSecondTypeGrid(uint firstKeyId, uint secondKeyId)
    {
        UIGrowUpSecondTypeGrid grid = null;

        List<uint> firstKeyIdList = m_dicGrowUp.Keys.ToList<uint>();

        Dictionary<uint, List<uint>> secondKeyDic;
        if (m_dicGrowUp.TryGetValue(firstKeyId, out secondKeyDic))
        {
            List<uint> secondKeyIdList = secondKeyDic.Keys.ToList<uint>();
            if (secondKeyIdList.Contains(secondKeyId))
            {
                grid = m_SecondTabCreator.GetGrid<UIGrowUpSecondTypeGrid>(firstKeyIdList.IndexOf(firstKeyId), secondKeyIdList.IndexOf(secondKeyId));
            }
        }

        return grid;
    }

    void CreateLeftGrid()
    {
        List<int> list = new List<int>();

        Dictionary<uint, Dictionary<uint, List<uint>>>.Enumerator enumerator = m_dicGrowUp.GetEnumerator();
        while (enumerator.MoveNext())
        {
            Dictionary<uint, List<uint>> dic = enumerator.Current.Value;
            list.Add(dic.Keys.Count);
        }

        m_SecondTabCreator.CreateGrids(list);
    }

    void CreateGrowUpFightPowerGrid()
    {
        m_trans_growrightscrollview.gameObject.SetActive(false);
        m_trans_growFightPowerContent.gameObject.SetActive(true);

        m_lstGrowUpFightPowerDB = DataManager.Manager<GrowUpManager>().GetGrowUpFightPowerGridList();
        if (m_GrowUpFightPowerGridCreator != null)
        {
            m_GrowUpFightPowerGridCreator.CreateGrids(m_lstGrowUpFightPowerDB != null ? m_lstGrowUpFightPowerDB.Count : 0);
        }
    }



    void CreateGrowUpGrid()
    {
        m_trans_growrightscrollview.gameObject.SetActive(true);
        m_trans_growFightPowerContent.gameObject.SetActive(false);

        List<uint> idList;
        if (DataManager.Manager<GrowUpManager>().TryGetGrowUpIdListByKeyAndSecondkey(this.m_selectFirstKeyId, this.m_selectSecondKeyId, out idList))
        {
            if (m_GrowUpGridCreator != null)
            {
                m_GrowUpGridCreator.CreateGrids(idList.Count);
            }
        }
    }

    private void OnGridDataUpdate(UIGridBase gridData, int index)
    {
        if (gridData is UIGrowUpGrid)
        {
            List<uint> idList;
            if (DataManager.Manager<GrowUpManager>().TryGetGrowUpIdListByKeyAndSecondkey(this.m_selectFirstKeyId, this.m_selectSecondKeyId, out idList))
            {
                if (idList.Count > index)
                {
                    UIGrowUpGrid grid = gridData as UIGrowUpGrid;
                    if (grid == null)
                    {
                        return;
                    }

                    GrowUpDabaBase growUpDB = m_lstGrowUpDabaBase.Find((data) => { return data.dwID == idList[index]; });
                    if (growUpDB == null)
                    {
                        return;
                    }

                    grid.SetGridData(idList[index]);
                    grid.SetName(growUpDB.Name);
                    grid.SetIcon(growUpDB.IconName);
                    grid.SetStar(growUpDB.Star);
                    grid.SetDes(growUpDB.Des);

                    IPlayer MainPlayer = Client.ClientGlobal.Instance().MainPlayer;
                    if (MainPlayer != null)
                    {
                        int playerLv = MainPlayer.GetProp((int)Client.CreatureProp.Level);
                        grid.SetGotoBtnEnable(playerLv >= growUpDB.OpenLv, growUpDB.OpenLv);
                    }

                }
            }
        }

        //战斗力
        if (gridData is UIGrowUpFightPowerGrid)
        {
            if (m_lstGrowUpFightPowerDB != null && m_lstGrowUpFightPowerDB.Count > index)
            {
                UIGrowUpFightPowerGrid grid = gridData as UIGrowUpFightPowerGrid;
                if (grid == null)
                {
                    return;
                }

                grid.SetGridData(m_lstGrowUpFightPowerDB[index].dwID);
                grid.SetName(m_lstGrowUpFightPowerDB[index].Name);

                string des = m_lstGrowUpFightPowerDB[index].Des;
                uint[] desData;
                if (DataManager.Manager<GrowUpManager>().TryGetFightPowerDatabaseById(m_lstGrowUpFightPowerDB[index].dwID, out desData))
                {
                    if (desData.Length == 1)
                    {
                        des = string.Format(m_lstGrowUpFightPowerDB[index].Des, desData[0]);
                    }
                    else if (desData.Length == 2)
                    {
                        des = string.Format(m_lstGrowUpFightPowerDB[index].Des, desData[0], desData[1]);
                    }
                }
                grid.SetDes(des);
                grid.SetIcon(m_lstGrowUpFightPowerDB[index].IconName);
                float value = DataManager.Manager<GrowUpManager>().GetFightPowerSliderValueById(m_lstGrowUpFightPowerDB[index].dwID);
                grid.SetSliderAndPercent(value);

                int[] arr = new int[3];

                string s = string.Format("", arr);
            }
        }
    }

    private void OnGridUIEvent(UIEventType eventType, object data, object param)
    {
        if (eventType == UIEventType.Click)
        {
            if (data is UIGrowUpGrid)
            {
                UIGrowUpGrid grid = data as UIGrowUpGrid;
                if (grid == null)
                {
                    return;
                }

                int gotoBtnIndex = 1;

                if (param != null && (int)param == gotoBtnIndex)
                {
                    //立即前往
                    GrowUpDabaBase growUpDB = GameTableManager.Instance.GetTableItem<GrowUpDabaBase>(grid.Id);
                    if (growUpDB != null)
                    {
                        ItemManager.DoJump(growUpDB.JumpId);
                    }
                }
            }

            if (data is UIGrowUpFightPowerGrid)
            {
                UIGrowUpFightPowerGrid grid = data as UIGrowUpFightPowerGrid;
                if (grid == null)
                {
                    return;
                }

                int gotoBtnIndex = 1;
                if (param != null && (int)param == gotoBtnIndex)
                {
                    //立即前往
                    GrowUpFightPowerDabaBase growUpFightPowerDB = GameTableManager.Instance.GetTableItem<GrowUpFightPowerDabaBase>(grid.Id);
                    if (growUpFightPowerDB != null)
                    {
                        ItemManager.DoJump(growUpFightPowerDB.JumpId);
                    }
                }
            }
        }
    }

    /// <summary>
    /// 分析
    /// </summary>
    void DoaAnalysis()
    {
        m_trans_GoUpWight.gameObject.SetActive(false);

        //进度条置0
        List<UIGrowUpFightPowerGrid> list = m_GrowUpFightPowerGridCreator.GetGrids<UIGrowUpFightPowerGrid>(true);

        for (int i = 0; i < list.Count; i++)
        {
            list[i].SetAnalysisProgress(0);
        }

        StartCoroutine(GridProgress());
    }

    /// <summary>
    /// 跑进度条动画
    /// </summary>
    /// <returns></returns>
    IEnumerator GridProgress()
    {
        List<UIGrowUpFightPowerGrid> gridList = m_GrowUpFightPowerGridCreator.GetGrids<UIGrowUpFightPowerGrid>(true);

        float gridTime = 0.5f;//每个grid要跑的时间

        for (int i = 0; i < gridList.Count; i++)
        {
            for (float time = 0; time <= gridTime; time += Time.deltaTime)
            {
                gridList[i].SetAnalysisProgress(gridTime > 0 ? time / gridTime : 0);
                yield return 0;
            }

            gridList[i].SetAnalysisProgress(1);

            m_GrowUpFightPowerGridCreator.FocusGrid(i, false);

            gridList = m_GrowUpFightPowerGridCreator.GetGrids<UIGrowUpFightPowerGrid>(true);
        }

        ShowGoUpWidget();
        yield return 0;
    }

    /// <summary>
    /// 推荐结果上浮
    /// </summary>
    void ShowGoUpWidget()
    {
        m_sprite_goUp.transform.localPosition = new Vector3(0, -120, 0);

        m_trans_GoUpWight.gameObject.SetActive(true);

        //上浮
        TweenPosition.Begin(m_sprite_goUp.gameObject, 0.35f, Vector3.zero);

        //评分
        string systemScore = DataManager.Manager<GrowUpManager>().GetSystemScore();
        string systemScoreSpriteName = string.Format("tubiao_pingfen_{0}", systemScore);
        m_sprite_ScoreNum.gameObject.SetActive(true);
        m_sprite_ScoreNum.spriteName = systemScoreSpriteName;

        //推荐
        m_bestRecommendId = DataManager.Manager<GrowUpManager>().GetBestRecommendId();
        GrowUpFightPowerDabaBase db = GameTableManager.Instance.GetTableItem<GrowUpFightPowerDabaBase>(m_bestRecommendId);

        string des = string.Empty;
        if (db != null)
        {
            des = string.Format("恭喜！您的战力评分为：{0}  经检测，当前进行 {1} 的性价比最高！", systemScore.ToUpper(), db.Name);
        }

        m_label_goUpDes.text = des;

        m_label_btn_analysis_label.text = "重新分析";
    }

    #endregion


    #region click
    /// <summary>
    /// 分析
    /// </summary>
    /// <param name="caster"></param>
    void onClick_Btn_analysis_Btn(GameObject caster)
    {
        CreateGrowUpFightPowerGrid();

        DoaAnalysis();
    }

    /// <summary>
    /// 分析后立即前往
    /// </summary>
    /// <param name="caster"></param>
    void onClick_GoUpBtn_Btn(GameObject caster)
    {
        GrowUpFightPowerDabaBase growUpFightPowerDb = GameTableManager.Instance.GetTableItem<GrowUpFightPowerDabaBase>(m_bestRecommendId);
        if (growUpFightPowerDb != null)
        {
            ItemManager.DoJump(growUpFightPowerDb.JumpId);
        }

        m_trans_GoUpWight.gameObject.SetActive(false);
    }

    /// <summary>
    /// 关闭
    /// </summary>
    /// <param name="caster"></param>
    void onClick_GoUpCloseBtn_Btn(GameObject caster)
    {
        m_trans_GoUpWight.gameObject.SetActive(false);
    }

    /// <summary>
    /// 我要变强
    /// </summary>
    /// <param name="caster"></param>
    void onClick_Btn_fightPower_Btn(GameObject caster)
    {
        CreateGrowUpFightPowerGrid();
    }
    #endregion
}

