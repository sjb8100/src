using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using table;
using Client;
class PetMessage : MonoBehaviour
{
    UILabel petLevel;
    UILabel petName;
    UILabel typeName;
    Transform petModeTrans;
    PetDataManager petDataManager;
    UITexture m__CharacterRenderTexture;
    UILabel fightLabel;
    //角色渲染
    private IRenerTextureObj characterRenderobj = null;
    PetDataBase m_pdb = null;
    bool bInit = false;
    void Awake()
    {

        InitControls();
    }
    void InitControls()
    {
        if (!bInit)
        {
            petLevel = transform.Find("petlevel").GetComponent<UILabel>();
            petName = transform.Find("petshowname").GetComponent<UILabel>();
            typeName = transform.Find("PetType/typeName").GetComponent<UILabel>();
            // fightLabel = transform.Find( "fighting/Label" ).GetComponent<UILabel>();
            petModeTrans = transform.Find("PetModel");
            UIEventListener.Get(petModeTrans.gameObject).onClick = OnPlayAudio;
            m__CharacterRenderTexture = petModeTrans.GetComponent<UITexture>();
            petDataManager = DataManager.Manager<PetDataManager>();
            bInit = true;
        }
    }
    void OnPlayAudio(GameObject go)
    {
        if(m_pdb != null)
        {
            petDataManager.PlayCreateAudio(m_pdb.fightAudio);
        }
    }
    public void OnDisable()
    {

    }

    void OnValueUpdateEventArgs(object obj, ValueUpdateEventArgs v)
    {
        if (v != null)
        {
            if (v.key == PetDispatchEventString.ChangePet.ToString())
            {
                InitData();
                if(petDataManager.CurPet != null)
                {
                    uint petID = petDataManager.CurPet.PetBaseID;
                    PetDataBase pdb = petDataManager.GetPetDataBase(petID);
                    if(pdb != null)
                    {
                        m_pdb = pdb;
                        RefreshTexture(pdb);
                    }
                }
            
            }
            else if (v.key == PetDispatchEventString.PetRefreshProp.ToString())
            {
                InitData();
            }
        }
    }
    void OnDestroy()
    {
        petDataManager.ValueUpdateEvent -= OnValueUpdateEventArgs;
    }
    void InitData(PetDataBase db = null)
    {
        InitControls();
        if (db != null)
        {
            m_pdb = db;
            if (petLevel != null)
            {
                petLevel.gameObject.SetActive(false);
            }
            if (petName != null)
            {
                petName.text = db.petName;
            }
            if (typeName != null)
            {
                typeName.text = petDataManager.GetPetAttackStrType(db);
            }

            //return;
        }
        if (petDataManager.CurPet != null)
        {
            db = petDataManager.GetPetDataBase(petDataManager.CurPet.PetBaseID);
            if (petLevel != null)
            {
                petLevel.text = petDataManager.GetCurPetLevelStr();
            }
            if (petName != null)
            {
                petName.text = petDataManager.GetCurPetName();
            }
            if (typeName != null)
            {
                typeName.text = petDataManager.GetCurPetStrType();
            }
            /*  fightLabel.text = petDataManager.CurPet.GetProp( (int)FightCreatureProp.Power ).ToString();*/
        }


    }
    public void InitPetTexture(PetDataBase db)
    {
        InitData(db);
        RefreshTexture(db);
       
    }
    void RefreshTexture(PetDataBase db)
    {
        InitControls();
        m_pdb = db;
        OnPlayAudio(null);
        if (db != null)
        {
            if (null != m__CharacterRenderTexture)
            {
                if (characterRenderobj != null)
                {
                    characterRenderobj.Release();
                }
                characterRenderobj = DataManager.Manager<RenderTextureManager>().CreateRenderTextureObj((int)db.viewModelID, 512);
                if (characterRenderobj == null)
                {
                    Engine.Utility.Log.Error("宠物模型id{0}创建贴图失败!", db.viewModelID);
                    return;
                }
                DataManager.Manager<PetDataManager>().AddRenderObj(db.viewModelID, characterRenderobj);
                ModeDiplayDataBase modelDisp = GameTableManager.Instance.GetTableItem<ModeDiplayDataBase>(db.viewModelID);
                if (modelDisp == null)
                {
                    Engine.Utility.Log.Error("宠物模型ID为{0}的模型展示数据为空", db.viewModelID);
                    return;
                }
                characterRenderobj.SetDisplayCamera(modelDisp.pos512, modelDisp.rotate512,modelDisp.Modelrotation);
                characterRenderobj.PlayModelAni(Client.EntityAction.Stand);
                UIRenderTexture rt = m__CharacterRenderTexture.GetComponent<UIRenderTexture>();
                if (null == rt)
                {
                    rt = m__CharacterRenderTexture.gameObject.AddComponent<UIRenderTexture>();
                }
                if (null != rt)
                {
                    rt.SetDepth(0);
                    rt.Initialize(characterRenderobj, characterRenderobj.YAngle, new Vector2(512, 512));
                }
            }   
        }   
    }
    void OnEnable()
    {
        petDataManager.ValueUpdateEvent += OnValueUpdateEventArgs;
        InitData();
        EnableRenderTexture(true);
    }

    public void EnableRenderTexture(bool enable)
    {
        if (null != m__CharacterRenderTexture && null != m__CharacterRenderTexture.GetComponent<UIRenderTexture>())
        {
            m__CharacterRenderTexture.GetComponent<UIRenderTexture>().Enable(enable);
        }
    }
}

