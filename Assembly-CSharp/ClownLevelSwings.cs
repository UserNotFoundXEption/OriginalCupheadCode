using System;
using System.Collections;
using UnityEngine;

// Token: 0x020001A9 RID: 425
public class ClownLevelSwings : AbstractCollidableObject
{
	// Token: 0x06001432 RID: 5170 RVA: 0x00011042 File Offset: 0x0000F242
	public void Init(Vector3 pos, LevelProperties.Clown.Swing properties, float spacing, float enterAngle)
	{
		this.properties = properties;
		base.transform.position = pos;
		this.spacing = spacing;
		this.enterAngle = enterAngle;
	}

	// Token: 0x06001433 RID: 5171 RVA: 0x00099AC8 File Offset: 0x00097CC8
	public void Start()
	{
		this.sprite = base.GetComponent<SpriteRenderer>();
		this.defaultColor = base.GetComponent<SpriteRenderer>().color;
		if (this.isBackSeat)
		{
			this.rod.transform.SetEulerAngles(null, null, new float?(this.startAngleBack - this.enterAngle));
		}
		else
		{
			this.rod.transform.SetEulerAngles(null, null, new float?(this.startAngleFront + this.enterAngle));
		}
		base.StartCoroutine(this.rotation_cr());
		base.StartCoroutine(this.move_swing_y_cr());
		base.StartCoroutine(this.move_swing_x_cr());
		if (this.properties.swingDropOn)
		{
			base.StartCoroutine(this.player_check_cr());
		}
	}

	// Token: 0x06001434 RID: 5172 RVA: 0x00099BB0 File Offset: 0x00097DB0
	public IEnumerator move_swing_y_cr()
	{
		bool starting = true;
		float distanceLeft = 0f;
		float distanceRight = 0f;
		for (;;)
		{
			Vector3 pos = base.transform.position;
			float speed = (!starting) ? 50f : 150f;
			if (this.isBackSeat)
			{
				distanceLeft = -this.distAmount;
				distanceRight = this.distAmount + this.distAmount / 2f;
			}
			else
			{
				distanceLeft = -this.distAmount - this.distAmount / 2f;
				distanceRight = this.distAmount;
			}
			if (base.transform.position.x > distanceRight || base.transform.position.x < distanceLeft)
			{
				if (base.transform.position.y != this.highPoint)
				{
					pos.y = Mathf.MoveTowards(base.transform.position.y, this.highPoint, speed * CupheadTime.Delta);
					base.transform.position = pos;
				}
				else
				{
					starting = false;
				}
			}
			else if (base.transform.position.y != this.lowestPoint)
			{
				pos.y = Mathf.MoveTowards(base.transform.position.y, this.lowestPoint, speed * CupheadTime.Delta);
				base.transform.position = pos;
			}
			else
			{
				starting = false;
			}
			yield return null;
		}
		yield break;
	}

	// Token: 0x06001435 RID: 5173 RVA: 0x00099BCC File Offset: 0x00097DCC
	public IEnumerator move_swing_x_cr()
	{
		ClownLevelSwings.moveSpeed = this.properties.swingSpeed;
		float size = base.transform.GetComponent<Renderer>().bounds.size.x;
		float sizeDivided = size / 4f;
		for (;;)
		{
			if (this.isBackSeat)
			{
				float end = 640f - this.spacing * 5f;
				while (base.transform.position.x > end)
				{
					base.transform.position -= base.transform.right * ClownLevelSwings.moveSpeed * CupheadTime.Delta;
					yield return null;
				}
				base.transform.position = new Vector3(640f + this.spacing, this.highPoint, 0f);
			}
			else
			{
				float end2 = -640f + (this.spacing * 5f + size);
				base.transform.GetComponent<Collider2D>().enabled = true;
				while (base.transform.position.x < end2)
				{
					if (base.transform.position.x > 640f + sizeDivided)
					{
						base.transform.GetComponent<Collider2D>().enabled = false;
					}
					base.transform.position += base.transform.right * ClownLevelSwings.moveSpeed * CupheadTime.Delta;
					yield return null;
				}
				this.resetWarning = true;
				this.SwingReappear();
				base.transform.position = new Vector3(-640f - (this.spacing - size), this.highPoint, 0f);
			}
			base.StopCoroutine(this.rotation_cr());
			this.enterAngle = 0f;
			if (this.isBackSeat)
			{
				this.rod.transform.SetEulerAngles(null, null, new float?(this.startAngleBack));
			}
			else
			{
				this.rod.transform.SetEulerAngles(null, null, new float?(this.startAngleFront));
			}
			base.StartCoroutine(this.rotation_cr());
			yield return null;
		}
		yield break;
	}

