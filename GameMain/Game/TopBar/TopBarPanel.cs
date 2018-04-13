using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Client;

partial class TopBarPanel
{
    #region Override method

    GameObject m_goPropetyPrefab = null;
    List<UIPlayerProprty> m_lstUIPlayerProprty = new List<UIPlayerProprty>(3);
    List<string> m_lstDepende = new List<string>();
    protected override void OnLoading()
    {
        base.OnLoading();
        AdjustUI();
        m_goPropetyPrefab = m_sprite_uiPropetyPrefab.gameObject;
        m_goPropetyPrefab.SetActive(false);
    }

    protected override void OnShow(object data)
    {
        base.OnShow(data);
        if (data != null && data is UIPanelManager.LocalPanelInfo)
        {
            UIPanelManager.LocalPanelInfo uidata = data as UIPanelManager.LocalPanelInfo;
            //Client.UIPanelInfo uidata = data as Client.UIPanelInfo;
            m_label_panelName.text = uidata.TitleName;
            ShowPropety(uidata.TopBarProperty);
        }

        Engine.Utility.EventEngine.Instance().AddEventListener((int)Client.GameEventID.UIEVENT_REFRESHCURRENCYNUM, OnUIEvent);
    }

    void OnUIEvent(int type,object param)
    {
        if (type == (int)Client.GameEventID.UIEVENT_REFRESHCURRENCYNUM)
        {
            for (int k = 0; k < m_lstUIPlayerProprty.Count; k++)
            {
                if (!m_lstUIPlayerProprty[k].gameObject.activeSelf)
                {
                    continue;
                }
                m_lstUIPlayerProprty[k].Refersh();
            }
        }
    }

    protected override void OnHide()
    {
        base.OnHide();
        Engine.Utility.EventEngine.Instance().RemoveEventListener((int)Client.GameEventID.UIEVENT_REFRESHCURRENCYNUM, OnUIEvent);
    }

    void ShowPropety(List<int> lstPlayerPropety)
    {
        for (int k = 0; k < m_lstUIPlayerProprty.Count; k++)
        {
            m_lstUIPlayerProprty[k].gameObject.SetActive(false);
        }

        UIPlayerProprty prop  = null;

        for (int i = 0; i < lstPlayerPropety.Count; ++i)
        {
            if (i >= m_lstUIPlayerProprty.Count)
            {
                GameObject go = NGUITools.AddChild(m_trans_currencyarea.gameObject, m_goPropetyPrefab);
                if (go == null)
                {
                    continue;
                }
                prop = go.AddComponent<UIPlayerProprty>();
                m_lstUIPlayerProprty.Add(prop);
            }
            else
            {
                prop = m_lstUIPlayerProprty[i];
            }

            if (prop != null)
            {
                prop.gameObject.SetActive(true);
                prop.Init(lstPlayerPropety[i]);
                prop.transform.localPosition = new UnityEngine.Vector3(-i * 181, 0, 0);
            }
        }

    }

    private void AdjustUI()
    {
        Vector2 offset = UIRootHelper.Instance.OffsetSize;
        Vector2 targetFillSize = UIRootHelper.Instance.TargetFillSize;
        if (null != m_sprite_TopbarBg)
        {
            m_sprite_TopbarBg.width = (int)targetFillSize.x;
        }

        if (null != m_sprite_MaskTop)
        {
            m_sprite_MaskTop.width = (int)targetFillSize.x;
            m_sprite_MaskTop.height = Mathf.CeilToInt(offset.y * 0.5f);
        }

        if (null != m_sprite_MaskBottom)
        {
            m_sprite_MaskBottom.width = (int)targetFillSize.x;
            m_sprite_MaskBottom.height = Mathf.CeilToInt(offset.y * 0.5f);
        }
    }
  
    #endregion
    #region UIEvent
    void onClick_Close_Btn(GameObject caster)
    {
        DataManager.Manager<UIPanelManager>().OnPanelCacheBack();
    }

    void onClick_BtnHelp_Btn(GameObject caster)
    {
    }

    void onClick_ShopBtn_Btn(GameObject caster)
    {
        if (!DataManager.Manager<UIPanelManager>().IsShowPanel(PanelID.ShopPanel))
        {
            DataManager.Manager<UIPanelManager>().ShowPanel(PanelID.ShopPanel);
        }
    }
    #endregion

}