using Assets.Enums;
using Assets.Scripts;
using Assets.Scripts.ScoorRepository;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    [SerializeField]
    private UnityEngine.UI.Button _loadGame;

    [SerializeField]
    private GameObject hieghtScoors;

    [SerializeField]
    private GameObject scoorPreFab;

    [SerializeField]
    private UnityEngine.UI.InputField inputName;

    void Start()
    {

        _loadGame.onClick.AddListener(LoadNewGame);
        AddScoors();

    }

    private void LoadNewGame()
    {
        UnitySingleton.Instance.playerName = inputName.text == "" ? "NO Name" : inputName.text;
        SceneManager.LoadScene((int)EScens.MainGame);
    }

    private void AddScoors()
    {
        ScoreRepositoryAction scoreRepositoryAction = new ScoreRepositoryAction();
        var scoreList = scoreRepositoryAction.GetScores();

        foreach (var score in scoreList.Scores)
        {
            var text = Instantiate(scoorPreFab);
            string scoreText = score.Name + " - Level: " + score.Level + " - Round: " + score.Round;
            text.GetComponent<Text>().text = scoreText;
            text.transform.parent = hieghtScoors.transform;
            text.transform.localScale = new Vector3(1, 1, 1);
        }


    }
}
