
//*************************************************************************
//	创建日期:	2018/2/1 星期四 14:36:49
//	文件名称:	RidePanel_Knight
//   创 建 人:   zhudianyu	
//	版权所有:	zhudianyu
//	说    明:	
//*************************************************************************

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GameCmd;
using table;
using Engine.Utility;
using Engine;
using Client;
using UnityEngine;
partial class RidePanel
{
    List<string> m_knightLevelUPIdList = null;
    uint breakItemID = 0;
    CMResAsynSeedData<CMAtlas> m_knightQualityIcon = null;
    CMResAsynSeedData<CMTexture> m_knightTextureSeed = null;
    Color m_beforeColor = new Color(28 * 1.0f / 255, 40 * 1.0f / 255, 50 * 1.0f / 255);
    Color m_afterColor = new Color(9 * 1.0f / 255, 127 * 1.0f / 255, 29 * 1.0f / 255);
    Color GetColorByInt(int r, int g, int b)
    {
        return new Color(r * 1.0f / 255, g * 1.0f / 255, b * 1.0f / 255);
    }
    void ShowRedPoint()
    {
        Transform trans = transform.Find("RightTabs(Clone)/bg/btnRoot/QiShu");
        if(trans != null)
        {
            UITabGrid grid = trans.GetComponent<UITabGrid>();
            if(grid != null)
            {
                grid = trans.gameObject.AddComponent<UITabGrid>();
            }
            if(m_rideMgr.IsShowRideRedPoint())
            {
                grid.SetRedPointStatus(true);
            }
            else
            {
                grid.SetRedPointStatus(false);
            }
        }
    }
    void InitKnightUI()
    {
        ShowRedPoint();
        if (m_knightLevelUPIdList == null)
        {
            m_knightLevelUPIdList = GameTableManager.Instance.GetGlobalConfigKeyList("Knight_ExpItem");
            m_knightLevelUPIdList.Sort();
        }
        if (breakItemID == 0)
        {
            breakItemID = GameTableManager.Instance.GetGlobalConfig<uint>("KngithRankItem");
        }
        rtBaseID = 0;
        InitAttrUI();
        InitRankUI();
        InitRt();
        m_label_qishushuoming.text = DataManager.Manager<TextManager>().GetLocalText(LocalTextType.Ride_Commond_zuoqizhuanhuabi);
        InitUPLevelItem();
        ShowMaxAndBreak();
        RefreshPower();
        RefreshTexture();
    }
    void ReleseKnightTexture()
    {
        if (m_knightTextureSeed != null)
        {
            m_knightTextureSeed.Release();
            m_knightTextureSeed = null;
        }
        if(m_knightQualityIcon != null)
        {
            m_knightQualityIcon.Release();
            m_knightQualityIcon = null;
        }
    }
    void RefreshTexture()
    {
        RideData data = m_rideMgr.GetRideDataById(m_rideMgr.Auto_Ride);
        if (data == null)
        {
            m__selecthorse.mainTexture = null;
            m_label_Ride_Name2.text = string.Empty;
        }
        else
        {
            m_label_Ride_Name2.text = data.name;
            UIManager.GetTextureAsyn(data.icon, ref m_knightTextureSeed, () =>
            {
                if (null != m__selecthorse)
                {
                    m__selecthorse.mainTexture = null;
                }
            }, m__selecthorse, false);

            UIManager.GetQualityAtlasAsyn(data.quality, ref m_knightQualityIcon, () =>
            {
                if (null != m_sprite_biankuang)
                {
                    m_sprite_biankuang.atlas = null;
                }
            }, m_sprite_biankuang, false);
        }
    }
 
