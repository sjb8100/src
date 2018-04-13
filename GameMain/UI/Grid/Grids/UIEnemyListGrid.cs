using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class UIEnemyListGrid : UIGridBase
{
    private UILabel name;
    private UILabel num;
    private UILabel aggro;


    public int index { set; get; }
    protected override void OnAwake()
    {
        base.OnAwake();
        name = transform.Find("Number").GetComponent<UILabel>();
        num = transform.Find("PlayerName").GetComponent<UILabel>();
        aggro = transform.Find("Aggro").GetComponent<UILabel>();
    }

    public override void SetGridData(object data)
    {
        base.SetGridData(data);
        if (data != null)
        {
            index = (int)data;
            num.text = (index + 1).ToString();
        }
       
        

    }
    public void SetDataValue(GameCmd.stEnmityDataUserCmd_S.Enmity data) 
    {
        GameCmd.stEnmityDataUserCmd_S.Enmity en = data;
        if (en != null)
        {
            name.text = en.name;
            aggro.text = en.enmity.ToString();
        }
    }

}