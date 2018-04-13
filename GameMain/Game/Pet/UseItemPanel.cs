//-----------------------------------------
//此文件自动生成，请勿手动修改
//生成日期: 6/25/2016 2:13:44 PM
//-----------------------------------------
//using UnityEngine;
//using Client;
//using System.Collections.Generic;
//using table;
//using GameCmd;
//using Engine.Utility;
//using System.Collections;
//public enum UseItemType
//{
//    Exp,
//    Life,
//}
//partial class UseItemPanel : ITimer
//{
//    List<UIWidget> widgetList = new List<UIWidget>();

//    private readonly uint m_uUseItemTimerID =1000;
//    PetDataManager petDataManager
//    {
//        get
//        {
//            return DataManager.Manager<PetDataManager>();
//        }
//    }
//    public IPet CurPet
//    {
//        get
//        {
//            return petDataManager.CurPet;
//        }
//    }
//    uint m_nItemID;
//    protected override void OnInitPanelData()
//    {
//        base.OnInitPanelData();
//        panelBaseData.m_em_colliderMode = UIDefine.UIPanelColliderMode.Normal;
//    }
//    public override void OnColliderMaskClicked()
//    {
//        HideSelf();
//        base.OnColliderMaskClicked();
//    }
//    public override void OnLoading()
//    {
//        base.OnLoading();
 
//        petDataManager.ValueUpdateEvent += OnValueUpdateEventArgs;
//    }
//    protected override void OnPanelBaseDestory()
//    {
//        base.OnPanelBaseDestory();
//        petDataManager.ValueUpdateEvent -= OnValueUpdateEventArgs;
//    }

//    void OnValueUpdateEventArgs(object obj, ValueUpdateEventArgs v)
//    {
//        RefreshData();

//    }
//    bool bStop = false;
//    void LongClick(GameObject go, bool state)
//    {
//        0-3s 300ms一次 超过3s 200ms一次
//        if (CurPet == null)
//            return;
//        Transform item = go.transform.parent;
//        string name = item.name;
//        char c = name[name.Length - 1];
//        int num = 0;
//        if (int.TryParse(c.ToString(), out num))
//        {
//            num -= 1;
//            if (num < itemIDList.Count)
//            {
//                m_nItemID = itemIDList[num];
//            }
//        }


//        if (state)
//        {
//            SendNetMessage();
//            bStop = false;
//            StartCoroutine(StopTimer());
//            TimerAxis.Instance().SetTimer(m_uUseItemTimerID, 300, this);
//        }
//        else
//        {
//            bStop = true;
//            TimerAxis.Instance().KillTimer(m_uUseItemTimerID, this);
//        }
//    }
//    bool bHasDanyao()
//    {
//        int itemCount = DataManager.Manager<ItemManager>().GetItemNumByBaseId(m_nItemID);
//        if (itemCount <= 0)
//        {
//            return false;
//        }
//        return true;
//    }
//    bool IsCanUpLevel()
//    {
//        if (m_type == UseItemType.Exp)
//        {
//            int level = ClientGlobal.Instance().MainPlayer.GetProp((int)CreatureProp.Level);
//            int yinhunLevel = CurPet.GetProp((int)PetProp.YinHunLevel);
//            int totalLevel = level + yinhunLevel;
//            int petlevel = CurPet.GetProp((int)CreatureProp.Level);
//            if (petlevel > totalLevel)
//            {
//                return false;
//            }
//            else
//            {
//                return true;
//            }
//        }
//        return false;
//    }
//    IEnumerator StopTimer()
//    {
//        yield return new WaitForSeconds(3);
//        if (bStop)
//        {
//            TimerAxis.Instance().KillTimer(m_uUseItemTimerID, this);
//            yield return null;
//        }
//        else
//        {
//            TimerAxis.Instance().KillTimer(m_uUseItemTimerID, this);
//            TimerAxis.Instance().SetTimer(m_uUseItemTimerID, 200, this);
//        }


//    }
//    public void OnTimer(uint uTimerID)
//    {
//        if (uTimerID == m_uUseItemTimerID)
//        {
//            SendNetMessage();
//        }
//    }
//    void SendNetMessage()
//    {
//        if (!Check())
//            return;
//        if (m_type == UseItemType.Exp)
//        {

//            stAddExpPetUserCmd_C cmd = new stAddExpPetUserCmd_C();
//            cmd.id = CurPet.GetID();
//            cmd.item = m_nItemID;
//            NetService.Instance.Send(cmd);
//        }
//        if (m_type == UseItemType.Life)
//        {
//            stAddLifePetUserCmd_C cmd = new stAddLifePetUserCmd_C();
//            cmd.id = CurPet.GetID();
//            cmd.item = m_nItemID;
//            NetService.Instance.Send(cmd);
//        }
//    }

