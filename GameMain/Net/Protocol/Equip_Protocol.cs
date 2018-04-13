using System;
using System.ComponentModel;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GameCmd;
using Common;

partial class Protocol
{
    #region Equip
    /// <summary>
    /// 精炼请求
    /// </summary>
    /// <param name="qwThisId"></param>
    /// <param name="assistItemId"></param>
    /// <param name="useTickets"></param>
    public void EquipRefineReq(uint qwThisId, uint assistItemId, bool useTickets)
    {
        stEquipRefineItemWorkUserCmd_CS cmd = new stEquipRefineItemWorkUserCmd_CS()
        {
            assist_auto = useTickets,
            thisid = qwThisId,
            assist_id = assistItemId,

        };
        SendCmd(cmd);
    }

    /// <summary>
    /// 精炼完成响应
    /// </summary>
    /// <param name="msg"></param>
    [Execute]
    public void OnEquipRefineRes(stEquipRefineItemWorkUserCmd_CS msg)
    {
        DataManager.Manager<EquipManager>().OnEquipRefineRes(msg.ret);
    }

    /// <summary>
    /// 合成请求
    /// </summary>
    public void EquipCompoundReq(uint qwThisId,List<uint> deputyIds,bool zf,List<uint> protectAttrID)
    {
        stEquipComposeItemWorkUserCmd_CS cmd = new stEquipComposeItemWorkUserCmd_CS();
        cmd.thisid = qwThisId;
        cmd.deputy_id.AddRange(deputyIds);
        cmd.use_protect = zf;
        if (zf && null != protectAttrID && protectAttrID.Count > 0)
            cmd.prop_id.AddRange(protectAttrID);
        SendCmd(cmd);
    }

    /// <summary>
    /// 合成响应
    /// </summary>
    /// <param name="msg"></param>
    [Execute]
    public void OnEquipCompoundRes(stEquipComposeItemWorkUserCmd_CS msg)
    {
        DataManager.Manager<EquipManager>().OnEquipSelectCompoud(msg);
    }

    /// <summary>
    /// 打开合成结果
    /// </summary>
    /// <param name="index"></param>
    public void OpenEquipCompoundCardReq(uint index)
    {
        stComposeOpenCardItemWorkUserCmd_CS cmd = new stComposeOpenCardItemWorkUserCmd_CS()
        {
            index = index,
        };
        SendCmd(cmd);
    }

    /// <summary>
    /// 打开合成结果
    /// </summary>
    /// <param name="msg"></param>
    [Execute]
    public void OnOpenEquipCompoundCardRes(stComposeOpenCardItemWorkUserCmd_CS msg)
    {
        DataManager.Manager<EquipManager>().OnOpenEquipCompoundResult(msg.index);
    }

    /// <summary>
    /// 装备合成选择属性
    /// </summary>
    /// <param name="index"></param>
    /// <param name="qwThisId"></param>
    public void EquipCompoundSelectReq(uint index ,uint qwThisId)
    {
        stEquipComposeSelectItemWorkUserCmd_CS cmd = new stEquipComposeSelectItemWorkUserCmd_CS()
        {
            sel_index = index,
            this_id = qwThisId,
        };
        SendCmd(cmd);
    }

    /// <summary>
    /// 装备合成选择属性响应
    /// </summary>
    /// <param name="index"></param>
    /// <param name="qwThisId"></param>
    [Execute]
    public void OnEquipCompoundSelectRes(stEquipComposeSelectItemWorkUserCmd_CS msg)
    {
        DataManager.Manager<EquipManager>().OnEquipCompoud(msg.this_id);
    }

    /// <summary>
    /// 装备请求
    /// </summary>
    /// <param name="qwThisId"></param>
    public void EquipRepairReq(List<uint> qwThisId)
    {
        stEquipRepairItemWorkUserCmd_CS cmd = new stEquipRepairItemWorkUserCmd_CS();
        cmd.thisid.AddRange(qwThisId);
        SendCmd(cmd);
    }

    /// <summary>
    /// 装备修理响应
    /// </summary>
    /// <param name="msg"></param>
    [Execute]
    public void OnEquipRepairRes(stEquipRepairItemWorkUserCmd_CS msg)
    {
        TipsManager.Instance.ShowTips(string.Format("成功修理{0}件装备", msg.thisid.Count));
    }

