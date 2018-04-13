//=========================================================================================
//
//    Author: zhudianyu
//    宠物属性添加界面处理
//    CreateTime:  2016/06/24
//
//=========================================================================================
using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using Client;
using table;
using Engine.Utility;
using GameCmd;
partial class PetAddPointPanel
{
    PetDataManager petDataManager
    {
        get
        {
            return DataManager.Manager<PetDataManager>();
        }
    }
    /// <summary>
    /// 宠物属性相关的label
    /// </summary>
    Dictionary<string, UILabel> labelDic = new Dictionary<string, UILabel>();
    Dictionary<string, UILabel> labelAddDic = new Dictionary<string, UILabel>();
    Dictionary<string, UISlider> sliderDic = new Dictionary<string, UISlider>();
    Dictionary<string, UILabel> labelAddPointDic = new Dictionary<string, UILabel>();
    /// <summary>
    /// 要增加的点数
    /// </summary>
    Dictionary<string, int> addPointDic = new Dictionary<string, int>();
    Dictionary<string, float> finishSliderDic = new Dictionary<string, float>();
    uint PetThisID;
    IPet curPet;
    public IPet CurPet
    {
        get
        {
            return petDataManager.CurPet;
        }
    }
    int attrLeftPoint = 0;
    int LeftPoint
    {
        get
        {
            return attrLeftPoint;
        }
        set
        {
            if (value < 0)
                return;
            attrLeftPoint = value;
            m_label_jiadian_point.text = attrLeftPoint.ToString();
        }
    }
    #region panelbase overide
    protected override void OnLoading()
    {
        base.OnLoading();
        if (CurPet == null)
            return;


        InitLabel();
        sliderDic.Add(FightCreatureProp.Strength.ToString(), m_slider_jiadian_liliang);
        sliderDic.Add(FightCreatureProp.Corporeity.ToString(), m_slider_jiadian_tili);
        sliderDic.Add(FightCreatureProp.Intelligence.ToString(), m_slider_jiadian_zhili);
        sliderDic.Add(FightCreatureProp.Spirit.ToString(), m_slider_jiadian_jingshen);
        sliderDic.Add(FightCreatureProp.Agility.ToString(), m_slider_jiadian_minjie);

        addPointDic.Add(FightCreatureProp.Strength.ToString(), 0);
        addPointDic.Add(FightCreatureProp.Corporeity.ToString(), 0);
        addPointDic.Add(FightCreatureProp.Intelligence.ToString(), 0);
        addPointDic.Add(FightCreatureProp.Spirit.ToString(), 0);
        addPointDic.Add(FightCreatureProp.Agility.ToString(), 0);

        finishSliderDic.Add(FightCreatureProp.Strength.ToString(), 0);
        finishSliderDic.Add(FightCreatureProp.Corporeity.ToString(), 0);
        finishSliderDic.Add(FightCreatureProp.Intelligence.ToString(), 0);
        finishSliderDic.Add(FightCreatureProp.Spirit.ToString(), 0);
        finishSliderDic.Add(FightCreatureProp.Agility.ToString(), 0);
        foreach (var dic in sliderDic)
        {
            UISlider slider = dic.Value;
            Transform lightblue = slider.transform.Find("lightblue");
            Transform thumb = slider.transform.Find("Thumb");
            PetAttrSlider lightblueSlider = lightblue.GetComponent<PetAttrSlider>();
            if (lightblueSlider == null)
            {
                lightblueSlider = lightblue.gameObject.AddComponent<PetAttrSlider>();
                lightblueSlider.foregroundWidget = lightblue.GetComponent<UIWidget>();
                lightblueSlider.thumb = thumb;
            }
            uint initbaseValue = petDataManager.GetCurPetBaseProp(dic.Key, true);
            int maxPoint = CurPet.GetProp((int)PetProp.MaxPoint);
            LeftPoint = maxPoint;
            int inittotalValue = (int)initbaseValue + maxPoint;
            int initattrPoint = GetAttrPoint(dic.Key);
            float initsliderValue = (initbaseValue + initattrPoint) * 1.0f / inittotalValue;

            if (lightblueSlider != null)
            {

                lightblueSlider.InitData(initsliderValue, dic.Key);
                lightblueSlider.value = initsliderValue;
                //lightblueSlider.numberOfSteps =(int) (initbaseValue + initattrPoint);
                lightblueSlider.onDragFinished = () =>
                {

                    float fs = GetCurSliderValue(lightblueSlider.propStr);
                    lightblueSlider.value = fs;
                    finishSliderDic[lightblueSlider.propStr] = fs;
                };
                EventDelegate.Add(lightblueSlider.onChange, () =>
                {

                    maxPoint = CurPet.GetProp((int)PetProp.MaxPoint);
                    uint baseValue = petDataManager.GetCurPetBaseProp(lightblueSlider.propStr, true);
                    int totalValue = (int)baseValue + maxPoint;
                    string propstr = lightblueSlider.propStr;

                    float fs = lightblueSlider.value;
                    float tempValue = fs;
                    float oldValue = finishSliderDic[propstr];
                    if (tempValue > oldValue)
                    {
                        float deltaValue = tempValue - oldValue;
                        int delta = Mathf.RoundToInt(deltaValue * totalValue);
                        if (delta >= 1)
                        {
                            if (delta > LeftPoint)
                            {
                                delta = LeftPoint;
                                float sliderAddDelta = delta * 1.0f / totalValue;
                            }
                            if (!AddPoint(propstr, delta, true))
                            {
                                // lightblueSlider.value = oldValue;
                            }
                            else
                            {
                                tempValue = oldValue + delta * 1.0f / totalValue;
                                finishSliderDic[propstr] = tempValue;
                            }
                        }

                    }
                    if (tempValue < oldValue)
                    {
                        float deltaValue = oldValue - tempValue;
                        int delta = Mathf.RoundToInt(deltaValue * totalValue);
                        if (delta >= 1)
                        {
                            int leftAddPoint = addPointDic[lightblueSlider.propStr];
                            if (delta > leftAddPoint)
                            {
                                delta = leftAddPoint;
                            }
                            if (delta <= 0)
                            {
                                return;
                            }
                            if (!DeletePoint(propstr, delta, true))
                            {
                                // lightblueSlider.value = oldValue;
                            }
                            else
                            {
                                tempValue = oldValue - delta * 1.0f / totalValue;
                                finishSliderDic[propstr] = tempValue;
                            }
                        }

                    }

                });


            }

        }
    }
    float GetCurSliderValue(string prop)
    {
        int maxPoint = CurPet.GetProp((int)PetProp.MaxPoint);
        UILabel addLabel = labelAddPointDic[prop];
        uint baseValue = petDataManager.GetCurPetBaseProp(prop, true);


        int totalValue = (int)baseValue + maxPoint;
        int attrPoint = GetAttrPoint(prop);
        int leftAddPoint = addPointDic[prop];
        float fs = (leftAddPoint + attrPoint + baseValue) * 1.0f / totalValue;
        return fs;
    }

