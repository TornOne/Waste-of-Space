using UnityEngine;

public class PieceSpawner : MonoBehaviour {
	public static PieceSpawner instance;

	public GameObject deadEndModel, connectorModel, energyShieldModel, engineModel, reinforcedModel, turretModel, reactorModel;
	public Piece connector;
	public EnergyShield energyShield;
	public Engine engine;
	public Reinforced reinforced;
	public Turret turret;
	public Reactor reactor;

	void Awake() => instance = this;

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

	void GetRandomUtilityPieceLayout(out bool[] hasConnector, out bool[] hasUtility) {
		int connectorSlot = Random.Range(0, 4);
		int utilitySlot = GetRandomDir(connectorSlot);

		hasConnector = new bool[4];
		hasUtility = new bool[4];
		hasConnector[connectorSlot] = true;
		hasUtility[utilitySlot] = true;
		foreach (int slot in GetRemainingDir(connectorSlot, utilitySlot)) {
			int slotType = Random.Range(0, 3);
			if (slotType == 1) {
				hasConnector[slot] = true;
			} else if (slotType == 3) {
				hasUtility[slot] = true;
			}
		}
	}

	void GetRandomMidPieceLayout(out bool[] hasConnector) {
		int connectorSlot = Random.Range(0, 4);
		hasConnector = new bool[4];
		for (int i = 0; i < 4; i++) {
			hasConnector[i] = connectorSlot == i || GetRandomBool();
		}
	}

	#region Connector
	Piece GetRandomConnector() {
		GetRandomMidPieceLayout(out bool[] hasConnector);
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
		GetRandomUtilityPieceLayout(out bool[] hasConnector, out bool[] hasReinforced);
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

	#region Energy Shield
	EnergyShield GetRandomEnergyShield() {
		GetRandomUtilityPieceLayout(out bool[] hasConnector, out bool[] hasEnergyShield);
		return CreateEnergyShield(hasConnector, hasEnergyShield);
	}

	EnergyShield CreateEnergyShield(bool[] hasConnector, bool[] hasEnergyShield) {
		EnergyShield parent = Instantiate(energyShield);
		parent.hasConnector = hasConnector;
		parent.hasEnergyShield = hasEnergyShield;

		for (int i = 0; i < 4; i++) {
			Instantiate(hasEnergyShield[i] ? energyShieldModel : (hasConnector[i] ? connectorModel : deadEndModel), parent.transform.position, Quaternion.Euler(-90, 90 * i, 0)).transform.parent = parent.transform;
		}
		return parent;
	}
	#endregion

	#region Turret
	Turret GetRandomTurret() {
		GetRandomUtilityPieceLayout(out bool[] hasConnector, out bool[] hasTurret);
		return CreateTurret(hasConnector, hasTurret);
	}

	Turret CreateTurret(bool[] hasConnector, bool[] hasTurret) {
		Turret parent = Instantiate(turret);
		parent.hasConnector = hasConnector;
		parent.hasTurret = hasTurret;

		for (int i = 0; i < 4; i++) {
			Instantiate(hasTurret[i] ? turretModel : (hasConnector[i] ? connectorModel : deadEndModel), parent.transform.position, Quaternion.Euler(-90, 90 * i, 0)).transform.parent = parent.transform;
		}
		return parent;
	}
	#endregion

	#region Reactor
	Reactor GetRandomReactor() {
		GetRandomMidPieceLayout(out bool[] hasConnector);
		return CreateReactor(hasConnector);
	}

	Reactor CreateReactor(params bool[] hasConnector) {
		Reactor parent = Instantiate(reactor);
		parent.hasConnector = hasConnector;

		Instantiate(reactorModel, parent.transform.position, Quaternion.Euler(-90, 0, 0)).transform.parent = parent.transform;
		for (int i = 0; i < 4; i++) {
			Instantiate(hasConnector[i] ? connectorModel : deadEndModel, parent.transform.position, Quaternion.Euler(-90, 90 * i, 0)).transform.parent = parent.transform;
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
