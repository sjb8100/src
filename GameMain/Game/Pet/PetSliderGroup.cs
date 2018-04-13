using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Client;
using table;
using Engine.Utility;

public enum PetSliderGroupType
{
    ShuXing,
    GuiYuan,
}
class PetSliderGroup:MonoBehaviour
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

    PetSliderGroupType m_type;
    public PetSliderGroupType SliderGropType
    {
        set
        {
            m_type = value;
        }
    }

    int m_nCurValue = 0;
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
        m_type = PetSliderGroupType.GuiYuan;
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
    void OnDestory()
    {
   
    }
    void OnValueUpdateEventArgs(object obj , ValueUpdateEventArgs v)
    {
        if (v == null)
        {
            return;
        }
        if (v.key == PetDispatchEventString.PetRefreshProp.ToString())
        {
            InitSliderData();
        }
        else if(v.key == PetDispatchEventString.ChangePet.ToString())
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
        int yinhunLv = CurPet.GetProp( (int)PetProp.YinHunLevel );
        PetYinHunDataBase ydb = GameTableManager.Instance.GetTableItem<PetYinHunDataBase>( (uint)yinhunLv );
        uint jiacheng = 0;
        if(ydb != null)
        {
            jiacheng = ydb.jiaChengBi;
        }
        float factor = 1 + (float)(jiacheng * 1.0f / 10000);
        if ( db != null )
        {
            string talent = db.PetTalent;
            string[] zizhiArray = talent.Split( ';' );
            var iter = sliderDic.GetEnumerator();
            while(iter.MoveNext())
            {
                var sliderPair = iter.Current;
                int curValue = petDataManager.GetPropByString( sliderPair.Key );
                if(m_type == PetSliderGroupType.GuiYuan)
                {
                    curValue = Mathf.RoundToInt( curValue * 1.0f / factor );
                }
                m_nCurValue = curValue;
                uint totalValue = 0;
                int maxValue = 0;
                string tempStr = "";
                int low = 0;
                if ( sliderPair.Key == PetProp.LevelExp.ToString() )
                {
                    // totalValue = db.upGradeExp;
                }
                else if ( sliderPair.Key == PetProp.StrengthTalent.ToString() )
                {
                    if ( zizhiArray.Length > 0 )
                    {
                        totalValue = GetTotalValue( petstate , zizhiArray[0] , ref tempStr , ref low ,ref maxValue);
                    }
                }
                else if ( sliderPair.Key == PetProp.AgilityTalent.ToString() )
                {
                    if ( zizhiArray.Length > 1 )
                    {
                        totalValue = GetTotalValue( petstate , zizhiArray[1] , ref tempStr , ref low ,ref maxValue);
                    }
                }
                else if ( sliderPair.Key == PetProp.IntelligenceTalent.ToString() )
                {
                    if ( zizhiArray.Length > 2 )
                    {
                        totalValue = GetTotalValue( petstate , zizhiArray[2] , ref tempStr , ref low,ref maxValue );
                    }
                }
                else if ( sliderPair.Key == PetProp.CorporeityTalent.ToString() )
                {
                    if ( zizhiArray.Length > 3 )
                    {
                        totalValue = GetTotalValue( petstate , zizhiArray[3] , ref tempStr , ref low,ref maxValue );
                    }
                }
                else if ( sliderPair.Key == PetProp.SpiritTalent.ToString() )
                {
                    if ( zizhiArray.Length > 4 )
                    {
                        totalValue = GetTotalValue( petstate , zizhiArray[4] , ref tempStr , ref low,ref maxValue );
                    }
                }
                UISlider slider = sliderPair.Value;
                UILabel label = slider.transform.Find( "Percent" ).GetComponent<UILabel>();
                string str = string.Format("{0}/{1}", curValue, maxValue);
                //string str = curValue.ToString();// +"" + tempStr;
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
                curValue = curValue - low;
                float precent = (float)( curValue * 1.0f / totalValue * 1.0f );
                slider.Set( precent );
            }

        }

    }

    uint GetTotalValue(int petstate, string powerArray, ref string temp, ref int low, ref int maxValue)
    {
        uint total = 0;
        string[] strArray = powerArray.Split( '_' );
        if ( strArray.Length > petstate )
        {
            if((petstate-1)  >=0 && (petstate )< strArray.Length)
            {
                string lowstr = strArray[petstate-1 ];
                if ( petstate == 5 )
                {
                    petstate += 1;
                }
                string high = strArray[petstate];
              
                temp = "(" + lowstr + "~" + high + ")";
                if(m_type == PetSliderGroupType.ShuXing)
                {
                    temp = "";
                }
                low = int.Parse( lowstr );
                total = uint.Parse( high );
                maxValue = int.Parse(high);
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

