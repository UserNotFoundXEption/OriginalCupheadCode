using System;
using System.Collections;
using UnityEngine;

// Token: 0x02000134 RID: 308
public class AirplaneLevelTerrier : AbstractCollidableObject
{
	// Token: 0x17000229 RID: 553
	// (get) Token: 0x06000E8E RID: 3726 RVA: 0x0000C592 File Offset: 0x0000A792
	// (set) Token: 0x06000E8F RID: 3727 RVA: 0x0000C59A File Offset: 0x0000A79A
	public bool IsDead { get; set; }

	// Token: 0x1700022A RID: 554
	// (get) Token: 0x06000E90 RID: 3728 RVA: 0x0000C5A3 File Offset: 0x0000A7A3
	// (set) Token: 0x06000E91 RID: 3729 RVA: 0x0000C5AB File Offset: 0x0000A7AB
	public bool ReadyToMove { get; set; }

	// Token: 0x06000E92 RID: 3730 RVA: 0x0000C5B4 File Offset: 0x0000A7B4
	public void Start()
	{
		this.damageDealer = DamageDealer.NewEnemy();
		this.damageReceiver = base.GetComponent<DamageReceiver>();
		this.damageReceiver.OnDamageTaken += this.OnDamageTaken;
	}

	// Token: 0x06000E93 RID: 3731 RVA: 0x0000C5E4 File Offset: 0x0000A7E4
	public override void OnDestroy()
	{
		base.OnDestroy();
		this.WORKAROUND_NullifyFields();
	}

	// Token: 0x06000E94 RID: 3732 RVA: 0x0008B51C File Offset: 0x0008971C
	public void Init(Transform pivotPoint, float angle, LevelProperties.Airplane.Terriers properties, float hp, float pivotOffsetX, float pivotOffsetY, bool isClockwise, int index)
	{
		this.angle = angle;
		this.pivotPoint = pivotPoint;
		this.pivotOffset = new Vector2(pivotOffsetX, pivotOffsetY);
		this.properties = properties;
		this.hp = hp;
		this.smokingThreshold = hp * properties.secretHPPercentage;
		this.isClockwise = isClockwise;
		this.index = index;
		this.wobbleTimer = (float)index;
		base.StartCoroutine(this.setup_dogs_cr());
	}

	// Token: 0x06000E95 RID: 3733 RVA: 0x0000C5F2 File Offset: 0x0000A7F2
	public void OnDamageTaken(DamageDealer.DamageInfo info)
	{
		this.hp -= info.damage;
		if (this.hp <= 0f)
		{
			Level.Current.RegisterMinionKilled();
			this.Die();
		}
	}

	// Token: 0x06000E96 RID: 3734 RVA: 0x0008B58C File Offset: 0x0008978C
	public bool IsSmoking()
	{
		if (!this.isSmoking && this.hp < this.smokingThreshold)
		{
			this.isSmoking = true;
			base.animator.SetTrigger(AirplaneLevelTerrier.OnShockParameterID);
			AudioManager.Play("sfx_dlc_dogfight_p2_terrierjetpack_dmgsmoke");
			this.emitAudioFromObject.Add("sfx_dlc_dogfight_p2_terrierjetpack_dmgsmoke");
		}
		return this.isSmoking;
	}

	// Token: 0x06000E97 RID: 3735 RVA: 0x0000C627 File Offset: 0x0000A827
	public float Health()
	{
		return this.hp;
	}

	// Token: 0x06000E98 RID: 3736 RVA: 0x0000C62F File Offset: 0x0000A82F
	public override void OnCollisionPlayer(GameObject hit, CollisionPhase phase)
	{
		if (phase != CollisionPhase.Exit)
		{
			this.damageDealer.DealDamage(hit);
		}
		base.OnCollisionPlayer(hit, phase);
	}

	// Token: 0x06000E99 RID: 3737 RVA: 0x0000C64D File Offset: 0x0000A84D
	public void Update()
	{
		if (this.damageDealer != null)
		{
			this.damageDealer.Update();
		}
		this.wobbleTimer += this.wobbleSpeed * CupheadTime.Delta;
	}

	// Token: 0x06000E9A RID: 3738 RVA: 0x0008B5EC File Offset: 0x000897EC
	public Vector3 WobblePos()
	{
		return new Vector3(Mathf.Sin(this.wobbleTimer * 3f) * this.wobbleX, Mathf.Sin(this.wobbleTimer * 2f) * this.wobbleY, 0f) * this.wobbleModifier;
	}

