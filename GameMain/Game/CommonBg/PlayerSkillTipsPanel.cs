//*************************************************************************
//	创建日期:	2016/11/14 14:17:11
//	文件名称:	PlayerSkillTipsPanel
//   创 建 人:   zhudianyu	
//	版权所有:	中青宝
//	说    明:	人物技能tips
//*************************************************************************

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using table;
using Client;
using Engine.Utility;

partial class PlayerSkillTipsPanel
{
    public enum SkillTipsType
    {
        Player,
        Pet,
        Ride,
    }
    protected override void OnShow(object data)
    {
        base.OnShow(data);
    }
    /// <summary>
    /// 初始化父节点 和相对 父节点的便宜
    /// </summary>
    /// <param name="parent"></param>
    /// <param name="relativeOffset"></param>
    public void InitParentTransform(Transform parent, Vector2 relativeOffset)
    {

        // Vector3 vec = transform.worldToLocalMatrix.MultiplyPoint( parent.position );
        //// Log.Error( "vec is " + vec );
        // Vector3 locPos = new Vector3( vec.x - m_sprite_bg.width/2 - relativeOffset.x/2 , vec.y + m_sprite_bg.height/2 + relativeOffset.y/2 , vec.z );
        //// Log.Error( "relative is " + relativeOffset );
        //// Log.Error( "spr " + m_sprite_bg.width + " " + m_sprite_bg.height + "  locpos is " + locPos );
        // m_sprite_bg.transform.localPosition = locPos;

        Vector3 vec = transform.worldToLocalMatrix.MultiplyPoint(parent.position);
        int height = 360; //Screen.height/2;
        int bgHeight = m_sprite_bg.height / 2;
        float y = vec.y + m_sprite_bg.height / 2 + relativeOffset.y / 2;
        float offsety = 0;
        if (y + bgHeight > height)
        {
            offsety = height - (y + bgHeight);
        }
        y = y + offsety;
        Vector3 locPos = new Vector3(vec.x - m_sprite_bg.width / 2 - relativeOffset.x / 2, y, vec.z);

        m_sprite_bg.transform.localPosition = locPos;
    }

