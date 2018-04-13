using UnityEngine;
using System.Collections.Generic;
using System.Collections;

partial class EmojiPanel : UIPanelBase
{
    enum ToggleEnum
    {
        None = 0,
        Emoji,
        Wear,
        Bag,
        Pet,
        Mount,
        RedEnvelope,
        Max,
    }
    private bool initGridCreator = false;
    UIGridCreatorBase gridCreator = null;
    //pet mount
    private List<ItemDefine.UIItemCommonData> m_lstUIItemCommondata = new List<ItemDefine.UIItemCommonData>();
    UIToggle m_btnEmoji;
    IChatInput m_currInputPanel;

    private ToggleEnum m_CurrToggleEnum = ToggleEnum.None;
    private ToggleEnum m_PreToggleEnum = ToggleEnum.None;

    protected override void OnAwake()
    {
        base.OnAwake();

        UIToggle[] toggles = m_trans_btn.GetComponentsInChildren<UIToggle>();
        foreach (var item in toggles )
        {
            if (item.name == "btn_emoji")
            {
                m_btnEmoji = item;
            }
            EventDelegate.Add(item.onChange, () =>
            {
                if (item.value)
                {
                    this.OnToggle(item.name);
                }
            });
        }

        UIEventListener.Get(m_widget_btn_close.gameObject).onClick = onClick_BtnClose_Btn;
        UIEventListener.Get(m_widget_btn_close2.gameObject).onClick = onClick_BtnClose_Btn;

        if (!initGridCreator)
        {
            gridCreator = m_trans_ItemGridScrollView.gameObject.AddComponent<UIGridCreatorBase>();

            if (gridCreator != null)
            {
                GameObject uiitemcommongridObj = UIManager.GetResGameObj(GridID.Uiitemcommongrid) as GameObject;

                gridCreator.gridContentOffset = new Vector2(-445, 80);
                gridCreator.arrageMent = UIGridCreatorBase.Arrangement.Horizontal;
                gridCreator.gridWidth = 89;
                gridCreator.gridHeight = 89;
                gridCreator.rowcolumLimit = 11;
                gridCreator.RefreshCheck();
                gridCreator.Initialize<UIItemCommonGrid>(uiitemcommongridObj, OnUpdateGridData, OnUIEventCallback);
            }
        }
        m_trans_Root.parent.gameObject.SetActive(true);
    }


    void OnToggle(string name)
    {
        Debug.Log(m_trans_Root.parent);
        m_trans_Root.parent.gameObject.SetActive(name == "btn_emoji");

        m_PreToggleEnum = m_CurrToggleEnum;
        if (name =="btn_emoji")
        {
            m_CurrToggleEnum = ToggleEnum.Emoji;
            m_trans_ItemGridScrollView.gameObject.SetActive(false);
            ShowEmoji();
        }
        else if (name == "btn_bag")
        {
            m_CurrToggleEnum = ToggleEnum.Bag;

            if (!m_trans_ItemGridScrollView.gameObject.activeSelf)
            {
                m_trans_ItemGridScrollView.gameObject.SetActive(true);
            }

            ShowItems();
        }else if (name.Equals("btn_wear"))
        {
            m_CurrToggleEnum = ToggleEnum.Wear;

            if (!m_trans_ItemGridScrollView.gameObject.activeSelf)
            {
                m_trans_ItemGridScrollView.gameObject.SetActive(true);
            }

            ShowEquips();
        }
        else if (name.Equals("btn_mount"))
        {
            m_CurrToggleEnum = ToggleEnum.Mount;

            if (!m_trans_ItemGridScrollView.gameObject.activeSelf)
            {
                m_trans_ItemGridScrollView.gameObject.SetActive(true);
            }
            ShowMounts();
        }
        else if (name.Equals("btn_pet"))
        {
            m_CurrToggleEnum = ToggleEnum.Pet;
            if (!m_trans_ItemGridScrollView.gameObject.activeSelf)
            {
                m_trans_ItemGridScrollView.gameObject.SetActive(true);
            }
            ShowPets();
        }
        else if(name == "btn_hongbao")
        {
            HideSelf();
           
            DataManager.Manager<UIPanelManager>().ShowPanel(PanelID.RedEnvelopePanel, panelShowAction: (panel) => {
                DataManager.Manager<UIPanelManager>().HidePanel(PanelID.ChatPanel);
            });
        }
    }

