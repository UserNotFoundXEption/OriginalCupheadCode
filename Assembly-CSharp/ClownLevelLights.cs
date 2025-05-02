using System;
using System.Collections;
using UnityEngine;

// Token: 0x020001A6 RID: 422
public class ClownLevelLights : AbstractPausableComponent
{
	// Token: 0x06001424 RID: 5156 RVA: 0x00010F14 File Offset: 0x0000F114
	public void Start()
	{
		this.redLight.enabled = false;
		this.greenLight.enabled = false;
		base.StartCoroutine(this.warning_lights_cr());
	}

	// Token: 0x06001425 RID: 5157 RVA: 0x00010F3B File Offset: 0x0000F13B
	public void StartWarningLights()
	{
		AudioManager.PlayLoop("clown_warning_lights_loop");
		this.emitAudioFromObject.Add("clown_warning_lights_loop");
		this.redLight.enabled = true;
		this.greenLight.enabled = true;
		this.isOn = true;
	}

	// Token: 0x06001426 RID: 5158 RVA: 0x00010F76 File Offset: 0x0000F176
	public void StopWarningLights()
	{
		AudioManager.Stop("clown_warning_lights_loop");
		this.redLight.enabled = false;
		this.greenLight.enabled = false;
		this.isOn = false;
	}

	// Token: 0x06001427 RID: 5159 RVA: 0x00099938 File Offset: 0x00097B38
	public IEnumerator warning_lights_cr()
	{
		float t = 0f;
		for (;;)
		{
			this.redLight.color = new Color(1f, 1f, 1f, 1f);
			this.greenLight.color = new Color(1f, 1f, 1f, 0f);
			if (this.isOn)
			{
				t = 0f;
				while (t < 0.083f)
				{
					this.redLight.color = new Color(1f, 1f, 1f, 1f - t / 0.083f);
					this.greenLight.color = new Color(1f, 1f, 1f, t / 0.083f);
					t += CupheadTime.Delta;
					yield return null;
				}
				this.redLight.color = new Color(1f, 1f, 1f, 0f);
				this.greenLight.color = new Color(1f, 1f, 1f, 1f);
				t = 0f;
				yield return CupheadTime.WaitForSeconds(this, 0.083f);
				yield return null;
				while (t < 0.083f)
				{
					this.redLight.color = new Color(1f, 1f, 1f, t / 0.083f);
					this.greenLight.color = new Color(1f, 1f, 1f, 1f - t / 0.083f);
					t += CupheadTime.Delta;
					yield return null;
				}
				this.redLight.color = new Color(1f, 1f, 1f, 1f);
				this.greenLight.color = new Color(1f, 1f, 1f, 0f);
				yield return CupheadTime.WaitForSeconds(this, 0.083f);
				yield return null;
			}
			yield return null;
		}
		yield break;
	}

	// Token: 0x04001065 RID: 4197
	[SerializeField]
	public SpriteRenderer redLight;

	// Token: 0x04001066 RID: 4198
	[SerializeField]
	public SpriteRenderer greenLight;

	// Token: 0x04001067 RID: 4199
	public const float fadeTime = 0.083f;

	// Token: 0x04001068 RID: 4200
	public bool isOn;
}
