using UnityEngine;

public class Parametrs : ScriptableObject
{
    public static float ASBackgroundV = 1;
    public static float ASEffectsV = 1;
    public static bool ASAudioMute;

    public static float[] LVLs = new float[] {
    Mathf.Clamp(10f, 10f, 20f),
    Mathf.Clamp(15f, 15f, 25f),
    Mathf.Clamp(20f, 20f, 30f)
    };
    public static float LVL = LVLs[0];
    public static int[] NUMBERs_LVL = new int[] { 0, 1, 2 };
    public static int NUMBER_LVL = NUMBERs_LVL[0];

    public static float HIGHSCORE = 0;

}