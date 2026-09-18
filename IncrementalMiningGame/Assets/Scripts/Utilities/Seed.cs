using System;

public static class Seed
{
    private static Random rand;

    public static int SEED;

    public static int GenerateSeed()
    {
        Random tempRand = new Random();
        SEED = tempRand.Next(int.MinValue, int.MaxValue);
        rand = new Random(SEED);

        return SEED;
    }

    public static void PickSeed(int seed)
    {
        rand = new Random(seed);
    }
    public static int RandomINT(int a, int b)
    {
        return rand.Next(a, b);
    }

    public static float RandomFLOAT(int a, int b)
    {
        return (float)(rand.NextDouble() * (b - a) + a);
    }
}
