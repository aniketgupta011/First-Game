using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PausedUI : MonoBehaviour
{
    [SerializeField] private Button resumeButton;
    [SerializeField] private Button soundVolumeButton;
    [SerializeField] private Button musicVolueButton;
    [SerializeField] private TextMeshProUGUI soundVolumeTextMesh;
    [SerializeField] private TextMeshProUGUI musicVolumeTextMesh;
    private void Awake()
    {
        soundVolumeButton.onClick.AddListener(() =>
        {
            soundmanager.Instance.ChangeSoundVolume();
            soundVolumeTextMesh.text = "SOUND" + soundmanager.Instance.GetSoundVolume();
        });
        musicVolueButton.onClick.AddListener(() =>
        {
            musicVolumeTextMesh.text = "MUSIC" + Musicmanager.Instance.GetMusicVolume();
        });
            soundVolumeButton.onClick.AddListener(() =>
        {

        });
        resumeButton.onClick.AddListener(() =>
            {
                GameManager.Instance.UnPauseGame();
            });
    }
    private void Start()
    {
        GameManager.Instance.OnGamePaused += GameManager_OnGamePaused;
        GameManager.Instance.OnGameUnPaused += GameManager_OnGameUnPaused;
        soundVolumeTextMesh.text = "SOUND" + soundmanager.Instance.GetSoundVolume();
        musicVolumeTextMesh.text = "MUSIC" + Musicmanager.Instance.GetMusicVolume();
        Hide();
    }
    private void GameManager_OnGameUnPaused(object sender, System.EventArgs e)
    {
        Hide();
    }
    private void GameManager_OnGamePaused(object sender, System.EventArgs e)
    {
        Show();
    }
    private void Show()
    {
        gameObject.SetActive(true);
    }
    private void Hide()
    {
        gameObject.SetActive(false);
    }
}
