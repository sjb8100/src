using System;
using table;
using GameCmd;
using System.Collections.Generic;
using UnityEngine;
using Client;
public enum QuestionTab
{
    None = 0,
    GongGao = 1,
    WenJuan_1 = 2,
    WenJuan_2 =3,
    WenJuan_3 =4,
    FanKui = 5,
}
partial class QuestionPanel : UIPanelBase
{
    QuestionTab activedType = QuestionTab.None;
    UIXmlRichText m_UIXmlRichText = new UIXmlRichText();
    List<QuestionNotice> m_lstNotice = new List<QuestionNotice>();
    List<string> m_lstDependece = null;
    RewardStatus status = RewardStatus.Reward_Lock;
    RewardStatus status2 = RewardStatus.Reward_Lock;
    string H5Address = "";
    public class QuestionNotice
    {
        public int index;
        public string title;
        public string content;
    }
    #region Override
    protected override void OnLoading()
    {
        base.OnLoading();
        ParseFromFile();
        Engine.Utility.EventEngine.Instance().AddEventListener((int)Client.GameEventID.REFRESHQUESTIONPANEL, OnEvent);
    }
    void OnEvent(int eventID, object param)
    {
        switch ((Client.GameEventID)eventID)
        {
            case GameEventID.REFRESHQUESTIONPANEL:
                {
                    SetActivedType(activedType, true);
                }
                break;
        }
    }
    protected override void OnShow(object data)
    {
        base.OnShow(data);
        SetActivedType(QuestionTab.GongGao);
    }
    protected override void OnHide()
    {
        base.OnHide();
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
        if (null != jumpData.Tabs && jumpData.Tabs.Length > 0)
        {
            firstTabData = jumpData.Tabs[0];

        }
        else
        {
            firstTabData = (int)QuestionTab.GongGao;
        }
        UIFrameManager.Instance.OnCilckTogglePanel(this.PanelId, 1, firstTabData);
    }
    public override UIPanelBase.PanelData GetPanelData()
    {
        PanelData pd = base.GetPanelData();
        pd.JumpData = new PanelJumpData();
        pd.JumpData.Tabs = new int[2];
        pd.JumpData.Tabs[0] = (int)activedType;
        return pd;
    }
    public override bool OnTogglePanel(int tabType, int pageid)
    {
        SetActivedType((QuestionTab)pageid);
        return base.OnTogglePanel(tabType, pageid);
    }
  
    #endregion

    void SetActivedType(QuestionTab type, bool force = false)
    {
        if (DataManager.Manager<QuestionManager>().QuestDic.ContainsKey((uint)type))
        {
            status = DataManager.Manager<QuestionManager>().QuestDic[(uint)type].state;

            UpdateApplyRedPoint((int)type, status == RewardStatus.Reward_Open);
    
        }
       
        status2 = DataManager.Manager<QuestionManager>().FeedBackStatus;
        if (type == activedType && !force)
        {
            return;
        }
        activedType = type;
        SetContent(type);
       
        if (DataManager.Manager<UIPanelManager>().IsShowPanel(PanelID.QuestionPanel))
        {
           
            if (DataManager.Manager<QuestionManager>().canGetFeed)
            {
                UpdateApplyRedPoint((int)QuestionTab.FanKui, DataManager.Manager<QuestionManager>().canGetFeed);
            }
        }

    }

