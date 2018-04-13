using System;
using System.Collections.Generic;
using System.Linq;
using GameCmd;
using System.Text;
using UnityEngine;

/// <summary>
/// 聊天频道过滤器 用于处理频道输出信息。过滤器直接和界面对接
/// </summary>
public class ChatChannelFilter
{
	/// <summary>
	/// 通知界面刷新输出
	/// </summary>
	/// <param name="text"></param>
	public delegate void RefreshText(IEnumerable<ChatInfo> text);
	public RefreshText OnRefreshOutput;
	/// <summary>
	/// 通知界面增加聊天
	/// </summary>
	public RefreshText OnAddOutput;

	/// <summary>
	/// 本过滤器下，显示的频道
	/// </summary>
	List<ChatChannel> showingChannel = new List<ChatChannel>();

	/// <summary>
	/// 聊天文本
	/// </summary>
	private readonly List<ChatInfo> lines = new List<ChatInfo>();

	bool active = false;

	public ChatChannelFilter(ChatChannel[] showingChannelList)
	{
		showingChannel.AddRange(showingChannelList);
	}
	/// <summary>
	/// 判断是否含有该频道
	/// </summary>
	/// <param name="type">频道类型</param>
	/// <returns></returns>
	public bool HasChatChannel(CHATTYPE type)
	{
		foreach (ChatChannel channel in showingChannel)
		{
			if (channel.ChannelType == type) return true;
		}
		return false;
	}

    public void RemoveChannel(CHATTYPE type)
    {
        foreach (ChatChannel channel in showingChannel)
        {
            if (channel.ChannelType == type)
            {
                channel.OnNewChat -= AddChatText;
                showingChannel.Remove(channel);
                Engine.Utility.Log.LogGroup(GameDefine.LogGroup.User_ZCX, "RemoveChannel : {0}", channel.Head);
                break;
            }
        }
    }

    public void AddChannel(ChatChannel channel)
    {
        Engine.Utility.Log.LogGroup(GameDefine.LogGroup.User_ZCX, "AddChannel : {0}",channel.Head);
        if (!showingChannel.Contains(channel))
        {
            showingChannel.Add(channel);
            channel.OnNewChat += AddChatText;   
        }
    }

	/// <summary>
	/// 激活过滤器
	/// </summary>
	/// <param name="bActive"></param>
	public void ActiveFilter(bool bActive = true)
	{
		active = bActive;
		if (active)
		{
			// 向各个频道监听“新聊天”事件
			foreach (var channel in showingChannel)
			{
				channel.OnNewChat += AddChatText;
			}
		}
		else // 过滤器失效
		{
			foreach (var channel in showingChannel)
			{
				channel.OnNewChat -= AddChatText;
				OnRefreshOutput = null;
				OnAddOutput = null;
			}
		}
	}

	/// <summary>
	/// 重新过滤频道
	/// </summary>
	public void InitFilterData()
	{
		lines.Clear();
		AddChatCmdToTextList(showingChannel.SelectMany(s => s.GetChatInfoList()).OrderBy(i => i.Timestamp));
		NoticeRefresh();
	}
	
	/// <summary>
	/// 通知界面刷新
	/// </summary>
	public void NoticeRefresh()
	{
		if (active == false)
			return;

		if (OnRefreshOutput != null)
			OnRefreshOutput(lines);
	}

	/// <summary>
	/// 通知界面增加聊天
	/// </summary>
	/// <param name="chatList"></param>
	private void NoticeAddText(IEnumerable<ChatInfo> chatList)
	{
		if (active == false)
			return;

		if (OnAddOutput != null)
			OnAddOutput(from i in chatList select i);
	}

	/// <summary>
	/// 把聊天内容存入聊天队列
	/// </summary>
	/// <param name="chatList"></param>
	private void AddChatCmdToTextList(IEnumerable<ChatInfo> chatList)
	{
		lines.AddRange(from i in chatList select i);
	}

	/// <summary>
	/// 添加新文字
	/// </summary>
	/// <param name="chatList"></param>
	void AddChatText(IEnumerable<ChatInfo> chatList)
	{
		AddChatCmdToTextList(chatList);

		NoticeAddText(chatList);
	}
}
