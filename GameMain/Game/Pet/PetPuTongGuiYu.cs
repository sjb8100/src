//-----------------------------------------
//此文件自动生成，请勿手动修改
//生成日期: 6/25/2016 2:13:33 PM
//-----------------------------------------
using UnityEngine;
using Client;
using table;
using GameCmd;
using Engine.Utility;
using System.Collections.Generic;
using DG.Tweening;
partial class PetPanel : ITimer
{

    bool autoPrefect = false;
    bool autoConsume = false;
    uint m_nItemID = 0;
    private readonly uint m_uGuiyuanTimerID = 1234;

    void InitPutongGuiYuanUI()
    {

        UIToggle wanmei = m_btn_guiyuanCommon_wanmei.GetComponent<UIToggle>();
        if(wanmei != null)
        {
            wanmei.value = autoPrefect;
        }
        UIToggle zidong = m_btn_guiyuanCommon_zidongbuzu.GetComponent<UIToggle>();
        if(zidong != null)
        {
            zidong.value = autoConsume;
        }
    
        if(m_trans_putongSliderContainer.gameObject.GetComponent<PetSliderGroup>() == null)
        {
            m_trans_putongSliderContainer.gameObject.AddComponent<PetSliderGroup>();
        }

        if (m_trans_gaojiSliderContainer.gameObject.GetComponent<PetGuiYuanHouSliderGroup>() == null)
        {
            m_trans_gaojiSliderContainer.gameObject.AddComponent<PetGuiYuanHouSliderGroup>();
        }
        InitData();
    }
    void OnPutongGuiYuanEvent(ValueUpdateEventArgs v)
    {
        InitData();
        if (v != null)
        {
            if (v.key == PetDispatchEventString.PetGuiYuanSucess.ToString())
            {
               
                bool bAdv = (bool)v.newValue;
                if (bAdv)
                {
                    ShowGaojiContainer(true);
                }
                else
                {
                    ShowGaojiContainer(false);
                }
            }
          
        }

    }
    void OnGuiYuanUIRelease()
    {
        if(null != m_guiyuantopIconAsynSeed)
        {
            m_guiyuantopIconAsynSeed.Release();
            m_guiyuantopIconAsynSeed = null;
        }
        if (null != m_guiyuanQualityAsynSeed)
        {
            m_guiyuanQualityAsynSeed.Release();
            m_guiyuanQualityAsynSeed = null;
        }
    }
    CMResAsynSeedData<CMTexture> m_guiyuantopIconAsynSeed = null;
    CMResAsynSeedData<CMAtlas> m_guiyuanQualityAsynSeed = null;
    void InitData()
    {
        if (CurPet == null)
        {
          //  Log.Error(" curpet is null");
            return;
        }
        PetDataBase pdb = petDataManager.GetPetDataBase(CurPet.PetBaseID);
        if (pdb == null)
        {
            Log.Error("pdb is null id is " + CurPet.PetBaseID.ToString());
            return;
        }
       // DataManager.Manager<UIManager>().SetSpriteDynamicIcon(m_sprite_guiyuantopicon, pdb.icon);
        UIManager.GetTextureAsyn(pdb.icon, ref m_guiyuantopIconAsynSeed, () =>
        {
            if (null != m__guiyuantopicon)
            {
                m__guiyuantopicon.mainTexture = null;
            }
        }, m__guiyuantopicon, false);
        Transform bgTrans = m__guiyuantopicon.transform.Find("pingzhi_box");
        if (bgTrans)
        {
            UISprite bgSpr = bgTrans.GetComponent<UISprite>();
            if (bgSpr)
            {
            //    string bgSpriteName = ItemDefine.GetItemBorderIcon(pdb.flag);
            //    DataManager.Manager<UIManager>().SetSpriteDynamicIcon(bgSpr, bgSpriteName);
                UIManager.GetQualityAtlasAsyn(pdb.flag, ref m_guiyuanQualityAsynSeed, () =>
                {
                    if (null != bgSpr)
                    {
                        bgSpr.atlas = null;
                    }
                }, bgSpr);
            }
        }
        int status = CurPet.GetProp((int)PetProp.PetGuiYuanStatus);
        PetGuiYuanDataBase guiyuandb = GameTableManager.Instance.GetTableItem<PetGuiYuanDataBase>(pdb.petQuality, status);
        if (guiyuandb != null)
        {
            m_label_guiyuanCommon_xiaohaogold.text = guiyuandb.consumeMoney.ToString();
        }
        int secondKey = 0;
        if(status == (int)PetGrowState.Perfect)
        {
            secondKey = 1;
        }
        List<uint> itemList = GameTableManager.Instance.GetGlobalConfigList<uint>("GYItem", secondKey.ToString());

        if (pdb == null)
        {
            Log.Error(" petdata base is null baseid is " + CurPet.PetBaseID.ToString());
            return;
        }
        uint itemID = 0;
        uint qua = pdb.petQuality;
        if (qua == 8)
        {
            itemID = itemList[1];
        }
        else
        {
            itemID = itemList[0];
        }

        m_nItemID = itemID;
        int itemCount = DataManager.Manager<ItemManager>().GetItemNumByBaseId(itemID);
        ShowColdLabel(itemID);
        ItemDataBase db = GameTableManager.Instance.GetTableItem<ItemDataBase>(itemID);
        if (db != null)
        {
            m_label_guiyuanCommon_name.text = db.itemName;
        }

        UIItem.AttachParent(m__guiyuanCommon_icon.transform, itemID, (uint)itemCount, ShowGuiYuanGetWayCallBack);
        m_label_guiyuanCommon_number.text = StringUtil.GetNumNeedString(itemCount, 1);

       
        if (status == (int)PetGrowState.Perfect)
        {
            m_label_guiyuannotingtext.text = DataManager.Manager<TextManager>().GetLocalText(LocalTextType.Local_Txt_Pet_guiyuanmanjineirong);
            ShowGaojiContainer(true);
            m_label_putongtitletext.text = CommonData.GetLocalString("原天赋");
            m_label_gaojititletext.text = CommonData.GetLocalString("新天赋");
            m_sprite_titlebg.gameObject.SetActive(true);
            m_label_putongjiebian.gameObject.SetActive(true);
            m_label_gaojijiebian.gameObject.SetActive(true);
            int commonLv = CurPet.GetProp((int)PetProp.CommonJieBianLv);
            m_label_putongjiebian.text = DataManager.Manager<PetDataManager>().GetJieBianString(commonLv);
            int advLv = CurPet.GetProp((int)PetProp.AdvaceJieBianLv);
            m_label_gaojijiebian.text =DataManager.Manager<PetDataManager>().GetJieBianString(advLv);
            m_trans_zuidajiebian.gameObject.SetActive(true);

            m_label_maxjiebian.text = pdb.maxJiebian;

            m_trans_PtGuiyuanGrowCotainer.gameObject.SetActive(false);
            
        }
        else
        {
            m_label_guiyuannotingtext.text = DataManager.Manager<TextManager>().GetLocalText(LocalTextType.Local_Txt_Pet_guiyuanweimanjineirong);
            ShowGaojiContainer(false);
            m_sprite_titlebg.gameObject.SetActive(false);
            m_label_putongtitletext.text = CommonData.GetLocalString("洗炼");
            m_label_gaojititletext.text = CommonData.GetLocalString("洗炼");
            m_label_putongjiebian.gameObject.SetActive(false);
            m_label_gaojijiebian.gameObject.SetActive(false);
            m_trans_zuidajiebian.gameObject.SetActive(false);
            m_trans_PtGuiyuanGrowCotainer.gameObject.SetActive(true);
        }
        InitGaojiData();
 
    }
    void ShowGuiYuanGetWayCallBack(UIItemCommonGrid grid)
    {
        int itemCount = DataManager.Manager<ItemManager>().GetItemNumByBaseId(m_nItemID);
        if (0 == itemCount)
        {
            TipsManager.Instance.ShowItemTips(m_nItemID);
            DataManager.Manager<UIPanelManager>().ShowPanel(PanelID.GetWayPanel, data: grid.Data.DwObjectId);

        }
        else
        {
            TipsManager.Instance.ShowItemTips(m_nItemID);
        }
    }

