using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Engine.Utility;

partial class ChannelSettingPanel : UIPanelBase
{
    ChatDataManager m_chatManager = null;
    Dictionary<BtnType, GameCmd.CHATTYPE> m_dicChannelType = null;
    List<UIToggle> m_toggles = new List<UIToggle>();
    List<UIToggle> m_autotoggles = new List<UIToggle>();
    private bool IsLiuliangAuto = false;
    protected override void OnAwake()
    {
        base.OnAwake();
        m_dicChannelType = new Dictionary<BtnType, GameCmd.CHATTYPE>((int)BtnType.Max - 1);
        m_chatManager = DataManager.Manager<ChatDataManager>();
        m_dicChannelType.Add(BtnType.world, GameCmd.CHATTYPE.CHAT_WORLD);
        m_dicChannelType.Add(BtnType.team, GameCmd.CHATTYPE.CHAT_TEAM);
        m_dicChannelType.Add(BtnType.demon, GameCmd.CHATTYPE.CHAT_DEMON);
        m_dicChannelType.Add(BtnType.clan, GameCmd.CHATTYPE.CHAT_CLAN);
        m_dicChannelType.Add(BtnType.near, GameCmd.CHATTYPE.CHAT_MAP);
        m_dicChannelType.Add(BtnType.sys, GameCmd.CHATTYPE.CHAT_SYS);

        m_dicChannelType.Add(BtnType.world_auto, GameCmd.CHATTYPE.CHAT_WORLD);
        m_dicChannelType.Add(BtnType.team_auto, GameCmd.CHATTYPE.CHAT_TEAM);
        m_dicChannelType.Add(BtnType.demon_auto, GameCmd.CHATTYPE.CHAT_DEMON);
        m_dicChannelType.Add(BtnType.clan_auto, GameCmd.CHATTYPE.CHAT_CLAN);
        m_dicChannelType.Add(BtnType.near_auto, GameCmd.CHATTYPE.CHAT_MAP);

        m_toggles.Add(m_btn_world.GetComponent<UIToggle>());
        m_toggles.Add(m_btn_team.GetComponent<UIToggle>());
        m_toggles.Add(m_btn_demon.GetComponent<UIToggle>());
        m_toggles.Add(m_btn_clan.GetComponent<UIToggle>());
        m_toggles.Add(m_btn_near.GetComponent<UIToggle>());
        m_toggles.Add(m_btn_sys.GetComponent<UIToggle>());

        m_autotoggles.Add(m_btn_world_auto.GetComponent<UIToggle>());
        m_autotoggles.Add(m_btn_team_auto.GetComponent<UIToggle>());
        m_autotoggles.Add(m_btn_demon_auto.GetComponent<UIToggle>());
        m_autotoggles.Add(m_btn_clan_auto.GetComponent<UIToggle>());
        m_autotoggles.Add(m_btn_near_auto.GetComponent<UIToggle>());
    }

    protected override void OnShow(object data)
    {
        base.OnShow(data);
        //根据设置来显示
        foreach (var item in m_toggles)
        {
            BtnType btntype = (BtnType)System.Enum.Parse(typeof(BtnType), item.name);
            if (m_dicChannelType.ContainsKey(btntype))
                item.value = m_chatManager.SimpleChannelContain(m_dicChannelType[btntype]);
        }
        foreach (var item in m_autotoggles)
        {
            BtnType btntype = (BtnType)System.Enum.Parse(typeof(BtnType), item.name);
            if (m_dicChannelType.ContainsKey(btntype))
                item.value = m_chatManager.IsAutoPlayVoice(m_dicChannelType[btntype]);
        }
        IsLiuliangAuto = m_chatManager.IsAutoPlayInLiuLiang();
        m_btn_liuliang.GetComponent<UIToggle>().value = IsLiuliangAuto;
    }

    void onClick_Btn_cancel_Btn(GameObject caster)
    {
        HideSelf();
    }

    void onClick_Btn_sure_Btn(GameObject caster)
    {
        foreach (var item in m_toggles)
        {
            BtnType btntype = (BtnType)System.Enum.Parse(typeof(BtnType), item.name);
            if (m_dicChannelType.ContainsKey(btntype))
            {
                if (item.value != m_chatManager.SimpleChannelContain(m_dicChannelType[btntype]))
                {
                    m_chatManager.SetSimpleChannel(m_dicChannelType[btntype], item.value);
                }
            }
        }
        foreach (var item in m_autotoggles)
        {
            BtnType btntype = (BtnType)System.Enum.Parse(typeof(BtnType), item.name);
            if (m_dicChannelType.ContainsKey(btntype))
            {
                if(item.value!=m_chatManager.IsAutoPlayVoice(m_dicChannelType[btntype]))
                m_chatManager.SetAutoPlayVoice(m_dicChannelType[btntype], item.value);
            }
        }
        m_chatManager.SetAutoInLiuLiangPlay(IsLiuliangAuto);
        HideSelf();
    }

    void OnLiuLiangToggerHandle()
    {

    }
    void OnBtnsClick(BtnType type)
    {
        if (type == BtnType.liuliang)
        {
            IsLiuliangAuto = m_btn_liuliang.GetComponent<UIToggle>().value;
        }
    }

    void onClick_Close_Btn(GameObject caster)
    {
        HideSelf();
    }
}
