//*************************************************************************
//	创建日期:	2016/10/19 18:24:51
//	文件名称:	FBCard
//   创 建 人:   zhuidanyu	
//	版权所有:	中青宝
//	说    明:	游戏主界面
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
using Engine;


partial class FBCard : UIGridBase
{
    CopyDataBase m_db = null;
    List<string> m_idList;
    UIGridCreatorBase m_dropCreator = null;

    Engine.ITexture m_texture = null;
    bool bInit = false;
    public override void Init()
    {
        base.Init();
        InitControls();
    }
    public void DestroyScrollChild()
    {
        if (m_dropCreator != null)
        {
            UIWardGrid[] grids = m_dropCreator.GetComponentsInChildren<UIWardGrid>();
            foreach (var item in grids)
            {
                item.Release();
            }
        }
        if (m_trans_WardScrollView != null)
        {
            m_trans_WardScrollView.DestroyChildren();
        }
    }
    protected override void OnEnable()
    {
        base.OnEnable();
        InitCard();
    }
    public void InitByCopyData(CopyDataBase db)
    {
        m_db = db;
        InitControls();

        m_label_enteritemname.text = "";
        m_label_enterneedcount.text = "";
        m_sprite_itembg.transform.DestroyChildren();

        InitCard();

        if (m_widget_Panel.gameObject.activeSelf)
        {
            m_widget_Panel.transform.localScale = Vector3.one;
            InitPanel();
        }
    }
    void InitCard()
    {
        if (m_db != null)
        {
            if (m_texture != null)
            {
                m_texture.Release();
                m_texture = null;
            }
            string bgPath = "ui/texture/pack/fuben/" + m_db.strIcon + ".unity3d";
            bool success = Engine.RareEngine.Instance().GetRenderSystem().CreateTexture(ref bgPath, ref m_texture, CreateTextureEvent, null, Engine.TaskPriority.TaskPriority_Immediate);
            if (success)
            {
                m__bgtexture.mainTexture = m_texture.GetTexture();
            }
            m_trans_WardItem.gameObject.SetActive(false);
            m_trans_lingpai.gameObject.SetActive(true);
            m_label_FB_name.text = m_db.copyName;
            string str = m_db.openLv + CommonData.GetLocalString("级解锁");
            m_label_lock_level.text = StringUtil.GetColorString((int)m_db.openLv, MainPlayerHelper.GetPlayerLevel(), ColorType.Red, ColorType.Green, str);

            if (m_db.copyType == (uint)CopyTypeTable.Camp)//阵营战
            {
                m_btn_btn_Two.gameObject.SetActive(true);
                m_btn_btn_Two.GetComponentInChildren<UILabel>().text = CommonData.GetLocalString("兑换");
                m_btn_btn_enter.GetComponentInChildren<UILabel>().text = CommonData.GetLocalString("报名");
            }
            else if (m_db.copyFlag == (uint)CopyFlag.DaTi)
            {
                m_btn_btn_Two.gameObject.SetActive(false);
                m_btn_btn_enter.GetComponentInChildren<UILabel>().text = CommonData.GetLocalString("进入");
            }
            else if (m_db.copyFlag != (uint)CopyFlag.Huodong && m_db.membType != 0)
            {
                m_btn_btn_Two.gameObject.SetActive(true);
                m_btn_btn_Two.GetComponentInChildren<UILabel>().text = CommonData.GetLocalString("前往组队");
            }
            else
            {
                m_btn_btn_Two.gameObject.SetActive(false);
                m_btn_btn_enter.GetComponentInChildren<UILabel>().text = CommonData.GetLocalString("进入");
            }
            string useStr = GetEnterItem();
            bool enterCostVisble = false;
            if (string.IsNullOrEmpty(useStr))
            {
                m_label_enteritemname.text = "";
                m_label_enterneedcount.text = "";
                m_sprite_itembg.transform.DestroyChildren();
            }
            else
            {
                List<uint> idlist = StringUtil.GetSplitStringList<uint>(useStr, '_');
                if (idlist.Count == 2)
                {
                    ItemDataBase db = GameTableManager.Instance.GetTableItem<ItemDataBase>(idlist[0]);
                    if (db != null)
                    {
                        m_label_enteritemname.text = db.itemName;
                        int count = DataManager.Manager<ItemManager>().GetItemNumByBaseId(idlist[0]);
                        m_label_enterneedcount.text = StringUtil.GetNumNeedString(count, idlist[1]);
                        m_needItmeID = idlist[0];
                        UIItem.AttachParent(m_sprite_itembg.transform, m_needItmeID, (uint)count, ShowGetWayCallBack, true, (uint)idlist[1]);
                        // DataManager.Manager<UIManager>().SetSpriteDynamicIcon(m_sprite_itembg, db.quality, false);
                        UIManager.GetQualityAtlasAsyn(db.quality, ref m_curQualityAsynSeed, () =>
                        {
                            if (null != m_sprite_itembg)
                            {
                                m_sprite_itembg.atlas = null;
                            }
                        }, m_sprite_itembg);
                        enterCostVisble = true;
                    }

                }
                else
                {
                    m_sprite_itembg.spriteName = "";
                }
            }

            if (null != m_trans_enterinfo && m_trans_enterinfo.gameObject.activeSelf != enterCostVisble)
            {
                m_trans_enterinfo.gameObject.SetActive(enterCostVisble);
            }
        }
    }
    CMResAsynSeedData<CMAtlas> m_curQualityAsynSeed = null;
    private void CreateTextureEvent(ITexture obj, object param = null)
    {
        m__bgtexture.mainTexture = m_texture.GetTexture();
    }
    uint m_needItmeID = 0;
    void ShowGetWayCallBack(UIItemCommonGrid grid)
    {
        int itemCount = DataManager.Manager<ItemManager>().GetItemNumByBaseId(m_needItmeID);
        if (grid.Data.Num < grid.Data.NeedNum)
        {
            TipsManager.Instance.ShowItemTips(m_needItmeID);
            DataManager.Manager<UIPanelManager>().ShowPanel(PanelID.GetWayPanel, data: grid.Data.DwObjectId);

        }
        else
        {
            TipsManager.Instance.ShowItemTips(m_needItmeID);
        }
    }
    string GetEnterItem()
    {
        if (m_db != null)
        {
            CopyInfo info = DataManager.Manager<ComBatCopyDataManager>().GetCopyInfoById(m_db.copyId);
            if (info != null)
            {
                string itemArray = m_db.useItem;
                List<string> itemList = StringUtil.GetSplitStringList<string>(itemArray, ';');
                int useNum = (int)info.CopyUseNum;
                string useStr = "";
                if (itemList.Count == 0)
                {
                    return null;
                }
                if (useNum < itemList.Count)
                {
                    useStr = itemList[useNum];
                }
                else
                {
                    useStr = itemList[itemList.Count - 1];
                }
                return useStr;
            }
        }
        return null;
    }
    void InitPanel()
    {
        if (m_db != null)
        {
            m_label_description.text = m_db.des;
            string dropStr = m_db.dropDes;
            if (m_idList != null)
            {
                m_idList.Clear();
            }
            StringUtility.SplitString(ref dropStr, "_", out m_idList);

            if (m_idList.Count > 0)
            {
                int id = 0;
                if (int.TryParse(m_idList[0], out id))
                {
                    ShowWardIcon(true);
                    if (m_dropCreator == null)
                    {
                        GameObject resObj = m_trans_WardItem.gameObject;

                        m_dropCreator = m_trans_WardScrollView.gameObject.AddComponent<UIGridCreatorBase>();
                        m_dropCreator.arrageMent = UIGridCreatorBase.Arrangement.Horizontal;
                        m_dropCreator.gridContentOffset = new Vector2(-400f, 0);
                        m_dropCreator.gridWidth = 100;
                        m_dropCreator.gridHeight = 100;
                        m_dropCreator.RefreshCheck();
                        m_dropCreator.Initialize<UIWardGrid>(resObj, OnShowGridData, OnGridUIEventDlg);

                    }
                    m_dropCreator.CreateGrids(m_idList.Count);
                }
                else
                {
                    ShowWardIcon(false);
                    m_label_reward_word.text = m_db.dropDes;
                }
            }

        }
    }
    private void OnShowGridData(UIGridBase grid, int index)
    {
        if (grid == null)
        {
            return;
        }
        UIWardGrid wg = grid as UIWardGrid;
        if (m_idList != null)
        {
            string idStr = m_idList[index];
            int itemID = 0;
            if (int.TryParse(idStr, out itemID))
            {
                ItemDataBase db = GameTableManager.Instance.GetTableItem<ItemDataBase>((uint)itemID);
                if (db != null)
                {

                    wg.ItemID = db.itemID;
                }
            }
        }

    }
    private void OnGridUIEventDlg(UIEventType eventType, object data, object param)
    {
        if (eventType == UIEventType.Click)
        {
            if (data is UIWardGrid)
            {
                UIWardGrid tabGrid = data as UIWardGrid;
                if (null != tabGrid)
                {
                    uint itemID = tabGrid.ItemID;
                    TipsManager.Instance.ShowItemTips(itemID, tabGrid.gameObject, false);
                }
            }
        }
    }

