//*************************************************************************
//	创建日期:	2017-4-1 19:02
//	文件名称:	ViewPetPanel.cs
//  创 建 人:   	Chengxue.Zhao
//	版权所有:	中青宝
//	说    明:	其他玩家宠物查看
//*************************************************************************
using System;
using System.Collections.Generic;
using UnityEngine;
using table;

partial class ViewPlayerPanel : UIPanelBase
{
    enum PetLabelEnum
    {
        None = 0,
        Level,
        Character,  //性格
        CarryLevel, //携带等级
        GrowStatus, //成长状态
        Cultivation,    //修为
        //Part1End,

        Hp,      //气血
        MATK,       //法攻
        PATK,       //物攻
        IceDEF,     //冰防
        PDEF,       //物防
        ElectricDEF,//电防
        FireDEF,    //火防
        DarkDEF,    //暗防
        //Part2End ,

        PowerTallent,      //力量天赋
        Power,      //力量
        AgileTallent,      //敏捷天赋
        Agile,      //敏捷
        IntellectTallent,  //智力天赋
        Intellect,  //智力
        StrengthTallent,   //体力天赋
        Strength,   //体质
        SpiritTallent,   //精神天赋
        Spirit,     //精神
        PCrit,       //物理致命
        HitRate,    //命中
        MCrit,       //法术致命
        Miss,       //闪避
        Max,

    }



    void InitLables()
    {
        m_dicLabls_Pet = new Dictionary<PetLabelEnum, UILabel>();
        if (m_trans_PetPropRoot != null)
        {
            int index = 1;
            int start = (int)PetLabelEnum.None + 1;
            int end = (int)PetLabelEnum.Hp;
            do
            {
                Transform labelRoot = m_trans_PetPropRoot.Find("part" + index.ToString());
                if (labelRoot != null)
                {
                    PetLabelEnum le = PetLabelEnum.None;
                    for (int i = start; i < end; i++)
                    {
                        le = (PetLabelEnum)i;

                        Transform child = labelRoot.Find(le.ToString());
                        if (child != null)
                        {
                            UILabel lable = child.GetComponent<UILabel>();
                            if (lable != null)
                            {
                                m_dicLabls_Pet.Add(le, lable);
                            }
                            else
                            {
                                Debug.LogError("GetComponent error " + le.ToString());

                            }
                        }
                        else
                        {
                            Debug.LogError(labelRoot.name + "  FindChild error " + le.ToString());
                        }
                    }
                }
                else
                {
                    Debug.LogError("FindChild error " + index.ToString());
                }
                start = end;

                if (index == 1)
                {
                    end = (int)PetLabelEnum.PowerTallent;
                }
                else if (index == 2)
                {
                    end = (int)PetLabelEnum.Max;
                }
                index++;
            } while (index <= 3);

        }
    }

    void CreatePetGrid()
    {
        if(m_ctor_petscrollview != null)
        {
            m_ctor_petscrollview.RefreshCheck();
            m_ctor_petscrollview.Initialize<UIPetRideGrid>(m_trans_UIPetRideGrid.gameObject, OnPetGridDataUpdate, OnPetGridUIEvent);
        }    
    }

    void CreateSkillGrid()
    {
        if (m_ctor_SkillRoot != null)
        {
            m_ctor_SkillRoot.RefreshCheck();
            m_ctor_SkillRoot.Initialize<UISkillGrid>(m_trans_UISkillGrid.gameObject,OnPetGridDataUpdate, OnSkillGridUIEvent);
        }
    }


    List<GameCmd.PetSkillObj> m_lst_PetSkill = new List<GameCmd.PetSkillObj>();
    void OnPetGridDataUpdate(UIGridBase data, int index)
    {
        if (data is UISkillGrid)
        {
            UISkillGrid grid = data as UISkillGrid;
            if (m_lst_PetSkill != null && index < m_lst_PetSkill.Count)
            {
                UISkillGridData paras = new UISkillGridData() { skillid = m_lst_PetSkill[index].id, skillType = 1, level = m_lst_PetSkill[index].lv };
                grid.SetGridData(paras);
            }
        }

        if (data is UIPetRideGrid)
        {
            UIPetRideGrid grid = data as UIPetRideGrid;
            if (m_lstPetData != null && index < m_lstPetData.Count)
            {
                grid.SetGridData(m_lstPetData[index]);
            }
        }
       
    }

