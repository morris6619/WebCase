using System;
using System.Text;

public static class StringExtensions
{
    //回傳重複的 "字串" 所組成的字串
    public static string Repeat(this string stringToRepeat, int repeat)
    {
        var builder = new StringBuilder(repeat * stringToRepeat.Length);
        for (int i = 0; i < repeat; i++)
        {
            builder.Append(stringToRepeat);
        }
        return builder.ToString();
    }

    public static string DayToChinese(this String value)
    {
        string[] chinese = { "例", "一", "二", "三", "四", "五", "六", "日", "例前一日", "例後一日" };
        if (string.IsNullOrEmpty(value)) return "";

        char[] cs = value.ToCharArray();
        string chineseDay = "";

        for (int i = 0; i < value.Length; i++)
        {
            chineseDay = chineseDay + chinese[Convert.ToInt32(value.Substring(i, 1))];
        }
        return chineseDay;
    }

    public static string ParseRunDate(this String value)
    {
        string rtnstr = string.Empty, tempstr = string.Empty;

        if (value.Contains("default(run)") && value.Contains("w("))
        {
            tempstr = value.Substring(value.IndexOf("w("));
            int v = value.IndexOf(")");
            tempstr = tempstr.Substring(0, tempstr.IndexOf(")"));
            if (!tempstr.Contains("1")) rtnstr = rtnstr + "1";
            if (!tempstr.Contains("2")) rtnstr = rtnstr + "2";
            if (!tempstr.Contains("3")) rtnstr = rtnstr + "3";
            if (!tempstr.Contains("4")) rtnstr = rtnstr + "4";
            if (!tempstr.Contains("5")) rtnstr = rtnstr + "5";
            if (!tempstr.Contains("6")) rtnstr = rtnstr + "6";
            if (!tempstr.Contains("7")) rtnstr = rtnstr + "7";
            if (value.Contains("holiday"))
            {
                if (!value.Contains("holiday(0)")) rtnstr = rtnstr + "0";
                if (!value.Contains("holiday(1)")) rtnstr = rtnstr + "8";
                if (!value.Contains("holiday(-1)")) rtnstr = rtnstr + "9";
            }
        }
        else if (value.Contains("default(stop)") && value.Contains("w("))
        {
            tempstr = value.Substring(value.IndexOf("w("));
            tempstr = tempstr.Substring(0, tempstr.IndexOf(")"));
            if (tempstr.Contains("1")) rtnstr = rtnstr + "1";
            if (tempstr.Contains("2")) rtnstr = rtnstr + "2";
            if (tempstr.Contains("3")) rtnstr = rtnstr + "3";
            if (tempstr.Contains("4")) rtnstr = rtnstr + "4";
            if (tempstr.Contains("5")) rtnstr = rtnstr + "5";
            if (tempstr.Contains("6")) rtnstr = rtnstr + "6";
            if (tempstr.Contains("7")) rtnstr = rtnstr + "7";
            if (value.Contains("holiday"))
            {
                if (value.Contains("holiday(0)")) rtnstr = rtnstr + "0";
                if (value.Contains("holiday(1)")) rtnstr = rtnstr + "8";
                if (value.Contains("holiday(-1)")) rtnstr = rtnstr + "9";
            }
        }
        else
            rtnstr = "12345670";

        return rtnstr;
    }

    public static string SimpleString(this String value, int length)
    {
        if (string.IsNullOrEmpty(value) || value.Length <= length)
        {
            return value;
        }
        else
        {
            return value.Substring(0, length) + "...";
        }
    }
}