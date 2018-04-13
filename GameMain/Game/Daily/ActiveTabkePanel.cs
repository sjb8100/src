using UnityEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using table;
using Engine;
using Client;
using GameCmd;
using System.Text;
using Common;
public enum ActiveTakeType 
{
    Daily =0, //日常
    Bind =1,  //绑定
}
public class ActiveTakeParam 
{
   public ActiveTakeType type = ActiveTakeType.Daily;
   public  uint boxID;
   //0    不可领     1  可领       2   已领
   public uint canGetState;   
   public List<uint> ids;
}
partial class ActiveTakePanel 
{

    protected override void OnLoading()
    {
        base.OnLoading();
        Engine.Utility.EventEngine.Instance().AddEventListener((int)Client.GameEventID.DAILY_GETREWARDBOXOVER, EventCallBack);
        AddCreator(m_trans_RewardRoot);
    }
    void EventCallBack(int nEventID, object param)
    {
        if (nEventID == (int)Client.GameEventID.DAILY_GETREWARDBOXOVER)
        {
            RefreshBoxGetBtn();
        }
    }
    protected override void OnHide()
    {
        base.OnHide();
        DataManager.Manager<DailyManager>().OnUpdateTimeEvent = null; ;
    }
    protected override void OnShow(object data)
    {
        base.OnShow(data);
        if (data != null && data is ActiveTakeParam)
        {

            ActiveTakeParam par = (ActiveTakeParam)data;
            if (par.type == ActiveTakeType.Daily)
            {
                InitPanel((ActiveTakeParam)data);
            }
            else 
            {
                m_label_VerifyBtnLabel.text = ColorManager.GetColorString(ColorType.JZRY_Txt_Black, "获取验证码"); 
                DataManager.Manager<DailyManager>().OnUpdateTimeEvent = OnUpdateTime;
                InitBind();
              
            }
            m_trans_DailyRewardContent.gameObject.SetActive(par.type == ActiveTakeType.Daily);
            m_trans_BindPhoneContent.gameObject.SetActive(par.type == ActiveTakeType.Bind);
        
        }
        
    }

