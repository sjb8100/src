//********************************************************************
//	创建日期:	2017-2-24   17:07
//	文件名称:	UINobleGrid.cs
//  创 建 人:   刘超	
//	版权所有:	中青宝
//	说    明:	皇令预制件
//********************************************************************
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GameCmd;
using UnityEngine;
using table;
using Client;
using Engine;
class UINobleGrid : UIGridBase
{
    private UITexture Bg;
    private UILabel name;
    private UILabel validity;
    private UILabel lijihuode;
    private UILabel exp;
    private UILabel expLabel;
    private UILabel meirilingqu;
    private GameObject btn;
    private UILabel btnLabel;
    private GameObject timeRemind;
    private UILabel timeLabel;
    private GameObject moneyRemind;
    private GameObject lingquRemind;
    private UILabel m_label;
    private UISprite redPoint;
    public uint nobleID { set; get; }

    protected override void OnAwake() 
    {
        base.OnAwake();
        Bg = CacheTransform.Find("Bg").GetComponent<UITexture>();
        name = CacheTransform.Find("name").GetComponent<UILabel>();
        validity = CacheTransform.Find("validity").GetComponent<UILabel>();
        lijihuode = CacheTransform.Find("lijihuode/value").GetComponent<UILabel>();
        exp = CacheTransform.Find("exp/value").GetComponent<UILabel>();
        expLabel = CacheTransform.Find("exp/Label").GetComponent<UILabel>();
        meirilingqu = CacheTransform.Find("meirilingqu/value").GetComponent<UILabel>();
        btn = CacheTransform.Find("btn").gameObject;
        btnLabel = CacheTransform.Find("btn/Label").GetComponent<UILabel>();
        timeRemind = CacheTransform.Find("timeRemind").gameObject;
        timeLabel = CacheTransform.Find("timeRemind").GetComponent<UILabel>();
        moneyRemind = CacheTransform.Find("moneyRemind").gameObject;
        lingquRemind = CacheTransform.Find("lingquRemind").gameObject;
        redPoint = CacheTransform.Find("btn/redPoint").GetComponent<UISprite>();
        m_label = CacheTransform.Find("moneyRemind").GetComponent<UILabel>();
    }
    public override void SetGridData(object data) 
    {
        OnAwake();
        base.SetGridData(data);
        nobleID = (uint)data;
        table.NobleDataBase db = GameTableManager.Instance.GetTableItem<table.NobleDataBase>(nobleID);
        if (null != db && null != Bg)
        {
            UIManager.GetTextureAsyn(db.bgResID, ref m_texSeedData, () =>
                {
                    if (null != Bg)
                    {
                        Bg = null;
                    }
                }, Bg,false);
        }
    
    }
    CMResAsynSeedData<CMTexture> m_texSeedData = null;
    public override void Release(bool depthRelease = true)
    {
        base.Release(depthRelease);
        if (null != m_texSeedData)
        {
            m_texSeedData.Release(true);
            m_texSeedData = null;
        }
    }
    