//    bool Check()
//    {
//        if (!bHasDanyao())
//        {
//            TipsManager.Instance.ShowTipsById(6);
//            TimerAxis.Instance().KillTimer(m_uUseItemTimerID, this);
//            return false;
//        }
//        if (m_type == UseItemType.Exp)
//        {
//            if (!IsCanUpLevel())
//            {
//                TipsManager.Instance.ShowTips(LocalTextType.Pet_Rank_zhanhundengjiyimanzanshiwufajixushengji);
//                TipsManager.Instance.ShowTipsById(108010);
//                TimerAxis.Instance().KillTimer(m_uUseItemTimerID, this);
//                return false;
//            }

//        }
//        if (m_type == UseItemType.Life)
//        {
//            int life = CurPet.GetProp((int)PetProp.Life);
//            int maxLife = GameTableManager.Instance.GetGlobalConfig<int>("MaxPetLife");
//            if (life >= maxLife)
//            {
//                TipsManager.Instance.ShowTips(LocalTextType.Pet_Age_zhanhunshoumingyiman);
//                TipsManager.Instance.ShowTipsById(108016);
//                return false;
//            }
//        }
//        return true;
//    }
//    void UseItem(GameObject go)
//    {
//        if (CurPet == null)
//            return;
//        if (!Check())
//            return;
//        Transform item = go.transform.parent;
//        string name = item.name;
//        char c = name[name.Length - 1];
//        int num = 0;
//        if (int.TryParse(c.ToString(), out num))
//        {
//            num -= 1;
//            if (num < itemIDList.Count)
//            {
//                uint itemID = itemIDList[num];

//                if (m_type == UseItemType.Exp)
//                {
//                    stAddExpPetUserCmd_C cmd = new stAddExpPetUserCmd_C();
//                    cmd.id = CurPet.GetID();
//                    cmd.item = itemID;
//                    NetService.Instance.Send(cmd);
//                }
//                if (m_type == UseItemType.Life)
//                {
//                    stAddLifePetUserCmd_C cmd = new stAddLifePetUserCmd_C();
//                    cmd.id = CurPet.GetID();
//                    cmd.item = itemID;
//                    NetService.Instance.Send(cmd);
//                }
//            }
//        }
//    }
//    void GetItem(GameObject go)
//    {
//        if (CurPet == null)
//            return;

//        Transform item = go.transform.parent;
//        string name = item.name;
//        char c = name[name.Length - 1];
//        int num = 0;
//        if (int.TryParse(c.ToString(), out num))
//        {
//            num -= 1;
//            if (num < itemIDList.Count)
//            {
//                uint itemID = itemIDList[num];
//                PetPanel panel = DataManager.Manager<UIPanelManager>().GetPanel<PetPanel>(PanelID.PetPanel);
//                GetWayPanel.GetWayParam data = new GetWayPanel.GetWayParam();
//                data.itemBaseId = itemID;
//                data.returnBackUIData = new ReturnBackUIData[2];
//                data.returnBackUIData[0] = new ReturnBackUIData();
//                data.returnBackUIData[0].msgid = UIMsgID.eShowUI;
//                data.returnBackUIData[0].panelid = PanelID.PetPanel;
//                data.returnBackUIData[0].param = new ReturnBackUIMsg()
//                {
//                    tabs = new int[1]{panel.RightToggleIndex },
//                    param = CurPet.GetID(),
//                };


//                data.returnBackUIData[1] = new ReturnBackUIData();
//                data.returnBackUIData[1].msgid = UIMsgID.eShowUI;
//                data.returnBackUIData[1].panelid = PanelID.Useitempanel;
//                data.returnBackUIData[1].param = m_type;
//                DataManager.Manager<UIPanelManager>().ShowPanel(PanelID.GetWayPanel, data: data);
//            }
//        }
 
//    }
//    UseItemType m_type;
//    protected override void OnShow(object data)
//    {
//        base.OnShow(data);
//        if (data == null)
//        {
//            Log.Error("useitem panel data is null");
//            m_type = UseItemType.Exp;
//        }
//        else
//        {
//            m_type = (UseItemType)data;
//        }

//        InitData();
//    }

//    public override bool OnMsg(UIMsgID msgid, object param)
//    {
//        if (msgid == UIMsgID.eShowUI)
//        {
//            OnShow((UseItemType)param);
//        }
//        return base.OnMsg(msgid, param);
//    }
//    void InitScrollView(int count)
//    {

//        NGUITools.DestroyChildren(m_grid_ItemGrid.transform);
    
//        widgetList.Clear();
//        for (int i = 0; i < count; i++)
//        {
//            GameObject item = NGUITools.AddChild(m_grid_ItemGrid.gameObject, m_widget_itemprefab.gameObject);
//            item.SetActive(true);
//            string name = string.Format("item_{0:D2}", i + 1);
//            item.name = name;
//            item.GetComponent<UIWidget>().depth = 3;
//            widgetList.Add(item.GetComponent<UIWidget>());
//            Transform btnUse = item.transform.Find("btn_use");
//            UIEventListener.Get(btnUse.gameObject).onPress = LongClick;
//            Transform btnGet = item.transform.Find("btn_get");
//            UIEventListener.Get(btnGet.gameObject).onClick = GetItem;
//        }
//        m_widget_itemprefab.gameObject.SetActive(false);
//        m_scrollview_ItemScrollView.ResetPosition();
//        m_grid_ItemGrid.repositionNow = true;
//    }
//    List<uint> itemIDList = new List<uint>();
//    void InitData()
//    {
//        if (CurPet == null)
//            return;

