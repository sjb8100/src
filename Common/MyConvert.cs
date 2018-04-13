using System;
using System.Runtime.InteropServices;
using System.Text;
using System.Xml;
using System.Text.RegularExpressions;
using System.Linq;

	/// <summary>
	/// 定制的数据转换器
	/// </summary>
public static class MyConvert
{
    /// <summary>
    /// 将字符串转换为对应的字符数组
    /// 字符数组的末尾会自动追加一个"\0"字符
    /// </summary>
    /// <param name="msg">要转换的字符串</param>
    /// <param name="encoding">转换所用的编码(默认为UTF8)</param>
    /// <returns></returns>
    private static byte[] ToBytes(string msg, System.Text.Encoding encoding, bool EndOfZero)
    {
        byte[] data = encoding.GetBytes(msg ?? string.Empty);
        if (!EndOfZero)
            return data;
        byte[] ans = new byte[data.Length + 1];
        System.Buffer.BlockCopy(data, 0, ans, 0, data.Length);
        ans[ans.Length - 1] = 0;
        return ans;
    }

    public static byte[] ToBytes(string msg)
    {
        return ToBytes(msg, System.Text.Encoding.GetEncoding("GB2312"), true);
    }
    /// <summary>
    /// 将字符串转换为定长的字符数组.(若不足则补\0,若超过则截断)
    /// </summary>
    /// <param name="msg">要转换的字符串</param>
    /// <param name="fixLength">定长</param>
    /// <param name="encoding">转换采用的编码(默认为UTF8)</param>
    /// <returns></returns>
    public static byte[] ToBytes(string msg, int fixLength)
    {
        byte[] source = ToBytes(msg);
        byte[] data = new byte[fixLength];
        int len = Math.Min(data.Length, source.Length);
        for (int i = 0; i < len; i++)
        {
            data[i] = source[i];
        }
        return data;
    }

    public static string ToString(byte[] data)
    {
        return ToString(data, System.Text.Encoding.GetEncoding("GB2312"));
    }

    /// <summary>
    /// 将字节数组转换为字符串
    /// </summary>
    /// <param name="data">要转换的字节数组</param>
    /// <param name="encoding">转换所采用的编码</param>
    /// <returns></returns>
    private static string ToString(byte[] data, System.Text.Encoding encoding)
    {
        return encoding.GetString(data).TrimEnd('\0');
    }

    public static string ToIPString(uint ip)
    {
        var bytes = BitConverter.IsLittleEndian ? BitConverter.GetBytes(ip).Reverse().ToArray() : BitConverter.GetBytes(ip);
        return string.Format("{0}.{1}.{2}.{3}", bytes[0], bytes[1], bytes[2], bytes[3]);
    }

    /// <summary>
    /// 根据脸谱判断角色性别
    /// </summary>
    /// <param name="face"></param>
    /// <returns></returns>
    //public static enmCharSex FaceToSex(byte face)
    //{
    //    return face < 100 ? enmCharSex.MALE : enmCharSex.FEMALE;
    //}
}
