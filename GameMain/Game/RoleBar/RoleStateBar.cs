//*************************************************************************
//	创建日期:	2017-3-30 9:49
//	文件名称:	RoleStateBar.cs
//  创 建 人:   	Chengxue.Zhao
//	版权所有:	中青宝
//	说    明:	头顶血条状态
//*************************************************************************
using System;
using System.Collections.Generic;
using UnityEngine;
using Client;
public class HeadTip
{
    public uint id = 0;
    public EntityType type = EntityType.EntityType_Null;
}

public class EntityHpSprite
{
    public string spriteName = "";
    public string bgSpriteName = "";
    public bool bShow = false;
    public UISprite.Type spriteType = UISprite.Type.Filled;
}
public class HeadTipData
{
    public HeadTipType type = HeadTipType.None;
    public object value;
    public ColorType color = ColorType.White;
    public int m_nFontSize = 20;
    public bool m_bVisible = true;
    public EntityHpSprite spriteParams= null;
 

    public HeadTipData()
    {

    }
    public HeadTipData(IEntity entity,HeadTipType htype,bool vivible)
    {
        if (entity == null)
        {
            throw new ArgumentNullException("entity");
        }

        this.type = htype;
        this.m_bVisible = vivible;

        switch (type)
        {
            case HeadTipType.Hp:
                {
                    this.value = entity.GetProp((int)CreatureProp.Hp) / (float)entity.GetProp((int)CreatureProp.MaxHp);
                    this.color = ColorType.Green;
                    this.spriteParams = GetSpiteName(entity);
                }
                break;
            case HeadTipType.Name:
                {
                    this.value = entity.GetName();
                    this.color = GetNameColor(entity);
                }
                break;
            case HeadTipType.HeadMaskIcon:
                {
                    table.NpcHeadMaskDataBase npcmaskDB = RoleStateBarManager.GetNPCHeadMaskDB(entity);
                    this.value = (null != npcmaskDB) ? npcmaskDB.headMaskIcon : "";
                }
                break;
            case HeadTipType.Clan:
                {
                    this.color = GetClanNameColor(entity);
//                    this.m_nFontSize = 18;
                    //this.value = //异步请求
                }
                break;
            case HeadTipType.Title:
                {
                    this.value = GetTitleText(entity);
                    this.color = ColorType.Green;
                }
                break;
            case HeadTipType.Collect:
                {
                    this.color = ColorType.JSXT_CaiJiWu;
                    this.m_bVisible = true;
                }
                break;
            case HeadTipType.Max:
                break;

            default:
                break;
        }
    }

    public void SetCollectType(GameCmd.UninterruptActionType type)
    {
        if (type == GameCmd.UninterruptActionType.UninterruptActionType_SYDJ)
        {
            this.value = "使用道具中";
        }
        else
        {
            this.value = "采集中";
        }
    }

    ColorType GetNameColor(IEntity entity)
    {
        ColorType nameColor = ColorType.White;
        if (entity.GetEntityType() == EntityType.EntityType_Player || entity.GetEntityType() == EntityType.EntityTYpe_Robot)
        {
            nameColor = GetPlayerNameColor(entity);
        }
        else if (entity.GetEntityType() == EntityType.EntityType_NPC)
        {
            nameColor = GetNpcNameColor(entity);
        }
        else if (entity.GetEntityType() == EntityType.EntityType_Pet)
        {
            nameColor = ColorType.JSXT_ZhanHun;
        }
        return nameColor;
    }

