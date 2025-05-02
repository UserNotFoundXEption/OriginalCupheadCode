using System;
using System.Collections;
using UnityEngine;

// Token: 0x020001F6 RID: 502
public class DicePalaceMainLevelCard : AbstractProjectile
{
	// Token: 0x17000280 RID: 640
	// (get) Token: 0x0600172B RID: 5931 RVA: 0x00013BD4 File Offset: 0x00011DD4
	public override float ParryMeterMultiplier
	{
		get
		{
			return 0.25f;
		}
	}

	// Token: 0x0600172C RID: 5932 RVA: 0x000A1010 File Offset: 0x0009F210
	public DicePalaceMainLevelCard Create(Vector3 pos, LevelProperties.DicePalaceMain.Cards properties, bool onLeft)
	{
		DicePalaceMainLevelCard dicePalaceMainLevelCard = base.Create() as DicePalaceMainLevelCard;
		dicePalaceMainLevelCard.properties = properties;
		dicePalaceMainLevelCard.transform.position = pos;
		dicePalaceMainLevelCard.onLeft = onLeft;
		return dicePalaceMainLevelCard;
	}

	// Token: 0x0600172D RID: 5933 RVA: 0x000A1044 File Offset: 0x0009F244
	public override void Start()
	{
		base.Start();
		this.direction = ((!this.onLeft) ? (-base.transform.right) : base.transform.right);
		if (base.CanParry && (PlayerManager.GetPlayer(PlayerId.PlayerOne).stats.isChalice || (PlayerManager.Multiplayer && PlayerManager.GetPlayer(PlayerId.PlayerTwo).stats.isChalice)))
		{
			this.nextRisingHeart = Random.Range(0, this.risingHeartAnimator.Length);
			this.chaliceParryableHearts.SetActive(true);
			base.StartCoroutine(this.rising_hearts_cr());
		}
	}

	// Token: 0x0600172E RID: 5934 RVA: 0x00013BDB File Offset: 0x00011DDB
	public override void OnCollisionPlayer(GameObject hit, CollisionPhase phase)
	{
		base.OnCollisionPlayer(hit, phase);
		if (phase != CollisionPhase.Exit)
		{
			this.damageDealer.DealDamage(hit);
		}
	}

	// Token: 0x0600172F RID: 5935 RVA: 0x00013BF9 File Offset: 0x00011DF9
	public override void Update()
	{
		base.Update();
		if (this.damageDealer != null)
		{
			this.damageDealer.Update();
		}
	}

	// Token: 0x06001730 RID: 5936 RVA: 0x000A10F4 File Offset: 0x0009F2F4
	public override void FixedUpdate()
	{
		base.FixedUpdate();
		base.transform.position += this.direction * this.properties.cardSpeed * CupheadTime.FixedDelta;
		if (base.CanParry && this.risingHeartRenderer[0].sortingOrder == -1 && Mathf.Abs(base.transform.position.x) < 230f)
		{
			for (int i = 0; i < this.risingHeartRenderer.Length; i++)
			{
				this.risingHeartRenderer[i].sortingOrder = 2;
			}
		}
	}

	// Token: 0x06001731 RID: 5937 RVA: 0x000A11A4 File Offset: 0x0009F3A4
	public IEnumerator rising_hearts_cr()
	{
		this.risingHeartAnimator[this.nextRisingHeart].Play(Random.Range(0, 6).ToString(), 0, 0.25f);
		this.nextRisingHeart = (this.nextRisingHeart + 1) % this.risingHeartAnimator.Length;
		this.risingHeartAnimator[this.nextRisingHeart].Play(Random.Range(0, 6).ToString(), 0, 0.5f);
		this.nextRisingHeart = (this.nextRisingHeart + 1) % this.risingHeartAnimator.Length;
		this.risingHeartAnimator[this.nextRisingHeart].Play(Random.Range(0, 6).ToString(), 0, 0.75f);
		this.nextRisingHeart = (this.nextRisingHeart + 1) % this.risingHeartAnimator.Length;
		for (;;)
		{
			yield return CupheadTime.WaitForSeconds(this, this.risingHeartSpawnTimeRange.RandomFloat());
			this.risingHeartAnimator[this.nextRisingHeart].Play(Random.Range(0, 6).ToString());
			this.nextRisingHeart = (this.nextRisingHeart + 1) % this.risingHeartAnimator.Length;
		}
		yield break;
	}

	// Token: 0x06001732 RID: 5938 RVA: 0x00013C17 File Offset: 0x00011E17
	public override void OnParry(AbstractPlayerController player)
	{
		this.SetParryable(false);
		base.StartCoroutine(this.parryCooldown_cr());
	}

	// Token: 0x06001733 RID: 5939 RVA: 0x000A11C0 File Offset: 0x0009F3C0
	public IEnumerator parryCooldown_cr()
	{
		float t = 0f;
		while (t < this.coolDown)
		{
			t += CupheadTime.Delta;
			yield return null;
		}
		this.SetParryable(true);
		yield return null;
		yield break;
	}

	// Token: 0x040012DA RID: 4826
	public LevelProperties.DicePalaceMain.Cards properties;

	// Token: 0x040012DB RID: 4827
	public bool onLeft;

	// Token: 0x040012DC RID: 4828
	public Vector3 direction;

	// Token: 0x040012DD RID: 4829
	[SerializeField]
	public float coolDown = 0.4f;

	// Token: 0x040012DE RID: 4830
	[SerializeField]
	public GameObject chaliceParryableHearts;

	// Token: 0x040012DF RID: 4831
	[SerializeField]
	public Animator[] risingHeartAnimator;

	// Token: 0x040012E0 RID: 4832
	public int nextRisingHeart;

	// Token: 0x040012E1 RID: 4833
	[SerializeField]
	public SpriteRenderer[] risingHeartRenderer;

	// Token: 0x040012E2 RID: 4834
	[SerializeField]
	public MinMax risingHeartSpawnTimeRange = new MinMax(0.1667f, 0.2333f);
}
