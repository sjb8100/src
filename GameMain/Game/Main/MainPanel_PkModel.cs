using System.ComponentModel;
using System.Collections.Generic;
using UnityEngine;
using Client;



partial class MainPanel
{

    UIPopupList popList = null;

  //  UISprite m_spriteBtnBG = null;
    UILabel m_lableBtnTop = null;
    GameObject m_goSelect = null;
    GameObject m_btnPrefab = null;
    Transform m_transBtnBg = null;
    private void InitPkUI()
    {
        //--
        m_btnPrefab = m_trans_pkmodel.Find("btn").gameObject;
        m_goSelect = m_btnPrefab.transform.Find("yuanquan/select").gameObject;

        Transform transBtnTop = m_trans_pkmodel.Find("btntop");
        if (transBtnTop != null)
        {
            m_lableBtnTop = transBtnTop.Find("Label").GetComponent<UILabel>();

            UIEventListener.Get(transBtnTop.gameObject).onClick = OnBtnTopClick;
        }

        m_transBtnBg = m_trans_pkmodel.Find("btnbg");
        if (m_transBtnBg == null)
        {
            return;
        }
       // m_spriteBtnBG = transBtnBg.GetComponent<UISprite>();

        UIEventListener.Get(m_transBtnBg.gameObject).onClick = OnClosePk;

        m_transBtnBg.gameObject.SetActive(false);
    }

    void OnClosePk(GameObject go)
    {
       // m_goSelect.transform.parent = m_trans_pkmodel.transform;
      //  m_goSelect.gameObject.SetActive(false);
        m_transBtnBg.gameObject.SetActive(false);
        m_transBtnBg.transform.DestroyChildren();
    }

