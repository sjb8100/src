using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Linq;
using System.Linq;
using System.Text.RegularExpressions;
using Common;
using Cmd;

public class RichXmlHelper
{
	public static string GetAtlasesByID(int id)
	{
		switch (id)
		{
			case 5:
				return "Atlases/TaskGUI";
			default:
				return "Atlases/TaskGUI";
		}
	}

	public static string GetFrameByID(int id)
	{
		switch (id)
		{
			case 35:
				return "money_ref"; //绑银
			case 36:
				return "money_ref"; //赠点
			case 40:
				return "EXP";
			case 53:
				return "xunzhang";
			default:
				return "EXP";
		}
	}

	private static IEnumerable<XElement> Convert(IEnumerable<XElement> xml)
	{
		foreach (var e in xml)
		{
			switch (e.Name.ToString())
			{
				case "p":
					{
						var firstChildNode = e.FirstNode;
						if (firstChildNode != null)
							yield return new XElement("p", Convert(e.Elements()));
						else
							yield return e;
					}
					break;
				case "n":
					{
						if (e.Attribute("color") != null)
						{
							yield return new XElement("n",
								new XElement("color", e.Value, new XAttribute("value", ConverColor(e.AttributeValue("color")))));
						}
						else if (e.AttributeValue("text") != null)
						{
							yield return new XElement("n", e.AttributeValue("text"));
						}else if (e.Attribute("fontsize") != null)
						{
                            yield return new XElement("n",
                                new XElement("size", e.Value, new XAttribute("value", e.AttributeValue("value"))));
						}
						else
							yield return e;
					}
					break;
				case "button":
					{
						if (e.Attribute("group") != null)
						{
							var buttonGropValue = System.Convert.ToInt32(e.AttributeValue("group"));
							//单选题任务
							if (buttonGropValue == 12)
							{
								int frameValue = System.Convert.ToInt32(e.AttributeValue("frame"));
								string tempValue = null;
								if (frameValue == 125)
									tempValue = "×";
								else if (frameValue == 126)
									tempValue = "√";
								tempValue = tempValue + " " + e.AttributeValue("text");
								yield return new XElement("a", new XAttribute("href", "answer://" + e.AttributeValue("choosevalue").ToString()), tempValue);
							}
							//任务奖励
							else if (buttonGropValue == 5)
							{
								yield return new XElement("img", new XAttribute("atlas", GetAtlasesByID(System.Convert.ToInt32(e.AttributeValue("group")))), new XAttribute("sprite", GetFrameByID(System.Convert.ToInt32(e.AttributeValue("frame")))));
								yield return new XElement("n", e.Value);
							}
						}
						else
						{
							yield return e;
						}
					}
					break;
				case "goto":
					{
						//地图跳转
						if (e.Attribute("pos") != null)
						{
							yield return new XElement("a", new XAttribute("href", new XElement("mgs", ConvertAttrbutes(e.Attributes())).ToString()), GetGoToMapElement(e));
						}
                        else if (e.Attribute("txt") != null)
                        {
                            yield return new XElement("a", new XAttribute("href", new XElement("mgs", ConvertAttrbutes(e.Attributes())).ToString()), e.Attribute("txt").Value);
                        }
						else if (e.Attribute("itemid") != null)
						{
                            yield return new XElement("a", new XAttribute("href", new XElement("mgs", ConvertAttrbutes(e.Attributes())).ToString()), GetHandleItem(e));
						}else
						{
							yield return new XElement("a", new XAttribute("href", new XElement("mgs", ConvertAttrbutes(e.Attributes())).ToString()), GetGoToNpcElement(e));
						}
					}
                    break;
                case "img":
                    {
                        //<img atlas="chatatlas" sprite="button_xiaolian"/>
                        yield return new XElement("img", ConvertAttrbutes(e.Attributes()));
                    }
                    break;
                case "ani":
                    {
                        yield return new XElement("ani", ConvertAttrbutes(e.Attributes()));
                    }
                    break;
				case "user":
					{
						//yield return new XElement("color", new XAttribute("value", "green"), MainRole.Instance.Role.RoleName);
                        yield return new XElement("color", new XAttribute("value", "green"), Client.ClientGlobal.Instance().MainPlayer.GetName());
					}
					break;
				case "newline":
					{
						yield return new XElement("br");
					}
					break;
				case "book":
					{
						yield return new XElement("a", e.AttributeValue("text"));
					}
					break;
				case "dialog":
					{
						yield return new XElement("a", e.AttributeValue("text"));
					}
					break;
				case "q":
					{
						yield return new XElement("n", e.Value);
					}
					break;
				case "shell":
					{
						yield return new XElement("a",
							new XElement("href", new KeyValueString(e.Attributes()).Value),
							e.Value);
					}
					break;
                case "item":
                    {
                        string strNum = e.AttributeValue("num");
                        int id = int.Parse(e.AttributeValue("id"));
                        int nnum = DataManager.Manager<ItemManager>().GetItemNumByBaseId((uint)id);
                        if (nnum >= int.Parse(strNum))
                        {
                            yield return new XElement("color", new XAttribute("value", "green"),
                                string.Format("({0}/{1})", nnum, strNum));
                        }else{
                            yield return new XElement("color", new XAttribute("value", "red"),
                                string.Format("({0}/{1})", nnum, strNum));
                        }
                    }
                    break;
                case "fontsize":
                    yield return new XElement("size", new XAttribute("value", e.Attribute("size")));
                    break;
				default:
					yield return e;
					break;
			}
		}
	}

