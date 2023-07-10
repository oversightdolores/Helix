using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class MenuScript : MonoBehaviour
{
    private UIDocument ui;
    private Label title;
    private Button play;
    private Button exit;

    private void Awake()
    {
        ui = GetComponent<UIDocument>();
         VisualElement root = ui.rootVisualElement;
        title = root.Q<Label>("Title");
        play = root.Q<Button>("Play");
        exit = root.Q<Button>("Exit");

        
            play.clicked += PlayGame;
        

        
            exit.clicked += QuitGame;
        
      
    }

   
    private void PlayGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    private void QuitGame()
    {
        Debug.Log("QUIT!");
        Application.Quit();
    }
}
