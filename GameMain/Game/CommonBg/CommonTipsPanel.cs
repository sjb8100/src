using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using table;
partial class CommonTipsPanel
{
    protected override void OnLoading()
    {
        base.OnLoading();
    }
    protected override void OnShow(object data)
    {
        base.OnShow(data);
        if(data != null && data is uint)
        {
            uint textID = (uint)data;
            LangTextDataBase tab = GameTableManager.Instance.GetTableItem<LangTextDataBase>(textID);
            if (tab != null)
            {
                m_label_text_label.text = tab.strText;
            }
        }
    }
    protected override void OnHide()
    {
        base.OnHide();
    }
    public override void OnColliderMaskClicked()
    {
        base.OnColliderMaskClicked();
        HideSelf();
    }
}

