using UnityEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using table;
using GameCmd;
using Client;
using Engine.Utility;

partial class RedEnvelopeSendPanel: UIPanelBase
{
    uint RedEnvelopeMoneyMinNum = 200;

	//红包消耗的元宝最大值
	uint RedEnvelopeMoneyMaxNum = 2000;
	//元宝兑换的货币比例
	uint RedEnvelopeExchangeScale = 1000;
	//元宝兑换的货币类型
	uint RedEnvelopeExchangeMoneyType = 1;
	//发送红包数量
	uint SendRedEnvelopeMinNum = 10;
	uint SendRedEnvelopeMaxNum = 50;

	//祝福语数量
	uint RedEnvelopeBlessMessageMax = 30;
	//超过多少元宝播放红包特效
	uint PlayRedEnvelopeEffectNum = 1000;
	//世界红包领取数量限制
    uint GetWorldRedEnvelopeMaxNum = 5;
    //输入的元宝数量
    uint m_uInputGoldNum = 0;

    uint InputGoldNum
    {
        get
        {
            return m_uInputGoldNum;
        }
        set
        {
            m_uInputGoldNum = value;
            if (m_uInputGoldNum < RedEnvelopeMoneyMinNum)
            {
                m_uInputGoldNum = RedEnvelopeMoneyMinNum;
            }
            if (m_uInputGoldNum > RedEnvelopeMoneyMaxNum)
            {
                m_uInputGoldNum = RedEnvelopeMoneyMaxNum;
            }
            m_label_ConsumePurchaseNum.text = m_uInputGoldNum.ToString();
         
            UpdateShowInfo();
        }
    }
    //输入的红包数量
    uint m_uInputRedEnvelopeNum = 0;
    uint InputSendNum
    {
        get
        {
            return m_uInputRedEnvelopeNum;
        }
        set
        {
            m_uInputRedEnvelopeNum = value;
            if (m_uInputRedEnvelopeNum < SendRedEnvelopeMinNum)
            {
                m_uInputRedEnvelopeNum = SendRedEnvelopeMinNum;
            }
            if (m_uInputRedEnvelopeNum > SendRedEnvelopeMaxNum)
            {
                m_uInputRedEnvelopeNum = SendRedEnvelopeMaxNum;
            }
            m_label_PurchaseNum.text = m_uInputRedEnvelopeNum.ToString();
        }
    }
    //红包元宝加减的增量值
    uint RedEnvelopeStepDelta = 10;

