using System;
using System.Collections;
using UnityEngine;

// Token: 0x02000368 RID: 872
public class SallyStagePlayLevelWindow : AbstractCollidableObject
{
	// Token: 0x0600269A RID: 9882 RVA: 0x000206A4 File Offset: 0x0001E8A4
	public void Start()
	{
		this.startPos = base.transform.position;
		base.GetComponent<Collider2D>().enabled = false;
		base.GetComponent<DamageReceiver>().OnDamageTaken += this.OnDamageTaken;
	}

	// Token: 0x0600269B RID: 9883 RVA: 0x000206DA File Offset: 0x0001E8DA
	public void Init(Vector2 pos, SallyStagePlayLevel parent)
	{
		base.transform.position = pos;
		this.parent = parent;
	}

	// Token: 0x0600269C RID: 9884 RVA: 0x000C9530 File Offset: 0x000C7730
	public void OnDamageTaken(DamageDealer.DamageInfo info)
	{
		this.HP -= info.damage;
		if (this.HP <= 0f && !this.isDying)
		{
			if (this.isBaby)
			{
				base.StartCoroutine(this.baby_slide_off());
			}
			else
			{
				this.NunDead();
			}
		}
	}

	// Token: 0x0600269D RID: 9885 RVA: 0x000206F4 File Offset: 0x0001E8F4
	public void WindowClosed()
	{
		base.animator.Play("Off");
	}

	// Token: 0x0600269E RID: 9886 RVA: 0x00020706 File Offset: 0x0001E906
	public void LeftWindow()
	{
		base.GetComponent<Collider2D>().enabled = false;
	}

	// Token: 0x0600269F RID: 9887 RVA: 0x000C9590 File Offset: 0x000C7790
	public void WindowOpenNun(LevelProperties.SallyStagePlay properties, bool isPink)
	{
		this.isBaby = false;
		this.speed = properties.CurrentState.nun.rulerSpeed;
		this.HP = (float)properties.CurrentState.nun.HP;
		base.GetComponent<SpriteRenderer>().enabled = true;
		base.GetComponent<Collider2D>().enabled = true;
		this.isPink = isPink;
		foreach (SpriteRenderer spriteRenderer in this.nunPink)
		{
			spriteRenderer.enabled = isPink;
		}
		base.animator.Play("Window_Nun");
	}

	// Token: 0x060026A0 RID: 9888 RVA: 0x000C9628 File Offset: 0x000C7828
	public void ShootRuler()
	{
		AbstractPlayerController next = PlayerManager.GetNext();
		Vector3 vector = next.transform.position - base.transform.position;
		float rotation = MathUtils.DirectionToAngle(vector);
		SallyStagePlayLevelWindowProjectile sallyStagePlayLevelWindowProjectile = (!this.isPink) ? this.ruler : this.rulerPink;
		sallyStagePlayLevelWindowProjectile.Create(base.transform.position, rotation, this.speed, this.parent);
	}

	// Token: 0x060026A1 RID: 9889 RVA: 0x000C96A4 File Offset: 0x000C78A4
	public void WindowOpenBaby(LevelProperties.SallyStagePlay properties)
	{
		this.isBaby = true;
		this.speed = properties.CurrentState.baby.bottleSpeed;
		this.HP = (float)properties.CurrentState.baby.HP;
		base.GetComponent<SpriteRenderer>().enabled = true;
		base.GetComponent<Collider2D>().enabled = true;
		string str = (!Rand.Bool()) ? "_Boy" : "_Girl";
		base.animator.Play("Window_Baby" + str);
	}

	// Token: 0x060026A2 RID: 9890 RVA: 0x000C9730 File Offset: 0x000C7930
	public void ShootBottle()
	{
		Vector3 vector = new Vector3(base.transform.position.x, -360f, 0f) - base.transform.position;
		float rotation = MathUtils.DirectionToAngle(vector);
		this.bottle.Create(new Vector2(base.transform.position.x, base.transform.position.y - 30f), rotation, this.speed, this.parent);
	}

	// Token: 0x060026A3 RID: 9891 RVA: 0x000C97C8 File Offset: 0x000C79C8
	public IEnumerator baby_slide_off()
	{
		this.isDying = true;
		base.animator.SetTrigger("Dead");
		base.GetComponent<Collider2D>().enabled = false;
		yield return CupheadTime.WaitForSeconds(this, 1.5f);
		Vector3 start = base.transform.position;
		Vector3 end = new Vector3(base.transform.position.x, base.transform.position.y - 50f);
		float t = 0f;
		float time = 0.1f;
		while (t < time)
		{
			t += CupheadTime.Delta;
			base.transform.position = Vector3.Lerp(start, end, t / time);
			yield return null;
		}
		yield return null;
		this.isDying = false;
		base.GetComponent<SpriteRenderer>().enabled = false;
		base.animator.Play("Off");
		base.transform.position = this.startPos;
		yield return null;
		yield break;
	}

	// Token: 0x060026A4 RID: 9892 RVA: 0x00020714 File Offset: 0x0001E914
	public void NunDead()
	{
		base.animator.SetTrigger("Dead");
		base.GetComponent<Collider2D>().enabled = false;
	}

	// Token: 0x060026A5 RID: 9893 RVA: 0x00020732 File Offset: 0x0001E932
	public void SoundBabyBoy()
	{
		AudioManager.Play("sally_baby_boy");
		this.emitAudioFromObject.Add("sally_baby_boy");
	}

	// Token: 0x060026A6 RID: 9894 RVA: 0x0002074E File Offset: 0x0001E94E
	public void SoundBabyGirl()
	{
		AudioManager.Play("sally_baby_girl");
		this.emitAudioFromObject.Add("sally_baby_girl");
	}

	// Token: 0x060026A7 RID: 9895 RVA: 0x0002076A File Offset: 0x0001E96A
	public override void OnDestroy()
	{
		base.OnDestroy();
		this.rulerPink = null;
		this.ruler = null;
		this.bottle = null;
	}

	// Token: 0x04001FD5 RID: 8149
	[SerializeField]
	public Transform projectileRoot;

	// Token: 0x04001FD6 RID: 8150
	[SerializeField]
	public SallyStagePlayLevelWindowProjectile rulerPink;

	// Token: 0x04001FD7 RID: 8151
	[SerializeField]
	public SallyStagePlayLevelWindowProjectile ruler;

	// Token: 0x04001FD8 RID: 8152
	[SerializeField]
	public SallyStagePlayLevelWindowProjectile bottle;

	// Token: 0x04001FD9 RID: 8153
	[SerializeField]
	public SpriteRenderer[] nunPink;

	// Token: 0x04001FDA RID: 8154
	public int windowNum;

	// Token: 0x04001FDB RID: 8155
	public LevelProperties.SallyStagePlay properties;

	// Token: 0x04001FDC RID: 8156
	public SallyStagePlayLevel parent;

	// Token: 0x04001FDD RID: 8157
	public Vector3 startPos;

	// Token: 0x04001FDE RID: 8158
	public float speed;

	// Token: 0x04001FDF RID: 8159
	public float HP;

	// Token: 0x04001FE0 RID: 8160
	public bool isDying;

	// Token: 0x04001FE1 RID: 8161
	public bool isBaby = true;

	// Token: 0x04001FE2 RID: 8162
	public bool isPink;
}
