using UnityEngine;

public class MobileInputManager : MonoBehaviour
{
    [SerializeField] private CanvasGroup mobileInputContainer;
    public bool testInIspector = false;
    private bool activeContainer;
    private bool activeFireContainer = false;
    private bool mobile = false;
    private int firstCheckMobile = 0;

    // Cameras
    [SerializeField] private GameObject desktopCam;
    [SerializeField] private GameObject mobileCam;

    public void ActiveMobileContainer()
    {
        activeContainer = !activeContainer;
        if (IsMobileDevice())
        {
            mobileInputContainer.alpha = activeContainer ? 1 : 0;
            mobileInputContainer.blocksRaycasts = activeContainer ? true : false;
        }
    }


    private void Start()
    {
        // Вызывает JavaScript для проверки сенсорности устройства
        Application.ExternalEval("checkForTouchDevice()");

        ActiveMobileContainer();
    }

#if !UNITY_EDITOR && UNITY_WEBGL
    [System.Runtime.InteropServices.DllImport("__Internal")]
    private static extern bool IsMobile();

#endif
    public bool IsMobileDevice()
    {
        if (firstCheckMobile == 1)
            return mobile;
        firstCheckMobile = 1;


        var isMobile = false;

#if !UNITY_EDITOR && UNITY_WEBGL
        isMobile = IsMobile();
#endif


        if (isMobile)
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            mobile = true;
            RCC_SceneManager.SetController(1);
            PlayerPrefs.SetInt("isMobile", 1);

            mobileCam.SetActive(true); 
            desktopCam.SetActive(false);
            return true;
        }
        else
        {
            if (testInIspector)
            {
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
                mobile = true;
                RCC_SceneManager.SetController(1);
                PlayerPrefs.SetInt("isMobile", 1);

                mobileCam.SetActive(true);
                desktopCam.SetActive(false);
                return true;
            }

            RCC_SceneManager.SetController(0);
            PlayerPrefs.SetInt("isMobile", 0);
            mobile = testInIspector;

            mobileCam.SetActive(false);
            desktopCam.SetActive(true);
            return testInIspector;
        }
    }
}
