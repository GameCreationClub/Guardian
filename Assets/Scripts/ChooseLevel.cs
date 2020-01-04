using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ChooseLevel : MonoBehaviour
{
    public GameObject buttonPrefab;
    public Transform parent;

    private TextAsset[] levels;
    private TextAsset loadedLevel;

    private void Start()
    {
        levels = Resources.LoadAll<TextAsset>("Levels");

        foreach (TextAsset level in levels)
        {
            Button button = Instantiate(buttonPrefab, parent).GetComponent<Button>();
            button.onClick.AddListener(() => LoadLevel(level));
            button.GetComponentInChildren<Text>().text = level.name;
        }
    }

    private void LevelLoaded(Scene scene, LoadSceneMode loadSceneMode)
    {
        if (scene.name.Equals("Game"))
        {
            LevelGenerator levelGenerator = FindObjectOfType<LevelGenerator>();
            levelGenerator.levelFile = loadedLevel;
            levelGenerator.GenerateLevel();
        }
    }

    public void LoadLevel(TextAsset level)
    {
        loadedLevel = level;
        SceneManager.sceneLoaded += LevelLoaded;
        SceneManager.LoadScene("Game", LoadSceneMode.Single);
    }
}
