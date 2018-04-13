using Engine.Utility;
using GameCmd;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using table;
using UnityEngine;


class UIShortcutItemGrid : UIGridBase, ITimer
{

    UITexture icon;
    UILabel num;
    UISprite overlay;
    UILabel timeLbl;
    GameObject m_goAdd;
    UISprite m_spBorder;

    public ShortCuts item;

    private readonly uint ITEMCD_TIMERID = 1000;

    private CMResAsynSeedData<CMAtlas> iuiBorderAtlas = null;

    CMResAsynSeedData<CMTexture> m_iconTextureSD = null;

    #region override

    public override void Release(bool depthRelease = true)
    {
        base.Release(depthRelease);

        if (null != iuiBorderAtlas)
        {
            iuiBorderAtlas.Release(true);
            iuiBorderAtlas = null;
        }
        if (m_iconTextureSD != null)
        {
            m_iconTextureSD.Release(depthRelease);
            m_iconTextureSD = null;
        }
    }

    protected override void OnUIBaseDestroy()
    {
        base.OnUIBaseDestroy();
        Release();
    }

    protected override void OnAwake()
    {
        base.OnAwake();

        icon = this.transform.Find("FixedIcon").GetComponent<UITexture>();
        m_spBorder = this.transform.Find("FixedIcon/Border").GetComponent<UISprite>();
        num = this.transform.Find("FixedNum").GetComponent<UILabel>();
        overlay = this.transform.Find("Overlay").GetComponent<UISprite>();
        timeLbl = this.transform.Find("Overlay/TimeLbl").GetComponent<UILabel>();
        m_goAdd = this.transform.Find("FixedAdd").gameObject;
    }

    public override void SetGridData(object data)
    {
        base.SetGridData(data);

        this.item = data as ShortCuts;

        if (this.item == null)
        {
            Engine.Utility.Log.Error("item 数据为 null ");
            return;
        }

        ItemDataBase itemDataBase = GameTableManager.Instance.GetTableItem<ItemDataBase>(this.item.itemid);

        if (itemDataBase == null)
        {
            icon.gameObject.SetActive(false);
            num.gameObject.SetActive(false);
            m_goAdd.SetActive(true);
            return;
        }

        //icon
        if (icon != null)
        {
            BaseItem baseItem = DataManager.Manager<ItemManager>().GetTempBaseItemByBaseID<BaseItem>(this.item.itemid);
            if (baseItem == null)
            {
                return;
            }
            UIManager.GetTextureAsyn(baseItem.Icon, ref m_iconTextureSD, () =>
            {
                if (null != icon)
                {
                    icon.mainTexture = null;
                }
            }, icon, makeperfect: false);

            icon.gameObject.SetActive(true);
            m_goAdd.SetActive(false);
            icon.SetDimensions(60, 60);
            UIManager.GetAtlasAsyn(UIManager.GetIconName(baseItem.BorderIcon, true), ref iuiBorderAtlas, () =>
            {
                if (m_spBorder != null)
                {
                    m_spBorder.atlas = null;
                }
            }, m_spBorder, false);
        }

        //num
        int itemCount = DataManager.Manager<ItemManager>().GetItemNumByBaseId(this.item.itemid);//道具存量
        if (num != null)
        {
            num.gameObject.SetActive(true);
            num.text = itemCount.ToString();

            if (itemCount == 0)
            {
                num.color = Color.red;
            }
            else
            {
                num.color = Color.white;
            }
        }
    }



    void SetCDState(bool b)
    {
        if (overlay != null && overlay.gameObject.activeSelf != b)
        {
            overlay.gameObject.SetActive(b);
        }
    }

    void OnEnable()
    {
        Engine.Utility.EventEngine.Instance().AddEventListener((int)Client.GameEventID.UIEVENT_USEITEMCD, DoGameEvent); // 创建实体
    }

    void OnDisable()
    {
        overlay.fillAmount = 0;
        timeLbl.text = "";
        Engine.Utility.EventEngine.Instance().RemoveEventListener((int)Client.GameEventID.UIEVENT_USEITEMCD, DoGameEvent); // 创建实体
    }

    void DoGameEvent(int EventId, object param)
    {
        if ((int)Client.GameEventID.UIEVENT_USEITEMCD == EventId)
        {
            if (this.item == null)
            {
                Engine.Utility.Log.Error("item 数据为 null ");
                return;
            }

            Client.stUseItemCD useItemCD = (Client.stUseItemCD)param;
            ItemManager.CDData cdData;
            if (DataManager.Manager<ItemManager>().TryGetItemCDByBaseId(this.item.itemid, out cdData))
            {
                TimerAxis.Instance().KillTimer(ITEMCD_TIMERID, this);
                TimerAxis.Instance().SetTimer(ITEMCD_TIMERID, 30, this);
            }
        }
    }

    public void OnTimer(uint uTimerID)
    {
        if (uTimerID == ITEMCD_TIMERID)
        {
            if (overlay == null)
            {
                return;
            }

            if (timeLbl == null)
            {
                return;
            }

            if (this.item == null)
            {
                return;
            }

            ItemManager.CDData cdData;
            if (DataManager.Manager<ItemManager>().TryGetItemCDByBaseId(this.item.itemid, out cdData))
            {
                SetCDState(true);
                overlay.fillAmount = cdData.remainCD / cdData.totalCD;
                timeLbl.text = ((int)cdData.remainCD + 1).ToString();
            }
            else
            {
                SetCDState(false);
                overlay.fillAmount = 0;
                timeLbl.text = "";
                TimerAxis.Instance().KillTimer(ITEMCD_TIMERID, this);
            }
        }
    }
    #endregion
}

