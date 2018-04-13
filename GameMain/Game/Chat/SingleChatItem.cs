using System;
using System.Collections.Generic;
using UnityEngine;

public class SingleChatItem : MonoBehaviour
{
    const int VOICEBGWIDTH = 210;
    Transform m_transNormal = null;
    Transform m_transSys = null;

    UIXmlRichText m_normalRichTextContext;
    UIXmlRichText m_sysRichTextContext;
    UISpriteEx m_spriteTitleBg = null;
    UILabel m_lableSysTitle = null;
    UIWidget m_spriteSysBg = null;
    UISpriteEx m_spriteNameBg = null;
    UISprite m_bgSprite;

    UITexture m_headIcon;
    UILabel m_lableChannel;
    UILabel m_lableName;
    //voice
    GameObject m_goVoice = null;
    UISprite m_spriteVoiceBg = null;
    UILabel m_labelVoice = null;
    GameObject m_goVoiceTip = null;
    UISprite m_VoiceSprite = null;
    [SerializeField]
    Bounds bound;
    /// <summary>
    /// 面板
    /// </summary>
    int m_panelWidth;
    /// <summary>
    /// 头像和边框距离
    /// </summary>
    public int BorderX = 10;
    public int headRightOffset = 5;//头像和边框之间的距离
    public int itemOffset = 5;
    public int rightOffset = 35;
    public ChatInfo m_chatdata;
    string m_strVoicePath = "";
    PanelID m_parentPanelID;
    //ChatPanel
    public ChatPanel m_parent;
    public int m_height;

    private bool IsPlaying = false;
    private float fliprecent = 0.25f;

    UILabel m_label_blessmsg;//红包祝福语
    Transform m_trans_normalInfo;
    Transform m_trans_redPacket;//红包节点


    private CMResAsynSeedData<CMTexture> iuiIcon = null;

    public void Release(bool depthRelease = true)
    {
        if (null != iuiIcon)
        {
            iuiIcon.Release(true);
            iuiIcon = null;
        }
    }

    void Awake()
    {
        InitSysUI();
        InitNormal();
        Engine.Utility.EventEngine.Instance().AddEventListener((int)Client.GameEventID.CHAT_VOICE_PLAY, OnEvent);

    }


    void InitSysUI()
    {
        m_transSys = transform.Find("system");
        m_lableSysTitle = m_transSys.Find("title").GetComponent<UILabel>();
        m_spriteTitleBg = m_transSys.Find("titleBg").GetComponent<UISpriteEx>();
        if (m_spriteTitleBg != null)
        {
            m_spriteTitleBg.pivot = UIWidget.Pivot.TopLeft;
        }
        m_sysRichTextContext = m_transSys.Find("xmlText").GetComponent<UIXmlRichText>();
        m_spriteSysBg = m_sysRichTextContext.transform.GetComponent<UIWidget>();

        m_sysRichTextContext.UrlClicked += OnClickUrl;
    }

    void InitNormal()
    {
        m_transNormal = transform.Find("normal");
        m_trans_normalInfo = m_transNormal.Find("normalinfo");
        m_normalRichTextContext = m_transNormal.Find("normalinfo/xmlText").GetComponent<UIXmlRichText>();
        m_normalRichTextContext.UrlClicked += OnClickUrl;

        m_bgSprite = m_normalRichTextContext.transform.GetComponent<UISprite>();
        m_headIcon = m_transNormal.Find("icon_head").GetComponent<UITexture>();
        if (m_headIcon != null)
        {
            m_headIcon.pivot = UIWidget.Pivot.TopLeft;
        }
        UIEventListener.Get(m_headIcon.gameObject).onClick = OnHeadClick;

        m_lableChannel = m_transNormal.Find("normalinfo/channelName").GetComponent<UILabel>();

        if (m_lableChannel != null)
        {
            m_lableChannel.pivot = UIWidget.Pivot.TopLeft;
        }
        m_lableName = m_transNormal.Find("normalinfo/playerName").GetComponent<UILabel>();
        if (m_lableName != null)
        {
            m_lableName.pivot = UIWidget.Pivot.TopLeft;
        }
        m_spriteNameBg = m_transNormal.Find("normalinfo/channelNameBg").GetComponent<UISpriteEx>();
        if (m_spriteNameBg != null)
        {
            m_spriteNameBg.pivot = UIWidget.Pivot.TopLeft;
        }

        //voice 
        m_goVoice = m_transNormal.Find("normalinfo/voice").gameObject;
        m_spriteVoiceBg = m_transNormal.Find("normalinfo/voice/bg/voicebg").GetComponent<UISprite>();
        m_labelVoice = m_transNormal.Find("normalinfo/voice/length").GetComponent<UILabel>();
        m_goVoiceTip = m_transNormal.Find("normalinfo/voice/voicetip").gameObject;
        m_VoiceSprite = m_transNormal.Find("normalinfo/voice/bg/Sprite").GetComponent<UISprite>();
        m_VoiceSprite.type = UIBasicSprite.Type.Filled;
        m_VoiceSprite.fillDirection = UIBasicSprite.FillDirection.Horizontal;
        m_VoiceSprite.fillAmount = 1;
        UIEventListener.Get(m_spriteVoiceBg.gameObject).onClick = OnClickVoice;

        m_trans_redPacket = m_transNormal.Find("redEnvelope");

        if (m_trans_redPacket != null)
        {
            Transform redText = m_trans_redPacket.Find("redText");
            if(redText == null)
            {
                return;
            }
            UIEventListener.Get(redText.gameObject).onClick = OnOpenRedPacket;
            Transform bless = redText.Find("blessmessage");
            if(bless != null)
            {
                m_label_blessmsg = bless.GetComponent<UILabel>();
            }
            
        }


    }

