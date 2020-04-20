using System.Collections.Generic;
using UnityEngine;

public class EnergyShield : Piece {
	public bool[] hasEnergyShield;
	public int disabledEnergyShieldDamageReceived = 2;

	bool isActive;
	readonly List<GameObject> shieldModels = new List<GameObject>(3);

	void Awake() {
		foreach (Transform child in transform) {
			foreach (Transform child2 in child) {
				if (child2.CompareTag("EnergyWall")) {
					shieldModels.Add(child2.gameObject);
				}
			}
		}
	}

	public override void Place(Vector2Int position) {
		base.Place(position);

		isActive = Core.instance.Energy >= 1;
		Core.instance.IsEnergyLow += SetShieldActive;
	}

	void SetShieldActive(bool isDisabled) {
		isActive = !isDisabled;
		foreach (GameObject shieldModel in shieldModels) {
			shieldModel.SetActive(isActive);
		}
	}

	public override void GetHit(int direction) {
		if (isActive && hasEnergyShield[direction]) {
			Core.instance.Energy -= 1;
		} else {
			Health -= hasEnergyShield[direction] ? disabledEnergyShieldDamageReceived : (hasConnector[direction] ? connectorDamageReceived : deadEndDamageReceived);
		}
	}

	public override void Rotate(bool clockwise) {
		base.Rotate(clockwise);
		RotateSlots(hasEnergyShield, clockwise);
	}

	protected override void Disable() {
		Core.instance.IsEnergyLow -= SetShieldActive;

		base.Disable();
	}
}
