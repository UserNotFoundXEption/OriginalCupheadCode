using System;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x020004D4 RID: 1236
public class MapUICoins : MonoBehaviour
{
	// Token: 0x06003343 RID: 13123 RVA: 0x0002A759 File Offset: 0x00028959
	public void Start()
	{
		this.singleDigitCoinPosition = this.coinImage.transform.localPosition;
	}

	// Token: 0x06003344 RID: 13124 RVA: 0x000F3C18 File Offset: 0x000F1E18
	public void Update()
	{
		if (this.playerId == PlayerId.PlayerTwo)
		{
			if (!PlayerManager.Multiplayer)
			{
				this.coinImage.enabled = false;
				this.currencyNbImage.enabled = false;
				return;
			}
			this.coinImage.enabled = true;
			this.currencyNbImage.enabled = true;
		}
		int currency = PlayerData.Data.GetCurrency(this.playerId);
		if (currency != this.previousCurrency)
		{
			this.previousCurrency = currency;
			this.currencyNbImage.sprite = this.coinSprites[currency];
			if (currency > 9)
			{
				this.coinImage.transform.localPosition = this.doubleDigitCoinTransform.localPosition;
			}
			else
			{
				this.coinImage.transform.localPosition = this.singleDigitCoinPosition;
			}
		}
	}

	// Token: 0x04002A5C RID: 10844
	[SerializeField]
	public PlayerId playerId;

	// Token: 0x04002A5D RID: 10845
	[SerializeField]
	public Image coinImage;

	// Token: 0x04002A5E RID: 10846
	[SerializeField]
	public Image currencyNbImage;

	// Token: 0x04002A5F RID: 10847
	[SerializeField]
	public Sprite[] coinSprites;

	// Token: 0x04002A60 RID: 10848
	[SerializeField]
	public Transform doubleDigitCoinTransform;

	// Token: 0x04002A61 RID: 10849
	public Vector3 singleDigitCoinPosition;

	// Token: 0x04002A62 RID: 10850
	public int previousCurrency = -1;
}