    void RefreshPower()
    {
        m_label_knightfightNumber.text = DataManager.Manager<RideManager>().KnightPower.ToString();
    }
    void RefreshKnightLevelAndBreak()
    {
        InitAttrUI();
        InitRankUI();
        ShowMaxAndBreak();
        InitUPLevelItem();

    }
    UIParticleWidget particle = null;
    void AddParticle() 
    {
        if(m_trans_Particle != null)
        {
            if (particle == null)
            {
                particle = m_trans_Particle.gameObject.AddComponent<UIParticleWidget>();
                particle.depth = 20;
            }
            if (particle != null)
            {
                particle.ReleaseParticle();
                particle.SetDimensions(1,1);
                particle.AddParticle(50049);
            }
        }
    }
    void InitAttrUI()
    {
        uint lv = DataManager.Manager<RideManager>().KnightLevel;
        uint curExp = DataManager.Manager<RideManager>().KnightExp;
        uint totalExp = curExp;
        HoursemanShipUPLevel db = GameTableManager.Instance.GetTableItem<HoursemanShipUPLevel>(lv);
        if (db != null)
        {
            SetCurAttr(db, m_beforeColor);
            totalExp = db.uplevelexp;
        }
        //if(db.breakLevel != 0)
        //{
        //    SetNextAttr(db, m_beforeColor);
        //}
        //else
        {

            HoursemanShipUPLevel nextDb = GameTableManager.Instance.GetTableItem<HoursemanShipUPLevel>(lv + 1);
            if (nextDb != null)
            {
                SetNextAttr(nextDb, m_afterColor);
            }
            else
            {
                SetNextAttr(db, m_afterColor);
                SetCurAttr(db, m_afterColor);
            }
        }
        m_label_levelNum.text = lv.ToString();
        m_label_exp2Label.text = string.Format("{0}/{1}", curExp, totalExp);
        if(totalExp != 0)
        {
            float fv = curExp * 1.0f / totalExp;
            m_slider_Exp2Slider.value = fv;
        }
    }
  
    void ShowMaxAndBreak()
    {
        uint lv = DataManager.Manager<RideManager>().KnightLevel;
        uint curExp = DataManager.Manager<RideManager>().KnightExp;
        HoursemanShipUPLevel nextDb = GameTableManager.Instance.GetTableItem<HoursemanShipUPLevel>(lv + 1);
        if (nextDb != null)
        {
            m_trans_Max.gameObject.SetActive(false);
            HoursemanShipUPLevel db = GameTableManager.Instance.GetTableItem<HoursemanShipUPLevel>(lv );
            if(db != null)
            {
                if (db.breakLevel != 0 && curExp >= db.uplevelexp  )
                {
                    m_trans_LevelUp.gameObject.SetActive(false);
                    m_trans_LevelBreak.gameObject.SetActive(true);
                  
                }
                else
                {
                    //没有突破成功的消息发下来  只能通过按钮隐现来判断是突破还是升级
                    if (!m_trans_LevelUp.gameObject.activeSelf)
                    {
                        AddParticle();
                    }
                    m_trans_LevelUp.gameObject.SetActive(true);
                    m_trans_LevelBreak.gameObject.SetActive(false);
                  
                   
                }
            }
           
        }
        else
        {
            m_trans_Max.gameObject.SetActive(true);
            m_trans_LevelUp.gameObject.SetActive(false);
            m_trans_LevelBreak.gameObject.SetActive(false);
        }
    }
    void InitUPLevelItem()
    {
        int n = m_trans_ItemRoot.childCount;

        for (int i = 0; i < n; i++)
        {
            Transform t = m_trans_ItemRoot.Find(i.ToString());
            UIKnightLevelUPItemGrid grid = t.GetComponent<UIKnightLevelUPItemGrid>();
            if (grid == null)
            {
                grid = t.gameObject.AddComponent<UIKnightLevelUPItemGrid>();
            }
            if(m_knightLevelUPIdList == null)
            {
                break;
            }
            if (i < m_knightLevelUPIdList.Count)
            {
                grid.IsBreak = false;
                grid.SetGridInfo( m_knightLevelUPIdList[i]);
                string expstr = GameTableManager.Instance.GetGlobalConfig<string>("Knight_ExpItem", m_knightLevelUPIdList[i]);
                if(expstr != null)
                {
                    if(i == 0)
                    {
                        m_label_exe1Num.text = expstr;
                    }
                    else if(i == 1)
                    {
                        m_label_exe2Num.text = expstr;
                    }
                    else if(i ==2)
                    {
                        m_label_exe3Num.text = expstr;
                    }
                }
            }

        }
        UIKnightLevelUPItemGrid breakGrid = m_trans_breakitem.GetComponent<UIKnightLevelUPItemGrid>();
        if (breakGrid == null)
        {
            breakGrid = m_trans_breakitem.gameObject.AddComponent<UIKnightLevelUPItemGrid>();
        }
        breakGrid.IsBreak = true;
        uint breakLv = m_rideMgr.KnightBreakLevel;
        uint needNum = 0;
        HoursemanShipUPDegree hdb = GameTableManager.Instance.GetTableItem<HoursemanShipUPDegree>(breakLv+1);
        if(hdb != null)
        {
            needNum = hdb.itemNum;
        }
        breakGrid.SetGridInfo(breakItemID.ToString(), needNum) ;
    }

