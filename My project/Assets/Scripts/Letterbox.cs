using UnityEngine;
public class Letterbox : MonoBehaviour
{
    [SerializeField] private GameObject leftLetterbox;
    [SerializeField] private GameObject rightLetterbox;
    [SerializeField] private GameObject bottomLetterbox;
    [SerializeField] private GameObject topLetterbox;
    [SerializeField] private Transform clouds;
    private float targetCameraSize;

    public static Letterbox Instance { get; private set; }
    private void Awake()
    {
        Instance = this;
    }
    private void OnValidate()
    {
        Instance = this;
    }
    private void Start()
    {
        targetCameraSize = Camera.main.orthographicSize;
        UpdateLetterbox();
    }

    public void UpdateLetterbox()
    {
        float width = (float)Screen.width;
        float height = (float)Screen.height;
        float targetRatio = 16f / 9f;

        if (width / height > targetRatio)
        {
            // wide screen
            Camera.main.orthographicSize = targetCameraSize;
        }
        else
        {
            // tall screen, zoom camera out to keep vertical FOV
            float sizeMultiplier = targetRatio / (width / height);
            Camera.main.orthographicSize = targetCameraSize * sizeMultiplier;
        }

        float letterboxWidth = (width - (height * targetRatio)) / 2f;
        float letterboxHeight = (height - (width / targetRatio)) / 2f;


        Vector2 desired_screen_size = new();
        Vector2 current_screen_size = new(width, height);

        desired_screen_size.y = current_screen_size.y;
        desired_screen_size.x = current_screen_size.y * targetRatio;

        float x_diff = desired_screen_size.x - current_screen_size.x;

        Vector2 Letterbox_left_pos = Camera.main.ScreenToWorldPoint(new Vector3(-x_diff/2, current_screen_size.y/2, 0));
        Vector2 Letterbox_right_pos = Camera.main.ScreenToWorldPoint(new Vector3(current_screen_size.x + x_diff/2, current_screen_size.y/2, 0));
        Vector2 Letterbox_up_pos = Camera.main.ScreenToWorldPoint(new Vector3(current_screen_size.x/2, current_screen_size.y, 0));
        Vector2 Letterbox_down_pos = Camera.main.ScreenToWorldPoint(new Vector3(current_screen_size.x/2, 0, 0));

        leftLetterbox.transform.position = new Vector3(Letterbox_left_pos.x - 15, Letterbox_left_pos.y, -29);
        rightLetterbox.transform.position = new Vector3(Letterbox_right_pos.x + 15, Letterbox_right_pos.y, -29);
        topLetterbox.transform.position = new Vector3(Letterbox_up_pos.x, Letterbox_up_pos.y + 15, -29);
        leftLetterbox.transform.position = new Vector3(Letterbox_down_pos.x, Letterbox_down_pos.y - 15, -29);
        
        clouds.position = new Vector3(Letterbox_up_pos.x-15, Letterbox_up_pos.y-15, -29);
    }
}
