using System;

public static class Sound
{
    public static float baseOfLogarithm = 1.15f;


    public static float VolumeToDb(double volume) {
        return (float)Math.Log(volume,baseOfLogarithm);
    }
    public static float DbToVolume(double db) {
        return (float)Math.Pow(baseOfLogarithm, db);
    }
}
