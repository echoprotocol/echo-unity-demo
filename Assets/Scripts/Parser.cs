public static class Parser
{
    private const int BLOCK_SIZE = 20;

    public static void Serialize(int[] array, ref string res)
    {
        res += BLOCK_SIZE.ToString("X32");
        res += array.Length.ToString("X32");
        
        for(int i = 0; i < array.Length; i++)
        {
            res += array[i].ToString("X32");
        }
    }
}