    EntityHpSprite GetSpiteName(IEntity entity) 
    {
            EntityHpSprite spriteParams = new EntityHpSprite();
            spriteParams.spriteType = UISprite.Type.Filled;
            bool needHp = RoleStateBarManagerOld.Instance().IsNeedHpSlider(entity);
            Client.ISkillPart skillPart = MainPlayerHelper.GetMainPlayer().GetPart(EntityPart.Skill) as Client.ISkillPart;
            bool canAttack = true;
            if (skillPart != null)
            {
                int skillerror = 0;
                canAttack = skillPart.CheckCanAttackTarget(entity, out skillerror);
            }
            if (entity.GetEntityType() == EntityType.EntityType_Player)
            {
                GameCmd.eCamp mycamp = (GameCmd.eCamp)MainPlayerHelper.GetMainPlayer().GetProp((int)CreatureProp.Camp);
                GameCmd.eCamp camp = (GameCmd.eCamp)entity.GetProp((int)CreatureProp.Camp);
                spriteParams.bgSpriteName = RoleStateBarManager.GetHpSpriteName(HpSpriteType.PlayerBg);
                if (entity == MainPlayerHelper.GetMainPlayer())
                {
                    spriteParams.spriteName = RoleStateBarManager.GetHpSpriteName(HpSpriteType.Me);                
                }
                else
                {
                    if (canAttack)
                    {
                        spriteParams.spriteName = RoleStateBarManager.GetHpSpriteName(HpSpriteType.Enemy);  
                    }
                    else
                    {
                        spriteParams.spriteName = RoleStateBarManager.GetHpSpriteName(HpSpriteType.Friend);  
                    }                
                }
                spriteParams.bShow = true;
               
            }
            else if (entity.GetEntityType() == EntityType.EntityType_NPC)
            {          
                INPC npc = entity as INPC;
                //任意NPC先给一个默认的血条
                spriteParams.bgSpriteName = RoleStateBarManager.GetHpSpriteName(HpSpriteType.NoneBg);
                spriteParams.spriteName = RoleStateBarManager.GetHpSpriteName(HpSpriteType.None);
                spriteParams.bShow = needHp;  
                //是可以攻击的NPC
                if (npc.IsCanAttackNPC())
                {
                    //是不是佣兵
                    if (npc.IsMercenary())
                    {
                        spriteParams.spriteName = RoleStateBarManager.GetHpSpriteName(HpSpriteType.Friend);
                        spriteParams.bgSpriteName = RoleStateBarManager.GetHpSpriteName(HpSpriteType.PlayerBg);
                        spriteParams.bShow = true;
                    }
                    else 
                    {
                        if (npc.IsPet())
                        {
                            spriteParams.bgSpriteName = RoleStateBarManager.GetHpSpriteName(HpSpriteType.PetBg);
                            if (npc.IsMainPlayerSlave())
                            {
                                spriteParams.spriteName = RoleStateBarManager.GetHpSpriteName(HpSpriteType.MyPet);
                            }
                            else
                            {
                                if (canAttack)
                                {
                                    spriteParams.spriteName = RoleStateBarManager.GetHpSpriteName(HpSpriteType.EnemyPet);
                                }
                            }
                        }
                        else if (npc.IsSummon())
                        {
                            spriteParams.bgSpriteName = RoleStateBarManager.GetHpSpriteName(HpSpriteType.SummonBg);
                            if (npc.IsMainPlayerSlave())
                            {
                                spriteParams.spriteName = RoleStateBarManager.GetHpSpriteName(HpSpriteType.MySummon);
                            }
                            else
                            {
                                if (canAttack)
                                {
                                    spriteParams.spriteName = RoleStateBarManager.GetHpSpriteName(HpSpriteType.EnemySummon);
                                }
                            }
                        }
                        else if (npc.IsMonster())
                        {
                            spriteParams.bgSpriteName = RoleStateBarManager.GetHpSpriteName(HpSpriteType.MonsterBg);
                            spriteParams.spriteName = RoleStateBarManager.GetHpSpriteName(HpSpriteType.Monster);
                            spriteParams.spriteType = UISprite.Type.Simple;
                        }
                    }
                   
                }
               
            }
            else if (entity.GetEntityType() == EntityType.EntityTYpe_Robot)
            {

                GameCmd.eCamp mycamp = (GameCmd.eCamp)MainPlayerHelper.GetMainPlayer().GetProp((int)CreatureProp.Camp);
                GameCmd.eCamp camp = (GameCmd.eCamp)entity.GetProp((int)CreatureProp.Camp);
                spriteParams.bgSpriteName = RoleStateBarManager.GetHpSpriteName(HpSpriteType.PlayerBg);
                if (entity == MainPlayerHelper.GetMainPlayer())
                {
                    spriteParams.spriteName = RoleStateBarManager.GetHpSpriteName(HpSpriteType.Me);
                }
                else
                {
                    if (canAttack)
                    {
                        spriteParams.spriteName = RoleStateBarManager.GetHpSpriteName(HpSpriteType.Enemy);
                    }
                    else
                    {
                        spriteParams.spriteName = RoleStateBarManager.GetHpSpriteName(HpSpriteType.Friend);
                    }
                }
                spriteParams.bShow = true;
            }

      
        return spriteParams;
    }


