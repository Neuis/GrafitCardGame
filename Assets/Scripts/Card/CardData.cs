public class CardData
{
    private int _cardType;
    private int _cardValue;

    public CardData(int cType, int cValue)
    {
        _cardType = cType;
        SetCardValue(cValue);
    }

    public int GetCardType()
    {
        return _cardType;
    }

    public int GetCardValue()
    {
        return _cardValue;
    }

    public void SetCardValue(int cValue)
    {
        _cardValue = cValue;
    }
}