    void OnPetGridUIEvent(UIEventType eventType, object data, object param)
    {
        switch (eventType)
        {
            case UIEventType.Click:
                UIPetRideGrid grid = data as UIPetRideGrid;
                if (grid != null)
                {
                    OnShowPetUI(grid.PetData);
                    m_ctor_petscrollview.SetSelect(grid);
                }
                break;
        }
    }

    void OnShowPetUI(GameCmd.PetData petData)
    {
        if (petData == null)
        {
            return;
        }
        OnShowPetProp(petData);
        table.PetDataBase petdb = GameTableManager.Instance.GetTableItem<table.PetDataBase>((uint)petData.base_id);
        if (petdb == null)
        {
            return;
        }
        OnShowPetTexture(petdb);

        OnShowSkills(petData.skill_list);

        if (!string.IsNullOrEmpty(petData.name))
        {
            m_label_petshowname.text = petData.name;
        }
        else
        {
            m_label_petshowname.text = petdb.petName;
        }
        m_label_typeName.text = petdb.attackType;
        m_label_fightingLabel.text = petData.attr.fight_power.ToString();
       
      
    }

    void OnShowPetProp(GameCmd.PetData petData)
    {
        table.PetUpGradeDataBase petupgrade = GameTableManager.Instance.GetTableItem<table.PetUpGradeDataBase>((uint)petData.lv,(int)petData.base_id);
        if (petupgrade == null)
        {
            Engine.Utility.Log.Error("PetUpGradeDataBase  null id {0}", petData.base_id);
            return;
        }
        foreach (var item in m_dicLabls_Pet)
        {
            switch (item.Key)
            {
                case PetLabelEnum.Level:
                    item.Value.text = petData.lv.ToString();
                    break;
                case PetLabelEnum.Character:
                    item.Value.text = DataManager.Manager<PetDataManager>().GetPetCharacterStr(petData.character);
                    break;
                case PetLabelEnum.CarryLevel:
                    table.PetDataBase petdb = GameTableManager.Instance.GetTableItem<table.PetDataBase>((uint)petData.base_id);
                    if (petdb != null)
                    {
                        item.Value.text = petdb.carryLevel.ToString();
                    }
                    break;
                case PetLabelEnum.GrowStatus:
                    item.Value.text = DataManager.Manager<PetDataManager>().GetGrowStatus(petData.grade);
                    break;
                case PetLabelEnum.Cultivation:
                    item.Value.text = petData.yh_lv.ToString();
                    break;
                case PetLabelEnum.Hp:
                    item.Value.text = petData.hp.ToString();
                    break;
                case PetLabelEnum.MATK:
                    item.Value.text = petData.attr.mdam.ToString();
                    break;
                case PetLabelEnum.PATK:
                    item.Value.text = petData.attr.pdam.ToString();
                    break;
                case PetLabelEnum.IceDEF:
                    item.Value.text = (petData.attr.biochdef + petData.attr.mdef).ToString();
                    break;
                case PetLabelEnum.PDEF:
                    item.Value.text = petData.attr.pdef.ToString();
                    break;
                case PetLabelEnum.ElectricDEF:
                    item.Value.text = (petData.attr.lightdef + petData.attr.mdef).ToString();
                    break;
                case PetLabelEnum.FireDEF:
                    item.Value.text = (petData.attr.heatdef + petData.attr.mdef).ToString();
                    break;
                case PetLabelEnum.DarkDEF:
                    item.Value.text = (petData.attr.wavedef + petData.attr.mdef).ToString();
                    break;
                case PetLabelEnum.PowerTallent:
                    item.Value.text = petData.cur_talent.liliang.ToString();
                    break;
                case PetLabelEnum.Power:
                    item.Value.text = (petData.attr_point.liliang + petupgrade.power).ToString();
                    break;
                case PetLabelEnum.AgileTallent:
                    item.Value.text = petData.cur_talent.minjie.ToString();
                    break;
                case PetLabelEnum.Agile:
                    item.Value.text = (petData.attr_point.minjie + petupgrade.minjie).ToString();
                    break;
                case PetLabelEnum.IntellectTallent:
                    item.Value.text = petData.cur_talent.zhili.ToString();
                    break;
                case PetLabelEnum.Intellect:
                    item.Value.text = (petData.attr_point.zhili + petupgrade.zhili).ToString();
                    break;
                case PetLabelEnum.StrengthTallent:
                    item.Value.text = petData.cur_talent.tizhi.ToString();
                    break;
                case PetLabelEnum.Strength:
                    item.Value.text = (petData.attr_point.tizhi + petupgrade.tizhi).ToString();
                    break;
                case PetLabelEnum.SpiritTallent:
                    item.Value.text = petData.cur_talent.jingshen.ToString();
                    break;
                case PetLabelEnum.Spirit:
                    item.Value.text = (petData.attr_point.jingshen + petupgrade.jingshen).ToString();
                    break;
                case PetLabelEnum.PCrit:
                    item.Value.text = string.Format("{0}%", petData.attr.plucky/ 100.0f);
                    break;
                case PetLabelEnum.HitRate:
                    item.Value.text = string.Format("{0}%", petData.attr.hit / 100.0f);
                    break;
                case PetLabelEnum.MCrit:
                    item.Value.text = string.Format("{0}%", petData.attr.mlucky / 100.0f);
                    break;
                case PetLabelEnum.Miss:
                    item.Value.text = string.Format("{0}%", petData.attr.hide / 100.0f);
                    break;
                case PetLabelEnum.Max:
                    break;
                default:
                    break;
            }
        }
    }

