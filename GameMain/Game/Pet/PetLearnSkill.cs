using System.Collections.Generic;
using UnityEngine;
using Client;
using table;
using GameCmd;
using Engine.Utility;
using System.Collections;

public enum PetSkillType
{
    [System.ComponentModel.Description("主动")]
    Initiative = 1,
    [System.ComponentModel.Description("自动")]
    Auto,
    [System.ComponentModel.Description("被动")]
    Passive,
    [System.ComponentModel.Description("推荐")]
    Recommend,
}
partial class PetLearnSkill
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

    public uint PetBaseID
    {
        get
        {
            if (CurPet != null)
            {
                return CurPet.PetBaseID;
            }
            Log.Error("get petbaseid error");
            return 0;
        }
    }
    PetSkillType skillType;
    public PetSkillType CurType
    {
        get
        {
            return skillType;
        }
        set
        {
            skillType = value;
            ShowLeftBtnHigh(skillType);
            InitScrollView();
        }
    }
    SkillDatabase skilldata;
    public SkillDatabase CurSkillDataBase
    {
        get
        {
            return skilldata;
        }
        set
        {
            skilldata = value;
            FilterSkill();
            InitLabel();
        }
    }
    uint lockSkillItem = 0;
    bool bAutoFull = false;

    Vector3 skillScrollPos = Vector3.zero;
    PetSkillItemGrop skillGrop = null;
    Dictionary<PetSkillType, Transform> m_leftBtnDic = new Dictionary<PetSkillType, Transform>();
    protected override void OnAwake()
    {
        base.OnAwake();
    }
    protected override void OnPrepareShow(object data)
    {
        base.OnPrepareShow(data);
        UIEventListener.Get(m_trans_tuijian.gameObject).onClick = onClick_Tuijian_Btn;
        UIEventListener.Get(m_trans_zhudong.gameObject).onClick = onClick_Zhudong_Btn;
        UIEventListener.Get(m_trans_shoudong.gameObject).onClick = onClick_Shoudong_Btn;
        UIEventListener.Get(m_trans_beidong.gameObject).onClick = onClick_Beidong_Btn;

    }
    protected override void OnShow(object data)
    {
        base.OnShow(data);
        RegisterGlobalUIEvent(true);

        skillGrop = m_widget_SkillbtnContainer.gameObject.GetComponent<PetSkillItemGrop>();
        if (skillGrop == null)
        {
            skillGrop = m_widget_SkillbtnContainer.gameObject.AddComponent<PetSkillItemGrop>();

        }
        skillGrop.SkillGropType = PetSkillGroupType.Lock;

        if (CurPet != null)
        {
            m_label_petshowname.text = petDataManager.GetCurPetName();
            m_label_petlevel.text = petDataManager.GetCurPetLevelStr();
        }

        skillScrollPos = m_ctor_skill_scrollview.CacheTransform.localPosition;

        SetLockSkillNum(petDataManager.LockSkillNum);
      
        AddLeftBtnDic(PetSkillType.Auto, m_trans_shoudong);
        AddLeftBtnDic(PetSkillType.Initiative, m_trans_zhudong);
        AddLeftBtnDic(PetSkillType.Passive, m_trans_beidong);
        AddLeftBtnDic(PetSkillType.Recommend, m_trans_tuijian);
        CurType = PetSkillType.Recommend;
        SetPetItemHighLight();
        SetSuoDingText(LocalTextType.Local_Txt_Pet_yisuodingjineng);
        if(petDataManager.CurPet != null)
        {
            uint baseID = petDataManager.CurPet.PetBaseID;
            PetDataBase pdb = GameTableManager.Instance.GetTableItem<PetDataBase>(baseID);
            if(pdb != null)
            {
              //  DataManager.Manager<UIManager>().SetSpriteDynamicIcon(m_sprite_peticon, pdb.icon);
                UIManager.GetTextureAsyn(pdb.icon, ref m_curIconAsynSeed, () =>
                {
                    if (null != m__peticon)
                    {
                        m__peticon.mainTexture = null;
                    }
                }, m__peticon, false);
            }
        }
    }
    CMResAsynSeedData<CMTexture> m_curIconAsynSeed = null;
    void AddLeftBtnDic(PetSkillType type, Transform btn)
    {
        if (!m_leftBtnDic.ContainsKey(type))
        {
            if (btn.GetComponent<UITabGrid>() == null)
            {
                btn.gameObject.AddComponent<UITabGrid>();
            }
            m_leftBtnDic.Add(type, btn);
        }
        else
        {
            m_leftBtnDic[type] = btn;
        }
    }

    protected override void OnHide()
    {
        base.OnHide();
        RegisterGlobalUIEvent(false);
        Release();
    }
    void RegisterGlobalUIEvent(bool register = true)
    {
        if (register)
        {
            petDataManager.ValueUpdateEvent += OnValueUpdateEventArgs;
            Engine.Utility.EventEngine.Instance().AddEventListener((int)Client.GameEventID.UIEVENT_UPDATEITEM, OnGlobalUIEventHandler);
        }
        else
        {
            petDataManager.ValueUpdateEvent -= OnValueUpdateEventArgs;
            Engine.Utility.EventEngine.Instance().RemoveEventListener((int)Client.GameEventID.UIEVENT_UPDATEITEM, OnGlobalUIEventHandler);
        }
    }
    private void OnGlobalUIEventHandler(int eventType, object data)
    {
        switch (eventType)
        {
            case (int)Client.GameEventID.UIEVENT_UPDATEITEM:
                OnUpdateItem((ItemDefine.UpdateItemPassData)data);
                break;
        }
    }
    void OnUpdateItem(ItemDefine.UpdateItemPassData passData)
    {
        if (null == passData)
        {
            return;
        }
        InitLabel();
    }
    protected override void OnInitPanelData()
    {
        base.OnInitPanelData();
    }
    protected override void OnLoading()
    {
        base.OnLoading();

    }
    void FilterSkill()
    {
        if (CurPet != null)
        {
            List<PetSkillObj> list = CurPet.GetPetSkillList();
            foreach (var skill in list)
            {
                if (skill.id == CurSkillDataBase.wdID)
                {
                    skilldata = GameTableManager.Instance.GetTableItem<SkillDatabase>((uint)skill.id, (int)skill.lv);
                }
            }
        }
    }
    void OnUpdateUIGrid(UIGridBase grid, int index)
    {
        if (grid is PetSkillItem)
        {
            if (index < m_typeList.Count)
            {
                SkillDatabase db = m_typeList[index];
                if (db != null)
                {
                    PetSkillItem item = grid as PetSkillItem;
                    item.gameObject.SetActive(true);
                    item.SetIcon(db);
                }
            }
        }

    }
    private void OnUIGridEventDlg(UIEventType eventType, object data, object param)
    {
        if (null == data)
        {
            return;
        }
        switch (eventType)
        {
            case UIEventType.Click:
                {
                    PetSkillItem item = data as PetSkillItem;
                    if (item != null)
                    {
                        OnSKillClick(item.gameObject);
                    }
                }
                break;
            default:
                break;
        }
    }
    List<SkillDatabase> m_typeList = new List<SkillDatabase>();
    private bool m_bInitSkillCreator = false;
    void InitScrollView()
    {
        if (CurPet == null)
        {
            return;
        }
        m_btn_skill_item.gameObject.SetActive(false);
        List<SkillDatabase> upList = GameTableManager.Instance.GetTableList<SkillDatabase>();
        List<SkillDatabase> typeList = upList.FindAll(x => { return (x.petType == (int)CurType && x.wdLevel == 1); });
        if (CurType == PetSkillType.Recommend)
        {
            typeList.Clear();
            PetDataBase pdb = GameTableManager.Instance.GetTableItem<PetDataBase>(petDataManager.CurPet.PetBaseID);
            if (pdb == null)
            {
                Log.Error("db is null");
                return;
            }
            int character = CurPet.GetProp((int)PetProp.Character);
            PetRecommendDataBase rdb = GameTableManager.Instance.GetTableItem<PetRecommendDataBase>((uint)pdb.petType, character);
            if (rdb == null)
            {
                Log.Error("rdb is null");
                return;
            }
            if (rdb != null)
            {
                string idStr = rdb.skillArray;
                string[] idArray = idStr.Split('_');
                foreach (var str in idArray)
                {
                    int id = 0;
                    if (int.TryParse(str, out id))
                    {
                        SkillDatabase db = GameTableManager.Instance.GetTableItem<SkillDatabase>((uint)id, 1);
                        if (db != null)
                        {
                            typeList.Add(db);
                        }

                    }
                }
            }
        }

        m_typeList = typeList;
        if (null != m_ctor_skill_scrollview)
        {
            if (!m_bInitSkillCreator)
            {
                m_bInitSkillCreator = true;
                GameObject go = m_btn_skill_item.gameObject;
                go.SetActive(false);
                m_ctor_skill_scrollview.RefreshCheck();
                m_ctor_skill_scrollview.Initialize<PetSkillItem>(go, OnUpdateUIGrid, OnUIGridEventDlg);
            }
            m_ctor_skill_scrollview.CreateGrids(typeList.Count);
        }
        
        bool bReset = false;
        CurSkillDataBase = typeList[0];

        SetPetItemHighLight();
    }

    public void SetLockSkillNum(uint num)
    {
        if (CurPet == null)
        {
            return;
        }
        if (num == 0)
        {
            //  m_label_jinengsuoding_number.gameObject.SetActive( false );
        }
        else
        {
            m_label_jinengsuoding_number.gameObject.SetActive(true);
        }
        int limitLock = GameTableManager.Instance.GetGlobalConfig<int>("PetSkillLockLimit");

        List<PetSkillObj> list = CurPet.GetPetSkillList();
        int lockNum = list.Count - limitLock;
        if (lockNum < 0)
        {
            lockNum = 0;
        }
        m_label_jinengsuoding_number.text = num.ToString() + "/" + lockNum.ToString();
        PetSkillLearnDataBase learnDb = GameTableManager.Instance.GetTableItem<PetSkillLearnDataBase>(num);
        if (learnDb != null)
        {
            m_label_suoding_xiaohao.text = learnDb.needDianJuan.ToString();
        }

    }
    void onClick_Xuejineng_zidongbuzu_Btn(GameObject caster)
    {
        bAutoFull = !bAutoFull;
        Transform spr = caster.transform.Find("Sprite");
        spr.gameObject.SetActive(bAutoFull);
        ShowLearnSkillColdLabel(m_nItemID);
    }
    void OnSKillClick(GameObject go)
    {
        PetSkillItem item = go.GetComponent<PetSkillItem>();
        if (item != null)
        {
            CurSkillDataBase = item.SkillData;
            m_ctor_skill_scrollview.SetSelect(item);
        }
    }
    void onClick_Jinengsuoding_Btn(GameObject caster)
    {
        int limitLock = GameTableManager.Instance.GetGlobalConfig<int>("PetSkillLockLimit");
        List<PetSkillObj> list = CurPet.GetPetSkillList();
        if (list.Count <= limitLock)
        {
            TipsManager.Instance.ShowLocalFormatTips(LocalTextType.Pet_Skill_xuehuiXgejinengcaikaiqisuodinggongneng, limitLock + 1);
            //      TipsManager.Instance.ShowTipsById( 108501 , limitLock + 1 );
            return;
        }
        petDataManager.bLockSkill = true;
        m_btn_jinengsuoding.gameObject.SetActive(false);
        m_btn_suodingqueding.gameObject.SetActive(true);
        SetSuoDingText(LocalTextType.Local_Txt_Pet_qingdianjisuodingjineng);
    }
    void SetSuoDingText(LocalTextType type)
    {
        Transform labelTrans = m_label_jinengsuoding_number.transform.Find("Label");
        if (labelTrans != null)
        {
            UILabel label = labelTrans.GetComponent<UILabel>();
            if (label != null)
            {
                label.text = DataManager.Manager<TextManager>().GetLocalText(type);
            }
        }
    }
    void onClick_Suodingqueding_Btn(GameObject caster)
    {
        SetSuoDingText(LocalTextType.Local_Txt_Pet_yisuodingjineng);
        petDataManager.bLockSkill = false;
        m_btn_jinengsuoding.gameObject.SetActive(true);
        m_btn_suodingqueding.gameObject.SetActive(false);

    }

    private CMResAsynSeedData<CMTexture> m_iconCASD = null;
    void InitLabel()
    {
        if (CurSkillDataBase != null)
        {
            m_label_xuejineng_Skillname.text = CurSkillDataBase.strName;
            m_label_xuejineng_SkillLevel.text = CurSkillDataBase.wdLevel.ToString() + CommonData.GetLocalString("级");
            if(m__skilldes_xuejineng_icon != null)
            {
                UIManager.GetTextureAsyn(CurSkillDataBase.iconPath, ref m_iconCASD, () =>
                {
                    if (null != m__skilldes_xuejineng_icon)
                    {
                        m__skilldes_xuejineng_icon.mainTexture = null;
                    }
                }, m__skilldes_xuejineng_icon);
            }
        
            m_label_xuejineng_NowLevel.text = CurSkillDataBase.strDesc;
            m_label_skilltype.text = petDataManager.GetSkillTypeStr((PetSkillType)CurSkillDataBase.petType);
            string itemArray = CurSkillDataBase.needItemArray;
            string[] itemIDArray = itemArray.Split(';');
            if (itemIDArray.Length < 1)
            {
                Log.Error("skilldatabase needitemarray error lenght less than 2 skillid is " + CurSkillDataBase.wdID);
                return;
            }
            string[] itemNum = itemIDArray[0].Split('_');
            if (itemNum.Length < 2)
            {
                Log.Error("skilldatabase needitemarray error lenght less than 2 skillid is " + CurSkillDataBase.wdID);
                return;
            }
            uint itemID = uint.Parse(itemNum[0]);
            uint needNum = uint.Parse(itemNum[1]);

            UIManager uiMan = DataManager.Manager<UIManager>();
            int itemCount = DataManager.Manager<ItemManager>().GetItemNumByBaseId(itemID);
            m_nItemID = itemID;
            m_label_xuejineng_number.text = StringUtil.GetNumNeedString(itemCount, needNum);
            UIItem.AttachParent(m_sprite_xuejineng_icon.transform, itemID, (uint)itemCount, ShowGetWayCallBack);
            ShowLearnSkillColdLabel(itemID);
            int count = 0;
            PetSkillLearnDataBase learnDb = GameTableManager.Instance.GetTableItem<PetSkillLearnDataBase>((uint)count);
            if (learnDb != null)
            {
                m_label_suoding_xiaohao.text = learnDb.needDianJuan.ToString();
            }
            m_label_xuejineng_goldxiaohao.text = CurSkillDataBase.dwMoney.ToString();
            ItemDataBase itemDb = GameTableManager.Instance.GetTableItem<ItemDataBase>(itemID);
            if (itemDb != null)
            {
                int num = DataManager.Manager<ItemManager>().GetItemNumByBaseId(itemDb.itemID);
                m_label_xuejineng_name.text = itemDb.itemName;
                m_label_xuejineng_number.text = StringUtil.GetNumNeedString(itemCount, needNum);
            }

            SetLockSkillNum(petDataManager.LockSkillNum);
        }
    }
    void ShowGetWayCallBack(UIItemCommonGrid grid)
    {
        DataManager.Manager<UIPanelManager>().ShowPanel(PanelID.GetWayPanel, data: grid.Data.DwObjectId);
    }
    uint m_nItemID = 0;
    bool CheckItem()
    {
        int count = DataManager.Manager<ItemManager>().GetItemNumByBaseId(m_nItemID);
        if (count <= 0)
        {
            if (bAutoFull)
            {

                if (!ShowLearnSkillColdLabel(m_nItemID))
                {
                    TipsManager.Instance.ShowTipsById(5);
                    return false;
                }
            }
            else
            {
                TipsManager.Instance.ShowTipsById(6);
                return false;
            }

        }
        return true;
    }
    bool ShowLearnSkillColdLabel(uint itemID)
    {
        int itemCount = DataManager.Manager<ItemManager>().GetItemNumByBaseId(itemID);
        if (itemCount <= 0)
        {
            if (bAutoFull)
            {
                string needStr = "";
                if (petDataManager.GetItemNeedColdString(itemID, ref needStr))
                {
                    m_label_xuejineng_number.gameObject.SetActive(false);
                    m_label_xuejineng_dianjuanxiaohao.gameObject.SetActive(true);
                    m_label_xuejineng_dianjuanxiaohao.text = needStr;
                    return true;
                }
                else
                {
                    m_label_xuejineng_number.gameObject.SetActive(false);
                    m_label_xuejineng_dianjuanxiaohao.gameObject.SetActive(true);
                    m_label_xuejineng_dianjuanxiaohao.text = needStr;
                    return false;
                }
            }
            else
            {
                m_label_xuejineng_number.gameObject.SetActive(true);
                m_label_xuejineng_dianjuanxiaohao.gameObject.SetActive(false);
            }
        }
        else
        {
            m_label_xuejineng_number.gameObject.SetActive(true);
            m_label_xuejineng_dianjuanxiaohao.gameObject.SetActive(false);
        }
        return true;
    }
    void OnValueUpdateEventArgs(object obj, ValueUpdateEventArgs v)
    {

        if (v != null)
        {
            if (v.key == PetDispatchEventString.ChangePet.ToString())
            {
                InitLabel();
                ResetSkillGrop();
                SetPetItemHighLight();
            }
            else if (v.key == PetDispatchEventString.PetRefreshProp.ToString())
            {
                InitLabel();
            }
        }

    }
    void SetPetItemHighLight()
    {

        PetSkillItem[] itemArray = m_ctor_skill_scrollview.GetComponentsInChildren<PetSkillItem>();
        foreach (var item in itemArray)
        {
            if (CurSkillDataBase == null)
            {
                m_ctor_skill_scrollview.SetSelect(item);

                break;
            }
            if (item.SkillData.wdID == CurSkillDataBase.wdID)
            {
                m_ctor_skill_scrollview.SetSelect(item);
                break;
            }
        }
    }
    void ResetSkillGrop()
    {
        petDataManager.bLockSkill = false;
        m_btn_jinengsuoding.gameObject.SetActive(true);
        m_btn_suodingqueding.gameObject.SetActive(false);
        SetLockSkillNum(0);
    }
    void onClick_Shengji_Btn(GameObject caster)
    {

        if (CurPet != null)
        {
            if (!CheckItem())
                return;
            List<PetSkillObj> skillList = CurPet.GetPetSkillList();
            bool hasShoudong = false;
            string skillname = "";
            foreach (var obj in skillList)
            {
                if (obj.id == (int)CurSkillDataBase.wdID)
                {
                    TipsManager.Instance.ShowTips(LocalTextType.Pet_Skill_yijingxuehuilegaijineng);
                    //TipsManager.Instance.ShowTipsById(108013 );
                    return;
                }
                SkillDatabase db = GameTableManager.Instance.GetTableItem<SkillDatabase>((uint)obj.id, 1);
                if (db != null)
                {
                    if (db.petType == (int)PetSkillType.Initiative)
                    {
                        skillname = db.strName;
                        hasShoudong = true;
                    }
                }
            }
            if (CurSkillDataBase.petType == (int)PetSkillType.Initiative)
            {
                if (hasShoudong)
                {
                    string tipstr = DataManager.Manager<TextManager>().GetLocalFormatText(LocalTextType.Pet_Skill_shoudongjinengfugaixuexitishi, skillname);
                    TipsManager.Instance.ShowTipWindow(TipWindowType.Custom, tipstr, () =>
                    {
                        stLearnSkillPetUserCmd_CS stcmd = new stLearnSkillPetUserCmd_CS();
                        stcmd.id = CurPet.GetID();
                        stcmd.auto_buy = bAutoFull;
                        stcmd.skill = (int)CurSkillDataBase.wdID;
                        NetService.Instance.Send(stcmd);
                    }, null, null,CommonData.GetLocalString("提示"), CommonData.GetLocalString("替换"), CommonData.GetLocalString("取消"));

                    return;
                }
            }

            stLearnSkillPetUserCmd_CS cmd = new stLearnSkillPetUserCmd_CS();
            cmd.id = CurPet.GetID();
            cmd.skill = (int)CurSkillDataBase.wdID;
            cmd.auto_buy = bAutoFull;
            NetService.Instance.Send(cmd);
        }

    }
    public override bool OnMsg(UIMsgID msgid, object param)
    {
        if (msgid == UIMsgID.eShowUI)
        {
            if (param is ReturnBackUIMsg)
            {
                ReturnBackUIMsg showInfo = (ReturnBackUIMsg)param;
                if (showInfo.tabs.Length > 0)
                {
                    CurType = (PetSkillType)showInfo.tabs[0];
                }

                foreach (Transform item in m_ctor_skill_scrollview.CacheTransform)
                {
                    PetSkillItem petItem = item.GetComponent<PetSkillItem>();
                    if (petItem != null && petItem.SkillData != null && petItem.SkillData.wdID == (uint)showInfo.param)
                    {
                        petItem.gameObject.SendMessage("OnClick", SendMessageOptions.RequireReceiver);
                    }
                }
            }

        }
        return base.OnMsg(msgid, param);
    }
    void onClick_Xuejineng_Sprite_Btn(GameObject caster)
    {

    }
    void onClick_Tuijian_Btn(GameObject caster)
    {
        CurType = PetSkillType.Recommend;
    }

    void onClick_Zhudong_Btn(GameObject caster)
    {
        CurType = PetSkillType.Initiative;
    }

    void onClick_Shoudong_Btn(GameObject caster)
    {
        CurType = PetSkillType.Auto;
    }

    void onClick_Beidong_Btn(GameObject caster)
    {
        CurType = PetSkillType.Passive;
    }
    void ShowLeftBtnHigh(PetSkillType st)
    {
        foreach (var dic in m_leftBtnDic)
        {
            Transform btn = m_leftBtnDic[st];
            if (btn != null)
            {
                UITabGrid tog = btn.GetComponent<UITabGrid>();
                if (tog != null)
                {
                    if (st == dic.Key)
                    {
                        tog.SetHightLight(true);
                    }
                    else
                    {
                        tog.SetHightLight(false);
                    }
                }
            }
        }
    }
    void onClick_Skill_item_Btn(GameObject caster)
    {

    }
    public override void Release(bool depthRelease = true)
    {
        base.Release(depthRelease);
        if(m_curIconAsynSeed != null)
        {
            m_curIconAsynSeed.Release();
            m_curIconAsynSeed = null;
        }

        if (null != m_iconCASD)
        {
            m_iconCASD.Release();
            m_iconCASD = null;
        }
        if(skillGrop != null)
        {
            skillGrop.Release();
        }
    }
    protected override void OnPanelBaseDestory()
    {
        base.OnPanelBaseDestory();
    }
}
