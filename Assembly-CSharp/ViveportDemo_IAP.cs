using System;
using System.Runtime.CompilerServices;
using UnityEngine;
using Viveport;
using Viveport.Core;

// Token: 0x02000977 RID: 2423
public class ViveportDemo_IAP : MonoBehaviour
{
	// Token: 0x06004976 RID: 18806 RVA: 0x001897E8 File Offset: 0x00187BE8
	private void Start()
	{
		this.mListener = new ViveportDemo_IAP.Result();
		if (ViveportDemo_IAP.f__mg0 == null)
		{
			ViveportDemo_IAP.f__mg0 = new StatusCallback(ViveportDemo_IAP.InitStatusHandler);
		}
		Api.Init(ViveportDemo_IAP.f__mg0, ViveportDemo_IAP.IAP_APP_TEST_ID);
		Viveport.Core.Logger.Log("Version: " + Api.Version());
		Viveport.Core.Logger.Log("UserId: " + User.GetUserId());
	}

	// Token: 0x06004977 RID: 18807 RVA: 0x00189850 File Offset: 0x00187C50
	private void Update()
	{
	}

	// Token: 0x06004978 RID: 18808 RVA: 0x00189854 File Offset: 0x00187C54
	private void OnGUI()
	{
		if (GUI.Button(new Rect((float)this.nXStart, (float)this.nYStart, (float)this.nWidth, (float)this.nHeight), "IsReady"))
		{
			Viveport.Core.Logger.Log("IsReady");
			IAPurchase.IsReady(this.mListener, ViveportDemo_IAP.IAP_APP_TEST_KEY);
		}
		if (GUI.Button(new Rect((float)this.nXStart, (float)(this.nYStart + this.nWidth + 10), (float)this.nWidth, (float)this.nHeight), "Request"))
		{
			Viveport.Core.Logger.Log("Request");
			this.mListener.mItem.items = new string[3];
			this.mListener.mItem.items[0] = "sword";
			this.mListener.mItem.items[1] = "knife";
			this.mListener.mItem.items[2] = "medicine";
			IAPurchase.Request(this.mListener, "1");
		}
		if (GUI.Button(new Rect((float)this.nXStart, (float)(this.nYStart + 2 * this.nWidth + 20), (float)this.nWidth, (float)this.nHeight), "Purchase"))
		{
			Viveport.Core.Logger.Log("Purchase mListener.mItem.ticket=" + this.mListener.mItem.ticket);
			IAPurchase.Purchase(this.mListener, this.mListener.mItem.ticket);
		}
		if (GUI.Button(new Rect((float)this.nXStart, (float)(this.nYStart + 3 * this.nWidth + 30), (float)this.nWidth, (float)this.nHeight), "Query"))
		{
			Viveport.Core.Logger.Log("Query");
			IAPurchase.Query(this.mListener, this.mListener.mItem.ticket);
		}
		if (GUI.Button(new Rect((float)this.nXStart, (float)(this.nYStart + 4 * this.nWidth + 40), (float)this.nWidth, (float)this.nHeight), "GetBalance"))
		{
			Viveport.Core.Logger.Log("GetBalance");
			IAPurchase.GetBalance(this.mListener);
		}
		if (GUI.Button(new Rect((float)(this.nXStart + this.nWidth + 10), (float)(this.nYStart + this.nWidth + 10), (float)(this.nWidth + 70), (float)this.nHeight), "RequestSubscription"))
		{
			Viveport.Core.Logger.Log("RequestSubscription");
			IAPurchase.RequestSubscription(this.mListener, "1", "month", 1, "day", 2, 3, "pID");
		}
		if (GUI.Button(new Rect((float)(this.nXStart + this.nWidth + 10), (float)(this.nYStart + 2 * this.nWidth + 20), (float)(this.nWidth + 120), (float)this.nHeight), "RequestSubscriptionWithPlanID"))
		{
			Viveport.Core.Logger.Log("RequestSubscriptionWithPlanID");
			IAPurchase.RequestSubscriptionWithPlanID(this.mListener, "pID");
		}
		if (GUI.Button(new Rect((float)(this.nXStart + this.nWidth + 10), (float)(this.nYStart + 3 * this.nWidth + 30), (float)this.nWidth, (float)this.nHeight), "Subscribe"))
		{
			Viveport.Core.Logger.Log("Subscribe");
			IAPurchase.Subscribe(this.mListener, this.mListener.mItem.subscription_ticket);
		}
		if (GUI.Button(new Rect((float)(this.nXStart + this.nWidth + 10), (float)(this.nYStart + 4 * this.nWidth + 40), (float)(this.nWidth + 50), (float)this.nHeight), "QuerySubscription"))
		{
			Viveport.Core.Logger.Log("QuerySubscription");
			IAPurchase.QuerySubscription(this.mListener, this.mListener.mItem.subscription_ticket);
		}
		if (GUI.Button(new Rect((float)(this.nXStart + this.nWidth + 10), (float)(this.nYStart + 5 * this.nWidth + 50), (float)(this.nWidth + 50), (float)this.nHeight), "CancelSubscription"))
		{
			Viveport.Core.Logger.Log("CancelSubscription");
			IAPurchase.CancelSubscription(this.mListener, this.mListener.mItem.subscription_ticket);
		}
	}