    void SetContent(QuestionTab type)
    {
        m_UIXmlRichText.Clear();
        QuestionDataBase db = GameTableManager.Instance.GetTableItem<QuestionDataBase>((uint)type);
        if (db == null)
        {
            Engine.Utility.Log.Error("问卷表格id{0}的数据为空", (uint)type);
            return;
        }

        switch (type)
        {
            case QuestionTab.GongGao:
                m_trans_GongGao.gameObject.SetActive(true);
                m_trans_WenJuan.gameObject.SetActive(false);
                m_trans_FanKui.gameObject.SetActive(false);
                m_label_title.text = db.title;
                string content = db.descr;
                m_UIXmlRichText.AddXml(RichXmlHelper.RichXmlAdapt(content));
                break;
            case QuestionTab.WenJuan_1:
                  m_trans_GongGao.gameObject.SetActive(false);
                m_trans_WenJuan.gameObject.SetActive(true);
                m_trans_FanKui.gameObject.SetActive(false);
                m_label_WenJuanText.text = db.descr;
                H5Address = db.H5Address;
                CreateRewardObj(m_trans_WJRewardGrid, db);
                SetWenJuanBtn();
                break;
            case QuestionTab.WenJuan_2:
                m_trans_GongGao.gameObject.SetActive(false);
                m_trans_WenJuan.gameObject.SetActive(true);
                m_trans_FanKui.gameObject.SetActive(false);
                m_label_WenJuanText.text = db.descr;
                H5Address = db.H5Address;
                CreateRewardObj(m_trans_WJRewardGrid, db);
                SetWenJuanBtn();
                break;
            case QuestionTab.WenJuan_3:
                m_trans_GongGao.gameObject.SetActive(false);
                m_trans_WenJuan.gameObject.SetActive(true);
                m_trans_FanKui.gameObject.SetActive(false);
                m_label_WenJuanText.text = db.descr;
                H5Address = db.H5Address;
                CreateRewardObj(m_trans_WJRewardGrid, db);
                SetWenJuanBtn();
                break;
            case QuestionTab.FanKui:
                m_trans_GongGao.gameObject.SetActive(false);
                m_trans_WenJuan.gameObject.SetActive(false);
                m_trans_FanKui.gameObject.SetActive(true);
                m_label_FanKuiText.text = db.descr;
                CreateRewardObj(m_trans_FKRewardGrid, db);
                SetFanKuiBtn();
                break;
        }
    }
    private void UpdateApplyRedPoint(int index, bool value)
    {
        UITabGrid tabGrid = null;
        Dictionary<int, UITabGrid> dicTabs = null;
        if (dicUITabGrid.TryGetValue(1, out dicTabs))
        {
            if (dicTabs != null && dicTabs.TryGetValue(index, out tabGrid))
            {
                tabGrid.SetRedPointStatus(value);
            }
        }
    }