    void InitRankUI()
    {
        uint rank = DataManager.Manager<RideManager>().KnightBreakLevel;
        uint curExp = DataManager.Manager<RideManager>().KnightExp;
        uint lv = DataManager.Manager<RideManager>().KnightLevel;
        HoursemanShipUPDegree db = GameTableManager.Instance.GetTableItem<HoursemanShipUPDegree>(rank);
        if (db != null)
        {
            SetCurRank(db, m_beforeColor);
        }
        HoursemanShipUPLevel updb = GameTableManager.Instance.GetTableItem<HoursemanShipUPLevel>(lv);
        HoursemanShipUPDegree nextdb = GameTableManager.Instance.GetTableItem<HoursemanShipUPDegree>(rank + 1);
        if (nextdb != null && updb.breakLevel != 0 && curExp >= updb.uplevelexp)
        {
            SetNextRank(nextdb, m_afterColor);
        }
        else
        {
            SetCurRank(db, m_beforeColor);
            SetNextRank(db, m_beforeColor);
        }
    }
    uint rtBaseID = 0;
    void InitRt()
    {
        RideData data = m_rideMgr.GetRideDataById(m_rideMgr.Auto_Ride);
        if (data == null)
        {
            if (m_KnightRTObj != null)
            {
                m_KnightRTObj.Release();
            }
            return;
        }
        if (rtBaseID != data.modelid)
        {
            rtBaseID = data.modelid;
            if (m_KnightRTObj != null)
            {
                m_KnightRTObj.Release();
            }

            m_KnightRTObj = DataManager.Manager<RenderTextureManager>().CreateRenderTextureObj((int)rtBaseID, 750);
            if (m_KnightRTObj == null)
            {
                return;
            }
            ModeDiplayDataBase modelDisp = GameTableManager.Instance.GetTableItem<ModeDiplayDataBase>(rtBaseID);
            if (modelDisp == null)
            {
                Engine.Utility.Log.Error("BOSS模型ID为{0}的模型展示数据为空", rtBaseID);
                return;
            }

            m_KnightRTObj.SetDisplayCamera(modelDisp.pos750, modelDisp.rotate750, modelDisp.Modelrotation);
            m_KnightRTObj.PlayModelAni(Client.EntityAction.Stand);
            UIRenderTexture rt = m__rideModel2.GetComponent<UIRenderTexture>();
            if (null == rt)
            {
                rt = m__rideModel2.gameObject.AddComponent<UIRenderTexture>();
            }
            if (null != rt)
            {
                rt.SetDepth(0);
                rt.Initialize(m_KnightRTObj, m_KnightRTObj.YAngle, new UnityEngine.Vector2(750, 750));
            }
        }
    }
    void SetCurRank(HoursemanShipUPDegree db, Color c)
    {
        m_label_sudu.text = string.Format("{0}%", (db.speed / 100)) ;
        m_label_sudu.color = c;
        m_label_zhuanghua.text =string.Format("{0}%", (db.scaling / 100));
        m_label_zhuanghua.color = c;
    }
    void SetNextRank(HoursemanShipUPDegree db, Color c)
    {
        m_label_suduNum.text = string.Format("{0}%", (db.speed / 100));
        m_label_suduNum.color = c;
        m_label_zhuanghuaNum.text = string.Format("{0}%", (db.scaling / 100));
        m_label_zhuanghuaNum.color = c;
    }
    void SetCurAttr(HoursemanShipUPLevel db, Color c)
    {
        if (db == null)
        {
            return;
        }

        m_label_jingli.text = db.jingshen.ToString();
        m_label_zhili.text = db.zhili.ToString();
        m_label_minjie.text = db.minjie.ToString();
        m_label_tili.text = db.tizhi.ToString();
        m_label_liliang.text = db.liliang.ToString();
        m_label_jingli.color = c;
        m_label_zhili.color = c;
        m_label_minjie.color = c;
        m_label_tili.color = c;
        m_label_liliang.color = c;
    }
    void SetNextAttr(HoursemanShipUPLevel db, Color c)
    {
        if (db == null)
        {
            return;
        }
        m_label_jingliNum.text = db.jingshen.ToString();
        m_label_zhiliNum.text = db.zhili.ToString();
        m_label_minjieNum.text = db.minjie.ToString();
        m_label_tiliNum.text = db.tizhi.ToString();
        m_label_liliangNum.text = db.liliang.ToString();
        m_label_jingliNum.color = c;
        m_label_zhiliNum.color = c;
        m_label_minjieNum.color = c;
        m_label_tiliNum.color = c;
        m_label_liliangNum.color = c;
    }
    void onClick_BreakBtn_Btn(GameObject caster)
    {
        stAddKnightRankRideUserCmd_C cmd = new stAddKnightRankRideUserCmd_C();
        NetService.Instance.Send(cmd);
    }

}
