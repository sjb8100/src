using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using Client;
using Engine.Utility;
using table;
using GameCmd;


partial class PetMarkPanel
{
    PetDataManager petDataManger
    {
        get
        {
            return DataManager.Manager<PetDataManager>();
        }
    }
    private PetDataBase _database;
    uint m_nItemCount = 0;
    uint m_nNeedCount = 0;
    protected override void OnShow(object data)
    {
        base.OnShow(data);
    
    }
    protected override void OnJump(UIPanelBase.PanelJumpData jumpData)
    {
        base.OnJump(jumpData);
        if (jumpData != null)
        {
            uint petID = (uint)jumpData.Param;
            _database = GameTableManager.Instance.GetTableItem<PetDataBase>(petID);
            if (_database != null)
            {
                InitPetDataBase(_database);
            }
        }
    }
    public override UIPanelBase.PanelData GetPanelData()
    {
        PanelData pd = base.GetPanelData();
        pd.JumpData = new PanelJumpData();
        pd.JumpData.Param = _database.petID;
        return pd;
    }
    public void InitPetDataBase(PetDataBase db)
    {
        if (db == null)
        {
            Log.Error(" db is null");
            return;
        }
        _database = db;
        PetMessage mesage = m_widget_PetMessage.GetComponent<PetMessage>();
        if (mesage == null)
        {
            mesage = m_widget_PetMessage.gameObject.AddComponent<PetMessage>();
        }
        mesage.InitPetTexture(db);
        m_label_score.text = db.petScore.ToString();
        int itemCount = DataManager.Manager<ItemManager>().GetItemNumByBaseId(db.fragmentID);
        uint needNum = db.fragmentNum;
        m_nItemCount = (uint)itemCount;
        m_nNeedCount = (uint)needNum;
        Transform trans = m_slider_suipian_scorllbar.transform.Find("Percent");

        string str = itemCount + "/" + needNum;

        float scale = itemCount * 1.0f / needNum;

        m_slider_suipian_scorllbar.value = scale;
        if (itemCount >= needNum)
        {
            str = StringUtil.GetNumNeedString(itemCount, needNum);
            UILabel label = trans.GetComponent<UILabel>();
            if (label != null)
            {
                label.color = Color.green;
            }
        }
        else
        {
            if (trans != null)
            {
                UILabel label = trans.GetComponent<UILabel>();
                if (label != null)
                {
                    label.color = Color.white;
                }

            }

        }
        UILabel numLabel = trans.GetComponent<UILabel>();
        if (numLabel != null)
        {
            numLabel.text = str;
        }
        m_label_showname.text = db.petName;
        m_label_xiedaidengji.text = db.carryLevel.ToString();
        m_label_leixing.text = petDataManger.GetPetStrType(db);

        m_label_getway.text = db.getPath;
        UIItem.AttachParent(m_sprite_btn_huoqu.transform, (uint)db.getItemID, callback: GetWayCallBack, showGetWay: true);

        string talent = db.PetTalent;
        string[] zizhiArray = talent.Split(';');
        string[] strArray = zizhiArray[0].Split('_');
        m_label_liliang.text = strArray[strArray.Length - 1];
        string[] minjieArray = zizhiArray[1].Split('_');
        m_label_minjie.text = minjieArray[minjieArray.Length - 1];

        string[] zhiliArray = zizhiArray[2].Split('_');
        m_label_zhili.text = zhiliArray[zhiliArray.Length - 1];

        string[] tiliArray = zizhiArray[3].Split('_');
        m_label_tili.text = tiliArray[tiliArray.Length - 1];

        string[] jingshenArray = zizhiArray[4].Split('_');
        m_label_jingshen.text = jingshenArray[jingshenArray.Length - 1];
    }
    void GetWayCallBack(UIItemCommonGrid grid)
    {
        int itemCount = DataManager.Manager<ItemManager>().GetItemNumByBaseId(grid.Data.DwObjectId);
        if (0 == itemCount)
        {
            TipsManager.Instance.ShowItemTips(grid.Data.DwObjectId);
            DataManager.Manager<UIPanelManager>().ShowPanel(PanelID.GetWayPanel, data: grid.Data.DwObjectId);

        }
        else
        {
            TipsManager.Instance.ShowItemTips(grid.Data.DwObjectId);
        }
    }
    void onClick_Close_Btn(GameObject caster)
    {
        HideSelf();
    }

    void onClick_Btn_hecheng_Btn(GameObject caster)
    {
        if (m_nItemCount < m_nNeedCount)
        {
            TipsManager.Instance.ShowTips(LocalTextType.Pet_Compose_zhanhunsuipianbuzuwufahecheng);
            //   TipsManager.Instance.ShowTipsById(406043);
            return;
        }
        stExchangePetUserCmd_C cmd = new stExchangePetUserCmd_C();
        cmd.pet = _database.petID;
        NetService.Instance.Send(cmd);
        HideSelf();
    }

    void onClick_Btn_shizhuang_Btn(GameObject caster)
    {
        if (_database != null)
        {
            uint getwayID = _database.suitGetWay;
            if (getwayID == 0)
            {
                TipsManager.Instance.ShowTips(LocalTextType.Pet_Suit_meiyouduiyingdezhenshoushizhuang);
                return;
            }
            ItemManager.DoJump(getwayID);
        }
    }

    void onClick_Btn_right_Btn(GameObject caster)
    {
        PetDataBase db = petDataManger.GetNextPet(_database);
        if (db != null)
        {
            InitPetDataBase(db);
        }
    }

    void onClick_Btn_left_Btn(GameObject caster)
    {
        PetDataBase db = petDataManger.GetLastPet(_database);
        if (db != null)
        {
            InitPetDataBase(db);
        }
    }
    protected override void OnHide()
    {
        base.OnHide();
        petDataManger.ReleaseRenderObj();
    }

}