    void SetWenJuanBtn()
    {

        if (status == RewardStatus.Reward_Get)
        {
            m_btn_WenJuanGetRewardBtn.isEnabled = false;
            m_sprite_questWarning2.gameObject.SetActive(false);
            m_label_questLabel2.text = "已领取";
        }
        else if (status == RewardStatus.Reward_Open)
        {
            m_btn_WenJuanGetRewardBtn.isEnabled = true;
            m_sprite_questWarning2.gameObject.SetActive(true);
            m_label_questLabel2.text = "领取奖励";
        }
        else
        {
            m_btn_WenJuanGetRewardBtn.isEnabled = false;
            m_sprite_questWarning2.gameObject.SetActive(false);
            m_label_questLabel2.text = "领取奖励";
        }
    }
    void SetFanKuiBtn()
    {
        if (status2 == RewardStatus.Reward_Get)
        {
            m_label_feedLabel.text = "我要反馈";
            m_sprite_FeedWarning.gameObject.SetActive(false);
        }
        else if (status2 == RewardStatus.Reward_Open)
        {
            m_label_feedLabel.text = "领取奖励";
            m_sprite_FeedWarning.gameObject.SetActive(true);
        }
        else
        {
            m_label_feedLabel.text = "我要反馈";
            m_sprite_FeedWarning.gameObject.SetActive(false);
        }
        UILabel label = m_label_times.GetComponent<UILabel>();
        QuestionDataBase table = GameTableManager.Instance.GetTableItem<QuestionDataBase>((uint)QuestionTab.FanKui);
        if (label != null && table != null)
        {
            label.text = string.Format("{0}/{1}", (table.rewarn_time - DataManager.Manager<QuestionManager>().FeedRewardLeftTime), table.rewarn_time);
        }
    }
    void CreateRewardObj(Transform parent, QuestionDataBase db)
    {
         AddCreator(parent);
         m_lst_UIItemRewardDatas.Clear();
        if (!string.IsNullOrEmpty(db.money_reward))
        {
            string[] param = db.money_reward.Split(';');
            for (int m = 0; m < param.Length; m++)
            {
                string[] param2 = param[m].Split('_');
                uint money = 0;
                uint num = 0;
                if (uint.TryParse(param2[0], out money) && uint.TryParse(param2[1], out num))
                {
                    CurrencyIconData data = CurrencyIconData.GetCurrencyIconByMoneyType((ClientMoneyType)money);
                    if (data == null)
                    {
                        return;
                    }
                    m_lst_UIItemRewardDatas.Add(new UIItemRewardData()
                    {
                        itemID = data.itemID,
                        num = num,
                    });
                }
            }

        }
        if (!string.IsNullOrEmpty(db.item_reward))
        {
            uint itemID = 0;
            uint itemNum = 0;
            string[] data = db.item_reward.Split(';');
           
            for (int i = 0; i < data.Length; i++)
            {
                string[] data2 = data[i].Split('_');
                if (uint.TryParse(data2[0], out itemID) && uint.TryParse(data2[1], out itemNum))
                {
                      m_lst_UIItemRewardDatas.Add(new UIItemRewardData()
                    {
                        itemID = itemID,
                        num = itemNum,
                    });
                }
            }
        }
        m_ctor_UIItemRewardCreator.CreateGrids(m_lst_UIItemRewardDatas.Count);
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
                    uint num = data.num;
                    itemShow.SetGridData(itemID, num);
                }
            }
        }
    }
    #endregion

    public override void Release(bool depthRelease = true)
    {
        base.Release(depthRelease);
    }

    protected override void OnPanelBaseDestory()
    {
        base.OnPanelBaseDestory();
        Engine.Utility.EventEngine.Instance().RemoveEventListener((int)Client.GameEventID.REFRESHQUESTIONPANEL, OnEvent);
        Release();
    }


    void ParseFromFile()
    {
        m_UIXmlRichText = m_widget_root.GetComponent<UIXmlRichText>();
        UnityEngine.Object prefab = UIManager.GetResGameObj("prefab/grid/richtextlabel.unity3d", "Assets/UI/prefab/Grid/RichTextLabel.prefab");
        m_UIXmlRichText.protoLabel = prefab as GameObject;
        m_UIXmlRichText.fontSize = 24;
        string strFilePath = "notice.json";
        Engine.JsonNode root = Engine.RareJson.ParseJsonFile(strFilePath);
        if (root == null)
        {
            Engine.Utility.Log.Warning("华夏宝典解析{0}文件失败！", strFilePath);
            return;
        }
        Engine.JsonArray noticeArray = (Engine.JsonArray)root["notice"];
        for (int i = 0, imax = noticeArray.Count; i < imax; i++)
        {
            Engine.JsonObject noticeObj = (Engine.JsonObject)noticeArray[i];
            if (noticeObj == null)
            {
                continue;
            }
            QuestionNotice notice = new QuestionNotice();
            notice.index = int.Parse(noticeObj["index"]);
            notice.title = noticeObj["title"];
            notice.content = noticeObj["content"];
            m_lstNotice.Add(notice);
        }
    }
    #region NGUI BTN
    void onClick_Close_Btn(GameObject caster)
    {
        HideSelf();
    }

    void onClick_GongGaoConfirmBtn_Btn(GameObject caster)
    {
        HideSelf();
    }

    void onClick_WenJuanConfirmBtn_Btn(GameObject caster)
    {
        if (string.IsNullOrEmpty(H5Address))
        {
            return;
        }
        Application.OpenURL(H5Address);
        if (status == RewardStatus.Reward_Lock)
        {
            NetService.Instance.Send(new stQuestionPropertyUserCmd_C() { id = (uint)activedType });
        }
    }
    void onClick_WenJuanGetRewardBtn_Btn(GameObject caster)
    {
        if (status == RewardStatus.Reward_Open)
        {
            if (activedType == QuestionTab.WenJuan_1 || activedType == QuestionTab.WenJuan_2 || activedType == QuestionTab.WenJuan_3)
            {
                 NetService.Instance.Send(new stGetRewardQuestionPropertyUserCmd_C() { id = (uint)activedType });
            }
              else 
            {
                Engine.Utility.Log.Error("问卷页签不包括当前页签{0}", activedType);
            }
          
        }
        else
        {
            TipsManager.Instance.ShowTips("请填写问卷之后领取奖励");
        }
    }


    void onClick_FanKuiConfirmBtn_Btn(GameObject caster)
    {
        if (status2 == RewardStatus.Reward_Open)
        {
            NetService.Instance.Send(new stGetRewardQuestionPropertyUserCmd_C() { id = (uint)QuestionTab.FanKui });
        }
        else
        {
            UIPanelBase.PanelJumpData jumpdata = new UIPanelBase.PanelJumpData();
            if (jumpdata.Tabs == null)
            {
                jumpdata.Tabs = new int[2];
            }
            jumpdata.Tabs[0] = (int)SettingPanel.RightTagType.feedback;
            DataManager.Manager<UIPanelManager>().ShowPanel(PanelID.SettingPanel, jumpData: jumpdata);
            NetService.Instance.Send(new stQuestionPropertyUserCmd_C() { id =(uint)QuestionTab.FanKui });
        }

    }
    #endregion

}
