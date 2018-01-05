using System;
using System.Collections;
using System.Globalization;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace UIWidgets
{
	// Token: 0x02000969 RID: 2409
	[AddComponentMenu("UI/SpinnerFloat", 270)]
	public class SpinnerFloat : InputField, IPointerDownHandler, IEventSystemHandler
	{
		// Token: 0x17000AF9 RID: 2809
		// (get) Token: 0x06004902 RID: 18690 RVA: 0x0018654E File Offset: 0x0018494E
		// (set) Token: 0x06004903 RID: 18691 RVA: 0x00186556 File Offset: 0x00184956
		public float Min
		{
			get
			{
				return this._min;
			}
			set
			{
				this._min = value;
			}
		}

		// Token: 0x17000AFA RID: 2810
		// (get) Token: 0x06004904 RID: 18692 RVA: 0x0018655F File Offset: 0x0018495F
		// (set) Token: 0x06004905 RID: 18693 RVA: 0x00186567 File Offset: 0x00184967
		public float Max
		{
			get
			{
				return this._max;
			}
			set
			{
				this._max = value;
			}
		}

		// Token: 0x17000AFB RID: 2811
		// (get) Token: 0x06004906 RID: 18694 RVA: 0x00186570 File Offset: 0x00184970
		// (set) Token: 0x06004907 RID: 18695 RVA: 0x00186578 File Offset: 0x00184978
		public float Step
		{
			get
			{
				return this._step;
			}
			set
			{
				this._step = value;
			}
		}

		// Token: 0x17000AFC RID: 2812
		// (get) Token: 0x06004908 RID: 18696 RVA: 0x00186581 File Offset: 0x00184981
		// (set) Token: 0x06004909 RID: 18697 RVA: 0x00186589 File Offset: 0x00184989
		public float Value
		{
			get
			{
				return this._value;
			}
			set
			{
				this._value = this.InBounds(value);
				base.text = this._value.ToString("0.00");
			}
		}

		// Token: 0x17000AFD RID: 2813
		// (get) Token: 0x0600490A RID: 18698 RVA: 0x001865AE File Offset: 0x001849AE
		// (set) Token: 0x0600490B RID: 18699 RVA: 0x001865B8 File Offset: 0x001849B8
		public ButtonAdvanced PlusButton
		{
			get
			{
				return this._plusButton;
			}
			set
			{
				if (this._plusButton != null)
				{
					this._plusButton.onClick.RemoveListener(new UnityAction(this.OnPlusClick));
					this._plusButton.onPointerDown.RemoveListener(new UnityAction<PointerEventData>(this.OnPlusButtonDown));
					this._plusButton.onPointerUp.RemoveListener(new UnityAction<PointerEventData>(this.OnPlusButtonUp));
				}
				this._plusButton = value;
				if (this._plusButton != null)
				{
					this._plusButton.onClick.AddListener(new UnityAction(this.OnPlusClick));
					this._plusButton.onPointerDown.AddListener(new UnityAction<PointerEventData>(this.OnPlusButtonDown));
					this._plusButton.onPointerUp.AddListener(new UnityAction<PointerEventData>(this.OnPlusButtonUp));
				}
			}
		}

		// Token: 0x17000AFE RID: 2814
		// (get) Token: 0x0600490C RID: 18700 RVA: 0x00186696 File Offset: 0x00184A96
		// (set) Token: 0x0600490D RID: 18701 RVA: 0x001866A0 File Offset: 0x00184AA0
		public ButtonAdvanced MinusButton
		{
			get
			{
				return this._minusButton;
			}
			set
			{
				if (this._minusButton != null)
				{
					this._minusButton.onClick.RemoveListener(new UnityAction(this.OnMinusClick));
					this._minusButton.onPointerDown.RemoveListener(new UnityAction<PointerEventData>(this.OnMinusButtonDown));
					this._minusButton.onPointerUp.RemoveListener(new UnityAction<PointerEventData>(this.OnMinusButtonUp));
				}
				this._minusButton = value;
				if (this._minusButton != null)
				{
					this._minusButton.onClick.AddListener(new UnityAction(this.OnMinusClick));
					this._minusButton.onPointerDown.AddListener(new UnityAction<PointerEventData>(this.OnMinusButtonDown));
					this._minusButton.onPointerUp.AddListener(new UnityAction<PointerEventData>(this.OnMinusButtonUp));
				}
			}
		}

		// Token: 0x0600490E RID: 18702 RVA: 0x0018677E File Offset: 0x00184B7E
		public void Plus()
		{
			this.Value += this.Step;
		}

		// Token: 0x0600490F RID: 18703 RVA: 0x00186793 File Offset: 0x00184B93
		public void Minus()
		{
			this.Value -= this.Step;
		}

		// Token: 0x06004910 RID: 18704 RVA: 0x001867A8 File Offset: 0x00184BA8
		protected override void Start()
		{
			base.Start();
			base.onValidateInput = new InputField.OnValidateInput(this.IntValidate);
			base.onValueChanged.AddListener(new UnityAction<string>(this.ValueChange));
			base.onEndEdit.AddListener(new UnityAction<string>(this.ValueEndEdit));
			this.PlusButton = this._plusButton;
			this.MinusButton = this._minusButton;
			this.Value = this._value;
		}

		// Token: 0x06004911 RID: 18705 RVA: 0x00186820 File Offset: 0x00184C20
		private IEnumerator HoldPlus()
		{
			yield return new WaitForSeconds(this.HoldStartDelay);
			for (;;)
			{
				this.Plus();
				yield return new WaitForSeconds(this.HoldChangeDelay);
			}
			yield break;
		}

		// Token: 0x06004912 RID: 18706 RVA: 0x0018683C File Offset: 0x00184C3C
		private IEnumerator HoldMinus()
		{
			yield return new WaitForSeconds(this.HoldStartDelay);
			for (;;)
			{
				this.Minus();
				yield return new WaitForSeconds(this.HoldChangeDelay);
			}
			yield break;
		}

		// Token: 0x06004913 RID: 18707 RVA: 0x00186857 File Offset: 0x00184C57
		public void OnMinusClick()
		{
			this.Minus();
			this.onMinusClick.Invoke();
		}

		// Token: 0x06004914 RID: 18708 RVA: 0x0018686A File Offset: 0x00184C6A
		public void OnPlusClick()
		{
			this.Plus();
			this.onPlusClick.Invoke();
		}

		// Token: 0x06004915 RID: 18709 RVA: 0x0018687D File Offset: 0x00184C7D
		public void OnPlusButtonDown(PointerEventData eventData)
		{
			this.corutine = this.HoldPlus();
			base.StartCoroutine(this.corutine);
		}

		// Token: 0x06004916 RID: 18710 RVA: 0x00186898 File Offset: 0x00184C98
		public void OnPlusButtonUp(PointerEventData eventData)
		{
			base.StopCoroutine(this.corutine);
		}

		// Token: 0x06004917 RID: 18711 RVA: 0x001868A6 File Offset: 0x00184CA6
		public void OnMinusButtonDown(PointerEventData eventData)
		{
			this.corutine = this.HoldMinus();
			base.StartCoroutine(this.corutine);
		}

		// Token: 0x06004918 RID: 18712 RVA: 0x001868C1 File Offset: 0x00184CC1
		public void OnMinusButtonUp(PointerEventData eventData)
		{
			base.StopCoroutine(this.corutine);
		}

		// Token: 0x06004919 RID: 18713 RVA: 0x001868CF File Offset: 0x00184CCF
		protected void onDestroy()
		{
			base.onValueChanged.RemoveListener(new UnityAction<string>(this.ValueChange));
			base.onEndEdit.RemoveListener(new UnityAction<string>(this.ValueEndEdit));
			this.PlusButton = null;
			this.MinusButton = null;
		}

		// Token: 0x0600491A RID: 18714 RVA: 0x00186910 File Offset: 0x00184D10
		private void ValueChange(string value)
		{
			if (value.Length == 0)
			{
				value = "0";
			}
			float value2;
			if (float.TryParse(value, this.NumberStyle, this.Culture, out value2))
			{
				this._value = this.InBounds(value2);
			}
			else
			{
				this.Value = this._value;
			}
			this.onValueChangeFloat.Invoke(this.Value);
		}

		// Token: 0x0600491B RID: 18715 RVA: 0x00186978 File Offset: 0x00184D78
		private void ValueEndEdit(string value)
		{
			if (value.Length == 0)
			{
				value = "0";
			}
			float value2;
			if (float.TryParse(value, this.NumberStyle, this.Culture, out value2))
			{
				this.Value = value2;
			}
			else
			{
				this.Value = this._value;
			}
			this.onEndEditFloat.Invoke(this.Value);
		}

		// Token: 0x0600491C RID: 18716 RVA: 0x001869DC File Offset: 0x00184DDC
		private char IntValidate(string validateText, int charIndex, char addedChar)
		{
			string text = validateText.Insert(charIndex, addedChar.ToString());
			if (this.Min > 0f && charIndex == 0 && charIndex == 45)
			{
				return '\0';
			}
			int num = (text.Length <= 0 || text[0] != '-') ? 1 : 2;
			if (text.Length >= num)
			{
				float num2;
				if (!float.TryParse(text, this.NumberStyle, this.Culture, out num2))
				{
					return '\0';
				}
				if (num2 != this.InBounds(num2))
				{
					return '\0';
				}
				this._value = num2;
			}
			return addedChar;
		}

		// Token: 0x0600491D RID: 18717 RVA: 0x00186A7E File Offset: 0x00184E7E
		private float InBounds(float value)
		{
			if (value < this._min)
			{
				return this._min;
			}
			if (value > this._max)
			{
				return this._max;
			}
			return value;
		}

		// Token: 0x0400317E RID: 12670
		[SerializeField]
		private float _min;

		// Token: 0x0400317F RID: 12671
		[SerializeField]
		private float _max = 100f;

		// Token: 0x04003180 RID: 12672
		[SerializeField]
		private float _step = 1f;

		// Token: 0x04003181 RID: 12673
		[SerializeField]
		private float _value;

		// Token: 0x04003182 RID: 12674
		[SerializeField]
		public float HoldStartDelay = 0.5f;

		// Token: 0x04003183 RID: 12675
		[SerializeField]
		public float HoldChangeDelay = 0.1f;

		// Token: 0x04003184 RID: 12676
		[SerializeField]
		private ButtonAdvanced _plusButton;

		// Token: 0x04003185 RID: 12677
		[SerializeField]
		private ButtonAdvanced _minusButton;

		// Token: 0x04003186 RID: 12678
		private NumberStyles NumberStyle = NumberStyles.AllowLeadingSign | NumberStyles.AllowDecimalPoint | NumberStyles.AllowThousands;

		// Token: 0x04003187 RID: 12679
		private CultureInfo Culture = CultureInfo.InvariantCulture;

		// Token: 0x04003188 RID: 12680
		public OnChangeEventFloat onValueChangeFloat = new OnChangeEventFloat();

		// Token: 0x04003189 RID: 12681
		public SubmitEventFloat onEndEditFloat = new SubmitEventFloat();

		// Token: 0x0400318A RID: 12682
		public UnityEvent onPlusClick = new UnityEvent();

		// Token: 0x0400318B RID: 12683
		public UnityEvent onMinusClick = new UnityEvent();

		// Token: 0x0400318C RID: 12684
		private IEnumerator corutine;
	}
}