    /// <summary>
    /// 装备分解
    /// </summary>
    /// <param name="qwThisId"></param>
    public void EquipDecomposeReq(List<uint> qwThisId)
    {
        stEquipDecomposeItemWorkUserCmd_CS cmd = new stEquipDecomposeItemWorkUserCmd_CS();
        cmd.thisid.AddRange(qwThisId);
        SendCmd(cmd);
    }

    /// <summary>
    /// 装备分解响应
    /// </summary>
    /// <param name="msg"></param>
    [Execute]
    public void OnEquipDecomposeRes(stEquipDecomposeItemWorkUserCmd_CS msg)
    {
        DataManager.Manager<EquipManager>().OnSplitEquip(msg.thisid);
    }
    #endregion

    #region Property
    /// <summary>
    /// 消除属性
    /// </summary>
    /// <param name="qwThisId"></param>
    /// <param name="assitItembaseId"></param>
    /// <param name="propIndex"></param>
    public void EquipPropRemoveReq(uint qwThisId,uint assitItembaseId,uint propIndex)
    {
        stEquipPropRemoveItemWorkUserCmd_CS cmd = new stEquipPropRemoveItemWorkUserCmd_CS()
        {
            thisid = qwThisId,
            assist_id = assitItembaseId,
            prop_index = propIndex,
        };
        SendCmd(cmd);
    }

    /// <summary>
    /// 消除属性响应
    /// </summary>
    /// <param name="msg"></param>
    [Execute]
    public void OnEquipPropRemoveRes(stEquipPropRemoveItemWorkUserCmd_CS msg)
    {
        DataManager.Manager<EquipManager>().OnEquipPropertyRemove(msg);
    }

    /// <summary>
    /// 装备属性提升请求
    /// </summary>
    /// <param name="qwThisId"></param>
    /// <param name="assitBaseId"></param>
    /// <param name="propIndex"></param>
    public void EquipPropPromoteReq(uint qwThisId,uint assitBaseId,uint propIndex)
    {
        stEquipPropPromoteItemWorkUserCmd_CS cmd = new stEquipPropPromoteItemWorkUserCmd_CS()
        {
            thisid = qwThisId,
            assist_id = assitBaseId,
            prop_index = propIndex,
        };
        SendCmd(cmd);
    }

    /// <summary>
    /// 属性清除
    /// </summary>
    /// <param name="msg"></param>
    [Execute]
    public void OnEquipPropPromoteRes(stEquipPropPromoteItemWorkUserCmd_CS msg)
    {
        DataManager.Manager<EquipManager>().OnEquipPropertyPromote(msg);
    }
    #endregion

    #region Gem

    /// <summary>
    /// 服务器下发装备格宝石镶嵌数据
    /// </summary>
    /// <param name="msg"></param>
    [Execute]
    public void OnAddGemInlayListData(stAddGemInlayListPropertyUserCmd_S msg)
    {
        DataManager.Manager<EquipManager>().OnAddGemInlayListData(msg.geminlaydata);
    }
    
    /// <summary>
    /// 宝石合成
    /// </summary>
    /// <param name="gemId"></param>
    /// <param name="composeType"> 0:一个 1： 所有</param>
    public void GemComposeReq(uint gemId,uint composeType)
    {
//         stGemComposeItemWorkUserCmd_CS cmd = new stGemComposeItemWorkUserCmd_CS()
//         {
//             gemid = gemId,
//             compose_type = composeType,
//         };
//         SendCmd(cmd);
    }

    /// <summary>
    /// 宝石合成响应
    /// </summary>
    /// <param name="msg"></param>
 //   [Execute]
    ////public void OnGemComposeRes(stGemComposeItemWorkUserCmd_CS msg)
    ////{
    ////    DataManager.Manager<EquipManager>().OnGemCompose(msg.gemid);
    ////}

    /// <summary>
    /// 宝石镶嵌请求
    /// </summary>
    /// <param name="euquipIndex"></param>
    /// <param name="gemBaseId"></param>
    public void InlayGemReq( GameCmd.EquipPos pos, uint equipIndex, uint gemBaseId)
    {
        stGemInlayItemWorkUserCmd_CS cmd = new stGemInlayItemWorkUserCmd_CS()
        {
            equippos = pos,
            equip_index = equipIndex,
            gemid = gemBaseId,
        };
        SendCmd(cmd);
    }

    /// <summary>
    /// 宝石镶嵌响应
    /// </summary>
    /// <param name="msg"></param>
    [Execute]
    public void OnInlayGemRes(stGemInlayItemWorkUserCmd_CS msg)
    {
        DataManager.Manager<EquipManager>().OnGemInlay(msg.equippos,(int)msg.equip_index,msg.gemid);
    }
    
