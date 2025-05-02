using System;
using System.Collections;
using UnityEngine;

// Token: 0x020002D9 RID: 729
public class OldManLevelDwarf : AbstractProjectile
{
	// Token: 0x170002DF RID: 735
	// (get) Token: 0x0600203B RID: 8251 RVA: 0x0001B5FC File Offset: 0x000197FC
	// (set) Token: 0x0600203C RID: 8252 RVA: 0x0001B604 File Offset: 0x00019804
	public bool inPlace { get; set; }

	// Token: 0x0600203D RID: 8253 RVA: 0x0001B60D File Offset: 0x0001980D
	public override void OnDieDistance()
	{
	}

	// Token: 0x0600203E RID: 8254 RVA: 0x0001B60F File Offset: 0x0001980F
	public override void OnDieLifetime()
	{
	}

	// Token: 0x0600203F RID: 8255 RVA: 0x000B7620 File Offset: 0x000B5820
	public override void Start()
	{
		base.Start();
		this.startPos = base.transform.position;
		this.damageReceiver = base.GetComponent<DamageReceiver>();
		this.damageReceiver.OnDamageTaken += this.OnDamageTaken;
		this.damageReceiver.enabled = false;
		this.inPlace = true;
	}

	// Token: 0x06002040 RID: 8256 RVA: 0x0001B611 File Offset: 0x00019811
	public override void OnCollisionPlayer(GameObject hit, CollisionPhase phase)
	{
		base.OnCollisionPlayer(hit, phase);
		if (phase != CollisionPhase.Exit)
		{
			this.damageDealer.DealDamage(hit);
		}
	}

	// Token: 0x06002041 RID: 8257 RVA: 0x0001B62F File Offset: 0x0001982F
	public override void OnParry(AbstractPlayerController player)
	{
		this.Death(true);
	}

	// Token: 0x06002042 RID: 8258 RVA: 0x0001B638 File Offset: 0x00019838
	public void OnDamageTaken(DamageDealer.DamageInfo info)
	{
		this.health -= info.damage;
		if (this.health < 0f)
		{
			Level.Current.RegisterMinionKilled();
			this.Death(false);
		}
	}

	// Token: 0x06002043 RID: 8259 RVA: 0x000B767C File Offset: 0x000B587C
	public IEnumerator move_up_cr()
	{
		YieldInstruction wait = new WaitForFixedUpdate();
		float speed = 200f;
		this.rend.sortingLayerID = SortingLayer.NameToID("Default");
		this.rend.sortingOrder = 2;
		while (base.transform.position.y < -430f)
		{
			base.transform.AddPosition(0f, speed * CupheadTime.FixedDelta, 0f);
			yield return wait;
		}
		this.beardController.CueRuffle(this.rufflePos);
		base.animator.SetTrigger("Continue");
		string typeString = (!this.typeA) ? "_B" : "_A";
		yield return base.animator.WaitForAnimationToEnd(this, "Trans" + typeString + this.colorString, false, true);
		yield return null;
		yield break;
	}

	// Token: 0x06002044 RID: 8260 RVA: 0x0001B66E File Offset: 0x0001986E
	public override void SetParryable(bool parryable)
	{
		base.SetParryable(parryable);
	}

	// Token: 0x06002045 RID: 8261 RVA: 0x000B7698 File Offset: 0x000B5898
	public void ShootInArc(float apexHeight, float timeToApex, float health, bool typeA, bool parryable, float warningTime)
	{
		this.apexheight = apexHeight;
		this.apexTime = timeToApex;
		this.health = health;
		this.typeA = typeA;
		this.inPlace = false;
		this.damageReceiver.enabled = true;
		this.parryable = parryable;
		this.warningTime = warningTime;
		if (parryable)
		{
			this.colorString = "_Pink";
		}
		else
		{
			this.colorString = ((!this.isBlue) ? "_Teal" : string.Empty);
			this.isBlue = !this.isBlue;
		}
		this.SetParryable(false);
		base.StartCoroutine(this.arc_cr());
	}

	// Token: 0x06002046 RID: 8262 RVA: 0x000B7740 File Offset: 0x000B5940
	public void CalculateArc()
	{
		float num = this.apexheight;
		float num2 = this.apexTime * this.apexTime;
		float num3 = -2f * num / num2;
		float num4 = 2f * num / this.apexTime;
		float num5 = num4 * num4;
		Vector3 position = base.transform.position;
		Vector3 position2 = PlayerManager.GetNext().transform.position;
		float num6 = position2.x - position.x;
		float num7 = position2.y - position.y;
		float num8 = num5 + 2f * num3 * num7;
		if (num8 < 0f)
		{
			num8 = 0f;
		}
		float num9 = (-num4 + Mathf.Sqrt(num8)) / num3;
		float num10 = (-num4 - Mathf.Sqrt(num8)) / num3;
		float num11 = Mathf.Max(num9, num10);
		this.vel.x = num6 / num11;
		this.vel.y = num4;
		this.gravity = num3;
	}

