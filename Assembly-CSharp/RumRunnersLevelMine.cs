using System;
using System.Collections;
using UnityEngine;

// Token: 0x02000348 RID: 840
public class RumRunnersLevelMine : AbstractProjectile
{
	// Token: 0x17000308 RID: 776
	// (get) Token: 0x060024C6 RID: 9414 RVA: 0x0001F105 File Offset: 0x0001D305
	// (set) Token: 0x060024C7 RID: 9415 RVA: 0x0001F10D File Offset: 0x0001D30D
	public int xPos { get; set; }

	// Token: 0x17000309 RID: 777
	// (get) Token: 0x060024C8 RID: 9416 RVA: 0x0001F116 File Offset: 0x0001D316
	// (set) Token: 0x060024C9 RID: 9417 RVA: 0x0001F11E File Offset: 0x0001D31E
	public int yPos { get; set; }

	// Token: 0x1700030A RID: 778
	// (get) Token: 0x060024CA RID: 9418 RVA: 0x0001F127 File Offset: 0x0001D327
	// (set) Token: 0x060024CB RID: 9419 RVA: 0x0001F12F File Offset: 0x0001D32F
	public int endPhaseExplodePriority { get; set; }

	// Token: 0x1700030B RID: 779
	// (get) Token: 0x060024CC RID: 9420 RVA: 0x0001F138 File Offset: 0x0001D338
	public override float DestroyLifetime
	{
		get
		{
			return 0f;
		}
	}

	// Token: 0x060024CD RID: 9421 RVA: 0x000C4BC8 File Offset: 0x000C2DC8
	public RumRunnersLevelMine Init(Vector3 targetPos, LevelProperties.RumRunners.Mine properties, RumRunnersLevelSpider parent, int xPos, int yPos)
	{
		base.ResetLifetime();
		base.ResetDistance();
		base.transform.position = new Vector3(targetPos.x, 800f, -targetPos.y * 1E-05f);
		this.targetPos = targetPos;
		this.targetPos.z = -targetPos.y * 1E-05f;
		this.xPos = xPos;
		this.yPos = yPos;
		if (this.xPos == 3 && this.yPos == 2)
		{
			this.endPhaseExplodePriority = 0;
		}
		else if (this.xPos == 2)
		{
			this.endPhaseExplodePriority = 1;
		}
		else
		{
			this.endPhaseExplodePriority = 2;
		}
		this.properties = properties;
		base.animator.Play("Drop");
		base.GetComponent<SpriteRenderer>().enabled = true;
		this.parent = parent;
		base.StartCoroutine(this.lifetime_cr());
		this.MoveDown();
		this.webRenderer.transform.eulerAngles = new Vector3(0f, 0f, (float)Random.Range(0, 360));
		this.explosionRenderer.transform.eulerAngles = new Vector3(0f, 0f, (float)Random.Range(0, 360));
		this.smokeRenderer.transform.eulerAngles = new Vector3(0f, 0f, (float)Random.Range(0, 360));
		this.webRenderer.transform.localScale = new Vector3((float)MathUtils.PlusOrMinus(), (float)MathUtils.PlusOrMinus(), 1f);
		this.explosionRenderer.transform.localScale = new Vector3((float)MathUtils.PlusOrMinus(), (float)MathUtils.PlusOrMinus(), 1f);
		this.smokeRenderer.transform.localScale = new Vector3((float)MathUtils.PlusOrMinus(), (float)MathUtils.PlusOrMinus(), 1f);
		if (yPos == 0)
		{
			AudioManager.Play("sfx_dlc_rumrun_mine_drop_high");
			this.emitAudioFromObject.Add("sfx_dlc_rumrun_mine_drop_high");
		}
		else if (yPos == 1)
		{
			AudioManager.Play("sfx_dlc_rumrun_mine_drop_mid");
			this.emitAudioFromObject.Add("sfx_dlc_rumrun_mine_drop_mid");
		}
		else
		{
			AudioManager.Play("sfx_dlc_rumrun_mine_drop_low");
			this.emitAudioFromObject.Add("sfx_dlc_rumrun_mine_drop_low");
		}
		return this;
	}

	// Token: 0x060024CE RID: 9422 RVA: 0x0001F13F File Offset: 0x0001D33F
	public void MoveDown()
	{
		base.StartCoroutine(this.move_down_cr());
	}

	// Token: 0x060024CF RID: 9423 RVA: 0x000C4E18 File Offset: 0x000C3018
	public IEnumerator move_down_cr()
	{
		base.transform.position = this.targetPos;
		yield return base.animator.WaitForAnimationToEnd(this, "Drop", false, true);
		base.StartCoroutine(this.check_distance_cr());
		yield break;
	}

