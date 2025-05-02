using System;
using System.Collections;
using UnityEngine;

// Token: 0x0200019E RID: 414
public class ClownLevelCoaster : AbstractCollidableObject
{
	// Token: 0x060013D6 RID: 5078 RVA: 0x00010A8F File Offset: 0x0000EC8F
	public void Init(Vector3 backStartPos, Vector3 frontStartPos, LevelProperties.Clown.Coaster properties, float coasterLength, float coasterSize, ClownLevelLights warningLights)
	{
		base.transform.position = backStartPos;
		this.frontStartPos = frontStartPos;
		this.properties = properties;
		this.coasterLength = coasterLength;
		this.coasterSize = coasterSize;
		this.warningLights = warningLights;
		this.sprite = base.GetComponent<SpriteRenderer>();
	}

	// Token: 0x060013D7 RID: 5079 RVA: 0x00010ACF File Offset: 0x0000ECCF
	public override void Awake()
	{
		base.Awake();
		this.damageDealer = DamageDealer.NewEnemy();
	}

	// Token: 0x060013D8 RID: 5080 RVA: 0x00098A0C File Offset: 0x00096C0C
	public override void OnCollisionPlayer(GameObject hit, CollisionPhase phase)
	{
		base.OnCollisionPlayer(hit, phase);
		if (phase != CollisionPhase.Exit)
		{
			this.damageDealer.DealDamage(hit);
			hit.GetComponent<LevelPlayerController>().OnPitKnockUp(base.transform.position.y, 0.85f);
		}
	}

	// Token: 0x060013D9 RID: 5081 RVA: 0x00010AE2 File Offset: 0x0000ECE2
	public void Update()
	{
		if (this.damageDealer != null)
		{
			this.damageDealer.Update();
		}
	}

	// Token: 0x060013DA RID: 5082 RVA: 0x00010AFA File Offset: 0x0000ECFA
	public void ChompSound()
	{
		if (this.inView)
		{
			AudioManager.Play("clown_coaster_main");
			this.emitAudioFromObject.Add("clown_coaster_main");
		}
	}

	// Token: 0x060013DB RID: 5083 RVA: 0x00098A58 File Offset: 0x00096C58
	public IEnumerator move_coaster_front_cr()
	{
		bool lightsOff = true;
		AudioManager.PlayLoop("sfx_clown_coaster_ratchet_loop");
		this.emitAudioFromObject.Add("sfx_clown_coaster_ratchet_loop");
		yield return CupheadTime.WaitForSeconds(this, this.properties.coasterBackToFrontDelay);
		while (base.transform.position.x > -640f - this.coasterSize * this.coasterLength)
		{
			base.transform.position += -base.transform.right * this.properties.coasterSpeed * CupheadTime.Delta;
			if (base.transform.position.x < 640f + 0.2f * this.coasterSize && lightsOff)
			{
				this.warningLights.StartWarningLights();
				lightsOff = false;
			}
			if (base.transform.position.x < -640f - this.coasterSize * this.coasterLength && !lightsOff)
			{
				this.warningLights.StopWarningLights();
				lightsOff = true;
			}
			yield return null;
		}
		this.inView = false;
		AudioManager.Stop("sfx_clown_coaster_ratchet_loop");
		this.Die();
		yield return null;
		yield break;
	}

	// Token: 0x060013DC RID: 5084 RVA: 0x00098A74 File Offset: 0x00096C74
	public IEnumerator move_coaster_back_cr()
	{
		this.inView = true;
		AudioManager.PlayLoop("sfx_clown_coaster_distant_by");
		this.emitAudioFromObject.Add("sfx_clown_coaster_distant_by");
		while (base.transform.position.x < 640f + this.coasterSize * 0.44f * this.coasterLength)
		{
			base.transform.position += base.transform.right * this.properties.coasterSpeed * CupheadTime.Delta;
			yield return null;
		}
		AudioManager.Stop("sfx_clown_coaster_distant_by");
		this.FrontCoasterSetup();
		yield return null;
		yield break;
	}

