using UnityEngine;

public static class Parser
{
    private const int BLOCK_SIZE = 32;

    public static void SerializeInts(ref string res, params int[] ints)
    {
        for (int i = 0; i < ints.Length; i++)
        {
            res += ints[i].ToString("X64");
        }
    }

    public static int DeserializeInt(string res)
    {
        return int.Parse(res, System.Globalization.NumberStyles.HexNumber);
    }
}