    void OnShowPetTexture(table.PetDataBase petdb)
    {

        if (m_RTObj_Pet != null)
        {
            m_RTObj_Pet.Release();
        }
        m_RTObj_Pet = DataManager.Manager<RenderTextureManager>().CreateRenderTextureObj((int)petdb.modelID, 512);
        if (m_RTObj_Pet == null)
        {
            return;
        }
//        float rotateY = 170f;
        ModeDiplayDataBase modelDisp = GameTableManager.Instance.GetTableItem<ModeDiplayDataBase>(petdb.modelID);
        if (modelDisp == null)
        {
            Engine.Utility.Log.Error("宠物模型ID为{0}的模型展示数据为空",petdb.modelID);
            return;
        }
        m_RTObj_Pet.SetDisplayCamera(modelDisp.pos512, modelDisp.rotate512, modelDisp.Modelrotation);
        m_RTObj_Pet.PlayModelAni(Client.EntityAction.Stand);
        UIRenderTexture rt = m__PetModel.GetComponent<UIRenderTexture>();
        if (null == rt)
        {
            rt = m__PetModel.gameObject.AddComponent<UIRenderTexture>();
        }
        if (null != rt)
        {
            rt.SetDepth(0);
            rt.Initialize(m_RTObj_Pet, m_RTObj_Pet.YAngle, new Vector2(512, 512));
        }
      
    }


    void OnShowSkills(List<GameCmd.PetSkillObj> skill_list)
    {
        m_lst_PetSkill = skill_list;
        m_ctor_SkillRoot.CreateGrids(m_lst_PetSkill.Count);
    }

    void OnSkillGridUIEvent(UIEventType eventType, object data, object param)
    {
        switch (eventType)
        {
            case UIEventType.Click:
                UISkillGrid grid = data as UISkillGrid;
                if (grid != null)
                {
                    if (grid.Data != null && grid.Data.skillid != 0)
                    {
                        DataManager.Manager<UIPanelManager>().ShowPanel(PanelID.PlayerSkillTipsPanel, panelShowAction: (pb) =>
                        {
                            if (null != pb && pb is PlayerSkillTipsPanel)
                            {
                                PlayerSkillTipsPanel panel = pb as PlayerSkillTipsPanel;
                                if (panel != null)
                                {
                                    table.SkillDatabase skilldb = GameTableManager.Instance.GetTableItem<table.SkillDatabase>((uint)grid.Data.skillid, grid.Data.level);
                                    panel.ShowUI(PlayerSkillTipsPanel.SkillTipsType.Pet, skilldb, grid.Data.level);
                                    panel.InitParentTransform(grid.transform, new Vector2(0, 80));
                                }
                            }                          
                          
                        });
                    }
                }
                break;
        }
    }

    
   
}
