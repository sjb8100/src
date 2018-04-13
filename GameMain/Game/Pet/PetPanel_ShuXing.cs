//=========================================================================================
//
//    Author: zhudianyu
//    宠物属性界面处理
//    CreateTime:  2016/06/23
//
//=========================================================================================
using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using Client;
using Engine.Utility;
using table;
using GameCmd;

partial class PetPanel
{
    PetDataManager petDataManager
    {
        get
        {
            return  DataManager.Manager<PetDataManager>();
        }
    }

    /// <summary>
    /// 宠物属性相关的label
    /// </summary>
    Dictionary<string, List<UILabel>> labelDic = new Dictionary<string, List<UILabel>>();
    Dictionary<string, UISlider> sliderDic = new Dictionary<string, UISlider>();
    /*    List<Transform> rightContentList = new List<Transform>();*/
    Dictionary<int, Transform> m_dicContent = new Dictionary<int, Transform>();

    //  List<UIButton> rightBtnList = new List<UIButton>();
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

    #region panelbase overide
    PetSkillItemGrop shuxinggrop;
    PetSkillItemGrop jinenggrop;
    protected override void OnLoading()
    {
        base.OnLoading();

   
        InitTabs();
        InitLabel();
        InitRightBtn();
        InitTujianBtns();

        shuxinggrop = m_widget_ShuxingSkillbtnContainer.gameObject.AddComponent<PetSkillItemGrop>();
        if (shuxinggrop != null)
        {
            shuxinggrop.SkillGropType = PetSkillGroupType.Shuxing;
        }
        jinenggrop = m_widget_SkillbtnContainer.gameObject.AddComponent<PetSkillItemGrop>();
        if (jinenggrop != null)
        {
            jinenggrop.SkillGropType = PetSkillGroupType.MainSkill;
        }

        UIEventListener.Get(m_widget_xiuweishuxingtips.gameObject).onClick = ShowTianfuTips;
        UIEventListener.Get(m_widget_guiyuantips.gameObject).onClick = ShowTianfuTips;
   
    }
    void InitMedician()
    {
        uint id = GameTableManager.Instance.GetGlobalConfig<uint>("PetRecoverHPMedicineID");
        ItemDataBase db = GameTableManager.Instance.GetTableItem<ItemDataBase>(id);
        if (db != null)
        {
            UIManager.GetTextureAsyn(db.itemIcon, ref m_curMedicanIconAsynSeed, () =>
            {
                if (null != m__PetMedicanIcon)
                {
                    m__PetMedicanIcon.mainTexture = null;
                }
            }, m__PetMedicanIcon, false);
           // DataManager.Manager<UIManager>().SetSpriteDynamicIcon(m_sprite_PetMedicanIcon, db.itemIcon, false, true);
        }
        RefreshMedicianNum();
        UIEventListener.Get(m__PetMedicanIcon.gameObject).onClick = OnGotoMedician;
    }
    void RefreshMedicianNum()
    {
        uint id = GameTableManager.Instance.GetGlobalConfig<uint>("PetRecoverHPMedicineID");

        int num = DataManager.Manager<ItemManager>().GetItemNumByBaseId(id);
        m_label_PetMedicanNum.text = num.ToString();
    }
    void OnGotoMedician(GameObject go)
    {
        uint jump = GameTableManager.Instance.GetGlobalConfig<uint>("PetMedicineGotoID");
        ItemManager.DoJump(jump);
    }
    void ShowTianfuTips(GameObject go)
    {
        DataManager.Manager<UIPanelManager>().ShowPanel(PanelID.GuiyuanExplanationPanel);
    }
    Dictionary<GameObject, UITabGrid> m_titleTabs = new Dictionary<GameObject, UITabGrid>();
    void InitTabs()
    {

        UITabGrid tab1 = m_trans_BaseProp.gameObject.GetComponent<UITabGrid>();
        if (tab1 == null)
        {
            tab1 = m_trans_BaseProp.gameObject.AddComponent<UITabGrid>();
        }
        if (tab1 != null)
        {
            if (!m_titleTabs.ContainsKey(m_trans_BaseProp.gameObject))
            {
                m_titleTabs.Add(m_trans_BaseProp.gameObject, tab1);
                tab1.RegisterUIEventDelegate(OnUITabGridEventDlg);
                tab1.TabID = 100;
            }

        }
        UITabGrid tab2 = m_trans_DetailProp.gameObject.GetComponent<UITabGrid>();
        if (tab2 == null)
        {
            tab2 = m_trans_DetailProp.gameObject.AddComponent<UITabGrid>();
        }
        if (tab2 != null)
        {
            if (!m_titleTabs.ContainsKey(m_trans_DetailProp.gameObject))
            {
                m_titleTabs.Add(m_trans_DetailProp.gameObject, tab2);
                tab2.RegisterUIEventDelegate(OnUITabGridEventDlg);
                tab2.TabID = 101;
            }
        }


    }
    private void OnUITabGridEventDlg(UIEventType eventType, object data, object param)
    {
        if (null == data)
        {
            return;
        }
        switch (eventType)
        {
            case UIEventType.Click:
                {

                    if (data is UITabGrid)
                    {
                        UITabGrid tab = data as UITabGrid;
                        if (tab.TabID == 100)
                        {
                            onClick_BaseProp_Btn(m_trans_BaseProp.gameObject);
                        }
                        else if (tab.TabID == 101)
                        {
                            onClick_DetailProp_Btn(m_trans_DetailProp.gameObject);
                        }
                    }

                }
                break;
        }
    }
    void SetTitleTabsHighLight(GameObject go)
    {
        foreach (var dic in m_titleTabs)
        {
            if (dic.Key.name == go.name)
            {
                dic.Value.SetHightLight(true);
            }
            else
            {
                dic.Value.SetHightLight(false);
            }
        }
    }
    protected override void OnPanelBaseDestory()
    {
        base.OnPanelBaseDestory();

    }
    protected override void OnHide()
    {
        base.OnHide();
        petDataManager.ValueUpdateEvent -= OnValueUpdateEventArgs;
        petDataManager.ReleaseRenderObj();
        RegisterGlobalUIEvent(false);
        Release();
    }
    protected override void OnShow(object data)
    {
        base.OnShow(data);
        petDataManager.ValueUpdateEvent += OnValueUpdateEventArgs;
        petDataManager.InitCurPet();

        InitShuXingData();
        InitPutongGuiYuanUI();

        InitTujian();
        RegisterGlobalUIEvent(true);
        UpdatePetScollView();
        ShowFightBtn();
        ShowRedPoint();
        ShowTujianRedPoint();
        InitMedician();
    }
    protected override void OnJump(UIPanelBase.PanelJumpData jumpData)
    {
        base.OnJump(jumpData);

        if (jumpData == null)
        {
            int pageID = (int)TabMode.ShuXing;
            if (CurPet != null)
            {
                pageID = (int)TabMode.ShuXing;
                onClick_BaseProp_Btn(m_trans_BaseProp.gameObject);
                ShowNullTips();
            }
            else
            {
                pageID = (int)TabMode.TuJian;
            }
            UIFrameManager.Instance.OnCilckTogglePanel(this.PanelId, 1, pageID);

        }
        else
        {
            if (jumpData.Tabs != null)
            {
                if (jumpData.Tabs.Length > 0)
                {
                    int pageid = jumpData.Tabs[0];
                    UIFrameManager.Instance.OnCilckTogglePanel(this.PanelId, 1, pageid);

                    if (pageid == (int)TabMode.ChuanCheng)
                    {
                        if(jumpData.ExtParam != null && jumpData.ExtParam is PetInheritJumpData)
                        {
                            PetInheritJumpData data = (PetInheritJumpData)jumpData.ExtParam;
                            OnJumpHerit(data);
                        }
                   
                    }
                    else if(pageid == (int)TabMode.TuJian)
                    {
                        return;
                    }
                    else
                    {
                        if (jumpData.Param != null)
                        {
                            foreach (Transform item in m_ctor_petscrollview.transform.GetComponentsInChildren<Transform>())
                            {
                                PetScrollItem petItem = item.GetComponent<PetScrollItem>();
                                if (petItem != null && petItem.PetData != null)
                                {
                                    uint petID = petItem.PetData.GetID();
                                    if(jumpData.Param is uint)
                                    {
                                        if (petID == (uint)jumpData.Param)
                                        {
                                            petItem.gameObject.SendMessage("OnClick", SendMessageOptions.DontRequireReceiver);
                                        }
                                    }
                                   
                                }
                            }
                        }
                    }

                }
                
                ShowNullTips();
            }
        }

    }
    void ShowNullTips()
    {

        if (CurPet == null)
        {
            var iter = m_dicContent.GetEnumerator();
            while (iter.MoveNext())
            {
                iter.Current.Value.gameObject.SetActive(false);
            }
            m_trans_NullPetTipsContent.gameObject.SetActive(true);
        }
        else
        {
            m_trans_NullPetTipsContent.gameObject.SetActive(false);
        }
    }
    public override UIPanelBase.PanelData GetPanelData()
    {
        PanelData pd = base.GetPanelData();
        pd.JumpData = new PanelJumpData();
        pd.JumpData.Tabs = new int[2];
        if(m_nRightToggleIndex == 6)
        {
            //学习技能
            m_nRightToggleIndex = (int)TabMode.ShuXing;
        }
        pd.JumpData.Tabs[0] = (int)m_nRightToggleIndex;
        if (CurPet != null)
        {
            pd.JumpData.Param = CurPet.GetID();
        }
        if (m_nRightToggleIndex == (int)TabMode.ChuanCheng)
        {
            PetInheritJumpData data = new PetInheritJumpData();
            data.newPetID = m_uPetNewThisID;
            data.oldPetID = m_uPetOldThisID;
            data.bInheritExp = petDataManager.bInheritExp;
            data.bInheritSkill = petDataManager.bInheritSkill;
            data.bInheritXiuwei = petDataManager.bInheritXiuwei;
            pd.JumpData.ExtParam = data;
        }
        return pd;
    }

