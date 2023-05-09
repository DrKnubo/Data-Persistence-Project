using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
#if   UNITY_EDITOR
using UnityEditor;
using System.IO;

#endif

public class DataManager : MonoBehaviour
{
    [System.Serializable]
    public class GameData
    {
        public string playerName;
        public int highScore;

        public GameData()
        {
            playerName = "";
            highScore = 0;
        }
    }
    public TMP_InputField m_Text;

    private static DataManager m_Instance;
    private GameData m_LoadedGameData;

    public string ActualPlayerName { get; set; } = "";
    public GameData PreviousSessionData { get { return m_LoadedGameData; } }

    private void Awake()
    {
        if (m_Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        m_Instance = this;
        DontDestroyOnLoad(gameObject);
    }
    // Start is called before the first frame update
    void Start()
    {

        LoadSessionFromFile();
    }

    public static DataManager Instance
    {
        get
        {
            if (m_Instance == null)
                Debug.Log("No Instance awake");

            return m_Instance;
        }
    }

    public static bool HasInstance
    {
        get { return m_Instance != null; }
    }
    public void StartButton()
    {
        SceneManager.LoadScene(1);
        ActualPlayerName = m_Text.text;
    }
    public void ExitButton()
    {
#if UNITY_EDITOR
        EditorApplication.ExitPlaymode();
#else
        Application.Quit();
#endif
    }
    public void SaveSessionToFile(int score)
    {
        m_LoadedGameData.playerName = ActualPlayerName;
        m_LoadedGameData.highScore = score;

        string jsonstring = JsonUtility.ToJson(m_LoadedGameData);

        File.WriteAllText(Application.persistentDataPath + "SessionData.json", jsonstring);
    }
    public void LoadSessionFromFile()
    {

        m_LoadedGameData = new GameData();
        string path = Application.persistentDataPath + "SessionData.json";
        if (File.Exists(path))
        {
            string jsonString = File.ReadAllText(path);
            if (!string.IsNullOrEmpty(jsonString))
                m_LoadedGameData = JsonUtility.FromJson<GameData>(jsonString);
        }
        if (!string.IsNullOrEmpty(m_LoadedGameData.playerName))
            m_Text.text = m_LoadedGameData.playerName;
    }
}