    /// <summary>
    /// 从装备格上卸下宝石
    /// </summary>
    /// <param name="pos"></param>
    /// <param name="equipGridIndex"></param>
    public void GemUnloadReq(GameCmd.EquipPos pos,uint equipGridIndex)
    {
        stGemUninlayItemWorkUserCmd_CS cmd = new stGemUninlayItemWorkUserCmd_CS()
        {
            equip_index = equipGridIndex,
            equippos = pos,
        };
        SendCmd(cmd);
    }

    /// <summary>
    /// 卸下宝石回调
    /// </summary>
    /// <param name="msg"></param>
    [Execute]
    public void OnGemUnloadRes(stGemUninlayItemWorkUserCmd_CS msg)
    {
        DataManager.Manager<EquipManager>().OnGemUnload(msg.equippos,(int)msg.equip_index);
    }


    #endregion

    #region RunStone(符石)

    /// <summary>
    /// 符石激活
    /// </summary>
    /// <param name="gemId"></param>
    /// <param name="equipThisId"></param>
    public void RunStoneActiveReq(uint runStoneId, uint equipThisId)
    {
        stRunesActivationItemWorkUserCmd_CS cmd = new stRunesActivationItemWorkUserCmd_CS()
        {
            runes_id = runStoneId,
            equip_thisid = equipThisId,
        };
        SendCmd(cmd);
    }

    /// <summary>
    /// 符石激活响应
    /// </summary>
    /// <param name="msg"></param>
    [Execute]
    public void OnRunStoneActiveRes(stRunesActivationItemWorkUserCmd_CS msg)
    {
        DataManager.Manager<EquipManager>().OnActiveRuneStone(msg.runes_id,msg.equip_thisid);
    }

    #endregion

    #region Soul

    /// <summary>
    /// 圣魂融合请求
    /// </summary>
    /// <param name="qwThisId"></param>
    /// <param name="otherId"></param>
    /// <param name="autoUseDQ"></param>
    public void UnionWeaponSoulReq(uint qwThisId,uint otherId,bool autoUseDQ)
    {
        stUnionWeaponSoulUserCmd_C cmd = new stUnionWeaponSoulUserCmd_C()
        {
            id = qwThisId,
            other_id = otherId,
            auto_buy = autoUseDQ,
        };
        SendCmd(cmd);

    }

    /// <summary>
    /// 圣魂融合响应
    /// </summary>
    /// <param name="msg"></param>
    [Execute]
    public void OnUnionWeaponSoulRes(stUnionWeaponSoulUserCmd_S msg)
    {
        DataManager.Manager<EquipManager>().OnBlendWeaponSoul(msg.item);
    }

    /// <summary>
    /// 进化请求
    /// </summary>
    /// <param name="qwThisId"></param>
    /// <param name="deputyIds"></param>
    /// <param name="autoUseDQ"></param>
    public void EvolutionWeaponSoulReq(uint qwThisId, List<uint> deputyIds, bool autoUseDQ)
    {
        stEvolutionWeaponSoulUserCmd_C cmd = new stEvolutionWeaponSoulUserCmd_C();
        cmd.id = qwThisId;
        cmd.auto_buy = autoUseDQ;
        if (null != deputyIds)
        {
            cmd.other_id.AddRange(deputyIds);
        }
        SendCmd(cmd);
    }

    /// <summary>
    /// 圣魂进化响应
    /// </summary>
    /// <param name="msg"></param>
    [Execute]
    public void OnEvolutionWeaponSoulRes(stEvolutionWeaponSoulUserCmd_S msg)
    {
        DataManager.Manager<EquipManager>().OnEvolutionWeaponSoul(msg.item);
    }

    /// <summary>
    /// 激活圣魂请求
    /// </summary>
    /// <param name="qwThisId"></param>
    /// <param name="autoUseDQ"></param>
    public void ActivationWeaponSoulReq(uint qwThisId,bool autoUseDQ)
    {
        stActivateWeaponSoulUserCmd_C cmd = new stActivateWeaponSoulUserCmd_C()
        {
            id = qwThisId,
            auto_buy = autoUseDQ,
        };
        SendCmd(cmd);
    }

    /// <summary>
    /// 激活圣魂响应
    /// </summary>
    /// <param name="msg"></param>
    [Execute]
    public void OnActivationWeaponSoulRes(stActivateWeaponSoulUserCmd_S msg)
    {
        DataManager.Manager<EquipManager>().OnActiveWeaponSoul(msg.item);
    }