	private static IEnumerable<XAttribute> ConvertAttrbutes(IEnumerable<XAttribute> e)
	{
		foreach (var attr in e)
		{
			yield return new XAttribute(attr.Name, attr.Value);
		}
	}

	/// <summary>
	/// 根据NPC表格获得对应NPC名字的颜色
	/// </summary>
	/// <param name="colorid"></param>
	/// <returns></returns>
	public static string GetNameColor(uint colorid)
	{
		//颜色 0.无效 1.白 2.蓝 3.黄 4.绿 5.紫
		var map = new GXColor[]
		{
			GXColor.None,
			GXColor.White, 
			GXColor.Blue,
			GXColor.Yellow,
			GXColor.Green,
			GXColor.Violet,
		};
		var c = map[0];
		if (colorid < map.Length)
			c = map[colorid];
		return string.Format("#{0:X8}", (uint)c);
	}

	private static string ConverColor(string color)
	{
		var name = color.Substring(color.IndexOf("GXColor") + 7); // GXColorNAME -> NAME
		var c = GXColor.None;
		if(System.Enum.IsDefined(typeof(GXColor), name))
			c = (GXColor)System.Enum.Parse(typeof(GXColor), name);
		return string.Format("#{0:X8}", (uint)c);
	}
    public static System.Text.StringBuilder tempStringBuilder = new System.Text.StringBuilder();

	/// <summary>
	/// 将《兵王》格式的RichXML转换为<see cref="UIXmlRichText"/>格式的XML
	/// </summary>
	/// <param name="xml"></param>
	/// <returns></returns>
	public static string RichXmlAdapt(string xml)
	{
		if (xml == null)
			return "";
		xml = xml.Trim();
		if (xml.FirstOrDefault() != '<')
		{
//			Debug.Log("不符合我们规定的XML语法直接返回原字符串");
			return xml;
		}
		try
		{
            var pre = "<root>";
            var post = "</root>";
            //tempStringBuilder.Remove(0, tempStringBuilder.Length);
            //tempStringBuilder.Append(pre);
            //tempStringBuilder.Append(XmlRepair(xml));
            ////tempStringBuilder.Append(xml);
            //tempStringBuilder.Append(post);
            
            var str = pre + XmlRepair(xml) + post;
            //var str = tempStringBuilder.ToString();
            var well = RichXmlHelper.Convert(XDocument.Parse(str).Root.Elements());

			str = new XElement("root", well).ToString(SaveOptions.DisableFormatting);
			str = str.Substring(pre.Length, str.Length - pre.Length - post.Length);
#if UNITY_EDITOR
			//Debug.Log(string.Format("{0}\n{1}", xml, str));
#endif
			return str;
		}
		catch (System.Exception ex)
		{
			Debug.LogError("RichXmlAdapt" + ex.Message + "\n" + xml);
			return xml;
		}
	}