    void OnOpenRedPacket(GameObject go)
    {
        if (!DataManager.Manager<UIPanelManager>().IsShowPanel(PanelID.RedEnvelopePanel))
        {
            DataManager.Manager<UIPanelManager>().ShowPanel(PanelID.RedEnvelopePanel,panelShowAction:(panel)=>{
                DataManager.Manager<UIPanelManager>().HidePanel(PanelID.ChatPanel);
            });
        }
    }
    void OnEvent(int nEventId, object param)
    {
        if ((int)Client.GameEventID.CHAT_VOICE_PLAY == nEventId)
        {
            if (string.IsNullOrEmpty(m_strVoicePath))
            {
                return;
            }
            Client.stVoicePlay vp = (Client.stVoicePlay)param;
            if (vp.fileId.Equals(m_strVoicePath))
            {
                IsPlaying = false;
                m_VoiceSprite.fillAmount = 1;
                m_chatdata.played = true;
                m_goVoiceTip.SetActive(false);

                if (m_chatdata != null && DataManager.Manager<ChatDataManager>().IsCanAutoPlayVoice(m_chatdata.Channel))
                {
                    if (m_parent != null)
                    {
                        m_parent.PlayNext(m_chatdata.Timestamp);
                    }
                }
            }

        }
    }

    public void PlayeVoice()
    {
        if (m_chatdata != null)
        {
            if (GVoiceManger.Instance.IsPlaying)
            {
                GVoiceManger.Instance.StopPlayRecordFile();
                return;
            }
            m_strVoicePath = DateTime.Now.ToFileTime().ToString();
            GVoiceManger.Instance.DownloadRecordedFile(m_chatdata.voiceFileid, m_strVoicePath);
            IsPlaying = true;
            m_VoiceSprite.fillAmount = 0;
        }
    }

    void OnClickVoice(GameObject go)
    {
        PlayeVoice();
    }

    void OnHeadClick(GameObject go)
    {
        if (m_chatdata != null)
        {
            DataManager.Instance.Sender.RequestPlayerInfoForOprate(m_chatdata.Id, PlayerOpreatePanel.ViewType.Normal);
        }
    }

    void OnClickUrl(UIWidget sender, string url)
    {
        Debug.Log(url);

        if (!string.IsNullOrEmpty(url))
        {
            DataManager.Manager<ChatDataManager>().OnUrlClick(url);
        }
    }

