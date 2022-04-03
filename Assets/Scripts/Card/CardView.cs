using DG.Tweening;
using UnityEngine;
using TMPro;

public class CardView : MonoBehaviour
{
    [SerializeField] private GameObject greenOverlay;
    [SerializeField] private GameObject redOverlay;
    [SerializeField] private GameObject heroOverlay;
    [SerializeField] private TextMeshProUGUI cardText;

    private Transform _thisTransform;
    private GameObject _thisGameObject;

    private void Awake()
    {
        _thisTransform = transform;
        _thisGameObject = gameObject;
    }

    public void UpdateCardType(int newType)
    {
        greenOverlay.SetActive(false);
        redOverlay.SetActive(false);
        heroOverlay.SetActive(false);
        switch (newType)
        {
            case Card.CARD_TYPE_GREEN:
                greenOverlay.SetActive(true);
                break;

            case Card.CARD_TYPE_RED:
                redOverlay.SetActive(true);
                break;

            default:
                heroOverlay.SetActive(true);
                break;
        }
    }

    public void UpdateCardValue(int newValue)
    {
        cardText.text = newValue.ToString();
    }

    public void MakeSmallWithAnimation()
    {
        _thisTransform.DOScale(Vector2.zero, FightView.ANIMATION_DURATION);
    }

    public void MakeBigWithAnimation()
    {
        _thisTransform.DOScale(Vector2.one, FightView.ANIMATION_DURATION);
    }

    public void MakeSmall()
    {
        _thisTransform.localScale = Vector2.zero;
    }

    public void MakeBig()
    {
        _thisTransform.localScale = Vector2.one;
    }

    public void TeleportToPosition(Vector2 newPos)
    {
        _thisTransform.position = newPos;
    }

    public void ShowCard()
    {
        _thisGameObject.SetActive(true);
    }

    public void HideCard()
    {
        _thisGameObject.SetActive(false);
    }
}