    public  void SetNobleGridData(NobleDataBase data) 
    {
        if (data == null)
        {
            return;
        }
        name.text = data.name;
        m_label.text = data.zengsongdianjuan.ToString();
        validity.text = "时效" + data.days.ToString() + "天";
        expLabel.text = "打怪经验+";
        exp.text = data.ExtreExp / 100 + "%";
        lijihuode.text = data.lidedianjuan.ToString();
        meirilingqu.text = data.zengsongdianjuan.ToString();
        switch(data.dwID)
        {
            case 1:
                break;
            case 2:              
                validity.effectColor = ColorManager.GetColor32OfType(ColorType.JZRY_Noble_Blue);                
                expLabel.effectColor = ColorManager.GetColor32OfType(ColorType.JZRY_Noble_MiddleBlue);           
                exp.effectColor = ColorManager.GetColor32OfType(ColorType.JZRY_Noble_MiddleBlue);           
                lijihuode.effectColor = ColorManager.GetColor32OfType(ColorType.JZRY_Noble_LightBlue);              
                meirilingqu.effectColor = ColorManager.GetColor32OfType(ColorType.JZRY_Noble_LightBlue);             
                break;
            case 3:
                validity.effectColor = ColorManager.GetColor32OfType(ColorType.JZRY_Noble_Pink);
                expLabel.effectColor = ColorManager.GetColor32OfType(ColorType.JZRY_Noble_MiddlePink);
                exp.effectColor = ColorManager.GetColor32OfType(ColorType.JZRY_Noble_MiddlePink);
                lijihuode.effectColor = ColorManager.GetColor32OfType(ColorType.JZRY_Noble_LightPink);
                meirilingqu.effectColor = ColorManager.GetColor32OfType(ColorType.JZRY_Noble_LightPink);    
                break;
            case 4:
                validity.effectColor = ColorManager.GetColor32OfType(ColorType.JZRY_Noble_Yellow);
                expLabel.effectColor = ColorManager.GetColor32OfType(ColorType.JZRY_Noble_MiddleYellow);
                exp.effectColor = ColorManager.GetColor32OfType(ColorType.JZRY_Noble_MiddleYellow);
                lijihuode.effectColor = ColorManager.GetColor32OfType(ColorType.JZRY_Noble_LightYellow);
                meirilingqu.effectColor = ColorManager.GetColor32OfType(ColorType.JZRY_Noble_LightYellow);    
                break;
        }
      
    }
    public void SetBtn(Dictionary<uint,NobleParam>  nd,NobleDataBase table) 
    {
        if (redPoint != null)
        {
           redPoint.gameObject.SetActive(false);
        }
        
        if (nd.ContainsKey(nobleID))
        {
            //买了
            if (nd[nobleID].freeWenqian == 0)
            {
                //今日没领
                timeRemind.SetActive(false);
                moneyRemind.SetActive(true);
                lingquRemind.SetActive(false);
                btnLabel.text = "领 取";
                redPoint.gameObject.SetActive(true);
                UIEventListener.Get(btn).onClick = OnGet;
            }
            else
            {
                //今日已领
                timeRemind.SetActive(true);
                moneyRemind.SetActive(false);
                lingquRemind.SetActive(true);
                btnLabel.text = table.value + "元";
                TimeSpan dt = SetTime(nd[nobleID].time);
                if (dt.Days > 7)
                {
                    timeLabel.text ="剩余期限:"+ (dt.Days).ToString()+"天";
                }
                else 
                {
                    if (dt.Days >= 1)
                    {
                        if (dt.Hours != 0)
                        {
                            timeLabel.text = "剩余期限:" + dt.Days.ToString() + "天" + dt.Hours + "小时";
                        }
                        else 
                        {
                            timeLabel.text = "剩余期限:" + dt.Days.ToString() + "天";
                        }
                    }
                    else 
                    {
                        int hour = dt.Hours;
                        if (hour < 0)
                        {
                            hour = 0;
                        }
                        timeLabel.text = "剩余期限:" + hour + "小时";                      
                    }
                }
                UIEventListener.Get(btn).onClick = OnBuy;
               
            }
        }
        else 
        {
            //没买
            timeRemind.SetActive(false);
            moneyRemind.SetActive(false);
            lingquRemind.SetActive(false);
            btnLabel.text =  table.value + "元";
            UIEventListener.Get(btn).onClick = OnBuy;
        }
    }
    TimeSpan SetTime(uint time)
    {
        DateTime UnixBase = new DateTime(1970, 1, 1, 0, 0, 0);
        TimeSpan t =TimeSpan.FromSeconds( time - (DateTime.Now - UnixBase).TotalSeconds);
            return t;
        }
    void OnGet(GameObject caster) 
    {
        DataManager.Manager<Mall_HuangLingManager>().GetNobleDailyMoney(nobleID);
        
    }
    void OnBuy(GameObject caster) 
    {
        NobleDataBase noble = GameTableManager.Instance.GetTableItem<NobleDataBase>(nobleID);
        Action no = delegate
        {

        };
        Action BuyNoble = delegate
        {
            DataManager.Manager<Mall_HuangLingManager>().BuyNobleRequest(nobleID);
        };
        TipsManager.Instance.ShowTipWindow(TipWindowType.YesNO, string.Format(DataManager.Manager<TextManager>().GetLocalText(LocalTextType.Local_TXT_Notice_Noble_BuyNoble), noble.value, noble.name), BuyNoble, no,null, "提示", "确定", "取消");
       
    }

}