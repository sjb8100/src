using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Client;
using table;
using GameCmd;
using Common;
using Engine.Utility;
partial class Protocol
{

    #region 只处理自己的宠物

    [Execute]
    public void OnAllPetData(stUserDataPetUserCmd_S cmd)
    {
        PetUserData userData = cmd.data;
        PetDataManager pm = DataManager.Manager<PetDataManager>();
        pm.maxPetNum = userData.max_pet;
        for (int i = 0; i < userData.pet_list.Count; ++i)
        {
            AddPet(userData.pet_list[i]);
        }
        pm.HasPossessPetList = userData.pet_atlas;
       
        pm.SetQuickList( cmd.data.quick_list );
        pm.OnFirstForceFightPetUser(userData.first_force_fight);

    }
    [Execute]
    public void OnFirstForceFightPetUser(stFirstForceFightPetUserCmd_CS cmd)
    {
        PetDataManager pm = DataManager.Manager<PetDataManager>();
        pm.OnFirstForceFightPetUser(cmd.force_fight);
    }
    /// <summary>
    /// 添加一个宠物
    /// </summary>
    /// <param name="cmd"></param>
    [Execute]
    public void OnAddPet(stAddPetUserCmd_S cmd)
    {
        if ( cmd.action == (uint)AddPetAction.AddPetAction_Refresh )
        {
            IEntitySystem es = ClientGlobal.Instance().GetEntitySystem();
            if ( es == null )
            {
                Engine.Utility.Log.Error( "严重错误：EntitySystem is null!" );
                return;
            }
            EntityCreateData data = new EntityCreateData();
            data.PropList = new EntityAttr[(int)PetProp.End - (int)EntityProp.Begin];
            data.ID = cmd.obj.id;
            RoleUtil.BuildPetPropListByPetData( cmd.obj , ref data.PropList );
            IPet pet = es.FindPet( cmd.obj.id );
            if ( pet != null )
            {
                pet.UpdateProp( data );
                pet.SetExtraData( cmd.obj );
                DataManager.Manager<PetDataManager>().RefreshPetProp();
            }
            else
            {
                Log.Error( "pet entity create failed !!!" );
            }
        }
        else
        {
            AddPet( cmd.obj );
            PetDataBase pdb = GameTableManager.Instance.GetTableItem<PetDataBase>(cmd.obj.base_id);
            if (pdb != null)
            {
                string tips = string.Format("{0}{1}", CommonData.GetLocalString("获得珍兽"), pdb.petName);
                TipsManager.Instance.ShowTips(tips);
                string txt = DataManager.Manager<TextManager>().GetLocalFormatText(LocalTextType.Talk_System_huodezhanghun, pdb.petName);
                ChatDataManager.SendToChatSystem(txt);
            }
        }
    }
    void AddPet(PetData data)
    {
        IEntitySystem es = ClientGlobal.Instance().GetEntitySystem();
        if ( es == null )
        {
            Engine.Utility.Log.Error( "严重错误：EntitySystem is null!" );
            return;
        }
        EntityCreator.Instance().AddPet( data );
        IPet pet = es.FindPet( data.id );
        if ( pet != null )
        {
            pet.SetExtraData( data );
            DataManager.Manager<PetDataManager>().AddPet( pet.GetID() , pet );
        }
        else
        {
            Log.Error( "pet entity create failed !!!" );
        }
    }

    /// <summary>
    /// 升级宠物
    /// </summary>
    /// <param name="cmd"></param>
    [Execute]
    public void OnPetLevelUP(stLevelUpPetUserCmd_S cmd)
    {
        DataManager.Manager<PetDataManager>().OnPetLevelUP( cmd );
    }
    /// <summary>
    /// 加点 和 重置消息  请求和回应
    /// </summary>
    /// <param name="cmd"></param>
    [Execute]
    public void OnAddPoint(stAttrPointPetUserCmd_S cmd)
    {
        DataManager.Manager<PetDataManager>().OnAddPoint( cmd );
    }

