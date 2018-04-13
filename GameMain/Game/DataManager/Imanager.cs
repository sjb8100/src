using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public interface IManager
{
    /// <summary>
    /// 初始化
    /// </summary>
    void Initialize();
    /// <summary>
    /// 重置
    /// </summary>
    /// <param name="depthClearData">是否深度清除管理器数据</param>
    void Reset(bool depthClearData = false);
    /// <summary>
    /// 每帧执行
    /// </summary>
    /// <param name="deltaTime"></param>
    void Process(float deltaTime);
    /// <summary>
    /// 清理数据
    /// </summary>
    void ClearData();

}