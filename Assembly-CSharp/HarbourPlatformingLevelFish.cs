using System;
using System.Collections;
using UnityEngine;

// Token: 0x0200042A RID: 1066
public class HarbourPlatformingLevelFish : AbstractPlatformingLevelEnemy
{
	// Token: 0x06002E16 RID: 11798 RVA: 0x000266DC File Offset: 0x000248DC
	public void Init(Vector2 pos, float rotation, string type)
	{
		base.transform.position = pos;
		this.type = type;
		this.rotation = rotation;
	}

	// Token: 0x06002E17 RID: 11799 RVA: 0x000266FD File Offset: 0x000248FD
	public override void OnStart()
	{
	}

	// Token: 0x06002E18 RID: 11800 RVA: 0x000DE728 File Offset: 0x000DC928
	public override void Start()
	{
		this.blinkLayer.enabled = false;
		this.blinkCounterMax = Random.Range(10, 20);
		base.transform.SetScale(new float?((this.rotation != 180f) ? (-base.transform.localScale.x) : base.transform.localScale.x), null, null);
		for (int i = 0; i < 5; i++)
		{
			if (this.type.Substring(0, 1) == this.letters.Split(new char[]
			{
				','
			})[i])
			{
				this.num = i + 1;
			}
		}
		this.isA = (this.type.Substring(1, 1) == "A");
		base.animator.SetInteger("Type", this.num);
		base.animator.SetBool("Is1", this.isA);
		if (this.num == 4)
		{
			this._canParry = true;
		}
		base.StartCoroutine(this.movement_cr());
		base.Start();
	}

	// Token: 0x06002E19 RID: 11801 RVA: 0x000266FF File Offset: 0x000248FF
	public void FlyingFishSFX()
	{
		AudioManager.Play("harbour_flying_fish_idle");
		this.emitAudioFromObject.Add("harbour_flying_fish_idle");
	}

	// Token: 0x06002E1A RID: 11802 RVA: 0x000DE874 File Offset: 0x000DCA74
	public IEnumerator movement_cr()
	{
		float angle = 0f;
		Vector3 xVelocity = Vector3.zero;
		for (;;)
		{
			angle += base.Properties.flyingFishSinVelocity * CupheadTime.Delta;
			xVelocity = ((this.rotation != 180f) ? base.transform.right : (-base.transform.right));
			Vector3 moveY = new Vector3(0f, Mathf.Sin(angle) * CupheadTime.Delta * 60f * base.Properties.flyingFishSinSize);
			Vector3 moveX = xVelocity * base.Properties.flyingFishVelocity * CupheadTime.Delta;
			if (CupheadTime.Delta != 0f)
			{
				base.transform.position += moveX + moveY;
			}
			if (!CupheadLevelCamera.Current.ContainsPoint(base.transform.position, AbstractPlatformingLevelEnemy.CAMERA_DEATH_PADDING))
			{
				AudioManager.Stop("harbour_flying_fish_idle");
				Object.Destroy(base.gameObject);
			}
			yield return null;
		}
		yield break;
	}

	// Token: 0x06002E1B RID: 11803 RVA: 0x000DE890 File Offset: 0x000DCA90
	public void IncrementBlinkCounter()
	{
		this.FlyingFishSFX();
		if (this.blinkCounter < this.blinkCounterMax)
		{
			this.blinkLayer.enabled = false;
			this.blinkCounter++;
		}
		else
		{
			this.blinkLayer.enabled = true;
			this.blinkCounter = 0;
			this.blinkCounterMax = Random.Range(5, 10);
		}
	}

	// Token: 0x06002E1C RID: 11804 RVA: 0x0002671B File Offset: 0x0002491B
	public override void Die()
	{
		AudioManager.Stop("harbour_flying_fish_idle");
		AudioManager.Play("harbour_flying_fish_death");
		this.emitAudioFromObject.Add("harbour_flying_fish_death");
		base.Die();
	}

	// Token: 0x04002625 RID: 9765
	[SerializeField]
	public SpriteRenderer blinkLayer;

	// Token: 0x04002626 RID: 9766
	public string letters = "A,B,C,D,E,F,G,H,I,J,K,L,M,N,O,P";

	// Token: 0x04002627 RID: 9767
	public string type;

	// Token: 0x04002628 RID: 9768
	public float rotation;

	// Token: 0x04002629 RID: 9769
	public int num;

	// Token: 0x0400262A RID: 9770
	public bool isA;

	// Token: 0x0400262B RID: 9771
	public int blinkCounter;

	// Token: 0x0400262C RID: 9772
	public int blinkCounterMax;
}
