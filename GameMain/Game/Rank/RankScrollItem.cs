//********************************************************************
//	创建日期:	2016-10-19   17:57
//	文件名称:	RankScrollItem.cs
//  创 建 人:   刘超	
//	版权所有:	中青宝
//	说    明:	排行榜左侧列表
//********************************************************************
using UnityEngine;
using System.Collections;
using System;
[Flags]

public enum RankScrollTypeItem
{
    Person = 0 ,
    Profession ,
    Arena ,
    Family ,
    Famous ,
}
public class RankScrollItem : MonoBehaviour
{
    RankScrollTypeItem m_type;
}
