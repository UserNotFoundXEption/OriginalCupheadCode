using System;
using System.Collections;
using UnityEngine;

// Token: 0x020002E3 RID: 739
public class OldManLevelPuppetBall : AbstractProjectile
{
	// Token: 0x170002E3 RID: 739
	// (get) Token: 0x060020C2 RID: 8386 RVA: 0x0001BE2C File Offset: 0x0001A02C
	// (set) Token: 0x060020C3 RID: 8387 RVA: 0x0001BE34 File Offset: 0x0001A034
	public bool readyToCatch { get; set; }

	// Token: 0x170002E4 RID: 740
	// (get) Token: 0x060020C4 RID: 8388 RVA: 0x0001BE3D File Offset: 0x0001A03D
	// (set) Token: 0x060020C5 RID: 8389 RVA: 0x0001BE45 File Offset: 0x0001A045
	public bool isMoving { get; set; }

	// Token: 0x060020C6 RID: 8390 RVA: 0x000B8A9C File Offset: 0x000B6C9C
	public virtual OldManLevelPuppetBall Init(Vector3 startPos, Vector3 platformPos, Vector3 endPos, LevelProperties.OldMan.Hands properties)
	{
		base.ResetLifetime();
		base.ResetDistance();
		base.transform.position = startPos;
		base.transform.localScale = new Vector3(Mathf.Sign(endPos.x - startPos.x), 1f);
		this.startPos = startPos;
		this.endPos = endPos;
		this.platformPos = platformPos + Vector3.up * -10f;
		this.properties = properties;
		this.Move();
		base.animator.Play("Idle", 0, 0.7647059f);
		return this;
	}

	// Token: 0x060020C7 RID: 8391 RVA: 0x0001BE4E File Offset: 0x0001A04E
	public override void OnDestroy()
	{
		base.OnDestroy();
		this.WORKAROUND_NullifyFields();
	}

	// Token: 0x060020C8 RID: 8392 RVA: 0x0001BE5C File Offset: 0x0001A05C
	public override void OnCollisionPlayer(GameObject hit, CollisionPhase phase)
	{
		base.OnCollisionPlayer(hit, phase);
		if (phase != CollisionPhase.Exit)
		{
			this.damageDealer.DealDamage(hit);
		}
		this.SFX_OMM_P2_DamagePlayerCheer();
	}

	// Token: 0x060020C9 RID: 8393 RVA: 0x0001BE80 File Offset: 0x0001A080
	public void Move()
	{
		this.isMoving = true;
		base.StartCoroutine(this.move_cr());
	}

	// Token: 0x060020CA RID: 8394 RVA: 0x000B8B38 File Offset: 0x000B6D38
	public IEnumerator move_cr()
	{
		this.readyToCatch = false;
		float STRAIGHT_BOUNCE_CUTOFF = 0.66f;
		YieldInstruction wait = new WaitForFixedUpdate();
		float percentage = Mathf.Abs(this.startPos.x - this.platformPos.x) / Mathf.Abs(this.startPos.x - this.endPos.x);
		float newX = base.transform.position.x;
		float newY = base.transform.position.y;
		float direction = Mathf.Sign(this.endPos.x - this.startPos.x);
		float xTotalDist = Mathf.Abs(this.startPos.x - this.platformPos.x);
		float yRad = base.transform.position.y - (this.platformPos.y + this.size);
		while (base.transform.position.y > this.platformPos.y + this.size + 20f)
		{
			newX += direction * CupheadTime.FixedDelta * this.properties.ballSpeed * ((!this.puppetDead) ? 1f : 0f);
			float xDist = Mathf.Abs(newX - this.startPos.x);
			newY = ((percentage >= 1f - STRAIGHT_BOUNCE_CUTOFF) ? (this.platformPos.y + this.size + yRad * Mathf.Cos(xDist / xTotalDist * 1.57079637f)) : Mathf.Lerp(this.startPos.y, this.platformPos.y + this.size, xDist / xTotalDist));
			base.transform.SetPosition(new float?(newX), new float?(newY), null);
			yield return wait;
		}
		base.transform.SetPosition(new float?(this.platformPos.x), new float?(this.platformPos.y + this.size), null);
		newX = base.transform.position.x;
		xTotalDist = Mathf.Abs(this.platformPos.x - this.endPos.x);
		yRad = this.endPos.y - base.transform.position.y;
		base.animator.SetTrigger("OnBounce");
		yield return base.animator.WaitForAnimationToEnd(this, "Bounce", false, true);
		while (Mathf.Sign(this.endPos.x - base.transform.position.x) == direction || this.puppetDead)
		{
			newX += direction * CupheadTime.FixedDelta * this.properties.ballSpeed * ((!this.puppetDead) ? 1f : 0f);
			float xDist = Mathf.Abs(newX - this.platformPos.x);
			newY = ((percentage <= STRAIGHT_BOUNCE_CUTOFF) ? (this.platformPos.y + this.size + yRad * Mathf.Sin(xDist / xTotalDist * 1.57079637f)) : Mathf.Lerp(this.platformPos.y + this.size, this.endPos.y, xDist / xTotalDist));
			base.transform.SetPosition(new float?(newX), new float?(newY), null);
			if (!this.readyToCatch && xDist / xTotalDist >= 0.9f && !this.puppetDead)
			{
				this.readyToCatch = true;
			}
			if (this.puppetDead && (base.transform.position.x > 1140f || base.transform.position.x < -1140f))
			{
				Object.Destroy(base.gameObject);
			}
			yield return wait;
		}
		yield break;
	}