    void InitSliderData()
    {
        foreach (var dic in sliderDic)
        {
            uint baseValue = petDataManager.GetCurPetBaseProp(dic.Key);
            int maxPoint = CurPet.GetProp((int)PetProp.MaxPoint);
            int attrPoint = GetAttrPoint(dic.Key);
            if (baseValue > attrPoint)
            {
                baseValue = baseValue - (uint)attrPoint;
            }
            else
            {
                Log.Error("基础值小于添加值 不科学");
            }
            int totalValue = (int)baseValue + maxPoint;

            UISlider slider = dic.Value;
            Transform green = slider.transform.Find("green");
            UISlider greenSlider = slider;
            if (greenSlider != null)
            {
                float sliderValue = baseValue * 1.0f / totalValue;
                greenSlider.value = sliderValue;

            }
            Transform blue = slider.transform.Find("blue");
            UISlider blueSlider = blue.GetComponent<UISlider>();
            if (blueSlider != null)
            {

                float sliderValue = (baseValue + attrPoint) * 1.0f / totalValue;
                blueSlider.value = sliderValue;
            }
            Transform lightblue = slider.transform.Find("lightblue");
            PetAttrSlider lightblueSlider = lightblue.GetComponent<PetAttrSlider>();
            if (lightblueSlider != null)
            {
                int addPoint = addPointDic[dic.Key];
                float sliderValue = (baseValue + attrPoint + addPoint) * 1.0f / totalValue;

                finishSliderDic[dic.Key] = sliderValue;
                lightblueSlider.value = sliderValue;
            }


        }
    }
    void InitData()
    {
        if (CurPet != null)
        {
            m_label_jiadian_petshowname.text = petDataManager.GetCurPetName();
            m_label_jiadian_level.text = petDataManager.GetCurPetLevelStr();
            addPointDic[FightCreatureProp.Strength.ToString()] = 0;
            addPointDic[FightCreatureProp.Corporeity.ToString()] = 0;
            addPointDic[FightCreatureProp.Intelligence.ToString()] = 0;
            addPointDic[FightCreatureProp.Spirit.ToString()] = 0;
            addPointDic[FightCreatureProp.Agility.ToString()] = 0;
            foreach (var labelPair in labelDic)
            {
                UILabel label = labelPair.Value;
                label.text = petDataManager.GetPropByString(labelPair.Key).ToString();
                if (labelPair.Key == PetProp.MaxPoint.ToString())
                {
                    int maxPoint = CurPet.GetProp((int)PetProp.MaxPoint);
                    PetTalent attrPoint = CurPet.GetAttrTalent();
                    uint usePint = attrPoint.jingshen + attrPoint.liliang + attrPoint.minjie + attrPoint.zhili + attrPoint.tizhi;
                    int leftPoint = maxPoint - (int)usePint;
                    LeftPoint = leftPoint;
                }
            }


            PetDataBase pdb = petDataManager.GetPetDataBase(CurPet.PetBaseID);
            if (pdb != null)
            {
                UIManager.GetTextureAsyn(pdb.icon, ref m_curIconAsynSeed, () =>
                {
                    if (null != m__jiadian_icon)
                    {
                        m__jiadian_icon.mainTexture = null;
                    }
                }, m__jiadian_icon, false);
            }

            // LeftPoint = CurPet.GetProp( (int)PetProp.MaxPoint );
            InitAddLabelData();
            InitAddPointLabelData();
            InitSliderData();
        }

    }
    CMResAsynSeedData<CMTexture> m_curIconAsynSeed = null;
    protected override void OnPanelBaseDestory()
    {
        base.OnPanelBaseDestory();
    }
    public override void Release(bool depthRelease = true)
    {
        base.Release(depthRelease);
        if(null != m_curIconAsynSeed)
        {
            m_curIconAsynSeed.Release();
            m_curIconAsynSeed = null;
        }
    }
    protected override void OnHide()
    {
        base.OnHide();
        petDataManager.ValueUpdateEvent -= OnValueUpdateEventArgs;
        Release();

    }
    protected override void OnShow(object data)
    {
        base.OnShow(data);
        if (CurPet == null)
            return;
        petDataManager.ValueUpdateEvent += OnValueUpdateEventArgs;
        InitData();
        PetDataBase db = petDataManager.GetPetDataBase(CurPet.PetBaseID);
        if (db != null)
        {
            m_label_attackType.text = db.attackType;
        }
    }

