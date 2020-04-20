using System.Collections.Generic;
using UnityEngine;

public class Turret : Piece {
	public bool[] hasTurret;
	public int turretDamageReceived = 2;
	const float sqrRange = 4 * 4;
	public float shotCooldown = 1;
	readonly float[] lastShot = new float[4];
	bool isDisabled;

	readonly GameObject[] turretModels = new GameObject[4];
	HashSet<Asteroid> allAsteroids;

	void Awake() {
		enabled = false;
		allAsteroids = AsteroidSpawner.instance.allAsteroids;

		for (int i = 0; i < 4; i++) {
			foreach (Transform child in transform.GetChild(i)) {
				if (child.CompareTag("TurretWall")) {
					int dir = (int)(child.localRotation.eulerAngles.y / 90 + 4.5f) % 4;
					turretModels[dir] = child.gameObject;
				}
			}
		}
	}

	public override void Place(Vector2Int position) {
		base.Place(position);

		foreach (Transform child in transform) {
			foreach (Transform child2 in child) {
				if (child2.CompareTag("TurretCoF")) {
					child2.gameObject.SetActive(false);
				}
			}
		}

		isDisabled = Core.instance.Energy < 1;
		Core.instance.IsEnergyLow += SetTurretActive;
	}

	void SetTurretActive(bool isDisabled) => this.isDisabled = isDisabled;

	void Update() {
		for (int i = 0; i < 4; i++) {
			//Has turret in slot, is off cooldown, has enough energy
			if (isDisabled || !hasTurret[i] || Time.time - lastShot[i] < shotCooldown) {
				continue;
			}

			//Find suitable asteroid
			float minSqrDist = sqrRange;
			Asteroid target = null;
			foreach (Asteroid asteroid in allAsteroids) {
				Vector3 vectorToAsteroid = asteroid.transform.position - transform.position;
				float sqrDist = vectorToAsteroid.sqrMagnitude;
				if (sqrDist <= minSqrDist && GetDirection(vectorToAsteroid.x, vectorToAsteroid.z) == i) {
					minSqrDist = sqrDist;
					target = asteroid;
				}
			}
			if (target == null) {
				continue;
			}

			//Face asteroid and shoot
			turretModels[i].transform.LookAt(target.transform);
			//TODO: Spawn laser effect
			Core.instance.Energy -= 1;
			target.Explode();
		}
	}

	public override void GetHit(int direction) => Health -= hasTurret[direction] ? turretDamageReceived : (hasConnector[direction] ? connectorDamageReceived : deadEndDamageReceived);

	public override void Rotate(bool clockwise) {
		base.Rotate(clockwise);
		RotateSlots(hasTurret, clockwise);
	}
}
