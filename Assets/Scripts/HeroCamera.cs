using UnityEngine;

public class HeroCamera : MonoBehaviour
{
    private Camera mainCamera;

    //初始分辨率为1920x1080，对应的FOV为60度
    public float initialWidth = 1920;
    public float initialHeight = 1080;
    public float initialFov = 60;
    public float initialY = 100;
    public float initialZ = -600;

    private float NowWidth;
    private float NowHeight;

    void Start()
    {
        mainCamera = GetComponent<Camera>();
        mainCamera.fieldOfView = initialFov;
        mainCamera.transform.position = new Vector3(0, initialY, initialZ);
        NowWidth = Screen.width;
        NowHeight = Screen.height;
    }

    private void Update()
    {
        // 检测分辨率是否改变
        if (Screen.width != NowWidth || Screen.height != NowHeight)
        {
            NowWidth = Screen.width;
            NowHeight = Screen.height;
            AdjustCameraForResolution();
        }
    }

    void AdjustCameraForResolution()
    {
        mainCamera.transform.position = new Vector3(0, initialY * ((float)Screen.height / initialHeight), initialZ * ((float)Screen.width / initialWidth));
    }
}