using UnityEngine;

public class SceneUIActions : MonoBehaviour
{
    public void OpenScene(string sceneName)
    {
        GameManager.Instance.OpenScene(sceneName);
    }
	public void NewStory()
	{
		GameManager.Instance.NewStory();
	}
	public void CloseScene()
    {
        GameManager.Instance.CloseScene();
    }

    public void QuitGame()
    {
        GameManager.Instance.QuitGame();
    }
}