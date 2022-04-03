using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Card : MonoBehaviour
{
    public const int CARD_TYPE_GREEN = 0;
    public const int CARD_TYPE_RED = 1;
    public const int CARD_TYPE_USER = 2;

    [SerializeField] private CardView cardView;

    public int CardRow { get; set; }
    public int CardColumn { get; set; }

    private CardData _data;

    public void SetCardData(CardData data)
    {
        _data = data;
        UpdateCardView();
    }

    private void UpdateCardView()
    {
        cardView.UpdateCardType(_data.GetCardType());
        cardView.UpdateCardValue(_data.GetCardValue());
    }

    public CardData GetCardData()
    {
        return _data;
    }

    public CardView GetCardView()
    {
        return cardView;
    }

    public void OnCardClick()
    {
        Fight.instance.OnCardClick(this);
    }
}