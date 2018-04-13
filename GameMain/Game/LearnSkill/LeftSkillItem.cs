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

public enum LeftLearnSkillItemState
{
    Lock,                       //未解锁
    LockSelect,                  //未解锁高亮
    OpenNotEqualCurState,       //解锁不符合当前形态
    OpenHasSetLeft,               //解锁符合当前形态已经设置 左面版
    OpenNotSetAndSelect,      //解锁符合当前形态没有设置并且选中
    OpenNotSetAndNotSelect,    //解锁符合当前形态没有设置并且没有选中
    OpenNotSetAndCanSet,      //解锁符合当前形态没有设置并且可以设置

}
class LeftSkillItem : MonoBehaviour
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
    LearnSkillDataManager dataManager
    {
        get
        {
            return DataManager.Manager<LearnSkillDataManager>();
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

    GameObject m_goRedPoint;
    UIParticleWidget particle;

    bool bInit = false;
    void Awake()
    {
        InitControls();
    }
    void InitControls()
    {
        if(!bInit)
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
                particle = iconTrans.Find("TeXiaoRoot").GetComponent<UIParticleWidget>();
                if (particle == null)
                {
                    particle = iconTrans.Find("TeXiaoRoot").gameObject.AddComponent<UIParticleWidget>();
                    particle.depth = 100;
                }
            }


            m_transLock = parent.Find("lock");
            m_transSet = parent.Find("setting");
            m_goRedPoint = parent.Find("redPoint").gameObject;
            bInit = true;
        }
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

    void SetRedPoint(bool b)
    {
        if (m_goRedPoint != null && m_goRedPoint.activeSelf != b)
        {
            m_goRedPoint.SetActive(b);
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
    CMResAsynSeedData<CMTexture> m_curIconAsynSeed = null;
    public void InitItem(SkillDatabase db)
    {
        InitControls();
        if (db == null)
        {
            return;
        }
        m_dataBase = db;
        if (m_sprIcon != null)
        {
        //    string iconName = UIManager.BuildCircleIconName(db.iconPath);
            UIManager.GetTextureAsyn(db.iconPath, ref m_curIconAsynSeed, () =>
           {
               if (null != m_sprIcon)
               {
                   m_sprIcon.mainTexture = null;
               }
           }, m_sprIcon);
        //   DataManager.Manager<UIManager>().SetSpriteDynamicIcon(m_sprIcon, db.iconPath, true);
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

        SetRedPoint(dataManager.IsSkillEnableUpgrade(db));
    }

    public void AddEffectInSkillPanel() 
    {
        if (particle != null)
        {
            if (particle != null)
            {

                particle.SetDimensions(1, 1);
                particle.ReleaseParticle();
                particle.AddParticle(50023);
            }         
        }
    }
    public void Release(bool depthRelease = true)
    {
        if (m_curIconAsynSeed != null)
        {
            m_curIconAsynSeed.Release(depthRelease);
            m_curIconAsynSeed = null;
        }
    }
}