    void ShowGaojiContainer(bool bShow)
    {
        m_trans_contentcontainer.gameObject.SetActive(bShow);
        m_trans_nothingshow.gameObject.SetActive(!bShow);
    }
    void InitGaojiData()
    {
        if (CurPet != null)
        {
            GameCmd.PetTalent replaceTalent = CurPet.GetReplaceTalent();
            if(replaceTalent.jingshen == 0&&
                replaceTalent.liliang == 0&&
                replaceTalent.minjie == 0&&
                replaceTalent.tizhi == 0&&
                replaceTalent.zhili == 0)
            {
                ShowGaojiContainer(false);
            }
        
        }
    }

    void onClick_GuiyuanCommon_wanmei_Btn(GameObject caster)
    {
        autoPrefect = !autoPrefect;
        Transform sprite = caster.transform.Find("Sprite");
        if (sprite != null)
        {
            sprite.gameObject.SetActive(autoPrefect);
        }

    }
    void onClick_BtnSaveTianfu_Btn(GameObject caster)
    {
        if (CurPet == null)
        {
            Log.Error(" curpet is null");
            return;
        }

  
        // m_widget_guiyuanhou.gameObject.SetActive( false );
        if (CurPet != null)
        {
            stReplaceTalentPetUserCmd_C cmd = new stReplaceTalentPetUserCmd_C();
            cmd.id = CurPet.GetID();
            NetService.Instance.Send(cmd);
            ShowGaojiContainer(false);

        }

    }
    void onClick_GuiyuanCommon_zidongbuzu_Btn(GameObject caster)
    {
        autoConsume = !autoConsume;
        Transform sprite = caster.transform.Find("Sprite");
        if (sprite != null)
        {
            sprite.gameObject.SetActive(autoConsume);
        }
        ShowColdLabel(m_nItemID);
    }

