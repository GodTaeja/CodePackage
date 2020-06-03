using System.Collections.Generic;

public static class CfgTool 
{
    public static List<int> ParseInt(string str)
    {
        var arr = str.Split(',');

        var ret = new List<int>(arr.Length);
        for (int i = 0; i < arr.Length; i++)
        {
            ret.Add(int.Parse(arr[i]));
        }

        return ret;
    }
    public static List<float> ParseFloat(string str)
    {
        var arr = str.Split(',');

        var ret = new List<float>(arr.Length);
        for (int i = 0; i < arr.Length; i++)
        {
            ret.Add(float.Parse(arr[i]));
        }

        return ret;
    }




}
