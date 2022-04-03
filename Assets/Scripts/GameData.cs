using System;

[Serializable]
public class GameData
{
    private int _maxDifficulty = 0;
    private int _maxScore = 0;

    private int _lastGameScore;

    public int LastGameScore
    {
        get => _lastGameScore;
        set
        {
            _lastGameScore = value;
            if (_lastGameScore > _maxScore)
            {
                _maxScore = _lastGameScore;
            }
        }
    }

    private int _lastGameDifficulty;

    public int LastGameDifficulty
    {
        get => _lastGameDifficulty;
        set
        {
            _lastGameDifficulty = value;
            if (_lastGameDifficulty > _maxDifficulty)
            {
                _maxDifficulty = _lastGameDifficulty;
            }
        }
    }

    public GameData()
    {
        LastGameScore = 0;
        LastGameDifficulty = 0;
    }
}