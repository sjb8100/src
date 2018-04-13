//*************************************************************************
//	创建日期:	2016-12-27 13:56
//	文件名称:	LoginNoticePanel.cs
//  创 建 人:   	Chengxue.Zhao
//	版权所有:	中青宝
//	说    明:	登录公告面板
//*************************************************************************
using System;
using System.Collections.Generic;
using UnityEngine;

public partial class LoginNoticePanel : UIPanelBase
{


    List<Notice> m_lstNotice = new List<Notice>();
    int m_index = 0;
    NoticeBtn m_preNoticeBtn = null;
    NoticeBtn m_currNoticeBtn = null;

    UIXmlRichText m_UIXmlRichText = null;

    UILabel m_noticeLable = null;
    protected override void OnLoading()
    {
        base.OnLoading();
        m_trans_ListContent.gameObject.SetActive(false);
        m_UIXmlRichText = m_widget_NoticeMessage.GetComponent<UIXmlRichText>();
        m_UIXmlRichText.fontSize = 24;
        m_noticeLable = m_scrollview_view.transform.Find("root/noticelabel").GetComponent<UILabel>();

        DataManager.Manager<LoginDataManager>().LoadFromUrl();
    }
    /*放到数据管理类处理
    void LoadFromUrl()
    {
        string strURL = GlobalConfig.Instance().NoticeUrl;
        Engine.Utility.FileUtils.Instance().LoadHttpURL(strURL, LoadNoticeFileFinish, GameEntry.Instance().gameObject);
    }
    private void LoadNoticeFileFinish(WWW www, object param = null)
    {
        if (www.error != null || www.text == string.Empty || www.text.Length == 0)
        {
            UnityEngine.Debug.LogError("LoadNoticeFileFinish----error: " + www.error + "---www.text :" + www.text + "---www.text.Length:" + www.text.Length);
         
            ParesFromFile();
            return;
        }
    
        Engine.JsonNode root = Engine.RareJson.ParseJson(www.text);
        if (root == null)
        {
            Engine.Utility.Log.Warning("LoginNotice 解析{0}文件失败!", www.text);
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

            Notice notice = new Notice();
            notice.index = int.Parse(noticeObj["index"]);
            notice.title = noticeObj["title"];
            notice.content = noticeObj["content"];
            m_lstNotice.Add(notice);
        }
        ShowContent();
    }
    void ParesFromFile()
    {

        string strFilePath = "notice.json";
        Engine.JsonNode root = Engine.RareJson.ParseJsonFile(strFilePath);
        if (root == null)
        {
            Engine.Utility.Log.Warning("LoginNotice 解析{0}文件失败!", strFilePath);
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

            Notice notice = new Notice();
            notice.index = int.Parse(noticeObj["index"]);
            notice.title = noticeObj["title"];
            notice.content = noticeObj["content"];
            m_lstNotice.Add(notice);
        }
        ShowContent();
    }
    */
    protected override void OnShow(object data)
    {
        base.OnShow(data);
        if (DataManager.Manager<LoginDataManager>().NoticeList.Count != 0)
        {
         
            m_lstNotice = DataManager.Manager<LoginDataManager>().NoticeList;
            ShowContent();
        }
        DataManager.Manager<LoginDataManager>().ValueUpdateEvent += LoginNoticePanel_ValueUpdateEvent;
    }

    void LoginNoticePanel_ValueUpdateEvent(object sender, ValueUpdateEventArgs e)
    {
       if(e != null)
       {
           if(e.key == "RefreshNotice")
           {
             
               m_lstNotice = DataManager.Manager<LoginDataManager>().NoticeList;
               ShowContent();
           }
       }
    }

    void ShowContent()
    {
        m_index = 0;
        m_trans_ListRoot.parent.GetComponent<UIScrollView>().ResetPosition();
        m_trans_ListRoot.DestroyChildren();

        NoticeBtn first = null;
        for (int i = 0; i < m_lstNotice.Count; i++)
        {
            GameObject go = NGUITools.AddChild(m_trans_ListRoot.gameObject, m_trans_ListContent.gameObject);
            NoticeBtn btn = go.AddComponent<NoticeBtn>();
            go.SetActive(true);
            btn.Init(m_lstNotice[i], OnClickBtn);
            go.transform.localPosition = new UnityEngine.Vector3(0, -i * 85, 0);

            if (i == 0)
            {
                first = btn;
            }
        }
        if (first != null)
        {
            OnClickBtn(first);
        }
    }
    void OnClickBtn(NoticeBtn btn)
    {
        if (btn == null)
        {
            return;
        }
        m_preNoticeBtn = m_currNoticeBtn;
        if (m_preNoticeBtn != null)
        {
            m_preNoticeBtn.ToggleSelect(false);
        }

        m_currNoticeBtn = btn;
        m_currNoticeBtn.ToggleSelect(true);

        if (m_currNoticeBtn.Notice.index != m_index)
        {
           // m_UIXmlRichText.Clear();
            string strNotice = m_currNoticeBtn.Notice.content;
            if(m_noticeLable != null)
            {
                m_noticeLable.text = strNotice;
            }
            //m_UIXmlRichText.AddXml(RichXmlHelper.RichXmlAdapt(strNotice));
            m_label_NoticeName.text = m_currNoticeBtn.Notice.title;
        }
        m_scrollview_view.ResetPosition();
    }

    protected override void OnHide()
    {
        base.OnHide();
        DataManager.Manager<LoginDataManager>().ValueUpdateEvent -= LoginNoticePanel_ValueUpdateEvent;
    }

    void onClick_BtnClose_Btn(GameObject caster)
    {
        this.HideSelf();
    }


}

public class NoticeBtn : MonoBehaviour
{

    UILabel m_lableTitle = null;
    GameObject m_goSelectMask = null;
    GameObject m_goNewTip = null;

    Notice m_notice;
    public Notice Notice { get { return m_notice; } }

    Action<NoticeBtn> m_callback = null;

    void Awake()
    {
        m_lableTitle = transform.Find("Name").GetComponent<UILabel>();
        m_goSelectMask = transform.Find("mask").gameObject;
        m_goNewTip = transform.Find("Sign_New").gameObject;

        GameObject btn = transform.Find("Bg").gameObject;
        if (btn != null)
        {
            UIEventListener.Get(btn).onClick = OnSelect;
        }
    }

    void OnSelect(GameObject go)
    {
        if (m_callback != null)
        {
            m_callback(this);
        }
    }

    public void ToggleSelect(bool active)
    {
        if (m_goSelectMask != null)
        {
            m_goSelectMask.SetActive(active);
        }
    }

    public void Init(Notice notice, Action<NoticeBtn> callback)
    {
        m_callback = callback;
        m_notice = notice;
        m_lableTitle.text = m_notice.title;
        ToggleSelect(false);
    }


}