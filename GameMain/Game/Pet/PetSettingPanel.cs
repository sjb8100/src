//*************************************************************************
//	创建日期:	2016/10/28 17:25:19
//	文件名称:	PetSettingPanel
//   创 建 人:   zhudianyu	
//	版权所有:	中青宝
//	说    明:	游戏主界面
//*************************************************************************

using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using Client;
using Engine.Utility;
using table;
using GameCmd;
using DG.Tweening;
using Vector2 = UnityEngine.Vector2;
using System.Text;


partial class PetSettingPanel
{

    PetDataManager m_petData
    {
        get
        {
            return DataManager.Manager<PetDataManager>();
        }
    }
    List<UILabel> fightCountDownList = new List<UILabel>();
    List<Transform> relieveList = new List<Transform>();

    List<Transform> m_transChanZhanBtnList = new List<Transform>();

    List<Transform> m_removeBtnList = new List<Transform>();

    List<UILabel> fighingCDList = new List<UILabel>();

    List<UILabel> m_wordList = new List<UILabel>();

    int m_nPetUICount = 4;
    protected override void OnLoading()
    {
        base.OnLoading();

        m_petData.ValueUpdateEvent += m_petData_ValueUpdateEvent;

        m_nPetUICount = m_trans_petGroup.childCount;
        // UIEventListener.Get(m_widget_close.gameObject).onClick = HidePetSet;
    }
    public override void OnColliderMaskClicked()
    {
        base.OnColliderMaskClicked();
        ToggleSetting(true);
        DataManager.Manager<UIPanelManager>().HidePanel(PanelID.PetListPanel);
        HideSelf();

    }
    void HidePetSet(GameObject go)
    {

    }

    void m_petData_ValueUpdateEvent(object sender, ValueUpdateEventArgs e)
    {
        if (e != null)
        {
            if (e.key == PetDispatchEventString.RefreshUserQuickSetting.ToString())
            {
                RefreshQuickSetting();
                ShowSetBtn();
            }

        }
    }
    public override bool OnMsg(UIMsgID msgid, object param)
    {
        if (msgid == UIMsgID.eShowQuickSettingBtn)
        {
            ToggleSetting(true);
        }
        return base.OnMsg(msgid, param);
    }