	// Token: 0x06000E9B RID: 3739 RVA: 0x0008B640 File Offset: 0x00089840
	public IEnumerator setup_dogs_cr()
	{
		this.rotationOffset = Vector3.zero;
		YieldInstruction wait = new WaitForFixedUpdate();
		int indexToPlay = this.index;
		base.transform.SetScale(new float?((!this.isClockwise) ? Mathf.Abs(base.transform.localScale.x) : (-Mathf.Abs(base.transform.localScale.x))), null, null);
		int num = this.index;
		if (num != 1)
		{
			if (num == 3)
			{
				indexToPlay = ((!this.isClockwise) ? this.index : (indexToPlay = 1));
			}
		}
		else
		{
			indexToPlay = ((!this.isClockwise) ? this.index : (indexToPlay = 3));
		}
		base.animator.Play("Intro_" + indexToPlay);
		int flamePos = indexToPlay * 4;
		if (indexToPlay == 3)
		{
			flamePos = 4;
		}
		this.flame.transform.localPosition = this.flameOffset[flamePos];
		if (indexToPlay == 1)
		{
			this.flame.transform.localPosition = new Vector3(-this.flame.transform.localPosition.x, this.flame.transform.localPosition.y);
		}
		this.angle *= 0.0174532924f;
		this.loopSizeX = 675f;
		this.loopSizeY = 328.5f;
		this.rotationOffset.x = Mathf.Sin(this.angle) * this.loopSizeX;
		this.rotationOffset.y = Mathf.Cos(this.angle) * this.loopSizeY;
		Vector3 startPos = this.pivotPoint.position + this.pivotOffset + this.rotationOffset * 2f;
		Vector3 endPos = this.pivotPoint.position + this.pivotOffset + this.rotationOffset;
		float t = 0f;
		float time = 0.5f;
		base.transform.position = startPos;
		while (t < time)
		{
			t += CupheadTime.FixedDelta;
			base.transform.position = Vector3.Lerp(startPos, endPos, t / time) + this.WobblePos();
			yield return wait;
		}
		base.animator.SetTrigger("ContinueIntro");
		t = 0f;
		while (t < 0.9f)
		{
			t += CupheadTime.FixedDelta;
			base.transform.position = endPos + this.WobblePos();
			this.wobbleModifier = Mathf.Lerp(1f, 0f, t / 0.9f);
			yield return wait;
		}
		this.wobbleModifier = 0f;
		base.animator.SetTrigger("EndIntro");
		this.rotationSpeed = 0f;
		this.ReadyToMove = true;
		yield return base.animator.WaitForAnimationToStart(this, "Idle", false);
		this.introFinished = true;
		((AirplaneLevel)Level.Current).terriersIntroFinished = true;
		yield break;
	}

	// Token: 0x06000E9C RID: 3740 RVA: 0x0008B65C File Offset: 0x0008985C
	public IEnumerator ease_to_full_speed_and_radius_cr()
	{
		float t = 0f;
		YieldInstruction wait = new WaitForFixedUpdate();
		while (t < 1f)
		{
			this.loopSizeX = Mathf.Lerp(675f, 750f, EaseUtils.EaseOutSine(0f, 1f, t / 1f));
			this.loopSizeY = Mathf.Lerp(328.5f, 365f, EaseUtils.EaseOutSine(0f, 1f, t / 1f));
			this.rotationSpeed = Mathf.Lerp(0f, this.properties.rotationTime, EaseUtils.EaseInSine(0f, 1f, t / 1f));
			t += CupheadTime.FixedDelta;
			yield return wait;
		}
		this.loopSizeX = 750f;
		this.loopSizeY = 365f;
		this.rotationSpeed = this.properties.rotationTime;
		yield break;
	}

	// Token: 0x06000E9D RID: 3741 RVA: 0x0000C683 File Offset: 0x0000A883
	public void StartMoving()
	{
		base.StartCoroutine(this.move_in_circle_cr());
		base.StartCoroutine(this.ease_to_full_speed_and_radius_cr());
	}

