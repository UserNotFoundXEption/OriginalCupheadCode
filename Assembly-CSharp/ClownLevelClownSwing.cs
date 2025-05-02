using System;
using System.Collections;
using UnityEngine;

// Token: 0x0200019D RID: 413
public class ClownLevelClownSwing : LevelProperties.Clown.Entity
{
	// Token: 0x1700025E RID: 606
	// (get) Token: 0x060013C6 RID: 5062 RVA: 0x00010989 File Offset: 0x0000EB89
	// (set) Token: 0x060013C7 RID: 5063 RVA: 0x00010991 File Offset: 0x0000EB91
	public ClownLevelClownSwing.State state { get; set; }

	// Token: 0x060013C8 RID: 5064 RVA: 0x0001099A File Offset: 0x0000EB9A
	public override void Awake()
	{
		base.Awake();
		this.damageReceiver = base.GetComponent<DamageReceiver>();
		this.damageReceiver.OnDamageTaken += this.OnDamageTaken;
		this.state = ClownLevelClownSwing.State.Intro;
	}

	// Token: 0x060013C9 RID: 5065 RVA: 0x000988B0 File Offset: 0x00096AB0
	public void OnDamageTaken(DamageDealer.DamageInfo info)
	{
		base.properties.DealDamage(info.damage);
		if (base.properties.CurrentHealth <= 0f && this.state != ClownLevelClownSwing.State.Death)
		{
			this.state = ClownLevelClownSwing.State.Death;
			this.StartDeath();
		}
	}

	// Token: 0x060013CA RID: 5066 RVA: 0x000109CC File Offset: 0x0000EBCC
	public override void LevelInit(LevelProperties.Clown properties)
	{
		base.LevelInit(properties);
		this.eyeMainIndex = Random.Range(0, properties.CurrentState.swing.positionString.Length);
	}

	// Token: 0x060013CB RID: 5067 RVA: 0x000109F3 File Offset: 0x0000EBF3
	public void StartSwing()
	{
		AudioManager.Play("clown_swing_face_intro");
		this.emitAudioFromObject.Add("clown_swing_face_intro");
		base.StartCoroutine(this.swing_intro_cr());
	}

	// Token: 0x060013CC RID: 5068 RVA: 0x000988FC File Offset: 0x00096AFC
	public IEnumerator swing_intro_cr()
	{
		float t = 0f;
		float time = 5f;
		Vector2 start = base.transform.position;
		while (t < time)
		{
			float val = EaseUtils.Ease(EaseUtils.EaseType.easeInOutSine, 0f, 1f, t / time);
			base.transform.position = Vector2.Lerp(start, this.swingStopPosition.position, val);
			t += CupheadTime.Delta;
			yield return null;
		}
		base.transform.position = this.swingStopPosition.position;
		t = 0f;
		time = 0.5f;
		start = this.umbrella.transform.position;
		Vector2 end = new Vector3(this.umbrella.transform.position.x, this.umbrella.transform.position.y - 30f);
		while (t < time)
		{
			float val2 = EaseUtils.Ease(EaseUtils.EaseType.easeInOutSine, 0f, 1f, t / time);
			this.umbrella.transform.position = Vector2.Lerp(start, end, val2);
			t += CupheadTime.Delta;
			yield return null;
		}
		this.umbrella.transform.position = start;
		this.umbrella.GetComponent<SpriteRenderer>().sortingLayerName = "Player";
		this.umbrella.GetComponent<SpriteRenderer>().sortingOrder = 200;
		this.state = ClownLevelClownSwing.State.Idle;
		base.StartCoroutine(this.swing_cr());
		this.coasterHandler.OnCoasterLeave += this.StartEnemies;
		yield return null;
		yield break;
	}

	// Token: 0x060013CD RID: 5069 RVA: 0x00098918 File Offset: 0x00096B18
	public IEnumerator swing_cr()
	{
		LevelProperties.Clown.Swing p = base.properties.CurrentState.swing;
		float spacingFront = this.swingFrontPrefab.GetComponent<Renderer>().bounds.size.x + p.swingSpacing;
		float spacingBack = this.swingBackPrefab.GetComponent<Renderer>().bounds.size.x + p.swingSpacing;
		int numOfSwings = 6;
		AudioManager.Play("clown_swing_open");
		this.emitAudioFromObject.Add("clown_swing_open");
		base.animator.SetTrigger("Continue");
		for (int i = 0; i < numOfSwings; i++)
		{
			Vector3 pos;
			pos..ctor(-640f - spacingFront + spacingFront * (float)i, 360f, 0f);
			ClownLevelSwings clownLevelSwings = Object.Instantiate<ClownLevelSwings>(this.swingFrontPrefab);
			clownLevelSwings.Init(pos, base.properties.CurrentState.swing, spacingFront, (float)i);
		}
		for (int j = 0; j < numOfSwings; j++)
		{
			Vector3 pos2;
			pos2..ctor(640f + spacingBack - spacingBack * (float)j, 360f, 0f);
			ClownLevelSwings clownLevelSwings2 = Object.Instantiate<ClownLevelSwings>(this.swingBackPrefab);
			clownLevelSwings2.Init(pos2, base.properties.CurrentState.swing, spacingBack, (float)j);
		}
		yield return null;
		yield break;
	}