    public void SetChatInfo(int panelWidth, ChatInfo text, PanelID panelid = PanelID.None)
    {
        m_parentPanelID = panelid;
        m_panelWidth = panelWidth;
        float halfPanelWidth = m_panelWidth * 0.5f;
        if ((text.Channel == GameCmd.CHATTYPE.CHAT_SYS || text.Id == 0)&&!text.IsRedPacket)
        {
            if (m_transNormal.gameObject.activeSelf)
            {
                m_transNormal.gameObject.SetActive(false);
            }
            if (!m_transSys.gameObject.activeSelf)
            {
                m_transSys.gameObject.SetActive(true);
            }


            if (m_spriteTitleBg != null)
            {
                m_spriteTitleBg.transform.localPosition = new UnityEngine.Vector3(-halfPanelWidth, 5, 0);

                m_spriteTitleBg.ChangeSprite(GetChannelIndex(GameCmd.CHATTYPE.CHAT_SYS));
            }

            if (m_lableSysTitle != null)
            {
                if (text.Id == 0)
                {
                    m_lableSysTitle.text = DataManager.Manager<ChatDataManager>().GetChannelStr(text.Channel);
                }
                else
                {
                    m_lableSysTitle.text = "系统";
                }
                m_spriteSysBg.height = m_lableSysTitle.height;

                // m_spriteTitleBg.width = m_lableSysTitle.width + 10;
                //m_spriteTitleBg.height = m_lableSysTitle.height + 10;

                m_lableSysTitle.transform.localPosition = new Vector3(-halfPanelWidth + 5, 0, 0);
            }

            m_spriteSysBg.transform.localPosition = new Vector3(m_spriteTitleBg.transform.localPosition.x + m_spriteTitleBg.width + 2, 0, 0);

            // m_spriteSysBg.width = GetMaxWidth() - m_lableSysTitle.width;
            m_sysRichTextContext.Clear();
            m_sysRichTextContext.AddXml(text.Content);
            bound = NGUIMath.CalculateRelativeWidgetBounds(transform, false);
            m_chatdata = null;
        }
        else
        {
            if (!m_transNormal.gameObject.activeSelf)
            {
                m_transNormal.gameObject.SetActive(true);
            }
            if (m_transSys.gameObject.activeSelf)
            {
                m_transSys.gameObject.SetActive(false);
            }

            m_chatdata = text;

            m_normalRichTextContext.Clear();
            if (text.IsRedPacket)
            {
                m_trans_normalInfo.gameObject.SetActive(false);
                m_trans_redPacket.gameObject.SetActive(true);

            }
            else
            {
                m_trans_normalInfo.gameObject.SetActive(true);
                m_trans_redPacket.gameObject.SetActive(false);
            }
            m_label_blessmsg.text = text.RichText;
            if (text.IsMe)
            {
                SetChatMsgUI_Right(text);
            }
            else
            {
                SetChatMsgUI_Left(text);
            }
        }
    }

