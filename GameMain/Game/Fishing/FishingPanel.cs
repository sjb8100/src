using Client;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using table;
using UnityEngine;


partial class FishingPanel
{
    #region interface

    uint m_fishingCD;

    float m_degOffset;

    UIPanelManager panelMgr;

    TweenRotation tr;

    CMResAsynSeedData<CMTexture> m_iconAsynSeed = null;

    CMResAsynSeedData<CMAtlas> iuiBorderAtlas = null;

    #endregion

    #region override


    protected override void OnLoading()
    {
        base.OnLoading();

        m_fishingCD = GameTableManager.Instance.GetGlobalConfig<uint>("FS_CD");

        panelMgr = DataManager.Manager<UIPanelManager>();

        tr = m_sprite_Pointer.GetComponent<TweenRotation>();
    }

    protected override void OnPrepareShow(object data)
    {
        base.OnPrepareShow(data);
    }

    protected override void OnShow(object data)
    {
        //关闭收鱼按钮
        m_trans_FishingContent.gameObject.SetActive(false);
        m_trans_Center.gameObject.SetActive(false);
        m_trans_PointerContent.gameObject.SetActive(false);

        //初始化指针
        InitPointer();

        //自己的排行数据
        UpdateMyRank();
    }

    protected override void OnHide()
    {
        base.OnHide();
    }

    protected override void OnPanelBaseDestory()
    {
        base.OnPanelBaseDestory();

    }


    public override void Release(bool depthRelease = true)
    {
        base.Release(depthRelease);

        if (m_iconAsynSeed != null)
        {
            m_iconAsynSeed.Release(depthRelease);
            m_iconAsynSeed = null;
        }

        if (iuiBorderAtlas != null)
        {
            iuiBorderAtlas.Release(depthRelease);
            iuiBorderAtlas = null;
        }

        ReleaseEffect();
    }

    protected override void OnJump(UIPanelBase.PanelJumpData jumpData)
    {
        base.OnJump(jumpData);

    }


    public override bool OnTogglePanel(int tabType, int pageid)
    {
        return base.OnTogglePanel(tabType, pageid);
    }


    public override bool OnMsg(UIMsgID msgid, object param)
    {

        if (msgid == UIMsgID.eFishingGetOne)
        {
            InitPointer();
        }
        else if (msgid == UIMsgID.eFishingSuccess)
        {
            InitSuccessFishing();
        }
        else if (msgid == UIMsgID.eFishingMyRank)
        {
            UpdateMyRank();
        }

        return true;
    }

    #endregion

    #region mono

    void Update()
    {
        if (false == DataManager.Manager<FishingManager>().IsFishing)
        {
            return;
        }

        if (panelMgr.IsShowPanel(PanelId))
        {
            //钓鱼时间
            UpdateFishingDisplay();

            //上鱼倒计时（倒计时转圈）
            UpdateFishingUpDisplay();
        }
    }

    /// <summary>
    /// 钓鱼倒计时
    /// </summary>
    void UpdateFishingDisplay()
    {
        float leftTime1 = DataManager.Manager<FishingManager>().FishingTime - Time.realtimeSinceStartup;
        if (leftTime1 > 0)
        {
            if (m_trans_FishTimeContent.gameObject.activeSelf == false)
            {
                m_trans_FishTimeContent.gameObject.SetActive(true);
            }

            m_slider_FishTimeBg.value = leftTime1 * 1f / m_fishingCD;
        }
        else
        {
            if (m_trans_FishTimeContent.gameObject.activeSelf == true)
            {
                m_trans_FishTimeContent.gameObject.SetActive(false);
            }
        }
    }

    /// <summary>
    /// 上鱼倒计时（倒计时转圈）
    /// </summary>
    void UpdateFishingUpDisplay()
    {
        float fishingUpTime = DataManager.Manager<FishingManager>().FishingUpTime;
        float fishingUpCd = DataManager.Manager<FishingManager>().FishingUpCd;

        //string s = string.Format("{0}--{1}--{2}", fishingUpTime, fishingUpTime + fishingUpCd, Time.realtimeSinceStartup);
        //Engine.Utility.Log.Error(s);

        if (Time.realtimeSinceStartup > fishingUpTime && Time.realtimeSinceStartup < fishingUpTime + fishingUpCd)
        {
            //1、 倒计时转圈
            if (m_trans_FishingContent.gameObject.activeSelf == false)
            {
                m_trans_FishingContent.gameObject.SetActive(true);
                PlayCircleEffect();
            }

            float leftTime2 = fishingUpTime + fishingUpCd - Time.realtimeSinceStartup;
            if (fishingUpCd != 0)
            {
                m_sprite_StartFishingBtnBg.fillAmount = leftTime2 / fishingUpCd;
            }

            //2、半圆弧倒计时
            if (m_trans_PointerContent.gameObject.activeSelf == false)
            {
                m_trans_PointerContent.gameObject.SetActive(true);
            }

        }
        else
        {
            //1、 倒计时转圈
            if (m_trans_FishingContent.gameObject.activeSelf == true)
            {
                m_trans_FishingContent.gameObject.SetActive(false);
                onClick_StartFishingBtn_Btn(null);
            }

            //2、半圆弧倒计时
            if (m_trans_PointerContent.gameObject.activeSelf == true)
            {
                m_trans_PointerContent.gameObject.SetActive(false);
            }
        }
    }