	// Token: 0x06000E9E RID: 3742 RVA: 0x0008B678 File Offset: 0x00089878
	public IEnumerator move_in_circle_cr()
	{
		this.rotationOffset = Vector3.zero;
		YieldInstruction wait = new WaitForFixedUpdate();
		for (;;)
		{
			this.angle += this.rotationSpeed * CupheadTime.FixedDelta * (float)((!this.isClockwise) ? -1 : 1);
			if (!this.gettingEaten)
			{
				this.rotationOffset.x = Mathf.Sin(this.angle) * this.loopSizeX;
			}
			else
			{
				bool flag = (!this.isClockwise) ? (this.angle < 3.14159274f) : (this.angle > 3.14159274f);
				this.rotationOffset.x = Mathf.Sin(this.angle) * ((!flag) ? this.loopSizeX : 1000f);
				if (flag)
				{
					this.loopSizeY -= CupheadTime.FixedDelta * 50f;
				}
			}
			this.rotationOffset.y = Mathf.Cos(this.angle) * this.loopSizeY;
			Vector3 lastPos = base.transform.position;
			base.transform.position = this.pivotPoint.position + this.pivotOffset;
			base.transform.position += this.rotationOffset;
			this.flame.flipX = (Mathf.Sign(lastPos.x - base.transform.position.x) == -1f);
			if (this.angle > 6.28318548f)
			{
				this.angle -= 6.28318548f;
			}
			if (this.angle < 0f)
			{
				this.angle += 6.28318548f;
			}
			yield return wait;
		}
		yield break;
	}

	// Token: 0x06000E9F RID: 3743 RVA: 0x0008B694 File Offset: 0x00089894
	public Vector3 GetPredictedAttackPos()
	{
		float num = 0.125f;
		float num2 = this.angle + this.properties.rotationTime * num * (float)((!this.isClockwise) ? -1 : 1);
		return this.pivotPoint.position + this.pivotOffset + new Vector2(Mathf.Sin(num2) * 750f, Mathf.Cos(num2) * 365f);
	}

	// Token: 0x06000EA0 RID: 3744 RVA: 0x0008B714 File Offset: 0x00089914
	public void StartAttack(bool isPink, bool isWow)
	{
		this.isPink = isPink;
		this.isWow = isWow;
		if (this.isClockwise)
		{
			base.transform.SetScale(new float?(Mathf.Abs(base.transform.localScale.x)), null, null);
		}
		base.animator.Play("Attack");
		this.SFX_DOGFIGHT_P2_TerrierJetpack_BarkShoot();
	}

	// Token: 0x06000EA1 RID: 3745 RVA: 0x0008B78C File Offset: 0x0008998C
	public void AniEvent_BarkFX()
	{
		this.barkFXRenderer.sortingLayerID = this.rends[this.currentAngle].sortingLayerID;
		this.barkFXRenderer.sortingOrder = this.rends[this.currentAngle].sortingOrder + ((this.currentAngle > 4) ? -1 : 1);
		this.barkFXRenderer.flipX = this.rends[this.currentAngle].flipX;
		this.barkFXRenderer.transform.localPosition = -this.flame.transform.localPosition * 0.5f;
		if (this.currentAngle == 1)
		{
			this.barkFXRenderer.transform.localPosition += new Vector3((float)((!this.barkFXRenderer.flipX) ? 10 : -10), 12f);
		}
		if (this.currentAngle == 2)
		{
			this.barkFXRenderer.transform.localPosition += new Vector3((float)((!this.barkFXRenderer.flipX) ? -10 : 10), 5f);
		}
		this.barkFXRenderer.transform.eulerAngles = new Vector3(0f, 0f, (float)Random.Range(0, 360));
		this.barkFXAnimator.Play((!Rand.Bool()) ? "B" : "A");
	}

	// Token: 0x06000EA2 RID: 3746 RVA: 0x0008B918 File Offset: 0x00089B18
	public void AniEvent_ShootProjectile()
	{
		AbstractPlayerController next = PlayerManager.GetNext();
		Vector3 vector = next.center - this.barkFXRenderer.transform.position;
		float acceleration = Vector3.Magnitude(this.rotationOffset) / 750f;
		AirplaneLevelTerrierBullet airplaneLevelTerrierBullet;
		if (this.isPink)
		{
			airplaneLevelTerrierBullet = this.pinkProjectile.Create(this.barkFXRenderer.transform.position, MathUtils.DirectionToAngle(vector), this.properties.shotSpeed, acceleration);
		}
		else
		{
			airplaneLevelTerrierBullet = this.regularProjectile.Create(this.barkFXRenderer.transform.position, MathUtils.DirectionToAngle(vector), this.properties.shotSpeed, acceleration);
		}
		if (this.isWow)
		{
			airplaneLevelTerrierBullet.PlayWow();
		}
	}