    public void ShowUI(string strName, string strTime, string strCost, string strDesc, string strCostType)
    {
        m_label_name.text = strName;
        m_label_CDNum.text = strTime;
        // m_label_ManaCostNum.text = strCost;
        m_label_result.text = strDesc;
        m_label_ManaLabel.text = strCostType;
    }
    public void ShowUI(SkillTipsType type, ProtoBuf.IExtensible db,int lv =0)
    {

        if (type == SkillTipsType.Ride)
        {
            RideSkillDes skilldb = (RideSkillDes)db;
            if (skilldb == null)
            {
                return;
            }
            m_label_name.text = skilldb.skillName;
            m_label_CDNum.text = CommonData.GetLocalString("冷却时间:") + skilldb.skillCD.ToString() + CommonData.GetLocalString("秒");
            // m_label_ManaCostNum.text = skilldb.costrepletion.ToString();
            m_label_result.text = skilldb.skillDesc;
            string showStr = CommonData.GetLocalString("饱食度消耗:") + skilldb.costrepletion.ToString();
            m_label_ManaLabel.text = showStr;
//             m_sprite_iconspr.atlas = DataManager.Manager<UIManager>().GetAtlasByIconName(skilldb.skillIcon);
//             m_sprite_iconspr.spriteName = skilldb.skillIcon;
            UIManager.GetTextureAsyn(skilldb.skillIcon, ref m_curIconAsynSeed, () =>
            {
                if (null != m__iconspr)
                {
                    m__iconspr.mainTexture = null;
                }
            }, m__iconspr);
           // DataManager.Manager<UIManager>().SetSpriteDynamicIcon(m_sprite_iconspr, skilldb.skillIcon, true, true);
            m_label_skillstate.text = CommonData.GetLocalString("通用");
            m_label_attackdis.text = CommonData.GetLocalString("攻击距离:无");
            m_label_level.gameObject.SetActive(false);
        }
        else
        {
            m_label_level.gameObject.SetActive(true);
            SkillDatabase skilldb = (SkillDatabase)db;
            if (skilldb == null)
            {
                return;
            }
            if (type == SkillTipsType.Pet)
            {
                string showStr = CommonData.GetLocalString("道具消耗:") + GetItemStr(skilldb.skillPlayCostItem);
                m_label_ManaLabel.text = showStr;

                m_label_skillstate.text = DataManager.Manager<PetDataManager>().GetSkillTypeStr((PetSkillType)skilldb.petType);
                GameCmd.PetSkillObj obj = DataManager.Manager<PetDataManager>().GetCurPetSkillInfo(skilldb.wdID);
                if (obj != null)
                {
                    m_label_level.text = obj.lv.ToString();
                }
                else 
                {
                    m_label_level.text = lv.ToString();
                }
            }
            else
            {
                string showStr = CommonData.GetLocalString("法力消耗:") + skilldb.costsp.ToString();
                m_label_ManaLabel.text = showStr;
                SkillInfo sinfo = DataManager.Manager<LearnSkillDataManager>().GetOwnSkillInfoById(skilldb.wdID);
                if (sinfo != null)
                {
                    m_label_level.text = sinfo.level.ToString();
                }
                else 
                {
                    m_label_level.text =lv.ToString();
                }
                m_label_skillstate.text = DataManager.Manager<LearnSkillDataManager>().GetStatus(skilldb.dwSkillSatus);
            }
            m_label_name.text = skilldb.strName;
            m_label_CDNum.text = CommonData.GetLocalString("冷却时间:") + skilldb.dwIntervalTime.ToString() + CommonData.GetLocalString("秒");
            if (type == SkillTipsType.Player)
            {
                uint minusCd = DataManager.Manager<LearnSkillDataManager>().GetHeartCd(skilldb.wdID);
                uint cd = skilldb.dwIntervalTime - minusCd;
                string cdStr = cd.ToString();
                if(minusCd != 0)
                {
                    cdStr = ColorManager.GetColorString(49, 170, 240, 255, cdStr);
                }
                m_label_CDNum.text = CommonData.GetLocalString("冷却时间:") + cdStr + CommonData.GetLocalString("秒");
                string heartDes = DataManager.Manager<LearnSkillDataManager>().GetHeartDes(skilldb.wdID);
                m_label_result.text = skilldb.strDesc + "\n" + ColorManager.GetColorString(49, 170, 240, 255, heartDes);
            }
            else
            {
                m_label_result.text = skilldb.strDesc;
            }
           // DataManager.Manager<UIManager>().SetSpriteDynamicIcon(m_sprite_iconspr, skilldb.iconPath, true, true);
            UIManager.GetTextureAsyn(skilldb.iconPath, ref m_curIconAsynSeed, () =>
            {
                if (null != m__iconspr)
                {
                    m__iconspr.mainTexture = null;
                }
            }, m__iconspr);
//             m_sprite_iconspr.atlas = DataManager.Manager<UIManager>().GetAtlasByIconName( skilldb.iconPath);
//             m_sprite_iconspr.spriteName = skilldb.iconPath;
            m_label_attackdis.text = CommonData.GetLocalString("攻击距离:") + skilldb.dwAttackDis + CommonData.GetLocalString("米");
        }
    }
    CMResAsynSeedData<CMTexture> m_curIconAsynSeed = null;
    string GetItemStr(string dbCostStr)
    {
        string showStr = CommonData.GetLocalString("无");
        string needstr = dbCostStr;
        string[] itemArray = needstr.Split('_');
        if (itemArray.Length > 1)
        {
            uint itemID = 0, itemCount = 0;
            if (uint.TryParse(itemArray[0], out itemID))
            {
                if (uint.TryParse(itemArray[1], out itemCount))
                {
                    ItemDataBase db = GameTableManager.Instance.GetTableItem<ItemDataBase>(itemID);
                    if (db != null)
                    {
                        showStr = db.itemName + "*" + itemCount;
                    }
                }
            }
        }
        return showStr;
    }
    void onClick_Close_Btn(GameObject caster)
    {
        HideSelf();

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
    protected override void OnHide()
    {
        base.OnHide();
        Release();
    }
}

