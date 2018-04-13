using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Client;
using table;
using Engine.Utility;
class PetGuiYuanHouSliderGroup:MonoBehaviour
{
    PetDataManager petDataManager;
    IPet curPet;
    public IPet CurPet
    {
        get
        {
            return petDataManager.CurPet;
        }
    }
    Dictionary<string , UISlider> sliderDic = new Dictionary<string , UISlider>();
    void Awake()
    {
        petDataManager = DataManager.Manager<PetDataManager>();
        UISlider liliangslider = transform.Find( "liliangslider" ).GetComponent<UISlider>();
        sliderDic.Add( PetProp.StrengthTalent.ToString() , liliangslider );
        UISlider tilislider = transform.Find( "tilislider" ).GetComponent<UISlider>();
        sliderDic.Add( PetProp.CorporeityTalent.ToString() , tilislider );
        UISlider zhilislider = transform.Find( "zhilislider" ).GetComponent<UISlider>();
        sliderDic.Add( PetProp.IntelligenceTalent.ToString() , zhilislider );
        UISlider jingshenslider = transform.Find( "jingshenslider" ).GetComponent<UISlider>();
        sliderDic.Add( PetProp.SpiritTalent.ToString() , jingshenslider );
        UISlider minjieslider = transform.Find( "minjieslider" ).GetComponent<UISlider>();
        sliderDic.Add( PetProp.AgilityTalent.ToString() , minjieslider );
    }
    void Start()
    {

    }
    void OnEnable()
    {
        petDataManager.ValueUpdateEvent += OnValueUpdateEventArgs;
        InitSliderData();
    }
    void OnDisable()
    {
        petDataManager.ValueUpdateEvent -= OnValueUpdateEventArgs;
    }
    void OnValueUpdateEventArgs(object obj , ValueUpdateEventArgs v)
    {
        if(v == null)
        {
            return;
        }
        if(v.key == PetDispatchEventString.PetRefreshProp.ToString()||
           v.key == PetDispatchEventString.ChangePet.ToString())
        {
            InitSliderData();
        }
    }

    void InitSliderData()
    {
        if ( CurPet == null )
            return;
        int level = CurPet.GetProp( (int)CreatureProp.Level );
        int petstate = CurPet.GetProp( (int)PetProp.PetGuiYuanStatus );
        //PetUpGradeDataBase db = GetPetUpGradeDataBase( (uint)level , petstate );
        PetDataBase db = petDataManager.GetPetDataBase( CurPet.PetBaseID );

        int yinhunLv = CurPet.GetProp((int)PetProp.YinHunLevel);
        PetYinHunDataBase ydb = GameTableManager.Instance.GetTableItem<PetYinHunDataBase>((uint)yinhunLv);
        uint jiacheng = 0;
        if (ydb != null)
        {
            jiacheng = ydb.jiaChengBi;
        }
        float factor = 1 + (float)(jiacheng * 1.0f / 10000);
        if ( db != null )
        {
            string talent = db.PetTalent;
            string[] zizhiArray = talent.Split( ';' );
            GameCmd.PetTalent replaceTalent = CurPet.GetReplaceTalent();
            foreach ( var sliderPair in sliderDic )
            {
                uint curValue = 0;
                uint totalValue = 0;
                string tempStr = "";
                int low = 0;
                if (sliderPair.Key == PetProp.LevelExp.ToString())
                {
                    // totalValue = db.upGradeExp;
                }
                else if (sliderPair.Key == PetProp.StrengthTalent.ToString())
                {
                    curValue = replaceTalent.liliang;
                    if (zizhiArray.Length > 0)
                    {
                        totalValue = GetTotalValue(petstate, zizhiArray[0], ref tempStr, ref low);
                    }
                }
                else if (sliderPair.Key == PetProp.AgilityTalent.ToString())
                {
                    curValue = replaceTalent.minjie;
                    if (zizhiArray.Length > 1)
                    {
                        totalValue = GetTotalValue(petstate, zizhiArray[1], ref tempStr, ref low);
                    }
                }
                else if (sliderPair.Key == PetProp.IntelligenceTalent.ToString())
                {
                    curValue = replaceTalent.zhili;
                    if (zizhiArray.Length > 2)
                    {
                        totalValue = GetTotalValue(petstate, zizhiArray[2], ref tempStr, ref low);
                    }
                }
                else if (sliderPair.Key == PetProp.CorporeityTalent.ToString())
                {
                    curValue = replaceTalent.tizhi;
                    if (zizhiArray.Length > 3)
                    {
                        totalValue = GetTotalValue(petstate, zizhiArray[3], ref tempStr, ref low);
                    }
                }
                else if (sliderPair.Key == PetProp.SpiritTalent.ToString())
                {
                    curValue = replaceTalent.jingshen;
                    if (zizhiArray.Length > 4)
                    {
                        totalValue = GetTotalValue(petstate, zizhiArray[4], ref tempStr, ref low);
                    }
                }
  
                UISlider slider = sliderPair.Value;
                UILabel label = slider.transform.Find( "Percent" ).GetComponent<UILabel>();
                int oldTelant = petDataManager.GetPropByString(sliderPair.Key);
                oldTelant = Mathf.RoundToInt((int)oldTelant * 1.0f / factor);
                curValue = (uint)Mathf.RoundToInt((int)curValue * 1.0f / factor);
                int delta = (int)curValue - oldTelant;
                ColorType c = ColorType.Green;
                string sign = "+";
                if(delta >= 0)
                {
                    c = ColorType.Green;
                    if(delta == 0)
                    {
                        sign = "";
                        c = ColorType.White;
                    }
                }
                else
                {
                    sign = "";
                    c = ColorType.Red;
                }

                string str = curValue + " " + ColorManager.GetColorString(c, "(" + sign + delta + ")");
                label.text = str;
                if ( totalValue == 0 )
                {
                    totalValue = (uint)curValue;
                    if(totalValue == 0)
                    {
                        totalValue = 10000;
                    }
                    Log.Info( "total value is 0" );
                }
                curValue = curValue - (uint)low;
                float precent = (float)( curValue * 1.0f / totalValue * 1.0f );
                slider.Set( precent );
            }

        }

    }
    uint GetTotalValue(int petstate,string powerArray , ref string temp,ref int low)
    {
        uint total = 0;
        string[] strArray = powerArray.Split( '_' );
   
        if ( strArray.Length > petstate )
        {
            if((petstate-1)  >=0 && (petstate )< strArray.Length)
            {
                string lowstr = strArray[petstate-1 ];
                if ( petstate == 5 )
                {//高级归元的区间
                    petstate += 1;
                }
                string high = strArray[petstate];
                temp = "(" + lowstr + "~" + high + ")";
                low = int.Parse( lowstr );
                total = uint.Parse( high );
                total = total - (uint)low;
            }
            else
            {
                Log.Error( " petstate is " + petstate.ToString() );
            }
        }
        return total;
    }
    
}

