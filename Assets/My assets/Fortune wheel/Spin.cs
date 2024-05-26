using UnityEngine;
using DG.Tweening;

public class Spin : MonoBehaviour
{
    public RectTransform wheel; // Assign the RectTransform of the wheel in the Inspector
    public float spinDuration = 4f; // Duration of the spin
    public int numberOfSections = 8; // Number of sections on the wheel

    public void RotateWheel()
    {
        // Calculate the target rotation angle. We spin several times + the angle to land on a section.
        // Introduce randomness with small random offset
        float baseAngle = 360f / numberOfSections;
        float randomOffset = Random.Range(0f, baseAngle);
        float targetAngle = (360 * Random.Range(3, 6)) + (baseAngle * Random.Range(0, numberOfSections)) + randomOffset;

        // Rotate the wheel using DOTween
        wheel.DORotate(new Vector3(0, 0, -targetAngle), spinDuration, RotateMode.FastBeyond360)
            .SetEase(Ease.OutQuart) // Easing function to simulate realistic slowing down
            .OnComplete(GetWheelResult);
    }

    private void GetWheelResult()
    {
        // Normalize the rotation angle to a value between 0 and 360
        float normalizedAngle = wheel.eulerAngles.z % 360;

        // Adjust the normalized angle by adding the sectionOffset
        normalizedAngle = normalizedAngle % 360;

        // Calculate the section
        float baseAngle = 360f / numberOfSections;
        int section = Mathf.FloorToInt(normalizedAngle / baseAngle);

        // Output the result (optional, for testing purposes)
        Debug.Log("Wheel stopped at section: " + section);
    }
}
