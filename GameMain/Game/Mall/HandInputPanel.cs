using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

partial class HandInputPanel : UIPanelBase
{
    public delegate uint OnGetInputMaxDlg();
    #region Define
    /// <summary>
    /// 手动输入框初始化数据
    /// </summary>
    public class HandInputInitData
    {
        public Action<int> onInputValue = null;
        public Vector3 showLocalOffsetPosition = Vector3.zero;
        public uint maxInputNum = 99;
        public OnGetInputMaxDlg onGetInputMaxDlg = null;
        public Action onClose = null;
    }
    #endregion
    #region Property
    private HandInputInitData initData = null;
    //输入框字符串构造器
    private StringBuilder handerInputStringBilder = new StringBuilder();
    private int handInputNum = 0;

    #endregion

    #region Override Method

    protected override void OnLoading()
    {
        base.OnLoading();
       

        foreach (Transform btn in m_grid_KeyBoard.transform)
        {
            UIEventListener.Get(btn.gameObject).onClick = OnKeyBoard;
        }
    }

    protected override void OnPrepareShow(object data)
    {
        initData = (null == data) ? new HandInputInitData() : (data as HandInputInitData);
        SetLocalOffset(initData.showLocalOffsetPosition);
        ResetHandInputStringBuilder();
    }

    protected override void OnShow(object data)
    {
        base.OnShow(data);
        RegisterGolbalEvent(true);
    }

    protected override void OnHide()
    {
        base.OnHide();
        RegisterGolbalEvent(false);
        if (initData != null && initData.onClose != null)
        {
            initData.onClose();
        }
    }

    protected override void OnPanelBaseDestory()
    {
        base.OnPanelBaseDestory();
    }

    public override void OnColliderMaskClicked()
    {
        base.OnColliderMaskClicked();
        this.HideSelf();
    }
    #endregion

    void OnKeyBoard(GameObject go)
    {
        int index = 0;

        if (int.TryParse(go.name, out index))
        {
            if (index == 100)//关闭
            {
                HideSelf();
            }
            else //-1删除
            {
                if (index == -1)
                {
                    if (handerInputStringBilder.Length >= 1)
                    {
                        handerInputStringBilder.Remove(handerInputStringBilder.Length - 1, 1);
                    }

                    if (handerInputStringBilder.Length == 0)
                    {
                        handerInputStringBilder.Append("0");
                    }
                }
                else
                {
//                     if (handerInputStringBilder.Length == 1 && index == 0)
//                     {
//                         handerInputStringBilder.Remove(0,1);                        
//                     }
                    handerInputStringBilder.Append(go.name);
                }

                int num = int.Parse(handerInputStringBilder.ToString()) ;
                if (num > initData.maxInputNum)
                {
                    ResetHandInputStringBuilder();
                    handerInputStringBilder.Append(initData.maxInputNum.ToString());
                    num = (int)initData.maxInputNum;
                }

                if (initData != null && initData.onInputValue != null)
                {
                    initData.onInputValue(num);
                }
            }
        }
    }
    private void RegisterGolbalEvent(bool register)
    {
        if(register)
        {
            Engine.Utility.EventEngine.Instance().AddEventListener((int)Client.GameEventID.UIEVENT_REFRESHINPUTMAXNUM, OnRefreshInputMaxNum);
        }
        else
        {
            Engine.Utility.EventEngine.Instance().RemoveEventListener((int)Client.GameEventID.UIEVENT_REFRESHINPUTMAXNUM, OnRefreshInputMaxNum);
        }
    }

    private void OnRefreshInputMaxNum(int eventType,object data)
    {
        if (null != initData && null != initData.onGetInputMaxDlg)
        {
            initData.maxInputNum = initData.onGetInputMaxDlg.Invoke();
        }
    }

    private void SetLocalOffset(Vector3 offset)
    {
        if (null != m_trans_Content)
            m_trans_Content.localPosition = offset;
    }

    private void ResetHandInputStringBuilder()
    {
        if (handerInputStringBilder.Length > 0)
        {
            handerInputStringBilder.Remove(0, handerInputStringBilder.Length);
        }
    }
}