    ColorType GetPlayerNameColor(IEntity en)
    {
        ColorType color = ColorType.White;
        GameCmd.eCamp mycamp = (GameCmd.eCamp)MainPlayerHelper.GetMainPlayer().GetProp((int)CreatureProp.Camp);
        GameCmd.eCamp camp = (GameCmd.eCamp)en.GetProp((int)CreatureProp.Camp);
        if (camp != GameCmd.eCamp.CF_None && mycamp != GameCmd.eCamp.CF_None)
        {
            if (camp != mycamp)
            {
                int pkvalue = en.GetProp((int)PlayerProp.PKValue);
                switch (pkvalue)
                {
                    case 0:
                        color = ColorType.JSXT_Enemy_White;                     
                        break;
                    case 1:
                        color = ColorType.JSXT_Enemy_Gray;     
                        break;
                    case 2:
                        color = ColorType.JSXT_Enemy_Yellow;     
                        break;
                    case 3:
                        color = ColorType.JSXT_Enemy_Red;     
                        break;                    
                }
                return color;
               
            }
            else if (camp == mycamp)
            {
                return ColorType.White;
            }
        }

        GameCmd.enumGoodNess goodNess = (GameCmd.enumGoodNess)en.GetProp((int)PlayerProp.GoodNess);

        switch (goodNess)
        {
            case GameCmd.enumGoodNess.GoodNess_Badman:
                color = ColorType.JSXT_Enemy_Yellow;
                break;
            case GameCmd.enumGoodNess.GoodNess_Evil:
                color = ColorType.JSXT_Enemy_Red;
                break;
            case GameCmd.enumGoodNess.GoodNess_Hero:
                break;
            case GameCmd.enumGoodNess.GoodNess_Normal:
                color = ColorType.JSXT_Enemy_White;
                break;
            case GameCmd.enumGoodNess.GoodNess_Rioter:
                color = ColorType.JSXT_Enemy_Gray;
                break;
            default:
                break;
        }

        return color;
    }

