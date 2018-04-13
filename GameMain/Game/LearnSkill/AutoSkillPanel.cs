using System.Collections.Generic;
using UnityEngine;
using table;
using Engine.Utility;

partial class AutoSkillPanel : UIPanelBase
{
    LearnSkillDataManager skilldataManager
    {
        get
        {
            return DataManager.Manager<LearnSkillDataManager>();
        }
    }
    protected override void OnLoading()
    {
        base.OnLoading();
        UIEventListener.Get(m_sprite_btn_Close.gameObject).onClick = OnClosePlanel;
    }
    protected override void OnShow(object data)
    {
        base.OnShow(data);
        List<SkillDatabase> commonSkillData = skilldataManager.GetCommonSkillDataBase();
        SetItemInfo(m_trans_currency, commonSkillData);

        List<SkillDatabase> stateOneData = skilldataManager.GetShowStateOneList();
        SetItemInfo(m_trans_state_1, stateOneData);
 
        List<SkillDatabase> stateTwoData = skilldataManager.GetShowStateTwoList();
        SetItemInfo(m_trans_state_2, stateTwoData);
    }


    void SetItemInfo(Transform parent,List<SkillDatabase> skillData)
    {
        for (int i = 0; i < skillData.Count; i++)
        {
            Transform trans = parent.Find((i + 1).ToString());
            if (trans != null)
            {
                Transform autoItem = trans.Find("autoitem");
                if (autoItem == null)
                {
                    GameObject go = NGUITools.AddChild(trans.gameObject, m_sprite_autoskillitem.gameObject);
                    go.name = "autoitem";
                    autoItem = go.transform;
                }
                autoItem.gameObject.SetActive(true);
                AutoSettingSkillItem setitem = autoItem.gameObject.GetComponent<AutoSettingSkillItem>();
                if (setitem == null)
                {
                    setitem = autoItem.gameObject.AddComponent<AutoSettingSkillItem>();
                }
                setitem.InitSkillInfo(skillData[i]);
            }
        }
    }
    void onClick_Btn_right_Btn(GameObject caster)
    {
        skilldataManager.SendAutoSkillSet();
    }

    void OnClosePlanel(GameObject go)
    {
        HideSelf();
    }
    protected override void OnHide()
    {
        base.OnHide();
        Release();
    }
    public override void Release(bool depthRelease = true)
    {
        base.Release(depthRelease);
        AutoSettingSkillItem[] skillItems = m_trans_currency.GetComponentsInChildren<AutoSettingSkillItem>();
        foreach(var item in skillItems)
        {
            item.ReleaseAutoSetting();
        }
        skillItems = m_trans_state_1.GetComponentsInChildren<AutoSettingSkillItem>();
        foreach (var item in skillItems)
        {
            item.ReleaseAutoSetting();
        }
        skillItems = m_trans_state_2.GetComponentsInChildren<AutoSettingSkillItem>();
        foreach (var item in skillItems)
        {
            item.ReleaseAutoSetting();
        }
    }
    protected override void OnPanelBaseDestory()
    {
        base.OnPanelBaseDestory();
    }
}