    bool bStart = false;
    void onClick_Kaishiguiyuan_Btn(GameObject caster)
    {
        if (CurPet != null)
        {
            int status = CurPet.GetProp((int)PetProp.PetGuiYuanStatus);
            if (status < (int)PetGrowState.Perfect)
            {
                CommonGuiYuan(caster);
            }
            else
            {
                AdvanceGuiYuan();
            }
        }

    }
    void AdvanceGuiYuan()
    {
        if (!CheckItem())
        {
            return;
        }

        stGuiYuanPetUserCmd_CS cmd = new stGuiYuanPetUserCmd_CS();
        cmd.id = CurPet.GetID();
        cmd.adv = true;
        cmd.auto_buy = autoConsume;
        NetService.Instance.Send(cmd);
    }
    void CommonGuiYuan(GameObject caster)
    {
        if (bStart)
            return;


        if (!CheckItem())
        {
            return;
        }

        stGuiYuanPetUserCmd_CS cmd = new stGuiYuanPetUserCmd_CS();
        cmd.id = CurPet.GetID();
        cmd.adv = false;
        cmd.auto_buy = autoConsume;
        NetService.Instance.Send(cmd);
        if (autoPrefect)
        {
            caster.SetActive(false);
            m_btn_tingzhiguiyuan.gameObject.SetActive(true);
            bStart = true;
            TimerAxis.Instance().SetTimer(m_uGuiyuanTimerID, 1000, this);
        }
    }
    void onClick_Tingzhiguiyuan_Btn(GameObject caster)
    {
        StopGuiyuan();
    }
    bool ShowColdLabel(uint itemID)
    {
        int itemCount = DataManager.Manager<ItemManager>().GetItemNumByBaseId(itemID);
        if (itemCount <= 0)
        {
            if (autoConsume)
            {
                string needStr = "";
                if (petDataManager.GetItemNeedColdString(itemID, ref needStr))
                {
                    m_label_guiyuanCommon_number.gameObject.SetActive(false);
                    m_label_guiyuanCommon_dianjuanxiaohao.gameObject.SetActive(true);
                    m_label_guiyuanCommon_dianjuanxiaohao.text = needStr;
                    return true;
                }
                else
                {
                    m_label_guiyuanCommon_number.gameObject.SetActive(false);
                    m_label_guiyuanCommon_dianjuanxiaohao.gameObject.SetActive(true);
                    m_label_guiyuanCommon_dianjuanxiaohao.text = needStr;
                    return false;
                }
            }
            else
            {
                m_label_guiyuanCommon_number.gameObject.SetActive(true);
                m_label_guiyuanCommon_dianjuanxiaohao.gameObject.SetActive(false);
            }
        }
        else
        {
            m_label_guiyuanCommon_number.gameObject.SetActive(true);
            m_label_guiyuanCommon_dianjuanxiaohao.gameObject.SetActive(false);
        }
        return true;
    }
    bool CheckItem()
    {
        PetDataBase pdb = petDataManager.GetPetDataBase(CurPet.PetBaseID);
        if (pdb == null)
        {
            Log.Error("pdb is null id is " + CurPet.PetBaseID.ToString());
            return false;
        }
        int status = CurPet.GetProp((int)PetProp.PetGuiYuanStatus);
        PetGuiYuanDataBase guiyuandb = GameTableManager.Instance.GetTableItem<PetGuiYuanDataBase>(pdb.petQuality, status);
        if (guiyuandb != null)
        {
            if (!MainPlayerHelper.IsHasEnoughMoney(ClientMoneyType.Gold, guiyuandb.consumeMoney))
            {
                StopGuiyuan();
                DataManager.Manager<Mall_HuangLingManager>().ShowExchange((uint)ErrorEnum.NotEnoughGold, ClientMoneyType.Gold, "提示", "去兑换", "取消");
                return false;
            }
        }


        int count = DataManager.Manager<ItemManager>().GetItemNumByBaseId(m_nItemID);
        if (count <= 0)
        {

            if (autoConsume)
            {

                if (!ShowColdLabel(m_nItemID))
                {
                    StopGuiyuan();
                    TipsManager.Instance.ShowTipsById(5);
                    return false;
                }
            }
            else
            {
                StopGuiyuan();
                TipsManager.Instance.ShowTipsById(6);
                return false;
            }

        }
        return true;
    }
    void StopGuiyuan()
    {
        m_btn_tingzhiguiyuan.gameObject.SetActive(false);
        m_btn_kaishiguiyuan.gameObject.SetActive(true);
        bStart = false;
        TimerAxis.Instance().KillTimer(m_uGuiyuanTimerID, this);
    }
    public void OnTimer(uint uTimerID)
    {
        if (uTimerID != m_uGuiyuanTimerID)
            return;
        if (!CheckItem())
        {
            return;
        }
        if (CurPet != null)
        {
            int status = CurPet.GetProp((int)PetProp.PetGuiYuanStatus);
            if (status < (int)PetGrowState.Perfect)
            {
                stGuiYuanPetUserCmd_CS cmd = new stGuiYuanPetUserCmd_CS();
                cmd.id = CurPet.GetID();
                cmd.adv = false;
                cmd.auto_buy = autoConsume;
                NetService.Instance.Send(cmd);
            }
            else
            {
                StopGuiyuan();
            }
        }
    }
}