    /// <summary>
    /// 左边其他人UI
    /// </summary>
    /// <param name="text"></param>
    private CMResAsynSeedData<CMTexture> iuiIconAtlas = null;
    private void SetChatMsgUI_Left(ChatInfo text)
    {
        Vector3 origin = m_trans_redPacket.localPosition;
        m_trans_redPacket.localPosition = new Vector3(146, origin.y, 0);
        if (m_headIcon != null)
        {
            if (text.Channel == GameCmd.CHATTYPE.CHAT_GM)
            {
                UIManager.GetTextureAsyn("n031", ref iuiIconAtlas, () =>
                {
                    if (null != m_headIcon)
                    {
                        m_headIcon.mainTexture = null;
                    }
                }, m_headIcon);
            }
            else 
            {
                table.SelectRoleDataBase sdb = table.SelectRoleDataBase.Where((GameCmd.enumProfession)text.job, (GameCmd.enmCharSex)1);
                if (sdb != null)
                {                 
                    UIManager.GetTextureAsyn(sdb.strprofessionIcon, ref iuiIcon, 
                        () => { if (m_headIcon != null) { m_headIcon.mainTexture = null; } }, 
                        m_headIcon, false);
                }
            }
            m_headIcon.transform.localPosition = new Vector3(-m_panelWidth * 0.5f + BorderX, 0, 0);

           
        }
        if (GameCmd.CHATTYPE.CHAT_GMTOOL == text.Channel)
        {
            text.Channel = GameCmd.CHATTYPE.CHAT_GMTOOL;
        }

        if (text.Channel == GameCmd.CHATTYPE.CHAT_SILENT)
        {
            if (DataManager.Manager<RelationManager>().IsMyFriend(text.Id))
            {
                m_lableChannel.text = "好友";
            }
            else
            {
                m_lableChannel.text = "陌生人";
            }
        }
        else
        {
            m_lableChannel.text = DataManager.Manager<ChatDataManager>().GetChannelStr(text.Channel);
        }

        m_lableChannel.transform.localPosition = m_headIcon.transform.localPosition + new Vector3(m_headIcon.width + 25, 0, 0);


        m_lableName.text = text.Name;
        m_lableName.transform.localPosition = m_lableChannel.transform.localPosition + new Vector3(m_lableChannel.width+5, 0, 0);

        if (m_spriteNameBg != null)
        {
            m_spriteNameBg.transform.localPosition = m_lableChannel.transform.localPosition + new Vector3(-5, 5, 0);
            m_spriteNameBg.width = m_lableChannel.width + 10;
            m_spriteNameBg.height = m_lableChannel.fontSize + 10;
            m_spriteNameBg.enabled = true;
            m_spriteNameBg.ChangeSprite(GetChannelIndex(text.Channel));

        }

        UISprite sp = m_bgSprite as UISprite;
        sp.spriteName = "biankuang_duihuazuo";

        m_bgSprite.width = GetMaxWidth() - 10;//左边小箭头

        if (!string.IsNullOrEmpty(text.voiceFileid))
        {
            m_goVoice.gameObject.SetActive(true);
            m_goVoiceTip.SetActive(!m_chatdata.played);

            m_labelVoice.text = string.Format("{0}'", text.voiceLegth.ToString());

            //从左边25开始
            m_normalRichTextContext.offset = new Vector2(25, 0);
            //没有执行AddNewLine()先指定m_layout
            m_normalRichTextContext.m_layout = new Vector2(25, -60f);
        }
        else
        {
            m_goVoice.gameObject.SetActive(false);

            //从左边25开始
            m_normalRichTextContext.offset = new Vector2(25, 0);
            //没有执行AddNewLine()先指定m_layout
            m_normalRichTextContext.m_layout = new Vector2(25, -10f);

        }
        m_normalRichTextContext.AddXml(text.Content);

        m_bgSprite.height = Mathf.CeilToInt(-m_normalRichTextContext.m_layout.y) + 10;

        if (m_normalRichTextContext.overline)
        {
            m_bgSprite.width = GetMaxWidth();
        }
        else
        {
            m_bgSprite.width = Mathf.CeilToInt(m_normalRichTextContext.m_layout.x) + 15;//多添加15
        }

        if (!string.IsNullOrEmpty(text.voiceFileid))
        {
            if (m_bgSprite.width < VOICEBGWIDTH)
            {
                m_bgSprite.width = VOICEBGWIDTH;
            }
            m_bgSprite.transform.localPosition = new Vector3(m_headIcon.transform.localPosition.x + m_headIcon.width + BorderX + headRightOffset, m_bgSprite.transform.localPosition.y, 0);

            float posx = m_bgSprite.transform.localPosition.x + m_spriteVoiceBg.width * 0.5f + 40;
            m_goVoice.transform.localPosition = new UnityEngine.Vector3(posx, -m_headIcon.height * 0.5f - 28, 0);
            m_goVoice.transform.localRotation = Quaternion.Euler(Vector3.zero);
            m_labelVoice.transform.localRotation = Quaternion.Euler(Vector3.zero);
            m_labelVoice.transform.localPosition = new UnityEngine.Vector3(-m_labelVoice.transform.localPosition.x, 0, 0);
        }
        else
        {
            m_bgSprite.transform.localPosition = new Vector3(m_headIcon.transform.localPosition.x + m_headIcon.width + BorderX + headRightOffset, m_bgSprite.transform.localPosition.y, 0);

        }
        bound = NGUIMath.CalculateRelativeWidgetBounds(transform, false);
    }

