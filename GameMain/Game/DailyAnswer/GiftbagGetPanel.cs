using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;


public class GiftbagParam
{
    public string firstTitle;   //一级标题

    public string secondTitle;  //二级标题

    public uint canGetState;   //0    不可领     1  可领       2   已领

    public Dictionary<uint, uint> giftbagDic;  // key :basrItemId, value:数量

    public Action getGiftbagEvent;
}

partial class GiftbagGetPanel
{
    GiftbagParam m_data;

    #region override

    protected override void OnInitPanelData()
    {
        base.OnInitPanelData();
    }

    protected override void OnLoading()
    {
        base.OnLoading();
    }

    protected override void OnPrepareShow(object data)
    {
        base.OnPrepareShow(data);
    }

    protected override void OnShow(object data)
    {
        base.OnShow(data);

        if (data == null)
        {
            return;
        }

        GiftbagParam param = data as GiftbagParam;
        if (param != null)
        {
            m_data = new GiftbagParam();

            this.m_data.firstTitle = param.firstTitle;

            this.m_data.secondTitle = param.secondTitle;

            this.m_data.canGetState = param.canGetState;

            this.m_data.giftbagDic = new Dictionary<uint, uint>();

            Dictionary<uint, uint>.Enumerator etr = param.giftbagDic.GetEnumerator();
            while (etr.MoveNext() == true)
            {
                this.m_data.giftbagDic.Add(etr.Current.Key, etr.Current.Value);
            }

            this.m_data.getGiftbagEvent = param.getGiftbagEvent;

            InitUI();
        }

    }

    public override bool OnMsg(UIMsgID msgid, object param)
    {

        return true;
    }

    protected override void OnHide()
    {
        Release();
    }

    protected override void OnPanelBaseDestory()
    {
        base.OnPanelBaseDestory();


    }


    public override void Release(bool depthRelease = true)
    {
        base.Release(depthRelease);

    }

    #endregion

    #region method

    void InitUI()
    {
        m_label_Title_Label.text = this.m_data.firstTitle;

        m_label_BiaoTi_Label.text = this.m_data.secondTitle;

        InitGrid();

        InitBtn();
    }

    void InitGrid()
    {
        m_grid_rewardroot.transform.DestroyChildren();

        Dictionary<uint, uint>.Enumerator etr = this.m_data.giftbagDic.GetEnumerator();
        while (etr.MoveNext() == true)
        {
            uint baseItemId = etr.Current.Key;
            uint itemNum = etr.Current.Value;          
            if (null == m_trans_UIItemRewardGrid)
            {
                return;
            }
            GameObject cloneObj = NGUITools.AddChild(m_grid_rewardroot.gameObject, m_trans_UIItemRewardGrid.gameObject);
            UIItemRewardGrid itemShow = cloneObj.transform.GetComponent<UIItemRewardGrid>();
            if (itemShow == null)
            {
                itemShow = cloneObj.AddComponent<UIItemRewardGrid>();
            }
            itemShow.MarkAsParentChanged();
            itemShow.gameObject.SetActive(true);
            itemShow.SetGridData(baseItemId, itemNum, true);
        }

        m_grid_rewardroot.Reposition();
    }

    void InitBtn()
    {
        if (this.m_data.canGetState == 0)
        {
            m_sprite_btn_notake.gameObject.SetActive(true);
            m_btn_btn_take.gameObject.SetActive(false);
            m_sprite_btn_alreadyTake.gameObject.SetActive(false);
        }
        else if (this.m_data.canGetState == 1)
        {
            m_sprite_btn_notake.gameObject.SetActive(false);
            m_btn_btn_take.gameObject.SetActive(true);
            m_sprite_btn_alreadyTake.gameObject.SetActive(false);
        }
        else if (this.m_data.canGetState == 2)
        {
            m_sprite_btn_notake.gameObject.SetActive(false);
            m_btn_btn_take.gameObject.SetActive(false);
            m_sprite_btn_alreadyTake.gameObject.SetActive(true);
        }
    }

    #endregion

    #region click

    void onClick_Btn_take_Btn(GameObject caster)
    {
        if (this.m_data != null)
        {
            if (this.m_data.getGiftbagEvent != null)
            {
                this.m_data.getGiftbagEvent.Invoke();

                m_btn_btn_take.gameObject.SetActive(false);
                m_sprite_btn_alreadyTake.gameObject.SetActive(true);
            }
        }
        HideSelf();
    }


    void onClick_Btn_Close_Btn(GameObject caster)
    {
        HideSelf();
    }
    #endregion
}

