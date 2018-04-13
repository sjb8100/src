using System;
using System.Collections.Generic;
using UnityEngine;
/*
public class UIFunctionPushListGrid : UIGridBase
{
    private UILabel m_lableName = null;
    private UILabel m_lableUnlock = null;
    private UISprite m_spriteIcon = null;
    private GameObject m_goOpenTip = null;
    private GameObject m_goLockTip = null;

    table.Trailerdatabase m_Trailerdata = null;
    
    public table.Trailerdatabase Data
    {
        get { return m_Trailerdata; }
    }

    void Awake()
    {
        m_lableName = transform.Find("Name").GetComponent<UILabel>();
        m_lableUnlock = transform.Find("LockLevel").GetComponent<UILabel>();
        m_spriteIcon = transform.Find("Icon").GetComponent<UISprite>();
        m_goOpenTip = transform.Find("Open").gameObject;
        m_goLockTip = transform.Find("Lock").gameObject;
    }


    public void SetGridData(table.Trailerdatabase data)
    {
        m_Trailerdata = data;
        FunctionPushManager dataMgr = DataManager.Manager<FunctionPushManager>();

        bool isOpened = dataMgr.IsOpened(m_Trailerdata.dwId);
        bool canOpend = dataMgr.CanOpen(m_Trailerdata);
        if (m_goOpenTip != null)
        {
            if (isOpened)
            {
                m_goOpenTip.SetActive(false);
            }
            else 
            {
                m_goOpenTip.SetActive(canOpend);
            }
        }

        if (m_goLockTip != null)
        {
            m_goLockTip.SetActive(!isOpened && !canOpend);
        }

        if (m_lableName != null)
        {
            m_lableName.text = m_Trailerdata.strDesc;
        }

        if (m_lableUnlock != null)
        {
            if (m_Trailerdata.unlockType == 1)
            {
                m_lableUnlock.text = DataManager.Manager<TextManager>().GetLocalFormatText(LocalTextType.Local_TXT_FM_Trailer_Level,m_Trailerdata.param1);
            }else if (m_Trailerdata.unlockType == 2)
            {
                string[] strMission = m_Trailerdata.param2.Split(';');
                if (strMission.Length > 0)
                {
                    table.QuestDataBase quest = GameTableManager.Instance.GetTableItem<table.QuestDataBase>(uint.Parse(strMission[0]));
                    if (quest != null)
                    {
                        m_lableUnlock.text = DataManager.Manager<TextManager>().GetLocalFormatText(LocalTextType.Local_TXT_FM_Trailer_Task, quest.strName);
                    }
                }
                   
            }

            m_lableUnlock.color = isOpened || canOpend ? Color.green : Color.red;
        }

        if (m_spriteIcon != null)
        {
            m_spriteIcon.spriteName = m_Trailerdata.stricon;
            m_spriteIcon.color = isOpened || canOpend ? Color.white : Color.gray;
        }

    }

}
*/