    void ShowWardIcon(bool bShow)
    {
        m_trans_iconContent.gameObject.SetActive(bShow);
        m_label_reward_word.gameObject.SetActive(!bShow);

    }
    protected override void OnStart()
    {
        base.OnStart();
    }
    protected override void OnDisable()
    {
        base.OnDisable();
        if (m_texture != null)
        {
            m_texture.Release();
            m_texture = null;
        }
    }
    void onClick_Btn_enter_Btn(GameObject caster)
    {
        if (m_db != null)
        {
            if (!KHttpDown.Instance().SceneFileExists(m_db.mapId))
            {
                //打开下载界面
                DataManager.Manager<UIPanelManager>().ShowPanel(PanelID.DownloadPanel);
                return;
            }
        }

        if (m_db != null)
        {
            if (m_db.membType == 3)
            {
                TeamDataManager teamData = DataManager.Manager<TeamDataManager>();
                bool showTips = false;
                if (teamData.IsJoinTeam)
                {
                    List<TeamMemberInfo> teamMemberList = teamData.TeamMemberList;
                    int count = teamMemberList.Count;
                    if (count == 1)
                    {
                        showTips = true;
                    }
                }
                else
                {
                    showTips = true;
                }
                if (showTips)
                {

                    string tips = DataManager.Manager<TextManager>().GetLocalText(LocalTextType.Copy_Commond_fubenjinrutips);
                    TipsManager.Instance.ShowTipWindow(TipWindowType.CancelOk, tips, () =>
                    {
                        EnterCopy();
                    });
                }
                else
                {
                    EnterCopy();
                }

            }
            else
            {
                EnterCopy();
            }

        }
    }
    void EnterCopy()
    {
        if (m_db != null)
        {
            ComBatCopyDataManager copyData = DataManager.Manager<ComBatCopyDataManager>();
            TeamDataManager teamData = DataManager.Manager<TeamDataManager>();
            List<TeamMemberInfo> teamMemberList = teamData.TeamMemberList;
            int count = teamMemberList.Count;
            copyData.CPFlag = (CopyFlag)m_db.copyFlag;
            if (copyData.CPFlag == CopyFlag.Danren)
            {
                DataManager.Manager<ComBatCopyDataManager>().ReqEnterCopy(m_db.copyId);
            }
            else if (copyData.CPFlag == CopyFlag.Juqing)
            {
                if (count > 1)
                {
                    if (teamData.MainPlayerIsLeader())
                    {
                        DataManager.Manager<CampCombatManager>().ReqAskTeamrCopy(m_db.copyId);
                    }
                    else
                    {
                        TipsManager.Instance.ShowTips(LocalTextType.Copy_Commond_zhiyouduizhangcainengjinfubeno);
                    }

                }
                else
                {
                    if (m_db.copyId != 4001)
                    {
                        DataManager.Manager<ComBatCopyDataManager>().ReqEnterCopy(m_db.copyId);
                    }
                }

            }
            else if (copyData.CPFlag == CopyFlag.Zudui)
            {
                CopyDataBase db = GameTableManager.Instance.GetTableItem<CopyDataBase>(m_db.copyId);
                if (db != null)
                {
                    if (MainPlayerHelper.GetPlayerLevel() < db.openLv)
                    {
                        TipsManager.Instance.ShowTipsById(21);
                        return;
                    }
                }
                stAskTeamrCopyUserCmd_CS cmd = new stAskTeamrCopyUserCmd_CS();
                cmd.copy_base_id = m_db.copyId;
                NetService.Instance.Send(cmd);
            }
            else if (copyData.CPFlag == CopyFlag.Huodong)
            {
                if (m_db.copyType == 3)//阵营战
                {
                    DataManager.Manager<CampCombatManager>().GetSignCampInfo(0);
                    DataManager.Manager<UIPanelManager>().ShowPanel(PanelID.CampWarPanel);
                }
            }
            else if (copyData.CPFlag == CopyFlag.DaTi)
            {
                DataManager.Manager<AnswerManager>().ReqEnterAnswerCopy();
            }
            else
            {
                stAskTeamrCopyUserCmd_CS cmd = new stAskTeamrCopyUserCmd_CS();
                cmd.copy_base_id = m_db.copyId;
                NetService.Instance.Send(cmd);
            }
        }

    }
    void onClick_Btn_CityWarDes_Btn(GameObject caster)
    {

    }