	// Token: 0x060013CE RID: 5070 RVA: 0x00010A1C File Offset: 0x0000EC1C
	public void StartBottom()
	{
		base.animator.Play("Swing_Bottom_Idle");
	}

	// Token: 0x060013CF RID: 5071 RVA: 0x00010A2E File Offset: 0x0000EC2E
	public override void OnDestroy()
	{
		base.OnDestroy();
		this.enemy = null;
		this.swingFrontPrefab = null;
		this.swingBackPrefab = null;
	}

	// Token: 0x060013D0 RID: 5072 RVA: 0x00098934 File Offset: 0x00096B34
	public void StartDeath()
	{
		if (this.OnDeath != null)
		{
			this.OnDeath();
		}
		this.StopAllCoroutines();
		AudioManager.Play("clown_swing_death");
		this.emitAudioFromObject.Add("clown_swing_death");
		base.animator.Play("Swing_Death");
		base.animator.Play("Swing_Bottom_Death");
		base.animator.Play("Face_Death");
		ClownLevelSwings.moveSpeed = base.properties.CurrentState.swing.swingSpeed * 2f;
		base.GetComponent<Collider2D>().enabled = false;
	}

	// Token: 0x060013D1 RID: 5073 RVA: 0x00010A4B File Offset: 0x0000EC4B
	public void SetMoveDirection(int set)
	{
		if (set == 1)
		{
			this.moveUp = true;
		}
		else
		{
			this.moveUp = false;
		}
	}

	// Token: 0x060013D2 RID: 5074 RVA: 0x000989D4 File Offset: 0x00096BD4
	public IEnumerator move_topper_cr()
	{
		float speed = 60f;
		for (;;)
		{
			if (this.moveUp)
			{
				this.topper.transform.position += Vector3.up * speed * CupheadTime.Delta;
			}
			else
			{
				this.topper.transform.position -= Vector3.up * speed * CupheadTime.Delta;
			}
			yield return null;
		}
		yield break;
	}

	// Token: 0x060013D3 RID: 5075 RVA: 0x00010A67 File Offset: 0x0000EC67
	public void StartEnemies()
	{
		base.animator.SetBool("IsAttacking", true);
		base.StartCoroutine(this.enemies_cr());
	}

	// Token: 0x060013D4 RID: 5076 RVA: 0x000989F0 File Offset: 0x00096BF0
	public IEnumerator enemies_cr()
	{
		LevelProperties.Clown.Swing p = base.properties.CurrentState.swing;
		string[] enemyPosString = p.positionString[this.eyeMainIndex].Split(new char[]
		{
			','
		});
		this.state = ClownLevelClownSwing.State.Enemies;
		AudioManager.Play("clown_swing_face_attack_intro");
		this.emitAudioFromObject.Add("clown_swing_face_attack_intro");
		yield return base.animator.WaitForAnimationToEnd(this, "Face_Attack_Intro", 1, false, true);
		for (int i = 0; i < enemyPosString.Length; i++)
		{
			string[] enemyPos = enemyPosString[i].Split(new char[]
			{
				'-'
			});
			foreach (string pos in enemyPos)
			{
				float targetX = 0f;
				Parser.FloatTryParse(pos, out targetX);
				this.enemy.Create(base.transform.position, targetX, p.HP, p, this);
				yield return CupheadTime.WaitForSeconds(this, p.spawnDelay);
			}
		}
		this.eyeMainIndex = (this.eyeMainIndex + 1) % p.positionString.Length;
		base.animator.SetBool("IsAttacking", false);
		AudioManager.Play("clown_swing_face_attack_outro");
		this.emitAudioFromObject.Add("clown_swing_face_attack_outro");
		yield return null;
		yield break;
	}

	// Token: 0x0400100B RID: 4107
	public const int NumOfSwings = 6;

	// Token: 0x0400100C RID: 4108
	[SerializeField]
	public ClownLevelCoasterHandler coasterHandler;

	// Token: 0x0400100D RID: 4109
	[SerializeField]
	public GameObject umbrella;

	// Token: 0x0400100E RID: 4110
	[SerializeField]
	public GameObject topper;

	// Token: 0x0400100F RID: 4111
	[SerializeField]
	public ClownLevelEnemy enemy;

	// Token: 0x04001010 RID: 4112
	[SerializeField]
	public ClownLevelSwings swingFrontPrefab;

	// Token: 0x04001011 RID: 4113
	[SerializeField]
	public ClownLevelSwings swingBackPrefab;

	// Token: 0x04001012 RID: 4114
	[SerializeField]
	public BasicProjectile clownBullet;

	// Token: 0x04001013 RID: 4115
	[SerializeField]
	public Transform swingStopPosition;

	// Token: 0x04001015 RID: 4117
	public DamageReceiver damageReceiver;

	// Token: 0x04001016 RID: 4118
	public bool moveUp;

	// Token: 0x04001017 RID: 4119
	public int eyeMainIndex;

	// Token: 0x04001018 RID: 4120
	public Action OnDeath;

	// Token: 0x02000B06 RID: 2822
	public enum State
	{
		// Token: 0x040050F2 RID: 20722
		Intro,
		// Token: 0x040050F3 RID: 20723
		Idle,
		// Token: 0x040050F4 RID: 20724
		Enemies,
		// Token: 0x040050F5 RID: 20725
		Death
	}
}
