//*************************************************************************
//	创建日期:	2017-3-31 16:49
//	文件名称:	ViewPlayerPanel.cs
//  创 建 人:   	Chengxue.Zhao
//	版权所有:	中青宝
//	说    明:	查看玩家属性界面
//*************************************************************************
using System;
using System.Collections.Generic;
using UnityEngine;


public enum ViewPlayerPanelPageEnum
{
    None = 0,
    Page_role = 1,
    Page_pet = 2,
    Page_ride = 3,
    Max,
}


public partial class ViewPlayerPanel : UIPanelBase
{
    enum PlayerLabelEnum
    {
        None = 0,
        Name,
        ID,
        Clan,//氏族
        Level,
        DemonLv,    //神魔等级
        //Part1End,

        HpMax,      //最大生命值
        MpMax,      //最大法术值
        PATK,       //物理攻击
        MATK,       //法术攻击
        PDEF,       //物理防御
        MDEF,       //法术防御
        //Part2End ,

        Power,      //力量
        Agile,      //敏捷
        Intellect,  //智力
        Strength,   //体力
        Spirit,     //精神
        //Part3End ,

        FireATK,    //火攻击
        IceATK,     //冰攻击
        ElectricATK,//电攻击
        DarkATK,    //暗攻击
        FireDEF,    //火抗性
        IceDEF,     //冰抗性
        ElectricDEF,//电抗性
        DarkDEF,    //暗抗性
        //Part4End ,

        PCrit,       //物理暴击
        MCrit,       //法术暴击
        CritDamg,   //暴击伤害
        Cure,       //治疗
        HitRate,    //命中
        Miss,       //闪避
        PDamgAbsorb, //物理伤害吸收
        MDamgAbsorb, //法术伤害吸收
        DamgDeepen,  //伤害加深
        DamgAbsorb,  //伤害加深
        Max,

    }
    #region Player
    //装备格子
    private Dictionary<GameCmd.EquipPos, UIEquipGrid> equipGridDic = null;
    //装备列表
    private Dictionary<GameCmd.EquipPos, uint> equipDic = null;
    private Dictionary<GameCmd.EquipSuitType, UIEquipGrid> m_dicSuitGrid = null;
    Dictionary<PlayerLabelEnum, UILabel> m_dicLabls_Player = null;
    private IRenerTextureObj m_RTObj_Player = null;
    float rotateY = 170f;

    int m_nShowMode = 1;//1 装备2 时装

    ViewPlayerData m_showInfo = null;

    bool m_bEnableRide = false;
    bool m_bEnablePet = false;
    #endregion


    #region Ride
    const int SKILLNUM = 6;

    List<RideData> m_lstRideData = null;
    private int m_nModelId = 0;
    private IRenerTextureObj m_RTObj_Ride = null;
    #endregion

    #region Pet
    Dictionary<PetLabelEnum, UILabel> m_dicLabls_Pet = null;
    List<GameCmd.PetData> m_lstPetData = null;
    private IRenerTextureObj m_RTObj_Pet = null;
    #endregion

    protected override void OnLoading()
    {
        base.OnLoading();
        OnInitUI();

      
    }

    protected override void OnShow(object data)
    {
        base.OnShow(data);
        //ride
        m_lstRideData = new List<RideData>();

        InitRideSkill();

        //pet

        m_lstPetData = new List<GameCmd.PetData>();
        InitLables();

        CreateSkillGrid();

        m_bEnableRide = false;
        m_bEnablePet = false;
        CreatePetGrid();
        OnCreateRideGrid();
        if (data is ViewPlayerData)
        {
            m_showInfo = (ViewPlayerData)data;

            if (m_showInfo.rideData != null)
            {
                m_lstRideData.Clear();
                for (int i = 0; i < m_showInfo.rideData.ride_list.Count; i++)
                {
                    AddRide(m_showInfo.rideData.ride_list[i]);
                }
               
                m_bEnableRide = m_showInfo.rideData.ride_list.Count > 0;
            }
             if (m_showInfo.petdata != null)
            {
                m_bEnablePet = m_showInfo.petdata.pet_list.Count > 0;
                m_lstPetData.Clear();
                m_lstPetData.AddRange(m_showInfo.petdata.pet_list);
            }
            else
            {
                m_bEnablePet = false;
            }


            OnShowPropety(m_showInfo);

            OnShowEquipGridInfo(m_showInfo.itemList);

            OnShowSuit(m_showInfo.suit_data);

            OnShowModelTexture(m_showInfo.suit_data, (int)m_showInfo.job, (int)m_showInfo.sex);

            ChangeShowLeftUI(1);

            ShowColorStoneStrengthLabel(m_showInfo.ActiveColorSuitLv, m_showInfo.ActiveStoneSuitLv, m_showInfo.ActiveStrengthenSuitLv);
        }
    }