    protected override void OnShow(object data)
    {
        base.OnShow(data);

       // if (m_btnEmoji.value == false)
        //{
        //    m_btnEmoji.gameObject.SendMessage("OnClick", SendMessageOptions.DontRequireReceiver);
        //}
        m_btnEmoji.value = true;
        m_CurrToggleEnum = ToggleEnum.Emoji;
        m_trans_ItemGridScrollView.gameObject.SetActive(false);
        ShowEmoji();
        if (data is IChatInput)
        {
            m_currInputPanel = (IChatInput)data;
        }
    }

    int SortByName(UISpriteData a, UISpriteData b) { return string.Compare(a.name, b.name); }

    private void OnGetEmojiAtlas(IUIAtlas cmatlas,object param1,object param2,object param3)
    {
        if (null == cmatlas)
            return;
        UIAtlas atlas = cmatlas.GetAtlas();
        if (null == atlas)
        {
            return;
        }

        List<UISpriteData> spritedata = atlas.spriteList;
        spritedata.Sort(SortByName);

        Dictionary<string, string> spriteDic = new Dictionary<string, string>();
        List<string> spriteNameList = new List<string>();

        foreach (var item in spritedata)
        {
            int index = item.name.LastIndexOf('_');//3_3
            if (index != -1)
            {
                string strtype = item.name.Substring(0, index);
                if (spriteDic.ContainsKey(strtype) == false)
                {
                    spriteNameList.Add(strtype + "_1");
                    spriteDic[strtype] = strtype + "_1";
                }
            }
        }

        Debug.Log("emoji count  :" + spriteDic.Count);

        //设置最大表情数
        DataManager.Manager<ChatDataManager>().EmojiMaxNum = spriteDic.Count;

        for (int i = 0; i < spriteNameList.Count; i++)
        {
            int x = i / 4;
            int y = i % 4;

            GameObject go = GameObject.Instantiate(m_sprite_Emoji.gameObject) as GameObject;
            go.transform.parent = m_trans_Root;
            go.transform.localScale = Vector3.one;
            go.transform.localPosition = new Vector3(x * 60, y * (-60), 0);
            UIEventListener.Get(go).onClick = OnClickFaceItem;

            UISprite sprite = go.GetComponent<UISprite>();
            sprite.atlas = atlas;
            sprite.name = spriteNameList[i];
            sprite.spriteName = spriteNameList[i];

            sprite.MakePixelPerfect();
            if (sprite.width < 50)
            {
                sprite.width = 50;
            }
            if (sprite.height < 50)
            {
                sprite.height = 50;
            }
        }
    }

    private void OnEmojiRelease()
    {
        m_trans_Root.DestroyChildren();
    }

    CMResAsynSeedData<CMAtlas> m_emjCASD = null;


    void ShowEmoji()
    {
        if (null == m_emjCASD)
        {
            m_emjCASD = new CMResAsynSeedData<CMAtlas>(OnEmojiRelease);
        }
        UIManager.GetAtlasAsyn((uint)AtlasID.Emojiatlas, ref m_emjCASD, OnGetEmojiAtlas);        
    }



    void OnClickFaceItem(GameObject go)
    {
        int index = go.name.IndexOf('_');
        if (index != -1 )
        {
            string strtype = go.name.Substring(0,index);
            if (m_currInputPanel != null)
            {
                m_currInputPanel.AppendText("#" + strtype+"#");
            }
        }
    }

    void onClick_BtnClose_Btn(GameObject caster)
    {
        HideSelf();
    }

    protected override void OnHide()
    {
        if (m_currInputPanel != null)
        {
            m_currInputPanel.ResetPos();
        }
        Release();
    }

    public override void Release(bool depthRelease = true)
    {
        base.Release(depthRelease);
        if (null != m_emjCASD)
        {
            m_emjCASD.Release(true);
            m_emjCASD = null;
        }
    }



    private void ShowItems()
    {
        //获取数据
        List<uint> knapsackPackData = DataManager.Manager<ItemManager>().GetItemDataByPackageType(GameCmd.PACKAGETYPE.PACKAGETYPE_MAIN);

        m_lstUIItemCommondata.Clear();
        for (int i = 0; i < knapsackPackData.Count; i++)
        {
            BaseItem baseItem = DataManager.Manager<ItemManager>().GetBaseItemByQwThisId(knapsackPackData[i]);
            if (baseItem != null)
            {
                m_lstUIItemCommondata.Add(
                    new ItemDefine.UIItemCommonData()
                    {
                        IconName = baseItem.BaseData.itemIcon,
                        ShowGetWay = false,
                        Qulity = baseItem.BaseData.quality,
                        ItemThisId = knapsackPackData[i],
                        Name = baseItem.BaseData.itemName,
                    });
            }
        }
        //生成UI
        gridCreator.CreateGrids(m_lstUIItemCommondata.Count);
    }

