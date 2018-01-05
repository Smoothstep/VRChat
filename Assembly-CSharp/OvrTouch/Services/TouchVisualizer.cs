using System;
using OvrTouch.Controllers;
using OvrTouch.Hands;
using UnityEngine;

namespace OvrTouch.Services
{
	// Token: 0x02000727 RID: 1831
	public class TouchVisualizer : MonoBehaviour
	{
		// Token: 0x06003B8B RID: 15243 RVA: 0x0012B885 File Offset: 0x00129C85
		private void Awake()
		{
			this.ModeChange(this.m_displayMode);
		}

		// Token: 0x06003B8C RID: 15244 RVA: 0x0012B894 File Offset: 0x00129C94
		private void LateUpdate()
		{
			if (!(this.m_hand != null) || !(this.m_hand.TrackedController != null))
			{
				return;
			}
			TouchVisualizer.DisplayMode displayMode = this.m_displayMode;
			bool buttonJoystick = this.m_hand.TrackedController.ButtonJoystick;
			if (buttonJoystick && !this.m_wasButtonDown)
			{
				displayMode = (DisplayMode)((int)(this.m_displayMode + 1) % (int)TouchVisualizer.DisplayMode.Count);
			}
			this.m_wasButtonDown = buttonJoystick;
			if (this.m_displayMode != displayMode)
			{
				this.ModeChange(displayMode);
			}
		}

		// Token: 0x06003B8D RID: 15245 RVA: 0x0012B91C File Offset: 0x00129D1C
		private void ModeChange(TouchVisualizer.DisplayMode nextDisplayMode)
		{
			this.m_controller.SetVisible(false);
			this.m_hand.SetVisible(false);
			if (nextDisplayMode != TouchVisualizer.DisplayMode.Hand)
			{
				if (nextDisplayMode != TouchVisualizer.DisplayMode.Controller)
				{
					if (nextDisplayMode == TouchVisualizer.DisplayMode.HandAndController)
					{
						this.m_hand.SetVisible(true);
						this.m_controller.SetVisible(true);
					}
				}
				else
				{
					this.m_controller.SetVisible(true);
				}
			}
			else
			{
				this.m_hand.SetVisible(true);
			}
			this.m_displayMode = nextDisplayMode;
		}

		// Token: 0x0400244D RID: 9293
		[SerializeField]
		private TouchVisualizer.DisplayMode m_displayMode = TouchVisualizer.DisplayMode.Controller;

		// Token: 0x0400244E RID: 9294
		[SerializeField]
		private Hand m_hand;

		// Token: 0x0400244F RID: 9295
		[SerializeField]
		private TouchController m_controller;

		// Token: 0x04002450 RID: 9296
		private bool m_wasButtonDown;

		// Token: 0x02000728 RID: 1832
		private enum DisplayMode
		{
			// Token: 0x04002452 RID: 9298
			Hand,
			// Token: 0x04002453 RID: 9299
			Controller,
			// Token: 0x04002454 RID: 9300
			HandAndController,
			// Token: 0x04002455 RID: 9301
			Count
		}
	}
}
