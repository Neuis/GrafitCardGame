using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Random = UnityEngine.Random;

public class Fight : MonoBehaviour
{
    private static int RED_CARD_SPAWN_CHANCE_MAX = 5;
    
    private const int DEFAULT_DIFFICULTY = 1;
    private const byte FIELD_SIZE = 3;
    private const int USER_DEFAULT_HP = 5;
    private const byte USER_CARD_ROW_N_COLUMN = 1;
    private const int MIN_CARD_VALUE = 1;
    private const int STEPS_PER_DIFFICULTY_LEVEL = 10;

    [SerializeField] private TextMeshProUGUI redCardChanceCap;
    [SerializeField] private TextMeshProUGUI currentRedCapChance;

    [SerializeField] private Transform cardsContainer;
    [SerializeField] private FightView fightView;
    [SerializeField] private Card fakeCard;

    public static Fight instance = null;

    public delegate void DifficultyChanger(int newDifficulty);

    public event DifficultyChanger OnDifficultyChanged;

    public delegate void ScoreChanger(int newScore);

    public event ScoreChanger OnScoreChanged;

    private int _currentScore;

    public int CurrentScore
    {
        get => _currentScore;
        set
        {
            _currentScore = value;
            OnScoreChanged?.Invoke(_currentScore);
        }
    }

    private int _currentDifficulty;

    public int CurrentDifficulty
    {
        get => _currentDifficulty;
        set
        {
            _currentDifficulty = value;
            _enemySpawnChance = _currentDifficulty > RED_CARD_SPAWN_CHANCE_MAX
                ? RED_CARD_SPAWN_CHANCE_MAX
                : _currentDifficulty;
            OnDifficultyChanged?.Invoke(_currentDifficulty);
            UpdateViewChances();
        }
    }

    private static bool _shouldStartGame = false;
    private Card _userCard;
    private Card[][] _cards = new Card[FIELD_SIZE][];
    private int _userHP;
    private int _enemySpawnChance;

    private Card _clickedCard;
    private bool _cardsAreMovingNow = false;

    private void Awake()
    {
        FindAllCards();
        CreateInstance();
    }

    private void Start()
    {
        if (_shouldStartGame)
        {
            fightView.SetFakeCard(fakeCard.GetCardView());
            _shouldStartGame = false;
            StartNewGame();
            fightView.UpdateStats();
        }
    }

    private void CreateInstance()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Debug.Log("You are trying to create instance of Fight but you already have one");
        }
    }

    private void FindAllCards()
    {
        var counter = 0;
        for (var i = 0; i < FIELD_SIZE; i++)
        {
            _cards[i] = new Card[FIELD_SIZE];
            for (var j = 0; j < FIELD_SIZE; j++)
            {
                _cards[i][j] = cardsContainer.GetChild(counter).GetComponent<Card>();
                _cards[i][j].CardRow = j;
                _cards[i][j].CardColumn = i;
                counter++;
            }
        }
    }

    private void PrepareCardsForNewGame()
    {
        for (var i = 0; i < FIELD_SIZE; i++)
        {
            for (var j = 0; j < FIELD_SIZE; j++)
            {
                if (i == j && i == USER_CARD_ROW_N_COLUMN)
                    SetUserCardData(_cards[i][j]);
                else
                    SetCardData(_cards[i][j]);
            }
        }
    }

    private void SetUserCardData(Card card)
    {
        _userCard = card;
        var cData = new CardData(Card.CARD_TYPE_USER, _userHP);
        _userCard.SetCardData(cData);
    }

    private void SetCardData(Card card)
    {
        card.SetCardData(CreateNewCardData());
    }

    private CardData CreateNewCardData()
    {
        return new CardData(FindCardType(), FindCardValue());
    }

    private int FindCardType()
    {
        return Random.Range(0, 10) < _enemySpawnChance ? Card.CARD_TYPE_RED : Card.CARD_TYPE_GREEN;
    }

    private int FindCardValue()
    {
        return MIN_CARD_VALUE +
               Random.Range(0, _currentDifficulty + 1); //в задании диапазон был в квадратных скобках по этому +1
    }

    public static void StartGameIfPossible()
    {
        if (instance == null)
            _shouldStartGame = true;
        else
            instance.StartNewGame();
    }

    public void StartNewGame()
    {
        CurrentDifficulty = DEFAULT_DIFFICULTY;
        CurrentScore = 0;
        _userHP = USER_DEFAULT_HP;
        PrepareCardsForNewGame();

        fightView.SetUserCard(_userCard);
    }

    private void IncreaseMoveCounter()
    {
        CurrentScore++;
        if (CurrentScore % STEPS_PER_DIFFICULTY_LEVEL == 0)
        {
            CurrentDifficulty++;
        }
    }

    private void AddUserHp(int hpToAdd)
    {
        _userHP += hpToAdd;
        if (_userHP < 1)
            _userHP = 0;
        _userCard.GetCardData().SetCardValue(_userHP);
    }

    public void OnCardClick(Card clickedCard)
    {
        if (_cardsAreMovingNow) return;
        var cRow = clickedCard.CardRow;
        var cColumn = clickedCard.CardColumn;
        var uRow = _userCard.CardRow;
        var uColumn = _userCard.CardColumn;
        if ((cRow == uRow && Math.Abs(cColumn - uColumn) == 1) ||
            (cColumn == uColumn && Math.Abs(cRow - uRow) == 1))
        {
            _cardsAreMovingNow = true;
            _clickedCard = clickedCard;
            var cardValue = clickedCard.GetCardData().GetCardValue();
            AddUserHp(clickedCard.GetCardData().GetCardType() == Card.CARD_TYPE_GREEN ? cardValue : -cardValue);
            fakeCard.SetCardData(_userCard.GetCardData());
            _userCard.SetCardData(CreateNewCardData());
            fightView.ShowCardsMoveAnimation(clickedCard);
            IncreaseMoveCounter();
        }
    }

    public void OnCardMoveAnimationComplete()
    {
        if (_userHP < 1)
            OnGameEnded();
        else
        {
            _clickedCard.SetCardData(fakeCard.GetCardData());
            _userCard = _clickedCard;
            fightView.SetUserCard(_userCard);
            _cardsAreMovingNow = false;
        }
    }

    private void OnGameEnded()
    {
        MainGameController.instance.SaveUserData(CurrentScore, CurrentDifficulty);
        GameSceneManager.instance.OpenGameOverScene();
    }

    public void IncreaseRedCardSpawnChanceCap()
    {
        RED_CARD_SPAWN_CHANCE_MAX++;
        if (RED_CARD_SPAWN_CHANCE_MAX > 10)
        {
            RED_CARD_SPAWN_CHANCE_MAX = 10;
        }
        
        _enemySpawnChance = _currentDifficulty > RED_CARD_SPAWN_CHANCE_MAX
            ? RED_CARD_SPAWN_CHANCE_MAX
            : _currentDifficulty;

        UpdateViewChances();
    }

    private void UpdateViewChances()
    {
        redCardChanceCap.text = (RED_CARD_SPAWN_CHANCE_MAX*10).ToString();
        currentRedCapChance.text = (_enemySpawnChance*10).ToString();
    }
}