    #region Daily
    uint boxID = 0;
    void InitPanel(ActiveTakeParam data) 
    {
        m_label_Title.text = "活跃领取";
        boxID = data.boxID;
        DailyAwardDataBase awardData = GameTableManager.Instance.GetTableItem<DailyAwardDataBase>(boxID);
        m_label_Des_Label.text = string.Format(DataManager.Manager<TextManager>().GetLocalText(LocalTextType.Local_Txt_Daily_BoxTips), awardData.liveness);
        m_lst_UIItemRewardDatas.Clear();
        if (awardData.Exp >0)
        {
            m_lst_UIItemRewardDatas.Add(new UIItemRewardData()
            {
                itemID = 60006,
                num = awardData.Exp,
            });
        }
        if (awardData.gold >0)
        {
            m_lst_UIItemRewardDatas.Add(new UIItemRewardData()
            {
                itemID = 60001,
                num = awardData.gold,
            });
        }
        if (awardData.ticket>0)
        {
            m_lst_UIItemRewardDatas.Add(new UIItemRewardData()
            {
                itemID = 60002,
                num = awardData.ticket,
            });
        }
        string[] items = awardData.awarditem.Split(';');
    
        for (int i = 0; i < items.Length; i++)
        {
            string[] item = items[i].Split("_".ToCharArray());
            uint itemID;
            uint num =1;
            if (uint.TryParse(item[0], out itemID))
            {
                table.ItemDataBase itemdata = GameTableManager.Instance.GetTableItem<ItemDataBase>(itemID);
                if (itemdata != null)
                {
                    if(uint.TryParse(item[1],out num ))
                    {
                        m_lst_UIItemRewardDatas.Add(new UIItemRewardData()
                        {
                            itemID = itemdata.itemID,
                            num = num,
                        });
                    }
                    else
                    {
                        m_lst_UIItemRewardDatas.Add(new UIItemRewardData()
                        {
                            itemID = itemdata.itemID,
                            num = 1,
                        });
                    }
                 
                }
                else
                {
                    Engine.Utility.Log.Info("宝箱奖励的道具ID配置有误！");
                }
            }
        }

        m_ctor_UIItemRewardCreator.CreateGrids(m_lst_UIItemRewardDatas.Count);

        if (!data.ids.Contains(boxID))
        {
            if (data.canGetState == 1)
            {
                m_btn_btn_Take.isEnabled = true;
                m_btn_btn_Take.transform.GetComponentInChildren<UILabel>().text = "领取";
            }
            else if (data.canGetState == 2)
            {
                m_btn_btn_Take.isEnabled = false;
                m_btn_btn_Take.transform.GetComponentInChildren<UILabel>().text = "已领取";
            }
            else
            {
                m_btn_btn_Take.isEnabled = false;
                m_btn_btn_Take.transform.GetComponentInChildren<UILabel>().text = "领取";
            }
        }
        else 
        {
            m_btn_btn_Take.isEnabled = false;
            m_btn_btn_Take.transform.GetComponentInChildren<UILabel>().text = "已领取";
        }
       
    }
    #region UIItemRewardGridCreator
    UIGridCreatorBase m_ctor_UIItemRewardCreator;
    List<UIItemRewardData> m_lst_UIItemRewardDatas = new List<UIItemRewardData>();
    void AddCreator(Transform parent)
    {
        if (parent != null)
        {
            m_ctor_UIItemRewardCreator = parent.GetComponent<UIGridCreatorBase>();
            if (m_ctor_UIItemRewardCreator == null)
            {
                m_ctor_UIItemRewardCreator = parent.gameObject.AddComponent<UIGridCreatorBase>();
            }
            m_ctor_UIItemRewardCreator.arrageMent = UIGridCreatorBase.Arrangement.Horizontal;
            m_ctor_UIItemRewardCreator.gridWidth = 90;
            m_ctor_UIItemRewardCreator.gridHeight = 90;
            m_ctor_UIItemRewardCreator.RefreshCheck();
            m_ctor_UIItemRewardCreator.Initialize<UIItemRewardGrid>(m_trans_UIItemRewardGrid.gameObject, OnUpdateGridData, null);
        }
    }
    void OnUpdateGridData(UIGridBase grid, int index)
    {
        if (grid is UIItemRewardGrid)
        {
            UIItemRewardGrid itemShow = grid as UIItemRewardGrid;
            if (itemShow != null)
            {
                if (index < m_lst_UIItemRewardDatas.Count)
                {
                    UIItemRewardData data = m_lst_UIItemRewardDatas[index];
                    uint itemID = data.itemID;
                    uint num = data.num;
                    itemShow.SetGridData(itemID, num, false);
                }
            }
        }
    }
    #endregion
//     void CreatObj(uint itemID, uint num, int i)
//     {
//         if (null == m_lst_award)
//         {
//             m_lst_award = new List<UIItemRewardGrid>();
//         }
//         GameObject cloneObj = NGUITools.AddChild(m_trans_RewardRoot.gameObject, m_trans_UIItemRewardGrid.gameObject);
//         UIItemRewardGrid itemShow = cloneObj.transform.GetComponent<UIItemRewardGrid>();
//         if (itemShow == null)
//         {
//             itemShow = cloneObj.AddComponent<UIItemRewardGrid>();
//         }
//         itemShow.MarkAsParentChanged();
//         itemShow.gameObject.SetActive(true);
//         itemShow.SetGridData(itemID, num, false);
//         itemShow.transform.localPosition = new UnityEngine.Vector3(i * 90, 0, 0);
// 
//         m_lst_award.Add(itemShow);
//     }
    void onClick_Btn_unclose_Btn(GameObject caster)
    {
        
    }

    void onClick_Btn_close_Btn(GameObject caster)
    {
        HideSelf();
    }

    void onClick_Btn_Take_Btn(GameObject caster)
    {
        stRequestLivenessRewardDataUserCmd_CS cmd = new stRequestLivenessRewardDataUserCmd_CS();
        cmd.rewardid = boxID;
        NetService.Instance.Send(cmd);

        HideSelf();
    }

