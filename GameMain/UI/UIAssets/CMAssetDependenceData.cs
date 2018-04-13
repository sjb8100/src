
//*************************************************************************
//	创建日期:	2018/1/11 星期四 14:22:33
//	文件名称:	CMAssetDependenceData
//   创 建 人:   zhudianyu	
//	版权所有:	zhudianyu
//	说    明:	
//*************************************************************************

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class CMAssetDependenceData:ScriptableObject
{
    public string AssetPath;
    public string DependentAssetPath;
    public string DependentAssetBundlePath;
}