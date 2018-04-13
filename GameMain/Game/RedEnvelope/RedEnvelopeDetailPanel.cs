using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using table;
using GameCmd;
using Client;
using Engine.Utility;
using UnityEngine;
partial class RedEnvelopeDetailPanel : UIPanelBase
{
    struct PepoleRedPacketInfo
    {
        public string palyerName;
        public uint goldNum;
    }
    RedEnvelopeDataManager m_dataManager
    {
        get
        {
            return DataManager.Manager<RedEnvelopeDataManager>();
        }
    }
    private CMResAsynSeedData<CMTexture> m_playerCASD = null;
    List<PepoleRedPacketInfo> m_peopleInfoList = new List<PepoleRedPacketInfo>();
    uint m_uRedID = 0;
    uint m_moneyType = 0;
    protected override void OnLoading()
    {
        base.OnLoading();
        UIEventListener.Get(m_sprite_btn_Close.gameObject).onClick = OnClosePanel;
        UIEventListener.Get(m_sprite_btn_take.gameObject).onClick = OnThanksBoss;
        InitDetailScrollView();
    }
    void OnClosePanel(GameObject go)
    {
        HideSelf();
    }
    void OnThanksBoss(GameObject go)
    {
        RedPacket rp = m_dataManager.GetRedPacketByID(m_uRedID);
        if(rp != null)
        {
            string str = DataManager.Manager<TextManager>().GetLocalFormatText(LocalTextType.Red_Tips_xiexielaoban, rp.name);
            CHATTYPE ct = m_dataManager.GetRedEnvelopeChannelByID(m_uRedID);
            DataManager.Manager<ChatDataManager>().SetChatInputType(ct);
            DataManager.Manager<ChatDataManager>().SendChatText(str);
        }
        HideSelf();
    }
    protected override void OnShow(object data)
    {
        base.OnShow(data);

    }
    protected override void OnJump(UIPanelBase.PanelJumpData jumpData)
    {
        base.OnJump(jumpData);
        if (jumpData != null)
        {
            if (jumpData.Param != null)
            {
                stRedPacketInfoChatUserCmd_S cmd = jumpData.Param as stRedPacketInfoChatUserCmd_S;
                if (cmd != null)
                {
                    m_uRedID = cmd.id;
                    m_peopleInfoList.Clear();
                    m_moneyType = cmd.money_type;
                    RedPacket rp = m_dataManager.GetRedPacketByID(cmd.id);
                    if (rp != null)
                    {
                        m_label_text_label.text = rp.title;
                    }

                    int job = (int)cmd.job;
                    SelectRoleDataBase roledata = table.SelectRoleDataBase.Where((GameCmd.enumProfession)job, (GameCmd.enmCharSex)1);
                    if (roledata != null && null != m__icon)
                    {
                        UIManager.GetTextureAsyn(roledata.strprofessionIcon, ref m_playerCASD, () =>
                        {
                            if (m__icon != null)
                            {
                                m__icon.mainTexture = null;
                            }
                        }, m__icon, false);
                    }
                    if (MainPlayerHelper.GetMainPlayer() != null)
                    {
                        string playerName = MainPlayerHelper.GetMainPlayer().GetName();

                        m_label_playername_label.text = rp.name;
                        if(rp.name == playerName )
                        {
                            m_sprite_btn_take.gameObject.SetActive(false);
                        }
                        else
                        {
                            m_sprite_btn_take.gameObject.SetActive(true);

                        }
                    }
                    m_sprite_goldicon.spriteName = MainPlayerHelper.GetMoneyIconByType((int)cmd.money_type);
                    m_sprite_goldicon2.spriteName = MainPlayerHelper.GetMoneyIconByType((int)cmd.money_type);
                    m_label_procureNum_label.text = cmd.gold.ToString();
                    m_label_takeNum_label.text = (cmd.num_max - cmd.num).ToString() + "/" + cmd.num_max.ToString();
                    m_label_totalNum_label.text = cmd.gold_max.ToString();
                    for (int i = 0; i < cmd.name.Count; i++)
                    {
                        PepoleRedPacketInfo info = new PepoleRedPacketInfo();
                        info.palyerName = cmd.name[i];
                        info.goldNum = cmd.recv[i];
                        m_peopleInfoList.Add(info);
                    }
                    m_peopleInfoList.Sort((x1, x2) =>
                    {
                        if (x1.goldNum > x2.goldNum)
                        {
                            return -1;
                        }
                        else if (x1.goldNum < x2.goldNum)
                        {
                            return 1;
                        }
                        else
                        {
                            return 0;
                        }
                    });
                    m_ctor_redEnvelopeDetailScrowView.CreateGrids(cmd.name.Count);
                }
            }
        }

    }
    void InitDetailScrollView()
    {
        Transform resObj = m_trans_uiRedEnvelopeDetailGrid;
        if (resObj != null)
        {
            m_ctor_redEnvelopeDetailScrowView.Initialize<UIRedEnvelopeDetailGrid>(resObj.gameObject, OnShowGridData, OnGridUIEventDlg);
            m_ctor_redEnvelopeDetailScrowView.RefreshCheck();

        }
    }
    void OnShowGridData(UIGridBase grid, int index)
    {
        UIRedEnvelopeDetailGrid redGrid = grid as UIRedEnvelopeDetailGrid;
        if (redGrid != null)
        {
            if (index < m_peopleInfoList.Count)
            {
                PepoleRedPacketInfo info = m_peopleInfoList[index];
                redGrid.InitDetailGrid(info.palyerName, info.goldNum, index, m_moneyType);
            }

        }
    }
    void OnGridUIEventDlg(UIEventType eventType, object data, object param)
    {

    }
    public override void Release(bool depthRelease = true)
    {
        base.Release(depthRelease);


        if (null != m_playerCASD)
        {
            m_playerCASD.Release();
            m_playerCASD = null;
        }
    }

}
