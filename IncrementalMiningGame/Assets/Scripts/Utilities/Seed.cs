using System;

public static class Seed
{
    private static Random random;

    private static int SEED;

    public static int GenerateSeed()
    {
        Random tempRandom = new Random();
        SEED = tempRandom.Next(int.MinValue, int.MaxValue);
        random = new Random(SEED);

        return SEED;
    }

    public static void PickSeed(int seed)
    {
        random = new Random(seed);
    }

    public static int RandomINT(int min, int max)
    {
        return random.Next(min, max);
    }

    public static float RandomFLOAT(float min, float max)
    {
        return (float)(random.NextDouble() * (max - min) + min);
    }
}
