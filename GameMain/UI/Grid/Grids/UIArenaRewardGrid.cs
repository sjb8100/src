using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using table;


class UIArenaRewardGrid : UIGridBase
{
    UISprite rewardRankSpr;      //奖励排行
    UILabel rewardRankLbl;       //奖励排行
    UILabel rewardLblOne;        //奖励1
    UILabel rewardLblTwo;        //奖励2
    UILabel rewardLblThree;      //奖励3
    UILabel rewardLblFour;       //奖励4

    public uint m_reawrdRankId;

    protected override void OnAwake()
    {
        base.OnAwake();

        rewardRankSpr = this.transform.Find("rewardRank/RankIcon").GetComponent<UISprite>();
        rewardRankLbl = this.transform.Find("rewardRank/RankLbl").GetComponent<UILabel>();          //奖励排行
        rewardLblOne = this.transform.Find("wenqian/wenqian_label").GetComponent<UILabel>();        //奖励1
        rewardLblTwo = this.transform.Find("jingbi/jingbi_label").GetComponent<UILabel>();          //奖励2
        rewardLblThree = this.transform.Find("gongxian/gongxian_label").GetComponent<UILabel>();    //奖励3
        rewardLblFour = this.transform.Find("Exp/Exp_label").GetComponent<UILabel>();               //奖励4
    }


    public override void SetGridData(object data)
    {
        base.SetGridData(data);

        this.m_reawrdRankId = (uint)data;
    }

    public void SetRewardRankSpr(uint rank)
    {
        if (rewardRankSpr != null && rewardRankLbl != null)
        {
            rewardRankSpr.gameObject.SetActive(true);

            string spriteName = string.Empty;
            switch (rank)
            {
                case 1: spriteName = "tubiao_paiming_jin"; break;
                case 2: spriteName = "tubiao_paiming_yin"; break;
                case 3: spriteName = "tubiao_paiming_tong"; break;
                default: spriteName = "tubiao_paiming_tong"; break;
            }

            rewardRankSpr.spriteName = spriteName;

            rewardRankLbl.gameObject.SetActive(false);
        }
    }

    public void SetRewardRankLbl(uint rankCap, uint rankfloor)
    {
        if (rewardRankSpr != null && rewardRankLbl != null)
        {
            rewardRankLbl.gameObject.SetActive(true);
            rewardRankLbl.text = string.Format("{0} - {1}", rankCap, rankfloor);

            rewardRankSpr.gameObject.SetActive(false);
        }
    }


    public void SetRewardOne(uint num)
    {
        if (rewardLblOne != null)
        {
            rewardLblOne.text = num.ToString();
        }
    }

    public void SetRewardTwo(uint num)
    {
        if (rewardLblTwo != null)
        {
            rewardLblTwo.text = num.ToString();
        }

    }

    public void SetRewardThree(uint num)
    {
        if (rewardLblThree != null)
        {
            rewardLblThree.text = num.ToString();
        }
    }

    public void SetRewardFour(uint num)
    {
        if (rewardLblFour != null)
        {
            rewardLblFour.text = num.ToString();
        }
    }
}

