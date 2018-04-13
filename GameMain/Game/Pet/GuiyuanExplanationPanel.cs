//-----------------------------------------
//此文件自动生成，请勿手动修改
//-----------------------------------------
using UnityEngine;


partial class GuiyuanExplanationPanel: UIPanelBase
{

    protected override void OnLoading()
    {
        base.OnLoading();
        UIEventListener.Get(m_widget_close.gameObject).onClick = OnClose;
    }
    void OnClose(GameObject go)
    {
        HideSelf();
    }

}
