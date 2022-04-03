using System;
using System.Collections;
using System.Collections.Generic;
using System.Dynamic;
using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public class MainGameController : MonoBehaviour
{
    private const string SAVE_FILE_NAME = "/save.dat";

    [SerializeField] private StatsView menuSceneStats;

    public static MainGameController instance = null;
    public bool chanceModificationEnabled = false;
    
    private GameData _gameData;

    private void Awake()
    {
        CreateInstance();
        LoadUserData();
        OnDataLoaded();
    }

    private void CreateInstance()
    {
        if (instance == null)
            instance = this;
        else
            Debug.Log("You are trying to create instance of GameController but you already have one");
        DontDestroyOnLoad(gameObject);
    }

    private string GetGameDataPath()
    {
        return Application.persistentDataPath + SAVE_FILE_NAME;
    }


    public void SaveUserData(int score, int difficulty)
    {
        _gameData.LastGameDifficulty = difficulty;
        _gameData.LastGameScore = score;

        var gameDataPath = GetGameDataPath();
        var file = File.Exists(gameDataPath) ? File.OpenWrite(gameDataPath) : File.Create(gameDataPath);

        var bf = new BinaryFormatter();
        bf.Serialize(file, _gameData);
        file.Close();
    }

    private void LoadUserData()
    {
        var gameDataPath = GetGameDataPath();
        FileStream file;
        if (File.Exists(gameDataPath))
        {
            file = File.OpenRead(gameDataPath);
        }
        else
        {
            Debug.Log("I can't find game data file with this destination: " + gameDataPath);
            Debug.Log("This is probably new User. We should create new file for him");
            CreateNewGameData();
            return;
        }

        var bf = new BinaryFormatter();
        try
        {
            _gameData = (GameData) bf.Deserialize(file);
        }
        catch
        {
            Debug.LogError("I CAN'T LOAD GAME DATA. I'LL CREATE NEW EMPTY GAME DATA");
            file.Close();
            CreateNewGameData();
            return;
        }

        file.Close();
    }

    private void CreateNewGameData()
    {
        _gameData = new GameData();
    }

    private void OnDataLoaded()
    {
        menuSceneStats.gameObject.SetActive(_gameData.LastGameScore > 0);
        if (_gameData.LastGameScore > 0)
        {
            menuSceneStats.UpdateDifficulty(_gameData.LastGameDifficulty);
            menuSceneStats.UpdateScore(_gameData.LastGameScore);
        }
    }

    public void EnableChanceModification()
    {
        chanceModificationEnabled = true;
    }
}