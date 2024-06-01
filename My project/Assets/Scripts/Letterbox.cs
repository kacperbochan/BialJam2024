using UnityEngine;
public class Letterbox : MonoBehaviour
{
    [SerializeField] private GameObject leftLetterbox;
    [SerializeField] private GameObject rightLetterbox;
    [SerializeField] private GameObject bottomLetterbox;
    [SerializeField] private GameObject topLetterbox;
    [SerializeField] private Transform clouds;
    private const float CAMERA_SIZE_TO_CLOUD_SCALE_RATIO = 5f;

    public static Letterbox Instance { get; private set; }
    private void Awake()
    {
        Instance = this;
        UpdateLetterbox();
    }
    private void OnValidate()
    {
        Instance = this;
        UpdateLetterbox();
    }

    public void UpdateLetterbox()
    {
        /*
        float width = (float)Screen.width;
        float height = (float)Screen.height;
        float targetRatio = 16f / 9f;

        float letterboxWidth = (width - (height * targetRatio)) / 2f;
        float letterboxHeight = (height - (width / targetRatio)) / 2f;

        float desiredWidth = height * targetRatio;

        Vector2 Letterbox_left_pos = Camera.main.ScreenToWorldPoint(new Vector2(-(desiredWidth - width) / 2, height/2));
        Vector2 Letterbox_right_pos = Camera.main.ScreenToWorldPoint(new Vector2(width + (desiredWidth - width) / 2, height/2));
        Vector2 Letterbox_up_pos = Camera.main.ScreenToWorldPoint(new Vector2(width/2, height));
        Vector2 Letterbox_down_pos = Camera.main.ScreenToWorldPoint(new Vector2(width/2, 0));

        leftLetterbox.transform.position = new Vector2(Letterbox_left_pos.x - 15, Letterbox_left_pos.y);
        rightLetterbox.transform.position = new Vector2(Letterbox_right_pos.x + 15, Letterbox_right_pos.y);
        topLetterbox.transform.position = new Vector2(Letterbox_up_pos.x, Letterbox_up_pos.y + 15);
        leftLetterbox.transform.position = new Vector2(Letterbox_down_pos.x, Letterbox_down_pos.y - 15);
        */
        float cloudsScale = Camera.main.orthographicSize / CAMERA_SIZE_TO_CLOUD_SCALE_RATIO;
        clouds.position = new Vector3(Camera.main.transform.position.x - 10f * cloudsScale, Camera.main.transform.position.y);
        clouds.localScale = Vector3.one * cloudsScale;

        leftLetterbox.transform.localScale = Vector3.one * cloudsScale * 100f;
        leftLetterbox.transform.position = new Vector3(Camera.main.transform.position.x - cloudsScale * 58.9f, Camera.main.transform.position.y);

        rightLetterbox.transform.localScale = Vector3.one * cloudsScale * 100f;
        rightLetterbox.transform.position = new Vector3(Camera.main.transform.position.x + cloudsScale * 58.9f, Camera.main.transform.position.y);

        topLetterbox.transform.localScale = Vector3.one * cloudsScale * 100f;
        topLetterbox.transform.position = new Vector3(Camera.main.transform.position.x, Camera.main.transform.position.y + cloudsScale * 55f);

        bottomLetterbox.transform.localScale = Vector3.one * cloudsScale * 100f;
        bottomLetterbox.transform.position = new Vector3(Camera.main.transform.position.x, Camera.main.transform.position.y - cloudsScale * 55f);
    }
}
