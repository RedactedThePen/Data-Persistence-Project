using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using System.IO;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class StartScreenUIManager : MonoBehaviour
{
    public GameObject nameInputField;
    public string playerName;
    public static StartScreenUIManager Instance;
    public int highScore;
    public string highScoreName;
    public TextMeshProUGUI startScreenScoreText;
    // Start is called before the first frame update
    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
        LoadHighScore();
        startScreenScoreText.text = ("Best Score: " + highScoreName + ": " + highScore);
    }

    // Update is called once per frame
    void Update()
    {

    }
    public void Exit()
    {
        SaveHighScore();

#if UNITY_EDITOR
        EditorApplication.ExitPlaymode();
#else
        Application.Quit();
#endif
    }
    public void StartNew()
    {
        SceneManager.LoadScene(1);
    }

    public void GetName(string input)
    {
        playerName = input;
    }

    public void changeHighScore(int score)
    {
        if (score > highScore)
        {
            highScore = score;
            highScoreName = playerName;
        }
    }
    [System.Serializable]
    class SaveScore
    {
        public string highScoreName;
        public int highScore;
    }

    public void SaveHighScore()
    {
        SaveScore score = new SaveScore();
        score.highScore = highScore;
        score.highScoreName = highScoreName;

        string json = JsonUtility.ToJson(score);

        File.WriteAllText(Application.persistentDataPath + "/savefile.json", json);
    }
    public void LoadHighScore()
    {
        string path = Application.persistentDataPath + "/savefile.json";
        if (File.Exists(path))
        {
            string json = File.ReadAllText(path);
            SaveScore data = JsonUtility.FromJson<SaveScore>(json);

            highScore = data.highScore;
            highScoreName = data.highScoreName;
        }
    }
}
