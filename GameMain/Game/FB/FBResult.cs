//-----------------------------------------
//此文件自动生成，请勿手动修改
//-----------------------------------------
using UnityEngine;
using System.Collections.Generic;

partial class FBResult : UIPanelBase
{

    protected override void OnShow(object data)
    {
        base.OnShow(data);
        Invoke("HideResult", 2);

        //通关成功或者失败
        bool b = true;
        if (data != null)
        {
            b = (bool)data;
        }

        //成功
        if (b)
        {
            m_sprite_win.gameObject.SetActive(true);
            m_sprite_defeat.gameObject.SetActive(false);
        }
        //失败
        else
        {
            m_sprite_win.gameObject.SetActive(false);
            m_sprite_defeat.gameObject.SetActive(true);
        }


    }
    void HideResult()
    {
        HideSelf();
    }

}