	// Token: 0x06000EA3 RID: 3747 RVA: 0x0008B9EC File Offset: 0x00089BEC
	public void AniEvent_SetScale()
	{
		if (this.isClockwise)
		{
			base.transform.SetScale(new float?(Mathf.Abs(base.transform.localScale.x)), null, null);
		}
	}

	// Token: 0x06000EA4 RID: 3748 RVA: 0x0000C69F File Offset: 0x0000A89F
	public void StartSecret()
	{
		base.GetComponent<Collider2D>().enabled = false;
		base.StartCoroutine(this.move_into_mouth_cr());
	}

	// Token: 0x06000EA5 RID: 3749 RVA: 0x0008BA40 File Offset: 0x00089C40
	public void PrepareForChomp()
	{
		this.gettingEaten = true;
		foreach (SpriteRenderer spriteRenderer in this.rends)
		{
			spriteRenderer.sortingOrder = 2;
			spriteRenderer.sortingLayerName = "Foreground";
		}
	}

	// Token: 0x06000EA6 RID: 3750 RVA: 0x0008BA88 File Offset: 0x00089C88
	public IEnumerator move_into_mouth_cr()
	{
		float t = 0f;
		float startSpeed = this.rotationSpeed;
		YieldInstruction wait = new WaitForFixedUpdate();
		while (t < 1f)
		{
			t += CupheadTime.FixedDelta;
			this.rotationSpeed = Mathf.Lerp(startSpeed, 2.5f, EaseUtils.EaseInSine(0f, 1f, t));
			this.loopSizeX = Mathf.Lerp(750f, 600f, EaseUtils.EaseInSine(0f, 1f, t));
			this.loopSizeY = Mathf.Lerp(365f, 292f, EaseUtils.EaseInSine(0f, 1f, t));
			yield return wait;
		}
		yield break;
	}

	// Token: 0x06000EA7 RID: 3751 RVA: 0x0008BAA4 File Offset: 0x00089CA4
	public void Die()
	{
		this.IsDead = true;
		this.flame.enabled = false;
		this.coll.enabled = false;
		base.animator.Play((!this.lastOne) ? "Death" : "DeathShort");
		this.SFX_DOGFIGHT_P2_TerrierJetpack_Explosion();
		base.StartCoroutine(this.SFX_DOGFIGHT_P2_TerrierJetpack_DeathBark_cr(this.index));
	}

	// Token: 0x06000EA8 RID: 3752 RVA: 0x0000C6BA File Offset: 0x0000A8BA
	public void AniEvent_DeathLayering()
	{
		this.deathRenderer.sortingLayerName = "Background";
		this.deathRenderer.sortingOrder = 0;
	}

	// Token: 0x06000EA9 RID: 3753 RVA: 0x0000C6D8 File Offset: 0x0000A8D8
	public void AniEvent_OnDeath()
	{
		this.StopAllCoroutines();
		Object.Destroy(base.gameObject);
	}

