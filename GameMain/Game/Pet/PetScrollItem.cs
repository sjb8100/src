using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Client;
class PetScrollItem : UIGridBase
{
    Transform petLockBtn;
    Transform petIconBtn;
    Transform addPetBtn;
    UILabel petNameLabel;
    UISprite petFlag;

    Transform fightFlag;
    IPet selectPet;
    public IPet PetData { get { return selectPet; } }
    Transform m_reliveTrans;
    UILabel m_reliveLabel;

    UILabel m_fightCdLabel;
    UILabel m_cdtimeLabel;
    UILabel m_levelLabel;
    bool m_bInit = false;
    public enum PetItemShowType
    {
        Lock = 0,
        Normal = 1,
        Add,
    }
    PetDataManager petDataManager
    {
        get
        {
            return DataManager.Manager<PetDataManager>();
        }
    }
    protected override void OnAwake()
    {
        base.OnAwake();
        InitControl();
    }
 
    void InitControl()
    {
        if (m_bInit)
        {
            return;
        }
        petLockBtn = transform.Find("bg/petlock");
        if (petLockBtn != null)
        {
            petLockBtn.gameObject.SetActive(false);
        }
        petIconBtn = transform.Find("bg/peticon");
        addPetBtn = transform.Find("bg/addpet");
        Transform level = transform.Find("bg/petlevel");
        if (level != null)
        {
            m_levelLabel = level.GetComponent<UILabel>();
            if (m_levelLabel != null)
            {
                m_levelLabel.text = "";
            }
        }
        Transform petName = transform.Find("bg/petname");
        if (petName != null)
        {
            petNameLabel = petName.GetComponent<UILabel>();
        }
        fightFlag = transform.Find("sign_fight");
        UIEventListener.Get(this.gameObject).onClick = OnClickItem;
        m_reliveTrans = transform.Find("ReLiveTime");
        if (m_reliveTrans != null)
        {
            m_reliveLabel = m_reliveTrans.GetComponent<UILabel>();
        }
        Transform fightcd = transform.Find("fighttime");
        if (fightcd != null)
        {
            m_fightCdLabel = fightcd.GetComponent<UILabel>();
        }
        Transform cdtime = transform.Find("cdtime");
        if (cdtime != null)
        {
            m_cdtimeLabel = cdtime.GetComponent<UILabel>();
        }
        m_bInit = true;
    }
    protected override void OnStart()
    {

        if (this.gameObject.name == "000")
        {
            Transform spr = transform.Find("Sprite");
            if (spr != null)
            {
                spr.gameObject.SetActive(true);
            }
        }
        m_reliveTrans.gameObject.SetActive(false);
    }
    protected override void OnEnable()
    {
        petDataManager.ValueUpdateEvent += OnValueUpdateEventArgs;
    }

    protected override void OnDisable()
    {
        petDataManager.ValueUpdateEvent -= OnValueUpdateEventArgs;
    }
    void OnValueUpdateEventArgs(object obj, ValueUpdateEventArgs v)
    {
        if (v != null)
        {
            if (v.key == PetDispatchEventString.ChangeFight.ToString())
            {
                if (fightFlag != null)
                {
                    if (selectPet != null)
                    {
                        if (petDataManager.CurPet.GetID() == selectPet.GetID())
                        {
                            if (petDataManager.IsCurPetFighting())
                            {
                                fightFlag.gameObject.SetActive(true);
                                return;
                            }
                        }
                    }
                    fightFlag.gameObject.SetActive(false);
                }
                return;
            }
        }
        if (selectPet != null)
        {
            petNameLabel.text = petDataManager.GetPetName(selectPet);
            m_levelLabel.text = petDataManager.GetPetLvelStr(selectPet.GetID());
        }
        Transform gaoliangSpr = transform.Find("Sprite");
        if (gaoliangSpr != null)
        {
            if (selectPet == null)
            {
                gaoliangSpr.gameObject.SetActive(false);
                return;
            }
            if (petDataManager.CurPetThisID == selectPet.GetID())
            {
                gaoliangSpr.gameObject.SetActive(true);
            }
            else
            {
                gaoliangSpr.gameObject.SetActive(false);
            }

        }
    }
    int m_nFrameNum = 0;
    void Update()
    {
        m_nFrameNum++;
        if (m_nFrameNum > 100000)
        {
            m_nFrameNum = 0;
        }
        if (m_nFrameNum % 20 != 0)
        {
            return;
        }
        if (selectPet != null)
        {
            int time = petDataManager.GetRelieveCDTime(selectPet.GetID());
          //  int fightCd = petDataManager.GetPetFightCDTime(selectPet.GetID());
         //   int showTime = fightCd > time ? fightCd : time;

            if (time > 0)
            {
                #region 出战cd不在显示
                //m_cdtimeLabel.text = showTime.ToString();
                //m_cdtimeLabel.gameObject.SetActive(true);
                //if (fightCd > time)
                //{//复活cd不在显示
                //    //m_fightCdLabel.gameObject.SetActive(true);
                //    //m_reliveLabel.gameObject.SetActive(false);
                //    //m_cdtimeLabel.color = Color.white;
                //}
                //else
                //{
                //    m_cdtimeLabel.color = Color.red;
                //    m_fightCdLabel.gameObject.SetActive(false);
                //    m_reliveLabel.gameObject.SetActive(true);
                //}
                #endregion
                if (m_cdtimeLabel.gameObject.activeSelf)
                {
                    m_cdtimeLabel.text = time.ToString();
                }
                else
                {
                    m_cdtimeLabel.text = time.ToString();
                    m_cdtimeLabel.gameObject.SetActive(true);
                }
               
            }
            else
            {
                if(m_cdtimeLabel.gameObject.activeSelf)
                {
                    m_cdtimeLabel.gameObject.SetActive(false);
                    m_fightCdLabel.gameObject.SetActive(false);
                    m_reliveLabel.gameObject.SetActive(false);
                }
              
            }

        }

    }