    /// <summary>
    /// 显示图鉴红点
    /// </summary>
    void ShowTujianRedPoint()
    {
        Dictionary<int, UITabGrid> dic = null;
        if(dicUITabGrid.TryGetValue(1,out dic))
        {
            UITabGrid grid = null;
            if (dic.TryGetValue((int)TabMode.TuJian, out grid))
            {
                if (petDataManager.GetCanComposePetList().Count > 0)
                {
                    grid.SetRedPointStatus(true);
                }
                else
                {
                    grid.SetRedPointStatus(false);
                }
            }
        }
    
      
    }
    /// <summary>
    /// 显示加点红点
    /// </summary>
    void ShowRedPoint()
    {
        if (CurPet == null)
        {
            return;
        }
        if(m_trans_shuxingPointWarring == null)
        {
            return;
        }
        Transform pointWarringTrans = m_trans_shuxingPointWarring.transform.Find("Label");
        if (pointWarringTrans != null)
        {
            UILabel pointLable = pointWarringTrans.GetComponent<UILabel>();
            if (pointLable != null)
            {
                int maxPoint = CurPet.GetProp((int)PetProp.MaxPoint);
                PetTalent attrPoint = CurPet.GetAttrTalent();
                uint usePint = attrPoint.jingshen + attrPoint.liliang + attrPoint.minjie + attrPoint.zhili + attrPoint.tizhi;
                int leftPoint = maxPoint - (int)usePint;
                if (leftPoint == 0)
                {
                    m_trans_shuxingPointWarring.gameObject.SetActive(false);
                }
                else
                {
                    m_trans_shuxingPointWarring.gameObject.SetActive(true);
                }
                if (leftPoint > 99)
                {
                    leftPoint = 99;
                }
                pointLable.text = leftPoint.ToString();
            }
        }
    }

