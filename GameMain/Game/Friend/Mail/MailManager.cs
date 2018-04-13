using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common;
using UnityEngine;
using GameCmd;
using table;
using Client;
public class MailInfo
{
    public uint mailId;
    public uint leftTime;
    public uint creatTime;
    public uint state;  //0  new ;  1 read  ;  2  get
    public List<SendMoney> sendMoney;
    public string mailTitle;
    public string text;
    //ItemSerialize是一个附件  items对应qwThisID
    public Dictionary<uint, ItemSerialize> item_dic = new Dictionary<uint, ItemSerialize>();
}


public class MailManager : BaseModuleData, IManager
{
//    private uint readMailId;
     List<ListMailInfo> mailList = new List<ListMailInfo>();
    public List<ListMailInfo> MailList 
    {
        get 
        {
            return mailList;
        }
        set 
        {
            mailList = value;
        }
    }
    public  Dictionary<uint, ListMailInfo> mail_dic = new Dictionary<uint, ListMailInfo>();
    private List<uint> deletedList = new List<uint>();
    private List<uint> mailSortList = new List<uint>();


    public bool HaveMailCanGet 
    {
        get 
        {
            return ViewMailState();
        }
    }
    // 是否一键领取
    private bool m_bCollectAll = false;


    public uint WorldLevel { set; get; }
    public void Initialize()
    {
        RegisterGlobalEvent(true);
       
    }
    private void RegisterGlobalEvent(bool register)
    {
        if (register)
        {
            Engine.Utility.EventEngine.Instance().AddEventListener((int)GameEventID.PLAYER_LOGIN_SUCCESS, OnPlayerLoginSuccess);
            Engine.Utility.EventEngine.Instance().AddEventListener((int)GameEventID.RECONNECT_SUCESS, OnPlayerLoginSuccess);
            Engine.Utility.EventEngine.Instance().AddEventListener((int)GameEventID.NETWORK_CONNECTE_CLOSE, OnPlayerLoginSuccess);
        }
        else
        {
            Engine.Utility.EventEngine.Instance().RemoveEventListener((int)GameEventID.PLAYER_LOGIN_SUCCESS, OnPlayerLoginSuccess);
            Engine.Utility.EventEngine.Instance().RemoveEventListener((int)GameEventID.RECONNECT_SUCESS, OnPlayerLoginSuccess);
            Engine.Utility.EventEngine.Instance().RemoveEventListener((int)GameEventID.NETWORK_CONNECTE_CLOSE, OnPlayerLoginSuccess);
        }
    }
    public void ClearData()
    {

    }
    public void Reset(bool depthClearData = false)
    {
        mailList.Clear();
        mail_dic.Clear();
        deletedList.Clear();
        mailSortList.Clear();
        m_bCollectAll = false;
    }

    public void Process(float deltaTime)
    {

    }

    /// <summary>
    /// 接收消息并将消息存储到m_lstMailInfo  每封邮件mail  附件mail.items
    /// </summary>
    /// <param name="mailList"></param>
    public void SetMailDic(List<ListMailInfo> mList)
    {
        MailList = mList;
       
        MailSort(MailList);
        for (int i = 0; i < mList.Count;i++ )
        {
            mail_dic.Add(mList[i].mailid, mList[i]);
        }
    }
    public void GetNewMail(stNotifyNewMailUserCmd_S cmd)
    {
        if (MailList == null)
        {
            MailList = new List<ListMailInfo>();
        }
        this.MailList.Add(cmd.data);
        MailSort(MailList);
        mail_dic.Add(cmd.data.mailid, cmd.data);
        Engine.Utility.EventEngine.Instance().DispatchEvent((int)Client.GameEventID.MAIL_ADDNEWMAIL, null);

        stShowMainPanelRedPoint st = new stShowMainPanelRedPoint()
        {
            modelID = (int)WarningEnum.Mail,
            direction = (int)WarningDirection.None,
            bShowRed = true,
        };
        Engine.Utility.EventEngine.Instance().DispatchEvent((int)Client.GameEventID.MAINPANEL_SHOWREDWARING, st);
    }
    
