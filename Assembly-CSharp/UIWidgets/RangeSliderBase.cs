using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace UIWidgets
{
	// Token: 0x0200095A RID: 2394
	public abstract class RangeSliderBase<T> : MonoBehaviour, IPointerClickHandler, IEventSystemHandler where T : struct
	{
		// Token: 0x17000AE7 RID: 2791
		// (get) Token: 0x06004885 RID: 18565 RVA: 0x001849AF File Offset: 0x00182DAF
		// (set) Token: 0x06004886 RID: 18566 RVA: 0x001849B7 File Offset: 0x00182DB7
		public T ValueMin
		{
			get
			{
				return this.valueMin;
			}
			set
			{
				this.valueMin = this.InBoundsMin(value);
				this.UpdateMinHandle();
				this.UpdateFill();
				this.OnValuesChange.Invoke(this.valueMin, this.valueMax);
			}
		}

		// Token: 0x17000AE8 RID: 2792
		// (get) Token: 0x06004887 RID: 18567 RVA: 0x001849E9 File Offset: 0x00182DE9
		// (set) Token: 0x06004888 RID: 18568 RVA: 0x001849F1 File Offset: 0x00182DF1
		public T ValueMax
		{
			get
			{
				return this.valueMax;
			}
			set
			{
				this.valueMax = this.InBoundsMax(value);
				this.UpdateMaxHandle();
				this.UpdateFill();
				this.OnValuesChange.Invoke(this.valueMin, this.valueMax);
			}
		}

		// Token: 0x17000AE9 RID: 2793
		// (get) Token: 0x06004889 RID: 18569 RVA: 0x00184A23 File Offset: 0x00182E23
		// (set) Token: 0x0600488A RID: 18570 RVA: 0x00184A2B File Offset: 0x00182E2B
		public T Step
		{
			get
			{
				return this.step;
			}
			set
			{
				this.step = value;
			}
		}

		// Token: 0x17000AEA RID: 2794
		// (get) Token: 0x0600488B RID: 18571 RVA: 0x00184A34 File Offset: 0x00182E34
		// (set) Token: 0x0600488C RID: 18572 RVA: 0x00184A3C File Offset: 0x00182E3C
		public T LimitMin
		{
			get
			{
				return this.limitMin;
			}
			set
			{
				this.limitMin = value;
				this.ValueMin = this.valueMin;
				this.ValueMax = this.valueMax;
			}
		}

		// Token: 0x17000AEB RID: 2795
		// (get) Token: 0x0600488D RID: 18573 RVA: 0x00184A5D File Offset: 0x00182E5D
		// (set) Token: 0x0600488E RID: 18574 RVA: 0x00184A65 File Offset: 0x00182E65
		public T LimitMax
		{
			get
			{
				return this.limitMax;
			}
			set
			{
				this.limitMax = value;
				this.ValueMin = this.valueMin;
				this.ValueMax = this.valueMax;
			}
		}

		// Token: 0x17000AEC RID: 2796
		// (get) Token: 0x0600488F RID: 18575 RVA: 0x00184A86 File Offset: 0x00182E86
		public RectTransform HandleMinRect
		{
			get
			{
				if (this.handleMin != null && this.handleMinRect == null)
				{
					this.handleMinRect = this.handleMin.GetComponent<RectTransform>();
				}
				return this.handleMinRect;
			}
		}

		// Token: 0x17000AED RID: 2797
		// (get) Token: 0x06004890 RID: 18576 RVA: 0x00184AC1 File Offset: 0x00182EC1
		// (set) Token: 0x06004891 RID: 18577 RVA: 0x00184ACC File Offset: 0x00182ECC
		public RangeSliderHandle HandleMin
		{
			get
			{
				return this.handleMin;
			}
			set
			{
				this.handleMin = value;
				this.handleMin.IsHorizontal = new Func<bool>(this.IsHorizontal);
				this.handleMin.PositionLimits = new Func<Vector2>(this.MinPositionLimits);
				this.handleMin.PositionChanged = new Action<float>(this.UpdateMinValue);
				this.handleMin.OnSubmit = new Action(this.SelectMaxHandle);
				this.handleMin.Increase = new Action(this.IncreaseMin);
				this.handleMin.Decrease = new Action(this.DecreaseMin);
			}
		}

		// Token: 0x17000AEE RID: 2798
		// (get) Token: 0x06004892 RID: 18578 RVA: 0x00184B6E File Offset: 0x00182F6E
		public RectTransform HandleMaxRect
		{
			get
			{
				if (this.handleMax != null && this.handleMaxRect == null)
				{
					this.handleMaxRect = this.handleMax.GetComponent<RectTransform>();
				}
				return this.handleMaxRect;
			}
		}

		// Token: 0x17000AEF RID: 2799
		// (get) Token: 0x06004893 RID: 18579 RVA: 0x00184BA9 File Offset: 0x00182FA9
		// (set) Token: 0x06004894 RID: 18580 RVA: 0x00184BB4 File Offset: 0x00182FB4
		public RangeSliderHandle HandleMax
		{
			get
			{
				return this.handleMax;
			}
			set
			{
				this.handleMax = value;
				this.handleMax.IsHorizontal = new Func<bool>(this.IsHorizontal);
				this.handleMax.PositionLimits = new Func<Vector2>(this.MaxPositionLimits);
				this.handleMax.PositionChanged = new Action<float>(this.UpdateMaxValue);
				this.handleMax.OnSubmit = new Action(this.SelectMinHandle);
				this.handleMax.Increase = new Action(this.IncreaseMax);
				this.handleMax.Decrease = new Action(this.DecreaseMax);
			}
		}

		// Token: 0x17000AF0 RID: 2800
		// (get) Token: 0x06004895 RID: 18581 RVA: 0x00184C56 File Offset: 0x00183056
		public RectTransform RangeSliderRect
		{
			get
			{
				if (this.rangeSliderRect == null)
				{
					this.rangeSliderRect = base.GetComponent<RectTransform>();
				}
				return this.rangeSliderRect;
			}
		}

		// Token: 0x06004896 RID: 18582 RVA: 0x00184C7B File Offset: 0x0018307B
		private void Awake()
		{
		}

		// Token: 0x06004897 RID: 18583 RVA: 0x00184C7D File Offset: 0x0018307D
		private void Init()
		{
			if (this.initCalled)
			{
				return;
			}
			this.initCalled = true;
			this.HandleMin = this.handleMin;
			this.HandleMax = this.handleMax;
			this.UpdateMinHandle();
			this.UpdateMaxHandle();
			this.UpdateFill();
		}

		// Token: 0x06004898 RID: 18584 RVA: 0x00184CBC File Offset: 0x001830BC
		private void Start()
		{
			this.Init();
		}

		// Token: 0x06004899 RID: 18585 RVA: 0x00184CC4 File Offset: 0x001830C4
		public void SetValue(T min, T max)
		{
			this.valueMin = min;
			this.valueMax = max;
			this.ValueMin = this.valueMin;
			this.ValueMax = this.valueMax;
		}

		// Token: 0x0600489A RID: 18586 RVA: 0x00184CEC File Offset: 0x001830EC
		public void SetLimit(T min, T max)
		{
			this.limitMin = min;
			this.limitMax = max;
			this.LimitMin = this.limitMin;
			this.LimitMax = this.limitMax;
		}

		// Token: 0x0600489B RID: 18587 RVA: 0x00184D14 File Offset: 0x00183114
		protected virtual bool IsHorizontal()
		{
			return true;
		}

		// Token: 0x0600489C RID: 18588 RVA: 0x00184D18 File Offset: 0x00183118
		protected float FillSize()
		{
			if (this.IsHorizontal())
			{
				return this.UsableRangeRect.rect.width - this.MinHandleSize() / 2f - this.MaxHandleSize() / 2f;
			}
			return this.UsableRangeRect.rect.height - this.MinHandleSize() / 2f - this.MaxHandleSize() / 2f;
		}

		// Token: 0x0600489D RID: 18589 RVA: 0x00184D8C File Offset: 0x0018318C
		protected float MinHandleSize()
		{
			if (this.IsHorizontal())
			{
				return this.HandleMinRect.rect.width;
			}
			return this.HandleMinRect.rect.height;
		}

		// Token: 0x0600489E RID: 18590 RVA: 0x00184DCC File Offset: 0x001831CC
		protected float MaxHandleSize()
		{
			if (this.IsHorizontal())
			{
				return this.HandleMaxRect.rect.width;
			}
			return this.HandleMaxRect.rect.height;
		}

		// Token: 0x0600489F RID: 18591 RVA: 0x00184E0B File Offset: 0x0018320B
		protected void UpdateMinValue(float position)
		{
			this.valueMin = this.PositionToValue(position - this.GetStartPoint());
			this.UpdateMinHandle();
			this.UpdateFill();
			this.OnValuesChange.Invoke(this.valueMin, this.valueMax);
		}

		// Token: 0x060048A0 RID: 18592 RVA: 0x00184E44 File Offset: 0x00183244
		protected void UpdateMaxValue(float position)
		{
			this.valueMax = this.PositionToValue(position - this.GetStartPoint());
			this.UpdateMaxHandle();
			this.UpdateFill();
			this.OnValuesChange.Invoke(this.valueMin, this.valueMax);
		}

		// Token: 0x060048A1 RID: 18593
		protected abstract float ValueToPosition(T value);

		// Token: 0x060048A2 RID: 18594
		protected abstract T PositionToValue(float position);

		// Token: 0x060048A3 RID: 18595 RVA: 0x00184E80 File Offset: 0x00183280
		protected float GetStartPoint()
		{
			Vector3 position = this.UsableRangeRect.position;
			Vector2 pivot = this.UsableRangeRect.pivot;
			Rect rect = this.UsableRangeRect.rect;
			return (!this.IsHorizontal()) ? (position.y - rect.height * pivot.y + this.MinHandleSize() / 2f) : (position.x - rect.width * pivot.x + this.MinHandleSize() / 2f);
		}

		// Token: 0x060048A4 RID: 18596
		protected abstract Vector2 MinPositionLimits();

		// Token: 0x060048A5 RID: 18597
		protected abstract Vector2 MaxPositionLimits();

		// Token: 0x060048A6 RID: 18598
		protected abstract T InBounds(T value);

		// Token: 0x060048A7 RID: 18599
		protected abstract T InBoundsMin(T value);

		// Token: 0x060048A8 RID: 18600
		protected abstract T InBoundsMax(T value);

		// Token: 0x060048A9 RID: 18601
		protected abstract void IncreaseMin();

		// Token: 0x060048AA RID: 18602
		protected abstract void DecreaseMin();

		// Token: 0x060048AB RID: 18603
		protected abstract void IncreaseMax();

		// Token: 0x060048AC RID: 18604
		protected abstract void DecreaseMax();

		// Token: 0x060048AD RID: 18605 RVA: 0x00184F0B File Offset: 0x0018330B
		protected void UpdateMinHandle()
		{
			this.UpdateHandle(this.HandleMinRect, this.valueMin);
		}

		// Token: 0x060048AE RID: 18606 RVA: 0x00184F1F File Offset: 0x0018331F
		protected void UpdateMaxHandle()
		{
			this.UpdateHandle(this.HandleMaxRect, this.valueMax);
		}

		// Token: 0x060048AF RID: 18607 RVA: 0x00184F34 File Offset: 0x00183334
		private void UpdateFill()
		{
			if (this.IsHorizontal())
			{
				this.FillRect.position = new Vector3(this.HandleMinRect.position.x, this.FillRect.position.y, this.FillRect.position.z);
				Vector2 sizeDelta = new Vector2(this.HandleMaxRect.position.x - this.HandleMinRect.position.x, this.FillRect.sizeDelta.y);
				this.FillRect.sizeDelta = sizeDelta;
			}
			else
			{
				this.FillRect.position = new Vector3(this.FillRect.position.x, this.HandleMinRect.position.y, this.FillRect.position.z);
				Vector2 sizeDelta2 = new Vector2(this.FillRect.sizeDelta.x, this.HandleMaxRect.position.y - this.HandleMinRect.position.y);
				this.FillRect.sizeDelta = sizeDelta2;
			}
		}

		// Token: 0x060048B0 RID: 18608 RVA: 0x00185088 File Offset: 0x00183488
		protected void UpdateHandle(RectTransform handleTransform, T value)
		{
			Vector3 position = handleTransform.position;
			if (this.IsHorizontal())
			{
				position.x = this.ValueToPosition(value);
			}
			else
			{
				position.y = this.ValueToPosition(value);
			}
			handleTransform.position = position;
		}

		// Token: 0x060048B1 RID: 18609 RVA: 0x001850CF File Offset: 0x001834CF
		private void SelectMinHandle()
		{
			if (EventSystem.current != null && !EventSystem.current.alreadySelecting)
			{
				EventSystem.current.SetSelectedGameObject(this.handleMin.gameObject);
			}
		}

		// Token: 0x060048B2 RID: 18610 RVA: 0x00185105 File Offset: 0x00183505
		private void SelectMaxHandle()
		{
			if (EventSystem.current != null && !EventSystem.current.alreadySelecting)
			{
				EventSystem.current.SetSelectedGameObject(this.handleMax.gameObject);
			}
		}

		// Token: 0x060048B3 RID: 18611 RVA: 0x0018513C File Offset: 0x0018353C
		public void OnPointerClick(PointerEventData eventData)
		{
			Vector2 a;
			if (!RectTransformUtility.ScreenPointToLocalPointInRectangle(this.UsableRangeRect, eventData.position, eventData.pressEventCamera, out a))
			{
				return;
			}
			a -= this.UsableRangeRect.rect.position;
			float num = ((!this.IsHorizontal()) ? a.y : a.x) + this.GetStartPoint();
			float num2 = num - this.MaxHandleSize();
			float num3 = (!this.IsHorizontal()) ? this.HandleMinRect.position.y : this.HandleMinRect.position.x;
			float num4 = (!this.IsHorizontal()) ? this.HandleMaxRect.position.y : this.HandleMaxRect.position.x;
			float num5 = num - num3;
			float num6 = num4 - num2;
			if (num5 < num6)
			{
				this.UpdateMinValue(num);
			}
			else
			{
				this.UpdateMaxValue(num2);
			}
		}

		// Token: 0x04003151 RID: 12625
		[SerializeField]
		protected T valueMin;

		// Token: 0x04003152 RID: 12626
		[SerializeField]
		protected T valueMax;

		// Token: 0x04003153 RID: 12627
		[SerializeField]
		protected T step;

		// Token: 0x04003154 RID: 12628
		[SerializeField]
		protected T limitMin;

		// Token: 0x04003155 RID: 12629
		[SerializeField]
		protected T limitMax;

		// Token: 0x04003156 RID: 12630
		[SerializeField]
		protected RangeSliderHandle handleMin;

		// Token: 0x04003157 RID: 12631
		protected RectTransform handleMinRect;

		// Token: 0x04003158 RID: 12632
		[SerializeField]
		protected RangeSliderHandle handleMax;

		// Token: 0x04003159 RID: 12633
		protected RectTransform handleMaxRect;

		// Token: 0x0400315A RID: 12634
		[SerializeField]
		protected RectTransform UsableRangeRect;

		// Token: 0x0400315B RID: 12635
		[SerializeField]
		protected RectTransform FillRect;

		// Token: 0x0400315C RID: 12636
		protected RectTransform rangeSliderRect;

		// Token: 0x0400315D RID: 12637
		public RangeSliderBase<T>.OnChangeEvent OnValuesChange = new RangeSliderBase<T>.OnChangeEvent();

		// Token: 0x0400315E RID: 12638
		public bool WholeNumberOfSteps;

		// Token: 0x0400315F RID: 12639
		private bool initCalled;

		// Token: 0x0200095B RID: 2395
		public class OnChangeEvent : UnityEvent<T, T>
		{
		}
	}
}
