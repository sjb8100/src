using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
partial class advertisePanel 
{
    protected override void OnShow(object data)
    {
        base.OnShow(data);
        if (m_trans_citywar)
        {
            m_trans_citywar.gameObject.SetActive(true);
        }

        if (m_trans_huangling)
        {
            m_trans_huangling.gameObject.SetActive(false);
        }
    }

    protected override void OnLoading()
    {
        base.OnLoading();
    }

    void onClick_Colsebtn_Btn(GameObject caster)
    {
        //HideSelf();
        if (m_trans_citywar)
        {
            m_trans_citywar.gameObject.SetActive(false);
        }

        if (m_trans_huangling)
        {
            m_trans_huangling.gameObject.SetActive(true);
        }

    }

    void onClick_Colsebtn2_Btn(GameObject caster)
    {
        if (m_trans_huangling)
        {
            m_trans_huangling.gameObject.SetActive(false);
        }
        HideSelf();
               
    }

    void onClick_Checkbtn_Btn(GameObject caster)
    {
        HideSelf();
        ItemManager.DoJump(112);
    }
    protected override void OnHide()
    {
        base.OnHide();
    }
}