	// Token: 0x06000EAA RID: 3754 RVA: 0x0008BB10 File Offset: 0x00089D10
	public void CheckAllAnimations()
	{
		if (this.gettingEaten)
		{
			return;
		}
		float num = this.angle * 57.29578f;
		if ((num > 344f && num < 360f) || (num < 8.2f && num > 0f))
		{
			this.ChangeAngle(false, 0);
		}
		else if (num > 8.2f && num < 31.8f)
		{
			this.ChangeAngle(true, 1);
		}
		else if (num > 31.8f && num < 55.4f)
		{
			this.ChangeAngle(true, 2);
		}
		else if (num > 55.4f && num < 79f)
		{
			this.ChangeAngle(true, 3);
		}
		else if (num > 79f && num < 102.6f)
		{
			this.ChangeAngle(true, 4);
		}
		else if (num > 102.6f && num < 126.2f)
		{
			this.ChangeAngle(true, 5);
		}
		else if (num > 126.2f && num < 149.8f)
		{
			this.ChangeAngle(true, 6);
		}
		else if (num > 149.8f && num < 164.75f)
		{
			this.ChangeAngle(true, 7);
		}
		else if (num > 164.75f && num < 195.25f)
		{
			this.ChangeAngle(false, 8);
		}
		else if (num > 195.25f && num < 218.85f)
		{
			this.ChangeAngle(false, 7);
		}
		else if (num > 218.85f && num < 242.45f)
		{
			this.ChangeAngle(false, 6);
		}
		else if (num > 242.45f && num < 259f)
		{
			this.ChangeAngle(false, 5);
		}
		else if (num > 259f && num < 282.6f)
		{
			this.ChangeAngle(false, 4);
		}
		else if (num > 282.6f && num < 306.2f)
		{
			this.ChangeAngle(false, 3);
		}
		else if (num > 306.2f && num < 329.8f)
		{
			this.ChangeAngle(false, 2);
		}
		else if (num > 329.8f && num < 344f)
		{
			this.ChangeAngle(false, 1);
		}
	}

	// Token: 0x06000EAB RID: 3755 RVA: 0x0008BD78 File Offset: 0x00089F78
	public void ChangeAngle(bool flipSprite, int layerIndex)
	{
		this.currentAngle = layerIndex;
		if (!this.isCurved != (layerIndex == 3 || layerIndex == 4 || layerIndex == 5))
		{
			this.flameAnimator.Play((layerIndex != 3 && layerIndex != 4 && layerIndex != 5) ? "Curve" : "Straight", 0, this.flameAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime);
			this.isCurved = !this.isCurved;
		}
		foreach (GameObject gameObject in this.terrierLayers)
		{
			gameObject.SetActive(gameObject == this.terrierLayers[layerIndex]);
		}
		this.rends[layerIndex].flipX = flipSprite;
	}

	// Token: 0x06000EAC RID: 3756 RVA: 0x0008BE44 File Offset: 0x0008A044
	public void FixedUpdate()
	{
		if (!this.IsDead)
		{
			this.smokeTimer += CupheadTime.FixedDelta * ((!this.introFinished) ? 0.2f : ((!this.gettingEaten) ? 1f : 0.1f));
			if (this.smokeTimer > this.smokeDelay)
			{
				this.smokeTimer -= this.smokeDelay;
				((AirplaneLevel)Level.Current).CreateSmokeFX(this.flame.transform.position, (!this.introFinished) ? (MathUtils.AngleToDirection(this.flame.transform.eulerAngles.z - 90f) * 300f) : Vector2.zero, this.hp < this.smokingThreshold, this.rends[this.currentAngle].sortingLayerID, (this.currentAngle > 4) ? 30 : -1);
			}
		}
	}

	// Token: 0x06000EAD RID: 3757 RVA: 0x0008BF5C File Offset: 0x0008A15C
	public void LateUpdate()
	{
		if (this.rends[9].sprite == null)
		{
			this.CheckAllAnimations();
		}
		if (this.introFinished)
		{
			this.flame.sortingLayerID = this.rends[this.currentAngle].sortingLayerID;
			this.flame.sortingOrder = this.rends[this.currentAngle].sortingOrder - 1;
			this.flame.transform.localPosition = new Vector3(this.flameOffset[this.currentAngle].x * (float)((!this.rends[this.currentAngle].flipX) ? 1 : -1), this.flameOffset[this.currentAngle].y);
		}
	}

	// Token: 0x06000EAE RID: 3758 RVA: 0x0000C6EB File Offset: 0x0000A8EB
	public float RelativeAngle()
	{
		if (!this.isClockwise)
		{
			return 6.28318548f - this.angle;
		}
		return this.angle;
	}

	// Token: 0x06000EAF RID: 3759 RVA: 0x0008C030 File Offset: 0x0008A230
	public override void OnDrawGizmos()
	{
		base.OnDrawGizmos();
		float[] array = new float[]
		{
			344f,
			8.2f,
			31.8f,
			55.4f,
			329.8f,
			306.2f,
			282.6f,
			164.75f,
			242.45f,
			218.85f,
			195.25f,
			102.6f,
			126.2f,
			149.8f,
			79f,
			259f
		};
		Vector3 vector = Vector3.zero;
		float num = 400f;
		for (int i = 0; i < array.Length; i++)
		{
			vector = MathUtils.AngleToDirection(array[i] + 90f);
			if (array[i] == 344f || array[i] == 164.75f || array[i] == 259f || array[i] == 79f)
			{
				Gizmos.color = Color.blue;
			}
			else if (array[i] == 195.25f || array[i] == 282.6f || array[i] == 8.2f || array[i] == 102.6f)
			{
				Gizmos.color = Color.green;
			}
			else
			{
				Gizmos.color = Color.red;
			}
			Gizmos.DrawLine(Vector3.zero, vector * num);
		}
	}