    #endregion
    #region contronls
    void InitLabel()
    {

        labelDic.Add(CreatureProp.MaxHp.ToString(), new List<UILabel>() { m_label_qixue });
        labelDic.Add(FightCreatureProp.Strength.ToString(), new List<UILabel>() { m_label_liliangzhi });
        labelDic.Add(FightCreatureProp.Corporeity.ToString(), new List<UILabel>() { m_label_tilizhi });
        labelDic.Add(FightCreatureProp.Intelligence.ToString(), new List<UILabel>() { m_label_zhilizhi });
        labelDic.Add(FightCreatureProp.Spirit.ToString(), new List<UILabel>() { m_label_jingshenzhi });
        labelDic.Add(FightCreatureProp.Agility.ToString(), new List<UILabel>() { m_label_minjiezhi });

        labelDic.Add(PetProp.StrengthTalent.ToString(), new List<UILabel>() { m_label_Pro_liliang, /*m_label_PtGuiyuanLiLiang*/ });
        labelDic.Add(PetProp.CorporeityTalent.ToString(), new List<UILabel>() { m_label_Pro_tili, /*m_label_PtGuiyuanTiLi*/ });
        labelDic.Add(PetProp.IntelligenceTalent.ToString(), new List<UILabel>() { m_label_Pro_zhili, /*m_label_PtGuiyuanZhiLi*/ });
        labelDic.Add(PetProp.SpiritTalent.ToString(), new List<UILabel>() { m_label_Pro_jingshen, /*m_label_PtGuiyuanJingShen*/ });
        labelDic.Add(PetProp.AgilityTalent.ToString(), new List<UILabel>() { m_label_Pro_minjie, /*m_label_PtGuiyuanMinJie*/ });

        labelDic.Add(FightCreatureProp.PhysicsAttack.ToString(), new List<UILabel>() { m_label_wugong });
        labelDic.Add(FightCreatureProp.PhysicsDefend.ToString(), new List<UILabel>() { m_label_wufang });
        labelDic.Add(FightCreatureProp.MagicAttack.ToString(), new List<UILabel>() { m_label_fagong });
        labelDic.Add(FightCreatureProp.MagicDefend.ToString(), new List<UILabel>() { m_label_fafang });
        labelDic.Add(FightCreatureProp.Hit.ToString(), new List<UILabel>() { m_label_wulimingzhong });

        labelDic.Add(FightCreatureProp.IceDefend.ToString(), new List<UILabel>() { m_label_bingfang });
        labelDic.Add(FightCreatureProp.FireDefend.ToString(), new List<UILabel>() { m_label_huofang });
        labelDic.Add(FightCreatureProp.EleDefend.ToString(), new List<UILabel>() { m_label_dianfang });
        labelDic.Add(FightCreatureProp.WitchDefend.ToString(), new List<UILabel>() { m_label_anfang });
        labelDic.Add(FightCreatureProp.Power.ToString(), new List<UILabel>() { m_label_PetFightPowerLabel });
        labelDic.Add(FightCreatureProp.Dodge.ToString(), new List<UILabel>() { m_label_shanbi });
        labelDic.Add(PetProp.YinHunLevel.ToString(), new List<UILabel>() { m_label_xiuwei, m_label_yuan_xiuwei, m_label_xin_xiuwei });
        labelDic.Add(PetProp.Character.ToString(), new List<UILabel>() { m_label_chongwutedian });
        labelDic.Add(FightCreatureProp.PhysicsCrit.ToString(), new List<UILabel>() { m_label_wulibaoji });
        labelDic.Add(FightCreatureProp.MagicCrit.ToString(), new List<UILabel>() { m_label_fashubaoji });
        //  labelDic.Add( FightCreatureProp.Power.ToString() , m_label_Label );
        labelDic.Add(PetProp.PetGuiYuanStatus.ToString(), new List<UILabel>() { m_label_chengzhangdengji, m_label_growstate, m_label_PtGuiyuanGrow, m_label_GjGuiyuanGrow });

    }
    void InitRightBtn()
    {
        m_dicContent.Add((int)TabMode.ShuXing, m_trans_shuxingcontent);
        m_dicContent.Add((int)TabMode.XiLian, m_trans_xinguiyuanContent);
        m_dicContent.Add((int)TabMode.XiuLing, m_trans_yinhuncontent);
        m_dicContent.Add((int)TabMode.ChuanCheng, m_trans_ChuanChengContent);
        m_dicContent.Add((int)TabMode.TuJian, m_trans_tujiancontent);
        m_dicContent.Add(6, m_trans_jinengcontent);


    }

