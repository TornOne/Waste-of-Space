using System.Collections.Generic;
using UnityEngine;

public class Turret : Piece {
	public LineRenderer laser;

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
	}

	public override void Place(Vector2Int position) {
		base.Place(position);

		foreach (Transform child in transform) {
			foreach (Transform child2 in child) {
				if (child2.CompareTag("TurretCoF")) {
					child2.gameObject.SetActive(false);
				} else if (child2.CompareTag("TurretWall")) {
					int dir = (int)(child.rotation.eulerAngles.y / 90 + 4.5f) % 4;
					turretModels[dir] = child2.gameObject;
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
			LineRenderer laserBeam = Instantiate(laser);
			laserBeam.SetPosition(0, turretModels[i].transform.position);
			laserBeam.SetPosition(1, target.transform.position);

			Core.instance.Energy -= 1;
			lastShot[i] = Time.time;
			target.Explode();
		}
	}

	public override void GetHit(int direction) => Health -= hasTurret[direction] ? turretDamageReceived : (hasConnector[direction] ? connectorDamageReceived : deadEndDamageReceived);

	public override void Rotate(bool clockwise) {
		base.Rotate(clockwise);
		RotateSlots(hasTurret, clockwise);
	}
}
