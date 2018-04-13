using Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;


partial class GMPanel : UIPanelBase
{
    protected override void OnInitPanelData()
    {
        base.OnInitPanelData();
    }

    void onClick_Close_Btn(GameObject caster)
    {
        DataManager.Manager<UIPanelManager>().HidePanel(PanelID.GMPanel);
    }

    void onClick_JuQing_Btn(GameObject caster)
    {
        SequencerManager.Instance().PlaySequencer("sequencer/SequenceStory-kaichang01.xml");
    }
    
    

    #region BaseContent
    void onClick_Level_queding_Btn(GameObject caster)
    {
        UILabel level_label = m_input_Level_Input.GetComponentInChildren<UILabel>();
        if (level_label != null)
        {
            if (level_label.text == "输入等级" || level_label.text == "")
            {
                Engine.Utility.Log.Error("没输入等级Label!");
            }
            else
            {
                uint label = uint.Parse(level_label.text);
                string msg = string.Format("//setlevel level={0}", label);
                DataManager.Manager<ChatDataManager>().SendChatText(msg);
            }

        }
        else
        {
            Engine.Utility.Log.Error("等级Label为空!");
        }


    }


    void onClick_Exp_queding_Btn(GameObject caster)
    {
        //Exp_queding
        UILabel Exp_label = m_input_Exp_Input.GetComponentInChildren<UILabel>();
        if (Exp_label != null)
        {
            if (Exp_label.text == "输入经验" || Exp_label.text == "")
            {
                Engine.Utility.Log.Error("没输入经验Label!");
            }
            else
            {
                uint label = uint.Parse(Exp_label.text);
                string msg = string.Format("//addexp num={0}", label);
                DataManager.Manager<ChatDataManager>().SendChatText(msg);
            }
        }
        else
        {
            Engine.Utility.Log.Error("经验Label为空!");
        }
    }

    void onClick_Item_queding_Btn(GameObject caster)
    {
        UILabel label = m_input_ItemID_Input.GetComponentInChildren<UILabel>();
        //输入物品数量最少默认1
        UILabel num = m_input_ItemNum_Input.GetComponentInChildren<UILabel>();
        if (label != null)
        {     
           
            if (label.text == "输入物品ID" || label.text == "")
            {
                Engine.Utility.Log.Error("请输入物品ID   !");
            }
            else
            {
                uint text = uint.Parse(label.text);
                if (num.text == "输入数量" || num.text == "")
                {
                    string msg = string.Format("//dolua AddItem({0},{1})", text,1);
                    DataManager.Manager<ChatDataManager>().SendChatText(msg);
                }
                else
                {
                    uint number = uint.Parse(num.text);
                    string msg = string.Format("//dolua AddItem({0},{1})", text, number);
                    DataManager.Manager<ChatDataManager>().SendChatText(msg);
                    Debug.Log(msg);
                }
            }


        }
        else
        {
            Engine.Utility.Log.Error("货币类型为空!");
        }

        
    }
 
    void onClick_Gold_queidng_Btn(GameObject caster)
    {
        int type =(int)ClientMoneyType.Wenqian;
        UILabel label = m_sprite_MoneyType.GetComponentInChildren<UILabel>();
        //输入物品数量最少默认1
        UILabel num = m_input_GoldNum_Input.GetComponentInChildren<UILabel>();
        if (label != null)
        {
            if (label.text == "金币")
            {
                type =   (int)ClientMoneyType.Gold;
            }
            else if (label.text == "元宝")
            {
                type =  (int)ClientMoneyType.YuanBao;
            }
            else if (label.text == "积分")
            {
                type = (int)ClientMoneyType.JiFen;
            }
            else if (label.text == "声望")
            {
                type = (int)ClientMoneyType.ShengWang;
            }
            else if(label.text == "文钱")
            {
                type = (int)ClientMoneyType.Wenqian;
            }
                uint number =1;
                try
                {
                    number = uint.Parse(num.text);
                }
                catch (Exception e)
                {
                    Engine.Utility.Log.Error("异常捕捉{0}", e);
                }
                finally
                {
                    string msg = string.Format("//getmoney type={0} num={1}", type, number);
                    DataManager.Manager<ChatDataManager>().SendChatText(msg);
                    Debug.Log(msg);
                }
            
        }
        else
        {
            Engine.Utility.Log.Error("货币类型为空!");
        }
    }