	// Token: 0x060013DD RID: 5085 RVA: 0x00098A90 File Offset: 0x00096C90
	public void BackCoasterSetup()
	{
		int num = 97;
		this.knobCollider.enabled = false;
		base.GetComponent<Collider2D>().enabled = false;
		this.childrenSprites = base.GetComponentsInChildren<SpriteRenderer>();
		foreach (SpriteRenderer spriteRenderer in this.childrenSprites)
		{
			if (spriteRenderer.gameObject.GetComponent<LevelPlatform>() != null)
			{
				spriteRenderer.gameObject.GetComponent<Collider2D>().enabled = false;
			}
		}
		base.transform.SetScale(new float?(-0.44f), new float?(0.44f), null);
		base.transform.SetEulerAngles(null, null, new float?(17.57f));
		this.sprite.sortingLayerName = "Background";
		this.sprite.sortingOrder = 45;
		Color color;
		ColorUtility.TryParseHtmlString("#b6b6b6", ref color);
		this.sprite.color = color;
		for (int j = 0; j < this.childrenSprites.Length; j++)
		{
			this.childrenSprites[j].color = color;
			this.childrenSprites[j].sortingLayerName = "Background";
			this.childrenSprites[j].sortingOrder = num - j;
			if (this.childrenSprites[j].GetComponent<ClownLevelRiders>() != null)
			{
				this.childrenSprites[j].GetComponent<Collider2D>().enabled = false;
			}
		}
		this.knobSprite.GetComponent<SpriteRenderer>().sortingLayerName = "Background";
		this.knobSprite.GetComponent<SpriteRenderer>().sortingOrder = num + 1;
		base.StartCoroutine(this.move_coaster_back_cr());
	}

	// Token: 0x060013DE RID: 5086 RVA: 0x00098C48 File Offset: 0x00096E48
	public void FrontCoasterSetup()
	{
		int num = 79;
		this.knobCollider.enabled = true;
		base.GetComponent<Collider2D>().enabled = true;
		foreach (SpriteRenderer spriteRenderer in this.childrenSprites)
		{
			if (spriteRenderer.gameObject.GetComponent<LevelPlatform>() != null)
			{
				spriteRenderer.gameObject.GetComponent<Collider2D>().enabled = true;
			}
		}
		base.transform.position = this.frontStartPos;
		base.transform.SetScale(new float?(1f), new float?(1f), null);
		base.transform.SetEulerAngles(null, null, new float?(0f));
		this.sprite.sortingLayerName = "Player";
		this.sprite.sortingOrder = 80;
		Color color;
		ColorUtility.TryParseHtmlString("#FFFFFFFF", ref color);
		this.sprite.color = color;
		for (int j = 0; j < this.childrenSprites.Length; j++)
		{
			this.childrenSprites[j].color = color;
			if (this.childrenSprites[j].transform.parent == base.transform || this.childrenSprites[j].transform == base.transform)
			{
				this.childrenSprites[j].sortingLayerName = "Player";
				this.childrenSprites[j].sortingOrder = num - j;
			}
			else if (this.childrenSprites[j].GetComponent<ClownLevelRiders>() != null)
			{
				this.childrenSprites[j].GetComponent<Collider2D>().enabled = true;
				this.childrenSprites[j].sortingLayerName = "Player";
				this.childrenSprites[j].sortingOrder = num - j;
				this.childrenSprites[j].GetComponent<ClownLevelRiders>().FrontLayers(num - j);
			}
			else if (!this.childrenSprites[j].transform.parent.GetComponent<ClownLevelRiders>())
			{
				this.childrenSprites[j].sortingLayerName = "Default";
				this.childrenSprites[j].sortingOrder = 4;
			}
		}
		this.knobSprite.GetComponent<SpriteRenderer>().sortingLayerName = "Player";
		this.knobSprite.GetComponent<SpriteRenderer>().sortingOrder = num + 1;
		base.StartCoroutine(this.move_coaster_front_cr());
	}

	// Token: 0x060013DF RID: 5087 RVA: 0x00010B21 File Offset: 0x0000ED21
	public void Die()
	{
		Object.Destroy(base.gameObject);
	}

	// Token: 0x04001019 RID: 4121
	[SerializeField]
	public SpriteRenderer knobSprite;

	// Token: 0x0400101A RID: 4122
	[SerializeField]
	public Collider2D knobCollider;

	// Token: 0x0400101B RID: 4123
	public Transform pieceRoot;

	// Token: 0x0400101C RID: 4124
	public LevelProperties.Clown.Coaster properties;

	// Token: 0x0400101D RID: 4125
	public ClownLevelLights warningLights;

	// Token: 0x0400101E RID: 4126
	public SpriteRenderer sprite;

	// Token: 0x0400101F RID: 4127
	public SpriteRenderer[] childrenSprites;

	// Token: 0x04001020 RID: 4128
	public Vector3 frontStartPos;

	// Token: 0x04001021 RID: 4129
	public DamageDealer damageDealer;

	// Token: 0x04001022 RID: 4130
	public float coasterSize;

	// Token: 0x04001023 RID: 4131
	public float coasterLength;

	// Token: 0x04001024 RID: 4132
	public bool inView;
}