	// Token: 0x060024D0 RID: 9424 RVA: 0x000C4E34 File Offset: 0x000C3034
	public IEnumerator check_distance_cr()
	{
		this.damageDealer.SetDamage(this.properties.mineBossDamage);
		this.damageDealer.SetRate(0f);
		this.checkingPlayers = true;
		while (this.checkingPlayers)
		{
			if (this.parent && this.parent.moving && !this.parent.isSummoning && Vector3.Distance(this.parent.transform.position, base.transform.position) < this.properties.mineDistToExplode)
			{
				base.animator.Play((!this.parent.goingLeft) ? "SwingRight" : "SwingLeft");
			}
			LevelPlayerController player = PlayerManager.GetPlayer(PlayerId.PlayerOne) as LevelPlayerController;
			LevelPlayerController player2 = PlayerManager.GetPlayer(PlayerId.PlayerTwo) as LevelPlayerController;
			float player1Dist = Vector3.Distance(player.center, base.transform.position);
			if (!player.IsDead && player1Dist < this.properties.mineDistToExplode)
			{
				this.checkingPlayers = false;
			}
			if (player2 != null)
			{
				float num = Vector3.Distance(player2.center, base.transform.position);
				if (!player2.IsDead && num < this.properties.mineDistToExplode)
				{
					this.checkingPlayers = false;
				}
			}
			yield return null;
		}
		if (!this.exploding)
		{
			base.StartCoroutine(this.explosion_cr(false));
		}
		yield break;
	}

	// Token: 0x060024D1 RID: 9425 RVA: 0x000C4E50 File Offset: 0x000C3050
	public IEnumerator explosion_cr(bool timedOut)
	{
		this.exploding = true;
		base.animator.Play("PreExplode");
		AudioManager.Play("sfx_dlc_rumrun_mine_preexplode");
		this.emitAudioFromObject.Add("sfx_dlc_rumrun_mine_preexplode");
		yield return CupheadTime.WaitForSeconds(this, this.properties.mineExplosionWarning * (float)((!timedOut) ? 1 : 2));
		AudioManager.Play("sfx_dlc_rumrun_mine_explode");
		this.emitAudioFromObject.Add("sfx_dlc_rumrun_mine_explode");
		base.animator.Play("Explode");
		yield break;
	}

	// Token: 0x060024D2 RID: 9426 RVA: 0x0001F14E File Offset: 0x0001D34E
	public override void OnCollisionEnemy(GameObject hit, CollisionPhase phase)
	{
		base.OnCollisionPlayer(hit, phase);
		if (phase != CollisionPhase.Exit)
		{
			this.damageDealer.DealDamage(hit);
		}
	}

	// Token: 0x060024D3 RID: 9427 RVA: 0x0001F16C File Offset: 0x0001D36C
	public override void OnCollisionPlayer(GameObject hit, CollisionPhase phase)
	{
		base.OnCollisionPlayer(hit, phase);
		if (phase != CollisionPhase.Exit)
		{
			this.damageDealer.DealDamage(hit);
		}
	}

	// Token: 0x060024D4 RID: 9428 RVA: 0x0001F18A File Offset: 0x0001D38A
	public void SetTimer(float t)
	{
		if (!this.exploding)
		{
			this.timer = t;
		}
	}

	// Token: 0x060024D5 RID: 9429 RVA: 0x000C4E74 File Offset: 0x000C3074
	public IEnumerator lifetime_cr()
	{
		this.timer = this.properties.mineTimer;
		while (this.timer > 0f)
		{
			this.timer -= CupheadTime.Delta;
			yield return null;
		}
		this.checkingPlayers = false;
		if (!this.exploding)
		{
			base.StartCoroutine(this.explosion_cr(true));
		}
		yield break;
	}

	// Token: 0x060024D6 RID: 9430 RVA: 0x0001F19E File Offset: 0x0001D39E
	public void Death()
	{
		this.StopAllCoroutines();
		this.Recycle<RumRunnersLevelMine>();
	}

	// Token: 0x04001E6D RID: 7789
	public const float START_HEIGHT = 800f;

	// Token: 0x04001E71 RID: 7793
	public LevelProperties.RumRunners.Mine properties;

	// Token: 0x04001E72 RID: 7794
	public RumRunnersLevelSpider parent;

	// Token: 0x04001E73 RID: 7795
	public Vector3 targetPos;

	// Token: 0x04001E74 RID: 7796
	public bool checkingPlayers;

	// Token: 0x04001E75 RID: 7797
	public bool exploding;

	// Token: 0x04001E76 RID: 7798
	public float timer;

	// Token: 0x04001E77 RID: 7799
	[SerializeField]
	public GameObject webRenderer;

	// Token: 0x04001E78 RID: 7800
	[SerializeField]
	public GameObject explosionRenderer;

	// Token: 0x04001E79 RID: 7801
	[SerializeField]
	public GameObject smokeRenderer;
}