    void onClick_Skill_queidng_Btn(GameObject caster)
    {

       
    }

    void onClick_Pro_queidng_Btn(GameObject caster)
    {
     
    }

    /// <summary>
    /// 清空背包
    /// </summary>
    /// <param name="caster"></param>
    void onClick_Btn_ClearBag_Btn(GameObject caster)
    {
        string msg = string.Format("//clearitem pack=1");
        DataManager.Manager<ChatDataManager>().SendChatText(msg);
        Debug.Log(msg);
    }
    void onClick_PKNum_queding_Btn(GameObject caster)
    {
        UILabel label = m_input_PKNum_Input.GetComponentInChildren<UILabel>();
        if (label != null)
        {
            if (label.text == "输入PK值" || label.text == "")
            {
                Engine.Utility.Log.Error("没输入PK值Label!");
            }
            else
            {
                if (this.isNumber(label.text))
                {
                    uint l = uint.Parse(label.text);
                    string msg = string.Format("//setpkvalue pkvalue={0}", l);
                    DataManager.Manager<ChatDataManager>().SendChatText(msg);
                    Debug.Log(msg);
                }
                else 
                {
                    Engine.Utility.Log.Error("PK值Label只可以输入数字!");
                }
            }

        }


    }

    void onClick_Sanshiji_Btn(GameObject caster)
    {
        string msg = string.Format("//setlevel level={0}", 30);
        DataManager.Manager<ChatDataManager>().SendChatText(msg);
    }

    void onClick_Yibaiji_Btn(GameObject caster)
    {
        string msg = string.Format("//setlevel level={0}", 100);
        DataManager.Manager<ChatDataManager>().SendChatText(msg);
    }

    void onClick_Dafuweng_Btn(GameObject caster)
    {

        string msg = string.Format("//getmoney type={0} num={1}", (int)ClientMoneyType.Wenqian, 999999);
        DataManager.Manager<ChatDataManager>().SendChatText(msg);
        string msg1 = string.Format("//getmoney type={0} num={1}", (int)ClientMoneyType.Gold, 999999);
        DataManager.Manager<ChatDataManager>().SendChatText(msg1);
        string msg2 = string.Format("//getmoney type={0} num={1}",  (int)ClientMoneyType.YuanBao, 999999);
        DataManager.Manager<ChatDataManager>().SendChatText(msg2);
    }

    #endregion


    #region MissionContent
    void onClick_AcessMission_queidng_Btn(GameObject caster)
    {
        UILabel label = m_input_AcessMission_Input.GetComponentInChildren<UILabel>();
        if (this.isNumber(label.text))
        {
            uint number = uint.Parse(label.text);
            string msg = string.Format("//dolua AddTask({0})", number);
            DataManager.Manager<ChatDataManager>().SendChatText(msg);
            Debug.Log(msg);
        }
        else
        {
            Engine.Utility.Log.Error("接取任务ID数值输入不规范!");
        }
    }

    void onClick_CompleteMission_queidng_Btn(GameObject caster)
    {
        UILabel label = m_input_CompleteMission_Input.GetComponentInChildren<UILabel>();
        if (this.isNumber(label.text))
        {
            uint number = uint.Parse(label.text);
            string msg = string.Format("//dolua FinishTask({0})", number);
            DataManager.Manager<ChatDataManager>().SendChatText(msg);
            Debug.Log(msg);
        }
        else
        {
            Engine.Utility.Log.Error("完成任务ID数值输入不规范!");
        }
    }

    void onClick_MissionAnimation_queidng_Btn(GameObject caster)
    {
        int nID = int.Parse(m_input_MissionAnimation_Input.text);
        if (nID > 0)
        {
            SequencerManager.Instance().PlaySequencer(nID);
        }
    }

