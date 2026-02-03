using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class StatsUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI statstextmesh;
    [SerializeField] private GameObject speedUpArrowGameObject;
    [SerializeField] private GameObject speedLeftArrowGameObject;
    [SerializeField] private GameObject speedRightArrowGameObject;
    [SerializeField] private GameObject speedDownArrowGameObject;
    [SerializeField] private Image fuelImage;
    private void Update()
    {
        UpdateStatsTextMesh(); 
    }
    private void UpdateStatsTextMesh()
    {
        speedUpArrowGameObject.SetActive(lander.Instance.GetSpeedY() >= 0);
        speedDownArrowGameObject.SetActive(lander.Instance.GetSpeedY() < 0);
        speedRightArrowGameObject.SetActive(lander.Instance.GetSpeedX() >= 0);
        speedLeftArrowGameObject.SetActive(lander.Instance.GetSpeedX() < 0);

        fuelImage.fillAmount=lander.Instance.GetFuelAmountNormalized();
        statstextmesh.text= GameManager.Instance.GetLevelNumber() + "\n" + GameManager.Instance.GetScore() + "\n" + Mathf.Round(GameManager.Instance.GetTime()) + "\n"+ Mathf.Abs(Mathf.Round(lander.Instance.GetSpeedX() * 10f)) + "\n"+ Mathf.Abs(Mathf.Round(lander.Instance.GetSpeedY()) * 10f);
    }
}
