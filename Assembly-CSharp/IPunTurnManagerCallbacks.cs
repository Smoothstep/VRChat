using System;

// Token: 0x0200079F RID: 1951
public interface IPunTurnManagerCallbacks
{
	// Token: 0x06003F22 RID: 16162
	void OnTurnBegins(int turn);

	// Token: 0x06003F23 RID: 16163
	void OnTurnCompleted(int turn);

	// Token: 0x06003F24 RID: 16164
	void OnPlayerMove(PhotonPlayer player, int turn, object move);

	// Token: 0x06003F25 RID: 16165
	void OnPlayerFinished(PhotonPlayer player, int turn, object move);

	// Token: 0x06003F26 RID: 16166
	void OnTurnTimeEnds(int turn);
}
