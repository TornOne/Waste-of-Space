using UnityEngine;

public class PieceSpawner : MonoBehaviour {
	public GameObject deadEndModel, connectorModel, energyShieldModel, engineModel, reinforcedModel, turretModel;
	public Piece connector;
	public EnergyShield energyShield;
	public Engine engine;
	public Reinforced reinforced;
	public Turret turret;

	int GetRandomDir(int excluding) {
		int randomDir;
		do {
			randomDir = Random.Range(0, 4);
		} while (randomDir == excluding);
		return randomDir;
	}

	int[] GetRemainingDir(params int[] excluding) {
		int[] remainingDir = new int[2];
		for (int i = 0, j = 0; i < 4; i++) {
			if (!System.Array.Exists(excluding, x => x == i)) {
				remainingDir[j++] = i;
			}
		}
		return remainingDir;
	}

	bool GetRandomBool() => Random.Range(0, 2) == 0;

	#region Connector
	Piece GetRandomConnector() {
		int connectorSlot = Random.Range(0, 4);
		bool[] hasConnector = new bool[4];
		for (int i = 0; i < 4; i++) {
			hasConnector[i] = connectorSlot == i || GetRandomBool();
		}
		return CreateConnector(hasConnector);
	}

	Piece CreateConnector(params bool[] hasConnector) {
		Piece parent = Instantiate(connector);
		parent.hasConnector = hasConnector;

		for (int i = 0; i < 4; i++) {
			Instantiate(hasConnector[i] ? connectorModel : deadEndModel, parent.transform.position, Quaternion.Euler(-90, 90 * i, 0)).transform.parent = parent.transform;
		}
		return parent;
	}
	#endregion

	#region Engine
	Engine GetRandomEngine() {
		int connectorSlot = GetRandomDir(2);
		return CreateEngine(connectorSlot == 0 || GetRandomBool(), connectorSlot == 1 || GetRandomBool(), false, connectorSlot == 2 || GetRandomBool());
	}

	Engine CreateEngine(params bool[] hasConnector) {
		Engine parent = Instantiate(engine);
		parent.hasConnector = hasConnector;

		for (int i = 0; i < 4; i++) {
			Instantiate(i == 2 ? engineModel : (hasConnector[i] ? connectorModel : deadEndModel), parent.transform.position, Quaternion.Euler(-90, 90 * i, 0)).transform.parent = parent.transform;
		}
		return parent;
	}
	#endregion

	#region Reinforced
	Reinforced GetRandomReinforced() {
		int connectorSlot = Random.Range(0, 4);
		int reinforcedSlot = GetRandomDir(connectorSlot);

		bool[] hasConnector = new bool[4];
		bool[] hasReinforced = new bool[4];
		hasConnector[connectorSlot] = true;
		hasReinforced[reinforcedSlot] = true;
		foreach (int slot in GetRemainingDir(connectorSlot, reinforcedSlot)) {
			int slotType = Random.Range(0, 3);
			if (slotType == 1) {
				hasConnector[slot] = true;
			} else if (slotType == 3) {
				hasReinforced[slot] = true;
			}
		}

		return CreateReinforced(hasConnector, hasReinforced);
	}

	Reinforced CreateReinforced(bool[] hasConnector, bool[] hasReinforced) {
		Reinforced parent = Instantiate(reinforced);
		parent.hasConnector = hasConnector;
		parent.hasReinforced = hasReinforced;

		for (int i = 0; i < 4; i++) {
			Instantiate(hasReinforced[i] ? reinforcedModel : (hasConnector[i] ? connectorModel : deadEndModel), parent.transform.position, Quaternion.Euler(-90, 90 * i, 0)).transform.parent = parent.transform;
		}
		return parent;
	}
	#endregion

	public Piece GetRandomPiece() {
		float random = Random.Range(0f, 6f);

		if (random < 1) {
			return GetRandomConnector();
		} else if (random < 2) {
			return GetRandomReinforced();
		} else if (random < 3) {
			return GetRandomEngine();
		} else if (random < 4) {
			return GetRandomEnergyShield();
		} else if (random < 5) {
			return GetRandomReactor();
		} else {
			return GetRandomTurret();
		}
	}
}
