using UnityEngine;
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
}
