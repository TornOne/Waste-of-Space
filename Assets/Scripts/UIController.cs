using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour {
	public Text scoreText;
	public float Score {
		set => scoreText.text = $"Score: {value:0}";
	}

	public Image energyBar;
	public Text energyText;
	public float Energy {
		set {
			energyText.text = $"{value:0} Energy";
			energyBar.fillAmount = 1 - 100 / (value + 100);
		}
	}

	void Awake() {
		Score = 0;
		Energy = 0;
	}
}
