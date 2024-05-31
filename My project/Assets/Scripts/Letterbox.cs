using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Letterbox : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 desired_screen_size = new Vector2();
        Vector2 current_screen_size = getScreenSize();

        desired_screen_size.y = current_screen_size.y;
        desired_screen_size.x = current_screen_size.y / 9 * 16;

        float x_diff = desired_screen_size.x - current_screen_size.x;
        float y_diff = desired_screen_size.y - current_screen_size.y;

        Vector2 Letterbox_left_pos = Camera.main.ScreenToWorldPoint(new Vector3(-x_diff/2, current_screen_size.y/2, 0));
        Vector2 Letterbox_right_pos = Camera.main.ScreenToWorldPoint(new Vector3(current_screen_size.x + x_diff/2, current_screen_size.y/2, 0));
        Vector2 Letterbox_up_pos = Camera.main.ScreenToWorldPoint(new Vector3(current_screen_size.x/2, current_screen_size.y, 0));
        Vector2 Letterbox_down_pos = Camera.main.ScreenToWorldPoint(new Vector3(current_screen_size.x/2, 0, 0));

        foreach(Letterbox_Collider obj in gameObject.GetComponentsInChildren<Letterbox_Collider>())
        {
            if (obj == null) continue;
            if (obj.name == "LetterboxLeft") obj.transform.position = new Vector3(Letterbox_left_pos.x - 15, Letterbox_left_pos.y, -29);
            if (obj.name == "LetterboxRight") obj.transform.position = new Vector3(Letterbox_right_pos.x + 15 , Letterbox_right_pos.y, -29);
            if (obj.name == "LetterboxUp") obj.transform.position = new Vector3(Letterbox_up_pos.x, Letterbox_up_pos.y + 15, -29);
            if (obj.name == "LetterboxDown") obj.transform.position = new Vector3(Letterbox_down_pos.x, Letterbox_down_pos.y - 15, -29);
        }
    }
    public static Vector2 getScreenSize()
    {
        return new Vector2(Screen.width, Screen.height);
    }
}
