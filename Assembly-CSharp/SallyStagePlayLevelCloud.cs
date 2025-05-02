using System;
using System.Collections;
using UnityEngine;

// Token: 0x0200035A RID: 858
public class SallyStagePlayLevelCloud : AbstractPausableComponent
{
	// Token: 0x060025F3 RID: 9715 RVA: 0x0001FD67 File Offset: 0x0001DF67
	public void Start()
	{
		base.FrameDelayedCallback(new Action(this.GetSprites), 1);
	}

	// Token: 0x060025F4 RID: 9716 RVA: 0x000C8064 File Offset: 0x000C6264
	public void GetSprites()
	{
		this.normalClones = this.normalSprite.gameObject.transform.GetComponentsInChildren<SpriteRenderer>();
		this.shadowClones = this.shadowSprite.gameObject.transform.GetComponentsInChildren<SpriteRenderer>();
		base.StartCoroutine(this.move_all_cr());
		base.StartCoroutine(this.move_shadow_local_pos_cr());
	}

	// Token: 0x060025F5 RID: 9717 RVA: 0x000C80C4 File Offset: 0x000C62C4
	public IEnumerator move_all_cr()
	{
		float size = 500f;
		float speed = 30f;
		for (;;)
		{
			for (int i = 0; i < this.normalClones.Length; i++)
			{
				if (this.normalClones[i].transform.position.x > -640f - size)
				{
					this.normalClones[i].transform.position += Vector3.left * speed * CupheadTime.Delta;
				}
				else
				{
					this.normalClones[i].transform.position = new Vector3(640f + size, this.normalClones[i].transform.position.y, 0f);
				}
			}
			for (int j = 0; j < this.shadowClones.Length; j++)
			{
				if (this.shadowClones[j].transform.position.x > -640f - size)
				{
					this.shadowClones[j].transform.position += Vector3.left * speed * CupheadTime.Delta;
				}
				else
				{
					Vector3 position = this.shadowClones[j].transform.position;
					position.x = this.normalClones[j].transform.position.x;
					this.shadowClones[j].transform.position = position;
				}
			}
			yield return null;
		}
		yield break;
	}

	// Token: 0x060025F6 RID: 9718 RVA: 0x000C80E0 File Offset: 0x000C62E0
	public IEnumerator move_shadow_local_pos_cr()
	{
		float speed = 1.3f;
		float shadowOffset = 5f;
		for (;;)
		{
			for (int i = 0; i < this.shadowClones.Length; i++)
			{
				if (this.normalClones[i].transform.position.x > 0f && this.normalClones[i].transform.position.x < 440f)
				{
					if (this.shadowClones[i].transform.position.x < this.normalClones[i].transform.position.x + shadowOffset)
					{
						this.shadowClones[i].transform.position += Vector3.right * speed * CupheadTime.Delta;
					}
				}
				else if (this.normalClones[i].transform.position.x < 0f && this.normalClones[i].transform.position.x > -640f && this.shadowClones[i].transform.position.x > this.normalClones[i].transform.position.x - shadowOffset)
				{
					this.shadowClones[i].transform.position -= Vector3.right * speed * CupheadTime.Delta;
				}
			}
			yield return null;
		}
		yield break;
	}

	// Token: 0x04001F73 RID: 8051
	[SerializeField]
	public SpriteRenderer shadowSprite;

	// Token: 0x04001F74 RID: 8052
	public SpriteRenderer[] shadowClones;

	// Token: 0x04001F75 RID: 8053
	[SerializeField]
	public SpriteRenderer normalSprite;

	// Token: 0x04001F76 RID: 8054
	public SpriteRenderer[] normalClones;
}