    #endregion
    #region init data
    void InitShuXingData()
    {
        if (CurPet == null)
        {

            return;
        }
        InitLabelData();
        InitTableData();
        InitSliderData();
        InitSkillPanel();
        InitYinhunPaneel();
        RefreshMedicianNum();
    }
    void InitLabelData()
    {
        if (CurPet != null)
        {
            bool isFight = petDataManager.IsCurPetFighting();
            var iter = labelDic.GetEnumerator();

            while (iter.MoveNext())
            {
                var labelPair = iter.Current;
                List<UILabel> laList = labelPair.Value;

                for (int i = 0; i < laList.Count; i++)
                {
                    var label = laList[i];
                    if (label != null)
                    {
                        label.text = petDataManager.GetPropByString(labelPair.Key).ToString();
                        if (labelPair.Key == PetProp.PetGuiYuanStatus.ToString())
                        {
                            int status = CurPet.GetProp((int)PetProp.PetGuiYuanStatus);
                            string str = petDataManager.GetGrowStatus(status);
                            label.text = str;
                        }
                        else if (labelPair.Key == PetProp.Character.ToString())
                        {
                            int c = CurPet.GetProp((int)PetProp.Character);
                            label.text = petDataManager.GetPetCharacterStr(c);
                        }
                        else if (labelPair.Key == FightCreatureProp.PhysicsCrit.ToString() ||
                            labelPair.Key == FightCreatureProp.MagicCrit.ToString() ||
                            labelPair.Key == FightCreatureProp.Hit.ToString() ||
                            labelPair.Key == FightCreatureProp.Dodge.ToString())
                        {
                            FightCreatureProp prop = (FightCreatureProp)Enum.Parse(typeof(FightCreatureProp), labelPair.Key);
                            int phy = 0;
                            if (isFight)
                            {
                                phy = petDataManager.GetFightPetAttr((int)prop);
                            }
                            else
                            {
                                phy = CurPet.GetProp((int)prop);
                            }
                            int pv = phy / 100;
                            label.text = pv + "%";
                        }
                        else if (labelPair.Key == FightCreatureProp.PhysicsAttack.ToString() ||
                          labelPair.Key == FightCreatureProp.PhysicsDefend.ToString() ||
                          labelPair.Key == FightCreatureProp.MagicAttack.ToString() ||
                             labelPair.Key == FightCreatureProp.IceDefend.ToString() ||
                             labelPair.Key == FightCreatureProp.EleDefend.ToString() ||
                             labelPair.Key == FightCreatureProp.WitchDefend.ToString() ||
                          labelPair.Key == FightCreatureProp.FireDefend.ToString())
                        {
                            if (isFight)
                            {
                                FightCreatureProp prop = (FightCreatureProp)Enum.Parse(typeof(FightCreatureProp), labelPair.Key);
                                int phy = petDataManager.GetFightPetAttr((int)prop);

                                label.text = phy.ToString();
                            }
                        }
                        else if(labelPair.Key == CreatureProp.MaxHp.ToString())
                        {
                            if (isFight)
                            {
                                CreatureProp prop = (CreatureProp)Enum.Parse(typeof(CreatureProp), labelPair.Key);
                                int phy = petDataManager.GetFightPetAttr((int)prop);

                                label.text = phy.ToString();
                            }
                        }
                    }
                }
            }
            m_label_exppetlevel.text = petDataManager.GetCurPetLevelStr();
            ShowNextXiuWei();
        }
    }
    void InitTableData()
    {
        PetDataBase pb = petDataManager.GetPetDataBase(PetBaseID);
        if (pb != null)
        {
            m_label_xiedaidengji.text = pb.carryLevel.ToString();
        }
    }
    void RegisterGlobalUIEvent(bool register = true)
    {
        if (register)
        {
            Engine.Utility.EventEngine.Instance().AddEventListener((int)Client.GameEventID.UIEVENT_UPDATEITEM, OnGlobalUIEventHandler);
        }
        else
        {
            Engine.Utility.EventEngine.Instance().RemoveEventListener((int)Client.GameEventID.UIEVENT_UPDATEITEM, OnGlobalUIEventHandler);
        }
    }
    private void OnGlobalUIEventHandler(int eventType, object data)
    {
        switch (eventType)
        {

            case (int)Client.GameEventID.UIEVENT_UPDATEITEM:
                OnUpdateItem((ItemDefine.UpdateItemPassData)data);
                InitSliderData();
                break;
        }
    }
    void OnUpdateItem(ItemDefine.UpdateItemPassData passData)
    {
        if (null == passData)
        {
            return;
        }
        uint qwThisId = passData.QWThisId;

        InitYinhunPaneel();
        InitSkillPanel();
        InitPutongGuiYuanUI();

    }
    void InitSliderData()
    {
        if (CurPet == null)
        {
            return;
        }
        int level = CurPet.GetProp((int)CreatureProp.Level);
        int petstate = CurPet.GetProp((int)PetProp.PetGuiYuanStatus);
        int curLife = CurPet.GetProp((int)PetProp.Life);
        int curExp = CurPet.GetProp((int)PetProp.LevelExp);
        uint totalExp = GameTableManager.Instance.GetTableItem<PetUpGradeDataBase>((uint)level, (int)CurPet.PetBaseID).upGradeExp;
        PetDataBase db = petDataManager.GetPetDataBase(CurPet.PetBaseID);
        if (db != null)
        {
            uint life = db.life;
            ShowSlider(m_slider_lifescorllbar, curLife, (int)life);
        }
        ShowSlider(m_slider_expscorllbar, curExp, (int)totalExp);
        int yinhunLv = CurPet.GetProp((int)PetProp.YinHunLevel);
        int yinhunExp = CurPet.GetProp((int)PetProp.YinHunExp);
        PetYinHunDataBase yhDb = GameTableManager.Instance.GetTableItem<PetYinHunDataBase>((uint)yinhunLv);
        if (yhDb != null)
        {
            ShowSlider(m_slider_lingqizhi, yinhunExp, (int)yhDb.lingQiMax);
        }

    }
    #region 图鉴
    uint petTujianType = 0;
    private bool m_bInitTujianCreator = false;
    void InitTujian()
    {
        List<PetDataBase> dbList = petDataManager.GetAllCanMakePets();
        if (petTujianType != 0)
        {
            dbList = dbList.FindAll((x) => { return x.flag == petTujianType; });
        }
        dbList.Sort((x1, x2) =>
        {
            if (x1.sortID > x2.sortID)
                return 1;
            else if (x1.sortID == x2.sortID)
                return 0;
            else
                return -1;
        });
        List<PetDataBase> canComposeList = new List<PetDataBase>();
        List<PetDataBase> notComposeList = new List<PetDataBase>();

        for (int i = 0; i < dbList.Count; i++)
        {
            var db = dbList[i];
            int itemCount = DataManager.Manager<ItemManager>().GetItemNumByBaseId(db.fragmentID);
            uint needNum = db.fragmentNum;
            if (itemCount >= needNum)
            {
                canComposeList.Add(db);
            }
            else
            {
                notComposeList.Add(db);
            }
        }
        dbList.Clear();
        dbList.AddRange(canComposeList);
        dbList.AddRange(notComposeList);

        petDataManager.PetTuJianList.Clear();
        petDataManager.PetTuJianList.AddRange(dbList);
        if (dbList != null)
        {
            bool visible = m_trans_tujiancontent.gameObject.activeSelf;
            if (null != m_ctor_tujianscroll)
            {
                if (!m_bInitTujianCreator)
                {
                    m_bInitTujianCreator = true;
                    GameObject go = m_widget_PetTujianItem.gameObject;
                    go.SetActive(false);
                    m_ctor_tujianscroll.RefreshCheck();
                    m_ctor_tujianscroll.Initialize<PetTujianItem>(go, OnUpdateUIGrid, OnUIGridEventDlg);
                }
                int createCount = dbList.Count;
                m_ctor_tujianscroll.CreateGrids(createCount);
                
            }
        }

    }

