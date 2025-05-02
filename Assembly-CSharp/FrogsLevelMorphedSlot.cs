using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x0200029E RID: 670
public class FrogsLevelMorphedSlot : AbstractPausableComponent
{
	// Token: 0x170002C3 RID: 707
	// (get) Token: 0x06001E35 RID: 7733 RVA: 0x000197C4 File Offset: 0x000179C4
	// (set) Token: 0x06001E36 RID: 7734 RVA: 0x000197CC File Offset: 0x000179CC
	public Slots.Mode mode { get; set; }

	// Token: 0x170002C4 RID: 708
	// (get) Token: 0x06001E37 RID: 7735 RVA: 0x000197D5 File Offset: 0x000179D5
	// (set) Token: 0x06001E38 RID: 7736 RVA: 0x000197DD File Offset: 0x000179DD
	public FrogsLevelMorphedSlot.State state { get; set; }

	// Token: 0x170002C5 RID: 709
	// (get) Token: 0x06001E39 RID: 7737 RVA: 0x000197E6 File Offset: 0x000179E6
	// (set) Token: 0x06001E3A RID: 7738 RVA: 0x000197EE File Offset: 0x000179EE
	public FrogsLevelMorphedSlot.Action action { get; set; }

	// Token: 0x06001E3B RID: 7739 RVA: 0x000B283C File Offset: 0x000B0A3C
	public override void Awake()
	{
		base.Awake();
		this.offsets = new Dictionary<Slots.Mode, float>();
		this.offsets[Slots.Mode.Snake] = 0.4f;
		this.offsets[Slots.Mode.Tiger] = -0.095f;
		this.offsets[Slots.Mode.Bison] = 0.095f;
		this.offsets[Slots.Mode.Oni] = -0.4f;
		this.mat = base.GetComponent<Renderer>().material;
		this.SetTexture(this.textures.Get(FrogsLevelMorphedSlot.State.Normal, 0));
		this.SetOffset(this.offsets[Slots.Mode.Snake]);
	}

	// Token: 0x06001E3C RID: 7740 RVA: 0x000197F7 File Offset: 0x000179F7
	public void Start()
	{
		base.StartCoroutine(this.animate_cr());
	}

	// Token: 0x06001E3D RID: 7741 RVA: 0x00019806 File Offset: 0x00017A06
	public void SetTexture(Texture2D texture)
	{
		this.mat.mainTexture = texture;
	}

	// Token: 0x06001E3E RID: 7742 RVA: 0x000B28D4 File Offset: 0x000B0AD4
	public void SetOffset(float y)
	{
		Vector2 mainTextureOffset = this.mat.mainTextureOffset;
		mainTextureOffset.y = y;
		this.mat.mainTextureOffset = mainTextureOffset;
	}

	// Token: 0x06001E3F RID: 7743 RVA: 0x00019814 File Offset: 0x00017A14
	public void StartSpin()
	{
		AudioManager.PlayLoop("level_frogs_morphed_spin_loop");
		this.emitAudioFromObject.Add("level_frogs_morphed_spin_loop");
		this.StopAllCoroutines();
		base.StartCoroutine(this.animate_cr());
		base.StartCoroutine(this.spin_cr());
	}

	// Token: 0x06001E40 RID: 7744 RVA: 0x000B2904 File Offset: 0x000B0B04
	public void StopSpin(Slots.Mode mode)
	{
		AudioManager.Stop("level_frogs_morphed_spin_loop");
		this.emitAudioFromObject.Add("level_frogs_morphed_spin_loop");
		AudioManager.Play("level_frogs_morphed_spin");
		this.emitAudioFromObject.Add("level_frogs_morphed_spin");
		this.mode = mode;
		this.action = FrogsLevelMorphedSlot.Action.Ending;
	}

	// Token: 0x06001E41 RID: 7745 RVA: 0x00019850 File Offset: 0x00017A50
	public void Flash()
	{
		base.StartCoroutine(this.flash_cr());
	}

