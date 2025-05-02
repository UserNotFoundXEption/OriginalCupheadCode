using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace TMPro
{
	// Token: 0x02000663 RID: 1635
	[ExecuteInEditMode]
	[RequireComponent(typeof(RectTransform))]
	[AddComponentMenu("Layout/Text Container")]
	public class TextContainer : UIBehaviour
	{
		// Token: 0x17000615 RID: 1557
		// (get) Token: 0x060044E8 RID: 17640 RVA: 0x00036BC6 File Offset: 0x00034DC6
		// (set) Token: 0x060044E9 RID: 17641 RVA: 0x00036BCE File Offset: 0x00034DCE
		public bool hasChanged
		{
			get
			{
				return this.m_hasChanged;
			}
			set
			{
				this.m_hasChanged = value;
			}
		}

		// Token: 0x17000616 RID: 1558
		// (get) Token: 0x060044EA RID: 17642 RVA: 0x00036BD7 File Offset: 0x00034DD7
		// (set) Token: 0x060044EB RID: 17643 RVA: 0x00036BDF File Offset: 0x00034DDF
		public Vector2 pivot
		{
			get
			{
				return this.m_pivot;
			}
			set
			{
				if (this.m_pivot != value)
				{
					this.m_pivot = value;
					this.m_anchorPosition = this.GetAnchorPosition(this.m_pivot);
					this.m_hasChanged = true;
					this.OnContainerChanged();
				}
			}
		}

		// Token: 0x17000617 RID: 1559
		// (get) Token: 0x060044EC RID: 17644 RVA: 0x00036C18 File Offset: 0x00034E18
		// (set) Token: 0x060044ED RID: 17645 RVA: 0x00036C20 File Offset: 0x00034E20
		public TextContainerAnchors anchorPosition
		{
			get
			{
				return this.m_anchorPosition;
			}
			set
			{
				if (this.m_anchorPosition != value)
				{
					this.m_anchorPosition = value;
					this.m_pivot = this.GetPivot(this.m_anchorPosition);
					this.m_hasChanged = true;
					this.OnContainerChanged();
				}
			}
		}

		// Token: 0x17000618 RID: 1560
		// (get) Token: 0x060044EE RID: 17646 RVA: 0x00036C54 File Offset: 0x00034E54
		// (set) Token: 0x060044EF RID: 17647 RVA: 0x00036C5C File Offset: 0x00034E5C
		public Rect rect
		{
			get
			{
				return this.m_rect;
			}
			set
			{
				if (this.m_rect != value)
				{
					this.m_rect = value;
					this.m_hasChanged = true;
					this.OnContainerChanged();
				}
			}
		}

		// Token: 0x17000619 RID: 1561
		// (get) Token: 0x060044F0 RID: 17648 RVA: 0x00036C83 File Offset: 0x00034E83
		// (set) Token: 0x060044F1 RID: 17649 RVA: 0x0013DD94 File Offset: 0x0013BF94
		public Vector2 size
		{
			get
			{
				return new Vector2(this.m_rect.width, this.m_rect.height);
			}
			set
			{
				if (new Vector2(this.m_rect.width, this.m_rect.height) != value)
				{
					this.SetRect(value);
					this.m_hasChanged = true;
					this.m_isDefaultWidth = false;
					this.m_isDefaultHeight = false;
					this.OnContainerChanged();
				}
			}
		}

		// Token: 0x1700061A RID: 1562
		// (get) Token: 0x060044F2 RID: 17650 RVA: 0x00036CA0 File Offset: 0x00034EA0
		// (set) Token: 0x060044F3 RID: 17651 RVA: 0x00036CAD File Offset: 0x00034EAD
		public float width
		{
			get
			{
				return this.m_rect.width;
			}
			set
			{
				this.SetRect(new Vector2(value, this.m_rect.height));
				this.m_hasChanged = true;
				this.m_isDefaultWidth = false;
				this.OnContainerChanged();
			}
		}

		// Token: 0x1700061B RID: 1563
		// (get) Token: 0x060044F4 RID: 17652 RVA: 0x00036CDA File Offset: 0x00034EDA
		// (set) Token: 0x060044F5 RID: 17653 RVA: 0x00036CE7 File Offset: 0x00034EE7
		public float height
		{
			get
			{
				return this.m_rect.height;
			}
			set
			{
				this.SetRect(new Vector2(this.m_rect.width, value));
				this.m_hasChanged = true;
				this.m_isDefaultHeight = false;
				this.OnContainerChanged();
			}
		}

		// Token: 0x1700061C RID: 1564
		// (get) Token: 0x060044F6 RID: 17654 RVA: 0x00036D14 File Offset: 0x00034F14
		public bool isDefaultWidth
		{
			get
			{
				return this.m_isDefaultWidth;
			}
		}

		// Token: 0x1700061D RID: 1565
		// (get) Token: 0x060044F7 RID: 17655 RVA: 0x00036D1C File Offset: 0x00034F1C
		public bool isDefaultHeight
		{
			get
			{
				return this.m_isDefaultHeight;
			}
		}

		// Token: 0x1700061E RID: 1566
		// (get) Token: 0x060044F8 RID: 17656 RVA: 0x00036D24 File Offset: 0x00034F24
		// (set) Token: 0x060044F9 RID: 17657 RVA: 0x00036D2C File Offset: 0x00034F2C
		public bool isAutoFitting
		{
			get
			{
				return this.m_isAutoFitting;
			}
			set
			{
				this.m_isAutoFitting = value;
			}
		}

		// Token: 0x1700061F RID: 1567
		// (get) Token: 0x060044FA RID: 17658 RVA: 0x00036D35 File Offset: 0x00034F35
		public Vector3[] corners
		{
			get
			{
				return this.m_corners;
			}
		}

		// Token: 0x17000620 RID: 1568
		// (get) Token: 0x060044FB RID: 17659 RVA: 0x00036D3D File Offset: 0x00034F3D
		public Vector3[] worldCorners
		{
			get
			{
				return this.m_worldCorners;
			}
		}

		// Token: 0x17000621 RID: 1569
		// (get) Token: 0x060044FC RID: 17660 RVA: 0x00036D45 File Offset: 0x00034F45
		// (set) Token: 0x060044FD RID: 17661 RVA: 0x00036D4D File Offset: 0x00034F4D
		public Vector4 margins
		{
			get
			{
				return this.m_margins;
			}
			set
			{
				if (this.m_margins != value)
				{
					this.m_margins = value;
					this.m_hasChanged = true;
					this.OnContainerChanged();
				}
			}
		}

		// Token: 0x17000622 RID: 1570
		// (get) Token: 0x060044FE RID: 17662 RVA: 0x00036D74 File Offset: 0x00034F74
		public RectTransform rectTransform
		{
			get
			{
				if (this.m_rectTransform == null)
				{
					this.m_rectTransform = base.GetComponent<RectTransform>();
				}
				return this.m_rectTransform;
			}
		}

		// Token: 0x17000623 RID: 1571
		// (get) Token: 0x060044FF RID: 17663 RVA: 0x00036D99 File Offset: 0x00034F99
		public TextMeshPro textMeshPro
		{
			get
			{
				if (this.m_textMeshPro == null)
				{
					this.m_textMeshPro = base.GetComponent<TextMeshPro>();
				}
				return this.m_textMeshPro;
			}
		}

		// Token: 0x06004500 RID: 17664 RVA: 0x0013DDEC File Offset: 0x0013BFEC
		public override void Awake()
		{
			this.m_rectTransform = this.rectTransform;
			if (this.m_rectTransform == null)
			{
				Vector2 pivot = this.m_pivot;
				this.m_rectTransform = base.gameObject.AddComponent<RectTransform>();
				this.m_pivot = pivot;
			}
			this.m_textMeshPro = (base.GetComponent(typeof(TextMeshPro)) as TextMeshPro);
			if (this.m_rect.width == 0f || this.m_rect.height == 0f)
			{
				if (this.m_textMeshPro != null && this.m_textMeshPro.anchor != TMP_Compatibility.AnchorPositions.None)
				{
					this.m_isDefaultHeight = true;
					int num = (int)this.m_textMeshPro.anchor;
					this.m_textMeshPro.anchor = TMP_Compatibility.AnchorPositions.None;
					if (num == 9)
					{
						switch (this.m_textMeshPro.alignment)
						{
						case TextAlignmentOptions.TopLeft:
							this.m_textMeshPro.alignment = TextAlignmentOptions.BaselineLeft;
							break;
						case TextAlignmentOptions.Top:
							this.m_textMeshPro.alignment = TextAlignmentOptions.Baseline;
							break;
						case TextAlignmentOptions.TopRight:
							this.m_textMeshPro.alignment = TextAlignmentOptions.BaselineRight;
							break;
						case TextAlignmentOptions.TopJustified:
							this.m_textMeshPro.alignment = TextAlignmentOptions.BaselineJustified;
							break;
						}
						num = 3;
					}
					this.m_anchorPosition = (TextContainerAnchors)num;
					this.m_pivot = this.GetPivot(this.m_anchorPosition);
					if (this.m_textMeshPro.lineLength == 72f)
					{
						this.m_rect.size = this.m_textMeshPro.GetPreferredValues(this.m_textMeshPro.text);
					}
					else
					{
						this.m_rect.width = this.m_textMeshPro.lineLength;
						this.m_rect.height = this.m_textMeshPro.GetPreferredValues(this.m_rect.width, float.PositiveInfinity).y;
					}
				}
				else
				{
					this.m_isDefaultWidth = true;
					this.m_isDefaultHeight = true;
					this.m_pivot = this.GetPivot(this.m_anchorPosition);
					this.m_rect.width = 20f;
					this.m_rect.height = 5f;
					this.m_rectTransform.sizeDelta = this.size;
				}
				this.m_margins = new Vector4(0f, 0f, 0f, 0f);
				this.UpdateCorners();
			}
		}

		// Token: 0x06004501 RID: 17665 RVA: 0x00036DBE File Offset: 0x00034FBE
		public override void OnEnable()
		{
			this.OnContainerChanged();
		}

		// Token: 0x06004502 RID: 17666 RVA: 0x00036DC6 File Offset: 0x00034FC6
		public override void OnDisable()
		{
		}

		// Token: 0x06004503 RID: 17667 RVA: 0x0013E04C File Offset: 0x0013C24C
		public void OnContainerChanged()
		{
			this.UpdateCorners();
			if (this.m_rectTransform != null)
			{
				this.m_rectTransform.sizeDelta = this.size;
				this.m_rectTransform.hasChanged = true;
			}
			if (this.textMeshPro != null)
			{
				this.m_textMeshPro.SetVerticesDirty();
				this.m_textMeshPro.margin = this.m_margins;
			}
		}

		// Token: 0x06004504 RID: 17668 RVA: 0x0013E0BC File Offset: 0x0013C2BC
		public override void OnRectTransformDimensionsChange()
		{
			if (this.rectTransform == null)
			{
				this.m_rectTransform = base.gameObject.AddComponent<RectTransform>();
			}
			if (this.m_rectTransform.sizeDelta != TextContainer.k_defaultSize)
			{
				this.size = this.m_rectTransform.sizeDelta;
			}
			this.pivot = this.m_rectTransform.pivot;
			this.m_hasChanged = true;
			this.OnContainerChanged();
		}

		// Token: 0x06004505 RID: 17669 RVA: 0x00036DC8 File Offset: 0x00034FC8
		public void SetRect(Vector2 size)
		{
			this.m_rect = new Rect(this.m_rect.x, this.m_rect.y, size.x, size.y);
		}

		// Token: 0x06004506 RID: 17670 RVA: 0x0013E134 File Offset: 0x0013C334
		public void UpdateCorners()
		{
			this.m_corners[0] = new Vector3(-this.m_pivot.x * this.m_rect.width, -this.m_pivot.y * this.m_rect.height);
			this.m_corners[1] = new Vector3(-this.m_pivot.x * this.m_rect.width, (1f - this.m_pivot.y) * this.m_rect.height);
			this.m_corners[2] = new Vector3((1f - this.m_pivot.x) * this.m_rect.width, (1f - this.m_pivot.y) * this.m_rect.height);
			this.m_corners[3] = new Vector3((1f - this.m_pivot.x) * this.m_rect.width, -this.m_pivot.y * this.m_rect.height);
			if (this.m_rectTransform != null)
			{
				this.m_rectTransform.pivot = this.m_pivot;
			}
		}

		// Token: 0x06004507 RID: 17671 RVA: 0x0013E290 File Offset: 0x0013C490
		public Vector2 GetPivot(TextContainerAnchors anchor)
		{
			Vector2 zero = Vector2.zero;
			switch (anchor)
			{
			case TextContainerAnchors.TopLeft:
				zero..ctor(0f, 1f);
				break;
			case TextContainerAnchors.Top:
				zero..ctor(0.5f, 1f);
				break;
			case TextContainerAnchors.TopRight:
				zero..ctor(1f, 1f);
				break;
			case TextContainerAnchors.Left:
				zero..ctor(0f, 0.5f);
				break;
			case TextContainerAnchors.Middle:
				zero..ctor(0.5f, 0.5f);
				break;
			case TextContainerAnchors.Right:
				zero..ctor(1f, 0.5f);
				break;
			case TextContainerAnchors.BottomLeft:
				zero..ctor(0f, 0f);
				break;
			case TextContainerAnchors.Bottom:
				zero..ctor(0.5f, 0f);
				break;
			case TextContainerAnchors.BottomRight:
				zero..ctor(1f, 0f);
				break;
			}
			return zero;
		}

		// Token: 0x06004508 RID: 17672 RVA: 0x0013E39C File Offset: 0x0013C59C
		public TextContainerAnchors GetAnchorPosition(Vector2 pivot)
		{
			if (pivot == new Vector2(0f, 1f))
			{
				return TextContainerAnchors.TopLeft;
			}
			if (pivot == new Vector2(0.5f, 1f))
			{
				return TextContainerAnchors.Top;
			}
			if (pivot == new Vector2(1f, 1f))
			{
				return TextContainerAnchors.TopRight;
			}
			if (pivot == new Vector2(0f, 0.5f))
			{
				return TextContainerAnchors.Left;
			}
			if (pivot == new Vector2(0.5f, 0.5f))
			{
				return TextContainerAnchors.Middle;
			}
			if (pivot == new Vector2(1f, 0.5f))
			{
				return TextContainerAnchors.Right;
			}
			if (pivot == new Vector2(0f, 0f))
			{
				return TextContainerAnchors.BottomLeft;
			}
			if (pivot == new Vector2(0.5f, 0f))
			{
				return TextContainerAnchors.Bottom;
			}
			if (pivot == new Vector2(1f, 0f))
			{
				return TextContainerAnchors.BottomRight;
			}
			return TextContainerAnchors.Custom;
		}

		// Token: 0x04003556 RID: 13654
		public bool m_hasChanged;

		// Token: 0x04003557 RID: 13655
		[SerializeField]
		public Vector2 m_pivot;

		// Token: 0x04003558 RID: 13656
		[SerializeField]
		public TextContainerAnchors m_anchorPosition = TextContainerAnchors.Middle;

		// Token: 0x04003559 RID: 13657
		[SerializeField]
		public Rect m_rect;

		// Token: 0x0400355A RID: 13658
		public bool m_isDefaultWidth;

		// Token: 0x0400355B RID: 13659
		public bool m_isDefaultHeight;

		// Token: 0x0400355C RID: 13660
		public bool m_isAutoFitting;

		// Token: 0x0400355D RID: 13661
		public Vector3[] m_corners = new Vector3[4];

		// Token: 0x0400355E RID: 13662
		public Vector3[] m_worldCorners = new Vector3[4];

		// Token: 0x0400355F RID: 13663
		[SerializeField]
		public Vector4 m_margins;

		// Token: 0x04003560 RID: 13664
		public RectTransform m_rectTransform;

		// Token: 0x04003561 RID: 13665
		public static Vector2 k_defaultSize = new Vector2(100f, 100f);

		// Token: 0x04003562 RID: 13666
		public TextMeshPro m_textMeshPro;
	}
}