    void onClick_TujiantitleItem_Btn(GameObject caster)
    {

    }
    List<string> GetSortTitleList()
    {
        List<string> keyList = GameTableManager.Instance.GetGlobalConfigKeyList("PetUIType");
        keyList.Sort((x1, x2) =>
        {
            List<string> aCon = GameTableManager.Instance.GetGlobalConfigList<string>("PetUIType", x1);
            List<string> bCon = GameTableManager.Instance.GetGlobalConfigList<string>("PetUIType", x2);
            int asort = 0, bsort = 0;
            if (aCon == null || bCon == null)
            {
                return 0;
            }
            if (aCon.Count == 2 && bCon.Count == 2)
            {

                if (int.TryParse(aCon[1], out asort))
                {
                    if (int.TryParse(bCon[1], out bsort))
                    {

                    }

                }
            }
            if (asort < bsort)
            {
                return -1;
            }
            else if (asort > bsort)
            {
                return 1;
            }
            else
            {
                return 0;
            }

        });
        return keyList;
    }
    string GetSortTitleByIndex(int index)
    {
        List<string> keyList = GetSortTitleList();
        if (keyList == null)
        {
            return "";
        }
        if (index < keyList.Count)
        {
            string str = keyList[index];
            List<string> aCon = GameTableManager.Instance.GetGlobalConfigList<string>("PetUIType", str);
            if (aCon != null)
            {

                return aCon[0];
            }
        }
        return "";

    }
    int GetSortTitleTypeByIndex(int sortID)
    {
        List<string> keyList = GetSortTitleList();
        if (keyList == null)
        {
            return 0;
        }
        for (int i = 0; i < keyList.Count; i++)
        {
            string str = keyList[i];
            List<string> aCon = GameTableManager.Instance.GetGlobalConfigList<string>("PetUIType", str);
            if (aCon != null)
            {
                if (aCon.Count == 2)
                {
                    int type = 0;
                    if (int.TryParse(aCon[1], out type))
                    {
                        if (type == sortID)
                        {
                            if (int.TryParse(str, out type))
                            {
                                return type;
                            }
                        }
                    }

                }
            }
        }

        return 0;

    }
    void InitTujianBtns()
    {
        if (null != m_ctor_tujiantitlescroll)
        {
            GameObject go = m_sprite_tujiantitleItem.gameObject;
            go.SetActive(false);
            m_ctor_tujiantitlescroll.RefreshCheck();
            m_ctor_tujiantitlescroll.Initialize<PetTujianTitleItem>(go, OnUpdateUIGrid, OnUIGridEventDlg);
            List<string> keyList = GameTableManager.Instance.GetGlobalConfigKeyList("PetUIType");


            int createCount = keyList.Count;

            m_ctor_tujiantitlescroll.CreateGrids(createCount);
        }


    }
    void TujianBtnClick(GameObject go)
    {
        string name = go.name;
        uint type = 0;
        if (uint.TryParse(name, out type))
        {
            type = type + 1;
            int t = GetSortTitleTypeByIndex((int)type);
            petTujianType = (uint)t;
            InitTujian();
            return;
        }
        Engine.Utility.Log.Error("解析类型错误");
        return;
    }
    #endregion


    void ShowSlider(UISlider slider, int curValue, int totalValue)
    {

        UILabel label = slider.transform.Find("Percent").GetComponent<UILabel>();
        string str = "" + curValue + "/" + totalValue;
        label.text = str;
        if (totalValue == 0)
        {
            curValue = 0;
            totalValue = 10000;
            Log.Info("total value is 0");
        }
        float precent = (float)(curValue * 1.0f / totalValue * 1.0f);
        slider.Set(precent);

    }
    #endregion


    #region btnclick
    int m_nLastToggleIndex = 0;
    int m_nRightToggleIndex = 0;
    public int RightToggleIndex { get { return m_nRightToggleIndex; } }

