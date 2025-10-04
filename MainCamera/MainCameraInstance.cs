using UnityEngine;

public class MainCameraInstance : MonoBehaviour
{
    public static Camera instance;

    private void Awake()
    {
        instance = GetComponent<Camera>();
    }
}