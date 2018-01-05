using System;
using System.Collections;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.UI;

namespace UIWidgets
{
	// Token: 0x02000958 RID: 2392
	[AddComponentMenu("UI/Progressbar", 260)]
	public class Progressbar : MonoBehaviour
	{
		// Token: 0x0600485E RID: 18526 RVA: 0x0018430F File Offset: 0x0018270F
		public Progressbar() : base()
		{
			if (Progressbar.f__mg3 == null)
			{
				Progressbar.f__mg3 = new Func<Progressbar, string>(Progressbar.TextPercent);
			}
			this.textFunc = Progressbar.f__mg3;
		}

		// Token: 0x17000AE0 RID: 2784
		// (get) Token: 0x0600485F RID: 18527 RVA: 0x0018434D File Offset: 0x0018274D
		// (set) Token: 0x06004860 RID: 18528 RVA: 0x00184355 File Offset: 0x00182755
		public int Value
		{
			get
			{
				return this._value;
			}
			set
			{
				if (value > this.Max)
				{
					value = this.Max;
				}
				this._value = value;
				this.UpdateProgressbar();
			}
		}

		// Token: 0x17000AE1 RID: 2785
		// (get) Token: 0x06004861 RID: 18529 RVA: 0x00184378 File Offset: 0x00182778
		// (set) Token: 0x06004862 RID: 18530 RVA: 0x00184380 File Offset: 0x00182780
		public ProgressbarTypes Type
		{
			get
			{
				return this.type;
			}
			set
			{
				this.type = value;
				this.ToggleType();
			}
		}

		// Token: 0x17000AE2 RID: 2786
		// (get) Token: 0x06004863 RID: 18531 RVA: 0x0018438F File Offset: 0x0018278F
		// (set) Token: 0x06004864 RID: 18532 RVA: 0x00184397 File Offset: 0x00182797
		public Image FullBar
		{
			get
			{
				return this.fullBar;
			}
			set
			{
				this.fullBar = value;
			}
		}

		// Token: 0x17000AE3 RID: 2787
		// (get) Token: 0x06004865 RID: 18533 RVA: 0x001843A0 File Offset: 0x001827A0
		private RectTransform FullBarRectTransform
		{
			get
			{
				return this.fullBar.GetComponent<RectTransform>();
			}
		}

		// Token: 0x17000AE4 RID: 2788
		// (get) Token: 0x06004866 RID: 18534 RVA: 0x001843AD File Offset: 0x001827AD
		// (set) Token: 0x06004867 RID: 18535 RVA: 0x001843B5 File Offset: 0x001827B5
		[SerializeField]
		public ProgressbarTextTypes TextType
		{
			get
			{
				return this.textType;
			}
			set
			{
				this.textType = value;
				this.ToggleTextType();
			}
		}

		// Token: 0x17000AE5 RID: 2789
		// (get) Token: 0x06004868 RID: 18536 RVA: 0x001843C4 File Offset: 0x001827C4
		// (set) Token: 0x06004869 RID: 18537 RVA: 0x001843CC File Offset: 0x001827CC
		public Func<Progressbar, string> TextFunc
		{
			get
			{
				return this.textFunc;
			}
			set
			{
				this.textFunc = value;
				this.UpdateText();
			}
		}

		// Token: 0x17000AE6 RID: 2790
		// (get) Token: 0x0600486A RID: 18538 RVA: 0x001843DB File Offset: 0x001827DB
		// (set) Token: 0x0600486B RID: 18539 RVA: 0x001843E3 File Offset: 0x001827E3
		public bool IsAnimationRun { get; protected set; }

		// Token: 0x0600486C RID: 18540 RVA: 0x001843EC File Offset: 0x001827EC
		public static string TextNone(Progressbar bar)
		{
			return string.Empty;
		}

		// Token: 0x0600486D RID: 18541 RVA: 0x001843F3 File Offset: 0x001827F3
		public static string TextPercent(Progressbar bar)
		{
			return string.Format("{0:P0}", (float)bar.Value / (float)bar.Max);
		}

		// Token: 0x0600486E RID: 18542 RVA: 0x00184413 File Offset: 0x00182813
		public static string TextRange(Progressbar bar)
		{
			return string.Format("{0} / {1}", bar.Value, bar.Max);
		}

		// Token: 0x0600486F RID: 18543 RVA: 0x00184438 File Offset: 0x00182838
		public void Animate(int? targetValue = null)
		{
			if (this.currentAnimation != null)
			{
				base.StopCoroutine(this.currentAnimation);
			}
			if (this.Type == ProgressbarTypes.Indeterminate)
			{
				this.currentAnimation = this.IndeterminateAnimation();
			}
			else
			{
				this.currentAnimation = this.DeterminateAnimation((targetValue == null) ? this.Max : targetValue.Value);
			}
			this.IsAnimationRun = true;
			base.StartCoroutine(this.currentAnimation);
		}

		// Token: 0x06004870 RID: 18544 RVA: 0x001844B7 File Offset: 0x001828B7
		public void Stop()
		{
			if (this.IsAnimationRun)
			{
				base.StopCoroutine(this.currentAnimation);
				this.IsAnimationRun = false;
			}
		}

		// Token: 0x06004871 RID: 18545 RVA: 0x001844D8 File Offset: 0x001828D8
		private IEnumerator DeterminateAnimation(int targetValue)
		{
			if (targetValue > this.Max)
			{
				targetValue = this.Max;
			}
			int delta = targetValue - this.Value;
			if (delta != 0)
			{
				for (;;)
				{
					if (delta > 0)
					{
						this._value++;
					}
					else
					{
						this._value--;
					}
					this.UpdateProgressbar();
					if (this._value == targetValue)
					{
						break;
					}
					yield return new WaitForSeconds(this.Speed);
				}
			}
			this.IsAnimationRun = false;
			yield break;
		}

