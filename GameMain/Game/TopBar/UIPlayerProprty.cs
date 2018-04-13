using System;
using System.Collections.Generic;
using UnityEngine;
//-181
public class UIPlayerProprty : MonoBehaviour
{
//     public enum ProprtyEnum
//     {
//         None = 0,
//         Money ,//文钱
//         Coupon ,//金币
//         Cold ,//元宝
//         AreanScore ,//武斗场积分
//         Reputation ,//氏族声望
//     }
    UISprite m_spriteIcon = null;
    UILabel m_labelNum = null;

    [SerializeField]
    ClientMoneyType m_nType = ClientMoneyType.Invalid;
    void Awake()
    {
        m_spriteIcon = transform.Find("Icon").GetComponent<UISprite>();
        m_labelNum = transform.Find("Num").GetComponent<UILabel>();
        Transform transBtn = transform.Find("Addbtn");
        if (transBtn != null)
        {
            UIEventListener.Get(transBtn.gameObject).onClick = OnAddBtn;
        }
    }

    void OnAddBtn(GameObject go)
    {
        UIFrameManager.Instance.OnPropetyClick(m_nType);
    }

    public void Init(int nType) 
    {
        m_nType = (ClientMoneyType)nType;
        string spriteName = "";
        switch (m_nType)
        {
            case ClientMoneyType.Gold:
                spriteName = "tubiao_tong";
                break;
            case ClientMoneyType.Wenqian:
                spriteName = "tubiao_yin";
                break;
            case ClientMoneyType.YuanBao:
                spriteName = "tubiao_jin";
                break;
            case ClientMoneyType.YinLiang:
                spriteName = "tubiao_yinliang";
                break;
        }
        m_spriteIcon.spriteName = spriteName;
        m_spriteIcon.MakePixelPerfect();
//         UIAtlas atlas = DataManager.Manager<UIManager>().GetAtlasByIconName(spriteName);
//         if (atlas != null)
//         {
//             m_spriteIcon.atlas = atlas;
//            
//         }
       
      
        Refersh();
    }

    public void Refersh()
    {
        int nNum = GetNum(m_nType);
        if (m_labelNum != null)
        {
            m_labelNum.text = TextManager.GetFormatNumText((uint)nNum);
        }
        //DoAnim(nNum);
    }

    public static int GetNum(ClientMoneyType nType)
    {
         Client.IPlayer player = Client.ClientGlobal.Instance().MainPlayer;
        if (player == null)
        {
            return 0;
        }
        int type = 0;
        if (nType == ClientMoneyType.Wenqian)
        {
            type = (int)Client.PlayerProp.Money;
        }
        else if (nType == ClientMoneyType.Gold)
        {
            type = (int)Client.PlayerProp.Coupon;
        }
        else if (nType == ClientMoneyType.YuanBao)
        {
            type = (int)Client.PlayerProp.Cold;
        }
        else if (nType == ClientMoneyType.JiFen)
        {
            type = (int)Client.PlayerProp.Score;
        }
        else if (nType == ClientMoneyType.ShengWang)
        {
            type = (int)Client.PlayerProp.Reputation;
        }
        else if (nType == ClientMoneyType.YinLiang)
        {
            type = (int)Client.PlayerProp.YinLiang;
        }
        else if (nType == ClientMoneyType.FishingMoney)
        {
            type = (int)Client.PlayerProp.FishingMoney;
        }

        else if (nType == ClientMoneyType.HuntingCoin)
        {
            type = (int)Client.PlayerProp.ShouLieScore;
        }
        else if (nType == ClientMoneyType.ChengJiuDian)
        {
            type = (int)Client.PlayerProp.AchievePoint;
        }
        else if (nType == ClientMoneyType.ZhenYingZhanJiFen)
        {
            type = (int)Client.PlayerProp.CampCoin;
        }
        if(type == 0)
        {
            return 0;
        }
        return player.GetProp(type);
    }

    void DoAnim(int nNum)
    {
        if (m_labelNum != null)
        {
            m_labelNum.text = "";
            SlideAnimation slider = m_labelNum.transform.GetComponent<SlideAnimation>();
            if (slider == null)
            {
                slider = m_labelNum.gameObject.AddComponent<SlideAnimation>();
            }

            int gap = (int)Mathf.Abs(nNum - slider.value);
            float speed = (gap > 3000) ? gap / 3f : 1000;
            if (slider != null)
            {
                slider.DoSlideAnim(slider.value, nNum, false, speed, null);
            }
        }
    }
}