    protected override void OnUIBaseDestroy()
    {
        petDataManager.ValueUpdateEvent -= OnValueUpdateEventArgs;
    }
    void OnClickItem(GameObject go)
    {
        if (petIconBtn.gameObject.activeSelf)
        {
            Transform parent = transform.parent;
            for (int i = 0; i < parent.childCount; i++)
            {
                Transform ct = parent.GetChild(i);
                Transform spr = ct.Find("Sprite");
                if (ct.name != go.name)
                {
                    spr.gameObject.SetActive(false);
                }
                else
                {
                    spr.gameObject.SetActive(true);
                }
            }

        }
        if (petLockBtn.gameObject.activeSelf)
        {
            DataManager.Manager<UIPanelManager>().ShowPanel(PanelID.UnlockPanel);
        }
        else
        {

            if (selectPet != null)
            {
                petDataManager.CurPetThisID = selectPet.GetID();
            }
        }
        return;
    }
    void ShowPet(PetItemShowType type)
    {

        InitControl();

        if (type == PetItemShowType.Normal)
        {
            petIconBtn.gameObject.SetActive(true);
            petLockBtn.gameObject.SetActive(false);
            addPetBtn.gameObject.SetActive(false);
        }
        else if (type == PetItemShowType.Lock)
        {
            petIconBtn.gameObject.SetActive(false);
            petLockBtn.gameObject.SetActive(true);
            addPetBtn.gameObject.SetActive(false);
        }
        else if (type == PetItemShowType.Add)
        {
            petIconBtn.gameObject.SetActive(false);
            petLockBtn.gameObject.SetActive(false);
            addPetBtn.gameObject.SetActive(true);
        }
    }
    CMResAsynSeedData<CMTexture> m_curIconAsynSeed = null;
    CMResAsynSeedData<CMAtlas> m_curQualityAsynSeed = null;
    public void UpdatePetItemData(IPet data, PetItemShowType type = PetItemShowType.Normal)
    {
        selectPet = data;
        ShowPet(type);
        if (data != null)
        {
            table.PetDataBase petdb = petDataManager.GetPetDataBase(data.PetBaseID);
            if (petdb != null)
            {
                petNameLabel.text = petDataManager.GetPetName(data);
                if (selectPet != null)
                {
                    m_levelLabel.text = petDataManager.GetPetLvelStr(selectPet.GetID());
                }
                if (petIconBtn != null)
                {
                    //UIButton spr = petIconBtn.GetComponent<UIButton>();
                    //if (spr != null)
                    {
                        UITexture petspr = petIconBtn.GetComponent<UITexture>();
                        if(petspr)
                        {
                          //  DataManager.Manager<UIManager>().SetSpriteDynamicIcon(petspr, petdb.icon);
                            UIManager.GetTextureAsyn(petdb.icon, ref m_curIconAsynSeed, () =>
                            {
                                if (null != petspr)
                                {
                                    petspr.mainTexture = null;
                                }
                            }, petspr, false);
                            Transform bgTrans = petspr.transform.Find("Pingzhi_bg");
                            if (bgTrans)
                            {
                                UISprite bgSpr = bgTrans.GetComponent<UISprite>();
                                if (bgSpr)
                                {
                                    string bgName = ItemDefine.GetItemFangBorderIconByItemID(petdb.ItemID);
                                    UIManager.GetAtlasAsyn(bgName, ref m_curQualityAsynSeed, () =>
                                    {
                                        if (null != bgSpr)
                                        {
                                            bgSpr.atlas = null;
                                        }
                                    }, bgSpr);
                                }
                            }
                           
                        }
                       // spr.normalSprite = petdb.icon;
                    }
                }
            }
            if (petDataManager.CurFightingPet == selectPet.GetID())
            {
                fightFlag.gameObject.SetActive(true);
            }
            else
            {
                fightFlag.gameObject.SetActive(false);
            }
        }
        else
        {

            if (fightFlag == null)
            {
                return;
            }
            fightFlag.gameObject.SetActive(false);
            m_levelLabel.text = "";
            if (type == PetItemShowType.Add)
            {
                UISprite spr = addPetBtn.Find("iconspr").GetComponent<UISprite>();
                if (spr != null)
                {
                    spr.gameObject.SetActive(false);
                }
                petNameLabel.text = "";
            }
            else if (type == PetItemShowType.Lock)
            {
                petNameLabel.color = Color.white;
                petNameLabel.text = ColorManager.GetColorString(0,144,255,255, "[u]" + CommonData.GetLocalString("增加珍兽上限") + "[/u]");
            }
            Transform gaoliangSpr = transform.Find("Sprite");
            if (gaoliangSpr != null)
            {
                if (selectPet == null)
                {
                    gaoliangSpr.gameObject.SetActive(false);
                    return;
                }
                if (petDataManager.CurPetThisID == selectPet.GetID())
                {
                    gaoliangSpr.gameObject.SetActive(true);
                }
                else
                {
                    gaoliangSpr.gameObject.SetActive(false);
                }

            }
        }

    }
    public override void Release(bool depthRelease = true)
    {
        base.Release(depthRelease);
        if (m_curIconAsynSeed != null)
        {
            m_curIconAsynSeed.Release(depthRelease);
            m_curIconAsynSeed = null;
        }
        if (m_curQualityAsynSeed != null)
        {
            m_curQualityAsynSeed.Release(depthRelease);
            m_curQualityAsynSeed = null;
        }
    }
}
