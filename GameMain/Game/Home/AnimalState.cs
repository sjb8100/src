using Engine.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;


class AnimalState : MonoBehaviour
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
    GameObject gainBtn;
    GameObject growBtn;
    UILabel timeLable;
    UILabel landState;
    int m_nLeftTime = -1;
    int m_nLandIndex;//土地的index
    void Awake()
    {
        gainTrans = transform.Find("Mature_status");
        growTrans = transform.Find("Grow_status");
        gainBtn = transform.Find("Mature_status/gain").gameObject;
        growBtn = transform.Find("Grow_status/Growing").gameObject;


        if (growTrans != null)
        {
            Transform tt = growTrans.Find("Time");
            if (tt != null)
            {
                timeLable = tt.GetComponent<UILabel>();
            }
        }
        if (gainTrans != null)
        {
            Transform gainLabel = gainTrans.Find("gain/Label");
            if (gainLabel != null)
            {
                landState = gainLabel.GetComponent<UILabel>();
                if (landState != null)
                {
                    landState.text = "可收获";
                }
            }
        }

        UIEventListener.Get(gainBtn).onClick = OnClickGain;
        UIEventListener.Get(growBtn).onClick = OnClickGrow;
    }

    float tempTime = 0;
    void Update() 
    {
        tempTime += Time.deltaTime;
        if (tempTime >0.5f)
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
                    ShowAnimalUIByState(HomeDataManager.CreatureSmallState.CanGain);
                }
                
            }

            tempTime = 0;
        }
    }

    public void ShowLandState()
    {
        gameObject.SetActive(true);
        ShowGain(true);
        if (landState != null)
        {
            landState.text = "可扩建";
        }
    }
    public void InitIndex(int index)
    {
        m_nLandIndex = index;
    }
    public void Init(uint leftTime, int landIndex, int state)
    {
        ShowAnimalUIByState((HomeDataManager.CreatureSmallState)state);
    }

    public void ShowAnimalUIByState(HomeDataManager.CreatureSmallState ps)
    {
        if (ps == HomeDataManager.CreatureSmallState.CanGain)//可收获
        {
            gameObject.SetActive(true);
            gainTrans.gameObject.SetActive(true);
            growTrans.gameObject.SetActive(false);
            homeDM.AddCanGainLandIndex(m_nLandIndex);
        }
        else if (ps == HomeDataManager.CreatureSmallState.Seed || ps == HomeDataManager.CreatureSmallState.Seeding || ps == HomeDataManager.CreatureSmallState.Ripe)
        {
            gameObject.SetActive(true);
            gainTrans.gameObject.SetActive(false);
            growTrans.gameObject.SetActive(true);
        }
    }

    void ShowGain(bool bShow)
    {
        if (gainTrans != null)
        {
            gainTrans.gameObject.SetActive(bShow);
        }
        if (growTrans != null)
        {
            growTrans.gameObject.SetActive(!bShow);
        }
    }

    /// <summary>
    /// 收获
    /// </summary>
    void OnClickGain(GameObject go)
    {
        Dictionary<uint, uint> SeedIndexDic = DataManager.Manager<HomeDataManager>().SeedIndexDic;
        uint m_animalIndexStart = DataManager.Manager<HomeDataManager>().GetAnimalIndexStart();

        if (SeedIndexDic.ContainsKey((uint)this.m_nLandIndex))
        {
            DataManager.Manager<HomeDataManager>().ReqGainAnimal(SeedIndexDic[(uint)this.m_nLandIndex], (uint)this.m_nLandIndex - m_animalIndexStart);
        }
    }

    /// <summary>
    /// 立即成熟
    /// </summary>
    void OnClickGrow(GameObject go)
    {
        uint m_animalIndexStart = DataManager.Manager<HomeDataManager>().GetAnimalIndexStart();

        DataManager.Manager<HomeDataManager>().ReqAtOnceGrow((uint)this.m_nLandIndex - m_animalIndexStart);
    }
}