	// Token: 0x06002047 RID: 8263 RVA: 0x000B782C File Offset: 0x000B5A2C
	public IEnumerator arc_cr()
	{
		bool finishedArcing = false;
		this.inPlace = false;
		string typeString = (!this.typeA) ? "_B" : "_A";
		base.animator.Play("Climb" + typeString + this.colorString);
		base.GetComponent<SpriteRenderer>().sortingOrder = 2;
		yield return base.StartCoroutine(this.move_up_cr());
		Effect beardPopPrefab = (!this.typeA) ? this.beardPopB : this.beardPopA;
		this.beardPop = beardPopPrefab.Create(new Vector3(base.transform.position.x, -335f));
		yield return null;
		yield return this.beardPop.animator.WaitForAnimationToStart(this, "Pimple_Warning", false);
		AudioManager.Play("sfx_dlc_omm_gnome_groundstretch");
		this.emitAudioFromObject.Add("sfx_dlc_omm_gnome_groundstretch");
		yield return CupheadTime.WaitForSeconds(this, this.warningTime);
		this.CalculateArc();
		this.SetParryable(this.parryable);
		this.coll.enabled = true;
		base.transform.position = new Vector3(base.transform.position.x, -325f);
		base.transform.localScale = new Vector3(Mathf.Sign(this.vel.x), 1f);
		base.animator.Play("Spin" + typeString + this.colorString);
		this.rend.sortingLayerID = SortingLayer.NameToID("Player");
		this.rend.sortingOrder = 50;
		AudioManager.Play("sfx_dlc_omm_gnome_groundstretchpop");
		this.emitAudioFromObject.Add("sfx_dlc_omm_gnome_groundstretchpop");
		AudioManager.Play("sfx_dlc_omm_gnome_somersault_voice");
		this.emitAudioFromObject.Add("sfx_dlc_omm_gnome_somersault_voice");
		AudioManager.Play("sfx_dlc_omm_gnome_somersault");
		this.emitAudioFromObject.Add("sfx_dlc_omm_gnome_somersault");
		this.beardPop.animator.Play("Pimple_End");
		this.groundShadow = true;
		this.currentArcTime = 0f;
		YieldInstruction wait = new WaitForFixedUpdate();
		while (!finishedArcing)
		{
			this.vel += new Vector3(0f, this.gravity * CupheadTime.FixedDelta);
			base.transform.Translate(this.vel * CupheadTime.FixedDelta);
			if (this.rend.sortingOrder == 50 && this.vel.y < 0f)
			{
				this.rend.sortingLayerID = SortingLayer.NameToID("Enemies");
				this.rend.sortingOrder = 4;
			}
			if (this.vel.y < 0f && base.transform.position.y < -289f)
			{
				finishedArcing = true;
				break;
			}
			this.currentArcTime += CupheadTime.FixedDelta;
			yield return wait;
		}
		this.groundShadow = false;
		Vector3 pos = new Vector3(base.transform.position.x, -289f);
		Effect beardHealPrefab = (!this.typeA) ? this.beardHealB : this.beardHealA;
		beardHealPrefab.Create(pos + Vector3.down * 25f);
		this.rend.sortingLayerID = SortingLayer.NameToID("Default");
		this.rend.sortingOrder = 5;
		this.vel.x = 0f;
		this.vel.y = this.vel.y * 0.5f;
		float t = 0f;
		while (t < 0.0416666679f && base.transform.position.y > -334f)
		{
			t += CupheadTime.FixedDelta;
			base.transform.Translate(this.vel * CupheadTime.FixedDelta);
			yield return wait;
		}
		this.Respawn();
		yield return null;
		yield break;
	}

	// Token: 0x06002048 RID: 8264 RVA: 0x000B7848 File Offset: 0x000B5A48
	public void Death(bool parried = false)
	{
		if (this.beardPop)
		{
			Object.Destroy(this.beardPop.gameObject);
		}
		if (base.transform.position.y > this.startPos.y)
		{
			this.deathPuff.Create(base.transform.position);
		}
		if (!parried)
		{
			for (int i = 0; i < this.deathParts.Length; i++)
			{
				if (i != 0 || Random.Range(0, 10) == 0)
				{
					SpriteDeathParts spriteDeathParts = this.deathParts[i].CreatePart(base.transform.position);
					if (i != 0)
					{
						spriteDeathParts.animator.Play(this.colorString);
					}
				}
			}
			AudioManager.Play("sfx_dlc_omm_gnome_popper_death");
			this.emitAudioFromObject.Add("sfx_dlc_omm_gnome_popper_death");
		}
		AudioManager.Stop("sfx_dlc_omm_gnome_somersault");
		AudioManager.Stop("sfx_dlc_omm_gnome_somersault_voice");
		this.groundShadow = false;
		this.Respawn();
	}

