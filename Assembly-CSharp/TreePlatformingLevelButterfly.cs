using System;
using System.Collections;
using UnityEngine;

// Token: 0x020003F1 RID: 1009
public class TreePlatformingLevelButterfly : AbstractPausableComponent
{
	// Token: 0x17000352 RID: 850
	// (get) Token: 0x06002C55 RID: 11349 RVA: 0x0002522E File Offset: 0x0002342E
	// (set) Token: 0x06002C56 RID: 11350 RVA: 0x00025236 File Offset: 0x00023436
	public bool isActive { get; set; }

	// Token: 0x06002C57 RID: 11351 RVA: 0x000D9830 File Offset: 0x000D7A30
	public void Start()
	{
		this.maxCounter = Random.Range(4, 7);
		if (this.sprite3.GetComponent<ParrySwitch>() != null)
		{
			this.sprite3.GetComponent<ParrySwitch>().OnActivate += this.Deactivate;
		}
	}

	// Token: 0x06002C58 RID: 11352 RVA: 0x000D987C File Offset: 0x000D7A7C
	public void Init(Vector2 velocity, float scale, int color, MinMax velMinMax)
	{
		this.isActive = true;
		base.transform.SetScale(new float?(scale), null, null);
		base.transform.SetEulerAngles(null, null, new float?((scale >= 0f) ? base.transform.eulerAngles.z : (-base.transform.eulerAngles.z)));
		this.velocity = velocity;
		this.velMinMax = velMinMax;
		this.SelectColor(color);
		this.Setup();
	}

	// Token: 0x06002C59 RID: 11353 RVA: 0x000D9928 File Offset: 0x000D7B28
	public void Setup()
	{
		string text = "P" + Random.Range(1, 5).ToStringInvariant();
		base.animator.Play(text);
		base.StartCoroutine(this.check_dist_cr());
		base.StartCoroutine(this.move_cr());
		base.StartCoroutine(this.switch_y_cr());
		base.StartCoroutine(this.adjust_x_speed(this.velMinMax));
	}

	// Token: 0x06002C5A RID: 11354 RVA: 0x0002523F File Offset: 0x0002343F
	public void Deactivate()
	{
		this.isActive = false;
		this.StopAllCoroutines();
		this.sprite1.SetActive(false);
		this.sprite2.SetActive(false);
		this.sprite3.SetActive(false);
	}

	// Token: 0x06002C5B RID: 11355 RVA: 0x000D9994 File Offset: 0x000D7B94
	public void SelectColor(int color)
	{
		this.sprite1.SetActive(false);
		this.sprite2.SetActive(false);
		this.sprite3.SetActive(false);
		if (color != 1)
		{
			if (color != 2)
			{
				if (color == 3)
				{
					this.sprite3.SetActive(true);
				}
			}
			else
			{
				this.sprite2.SetActive(true);
			}
		}
		else
		{
			this.sprite1.SetActive(true);
		}
	}

	// Token: 0x06002C5C RID: 11356 RVA: 0x000D9A14 File Offset: 0x000D7C14
	public IEnumerator move_cr()
	{
		for (;;)
		{
			this.frameTime += CupheadTime.Delta;
			if (this.frameTime > 0.0833333358f)
			{
				this.frameTime -= 0.0833333358f;
				Vector2 vector = base.transform.position;
				vector += this.velocity * CupheadTime.Delta;
				base.transform.position = vector;
			}
			yield return null;
		}
		yield break;
	}

	// Token: 0x06002C5D RID: 11357 RVA: 0x000D9A30 File Offset: 0x000D7C30
	public IEnumerator switch_y_cr()
	{
		float time = Random.Range(1f, 2f);
		float t = 0f;
		float startVel = this.velocity.y;
		float endVel = -this.velocity.y;
		for (;;)
		{
			yield return CupheadTime.WaitForSeconds(this, Random.Range(3f, 6f));
			while (t < time)
			{
				t += CupheadTime.Delta;
				this.velocity.y = Mathf.Lerp(startVel, endVel, t / time);
				yield return null;
			}
			this.velocity.y = endVel;
			t = 0f;
			startVel = this.velocity.y;
			endVel = -this.velocity.y;
			yield return null;
		}
		yield break;
	}

	// Token: 0x06002C5E RID: 11358 RVA: 0x000D9A4C File Offset: 0x000D7C4C
	public IEnumerator adjust_x_speed(MinMax adjustment)
	{
		float t = 0f;
		float time = Random.Range(1f, 2f);
		float startVel = this.velocity.x;
		float endVel = (Mathf.Sign(this.velocity.x) != 1f) ? (-adjustment.RandomFloat()) : adjustment.RandomFloat();
		for (;;)
		{
			yield return CupheadTime.WaitForSeconds(this, Random.Range(4f, 6f));
			while (t < time)
			{
				this.velocity.x = Mathf.Lerp(startVel, endVel, time);
				yield return null;
			}
			this.velocity.x = endVel;
			endVel = ((Mathf.Sign(this.velocity.x) != 1f) ? (-adjustment.RandomFloat()) : adjustment.RandomFloat());
			startVel = this.velocity.x;
			t = 0f;
			yield return null;
		}
		yield break;
	}

	// Token: 0x06002C5F RID: 11359 RVA: 0x000D9A70 File Offset: 0x000D7C70
	public IEnumerator check_dist_cr()
	{
		for (;;)
		{
			float dist = Vector3.Distance(CupheadLevelCamera.Current.transform.position, base.transform.position);
			if (dist > 2000f)
			{
				break;
			}
			yield return null;
		}
		this.Deactivate();
		yield return null;
		yield break;
	}

	// Token: 0x06002C60 RID: 11360 RVA: 0x000D9A8C File Offset: 0x000D7C8C
	public void Counter()
	{
		if (this.loopCounter < this.maxCounter)
		{
			this.loopCounter++;
		}
		else
		{
			string text = "P" + Random.Range(1, 5).ToStringInvariant();
			base.animator.Play(text);
			this.maxCounter = Random.Range(4, 6);
			this.loopCounter = 0;
		}
	}

	// Token: 0x0400248E RID: 9358
	[SerializeField]
	public GameObject sprite1;

	// Token: 0x0400248F RID: 9359
	[SerializeField]
	public GameObject sprite2;

	// Token: 0x04002490 RID: 9360
	[SerializeField]
	public GameObject sprite3;

	// Token: 0x04002491 RID: 9361
	public const float FRAME_TIME = 0.0833333358f;

	// Token: 0x04002492 RID: 9362
	public Vector2 velocity;

	// Token: 0x04002493 RID: 9363
	public float rotation;

	// Token: 0x04002494 RID: 9364
	public float frameTime;

	// Token: 0x04002495 RID: 9365
	public int loopCounter;

	// Token: 0x04002496 RID: 9366
	public int maxCounter;

	// Token: 0x04002497 RID: 9367
	public MinMax velMinMax;
}