    #endregion

    #region method

    /// <summary>
    /// 初始化指针
    /// </summary>
    void InitPointer()
    {
        uint fishingUpFishId = DataManager.Manager<FishingManager>().FishingUpFishId;
        FishingDataBase fishingDb = GameTableManager.Instance.GetTableItem<FishingDataBase>(fishingUpFishId);

        //初始化指针
        if (tr != null)
        {
            tr.ResetToBeginning();
            tr.PlayForward();
            if (fishingDb != null)
            {
                tr.duration = fishingDb.ArcTime;
            }
        }

        //初始化转盘区域
        m_sprite_PointerBg2.transform.eulerAngles = new Vector3(0, -90, 0);

        if (fishingDb != null)
        {
            uint deg = fishingDb.pointerRange;        //加成区域圆弧度数
            float fillAmount = deg * 1f / 180f;
            m_sprite_PointerBg2.fillAmount = fillAmount;

            this.m_degOffset = GetDegOffset();
            m_sprite_PointerBg2.transform.eulerAngles = new Vector3(0, 0, -90 + deg * 0.5f + this.m_degOffset);
        }
        else
        {
            m_sprite_PointerBg2.fillAmount = 0;
        }
    }

    /// <summary>
    /// 深色弧度的一半
    /// </summary>
    /// <returns></returns>
    float GetHalfDeg()
    {
        uint fishingUpFishId = DataManager.Manager<FishingManager>().FishingUpFishId;
        FishingDataBase fishingDb = GameTableManager.Instance.GetTableItem<FishingDataBase>(fishingUpFishId);

        if (fishingDb != null)
        {
            uint deg = fishingDb.pointerRange;  //加成区域圆弧度数
            return deg * 0.5f;
        }
        else
        {
            Engine.Utility.Log.Error(" fishingUpFishId  出错了  取不到数据！！！ ");
            return 0;
        }
    }

    /// <summary>
    /// 获取深色弧形偏移
    /// </summary>
    /// <returns></returns>
    float GetDegOffset()
    {
        float halfDeg = GetHalfDeg();
        float leftLimit = -45 + halfDeg;
        float rightLimit = 45 - halfDeg;
        return UnityEngine.Random.Range(leftLimit, rightLimit);
    }

    /// <summary>
    /// 获取钓鱼加成
    /// </summary>
    /// <returns></returns>
    uint GetAddIndex()
    {
        uint fishingUpFishId = DataManager.Manager<FishingManager>().FishingUpFishId;
        FishingDataBase fishingDb = GameTableManager.Instance.GetTableItem<FishingDataBase>(fishingUpFishId);

        if (fishingDb != null)
        {
            uint deg = fishingDb.pointerRange;///加成区域圆弧度数
            if (tr != null)
            {
                Vector3 eulerAngles = tr.value.eulerAngles;
                float z = eulerAngles.z > 300 ? eulerAngles.z - 360 : eulerAngles.z;
                //Engine.Utility.Log.Error("--->>> 指针= " + z + " 0.5deg =" + deg * 0.5f + " offset=" + this.m_degOffset);

                if (z > -deg * 0.5f + this.m_degOffset && z < deg * 0.5f + this.m_degOffset)
                {
                    //高加成的
                    Engine.Utility.Log.Error("--->>> 加成为2 ！！！");
                    return 2;
                }
            }
        }

        Engine.Utility.Log.Error("--->>> 加成为1 ！！！");
        return 1;
    }