	// Token: 0x06001E42 RID: 7746 RVA: 0x000B2954 File Offset: 0x000B0B54
	public IEnumerator animate_cr()
	{
		int frame = 0;
		for (;;)
		{
			yield return CupheadTime.WaitForSeconds(this, 0.06f);
			this.SetTexture(this.textures.Get(this.state, frame));
			frame = (int)Mathf.Repeat((float)(frame + 1), 3f);
		}
		yield break;
	}

	// Token: 0x06001E43 RID: 7747 RVA: 0x000B2970 File Offset: 0x000B0B70
	public IEnumerator spin_cr()
	{
		float offset = this.mat.mainTextureOffset.y;
		this.action = FrogsLevelMorphedSlot.Action.Spinning;
		while (this.action == FrogsLevelMorphedSlot.Action.Spinning)
		{
			offset = Mathf.Repeat(offset + 5f * CupheadTime.Delta, 1f);
			this.SetOffset(offset);
			yield return null;
		}
		float t = 0f;
		this.SetOffset(-3f);
		float startOffset = this.mat.mainTextureOffset.y;
		while (t < 1f)
		{
			float val = t / 1f;
			float o = EaseUtils.Ease(EaseUtils.EaseType.easeOutElastic, startOffset, this.offsets[this.mode], val);
			this.SetOffset(o);
			t += CupheadTime.Delta;
			yield return null;
		}
		yield break;
	}

	// Token: 0x06001E44 RID: 7748 RVA: 0x000B298C File Offset: 0x000B0B8C
	public IEnumerator flash_cr()
	{
		this.state = FrogsLevelMorphedSlot.State.Flashing;
		yield return CupheadTime.WaitForSeconds(this, 0.2f);
		this.state = FrogsLevelMorphedSlot.State.Normal;
		yield break;
	}

	// Token: 0x06001E45 RID: 7749 RVA: 0x0001985F File Offset: 0x00017A5F
	public override void OnDestroy()
	{
		base.OnDestroy();
		Object.Destroy(this.mat);
		this.textures.flashing = null;
		this.textures.normal = null;
	}

	// Token: 0x040018B8 RID: 6328
	public const float STOP_OFFSET = 3f;

	// Token: 0x040018B9 RID: 6329
	public const float STOP_TIME = 1f;

	// Token: 0x040018BA RID: 6330
	public const float OFFSET_SPEED = 5f;

	// Token: 0x040018BB RID: 6331
	public const float FLASH_TIME = 0.2f;

	// Token: 0x040018BC RID: 6332
	[SerializeField]
	public FrogsLevelMorphedSlot.Textures textures;

	// Token: 0x040018C0 RID: 6336
	public Material mat;

	// Token: 0x040018C1 RID: 6337
	public Dictionary<Slots.Mode, float> offsets;

	// Token: 0x02000D71 RID: 3441
	public enum State
	{
		// Token: 0x04006161 RID: 24929
		Normal,
		// Token: 0x04006162 RID: 24930
		Flashing
	}

	// Token: 0x02000D72 RID: 3442
	public enum Action
	{
		// Token: 0x04006164 RID: 24932
		Static,
		// Token: 0x04006165 RID: 24933
		Spinning,
		// Token: 0x04006166 RID: 24934
		Ending
	}

	// Token: 0x02000D73 RID: 3443
	[Serializable]
	public class Textures
	{
		// Token: 0x060069FB RID: 27131 RVA: 0x0004A96F File Offset: 0x00048B6F
		public Texture2D Get(FrogsLevelMorphedSlot.State state, int frame)
		{
			if (state == FrogsLevelMorphedSlot.State.Normal || state != FrogsLevelMorphedSlot.State.Flashing)
			{
				return this.normal[frame];
			}
			return this.flashing[frame];
		}

		// Token: 0x04006167 RID: 24935
		public Texture2D[] normal;

		// Token: 0x04006168 RID: 24936
		public Texture2D[] flashing;
	}
}