    private void ShowEquips()
    {
        //获取数据
        List<uint> knapsackPackData = DataManager.Manager<EquipManager>().GetWearEquips();

        m_lstUIItemCommondata.Clear();
        for (int i = 0; i < knapsackPackData.Count; i++)
        {
            BaseItem baseItem = DataManager.Manager<ItemManager>().GetBaseItemByQwThisId(knapsackPackData[i]);

            if (baseItem != null)
            {
                m_lstUIItemCommondata.Add(
                    new ItemDefine.UIItemCommonData()
                    {
                        IconName = baseItem.BaseData.itemIcon,
                        ShowGetWay = false,
                        Qulity = baseItem.BaseData.quality,
                        ItemThisId = knapsackPackData[i],
                        Name = baseItem.BaseData.itemName,
                    });
            }
        }
        //生成UI
        gridCreator.CreateGrids(m_lstUIItemCommondata.Count);
    }
    void ShowMounts()
    {
        //获取数据
        List<RideData> rideList = DataManager.Manager<RideManager>().GetRideList();

        m_lstUIItemCommondata.Clear();
        if (null != rideList && rideList.Count > 0)
        {
            for (int i = 0; i < rideList.Count; i++)
            {
                m_lstUIItemCommondata.Add(
                    new ItemDefine.UIItemCommonData()
                    {
                        IconName = rideList[i].icon,
                        ShowGetWay = false,
                        Qulity = rideList[i].quality,
                        ItemThisId = rideList[i].id,
                        Name = rideList[i].name,
                    });
            }
        }

        //生成UI
        gridCreator.CreateGrids(m_lstUIItemCommondata.Count);
    }

    void ShowPets()
    {
        //获取数据
        Dictionary<uint, Client.IPet> petDic = DataManager.Manager<PetDataManager>().GetPetDic();

        m_lstUIItemCommondata.Clear();
        if (null != petDic && petDic.Count > 0)
        {
            foreach (var item in petDic.Values)
            {
                table.PetDataBase petdb = DataManager.Manager<PetDataManager>().GetPetDataBase(item.PetBaseID);
                if (petdb != null)
                {
                    m_lstUIItemCommondata.Add(
                    new ItemDefine.UIItemCommonData()
                    {
                        IconName = petdb.icon,
                        ShowGetWay = false,
                        Qulity = petdb.petQuality,
                        ItemThisId = item.GetID(),
                        Name = petdb.petName,
                    });
                }
            }
        }

        //生成UI
        gridCreator.CreateGrids(m_lstUIItemCommondata.Count);
    }

    private void OnUpdateGridData(UIGridBase grid, int index)
    {
        UIItemCommonGrid itemGrid = grid as UIItemCommonGrid;

        if (itemGrid != null)
        {
            if (m_lstUIItemCommondata != null && m_lstUIItemCommondata.Count > index && index >= 0)
            {
                itemGrid.SetGridData(m_lstUIItemCommondata[index]);
            }
        }
    }

    private void OnUIEventCallback(UIEventType eventType, object data, object param)
    {
        switch (eventType)
        {
            case UIEventType.Click:
                {
                     if (data is UIItemCommonGrid)
                    {
                        UIItemCommonGrid grid = data as UIItemCommonGrid;
                        if (grid != null)
                        {
                            if (m_currInputPanel != null)
                            {   
                                //1 物品 2 坐骑 3 宠物
                                int type = 2;
                                if (m_CurrToggleEnum == ToggleEnum.Pet)
                                {
                                    type = 3;
                                }else if (m_CurrToggleEnum == ToggleEnum.Mount)
                                {
                                    type = 2;
                                }else if (m_CurrToggleEnum == ToggleEnum.Wear || m_CurrToggleEnum == ToggleEnum.Bag)
                                {
                                    type = 1;
                                }
                                m_currInputPanel.AddLinkerItem(grid.Data.Name, grid.Data.ItemThisId, grid.Data.Qulity, type );
                            }
                        }
                    }
                }
                break;
        }
    }

//     private void OnUpdateMountGridData(UIGridBase grid, int index)
//     {
//         UIItemCommonGrid itemGrid = grid as UIItemCommonGrid;
// 
//         if (itemGrid != null)
//         {
//             if (m_lstUIItemCommondata != null && m_lstUIItemCommondata.Count > index && index >= 0)
//             {
//                 itemGrid.SetGridData(m_lstUIItemCommondata[index]);
//             }
//         }
//     }
}
