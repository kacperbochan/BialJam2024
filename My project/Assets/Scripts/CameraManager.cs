using System.Collections;
using UnityEditor;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
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

    private static float backgroundCamera1TargetX, backgroundCamera1MoveSpeed;
    private static float backgroundCamera2TargetX, backgroundCamera2MoveSpeed;
    private static float backgroundCamera3TargetX, backgroundCamera3MoveSpeed;

    private void Awake()
    {
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
    }

    [MenuItem("Camera Manager/Go to stage 1")]
    private static void GoToStage1()
    {
        SetCameraTargetsToStage(1);
        TeleportCameras();
    }
    [MenuItem("Camera Manager/Go to stage 2")]
    private static void GoToStage2()
    {
        SetCameraTargetsToStage(2);
        TeleportCameras();
    }
    [MenuItem("Camera Manager/Go to stage 3")]
    private static void GoToStage3()
    {
        SetCameraTargetsToStage(3);
        TeleportCameras();
    }
    [MenuItem("Camera Manager/Go to stage 4")]
    private static void GoToStage4()
    {
        SetCameraTargetsToStage(4);
        TeleportCameras();
    }
    [MenuItem("Camera Manager/Go to stage 5")]
    private static void GoToStage5()
    {
        SetCameraTargetsToStage(5);
        TeleportCameras();
    }
    [MenuItem("Camera Manager/Go to stage 6")]
    private static void GoToStage6()
    {
        SetCameraTargetsToStage(6);
        TeleportCameras();
    }
    [MenuItem("Camera Manager/Go to stage 7")]
    private static void GoToStage7()
    {
        SetCameraTargetsToStage(7);
        TeleportCameras();
    }
    [MenuItem("Camera Manager/Go to stage 8")]
    private static void GoToStage8()
    {
        SetCameraTargetsToStage(8);
        TeleportCameras();
    }

    public void GoToStage(int stage)
    {
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

            if ((Mathf.Abs(backgroundCamera1.transform.position.x - backgroundCamera1TargetX) < NEGLIGIBLE_DIFFERENCE) &&
                (Mathf.Abs(backgroundCamera2.transform.position.x - backgroundCamera2TargetX) < NEGLIGIBLE_DIFFERENCE) &&
                (Mathf.Abs(backgroundCamera3.transform.position.x - backgroundCamera3TargetX) < NEGLIGIBLE_DIFFERENCE))
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
    }
}
