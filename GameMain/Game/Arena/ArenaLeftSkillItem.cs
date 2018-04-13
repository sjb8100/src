//*************************************************************************
//	创建日期:	2016/11/11 9:59:34
//	文件名称:	LeftSkillItem
//   创 建 人:   zhudianyu	
//	版权所有:	中青宝
//	说    明:	学习技能左边技能item
//*************************************************************************

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using table;
using Client;


class ArenaLeftSkillItem : MonoBehaviour
{
    SkillDatabase m_dataBase;
    public SkillDatabase ItemDataBase
    {
        get
        {
            if (m_dataBase == null)
            {
                Engine.Utility.Log.Error("LearnSkillItem  database is null");
                return null;
            }
            return m_dataBase;
        }
    }
    LeftLearnSkillItemState m_itemState;
    public LeftLearnSkillItemState ItemState
    {
        get
        {
            return m_itemState;
        }
    }
    ArenaSetSkillManager dataManager
    {
        get
        {
            return DataManager.Manager<ArenaSetSkillManager>();
        }
    }
    UILabel m_labelLevel;

    UILabel m_labelName;

    UISprite m_sprSelect;

    UISprite m_sprMask;

    Transform m_transChange;

    UITexture m_sprIcon;

    Transform m_transLock;

    Transform m_transSet;

    private CMResAsynSeedData<CMTexture> iuiIconAtlas = null;

    void Awake()
    {
        Transform parent = transform.parent;
        if (parent == null)
            return;
        Transform levelTrans = parent.Find("level");
        if (levelTrans != null)
        {
            m_labelLevel = levelTrans.GetComponent<UILabel>();
        }

        Transform nameTrans = parent.Find("name");
        if (nameTrans != null)
        {
            m_labelName = nameTrans.GetComponent<UILabel>();
        }

        GameObject go = parent.Find("choose").gameObject;
        if (go != null)
        {
            m_sprSelect = parent.Find("choose").GetComponent<UISprite>();
            m_sprSelect.gameObject.SetActive(false);
        }

        Transform maskTrans = parent.Find("mask");

        if (maskTrans != null)
        {
            m_sprMask = maskTrans.GetComponent<UISprite>();
            m_sprMask.gameObject.SetActive(true);
        }

        m_transChange = parent.Find("canset");

        Transform iconTrans = parent.Find("select");
        if (iconTrans != null)
        {
            m_sprIcon = iconTrans.GetComponent<UITexture>();
        }

        m_transLock = parent.Find("lock");
        m_transSet = parent.Find("setting");
    }
    void Start()
    {

    }
    void ShowMask(bool bHigh = false)
    {
        if (dataManager.IsSettingPanel)
        {
            m_sprMask.gameObject.SetActive(true);
        }
        else
        {
            m_sprMask.gameObject.SetActive(false);

        }
        m_sprSelect.gameObject.SetActive(bHigh);
        m_transChange.gameObject.SetActive(false);
    }
    void ShowLock(bool bShow)
    {
        if (m_transLock != null)
        {
            m_transLock.gameObject.SetActive(bShow);
        }
    }
    void ShowSetting(bool bShow)
    {
        if (m_transSet != null)
        {
            m_transSet.gameObject.SetActive(bShow);
        }
    }
    public void SetItemState(LeftLearnSkillItemState st)
    {
        m_itemState = st;
        if (m_itemState == LeftLearnSkillItemState.Lock)
        {
            ShowMask();
            ShowLock(true);
            ShowSetting(false);
        }
        else if (m_itemState == LeftLearnSkillItemState.LockSelect)
        {
            ShowMask(true);
            ShowLock(true);
            ShowSetting(false);
        }
        else if (m_itemState == LeftLearnSkillItemState.OpenHasSetLeft)
        {
            ShowMask();
            ShowLock(false);
            ShowSetting(true);
        }
        else if (m_itemState == LeftLearnSkillItemState.OpenNotEqualCurState)
        {
            ShowMask();
            ShowLock(false);
            ShowSetting(false);
        }
        else if (m_itemState == LeftLearnSkillItemState.OpenNotSetAndCanSet)
        {
            m_sprMask.gameObject.SetActive(false);
            m_sprSelect.gameObject.SetActive(false);
            m_transChange.gameObject.SetActive(true);
            ShowLock(false);
            ShowSetting(false);
        }
        else if (m_itemState == LeftLearnSkillItemState.OpenNotSetAndNotSelect)
        {
            m_sprMask.gameObject.SetActive(false);
            m_sprSelect.gameObject.SetActive(false);
            m_transChange.gameObject.SetActive(false);
            ShowLock(false);
            ShowSetting(false);
        }
        else if (m_itemState == LeftLearnSkillItemState.OpenNotSetAndSelect)
        {
            m_sprMask.gameObject.SetActive(false);
            m_sprSelect.gameObject.SetActive(true);
            m_transChange.gameObject.SetActive(false);
            ShowLock(false);
            ShowSetting(false);
        }
    }
    public void ResetState()
    {
        if (m_dataBase == null)
            return;
        int level = dataManager.GetPlayerLevel();
        SkillDatabase sdb = GameTableManager.Instance.GetTableItem<SkillDatabase>(m_dataBase.wdID, 1);
        if (level < sdb.dwNeedLevel)
        {
            SetItemState(LeftLearnSkillItemState.Lock);
        }
        else
        {
            bool isCurShowState = false;
            if (dataManager.ShowState == SkillSettingState.StateOne)
            {
                if (m_dataBase.dwSkillSatus < 10)
                {
                    isCurShowState = true;
                }
            }
            if (dataManager.ShowState == SkillSettingState.StateTwo)
            {
                if (m_dataBase.dwSkillSatus > 10)
                {
                    isCurShowState = true;
                }
            }
            if (m_dataBase.dwSkillSatus == 0 || isCurShowState)
            {//已经解锁 普通技能 或者和当前状态相等
                int loc = 0;
                if (dataManager.IsSkillSave(m_dataBase.wdID, ref loc))
                {
                    SetItemState(LeftLearnSkillItemState.OpenHasSetLeft);
                }
                else
                {
                    SetItemState(LeftLearnSkillItemState.OpenNotSetAndNotSelect);
                }
            }
            else
            {
                SetItemState(LeftLearnSkillItemState.OpenNotEqualCurState);
            }

        }
    }
    public void InitItem(SkillDatabase db)
    {
        if (db == null)
        {
            return;
        }
        m_dataBase = db;
        if (m_sprIcon != null)
        {
            UIManager.GetTextureAsyn(db.iconPath, ref iuiIconAtlas, () =>
            {
                if (m_sprIcon != null)
                {
                    m_sprIcon.mainTexture = null;
                }
            }, m_sprIcon, true);

        }
        if (m_labelLevel != null)
        {
            if (db.wdLevel == 0)
            {
                m_labelLevel.text = "";
            }
            else
            {
                m_labelLevel.text = db.wdLevel.ToString();
            }
        }
        if (m_labelName != null)
        {
            m_labelName.text = db.strName;
        }
        if (dataManager.IsSettingPanel)
        {
            ResetState();
        }

    }

    public void Release()
    {
        if (null != iuiIconAtlas)
        {
            iuiIconAtlas.Release(true);
            iuiIconAtlas = null;
        }
    }
}

