//********************************************************************
//	创建日期:	2016-10-19   17:58
//	文件名称:	UIRankGrid.cs
//  创 建 人:   刘超	
//	版权所有:	中青宝
//	说    明:	排行榜条目Grid
//********************************************************************
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GameCmd;
using UnityEngine;
using table;



class UIRankGrid : UIGridBase
{
    UILabel rank;
    UILabel name;
    UILabel line3;
    UILabel line4;
    UILabel line5;
    UISprite bg;
    UISprite rankBg;
    UISprite select;
 //   public Vector3 pos;
    GameCmd.stAnswerOrderListRelationUserCmd_S.Data info;
    private uint rankIndex;
    public uint RankIndex
    {
        get
        {
            return rankIndex;
        }
    }
    private uint palyerID;
    public uint PlayID 
    {
        get
        {
            return palyerID;
        }
    
    }
    protected override void OnAwake()
    {
        base.OnAwake();

        rank = this.transform.Find("rank").GetComponent<UILabel>();
        name = this.transform.Find("name").GetComponent<UILabel>();
        line3 = this.transform.Find("line3").GetComponent<UILabel>();
        line4 = this.transform.Find("line4").GetComponent<UILabel>();
        line5 = this.transform.Find("line5").GetComponent<UILabel>();
        bg = this.transform.Find("Sprite").GetComponent<UISprite>();
        rankBg = this.transform.Find("rank/Sprite").GetComponent<UISprite>();
        select = this.transform.Find("select").GetComponent<UISprite>(); 
    }
    public override void SetGridData(object data)
    {
        base.SetGridData(data);
        this.info = data as GameCmd.stAnswerOrderListRelationUserCmd_S.Data;
        if (info != null)
        {
            if (rankIndex < 3)
            {
                if (rankIndex ==0)
                {
                  rankBg.gameObject.SetActive(true);
                  rankBg.spriteName = "tubiao_paiming_jin";
                  rankBg.MakePixelPerfect();
                }
                else if (rankIndex == 1)
                {
                    rankBg.gameObject.SetActive(true);
                    rankBg.spriteName = "tubiao_paiming_yin";
                    rankBg.MakePixelPerfect();
                }
                else if (rankIndex == 2)
                {
                    rankBg.gameObject.SetActive(true);
                    rankBg.spriteName = "tubiao_paiming_tong";
                    rankBg.MakePixelPerfect();
                }
              
            }
            else 
            {
                rankBg.gameObject.SetActive(false);
            }
           SetRankData(info);        
        }
        if (bg != null)
        {
            if (rankIndex % 2 == 0)
            {
                bg.spriteName = "biankuang_bantou_baikuai";
            }
            else
            {
                bg.spriteName = "biankuang_bantou_danlan";
            }
        }
      
    }

    public void SetRankIndex(uint index)
    {
        this.rankIndex = index;
    }

    public void SetRankData(GameCmd.stAnswerOrderListRelationUserCmd_S.Data info)
    {
        uint t = (uint)DataManager.Manager<RankManager>().OrderType;
        rank.text = this.info.rank.ToString();
        palyerID = this.info.thisid;
        name.text = this.info.name.ToString();
        if (t >= 11 && t <= 15)
        {
            line3.text = this.info.clan.ToString();
            line4.text = this.info.type.ToString();
            line5.text = this.info.yiju.ToString();
        }
        else
        {
            uint job = 0;
            if (uint.TryParse(info.type,out job))
            {
                SelectRoleDataBase data = table.SelectRoleDataBase.Where((GameCmd.enumProfession)job, (GameCmd.enmCharSex)1);
                if (data != null)
                {
                    line3.text = data.professionName;
                }
            }
            else
            {
                line3.text = info.type;
            }


            line4.text = this.info.clan.ToString();
            line5.text = this.info.yiju.ToString();
            }
    }
    public void  SetSelect(bool value) 
    {
        if (null != select)
        {
            select.gameObject.SetActive(value);
        }
    }
}