    /// <summary>
    /// 成功登陆的话读取所有邮件状态并在通知主界面邮件图标闪烁或者有红点
    /// </summary>
    /// <param name="eventid"></param>
    /// <param name="cmd"></param>
    private void OnPlayerLoginSuccess(int eventid, object cmd)
    {
        switch (eventid)
        {
            case (int)GameEventID.PLAYER_LOGIN_SUCCESS:
                    NetService.Instance.Send(new stRequetListMailUserCmd_C());
                break;
            case (int)GameEventID.RECONNECT_SUCESS:
                    //重连重置邮件提示
                    NetService.Instance.Send(new stRequetListMailUserCmd_C());           
                break;
            case (int)GameEventID.NETWORK_CONNECTE_CLOSE:
                {
                    Reset();
                }
                break;       
        }
     
    }

    bool ViewMailState()
    {
        for (int i = 0; i < MailList.Count; i++)
        {
            if (MailList[i].state == 0)
            {
                return true;
            }
            else if (MailList[i].state == 1  )
            {
               if(MailList[i].sendMoney.Count + MailList[i].item.Count > 0 )
               {
                   return true;
               }
            }
        }
        return false;
    }

    public void ClearMail()
    {
        if (MailList == null)
        {
            return;
        }
        bool haveMailToDel = false;
        deletedList.Clear();
        for (int i = 0; i < MailList.Count; i++)
        {
            if (MailList[i].state == 1 && MailList[i].sendMoney.Count == 0 && MailList[i].item.Count == 0)
            {              
                NetService.Instance.Send(new stDelMailUserCmd_CS() { mailid = MailList[i].mailid });
                if (!deletedList.Contains(MailList[i].mailid))
                {
                    deletedList.Add(MailList[i].mailid);
                }
                haveMailToDel = true;
            }
            else if (MailList[i].state == 2)
            {
                NetService.Instance.Send(new stDelMailUserCmd_CS() { mailid = MailList[i].mailid });
                haveMailToDel = true;
                if (!deletedList.Contains(MailList[i].mailid))
                {
                    deletedList.Add(MailList[i].mailid);
                }
            }
        }
        if (!haveMailToDel)
        {
            TipsManager.Instance.ShowTips("当前无已读邮件");
        }
    }
    public void DelMail(uint mailId) 
    {
        for (int i = 0; i < MailList.Count; i++)
       {
            if(MailList[i].mailid == mailId)
            {
                this.MailList.Remove(MailList[i]);
                mail_dic.Remove(mailId);
            }
        }
        if (deletedList.Contains(mailId))
        {
            deletedList.Remove(mailId);
            if (deletedList.Count == 0)
            {
                TipsManager.Instance.ShowTips("成功删除全部已读邮件");
            }
        }      
        ValueUpdateEventArgs arg = new ValueUpdateEventArgs("OnUpdateMailList", null, null);
        DispatchValueUpdateEvent(arg);
    }
    /// <summary>
    /// 点击邮件列表时从服务器交互成功后显示
    /// </summary>
    /// <param name="mailId"></param>


    public void ReadMail(uint mailId)
    {
        NetService.Instance.Send(new stReadTollMailUserCmd_CS() { mailid = mailId });
    }
    public void ReadMailFinish(uint mailId)
    {
        ListMailInfo mail = null;
        int index =0;
        for (int i = 0; i < MailList.Count;i++ )
        {
            if (MailList[i].mailid == mailId)
            {
                index =i;
                mail = MailList[i];
                MailList[i].state = 1;
            }
        }
        if (mail == null)
        {
            Engine.Utility.Log.Error("邮件阅读出错，id不存在!");
            return;
        }
        if (mail.item.Count == 0 && mail.sendMoney.Count == 0)
        {
            Client.stMailStateChangeNoAttach mscn = new Client.stMailStateChangeNoAttach();
            mscn.mailid = mailId;
            mscn.index = index;
            Engine.Utility.EventEngine.Instance().DispatchEvent((int)Client.GameEventID.MAIL_STATECHANGENOATTACH, mscn);
        }
    }