	// Token: 0x06004979 RID: 18809 RVA: 0x00189C9F File Offset: 0x0018809F
	private static void InitStatusHandler(int nResult)
	{
		Viveport.Core.Logger.Log("InitStatusHandler: " + nResult);
	}

	// Token: 0x040031D8 RID: 12760
	private int nWidth = 80;

	// Token: 0x040031D9 RID: 12761
	private int nHeight = 40;

	// Token: 0x040031DA RID: 12762
	private int nXStart = 10;

	// Token: 0x040031DB RID: 12763
	private int nYStart = 35;

	// Token: 0x040031DC RID: 12764
	private static string IAP_APP_TEST_ID = "app_test_id";

	// Token: 0x040031DD RID: 12765
	private static string IAP_APP_TEST_KEY = "app_test_key";

	// Token: 0x040031DE RID: 12766
	private ViveportDemo_IAP.Result mListener;

	// Token: 0x040031DF RID: 12767
	private static bool bIsDuplicatedSubscription;

	// Token: 0x040031E0 RID: 12768
	[CompilerGenerated]
	private static StatusCallback f__mg0;

	// Token: 0x02000978 RID: 2424
	public class Item
	{
		// Token: 0x040031E1 RID: 12769
		public string ticket = "test_id";

		// Token: 0x040031E2 RID: 12770
		public string[] items;

		// Token: 0x040031E3 RID: 12771
		public string subscription_ticket = "unity_test_subscriptionId";
	}

	// Token: 0x02000979 RID: 2425
	private class Result : IAPurchase.IAPurchaseListener
	{
		// Token: 0x0600497D RID: 18813 RVA: 0x00189D1B File Offset: 0x0018811B
		public override void OnSuccess(string pchCurrencyName)
		{
			Viveport.Core.Logger.Log("[OnSuccess] pchCurrencyName=" + pchCurrencyName);
		}

		// Token: 0x0600497E RID: 18814 RVA: 0x00189D2D File Offset: 0x0018812D
		public override void OnRequestSuccess(string pchPurchaseId)
		{
			this.mItem.ticket = pchPurchaseId;
			Viveport.Core.Logger.Log("[OnRequestSuccess] pchPurchaseId=" + pchPurchaseId + ",mItem.ticket=" + this.mItem.ticket);
		}

		// Token: 0x0600497F RID: 18815 RVA: 0x00189D5B File Offset: 0x0018815B
		public override void OnPurchaseSuccess(string pchPurchaseId)
		{
			Viveport.Core.Logger.Log("[OnPurchaseSuccess] pchPurchaseId=" + pchPurchaseId);
			if (this.mItem.ticket == pchPurchaseId)
			{
				Viveport.Core.Logger.Log("[OnPurchaseSuccess] give items to user");
			}
		}

