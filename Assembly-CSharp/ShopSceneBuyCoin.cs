using System;
using System.Collections;
using UnityEngine;

// Token: 0x0200059C RID: 1436
public class ShopSceneBuyCoin : MonoBehaviour
{
	// Token: 0x06003CAD RID: 15533 RVA: 0x0011614C File Offset: 0x0011434C
	public void Start()
	{
		this.velocity = new Vector2(Random.Range(this.VelocityXMin, this.VelocityXMax), Random.Range(this.VelocityYMin, this.VelocityYMax));
		this.randomRotation = new Vector2((float)Random.Range(-500, 500), (float)Random.Range(-500, 500));
		base.StartCoroutine(this.scaledown_cr());
	}

	// Token: 0x06003CAE RID: 15534 RVA: 0x001161C0 File Offset: 0x001143C0
	public void Update()
	{
		base.transform.position += (this.velocity + new Vector2(-300f, this.accumulatedGravity)) * Time.fixedDeltaTime;
		this.accumulatedGravity += -100f;
		base.transform.Rotate(this.randomRotation * Time.deltaTime);
	}

	// Token: 0x06003CAF RID: 15535 RVA: 0x00116240 File Offset: 0x00114440
	public IEnumerator scaledown_cr()
	{
		Vector2 startScale = base.transform.localScale;
		float t = 0f;
		float TIME = 1f;
		while (t < TIME)
		{
			float val = t / TIME;
			float newAlpha = Mathf.Lerp(1f, 0f, EaseUtils.Ease(EaseUtils.EaseType.easeInOutSine, 0f, 1f, val));
			Color newColor = this.spriteRenderer.color;
			newColor.a = newAlpha;
			this.spriteRenderer.color = newColor;
			t += Time.deltaTime;
			yield return null;
		}
		Object.Destroy(base.gameObject);
		yield break;
	}

	// Token: 0x06003CB0 RID: 15536 RVA: 0x00031084 File Offset: 0x0002F284
	public void OnDestroy()
	{
		this.spriteRenderer = null;
	}

	// Token: 0x0400301A RID: 12314
	public float VelocityXMin = -500f;

	// Token: 0x0400301B RID: 12315
	public float VelocityXMax = 500f;

	// Token: 0x0400301C RID: 12316
	public float VelocityYMin = 500f;

	// Token: 0x0400301D RID: 12317
	public float VelocityYMax = 1000f;

	// Token: 0x0400301E RID: 12318
	public const float GRAVITY = -100f;

	// Token: 0x0400301F RID: 12319
	public Vector2 velocity;

	// Token: 0x04003020 RID: 12320
	public Vector2 randomRotation;

	// Token: 0x04003021 RID: 12321
	public float accumulatedGravity;

	// Token: 0x04003022 RID: 12322
	[SerializeField]
	public SpriteRenderer spriteRenderer;

	// Token: 0x04003023 RID: 12323
	[SerializeField]
	public bool isCoinA;
}