    #endregion
    #region 属性改变
    void OnValueUpdateEventArgs(object obj, ValueUpdateEventArgs v)
    {
        if (v != null)
        {
            if (v.key == PetDispatchEventString.PetRefreshProp.ToString())
            {
                InitData();
            }
        }
    }

    #endregion
    void InitLabel()
    {

        labelDic.Add(CreatureProp.MaxHp.ToString(), m_label_jiadian_qixue);
        labelDic.Add(FightCreatureProp.PhysicsAttack.ToString(), m_label_jiadian_wuligongji);
        labelDic.Add(FightCreatureProp.PhysicsDefend.ToString(), m_label_jiadian_wulifangyu);
        labelDic.Add(FightCreatureProp.MagicAttack.ToString(), m_label_jiadian_fashugongji);
        labelDic.Add(FightCreatureProp.MagicDefend.ToString(), m_label_jiadian_fashufangyu);
        labelDic.Add(FightCreatureProp.Hit.ToString(), m_label_jiadian_mingzhong);
        labelDic.Add(FightCreatureProp.Dodge.ToString(), m_label_jiadian_shanbi);
        labelDic.Add(FightCreatureProp.PhysicsCrit.ToString(), m_label_jiadian_wulizhimingyiji);
        labelDic.Add(FightCreatureProp.MagicCrit.ToString(), m_label_jiadian_fashuzhimingyiji);
        labelDic.Add(FightCreatureProp.Strength.ToString(), m_label_jiadian_liliangnowpoint);
        labelDic.Add(FightCreatureProp.Corporeity.ToString(), m_label_jiadian_tilinowpoint);
        labelDic.Add(FightCreatureProp.Intelligence.ToString(), m_label_jiadian_zhilinowpoint);
        labelDic.Add(FightCreatureProp.Agility.ToString(), m_label_jiadian_minjienowpoint);
        labelDic.Add(FightCreatureProp.Spirit.ToString(), m_label_jiadian_jingshennowpoint);//
        labelDic.Add(PetProp.MaxPoint.ToString(), m_label_jiadian_point);

        labelAddDic.Add(CreatureProp.MaxHp.ToString(), m_label_jiadian_qixueadd);
        labelAddDic.Add(FightCreatureProp.PhysicsAttack.ToString(), m_label_jiadian_wugongadd);
        labelAddDic.Add(FightCreatureProp.PhysicsDefend.ToString(), m_label_jiadian_wufangadd);
        labelAddDic.Add(FightCreatureProp.MagicAttack.ToString(), m_label_jiadian_fagongadd);
        labelAddDic.Add(FightCreatureProp.MagicDefend.ToString(), m_label_jiadian_fafangadd);
        labelAddDic.Add(FightCreatureProp.Hit.ToString(), m_label_jiadian_mingzhongadd);
        labelAddDic.Add(FightCreatureProp.Dodge.ToString(), m_label_jiadian_shanbiadd);
        labelAddDic.Add(FightCreatureProp.PhysicsCrit.ToString(), m_label_jiadian_wuzhiadd);
        labelAddDic.Add(FightCreatureProp.MagicCrit.ToString(), m_label_jiadian_fazhiadd);

        labelAddPointDic.Add(FightCreatureProp.Strength.ToString(), m_label_jiadian_liliangaddpoint);
        labelAddPointDic.Add(FightCreatureProp.Corporeity.ToString(), m_label_jiadian_tiliaddpoint);
        labelAddPointDic.Add(FightCreatureProp.Spirit.ToString(), m_label_jiadian_jingshenaddpoint);
        labelAddPointDic.Add(FightCreatureProp.Agility.ToString(), m_label_jiadian_minjieaddpoint);
        labelAddPointDic.Add(FightCreatureProp.Intelligence.ToString(), m_label_jiadian_zhiliaddpoint);

    }
    /*
     	力量天赋系数计算公式
力量天赋系数 = （力量天赋 / 10000 + 1）* 100%
	体质天赋系数计算公式
体质天赋系数 = （体质天赋 / 10000 + 1）* 100%
	敏捷天赋系数计算公式
敏捷天赋系数 = （敏捷天赋 / 10000 + 1）* 100%
	智力天赋系数计算公式
智力天赋系数 = （智力天赋 / 10000 + 1）* 100%
	精神天赋系数计算公式
精神天赋系数 = （精神天赋 / 10000 + 1）* 100%

     */
    void InitAddLabelData()
    {
        if (CurPet != null)
        {
            float liliangfactor = ((float)(CurPet.GetProp((int)PetProp.StrengthTalent) * 1.0f / 10000) + 1);
            float tizhifactor = ((float)(CurPet.GetProp((int)PetProp.CorporeityTalent) * 1.0f / 10000) + 1);
            float minjiefactor = ((float)(CurPet.GetProp((int)PetProp.AgilityTalent) * 1.0f / 10000) + 1);
            float zhilifactor = ((float)(CurPet.GetProp((int)PetProp.IntelligenceTalent) * 1.0f / 10000) + 1);
            float jingshenfactor = ((float)(CurPet.GetProp((int)PetProp.SpiritTalent) * 1.0f / 10000) + 1);
            int level = petDataManager.GetCurPetLevel();
            //成长属性系数
            List<float> gradeAttr = GameTableManager.Instance.GetGlobalConfigList<float>("GradeAttr");
            int state = CurPet.GetProp((int)PetProp.PetGuiYuanStatus) - 1;

            float growFactor = gradeAttr[state];
            //成长潜修系数
            List<float> gradeArg = GameTableManager.Instance.GetGlobalConfigList<float>("GradeArg");

            float qianxiuFactor = gradeArg[state];
            PetUpGradeDataBase db = GameTableManager.Instance.GetTableItem<PetUpGradeDataBase>((uint)CurPet.PetBaseID, level);
            foreach (var dic in labelAddDic)
            {
                UILabel label = dic.Value;
                int oldValue = petDataManager.GetPropByString(dic.Key);
                string newStr = "";
                int newValue = 0;
                if (dic.Key == CreatureProp.MaxHp.ToString())
                {//生命基础值 + 体质基础值 * 体质天赋系数 * 成长属性系数 * 体质生命转换系数
                    int baseValue = addPointDic[FightCreatureProp.Corporeity.ToString()];
                    float factor = GameTableManager.Instance.GetGlobalConfig<float>("Tizhi2HP");
                    newValue = (int)( /*db.maxHp+*/baseValue * tizhifactor * growFactor * factor);
                    newStr = newValue.ToString();
                }
                else if (dic.Key == FightCreatureProp.PhysicsAttack.ToString())
                {//b)	物理攻击 = 力量基础值 * 力量天赋系数 * 成长属性系数 * 力量物攻转换系数
                    int baseValue = addPointDic[FightCreatureProp.Strength.ToString()];
                    float factor = GameTableManager.Instance.GetGlobalConfig<float>("Lilang2Pdam");
                    newValue = (int)(baseValue * liliangfactor * growFactor * factor);
                    newStr = newValue.ToString();
                }
                else if (dic.Key == FightCreatureProp.PhysicsDefend.ToString())
                {//b)	物理防御 = 体质基础值 * 体质天赋系数 * 成长属性系数 * 体质物防转换系数
                    int baseValue = addPointDic[FightCreatureProp.Corporeity.ToString()];
                    float factor = GameTableManager.Instance.GetGlobalConfig<float>("Tizhi2Pdef");
                    newValue = (int)(baseValue * tizhifactor * growFactor * factor);
                    newStr = newValue.ToString();
                }
                else if (dic.Key == FightCreatureProp.MagicAttack.ToString())
                {//b)	法术攻击 = 智力基础值 * 智力天赋系数 * 成长属性系数 * 智力法攻转换系数
                    int baseValue = addPointDic[FightCreatureProp.Intelligence.ToString()];
                    float factor = GameTableManager.Instance.GetGlobalConfig<float>("Zhili2Mdam");
                    newValue = (int)(baseValue * zhilifactor * growFactor * factor);
                    newStr = newValue.ToString();
                }
                else if (dic.Key == FightCreatureProp.MagicDefend.ToString())
                {//b)	法术防御 = 精神基础值 * 精神天赋系数 * 成长属性系数 * 精神法防转换系
                    int baseValue = addPointDic[FightCreatureProp.Spirit.ToString()];
                    float factor = GameTableManager.Instance.GetGlobalConfig<float>("Jingshen2Mdef");
                    newValue = (int)(baseValue * jingshenfactor * growFactor * factor);
                    newStr = newValue.ToString();
                }
                else if (dic.Key == FightCreatureProp.Hit.ToString())
                {//d)	命中值 =（命中初始加成值 + 敏捷基础值 * 敏捷命中转换系数 * 敏捷天赋系数）* 命中修正系)
                    //	命中值 =（命中初始加成值 + 敏捷基础值 * 敏捷命中转换系数 * 敏捷天赋系数） 新公式 like说不乘修正系数
                    int baseValue1 = addPointDic[FightCreatureProp.Agility.ToString()];
                    int baseValue2 = GameTableManager.Instance.GetGlobalConfig<int>("HitInit");
                    float factor = GameTableManager.Instance.GetGlobalConfig<float>("Minjie2Hit");
                    float factor2 = GameTableManager.Instance.GetGlobalConfig<float>("HitFix");
                    newValue = (int)((/* baseValue2 +*/ baseValue1 * factor * minjiefactor)*100);
                    newStr = newValue.ToString();
                }
                else if (dic.Key == FightCreatureProp.Dodge.ToString())
                {//d)	回避值 = （回避初始加成值 + 敏捷基础值 * 敏捷回避转换系数 * 敏捷天赋系数）* 回避修正系数
                    //回避值 = （回避初始加成值 + 敏捷基础值 * 敏捷回避转换系数 * 敏捷天赋系数）
                    int baseValue1 = addPointDic[FightCreatureProp.Agility.ToString()];
                    int baseValue2 = GameTableManager.Instance.GetGlobalConfig<int>("HideInit");
                    float factor = GameTableManager.Instance.GetGlobalConfig<float>("Minjie2Hide");
                    float factor2 = GameTableManager.Instance.GetGlobalConfig<float>("HideFix");
                    newValue = (int)(( /*baseValue2 +*/ baseValue1 * factor * minjiefactor) *100);
                    newStr = newValue.ToString();
                }
                else if (dic.Key == FightCreatureProp.PhysicsCrit.ToString())
                {//c)	物理致命几率 = （物理致命基础附加值 + 敏捷基础值 * 敏捷天赋系数 * 成长属性系数 * 敏捷物爆转换系数 / 100） * 100%
                    int baseValue1 = addPointDic[FightCreatureProp.Agility.ToString()];
                    double baseValue2 = GameTableManager.Instance.GetGlobalConfig<double>("PluckyInit");
                    float factor = GameTableManager.Instance.GetGlobalConfig<float>("Minjie2Plucky");
                    int nv = (int)(( /*baseValue2 +*/ baseValue1 * growFactor * minjiefactor * factor / 100) * 10000);
                    newStr = nv.ToString();
                }
                else if (dic.Key == FightCreatureProp.MagicCrit.ToString())
                {//c)	法术致命几率 = （法术致命基础附加值 + 智力基础值 * 智力天赋系数 * 成长属性系数 * 智力法爆转换系数 / 100） * 100%
                    int baseValue1 = addPointDic[FightCreatureProp.Intelligence.ToString()];
                    double baseValue2 = GameTableManager.Instance.GetGlobalConfig<double>("MluckyInit");
                    float factor = GameTableManager.Instance.GetGlobalConfig<float>("Zhili2Mlucky");
                    int nv = (int)(( /*baseValue2 +*/ baseValue1 * growFactor * zhilifactor * factor / 100) * 10000);
                    newStr = nv.ToString();
                }

                label.text = newStr;
            }
        }

    }
    /// <summary>
    /// 刷新将要增长的点数和原来点数之和
    /// </summary>
    void RefreshWillAddLabel()
    {
        foreach (var labelPair in labelDic)
        {
            UILabel label = labelPair.Value;
            int curProp = petDataManager.GetPropByString(labelPair.Key);
            if (addPointDic.ContainsKey(labelPair.Key))
            {
                int willAdd = addPointDic[labelPair.Key];
                label.text = (willAdd + curProp).ToString();
            }
        }
    }
    int GetAttrPoint(string prop)
    {
        if (CurPet == null)
            return 0;
        PetTalent attr = CurPet.GetAttrTalent();
        uint point = 0;
        if (prop == FightCreatureProp.Strength.ToString())
        {
            point = attr.liliang;
        }
        else if (prop == FightCreatureProp.Agility.ToString())
        {
            point = attr.minjie;
        }
        else if (prop == FightCreatureProp.Spirit.ToString())
        {
            point = attr.jingshen;
        }
        else if (prop == FightCreatureProp.Corporeity.ToString())
        {
            point = attr.tizhi;
        }
        else if (prop == FightCreatureProp.Intelligence.ToString())
        {
            point = attr.zhili;
        }
        return (int)point;
    }
    void InitAddPointLabelData()
    {
        if (CurPet != null)
        {
            foreach (var dic in labelAddPointDic)
            {
                UILabel label = dic.Value;
                int point = addPointDic[dic.Key];

                label.text = point.ToString();
            }
        }
    }
    bool AddPoint(string prop, int deltaPoint, bool isSlider = false)
    {
        UISlider slider = sliderDic[prop];
        Transform lightblue = slider.transform.Find("lightblue");
        PetAttrSlider lightblueSlider = lightblue.GetComponent<PetAttrSlider>();
        UILabel addLabel = null;
        if (labelAddPointDic.TryGetValue(prop, out addLabel))
        {
            uint baseValue = petDataManager.GetCurPetBaseProp(prop, true);

            int maxPoint = CurPet.GetProp((int)PetProp.MaxPoint);
            int totalValue = (int)baseValue + maxPoint;
            int attrPoint = GetAttrPoint(prop);
            int curPoint = (int)(lightblueSlider.value * totalValue);

            int point = 0;
            if (addPointDic.TryGetValue(prop, out point))
            {
                if (deltaPoint > LeftPoint)
                {
                    deltaPoint = LeftPoint;
                }
                float initValue = (baseValue + attrPoint + point) * 1.0f / totalValue;
                if (LeftPoint == 0)
                {
                    if (!isSlider)
                    {
                        TipsManager.Instance.ShowTips(DataManager.Manager<TextManager>().GetLocalText(LocalTextType.Local_TXT_Notice_NoEnoughQianXiuPoint));
                        return false;
                    }
                    Log.Error("没有潜能点可分配" + initValue.ToString());
                    LeftPoint = 0;
                    InitAddPointLabelData();
                    ///  lightblueSlider.value = initValue;
                    return false;
                }
                //Log.Error( " prop is " + prop + " totalvalue " + totalValue.ToString() );
                //Log.LogGroup( "ZDY" , "潜能点可分配" + deltaPoint.ToString() + " curPoint = " + curPoint.ToString() + " attrPoint = " + attrPoint.ToString() + " point = " + point.ToString() );
                addPointDic[prop] = point + deltaPoint;
                LeftPoint -= deltaPoint;
                InitAddLabelData();
                InitAddPointLabelData();
                RefreshWillAddLabel();

                return true;
            }
            else
            {
                Log.Error("labelAddPointDic dosen't contain key {0}", prop);
            }
        }
        Log.Error("labelAddPointDic dosen't contain key {0}", prop);
        return false;
    }
    void OnAddPoint(string prop)
    {
        AddPoint(prop, 1);
        InitSliderData();

    }
    bool DeletePoint(string prop, int deltaPoint, bool isSlider = false)
    {
        Log.Error("delete deltapoint is " + deltaPoint.ToString());
        UISlider slider = sliderDic[prop];
        Transform lightblue = slider.transform.Find("lightblue");
        PetAttrSlider lightblueSlider = lightblue.GetComponent<PetAttrSlider>();

        uint baseValue = petDataManager.GetCurPetBaseProp(prop, true);

        int maxPoint = CurPet.GetProp((int)PetProp.MaxPoint);
        int totalValue = (int)baseValue + maxPoint;
        int attrPoint = GetAttrPoint(prop);
        float initValue = (baseValue + attrPoint) * 1.0f / totalValue;
        if (lightblueSlider.value <= initValue)
        {
            if (!isSlider)
            {
                TipsManager.Instance.ShowTips("已经分配的潜能点不能重新分配");
                return false;
            }
        }
        int point = addPointDic[prop];
        if (deltaPoint > point)
        {
            deltaPoint = point;
        }
        addPointDic[prop] = point - deltaPoint;
        if (addPointDic[prop] == 0)
        {
            int a = 0;
        }
        LeftPoint += deltaPoint;
        InitAddLabelData();
        InitAddPointLabelData();
        RefreshWillAddLabel();
        return true;
    }
    void OnDeletePoint(string prop)
    {
        DeletePoint(prop, 1);
        InitSliderData();
    }
    void onClick_Jiadian_liliangless_Btn(GameObject caster)
    {
        OnDeletePoint(FightCreatureProp.Strength.ToString());
    }

