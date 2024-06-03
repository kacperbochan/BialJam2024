using UnityEngine;
using UnityEngine.UI;

public class PlayButton : MonoBehaviour
{
    [SerializeField] private Animator animator;
    private void Start()
    {

        GetComponent<Button>().onClick.AddListener(() => {
            animator.SetTrigger("clicked");
        });

    }

    void Update()
    {
        if (Input.GetKeyDown("space") ||
            Input.GetKeyDown("return"))
        {
            animator.SetTrigger("clicked");
        }
    }
}
