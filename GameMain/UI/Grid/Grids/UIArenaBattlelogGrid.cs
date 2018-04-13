using GameCmd;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;


class UIArenaBattlelogGrid : UIGridBase
{
    UILabel name;
    UILabel job;
    UILabel result;
    UILabel myRank;
    GameObject upArrow;//箭头
    GameObject downArrow;//箭头
    UILabel updown;

    public ArenaBattleLog battlelogInfo;
    //public Vector3 pos;

    protected override void OnAwake()
    {
        base.OnAwake();
        name = this.transform.Find("target_name").GetComponent<UILabel>();
        job = this.transform.Find("target_job").GetComponent<UILabel>();
        result = this.transform.Find("fight_result").GetComponent<UILabel>();
        myRank = this.transform.Find("MyRank").GetComponent<UILabel>();
        upArrow = this.transform.Find("Rank_Change/up").gameObject;
        downArrow = this.transform.Find("Rank_Change/down").gameObject;
        updown = this.transform.Find("Rank_Change/Label").GetComponent<UILabel>();

    }

    public override void SetGridData(object data)
    {
        base.SetGridData(data);
        this.battlelogInfo = data as ArenaBattleLog;

        if (this.battlelogInfo != null)
        {
            if (this.battlelogInfo.result.Equals(1))
            {
                result.text = "胜利";
                result.color = Color.green;
                upArrow.gameObject.SetActive(true);
                downArrow.gameObject.SetActive(false);
                //UIAtlas atlas = DataManager.Manager<UIManager>().GetAtlasByIconName("tubiao_jiantou_shangsheng");
                //if (atlas != null)
                //{
                //    arrow.atlas = atlas;
                //}
                //arrow.spriteName = "tubiao_jiantou_shangSheng";
                //arrow.MakePixelPerfect();
            }
            else
            {
                result.text = "失败";
                result.color = Color.red;
                upArrow.gameObject.SetActive(false);
                downArrow.gameObject.SetActive(true);
                //UIAtlas atlas = DataManager.Manager<UIManager>().GetAtlasByIconName("tubiao_jiantou_xiajiang");
                //if (atlas != null)
                //{
                //    arrow.atlas = atlas;
                //}
                //arrow.spriteName = "tubiao_jiantou_xiajiang";
                //arrow.MakePixelPerfect();
            }
            myRank.text = this.battlelogInfo.rank.ToString();
            updown.text = this.battlelogInfo.change.ToString();
        }
    }

    /// <summary>
    /// 姓名
    /// </summary>
    /// <param name="name"></param>
    public void SetName(string name)
    {
        this.name.text = name;
    }

    /// <summary>
    /// 职业
    /// </summary>
    public void SetJob(uint job ,uint sex)
    {
        this.job.text = GetJobName(job ,sex);
    }

    string GetJobName(uint job, uint sex)
    {
        string jobName = "";

        table.SelectRoleDataBase sdb = table.SelectRoleDataBase.Where((GameCmd.enumProfession)job, (GameCmd.enmCharSex)sex);

        if (sdb != null)
        {
            jobName = sdb.professionName;
        }

        //switch (job)
        //{
        //    case 1: jobName = "战士"; break;
        //    case 2: jobName = "幻师"; break;
        //    case 3: jobName = "法师"; break;
        //    case 4: jobName = "暗巫"; break;
        //}

        return jobName;
    }

}