    void Update()
    {
        for (int i = 0; i < m_nPetUICount; i++)
        {
            List<uint> idList = m_petData.GetUserQuicSettingList();
            if (i < idList.Count)
            {
                UILabel label = m_wordList[i];

                string wordStr = CommonData.GetLocalString("出战");
                string tempstr = CommonData.GetLocalString("出战");
                Transform btnTrans = m_transChanZhanBtnList[i];
                UIButton btn = btnTrans.GetComponent<UIButton>();
                Transform reliveTrans = relieveList[i];
                uint petID = idList[i];
                int fightCd = m_petData.GetPetFightCDTime(petID);
                if (i < m_wordList.Count)
                {
                    if (btn != null)
                    {
                        if (fightCd != 0)
                        {
                            btn.isEnabled = false;
                            wordStr = string.Format("{0}({1})", tempstr, fightCd);
                            // wordStr += "(" + fightCd.ToString() + ")";
                        }
                        else
                        {
                            if(petID == 0)
                            {
                                wordStr = CommonData.GetLocalString("设置");
                            }
                          
                            btn.isEnabled = true;

                        }
                    }


                    label.text = wordStr.ToString();
                }

    
                int tempTime = m_petData.GetRelieveCDTime(petID);
                int reliveCd =  tempTime;
                //UILabel cdword = fighingCDList[i];
                //if (cdword != null)
                //{
                //    cdword.gameObject.SetActive(true);
                //    if(fightCd > tempTime)
                //    {
                //        cdword.text = CommonData.GetLocalString("出战中");
                //    }
                //    else
                //    {
                //        cdword.text = CommonData.GetLocalString("复活中");
                //    }
                //}
                if (reliveCd > 0)
                {
                    UILabel reliveLabel = reliveTrans.GetComponent<UILabel>();
                    if (reliveLabel != null)
                    {
                        reliveLabel.text = reliveCd.ToString();
                    }
                    reliveTrans.gameObject.SetActive(true);
                }
                else
                {
                    reliveTrans.gameObject.SetActive(false);
                }
            }

        }

    }
    protected override void OnHide()
    {
        base.OnHide();
        Release();
    }
    public override void Release(bool depthRelease = true)
    {
        base.Release(depthRelease);
        if(m_curIconAsynSeed != null)
        {
            m_curIconAsynSeed.Release();
            m_curIconAsynSeed = null;
        }
    }
    CMResAsynSeedData<CMTexture> m_curIconAsynSeed = null;
    void RefreshQuickSetting()
    {
        int count = m_trans_petGroup.childCount;
        for (int i = 0; i < count; i++)
        {
            string name = "pet_" + (i + 1).ToString();
            Transform item = m_trans_petGroup.Find(name);
            if (item != null)
            {
                List<uint> idList = m_petData.GetUserQuicSettingList();
                if (idList != null)
                {
                    if (idList.Count > i)
                    {
                        uint petid = idList[i];
                        Transform icon = item.Find("icon");
                        if (icon != null)
                        {
                            UITexture spr = icon.GetComponent<UITexture>();
                            if (spr != null)
                            {
                                UILabel fightText = null;
                                Transform textTrans = item.Find("btn/btn_word");
                                if (textTrans != null)
                                {
                                    fightText = textTrans.GetComponent<UILabel>();
                                    m_wordList.Add(fightText);
                                }
                                if (petid != 0)
                                {
                                    IPet pet = m_petData.GetPetByThisID(petid);
                                    if (pet != null)
                                    {
                                        int baseID = pet.GetProp((int)PetProp.BaseID);
                                        PetDataBase pdb = m_petData.GetPetDataBase((uint)baseID);
                                        if (pdb != null)
                                        {
                                            UIManager.GetTextureAsyn(pdb.icon, ref m_curIconAsynSeed, () =>
                                            {
                                                if (null != spr)
                                                {
                                                    spr.mainTexture = null;
                                                }
                                            }, spr, false);
                                          //  DataManager.Manager<UIManager>().SetSpriteDynamicIcon(spr, pdb.icon);
                                        }

                                        icon.gameObject.SetActive(true);
                                        fightText.text = CommonData.GetLocalString("出战");
                                  

                                    }
                                }
                                else
                                {
                                    icon.gameObject.SetActive(false);
                                    //spr.spriteName = "";
                                    spr.mainTexture = null;
                                    fightText.text = CommonData.GetLocalString("设置");

                                }
                            }
                        }
                    }
                }
            }
        }
    }
    public void OnDestroy()
    {
        m_petData.ValueUpdateEvent -= m_petData_ValueUpdateEvent;
    }

