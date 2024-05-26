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
        // Подписка на событие
        YandexGame.ReviewSentEvent += OnReviewSent;
    }

    void OnDisable()
    {
        // Отписка от события
        YandexGame.ReviewSentEvent -= OnReviewSent;
    }

    // Метод-обработчик события
    void OnReviewSent(bool sent)
    {
        if (sent)
        {
            Debug.Log("Пользователь оставил отзыв.");
            // Ваш код для обработки оставленного отзыва
            Smile();
        }
        else
        {
            Debug.Log("Пользователь закрыл окно без оставления отзыва.");
            // Ваш код для обработки закрытого окна без отзыва
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
