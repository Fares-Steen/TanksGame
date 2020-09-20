using Assets.Enums;
using Assets.Scripts;
using Assets.Scripts.Extension;
using Assets.Scripts.ScoorRepository;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    [SerializeField]
    private UnityEngine.UI.Button _loadGame;

    [SerializeField]
    private UnityEngine.UI.Button godMod;

    [SerializeField]
    private GameObject hieghtScoors;

    [SerializeField]
    private GameObject scoorPreFab;

    [SerializeField]
    private UnityEngine.UI.InputField inputName;

    void Start()
    {

        _loadGame.onClick.AddListener(LoadNewGame);
        godMod.onClick.AddListener(LoadNewGameInGodMod);
        AddScoors();

    }

    private void LoadNewGame()
    {
        UnitySingleton.Instance.playerName = inputName.text == "" ? "NO Name" : inputName.text;
        SceneManager.LoadScene((int)EScens.MainGame);
    }
    private void LoadNewGameInGodMod()
    {

        UnitySingleton.Instance.godMod = true;
        SceneManager.LoadScene((int)EScens.MainGame);
    }

    private void AddScoors()
    {
        ScoreRepositoryAction scoreRepositoryAction = new ScoreRepositoryAction();
        var scoreList = scoreRepositoryAction.GetScores();
        if (scoreList.Scores.Count > 0)
        {
            scoreList.Scores = scoreList.Scores.SortBy(s => s.Round);
            scoreList.Scores = scoreList.Scores.SortBy(s => s.Level);
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
}
