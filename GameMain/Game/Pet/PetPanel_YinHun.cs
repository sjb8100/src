using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using GameCmd;
using table;
using Client;
partial class PetPanel
{

    #region 引魂界面
    void InitCurXiuWei()
    {
        int level = CurPet.GetProp((int)PetProp.YinHunLevel);

        PetYinHunDataBase db = petDataManager.GetPetYinHunDataBase((uint)level);
        if (db != null)
        {
            int scale = Mathf.FloorToInt(db.jiaChengBi * 0.01f);
            string scaleStr = ColorManager.GetColorString(ColorType.Green, string.Format(" (+{0}%)", scale));
            int liliang = CurPet.GetProp((int)PetProp.StrengthTalent);
            m_label_yuan_liliangtianfu.text = liliang.ToString() + scaleStr;
            int minjie = CurPet.GetProp((int)PetProp.AgilityTalent);
            m_label_yuan_minjietianfu.text = minjie.ToString() + scaleStr;
            int zhili = CurPet.GetProp((int)PetProp.IntelligenceTalent);
            m_label_yuan_zhilitianfu.text = zhili.ToString() + scaleStr;
            int tili = CurPet.GetProp((int)PetProp.CorporeityTalent);
            m_label_yuan_tilitianfu.text = tili.ToString() + scaleStr;
            int jingshen = CurPet.GetProp((int)PetProp.SpiritTalent);
            m_label_yuan_jingshentianfu.text = jingshen.ToString() + scaleStr;
        }
    }
    void ShowNextXiuWei()
    {
        InitCurXiuWei();
        int level = CurPet.GetProp((int)PetProp.YinHunLevel);
        int nextLv = level + 1;
        float factor = 10000;// 0.0001f;
        PetYinHunDataBase db = petDataManager.GetPetYinHunDataBase((uint)nextLv);
        if (db != null)
        {
            PetYinHunDataBase curdb = petDataManager.GetPetYinHunDataBase((uint)level);
            if (curdb != null)
            {
                uint jiacheng = db.jiaChengBi + 10000;
                uint lastFactor = curdb.jiaChengBi + 10000;
                int scale = Mathf.FloorToInt(db.jiaChengBi * 0.01f);
                string scaleStr = ColorManager.GetColorString(ColorType.Green, string.Format(" (+{0}%)", scale));

                //   lastFactor = 1 + db.jiaChengBi * 1.0f / 100;
                factor = jiacheng * 1.0f / lastFactor;
                m_label_xin_xiuwei.text = nextLv.ToString();
                int liliang = CurPet.GetProp((int)PetProp.StrengthTalent);
                int nextll = Mathf.FloorToInt(liliang * factor);
                m_label_xin_liliangtianfu.text = nextll.ToString() + scaleStr;
                int minjie = CurPet.GetProp((int)PetProp.AgilityTalent);
                int nextmj = Mathf.FloorToInt(minjie * factor);
                m_label_xin_minjietianfu.text = nextmj.ToString() + scaleStr;
                int zhili = CurPet.GetProp((int)PetProp.IntelligenceTalent);
                int nextzhili = Mathf.FloorToInt(zhili * factor);
                m_label_xin_zhilitianfu.text = nextzhili.ToString() + scaleStr;
                int tili = CurPet.GetProp((int)PetProp.CorporeityTalent);
                int nexttili = Mathf.FloorToInt(tili * factor);
                m_label_xin_tilitianfu.text = nexttili.ToString() + scaleStr;
                int jingshen = CurPet.GetProp((int)PetProp.SpiritTalent);
                int nextjingshen = Mathf.FloorToInt(jingshen * factor);
                m_label_xin_jingshentianfu.text = nextjingshen.ToString() + scaleStr;

                int lingqiExp = CurPet.GetProp((int)PetProp.YinHunExp);
                uint totalExp = db.lingQiMax;
            }
            // ShowSlider();
        }


    }
    uint m_nYinHunNeedItemID = 0;
    CMResAsynSeedData<CMTexture> m_yinhunJinengAsynSeed = null;
    CMResAsynSeedData<CMTexture> m_yinhunTopIconAsynSeed = null;
    CMResAsynSeedData<CMAtlas> m_yinhunBgIconAsynSeed = null;
    CMResAsynSeedData<CMAtlas> m_yinhunPinzhiIconAsynSeed = null;
    void OnYinHunUIRelease()
    {
        if (m_yinhunJinengAsynSeed != null)
        {
            m_yinhunJinengAsynSeed.Release();
            m_yinhunJinengAsynSeed = null;
        }
        if (m_yinhunTopIconAsynSeed != null)
        {
            m_yinhunTopIconAsynSeed.Release();
            m_yinhunTopIconAsynSeed = null;

        }
        if (m_yinhunBgIconAsynSeed != null)
        {
            m_yinhunBgIconAsynSeed.Release();
            m_yinhunBgIconAsynSeed = null;

        }
        if (m_yinhunPinzhiIconAsynSeed != null)
        {
            m_yinhunPinzhiIconAsynSeed.Release();
            m_yinhunPinzhiIconAsynSeed = null;

        }
    }
    void InitYinhunPaneel()
    {
        if (CurPet != null)
        {
            PetDataBase pdb = petDataManager.GetPetDataBase(CurPet.PetBaseID);
            if (pdb != null)
            {
               // DataManager.Manager<UIManager>().SetSpriteDynamicIcon(m_sprite_jinengpeticon, pdb.icon);
                UIManager.GetTextureAsyn(pdb.icon, ref m_yinhunJinengAsynSeed, () =>
                {

                    if (null != m__jinengpeticon)
                    {
                        m__jinengpeticon.mainTexture = null;
                    }
                }, m__jinengpeticon, false);

                UIManager.GetTextureAsyn(pdb.icon, ref m_yinhunTopIconAsynSeed, () =>
                {
                    if (null != m__yinhuntopicon)
                    {
                        m__yinhuntopicon.mainTexture = null;
                    }
                }, m__yinhuntopicon, false);
              //  DataManager.Manager<UIManager>().SetSpriteDynamicIcon(m_sprite_yinhuntopicon, pdb.icon);


                //背景框品质
                string bgSpriteName = ItemDefine.GetItemFangBorderIconByItemID(pdb.ItemID);


                Transform bgTrans = m__yinhuntopicon.transform.Find("pingzhi_box");
                if (bgTrans)
                {
                    UISprite bgSpr = bgTrans.GetComponent<UISprite>();
                    if (bgSpr)
                    {
                        UIManager.GetAtlasAsyn(bgSpriteName, ref m_yinhunBgIconAsynSeed, () =>
                        {
                            if (null != bgSpr)
                            {
                                bgSpr.atlas = null;
                            }
                        }, bgSpr, false);
                      //  DataManager.Manager<UIManager>().SetSpriteDynamicIcon(bgSpr, bgSpriteName);
                    }
                }
                UIManager.GetAtlasAsyn(bgSpriteName, ref m_yinhunPinzhiIconAsynSeed, () =>
                {
                    if (null != m_sprite_petpingzhi)
                    {
                        m_sprite_petpingzhi.atlas = null;
                    }
                }, m_sprite_petpingzhi, false);
              //  DataManager.Manager<UIManager>().SetSpriteDynamicIcon(m_sprite_petpingzhi, bgSpriteName);

            }


            int yLv = CurPet.GetProp((int)PetProp.YinHunLevel);
            PetYinHunDataBase db = GameTableManager.Instance.GetTableItem<PetYinHunDataBase>((uint)yLv);
            if (db != null)
            {
                uint itemID = db.needItem;
                m_nYinHunNeedItemID = itemID;
                int itemCount = DataManager.Manager<ItemManager>().GetItemNumByBaseId(itemID);
                UIItem.AttachParent(m__yinhun_icon.transform, itemID, (uint)itemCount, ShowYinhunGetWayCallBack);
                m_label_yinhun_xiaohaogold.text = db.needMoney.ToString();
                m_label_yinhunshici_xiaohaogold.text = (10 * db.needMoney).ToString();
                ItemDataBase itemDb = GameTableManager.Instance.GetTableItem<ItemDataBase>(itemID);
                if (itemDb != null)
                {
                    int num = DataManager.Manager<ItemManager>().GetItemNumByBaseId(itemDb.itemID);
                    ShowYinHunColdLabel(itemID);
                    m_label_yinhun_xiaohaoname.text = itemDb.itemName;
                    m_label_yinhun_xiaohaonumber.text = StringUtil.GetNumNeedString(num, 1);
                }
            }

            m_label_yinhunmanji.text = DataManager.Manager<TextManager>().GetLocalText(LocalTextType.Local_Txt_Pet_yinhunmanjineirong);
            if (yLv == 10)
            {
                m_widget_yinhun_nomax.gameObject.SetActive(false);
                m_trans_xinxiuweicontentparent.gameObject.SetActive(false);
                m_label_yinhunmanji.gameObject.SetActive(true);
                m_label_xin_xiuwei.text = CommonData.GetLocalString("满");
                // m_widget_yinhun_nomax.gameObject.SetActive(false);
                // m_trans_yuanxiuwei.transform.localPosition = new Vector3(16, 122, 0);
                // m_label_yinhun_maxLabel.gameObject.SetActive(true);

            }
            else
            {
                m_widget_yinhun_nomax.gameObject.SetActive(true);
                m_trans_xinxiuweicontentparent.gameObject.SetActive(true);
                m_label_yinhunmanji.gameObject.SetActive(false);
                // m_widget_yinhun_nomax.gameObject.SetActive(true);
                // m_trans_yuanxiuwei.transform.localPosition = new Vector3(-180, 122, 0);
                // m_label_yinhun_maxLabel.gameObject.SetActive(false);
            }
        }

    }
    void ShowYinhunGetWayCallBack(UIItemCommonGrid grid)
    {
        int itemCount = DataManager.Manager<ItemManager>().GetItemNumByBaseId(m_nYinHunNeedItemID);
        if (0 == itemCount)
        {
            TipsManager.Instance.ShowItemTips(m_nYinHunNeedItemID);
            DataManager.Manager<UIPanelManager>().ShowPanel(PanelID.GetWayPanel, data: grid.Data.DwObjectId);

        }
        else
        {
            TipsManager.Instance.ShowItemTips(m_nYinHunNeedItemID);
        }
    }
    bool ShowYinHunColdLabel(uint itemID)
    {
        int itemCount = DataManager.Manager<ItemManager>().GetItemNumByBaseId(itemID);
        if (itemCount <= 0)
        {
            if (bAutoYinHunBuy)
            {
                string needStr = "";
                if (petDataManager.GetItemNeedColdString(itemID, ref needStr))
                {
                    m_label_yinhun_xiaohaonumber.gameObject.SetActive(false);
                    m_label_yinhun_dianjuanxiaohao.gameObject.SetActive(true);
                    m_label_yinhun_dianjuanxiaohao.text = needStr;
                    return true;
                }
                else
                {
                    m_label_yinhun_xiaohaonumber.gameObject.SetActive(false);
                    m_label_yinhun_dianjuanxiaohao.gameObject.SetActive(true);
                    m_label_yinhun_dianjuanxiaohao.text = needStr;
                    return false;
                }
            }
            else
            {
                m_label_yinhun_xiaohaonumber.gameObject.SetActive(true);
                m_label_yinhun_dianjuanxiaohao.gameObject.SetActive(false);
            }

        }
        else
        {
            m_label_yinhun_xiaohaonumber.gameObject.SetActive(true);
            m_label_yinhun_dianjuanxiaohao.gameObject.SetActive(false);
        }
        return true;
    }
    #endregion
    bool bAutoYinHunBuy = false;
    void onClick_Yinhun_xiaohaoSprite_Btn(GameObject caster)
    {
        bAutoYinHunBuy = !bAutoYinHunBuy;
        Transform trans = caster.transform.Find("Sprite");
        if (trans != null)
        {
            trans.gameObject.SetActive(bAutoYinHunBuy);
        }
        ShowYinHunColdLabel(m_nYinHunNeedItemID);
    }

