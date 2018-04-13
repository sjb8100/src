using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Client;

[ExecuteInEditMode]
public class MapManager : MonoBehaviour
{
	public int gridXNum = 120;
	public int gridZNum = 120;

	/// <summary>
	/// 索引方式：[z * gridXNum + x]
	/// </summary>
	public TileType[] grids;

	/// <summary>
	/// 显示格子
	/// </summary>
	public bool ShowGrids { get; set; }

	/// <summary>
	/// get a grid at index of grids
	/// </summary>
	/// <param name="x"></param>
	/// <param name="z"></param>
	/// <returns></returns>
	public TileType this[int x, int z]
	{
		get
		{
			var index = z * gridXNum + x;
			if (grids == null || index < 0 || index > grids.Length)
				return TileType.TileType_None;
			return grids[index];
		}
		set
		{
			var index = z * gridXNum + x;
			if (grids == null || index < 0 || index > grids.Length)
				return;
			grids[index] = value;
		}
	}

	/// <summary>
	/// Creates a new grid of tile nodes of x by y count
	/// </summary>
	public void Reset()
	{
		Debug.LogWarning(string.Format("阻挡数据重置为 {0}*{1}", gridXNum, gridZNum));
		grids = new TileType[gridZNum * gridXNum];
	}
}