    public static int GetChannelIndex(GameCmd.CHATTYPE chatType)
    {
        int index = 1;
        switch (chatType)
        {
            case GameCmd.CHATTYPE.CHAT_NONE:
                index = 1;
                break;
            case GameCmd.CHATTYPE.CHAT_SYS:
                index = 2;
                break;
            case GameCmd.CHATTYPE.CHAT_WORLD:
                index = 3;
                break;
            case GameCmd.CHATTYPE.CHAT_TEAM:
            case GameCmd.CHATTYPE.CHAT_RECRUIT:
                index = 4;
                break;
            case GameCmd.CHATTYPE.CHAT_CLAN:
                index = 5;
                break;
            case GameCmd.CHATTYPE.CHAT_MAP:
                index = 6;
                break;
            case GameCmd.CHATTYPE.CHAT_DEMON:
                index = 7;
                break;
            case GameCmd.CHATTYPE.CHAT_SILENT:
                index = 8;
                break;
            default:
                break;
        }
        return index;
    }
    int GetMaxWidth()
    {
        int width = m_panelWidth - rightOffset - headRightOffset;
        if (m_headIcon != null)
        {
            width -= m_headIcon.width;
        }
        return width;
    }
    /// <summary>
    /// 右边自己的UI
    /// </summary>
    /// <param name="text"></param>
    private void SetChatMsgUI_Right(ChatInfo text)
    {
        Vector3 origin = m_trans_redPacket.localPosition;
        m_trans_redPacket.localPosition = new Vector3(187, origin.y, 0);
        if (m_headIcon != null)
        {
            //m_headIcon.ChangeSprite(text.job);
            //m_headIcon.MakePixelPerfect();
            m_headIcon.transform.localPosition = new Vector3(m_panelWidth * 0.5f - BorderX - m_headIcon.width, 0, 0);

            table.SelectRoleDataBase sdb = table.SelectRoleDataBase.Where((GameCmd.enumProfession)text.job, (GameCmd.enmCharSex)1);
            if (sdb != null)
            {
                UIManager.GetTextureAsyn(sdb.strprofessionIcon, ref iuiIcon, () => { if (m_headIcon != null) { m_headIcon.mainTexture = null; } }, m_headIcon, false);
            }
        }
        if (m_spriteNameBg != null)
        {
            m_spriteNameBg.enabled = false;
        }
        m_lableChannel.text = "";
        m_lableName.text = "";
        UISprite sp = m_bgSprite as UISprite;
        sp.spriteName = "biankuang_duihuayou";
        m_bgSprite.width = GetMaxWidth() - 10;//右边小箭头

      
        if (!string.IsNullOrEmpty(text.voiceFileid))
        {
            m_goVoiceTip.gameObject.SetActive(false);
            //m_spriteVoiceBg.transform.localPosition = new UnityEngine.Vector3(85, -57, 0);
            m_goVoice.SetActive(true);
            m_labelVoice.text = string.Format("{0}'", text.voiceLegth.ToString());
            m_normalRichTextContext.offset = new Vector2(15f, 0);
            //没有执行AddNewLine()先指定m_layout
            m_normalRichTextContext.m_layout = new Vector2(15f, -60f);
        }
        else
        {
            m_goVoice.gameObject.SetActive(false);
            m_normalRichTextContext.offset = new Vector2(15f, 0);
            //没有执行AddNewLine()先指定m_layout
            m_normalRichTextContext.m_layout = new Vector2(15f, -10f);

        }

        m_normalRichTextContext.AddXml(text.Content);

        m_bgSprite.height = Mathf.CeilToInt(-m_normalRichTextContext.m_layout.y) + 10;

        if (m_normalRichTextContext.overline)
        {
            m_bgSprite.width = GetMaxWidth();
        }
        else
        {
            m_bgSprite.width = Mathf.CeilToInt(m_normalRichTextContext.m_layout.x) + 25;
        }


        if (!string.IsNullOrEmpty(text.voiceFileid))
        {
            if (m_bgSprite.width < VOICEBGWIDTH)
            {
                m_bgSprite.width = VOICEBGWIDTH;
            }
            float posx = m_headIcon.transform.localPosition.x - BorderX - m_bgSprite.width;
            m_bgSprite.transform.localPosition = new Vector3(posx, -m_headIcon.height * 0.5f, 0);
            posx = m_headIcon.transform.localPosition.x - m_spriteVoiceBg.width * 0.5f - 40;
            m_goVoice.transform.localPosition = new UnityEngine.Vector3(posx, -m_headIcon.height * 0.5f - 28, 0);
            m_goVoice.transform.localRotation = Quaternion.Euler(new Vector3(0, 180, 0));
            m_labelVoice.transform.localRotation = Quaternion.Euler(new Vector3(0, 180, 0));
            m_labelVoice.transform.localPosition = new UnityEngine.Vector3(-m_labelVoice.transform.localPosition.x, 0, 0);
        }
        else
        {
            float posx = m_headIcon.transform.localPosition.x - BorderX - m_bgSprite.width;
            m_bgSprite.transform.localPosition = new Vector3(posx, -m_headIcon.height * 0.5f, 0);
        }

 
        bound = NGUIMath.CalculateRelativeWidgetBounds(transform, false);

    }

    public void Clear()
    {
        m_normalRichTextContext.Clear();
    }

    public float GetHeight()
    {
        m_height = (int)bound.size.y + itemOffset;
        return bound.size.y + itemOffset;
    }
    public float GetOffsetY()
    {
        return -bound.center.y + bound.extents.y + itemOffset * 0.5f;
    }
    public void Update()
    {
        if (IsPlaying && Time.frameCount % 10 == 0)
        {
            m_VoiceSprite.fillAmount += fliprecent;
            if (m_VoiceSprite.fillAmount >= 1)
                m_VoiceSprite.fillAmount = 0;
        }
    }
}