    void onClick_Jiadian_liliangadd_Btn(GameObject caster)
    {
        OnAddPoint(FightCreatureProp.Strength.ToString());
    }

    void onClick_Jiadian_minjieless_Btn(GameObject caster)
    {
        OnDeletePoint(FightCreatureProp.Agility.ToString());
    }

    void onClick_Jiadian_minjieadd_Btn(GameObject caster)
    {
        OnAddPoint(FightCreatureProp.Agility.ToString());
    }

    void onClick_Jiadian_zhililess_Btn(GameObject caster)
    {
        OnDeletePoint(FightCreatureProp.Intelligence.ToString());
    }

    void onClick_Jiadian_zhiliadd_Btn(GameObject caster)
    {
        OnAddPoint(FightCreatureProp.Intelligence.ToString());
    }

    void onClick_Jiadian_tililess_Btn(GameObject caster)
    {
        OnDeletePoint(FightCreatureProp.Corporeity.ToString());

    }

    void onClick_Jiadian_tiliadd_Btn(GameObject caster)
    {
        OnAddPoint(FightCreatureProp.Corporeity.ToString());
    }

    void onClick_Jiadian_jingshenless_Btn(GameObject caster)
    {
        OnDeletePoint(FightCreatureProp.Spirit.ToString());
    }

