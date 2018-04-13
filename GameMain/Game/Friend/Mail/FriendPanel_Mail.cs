using System;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System.Text;
using Common;
using GameCmd;
using table;
using Client;

partial class FriendPanel : UIPanelBase
{
    ListMailInfo readNoteMail;
    private uint selectedMailIndex = 0;

    private List<ListMailInfo> mailList = new List<ListMailInfo>();

    ListMailInfo curMail = null;
    private enum MailType
    {
        None = 0,
        NoteMail = 1,
        ItemMail,
        Max,
    }

    void MailEventRegister(bool value)
    {
        if (value)
        {
            //shujubianhua   showmail   第一次打开showmail
            Engine.Utility.EventEngine.Instance().AddEventListener((int)Client.GameEventID.PLAYER_LOGIN_SUCCESS, EventCallBack);
            Engine.Utility.EventEngine.Instance().AddEventListener((int)Client.GameEventID.MAIL_ADDNEWMAIL, EventCallBack);
            Engine.Utility.EventEngine.Instance().AddEventListener((int)Client.GameEventID.MAIL_STATECHANGENOATTACH, EventCallBack);
            Engine.Utility.EventEngine.Instance().AddEventListener((int)Client.GameEventID.MAIL_STATECHANGE, EventCallBack);
            Engine.Utility.EventEngine.Instance().AddEventListener((int)Client.GameEventID.MAINPANEL_SHOWREDWARING, EventCallBack);
            Engine.Utility.EventEngine.Instance().AddEventListener((int)Client.GameEventID.FRIEND_ADDNEWMSG, EventCallBack);
            DataManager.Manager<MailManager>().ValueUpdateEvent += OnValueUpdateMailEventArgs;
        }
        else
        {
            Engine.Utility.EventEngine.Instance().RemoveEventListener((int)Client.GameEventID.PLAYER_LOGIN_SUCCESS, EventCallBack);
            Engine.Utility.EventEngine.Instance().RemoveEventListener((int)Client.GameEventID.MAINPANEL_SHOWREDWARING, EventCallBack);
            Engine.Utility.EventEngine.Instance().RemoveEventListener((int)Client.GameEventID.MAIL_ADDNEWMAIL, EventCallBack);
            Engine.Utility.EventEngine.Instance().RemoveEventListener((int)Client.GameEventID.MAIL_STATECHANGENOATTACH, EventCallBack);
            Engine.Utility.EventEngine.Instance().RemoveEventListener((int)Client.GameEventID.MAIL_STATECHANGE, EventCallBack);
            Engine.Utility.EventEngine.Instance().RemoveEventListener((int)Client.GameEventID.FRIEND_ADDNEWMSG, EventCallBack);
            DataManager.Manager<MailManager>().ValueUpdateEvent -= OnValueUpdateMailEventArgs;

        }
    }
    void OnValueUpdateMailEventArgs(object obj, ValueUpdateEventArgs v)
    {
        if (v.key.Equals("OnUpdateMailList"))
        {
            RefreshMailListUI();
               
        }
    }
    void RefreshMailListUI() 
    {
        int preNum = mailList.Count;
        if (mailList != null)
        {
            mailList.Clear();
            mailList.AddRange( DataManager.Manager<MailManager>().MailList);
        }
        int gap = preNum - mailList.Count;
        if (gap < 0)
        {
            //加邮件
            gap = Mathf.Abs(gap);
            for (int i = 0; i < gap; i++)
            {
                m_ctor_MailScroll.InsertData(preNum +i);
            }
        }
        else if (gap >0 )
        {
            //减邮件
            for (int i = 0; i < gap; i++)
            {
                m_ctor_MailScroll.RemoveData(preNum - i - 1);
            }
        }
        m_ctor_MailScroll.UpdateActiveGridData();

        UpdateMailNumDetail();
    }
    void EventCallBack(int nEventID, object param)
    {
        if (nEventID == (int)Client.GameEventID.MAIL_ADDNEWMAIL)
        {
            UpdateApplyRedPoint(FriendPanelPageEnum.Page_邮箱);
        }
        else if (nEventID == (int)Client.GameEventID.MAIL_STATECHANGENOATTACH)
        {
            Client.stMailStateChangeNoAttach mscna = (Client.stMailStateChangeNoAttach)param;
            if (mscna.mailid != 0)
            {
                ResReadMail((uint)MailType.NoteMail, mscna.mailid, mscna.index);
                UpdateApplyRedPoint(FriendPanelPageEnum.Page_邮箱);
            }

        }
        else if (nEventID == (int)Client.GameEventID.MAIL_STATECHANGE)
        {
            Client.stMailStateChange msc = (Client.stMailStateChange)param;
            if (msc.mailid != 0)
            {
                ResReadMail((uint)MailType.ItemMail, msc.mailid, msc.index);
                GetItemFinish();
                UpdateApplyRedPoint(FriendPanelPageEnum.Page_邮箱);
            }
        }
        else if (nEventID == (int)Client.GameEventID.FRIEND_ADDNEWMSG)
        {
            UpdateApplyRedPoint(FriendPanelPageEnum.Page_最近);
        }

    }

