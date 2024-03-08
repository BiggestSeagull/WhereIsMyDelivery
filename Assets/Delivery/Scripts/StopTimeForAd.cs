using UnityEngine;

public class StopTimeForAd : MonoBehaviour
{
    public void StopTime()
    {
        Time.timeScale = 0f;
    }
    public void NormalTime()
    {
        Time.timeScale = 1.0f;
    }
}
