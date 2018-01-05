using System;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace UIWidgets
{
	// Token: 0x0200095E RID: 2398
	[RequireComponent(typeof(RectTransform))]
	public class RangeSliderHandle : Selectable, IDragHandler, IInitializePotentialDragHandler, ISubmitHandler, IEventSystemHandler
	{
		// Token: 0x060048C3 RID: 18627 RVA: 0x001855F4 File Offset: 0x001839F4
		public RangeSliderHandle()
        {
            if (RangeSliderHandle.f__mg0 == null)
			{
				RangeSliderHandle.f__mg0 = new Func<bool>(RangeSliderHandle.ReturnTrue);
			}
			this.IsHorizontal = RangeSliderHandle.f__mg0;
			if (RangeSliderHandle.f__mg1 == null)
			{
				RangeSliderHandle.f__mg1 = new Action(RangeSliderHandle.DoNothing);
			}
			this.Decrease = RangeSliderHandle.f__mg1;
			if (RangeSliderHandle.f__mg2 == null)
			{
				RangeSliderHandle.f__mg2 = new Action(RangeSliderHandle.DoNothing);
			}
			this.Increase = RangeSliderHandle.f__mg2;
			if (RangeSliderHandle.f__mg3 == null)
			{
				RangeSliderHandle.f__mg3 = new Action(RangeSliderHandle.DoNothing);
			}
			this.OnSubmit = RangeSliderHandle.f__mg3;
            //base();
        }

		// Token: 0x17000AF1 RID: 2801
		// (get) Token: 0x060048C4 RID: 18628 RVA: 0x00185693 File Offset: 0x00183A93
		public RectTransform RectTransform
		{
			get
			{
				if (this.rectTransform == null)
				{
					this.rectTransform = base.GetComponent<RectTransform>();
				}
				return this.rectTransform;
			}
		}

		// Token: 0x060048C5 RID: 18629 RVA: 0x001856B8 File Offset: 0x00183AB8
		private bool CanDrag(PointerEventData eventData)
		{
			return this.IsActive() && this.IsInteractable() && eventData.button == PointerEventData.InputButton.Left;
		}

		// Token: 0x060048C6 RID: 18630 RVA: 0x001856DC File Offset: 0x00183ADC
		private static bool ReturnTrue()
		{
			return true;
		}

		// Token: 0x060048C7 RID: 18631 RVA: 0x001856DF File Offset: 0x00183ADF
		private static void DoNothing()
		{
		}

		// Token: 0x060048C8 RID: 18632 RVA: 0x001856E1 File Offset: 0x00183AE1
		void ISubmitHandler.OnSubmit(BaseEventData eventData)
		{
			this.OnSubmit();
		}

		// Token: 0x060048C9 RID: 18633 RVA: 0x001856F0 File Offset: 0x00183AF0
		public virtual void OnDrag(PointerEventData eventData)
		{
			if (!this.CanDrag(eventData))
			{
				return;
			}
			Vector2 a;
			if (!RectTransformUtility.ScreenPointToLocalPointInRectangle(this.RectTransform, eventData.position, eventData.pressEventCamera, out a))
			{
				return;
			}
			a -= this.RectTransform.rect.position;
			Vector2 vector = this.PositionLimits();
			Vector3 position = this.RectTransform.position;
			if (this.IsHorizontal())
			{
				position.x = Mathf.Clamp(position.x + a.x, vector.x, vector.y);
			}
			else
			{
				position.y = Mathf.Clamp(position.y + a.y, vector.x, vector.y);
			}
			if (this.IsNewPosition(position))
			{
				this.RectTransform.position = position;
				this.PositionChanged((!this.IsHorizontal()) ? position.y : position.x);
			}
		}

		// Token: 0x060048CA RID: 18634 RVA: 0x00185808 File Offset: 0x00183C08
		private bool IsNewPosition(Vector3 pos)
		{
			if (this.IsHorizontal())
			{
				return this.RectTransform.position.x != pos.x;
			}
			return this.RectTransform.position.y != pos.y;
		}

		// Token: 0x060048CB RID: 18635 RVA: 0x00185864 File Offset: 0x00183C64
		public virtual void OnInitializePotentialDrag(PointerEventData eventData)
		{
			eventData.useDragThreshold = false;
		}

		// Token: 0x060048CC RID: 18636 RVA: 0x00185870 File Offset: 0x00183C70
		public override void OnMove(AxisEventData eventData)
		{
			if (!this.IsActive() || !this.IsInteractable())
			{
				base.OnMove(eventData);
				return;
			}
			switch (eventData.moveDir)
			{
			case MoveDirection.Left:
				if (this.IsHorizontal() && this.FindSelectableOnLeft() == null)
				{
					this.Decrease();
				}
				else
				{
					base.OnMove(eventData);
				}
				break;
			case MoveDirection.Up:
				if (!this.IsHorizontal() && this.FindSelectableOnUp() == null)
				{
					this.Increase();
				}
				else
				{
					base.OnMove(eventData);
				}
				break;
			case MoveDirection.Right:
				if (this.IsHorizontal() && this.FindSelectableOnRight() == null)
				{
					this.Increase();
				}
				else
				{
					base.OnMove(eventData);
				}
				break;
			case MoveDirection.Down:
				if (!this.IsHorizontal() && this.FindSelectableOnDown() == null)
				{
					this.Decrease();
				}
				else
				{
					base.OnMove(eventData);
				}
				break;
			}
		}

		// Token: 0x060048CD RID: 18637 RVA: 0x001859B4 File Offset: 0x00183DB4
		public override Selectable FindSelectableOnLeft()
		{
			if (base.navigation.mode == Navigation.Mode.Automatic && this.IsHorizontal())
			{
				return null;
			}
			return base.FindSelectableOnLeft();
		}

		// Token: 0x060048CE RID: 18638 RVA: 0x001859F0 File Offset: 0x00183DF0
		public override Selectable FindSelectableOnRight()
		{
			if (base.navigation.mode == Navigation.Mode.Automatic && this.IsHorizontal())
			{
				return null;
			}
			return base.FindSelectableOnRight();
		}

		// Token: 0x060048CF RID: 18639 RVA: 0x00185A2C File Offset: 0x00183E2C
		public override Selectable FindSelectableOnUp()
		{
			if (base.navigation.mode == Navigation.Mode.Automatic && !this.IsHorizontal())
			{
				return null;
			}
			return base.FindSelectableOnUp();
		}

		// Token: 0x060048D0 RID: 18640 RVA: 0x00185A68 File Offset: 0x00183E68
		public override Selectable FindSelectableOnDown()
		{
			if (base.navigation.mode == Navigation.Mode.Automatic && !this.IsHorizontal())
			{
				return null;
			}
			return base.FindSelectableOnDown();
		}

		// Token: 0x04003160 RID: 12640
		public Func<bool> IsHorizontal;

		// Token: 0x04003161 RID: 12641
		public Action Decrease;

		// Token: 0x04003162 RID: 12642
		public Action Increase;

		// Token: 0x04003163 RID: 12643
		public Func<Vector2> PositionLimits;

		// Token: 0x04003164 RID: 12644
		public Action<float> PositionChanged;

		// Token: 0x04003165 RID: 12645
		public Action OnSubmit;

		// Token: 0x04003166 RID: 12646
		private RectTransform rectTransform;

		// Token: 0x04003167 RID: 12647
		[CompilerGenerated]
		private static Func<bool> f__mg0;

		// Token: 0x04003168 RID: 12648
		[CompilerGenerated]
		private static Action f__mg1;

		// Token: 0x04003169 RID: 12649
		[CompilerGenerated]
		private static Action f__mg2;

		// Token: 0x0400316A RID: 12650
		[CompilerGenerated]
		private static Action f__mg3;
	}
}