	// Token: 0x06000EB0 RID: 3760 RVA: 0x0000C70B File Offset: 0x0000A90B
	public void SFX_DOGFIGHT_P2_TerrierJetpack_BarkShoot()
	{
		AudioManager.Play("sfx_dlc_dogfight_p2_terrierjetpack_barkshoot");
	}

	// Token: 0x06000EB1 RID: 3761 RVA: 0x0000C717 File Offset: 0x0000A917
	public void SFX_DOGFIGHT_P2_TerrierJetpack_Explosion()
	{
		AudioManager.Play("sfx_dlc_dogfight_p2_terrierjetpack_explosion");
	}

	// Token: 0x06000EB2 RID: 3762 RVA: 0x0008C12C File Offset: 0x0008A32C
	public IEnumerator SFX_DOGFIGHT_P2_TerrierJetpack_DeathBark_cr(int id)
	{
		yield return CupheadTime.WaitForSeconds(this, 0.5f);
		AudioManager.Play("sfx_dlc_dogfight_p2_terrierjetpack_dmgdeath_0" + id);
		yield break;
	}

	// Token: 0x06000EB3 RID: 3763 RVA: 0x0008C150 File Offset: 0x0008A350
	public void WORKAROUND_NullifyFields()
	{
		this.coll = null;
		this.regularProjectile = null;
		this.pinkProjectile = null;
		this.terrierLayers = null;
		this.deathRenderer = null;
		this.rends = null;
		this.flameOffset = null;
		this.flame = null;
		this.flameAnimator = null;
		this.barkFXRenderer = null;
		this.barkFXAnimator = null;
		this.pivotPoint = null;
		this.damageDealer = null;
	}

	// Token: 0x04000BCD RID: 3021
	public static readonly int OnShockParameterID = Animator.StringToHash("OnShock");

	// Token: 0x04000BD0 RID: 3024
	public const float NINETY_DEGREES = 90f;

	// Token: 0x04000BD1 RID: 3025
	public const float THREE_SIXTY = 360f;

	// Token: 0x04000BD2 RID: 3026
	public const float LOOP_SIZE_Y = 365f;

	// Token: 0x04000BD3 RID: 3027
	public const float LOOP_SIZE_X = 750f;

	// Token: 0x04000BD4 RID: 3028
	public const float LOOP_SIZE_X_SECRET_INTRO = 1000f;

	// Token: 0x04000BD5 RID: 3029
	public const float LOOP_SIZE_INTRO_MOD = 0.9f;

	// Token: 0x04000BD6 RID: 3030
	public const float TIME_TO_FULL_LOOP_SIZE = 1f;

	// Token: 0x04000BD7 RID: 3031
	public const float UP = 344f;

	// Token: 0x04000BD8 RID: 3032
	public const float UP_RIGHT_1 = 8.2f;

	// Token: 0x04000BD9 RID: 3033
	public const float UP_RIGHT_2 = 31.8f;

	// Token: 0x04000BDA RID: 3034
	public const float UP_RIGHT_3 = 55.4f;

	// Token: 0x04000BDB RID: 3035
	public const float RIGHT = 79f;

	// Token: 0x04000BDC RID: 3036
	public const float DOWN_RIGHT_1 = 102.6f;

	// Token: 0x04000BDD RID: 3037
	public const float DOWN_RIGHT_2 = 126.2f;

	// Token: 0x04000BDE RID: 3038
	public const float DOWN_RIGHT_3 = 149.8f;

	// Token: 0x04000BDF RID: 3039
	public const float DOWN = 164.75f;

	// Token: 0x04000BE0 RID: 3040
	public const float DOWN_LEFT_1 = 242.45f;

	// Token: 0x04000BE1 RID: 3041
	public const float DOWN_LEFT_2 = 218.85f;