    string m_strBlessMessage = string.Empty;
    protected override void OnLoading()
    {
        base.OnLoading();
        RedEnvelopeMoneyMinNum = GameTableManager.Instance.GetGlobalConfig<uint>("RedEnvelopeMoneyMinNum");
        RedEnvelopeMoneyMaxNum = GameTableManager.Instance.GetGlobalConfig<uint>("RedEnvelopeMoneyMaxNum");
        RedEnvelopeExchangeScale = GameTableManager.Instance.GetGlobalConfig<uint>("RedEnvelopeExchangeScale");
        RedEnvelopeExchangeMoneyType = GameTableManager.Instance.GetGlobalConfig<uint>("RedEnvelopeExchangeMoneyType");
        SendRedEnvelopeMinNum = GameTableManager.Instance.GetGlobalConfig<uint>("SendRedEnvelopeMinNum");
        SendRedEnvelopeMaxNum = GameTableManager.Instance.GetGlobalConfig<uint>("SendRedEnvelopeMaxNum");
        PlayRedEnvelopeEffectNum = GameTableManager.Instance.GetGlobalConfig<uint>("PlayRedEnvelopeEffectNum");
        GetWorldRedEnvelopeMaxNum = GameTableManager.Instance.GetGlobalConfig<uint>("RedPacketRecvTime");
        RedEnvelopeStepDelta = GameTableManager.Instance.GetGlobalConfig<uint>("RedEnvelopeStepDelta");
        RedEnvelopeBlessMessageMax = GameTableManager.Instance.GetGlobalConfig<uint>("RedEnvelopeBlessMessageMax");
        InputGoldNum = RedEnvelopeMoneyMinNum;
        InputSendNum = SendRedEnvelopeMinNum;
    }
    protected override void OnShow(object data)
    {
        base.OnShow(data);
    }
    protected override void OnJump(UIPanelBase.PanelJumpData jumpData)
    {
        base.OnJump(jumpData);
        UIEventListener.Get(m_sprite_consumeInput.gameObject).onClick = OnMoneyInput;
        UIEventListener.Get(m_sprite_redenvelopenum.gameObject).onClick = OnNumInput;
        UIEventListener.Get(m_sprite_btn_Close.gameObject).onClick = OnClosePanel;
        UIEventListener.Get(m_input_blessmassageinput.gameObject).onClick = OnMessageInput;
        InitChannel();
        InitLabel();
    }
    void OnClosePanel(GameObject go)
    {
        HideSelf();
    }
    void OnMessageInput(GameObject go)
    {
        m_strBlessMessage = m_label_wordText.text;
        if (m_strBlessMessage.Length > RedEnvelopeBlessMessageMax)
        {
            m_strBlessMessage = m_strBlessMessage.Substring(0, (int)RedEnvelopeBlessMessageMax) + "...";
        }
    }
    void OnMoneyInput(GameObject go)
    {
        UIPanelManager mgr = DataManager.Manager<UIPanelManager>();
        if (mgr.IsShowPanel(PanelID.HandInputPanel))
        {
            mgr.HidePanel(PanelID.HandInputPanel);
        }
        else
        {
            mgr.ShowPanel(PanelID.HandInputPanel, data: new HandInputPanel.HandInputInitData()
            {
                maxInputNum = RedEnvelopeMoneyMaxNum,
                onInputValue = OnMoneyConfirm,
                showLocalOffsetPosition = new Vector3(435, -40, 0),
            });
        }
    }
    void OnMoneyConfirm(int num)
    {
   
        int playerGoldNum = MainPlayerHelper.GetMoneyNumByType(ClientMoneyType.Gold);
        if(num > playerGoldNum)
        {
            num = playerGoldNum;
        }
        InputGoldNum = (uint)num;
      
    }
    void OnNumInput(GameObject go)
    {
        UIPanelManager mgr = DataManager.Manager<UIPanelManager>();
        if (mgr.IsShowPanel(PanelID.HandInputPanel))
        {
            mgr.HidePanel(PanelID.HandInputPanel);
        }
        else
        {
            mgr.ShowPanel(PanelID.HandInputPanel, data: new HandInputPanel.HandInputInitData()
            {
                maxInputNum = SendRedEnvelopeMaxNum,
                onInputValue = OnNumConfirm,
                showLocalOffsetPosition = new Vector3(322, -180, 0),
            });
        }
       
    }
    void OnNumConfirm(int num)
    {
        InputSendNum =(uint) num;
       
    }
    void InitChannel()
    {
        CHATTYPE chatType = DataManager.Manager<ChatDataManager>().CurChatType;
        if(chatType == CHATTYPE.CHAT_WORLD)
        {
            m_toggle_wolrdselected.value = true;
            m_toggle_clanselected.value = false;
        }
        else
        {
            m_toggle_clanselected.value = true;
            m_toggle_wolrdselected.value = false;

        }
       
    }
    void InitLabel()
    {
        m_label_ConsumePurchaseNum.text = RedEnvelopeMoneyMinNum.ToString();
        m_label_PurchaseNum.text = SendRedEnvelopeMinNum.ToString();
        m_sprite_redmoneyicon.spriteName = MainPlayerHelper.GetMoneyIconByType((int)RedEnvelopeExchangeMoneyType);
        m_label_wordText.text = GameTableManager.Instance.GetGlobalConfig<string>("RedEnveLopeBlessMessage");
    }
    protected override void OnPanelBaseDestory()
    {
        base.OnPanelBaseDestory();
    }
    public override void Release(bool depthRelease = true)
    {
        base.Release(depthRelease);
    }

    void onClick_ConsumeBtnAdd_Btn(GameObject caster)
    {
        InputGoldNum += RedEnvelopeStepDelta;
    }

    void onClick_ConsumeBtnRemove_Btn(GameObject caster)
    {
        InputGoldNum -= RedEnvelopeStepDelta;
    }

    void onClick_BtnAdd_Btn(GameObject caster)
    {
        InputSendNum += 1;
    }

    void onClick_BtnRemove_Btn(GameObject caster)
    {
        InputSendNum -= 1;
    }

    void UpdateShowInfo()
    {
        uint money = InputGoldNum * RedEnvelopeExchangeScale;
        m_label_goldNum_label.text = money.ToString();
    }

    void onClick_Btn_Send_Btn(GameObject caster)
    {
  
        int level = GameTableManager.Instance.GetGlobalConfig<int>("UserRedPacketLevel");
        if(MainPlayerHelper.GetPlayerLevel() < level)
        {
            TipsManager.Instance.ShowLocalFormatTips(LocalTextType.Red_Tips_njicainengfafanghongbao,level);
            return;
        }

        int playerGoldNum = MainPlayerHelper.GetMoneyNumByType(ClientMoneyType.Gold);
        if(InputGoldNum > playerGoldNum)
        {
            TipsManager.Instance.ShowTipsById(5);
            return;
        }
     
        m_strBlessMessage = m_label_wordText.text;
        stSendRedPacketChatUserCmd_C cmd = new stSendRedPacketChatUserCmd_C();
        cmd.coin = m_uInputGoldNum;
        cmd.num = m_uInputRedEnvelopeNum;
        string chanelStr = "世界";
        if(m_toggle_wolrdselected.value)
        {
            cmd.world = true;
        }
        else
        {
            chanelStr = "氏族";
            cmd.world = false;
            if (!DataManager.Manager<ClanManger>().IsJoinFormal)
            {
                TipsManager.Instance.ShowTips(LocalTextType.Red_Tips_nihaimeiyoujiarushizu);
                return;
            }
        }
        string str = string.Format("是否确定花费{0}元宝发放到{1}频道",InputGoldNum,chanelStr);
        TipsManager.Instance.ShowTipWindow(TipWindowType.CancelOk, str, () => {
            cmd.title = m_strBlessMessage;
            NetService.Instance.Send(cmd);
            HideSelf();
        });
    
    }
}