    /// <summary>
    /// 对宠物使用物品加寿命
    /// </summary>
    /// <param name="cmd"></param>
    [Execute]
    public void OnAddPetLife(stLifePetUserCmd_S cmd)
    {
        DataManager.Manager<PetDataManager>().OnAddPetLife( cmd );
    }
    /// <summary>
    /// 宠物当前经验 战斗中获得的经验不用这条消息 
    /// </summary>
    /// <param name="cmd"></param>
    [Execute]
    public void OnPetCurExp(stExpPetUserCmd_S cmd)
    {
        DataManager.Manager<PetDataManager>().OnPetCurExp( cmd );
    }
    /// <summary>
    /// 请求改名 答复(出战中的宠物 就广播)
    /// </summary>
    /// <param name="cmd"></param>
    [Execute]
    public void OnChangePetName(stChangeNamePetUserCmd_CS cmd)
    {
        DataManager.Manager<PetDataManager>().OnPetChangeName( cmd );
    }
    /// <summary>
    /// 回复给客户端 当前的宠物上限
    /// </summary>
    /// <param name="cmd"></param>
    [Execute]
    public void OnPetMaxNum(stAddMaxNumPetUserCmd_CS cmd)
    {
        DataManager.Manager<PetDataManager>().OnPetMaxNum( cmd );
    }
    /// <summary>
    /// 引魂
    /// </summary>
    /// <param name="cmd"></param>
    [Execute]
    public void OnPetYinHun(stYinHunPetUserCmd_CS cmd)
    {
        DataManager.Manager<PetDataManager>().OnPetYinHun( cmd );

    }
    /// <summary>
    /// 归元出现新的天赋 或者 替换天赋也用这个消息回应
    /// </summary>
    /// <param name="cmd"></param>
    [Execute]
    public void OnPetTalent(stTalentPetUserCmd_S cmd)
    {
        DataManager.Manager<PetDataManager>().OnPetGuiYuan( cmd );
    }
    [Execute]
    public void OnLearnSkill(stLearnSkillPetUserCmd_CS cmd)
    {
        DataManager.Manager<PetDataManager>().OnLearnSkill( cmd );
    }

    [Execute]
    public void OnPetSkillUPGrade(stUpSkillPetUserCmd_CS cmd)
    {
        DataManager.Manager<PetDataManager>().OnSkillUpGrade( cmd );
    }

    [Execute]
    public void OnCallBackPet(stCallBackPetUserCmd_CS cmd)
    {
        DataManager.Manager<PetDataManager>().OnCallBackPet( cmd );
    }
    [Execute]
    public void OnRemovePet(stRemovePetUserCmd_CS cmd)
    {
        DataManager.Manager<PetDataManager>().RemovePet( cmd );
    }

    [Execute]
    public void OnDeletePetSkill(stRemoveSkillPetUserCmd_S cmd)
    {
        DataManager.Manager<PetDataManager>().DeletePetSkill( cmd );
    }
    [Execute]
    public void OnAddFightPet(stUseFightPetUserCmd_CS cmd)
    {
        DataManager.Manager<PetDataManager>().OnAddFightPet( cmd );
    }
    [Execute]
    public void OnAllCanMakePet(stYouCanExchangePetUserCmd_S cmd)
    {
        DataManager.Manager<PetDataManager>().AllCanMakePets( cmd );
    }

    [Execute]
    public void OnFightPower(stFightPowerPetUserCmd_S cmd)
    {
        DataManager.Manager<PetDataManager>().OnFightPower( cmd );
    }

    [Execute]
    public void OnPetDead(stPetDiePetUserCmd_S cmd)
    {
        DataManager.Manager<PetDataManager>().OnPetDead( cmd.id );
    }

    [Execute]
    public void OnPetQucickList(stSetQuickListPetUserCmd_CS cmd)
    {
        DataManager.Manager<PetDataManager>().SetQuickList( cmd.quick_list );
    }

    [Execute]
    public void OnRefreshPetAttr(stRefreshAttrPetUserCmd_S cmd)
    {
        DataManager.Manager<PetDataManager>().OnRefreshPetAttr( cmd );
    }

    [Execute]
    public void OnLockPetSkill(stLockSkillPetUserCmd_CS cmd)
    {
        DataManager.Manager<PetDataManager>().OnLockPetSkill( cmd );
    }

    [Execute]
    public void OnGuiyuanReturn(stGuiYuanPetUserCmd_CS cmd)
    {
        DataManager.Manager<PetDataManager>().OnGuiyuanReturn(cmd);
    }

    [Execute]
    public void OnChangeNpcName(stBroadCastNamePetUserCmd_S cmd)
    {
        DataManager.Manager<PetDataManager>().OnChangeNpcName(cmd);
    }

    [Execute]
    public void OnFightPetAttr(stFightPetAttrPetUserCmd_S cmd)
    {
        DataManager.Manager<PetDataManager>().OnFightPetAttr(cmd);
    }

    [Execute]
    public void OnInhertPet(stInheritPetUserCmd_CS cmd)
    {
        DataManager.Manager<PetDataManager>().OnInhertPet(cmd);
    }
    [Execute]
    public void OnPlanSucess(stAttrPlanPetUserCmd_C cmd)
    {
        DataManager.Manager<PetDataManager>().OnPlanSucess(cmd);
    }
    #endregion
}

