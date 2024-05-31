using System.Collections;
using UnityEditor;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    public static CameraManager Instance { get; private set; }

    private static Camera backgroundCamera1;
    private static Camera backgroundCamera2;
    private static Camera backgroundCamera3;
    [SerializeField] private float cameraMoveTime = 2f;
    private const float NEGLIGIBLE_DIFFERENCE = .01f;

    private const float INITIAL_BACKGROUND_CAMERA_POSITION = 9.6f;
    private const float BACKGROUND_CAMERA_FULL_MOVE = 28.8f - 9.6f;
    private const float BACKGROUND_CAMERA_1_MOVE = BACKGROUND_CAMERA_FULL_MOVE * 3f / 7f;
    private const float BACKGROUND_CAMERA_2_MOVE = BACKGROUND_CAMERA_FULL_MOVE * 5f / 7f;
    private const float BACKGROUND_CAMERA_3_MOVE = BACKGROUND_CAMERA_FULL_MOVE;

    private static readonly float[] mainCameraX = new float[] {  8.0f, 36.6f,  0.0f,  0.0f,  0.0f,  0.0f,  0.0f,  0.0f };
    private static readonly float[] mainCameraY = new float[] {  5.0f,  3.3f,  0.0f,  0.0f,  0.0f,  0.0f,  0.0f,  0.0f };
    private static readonly float[] mainCameraS = new float[] { 10.0f,  8.0f,  0.0f,  0.0f,  0.0f,  0.0f,  0.0f,  0.0f };

    private static readonly float[] sceneStartingPointX = new float[] { -6f, 24f,  0.0f,  0.0f,  0.0f,  0.0f,  0.0f,  0.0f };
    private static readonly float[] sceneStartingPointY = new float[] { -3f,  0f,  0.0f,  0.0f,  0.0f,  0.0f,  0.0f,  0.0f };

    private static float backgroundCamera1TargetX, backgroundCamera1MoveSpeed;
    private static float backgroundCamera2TargetX, backgroundCamera2MoveSpeed;
    private static float backgroundCamera3TargetX, backgroundCamera3MoveSpeed;
    private static float mainCameraTargetX, mainCameraXMoveSpeed;
    private static float mainCameraTargetY, mainCameraYMoveSpeed;
    private static float mainCameraTargetS, mainCameraSMoveSpeed;

    private void Awake()
    {
        Instance = this;
        backgroundCamera1 = FindObjectOfType<BackgroundCamera1>().GetComponent<Camera>();
        backgroundCamera2 = FindObjectOfType<BackgroundCamera2>().GetComponent<Camera>();
        backgroundCamera3 = FindObjectOfType<BackgroundCamera3>().GetComponent<Camera>();
    }

    private void OnValidate()
    {
        backgroundCamera1 = FindObjectOfType<BackgroundCamera1>().GetComponent<Camera>();
        backgroundCamera2 = FindObjectOfType<BackgroundCamera2>().GetComponent<Camera>();
        backgroundCamera3 = FindObjectOfType<BackgroundCamera3>().GetComponent<Camera>();
    }

    private static void SetCameraTargetsToStage(int number)
    {
        backgroundCamera1TargetX = INITIAL_BACKGROUND_CAMERA_POSITION + (number - 1) * BACKGROUND_CAMERA_1_MOVE;
        backgroundCamera2TargetX = INITIAL_BACKGROUND_CAMERA_POSITION + (number - 1) * BACKGROUND_CAMERA_2_MOVE;
        backgroundCamera3TargetX = INITIAL_BACKGROUND_CAMERA_POSITION + (number - 1) * BACKGROUND_CAMERA_3_MOVE;
        mainCameraTargetX = mainCameraX[number-1];
        mainCameraTargetY = mainCameraY[number-1];
        mainCameraTargetS = mainCameraS[number-1];
    }

    [MenuItem("Camera Manager/Go to stage 1")]
    private static void GoToStage1()
    {
        SetCameraTargetsToStage(1);
        TeleportCameras();
        Player1.Instance.transform.position = new Vector2(sceneStartingPointX[0], sceneStartingPointY[0]);
        Player2.Instance.transform.position = new Vector2(sceneStartingPointX[0], sceneStartingPointY[0]);

    }
    [MenuItem("Camera Manager/Go to stage 2")]
    private static void GoToStage2()
    {
        SetCameraTargetsToStage(2);
        TeleportCameras();
        Player1.Instance.transform.position = new Vector2(sceneStartingPointX[1], sceneStartingPointY[1]);
        Player2.Instance.transform.position = new Vector2(sceneStartingPointX[1], sceneStartingPointY[1]);
    }
    [MenuItem("Camera Manager/Go to stage 3")]
    private static void GoToStage3()
    {
        SetCameraTargetsToStage(3);
        TeleportCameras();
        Player1.Instance.transform.position = new Vector2(sceneStartingPointX[2], sceneStartingPointY[2]);
        Player2.Instance.transform.position = new Vector2(sceneStartingPointX[2], sceneStartingPointY[2]);
    }
    [MenuItem("Camera Manager/Go to stage 4")]
    private static void GoToStage4()
    {
        SetCameraTargetsToStage(4);
        TeleportCameras();
        Player1.Instance.transform.position = new Vector2(sceneStartingPointX[3], sceneStartingPointY[3]);
        Player2.Instance.transform.position = new Vector2(sceneStartingPointX[3], sceneStartingPointY[3]);
    }
    [MenuItem("Camera Manager/Go to stage 5")]
    private static void GoToStage5()
    {
        SetCameraTargetsToStage(5);
        TeleportCameras();
        Player1.Instance.transform.position = new Vector2(sceneStartingPointX[4], sceneStartingPointY[4]);
        Player2.Instance.transform.position = new Vector2(sceneStartingPointX[4], sceneStartingPointY[4]);
    }
    [MenuItem("Camera Manager/Go to stage 6")]
    private static void GoToStage6()
    {
        SetCameraTargetsToStage(6);
        TeleportCameras();
        Player1.Instance.transform.position = new Vector2(sceneStartingPointX[5], sceneStartingPointY[5]);
        Player2.Instance.transform.position = new Vector2(sceneStartingPointX[5], sceneStartingPointY[5]);
    }
    [MenuItem("Camera Manager/Go to stage 7")]
    private static void GoToStage7()
    {
        SetCameraTargetsToStage(7);
        TeleportCameras();
        Player1.Instance.transform.position = new Vector2(sceneStartingPointX[6], sceneStartingPointY[6]);
        Player2.Instance.transform.position = new Vector2(sceneStartingPointX[6], sceneStartingPointY[6]);
    }
    [MenuItem("Camera Manager/Go to stage 8")]
    private static void GoToStage8()
    {
        SetCameraTargetsToStage(8);
        TeleportCameras();
        Player1.Instance.transform.position = new Vector2(sceneStartingPointX[7], sceneStartingPointY[7]);
        Player2.Instance.transform.position = new Vector2(sceneStartingPointX[7], sceneStartingPointY[7]);
    }
    public void GoToStage(int stage)
    {
        Debug.Log("going to stage " + stage);
        SetCameraTargetsToStage(stage);
        StartCoroutine(MoveCameras());
    }

    private IEnumerator MoveCameras()
    {
        while(true)
        {
            backgroundCamera1.transform.position = new Vector3(
                Mathf.SmoothDamp(backgroundCamera1.transform.position.x, backgroundCamera1TargetX, ref backgroundCamera1MoveSpeed, cameraMoveTime),
                backgroundCamera1.transform.position.y, backgroundCamera1.transform.position.z);
            backgroundCamera2.transform.position = new Vector3(
                Mathf.SmoothDamp(backgroundCamera2.transform.position.x, backgroundCamera2TargetX, ref backgroundCamera2MoveSpeed, cameraMoveTime),
                backgroundCamera2.transform.position.y, backgroundCamera2.transform.position.z);
            backgroundCamera3.transform.position = new Vector3(
                Mathf.SmoothDamp(backgroundCamera3.transform.position.x, backgroundCamera3TargetX, ref backgroundCamera3MoveSpeed, cameraMoveTime),
                backgroundCamera3.transform.position.y, backgroundCamera3.transform.position.z);

            Camera.main.transform.position = new Vector3(
                Mathf.SmoothDamp(Camera.main.transform.position.x, mainCameraTargetX, ref mainCameraXMoveSpeed, cameraMoveTime),
                Mathf.SmoothDamp(Camera.main.transform.position.y, mainCameraTargetY, ref mainCameraYMoveSpeed, cameraMoveTime),
                Camera.main.transform.position.z);
            Camera.main.orthographicSize = Mathf.SmoothDamp(Camera.main.orthographicSize, mainCameraTargetS, ref mainCameraSMoveSpeed, cameraMoveTime);

            if ((Mathf.Abs(backgroundCamera1.transform.position.x - backgroundCamera1TargetX) < NEGLIGIBLE_DIFFERENCE))
            {
                break;
            }

            yield return null;
        }
        //Debug.Log("finished camera move");
    }
    private static void TeleportCameras()
    {
        backgroundCamera1.transform.position = new Vector3(backgroundCamera1TargetX, backgroundCamera1.transform.position.y, backgroundCamera1.transform.position.z);
        backgroundCamera2.transform.position = new Vector3(backgroundCamera2TargetX, backgroundCamera2.transform.position.y, backgroundCamera2.transform.position.z);
        backgroundCamera3.transform.position = new Vector3(backgroundCamera3TargetX, backgroundCamera3.transform.position.y, backgroundCamera3.transform.position.z);
        Camera.main.transform.position = new Vector3(mainCameraTargetX, mainCameraTargetY, Camera.main.transform.position.z);
        Camera.main.orthographicSize = mainCameraTargetS;
    }
}
