using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Client
{
    public enum ControllerType
    {
        ControllerType_null = 0, // 没有设置
        ControllerType_KeyBoard = 1, // 键盘
        ControllerType_Touch = 2, // 触摸
        ControllerType_Joystic = 3, // 摇杆
    }
}