		// Token: 0x06004872 RID: 18546 RVA: 0x001844FC File Offset: 0x001828FC
		private IEnumerator IndeterminateAnimation()
		{
			for (;;)
			{
				Rect r = this.IndeterminateBar.uvRect;
				if (this.Direction == ProgressbarDirection.Horizontal)
				{
					r.x = Time.time * this.Speed % 1f;
				}
				else
				{
					r.y = Time.time * this.Speed % 1f;
				}
				this.IndeterminateBar.uvRect = r;
				yield return null;
			}
			yield break;
		}

		// Token: 0x06004873 RID: 18547 RVA: 0x00184517 File Offset: 0x00182917
		public void Refresh()
		{
			this.FullBar = this.fullBar;
			this.ToggleType();
			this.ToggleTextType();
			this.UpdateProgressbar();
		}

		// Token: 0x06004874 RID: 18548 RVA: 0x00184538 File Offset: 0x00182938
		private void UpdateProgressbar()
		{
			if (this.BarMask != null && this.FullBar != null)
			{
				float num = (float)this.Value / (float)this.Max;
				this.BarMask.sizeDelta = ((this.Direction != ProgressbarDirection.Horizontal) ? new Vector2(this.FullBarRectTransform.rect.width, this.FullBarRectTransform.rect.height * num) : new Vector2(this.FullBarRectTransform.rect.width * num, this.FullBarRectTransform.rect.height));
			}
			this.UpdateText();
		}

		// Token: 0x06004875 RID: 18549 RVA: 0x001845F4 File Offset: 0x001829F4
		private void UpdateText()
		{
			string text = this.textFunc(this);
			if (this.FullBarText != null)
			{
				this.FullBarText.text = text;
			}
			if (this.EmptyBarText != null)
			{
				this.EmptyBarText.text = text;
			}
		}

		// Token: 0x06004876 RID: 18550 RVA: 0x00184648 File Offset: 0x00182A48
		private void ToggleType()
		{
			bool flag = this.type == ProgressbarTypes.Determinate;
			if (this.DeterminateBar != null)
			{
				this.DeterminateBar.gameObject.SetActive(flag);
			}
			if (this.IndeterminateBar != null)
			{
				this.IndeterminateBar.gameObject.SetActive(!flag);
			}
		}

		// Token: 0x06004877 RID: 18551 RVA: 0x001846A8 File Offset: 0x00182AA8
		private void ToggleTextType()
		{
			if (this.TextType == ProgressbarTextTypes.None)
			{
				if (Progressbar.f__mg0 == null)
				{
					Progressbar.f__mg0 = new Func<Progressbar, string>(Progressbar.TextNone);
				}
				this.textFunc = Progressbar.f__mg0;
				return;
			}
			if (this.TextType == ProgressbarTextTypes.Percent)
			{
				if (Progressbar.f__mg1 == null)
				{
					Progressbar.f__mg1 = new Func<Progressbar, string>(Progressbar.TextPercent);
				}
				this.textFunc = Progressbar.f__mg1;
				return;
			}
			if (this.TextType == ProgressbarTextTypes.Range)
			{
				if (Progressbar.f__mg2 == null)
				{
					Progressbar.f__mg2 = new Func<Progressbar, string>(Progressbar.TextRange);
				}
				this.textFunc = Progressbar.f__mg2;
				return;
			}
		}

		// Token: 0x0400313D RID: 12605
		[SerializeField]
		public int Max = 100;

		// Token: 0x0400313E RID: 12606
		[SerializeField]
		private int _value;

		// Token: 0x0400313F RID: 12607
		[SerializeField]
		private ProgressbarDirection Direction;

		// Token: 0x04003140 RID: 12608
		[SerializeField]
		private ProgressbarTypes type;

		// Token: 0x04003141 RID: 12609
		[SerializeField]
		public RawImage IndeterminateBar;

		// Token: 0x04003142 RID: 12610
		[SerializeField]
		public GameObject DeterminateBar;

		// Token: 0x04003143 RID: 12611
		[SerializeField]
		public Image EmptyBar;

		// Token: 0x04003144 RID: 12612
		[SerializeField]
		public Text EmptyBarText;

		// Token: 0x04003145 RID: 12613
		[SerializeField]
		private Image fullBar;

		// Token: 0x04003146 RID: 12614
		[SerializeField]
		public Text FullBarText;

		// Token: 0x04003147 RID: 12615
		[SerializeField]
		public RectTransform BarMask;

		// Token: 0x04003148 RID: 12616
		[SerializeField]
		private ProgressbarTextTypes textType;

		// Token: 0x04003149 RID: 12617
		[SerializeField]
		public float Speed = 0.1f;

		// Token: 0x0400314A RID: 12618
		private Func<Progressbar, string> textFunc;

		// Token: 0x0400314C RID: 12620
		private IEnumerator currentAnimation;

		// Token: 0x0400314D RID: 12621
		[CompilerGenerated]
		private static Func<Progressbar, string> f__mg0;

		// Token: 0x0400314E RID: 12622
		[CompilerGenerated]
		private static Func<Progressbar, string> f__mg1;

		// Token: 0x0400314F RID: 12623
		[CompilerGenerated]
		private static Func<Progressbar, string> f__mg2;

		// Token: 0x04003150 RID: 12624
		[CompilerGenerated]
		private static Func<Progressbar, string> f__mg3;
	}
}
