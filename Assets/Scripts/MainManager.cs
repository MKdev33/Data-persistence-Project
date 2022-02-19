using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.IO;

public class MainManager : MonoBehaviour
{
    public Brick BrickPrefab;
    public int LineCount = 6;
    public Rigidbody Ball;

    public Text ScoreText;
    public GameObject GameOverText;

    private int BestScore;
    public Text BestScoreText;
    private string setPlayerName;
    private string BestScoreName;

    private bool m_Started = false;
    private int m_Points;

    private bool m_GameOver = false;


    // Start is called before the first frame update
    void Start()
    {
        const float step = 0.6f;
        int perLine = Mathf.FloorToInt(4.0f / step);

        int[] pointCountArray = new [] {1,1,2,2,5,5};
        for (int i = 0; i < LineCount; ++i)
        {
            for (int x = 0; x < perLine; ++x)
            {
                Vector3 position = new Vector3(-1.5f + step * x, 2.5f + i * 0.3f, 0);
                var brick = Instantiate(BrickPrefab, position, Quaternion.identity);
                brick.PointValue = pointCountArray[i];
                brick.onDestroyed.AddListener(AddPoint);
            }
        }
    }

    void Awake()
    {
      setPlayerName = UIManager.Instance.playerName;
      ScoreText.text = $"{setPlayerName} - Score : {m_Points}";
      LoadBestScore();
      BestScoreText.text = $"Best Score : {BestScore} - {BestScoreName}";
    }

    private void Update()
    {
        if (!m_Started)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                m_Started = true;
                float randomDirection = Random.Range(-1.0f, 1.0f);
                Vector3 forceDir = new Vector3(randomDirection, 1, 0);
                forceDir.Normalize();

                Ball.transform.SetParent(null);
                Ball.AddForce(forceDir * 2.0f, ForceMode.VelocityChange);
            }
        }
        else if (m_GameOver)
        {
            SetBestScore();
            BestScoreText.text = $"Best Score : {BestScore} - {BestScoreName}";
            if (Input.GetKeyDown(KeyCode.Space))
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            }
        }
    }

    void AddPoint(int point)
    {
        m_Points += point;
        ScoreText.text = $"{setPlayerName} - Score : {m_Points}";
    }

    void SetBestScore()
    {
      if(m_Points >= BestScore)
      {
        BestScore = m_Points;
        BestScoreName = setPlayerName;
        SaveBestScore();
      }
    }

    public void GameOver()
    {
        m_GameOver = true;
        GameOverText.SetActive(true);
    }

    [System.Serializable]
    class SaveData
    {
      public int BestScore;
      public string BestScoreName;
    }

    public void SaveBestScore()
    {
        SaveData data = new SaveData();
        data.BestScore = BestScore;
        data.BestScoreName = BestScoreName;

        string json = JsonUtility.ToJson(data);

        File.WriteAllText(Application.persistentDataPath + "/savefile.json", json);
    }

    public void LoadBestScore()
    {
      string path = Application.persistentDataPath + "/savefile.json";
      if (File.Exists(path))
      {
        string json = File.ReadAllText(path);
        SaveData data = JsonUtility.FromJson<SaveData>(json);

        BestScore = data.BestScore;
        BestScoreName = data.BestScoreName;
      }
    }
}
