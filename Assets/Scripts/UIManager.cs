using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.IO;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{

    public static UIManager Instance;
    public InputField playerInputField;
    public string playerName;

    // Start is called before the first frame update
    void Start()
    {
      if (Instance != null)
      {
        Destroy(gameObject);
      }

      Instance = this;
      DontDestroyOnLoad(gameObject);
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void SetPlayerName()
    {
      playerName = playerInputField.text;
    }


    public void StartNew()
    {
      SceneManager.LoadScene(1);
    }

}
