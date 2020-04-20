using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIController : MonoBehaviour {
	public Text scoreText;
	public float Score {
		set => scoreText.text = value.ToString("0");
	}

	public Image energyBar;
	public float Energy {
		set => energyBar.fillAmount = 1 - 100 / (value + 100);
	}

	void Awake() {
		Score = 0;
		Energy = 0;
	}

	public GameObject gameOverScreen;
	public Text finalScore;
	public float FinalScore {
		set {
			gameOverScreen.SetActive(true);
			finalScore.text = $"Distance Travelled: {value:0}";
		}
	}

	public Text finalAsteroids;
	public int FinalAsteroids {
		set => finalAsteroids.text = $"Asteroids Exploded: {value}";
	}

	public Text finalBlocks;
	public int FinalBlocks {
		set => finalBlocks.text = $"Blocks Placed: {value}";
	}

	public Text finalEnergy;
	public float FinalEnergy {
		set => finalEnergy.text = $"Energy Produced: {value:0}";
	}

	public GameObject pauseMenu;
	bool isPaused = false;
	void Update() {
		if (Input.GetButtonDown("Cancel")) {
			isPaused = !isPaused;
			Time.timeScale = isPaused ? 0 : 1;
			pauseMenu.SetActive(isPaused);
		}
	}

	public void LoadScene(int sceneNumber) => SceneManager.LoadScene(sceneNumber);
}
