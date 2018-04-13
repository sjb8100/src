
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GameCmd;
class UIMailGrid : UIGridBase
{
    #region Property
    //邮件名称
    private UILabel mail_title;
    //邮件打开状态Icon
    private UISprite mail_icon;
    //邮件是否已阅的红点
    private UISprite red_dot;
    //邮件距离销毁的剩余时间
    private UILabel timeLabel;
    //邮件的背景
    private UISprite Bg;
    UISprite mark;

    private uint mailIndex;
    public uint MailIndex
    {
        get
        {
            return mailIndex;
        }
    }
    ListMailInfo info;
    protected override void OnAwake() 
    {
        base.OnAwake();
        mail_title = CacheTransform.Find("mail_title").GetComponent<UILabel>();
        mail_icon = CacheTransform.Find("mail_icon").GetComponent<UISprite>();
        timeLabel = CacheTransform.Find("left_time").GetComponent<UILabel>();
        red_dot = CacheTransform.Find("red_dot").GetComponent<UISprite>();
        Bg = CacheTransform.Find("item_bg").GetComponent<UISprite>();
        mark = CacheTransform.Find("rewardmark").GetComponent<UISprite>();
    }
    public override void SetGridData(object data) 
    {
        base.SetGridData(data);
        if (null == data)
        {
            return;
        }
        this.info = data as ListMailInfo;
        SetMailData(info);
    }
    public  void SetMailIndex(uint index) 
    {
        this.mailIndex = index;
    }
    public void SetMailData(ListMailInfo info)
    {
        mail_title.text = info.title;
        SetTime(info.leftTime);
    }
    void SetTime(uint timeTemp ) 
    {
        TimeSpan ts = new TimeSpan(0, 0, (int)timeTemp);
        if (ts.Days < 7 && ts.Days >= 1)
         {
             timeLabel.text = string.Format("[DD0C00FF]{0}天以后删除", ts.Days);
         }
        else if (ts.Days == 0)
        {
            if (ts.Hours >= 1)
            {
                timeLabel.text = string.Format("[DD0C00FF]{0}小时以后删除", ts.Hours);
            }
            if (ts.Hours < 1)
            {
                timeLabel.text = string.Format("[DD0C00FF]{0}小时以内删除", 1);
            }
        }
        else
        {
            timeLabel.text = "";
        }
    }
    public void SetSelect(bool select)
    {
        if (null != Bg)
        {
            Bg.spriteName = (select) ? "anniu_dikuang_anxia" :"anniu_dikuang_zhengchang" ;
        }
    }
    public void SetState(ListMailInfo mail) 
    {
        bool hasItem = (mail.sendMoney.Count +mail.item.Count ) >0;
        if (mail.state == 1 && !hasItem)
        {
            red_dot.gameObject.SetActive(false);
            mail_icon.spriteName = "tubiao_xin_yidu";
            mail_icon.MakePixelPerfect();
        }
        else if(mail.state == 2)
        {
            red_dot.gameObject.SetActive(false);
            mail_icon.spriteName = "tubiao_xin_yidu";
            mail_icon.MakePixelPerfect();
        }
        else 
        {
            red_dot.gameObject.SetActive(true);
            mail_icon.spriteName = "tubiao_xin_weidu";
            mail_icon.MakePixelPerfect();
        }


        mark.gameObject.SetActive(hasItem);

    }
    #endregion
}
   