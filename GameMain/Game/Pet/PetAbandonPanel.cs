
using UnityEngine;
using GameCmd;
using Client;
partial class PetAbandonPanel
{

    PetDataManager petDataManager
    {
        get
        {
            return DataManager.Manager<PetDataManager>();
        }
    }
    public override void OnColliderMaskClicked()
    {
        base.OnColliderMaskClicked();
        HideSelf();
    }
    protected override void OnShow(object data)
    {
        base.OnShow(data);
        if (petDataManager.CurPet != null)
        {
            string petName = petDataManager.GetCurPetName();
            m_label_diuqi_petshowname.text = petName;
            m_label_Level.text = petDataManager.GetCurPetLevelStr();
            m_label_life.text = petDataManager.CurPet.GetProp((int)PetProp.Life).ToString();
            int costLife = GameTableManager.Instance.GetGlobalConfig<int>("PetLifeCost");
            m_label_costlife.text = costLife.ToString();
            table.PetDataBase db = petDataManager.GetPetDataBase(petDataManager.CurPet.PetBaseID);
            if (db != null)
            {
                //m_sprite_Icon.spriteName = db.icon;
            }
        }
    }
    protected override void OnLoading()
    {
        base.OnLoading();
  
    }

    void onClick_Diuqi_close_Btn(GameObject caster)
    {
        HideSelf();
    }

    void onClick_Tanchuang_quxiao_Btn(GameObject caster)
    {
        HideSelf();
    }

    void onClick_Tanchuang_diuqi_Btn(GameObject caster)
    {
        if ( petDataManager.CurPet != null )
        {
            stRemovePetUserCmd_CS cmd = new stRemovePetUserCmd_CS();
            cmd.id = petDataManager.CurPet.GetID();
            NetService.Instance.Send( cmd );
        }
        HideSelf();
    }


}