    void onClick_Yinhunyici_Btn(GameObject caster)
    {
        SendYinHun();
    }
    bool CheckItemNum()
    {
        if (CurPet != null)
        {
            int lv = CurPet.GetProp((int)Client.PetProp.YinHunLevel);
            PetYinHunDataBase db = GameTableManager.Instance.GetTableItem<PetYinHunDataBase>((uint)lv);
            if (db != null)
            {
                uint jinbi = db.needMoney;
                if (!MainPlayerHelper.IsHasEnoughMoney(ClientMoneyType.Gold, jinbi))
                {
                    DataManager.Manager<Mall_HuangLingManager>().ShowExchange((uint)ErrorEnum.NotEnoughGold, ClientMoneyType.Gold, "提示", "去兑换", "取消");
                    return false;
                }
                uint itemID = db.needItem;
                int itemCount = DataManager.Manager<ItemManager>().GetItemNumByBaseId(itemID);
                if (bAutoYinHunBuy)
                {
                    if (!ShowYinHunColdLabel(itemID))
                    {
                        TipsManager.Instance.ShowTipsById(5);
                        return false;
                    }
                }
                else
                {
                    if (itemCount <= 0)
                    {
                        TipsManager.Instance.ShowTipsById(6);
                        return false;
                    }
                }

            }
            else
            {
                TipsManager.Instance.ShowTips(LocalTextType.Pet_YinHun_xiuweiyimanjibunengjixuyinhunle);
                //TipsManager.Instance.ShowTipsById( 108007 );
                return false;
            }
        }
        return true;
    }
    void SendYinHun()
    {
        if (CurPet != null)
        {
            if (!CheckItemNum())
                return;
            stYinHunPetUserCmd_CS cmd = new stYinHunPetUserCmd_CS();
            cmd.id = CurPet.GetID();
          //  cmd.yh_num = 1;
            cmd.auto_buy = bAutoYinHunBuy;
            NetService.Instance.Send(cmd);
        }
    }
    void onClick_Yinhunshici_Btn(GameObject caster)
    {

        #region 十次发一次消息
        /*
        int lv = CurPet.GetProp((int)Client.PetProp.YinHunLevel);
        PetYinHunDataBase db = GameTableManager.Instance.GetTableItem<PetYinHunDataBase>((uint)lv);
        if (db != null)
        {
            int jinbi = (int)db.needMoney;
            int totalNum = CommonData.GetMoneyNumByType(ClientMoneyType.Gold);
            int jinbiUseNum = 10;
            if (jinbi != 0)
            {
                jinbiUseNum = totalNum / jinbi;
            }
            if (jinbiUseNum == 0)
            {
                TipsManager.Instance.ShowTipsById(4);
                return;
            }
            uint needCold = petDataManager.GetItemNeedColdNum(m_nYinHunNeedItemID);
            int dianjuanTotalNum = CommonData.GetMoneyNumByType(ClientMoneyType.YuanBao);
            int dianjuanUssNum = 10;
            if (needCold != 0)
            {
                dianjuanUssNum = dianjuanTotalNum / (int)needCold;
            }
            if (dianjuanUssNum == 0)
            {
                TipsManager.Instance.ShowTipsById(5);
                return;
            }
            int num = dianjuanUssNum < jinbiUseNum ? dianjuanUssNum : jinbiUseNum;

            uint itemID = db.needItem;
            int itemCount = DataManager.Manager<ItemManager>().GetItemNumByBaseId(itemID);

            if (itemCount == 0)
            {
                if (!bAutoYinHunBuy)
                {
                    TipsManager.Instance.ShowTipsById(6);
                    return;
                }
            }
            else
            {
                num = num < itemCount ? num : itemCount;

            }
            if (num > 10)
            {
                num = 10;
            }
            stYinHunPetUserCmd_CS cmd = new stYinHunPetUserCmd_CS();
            cmd.id = CurPet.GetID();
            cmd.yh_num = (uint)num;
            cmd.auto_buy = bAutoYinHunBuy;
            NetService.Instance.Send(cmd);
        }
        else
        {
            TipsManager.Instance.ShowTips(LocalTextType.Pet_YinHun_xiuweiyimanjibunengjixuyinhunle);
        }*/
        #endregion

        int lv = CurPet.GetProp((int)Client.PetProp.YinHunLevel);
        PetYinHunDataBase db = GameTableManager.Instance.GetTableItem<PetYinHunDataBase>((uint)lv);
        if (db != null)
        {
            int jinbi = (int)db.needMoney;
            int totalNum = MainPlayerHelper.GetMoneyNumByType(ClientMoneyType.Gold);
            int jinbiUseNum = 10;
            if (jinbi != 0)
            {
                jinbiUseNum = totalNum / jinbi+1;
            }
            uint needCold = petDataManager.GetItemNeedColdNum(m_nYinHunNeedItemID);
            int dianjuanTotalNum = MainPlayerHelper.GetMoneyNumByType(ClientMoneyType.YuanBao);
            int dianjuanUssNum = 10;
            if (needCold != 0)
            {
                dianjuanUssNum = dianjuanTotalNum / (int)needCold+1;
            }
       
            int num = dianjuanUssNum < jinbiUseNum ? dianjuanUssNum : jinbiUseNum;
            if (num > 10)
            {
                num = 10;
            }
            for (int i = 0; i < num; i++)
            {
                if (!CheckItemNum())
                    return;
                SendYinHun();
            }
        }
    }
   


}