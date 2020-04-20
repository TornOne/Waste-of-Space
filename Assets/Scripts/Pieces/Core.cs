using System;
using System.Collections;
using UnityEngine;

public class Core : Piece {
	public static Core instance;

	UIController ui;

	float _score;
	public float Score {
		get => _score;
		set {
			ui.Score = value;
			_score = value;
		}
	}
	float _energy;
	public float Energy {
		get => _energy;
		set {
			if (_energy >= 1 && value < 1) {
				IsEnergyLow?.Invoke(true);
			} else if (_energy < 1 && value >= 1) {
				IsEnergyLow?.Invoke(false);
			}

			if (value > _energy) {
				totalEnergyProduced += value - _energy;
			}

			ui.Energy = value;
			_energy = value;
		}
	}
	float totalEnergyProduced;
	public int asteroidsDestroyed;
	public int blocksPlaced;
	public float energyPerSecond = 1;

	public Action<bool> IsEnergyLow;

	void Awake() {
		instance = this;
		ui = FindObjectOfType<UIController>();
	}

	void Update() => Energy += energyPerSecond * Time.deltaTime;

	protected override void Disable() {
		StartCoroutine(GameOverRoutine());
		base.Disable();
	}

	IEnumerator GameOverRoutine() {
		float finalScore = Score;
		float finalEnergy = totalEnergyProduced;
		int finalAsteroids = asteroidsDestroyed;
		int finalBlocks = blocksPlaced;

		yield return new WaitForSeconds(3);

		ui.FinalScore = finalScore;
		ui.FinalEnergy = finalEnergy;
		ui.FinalAsteroids = finalAsteroids;
		ui.FinalBlocks = finalBlocks;
	}
}
