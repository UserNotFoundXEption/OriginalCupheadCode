using System;
using System.Collections;
using UnityEngine;

// Token: 0x02000337 RID: 823
public class RobotLevelOrb : AbstractProjectile
{
	// Token: 0x060023F9 RID: 9209 RVA: 0x0001E59E File Offset: 0x0001C79E
	public override void Awake()
	{
		this.damageReceiver = base.GetComponent<DamageReceiver>();
		this.damageReceiver.OnDamageTaken += this.OnDamageTaken;
		base.Awake();
	}

	// Token: 0x060023FA RID: 9210 RVA: 0x0001E5CA File Offset: 0x0001C7CA
	public override void Start()
	{
		base.Start();
		base.StartCoroutine(this.fade_in_cr());
	}

	// Token: 0x060023FB RID: 9211 RVA: 0x0001E5DF File Offset: 0x0001C7DF
	public override void Update()
	{
		if (this.damageDealer != null)
		{
			this.damageDealer.Update();
		}
		base.Update();
	}

	// Token: 0x060023FC RID: 9212 RVA: 0x0001E5FD File Offset: 0x0001C7FD
	public override void OnCollisionPlayer(GameObject hit, CollisionPhase phase)
	{
		if (phase != CollisionPhase.Exit)
		{
			this.damageDealer.DealDamage(hit);
		}
		base.OnCollisionPlayer(hit, phase);
	}

	// Token: 0x060023FD RID: 9213 RVA: 0x0001E61B File Offset: 0x0001C81B
	public virtual void OnDamageTaken(DamageDealer.DamageInfo info)
	{
		if (!this.activeShields)
		{
			this.health -= info.damage;
			if (this.health <= 0f)
			{
				Object.Destroy(base.gameObject);
			}
		}
	}

	// Token: 0x060023FE RID: 9214 RVA: 0x000C23A4 File Offset: 0x000C05A4
	public override void OnParry(AbstractPlayerController player)
	{
		if (this.activeShields)
		{
			this.lasers.SetActive(false);
			this.activeShields = false;
			this.pinkTop.enabled = false;
			this.pinkBottom.enabled = false;
			this.SetParryable(false);
			AudioManager.Play("robot_orb_death");
			this.emitAudioFromObject.Add("robot_orb_death");
			base.animator.SetTrigger("Continue");
			base.StartCoroutine(this.slide_in_cr());
		}
	}

	// Token: 0x060023FF RID: 9215 RVA: 0x000C2428 File Offset: 0x000C0628
	public RobotLevelOrb Create(Vector3 position, Vector3 offsetAfterSpawn)
	{
		GameObject gameObject = Object.Instantiate<GameObject>(base.gameObject, position, Quaternion.identity);
		RobotLevelOrb component = gameObject.GetComponent<RobotLevelOrb>();
		component.offsetAfterSpawn = offsetAfterSpawn;
		return component;
	}