    void onClick_Btn_Two_Btn(GameObject caster)
    {
        if (m_db.copyType == (uint)CopyTypeTable.Camp)
        {
            ReturnBackUIData[] returnData = new ReturnBackUIData[1];
            returnData[0] = new ReturnBackUIData();
            returnData[0].msgid = UIMsgID.eNone;
            returnData[0].panelid = PanelID.FBPanel;
            returnData[0].param = null;

            UIPanelBase.PanelJumpData jumpData = new UIPanelBase.PanelJumpData();
            jumpData.Tabs = new int[1];

            jumpData.Tabs[0] = (int)ShopPanel.TabMode.ZhanXun;//zhanxun商城
            DataManager.Manager<UIPanelManager>().ShowPanel(PanelID.ShopPanel, jumpData: jumpData);
            return;
        }
        //TODO
        if (m_db.copyFlag != (uint)CopyFlag.Huodong && m_db.membType != 0)
        {
            if (DataManager.Manager<TeamDataManager>().IsJoinTeam)
            {
                DataManager.Manager<UIPanelManager>().ShowPanel(PanelID.TeamPanel);
            }
            else
            {
                DataManager.Manager<TeamDataManager>().ReqConvenientTeamListByCopyId(m_db.copyId);
            }
            return;
        }
    }
    public override void Release(bool depthRelease = true)
    {
        base.Release(depthRelease);
        if (m_curQualityAsynSeed != null)
        {
            m_curQualityAsynSeed.Release();
            m_curQualityAsynSeed = null;
        }
    }
    /*
    void onClick_Btn_Two_Btn(GameObject caster)
    {
        //TODO
        if (m_db.copyFlag != (uint)CopyFlag.Huodong && m_db.membType != 0)
        {
            if (DataManager.Manager<TeamDataManager>().IsJoinTeam)
            {
                DataManager.Manager<UIPanelManager>().ShowPanel(PanelID.TeamPanel);

                //设置目标
                if (DataManager.Manager<TeamDataManager>().MainPlayerIsLeader())
                {
                    if (m_db.TeamActivityID != 0)
                    {
                        DataManager.Manager<TeamDataManager>().ReqMatchActivity(m_db.TeamActivityID);//队长选择活动目标    
                    }
                }
            }
            else
            {
                // DataManager.Manager<TeamDataManager>().ReqConvenientTeamListByCopyId(m_db.copyId);
                if (m_db.TeamActivityID != 0)
                {
                    DataManager.Manager<TeamDataManager>().ReqConveientCreateTeam(m_db.TeamActivityID);
                }
            }
            return;
        }

    }
    */
}

