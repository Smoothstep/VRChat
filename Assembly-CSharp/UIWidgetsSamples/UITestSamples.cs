using System;
using System.Collections;
using System.Runtime.CompilerServices;
using UIWidgets;
using UnityEngine;

namespace UIWidgetsSamples
{
	// Token: 0x02000920 RID: 2336
	public class UITestSamples : MonoBehaviour
	{
		// Token: 0x0600461C RID: 17948 RVA: 0x0017D2BC File Offset: 0x0017B6BC
		public void ShowNotifySticky()
		{
			Notify.Template("NotifyTemplateSimple").Show("Sticky Notification. Click on the × above to close.", new float?(0f), null, null, null, null);
		}

		// Token: 0x0600461D RID: 17949 RVA: 0x0017D2F4 File Offset: 0x0017B6F4
		public void ShowNotifyAutohide()
		{
			Notify.Template("NotifyTemplateAutoHide").Show("Achievement unlocked. Hide after 3 seconds.", new float?(3f), null, null, null, null);
		}

		// Token: 0x0600461E RID: 17950 RVA: 0x0017D32B File Offset: 0x0017B72B
		private bool CallShowNotifyAutohide()
		{
			this.ShowNotifyAutohide();
			return true;
		}

		// Token: 0x0600461F RID: 17951 RVA: 0x0017D334 File Offset: 0x0017B734
		public void ShowNotifyAutohideRotate()
		{
			Notify notify = Notify.Template("NotifyTemplateAutoHide");
			string message = "Achievement unlocked. Hide after 4 seconds.";
			float? customHideDelay = new float?(4f);
			if (UITestSamples.f__mg0 == null)
			{
				UITestSamples.f__mg0 = new Func<Notify, IEnumerator>(Notify.AnimationRotate);
			}
			Func<Notify, IEnumerator> hideAnimation = UITestSamples.f__mg0;
			notify.Show(message, customHideDelay, null, null, hideAnimation, null);
		}

		// Token: 0x06004620 RID: 17952 RVA: 0x0017D390 File Offset: 0x0017B790
		public void ShowNotifyBlack()
		{
			Notify notify = Notify.Template("NotifyTemplateBlack");
			string message = "Another Notification. Hide after 5 seconds or click on the × above to close.";
			float? customHideDelay = new float?(5f);
			if (UITestSamples.f__mg1 == null)
			{
				UITestSamples.f__mg1 = new Func<Notify, IEnumerator>(Notify.AnimationCollapse);
			}
			Func<Notify, IEnumerator> hideAnimation = UITestSamples.f__mg1;
			notify.Show(message, customHideDelay, null, null, hideAnimation, new bool?(false));
		}

		// Token: 0x06004621 RID: 17953 RVA: 0x0017D3E8 File Offset: 0x0017B7E8
		private bool ShowNotifyYes()
		{
			Notify.Template("NotifyTemplateAutoHide").Show("Action on 'Yes' button click.", new float?(3f), null, null, null, null);
			return true;
		}

		// Token: 0x06004622 RID: 17954 RVA: 0x0017D420 File Offset: 0x0017B820
		private bool ShowNotifyNo()
		{
			Notify.Template("NotifyTemplateAutoHide").Show("Action on 'No' button click.", new float?(3f), null, null, null, null);
			return true;
		}

		// Token: 0x06004623 RID: 17955 RVA: 0x0017D458 File Offset: 0x0017B858
		public void ShowDialogSimple()
		{
			Dialog dialog = Dialog.Template("DialogTemplateSample");
			string title = "Simple Dialog";
			string message = "Simple dialog with only close button.";
			DialogActions dialogActions = new DialogActions();
			DialogActions dialogActions2 = dialogActions;
			string key = "Close";
			if (UITestSamples.f__mg2 == null)
			{
				UITestSamples.f__mg2 = new Func<bool>(Dialog.Close);
			}
			dialogActions2.Add(key, UITestSamples.f__mg2);
			dialog.Show(title, message, dialogActions, "Close", null, null, false, null, null, null);
		}

		// Token: 0x06004624 RID: 17956 RVA: 0x0017D4C8 File Offset: 0x0017B8C8
		private bool CallShowDialogSimple()
		{
			this.ShowDialogSimple();
			return true;
		}

		// Token: 0x06004625 RID: 17957 RVA: 0x0017D4D4 File Offset: 0x0017B8D4
		public void ShowDialogYesNoCancel()
		{
			Dialog dialog = Dialog.Template("DialogTemplateSample");
			string title = "Dialog Yes No Cancel";
			string message = "Question?";
			DialogActions dialogActions = new DialogActions();
			dialogActions.Add("Yes", new Func<bool>(this.ShowNotifyYes));
			dialogActions.Add("No", new Func<bool>(this.ShowNotifyNo));
			DialogActions dialogActions2 = dialogActions;
			string key = "Cancel";
			if (UITestSamples.f__mg3 == null)
			{
				UITestSamples.f__mg3 = new Func<bool>(Dialog.Close);
			}
			dialogActions2.Add(key, UITestSamples.f__mg3);
			dialogActions = dialogActions;
			string focusButton = "Yes";
			Sprite icon = this.questionIcon;
			dialog.Show(title, message, dialogActions, focusButton, null, icon, false, null, null, null);
		}

