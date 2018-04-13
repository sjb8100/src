using UnityEngine;
using System.Collections;
using Client;

public class UIConsignPetListGrid : UIGridBase
{
    public IPet petData
    {
        get;
        private set;
    }
    public UISprite Icon
    {
        get;
        private set;
    }

    protected override void OnAwake()
    {
        base.OnAwake();
        Icon = CacheTransform.Find("Icon").GetComponent<UISprite>();
    }

    public override void SetGridData(object data)
    {
        base.SetGridData(data);
        if (data == null)
	    {
            petData = null;
            return;
	    }
        petData = data as IPet;
        table.PetDataBase petdb = DataManager.Manager<PetDataManager>().GetPetDataBase(petData.PetBaseID);
        if (petdb != null)
	    {
            //Icon.atlas = DataManager.Manager<UIManager>().GetAtlasByIconName(DynamicAtlasType.DTAitem , petdb.icon);
            Icon.spriteName = petdb.icon;
	    }
    }
}