    void onClick_Deliver_queidng_Btn(GameObject caster)
    {
        string[] name = caster.name.Split("_".ToCharArray());
        GameObject target = m_widget_MissionContent.transform.Find(name[0]).gameObject;
        UIInput[] input = target.transform.GetComponentsInChildren<UIInput>();
        List<uint> paramings = new List<uint>();
        foreach (var inp in input)
        {
            UILabel l = inp.GetComponentInChildren<UILabel>();
            UILabel parent = inp.GetComponentInParent<UILabel>();
            if (this.isNumber(l.text))
            {
                uint number = uint.Parse(l.text);
                paramings.Add(number);
            }
            else
            {
                Engine.Utility.Log.Error("{0}数值输入不规范!", parent.name);
            }

        }
        Client.IPlayer player = Client.ClientGlobal.Instance().MainPlayer;
        if (player == null)
        {
            return;
        }
        bool moving = (bool)player.SendMessage(Client.EntityMessage.EntityCommand_IsMove, null);
        if (moving)
        {
            player.SendMessage(Client.EntityMessage.EntityCommand_StopMove, player.GetPos());
        }
        string msg = string.Format("//goto map={0} pos={1},{2}", paramings[0], paramings[1], paramings[2]);
        DataManager.Manager<ChatDataManager>().SendChatText(msg);
        Debug.Log(msg);
    }
   
    #endregion


    #region MonsterContent
    void onClick_Monster_queding_Btn(GameObject caster) 
    {
        //string[] name = caster.name.Split("_".ToCharArray());
        //GameObject target = m_widget_MonsterContent.transform.Find(name[0]).gameObject;
        //UIInput[] iput = target.transform.GetComponentsInChildren<UIInput>();
        //List<uint> paramings = new List<uint>();
        UILabel id = m_input_MonID_Input.GetComponentInChildren<UILabel>();
        UILabel num = m_input_MonNum_Input.GetComponentInChildren<UILabel>();
        if ((this.isNumber(id.text)) && (this.isNumber(num.text)))
        {
            string msg = string.Format("//summon id={0} num={1}", id.text, num.text);
            DataManager.Manager<ChatDataManager>().SendChatText(msg);
            Debug.Log(msg);
        }
        else 
        {
            Engine.Utility.Log.Error("怪物ID或数量只可以输入数字!");
        }

    }
    void onClick_KillMonster_queidng_Btn(GameObject caster)
    {
        //killnpc baseid=10007 杀掉九屏内的baseid为10007的怪物
        //killnpc thisid=19999  杀掉thisid为19999的怪物

        IControllerSystem cs = ClientGlobal.Instance().GetControllerSystem();
        if (cs == null)
        {
            return ;
        }

        IController ch = cs.GetActiveCtrl();
        if(ch != null)
        {
           IEntity en = ch.GetCurTarget();
            if(en == null)
            {
                TipsManager.Instance.ShowTips("请选中一个npc");
                return;
            }
            uint id = en.GetID();
            string msg = string.Format("//killnpc thisid={0}", id);
            DataManager.Manager<ChatDataManager>().SendChatText(msg);
        }
    }

    void onClick_KillAllMonster_queidng_Btn(GameObject caster)
    {
        IControllerSystem cs = ClientGlobal.Instance().GetControllerSystem();
        if (cs == null)
        {
            return;
        }

        IController ch = cs.GetActiveCtrl();
        if (ch != null)
        {
            IEntity en = ch.GetCurTarget();
            if (en == null)
            {
                TipsManager.Instance.ShowTips("请选中一个npc");
                return;
            }
            int id = en.GetProp((int)EntityProp.BaseID);
            string msg = string.Format("//killnpc baseid={0}", id);
            DataManager.Manager<ChatDataManager>().SendChatText(msg);
        }
    }
    #endregion


    #region 装备 圣魂 宠物 坐骑 邮件

