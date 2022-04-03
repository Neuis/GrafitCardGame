using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameOverView : MonoBehaviour
{
    [SerializeField] private StatsView statsView;

    public void OnRestartClick()
    {
        GameSceneManager.instance.OpenGameScene();
    }

    public void OnEnable()
    {
        statsView.UpdateFightStats();
    }
}