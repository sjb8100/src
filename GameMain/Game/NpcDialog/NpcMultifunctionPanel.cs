//*************************************************************************
//	创建日期:	2016-12-5 14:10
//	文件名称:	NpcMultifunctionPanel.cs
//  创 建 人:   	Chengxue.Zhao
//	版权所有:	中青宝
//	说    明:	npc功能选择
//*************************************************************************
using System;
using System.Collections.Generic;
using UnityEngine;

partial class NpcMultifunctionPanel : UIPanelBase
{

    LangTalkData m_LangTalkData = null;
    UIXmlRichText m_UIXmlRichText = null;
    protected override void OnLoading()
    {
        base.OnLoading();
        m_btn_btn.gameObject.SetActive(false);
        m_UIXmlRichText = m_widget_RichText.GetComponent<UIXmlRichText>();
    }

    protected override void OnShow(object data)
    {
        base.OnShow(data);

        if (data is LangTalkData)
        {
            m_LangTalkData = data as LangTalkData;

            m_trans_BtnRoot.transform.DestroyChildren();

            m_trans_BtnRoot.transform.parent.GetComponent<UIScrollView>().ResetPosition();

            table.NpcDataBase npcdb = GameTableManager.Instance.GetTableItem<table.NpcDataBase>(m_LangTalkData.nNpcId);
            if (npcdb != null)
            {
                m_label_Npc_Name.text = npcdb.strName;
            }

            m_UIXmlRichText.Clear();
            if (m_LangTalkData.lstTalks.Count > 0)
            {
                m_UIXmlRichText.AddXml(RichXmlHelper.RichXmlAdapt(m_LangTalkData.lstTalks[0].strText));
            }

            for (int i = 0; i < m_LangTalkData.buttons.Count; i++)
            {
                GameObject go = NGUITools.AddChild(m_trans_BtnRoot.gameObject, m_btn_btn.gameObject);
                if (go != null)
                {
                    go.GetComponentInChildren<UILabel>().text = m_LangTalkData.buttons[i].strBtnName;
                    go.name = string.Format("btn_{0}",i);
                    UIEventListener.Get(go).onClick = onClick_Btn_Btn;
                    go.transform.localPosition = new UnityEngine.Vector3(0,-i * 58,0);
                    go.SetActive(true);
                }
            }
        }
    }

    void onClick_Close_Btn(GameObject caster)
    {
        this.HideSelf();
    }

    void onClick_Btn_Btn(GameObject caster)
    {
        int index = int.Parse(caster.name.Replace("btn_", ""));
        if (m_LangTalkData == null)
        {
            return;
        }

        if (index >= 0 && index < m_LangTalkData.buttons.Count)
        {
            NetService.Instance.Send(new GameCmd.stDialogSelectScriptUserCmd_C()
            {
                step = m_LangTalkData.strStep,
                dwChose = m_LangTalkData.buttons[index].nindex,
            });
        }
        this.HideSelf();
    }
}