		// Token: 0x06004980 RID: 18816 RVA: 0x00189D8D File Offset: 0x0018818D
		public override void OnQuerySuccess(IAPurchase.QueryResponse response)
		{
			Viveport.Core.Logger.Log("[OnQuerySuccess] purchaseId=" + response.purchase_id + ",status=" + response.status);
		}

		// Token: 0x06004981 RID: 18817 RVA: 0x00189DAF File Offset: 0x001881AF
		public override void OnBalanceSuccess(string pchBalance)
		{
			Viveport.Core.Logger.Log("[OnBalanceSuccess] pchBalance=" + pchBalance);
		}

		// Token: 0x06004982 RID: 18818 RVA: 0x00189DC1 File Offset: 0x001881C1
		public override void OnRequestSubscriptionSuccess(string pchSubscriptionId)
		{
			this.mItem.subscription_ticket = pchSubscriptionId;
			Viveport.Core.Logger.Log("[OnRequestSubscriptionSuccess] pchSubscriptionId=" + pchSubscriptionId + ",mItem.subscription_ticket=" + this.mItem.subscription_ticket);
		}

		// Token: 0x06004983 RID: 18819 RVA: 0x00189DEF File Offset: 0x001881EF
		public override void OnRequestSubscriptionWithPlanIDSuccess(string pchSubscriptionId)
		{
			this.mItem.subscription_ticket = pchSubscriptionId;
			Viveport.Core.Logger.Log("[OnRequestSubscriptionWithPlanIDSuccess] pchSubscriptionId=" + pchSubscriptionId + ",mItem.subscription_ticket=" + this.mItem.subscription_ticket);
		}

		// Token: 0x06004984 RID: 18820 RVA: 0x00189E1D File Offset: 0x0018821D
		public override void OnSubscribeSuccess(string pchSubscriptionId)
		{
			Viveport.Core.Logger.Log("[OnSubscribeSuccess] pchSubscriptionId=" + pchSubscriptionId);
			if (this.mItem.subscription_ticket == pchSubscriptionId)
			{
				Viveport.Core.Logger.Log("[OnSubscribeSuccess] give virtual items to user");
			}
		}

		// Token: 0x06004985 RID: 18821 RVA: 0x00189E50 File Offset: 0x00188250
		public override void OnQuerySubscriptionSuccess(IAPurchase.Subscription[] subscriptionlist)
		{
			int num = subscriptionlist.Length;
			Viveport.Core.Logger.Log("[OnQuerySubscriptionSuccess] subscriptionlist size =" + num);
			if (num > 0)
			{
				for (int i = 0; i < num; i++)
				{
					Viveport.Core.Logger.Log(string.Concat(new object[]
					{
						"[OnQuerySubscriptionSuccess] subscriptionlist[",
						i,
						"].status =",
						subscriptionlist[i].status,
						", subscriptionlist[",
						i,
						"].plan_id = ",
						subscriptionlist[i].plan_id
					}));
					if (subscriptionlist[i].plan_id == "pID" && subscriptionlist[i].status == "ACTIVE")
					{
						ViveportDemo_IAP.bIsDuplicatedSubscription = true;
					}
				}
			}
		}

		// Token: 0x06004986 RID: 18822 RVA: 0x00189F1B File Offset: 0x0018831B
		public override void OnCancelSubscriptionSuccess(bool bCanceled)
		{
			Viveport.Core.Logger.Log("[OnCancelSubscriptionSuccess] bCanceled=" + bCanceled);
		}

		// Token: 0x06004987 RID: 18823 RVA: 0x00189F32 File Offset: 0x00188332
		public override void OnFailure(int nCode, string pchMessage)
		{
			Viveport.Core.Logger.Log(string.Concat(new object[]
			{
				"[OnFailed] ",
				nCode,
				", ",
				pchMessage
			}));
		}

		// Token: 0x040031E4 RID: 12772
		public ViveportDemo_IAP.Item mItem = new ViveportDemo_IAP.Item();
	}
}