    public void StartCollectAll()
    {
        m_bCollectAll = true;
        CollectNextMailAttachment();

    }

    public void StopCollectAll()
    {
        m_bCollectAll = false;
    }

    private void CollectNextMailAttachment()
    {
        bool bSendCollect = false;
        for (int i = 0; i < MailList.Count; i++)
        {
            if (MailList[i].state == 0)
            {
               ReadMail(MailList[i].mailid);
            }
         
            if (HasAttachment(MailList[i]))
            {
                CollectAttachment(MailList[i]);
                bSendCollect = true;
                break;
            }
        }

        if (!bSendCollect)
        {
            m_bCollectAll = false;
        }
    }
    private bool HasAttachment(ListMailInfo m)
    {
        bool hasAttach = false;
        if (m.item.Count > 0 || m.sendMoney.Count > 0)
        {
            hasAttach = true;
        }
        return hasAttach;
    }



    // mailId 邮件ID
    public void CollectAttachment(ListMailInfo m)
    {
        stGetItemMailUserCmd_CS cmd = new stGetItemMailUserCmd_CS() { mailid = m.mailid, item_num = m.item.Count };
        NetService.Instance.Send(cmd);
    }


    // 处理领取附件成功
    public void OnCollectAttachment(uint mailId)
    {
        if (mailId == 0)
        {
            Engine.Utility.Log.Error("OnCollectAttachment----邮件ID=0非法!");
            return;
        }
        int index = 0 ;
        for (int i = 0; i < MailList.Count;i++ )
        {
            if(MailList[i].mailid == mailId)
            {
                index = i;
                MailList[i].state = 2;
                MailList[i].item.Clear();
                MailList[i].sendMoney.Clear();
            }
        }
        //所有到这一步的邮件都可以视为已经提取
        Client.stMailStateChange msc = new Client.stMailStateChange();
        msc.mailid = mailId;
        msc.index = index;
        Engine.Utility.EventEngine.Instance().DispatchEvent((int)Client.GameEventID.MAIL_STATECHANGE, msc);
        if (m_bCollectAll)
        {
            CollectNextMailAttachment();
        }
    }

    public bool GetAllAttachments()
    {
        bool bl = false;
        int i = 0;
        for (int m = 0; m < MailList.Count;m++ )
        {
            if (MailList[m].item.Count > 0 || MailList[m].sendMoney.Count > 0)
            {
                i++;
            }

        }
        if (i == 0)
        {
            bl = true;
        }
        return bl;
    }


    /// <summary>
    /// 获取邮件创建时间
    /// </summary>
    /// <param name="timeStamp"></param>
    /// <returns></returns>
    public DateTime GetCreatTime(uint timeStamp)
    {
        DateTime UnixBase = new DateTime(1970, 1, 1, 0, 0, 0);
        return DateTime.Now.AddSeconds(timeStamp - (DateTime.Now - UnixBase).TotalSeconds);
    }
    public TimeSpan GetLeftTime(uint timeStamp)
    {
        TimeSpan ts = new TimeSpan(0, 0, (int)timeStamp);

        return ts;
    }

    public void MailSort(List<ListMailInfo> list)
    {
        if (list.Count > 1)
        {
            for (int i = 0; i < list.Count; i++)
            {
                for (int j = 0; j < list.Count; j++)
                {
                    if (list[i].createTime > list[j].createTime)
                    {
                        ListMailInfo temp = list[i];
                        list[i] = list[j];
                        list[j] = temp;
                    }
                }
            }
        }
    }


    public void OnRecWorldLevel(uint lv) 
    {
        WorldLevel = lv;
        Engine.Utility.EventEngine.Instance().DispatchEvent((int)GameEventID.REFRESHWORLDLEVEL, WorldLevel);
    }
}