using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class StatsView : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI difficulty;
    [SerializeField] private TextMeshProUGUI score;

    private void Start()
    {
        if (Fight.instance == null) return;
        Fight.instance.OnDifficultyChanged += UpdateDifficulty;
        Fight.instance.OnScoreChanged += UpdateScore;
    }

    public void UpdateFightStats()
    {
        UpdateScore(Fight.instance.CurrentScore);
        UpdateDifficulty(Fight.instance.CurrentDifficulty);
    }

    public void UpdateScore(int newScore)
    {
        score.text = newScore.ToString();
    }

    public void UpdateDifficulty(int newDifficulty)
    {
        difficulty.text = newDifficulty.ToString();
    }
}