    ColorType GetNpcNameColor(IEntity entity)
    {
        ColorType nameColor = ColorType.JSXT_NpcCheFu;
        INPC npc = entity as INPC;
        if (npc.IsCanAttackNPC())
        {
            if (npc.IsMercenary())
            {
                nameColor = ColorType.JSXT_Enemy_White;
            }
            else 
            {
                if (npc.IsPet())
                {
                    nameColor = ColorType.JSXT_ZhanHun;
                }
                else if (npc.IsSummon())
                {
                    nameColor = ColorType.JSXT_ZhaoHuanWu;
                }
                else
                {
                    int level = MainPlayerHelper.GetPlayerLevel();
                    int monsterLv = entity.GetProp((int)CreatureProp.Level);
                    int levelGap = GameTableManager.Instance.GetGlobalConfig<int>("MonsterWarningLevelGap");
                    if (level + levelGap < monsterLv)
                    {
                        nameColor = ColorType.JSXT_Enemy_Red;
                    }
                    else
                    {
                        nameColor = ColorType.JSXT_Enemy_White;
                    }
                }
            }          
        }
        else 
        {
            table.NpcDataBase npcdb = GameTableManager.Instance.GetTableItem<table.NpcDataBase>((uint)entity.GetProp((int)Client.EntityProp.BaseID));
            if (npcdb != null)
            {
                if (npcdb.dwType == (int)GameCmd.enumNpcType.NPC_TYPE_COLLECT_PLANT)
                {
                    nameColor = ColorType.JSXT_CaiJiWu;
                }
                else if (npcdb.dwType == (int)GameCmd.enumNpcType.NPC_TYPE_TRANSFER || npcdb.dwType == (int)GameCmd.enumNpcType.NPC_TYPE_FUNCTION)
                {
                    nameColor = ColorType.JSXT_ChuanSongZhen;
                }           
                else
                {
                    nameColor = ColorType.White;
                }
            }
          
        }

        return nameColor;
    }

    ColorType GetClanNameColor(IEntity entity)
    {
        ColorType nameColor = ColorType.JSXT_Clan;
        if (DataManager.Manager<ClanManger>().ClanInfo != null)
        {
            uint clanid = 0;
            if (entity.GetEntityType() == EntityType.EntityType_NPC)
            {
                CityWarTotem cityWarTotemInfo = null;
                if (DataManager.Manager<CityWarManager>().GetCityWarTotemClanName((uint)entity.GetProp((int)Client.EntityProp.BaseID), out cityWarTotemInfo))
                {
                    clanid = cityWarTotemInfo.clanId;
                }
            }
            else if (entity.GetEntityType() == EntityType.EntityType_Player)
            {
                uint clanIdLow = (uint)entity.GetProp((int)CreatureProp.ClanIdLow);
                uint clanIdHigh = (uint)entity.GetProp((int)CreatureProp.ClanIdHigh);
                clanid = (clanIdHigh << 16) | clanIdLow;

                //clanid = (uint)entity.GetProp((int)Client.CreatureProp.ClanId);
            }

            if (clanid == DataManager.Manager<ClanManger>().ClanInfo.Id)
            {
                nameColor = ColorType.JSXT_Clan;
            }
            else if (DataManager.Manager<ClanManger>().GetClanRivalryInfo((uint)clanid) != null)
            {
                nameColor = ColorType.JSXT_Enemy_Red;
            }
        }
        return nameColor;
    }

    string GetTitleText(IEntity entity)
    {
        int titleId = entity.GetProp((int)PlayerProp.TitleId);
        string titleText = "";
        if (titleId != 0)
        {
            table.TitleDataBase tdb = GameTableManager.Instance.GetTableItem<table.TitleDataBase>((uint)titleId);
            if (tdb != null && tdb.UIState == 0)   //0 仅仅文本显示
            {
                titleText = tdb.SceneTextUI;
            }
        }

        return titleText;
    }
}

public class RoleStateBar
{
    public Transform mtrans;
    public GameObject obj;
    public IEntity entity;
    public UILabel lableName;
    //头顶标识
    public UISprite m_spHeadMask;
    public UILabel lableTitle;
    public UISlider hpslider;
    public UILabel labelClan;
    public UILabel m_labelCollectTips;
    public float offsetY;//头顶与模型脚下坐标的差值 以后根据配置读取
    public string name;
    public UIWidget widget;

    public UISprite bgSprite;
    public UISprite sliderSprite;


