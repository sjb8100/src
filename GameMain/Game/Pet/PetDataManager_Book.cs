
//*************************************************************************
//	创建日期:	2017/8/8 星期二 17:16:16
//	文件名称:	PetDataManager_Tujian
//   创 建 人:   zhudianyu	
//	版权所有:	zhudianyu
//	说    明:	
//*************************************************************************

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using table;

partial class PetDataManager
{
    List<PetDataBase> m_petTujianDbList = new List<PetDataBase>();
    public List<PetDataBase> PetTuJianList
    {
        get
        {
            return m_petTujianDbList;
        }
        set
        {
            PetTuJianList = value;
        }
    }
    //当前选中图鉴
    public int CurPetTujianIndex = 0;

    public PetDataBase GetLastPet(PetDataBase curData)
    {
        int index = 0;
        for(int i = 0;i<PetTuJianList.Count;i++)
        {
            var db = PetTuJianList[i];
            if(curData.petID == db.petID)
            {
                index = i;
                break;
            }
        }
        if(index == 0)
        {
            index = PetTuJianList.Count - 1;
        }
        else
        {
            index -= 1;
        }
        return PetTuJianList[index];
    }
    public PetDataBase GetNextPet(PetDataBase curData)
    {
        int index = 0;
        for (int i = 0; i < PetTuJianList.Count; i++)
        {
            var db = PetTuJianList[i];
            if (curData.petID == db.petID)
            {
                index = i;
                break;
            }
        }
        if (index == PetTuJianList.Count - 1)
        {
            index = 0;
        }
        else
        {
            index += 1;
        }
        return PetTuJianList[index];
    }
}
