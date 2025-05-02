using System;
using System.Collections;
using UnityEngine;

// Token: 0x0200043B RID: 1083
public class MountainPlatformingLevelCyclopsBG : AbstractPausableComponent
{
	// Token: 0x17000361 RID: 865
	// (get) Token: 0x06002E8F RID: 11919 RVA: 0x00026D70 File Offset: 0x00024F70
	// (set) Token: 0x06002E90 RID: 11920 RVA: 0x00026D78 File Offset: 0x00024F78
	public bool isWalking { get; set; }

	// Token: 0x06002E91 RID: 11921 RVA: 0x000DFC50 File Offset: 0x000DDE50
	public void Start()
	{
		this.sizeY = base.GetComponent<SpriteRenderer>().bounds.size.y;
		this.blinkCounterMax = Random.Range(3, 6);
		this.eye.enabled = false;
	}

	// Token: 0x06002E92 RID: 11922 RVA: 0x000DFC98 File Offset: 0x000DDE98
	public void OnTurn()
	{
		base.transform.SetScale(new float?(-base.transform.localScale.x), null, null);
		if (base.transform.localScale.x == 1f)
		{
			base.transform.AddPosition(-47f, 0f, 0f);
		}
		else
		{
			base.transform.AddPosition(47f, 0f, 0f);
		}
	}

	// Token: 0x06002E93 RID: 11923 RVA: 0x00026D81 File Offset: 0x00024F81
	public void DropRocks()
	{
		base.StartCoroutine(this.drop_rocks_cr());
	}

	// Token: 0x06002E94 RID: 11924 RVA: 0x00026D90 File Offset: 0x00024F90
	public void StopWalking()
	{
		this.isWalking = false;
	}

	// Token: 0x06002E95 RID: 11925 RVA: 0x00026D99 File Offset: 0x00024F99
	public void StartWalking()
	{
		this.isWalking = true;
	}

	// Token: 0x06002E96 RID: 11926 RVA: 0x00026DA2 File Offset: 0x00024FA2
	public void GetPlayer(AbstractPlayerController player)
	{
		this.player = player;
	}

	// Token: 0x06002E97 RID: 11927 RVA: 0x000DFD34 File Offset: 0x000DDF34
	public IEnumerator drop_rocks_cr()
	{
		for (int i = 0; i < this.rockCount; i++)
		{
			Vector2 zero = Vector2.zero;
			zero.y = CupheadLevelCamera.Current.Bounds.yMax + 200f;
			zero.x = CupheadLevelCamera.Current.Bounds.xMin + this.projectile.GetComponent<Renderer>().bounds.size.x / 2f + this.projectile.GetComponent<Renderer>().bounds.size.x * (float)i;
			this.projectile.Create(base.transform.position, zero, this.rockSpeed, this.rockDelay * (float)i);
		}
		yield return null;
		yield break;
	}

	// Token: 0x06002E98 RID: 11928 RVA: 0x00026DAB File Offset: 0x00024FAB
	public void SlideUp()
	{
		if (!this.isDead)
		{
			base.StartCoroutine(this.slide_up_cr());
		}
	}

	// Token: 0x06002E99 RID: 11929 RVA: 0x00026DC5 File Offset: 0x00024FC5
	public void SlideDown()
	{
		base.StartCoroutine(this.slide_down_cr());
	}

	// Token: 0x06002E9A RID: 11930 RVA: 0x000DFD50 File Offset: 0x000DDF50
	public IEnumerator slide_up_cr()
	{
		this.player = PlayerManager.GetNext();
		base.transform.SetPosition(new float?(this.player.transform.position.x), null, null);
		float t = 0f;
		float time = 0.4f;
		float startPos = base.transform.position.y;
		float endPos = this.start.y;
		while (t < time)
		{
			t += CupheadTime.Delta;
			base.transform.SetPosition(null, new float?(Mathf.Lerp(startPos, endPos, t / time)), null);
			yield return null;
		}
		yield break;
	}

	// Token: 0x06002E9B RID: 11931 RVA: 0x000DFD6C File Offset: 0x000DDF6C
	public IEnumerator slide_down_cr()
	{
		float t = 0f;
		float time = 0.8f;
		float startPos = base.transform.position.y;
		float endPos = this.start.y - this.sizeY;
		while (t < time)
		{
			t += CupheadTime.Delta;
			base.transform.SetPosition(null, new float?(Mathf.Lerp(startPos, endPos, t / time)), null);
			yield return null;
		}
		if (this.isDead)
		{
			base.gameObject.SetActive(false);
		}
		else
		{
			yield return CupheadTime.WaitForSeconds(this, 0.8f);
			base.animator.SetTrigger("Continue");
		}
		yield return null;
		yield break;
	}

	// Token: 0x06002E9C RID: 11932 RVA: 0x000DFD88 File Offset: 0x000DDF88
	public void BlinkMaybe()
	{
		if (this.blinkCounter < this.blinkCounterMax)
		{
			this.eye.enabled = false;
			this.blinkCounter++;
		}
		else
		{
			this.eye.enabled = true;
			this.blinkCounter = 0;
			this.blinkCounterMax = Random.Range(3, 6);
		}
	}

	// Token: 0x06002E9D RID: 11933 RVA: 0x00026DD4 File Offset: 0x00024FD4
	public void SoundGiantRockThrow()
	{
		AudioManager.Play("castle_giant_rock_throw");
		this.emitAudioFromObject.Add("castle_giant_rock_throw");
	}

	// Token: 0x06002E9E RID: 11934 RVA: 0x00026DF0 File Offset: 0x00024FF0
	public void SoundGiantRockThrowAppear()
	{
		AudioManager.Play("castle_giant_rock_throw_appear");
		this.emitAudioFromObject.Add("castle_giant_rock_throw_appear");
	}

	// Token: 0x06002E9F RID: 11935 RVA: 0x00026E0C File Offset: 0x0002500C
	public void SoundGiantStartle()
	{
		AudioManager.Play("castle_giant_startle");
		this.emitAudioFromObject.Add("castle_giant_startle");
	}

	// Token: 0x040026A0 RID: 9888
	[SerializeField]
	public SpriteRenderer eye;

	// Token: 0x040026A1 RID: 9889
	[SerializeField]
	public float rockDelay;

	// Token: 0x040026A2 RID: 9890
	[SerializeField]
	public float rockSpeed;

	// Token: 0x040026A3 RID: 9891
	[SerializeField]
	public int rockCount;

	// Token: 0x040026A4 RID: 9892
	[SerializeField]
	public MountainPlatformingLevelRock projectile;

	// Token: 0x040026A5 RID: 9893
	public int blinkCounter;

	// Token: 0x040026A6 RID: 9894
	public int blinkCounterMax;

	// Token: 0x040026A7 RID: 9895
	public float sizeY;

	// Token: 0x040026A8 RID: 9896
	public bool isAltPattern;

	// Token: 0x040026A9 RID: 9897
	public AbstractPlayerController player;

	// Token: 0x040026AA RID: 9898
	public Vector3 start;

	// Token: 0x040026AC RID: 9900
	public bool isDead;
}
