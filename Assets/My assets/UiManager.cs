using UnityEngine;
using UnityEngine.UI;

public class UiManager : MonoBehaviour
{
    [SerializeField] private Spin Spin;

    [SerializeField] private Button spinWheel;

    // Start is called before the first frame update
    void Start()
    {
        spinWheel.onClick.AddListener(delegate { Spin.RotateWheel(); });
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
