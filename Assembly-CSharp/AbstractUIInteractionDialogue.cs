using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x020000E5 RID: 229
public abstract class AbstractUIInteractionDialogue : AbstractMonoBehaviour
{
	// Token: 0x06000AD6 RID: 2774 RVA: 0x00009C07 File Offset: 0x00007E07
	public AbstractUIInteractionDialogue()
	{
	}

	// Token: 0x170001BB RID: 443
	// (get) Token: 0x06000AD7 RID: 2775 RVA: 0x00009C0F File Offset: 0x00007E0F
	public virtual float OpenTime
	{
		get
		{
			return 0.3f;
		}
	}

	// Token: 0x170001BC RID: 444
	// (get) Token: 0x06000AD8 RID: 2776 RVA: 0x00009C16 File Offset: 0x00007E16
	public virtual float CloseTime
	{
		get
		{
			return 0.3f;
		}
	}

	// Token: 0x170001BD RID: 445
	// (get) Token: 0x06000AD9 RID: 2777 RVA: 0x00009C1D File Offset: 0x00007E1D
	public virtual float OpenScale
	{
		get
		{
			return 1f;
		}
	}

	// Token: 0x170001BE RID: 446
	// (get) Token: 0x06000ADA RID: 2778 RVA: 0x00009C24 File Offset: 0x00007E24
	// (set) Token: 0x06000ADB RID: 2779 RVA: 0x00009C31 File Offset: 0x00007E31
	public string Text
	{
		get
		{
			return this.tmpText.text;
		}
		set
		{
			this.tmpText.text = value;
		}
	}

	// Token: 0x170001BF RID: 447
	// (get) Token: 0x06000ADC RID: 2780 RVA: 0x00009C3F File Offset: 0x00007E3F
	public virtual float PreferredWidth
	{
		get
		{
			return this.tmpText.preferredWidth + this.glyph.preferredWidth;
		}
	}

	// Token: 0x06000ADD RID: 2781 RVA: 0x00009C58 File Offset: 0x00007E58
	public void Start()
	{
		base.transform.localScale = Vector3.zero;
	}

	// Token: 0x06000ADE RID: 2782 RVA: 0x0007C2B8 File Offset: 0x0007A4B8
	public virtual void Init(AbstractUIInteractionDialogue.Properties properties, PlayerInput player, Vector2 offset)
	{
		float num = 40f;
		this.target = player.transform;
		this.dialogueOffset = offset;
		int id;
		if (Parser.IntTryParse(properties.text, out id))
		{
			this.Text = Localization.Translate(id).text.ToUpper();
			this.tmpText.font = Localization.Instance.fonts[(int)Localization.language][28].fontAsset;
		}
		else
		{
			this.Text = properties.text.ToUpper();
		}
		this.glyph.rewiredPlayerId = (int)player.playerId;
		this.glyph.button = properties.button;
		this.glyph.Init();
		this.back.SetSizeWithCurrentAnchors(0, this.PreferredWidth + 10f);
		this.back.SetSizeWithCurrentAnchors(1, num + 11f);
		base.TweenValue(0f, 1f, this.OpenTime, EaseUtils.EaseType.linear, new AbstractMonoBehaviour.TweenUpdateHandler(this.OpenTween));
	}

	// Token: 0x06000ADF RID: 2783 RVA: 0x0007C3C8 File Offset: 0x0007A5C8
	public void Close()
	{
		this.closeScale = base.transform.localScale.x;
		this.StopAllCoroutines();
		base.TweenValue(0f, 1f, this.CloseTime, EaseUtils.EaseType.linear, new AbstractMonoBehaviour.TweenUpdateHandler(this.CloseTween));
	}