    void onClick_Jiadian_jingshenadd_Btn(GameObject caster)
    {
        OnAddPoint(FightCreatureProp.Spirit.ToString());
    }
    bool bHasAddPoint()
    {
        bool bAdd = false;
        foreach (var dic in addPointDic)
        {
            if (dic.Value > 0)
            {
                bAdd = true;
            }
        }
        return bAdd;
    }
    void onClick_Chongzhishuxing_Btn(GameObject caster)
    {
        if (CurPet != null)
        {
            stAttrPointPetUserCmd_C cmd = new stAttrPointPetUserCmd_C();
            cmd.id = CurPet.GetID();
            PetTalent attr = CurPet.GetAttrTalent();
            PetTalent tal = new PetTalent();
            tal.tizhi = (uint)addPointDic[FightCreatureProp.Corporeity.ToString()] + attr.tizhi;
            tal.minjie = (uint)addPointDic[FightCreatureProp.Agility.ToString()] + attr.minjie;
            tal.jingshen = (uint)addPointDic[FightCreatureProp.Spirit.ToString()] + attr.jingshen;
            tal.zhili = (uint)addPointDic[FightCreatureProp.Intelligence.ToString()] + attr.zhili;
            tal.liliang = (uint)addPointDic[FightCreatureProp.Strength.ToString()] + attr.liliang;


            if (attr.tizhi == 0 && attr.minjie == 0 && attr.jingshen == 0 && attr.zhili == 0 && attr.liliang == 0)
            {
                TipsManager.Instance.ShowTips(DataManager.Manager<TextManager>().GetLocalText(LocalTextType.Local_TXT_Notice_AlreadyResetPoint));
                return;
            }
            else
            {
                int resetLevel = GameTableManager.Instance.GetGlobalConfig<int>("PetFreeResetPointCount");
                uint petLevel = CurPet.GetFreeResetAttrNum();
                if (petLevel < resetLevel)
                {
                    string con = DataManager.Manager<TextManager>().GetLocalFormatText(LocalTextType.Pet_Commond_chongzhixiaohaotips, resetLevel - petLevel);
                    //string con = string.Format("{0}{1}", resetLevel, CommonData.GetLocalString("级之前可免费重置潜修点，重置后所有潜修点都会返还。"));
                    TipsManager.Instance.ShowTipWindow(TipWindowType.Custom, con, () =>
                    {
                        NetService.Instance.Send(cmd);
                    }, null,null, CommonData.GetLocalString("重置潜修点"), CommonData.GetLocalString("重置"));
                }
                else
                {
                    uint resetItemID = GameTableManager.Instance.GetGlobalConfig<uint>("PetAddPointResetItemID");
                    CommonSingleParam cp = new CommonSingleParam();
                    cp.consumNum = 1;
                    cp.autobuydes = CommonData.GetLocalString("道具不足自动元宝购买");
                    cp.titletips = CommonData.GetLocalString("提示");
                    cp.contentdes = CommonData.GetLocalString("重置潜修点需要消耗如下道具");
                    cp.bShowAutoBuy = true;
                    cp.canceltext = CommonData.GetLocalString("取消");
                    cp.oktext = CommonData.GetLocalString("重置");
                    cp.itemID = resetItemID;
                    cp.cancleAction = null;
                    cp.okAction = RsetPoint;
                    TipsManager.Instance.ShowSingelConsumPanel(cp);
                }
            }
        }

    }

