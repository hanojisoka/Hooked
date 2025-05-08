using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using NaughtyAttributes;
using System.IO;
using UnityEngine.SceneManagement;

public class DataPersistenceManager : SingletonMB<DataPersistenceManager>
{
    [Header("File Storage Config")]
    [SerializeField] private string fileName;

    private GameData gameData;
    private List<IDataPersistence> dataPersistenceObjects;
    private FileDataHandler dataHandler;

    public void NewGame()
    {
        this.gameData = new();

    }

    void Awake()
    {
        base.Awake();
        
        this.dataHandler = new FileDataHandler(Application.persistentDataPath, fileName);
        this.dataPersistenceObjects = FindAllDataPersistenceObjects();

        // Load data on start
        LoadGame();
    }

    private void Start()
    {
        
    }

    public void LoadGame()
    {
        // Load any saved data from a file using the data handler
        this.gameData = dataHandler.Load();

        // if no data can be loaded, NewGame
        if (this.gameData == null)
        {
            Debug.Log("No data was found. Initializing data to defaults.");
            NewGame();
        }

        // Load data to all other scripts that need it
        foreach(IDataPersistence dataPersistenceObj in dataPersistenceObjects)
        {
            dataPersistenceObj.LoadData(gameData);
        }
        
        Debug.Log($"Load Game \nLevel: {gameData.Level} \nFishCount: {gameData.FishCount}");
    }
    [Button]
    public void SaveGame()
    {
        // Pass the data to other scripts so they can update it
        foreach(IDataPersistence dataPersistenceObj in dataPersistenceObjects)
        {
            dataPersistenceObj.SaveData(ref gameData);
        }
        Debug.Log($"{dataPersistenceObjects.Count} COUNT");
        Debug.Log($"Save Game \nLevel: {gameData.Level} \nFishCount: {gameData.FishCount}");

        // Save that data to a file using the data handler
        dataHandler.Save(gameData);

    }

    private void OnApplicationQuit()
    {
        SaveGame();
    }

    private List<IDataPersistence> FindAllDataPersistenceObjects()
    {
        IEnumerable<IDataPersistence> dataPersistenceObjects = FindObjectsOfType<MonoBehaviour>().OfType<IDataPersistence>();
        return new List<IDataPersistence>(dataPersistenceObjects);
    }

    [Button]
    public void DeleteGameData()
    {
        string filePath = Path.Combine(Application.persistentDataPath, fileName);

        if (File.Exists(filePath))
        {
            File.Delete(filePath);
            Debug.Log("Game data deleted successfully.");
            NewGame(); // Reset game data to default
            SceneManager.LoadScene(0);
        }
        else
        {
            Debug.Log("No saved data file found to delete.");
        }
    }
}