	public static string RemoveP(string xml)
	{
		var pre = "<root>";
		var post = "</root>";
		var doc = XDocument.Parse(pre + xml + post);
		while (true)
		{
			var p = doc.Descendants("p").FirstOrDefault();
			if (p == null)
				break;
			p.ReplaceWith(p.Nodes());
		}
		var str = doc.ToString(SaveOptions.DisableFormatting);
		str = str.Substring(pre.Length, str.Length - pre.Length - post.Length);
		return str;
	}

	/// <summary>
	/// 修复xml格式。
	/// 属性加上双引号，"/>"前面加上空白
	/// </summary>
	/// <param name="poorXml"></param>
	/// <returns></returns>
	public static string XmlRepair(string poorXml)
	{
		if (string.IsNullOrEmpty(poorXml))
			return poorXml;
		var okend = RegexElementStop.Replace(poorXml, @" />");
		var xml = RegexElementHead.Replace(okend, mb =>
		{
			return RegexAttribute.Replace(mb.Value, ma =>
			{
				var k = ma.Groups["k"].Value;
				var v = ma.Groups["v"].Value;
				if (v.Length >= 2)
				{
					var h = v.First();
					var t = v.Last();
					if ((h == '\'' && t == '\'') ||
						(h == '‘' && t == '’') ||
						(h == '’' && t == '’') ||
						(h == '“' && t == '”') ||
						(h == '”' && t == '”'))
					{
						v = v.Substring(1, v.Length - 2);
					}
				}

				return k + "=\"" + v + "\"";
			});
		});
		return xml;
	}
	/// <summary>
	/// 匹配"/>"前有0个或多个空白
	/// </summary>
	private static readonly Regex RegexElementStop = new Regex(@"((?<=\S)|(\s+))/>");
	/// <summary>
	/// 匹配"<...>"片段，随后在此范围内进行属性搜索
	/// </summary>
	private static readonly Regex RegexElementHead = new Regex(@"\<[^<>]*=[^<>]*\>");
	/// <summary>
	/// 匹配"aa=bbb", "aa = bbb"
	/// </summary>
	private static readonly Regex RegexAttribute = new Regex(@"(?<k>\w+)\s*=\s*(?<v>[^ \t\"">]+)");

	private static XElement GetGoToNpcElement(XElement e)
	{
		string tempid = (string)e.AttributeValue("id");
		int id = 0;
		int.TryParse(tempid, out id);
        if (id == 0)
        {
            return null;
        }
        var npc = GameTableManager.Instance.GetTableItem<table.NpcDataBase>((uint)id);//Table.Query<table.NpcDataBase>().FirstOrDefault(i => i.dwID == (uint)id);
		if (npc == null)
			return null;

		var name = npc.strName;
		return new XElement("color", new XAttribute("value", "green"), name);
	}

    private static XElement GetHandleItem(XElement e)
    {
        string tempid = (string)e.AttributeValue("itemid");
        int id = 0;
        int.TryParse(tempid, out id);
        if (id == 0)
        {
            return null;
        }
        var item = GameTableManager.Instance.GetTableItem<table.ItemDataBase>((uint)id);
        if (item == null)
            return null;

        var name = item.itemName;
        return new XElement("color", new XAttribute("value", "green"), name);
    }

	private static XElement GetGoToMapElement(XElement e)
	{
		//var mapid =  System.Convert.ToInt32(e.AttributeValue("mapid"));
        //to do zhudianyu 临时修改
        //var mapInfo = UserData.MapList.FirstOrDefault(i => i.id == mapid);
      
        //if (mapInfo == null)
        //    return null;
        //return new XElement("color", new XAttribute("value", "green"), mapInfo.name);
        string strMapPos = string.Format("({0})",e.AttributeValue("pos"));
        return new XElement("color", new XAttribute("value", "green"), strMapPos);
	}
}
