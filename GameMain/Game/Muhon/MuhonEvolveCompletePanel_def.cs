//-----------------------------------------
//此文件自动生成，请勿手动修改
//-----------------------------------------
using UnityEngine;


partial class MuhonEvolveCompletePanel: UIPanelBase
{

   FastComponent         fastComponent;

    UIButton             m_btn_Confirm;

    UILabel              m_label_Label;

    UIScrollView         m_scrollview_AttrContent;

    UIWidget             m_widget_GapWidget;

    Transform            m_trans_BaseAttrContent;

    Transform            m_trans_BaseAttr;

    Transform            m_trans_1;

    Transform            m_trans_2;

    Transform            m_trans_3;

    Transform            m_trans_4;

    Transform            m_trans_AddtiveAttrContent;

    Transform            m_trans_AdditiveAttr;

    UILabel              m_label_MuhonName;

    Transform            m_trans_InfoGridRoot;

    UILabel              m_label_MuhonLv;

    UILabel              m_label_AttrUpLimit;

    UILabel              m_label_AttrNumPre;

    UILabel              m_label_AttrNumCur;

    UIButton             m_btn_Close;


    //初始化控件变量
    protected override void InitControls()
    {
       fastComponent = GetComponent<FastComponent>();
       fastComponent.BuildFastComponents();
        m_btn_Confirm = fastComponent.FastGetComponent<UIButton>("Confirm");
       if( null == m_btn_Confirm )
       {
            Engine.Utility.Log.Error("m_btn_Confirm 为空，请检查prefab是否缺乏组件");
       }
        m_label_Label = fastComponent.FastGetComponent<UILabel>("Label");
       if( null == m_label_Label )
       {
            Engine.Utility.Log.Error("m_label_Label 为空，请检查prefab是否缺乏组件");
       }
        m_scrollview_AttrContent = fastComponent.FastGetComponent<UIScrollView>("AttrContent");
       if( null == m_scrollview_AttrContent )
       {
            Engine.Utility.Log.Error("m_scrollview_AttrContent 为空，请检查prefab是否缺乏组件");
       }
        m_widget_GapWidget = fastComponent.FastGetComponent<UIWidget>("GapWidget");
       if( null == m_widget_GapWidget )
       {
            Engine.Utility.Log.Error("m_widget_GapWidget 为空，请检查prefab是否缺乏组件");
       }
        m_trans_BaseAttrContent = fastComponent.FastGetComponent<Transform>("BaseAttrContent");
       if( null == m_trans_BaseAttrContent )
       {
            Engine.Utility.Log.Error("m_trans_BaseAttrContent 为空，请检查prefab是否缺乏组件");
       }
        m_trans_BaseAttr = fastComponent.FastGetComponent<Transform>("BaseAttr");
       if( null == m_trans_BaseAttr )
       {
            Engine.Utility.Log.Error("m_trans_BaseAttr 为空，请检查prefab是否缺乏组件");
       }
        m_trans_1 = fastComponent.FastGetComponent<Transform>("1");
       if( null == m_trans_1 )
       {
            Engine.Utility.Log.Error("m_trans_1 为空，请检查prefab是否缺乏组件");
       }
        m_trans_2 = fastComponent.FastGetComponent<Transform>("2");
       if( null == m_trans_2 )
       {
            Engine.Utility.Log.Error("m_trans_2 为空，请检查prefab是否缺乏组件");
       }
        m_trans_3 = fastComponent.FastGetComponent<Transform>("3");
       if( null == m_trans_3 )
       {
            Engine.Utility.Log.Error("m_trans_3 为空，请检查prefab是否缺乏组件");
       }
        m_trans_4 = fastComponent.FastGetComponent<Transform>("4");
       if( null == m_trans_4 )
       {
            Engine.Utility.Log.Error("m_trans_4 为空，请检查prefab是否缺乏组件");
       }
        m_trans_AddtiveAttrContent = fastComponent.FastGetComponent<Transform>("AddtiveAttrContent");
       if( null == m_trans_AddtiveAttrContent )
       {
            Engine.Utility.Log.Error("m_trans_AddtiveAttrContent 为空，请检查prefab是否缺乏组件");
       }
        m_trans_AdditiveAttr = fastComponent.FastGetComponent<Transform>("AdditiveAttr");
       if( null == m_trans_AdditiveAttr )
       {
            Engine.Utility.Log.Error("m_trans_AdditiveAttr 为空，请检查prefab是否缺乏组件");
       }
        m_label_MuhonName = fastComponent.FastGetComponent<UILabel>("MuhonName");
       if( null == m_label_MuhonName )
       {
            Engine.Utility.Log.Error("m_label_MuhonName 为空，请检查prefab是否缺乏组件");
       }
        m_trans_InfoGridRoot = fastComponent.FastGetComponent<Transform>("InfoGridRoot");
       if( null == m_trans_InfoGridRoot )
       {
            Engine.Utility.Log.Error("m_trans_InfoGridRoot 为空，请检查prefab是否缺乏组件");
       }
        m_label_MuhonLv = fastComponent.FastGetComponent<UILabel>("MuhonLv");
       if( null == m_label_MuhonLv )
       {
            Engine.Utility.Log.Error("m_label_MuhonLv 为空，请检查prefab是否缺乏组件");
       }
        m_label_AttrUpLimit = fastComponent.FastGetComponent<UILabel>("AttrUpLimit");
       if( null == m_label_AttrUpLimit )
       {
            Engine.Utility.Log.Error("m_label_AttrUpLimit 为空，请检查prefab是否缺乏组件");
       }
        m_label_AttrNumPre = fastComponent.FastGetComponent<UILabel>("AttrNumPre");
       if( null == m_label_AttrNumPre )
       {
            Engine.Utility.Log.Error("m_label_AttrNumPre 为空，请检查prefab是否缺乏组件");
       }
        m_label_AttrNumCur = fastComponent.FastGetComponent<UILabel>("AttrNumCur");
       if( null == m_label_AttrNumCur )
       {
            Engine.Utility.Log.Error("m_label_AttrNumCur 为空，请检查prefab是否缺乏组件");
       }
        m_btn_Close = fastComponent.FastGetComponent<UIButton>("Close");
       if( null == m_btn_Close )
       {
            Engine.Utility.Log.Error("m_btn_Close 为空，请检查prefab是否缺乏组件");
       }
       if( null != fastComponent)
       {
            GameObject.Destroy(fastComponent);
       }
    }


    //注册控件事件处理函数
    protected override void RegisterControlEvents()
    {
        UIEventListener.Get(m_btn_Confirm.gameObject).onClick = _onClick_Confirm_Btn;
        UIEventListener.Get(m_btn_Close.gameObject).onClick = _onClick_Close_Btn;
    }

    void _onClick_Confirm_Btn(GameObject caster)
    {
        onClick_Confirm_Btn( caster );
    }

    void _onClick_Close_Btn(GameObject caster)
    {
        onClick_Close_Btn( caster );
    }


}