    void onClick_BtnOneKeyAdd_Btn(GameObject caster) 
    {
        uint equipLevel = 0;
        uint profession = 0;
        UILabel grade = m__Grade.GetComponentInChildren<UILabel>();
        UILabel job = m__Job.GetComponentInChildren<UILabel>();
        if (grade != null && job != null)
        {
            if(uint.TryParse(grade.text,out equipLevel))
            {
               if(job.text == "蛮武")
               {
                   profession = 1;
               }
               else if (job.text == "龙灵")
               {
                   profession = 3;
               }
               else if (job.text == "百草")
               {
                   profession = 2;
               }
               else if (job.text == "巫魅")
               {
                   profession = 4;
               }
               else 
               {
                   Engine.Utility.Log.Error("职业输入有误!");
               }
            }
            if (equipLevel != 0 && profession !=0)
            {
                string msg = string.Format("//dolua AddRandomEquipForOneRole({0},{1})",profession,equipLevel);
                DataManager.Manager<ChatDataManager>().SendChatText(msg);
                Debug.Log(msg);
            }
        }
      
    }
    /// <summary>
    /// 装备  
    /// </summary>
    /// <param name="caster"></param>
    void onClick_Equipment_queidng_Btn(GameObject caster)
    {
        List<uint> paramings = this.GetLeftInput(caster.name);
          int x=(int)paramings[3];
        for (int i = 0; i <x;i++ )
        {
            if (paramings != null)
            {
                string msg = string.Format("//dolua AddEquip({0},{1},{2},{3},{4},{5},{6},{7})", paramings[0], paramings[2], paramings[1],
                    paramings[4], paramings[5], paramings[6], paramings[7], paramings[8]);
                DataManager.Manager<ChatDataManager>().SendChatText(msg);
                Debug.Log(msg);
            }
            else
            {
                Engine.Utility.Log.Error("装备GM页面不能填文字");
            }
        }
        
    }


    /// <summary>
    /// 圣魂
    /// </summary>
    /// <param name="caster"></param>
    void onClick_Muhon_queidng_Btn(GameObject caster)
    {
         List<uint> paramings = this.GetLeftInput(caster.name);
         string msg = string.Format("//dolua AddWeap({0},{1},{2},{3},{4},{5},{6},{7})",
                                    paramings[0], paramings[1], paramings[2], paramings[3], paramings[4], paramings[5], paramings[6], paramings[7]);
         DataManager.Manager<ChatDataManager>().SendChatText(msg);
         Debug.Log(msg);
    }



    /// <summary>
    /// 宠物
    /// </summary>
    /// <param name="caster"></param>
    void onClick_Pet_queidng_Btn(GameObject caster)
    {
        List<uint> paramings = this.GetLeftInput(caster.name);
        string msg = string.Format("//dolua AddPet({0},{1},{2},{3},{4},{5},{6},{7},{8},{9})", 
        paramings[0], paramings[1], paramings[2], paramings[3], paramings[4], paramings[5], paramings[6], paramings[7], paramings[8], paramings[9]);
        DataManager.Manager<ChatDataManager>().SendChatText(msg);
        Debug.Log(msg);
    }




    /// <summary>
    /// 坐骑
    /// </summary>
    /// <param name="caster"></param>
    void onClick_Ride_queidng_Btn(GameObject caster)
    {
        List<uint> paramings = this.GetLeftInput(caster.name);
        string msg = string.Format("//dolua AddRide({0},{1},{2})", paramings[0], paramings[1], paramings[2]);
        DataManager.Manager<ChatDataManager>().SendChatText(msg);
        Debug.Log(msg);
    }


