using UnityEngine;
using UnityEngine.SceneManagement;

public class Teleport : MonoBehaviour
{
    [SerializeField] public string sceneName;
    private bool active = false;
    public YesNoUI ui;
    private string questionText;

    void OnTriggerExit2D(Collider2D collision)
    {
        active = true;
    }

    void Start()
    {
        if (SceneManager.GetActiveScene().name == "Menu")
        {
            questionText = "Enter the Dungeon?";
        }
        else
        {
            questionText = "Return to the Market?";
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        
        if (collision.gameObject.CompareTag("Player") && active)
        {
            ui.ShowYesNo(questionText, answer =>
            {
                if (answer)
                {
                    SceneManager.LoadScene(sceneName);
                    active = false;
                }
            });
        }
    }
}