	// Token: 0x06001436 RID: 5174 RVA: 0x00099BE8 File Offset: 0x00097DE8
	public IEnumerator rotation_cr()
	{
		float t = 0f;
		if (this.isBackSeat)
		{
			while (this.rod.transform.eulerAngles.z > this.endAngleBack)
			{
				if (CupheadTime.Delta != 0f)
				{
					this.rod.transform.SetEulerAngles(null, null, new float?(this.rod.transform.eulerAngles.z - t));
					t += 0.001f;
				}
				yield return null;
			}
		}
		else
		{
			while (this.rod.transform.eulerAngles.z < this.endAngleFront)
			{
				if (CupheadTime.Delta != 0f)
				{
					this.rod.transform.SetEulerAngles(null, null, new float?(this.rod.transform.eulerAngles.z + t));
					t += 0.001f;
				}
				yield return null;
			}
		}
		yield return null;
		yield break;
	}

	// Token: 0x06001437 RID: 5175 RVA: 0x00099C04 File Offset: 0x00097E04
	public IEnumerator player_check_cr()
	{
		AbstractPlayerController player = PlayerManager.GetNext();
		for (;;)
		{
			while (player == null || player.transform.parent != base.transform)
			{
				player = PlayerManager.GetNext();
				yield return null;
			}
			if (!this.resetWarning)
			{
				this.sprite.color = Color.red;
				yield return CupheadTime.WaitForSeconds(this, this.properties.swingDropWarningDuration);
				yield return null;
				this.sprite.color = Color.black;
				base.transform.GetComponent<Collider2D>().enabled = false;
				yield return CupheadTime.WaitForSeconds(this, this.properties.swingfullDropDuration);
			}
			this.SwingReappear();
			this.resetWarning = false;
			yield return null;
		}
		yield break;
	}

	// Token: 0x06001438 RID: 5176 RVA: 0x00011066 File Offset: 0x0000F266
	public void SwingReappear()
	{
		base.transform.GetComponent<Collider2D>().enabled = true;
		this.sprite.color = this.defaultColor;
	}

	// Token: 0x04001070 RID: 4208
	public static float moveSpeed;

	// Token: 0x04001071 RID: 4209
	public const float FALL_GRAVITY = -100f;

	// Token: 0x04001072 RID: 4210
	public bool isBackSeat;

	// Token: 0x04001073 RID: 4211
	[SerializeField]
	public Transform rod;

	// Token: 0x04001074 RID: 4212
	public LevelProperties.Clown.Swing properties;

	// Token: 0x04001075 RID: 4213
	public SpriteRenderer sprite;

	// Token: 0x04001076 RID: 4214
	public Color defaultColor;

	// Token: 0x04001077 RID: 4215
	public bool resetWarning;

	// Token: 0x04001078 RID: 4216
	public float spacing;

	// Token: 0x04001079 RID: 4217
	public float lowestPoint;

	// Token: 0x0400107A RID: 4218
	public float highPoint = 100f;

	// Token: 0x0400107B RID: 4219
	public float distAmount = 450f;

	// Token: 0x0400107C RID: 4220
	public float startAngleFront = 320f;

	// Token: 0x0400107D RID: 4221
	public float startAngleBack = 40f;

	// Token: 0x0400107E RID: 4222
	public float endAngleFront = 350f;

	// Token: 0x0400107F RID: 4223
	public float endAngleBack = 10f;

	// Token: 0x04001080 RID: 4224
	public float enterAngle;
}