	// Token: 0x06002049 RID: 8265 RVA: 0x0001B677 File Offset: 0x00019877
	public void Respawn()
	{
		this.StopAllCoroutines();
		this.damageReceiver.enabled = false;
		base.transform.position = this.startPos;
		this.inPlace = true;
		this.coll.enabled = false;
	}

	// Token: 0x0600204A RID: 8266 RVA: 0x000B794C File Offset: 0x000B5B4C
	public void LateUpdate()
	{
		if (this.groundShadow)
		{
			this.shadowRend.sortingOrder = 5;
			this.shadowRend.transform.position = new Vector3(base.transform.position.x, -314f + Mathf.Lerp(40f, 60f, this.currentArcTime / (this.apexTime * 2f)));
			if (base.transform.position.y < -314f + this.shadowRange)
			{
				float num = Mathf.Lerp(0f, (float)(this.shadowSprites.Length - 4), Mathf.InverseLerp(-314f, -314f + this.shadowRange, base.transform.position.y));
				this.shadowRend.sprite = this.shadowSprites[(int)num];
			}
			else
			{
				this.shadowRend.sprite = this.shadowSprites[this.shadowSprites.Length - 3 + (int)(this.currentArcTime * 24f) % 3];
			}
		}
		else
		{
			this.shadowRend.sortingOrder = 1;
			this.shadowRend.transform.localPosition = Vector3.zero;
		}
	}

	// Token: 0x04001A40 RID: 6720
	public const float START_POS_Y = -430f;

	// Token: 0x04001A41 RID: 6721
	public const float JUMP_POS_Y = -325f;

	// Token: 0x04001A42 RID: 6722
	public const float LAND_POS_Y = -314f;

	// Token: 0x04001A43 RID: 6723
	public const float LAND_OFFSET = 25f;

	// Token: 0x04001A44 RID: 6724
	public const float SHADOW_OFFSET_START = 40f;

	// Token: 0x04001A45 RID: 6725
	public const float SHADOW_OFFSET_END = 60f;

	// Token: 0x04001A46 RID: 6726
	[Header("Death FX")]
	[SerializeField]
	public Effect deathPuff;

	// Token: 0x04001A47 RID: 6727
	[SerializeField]
	public SpriteDeathParts[] deathParts;

	// Token: 0x04001A48 RID: 6728
	[Header("Beard FX")]
	[SerializeField]
	public Effect beardPopA;

	// Token: 0x04001A49 RID: 6729
	[SerializeField]
	public Effect beardPopB;

	// Token: 0x04001A4A RID: 6730
	[SerializeField]
	public Effect beardHealA;

	// Token: 0x04001A4B RID: 6731
	[SerializeField]
	public Effect beardHealB;

	// Token: 0x04001A4C RID: 6732
	[SerializeField]
	public SpriteRenderer rend;

	// Token: 0x04001A4D RID: 6733
	[SerializeField]
	public Collider2D coll;

	// Token: 0x04001A4F RID: 6735
	public DamageReceiver damageReceiver;

	// Token: 0x04001A50 RID: 6736
	public Vector3 startPos;

	// Token: 0x04001A51 RID: 6737
	public Vector3 vel;

	// Token: 0x04001A52 RID: 6738
	public float gravity;

	// Token: 0x04001A53 RID: 6739
	public float health;

	// Token: 0x04001A54 RID: 6740
	public float apexTime;

	// Token: 0x04001A55 RID: 6741
	public float bulletSpeed;

	// Token: 0x04001A56 RID: 6742
	public float apexheight;

	// Token: 0x04001A57 RID: 6743
	public float currentArcTime;

	// Token: 0x04001A58 RID: 6744
	public float warningTime;

	// Token: 0x04001A59 RID: 6745
	public bool typeA;

	// Token: 0x04001A5A RID: 6746
	public bool parryable;

	// Token: 0x04001A5B RID: 6747
	public string colorString;

	// Token: 0x04001A5C RID: 6748
	public bool isBlue = true;

	// Token: 0x04001A5D RID: 6749
	public Effect beardPop;

	// Token: 0x04001A5E RID: 6750
	[SerializeField]
	public float shadowRange = 100f;

	// Token: 0x04001A5F RID: 6751
	[SerializeField]
	public SpriteRenderer shadowRend;

	// Token: 0x04001A60 RID: 6752
	[SerializeField]
	public Sprite[] shadowSprites;

	// Token: 0x04001A61 RID: 6753
	public bool groundShadow;

	// Token: 0x04001A62 RID: 6754
	[SerializeField]
	public OldManLevelBeardController beardController;

	// Token: 0x04001A63 RID: 6755
	[SerializeField]
	public int rufflePos;
}
