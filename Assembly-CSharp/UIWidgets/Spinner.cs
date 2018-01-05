using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace UIWidgets
{
	// Token: 0x02000966 RID: 2406
	[AddComponentMenu("UI/Spinner", 270)]
	public class Spinner : InputField, IPointerDownHandler, IEventSystemHandler
	{
		// Token: 0x17000AF3 RID: 2803
		// (get) Token: 0x060048E3 RID: 18659 RVA: 0x00185E40 File Offset: 0x00184240
		// (set) Token: 0x060048E4 RID: 18660 RVA: 0x00185E48 File Offset: 0x00184248
		public int Min
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

		// Token: 0x17000AF4 RID: 2804
		// (get) Token: 0x060048E5 RID: 18661 RVA: 0x00185E51 File Offset: 0x00184251
		// (set) Token: 0x060048E6 RID: 18662 RVA: 0x00185E59 File Offset: 0x00184259
		public int Max
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

		// Token: 0x17000AF5 RID: 2805
		// (get) Token: 0x060048E7 RID: 18663 RVA: 0x00185E62 File Offset: 0x00184262
		// (set) Token: 0x060048E8 RID: 18664 RVA: 0x00185E6A File Offset: 0x0018426A
		public int Step
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

		// Token: 0x17000AF6 RID: 2806
		// (get) Token: 0x060048E9 RID: 18665 RVA: 0x00185E73 File Offset: 0x00184273
		// (set) Token: 0x060048EA RID: 18666 RVA: 0x00185E7B File Offset: 0x0018427B
		public int Value
		{
			get
			{
				return this._value;
			}
			set
			{
				this._value = this.InBounds(value);
				base.text = this._value.ToString();
			}
		}

		// Token: 0x17000AF7 RID: 2807
		// (get) Token: 0x060048EB RID: 18667 RVA: 0x00185EA1 File Offset: 0x001842A1
		// (set) Token: 0x060048EC RID: 18668 RVA: 0x00185EAC File Offset: 0x001842AC
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

		// Token: 0x17000AF8 RID: 2808
		// (get) Token: 0x060048ED RID: 18669 RVA: 0x00185F8A File Offset: 0x0018438A
		// (set) Token: 0x060048EE RID: 18670 RVA: 0x00185F94 File Offset: 0x00184394
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

		// Token: 0x060048EF RID: 18671 RVA: 0x00186072 File Offset: 0x00184472
		public void Plus()
		{
			this.Value += this.Step;
		}

		// Token: 0x060048F0 RID: 18672 RVA: 0x00186087 File Offset: 0x00184487
		public void Minus()
		{
			this.Value -= this.Step;
		}

		// Token: 0x060048F1 RID: 18673 RVA: 0x0018609C File Offset: 0x0018449C
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

		// Token: 0x060048F2 RID: 18674 RVA: 0x00186114 File Offset: 0x00184514
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

		// Token: 0x060048F3 RID: 18675 RVA: 0x00186130 File Offset: 0x00184530
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

		// Token: 0x060048F4 RID: 18676 RVA: 0x0018614B File Offset: 0x0018454B
		public void OnMinusClick()
		{
			this.Minus();
			this.onMinusClick.Invoke();
		}

		// Token: 0x060048F5 RID: 18677 RVA: 0x0018615E File Offset: 0x0018455E
		public void OnPlusClick()
		{
			this.Plus();
			this.onPlusClick.Invoke();
		}

		// Token: 0x060048F6 RID: 18678 RVA: 0x00186171 File Offset: 0x00184571
		public void OnPlusButtonDown(PointerEventData eventData)
		{
			this.corutine = this.HoldPlus();
			base.StartCoroutine(this.corutine);
		}

		// Token: 0x060048F7 RID: 18679 RVA: 0x0018618C File Offset: 0x0018458C
		public void OnPlusButtonUp(PointerEventData eventData)
		{
			base.StopCoroutine(this.corutine);
		}

		// Token: 0x060048F8 RID: 18680 RVA: 0x0018619A File Offset: 0x0018459A
		public void OnMinusButtonDown(PointerEventData eventData)
		{
			this.corutine = this.HoldMinus();
			base.StartCoroutine(this.corutine);
		}

		// Token: 0x060048F9 RID: 18681 RVA: 0x001861B5 File Offset: 0x001845B5
		public void OnMinusButtonUp(PointerEventData eventData)
		{
			base.StopCoroutine(this.corutine);
		}

		// Token: 0x060048FA RID: 18682 RVA: 0x001861C3 File Offset: 0x001845C3
		protected void onDestroy()
		{
			base.onValueChanged.RemoveListener(new UnityAction<string>(this.ValueChange));
			base.onEndEdit.RemoveListener(new UnityAction<string>(this.ValueEndEdit));
			this.PlusButton = null;
			this.MinusButton = null;
		}

		// Token: 0x060048FB RID: 18683 RVA: 0x00186201 File Offset: 0x00184601
		private void ValueChange(string value)
		{
			if (value.Length == 0)
			{
				value = "0";
			}
			this._value = int.Parse(value);
			this.onValueChangeInt.Invoke(this.Value);
		}

		// Token: 0x060048FC RID: 18684 RVA: 0x00186232 File Offset: 0x00184632
		private void ValueEndEdit(string value)
		{
			if (value.Length == 0)
			{
				value = "0";
			}
			this._value = int.Parse(value);
			this.onEndEditInt.Invoke(this.Value);
		}

		// Token: 0x060048FD RID: 18685 RVA: 0x00186264 File Offset: 0x00184664
		private char IntValidate(string validateText, int charIndex, char addedChar)
		{
			string text = validateText.Insert(charIndex, addedChar.ToString());
			if (this.Min > 0 && charIndex == 0 && charIndex == 45)
			{
				return '\0';
			}
			int num = (text.Length <= 0 || text[0] != '-') ? 1 : 2;
			if (text.Length >= num)
			{
				int num2;
				if (!int.TryParse(text, out num2))
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

		// Token: 0x060048FE RID: 18686 RVA: 0x001862F6 File Offset: 0x001846F6
		private int InBounds(int value)
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

		// Token: 0x04003171 RID: 12657
		[SerializeField]
		private int _min;

		// Token: 0x04003172 RID: 12658
		[SerializeField]
		private int _max = 100;

		// Token: 0x04003173 RID: 12659
		[SerializeField]
		private int _step = 1;

		// Token: 0x04003174 RID: 12660
		[SerializeField]
		private int _value;

		// Token: 0x04003175 RID: 12661
		[SerializeField]
		public float HoldStartDelay = 0.5f;

		// Token: 0x04003176 RID: 12662
		[SerializeField]
		public float HoldChangeDelay = 0.1f;

		// Token: 0x04003177 RID: 12663
		[SerializeField]
		private ButtonAdvanced _plusButton;

		// Token: 0x04003178 RID: 12664
		[SerializeField]
		private ButtonAdvanced _minusButton;

		// Token: 0x04003179 RID: 12665
		public UnityEvent<int> onValueChangeInt = new OnChangeEventInt();

		// Token: 0x0400317A RID: 12666
		public UnityEvent<int> onEndEditInt = new SubmitEventInt();

		// Token: 0x0400317B RID: 12667
		public UnityEvent onPlusClick = new UnityEvent();

		// Token: 0x0400317C RID: 12668
		public UnityEvent onMinusClick = new UnityEvent();

		// Token: 0x0400317D RID: 12669
		private IEnumerator corutine;
	}
}