    public void InitRenderTextureObj()
    {
        if (m_widget_PetMessageShuXing == null)
        {
            return;
        }

        PetMessage message = m_widget_PetMessageShuXing.gameObject.GetComponent<PetMessage>();
        if (message == null)
        {
            message = m_widget_PetMessageShuXing.gameObject.AddComponent<PetMessage>();
        }
        if (message != null)
        {
            PetDataBase db = null;
            if (petDataManager.CurPet != null)
            {
                db = petDataManager.GetPetDataBase(petDataManager.CurPet.PetBaseID);
            }
            message.InitPetTexture(db);
        }
    }

    public override bool OnTogglePanel(int tabType, int pageid)
    {
        base.OnTogglePanel(tabType, pageid);
        TabMode subFlag = (TabMode)pageid;
        if (subFlag == TabMode.ShuXing)
        {
            InitRenderTextureObj();
        }
        return ShowRightBtnContent(subFlag);

    }
    bool ShowRightBtnContent(TabMode flag)
    {
        //if (CurPet == null)
        //{
        //    flag = PetPanelSubFlag.Tujian;
        //}
        int index = (int)flag;
        if (!IsPetOpen())
        {
            if (flag != TabMode.TuJian)
            {
                return false;
            }
        }

        m_nLastToggleIndex = m_nRightToggleIndex;
        m_nRightToggleIndex = index;
        var iter = m_dicContent.GetEnumerator();
        while (iter.MoveNext())
        {
            if (iter.Current.Key == (int)index)
            {
                iter.Current.Value.gameObject.SetActive(true);
            }
            else
            {
                iter.Current.Value.gameObject.SetActive(false);
            }
        }

        if (flag == TabMode.TuJian)
        {
            InitTujian();
            m_widget_leftcontent.gameObject.SetActive(false);
        }
        else
        {
            m_widget_leftcontent.gameObject.SetActive(true);
        }


        if (flag == TabMode.ShuXing)
        {
            onClick_BaseProp_Btn(m_trans_BaseProp.gameObject);
        }
        else if (flag == TabMode.ChuanCheng)
        {
            InitInHerit();
        }
        InitSliderData();
        m_nRightToggleIndex = (int)flag;
        return true;
    }
    void RefreshReliveCountDown()
    {
        if (CurPet == null)
        {
            return;
        }
        if (m_label_chuanzhancd != null)
        {
            int time = petDataManager.GetPetFightCDTime(CurPet.GetID());
            string str = CommonData.GetLocalString("出战");
            if (time > 0)
            {
                m_label_chuanzhancd.gameObject.SetActive(true);
                m_label_chuanzhancd.text = str + "(" + time.ToString() + ")";
            }
            else
            {
                m_label_chuanzhancd.text = str;
            }
        }
    }
    void Update()
    {

    }
    void onClick_Close_Btn(GameObject caster)
    {
        HideSelf();
    }
    #region 页签

    bool IsPetOpen()
    {
        int openLevel = GameTableManager.Instance.GetGlobalConfig<int>("Pet_OpenLevel");
        int playerLevel = Client.ClientGlobal.Instance().MainPlayer.GetProp((int)CreatureProp.Level);
        if (playerLevel < openLevel)
        {
            ShowTipsEnum(LocalTextType.Pet_Commond_Xjikaiqizhanhungongneng, openLevel);
            //    ShowRightToggle();
            return false;
        }
        if (CurPet == null)
        {

            ShowTipsEnum(LocalTextType.Pet_Commond_meiyouzhanhunwufadakaishuxingjiemian);

            return false;
        }
        return true;
    }

    void onClick_BaseProp_Btn(GameObject caster)
    {
        SetTitleTabsHighLight(caster);
        m_widget_basePropcontent.gameObject.SetActive(true);
        m_widget_detailPropcontent.gameObject.SetActive(false);
    }

    void onClick_DetailProp_Btn(GameObject caster)
    {
        SetTitleTabsHighLight(caster);

        m_widget_basePropcontent.gameObject.SetActive(false);
        m_widget_detailPropcontent.gameObject.SetActive(true);
    }
    #endregion
    #region 属性面板按钮
    /// <summary>
    /// 丢弃
    /// </summary>
    /// <param name="caster"></param>
    void onClick_Diuqi_Btn(GameObject caster)
    {
        if (CurPet != null)
        {
            if (petDataManager.IsCurPetFighting())
            {
                ShowTipsEnum(LocalTextType.Pet_Commond_wufafengyinchuzhanzhongdezhanhun);
                return;
            }
            DataManager.Manager<UIPanelManager>().ShowPanel(PanelID.PetAbandonPanel);
        }
    }
    void ShowTipsEnum(LocalTextType error, params object[] args)
    {
        TipsManager.Instance.ShowLocalFormatTips(error, args);
    }
    void onClick_Chuzhan_Btn(GameObject caster)
    {
        if (CurPet != null)
        {
            int life = CurPet.GetProp((int)PetProp.Life);
            if (life <= 0)
            {
                ShowTipsEnum(LocalTextType.Pet_Age_zhanhunshoumingbuzuwufachuzhan);
                //ShowTips(108017);
                return;
            }
            int cd = petDataManager.GetPetFightCDTime(CurPet.GetID());
            if (cd > 0)
            {
                TipsManager.Instance.ShowTipsById(108020, cd);
                return;
            }
            stUseFightPetUserCmd_CS cmd = new stUseFightPetUserCmd_CS();
            cmd.id = CurPet.GetID();
            NetService.Instance.Send(cmd);
        }
    }
    void onClick_Xiuxi_Btn(GameObject caster)
    {
        if (CurPet != null)
        {
            stCallBackPetUserCmd_CS cmd = new stCallBackPetUserCmd_CS();
            cmd.id = CurPet.GetID();
            NetService.Instance.Send(cmd);
        }
    }
    bool changeName = false;
    void onClick_Changename_Btn(GameObject caster)
    {
        if (changeName)
        {
            ShowTipsEnum(LocalTextType.Pet_Commond_xiugaimingziCDzhong);
            //   ShowTips(108509);
            return;
        }
        DataManager.Manager<UIPanelManager>().ShowPanel(PanelID.PetChangeNamePanel);
        StartCoroutine(CountDown());
    }
    IEnumerator CountDown()
    {
        yield return new WaitForSeconds(300);
        changeName = true;
    }
    void onClick_Buzhen_Btn(GameObject caster)
    {
        DataManager.Manager<UIPanelManager>().ShowPanel(PanelID.PetEmbattlePanel);
    }
    void onClick_Shuxingfenpei_Btn(GameObject caster)
    {
        //HidePanel();

        DataManager.Manager<UIPanelManager>().ShowPanel(PanelID.PetAddPointPanel);
        //DataManager.Manager<UIPanelManager>().ShowPanel( PanelID.TopBarPanel );
    }

