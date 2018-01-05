using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace UIWidgets
{
	// Token: 0x0200092A RID: 2346
	public abstract class CenteredSliderBase<T> : MonoBehaviour, IPointerClickHandler, IEventSystemHandler where T : struct
	{
		// Token: 0x17000A96 RID: 2710
		// (get) Token: 0x0600464F RID: 17999 RVA: 0x0017E238 File Offset: 0x0017C638
		// (set) Token: 0x06004650 RID: 18000 RVA: 0x0017E240 File Offset: 0x0017C640
		public T Value
		{
			get
			{
				return this._value;
			}
			set
			{
				this._value = this.InBounds(value);
				this.UpdateHandle();
				this.OnValuesChange.Invoke(this._value);
			}
		}

		// Token: 0x17000A97 RID: 2711
		// (get) Token: 0x06004651 RID: 18001 RVA: 0x0017E266 File Offset: 0x0017C666
		// (set) Token: 0x06004652 RID: 18002 RVA: 0x0017E26E File Offset: 0x0017C66E
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

		// Token: 0x17000A98 RID: 2712
		// (get) Token: 0x06004653 RID: 18003 RVA: 0x0017E277 File Offset: 0x0017C677
		// (set) Token: 0x06004654 RID: 18004 RVA: 0x0017E27F File Offset: 0x0017C67F
		public T LimitMin
		{
			get
			{
				return this.limitMin;
			}
			set
			{
				this.limitMin = value;
				this.Value = this._value;
			}
		}

		// Token: 0x17000A99 RID: 2713
		// (get) Token: 0x06004655 RID: 18005 RVA: 0x0017E294 File Offset: 0x0017C694
		// (set) Token: 0x06004656 RID: 18006 RVA: 0x0017E29C File Offset: 0x0017C69C
		public T LimitMax
		{
			get
			{
				return this.limitMax;
			}
			set
			{
				this.limitMax = value;
				this.Value = this._value;
			}
		}

		// Token: 0x17000A9A RID: 2714
		// (get) Token: 0x06004657 RID: 18007 RVA: 0x0017E2B1 File Offset: 0x0017C6B1
		public RectTransform HandleRect
		{
			get
			{
				if (this.handle != null && this.handleRect == null)
				{
					this.handleRect = this.handle.GetComponent<RectTransform>();
				}
				return this.handleRect;
			}
		}

		// Token: 0x17000A9B RID: 2715
		// (get) Token: 0x06004658 RID: 18008 RVA: 0x0017E2EC File Offset: 0x0017C6EC
		// (set) Token: 0x06004659 RID: 18009 RVA: 0x0017E2F4 File Offset: 0x0017C6F4
		public RangeSliderHandle Handle
		{
			get
			{
				return this.handle;
			}
			set
			{
				this.handle = value;
				this.handle.IsHorizontal = new Func<bool>(this.IsHorizontal);
				this.handle.PositionLimits = new Func<Vector2>(this.PositionLimits);
				this.handle.PositionChanged = new Action<float>(this.UpdateValue);
				this.handle.Increase = new Action(this.Increase);
				this.handle.Decrease = new Action(this.Decrease);
			}
		}

		// Token: 0x17000A9C RID: 2716
		// (get) Token: 0x0600465A RID: 18010 RVA: 0x0017E37F File Offset: 0x0017C77F
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

		// Token: 0x0600465B RID: 18011 RVA: 0x0017E3A4 File Offset: 0x0017C7A4
		private void Awake()
		{
		}

		// Token: 0x0600465C RID: 18012 RVA: 0x0017E3A6 File Offset: 0x0017C7A6
		private void Init()
		{
			if (this.initCalled)
			{
				return;
			}
			this.initCalled = true;
			this.Handle = this.handle;
			this.UpdateHandle();
			this.UpdateFill();
		}

		// Token: 0x0600465D RID: 18013 RVA: 0x0017E3D3 File Offset: 0x0017C7D3
		private void Start()
		{
			this.Init();
		}

		// Token: 0x0600465E RID: 18014 RVA: 0x0017E3DB File Offset: 0x0017C7DB
		public void SetLimit(T min, T max)
		{
			this.limitMin = min;
			this.limitMax = max;
			this.LimitMin = this.limitMin;
			this.LimitMax = this.limitMax;
		}

		// Token: 0x0600465F RID: 18015 RVA: 0x0017E403 File Offset: 0x0017C803
		protected virtual bool IsHorizontal()
		{
			return true;
		}

		// Token: 0x06004660 RID: 18016 RVA: 0x0017E408 File Offset: 0x0017C808
		protected float RangeSize()
		{
			if (this.IsHorizontal())
			{
				return this.UsableRangeRect.rect.width - this.HandleSize() / 2f;
			}
			return this.UsableRangeRect.rect.height - this.HandleSize() / 2f;
		}

		// Token: 0x06004661 RID: 18017 RVA: 0x0017E464 File Offset: 0x0017C864
		protected float HandleSize()
		{
			if (this.IsHorizontal())
			{
				return this.HandleRect.rect.width;
			}
			return this.HandleRect.rect.height;
		}

		// Token: 0x06004662 RID: 18018 RVA: 0x0017E4A3 File Offset: 0x0017C8A3
		protected void UpdateValue(float position)
		{
			this._value = this.PositionToValue(position - this.GetStartPoint());
			this.UpdateHandle();
			this.OnValuesChange.Invoke(this._value);
		}

		// Token: 0x06004663 RID: 18019
		protected abstract float ValueToPosition(T value);

		// Token: 0x06004664 RID: 18020
		protected abstract T PositionToValue(float position);

		// Token: 0x06004665 RID: 18021 RVA: 0x0017E4D0 File Offset: 0x0017C8D0
		protected float GetStartPoint()
		{
			Vector3 position = this.UsableRangeRect.position;
			Vector2 pivot = this.UsableRangeRect.pivot;
			Rect rect = this.UsableRangeRect.rect;
			return (!this.IsHorizontal()) ? (position.y - rect.height * pivot.y) : (position.x - rect.width * pivot.x);
		}

		// Token: 0x06004666 RID: 18022
		protected abstract Vector2 PositionLimits();

		// Token: 0x06004667 RID: 18023
		protected abstract T InBounds(T value);

		// Token: 0x06004668 RID: 18024
		protected abstract void Increase();

		// Token: 0x06004669 RID: 18025
		protected abstract void Decrease();

		// Token: 0x0600466A RID: 18026 RVA: 0x0017E544 File Offset: 0x0017C944
		protected void UpdateHandle()
		{
			Vector3 position = this.HandleRect.position;
			if (this.IsHorizontal())
			{
				position.x = this.ValueToPosition(this._value);
			}
			else
			{
				position.y = this.ValueToPosition(this._value);
			}
			this.HandleRect.position = position;
			this.UpdateFill();
		}

		// Token: 0x0600466B RID: 18027
		protected abstract bool IsPositiveValue();

		// Token: 0x0600466C RID: 18028 RVA: 0x0017E5A8 File Offset: 0x0017C9A8
		private void UpdateFill()
		{
			this.FillRect.anchorMin = new Vector2(0.5f, 0.5f);
			this.FillRect.anchorMax = new Vector2(0.5f, 0.5f);
			if (this.IsHorizontal())
			{
				if (this.IsPositiveValue())
				{
					this.FillRect.pivot = new Vector2(0f, 0.5f);
					this.FillRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, this.HandleRect.localPosition.x - this.UsableRangeRect.localPosition.x);
				}
				else
				{
					this.FillRect.pivot = new Vector2(1f, 0.5f);
					this.FillRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, this.UsableRangeRect.localPosition.x - this.HandleRect.localPosition.x);
				}
			}
			else if (this.IsPositiveValue())
			{
				this.FillRect.pivot = new Vector2(0.5f, 0f);
				this.FillRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, this.HandleRect.localPosition.y - this.UsableRangeRect.localPosition.y);
			}
			else
			{
				this.FillRect.pivot = new Vector2(0.5f, 1f);
				this.FillRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, this.UsableRangeRect.localPosition.y - this.HandleRect.localPosition.y);
			}
		}

		// Token: 0x0600466D RID: 18029 RVA: 0x0017E754 File Offset: 0x0017CB54
		public void OnPointerClick(PointerEventData eventData)
		{
			Vector2 a;
			if (!RectTransformUtility.ScreenPointToLocalPointInRectangle(this.UsableRangeRect, eventData.position, eventData.pressEventCamera, out a))
			{
				return;
			}
			a -= this.UsableRangeRect.rect.position;
			float position = ((!this.IsHorizontal()) ? a.y : a.x) + this.GetStartPoint();
			this.UpdateValue(position);
		}

		// Token: 0x0400302C RID: 12332
		[SerializeField]
		protected T _value;

		// Token: 0x0400302D RID: 12333
		[SerializeField]
		protected T step;

		// Token: 0x0400302E RID: 12334
		[SerializeField]
		protected T limitMin;

		// Token: 0x0400302F RID: 12335
		[SerializeField]
		protected T limitMax;

		// Token: 0x04003030 RID: 12336
		[SerializeField]
		protected RangeSliderHandle handle;

		// Token: 0x04003031 RID: 12337
		protected RectTransform handleRect;

		// Token: 0x04003032 RID: 12338
		[SerializeField]
		protected RectTransform UsableRangeRect;

		// Token: 0x04003033 RID: 12339
		[SerializeField]
		protected RectTransform FillRect;

		// Token: 0x04003034 RID: 12340
		protected RectTransform rangeSliderRect;

		// Token: 0x04003035 RID: 12341
		public CenteredSliderBase<T>.OnChangeEvent OnValuesChange = new CenteredSliderBase<T>.OnChangeEvent();

		// Token: 0x04003036 RID: 12342
		public bool WholeNumberOfSteps;

		// Token: 0x04003037 RID: 12343
		private bool initCalled;

		// Token: 0x0200092B RID: 2347
		public class OnChangeEvent : UnityEvent<T>
		{
		}
	}
}