    bool m_bInit = false;
    public RoleStateBar(Transform trans, IEntity target, List<HeadTipData> lstdata, float offset = 0.5f)
    {
        if (trans == null || target == null)
        {
            return;
        }
        offsetY = offset;
        widget = trans.GetComponent<UIWidget>();
        mtrans = trans;
        obj = trans.gameObject;
        entity = target;

        if (!m_bInit)
        {
            for (int i = (int)HeadTipType.None + 1; i < (int)HeadTipType.Max; i++)
            {
                Transform child = mtrans.Find(((HeadTipType)i).ToString());
                if (child != null)
                {
                    GetWidget(((HeadTipType)i), child);
                }
            }
            m_bInit = true;
        }

        for (int i = (int)HeadTipType.None + 1; i < (int)HeadTipType.Max; i++)
        {
            SetWidgetState((HeadTipType)i, false);
        }

        for (int k = 0, imax = lstdata.Count; k < imax; k++)
        {
            UpdateWidget(lstdata[k]);
        }

    }
    /// <summary>
    /// 获取状态条在世界坐标的位置
    /// </summary>
    /// <returns></returns>
    public Vector3 GetNodeWorldPos(string strLocatorName)
    {
       
        Vector3 pos = Vector3.zero;

        if (entity == null)
        {
            Engine.Utility.Log.Error("GetNodeWorldPos ： entity is null");
            return pos;
        }

        Engine.Node node = entity.GetNode();
        if (node == null)
        {
            //Engine.Utility.Log.Error("GetNodeWorldPos ： entity node is null");
            return pos;
        }

        Transform trans = node.GetTransForm();
        if (trans == null)
        {
            Engine.Utility.Log.Error("GetNodeWorldPos ： entity transform is null");
            return pos;
        }

        entity.GetLocatorPos(strLocatorName, Vector3.zero, Quaternion.identity, ref pos, true);
     
        pos.y += offsetY * trans.lossyScale.y;
        return pos;
    }
    private void GetWidget(HeadTipType type, Transform child)
    {
        switch (type)
        {
            case HeadTipType.Name:
                lableName = child.GetComponent<UILabel>();
                break;
            case HeadTipType.HeadMaskIcon:
                m_spHeadMask = child.GetComponent<UISprite>();
                break;
            case HeadTipType.Hp:
                hpslider = child.GetComponent<UISlider>();
                bgSprite = child.Find("bg").GetComponent<UISprite>();
                sliderSprite = child.GetComponent<UISprite>();
                break;
            case HeadTipType.Title:
                lableTitle = child.GetComponent<UILabel>();
                break;
            case HeadTipType.Clan:
                labelClan = child.GetComponent<UILabel>();
                break;
            case HeadTipType.Collect:
                m_labelCollectTips = child.GetComponent<UILabel>();
                break;
            default:
                break;
        }
    }