    void onClick_Addexp_Btn(GameObject caster)
    {
        if (CurPet != null)
        {
            UseItemCommonPanel.UseItemParam data = new UseItemCommonPanel.UseItemParam();
            data.type = UseItemCommonPanel.UseItemEnum.PetExp;
            data.userId = CurPet.GetID();
            data.tabs = new int[] { (int)TabMode.ShuXing };
            DataManager.Manager<UIPanelManager>().ShowPanel(PanelID.UseItemCommonPanel, data: data);
        }
    }

    void onClick_Addlife_Btn(GameObject caster)
    {
        if (CurPet != null)
        {
            UseItemCommonPanel.UseItemParam data = new UseItemCommonPanel.UseItemParam();
            data.type = UseItemCommonPanel.UseItemEnum.PetLife;
            data.userId = CurPet.GetID();
            data.tabs = new int[] { (int)TabMode.ShuXing };
            DataManager.Manager<UIPanelManager>().ShowPanel(PanelID.UseItemCommonPanel, data: data);
        }
    }
    void ShowFightBtn()
    {
        bool isfight = petDataManager.IsHasPetFight;

        if (CurPet != null)
        {
            if (CurPet.GetID() == petDataManager.CurFightingPet)
            {
                if (isfight)
                {
                    isfight = true;
                }
            }
            else
            {
                isfight = false;
            }
        }
        else
        {
            isfight = false;
        }
        if (m_btn_xiuxi != null)
        {
            m_btn_xiuxi.gameObject.SetActive(isfight);
        }
        if (m_btn_chuzhan != null)
        {
            m_btn_chuzhan.gameObject.SetActive(!isfight);
        }
    }

    void onClick_Jinengxuexi_Btn(GameObject caster)
    {
        int lv = MainPlayerHelper.GetPlayerLevel();
        int openLv = GameTableManager.Instance.GetGlobalConfig<int>("PetSkillOpenLevel");
        if (lv >= openLv)
        {
            ShowRightBtnContent(TabMode.Max);
        }
        else
        {
            TipsManager.Instance.ShowLocalFormatTips(LocalTextType.Pet_Commond_kaifangchongwujinengxitong, openLv);
        }

    }
    #endregion

    #endregion
    #region 属性改变
    void OnValueUpdateEventArgs(object obj, ValueUpdateEventArgs v)
    {

        if (v != null)
        {
            if (v.key == PetDispatchEventString.DeletePet.ToString() || v.key == PetDispatchEventString.ChangePetNum.ToString() || v.key == PetDispatchEventString.AddPet.ToString())
            {
                UpdatePetScollView();
                if (v.key == PetDispatchEventString.DeletePet.ToString())
                {
                    OnDiscardPet();
                }
            }
            else if (v.key == PetDispatchEventString.ChangePet.ToString())
            {
                InitFirstSkill();
                ShowFightBtn();
                ShowRedPoint();
                RefreshReliveCountDown();
                InitSkillPanel();
                InitPutongGuiYuanUI();

            }
            else if (v.key == PetDispatchEventString.ChangeFight.ToString())
            {
                ShowFightBtn();
            }
            else if (v.key == PetDispatchEventString.PetRefreshProp.ToString())
            {
                ShowRedPoint();
            }
            else if (v.key == PetDispatchEventString.RefreshCountDown.ToString())
            {
                RefreshReliveCountDown();
                return;
            }
            else if (v.key == PetDispatchEventString.SelectPet.ToString())
            {
                OnInheritChangePet();
                return;
            }
            else if (v.key == PetDispatchEventString.ResetInheritPanel.ToString())
            {
                InitInHerit();
                return;
            }
            OnPutongGuiYuanEvent(v);
        }
        InitShuXingData();
    }

    #endregion

