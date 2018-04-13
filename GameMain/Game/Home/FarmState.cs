using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Client;
using table;
using Engine;
using Engine.Utility;
class FarmState : MonoBehaviour
{
    HomeDataManager homeDM
    {
        get
        {
            return DataManager.Manager<HomeDataManager>();
        }
    }
    Transform gainTrans;
    Transform growTrans;
    UILabel timeLable;
    UILabel landState;
    int m_nLeftTime = -1;
    int m_nLandIndex;//土地的index
    void Awake()
    {
        gainTrans = transform.Find( "Mature_status" );
        growTrans = transform.Find( "Grow_status" );
        if ( growTrans != null )
        {
            Transform tt = growTrans.Find( "Time" );
            if ( tt != null )
            {
                timeLable = tt.GetComponent<UILabel>();
            }
        }
        if ( gainTrans != null )
        {
            Transform gainLabel = gainTrans.Find( "gain/Label" );
            if ( gainLabel != null )
            {
                landState = gainLabel.GetComponent<UILabel>();
                if ( landState != null )
                {
                    landState.text = "可收获";
                }
            }
        }
    }

    float tempTime = 0;
    void Update()
    {
        tempTime += Time.deltaTime;
        if (tempTime > 0.5f)
        {
            if (DataManager.Manager<HomeDataManager>().CanGetLeftTime(this.m_nLandIndex))
            {
                m_nLeftTime = (int)DataManager.Manager<HomeDataManager>().GetLeftTimeByIndex(this.m_nLandIndex);
                if (m_nLeftTime > 0)
                {
                    timeLable.text = StringUtil.GetStringBySeconds((uint)m_nLeftTime);
                }
                if (m_nLeftTime == 0)
                {
                    ShowUIByState(HomeDataManager.CreatureSmallState.CanGain);
                }

            }

            tempTime = 0;
        }
    }


    public void ShowLandState()
    {
        gameObject.SetActive( true );
        ShowGain( true );
        if ( landState != null )
        {
            landState.text = "可扩建";
        }
    }
    public void InitIndex(int index)
    {
        m_nLandIndex = index;
    }
    public void Init(uint leftTime , int landIndex)
    {
   
        m_nLeftTime = (int)leftTime;
        if ( m_nLeftTime > 0 )
        {
            ShowUIByState( HomeDataManager.CreatureSmallState.Seeding );
            //TimerAxis.Instance().SetTimer( 1000 , 1000 , this );
        }
        if ( m_nLeftTime == 0 )
        {
            //TimerAxis.Instance().KillTimer( 1000 , this );

            ShowUIByState( HomeDataManager.CreatureSmallState.CanGain );

        }
    }

    public void ShowUIByState(HomeDataManager.CreatureSmallState ps)
    {

        if ( ps == HomeDataManager.CreatureSmallState.CanGain )
        {
            gameObject.SetActive( true );
            ShowGain( true );
            homeDM.AddCanGainLandIndex( m_nLandIndex );
            landState.text = "可收获";
           // TipsManager.Instance.ShowTipsById(114501);

        }
        else if ( ps == HomeDataManager.CreatureSmallState.None )
        {
            gameObject.SetActive( false );
        }
        else
        {
            gameObject.SetActive( true );
            ShowGain( false );
        }


    }

    void ShowGain(bool bShow)
    {
        if ( gainTrans != null )
        {
            gainTrans.gameObject.SetActive( bShow );
        }
        if ( growTrans != null )
        {
            growTrans.gameObject.SetActive( !bShow );
        }
    }
}