	// Token: 0x06002400 RID: 9216 RVA: 0x000C2458 File Offset: 0x000C0658
	public IEnumerator fade_in_cr()
	{
		base.transform.SetScale(new float?(0.5f), new float?(0.5f), null);
		this.pinkTop.GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, 0f);
		this.pinkBottom.GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, 0f);
		float t = 0f;
		float time = 0.9f;
		while (t < time)
		{
			t += CupheadTime.Delta;
			float val = EaseUtils.Ease(EaseUtils.EaseType.easeInSine, 0.5f, 1f, t / time);
			base.transform.SetScale(new float?(val), new float?(val), null);
			this.pinkTop.GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, t / time);
			this.pinkBottom.GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, t / time);
			yield return null;
		}
		yield return null;
		yield break;
	}

	// Token: 0x06002401 RID: 9217 RVA: 0x000C2474 File Offset: 0x000C0674
	public void InitOrb(LevelProperties.Robot properties)
	{
		base.transform.position += Vector3.left * (float)properties.CurrentState.orb.orbMovementSpeed * CupheadTime.Delta;
		this.properties = properties;
		if (properties.CurrentState.orb.orbShieldIsActive)
		{
			this.SetParryable(true);
			this.activeShields = true;
			this.pinkTop.enabled = true;
			this.pinkBottom.enabled = true;
		}
		else
		{
			this.activeShields = false;
		}
		this.health = (float)properties.CurrentState.orb.orbHP;
		this.speed = properties.CurrentState.orb.orbMovementSpeed;
		base.transform.right = Vector3.left;
		base.StartCoroutine(this.fade_color_cr());
		base.StartCoroutine(this.lasers_cr());
		base.StartCoroutine(this.move_cr());
	}

	// Token: 0x06002402 RID: 9218 RVA: 0x0001E656 File Offset: 0x0001C856
	public void InitChildOrb(int speed, float health, bool activeShields)
	{
		this.speed = speed;
		this.health = health;
		this.activeShields = activeShields;
		base.StartCoroutine(this.move_cr());
		this.lasers.SetActive(this.lasers.activeSelf);
	}

	// Token: 0x06002403 RID: 9219 RVA: 0x000C2574 File Offset: 0x000C0774
	public IEnumerator lasers_cr()
	{
		yield return CupheadTime.WaitForSeconds(this, this.properties.CurrentState.orb.orbInitalOpenDelay);
		if (!this.activeShields)
		{
			yield break;
		}
		base.StartCoroutine(this.slide_out_cr());
		yield return CupheadTime.WaitForSeconds(this, this.properties.CurrentState.orb.orbInitialLaserDelay);
		if (!this.activeShields)
		{
			yield break;
		}
		base.animator.Play("Laser_Start");
		this.lasers.SetActive(true);
		AudioManager.PlayLoop("robot_orb_spark_loop");
		yield return null;
		yield break;
	}

	// Token: 0x06002404 RID: 9220 RVA: 0x000C2590 File Offset: 0x000C0790
	public IEnumerator shields_cr()
	{
		yield return CupheadTime.WaitForSeconds(this, this.properties.CurrentState.orb.orbSpawnDelay);
		this.activeShields = true;
		yield break;
	}

	// Token: 0x06002405 RID: 9221 RVA: 0x000C25AC File Offset: 0x000C07AC
	public IEnumerator move_cr()
	{
		for (;;)
		{
			if (base.transform.position.x < this.offsetAfterSpawn.x && base.transform.position.y < this.offsetAfterSpawn.y)
			{
				base.transform.position += Vector3.up * (float)this.speed * CupheadTime.Delta * 0.5f;
			}
			base.transform.position += Vector3.left * (float)this.speed * CupheadTime.Delta;
			if (base.transform.position.x < (float)Level.Current.Left - base.GetComponents<BoxCollider2D>()[0].size.x / 2f)
			{
				AudioManager.Stop("robot_orb_spark_loop");
				Object.Destroy(base.gameObject);
			}
			yield return null;
		}
		yield break;
	}

	// Token: 0x06002406 RID: 9222 RVA: 0x000C25C8 File Offset: 0x000C07C8
	public IEnumerator slide_out_cr()
	{
		float sizeY = base.GetComponent<Collider2D>().bounds.size.y;
		float localPosTop = this.top.transform.localPosition.y + sizeY / 4f;
		float localPosBottom = this.bottom.transform.localPosition.y - sizeY / 4f;
		Vector3 topPos = this.top.transform.localPosition;
		Vector3 bottomPos = this.bottom.transform.localPosition;
		float time = 0.5f;
		float t = 0f;
		if (this.activeShields)
		{
			this.wasActive = true;
			AudioManager.Play("robot_orb_spark_start");
			base.animator.Play("Sparks_Start");
		}
		while (t < time)
		{
			float val = EaseUtils.Ease(EaseUtils.EaseType.easeInOutSine, 0f, 1f, t / time);
			topPos.y = Mathf.Lerp(0f, localPosTop, val);
			bottomPos.y = Mathf.Lerp(0f, localPosBottom, val);
			this.top.transform.localPosition = topPos;
			this.bottom.transform.localPosition = bottomPos;
			t += CupheadTime.Delta;
			yield return null;
		}
		yield return null;
		yield break;
	}

	// Token: 0x06002407 RID: 9223 RVA: 0x000C25E4 File Offset: 0x000C07E4
	public IEnumerator slide_in_cr()
	{
		Vector3 topPos = this.top.transform.localPosition;
		Vector3 bottomPos = this.bottom.transform.localPosition;
		float time = 0.5f;
		float t = 0f;
		while (t < time)
		{
			float val = EaseUtils.Ease(EaseUtils.EaseType.easeInOutSine, 0f, 1f, t / time);
			topPos.y = Mathf.Lerp(topPos.y, 0f, val);
			bottomPos.y = Mathf.Lerp(bottomPos.y, 0f, val);
			this.top.transform.localPosition = topPos;
			this.bottom.transform.localPosition = bottomPos;
			t += CupheadTime.Delta;
			yield return null;
		}
		if (this.wasActive)
		{
			AudioManager.Stop("robot_orb_spark_loop");
			base.animator.Play("Sparks_End");
		}
		yield return null;
		yield break;
	}

	// Token: 0x06002408 RID: 9224 RVA: 0x000C2600 File Offset: 0x000C0800
	public virtual IEnumerator fade_color_cr()
	{
		float t = 0f;
		float fadeTime = 0.5f;
		while (t < fadeTime)
		{
			if (!this.activeShields)
			{
				this.top.GetComponent<SpriteRenderer>().color = new Color(t / fadeTime, t / fadeTime, t / fadeTime, 1f);
				this.bottom.GetComponent<SpriteRenderer>().color = new Color(t / fadeTime, t / fadeTime, t / fadeTime, 1f);
			}
			else
			{
				this.pinkTop.color = new Color(t / fadeTime, t / fadeTime, t / fadeTime, 1f);
				this.pinkBottom.color = new Color(t / fadeTime, t / fadeTime, t / fadeTime, 1f);
			}
			t += CupheadTime.Delta;
			yield return null;
		}
		yield return null;
		yield break;
	}

	// Token: 0x04001DD1 RID: 7633
	[SerializeField]
	public GameObject lasers;

	// Token: 0x04001DD2 RID: 7634
	[SerializeField]
	public Transform top;

	// Token: 0x04001DD3 RID: 7635
	[SerializeField]
	public Transform bottom;

	// Token: 0x04001DD4 RID: 7636
	[SerializeField]
	public SpriteRenderer pinkTop;

	// Token: 0x04001DD5 RID: 7637
	[SerializeField]
	public SpriteRenderer pinkBottom;

	// Token: 0x04001DD6 RID: 7638
	public LevelProperties.Robot properties;

	// Token: 0x04001DD7 RID: 7639
	public DamageReceiver damageReceiver;

	// Token: 0x04001DD8 RID: 7640
	public bool activeShields;

	// Token: 0x04001DD9 RID: 7641
	public bool wasActive;

	// Token: 0x04001DDA RID: 7642
	public float health;

	// Token: 0x04001DDB RID: 7643
	public int speed;

	// Token: 0x04001DDC RID: 7644
	public Vector3 offsetAfterSpawn;
}
