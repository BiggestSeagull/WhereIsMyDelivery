using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class Ui_City : MonoBehaviour
{
    [SerializeField] private Delivery_OrderManager OrderManager;
    [SerializeField] private Delivery_Immersive Immersive;
    [SerializeField] private Delivery_Stats Stats;
    [SerializeField] private RCC_CarControllerV3 RCC;
    [SerializeField] private MusicManager MusicManager;
    [SerializeField] private TowManager TowManager;

    [Space(10)]

    [SerializeField] private TMP_Text coinsText;

    [Space(15)]

    // Menu
    [SerializeField] private GameObject menuBlock;      // Ui element itself
    [SerializeField] private Button menuButton;         // Button on the screen, enables menu block
    [SerializeField] private Button closeMenuButton;    // Disable menu
    [SerializeField] private Button musicOnButton;      // Menu element, on/off music
    [SerializeField] private Button musicOffButton;     
    [SerializeField] private Button towButton;          // Tow truck
    [SerializeField] private Button menuGoGarageButton; // Menu element, enable sub-menu

    [Space(15)]

    // Sub-menu
    [SerializeField] private GameObject arrowGarageBlock;   // Ui element itself
    [SerializeField] private Button yesArrowGarageButton;   // Point arrow to garage
    [SerializeField] private Button noArrowGarageButton;    // Disable sub-menu

    [Space(15)]

    // Garage trigger
    public GameObject triggerGoGarageBlock; // Changed SetActive from Customer.cs
    [SerializeField] private Button triggerYesGarageButton;
    [SerializeField] private Button triggerNoGarageButton;

    [Space(15)]

    // Cursor hiding
    private bool isCursorLocked;
    private bool canlockCursor = true; // being false when ad is showing

    // Start is called before the first frame update
    void Start()
    {
        MusicManager = GameObject.FindWithTag("MusicPlayer").GetComponent<MusicManager>();
        if (PlayerPrefs.GetInt("music", 1) == 0)
        {
            musicOnButton.gameObject.SetActive(false);
            musicOffButton.gameObject.SetActive(true);
        }

        ReloadCoins();

        // Menu
        menuButton.onClick.AddListener(delegate { menuBlock.SetActive(true); });
        musicOnButton.onClick.AddListener(delegate { MusicSwitch(0); });
        musicOffButton.onClick.AddListener(delegate { MusicSwitch(1); });
        towButton.onClick.AddListener(delegate { GoTow(); });
        menuGoGarageButton.onClick.AddListener(delegate { arrowGarageBlock.SetActive(true); });
        closeMenuButton.onClick.AddListener(delegate { menuBlock.SetActive(false); });

        // Sub-menu options
        yesArrowGarageButton.onClick.AddListener(delegate { OptionToGarage(1); });
        noArrowGarageButton.onClick.AddListener(delegate { OptionToGarage(0); });

        // Garage trigger 
        triggerYesGarageButton.onClick.AddListener(delegate { ToGarageScene(); });
        triggerNoGarageButton.onClick.AddListener(delegate { OptionToGarage(0); });
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.C) && canlockCursor)
        {
            SwitchCursor();
        }
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            // Unlocking cursor, web override cuz default not showing cursor, but unlocks it
            isCursorLocked = false;

            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
    }

    private void SwitchCursor()
    {
        if (isCursorLocked)
        {
            // Unlocking cursor
            isCursorLocked = false;

            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
        else
        {
            isCursorLocked = true;

            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
    }
    // Controlled in YandexGame script
    public void CanLockCursor()
    {
        canlockCursor = true;
    }
    public void CannotLockCursor()
    {
        canlockCursor = false;
    }
    public void CursorLockAfterAd()
    {
        isCursorLocked = true;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    // Behavior depend on pressed button in Garage sub-menu of pause
    private void OptionToGarage(int yesNo)
    {
        if(yesNo == 0)
        {
            OrderManager.isGoGarage = false;
            triggerGoGarageBlock.SetActive(false);
        }
        else
        {
            OrderManager.isGoGarage = true;
            OrderManager.ArrowToGarage();
        }

        arrowGarageBlock.SetActive(false);
        menuBlock.SetActive(false);
    }

    // Button in menu
    private void MusicSwitch(int onOff)
    {
        if (onOff == 0)
        {
            MusicManager.PauseMusic();
            PlayerPrefs.SetInt("music", 0);
            musicOnButton.gameObject.SetActive(false);
            musicOffButton.gameObject.SetActive(true);
        }
        else
        {
            MusicManager.PlayMusic();
            PlayerPrefs.SetInt("music", 1);
            musicOnButton.gameObject.SetActive(true);
            musicOffButton.gameObject.SetActive(false);
        }
    }

    // Tow the player
    private void GoTow()
    {
        TowManager.TowPlayer();
    }

    // Entered trigger and pressed yes
    private void ToGarageScene()
    {
        PlayerPrefs.SetInt("transFromMainScene", 1);
        SceneManager.LoadScene(0);
    }

    // Updating coins
    public void ReloadCoins()
    {
        coinsText.text = YG_Saves.LoadCoins().ToString();
    }

    public void CollectCoins()
    {
        Immersive.CollectCoins();
    }
}