	// Token: 0x04000BE2 RID: 3042
	public const float DOWN_LEFT_3 = 195.25f;

	// Token: 0x04000BE3 RID: 3043
	public const float LEFT = 259f;

	// Token: 0x04000BE4 RID: 3044
	public const float UP_LEFT_1 = 329.8f;

	// Token: 0x04000BE5 RID: 3045
	public const float UP_LEFT_2 = 306.2f;

	// Token: 0x04000BE6 RID: 3046
	public const float UP_LEFT_3 = 282.6f;

	// Token: 0x04000BE7 RID: 3047
	[SerializeField]
	public BoxCollider2D coll;

	// Token: 0x04000BE8 RID: 3048
	[SerializeField]
	public AirplaneLevelTerrierBullet regularProjectile;

	// Token: 0x04000BE9 RID: 3049
	[SerializeField]
	public AirplaneLevelTerrierBullet pinkProjectile;

	// Token: 0x04000BEA RID: 3050
	[SerializeField]
	public GameObject[] terrierLayers;

	// Token: 0x04000BEB RID: 3051
	[SerializeField]
	public SpriteRenderer deathRenderer;

	// Token: 0x04000BEC RID: 3052
	[SerializeField]
	public SpriteRenderer[] rends;

	// Token: 0x04000BED RID: 3053
	[SerializeField]
	public Vector3[] flameOffset;

	// Token: 0x04000BEE RID: 3054
	[SerializeField]
	public SpriteRenderer flame;

	// Token: 0x04000BEF RID: 3055
	[SerializeField]
	public Animator flameAnimator;

	// Token: 0x04000BF0 RID: 3056
	[SerializeField]
	public SpriteRenderer barkFXRenderer;

	// Token: 0x04000BF1 RID: 3057
	[SerializeField]
	public Animator barkFXAnimator;

	// Token: 0x04000BF2 RID: 3058
	public LevelProperties.Airplane.Terriers properties;

	// Token: 0x04000BF3 RID: 3059
	public float angle;

	// Token: 0x04000BF4 RID: 3060
	public float hp;

	// Token: 0x04000BF5 RID: 3061
	public float smokingThreshold;

	// Token: 0x04000BF6 RID: 3062
	public int index;

	// Token: 0x04000BF7 RID: 3063
	public bool isClockwise;

	// Token: 0x04000BF8 RID: 3064
	public bool isPink;

	// Token: 0x04000BF9 RID: 3065
	public bool isWow;

	// Token: 0x04000BFA RID: 3066
	public bool isSmoking;

	// Token: 0x04000BFB RID: 3067
	public bool gettingEaten;

	// Token: 0x04000BFC RID: 3068
	public Transform pivotPoint;

	// Token: 0x04000BFD RID: 3069
	public DamageDealer damageDealer;

	// Token: 0x04000BFE RID: 3070
	public DamageReceiver damageReceiver;

	// Token: 0x04000BFF RID: 3071
	public Vector2 pivotOffset;

	// Token: 0x04000C00 RID: 3072
	[SerializeField]
	public float wobbleX = 10f;

	// Token: 0x04000C01 RID: 3073
	[SerializeField]
	public float wobbleY = 10f;

	// Token: 0x04000C02 RID: 3074
	[SerializeField]
	public float wobbleSpeed = 1f;

	// Token: 0x04000C03 RID: 3075
	public float wobbleTimer;

	// Token: 0x04000C04 RID: 3076
	public float wobbleModifier = 1f;

	// Token: 0x04000C05 RID: 3077
	public float rotationSpeed;

	// Token: 0x04000C06 RID: 3078
	public float loopSizeX;

	// Token: 0x04000C07 RID: 3079
	public float loopSizeY;

	// Token: 0x04000C08 RID: 3080
	public bool isCurved;

	// Token: 0x04000C09 RID: 3081
	public int currentAngle;

	// Token: 0x04000C0A RID: 3082
	public float smokeDelay = 0.02f;

	// Token: 0x04000C0B RID: 3083
	public float smokeTimer;

	// Token: 0x04000C0C RID: 3084
	public bool introFinished;

	// Token: 0x04000C0D RID: 3085
	public bool lastOne;

	// Token: 0x04000C0E RID: 3086
	public Vector3 rotationOffset;
}