    void ShowColorStoneStrengthLabel(uint color ,uint stone, uint strength) 
    {
        m_label_ActiveGridSuitLv.text = strength.ToString();
        m_label_ActiveColorSuitLv.text = color.ToString();
        m_label_ActiveStoneSuitLv.text = stone.ToString();

        if (m_RTObj_Player != null)
        {
            if (m_RTObj_Player != null)
            {
                DataManager.Manager<EquipManager>().AddEquipStoneSuitParticle(m_RTObj_Player, stone);
            }
        }
    }
    void ToggleRightTab(ViewPlayerPanelPageEnum type) 
    {
        m_trans_PlayerContent.gameObject.SetActive(type != ViewPlayerPanelPageEnum.Page_ride && type != ViewPlayerPanelPageEnum.Page_pet);
        m_trans_PetContent.gameObject.SetActive(type == ViewPlayerPanelPageEnum.Page_pet);
        m_trans_RideContent.gameObject.SetActive(type == ViewPlayerPanelPageEnum.Page_ride);

        if (type == ViewPlayerPanelPageEnum.Page_pet)
        {
            if (m_ctor_petscrollview != null)
            {
                if (m_lstPetData.Count > 0)
                {
                    m_ctor_petscrollview.CreateGrids(m_lstPetData.Count);
                    OnShowPetUI(m_lstPetData[0]);
                    m_ctor_petscrollview.ResetPosition();
                }
            }
        }
        else if (type == ViewPlayerPanelPageEnum.Page_ride)
        {
            if (m_ctor_Ridescrollview != null)
            {
                if (m_lstRideData.Count > 0)
                {
                    m_ctor_Ridescrollview.CreateGrids(m_lstRideData.Count);
                    OnShowRideUI(m_lstRideData[0]);
                    m_ctor_Ridescrollview.ResetPosition();
                }
            }
        }
       
    }
    public override void Release(bool depthRelease = true)
    {
        base.Release(depthRelease);
        if (m_ctor_petscrollview != null)
        {
            m_ctor_petscrollview.Release(depthRelease);
        }
        if(m_ctor_Ridescrollview != null)
        {
            m_ctor_Ridescrollview.Release(depthRelease);
        }
        if (m_ctor_SkillRoot != null)
        {
            m_ctor_SkillRoot.Release(depthRelease);
        }
    }
    protected override void OnPanelBaseDestory()
    {
        base.OnPanelBaseDestory();
//         if (null != m_ctor_petscrollview)
//         {
//             
//             UIManager.OnObjsRelease(m_ctor_petscrollview.CacheTransform, (uint)GridID.Uipetridegrid);
//             m_ctor_petscrollview = null;
//         }
//         if (null != m_ctor_Ridescrollview)
//         {
//             UIManager.OnObjsRelease(m_ctor_Ridescrollview.CacheTransform, (uint)GridID.Uipetridegrid);
//             m_ctor_Ridescrollview = null;
//         }
//         if (m_ctor_SkillRoot != null)
//         {
//             UIManager.OnObjsRelease(m_ctor_SkillRoot.CacheTransform, (uint)GridID.Uiskillgrid);
//             m_ctor_SkillRoot = null;
//         }
    }
    public override bool OnTogglePanel(int tabType, int pageid)
    {
        if (tabType == 1)
        {

            if (pageid == (int)ViewPlayerPanelPageEnum.Page_ride)
            {
                if (!m_bEnableRide)
                {
                    TipsManager.Instance.ShowTips("他没有坐骑");
                    return false;
                }
               

            }
            else if (pageid == (int)ViewPlayerPanelPageEnum.Page_pet)
            {
                if (!m_bEnablePet)
                {
                    TipsManager.Instance.ShowTips(LocalTextType.Imformation_Commond_meiyouzhanhun);
                    return false;
                }               
            }
            ToggleRightTab((ViewPlayerPanelPageEnum)pageid);
        }
       
        return base.OnTogglePanel(tabType, pageid);
    }