    void OnBtnTopClick(GameObject go)
    {
       // m_goSelect.transform.parent = m_trans_pkmodel.transform;
          m_transBtnBg.gameObject.SetActive(true);
          m_transBtnBg.transform.DestroyChildren();
        
        Client.IMapSystem ms = Client.ClientGlobal.Instance().GetMapSystem();
        if (ms == null)
        {
            return;
        }
        table.MapDataBase mapdata = GameTableManager.Instance.GetTableItem<table.MapDataBase>(ms.GetMapID());
        if (mapdata == null)
        {
            return;
        }

        GameObject curBtn = null;
        if (ClientGlobal.Instance().MainPlayer != null)
        {
            int nmodel = ClientGlobal.Instance().MainPlayer.GetProp((int)PlayerProp.PkMode);
            int index = 0;
            for (int i = (int)PLAYERPKMODEL.PKMODE_M_NONE + 1; i <= (int)PLAYERPKMODEL.PKMODE_M_JUSTICE; i++,index++)
            {
                PLAYERPKMODEL model = (PLAYERPKMODEL)i;
                GameObject btn = NGUITools.AddChild(m_transBtnBg.gameObject, m_btnPrefab);
                btn.transform.localPosition = new UnityEngine.Vector3(174, -28 - index * 52, 0);
                btn.gameObject.SetActive(true);
                btn.GetComponentInChildren<UILabel>().text = GetDesByPkModel(model);
                btn.name = i.ToString();
                Transform flagTrans = btn.transform.Find("flag");
                if(flagTrans != null)
                {
                    UISprite sp = flagTrans.GetComponent<UISprite>();
                    if(sp != null)
                    {
                        sp.spriteName = GetFlagByPkModel(model);
                    }
                }
                SetSelect(btn, false);
                SetMask(btn, true);
                if((int)model == nmodel)
                {
                    curBtn = btn;
                }
                if (mapdata.supportModel == "0")
                {
                   UIEventListener.Get(btn).onClick = OnClickBtn;
                   SetMask(btn, false);
                }
                else
                {
                    string[] param =  mapdata.supportModel.Split('_');
                    for (int m = 0; m < param.Length;m++ )
                    {
                        int value = int.Parse(param[m]);
                        if (i == value)
                        {
                            UIEventListener.Get(btn).onClick = OnClickBtn;
                            SetMask(btn, false);
                        }
                        else 
                        {
                            UIEventListener.Get(btn).onClick = onClickUnenble;
                        }
                    }
                }
            }
            if(curBtn != null)
            {
                SetSelect(curBtn,true);
                SetMask(curBtn, false);
            }
        }
    }
    void SetMask(GameObject go, bool bShow)
    {
        Transform mask = go.transform.Find("mask");
        if (mask != null)
        {
            mask.gameObject.SetActive(bShow);
        }
        Transform select = go.transform.Find("yuanquan");
        if (select != null)
        {
            select.gameObject.SetActive(!bShow);
        }
    }
    void SetSelect(GameObject go,bool bShow)
    {
        Transform select = go.transform.Find("yuanquan/select");
        if(select != null)
        {
            select.gameObject.SetActive(bShow);
        }
    }
    void OnClickBtn(GameObject go)
    {
        int currModel = ClientGlobal.Instance().MainPlayer.GetProp((int)PlayerProp.PkMode);
        int selectModel = int.Parse(go.name);
        GameCmd.enumPKMODE mode = (GameCmd.enumPKMODE)selectModel;
        PKModeData.Instance.SetPkMode(mode);     
         m_transBtnBg.gameObject.SetActive(false);
         m_transBtnBg.transform.DestroyChildren();
    }
    void onClickUnenble(GameObject go) 
    {
        int currModel = ClientGlobal.Instance().MainPlayer.GetProp((int)PlayerProp.PkMode);
        int selectModel = int.Parse(go.name);
        PLAYERPKMODEL model = (PLAYERPKMODEL)selectModel;
        string msg = DataManager.Manager<TextManager>().GetLocalFormatText(LocalTextType.Talk_System_buzhichigaimoshi, model.GetEnumDescription());
//        string msg = string.Format("该地图不支持切换到{0}模式", );
        TipsManager.Instance.ShowTips(msg);
        ChatDataManager.SendToChatSystem(msg);
    }
    string GetFlagByPkModel(PLAYERPKMODEL model)
    {
        int pk = (int)model;
        List<string> list = GameTableManager.Instance.GetGlobalConfigList<string>("PkModelUIConfig", pk.ToString());
        if(list != null && list.Count == 3)
        {
            return list[1];
        }
        return null;
    }
    string GetDesByPkModel(PLAYERPKMODEL model)
    {
        int pk = (int)model;
        List<string> list = GameTableManager.Instance.GetGlobalConfigList<string>("PkModelUIConfig", pk.ToString());
        if (list != null && list.Count == 3)
        {
            List<uint> color = StringUtil.GetSplitStringList<uint>(list[2], '_');
            if(color.Count == 4)
            {
                Color c = new Color(color[0], color[1], color[2], color[3]);
                return ColorManager.GetColorString(c, list[0]);
            }
            return list[0];
        }
        return null;
    }

    void SetRolePkModel(Client.IEntity entity)
    {
        if (entity == null)
        {
            return;
        }
        if (!ClientGlobal.Instance().IsMainPlayer(entity.GetUID()))
        {
            return;
        }
        
        
        if (m_lableBtnTop != null)
        {        
            int model = entity.GetProp((int)PlayerProp.PkMode);
            if (model != 0)
            {
                m_lableBtnTop.text = GetPkModeDes(model);
            }
            else
            {
                m_lableBtnTop.text = GetPkModeDes(1);
            }               
        }
    }

    Dictionary<int, string> pk_dic = null;
    string GetPkModeDes(int model)
    {
        if (pk_dic == null)
        {
            pk_dic = new Dictionary<int, string>();
        }
        string desc = "";
        if (pk_dic.ContainsKey(model))
        {
            desc = pk_dic[model];
        }
        else 
        {
            PLAYERPKMODEL pkmodel = (PLAYERPKMODEL)model;
            desc =  pkmodel.GetEnumDescription();
            pk_dic.Add(model,desc);
        }
        return desc;
    }
}