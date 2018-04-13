/************************************************************************************
 * Copyright (c) 2017  All Rights Reserved.
 * CLR版本： 4.0.30319.42000
 * 公司名称：ZQB
 * 命名空间：GameMain.UI.Grid.Grids
 * 创建人：  wenjunhua.zqgame
 * 文件名：  UIItemInfoGrid
 * 版本号：  V1.0.0.0
 * 创建时间：2/7/2017 2:50:46 PM
 * 描述：
 ************************************************************************************/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class UIItemInfoGrid : UIGridBase
{
    #region property
    //图标
    private UITexture m_sp_icon;
    //数量
    private UILabel m_lab_num;
    //装备对比状态图标
    private Transform m_ts_fightUpMask;
    //装备对比状态图标
    private Transform m_ts_fightDownMask;
    private Transform m_ts_powerContent;
    //警告遮罩
    private UISprite m_sp_warningMask;
    //物品边框
    private UISprite m_sp_border;
    //锁
    private GameObject m_obj_lockmask = null;
    //限时标示
    private GameObject m_obj_timeLimitmask = null;
    //道具不足
    private bool m_bool_notEngouGet = false;
    public bool NotEnough
    {
        get
        {
            return m_bool_notEngouGet;
        }
    }
    private Transform m_ts_notEnoughGet = null;

    //圣魂
    private GameObject m_obj_muhonMaskContent;
    //星级
    private UISlider m_sl_starLv;
    private UILabel m_lab_muhonLv = null;
    private Transform m_tsMuhonLv = null;

    //符石
    private GameObject m_obj_runestoneMaskContent;
    //符石档次
    private UILabel m_lab_rsGrade;

    //高亮
    private Transform m_ts_highLight;

    //碎片遮罩
    private Transform m_tsChipsMask;

    //背景框
    private Transform mtsBg;
    #endregion

    #region overrideMethod

    protected override void OnAwake()
    {
        base.OnAwake();
        mtsBg = CacheTransform.Find("Content/Bg");
        Transform tempTs = CacheTransform.Find("Content/Icon");
        if (null != tempTs)
        {
            m_sp_icon = tempTs.GetComponent<UITexture>();
        }
        tempTs = CacheTransform.Find("Content/Border");
        if (null != tempTs)
        {
            m_sp_border = tempTs.GetComponent<UISprite>();
        }
        tempTs = CacheTransform.Find("Content/Num");
        if (null != tempTs)
        {
            m_lab_num = tempTs.GetComponent<UILabel>();
            m_lab_num.depth = 15;
            m_lab_num.color = ColorManager.GetColor32OfType(ColorType.White);
        }
        m_ts_powerContent = CacheTransform.Find("Content/PowerContent");
        m_ts_fightUpMask = CacheTransform.Find("Content/PowerContent/PowerPromote");
        m_ts_fightDownMask = CacheTransform.Find("Content/PowerContent/PowerFallOff");
        tempTs = CacheTransform.Find("Content/WarningMask");
        if (null != tempTs)
        {
            m_sp_warningMask = tempTs.GetComponent<UISprite>();
        }
        m_ts_highLight = CacheTransform.Find("Content/HighLight");

        tempTs = CacheTransform.Find("Content/Lock");
        if (null != tempTs)
        {
            m_obj_lockmask = tempTs.gameObject;
        }
        tempTs = CacheTransform.Find("Content/Clock");
        if (null != tempTs)
        {
            m_obj_timeLimitmask = tempTs.gameObject;
        }
        tempTs = CacheTransform.Find("Content/MuhonMask");
        if (null != tempTs)
        {
            m_obj_muhonMaskContent = tempTs.gameObject;
        }
        
        tempTs = CacheTransform.Find("Content/MuhonMask/StarSlider");
        if (null != tempTs)
        {
            m_sl_starLv = tempTs.GetComponent<UISlider>();
        }
        m_tsMuhonLv = CacheTransform.Find("Content/MuhonLevel");
        tempTs = CacheTransform.Find("Content/MuhonLevel/Grade/RsGrade");
        if (null != tempTs)
        {
            m_lab_muhonLv = tempTs.GetComponent<UILabel>();
        }

        tempTs = CacheTransform.Find("Content/RunestoneMask");
        if (null != tempTs)
        {
            m_obj_runestoneMaskContent = tempTs.gameObject;
        }
        tempTs = CacheTransform.Find("Content/RunestoneMask/Grade/RsGrade");
        if (null != tempTs)
        {
            m_lab_rsGrade = tempTs.GetComponent<UILabel>();
        }


        m_ts_notEnoughGet = CacheTransform.Find("Content/NotEnoughGetMask");
        if (null != m_ts_notEnoughGet)
        {
            tempTs = m_ts_notEnoughGet.Find("Get");
            if (null != tempTs)
            {
                UILabel label = tempTs.GetComponent<UILabel>();
                if (null != label)
                {
                    label.fontSize = 22;
                    label.color = ColorManager.GetColor32OfType(ColorType.Green);
                }
            }
        }
        UIWidget widgets = CacheTransform.GetComponent<UIWidget>();
        if (null == widgets)
        {
            widgets = CacheTransform.gameObject.AddComponent<UIWidget>();
        }
        if (null != widgets)
        {
            widgets.depth = 22;
        }

        m_tsChipsMask = CacheTransform.Find("Content/suipian");

    }

    public override void Release(bool depthRelease = true)
    {
        base.Release(depthRelease);
        if (null != iuiIconAtlas)
        {
            iuiIconAtlas.Release(true);
            iuiIconAtlas = null;
        }

        if (null != iuiBorderAtlas)
        {
            iuiBorderAtlas.Release(true);
            iuiBorderAtlas = null;
        }
    }

    public override void Reset()
    {
        base.Reset();
        SetBg(true);
        SetIcon(false);
        SetBorder(false);
        SetFightPower(false);
        SetWarningMask(false);
        SetLockMask(false);
        SetNum(false);
        SetTimeLimitMask(false);
        SetMuhonMask(false);
        SetRunestoneMask(false);
        SetNotEnoughGet(false);
        SetHightLight(false);
        SetChips(false);
        SetMuhonLv(false);
    }

    public override void SetHightLight(bool hightLight)
    {
        base.SetHightLight(hightLight);
        if (m_ts_highLight != null && m_ts_highLight.gameObject.activeSelf != hightLight)
        {
            m_ts_highLight.gameObject.SetActive(hightLight);
        }
    }

    #endregion

    #region Set
    public void SetChips(bool enable)
    {
        if (null != m_tsChipsMask && m_tsChipsMask.gameObject.activeSelf != enable)
        {
            m_tsChipsMask.gameObject.SetActive(enable);
        }
    }

    public void SetBg(bool enable)
    {
        if (null != mtsBg && mtsBg.gameObject.activeSelf != enable)
        {
            mtsBg.gameObject.SetActive(enable);
        }
    }
    
    /// <summary>
    /// 设置图标
    /// </summary>
    /// <param name="enable"></param>
    /// <param name="iconName"></param>
    /// <param name="dType"></param>
    public void SetIcon(bool enable, string iconName = "", bool makePerfectPixel = true,bool circleStyle = false)
    {
        if (null != m_sp_icon)
        {
            if (m_sp_icon.enabled != enable)
            {
                m_sp_icon.enabled = enable;
            }
            if (enable)
            {
                UIManager.GetTextureAsyn(DataManager.Manager<UIManager>().GetResIDByFileName(false, iconName, circleStyle), ref iuiIconAtlas, () =>
                {
                    if (null != m_sp_icon)
                    {
                        m_sp_icon.mainTexture = null;
                    }
                }, m_sp_icon);
            }
        }
    }

    private CMResAsynSeedData<CMTexture> iuiIconAtlas = null;

    private CMResAsynSeedData<CMAtlas> iuiBorderAtlas = null;


    /// <summary>
    /// 设置边框
    /// </summary>
    /// <param name="enale"></param>
    /// <param name="iconName"></param>
    public void SetBorder(bool enable, string iconName = "",bool circleStyle = false)
    {
        if (null != m_sp_border)
        {
            if (m_sp_border.enabled != enable)
            {
                m_sp_border.enabled = enable;
            }
            if (enable)
            {
                UIManager.GetAtlasAsyn(DataManager.Manager<UIManager>().GetResIDByFileName(true, iconName, circleStyle)
                    , ref iuiBorderAtlas
                    , () =>
                    {
                        if (null != m_sp_border)
                        {
                            m_sp_border.atlas = null;
                        }
                    }, m_sp_border
                    , UIManager.GetIconName(iconName, circleStyle)
                    , true);
            }
        }
    }
    /// <summary>
    /// 设置叠加数量
    /// </summary>
    /// <param name="eanble"></param>
    /// <param name="num"></param>
    public void SetNum(bool enable, string num = "")
    {
        if (null != m_lab_num)
        {
            if (enable && m_lab_num.enabled != enable)
            {
                m_lab_num.enabled = enable;
            }
            if (!enable)
            {
                m_lab_num.text = "";
            }
            else
            {
                m_lab_num.text = num;
            }
        }
    }

    /// <summary>
    /// 设置警告遮罩
    /// </summary>
    /// <param name="enable"></param>
    public void SetWarningMask(bool enable,bool circleStyle = false)
    {
        if (null != m_sp_warningMask && m_sp_warningMask.gameObject.activeSelf != enable)
        {
            m_sp_warningMask.gameObject.SetActive(enable);
            if (enable)
            {
                if (circleStyle)
                {
                    m_sp_warningMask.spriteName = "biankuang_zhezhao_yuan";
                    m_sp_warningMask.type = UIBasicSprite.Type.Simple;
                    m_sp_warningMask.MakePixelPerfect();
                }
                else
                {
                    m_sp_warningMask.spriteName = "biankuang_hongkuang";
                    m_sp_warningMask.type = UIBasicSprite.Type.Sliced;
                    m_sp_warningMask.width = 80;
                    m_sp_warningMask.height = 80;
                }
            }
        }
    }
    CMResAsynSeedData<CMAtlas> m_playerAvataCASD = null;
    public void SetDurableMask(bool enable)
    {
        if (null != m_sp_warningMask && m_sp_warningMask.gameObject.activeSelf != enable)
        {           
            if (enable)
            {
                m_sp_warningMask.gameObject.SetActive(enable);
                UIManager.GetAtlasAsyn("tubiao_xiuli", ref m_playerAvataCASD, () =>
                {
                    if (null != m_sp_warningMask)
                    {
                        m_sp_warningMask.atlas = null;
                    }
                }, m_sp_warningMask);
                m_sp_warningMask.type = UIBasicSprite.Type.Sliced;
                m_sp_warningMask.width = 80;
                m_sp_warningMask.height = 80;
            }
        }
    
    }

    /// <summary>
    /// 设置战斗力上升
    /// </summary>
    /// <param name="enable"></param>
    public void SetFightPower(bool enable,bool isUp = false)
    {
        if (null != m_ts_powerContent)
        {
            if (m_ts_powerContent.gameObject.activeSelf != enable)
            {
                m_ts_powerContent.gameObject.SetActive(enable);
            }
            if (enable)
            {
                if (null != m_ts_fightUpMask && m_ts_fightUpMask.gameObject.activeSelf != isUp)
                {
                    m_ts_fightUpMask.gameObject.SetActive(isUp);
                }

                if (null != m_ts_fightDownMask && m_ts_fightDownMask.gameObject.activeSelf == isUp)
                {
                    m_ts_fightDownMask.gameObject.SetActive(!isUp);
                }
            }
        }
    }

    /// <summary>
    /// 设置限时标示
    /// </summary>
    /// <param name="enable"></param>
    public void SetTimeLimitMask(bool enable)
    {
        if (null != m_obj_timeLimitmask&& m_obj_timeLimitmask.gameObject.activeSelf != enable)
        {
            m_obj_timeLimitmask.gameObject.SetActive(enable);
        }
    }

    /// <summary>
    /// 设置绑定标示
    /// </summary>
    /// <param name="enable"></param>
    public void SetLockMask(bool enable)
    {
        if (null != m_obj_lockmask && m_obj_lockmask.gameObject.activeSelf != enable)
        {
            m_obj_lockmask.gameObject.SetActive(enable);
        }
    }

    /// <summary>
    /// 设置符石标示
    /// </summary>
    /// <param name="enable"></param>
    /// <param name="grade"></param>
    public void SetRunestoneMask(bool enable,uint grade = 0)
    {
        if (null != m_obj_runestoneMaskContent)
        {
            if (m_obj_runestoneMaskContent.gameObject.activeSelf != enable)
            {
                m_obj_runestoneMaskContent.gameObject.SetActive(enable);
            }
            if (enable && null != m_lab_rsGrade)
            {
                m_lab_rsGrade.text = grade.ToString();
            }

        }
    }

    /// <summary>
    /// 设置圣魂标示
    /// </summary>
    /// <param name="enable">是否启用</param>
    /// <param name="starLv">星级</param>
    public void SetMuhonMask(bool enable,uint starLv = 0)
    {
        if (null != m_obj_muhonMaskContent)
        {
            if (m_obj_muhonMaskContent.gameObject.activeSelf != enable)
            {
                m_obj_muhonMaskContent.gameObject.SetActive(enable);
            }
            if (enable)
            {
                if (null != m_sl_starLv)
                {
                    float value = starLv / 5f;
                    m_sl_starLv.value = value;
                }
            }
        }
    }

    public void SetMuhonLv(bool enable,string txt ="")
    {
        if (null != m_tsMuhonLv)
        {
            if (m_tsMuhonLv.gameObject.activeSelf != enable)
            {
                m_tsMuhonLv.gameObject.SetActive(enable);
            }
            if (enable)
            {
                if (null != m_lab_muhonLv)
                {
                    m_lab_muhonLv.text = txt;
                }
            }
        }
    }

    /// <summary>
    /// 设置道具不足get
    /// </summary>
    /// <param name="enable"></param>
    public void SetNotEnoughGet(bool enable)
    {
        this.m_bool_notEngouGet = enable;
        if (null != m_ts_notEnoughGet && m_ts_notEnoughGet.gameObject.activeSelf != enable)
        {
            m_ts_notEnoughGet.gameObject.SetActive(enable);
        }
    }


    //public override void SetHighLight(bool highLight)
    //{
    //    if (m_ts_highLight != null && m_ts_highLight.gameObject.activeSelf != highLight)
    //    {
    //        m_ts_highLight.gameObject.SetActive(highLight);
    //    }
    //}


    #endregion

}