    /// <summary>
    /// 1.每次点击主界面显示panel界面   如果有新增邮件则找出这个邮件的ID创建
    ///
    /// </summary>
    void ShowMailPanel()
    {
        if (mailList != null)
        {
            mailList.Clear();
            mailList.AddRange(DataManager.Manager<MailManager>().MailList);
        }
        UpdateMailNumDetail();
        if (null != m_ctor_MailScroll)
        {
            m_ctor_MailScroll.RefreshCheck();
            m_ctor_MailScroll.Initialize<UIMailGrid>(m_trans_UIMailGrid.gameObject, OnUpdateMailGridData, OnGridUIEventDlg);
            m_ctor_MailScroll.CreateGrids((null != mailList) ? mailList.Count : 0);
            if (mailList.Count > 0)
            {
                UIMailGrid grid = m_ctor_MailScroll.GetGrid<UIMailGrid>(0);
                if (grid != null)
                {
                    SelectMail(grid);
                }
              
            }
           
        }
    }
    void UpdateMailNumDetail() 
    {
        bool haveMail = mailList.Count > 0;
        m_label_mark_message.gameObject.SetActive(!haveMail);
        m_btn_one_btn_get.gameObject.SetActive(haveMail);
        m_btn_DeleteBtn.gameObject.SetActive(haveMail);
        m_btn_lingqu_btn.gameObject.SetActive(false);
        m_sprite_arrow_left.gameObject.SetActive(false);
        m_sprite_arrow_right.gameObject.SetActive(false);
        m_label_mail_num.text = "数量: " + mailList.Count + "/100";
        if (!haveMail)
        {
            m_label_mail_text_name.text = "";
            m_label_mail_text.text = "";
            m_label_recieve_time.text = "";
        }
    }
    private void OnUpdateMailGridData(UIGridBase grid, int index)
    {
        if (grid is UIMailGrid)
        {
            UIMailGrid mailGrid = grid as UIMailGrid;
            if (index < mailList.Count)
            {
                ListMailInfo mail = mailList[index];
                mailGrid.SetGridData(mailList[index]);
                mailGrid.SetMailIndex((uint)index);
                mailGrid.SetSelect(index == selectedMailIndex);
                mailGrid.SetState(mail);
                mailGrid.name = mail.mailid.ToString();
            }
         
        }
    }
    private void OnGridUIEventDlg(UIEventType eventType, object data, object param)
    {
        switch (eventType)
        {
            case UIEventType.Click:
                if (data is UIMailGrid)
                {
                    UIMailGrid grid = data as UIMailGrid;
                    if (null != grid)
                    {
                        SelectMail(grid);
                    }
                }
                break;
        }
    }
    void SelectMail(UIMailGrid grid) 
    {
        SetSelect(grid.MailIndex);
        SetMsgText(grid.MailIndex);
        selectedMailIndex = grid.MailIndex;
        if ((int)grid.MailIndex < mailList.Count)
        {
            readNoteMail = mailList[(int)grid.MailIndex];
        }
      
    }
    void ResReadMail(uint type, uint mailID, int index)
    {
        UIMailGrid mg = m_ctor_MailScroll.GetGrid<UIMailGrid>(index);
        //mg.SetState(DataManager.Manager<MailManager>().mail_dic[mailID]);
        OnUpdateMailGridData(mg, index);
        if (type == (uint)MailType.ItemMail)
        {
            m_btn_lingqu_btn.gameObject.SetActive(false);
            m_ctor_UIItemRewardCreator.ClearAll();
        }



    }
    /// <summary>
    /// 左侧选中
    /// </summary>
    /// <param name="mailIndex"></param>
    private void SetSelect(uint mailIndex)
    {
        if (null != m_ctor_MailScroll)
        {
            UIMailGrid grid = m_ctor_MailScroll.GetGrid<UIMailGrid>((int)selectedMailIndex);
            if (null != grid)
            {
                grid.SetSelect(false);
            }
            grid = m_ctor_MailScroll.GetGrid<UIMailGrid>((int)mailIndex);
            if (null != grid)
            {
                grid.SetSelect(true);
            }
        }
        this.selectedMailIndex = mailIndex;
        if (selectedMailIndex < mailList.Count)
        {
            if (mailList[(int)selectedMailIndex].state == 0)
            {
                DataManager.Manager<MailManager>().ReadMail(mailList[(int)selectedMailIndex].mailid);
            }
        }
      

    }
    /// <summary>
    /// 右侧Text塞数据
    /// </summary>
    /// <param name="mailIndex"></param>
    private void SetMsgText(uint mailIndex)
    {
        if (mailList == null)
        {
            m_btn_lingqu_btn.gameObject.SetActive(false);
            m_btn_one_btn_get.gameObject.SetActive(false);
            m_label_mark_message.gameObject.SetActive(true);
            return;
        }
        if (mailList.Count == 0)
        {
            m_btn_lingqu_btn.gameObject.SetActive(false);
            m_btn_one_btn_get.gameObject.SetActive(false);
            m_label_mark_message.gameObject.SetActive(true);
            return;
        }
        if (mailIndex >= mailList.Count)
        {
            return;
        } 
        curMail = mailList[(int)mailIndex];
        m_label_mail_text_name.text = curMail.title;
        m_label_mail_text.text = curMail.text;
        DateTime dt = DataManager.Manager<MailManager>().GetCreatTime(curMail.createTime);
        m_label_recieve_time.text = dt.Year + "年" +
                                    dt.Month + "月" +
                                    dt.Day + "日";

        m_lst_UIItemRewardDatas.Clear();
        if (curMail.item.Count > 0)
        {
            for (int i = 0; i < curMail.item.Count; i++)
            {
                m_lst_UIItemRewardDatas.Add(new UIItemRewardData()
                {
                    itemID = curMail.item[i].dwObjectID,
                    num = curMail.item[i].dwNum,
                    data = curMail.item[i],
                });
            }
        }
        if (curMail.sendMoney.Count > 0)
        {
            foreach (var money in curMail.sendMoney)
            {
                CurrencyIconData data = CurrencyIconData.GetCurrencyIconByMoneyType((ClientMoneyType)money.moneytype);
                if (data == null)
                {
                    return;
                }
                 m_lst_UIItemRewardDatas.Add(new UIItemRewardData()
                {
                    itemID = data.itemID,
                    num = money.moneynum,
                });
            }
        }

         m_ctor_UIItemRewardCreator.CreateGrids(m_lst_UIItemRewardDatas.Count);
         int count = m_lst_UIItemRewardDatas.Count;
         m_sprite_arrow_left.gameObject.SetActive(count >5);
         m_sprite_arrow_right.gameObject.SetActive(count > 5);
         m_btn_lingqu_btn.gameObject.SetActive(count >0);
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
                    itemShow.SetGridData(itemID, num, true, false, true, data.data);
                    if (m_lst_UIItemRewardDatas.Count > 5)
                    {
                        itemShow.AddDrag();
                    }
                }
            }
        }
    }
    #endregion






    void GetItemFinish()
    {
        TipsManager.Instance.ShowTips("领取附件成功！");
    }


    /// <summary>
    /// 右侧界面中的附件领取
    /// </summary>
    /// <param name="caster"></param>
    void onClick_Lingqu_btn_Btn(GameObject caster)
    {
        if (selectedMailIndex < mailList.Count)
        {
            DataManager.Manager<MailManager>().CollectAttachment(mailList[(int)selectedMailIndex]);
            m_sprite_arrow_left.gameObject.SetActive(false);
            m_sprite_arrow_right.gameObject.SetActive(false);
        }

    }
    /// <summary>
    /// 一键领取，详细解释点开看里面
    /// </summary>
    /// <param name="caster"></param>
    void onClick_One_btn_get_Btn(GameObject caster)
    {
        DataManager.Manager<MailManager>().StartCollectAll();
        bool bl = DataManager.Manager<MailManager>().GetAllAttachments();
        if (bl)
        {
            TipsManager.Instance.ShowTips("当前无附件可以领取！");
            return;
        }

    }
    void onClick_DeleteBtn_Btn(GameObject caster)
    {
        DataManager.Manager<MailManager>().ClearMail();
    }
}
