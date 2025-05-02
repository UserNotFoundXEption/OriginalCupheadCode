using System;
using UnityEngine;

// Token: 0x02000479 RID: 1145
public class MapCoin : MonoBehaviour
{
	// Token: 0x0600308C RID: 12428 RVA: 0x00028490 File Offset: 0x00026690
	public void Start()
	{
		if (PlayerData.Data.coinManager.GetCoinCollected(this.coinID))
		{
			Object.Destroy(base.gameObject);
		}
	}

	// Token: 0x0600308D RID: 12429 RVA: 0x000E6934 File Offset: 0x000E4B34
	public void OnTriggerEnter2D(Collider2D collider)
	{
		if (!PlayerData.Data.coinManager.GetCoinCollected(this.coinID))
		{
			PlayerData.Data.coinManager.SetCoinValue(this.coinID, true, PlayerId.Any);
			PlayerData.Data.AddCurrency(PlayerId.PlayerOne, 1);
			PlayerData.Data.AddCurrency(PlayerId.PlayerTwo, 1);
			PlayerData.SaveCurrentFile();
			MapEventNotification.Current.ShowEvent(MapEventNotification.Type.Coin);
			if (this.mapNPCCoinMoneyman != null)
			{
				this.mapNPCCoinMoneyman.UpdateCoins();
			}
			Object.Destroy(base.gameObject);
		}
	}

	// Token: 0x04002827 RID: 10279
	[SerializeField]
	public MapNPCCoinMoneyman mapNPCCoinMoneyman;

	// Token: 0x04002828 RID: 10280
	public string coinID = Guid.NewGuid().ToString();
}
