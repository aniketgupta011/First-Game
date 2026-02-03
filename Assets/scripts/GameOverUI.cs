using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameOverUI : MonoBehaviour
{
    [SerializeField] private Button mainmenuButton;
    [SerializeField] private TextMeshProUGUI scoreTextMesh;

    private void Awake()
    {
        mainmenuButton.onClick.AddListener(() =>
        {
            SceneLoader.LoadScene(SceneLoader.Scene.MainMenu);
        });

    }
    private void Start()
    {
        scoreTextMesh.text = "FINAL SCORE: " + GameManager.Instance.GetTotalScore().ToString();
        mainmenuButton.Select();
    }
}
