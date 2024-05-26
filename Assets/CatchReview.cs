using UnityEngine;
using YG;

public class CatchReview : MonoBehaviour
{
    [SerializeField] private GameObject writeRewButton;
    [SerializeField] private GameObject smileFace;
    [SerializeField] private GameObject sadFace;
    private float timer;

    private void Update()
    {
        if (timer > 0) timer -= Time.deltaTime;
        else
        {
            smileFace.SetActive(false); 
            sadFace.SetActive(false);
        }
    }

    void OnEnable()
    {
        // �������� �� �������
        YandexGame.ReviewSentEvent += OnReviewSent;
    }

    void OnDisable()
    {
        // ������� �� �������
        YandexGame.ReviewSentEvent -= OnReviewSent;
    }

    // �����-���������� �������
    void OnReviewSent(bool sent)
    {
        if (sent)
        {
            Debug.Log("������������ ������� �����.");
            // ��� ��� ��� ��������� ������������ ������
            Smile();
        }
        else
        {
            Debug.Log("������������ ������ ���� ��� ���������� ������.");
            // ��� ��� ��� ��������� ��������� ���� ��� ������
            Sad();
            writeRewButton.SetActive(false);
        }
    }

    private void Smile()
    {
        timer = 3f;
        smileFace.SetActive(true);
    }
    private void Sad()
    {
        timer = 3f;
        sadFace.SetActive(true);
    }
}
