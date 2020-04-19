public class Turret : Piece {
	public bool[] hasTurret;
	public int turretDamageReceived = 2;

	public override void GetHit(int direction) => Health -= hasTurret[direction] ? turretDamageReceived : (hasConnector[direction] ? connectorDamageReceived : deadEndDamageReceived);
}
