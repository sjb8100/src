//=========================================================================================
//
//    Author: zhudianyu
//    宠物属性界面处理
//    CreateTime:  2016/06/24
//
//=========================================================================================
using UnityEngine;

using GameCmd;
partial class PetChangeNamePanel
{
    protected override void OnShow(object data)
    {

        base.OnShow(data);
        m_input_input.validation = UIInput.Validation.Filename;
    }
    string petname;
    void onClick_Gaiming_close_Btn(GameObject caster)
    {
        HideSelf();
    }

    void onClick_Gaiming_quxiao_Btn(GameObject caster)
    {
        petname = m_input_input.value;
        HideSelf();
    }

    void onClick_Gaiming_queding_Btn(GameObject caster)
    {
        petname = m_input_input.value;
        petname = petname.Replace(" ","");
        petname = DataManager.Manager<TextManager>().ReplaceSensitiveWord(petname, TextManager.MatchType.Max);
        stChangeNamePetUserCmd_CS cmd = new stChangeNamePetUserCmd_CS();
        PetDataManager dm = DataManager.Manager<PetDataManager>();

        if(dm.CurPet != null)
        {
            cmd.id = dm.CurPet.GetID();
            cmd.name = petname;
            NetService.Instance.Send( cmd );
        }
        HideSelf();
    }


}
