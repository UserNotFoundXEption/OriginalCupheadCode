using System;
using System.Collections;
using UnityEngine;

// Token: 0x0200036F RID: 879
public class SaltbakerLevelCutter : AbstractProjectile
{
	// Token: 0x17000321 RID: 801
	// (get) Token: 0x060026CE RID: 9934 RVA: 0x0002096C File Offset: 0x0001EB6C
	public override float DestroyLifetime
	{
		get
		{
			return 0f;
		}
	}

	// Token: 0x060026CF RID: 9935 RVA: 0x000C9E28 File Offset: 0x000C8028
	public SaltbakerLevelCutter Create(Vector3 position, float speed, bool goingLeft, int id)
	{
		SaltbakerLevelCutter saltbakerLevelCutter = this.InstantiatePrefab<SaltbakerLevelCutter>();
		saltbakerLevelCutter.transform.position = position;
		saltbakerLevelCutter.speed = speed;
		saltbakerLevelCutter.goingLeft = goingLeft;
		saltbakerLevelCutter.transform.localScale = new Vector3((float)((!goingLeft) ? 1 : -1), 1f);
		if (id == 1)
		{
			saltbakerLevelCutter.transform.position += Vector3.down * 5f;
		}
		saltbakerLevelCutter.sfxID = id;
		saltbakerLevelCutter.SFX_SALTBAKER_P3_PizzaWheel_Loop(id);
		saltbakerLevelCutter.rend.sortingOrder = id;
		return saltbakerLevelCutter;
	}

	// Token: 0x060026D0 RID: 9936 RVA: 0x00020973 File Offset: 0x0001EB73
	public void AniEvent_StartMove()
	{
		base.StartCoroutine(this.move_cr());
		this.dustFX.transform.parent = null;
		this.dustFX.SetActive(true);
	}

	// Token: 0x060026D1 RID: 9937 RVA: 0x000C9EC4 File Offset: 0x000C80C4
	public void AniEvent_Variation()
	{
		if (((this.goingLeft && base.transform.position.x > (float)Level.Current.Left + 50f + 100f) || (!this.goingLeft && base.transform.position.x < (float)Level.Current.Right - 50f - 100f)) && Random.Range(0, 4) == 0)
		{
			base.animator.SetTrigger("Variation");
		}
	}

	// Token: 0x060026D2 RID: 9938 RVA: 0x0002099F File Offset: 0x0001EB9F
	public void AniEvent_ChangeDirection()
	{
		this.goingLeft = !this.goingLeft;
	}

	// Token: 0x060026D3 RID: 9939 RVA: 0x000C9F64 File Offset: 0x000C8164
	public void AniEvent_CompleteTurn()
	{
		base.transform.localScale = new Vector3(-base.transform.localScale.x, 1f);
		this.turning = false;
	}

	// Token: 0x060026D4 RID: 9940 RVA: 0x000209B0 File Offset: 0x0001EBB0
	public void AniEvent_CompleteSink()
	{
		Object.Destroy(base.gameObject);
	}

	// Token: 0x060026D5 RID: 9941 RVA: 0x000209BD File Offset: 0x0001EBBD
	public void Sink()
	{
		base.animator.SetBool("Sink", true);
		this.SFX_SALTBAKER_P3_PizzaWheel_Dive(this.sfxID);
	}

	// Token: 0x060026D6 RID: 9942 RVA: 0x000C9FA4 File Offset: 0x000C81A4
	public IEnumerator move_cr()
	{
		float left = (float)Level.Current.Left + 50f;
		float right = (float)Level.Current.Right - 50f;
		YieldInstruction wait = new WaitForFixedUpdate();
		for (;;)
		{
			Vector3 dir = (!this.goingLeft) ? Vector3.right : Vector3.left;
			base.transform.position += dir * this.speed * CupheadTime.FixedDelta;
			if (!this.turning && ((this.goingLeft && base.transform.position.x < left) || (!this.goingLeft && base.transform.position.x > right)))
			{
				this.turning = true;
				base.animator.Play("Turn");
			}
			yield return wait;
		}
		yield break;
	}

	// Token: 0x060026D7 RID: 9943 RVA: 0x000209DC File Offset: 0x0001EBDC
	public override void OnCollisionPlayer(GameObject hit, CollisionPhase phase)
	{
		if (phase != CollisionPhase.Exit)
		{
			this.damageDealer.DealDamage(hit);
		}
		base.OnCollisionPlayer(hit, phase);
	}

	// Token: 0x060026D8 RID: 9944 RVA: 0x000C9FC0 File Offset: 0x000C81C0
	public void SFX_SALTBAKER_P3_PizzaWheel_Loop(int loopNumber)
	{
		string key = "sfx_dlc_saltbaker_p3_pizzawheel_movement_loop_" + (loopNumber + 1);
		AudioManager.PlayLoop(key);
		this.emitAudioFromObject.Add(key);
	}

	// Token: 0x060026D9 RID: 9945 RVA: 0x000209FA File Offset: 0x0001EBFA
	public void SFX_SALTBAKER_P3_PizzaWheel_Dive(int loopNumber)
	{
		AudioManager.Stop("sfx_dlc_saltbaker_p3_pizzawheel_movement_loop_" + (loopNumber + 1));
	}

	// Token: 0x060026DA RID: 9946 RVA: 0x00020A13 File Offset: 0x0001EC13
	public void AnimationEvent_SFX_SALTBAKER_P3_RunnerWheel_Spawn()
	{
		AudioManager.Play("sfx_dlc_saltbaker_p3_pizzawheel_spawn");
		this.emitAudioFromObject.Add("sfx_dlc_saltbaker_p3_pizzawheel_spawn");
	}

	// Token: 0x0400200F RID: 8207
	public const float SCREEN_EDGE_OFFSET = 50f;

	// Token: 0x04002010 RID: 8208
	public float speed;

	// Token: 0x04002011 RID: 8209
	public bool goingLeft;

	// Token: 0x04002012 RID: 8210
	public bool turning;

	// Token: 0x04002013 RID: 8211
	public int sfxID;

	// Token: 0x04002014 RID: 8212
	public LevelProperties.Saltbaker.Cutter properties;

	// Token: 0x04002015 RID: 8213
	[SerializeField]
	public GameObject dustFX;

	// Token: 0x04002016 RID: 8214
	[SerializeField]
	public SpriteRenderer rend;
}