//        string str = "ExpItem";
//        if (m_type == UseItemType.Life)
//        {
//            str = "LifeItem";
//        }

//        List<string> itemList = GameTableManager.Instance.GetGlobalConfigKeyList(str);
//        itemIDList.Clear();
//        PetDataBase pdb = GameTableManager.Instance.GetTableItem<PetDataBase>(CurPet.PetBaseID);
//        if (pdb == null)
//        {
//            return;
//        }
//        uint carryLevel = (uint)petDataManager.GetCurPetLevel();
//        #region  从lua里面拿出来的数据无序 要先排序
//        List<uint> tempList = new List<uint>();
//        foreach (var strID in itemList)
//        {
//            uint id = uint.Parse(strID);
//            tempList.Add(id);
//        }
//        tempList.Sort((x1, x2) =>
//        {
//            if (x1 > x2)
//                return 1;
//            else if (x1 < x2)
//                return -1;
//            else
//                return 0;
//        });
    
    
//        #endregion
//            for (int i = 0; i < tempList.Count; i++)
//            {
//                uint id = tempList[i];
//                if (m_type == UseItemType.Life)
//                {
//                    int firstLevel = GameTableManager.Instance.GetGlobalConfig<int>("PetFirstLevel");
//                    int secondLevel = GameTableManager.Instance.GetGlobalConfig<int>("PetSecondLevel");
//                    if (carryLevel < firstLevel)
//                    {
//                        if (i == 0 || i == 3)
//                        {
//                            itemIDList.Add(id);
//                        }
//                    }
//                    else if (carryLevel > secondLevel)
//                    {
//                        if (i == 2 || i == 5)
//                        {
//                            itemIDList.Add(id);
//                        }
//                    }
//                    else
//                    {
//                        if (i == 1 || i == 4)
//                        {
//                            itemIDList.Add(id);
//                        }
//                    }
//                }
//                else
//                {
//                    itemIDList.Add(id);
//                }

//            }

//        itemIDList.Sort((x1, x2) =>
//        {
//            if (x1 > x2)
//                return 1;
//            else if (x1 < x2)
//                return -1;
//            else
//                return 0;
//        });
//        if (m_type == UseItemType.Exp)
//        {
//            List<uint> hasList = new List<uint>();
//            List<uint> noList = new List<uint>();
//            for (int i = 0; i < itemIDList.Count; i++)
//            {
//                uint id = itemIDList[i];
//                int count = DataManager.Manager<ItemManager>().GetItemNumByBaseId(id);
//                if (count != 0)
//                {
//                    hasList.Add(id);
//                }
//                else
//                {
//                    noList.Add(id);
//                }
//            }
//            itemIDList.Clear();
//            itemIDList.AddRange(hasList);
//            itemIDList.AddRange(noList);
//        }
//        InitScrollView(itemIDList.Count);
//        foreach(var widget in widgetList)
//        {
//            widget.gameObject.SetActive( false );
//        }
//        RefreshData();
//    }
//    void RefreshData()
//    {
//        for (int i = 0; i < itemIDList.Count; i++)
//        {
//            uint itemId = itemIDList[i];
//            ItemDataBase db = GameTableManager.Instance.GetTableItem<ItemDataBase>(itemId);
//            if (db != null)
//            {
//                if (i >= widgetList.Count)
//                    return;
//                UIWidget widget = widgetList[i];
//                if (widget == null)
//                    break;

//                Transform iconTrans = widget.transform.Find("icon");
//                int itemCount = DataManager.Manager<ItemManager>().GetItemNumByBaseId(itemId);
         
//                UIItem.AttachParent(iconTrans.transform, itemId, (uint)itemCount);
//                Transform btnUse = widget.transform.Find("btn_use");
//                Transform btnGet = widget.transform.Find("btn_get");
//                if (itemCount <= 0)
//                {
//                    btnGet.gameObject.SetActive(true);
//                    btnUse.gameObject.SetActive(false);
//                }
//                else
//                {
//                    btnGet.gameObject.SetActive(false);
//                    btnUse.gameObject.SetActive(true);
//                }
//                Transform nameTrans = widget.transform.Find("name");
//                UILabel nameLabel = nameTrans.GetComponent<UILabel>();
//                if (nameLabel != null)
//                {
//                    nameLabel.text = db.itemName;
//                }

//                Transform resultTrans = widget.transform.Find("result");
//                UILabel desLabel = resultTrans.GetComponent<UILabel>();
//                if (desLabel != null)
//                {
//                    desLabel.text = db.description;
//                }
//            }

//        }
//    }

//    void onClick_Btn_close_Btn(GameObject caster)
//    {
//        HideSelf();
//    }

//    void onClick_Btn_unclose_Btn(GameObject caster)
//    {

//    }



//}