    public void UpdateWidget(HeadTipData data)
    {
        SetWidgetState(data.type, data.m_bVisible);

        switch (data.type)
        {
            case HeadTipType.Name:
                lableName.text = ColorManager.GetColorString(data.color, data.value.ToString());
                lableName.fontSize = data.m_nFontSize;
                lableName.height = data.m_nFontSize;
                break;
            case HeadTipType.HeadMaskIcon:
                {
                    //DataManager.Manager<UIManager>().SetSpriteDynamicIcon(m_spHeadMask, (string)data.value, true, false);
                    AdjustWidgetPos();
                }
                break;
            case HeadTipType.Hp:
                if (hpslider != null)
                {
                    hpslider.value = (float)data.value;
                  
                }
                if (data.spriteParams != null)
                {
                        if (bgSprite != null)
                        {
//                             UIAtlas atlas = DataManager.Manager<UIManager>().GetAtlasByIconName(data.spriteParams.bgSpriteName);
//                             if (atlas != null)
//                             {
//                                 bgSprite.atlas = atlas;
//                                 bgSprite.spriteName = data.spriteParams.bgSpriteName;
//                                 Debug.Log("BgName:"+data.spriteParams.bgSpriteName);
//                             }
                            bgSprite.spriteName = data.spriteParams.bgSpriteName;
                            bgSprite.MakePixelPerfect();
                        }
                        if (sliderSprite != null)
                        {
//                             UIAtlas atlas = DataManager.Manager<UIManager>().GetAtlasByIconName(data.spriteParams.spriteName);
//                             if (atlas != null)
//                             {
//                                 sliderSprite.atlas = atlas;
//                                 sliderSprite.spriteName = data.spriteParams.spriteName;
//                                 Debug.Log("sliderSprite:" + data.spriteParams.spriteName);
//                             }
                            sliderSprite.spriteName = data.spriteParams.spriteName;
                            sliderSprite.MakePixelPerfect();
                        }
                        hpslider.gameObject.SetActive(data.spriteParams.bShow);
                   
                }            
                break;
            case HeadTipType.Title:
                lableTitle.text = ColorManager.GetColorString(data.color, data.value.ToString());
                lableTitle.fontSize = data.m_nFontSize;
                AdjustWidgetPos();
                break;
            case HeadTipType.Clan:
                string name = DataManager.Manager<TextManager>().GetLocalFormatText(LocalTextType.Clan_Commond_shizumingzi, data.value);
                labelClan.text =ColorManager.GetColorString(data.color,name);
                labelClan.fontSize = data.m_nFontSize;
                labelClan.height = data.m_nFontSize;
                AdjustWidgetPos();
                break;
            case HeadTipType.Collect:
                m_labelCollectTips.text = ColorManager.GetColorString(data.color, data.value.ToString());
                m_labelCollectTips.fontSize = data.m_nFontSize;
                if (m_labelCollectTips.gameObject.activeSelf == false)
                {
                    m_labelCollectTips.gameObject.SetActive(true);
                }
                break;
            default:
                break;
        }


    }

    public void SetWidgetState(HeadTipType type, bool active)
    {
        switch (type)
        {
            case HeadTipType.Name:
                {
                    if (lableName != null)
                    {
                        lableName.enabled = active;
                    }
                }
                break;
            case HeadTipType.HeadMaskIcon:
                {
                    if (null != m_spHeadMask && m_spHeadMask.enabled != active)
                    {
                        m_spHeadMask.enabled = active;
                    }
                }
                break;
            case HeadTipType.Title:
                if (lableTitle != null)
                {
                    lableTitle.enabled = active;
                }
                break;

            case HeadTipType.Clan:
                {
                    if (labelClan != null)
                    {
                        labelClan.enabled = active;
                    }
                }
                break;
            case HeadTipType.Hp:
                {
                    if (hpslider != null)//血条有子节点
                    {
                        hpslider.gameObject.SetActive(active);
                    }
                }
                break;
            case HeadTipType.Collect:
                if (m_labelCollectTips != null)
                {
                    m_labelCollectTips.enabled = active;
                }
                break;
            case HeadTipType.Max:
                break;
            default:
                break;
        }
    }

    void AdjustWidgetPos()
    {
        if (null != lableName)
        {
            int Gap = GameTableManager.Instance.GetGlobalConfig<int>("HeadTitleGap");
            float gap = Gap * 1.0f;
            Vector3 pos = Vector3.zero;
            pos.y = lableName.transform.localPosition.y + lableName.height * 0.5f + gap;
            if (labelClan.enabled)
            {
                pos.y += labelClan.height * 0.5f;
                labelClan.transform.localPosition = pos;
                pos.y += labelClan.height * 0.5f + gap;
            }
            if (lableTitle.enabled)
            {
                pos.y += lableTitle.height * 0.5f;
                lableTitle.transform.localPosition = pos;
                pos.y += lableTitle.height * 0.5f + gap;
            }
            if (null != m_spHeadMask && m_spHeadMask.enabled)
            {
                pos.y += m_spHeadMask.height * 0.5f;
                m_spHeadMask.transform.localPosition = pos;
                pos.y += m_spHeadMask.height * 0.5f + gap;
            }
        }
    }

    public void Destroy()
    {
        GameObject.Destroy(mtrans.gameObject);
    }


}