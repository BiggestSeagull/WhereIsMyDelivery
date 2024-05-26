using UnityEngine;

public class WheelLamps : MonoBehaviour
{
    // 0 object if off, 1 is on
    [SerializeField] private GameObject[] lamps1;
    [SerializeField] private GameObject[] lamps2;
    public float defaultTimer = 1.7f;
    private float timer;

private void Update()
{
    timer += Time.deltaTime;
    if (timer >= defaultTimer)
    {
        timer = 0f;
        SwitchLights();
    }
}

private void SwitchLights()
{
    bool isOn = lamps1[0].activeSelf;
    lamps1[0].SetActive(!isOn);
    lamps1[1].SetActive(isOn);
    lamps2[0].SetActive(isOn);
    lamps2[1].SetActive(!isOn);
}
}
