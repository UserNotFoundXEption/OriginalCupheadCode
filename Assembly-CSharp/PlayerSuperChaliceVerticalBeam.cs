using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x0200052E RID: 1326
public class PlayerSuperChaliceVerticalBeam : AbstractPlayerSuper
{
	// Token: 0x060037D3 RID: 14291 RVA: 0x0002D90E File Offset: 0x0002BB0E
	public override void Awake()
	{
		base.Awake();
		this.damageReceivers = new List<DamageReceiver>();
	}

	// Token: 0x060037D4 RID: 14292 RVA: 0x00105108 File Offset: 0x00103308
	public override void Update()
	{
		base.Update();
		if (this.updateStraw)
		{
			this.UpdateStraw();
		}
		if (this.player == null)
		{
			this.Interrupt();
		}
		else
		{
			this.player.transform.position = base.transform.position;
		}
	}

	// Token: 0x060037D5 RID: 14293 RVA: 0x00105164 File Offset: 0x00103364
	public override void OnCollision(GameObject hit, CollisionPhase phase)
	{
		DamageReceiver component = hit.GetComponent<DamageReceiver>();
		if (component != null)
		{
			if (this.damageReceivers.Contains(component))
			{
				return;
			}
			this.damageReceivers.Add(component);
		}
	}

	// Token: 0x060037D6 RID: 14294 RVA: 0x001051A4 File Offset: 0x001033A4
	public void OnDealDamage(float damage, DamageReceiver receiver, DamageDealer dealer)
	{
		Collider2D componentInChildren = receiver.GetComponentInChildren<Collider2D>();
		Vector2 vector = Vector2.zero;
		Vector2 vector2 = Vector2.zero;
		if (componentInChildren.GetType() == typeof(BoxCollider2D))
		{
			vector = (componentInChildren as BoxCollider2D).size;
		}
		else
		{
			if (componentInChildren.GetType() != typeof(CircleCollider2D))
			{
				return;
			}
			vector = Vector2.one * (componentInChildren as CircleCollider2D).radius;
		}
		vector2..ctor(componentInChildren.transform.position.x + Random.Range(-vector.x / 2f, vector.x / 2f), componentInChildren.transform.position.y + Random.Range(-vector.y / 2f, vector.y / 2f));
		vector2 += componentInChildren.offset;
		this.hitPrefab.Create(vector2);
	}

	// Token: 0x060037D7 RID: 14295 RVA: 0x001052AC File Offset: 0x001034AC
	public override void Fire()
	{
		AudioManager.Play("player_super_chalice_superbeam");
		base.Fire();
		this.damageDealer = new DamageDealer(WeaponProperties.LevelSuperChaliceVertBeam.damage, WeaponProperties.LevelSuperChaliceVertBeam.damageRate, DamageDealer.DamageSource.Super, false, true, true);
		this.damageDealer.OnDealDamage += this.OnDealDamage;
		this.damageDealer.DamageMultiplier *= PlayerManager.DamageMultiplier;
		this.damageDealer.PlayerId = this.player.id;
		MeterScoreTracker meterScoreTracker = new MeterScoreTracker(MeterScoreTracker.Type.Super);
		meterScoreTracker.Add(this.damageDealer);
	}

	// Token: 0x060037D8 RID: 14296 RVA: 0x0010533C File Offset: 0x0010353C
	public override void StartSuper()
	{
		if (this.player == null)
		{
			return;
		}
		this.player.weaponManager.OnSuperStart -= this.player.motor.StartSuper;
		this.player.weaponManager.OnSuperEnd -= this.player.motor.OnSuperEnd;
		base.StartSuper();
		string str = (!this.player.motor.Grounded) ? "_Air" : string.Empty;
		base.animator.Play("Vert_Beam_Loop" + str);
		AudioManager.Play("player_super_chalice_superbeam_start");
	}

	// Token: 0x060037D9 RID: 14297 RVA: 0x0002D921 File Offset: 0x0002BB21
	public void AnimationDone()
	{
		if (this.player != null)
		{
			this.player.motor.CheckForPostSuperHop();
		}
		this.EndSuper(true);
		Object.Destroy(base.gameObject);
	}

	// Token: 0x060037DA RID: 14298 RVA: 0x001053F4 File Offset: 0x001035F4
	public void UpdateStraw()
	{
		Camera main = Camera.main;
		Transform transform = main.transform;
		this.StrawFX.transform.SetPosition(null, new float?(transform.position.y + -200f * base.transform.localScale.y), null);
	}

	// Token: 0x060037DB RID: 14299 RVA: 0x0002D956 File Offset: 0x0002BB56
	public void Ani_LockStraw()
	{
		this.updateStraw = true;
	}

	// Token: 0x060037DC RID: 14300 RVA: 0x0002D95F File Offset: 0x0002BB5F
	public void Ani_UnLockStraw()
	{
		this.updateStraw = false;
		AudioManager.Play("player_super_chalice_superbeam_end");
	}

	// Token: 0x060037DD RID: 14301 RVA: 0x0002D972 File Offset: 0x0002BB72
	public override void OnDestroy()
	{
		base.OnDestroy();
		this.hitPrefab = null;
		this.damageReceivers.Clear();
		this.damageReceivers = null;
	}

	// Token: 0x04002D01 RID: 11521
	public const float STRAW_Y_OFFSET = -200f;

	// Token: 0x04002D02 RID: 11522
	[Header("Effects")]
	[SerializeField]
	public Effect hitPrefab;

	// Token: 0x04002D03 RID: 11523
	[SerializeField]
	public GameObject StrawFX;

	// Token: 0x04002D04 RID: 11524
	public bool updateStraw;

	// Token: 0x04002D05 RID: 11525
	public List<DamageReceiver> damageReceivers;
}