    protected override void OnShow(object data)
    {
        m_transChanZhanBtnList.Clear();
        m_removeBtnList.Clear();
        int count = m_trans_petGroup.childCount;
        for (int i = 0; i < count; i++)
        {
            string name = "pet_" + (i + 1).ToString();
            Transform item = m_trans_petGroup.Find(name);
            if (item != null)
            {
                UIEventListener.Get(item.gameObject).onClick = OnPetClick;
            }
            Transform chuzhanBtn = item.Find("btn");
            if (chuzhanBtn != null)
            {
                UIEventListener.Get(chuzhanBtn.gameObject).onClick = OnChuZhanClick;
            }
            m_transChanZhanBtnList.Add(chuzhanBtn);

            Transform removeBtn = item.Find("removebtn");
            if (removeBtn != null)
            {
                UIEventListener.Get(removeBtn.gameObject).onClick = OnRemoveClick;
            }
            m_removeBtnList.Add(removeBtn);

            //Transform countdown = chuzhanBtn.Find( "Label" );
            //if(countdown != null)
            //{
            //    UILabel label = countdown.GetComponent<UILabel>();
            //    if(label != null)
            //    {
            //        fightCountDownList.Add( label );
            //    }
            //}
            Transform relive = item.Find("status_time");
            if (relive != null)
            {
                relieveList.Add(relive);
                Transform fightcd = relive.Find("Label");
                if (fightcd != null)
                {
                    UILabel label = fightcd.GetComponent<UILabel>();
                    if (label != null)
                    {
                        label.gameObject.SetActive(false);
                        fighingCDList.Add(label);
                    }
                }
            }
          

        }
        RefreshQuickSetting();
        ShowSetBtn();
        base.OnShow(data);
    }
    void OnChuZhanClick(GameObject go)
    {
        Transform parent = go.transform.parent;
        if (parent != null)
        {
            string name = parent.name;
            if (!string.IsNullOrEmpty(name))
            {
                char indexStr = name[name.Length - 1];
                int index = -1;
                if (int.TryParse(indexStr.ToString(), out index))
                {
                    index -= 1;
                    if (index != -1)
                    {
                        List<uint> petList = m_petData.GetUserQuicSettingList();
                        if (petList != null)
                        {
                            uint petid = petList[index];
                            if (petid == 0)
                            {
                                OnPetClick(null);
                                //TipsManager.Instance.ShowTips( "没有设置" );
                                return;
                            }
                            IPet pet = m_petData.GetPetByThisID(petid);
                            if (pet != null)
                            {
                                int life = pet.GetProp((int)PetProp.Life);
                                if (life <= 0)
                                {
                                    TipsManager.Instance.ShowTips(LocalTextType.Pet_Age_zhanhunshoumingbuzuwufachuzhan);
                                    //m_petData.ShowTips(108017);
                                    return;
                                }
                                if (petid == m_petData.CurFightingPet)
                                {
                                    //Local_TXT_Notice_Pet_HasChuzhan
                                    TipsManager.Instance.ShowTips(LocalTextType.Local_TXT_Notice_Pet_HasChuzhan);
                                    //TipsManager.Instance.ShowTips("已经出战");
                                    return;
                                }
                                stUseFightPetUserCmd_CS cmd = new stUseFightPetUserCmd_CS();
                                cmd.id = pet.GetID();
                                NetService.Instance.Send(cmd);
                            }

                        }
                    }
                }
            }

        }
    }
    void OnRemoveClick(GameObject go)
    {
        Transform parent = go.transform.parent;
        if (parent != null)
        {
            string name = parent.name;
            if (!string.IsNullOrEmpty(name))
            {
                char indexStr = name[name.Length - 1];
                int index = -1;
                if (int.TryParse(indexStr.ToString(), out index))
                {
                    index -= 1;
                    if (index != -1)
                    {
                        m_petData.SetUserQuickListByIndex(index, 0);
                    }
                }
            }
        }
        m_petData.SendQuickListMsg();
    }
    void OnPetClick(GameObject go)
    {
        DataManager.Manager<UIPanelManager>().ShowPanel(PanelID.PetListPanel);
        ToggleSetting(false);

    }
    void onClick_Btn_setting_Btn(GameObject caster)
    {
        if (!DataManager.Manager<UIPanelManager>().IsShowPanel(PanelID.PetListPanel))
        {
            DataManager.Manager<UIPanelManager>().ShowPanel(PanelID.PetListPanel);
            ToggleSetting(false);
        }
        else
        {

            DataManager.Manager<UIPanelManager>().HidePanel(PanelID.PetListPanel);
        }

    }

    void ShowSetBtn()
    {
        bool bShow = DataManager.Manager<UIPanelManager>().IsShowPanel(PanelID.PetListPanel);
        if (!bShow)
        {
            foreach (var trans in m_transChanZhanBtnList)
            {
                trans.gameObject.SetActive(!bShow);
            }
            for (int i = 0; i < m_transChanZhanBtnList.Count; i++)
            {
                Transform removeTrans = m_removeBtnList[i];
                removeTrans.gameObject.SetActive(bShow);
            }
        }
        else
        {
            for (int i = 0; i < m_transChanZhanBtnList.Count; i++)
            {
                Transform removeTrans = m_removeBtnList[i];
                Transform trans = m_transChanZhanBtnList[i];
                uint show = m_petData.GetUserQuickSettingByIndex(i);
                if (show == 0)
                {
                    trans.gameObject.SetActive(false);
                    removeTrans.gameObject.SetActive(false);
                }
                else
                {
                    trans.gameObject.SetActive(false);
                    removeTrans.gameObject.SetActive(true);
                }
            }
        }
    }
    void ToggleSetting(bool bShow)
    {
        m_btn_btn_setting.gameObject.SetActive(bShow);
        ShowSetBtn();
    }
}