    void onClick_Quedingjiadian_Btn(GameObject caster)
    {
        if (CurPet != null)
        {

            stAttrPointPetUserCmd_C cmd = new stAttrPointPetUserCmd_C();
            cmd.id = CurPet.GetID();
            PetTalent attr = CurPet.GetAttrTalent();
            PetTalent tal = new PetTalent();
            tal.tizhi = (uint)addPointDic[FightCreatureProp.Corporeity.ToString()] + attr.tizhi;
            tal.minjie = (uint)addPointDic[FightCreatureProp.Agility.ToString()] + attr.minjie;
            tal.jingshen = (uint)addPointDic[FightCreatureProp.Spirit.ToString()] + attr.jingshen;
            tal.zhili = (uint)addPointDic[FightCreatureProp.Intelligence.ToString()] + attr.zhili;
            tal.liliang = (uint)addPointDic[FightCreatureProp.Strength.ToString()] + attr.liliang;
            cmd.attr_point = tal;
            if (!bHasAddPoint())
            {
                TipsManager.Instance.ShowTips(DataManager.Manager<TextManager>().GetLocalText(LocalTextType.Local_TXT_Notice_NoAllotQianXiuPoint));
                return;
            }
            else
            {
                NetService.Instance.Send(cmd);
            }
        }
    }
    void RsetPoint(bool bReset)
    {

        if (CurPet != null)
        {
            stAttrPointPetUserCmd_C cmd = new stAttrPointPetUserCmd_C();
            cmd.id = CurPet.GetID();
            cmd.auto_buy = bReset;
            PetTalent attr = CurPet.GetAttrTalent();
            PetTalent tal = new PetTalent();
            tal.tizhi = (uint)addPointDic[FightCreatureProp.Corporeity.ToString()] + attr.tizhi;
            tal.minjie = (uint)addPointDic[FightCreatureProp.Agility.ToString()] + attr.minjie;
            tal.jingshen = (uint)addPointDic[FightCreatureProp.Spirit.ToString()] + attr.jingshen;
            tal.zhili = (uint)addPointDic[FightCreatureProp.Intelligence.ToString()] + attr.zhili;
            tal.liliang = (uint)addPointDic[FightCreatureProp.Strength.ToString()] + attr.liliang;


            if (attr.tizhi == 0 && attr.minjie == 0 && attr.jingshen == 0 && attr.zhili == 0 && attr.liliang == 0)
            {
                TipsManager.Instance.ShowTips(DataManager.Manager<TextManager>().GetLocalText(LocalTextType.Local_TXT_Notice_AlreadyResetPoint));
                return;
            }
            else
            {
                NetService.Instance.Send(cmd);
            }
        
        }

    }
    void onClick_Jiadianfangan_Btn(GameObject caster)
    {
        DataManager.Manager<UIPanelManager>().ShowPanel(PanelID.PetAddPointPlanPanel);
    }

    void onClick_Help_Btn(GameObject caster)
    {
        DataManager.Manager<UIPanelManager>().ShowPanel(PanelID.AddpointExplanationPanel);
    }

}
