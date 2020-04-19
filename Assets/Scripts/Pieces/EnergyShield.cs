public class EnergyShield : Piece {
	public bool[] hasEnergyShield;
	public int disabledEnergyShieldDamageReceived = 2;

	public override void GetHit(int direction) {
		//TODO: Energy calculations
		Health -= hasEnergyShield[direction] ? disabledEnergyShieldDamageReceived : (hasConnector[direction] ? connectorDamageReceived : deadEndDamageReceived);
	}

	public override void Rotate(bool clockwise) {
		base.Rotate(clockwise);
		RotateSlots(hasEnergyShield, clockwise);
	}
}