	// Token: 0x06000AE0 RID: 2784 RVA: 0x0007C41C File Offset: 0x0007A61C
	public virtual void OpenTween(float value)
	{
		float num = 40f;
		this.back.SetSizeWithCurrentAnchors(0, this.PreferredWidth + 10f);
		this.back.SetSizeWithCurrentAnchors(1, num + 11f);
		base.transform.localScale = Vector3.one * EaseUtils.Ease(EaseUtils.EaseType.easeOutSine, 0f, this.OpenScale, value);
	}

	// Token: 0x06000AE1 RID: 2785 RVA: 0x0007C484 File Offset: 0x0007A684
	public virtual void CloseTween(float value)
	{
		base.transform.localScale = Vector3.one * EaseUtils.Ease(EaseUtils.EaseType.easeInBack, this.closeScale, 0f, value);
		if (base.transform.localScale.x < 0.001f)
		{
			this.StopAllCoroutines();
			Object.Destroy(base.gameObject);
		}
	}

	// Token: 0x0400085D RID: 2141
	public const float PADDINGH = 10f;

	// Token: 0x0400085E RID: 2142
	public const float PADDINGV = 11f;

	// Token: 0x0400085F RID: 2143
	[SerializeField]
	public Text uiText;

	// Token: 0x04000860 RID: 2144
	[SerializeField]
	public TextMeshProUGUI tmpText;

	// Token: 0x04000861 RID: 2145
	[SerializeField]
	public CupheadGlyph glyph;

	// Token: 0x04000862 RID: 2146
	[SerializeField]
	public RectTransform back;

	// Token: 0x04000863 RID: 2147
	public Transform target;

	// Token: 0x04000864 RID: 2148
	public Vector2 dialogueOffset;

	// Token: 0x04000865 RID: 2149
	public float closeScale;

	// Token: 0x0200095D RID: 2397
	public enum AnimationType
	{
		// Token: 0x0400464B RID: 17995
		Full,
		// Token: 0x0400464C RID: 17996
		Individual
	}

	// Token: 0x0200095E RID: 2398
	[Serializable]
	public class Properties
	{
		// Token: 0x060054D2 RID: 21714 RVA: 0x001C465C File Offset: 0x001C285C
		public Properties()
		{
			this.text = string.Empty;
			this.subtext = string.Empty;
			this.button = CupheadButton.Accept;
			this.animationType = AbstractUIInteractionDialogue.AnimationType.Full;
		}

		// Token: 0x060054D3 RID: 21715 RVA: 0x001C46B4 File Offset: 0x001C28B4
		public Properties(string text)
		{
			this.text = text;
			this.subtext = string.Empty;
			this.button = CupheadButton.Accept;
			this.animationType = AbstractUIInteractionDialogue.AnimationType.Full;
		}

		// Token: 0x060054D4 RID: 21716 RVA: 0x001C4708 File Offset: 0x001C2908
		public Properties(string text, CupheadButton button)
		{
			this.text = text;
			this.subtext = string.Empty;
			this.button = button;
			this.animationType = AbstractUIInteractionDialogue.AnimationType.Full;
		}

		// Token: 0x060054D5 RID: 21717 RVA: 0x001C475C File Offset: 0x001C295C
		public Properties(string text, CupheadButton button, AbstractUIInteractionDialogue.AnimationType animationType)
		{
			this.text = text;
			this.subtext = string.Empty;
			this.button = button;
			this.animationType = animationType;
		}

		// Token: 0x0400464D RID: 17997
		public const AbstractUIInteractionDialogue.AnimationType DEFAULT_ANIM_TYPE = AbstractUIInteractionDialogue.AnimationType.Full;

		// Token: 0x0400464E RID: 17998
		public const CupheadButton DEFAULT_BUTTON = CupheadButton.Accept;

		// Token: 0x0400464F RID: 17999
		public string text = string.Empty;

		// Token: 0x04004650 RID: 18000
		public string subtext = string.Empty;

		// Token: 0x04004651 RID: 18001
		public AbstractUIInteractionDialogue.AnimationType animationType;

		// Token: 0x04004652 RID: 18002
		public CupheadButton button = CupheadButton.Accept;
	}
}
