//*************************************************************************
//	创建日期:	2017-1-17 14:23
//	文件名称:	MainUsePanel.cs
//  创 建 人:   	Chengxue.Zhao
//	版权所有:	中青宝
//	说    明:	任务使用道具面板
//*************************************************************************
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Client;


public delegate void MainUsePanelItemOnClick();
public class MainUsePanelData
{
    public uint type;       //类型  1为npc  2为item
    public uint Id;
    public MainUsePanelItemOnClick onClick;
}

partial class MainUsePanel : UIPanelBase
{
    uint m_nitemThisId;
    uint m_nitemBaseId;

    Vector3 startPos;
    int m_nRang = 9;

    const float CLOSE_TIME = 3f;

    string name;

    MainUsePanelData m_mainUsePanelData;


    protected override void OnLoading()
    {
        base.OnLoading();
        UIEventListener.Get(m_sprite_iconBg.gameObject).onClick = onClick_Btn_use_Btn;
    }

    protected override void OnShow(object data)
    {
        base.OnShow(data);
        Engine.Utility.EventEngine.Instance().DispatchEvent((int)Client.GameEventID.TASK_MAIN_ARROWHIDE, null);
        //if (DataManager.Manager<TaskDataManager>().IsShowSlider)
        if (DataManager.Manager<SliderDataManager>().IsReadingSlider)
        {
            this.HideSelf();
            return;
        }
        startPos = Client.ClientGlobal.Instance().MainPlayer.GetPos();
        m_nRang = GameTableManager.Instance.GetGlobalConfig<int>("Task_UseItem_Rang");
        m_nRang *= m_nRang;

        /*       if (data is uint)
               {
                   ShowItemInfo((uint)data);
               }
         */

        //新的
        this.m_mainUsePanelData = data as MainUsePanelData;
        if (this.m_mainUsePanelData != null)
        {
            ShowItemInfo(this.m_mainUsePanelData);
        }
    }
    private CMResAsynSeedData<CMTexture> m_iconCASD = null;
    void ShowItemInfo(MainUsePanelData data)
    {
        /*
        List<BaseItem> lstItems = DataManager.Manager<ItemManager>().GetItemByBaseId(itemBaseid);
        if (lstItems.Count <= 0)
        {
            m_nitemThisId = 0;
            m_nitemBaseId = 0;
            return;
        }
        m_nitemThisId = lstItems[0].QWThisID;
        m_nitemBaseId = itemBaseid;
         */

        //npc
        if (data.type == 1)
        {
            m__icon.gameObject.SetActive(false);
            m_sprite_iconCollect.gameObject.SetActive(true);
            Client.IEntitySystem es = Client.ClientGlobal.Instance().GetEntitySystem();
            if (es == null)
            {
                return;
            }

            INPC npc = es.FindNPC(data.Id);
            if (npc == null)
            {
                return;
            }

            //this.name = npc.GetName();
            this.name = "采集";
            m_label_item_name.text = string.Format("{0}[ffd966]({1})[-]", this.name, CLOSE_TIME);
        }
        //item
        else if (data.type == 2)
        {
            m__icon.gameObject.SetActive(true);
            m_sprite_iconCollect.gameObject.SetActive(false);
            table.ItemDataBase itemdata = GameTableManager.Instance.GetTableItem<table.ItemDataBase>(data.Id);
            if (itemdata != null)
            {
                UIManager.GetTextureAsyn(itemdata.itemIcon, ref m_iconCASD, () =>
                {
                    if (null != m__icon)
                    {
                        m__icon.mainTexture = null;
                    }
                }, m__icon);

                //this.name = itemdata.itemName;
                this.name = "使用";
                m_label_item_name.text = string.Format("{0}[ffd966]({1})[-]", this.name, CLOSE_TIME);
            }
        }

        //特效
        UIParticleWidget wight = m_sprite_iconBg.GetComponent<UIParticleWidget>();
        if (null == wight)
        {
            wight = m_sprite_iconBg.gameObject.AddComponent<UIParticleWidget>();
            wight.depth = 100;
        }

        if (wight != null)
        {
            UIParticleWidget p = wight.GetComponent<UIParticleWidget>();
            if (p == null)
            {
                p = wight.gameObject.AddComponent<UIParticleWidget>();
                p.depth = 20;
            }
            if (p != null)
            {

                p.SetDimensions(1, 1);
                p.ReleaseParticle();
                p.AddParticle(50019);
            }      
        }

        StartCoroutine(WaitToClose());
    }

    protected override void OnHide()
    {
        base.OnHide();
        Release();
    }

    public override void Release(bool depthRelease = true)
    {
        base.Release(depthRelease);
        if (null != m_iconCASD)
        {
            m_iconCASD.Release(true);
            m_iconCASD = null;
        }
    }


    IEnumerator WaitToClose()
    {
        float tempTime = 0;
        m_sprite_slider.fillAmount = 0f;
        while (tempTime < CLOSE_TIME)
        {
            tempTime += Time.deltaTime;
            //m_sprite_slider.fillAmount = (CLOSE_TIME - tempTime) / CLOSE_TIME;
            m_label_item_name.text = string.Format("{0}[ffd966]({1})[-]", this.name, (int)(CLOSE_TIME - tempTime + 1));

            yield return null;
        }
        onClick_Btn_use_Btn(null);
    }

    void onClick_Btn_use_Btn(GameObject caster)
    {
       
        StopAllCoroutines();

        if (this.m_mainUsePanelData != null && this.m_mainUsePanelData.onClick != null)
        {
            this.m_mainUsePanelData.onClick.Invoke();
        }

        this.HideSelf();

        /* Client.IEntity player = Client.ClientGlobal.Instance().MainPlayer;
         if (player == null)
         {
             return;
         }

         bool ismoving = (bool)player.SendMessage(EntityMessage.EntityCommand_IsMove, null);
         if (ismoving)
         {
             player.SendMessage(EntityMessage.EntityCommand_StopMove,player.GetPos());
         }

         DataManager.Manager<RideManager>().TryUnRide(
             (obj) =>
             {
                 Protocol.Instance.RequestUseItem(m_nitemThisId);
             }, 
             null);*/
    }

    void Update()
    {
        if (!this.Visible)
        {
            return;
        }
        IPlayer player =  Client.ClientGlobal.Instance().MainPlayer;
        if(player != null)
        {
            Vector3 pos = player.GetPos();
            //超出范围关闭界面
            if ((pos - startPos).sqrMagnitude > m_nRang)
            {
                StopAllCoroutines();
                this.HideSelf();
            }
        }
       
    }
}