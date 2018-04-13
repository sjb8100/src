
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

class UIBtnTipsGrid : UIGridBase
{

    private UILabel m_labDes = null;
    private UISprite sp = null;

    public uint data;
    public uint Data
    {
        get 
        {
            return data;
        }
        set 
        {
            data = value;
        }
    }
    protected override void OnAwake()
    {
        base.OnAwake();
        m_labDes = CacheTransform.GetComponent<UILabel>();
        //sp = CacheTransform.Find("sprite").GetComponent<UISprite>();
    }

    public override void SetGridData(object data)
    {
        base.SetGridData(data);
        this.data = (uint)data;
    }

    public  void SetName(string value)
    {
        if (m_labDes != null)
        {
            m_labDes.text = value;
        }
       
    }

}