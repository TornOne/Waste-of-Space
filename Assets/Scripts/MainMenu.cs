using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour {
	public void LoadScene(int sceneNumber) => SceneManager.LoadScene(sceneNumber);

	public void Quit() => Application.Quit(0);
}