    /// <summary>
    /// 钓上鱼了
    /// </summary>
    void InitSuccessFishing()
    {
        m_trans_Center.gameObject.SetActive(true);

        //鱼
        uint FishId = DataManager.Manager<FishingManager>().FishId;
        FishingDataBase fishingDb = GameTableManager.Instance.GetTableItem<FishingDataBase>(FishId);
        if (fishingDb != null)
        {
            // name
            m_label_fishname_label.text = fishingDb.strName;

            // icon
            UIManager.GetTextureAsyn(fishingDb.icon, ref m_iconAsynSeed, () =>
            {
                if (null != m__icon)
                {
                    m__icon.mainTexture = null;
                }
            }, m__icon, false);

            //border
            string borderName = ItemDefine.GetItemBorderIcon(fishingDb.borderId);
            UIManager.GetAtlasAsyn(UIManager.GetIconName(borderName, false), ref iuiBorderAtlas, () =>
            {
                if (m_sprite_qualitybox != null)
                {
                    m_sprite_qualitybox.atlas = null;
                }
            }, m_sprite_qualitybox, false);

            //积分
            m_label_score_label.text = fishingDb.score.ToString();

            //特效
            PlayFishIconEffect();
        }

        StartCoroutine(DelayToFalse());
    }

    IEnumerator DelayToFalse()
    {
        yield return new WaitForSeconds(1);
        m_trans_Center.gameObject.SetActive(false);
    }

    /// <summary>
    /// 自己的排行
    /// </summary>
    void UpdateMyRank()
    {
        m_label_score_num.text = DataManager.Manager<FishingManager>().FishingScore.ToString();

        m_label_rank_num.text = DataManager.Manager<FishingManager>().FishingRank.ToString();
    }

    /// <summary>
    /// 上鱼圆圈特效
    /// </summary>
    void PlayCircleEffect()
    {
        UIParticleWidget wight = m_btn_StartFishingBtn.GetComponent<UIParticleWidget>();
        if (null == wight)
        {
            wight = m_btn_StartFishingBtn.gameObject.AddComponent<UIParticleWidget>();
            wight.depth = 100;
        }

        if (wight != null)
        {
            wight.ReleaseParticle();
            wight.AddParticle(50045);
        }
    }

    /// <summary>
    /// 鱼icon特效
    /// </summary>
    void PlayFishIconEffect()
    {
        UIParticleWidget wight = m__icon.GetComponent<UIParticleWidget>();
        if (null == wight)
        {
            wight = m__icon.gameObject.AddComponent<UIParticleWidget>();
            wight.depth = 100;
        }

        if (wight != null)
        {
            wight.ReleaseParticle();
            wight.AddParticle(50044);
        }
    }

    /// <summary>
    /// 释放特效
    /// </summary>
    void ReleaseEffect()
    {
        if (m_btn_StartFishingBtn != null)
        {
            UIParticleWidget p = m_btn_StartFishingBtn.GetComponent<UIParticleWidget>();
            if (p != null)
            {
                p.ReleaseParticle();
            }
        }

        if (m__icon != null)
        {
            UIParticleWidget p = m__icon.GetComponent<UIParticleWidget>();
            if (p != null)
            {
                p.ReleaseParticle();
            }
        }
    }

    #endregion

    #region click

    /// <summary>
    /// 开始钓鱼
    /// </summary>
    /// <param name="caster"></param>
    void onClick_StartFishingBtn_Btn(GameObject caster)
    {
        //收杆
        DataManager.Manager<FishingManager>().FinishFishing(GetAddIndex());

        //延时2s后，第二次钓鱼
        DataManager.Manager<FishingManager>().NextFishing();

        //转圈关闭(同时清数据)
        m_trans_FishingContent.gameObject.SetActive(false);
        DataManager.Manager<FishingManager>().FishingUpTime = 0;
        DataManager.Manager<FishingManager>().FishingUpCd = 0;

        //钓鱼引导倒计时进度条关闭（同时清数据）
        m_trans_FishTimeContent.gameObject.SetActive(false);
        DataManager.Manager<FishingManager>().FishingTime = 0;
    }

    /// <summary>
    /// 退出钓鱼
    /// </summary>
    /// <param name="caster"></param>
    void onClick_ExitFishingBtn_Btn(GameObject caster)
    {
        DataManager.Manager<FishingManager>().ReqCloseFishing();

        HideSelf();
    }

    /// <summary>
    /// 起杆
    /// </summary>
    /// <param name="caster"></param>
    void onClick_StartFishingBtnBg_Btn(GameObject caster)
    {
        DataManager.Manager<FishingManager>().FinishFishing(GetAddIndex());

        m_trans_FishingContent.gameObject.SetActive(false);
    }

    /// <summary>
    /// 排行榜
    /// </summary>
    /// <param name="caster"></param>
    void onClick_Rank_btn_Btn(GameObject caster)
    {
        DataManager.Manager<UIPanelManager>().ShowPanel(PanelID.FishingRankPanel);

        DataManager.Manager<FishingManager>().ReqFishRanking();
    }

    #endregion

}

