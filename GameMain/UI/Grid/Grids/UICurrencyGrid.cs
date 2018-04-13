using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

class UICurrencyGrid : UIGridBase
{
    public class UICurrencyGridData
    {
#region Property
        private uint num;
        public uint Num
        {
            get
            {
                return num;
            }
        }
        private ColorType colorType = ColorType.JZRY_Txt_White;
        public ColorType ClorType
        {
            get
            {
                return colorType;
            }
        }

        private string icon;
        public string Icon
        {
            get
            {
                return icon;
            }
        }
#endregion
        public UICurrencyGridData(string icon, uint num, ColorType cType = ColorType.JZRY_Txt_White)
        {
            this.icon = icon;
            this.num = num;
            this.colorType = cType;
        }
    }
    #region Property
    private UILabel num;
    private UISprite icon;
    #endregion

    #region overridemethod
    protected override void OnAwake()
    {
        base.OnAwake();
        num = CacheTransform.Find("Content/Num").GetComponent<UILabel>();
        icon = CacheTransform.Find("Content/Icon").GetComponent<UISprite>();
    }

    public override void SetGridData(object data)
    {
        base.SetGridData(data);
        if (null == data || !(data is UICurrencyGridData))
        {
            return;
        }
        UICurrencyGridData cData = data as UICurrencyGridData;
        if (null != cData)
        {
            if (null != num)
                num.text = ColorManager.GetColorString(cData.ClorType, cData.Num + "");
            if (cData.Icon != null)
            {
              UpdateCurrencyType(cData.Icon);
            }
           
        }
        
    }

    public override void Release(bool depthRelease = true)
    {
        base.Release(depthRelease);
        if (null != m_currencyAsynSeed)
        {
            m_currencyAsynSeed.Release(true);
            m_currencyAsynSeed = null;
        }
    }

    #endregion

    /// <summary>
    /// 更新数量
    /// </summary>
    /// <param name="num"></param>
    public void UpdateNum(int num)
    {
        if (null != this.num)
            this.num.text = "" + num;
    }
    CMResAsynSeedData<CMAtlas> m_currencyAsynSeed = null;
    /// <summary>
    /// 更新图标
    /// </summary>
    /// <param name="cType"></param>
    public void UpdateCurrencyType(string icon)
    {
        if (null != this.icon)
        {
            UIManager.GetAtlasAsyn(icon, ref m_currencyAsynSeed, () =>
            {
                if (null != this.icon)
                {
                    this.icon.atlas = null;
                }
            }, this.icon);
        }
    }
}

