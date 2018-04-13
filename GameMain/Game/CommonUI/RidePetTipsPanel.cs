using System;
using System.Collections.Generic;
using table;
using UnityEngine;

partial class RidePetTipsPanel : UIPanelBase
{
    GameCmd.ItemSerialize m_itemServer;
    UILabel[] skillLabels = null;
    UILabel[] telentLabels = null;
    protected override void OnLoading()
    {
        base.OnLoading();
        int labelNum = 6;
        skillLabels = new UILabel[labelNum];
        for (int i = 1; i <= labelNum; i++)
        {
            skillLabels[i - 1] = m_trans_skilllabels.Find("Label" + i.ToString()).GetComponent<UILabel>();
            skillLabels[i - 1].enabled = false;
            skillLabels[i - 1].overflowMethod = UILabel.Overflow.ResizeFreely;
        }

        telentLabels = new UILabel[labelNum];
        for (int i = 1; i <= 6; i++)
        {
            telentLabels[i - 1] = m_trans_talentlabels.Find("Label" + i.ToString()).GetComponent<UILabel>();
            telentLabels[i - 1].enabled = false;
            telentLabels[i - 1].overflowMethod = UILabel.Overflow.ResizeFreely;
        }
    }

    protected override void OnShow(object data)
    {
        base.OnShow(data);

        if (data is GameCmd.ItemSerialize)
        {
            m_itemServer = (GameCmd.ItemSerialize)data;
            int type = 0; //1 ride 2 pet
            uint level = 0;
            uint baseId = 0;
            for (int i = 0; i < m_itemServer.numbers.Count; i++)
            {
                if (m_itemServer.numbers[i].id == (uint)GameCmd.eItemAttribute.Item_Attribute_Ride_Level)
                {
                    level = m_itemServer.numbers[i].value;
                    type = 1;
                }
                else if (m_itemServer.numbers[i].id == (uint)GameCmd.eItemAttribute.Item_Attribute_Ride_Base_Id)
                {
                    baseId = m_itemServer.numbers[i].value;
                }
                else if (m_itemServer.numbers[i].id == (uint)GameCmd.eItemAttribute.Item_Attribute_Pet_Lv)
                {
                    level = m_itemServer.numbers[i].value;
                    type = 2;
                }
                else if (m_itemServer.numbers[i].id == (uint)GameCmd.eItemAttribute.Item_Attribute_Pet_Base_Id)
                {
                    baseId = m_itemServer.numbers[i].value;
                }
            }

            m_label_level.text = string.Format("{0}级", level.ToString());

            if (type == 1)
            {
                m_sprite_bg.height = 286;
                m_label_Label_3.enabled = false;
                m_widget_talent.alpha = 0f;
                m_widget_skil.transform.localPosition = new UnityEngine.Vector3(m_widget_skil.transform.localPosition.x, 53, 0);

                OnRideUI(baseId, level, skillLabels);
            }
            else
            {
                m_widget_skil.transform.localPosition = new UnityEngine.Vector3(m_widget_skil.transform.localPosition.x, -59, 0);
                m_widget_talent.alpha = 1f;
                m_label_Label_3.enabled = true;

                m_sprite_bg.height = 380;
                ShowPetUI();
            }
        }
    }
    CMResAsynSeedData<CMTexture> m_curIconAsynSeed = null;
    CMResAsynSeedData<CMAtlas> m_curBordernAsynSeed = null;
    void ShowPetUI()
    {
        int labelNum = 6;
        UILabel[] labels = new UILabel[labelNum];
        for (int i = 1; i <= 6; i++)
        {
            labels[i - 1] = m_trans_skilllabels.Find("Label" + i.ToString()).GetComponent<UILabel>();
            labels[i - 1].enabled = false;
        }
        for (int i = 1; i <= 6; i++)
        {
            labels[i - 1] = m_trans_talentlabels.Find("Label" + i.ToString()).GetComponent<UILabel>();
            labels[i - 1].enabled = false;
        }
        for (int i = 0; i < m_itemServer.numbers.Count; i++)
        {
            GameCmd.PairNumber pn = m_itemServer.numbers[i];
            GameCmd.eItemAttribute bute = (GameCmd.eItemAttribute)pn.id;
            switch (bute)
            {
                case GameCmd.eItemAttribute.Item_Attribute_Pet_Lv:
                    m_label_level.text = string.Format("{0}{1}", pn.value.ToString(), CommonData.GetLocalString("级"));
                    break;
                case GameCmd.eItemAttribute.Item_Attribute_Pet_Life:
                    {
                        m_label_petLift.gameObject.SetActive(true);
                        string showStr = string.Format("{0} {1}", CommonData.GetLocalString("寿命:"), pn.value.ToString());
                        m_label_petLift.text = ColorManager.GetColorString(252, 230, 188, 255, showStr); 
                    }            
                    break;
                case GameCmd.eItemAttribute.Item_Attribute_Pet_Base_Id:
                    {
                        PetDataBase db = GameTableManager.Instance.GetTableItem<PetDataBase>(pn.value);
                        if (db != null)
                        {
                            m_label_Label_2.text = CommonData.GetLocalString("携带等级") + "  " + db.carryLevel;
                            m_label_name.text = db.petName;

                          //  DataManager.Manager<UIManager>().SetSpriteDynamicIcon(m_sprite_icon, db.icon, true);

                            UIManager.GetTextureAsyn(db.icon, ref m_curIconAsynSeed, () =>
                            {

                                if (null != m__icon)
                                {
                                    m__icon.mainTexture = null;
                                }
                            }, m__icon);

                            if (null != m_sprite_qulity)
                            {
                                UIManager.GetQualityAtlasAsyn(db.petQuality, ref m_curBordernAsynSeed, () =>
                                {
                                    if (null != m_sprite_qulity)
                                    {
                                        m_sprite_qulity.atlas = null;
                                    }
                                }, m_sprite_qulity);
                            }
                            m_label_Label_3.text = string.Format("{0}{1}", CommonData.GetLocalString("类型："), db.attackType);
                        }
                    }
                    break;
                case GameCmd.eItemAttribute.Item_Attribute_Pet_Grade:
                    {
                        string st = DataManager.Manager<PetDataManager>().GetGrowStatus((int)pn.value);
                        string showStr = string.Format("{0}  {1}", CommonData.GetLocalString("成长状态"), st);
                        m_label_petGradeValue.text = ColorManager.GetColorString(252, 230, 188, 255, showStr);
                    }
                    break;
                case GameCmd.eItemAttribute.Item_Attribute_Pet_Yh_Lv:
                    {
                        uint petYhLv = pn.value;
                        string showStr = string.Format("{0}  {1}{2}", CommonData.GetLocalString("修为"), petYhLv, CommonData.GetLocalString("级"));
                        m_label_petYhLv.text = ColorManager.GetColorString(252, 230, 188, 255, showStr);
                    }
                    break;
                case GameCmd.eItemAttribute.Item_Attribute_Pet_Character:
                    {
                        uint petCharacter = pn.value;
                        string cha = DataManager.Manager<PetDataManager>().GetPetCharacterStr((int)petCharacter);
                        string showStr = string.Format("{0} {1}", CommonData.GetLocalString("性格:"), cha);
                        m_label_petCharacter.text = ColorManager.GetColorString(252, 230, 188, 255, showStr);
                    }
                    break;
                case GameCmd.eItemAttribute.Item_Attribute_Pet_By_Lv:
                    {
                        uint variableLevel = pn.value;
                        string cha = DataManager.Manager<PetDataManager>().GetJieBianString((int)variableLevel, false);
                        string showStr = string.Format("{0} {1}", CommonData.GetLocalString("劫变:"), cha);
                        m_label_variableLevel.text = ColorManager.GetColorString(252, 230, 188, 255, showStr);
                    }
                    break;
                case GameCmd.eItemAttribute.Item_Attribute_Pet_Inherit_time:
                    {
                        uint InheritingNumber = pn.value;
                        string showStr = string.Format("{0} {1}", CommonData.GetLocalString("传承次数:"), InheritingNumber);
                        m_label_InheritingNumber.text = ColorManager.GetColorString(252, 230, 188, 255, showStr);
                    }
                    break;
            }
            if (bute >= GameCmd.eItemAttribute.Item_Attribute_Pet_Cur_Talent_Zhili && bute <= GameCmd.eItemAttribute.Item_Attribute_Pet_Cur_Talent_Jingshen)
            {
                int index = bute - GameCmd.eItemAttribute.Item_Attribute_Pet_Cur_Talent_Zhili;

                UILabel label = telentLabels[index];
                string showStr = "";
                if (bute == GameCmd.eItemAttribute.Item_Attribute_Pet_Cur_Talent_Zhili)
                {
                    showStr = CommonData.GetLocalString("智力天赋");
                }
                else if (bute == GameCmd.eItemAttribute.Item_Attribute_Pet_Cur_Talent_Minjie)
                {
                    showStr = CommonData.GetLocalString("敏捷天赋");
                }
                else if (bute == GameCmd.eItemAttribute.Item_Attribute_Pet_Cur_Talent_Tizhi)
                {
                    showStr = CommonData.GetLocalString("体质天赋");
                }
                else if (bute == GameCmd.eItemAttribute.Item_Attribute_Pet_Cur_Talent_Liliang)
                {
                    showStr = CommonData.GetLocalString("力量天赋");
                }
                else if (bute == GameCmd.eItemAttribute.Item_Attribute_Pet_Cur_Talent_Jingshen)
                {
                    showStr = CommonData.GetLocalString("精神天赋");
                }
                string labelStr = string.Format("{0}  {1}", showStr, pn.value);
                label.text = labelStr;
                label.enabled = true;
                label.depth = 4 + index;
            }


            if (bute >= GameCmd.eItemAttribute.Item_Attribute_Pet_Skill_List1_Id && bute <= GameCmd.eItemAttribute.Item_Attribute_Pet_Skill_List6_Id)
            {              
                int index = bute - GameCmd.eItemAttribute.Item_Attribute_Pet_Skill_List1_Id;
                UILabel label = skillLabels[index];
                if(pn.value != 0)
                {                
                    SkillDatabase db = GameTableManager.Instance.GetTableItem<SkillDatabase>(pn.value, 1);
                    if(db != null)
                    {
                        label.text = string.Format("{0}   ", db.strName);// db.strName;
                        label.enabled = true;
                        label.depth = 4 + i;
                    }
                }
                else
                {
                    label.enabled = false;
                }
            }
            if (bute >= GameCmd.eItemAttribute.Item_Attribute_Pet_Skill_List1_Lv && bute <= GameCmd.eItemAttribute.Item_Attribute_Pet_Skill_List6_Lv)
            {
                int index = bute - GameCmd.eItemAttribute.Item_Attribute_Pet_Skill_List1_Lv;
                UILabel label = skillLabels[index];
                if (pn.value != 0)
                {
                    string msg = string.Format("{0} lv.{1}", label.text, pn.value);
                    label.text = msg;
                }
            }
        }
    }
    private void OnRideUI(uint baseId, uint level, UILabel[] labels)
    {
        m_label_petGradeValue.text = string.Format("速度加成: {0}%", RideData.GetSpeedById_Level(baseId, (int)level));
        m_label_petLift.text = "";
        m_label_petCharacter.text = "";
        m_label_variableLevel.text = "";
        m_label_InheritingNumber.text = "";
        for (int i = 0; i < skillLabels.Length; i++)
        {
            skillLabels[i].enabled = false;
        }
        uint ride_life = 0;
        for (int i = 0; i < m_itemServer.numbers.Count; i++)
        {
            GameCmd.PairNumber pn = m_itemServer.numbers[i];
            GameCmd.eItemAttribute bute = (GameCmd.eItemAttribute)pn.id;
            switch (bute)
            {
                case GameCmd.eItemAttribute.Item_Attribute_Ride_Level:
                    break;
                case GameCmd.eItemAttribute.Item_Attribute_Ride_Life:
                    ride_life = pn.value;                
                    break;
                case GameCmd.eItemAttribute.Item_Attribute_Ride_Skill:
                    table.RideSkillData skilldata = GameTableManager.Instance.GetTableItem<table.RideSkillData>(baseId, (int)level);
                    if (skilldata != null)
                    {
                        for (int n = 0; n < skilldata.skillArray.Count; n++)
                        {
                            labels[n].enabled = true;
                            table.RideSkillDes rideSkillDes = GameTableManager.Instance.GetTableItem<table.RideSkillDes>(skilldata.skillArray[n]);
                            if (rideSkillDes != null)
                            {
                                labels[n].text = rideSkillDes.skillName;
                            }
                        }
                    }
                    break;
                case GameCmd.eItemAttribute.Item_Attribute_Ride_Base_Id:
                    table.RideDataBase ridedata = GameTableManager.Instance.GetTableItem<table.RideDataBase>(pn.value);
                    if (ridedata != null)
                    {
                        m_label_name.text = ridedata.name;

                        //DataManager.Manager<UIManager>().SetSpriteDynamicIcon(m_sprite_icon, ridedata.icon, true);
                        UIManager.GetTextureAsyn(ridedata.icon, ref m_curIconAsynSeed, () =>
                        {
                            if (null != m__icon)
                            {
                                m__icon.mainTexture= null;
                            }
                        }, m__icon);

                        if (null != m_sprite_qulity)
                        {
                            UIManager.GetQualityAtlasAsyn(ridedata.quality, ref m_curBordernAsynSeed, () =>
                            {
                                if (null != m_sprite_qulity)
                                {
                                    m_sprite_qulity.atlas = null;
                                }
                            }, m_sprite_qulity);
                        }

                        m_label_Label_2.text = string.Format("品质: {0}", DataManager.Manager<RideManager>().GetRideQualityStr(ridedata.quality));
                    }
                    break;
                default:
                    break;
            }
        }
        m_label_petYhLv.text = string.Format("寿命: {0}", ride_life);
    }
    string GetText()
    {
        string ret = "";


        return ret;
    }
    protected override void OnHide()
    {
        base.OnHide();
        Release();
    }

    public override void OnColliderMaskClicked()
    {
        base.OnColliderMaskClicked();
        this.HideSelf();
    }
    public override void Release(bool depthRelease = true)
    {
        base.Release(depthRelease);
        if (m_curIconAsynSeed != null)
        {
            m_curIconAsynSeed.Release(depthRelease);
            m_curIconAsynSeed = null;
        }
    }
}
