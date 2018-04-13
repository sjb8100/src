using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using table;

/**
@brief 数据中心
*/
public class DataCenter
{
    static DataCenter s_Inst = null;
    public static DataCenter Instance()
    {
        if (null == s_Inst)
        {
            s_Inst = new DataCenter();
        }

        return s_Inst;
    }

    /////////////////////////////////////////////////////////////////////////////////////////////////////////
    //public string GetErrorDesc(uint uErrorCode)
    //{
    //    ErrorDataBase data = GameTableManager.Instance.GetTableItem<ErrorDataBase>(uErrorCode);
    //    if(data==null)
    //    {
    //        return "";
    //    }

    //    return data.strDesc;
    //}
}

