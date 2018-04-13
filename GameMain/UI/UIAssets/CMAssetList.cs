
//*************************************************************************
//	创建日期:	2018/1/11 星期四 15:58:45
//	文件名称:	CMAssetList
//   创 建 人:   zhudianyu	
//	版权所有:	zhudianyu
//	说    明:	
//*************************************************************************

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
public class CMAssetList:ScriptableObject
{
    public List<CMAssetDependenceData> cmList = new List<CMAssetDependenceData>();
}