    #region left scroll view
    void OnDiscardPet()
    {//宠物为0
        if (CurPet == null)
        {
            TabMode subFlag = TabMode.TuJian;
            UIFrameManager.Instance.OnCilckTogglePanel(PanelID.PetPanel, 1, (int)subFlag);

            return;
        }
    }
    List<IPet> GetSortPetList(List<IPet> petList)
    {
        if (petList == null)
        {
            return null;
        }
        petList.Sort((x1, x2) =>
        {
            if (x1 == null || x2 == null)
            {
                return 0;
            }
            int lv1 = x1.GetProp((int)CreatureProp.Level);
            int lv2 = x2.GetProp((int)CreatureProp.Level);
            if (lv1 < lv2)
            {
                return 1;
            }
            else if (lv1 > lv2)
            {
                return -1;
            }
            return 0;
        });
        return petList;
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
                    if (data is PetTujianTitleItem)
                    {
                        PetTujianTitleItem item = data as PetTujianTitleItem;
                        if (item != null)
                        {
                            TujianBtnClick(item.gameObject);
                        }
                        if (m_ctor_tujiantitlescroll != null)
                        {
                            m_ctor_tujiantitlescroll.SetSelect(item);
                        }
                    }

                }
                break;
        }
    }
    void RefreshPetScrollItem(PetScrollItem petitem, int index)
    {
        List<IPet> petList = petDataManager.GetSortPetList();
        int petcount = petList.Count;
        int i = index;

        if (petitem != null)
        {
            petitem.gameObject.SetActive(true);
            if (i < petList.Count)
            {
                IPet pet = petList[i];
                petitem.UpdatePetItemData(pet);
            }


            int maxPet = petDataManager.maxPetNum;
            int leftCount = maxPet - petcount;
            if (leftCount < 0)
            {
                leftCount = 0;
            }
            if (i < maxPet && i >= petcount)
            {
                petitem.UpdatePetItemData(null, PetScrollItem.PetItemShowType.Add);
            }
            int lockIndex = maxPet;
            int petMaxConfig = GameTableManager.Instance.GetGlobalConfig<int>("MaxPetNum");
            if (i < petMaxConfig && i >= maxPet)
            {

                petitem.UpdatePetItemData(null, PetScrollItem.PetItemShowType.Lock);
            }
            if (petcount == 0)
            {

                petDataManager.CurPetThisID = 0;
            }
        }
    }
    void OnUpdateUIGrid(UIGridBase grid, int index)
    {
        if (grid is PetScrollItem)
        {
            PetScrollItem item = grid as PetScrollItem;
            RefreshPetScrollItem(item, index);
        }
        else if (grid is PetTujianItem)
        {
            PetTujianItem item = grid as PetTujianItem;
            if (item != null)
            {
                item.gameObject.SetActive(true);
                if (index < petDataManager.PetTuJianList.Count)
                {
                    PetDataBase db = petDataManager.PetTuJianList[index];
                    if (db != null)
                    {
                        item.UpdateData(db);
                    }
                }
            }

        }
        else if (grid is PetTujianTitleItem)
        {
            PetTujianTitleItem item = grid as PetTujianTitleItem;
            if (item != null)
            {
                item.gameObject.SetActive(true);
                string str = GetSortTitleByIndex(index);
                item.SetText(str);
            }
        }

    }

    private bool m_bInitPetSettingCreator = false;
    void UpdatePetScollView()
    {
        if (null != m_ctor_petscrollview)
        {
            if (!m_bInitPetSettingCreator)
            {
                m_bInitPetSettingCreator = true;
                GameObject go = m_widget_leftcontent.transform.Find("petitem").gameObject;
                go.SetActive(false);
                m_ctor_petscrollview.RefreshCheck();
                m_ctor_petscrollview.Initialize<PetScrollItem>(go, OnUpdateUIGrid, OnUIGridEventDlg);
            }

            List<IPet> petList = petDataManager.GetSortPetList();
            int petcount = petList.Count;
            int maxPet = petDataManager.maxPetNum;
            int petMaxConfig = GameTableManager.Instance.GetGlobalConfig<int>("MaxPetNum");
            int createCount = maxPet + 1;

            createCount = createCount > petMaxConfig ? petMaxConfig : createCount;
            m_ctor_petscrollview.CreateGrids(createCount);
        }
    }

    #endregion


    public override bool OnMsg(UIMsgID msgid, object param)
    {
        if (msgid == UIMsgID.eShowUI)
        {
            if (param is ReturnBackUIMsg)
            {
                ReturnBackUIMsg showInfo = (ReturnBackUIMsg)param;
                if (showInfo.tabs.Length > 0)
                {
                    UIFrameManager.Instance.OnCilckTogglePanel(this.PanelId, 1, showInfo.tabs[0]);
                    //ShowRightBtnContent((PetPanelSubFlag)showInfo.tabs[0]);
                }

                for (int i = 0; i < m_ctor_petscrollview.transform.childCount; i++)
                {
                    var item = m_ctor_petscrollview.transform.GetChild(i);
                    PetScrollItem petItem = item.GetComponent<PetScrollItem>();
                    if (petItem != null && petItem.PetData != null && petItem.PetData.GetID() == (uint)showInfo.param)
                    {
                        petItem.gameObject.SendMessage("OnClick", SendMessageOptions.RequireReceiver);
                    }
                }
            }

        }
        return base.OnMsg(msgid, param);
    }
    void onClick_Pettips5_Btn(GameObject caster)
    {

    }
    void onClick_Pettips4_Btn(GameObject caster)
    {

    }
    void onClick_Pettips3_Btn(GameObject caster)
    {

    }
    void onClick_Pettips2_Btn(GameObject caster)
    {

    }
    void onClick_Pettips1_Btn(GameObject caster)
    {

    }
    CMResAsynSeedData<CMTexture> m_curMedicanIconAsynSeed = null;
    void OnShuxingUIRelease()
    {
        if(m_curMedicanIconAsynSeed != null)
        {
            m_curMedicanIconAsynSeed.Release();
            m_curMedicanIconAsynSeed = null;
        }
    }
    public override void Release(bool depthRelease = true)
    {
        base.Release(depthRelease);
        if(shuxinggrop != null)
        {
            shuxinggrop.Release();
        }
        if(jinenggrop != null)
        {
            jinenggrop.Release();
        }
        OnShuxingUIRelease();
        OnGuiYuanUIRelease();
        OnInheritUIRelease();
        OnYinHunUIRelease();
    }

 
}
