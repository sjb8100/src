using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Client
{
    public interface ILuaSystem
    {
        void Release();

        // call lua function
        object[] CallLuaFunction(string strLuaFuncName, params object[] args);

        void DoString(string str);

        //运行Lua文件
        void DoFile(ref string strLuaFileNmae);

        void ExecuteEvent(int nHandler, string strObjName, object obj);

        // lua状态机
        IntPtr GetLuaState();
    }
}