    /// <summary>
    /// 解除激活圣魂请求
    /// </summary>
    /// <param name="qwThisId"></param>
    /// <param name="autoUseDQ"></param>
    public void UnActivationWeaponSoulReq(uint qwThisId,bool autoUseDQ,List<uint> propertyIds)
    {
        stUnActivateWeaponSoulUserCmd_C cmd = new stUnActivateWeaponSoulUserCmd_C()
        {
            id = qwThisId,
            auto_buy = autoUseDQ,
        };
        cmd.property_id.AddRange(propertyIds);
        SendCmd(cmd);
    }

    /// <summary>
    ///解除激活圣魂响应
    /// </summary>
    /// <param name="msg"></param>
    [Execute]
    public void OnUnActivationWeaponSoulRes(stUnActivateWeaponSoulUserCmd_S msg)
    {
        DataManager.Manager<EquipManager>().OnRemoveWeaponSoulProperty(msg.item);
    }

    /// <summary>
    /// 使用圣魂经验丹
    /// </summary>
    /// <param name="qwThisId">圣魂id</param>
    /// <param name="expItemId">经验丹id</param>
    public void AddExpWeaponSoulReq(uint qwThisId,uint expItemId,uint nNum)
    {
        stAddExpWeaponSoulUserCmd_C cmd = new stAddExpWeaponSoulUserCmd_C()
        {
            id = qwThisId,
            item = expItemId,
            num = nNum,
        };
        SendCmd(cmd);
    }

    /// <summary>
    /// 获得经验返回
    /// </summary>
    /// <param name="msg"></param>
    [Execute]
    public void OnExpWeaponSoulRes(stExpWeaponSoulUserCmd_S msg)
    {
        DataManager.Manager<EquipManager>().OnAddWeaponSoulExp(msg.id, msg.exp);
    }

    /// <summary>
    /// 成长等级升级返回
    /// </summary>
    /// <param name="msg"></param>
    [Execute]
    public void OnLevelUpWeaponSoulRes(stLevelUpWeaponSoulUserCmd_S msg)
    {
        DataManager.Manager<EquipManager>().OnWeaponSoulLevelUp(msg.item);
    }

    /// <summary>
    /// 圣魂操作错误
    /// </summary>
    /// <param name="msg"></param>
    [Execute]
    public void OnWeaponSoulError(stErrorWeaponSoulUserCmd_S msg)
    {
        TipsManager.Instance.ShowTips(ColorManager.GetColorString(ColorType.Red, "" + msg.error_no).ToString());
    }

    

    #endregion

    #region Strengthen(格子强化)

    /// <summary>
    /// 请求强化格子
    /// </summary>
    /// <param name="strengthenAll">是不是强化所有</param>
    /// <param name="pos">单个强化</param>
    public void GridStrengthenReq(bool strengthenAll,GameCmd.EquipPos pos = EquipPos.EquipPos_None)
    {
        stEquipStrengthenItemWorkUserCmd_CS cmd = new stEquipStrengthenItemWorkUserCmd_CS()
        {
            equip_pos = pos,
            all_intensify = strengthenAll,
        };
        SendCmd(cmd);
    }

    /// <summary>
    /// 强化完成响应
    /// </summary>
    /// <param name="msg"></param>
    [Execute]
    public void OnGridStrengthenRes(stEquipStrengthenItemWorkUserCmd_CS msg)
    {
        DataManager.Manager<EquipManager>().OnGridStrengthenCompelte(msg.all_intensify, msg.equip_pos);
        DataManager.Manager<ForgingManager>().OnGridStrengthenCompelte(msg.all_intensify, msg.equip_pos);
    }


    /// <summary>
    /// 服务器下发格子强化数据
    /// </summary>
    /// <param name="msg"></param>
    [Execute]
    public void OnGridStrengthenPropRes(stStrengthListPropertyUserCmd_S msg)
    {
        DataManager.Manager<EquipManager>().OnGridStrengthenProp(msg);
    }
    #endregion

    #region 兑换
    public void EquipExchange(uint exchangeId,uint num)
    {
        stEquipExchangeItemWorkUserCmd_CS cmd = new stEquipExchangeItemWorkUserCmd_CS()
        {
            index = exchangeId,
            num = num,
        };
        SendCmd(cmd);
    }

    [Execute]
    public void OnEquipExchange(stEquipExchangeItemWorkUserCmd_CS msg)
    {
        DataManager.Manager<EquipManager>().OnExchanged(msg.index, msg.num);
    }
    #endregion
}