	// Token: 0x060020CB RID: 8395 RVA: 0x000B8B54 File Offset: 0x000B6D54
	public void LateUpdate()
	{
		this.shadowRend.transform.position = new Vector3(base.transform.position.x, this.platformPos.y + this.size);
		if (base.transform.position.y < this.platformPos.y + this.size + this.shadowRange)
		{
			float num = Mathf.Lerp((float)(this.shadowSprites.Length - 1), 0f, Mathf.InverseLerp(this.platformPos.y + this.size, this.platformPos.y + this.size + this.shadowRange, base.transform.position.y));
			this.shadowRend.sprite = this.shadowSprites[(int)num];
		}
	}

	// Token: 0x060020CC RID: 8396 RVA: 0x0001BE96 File Offset: 0x0001A096
	public void GetCaught()
	{
		this.isMoving = false;
		this.Recycle<OldManLevelPuppetBall>();
	}

	// Token: 0x060020CD RID: 8397 RVA: 0x000B8C3C File Offset: 0x000B6E3C
	public void Explode()
	{
		this.puppetDead = true;
		this.shadowRend.enabled = false;
		SpriteRenderer component = base.GetComponent<SpriteRenderer>();
		component.sortingLayerName = "Effects";
		component.sortingOrder = 100;
		base.animator.Play("Explode");
		for (int i = 0; i < 12; i++)
		{
			this.coinPrefab.Create(base.transform.position + MathUtils.AngleToDirection((float)(Random.Range(0, 360) * Random.Range(0, 50))));
		}
		for (int j = 0; j < 15; j++)
		{
			this.featherPrefab.Create(base.transform.position + MathUtils.AngleToDirection((float)(Random.Range(0, 360) * Random.Range(0, 50))));
		}
	}

	// Token: 0x060020CE RID: 8398 RVA: 0x0001BEA5 File Offset: 0x0001A0A5
	public void SFX_OMM_P2_DamagePlayerCheer()
	{
		AudioManager.Play("sfx_dlc_omm_p2_puppet_ball_damageplayercheer");
		this.emitAudioFromObject.Add("sfx_dlc_omm_p2_puppet_ball_damageplayercheer");
	}

	// Token: 0x060020CF RID: 8399 RVA: 0x0001BEC1 File Offset: 0x0001A0C1
	public void AnimationEvent_SFX_OMM_P2_PuppetBallBounce()
	{
		AudioManager.Play("sfx_dlc_omm_p2_puppet_ball_bounce");
		this.emitAudioFromObject.Add("sfx_dlc_omm_p2_puppet_ball_bounce");
	}

	// Token: 0x060020D0 RID: 8400 RVA: 0x0001BEDD File Offset: 0x0001A0DD
	public void AnimationEvent_ExplodeEnd()
	{
		Object.Destroy(base.gameObject);
	}

	// Token: 0x060020D1 RID: 8401 RVA: 0x0001BEEA File Offset: 0x0001A0EA
	public void WORKAROUND_NullifyFields()
	{
		this.sprite = null;
		this.shadowRend = null;
		this.shadowSprites = null;
		this.coinPrefab = null;
		this.featherPrefab = null;
	}

	// Token: 0x04001ADF RID: 6879
	public const float GROUND_Y_OFFSET = -10f;

	// Token: 0x04001AE0 RID: 6880
	public const float HIT_GROUND_OFFSET = 20f;

	// Token: 0x04001AE3 RID: 6883
	public LevelProperties.OldMan.Hands properties;

	// Token: 0x04001AE4 RID: 6884
	public Vector3 startPos;

	// Token: 0x04001AE5 RID: 6885
	public Vector3 endPos;

	// Token: 0x04001AE6 RID: 6886
	public Vector3 platformPos;

	// Token: 0x04001AE7 RID: 6887
	public float size = 50f;

	// Token: 0x04001AE8 RID: 6888
	[SerializeField]
	public float shadowRange = 100f;

	// Token: 0x04001AE9 RID: 6889
	[SerializeField]
	public SpriteRenderer sprite;

	// Token: 0x04001AEA RID: 6890
	[SerializeField]
	public SpriteRenderer shadowRend;

	// Token: 0x04001AEB RID: 6891
	[SerializeField]
	public Sprite[] shadowSprites;

	// Token: 0x04001AEC RID: 6892
	[SerializeField]
	public Effect coinPrefab;

	// Token: 0x04001AED RID: 6893
	[SerializeField]
	public Effect featherPrefab;

	// Token: 0x04001AEE RID: 6894
	public bool puppetDead;
}
