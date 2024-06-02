using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class GuideForNewbie : MonoBehaviour
{
    private GameObject container;

    [SerializeField] private GameObject[] pages;
    private int currentPage = 0; // First page is 0

    [SerializeField] private Button arrowLeft;
    [SerializeField] private Button arrowRight;

    private void Start()
    {
        // Check if first time in game, local save
        if (PlayerPrefs.GetString("isFirstLaunch", "true") == "true")
        {
            PlayerPrefs.SetString("isFirstLaunch", "false");

            container = gameObject.transform.GetChild(0).gameObject;
            container.SetActive(true);
            pages[0].SetActive(true);

            StartCoroutine(UnlockCursorOnSecondFrame()); 
        }
        else
        {
            Ui_City.LockCursor();
            Destroy(gameObject);
        }

        arrowLeft.onClick.AddListener(delegate { PreviousPage(); });
        arrowRight.onClick.AddListener(delegate { NextPage(); });
    }

    private void NextPage()
    {
        if (currentPage < pages.Length -1)
        {
            pages[currentPage].SetActive(false);
            currentPage++;
            pages[currentPage].SetActive(true);
        }
        else if (currentPage >= pages.Length -1)
        {
            Ui_City.LockCursor();
            Destroy(gameObject);
        }
    }
    private void PreviousPage()
    {
        switch (currentPage)
        {
            case > 0:
                pages[currentPage].SetActive(false);
                currentPage--;
                pages[currentPage].SetActive(true);
                break;
            case <= 0:
                // Nothing to do
                break;
        }
    }

    IEnumerator UnlockCursorOnSecondFrame()
    {
        yield return new WaitForEndOfFrame();

        Ui_City.UnlockCursor();
    }
}