    protected override void OnJump(PanelJumpData jumpData)
    {
        base.OnJump(jumpData);
        int firstTab = -1;
        if (jumpData != null && jumpData.Tabs.Length > 0)
        {
            firstTab = jumpData.Tabs[0];
        }
        else 
        {
            firstTab = 1;
        }
        UIFrameManager.Instance.OnCilckTogglePanel(this.PanelId, 1, firstTab);
    }
    protected override void OnHide()
    {
        base.OnHide();
        m_nModelId = 0;
        if (m_RTObj_Ride != null)
        {
            m_RTObj_Ride.Release();
            m_RTObj_Ride = null;
        }

        Release();
        if (m_RTObj_Pet != null)
        {
            m_RTObj_Pet.Release();
            m_RTObj_Pet = null;
        }
    }

    void OnInitUI()
    {
        OnInitEquip();

        OnInitProp();

        UIEventListener.Get(m_spriteEx_equipfashionbtn.gameObject).onClick = onClick_Equipfashionbtn_Btn;
    }

    private void OnInitProp()
    {
        m_dicLabls_Player = new Dictionary<PlayerLabelEnum, UILabel>();
        if (m_trans_propRoot != null)
        {
            int index = 1;
            int start = (int)PlayerLabelEnum.None + 1;
            int end = (int)PlayerLabelEnum.HpMax;
            do
            {
                Transform labelRoot = m_trans_propRoot.Find("part" + index.ToString());
                if (labelRoot != null)
                {
                    PlayerLabelEnum le = PlayerLabelEnum.None;
                    for (int i = start; i < end; i++)
                    {
                        le = (PlayerLabelEnum)i;

                        Transform child = labelRoot.Find(le.ToString());
                        if (child != null)
                        {
                            UILabel lable = child.GetComponent<UILabel>();
                            if (lable != null)
                            {
                                m_dicLabls_Player.Add(le, lable);
                            }
                            else
                            {
                                Debug.LogError("GetComponent error " + le.ToString());

                            }
                        }
                        else
                        {
                            Debug.LogError("FindChild error " + le.ToString());
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
                    end = (int)PlayerLabelEnum.Power;
                }
                else if (index == 2)
                {
                    end = (int)PlayerLabelEnum.FireATK;
                }
                else if (index == 3)
                {
                    end = (int)PlayerLabelEnum.PCrit;
                }
                else if (index == 4)
                {
                    end = (int)PlayerLabelEnum.Max;
                }
                index++;
            } while (index <= 5);

        }
    }

    private void OnInitEquip()
    {
        equipGridDic = new Dictionary<GameCmd.EquipPos, UIEquipGrid>();
        equipDic = new Dictionary<GameCmd.EquipPos, uint>();

        GameObject cloneObj = m_trans_UIEquipGrid.gameObject;
        //创建装备格子初始化
        if (null != m_trans_EquipmentGridRoot)
        {
            UIEquipGrid equipGridTemp = null;
            Transform equipTs = null;
            GameObject equipTemp;
            uint equipId = 0;
            string equipTypeName = "";
            string name = "";
            GameObject tempEquipGrid = null;
            for (GameCmd.EquipPos i = GameCmd.EquipPos.EquipPos_Hat; i < GameCmd.EquipPos.EquipPos_Max; i++)
            {
                if (i == GameCmd.EquipPos.EquipPos_Capes
                    || i == GameCmd.EquipPos.EquipPos_Office)
                {
                    continue;
                }
                name = i.ToString().Split(new char[] { '_' })[1];
                equipTs = Util.findTransform(m_trans_EquipmentGridRoot, name);
                if (null == equipTs)
                {
                    continue;
                }
                tempEquipGrid = NGUITools.AddChild(equipTs.gameObject, cloneObj);
                if (null == tempEquipGrid)
                {
                    continue;
                }
                equipGridTemp = tempEquipGrid.GetComponent<UIEquipGrid>();
                if (null == equipGridTemp)
                {
                    equipGridTemp = tempEquipGrid.gameObject.AddComponent<UIEquipGrid>();
                }

                equipGridTemp.InitEquipGrid(i);
                equipGridTemp.RegisterUIEventDelegate(OnEquipGridEventCallback);
                equipGridDic.Add(i, equipGridTemp);
            }
        }
        OnInitSuit(cloneObj);
    }

    private void OnInitSuit(GameObject prefab)
    {
        m_dicSuitGrid = new Dictionary<GameCmd.EquipSuitType, UIEquipGrid>();
        Transform equipTs = null;
        UIEquipGrid equipGridTemp = null;
        for (GameCmd.EquipSuitType i = GameCmd.EquipSuitType.Clothes_Type; i <= GameCmd.EquipSuitType.Magic_Pet_Type; i++)
        {
            if (i == GameCmd.EquipSuitType.Back_Type || i == GameCmd.EquipSuitType.Face_Type)
            {
                continue;
            }
            string name = "";
            int index = i.ToString().LastIndexOf('_');
            if (index != -1)
            {
                name = i.ToString().Substring(0, index);
            }
            if (string.IsNullOrEmpty(name))
            {
                continue;
            }
            equipTs = Util.findTransform(m_widget_Fashion.transform, name);
            if (equipTs == null)
            {
                continue;
            }
            GameObject tempEquipGrid = NGUITools.AddChild(equipTs.gameObject, prefab);
            if (null == tempEquipGrid)
            {
                continue;
            }
            equipGridTemp = tempEquipGrid.GetComponent<UIEquipGrid>();
            if (null == equipGridTemp)
            {
                equipGridTemp = tempEquipGrid.AddComponent<UIEquipGrid>();
            }
            equipGridTemp.InitSuitGrid(i);
            equipGridTemp.RegisterUIEventDelegate(OnSuitGridEvent);
            m_dicSuitGrid.Add(i, equipGridTemp);
        }
    }
    /// <summary>
    /// 基本属性
    /// </summary>
    /// <param name="showInfo"></param>
    private void OnShowPropety(ViewPlayerData showInfo)
    {
        m_label_powerLabel.text = showInfo.power.ToString();
        m_trans_propRoot.parent.GetComponent<UIScrollView>().ResetPosition();
        GameCmd.ViewRoleData roledata = showInfo.viewRoleData;
        foreach (var item in m_dicLabls_Player)
        {
            PlayerLabelEnum type = item.Key;
            switch (type)
            {
                case PlayerLabelEnum.Name:
                    item.Value.text = showInfo.username;
                    break;
                case PlayerLabelEnum.ID:
                    item.Value.text = showInfo.userid.ToString();
                    break;
                case PlayerLabelEnum.Clan:
                    if (string.IsNullOrEmpty(showInfo.clan_name))
                    {
                        item.Value.text = "未加入";
                    }
                    else
                    {
                        item.Value.text = showInfo.clan_name.ToString();
                    }
                    break;
                case PlayerLabelEnum.Level:
                    item.Value.text = showInfo.user_level.ToString();
                    break;
                case PlayerLabelEnum.DemonLv:
                    item.Value.text = showInfo.god_level.ToString();
                    break;
                case PlayerLabelEnum.HpMax:
                    item.Value.text = roledata.maxhp.ToString();
                    break;
                case PlayerLabelEnum.MpMax:
                    item.Value.text = roledata.maxsp.ToString();
                    break;
                case PlayerLabelEnum.PATK:
                    item.Value.text = string.Format("{0}-{1}", roledata.pdam_min, roledata.pdam_max);
                    break;
                case PlayerLabelEnum.MATK:
                    item.Value.text = string.Format("{0}-{1}", roledata.mdam_min, roledata.mdam_max);
                    break;
                case PlayerLabelEnum.PDEF:
                    item.Value.text = string.Format("{0}-{1}", roledata.pdef_min, roledata.pdef_max);
                    break;
                case PlayerLabelEnum.MDEF:
                    item.Value.text = string.Format("{0}-{1}", roledata.mdef_min, roledata.mdef_max);
                    break;
                case PlayerLabelEnum.Power:
                    item.Value.text = roledata.liliang.ToString();
                    break;
                case PlayerLabelEnum.Agile:
                    item.Value.text = roledata.minjie.ToString();
                    break;
                case PlayerLabelEnum.Intellect:
                    item.Value.text = roledata.zhili.ToString();
                    break;
                case PlayerLabelEnum.Strength:
                    item.Value.text = roledata.tizhi.ToString();
                    break;
                case PlayerLabelEnum.Spirit:
                    item.Value.text = roledata.jingshen.ToString();
                    break;
                case PlayerLabelEnum.FireATK:
                    item.Value.text = roledata.esdam.ToString();
                    break;
                case PlayerLabelEnum.IceATK:
                    item.Value.text = roledata.ssdam.ToString();
                    break;
                case PlayerLabelEnum.ElectricATK:
                    item.Value.text = roledata.lsdam.ToString();
                    break;
                case PlayerLabelEnum.DarkATK:
                    item.Value.text = roledata.vsdam.ToString();
                    break;
                case PlayerLabelEnum.FireDEF:
                    item.Value.text = roledata.esdef.ToString();
                    break;
                case PlayerLabelEnum.IceDEF:
                    item.Value.text = roledata.ssdef.ToString();
                    break;
                case PlayerLabelEnum.ElectricDEF:
                    item.Value.text = roledata.lsdef.ToString();
                    break;
                case PlayerLabelEnum.DarkDEF:
                    item.Value.text = roledata.vsdef.ToString();
                    break;
                case PlayerLabelEnum.PCrit:
                    item.Value.text = string.Format("{0}%", roledata.lucky / 100.0f);
                    break;
                case PlayerLabelEnum.MCrit:
                    item.Value.text = string.Format("{0}%", roledata.mlucky / 100.0f);
                    break;
                case PlayerLabelEnum.CritDamg:
                    item.Value.text = string.Format("{0}%", roledata.criti_ratio / 100.0f);
                    break;
                case PlayerLabelEnum.Cure:
                    item.Value.text = roledata.cure.ToString();
                    break;
                case PlayerLabelEnum.HitRate:
                    item.Value.text = string.Format("{0}%", roledata.phit / 100.0f);
                    break;
                case PlayerLabelEnum.Miss:
                    item.Value.text = string.Format("{0}%", roledata.hide_per / 100.0f);
                    break;
                case PlayerLabelEnum.PDamgAbsorb:
                    item.Value.text = roledata.pabs.ToString();
                    break;
                case PlayerLabelEnum.MDamgAbsorb:
                    item.Value.text = roledata.mabs.ToString();
                    break;
                case PlayerLabelEnum.DamgDeepen:
                    item.Value.text = string.Format("{0}%", roledata.harm_add_per / 100.0f);
                    break;
                case PlayerLabelEnum.DamgAbsorb:
                    item.Value.text = string.Format("{0}%", roledata.harm_sub_per / 100.0f);
                    break;
                case PlayerLabelEnum.Max:
                    break;
                default:
                    break;
            }
        }
    }

    Dictionary<uint, uint> GetEquipGem()
    {
        Dictionary<uint, uint> gemList = new Dictionary<uint, uint>();
        for (int i = 0; i < m_showInfo.gem_data.Count; i++)
        {
            if (m_showInfo.gem_data[i].base_id != 0)
            {
                gemList.Add(m_showInfo.gem_data[i].index, m_showInfo.gem_data[i].base_id);
            }
        }
        return gemList;
    }
    /// <summary>
    /// 显示装备
    /// </summary>
    /// <param name="itemList"></param>
    private void OnShowEquipGridInfo(List<GameCmd.ItemSerialize> itemList)
    {
        //装备栏
        GameCmd.ItemSerialize equipItem = null;
        uint cacheEquipId = 0;
        UIEquipGrid equipGrid = null;
        BaseItem baseItem = null;
        bool needSetData = false;

        //GameCmd.eItemAttribute

        for (GameCmd.EquipPos i = GameCmd.EquipPos.EquipPos_Hat; i < GameCmd.EquipPos.EquipPos_Max; i++)
        {
            if (i == GameCmd.EquipPos.EquipPos_Capes
                || i == GameCmd.EquipPos.EquipPos_Office)
            {
                continue;
            }
            needSetData = false;
            if (equipGridDic.TryGetValue(i, out equipGrid))
            {
             

                if (Equip(itemList, (int)i, out equipItem))
                {
                    baseItem = new BaseEquip(equipItem.dwObjectID, equipItem);
                    if (equipDic.TryGetValue(i, out cacheEquipId)
                        && cacheEquipId != equipItem.qwThisID)
                    {
                        equipDic[i] = equipItem.qwThisID;
                        needSetData = true;
                    }
                    else if (!equipDic.ContainsKey(i))
                    {
                        equipDic.Add(i, equipItem.qwThisID);
                        needSetData = true;
                    }

                    if (needSetData)
                    {
                        equipGrid.SetGridData(baseItem);
                    }
                }
                else
                {
                    baseItem = null;
                    if (equipDic.ContainsKey(i))
                    {
                        equipDic[i] = 0;
                    }
                    needSetData = true;
                }

                if (needSetData)
                {
                    equipGrid.SetGridData(baseItem);
                }

                //强化等级
                GameCmd.StrengthList strengthData = this.m_showInfo.strengthList.Find((data) => { return data.equip_pos == i; });
                if (strengthData != null)
                {
                    equipGrid.UpdateStrengthenInfo(strengthData.level, baseItem != null);
                }
                else
                {
                    equipGrid.UpdateStrengthenInfo(0, baseItem != null);
                }
            }
        }
    }

    /// <summary>
    /// 显示模型
    /// </summary>
    /// <param name="lstSuit"></param>
    /// <param name="nJob"></param>
    /// <param name="nSex"></param>
    /// <param name="nSize"></param>
    private void OnShowModelTexture(List<GameCmd.SuitData> lstSuit, int nJob, int nSex, int nSize = 700)
    {
        if (m_RTObj_Player != null)
        {
            m_RTObj_Player.Release();
        }
        m_RTObj_Player = DataManager.Manager<RenderTextureManager>().CreateRenderTextureObj(lstSuit, nJob, nSex, nSize);
        if (m_RTObj_Player == null)
        {
            Engine.Utility.Log.Error("Create rt obj failed");
            return;
        }
        m_RTObj_Player.SetModelRotateY(-135f);
        m_RTObj_Player.SetCamera(new Vector3(0, 1f, 0f), new Vector3(0, 45, 0), 4f);
        m_RTObj_Player.PlayModelAni(Client.EntityAction.Stand);

        UIRenderTexture rt = m__modelTexture.GetComponent<UIRenderTexture>();
        if (null == rt)
        {
            rt = m__modelTexture.gameObject.AddComponent<UIRenderTexture>();
        }
        if (null != rt)
        {
            rt.SetDepth(1);
            rt.Initialize(m_RTObj_Player, -135f, new Vector2(700f, 700f), null);
        }
    }
    /// <summary>
    /// 显示时装
    /// </summary>
    /// <param name="lstSuit"></param>
    private void OnShowSuit(List<GameCmd.SuitData> lstSuit)
    {
        ClientSuitData suitdata = new ClientSuitData();
        foreach (var suitGrid in m_dicSuitGrid)
        {
            for (int i = 0; i < lstSuit.Count; ++i)
            {
                if (lstSuit[i].suit_type == suitGrid.Key)
                {
                    suitdata.suitBaseID = lstSuit[i].baseid;
                }
            }
            suitGrid.Value.SetSuitData(suitdata);
            suitdata.suitBaseID = 0;
        }
    }

    private bool Equip(List<GameCmd.ItemSerialize> itemList, int pos, out GameCmd.ItemSerialize item)
    {
        item = null;
        for (int i = 0, imax = itemList.Count; i < imax; i++)
        {
            ItemDefine.ItemLocation local = ItemDefine.TransformServerLocation2Local(itemList[i].pos.loc);
            if (local != null && local.Position.y == pos)
            {
                item = itemList[i];
                return true;
            }
        }
        return false;
    }

    private void ChangeShowLeftUI(int mode)
    {
        if (mode > 0 && mode < 3)
        {
            m_nShowMode = mode;
            if (m_nShowMode == 1)
            {
                m_spriteEx_equipfashionbtn.ChangeSprite(2);
            }
            else if (m_nShowMode == 2)
            {
                m_spriteEx_equipfashionbtn.ChangeSprite(1);
            }

            m_widget_Equipment.alpha = mode == 1 ? 1f : 0f;
            m_widget_Fashion.alpha = mode == 2 ? 1f : 0f;
        }
    }

    void onClick_Equipfashionbtn_Btn(GameObject caster)
    {
        if (m_nShowMode == 1)
        {
            ChangeShowLeftUI(2);
        }
        else if (m_nShowMode == 2)
        {
            ChangeShowLeftUI(1);
        }
    }
    /// <summary>
    /// 装备格子事件回调
    /// </summary>
    /// <param name="eventType"></param>
    /// <param name="data"></param>
    /// <param name="param"></param>
    private void OnEquipGridEventCallback(UIEventType eventType, object data, object param)
    {
        switch (eventType)
        {
            case UIEventType.Click:
                {
                    UIEquipGrid equipGrid = data as UIEquipGrid;
                    if (null != equipGrid.Data)
                    {
                        DataManager.Manager<UIPanelManager>().ShowOtherItemTips(equipGrid.Data, null, GetEquipGem(), m_showInfo.user_level);
                    }
                    else
                    {
                        Engine.Utility.Log.Info("Equip pos is null");
                    }
                }
                break;
        }
    }

    private void OnSuitGridEvent(UIEventType eventType, object data, object param)
    {
        switch (eventType)
        {
            case UIEventType.Click:
                {
                    UIEquipGrid equipGrid = data as UIEquipGrid;
                    if (equipGrid.SuitData.suitBaseID != 0)
                    {
                        table.SuitDataBase database = GameTableManager.Instance.GetTableItem<table.SuitDataBase>(equipGrid.SuitData.suitBaseID, 1);
                        if (database != null)
                        {
                            DataManager.Manager<UIPanelManager>().ShowPanel(PanelID.FashionTips, data: database);
                        }
                    }
                    else
                    {
                        Engine.Utility.Log.Info("Equip pos is null");
                    }
                }
                break;
        }
    }


    void onClick_BtnGridSuitNormal_Btn(GameObject caster)
    {
        SuitPanelParam param = new SuitPanelParam()
        {
            m_type = SuitPanelParam.SuitType.Strengthen,
            vec = new Vector3(133.8f, 201.31f, 0),
            isOther = true,
        };
        DataManager.Manager<UIPanelManager>().ShowPanel(PanelID.GridStrengthenSuitPanel, data: param);
    }

    void onClick_BtnGridSuitActive_Btn(GameObject caster)
    {
        SuitPanelParam param = new SuitPanelParam()
        {
            m_type = SuitPanelParam.SuitType.Strengthen,
            vec = new Vector3(133.8f, 201.31f, 0),
            isOther = true,
        };
        DataManager.Manager<UIPanelManager>().ShowPanel(PanelID.GridStrengthenSuitPanel, data: param);
    }
    void onClick_BtnColorSuitNormal_Btn(GameObject caster)
    {
        SuitPanelParam param = new SuitPanelParam()
        {
            m_type = SuitPanelParam.SuitType.Color,
            vec = new Vector3(133.8f, 201.31f, 0),
            isOther = true,
        };
        DataManager.Manager<UIPanelManager>().ShowPanel(PanelID.GridStrengthenSuitPanel, data: param);
    }

    void onClick_BtnColorSuitActive_Btn(GameObject caster)
    {
        SuitPanelParam param = new SuitPanelParam()
        {
            m_type = SuitPanelParam.SuitType.Color,
            vec = new Vector3(133.8f, 201.31f, 0),
            isOther = true,
        };
        DataManager.Manager<UIPanelManager>().ShowPanel(PanelID.GridStrengthenSuitPanel, data: param);
    }

    void onClick_BtnStoneSuitNormal_Btn(GameObject caster)
    {
        SuitPanelParam param = new SuitPanelParam()
        {
            m_type = SuitPanelParam.SuitType.Stone,
            vec = new Vector3(133.8f, 201.31f, 0),
            isOther = true,
        };
        DataManager.Manager<UIPanelManager>().ShowPanel(PanelID.GridStrengthenSuitPanel, data: param);
    }

    void onClick_BtnStoneSuitActive_Btn(GameObject caster)
    {
        SuitPanelParam param = new SuitPanelParam()
        {
            m_type = SuitPanelParam.SuitType.Stone,
            vec = new Vector3(133.8f, 201.31f, 0),
            isOther = true,
        };
        DataManager.Manager<UIPanelManager>().ShowPanel(PanelID.GridStrengthenSuitPanel, data: param);
    }
}