    void RefreshBoxGetBtn()
    {
        if (DataManager.Manager<UIPanelManager>().IsShowPanel(PanelID.ActiveTakePanel))
        {
            m_btn_btn_Take.isEnabled = false;
            m_btn_btn_Take.transform.GetComponentInChildren<UILabel>().text = "已领取";
        }
    }
    #endregion

    #region Bind

    void InitBind() 
    {
        m_label_Title.text = "绑定手机号";
    }
    bool m_init = false;
    void OnUpdateTime(float cd) 
    {
        if (cd > 0)
        {
            m_label_VerifyBtnLabel.text = ColorManager.GetColorString(ColorType.JZRY_Txt_White, DateTimeHelper.ParseTimeSecondsFliter(cd)); 
            if (!m_init)
            {
                m_btn_VerifyBtn.isEnabled = false;
                m_btn_VerifyBtn.GetComponent<UISprite>().spriteName = "anniu_hui";
                m_init = true;
            }         
        }
        else 
        {
            if (m_init)
            {
                m_label_VerifyBtnLabel.text = ColorManager.GetColorString(ColorType.JZRY_Txt_Black, "获取验证码");
                m_btn_VerifyBtn.isEnabled = true;
                m_btn_VerifyBtn.GetComponent<UISprite>().spriteName = "anniu_huang_xiao";           
                m_init = false;
             
            }          
        }
    }
    void onClick_VerifyBtn_Btn(GameObject caster)
    {
        UILabel phone_label = m_input_PhoneNumber.GetComponentInChildren<UILabel>();
        if (phone_label != null)
        {
            if (m_input_PhoneNumber.value == null)
            {
                TipsManager.Instance.ShowTips("请输入手机号");
                return;
            }
            else
            {
                if (isPhoneNumber(phone_label.text))
                {
                    stBindPhoneNumDataUserCmd_CS cmd = new stBindPhoneNumDataUserCmd_CS();
                    cmd.phone = phone_label.text;
                    cmd.type = BindPhoneCode.BindPhoneCode_Get;
                    NetService.Instance.Send(cmd);
                }
                else 
                {
                    TipsManager.Instance.ShowTips("无效手机号，请重新输入");
                    m_input_PhoneNumber.value = null;
                }
               
                
            }
        }
        
    }

    void onClick_BindBtn_Btn(GameObject caster)
    {
        UILabel Verify_label = m_input_VerifyNumber.GetComponentInChildren<UILabel>();
        UILabel phone_label =m_input_PhoneNumber.GetComponentInChildren<UILabel>();
        if (Verify_label != null)
        {
            if (m_input_VerifyNumber.value == null)
            {
                TipsManager.Instance.ShowTips("请输入验证码");
               return ;
            }
            else
            {
                if (isVerifyNumber(Verify_label.text))
                {
                    stBindPhoneNumDataUserCmd_CS cmd = new stBindPhoneNumDataUserCmd_CS();
                    cmd.phone = phone_label.text;
                    cmd.type = BindPhoneCode.BindPhoneCode_Ret;
                    cmd.code = Verify_label.text;
                    NetService.Instance.Send(cmd);
                }
                else 
                {
                    TipsManager.Instance.ShowTips("无效验证码，请重新输入");
                    m_input_VerifyNumber.value = null;
                }
               
            }
        }
     
    }
    private bool isVerifyNumber(string text)
    {
        int length = 0;
        if (string.IsNullOrEmpty(text))
        {
            return false;
        }
        foreach (char c in text)
        {
            length++;
        }
        if (length != 6)
        {
            return false;
        }
        return true;
    }
    private bool isPhoneNumber(string text)
    {
        int length = 0;
        if (string.IsNullOrEmpty(text))
        {
            return false;
        }
        foreach (char c in text)
        {
            if (!char.IsDigit(c))
            {
                return false;
            }
            length++;
        }
        if (length != 11)
        {
            return false;
        }    
        return true;
    }
    #endregion


    public override void Release(bool depthRelease = true)
    {
        base.Release(depthRelease);
    }

    protected override void OnPanelBaseDestory()
    {
        base.OnPanelBaseDestory();
        Release();
        Engine.Utility.EventEngine.Instance().RemoveEventListener((int)Client.GameEventID.DAILY_GETREWARDBOXOVER, EventCallBack);
    }
}
