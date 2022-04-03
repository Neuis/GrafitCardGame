using System;
using DG.Tweening;
using UnityEngine;
using System.Collections;

public class FightView : MonoBehaviour
{
    public const float ANIMATION_DURATION = 0.3f;

    [SerializeField] private StatsView statsView;
    [SerializeField] private GameObject redCardSpawnChanceContainer;

    private CardView _fakeCV;
    private CardView _userCV;
    private CardView _clickedCV;

    private bool _shouldUseIEnumerator = true;

    private void Start()
    {
        CheckChanceModification();
    }

    private void CheckChanceModification()
    {
        if (MainGameController.instance.chanceModificationEnabled)
        {
            redCardSpawnChanceContainer.SetActive(true);
        }
    }

    public void SetFakeCard(CardView cView)
    {
        _fakeCV = cView;
        _fakeCV.HideCard();
    }

    public void SetUserCard(Card card)
    {
        _userCV = card.GetCardView();
    }

    public void ShowCardsMoveAnimation(Card clickedCard)
    {
        _userCV.MakeSmall();
        _userCV.MakeBigWithAnimation();

        _clickedCV = clickedCard.GetCardView();
        _clickedCV.MakeSmallWithAnimation();

        _fakeCV.TeleportToPosition(_userCV.transform.position);
        _fakeCV.ShowCard();

        if (_shouldUseIEnumerator)
        {
            //если использовать IEnumerator, предпоследний пункт тех. задания
            _fakeCV.transform.DOMove(_clickedCV.transform.position, ANIMATION_DURATION);
            StartCoroutine(OnAnimationCompleteWithIEnumerator());
        }
        else
        {
            //если не использовать IEnumerator, предпоследний пункт тех. задания
            _fakeCV.transform.DOMove(_clickedCV.transform.position, ANIMATION_DURATION).OnComplete(OnAnimationComplete);
        }
    }

    private void OnAnimationComplete()
    {
        Fight.instance.OnCardMoveAnimationComplete();
        _fakeCV.HideCard();
        _clickedCV.MakeBig();
    }

    private IEnumerator OnAnimationCompleteWithIEnumerator()
    {
        yield return new WaitForSeconds(ANIMATION_DURATION);
        Fight.instance.OnCardMoveAnimationComplete();
        _fakeCV.HideCard();
        _clickedCV.MakeBig();
    }
    
    public void UpdateStats()
    {
        statsView.UpdateFightStats();
    }
}