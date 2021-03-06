﻿public class Reinforced : Piece {
	public bool[] hasReinforced;
	public int reinforcedDamageReceived = 1;

	public override void GetHit(int direction) => Health -= hasReinforced[direction] ? reinforcedDamageReceived : (hasConnector[direction] ? connectorDamageReceived : deadEndDamageReceived);

	public override void Rotate(bool clockwise) {
		base.Rotate(clockwise);
		RotateSlots(hasReinforced, clockwise);
	}
}
