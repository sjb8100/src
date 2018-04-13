using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using table;
using GameCmd;
using Client;
using Engine.Utility;
using UnityEngine;


partial class RedEnvelopeRecordPanel: UIPanelBase
{
    protected override void OnLoading()
    {
        base.OnLoading();

    }
    protected override void OnJump(UIPanelBase.PanelJumpData jumpData)
    {
        base.OnJump(jumpData);
        if (jumpData != null)
        {
            if (jumpData.Param != null)
            {
                stRedPacketLogChatUserCmd_S cmd = jumpData.Param as stRedPacketLogChatUserCmd_S;
                if (cmd != null)
                {
                    m_label_takeNum_label.text = cmd.recv_num.ToString();
                    m_label_takeNumber_label.text = cmd.recv_gold.ToString();
                    m_label_sendNum_label.text = cmd.send_num.ToString();
                    m_label_sendNumber_label.text = cmd.send_coin.ToString();
                }
            }
        }
    }
    void onClick_Btn_Close_Btn(GameObject caster)
    {
        HideSelf();
    }
    void onClick_Btn_confirm_Btn(GameObject caster)
    {
        HideSelf();
    }


}