    void onClick_Mail_queding_Btn(GameObject caster)
    {
        GameObject target = m_trans_left.transform.Find("MailContent").gameObject;
        UIInput[] input = target.transform.GetComponentsInChildren<UIInput>();
        List<string> text_para = new List<string>();
        List<uint>  uint_para =new List<uint>();
        uint mail_num = 1;
        foreach (var inp in input)
        {
            UILabel l = inp.GetComponentInChildren<UILabel>();
            UILabel parent = inp.GetComponentInParent<UILabel>();
            if (this.isNumber(l.text))
            {
                if (inp.name == "MailName_Input" || inp.name == "MailText_Input")
                {
                    Engine.Utility.Log.Error("{0}框还是填文字比较好", inp.name);
                }
                else
                {
                    if (inp.name == "MailNum_Input")
                    {

                        UILabel num_label = m_input_MailNum_Input.GetComponentInChildren<UILabel>();
                        
                        if (this.isNumber(num_label.text))
                        {
                            mail_num = uint.Parse(num_label.text);
                        }
                        else
                        {
                            Engine.Utility.Log.Error("邮件数目输入值不是数字类型!");
                        }
                    }
                    else
                    {
                        uint number = uint.Parse(l.text);
                        uint_para.Add(number);
                    }
                }
            }
            else
            {
                text_para.Add(l.text);
                
            }           
        }
        if (text_para.Count > 2)
        {
            Engine.Utility.Log.Error("物品货币框内只允许数字类型，指令不能发送!");
        }
        else
        {
           //左侧0  右侧也是0   右侧0  左侧也是0
            for (int m = 0; m < uint_para.Count;m++ )
            {
                if (m % 2 == 0)
                {
                    if (uint_para[m] == 0)
                    {
                        uint_para[m + 1] = 0;
                    }
                }
                else 
                {
                    if (uint_para[m] == 0)
                    {
                        uint_para[m -1] = 0;
                    }
                }
            
            }

            for (int x = 0; x < mail_num;x++ )
            {
                string msg = string.Format("//sendmail text={0} title={1} itemid1={2} dwnum1={3} itemid2={4} dwnum2={5} itemid3={6} dwnum3={7} type1={8} num1={9} type2={10} num2={11} ",
                text_para[1].ToString(), text_para[0].ToString(), uint_para[0], uint_para[1], uint_para[2], uint_para[3], uint_para[4], uint_para[5], uint_para[6], uint_para[7], uint_para[8], uint_para[9]);
                DataManager.Manager<ChatDataManager>().SendChatText(msg);
                Debug.Log(msg);
            }
        }
       
    }

    #endregion


    #region   获取Label值的方法和判断是否为数字
    private List<uint> GetLeftInput(string args)
    {
        string[] name = args.Split("_".ToCharArray()); 
        GameObject target = m_trans_left.transform.Find(name[0]+"Content").gameObject;
        UIInput[] input = target.transform.GetComponentsInChildren<UIInput>();
        List<uint> paramings = new List<uint>();
        foreach (var inp in input)
        {
            UILabel l = inp.GetComponentInChildren<UILabel>();
            UILabel parent = inp.GetComponentInParent<UILabel>();
            if (this.isNumber(l.text))
            {
                uint number = uint.Parse(l.text);
                paramings.Add(number);
            }
            else
            {
                Engine.Utility.Log.Error("{0}数值输入不规范!", parent.name);
            }

        }
        return paramings;
    }
    private bool isNumber(string text)
    {
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
        }
        return true;
    }
    #endregion


    //右边选项栏   点击一个显示一个内容
    #region   隐藏和显示界面 
 
    void onClick_Base_Btn(GameObject caster)
    {
        ModifyContent(caster.name + "Content");
    }

    void onClick_Mission_Btn(GameObject caster)
    {
        ModifyContent(caster.name + "Content");
    }

    void onClick_Monster_Btn(GameObject caster)
    {
        ModifyContent(caster.name + "Content");
    }

    void onClick_Equipment_Btn(GameObject caster)
    {
        ModifyContent(caster.name + "Content");
        
    }
    void onClick_Muhon_Btn(GameObject caster)
    {
        ModifyContent(caster.name + "Content");
    }

    void onClick_Pet_Btn(GameObject caster)
    {
        ModifyContent(caster.name + "Content");
    }

    void onClick_Ride_Btn(GameObject caster)
    {
        ModifyContent(caster.name + "Content");
    }

    void onClick_Mail_Btn(GameObject caster)
    {
        ModifyContent(caster.name + "Content");
    }


    void ModifyContent(string targetName)
    {
        for (int i = 0; i < m_trans_left.transform.childCount; i++)
        {
            GameObject left_content = m_trans_left.transform.GetChild(i).gameObject;
            left_content.SetActive(false);
        }
        //换名字
        GameObject target_content = m_trans_left.transform.Find(targetName).gameObject;
        target_content.SetActive(true);
    }

   
  
    #endregion 

    void onClick_LowMemory_Btn(GameObject caster)
    {
        GameApp.Instance().OnLowMemoryWarning();
    }
}