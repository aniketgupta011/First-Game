using UnityEngine;

public class GameLevel : MonoBehaviour
{
    [SerializeField] private int levelNumber;
    [SerializeField] private Transform landerStartPositionTransform;
    [SerializeField] private Transform CameraStartTargetTransform;
    [SerializeField] private float zooomedOutOrthographicSize;

    public int GetLevelNumber()
    {
        return levelNumber; 
    }

    public Vector3 GetLanderStartPosition()
    {
        return landerStartPositionTransform.position;
    }

    public Transform GetCameraStartTargetTransform()
    {
        return CameraStartTargetTransform; 
    }
    public float GetZoomedOutOrthographicSize()
    {
        return zooomedOutOrthographicSize;
    }
}
