
using Engine.Utility;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using table;
using UnityEngine;


partial class NvWaStartPanel
{
    public delegate void OKDelegate();
    public delegate void CancelDelegate(bool useItem, bool useYuanbao);

    private CMResAsynSeedData<CMTexture> iuiIconAtlas = null;

    public class NvWaStartParam
    {
        public uint itemId;
        public float OkCd;
        public NvWaStartPanel.OKDelegate OkDelegate;
        public NvWaStartPanel.CancelDelegate CancelDelegate;
    }

    uint itemId;

    float OkCdTime;

    float totleTime;

    bool useItem;

    bool useYuanBaoAutoBuy;

    OKDelegate OkDel;

    CancelDelegate CancelDel;

    //private const int NVWA_TIMERID = 2000;

    #region override

    //在窗口第一次加载时，调用
    protected override void OnLoading()
    {

    }

    protected override void OnPrepareShow(object data)
    {
        base.OnPrepareShow(data);
    }

    /// <summary>
    /// 界面显示回调
    /// </summary>
    /// <param name="data"></param>
    protected override void OnShow(object data)
    {
        base.OnShow(data);

        NvWaStartParam nvWaStart = data as NvWaStartParam;
        if (nvWaStart != null)
        {
            Init(nvWaStart);
        }

        //默认不开启元宝补足
        this.m_btn_yinhun_xiaohaoSprite.GetComponent<UIToggle>().value = false;
        m_trans_UseCost.gameObject.SetActive(false);
    }

    protected override void OnHide()
    {
        base.OnHide();

        ReleaseAtlas();
    }

    #endregion

    #region mono

    float temp = 0f;
    void Update()
    {
        float leftTime = DataManager.Manager<NvWaManager>().NvWaStartTime - Time.realtimeSinceStartup;
        if (leftTime >= 0)
        {
            if (m_sprite_itemProgressbar != null)
            {
                m_sprite_itemProgressbar.fillAmount = leftTime / 10;
            }

            temp += Time.deltaTime;
            if (temp > 0.95f)
            {
                m_label_rightBtn_Label.text = string.Format("开始({0})", (int)leftTime);
                temp = 0;
            }
        }
        else
        {
            if (DataManager.Manager<UIPanelManager>().IsShowPanel(PanelID.NvWaStartPanel))
            {
                onClick_Btn_right_Btn(null);
            }
        }
    }

    #endregion


    #region method

    void Init(NvWaStartParam nvWaStart)
    {
        InitItem(nvWaStart.itemId);

        this.OkCdTime = nvWaStart.OkCd;
        this.totleTime = nvWaStart.OkCd;

        this.OkDel = nvWaStart.OkDelegate;
        this.CancelDel = nvWaStart.CancelDelegate;
    }

    void InitItem(uint itemId)
    {
        this.itemId = itemId;

        int itemCount = DataManager.Manager<ItemManager>().GetItemNumByBaseId(this.itemId);//道具存量
        BaseItem baseItem = DataManager.Manager<ItemManager>().GetTempBaseItemByBaseID<BaseItem>(this.itemId);

        //icon
        UIManager.GetTextureAsyn(baseItem.Icon, ref iuiIconAtlas, () =>
        {
            if (m__itemIcon != null)
            {
                m__itemIcon.mainTexture = null;
            }
        }, m__itemIcon, false);

        //name
        m_label_itemName.text = baseItem.Name;

        //num
        m_label_itemNum.text = string.Format("1/{0}", itemCount);
        if (IsItemEnough())
        {
            m_label_itemNum.color = Color.white;
        }
        else
        {
            m_label_itemNum.color = Color.red;
        }
    }

    bool IsItemEnough()
    {
        int itemCount = DataManager.Manager<ItemManager>().GetItemNumByBaseId(this.itemId);//道具存量

        return itemCount > 0 ? true : false;
    }

    bool IsYuanBaoEnough()
    {
        //使用跳关符消耗
        uint jumpItem = DataManager.Manager<NvWaManager>().NvWaJumpItemId;

        //元宝存量
        int dianjuanCount = MainPlayerHelper.GetMoneyNumByType(ClientMoneyType.YuanBao);

        PointConsumeDataBase pcDb = GameTableManager.Instance.GetTableItem<PointConsumeDataBase>(jumpItem);
        uint dianJuanCost = 0;
        if (pcDb != null)
        {
            //每个道具要几个元宝来换
            dianJuanCost = pcDb.buyPrice;
        }

        return dianjuanCount >= dianJuanCost ? true : false;
    }

    #endregion

    #region click

    void onClick_Btn_right_Btn(GameObject caster)
    {
        if (this.OkDel != null)
        {
            this.OkDel.Invoke();
        }

        HideSelf();
    }

    void onClick_Btn_left_Btn(GameObject caster)
    {
        this.useYuanBaoAutoBuy = this.m_btn_yinhun_xiaohaoSprite.GetComponent<UIToggle>().value;

        bool canJump = false;
        if (this.CancelDel != null)
        {
            //1、如果道具充足，默认优先使用道具
            if (IsItemEnough())
            {
                this.useYuanBaoAutoBuy = false;
                canJump = true;
            }
            //2、道具不足，开启自动使用元宝
            else if (this.useYuanBaoAutoBuy)
            {
                if (IsYuanBaoEnough())
                {
                    canJump = true;
                }
                else
                {
                    TipsManager.Instance.ShowTips("元宝不足");
                }
            }
            //3、元宝 道具都不足
            else
            {
                TipsManager.Instance.ShowTips("道具不足");
            }
        }


        if (canJump)
        {
            this.CancelDel(true, this.useYuanBaoAutoBuy);

            HideSelf();
        }
    }

    void onClick_Yinhun_xiaohaoSprite_Btn(GameObject caster)
    {
        bool b = this.m_btn_yinhun_xiaohaoSprite.GetComponent<UIToggle>().value;
        if (b)
        {
            if (false == IsItemEnough())
            {
                m_trans_UseCost.gameObject.SetActive(true);
                SetDianjuanLabel();
                m_label_itemNum.gameObject.SetActive(false);
            }
        }
        else
        {
            m_trans_UseCost.gameObject.SetActive(false);
            m_label_itemNum.gameObject.SetActive(true);
        }
    }


    /// <summary>
    /// 元宝消耗
    /// </summary>
    void SetDianjuanLabel()
    {
        //使用跳关符消耗
        uint jumpItem = DataManager.Manager<NvWaManager>().NvWaJumpItemId;
        int dianjuanCount = MainPlayerHelper.GetMoneyNumByType(ClientMoneyType.YuanBao);   //元宝存量
        PointConsumeDataBase pcDb = GameTableManager.Instance.GetTableItem<PointConsumeDataBase>(jumpItem);
        uint dianJuanCost = 0;
        if (pcDb != null)
        {
            dianJuanCost = pcDb.buyPrice; //每个道具要几个元宝来换
        }

        m_label_UseCostNum.text = dianJuanCost.ToString();

        if (IsYuanBaoEnough())
        {
            m_label_UseCostNum.color = Color.white;
        }
        else
        {
            m_label_UseCostNum.color = Color.red;
        }
    }

    #endregion

    void ReleaseAtlas()
    {
        if (null != iuiIconAtlas)
        {
            iuiIconAtlas.Release(true);
            iuiIconAtlas = null;
        }
    }

}