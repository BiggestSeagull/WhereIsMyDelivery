using UnityEngine;
using DG.Tweening;

public class HoodControl : MonoBehaviour
{
    public Transform hood;

    // For buttons
    public void Hood_open()
    {
        Vector3 rotateTo = new(90, 0f, 0f);
        hood.DOLocalRotate(rotateTo, 1f);
    }
    public void Hood_close()
    {
        Vector3 rotateTo = new(0f, 0f, 0f);
        hood.DOLocalRotate(rotateTo, 1f);
    }
}
