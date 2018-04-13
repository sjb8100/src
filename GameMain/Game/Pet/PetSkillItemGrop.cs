using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using table;
using Client;
using GameCmd;
using Vector2 = UnityEngine.Vector2;
public enum PetSkillGroupType
{
    Common,
    Lock,
    Shuxing,
    MainSkill,
}
class PetSkillItemGrop : MonoBehaviour
{
    PetDataManager petDataManager
    {
        get
        {
            return DataManager.Manager<PetDataManager>();
        }
    }
    IPet curPet;
    public IPet CurPet
    {
        get
        {
            return petDataManager.CurPet;
        }
    }
    Dictionary<string, bool> lockSkillDic = new Dictionary<string, bool>();
    PetSkillGroupType m_type = PetSkillGroupType.Shuxing;
    public PetSkillGroupType SkillGropType
    {
        set
        {
            m_type = value;
        }
    }

    void Awake()
    {

    }
    void Start()
    {

    }
    void OnEnable()
    {
        petDataManager.ValueUpdateEvent += OnValueUpdateEventArgs;

        InitItem();
        SetFirstHigh();
    }
    void OnDisable()
    {
        petDataManager.ValueUpdateEvent -= OnValueUpdateEventArgs;
    }
    void OnDestory()
    {

    }
    void OnValueUpdateEventArgs(object obj, ValueUpdateEventArgs v)
    {
        if (v != null)
        {
            if (v.key == PetDispatchEventString.ChangePet.ToString())
            {
                ResetData();
                InitItem();
                SetFirstHigh();
            }
            else if (v.key == PetDispatchEventString.PetSkillInit.ToString())
            {
                InitItem();
            }
            else if (v.key == PetDispatchEventString.LockSkill.ToString())
            {
                stLockSkillPetUserCmd_CS cmd = (stLockSkillPetUserCmd_CS)v.newValue;
                if (cmd != null)
                {
                    RefreshLockInfo(cmd);
                }
            }
            else if(v.key == PetDispatchEventString.PetRefreshProp.ToString())
            {
                InitItem();
            }
        }

    }
    void RefreshLockInfo(stLockSkillPetUserCmd_CS cmd)
    {
        int skillID = cmd.skill;
        for (int i = 0; i < 6; i++)
        {
            string itemName = "petskill" + (i + 1).ToString();
            Transform go = transform.Find(itemName);
            if (go != null)
            {
                PetSkillItem skill = go.GetComponent<PetSkillItem>();
                if (skill != null)
                {
                    if (skill.SkillData.wdID == skillID)
                    {
                        Transform trans = go.transform.Find("gouxuan");
                        if (trans != null)
                        {
                            SetLockDic(itemName, cmd.@lock);
                            trans.gameObject.SetActive(cmd.@lock);

                        }
                        uint n = 0;
                        var iter = lockSkillDic.GetEnumerator();
                        while(iter.MoveNext())
                        {
                            var dic = iter.Current;
                            if (dic.Value)
                            {
                                n += 1;
                            }
                        }
                        petDataManager.LockSkillNum = n;
                    }
                }
            }
        }
    }
    void ResetData()
    {
        // petDataManager.LockSkillNum = 0;
        lockSkillDic.Clear();
        InitSkillIcon();
        uint num = 0;
        for (int i = 0; i < 6; i++)
        {
            string itemName = "petskill" + (i + 1).ToString();
            Transform item = transform.Find(itemName);
            if (item != null)
            {
                UIEventListener.Get(item.gameObject).onClick = OnSkillItemClick;
                if (CurPet != null)
                {
                    List<PetSkillObj> list = CurPet.GetPetSkillList();
                    if (i < list.Count)
                    {
                        var skill = list[i];
                        SetLockDic(itemName, skill.@lock);
                        if (skill.@lock)
                        {
                            num++;
                        }
                    }
                    else
                    {
                        SetLockDic(itemName, false);
                    }

                }
                else
                {
                    SetLockDic(itemName, false);
                }

                //PetSkillItem skillItem = item.GetComponent<PetSkillItem>();
                //if ( skillItem != null )
                //{
                //    Destroy( skillItem );
                //}
            }
        }
        petDataManager.LockSkillNum = num;
    }
    void SetLockDic(string itemName, bool bLock)
    {
        if (lockSkillDic.ContainsKey(itemName))
        {
            lockSkillDic[itemName] = bLock;
        }
        else
        {
            lockSkillDic.Add(itemName, bLock);
        }
    }
    void SetLockSetting()
    {
        var iter = lockSkillDic.GetEnumerator();
        while(iter.MoveNext())
        {
            var skill = iter.Current;
            Transform skillitem = transform.Find(skill.Key);
            Transform lockSpr = skillitem.Find("lock");
            Transform gouxuan = skillitem.Find("gouxuan");
            Transform ditu = skillitem.Find("ditu");
            Transform icon = skillitem.Find("icon");
            if (skillitem != null)
            {
                if (petDataManager.bLockSkill)
                {
                    if (lockSpr != null)
                    {
                        lockSpr.gameObject.SetActive(false);
                    }
                    if (ditu != null)
                    {
                        if (icon.gameObject.activeSelf)
                        {
                            ditu.gameObject.SetActive(true);
                        }
                    }
                    if (gouxuan != null)
                    {
                        gouxuan.gameObject.SetActive(skill.Value);
                    }
                }
                else
                {
                    if (lockSpr != null)
                    {
                        bool isLock = skill.Value;

                        lockSpr.gameObject.SetActive(isLock);
                    }
                    if (ditu != null)
                    {
                        ditu.gameObject.SetActive(false);
                    }
                    if (gouxuan != null)
                    {
                        gouxuan.gameObject.SetActive(false);
                    }
                }

            }

        }
    }