		// Token: 0x06004626 RID: 17958 RVA: 0x0017D588 File Offset: 0x0017B988
		public void ShowDialogExtended()
		{
			Dialog dialog = Dialog.Template("DialogTemplateSample");
			string title = "Another Dialog";
			string message = "Same template with another position and long text.\nChange\nheight\nto\nfit\ntext.";
			DialogActions dialogActions = new DialogActions();
			dialogActions.Add("Show notification", new Func<bool>(this.CallShowNotifyAutohide));
			dialogActions.Add("Open simple dialog", new Func<bool>(this.CallShowDialogSimple));
			DialogActions dialogActions2 = dialogActions;
			string key = "Close";
			if (UITestSamples.f__mg4 == null)
			{
				UITestSamples.f__mg4 = new Func<bool>(Dialog.Close);
			}
			dialogActions2.Add(key, UITestSamples.f__mg4);
			dialog.Show(title, message, dialogActions, "Show notification", new Vector3?(new Vector3(40f, -40f, 0f)), null, false, null, null, null);
		}

		// Token: 0x06004627 RID: 17959 RVA: 0x0017D638 File Offset: 0x0017BA38
		public void ShowDialogModal()
		{
			Dialog dialog = Dialog.Template("DialogTemplateSample");
			string title = "Modal Dialog";
			string message = "Simple Modal Dialog.";
			DialogActions dialogActions = new DialogActions();
			DialogActions dialogActions2 = dialogActions;
			string key = "Close";
			if (UITestSamples.f__mg5 == null)
			{
				UITestSamples.f__mg5 = new Func<bool>(Dialog.Close);
			}
			dialogActions2.Add(key, UITestSamples.f__mg5);
			dialogActions = dialogActions;
			string focusButton = "Close";
			Color? modalColor = new Color?(new Color(0f, 0f, 0f, 0.8f));
			dialog.Show(title, message, dialogActions, focusButton, null, null, true, null, modalColor, null);
		}

		// Token: 0x06004628 RID: 17960 RVA: 0x0017D6CC File Offset: 0x0017BACC
		public void ShowDialogSignIn()
		{
			Dialog dialog = Dialog.Template("DialogSignInTemplateSample");
			DialogInputHelper helper = dialog.GetComponent<DialogInputHelper>();
			helper.Refresh();
			Dialog dialog2 = dialog;
			string title = "Sign into your Account";
			DialogActions dialogActions = new DialogActions();
			dialogActions.Add("Sign in", () => this.SignInNotify(helper));
			DialogActions dialogActions2 = dialogActions;
			string key = "Cancel";
			if (UITestSamples.f__mg6 == null)
			{
				UITestSamples.f__mg6 = new Func<bool>(Dialog.Close);
			}
			dialogActions2.Add(key, UITestSamples.f__mg6);
			dialogActions = dialogActions;
			Color? modalColor = new Color?(new Color(0f, 0f, 0f, 0.8f));
			dialog2.Show(title, null, dialogActions, "Sign in", null, null, true, null, modalColor, null);
		}

		// Token: 0x06004629 RID: 17961 RVA: 0x0017D794 File Offset: 0x0017BB94
		private bool SignInNotify(DialogInputHelper helper)
		{
			if (!helper.Validate())
			{
				return false;
			}
			string message = "Sign in.\nUsername: " + helper.Username.text + "\nPassword: <hidden>";
			Notify.Template("NotifyTemplateAutoHide").Show(message, new float?(3f), null, null, null, null);
			return true;
		}

		// Token: 0x04003012 RID: 12306
		[SerializeField]
		private Sprite questionIcon;

		// Token: 0x04003013 RID: 12307
		[SerializeField]
		private Sprite attentionIcon;

		// Token: 0x04003014 RID: 12308
		[CompilerGenerated]
		private static Func<Notify, IEnumerator> f__mg0;

		// Token: 0x04003015 RID: 12309
		[CompilerGenerated]
		private static Func<Notify, IEnumerator> f__mg1;

		// Token: 0x04003016 RID: 12310
		[CompilerGenerated]
		private static Func<bool> f__mg2;

		// Token: 0x04003017 RID: 12311
		[CompilerGenerated]
		private static Func<bool> f__mg3;

		// Token: 0x04003018 RID: 12312
		[CompilerGenerated]
		private static Func<bool> f__mg4;

		// Token: 0x04003019 RID: 12313
		[CompilerGenerated]
		private static Func<bool> f__mg5;

		// Token: 0x0400301A RID: 12314
		[CompilerGenerated]
		private static Func<bool> f__mg6;
	}
}
