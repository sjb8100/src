using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Client;
partial class PetPanel
{
    void onClick_Guiyuan_Btn(GameObject caster)
    {
     //   DataManager.Manager<UIPanelManager>().ShowPanel( PanelID.Petputongguiyuan );
    }

    void onClick_Gaojiguiyuan_Btn(GameObject caster)
    {
        if ( CurPet == null )
            return;
        int status = CurPet.GetProp( (int)PetProp.PetGuiYuanStatus );
        if(status < 5)
        {
            TipsManager.Instance.ShowTips(LocalTextType.Pet_GuiYuan_haibunengjinxinggaojiguiyuan);
            //TipsManager.Instance.ShowTipsById( 108506 );
            return;
        }
      //  DataManager.Manager<UIPanelManager>().ShowPanel( PanelID.Petgaojieguiyuan);
    }

    void onClick_PetUItips_3_Btn(GameObject caster)
    {

    }

}