    private CMResAsynSeedData<CMTexture> m_iconCASD = null;
    void ShowHasSkill(Transform item,GameCmd.PetSkillObj skill)
    {
        Transform addskill = item.Find("addskill");
        if (addskill != null)
        {
            addskill.gameObject.SetActive(false);
        }
        SkillDatabase db = GameTableManager.Instance.GetTableItem<SkillDatabase>((uint)skill.id, 1);
        if (db != null)
        {
            Transform skillLv = item.Find("lv");
            if(skillLv != null)
            {
                UILabel lvLabel = skillLv.GetComponent<UILabel>();
                if(lvLabel != null)
                {
                    lvLabel.text = skill.lv.ToString();
                }
                skillLv.gameObject.SetActive(true);
            }
            PetSkillItem skillitem = item.gameObject.GetComponent<PetSkillItem>();
            if (skillitem == null)
            {
                skillitem = item.gameObject.AddComponent<PetSkillItem>();
            }
            skillitem.SkillData = db;
            skillitem.SetIcon(db);
            //Transform icon = item.Find("icon");
            //if (icon != null)
            //{
            //    icon.gameObject.SetActive(true);
            //    UITexture spr = icon.GetComponent<UITexture>();
            //    if (spr != null)
            //    {
            //        UIManager.GetTextureAsyn(db.iconPath, ref m_iconCASD, () =>
            //        {
            //            if (null != spr)
            //            {
            //                spr.mainTexture = null;
            //            }
            //        }, spr);
					
            //    }
            //    ShowTips(db, item, new Vector2(spr.width, spr.height));
            //}
            Transform nameTrans = item.Find("Label");
            if (nameTrans == null)
            {
                nameTrans = item.Find("skillname");
            }
            if (nameTrans != null)
            {
                UILabel nameLabel = nameTrans.GetComponent<UILabel>();
                if (nameLabel != null)
                {
                    nameLabel.text = db.strName;
                }
            }
            Transform sprTrans = item.Find("bg_choose");
            if (sprTrans != null)
            {
                if (petDataManager.SelectSkillDataBase != null)
                {
                    if (petDataManager.SelectSkillDataBase.wdID == skill.id)
                    {
                        sprTrans.gameObject.SetActive(true);
                    }
                    else
                    {
                        sprTrans.gameObject.SetActive(false);
                    }
                }
            }
        }
    }
    void ShowNoSkill(Transform item)
    {
        Transform addskill = item.Find("addskill");
        if (addskill != null)
        {
            addskill.gameObject.SetActive(true);
        }
        Transform sprTrans = item.Find("bg_choose");
        if (sprTrans != null)
        {
            sprTrans.gameObject.SetActive(false);
        }
        PetSkillItem ps = item.GetComponent<PetSkillItem>();
        if(ps != null)
        {
            ps.SetIcon(null);
        }
      
        Transform skillLv = item.Find("lv");
        if (skillLv != null)
        {
            skillLv.gameObject.SetActive(false);
        }
        Transform nameTrans = item.Find("Label");
        if (nameTrans == null)
        {
            nameTrans = item.Find("skillname");
        }
        if (nameTrans != null)
        {
            UILabel nameLabel = nameTrans.GetComponent<UILabel>();
            if (nameLabel != null)
            {
                nameLabel.text = CommonData.GetLocalString("技能");
            }
        }
    }
    void InitSkillIcon()
    {
        if (CurPet == null)
        {
            return;
        }
        List<PetSkillObj> list = CurPet.GetPetSkillList();
        for (int i = 0; i < list.Count; i++)
        {
            var skill = list[i];
            string itemName = "petskill" + (i + 1).ToString();
            Transform item = transform.Find(itemName);

            if (item != null)
            {
                ShowHasSkill(item, skill);
            }
        }

        for (int i = list.Count; i < 6; i++)
        {

            string itemName = "petskill" + (i + 1).ToString();
            Transform item = transform.Find(itemName);

            if (item != null)
            {
                ShowNoSkill(item);
            }
        }

    }
    void SetFirstHigh()
    {
        if (CurPet == null)
        {
            return;
        }

        List<PetSkillObj> list = CurPet.GetPetSkillList();
        if (list.Count != 0)
        {
            SetItemHight(0);
            if (m_type == PetSkillGroupType.MainSkill)
            {
                string itemName = "petskill1";
                Transform item = transform.Find(itemName);

                if (item != null)
                {
                    SetDataBase(1, item.gameObject);
                }
            }
        }
       
    }
    void SetItemHight(int index)
    {
        for (int i = 1; i < 7; i++)
        {
            string itemName = "petskill" + (i).ToString();
            Transform item = transform.Find(itemName);

            if (item != null)
            {
                Transform sprTrans = item.Find("bg_choose");
                if (sprTrans != null)
                {
                    if (i == index)
                    {
                        sprTrans.gameObject.SetActive(true);
                    }
                    else
                    {
                        sprTrans.gameObject.SetActive(false);
                    }
                }
            }
        }
    }
    void ShowTips(SkillDatabase db, Transform item, Vector2 offset)
    {
        LongPress lp = item.GetComponent<LongPress>();
        if (lp == null)
        {
            lp = item.gameObject.AddComponent<LongPress>();
        }
        lp.InitLongPress(() =>
        {
            DataManager.Manager<UIPanelManager>().ShowPanel(PanelID.PlayerSkillTipsPanel, panelShowAction: (pb) =>
            {
                if (null != pb && pb is PlayerSkillTipsPanel)
                {
                    PlayerSkillTipsPanel panel = pb as PlayerSkillTipsPanel;
                    if (panel != null)
                    {
                        panel.InitParentTransform(item, offset);
                        panel.ShowUI(PlayerSkillTipsPanel.SkillTipsType.Pet, db);
                    }
                }
            });
        },
        () =>
        {
            DataManager.Manager<UIPanelManager>().HidePanel(PanelID.PlayerSkillTipsPanel);
        });
    }
    void InitItem()
    {

        ResetData();
        SetLockSetting();
        InitSkillIcon();

    }
    //  bool IsLock = false;
    void OnSkillItemClick(GameObject go)
    {
        string name = go.name;
        char indexStr = name[name.Length - 1];
        int index = 0;

        if (m_type == PetSkillGroupType.Shuxing)
        {
            //if (CurPet == null)
            //{

            //    return;
            //}
            //if (int.TryParse(indexStr.ToString(), out index))
            //{
            //    List<PetSkillObj> list = CurPet.GetPetSkillList();
            //    if (index <= list.Count)
            //    {
            //        UIFrameManager.Instance.OnCilckTogglePanel(PanelID.PetPanel, 1, (int)PetPanel.TabMode.ShuXing);
            //    }
            //    else
            //    {
            //        DataManager.Manager<UIPanelManager>().ShowPanel(PanelID.PetLearnSkill);
            //    }
            //    SetDataBase(index, go);
            //}

            return;
        }
        if (m_type == PetSkillGroupType.Lock)
        {
            ExecuLockGrop(go);
        }
        else
        {
            if (int.TryParse(indexStr.ToString(), out index))
            {
                SetDataBase(index, go);
            }
        }

    }
    void SetDataBase(int index, GameObject go)
    {
        PetSkillItem skillitem = go.GetComponent<PetSkillItem>();
        if (skillitem != null)
        {
            petDataManager.SelectSkillDataBase = skillitem.SkillData;

            SetItemHight(index);

        }
        else
        {
            SetItemHight(0);
            petDataManager.SelectSkillDataBase = null;
        }
    }
    void ExecuLockGrop(GameObject go)
    {
        if (!petDataManager.bLockSkill)
        {
            return;
        }
        bool haLock = false;
        if (lockSkillDic.ContainsKey(go.name))
        {
            haLock = lockSkillDic[go.name];

        }
        if (!haLock)
        {
            int limitLock = GameTableManager.Instance.GetGlobalConfig<int>("PetSkillLockLimit");
            List<PetSkillObj> list = CurPet.GetPetSkillList();
            int hasLock = (int)petDataManager.LockSkillNum;
            int maxLock = list.Count - limitLock;
            if (list.Count - limitLock <= hasLock)
            {
                TipsManager.Instance.ShowLocalFormatTips(LocalTextType.Pet_Skill_zuiduosuodingXgejineng, maxLock);
                //  TipsManager.Instance.ShowTips( "最多锁定" + maxLock + "个技能" );
                return;
            }
        }
        if (m_type == PetSkillGroupType.Lock)
        {
            PetSkillItem skillitem = go.GetComponent<PetSkillItem>();
            if (skillitem != null && skillitem.SkillData != null)
            {
                haLock = !haLock;
                petDataManager.SelectSkillDataBase = skillitem.SkillData;
                SendSkillLock(skillitem.SkillData.wdID, haLock);

            }

        }

    }
    void SendSkillLock(uint skillID, bool block)
    {
        if (CurPet != null)
        {
            stLockSkillPetUserCmd_CS cmd = new stLockSkillPetUserCmd_CS();
            cmd.id = CurPet.GetID();
            cmd.skill = (int)skillID;
            cmd.@lock = block;

            NetService.Instance.Send(cmd);
        }
    }

    public void Release()
    {
        PetSkillItem[] items = GetComponentsInParent<PetSkillItem>();
        foreach(var item in items)
        {
            item.Release();
        }

        if (null != m_iconCASD)
        {
            m_iconCASD.Release();
            m_iconCASD = null;
        }
    }
}

