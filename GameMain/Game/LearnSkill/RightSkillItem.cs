//*************************************************************************
//	创建日期:	2016/11/11 10:00:48
//	文件名称:	RightSkillItem
//   创 建 人:   zhudianyu	
//	版权所有:	中青宝
//	说    明:	学习技能右边技能item
//*************************************************************************

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using table;
using Client;
public enum RightLearnSkillItemState
{
    NotSet,
    Lock,
    SetRightSelect,        //右面板选中
    SetRightNotSelect,        //右面板未选中
    SetRightCanChange,     //右面 可切换

}
class RightSkillItem : MonoBehaviour
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
    RightLearnSkillItemState m_itemState;
    public RightLearnSkillItemState ItemState
    {
        get
        {
            return m_itemState;
        }
    }

    public LearnSkillDataManager LearnDataManager
    {
        get
        {
            return DataManager.Manager<LearnSkillDataManager>();
        }
    }
    Transform m_transSelect;

    Transform m_transCanSet;

    Transform m_transLock;
    UILabel m_unLockLevel;
    UITexture m_sprIcon;
    //技能位置
    uint m_nLoc;
    void Awake()
    {
        m_transCanSet = transform.Find("canset");

        m_transSelect = transform.Find("select");

        m_transLock = transform.Find("lock");
        if (m_transLock != null)
        {
            Transform levelTrans = m_transLock.Find("unlockLevel");
            if (levelTrans != null)
            {
                m_unLockLevel = levelTrans.GetComponent<UILabel>();
            }
        }
        Transform spr = transform.Find("Sprite");
        if (spr != null)
        {
            m_sprIcon = spr.GetComponent<UITexture>();
        }
        string[] nameLoc = name.Split('_');
        if (nameLoc.Length == 2)
        {
            int unloc = 0;
            if (int.TryParse(nameLoc[1], out unloc))
            {
                IPlayer mainPlayer = ClientGlobal.Instance().MainPlayer;
                int job = mainPlayer.GetProp((int)PlayerProp.Job);
                int level = mainPlayer.GetProp((int)CreatureProp.Level);
                List<SkillDatabase> list = DataManager.Manager<LearnSkillDataManager>().GetRoleSkillList();
                m_nLoc = (uint)unloc;
                for (int i = 0; i < list.Count; i++)
                {
                    var db = list[i];
                    uint unLockLevel = db.dwNeedLevel;
                    int loc = GameTableManager.Instance.GetGlobalConfig<int>("UnlockSkillLocation", unLockLevel.ToString());
                    if (loc == 0)
                    {
                        InitItem(null);
                        break;
                    }
                    if (loc != 0 && loc == unloc)
                    {
                        InitItem(db);
                        break;
                    }
                }
            }
        }

    }
    void OnEnable()
    {

    }
    public void InitItem(SkillDatabase db)
    {
        m_dataBase = db;


        ResetState();
    }
    public uint GetUnLockLevel()
    {
        SkillSettingState curState = DataManager.Manager<LearnSkillDataManager>().ShowState;
        uint loc = curState == SkillSettingState.StateOne ? m_nLoc : m_nLoc + 4;
        if (curState == SkillSettingState.StateOne)
        {
            uint level = LearnDataManager.GetUnLockLevelByLoc(m_nLoc);
            return level;
        }
        else
        {
            return 0;
        }
    }
    public void ResetState()
    {
        if (m_nLoc == 0)
        {
            if (m_dataBase != null)
            {
                //DataManager.Manager<UIManager>().SetSpriteDynamicIcon(m_sprIcon, m_dataBase.iconPath, true, true);
                string iconName = UIManager.BuildCircleIconName(m_dataBase.iconPath);
                UIManager.GetTextureAsyn(iconName, ref m_curIconAsynSeed, () =>
                {
                    if (null != m_sprIcon)
                    {
                        m_sprIcon.mainTexture = null;
                    }
                }, m_sprIcon);
            }
            return;
        }
        if (m_dataBase == null)
        {
            SetItemState(RightLearnSkillItemState.NotSet);
            return;
        }
        else
        {
            SkillSettingState curState = DataManager.Manager<LearnSkillDataManager>().ShowState;
            uint skillID = 0;
            uint loc = curState == SkillSettingState.StateOne ? m_nLoc : m_nLoc + 4;
            if (DataManager.Manager<LearnSkillDataManager>().TryGetLocationSkillId(curState, (int)loc, out skillID))
            {
                m_dataBase = GameTableManager.Instance.GetTableItem<SkillDatabase>(skillID, 1);
                SetItemState(RightLearnSkillItemState.SetRightNotSelect);
                ShowSkillIcon(true);
            }
            else
            {
                if (curState == SkillSettingState.StateOne)
                {
                    uint level = LearnDataManager.GetUnLockLevelByLoc(m_nLoc);
                    if (level > MainPlayerHelper.GetPlayerLevel())
                    {
                        SetItemState(RightLearnSkillItemState.Lock);
                    }
                    else
                    {
                        SetItemState(RightLearnSkillItemState.NotSet);
                    }
                }
                else
                {
                    SetItemState(RightLearnSkillItemState.NotSet);
                }
                ShowSkillIcon(false);
            }
        }
    }
    public void SetItemState(RightLearnSkillItemState st)
    {
        if (st == RightLearnSkillItemState.NotSet)
        {
            if (m_transCanSet != null)
            {
                m_transCanSet.gameObject.SetActive(false);
            }
            if (m_transSelect != null)
            {
                m_transSelect.gameObject.SetActive(false);
            }
            ShowSkillIcon(false);
            if (m_transLock != null)
            {
                m_transLock.gameObject.SetActive(false);
            }
        }
        else if (st == RightLearnSkillItemState.Lock)
        {
            if (m_transCanSet != null)
            {
                m_transCanSet.gameObject.SetActive(false);
            }
            if (m_transSelect != null)
            {
                m_transSelect.gameObject.SetActive(false);
            }
            ShowSkillIcon(false);
            if (m_transLock != null)
            {
                m_transLock.gameObject.SetActive(true);
                if (m_dataBase != null)
                {
                    SkillDatabase db = GameTableManager.Instance.GetTableItem<SkillDatabase>(m_dataBase.wdID, 1);
                    if (db != null)
                    {
                        if (m_unLockLevel != null)
                        {
                            m_unLockLevel.text = db.dwNeedLevel.ToString();
                        }
                    }
                }
            }
        }
        else if (st == RightLearnSkillItemState.SetRightCanChange)
        {
            if (m_transCanSet != null)
            {
                m_transCanSet.gameObject.SetActive(true);
            }
            if (m_transSelect != null)
            {
                m_transSelect.gameObject.SetActive(false);
            }
            ShowSkillIcon(true);
            if (m_transLock != null)
            {
                m_transLock.gameObject.SetActive(false);
            }
        }
        else if (st == RightLearnSkillItemState.SetRightNotSelect)
        {
            if (m_transLock != null)
            {
                m_transLock.gameObject.SetActive(false);
            }
            if (m_transCanSet != null)
            {
                m_transCanSet.gameObject.SetActive(false);
            }
            if (m_transSelect != null)
            {
                m_transSelect.gameObject.SetActive(false);
            }
            ShowSkillIcon(true);
        }
        else if (st == RightLearnSkillItemState.SetRightSelect)
        {
            if (m_transLock != null)
            {
                m_transLock.gameObject.SetActive(false);
            }
            if (m_transCanSet != null)
            {
                m_transCanSet.gameObject.SetActive(false);
            }
            SetHighLight();
            ShowSkillIcon(true);
        }
    }
    void SetHighLight()
    {
        if (m_transSelect != null)
        {
            Transform parent = transform.parent;
            int count = parent.childCount;
            for (int i = 0; i < count; i++)
            {
                string subname = "skill_" + i;
                Transform child = parent.Find(subname);
                if (child != null)
                {
                    Transform slectTrans = child.Find("select");
                    if (slectTrans != null)
                    {
                        slectTrans.gameObject.SetActive(false);
                    }
                }
            }
            m_transSelect.gameObject.SetActive(true);
        }
    }
    void ShowSkillIcon(bool bShow)
    {
        if (m_sprIcon != null)
        {
            if (m_nLoc != 0)
            {
                m_sprIcon.gameObject.SetActive(bShow);
            }
            else
            {
                if (bShow)
                {

                    m_sprIcon.gameObject.SetActive(bShow);
                }
            }
        }
        SkillSettingState curState = DataManager.Manager<LearnSkillDataManager>().ShowState;
        uint skillID = 0;
        uint loc = curState == SkillSettingState.StateOne ? m_nLoc : m_nLoc + 4;
        if (DataManager.Manager<LearnSkillDataManager>().TryGetLocationSkillId(curState, (int)loc, out skillID))
        {
            m_dataBase = GameTableManager.Instance.GetTableItem<SkillDatabase>(skillID, 1);
            string iconName = UIManager.BuildCircleIconName(m_dataBase.iconPath);
            UIManager.GetTextureAsyn(iconName, ref m_curIconAsynSeed, () =>
            {
                if (null != m_sprIcon)
                {
                    m_sprIcon.mainTexture = null;
                }
            }, m_sprIcon);
        }
        else
        {
            if (m_nLoc != 0)
            {
                if (null != m_sprIcon)
                //m_sprIcon.spriteName = "";
                    m_sprIcon.mainTexture = null;
            }
        }
    }
    CMResAsynSeedData<CMTexture> m_curIconAsynSeed = null;
    public void Release(bool depthRelease = true)
    {
        if (m_curIconAsynSeed != null)
        {
            m_curIconAsynSeed.Release(depthRelease);
            m_curIconAsynSeed = null;
        }
    }
}

