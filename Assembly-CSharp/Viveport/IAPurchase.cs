using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using LitJson;
using Viveport.Core;
using Viveport.Internal;

namespace Viveport
{
	// Token: 0x0200098A RID: 2442
	public class IAPurchase
	{
		// Token: 0x060049CD RID: 18893 RVA: 0x0018ACFC File Offset: 0x001890FC
		public static void IsReady(IAPurchase.IAPurchaseListener listener, string pchAppKey)
		{
			IAPurchase.IAPHandler iaphandler = new IAPurchase.IAPHandler(listener);
            Viveport.Internal.IAPurchase.IsReady(iaphandler.getIsReadyHandler(), pchAppKey);
		}

		// Token: 0x060049CE RID: 18894 RVA: 0x0018AD1C File Offset: 0x0018911C
		public static void Request(IAPurchase.IAPurchaseListener listener, string pchPrice)
		{
			IAPurchase.IAPHandler iaphandler = new IAPurchase.IAPHandler(listener);
            Viveport.Internal.IAPurchase.Request(iaphandler.getRequestHandler(), pchPrice);
		}

		// Token: 0x060049CF RID: 18895 RVA: 0x0018AD3C File Offset: 0x0018913C
		public static void Purchase(IAPurchase.IAPurchaseListener listener, string pchPurchaseId)
		{
			IAPurchase.IAPHandler iaphandler = new IAPurchase.IAPHandler(listener);
            Viveport.Internal.IAPurchase.Purchase(iaphandler.getPurchaseHandler(), pchPurchaseId);
		}

		// Token: 0x060049D0 RID: 18896 RVA: 0x0018AD5C File Offset: 0x0018915C
		public static void Query(IAPurchase.IAPurchaseListener listener, string pchPurchaseId)
		{
            
			IAPurchase.IAPHandler iaphandler = new IAPurchase.IAPHandler(listener);
            Viveport.Internal.IAPurchase.Query(iaphandler.getQueryHandler(), pchPurchaseId);
		}

		// Token: 0x060049D1 RID: 18897 RVA: 0x0018AD7C File Offset: 0x0018917C
		public static void GetBalance(IAPurchase.IAPurchaseListener listener)
		{
			IAPurchase.IAPHandler iaphandler = new IAPurchase.IAPHandler(listener);
            Viveport.Internal.IAPurchase.GetBalance(iaphandler.getBalanceHandler());
		}

		// Token: 0x060049D2 RID: 18898 RVA: 0x0018AD9C File Offset: 0x0018919C
		public static void RequestSubscription(IAPurchase.IAPurchaseListener listener, string pchPrice, string pchFreeTrialType, int nFreeTrialValue, string pchChargePeriodType, int nChargePeriodValue, int nNumberOfChargePeriod, string pchPlanId)
		{
			IAPurchase.IAPHandler iaphandler = new IAPurchase.IAPHandler(listener);
            Viveport.Internal.IAPurchase.RequestSubscription(iaphandler.getRequestSubscriptionHandler(), pchPrice, pchFreeTrialType, nFreeTrialValue, pchChargePeriodType, nChargePeriodValue, nNumberOfChargePeriod, pchPlanId);
		}

		// Token: 0x060049D3 RID: 18899 RVA: 0x0018ADC8 File Offset: 0x001891C8
		public static void RequestSubscriptionWithPlanID(IAPurchase.IAPurchaseListener listener, string pchPlanId)
		{
			IAPurchase.IAPHandler iaphandler = new IAPurchase.IAPHandler(listener);
            Viveport.Internal.IAPurchase.RequestSubscriptionWithPlanID(iaphandler.getRequestSubscriptionWithPlanIDHandler(), pchPlanId);
		}

		// Token: 0x060049D4 RID: 18900 RVA: 0x0018ADE8 File Offset: 0x001891E8
		public static void Subscribe(IAPurchase.IAPurchaseListener listener, string pchSubscriptionId)
		{
			IAPurchase.IAPHandler iaphandler = new IAPurchase.IAPHandler(listener);
            Viveport.Internal.IAPurchase.Subscribe(iaphandler.getSubscribeHandler(), pchSubscriptionId);
		}

		// Token: 0x060049D5 RID: 18901 RVA: 0x0018AE08 File Offset: 0x00189208
		public static void QuerySubscription(IAPurchase.IAPurchaseListener listener, string pchSubscriptionId)
		{
			IAPurchase.IAPHandler iaphandler = new IAPurchase.IAPHandler(listener);
            Viveport.Internal.IAPurchase.QuerySubscription(iaphandler.getQuerySubscriptionHandler(), pchSubscriptionId);
		}

		// Token: 0x060049D6 RID: 18902 RVA: 0x0018AE28 File Offset: 0x00189228
		public static void CancelSubscription(IAPurchase.IAPurchaseListener listener, string pchSubscriptionId)
		{
			IAPurchase.IAPHandler iaphandler = new IAPurchase.IAPHandler(listener);
            Viveport.Internal.IAPurchase.CancelSubscription(iaphandler.getCancelSubscriptionHandler(), pchSubscriptionId);
		}

		// Token: 0x0200098B RID: 2443
		private class IAPHandler : IAPurchase.BaseHandler
		{
			// Token: 0x060049D7 RID: 18903 RVA: 0x0018AE50 File Offset: 0x00189250
			public IAPHandler(IAPurchase.IAPurchaseListener cb)
			{
				IAPurchase.IAPHandler.listener = cb;
			}

			// Token: 0x060049D8 RID: 18904 RVA: 0x0018AE5E File Offset: 0x0018925E
			public IAPurchaseCallback getIsReadyHandler()
			{
				return new IAPurchaseCallback(this.IsReadyHandler);
			}

			// Token: 0x060049D9 RID: 18905 RVA: 0x0018AE70 File Offset: 0x00189270
			protected override void IsReadyHandler(int code, [MarshalAs(UnmanagedType.LPStr)] string message)
			{
				Logger.Log("[IsReadyHandler] message=" + message);
				JsonData jsonData = JsonMapper.ToObject(message);
				int num = -1;
				string text = string.Empty;
				string text2 = string.Empty;
				if (code == 0)
				{
					try
					{
						num = (int)jsonData["statusCode"];
						text2 = (string)jsonData["message"];
					}
					catch (Exception arg)
					{
						Logger.Log("[IsReadyHandler] statusCode, message ex=" + arg);
					}
					Logger.Log(string.Concat(new object[]
					{
						"[IsReadyHandler] statusCode =",
						num,
						",errMessage=",
						text2
					}));
					if (num == 0)
					{
						try
						{
							text = (string)jsonData["currencyName"];
						}
						catch (Exception arg2)
						{
							Logger.Log("[IsReadyHandler] currencyName ex=" + arg2);
						}
						Logger.Log("[IsReadyHandler] currencyName=" + text);
					}
				}
				if (IAPurchase.IAPHandler.listener != null)
				{
					if (code == 0)
					{
						if (num == 0)
						{
							IAPurchase.IAPHandler.listener.OnSuccess(text);
						}
						else
						{
							IAPurchase.IAPHandler.listener.OnFailure(num, text2);
						}
					}
					else
					{
						IAPurchase.IAPHandler.listener.OnFailure(code, message);
					}
				}
			}

			// Token: 0x060049DA RID: 18906 RVA: 0x0018AFB8 File Offset: 0x001893B8
			public IAPurchaseCallback getRequestHandler()
			{
				return new IAPurchaseCallback(this.RequestHandler);
			}

			// Token: 0x060049DB RID: 18907 RVA: 0x0018AFC8 File Offset: 0x001893C8
			protected override void RequestHandler(int code, [MarshalAs(UnmanagedType.LPStr)] string message)
			{
				Logger.Log("[RequestHandler] message=" + message);
				JsonData jsonData = JsonMapper.ToObject(message);
				int num = -1;
				string text = string.Empty;
				string text2 = string.Empty;
				if (code == 0)
				{
					try
					{
						num = (int)jsonData["statusCode"];
						text2 = (string)jsonData["message"];
					}
					catch (Exception arg)
					{
						Logger.Log("[RequestHandler] statusCode, message ex=" + arg);
					}
					Logger.Log(string.Concat(new object[]
					{
						"[RequestHandler] statusCode =",
						num,
						",errMessage=",
						text2
					}));
					if (num == 0)
					{
						try
						{
							text = (string)jsonData["purchase_id"];
						}
						catch (Exception arg2)
						{
							Logger.Log("[RequestHandler] purchase_id ex=" + arg2);
						}
						Logger.Log("[RequestHandler] purchaseId =" + text);
					}
				}
				if (IAPurchase.IAPHandler.listener != null)
				{
					if (code == 0)
					{
						if (num == 0)
						{
							IAPurchase.IAPHandler.listener.OnRequestSuccess(text);
						}
						else
						{
							IAPurchase.IAPHandler.listener.OnFailure(num, text2);
						}
					}
					else
					{
						IAPurchase.IAPHandler.listener.OnFailure(code, message);
					}
				}
			}

			// Token: 0x060049DC RID: 18908 RVA: 0x0018B110 File Offset: 0x00189510
			public IAPurchaseCallback getPurchaseHandler()
			{
				return new IAPurchaseCallback(this.PurchaseHandler);
			}

			// Token: 0x060049DD RID: 18909 RVA: 0x0018B120 File Offset: 0x00189520
			protected override void PurchaseHandler(int code, [MarshalAs(UnmanagedType.LPStr)] string message)
			{
				Logger.Log("[PurchaseHandler] message=" + message);
				JsonData jsonData = JsonMapper.ToObject(message);
				int num = -1;
				string text = string.Empty;
				string text2 = string.Empty;
				long num2 = 0L;
				if (code == 0)
				{
					try
					{
						num = (int)jsonData["statusCode"];
						text2 = (string)jsonData["message"];
					}
					catch (Exception arg)
					{
						Logger.Log("[PurchaseHandler] statusCode, message ex=" + arg);
					}
					Logger.Log(string.Concat(new object[]
					{
						"[PurchaseHandler] statusCode =",
						num,
						",errMessage=",
						text2
					}));
					if (num == 0)
					{
						try
						{
							text = (string)jsonData["purchase_id"];
							num2 = (long)jsonData["paid_timestamp"];
						}
						catch (Exception arg2)
						{
							Logger.Log("[PurchaseHandler] purchase_id,paid_timestamp ex=" + arg2);
						}
						Logger.Log(string.Concat(new object[]
						{
							"[PurchaseHandler] purchaseId =",
							text,
							",paid_timestamp=",
							num2
						}));
					}
				}
				if (IAPurchase.IAPHandler.listener != null)
				{
					if (code == 0)
					{
						if (num == 0)
						{
							IAPurchase.IAPHandler.listener.OnPurchaseSuccess(text);
						}
						else
						{
							IAPurchase.IAPHandler.listener.OnFailure(num, text2);
						}
					}
					else
					{
						IAPurchase.IAPHandler.listener.OnFailure(code, message);
					}
				}
			}

			// Token: 0x060049DE RID: 18910 RVA: 0x0018B29C File Offset: 0x0018969C
			public IAPurchaseCallback getQueryHandler()
			{
				return new IAPurchaseCallback(this.QueryHandler);
			}

			// Token: 0x060049DF RID: 18911 RVA: 0x0018B2AC File Offset: 0x001896AC
			protected override void QueryHandler(int code, [MarshalAs(UnmanagedType.LPStr)] string message)
			{
				Logger.Log("[QueryHandler] message=" + message);
				JsonData jsonData = JsonMapper.ToObject(message);
				int num = -1;
				string text = string.Empty;
				string text2 = string.Empty;
				string text3 = string.Empty;
				string text4 = string.Empty;
				string text5 = string.Empty;
				string text6 = string.Empty;
				long num2 = 0L;
				if (code == 0)
				{
					try
					{
						num = (int)jsonData["statusCode"];
						text2 = (string)jsonData["message"];
					}
					catch (Exception arg)
					{
						Logger.Log("[QueryHandler] statusCode, message ex=" + arg);
					}
					Logger.Log(string.Concat(new object[]
					{
						"[QueryHandler] statusCode =",
						num,
						",errMessage=",
						text2
					}));
					if (num == 0)
					{
						try
						{
							text = (string)jsonData["purchase_id"];
							text3 = (string)jsonData["order_id"];
							text4 = (string)jsonData["status"];
							text5 = (string)jsonData["price"];
							text6 = (string)jsonData["currency"];
							num2 = (long)jsonData["paid_timestamp"];
						}
						catch (Exception arg2)
						{
							Logger.Log("[QueryHandler] purchase_id, order_id ex=" + arg2);
						}
						Logger.Log(string.Concat(new string[]
						{
							"[QueryHandler] status =",
							text4,
							",price=",
							text5,
							",currency=",
							text6
						}));
						Logger.Log(string.Concat(new object[]
						{
							"[QueryHandler] purchaseId =",
							text,
							",order_id=",
							text3,
							",paid_timestamp=",
							num2
						}));
					}
				}
				if (IAPurchase.IAPHandler.listener != null)
				{
					if (code == 0)
					{
						if (num == 0)
						{
							IAPurchase.QueryResponse queryResponse = new IAPurchase.QueryResponse();
							queryResponse.purchase_id = text;
							queryResponse.order_id = text3;
							queryResponse.price = text5;
							queryResponse.currency = text6;
							queryResponse.paid_timestamp = num2;
							queryResponse.status = text4;
							IAPurchase.IAPHandler.listener.OnQuerySuccess(queryResponse);
						}
						else
						{
							IAPurchase.IAPHandler.listener.OnFailure(num, text2);
						}
					}
					else
					{
						IAPurchase.IAPHandler.listener.OnFailure(code, message);
					}
				}
			}

			// Token: 0x060049E0 RID: 18912 RVA: 0x0018B50C File Offset: 0x0018990C
			public IAPurchaseCallback getBalanceHandler()
			{
				return new IAPurchaseCallback(this.BalanceHandler);
			}

			// Token: 0x060049E1 RID: 18913 RVA: 0x0018B51C File Offset: 0x0018991C
			protected override void BalanceHandler(int code, [MarshalAs(UnmanagedType.LPStr)] string message)
			{
				Logger.Log(string.Concat(new object[]
				{
					"[BalanceHandler] code=",
					code,
					",message= ",
					message
				}));
				JsonData jsonData = JsonMapper.ToObject(message);
				int num = -1;
				string str = string.Empty;
				string text = string.Empty;
				string text2 = string.Empty;
				if (code == 0)
				{
					try
					{
						num = (int)jsonData["statusCode"];
						text2 = (string)jsonData["message"];
					}
					catch (Exception arg)
					{
						Logger.Log("[BalanceHandler] statusCode, message ex=" + arg);
					}
					Logger.Log(string.Concat(new object[]
					{
						"[BalanceHandler] statusCode =",
						num,
						",errMessage=",
						text2
					}));
					if (num == 0)
					{
						try
						{
							str = (string)jsonData["currencyName"];
							text = (string)jsonData["balance"];
						}
						catch (Exception arg2)
						{
							Logger.Log("[BalanceHandler] currencyName, balance ex=" + arg2);
						}
						Logger.Log("[BalanceHandler] currencyName=" + str + ",balance=" + text);
					}
				}
				if (IAPurchase.IAPHandler.listener != null)
				{
					if (code == 0)
					{
						if (num == 0)
						{
							IAPurchase.IAPHandler.listener.OnBalanceSuccess(text);
						}
						else
						{
							IAPurchase.IAPHandler.listener.OnFailure(num, text2);
						}
					}
					else
					{
						IAPurchase.IAPHandler.listener.OnFailure(code, message);
					}
				}
			}

			// Token: 0x060049E2 RID: 18914 RVA: 0x0018B6A4 File Offset: 0x00189AA4
			public IAPurchaseCallback getRequestSubscriptionHandler()
			{
				return new IAPurchaseCallback(this.RequestSubscriptionHandler);
			}

			// Token: 0x060049E3 RID: 18915 RVA: 0x0018B6B4 File Offset: 0x00189AB4
			protected override void RequestSubscriptionHandler(int code, [MarshalAs(UnmanagedType.LPStr)] string message)
			{
				Logger.Log("[RequestSubscriptionHandler] message=" + message);
				JsonData jsonData = JsonMapper.ToObject(message);
				int num = -1;
				string text = string.Empty;
				string text2 = string.Empty;
				try
				{
					num = (int)jsonData["statusCode"];
					text2 = (string)jsonData["message"];
				}
				catch (Exception arg)
				{
					Logger.Log("[RequestSubscriptionHandler] statusCode, message ex=" + arg);
				}
				Logger.Log(string.Concat(new object[]
				{
					"[RequestSubscriptionHandler] statusCode =",
					num,
					",errMessage=",
					text2
				}));
				if (num == 0)
				{
					try
					{
						text = (string)jsonData["subscription_id"];
					}
					catch (Exception arg2)
					{
						Logger.Log("[RequestSubscriptionHandler] subscription_id ex=" + arg2);
					}
					Logger.Log("[RequestSubscriptionHandler] subscription_id =" + text);
				}
				if (IAPurchase.IAPHandler.listener != null)
				{
					if (code == 0)
					{
						if (num == 0)
						{
							IAPurchase.IAPHandler.listener.OnRequestSubscriptionSuccess(text);
						}
						else
						{
							IAPurchase.IAPHandler.listener.OnFailure(num, text2);
						}
					}
					else
					{
						IAPurchase.IAPHandler.listener.OnFailure(code, message);
					}
				}
			}

			// Token: 0x060049E4 RID: 18916 RVA: 0x0018B7F8 File Offset: 0x00189BF8
			public IAPurchaseCallback getRequestSubscriptionWithPlanIDHandler()
			{
				return new IAPurchaseCallback(this.RequestSubscriptionWithPlanIDHandler);
			}

			// Token: 0x060049E5 RID: 18917 RVA: 0x0018B808 File Offset: 0x00189C08
			protected override void RequestSubscriptionWithPlanIDHandler(int code, [MarshalAs(UnmanagedType.LPStr)] string message)
			{
				Logger.Log("[RequestSubscriptionWithPlanIDHandler] message=" + message);
				JsonData jsonData = JsonMapper.ToObject(message);
				int num = -1;
				string text = string.Empty;
				string text2 = string.Empty;
				try
				{
					num = (int)jsonData["statusCode"];
					text2 = (string)jsonData["message"];
				}
				catch (Exception arg)
				{
					Logger.Log("[RequestSubscriptionWithPlanIDHandler] statusCode, message ex=" + arg);
				}
				Logger.Log(string.Concat(new object[]
				{
					"[RequestSubscriptionWithPlanIDHandler] statusCode =",
					num,
					",errMessage=",
					text2
				}));
				if (num == 0)
				{
					try
					{
						text = (string)jsonData["subscription_id"];
					}
					catch (Exception arg2)
					{
						Logger.Log("[RequestSubscriptionWithPlanIDHandler] subscription_id ex=" + arg2);
					}
					Logger.Log("[RequestSubscriptionWithPlanIDHandler] subscription_id =" + text);
				}
				if (IAPurchase.IAPHandler.listener != null)
				{
					if (code == 0)
					{
						if (num == 0)
						{
							IAPurchase.IAPHandler.listener.OnRequestSubscriptionWithPlanIDSuccess(text);
						}
						else
						{
							IAPurchase.IAPHandler.listener.OnFailure(num, text2);
						}
					}
					else
					{
						IAPurchase.IAPHandler.listener.OnFailure(code, message);
					}
				}
			}

			// Token: 0x060049E6 RID: 18918 RVA: 0x0018B94C File Offset: 0x00189D4C
			public IAPurchaseCallback getSubscribeHandler()
			{
				return new IAPurchaseCallback(this.SubscribeHandler);
			}

			// Token: 0x060049E7 RID: 18919 RVA: 0x0018B95C File Offset: 0x00189D5C
			protected override void SubscribeHandler(int code, [MarshalAs(UnmanagedType.LPStr)] string message)
			{
				Logger.Log("[SubscribeHandler] message=" + message);
				JsonData jsonData = JsonMapper.ToObject(message);
				int num = -1;
				string text = string.Empty;
				string text2 = string.Empty;
				string str = string.Empty;
				try
				{
					num = (int)jsonData["statusCode"];
					text2 = (string)jsonData["message"];
				}
				catch (Exception arg)
				{
					Logger.Log("[SubscribeHandler] statusCode, message ex=" + arg);
				}
				Logger.Log(string.Concat(new object[]
				{
					"[SubscribeHandler] statusCode =",
					num,
					",errMessage=",
					text2
				}));
				if (num == 0)
				{
					try
					{
						text = (string)jsonData["subscription_id"];
						str = (string)jsonData["plan_id"];
						long num2 = (long)jsonData["subscribed_timestamp"];
					}
					catch (Exception arg2)
					{
						Logger.Log("[SubscribeHandler] subscription_id, plan_id ex=" + arg2);
					}
					Logger.Log("[SubscribeHandler] subscription_id =" + text + ",plan_id=" + str);
				}
				if (IAPurchase.IAPHandler.listener != null)
				{
					if (code == 0)
					{
						if (num == 0)
						{
							IAPurchase.IAPHandler.listener.OnSubscribeSuccess(text);
						}
						else
						{
							IAPurchase.IAPHandler.listener.OnFailure(num, text2);
						}
					}
					else
					{
						IAPurchase.IAPHandler.listener.OnFailure(code, message);
					}
				}
			}

			// Token: 0x060049E8 RID: 18920 RVA: 0x0018BAD4 File Offset: 0x00189ED4
			public IAPurchaseCallback getQuerySubscriptionHandler()
			{
				return new IAPurchaseCallback(this.QuerySubscriptionHandler);
			}

			// Token: 0x060049E9 RID: 18921 RVA: 0x0018BAE4 File Offset: 0x00189EE4
			protected override void QuerySubscriptionHandler(int code, [MarshalAs(UnmanagedType.LPStr)] string message)
			{
				Logger.Log("[QuerySubscriptionHandler] message=" + message);
				JsonData jsonData = JsonMapper.ToObject(message);
				int num = -1;
				string text = string.Empty;
				List<IAPurchase.Subscription> list = null;
				if (code == 0)
				{
					try
					{
						num = (int)jsonData["statusCode"];
						text = (string)jsonData["message"];
					}
					catch (Exception arg)
					{
						Logger.Log("[QuerySubscriptionHandler] statusCode, message ex=" + arg);
					}
					Logger.Log(string.Concat(new object[]
					{
						"[QuerySubscriptionHandler] statusCode =",
						num,
						",errMessage=",
						text
					}));
					if (num == 0)
					{
						try
						{
							IAPurchase.QuerySubscritionResponse querySubscritionResponse = JsonMapper.ToObject<IAPurchase.QuerySubscritionResponse>(message);
							list = querySubscritionResponse.subscriptions;
						}
						catch (Exception arg2)
						{
							Logger.Log("[QuerySubscriptionHandler] ex =" + arg2);
						}
					}
				}
				if (IAPurchase.IAPHandler.listener != null)
				{
					if (code == 0)
					{
						if (num == 0 && list != null && list.Count > 0)
						{
							IAPurchase.IAPHandler.listener.OnQuerySubscriptionSuccess(list.ToArray());
						}
						else
						{
							IAPurchase.IAPHandler.listener.OnFailure(num, text);
						}
					}
					else
					{
						IAPurchase.IAPHandler.listener.OnFailure(code, message);
					}
				}
			}

			// Token: 0x060049EA RID: 18922 RVA: 0x0018BC30 File Offset: 0x0018A030
			public IAPurchaseCallback getCancelSubscriptionHandler()
			{
				return new IAPurchaseCallback(this.CancelSubscriptionHandler);
			}

			// Token: 0x060049EB RID: 18923 RVA: 0x0018BC40 File Offset: 0x0018A040
			protected override void CancelSubscriptionHandler(int code, [MarshalAs(UnmanagedType.LPStr)] string message)
			{
				Logger.Log("[CancelSubscriptionHandler] message=" + message);
				JsonData jsonData = JsonMapper.ToObject(message);
				int num = -1;
				bool flag = false;
				string text = string.Empty;
				if (code == 0)
				{
					try
					{
						num = (int)jsonData["statusCode"];
						text = (string)jsonData["message"];
					}
					catch (Exception arg)
					{
						Logger.Log("[CancelSubscriptionHandler] statusCode, message ex=" + arg);
					}
					Logger.Log(string.Concat(new object[]
					{
						"[CancelSubscriptionHandler] statusCode =",
						num,
						",errMessage=",
						text
					}));
					if (num == 0)
					{
						flag = true;
						Logger.Log("[CancelSubscriptionHandler] isCanceled = " + flag);
					}
				}
				if (IAPurchase.IAPHandler.listener != null)
				{
					if (code == 0)
					{
						if (num == 0)
						{
							IAPurchase.IAPHandler.listener.OnCancelSubscriptionSuccess(flag);
						}
						else
						{
							IAPurchase.IAPHandler.listener.OnFailure(num, text);
						}
					}
					else
					{
						IAPurchase.IAPHandler.listener.OnFailure(code, message);
					}
				}
			}

			// Token: 0x04003211 RID: 12817
			private static IAPurchase.IAPurchaseListener listener;
		}

		// Token: 0x0200098C RID: 2444
		private abstract class BaseHandler
		{
			// Token: 0x060049ED RID: 18925
			protected abstract void IsReadyHandler(int code, [MarshalAs(UnmanagedType.LPStr)] string message);

			// Token: 0x060049EE RID: 18926
			protected abstract void RequestHandler(int code, [MarshalAs(UnmanagedType.LPStr)] string message);

			// Token: 0x060049EF RID: 18927
			protected abstract void PurchaseHandler(int code, [MarshalAs(UnmanagedType.LPStr)] string message);

			// Token: 0x060049F0 RID: 18928
			protected abstract void QueryHandler(int code, [MarshalAs(UnmanagedType.LPStr)] string message);

			// Token: 0x060049F1 RID: 18929
			protected abstract void BalanceHandler(int code, [MarshalAs(UnmanagedType.LPStr)] string message);

			// Token: 0x060049F2 RID: 18930
			protected abstract void RequestSubscriptionHandler(int code, [MarshalAs(UnmanagedType.LPStr)] string message);

			// Token: 0x060049F3 RID: 18931
			protected abstract void RequestSubscriptionWithPlanIDHandler(int code, [MarshalAs(UnmanagedType.LPStr)] string message);

			// Token: 0x060049F4 RID: 18932
			protected abstract void SubscribeHandler(int code, [MarshalAs(UnmanagedType.LPStr)] string message);

			// Token: 0x060049F5 RID: 18933
			protected abstract void QuerySubscriptionHandler(int code, [MarshalAs(UnmanagedType.LPStr)] string message);

			// Token: 0x060049F6 RID: 18934
			protected abstract void CancelSubscriptionHandler(int code, [MarshalAs(UnmanagedType.LPStr)] string message);
		}

		// Token: 0x0200098D RID: 2445
		public class IAPurchaseListener
		{
			// Token: 0x060049F8 RID: 18936 RVA: 0x00189CF2 File Offset: 0x001880F2
			public virtual void OnSuccess(string pchCurrencyName)
			{
			}

			// Token: 0x060049F9 RID: 18937 RVA: 0x00189CF4 File Offset: 0x001880F4
			public virtual void OnRequestSuccess(string pchPurchaseId)
			{
			}

			// Token: 0x060049FA RID: 18938 RVA: 0x00189CF6 File Offset: 0x001880F6
			public virtual void OnPurchaseSuccess(string pchPurchaseId)
			{
			}

			// Token: 0x060049FB RID: 18939 RVA: 0x00189CF8 File Offset: 0x001880F8
			public virtual void OnQuerySuccess(IAPurchase.QueryResponse response)
			{
			}

			// Token: 0x060049FC RID: 18940 RVA: 0x00189CFA File Offset: 0x001880FA
			public virtual void OnBalanceSuccess(string pchBalance)
			{
			}

			// Token: 0x060049FD RID: 18941 RVA: 0x00189CFC File Offset: 0x001880FC
			public virtual void OnFailure(int nCode, string pchMessage)
			{
			}

			// Token: 0x060049FE RID: 18942 RVA: 0x00189CFE File Offset: 0x001880FE
			public virtual void OnRequestSubscriptionSuccess(string pchSubscriptionId)
			{
			}

			// Token: 0x060049FF RID: 18943 RVA: 0x00189D00 File Offset: 0x00188100
			public virtual void OnRequestSubscriptionWithPlanIDSuccess(string pchSubscriptionId)
			{
			}

			// Token: 0x06004A00 RID: 18944 RVA: 0x00189D02 File Offset: 0x00188102
			public virtual void OnSubscribeSuccess(string pchSubscriptionId)
			{
			}

			// Token: 0x06004A01 RID: 18945 RVA: 0x00189D04 File Offset: 0x00188104
			public virtual void OnQuerySubscriptionSuccess(IAPurchase.Subscription[] subscriptionlist)
			{
			}

			// Token: 0x06004A02 RID: 18946 RVA: 0x00189D06 File Offset: 0x00188106
			public virtual void OnCancelSubscriptionSuccess(bool bCanceled)
			{
			}
		}

		// Token: 0x0200098E RID: 2446
		public class QueryResponse
		{
			// Token: 0x17000B04 RID: 2820
			// (get) Token: 0x06004A04 RID: 18948 RVA: 0x0018BD58 File Offset: 0x0018A158
			// (set) Token: 0x06004A05 RID: 18949 RVA: 0x0018BD60 File Offset: 0x0018A160
			public string order_id { get; set; }

			// Token: 0x17000B05 RID: 2821
			// (get) Token: 0x06004A06 RID: 18950 RVA: 0x0018BD69 File Offset: 0x0018A169
			// (set) Token: 0x06004A07 RID: 18951 RVA: 0x0018BD71 File Offset: 0x0018A171
			public string purchase_id { get; set; }

			// Token: 0x17000B06 RID: 2822
			// (get) Token: 0x06004A08 RID: 18952 RVA: 0x0018BD7A File Offset: 0x0018A17A
			// (set) Token: 0x06004A09 RID: 18953 RVA: 0x0018BD82 File Offset: 0x0018A182
			public string status { get; set; }

			// Token: 0x17000B07 RID: 2823
			// (get) Token: 0x06004A0A RID: 18954 RVA: 0x0018BD8B File Offset: 0x0018A18B
			// (set) Token: 0x06004A0B RID: 18955 RVA: 0x0018BD93 File Offset: 0x0018A193
			public string price { get; set; }

			// Token: 0x17000B08 RID: 2824
			// (get) Token: 0x06004A0C RID: 18956 RVA: 0x0018BD9C File Offset: 0x0018A19C
			// (set) Token: 0x06004A0D RID: 18957 RVA: 0x0018BDA4 File Offset: 0x0018A1A4
			public string currency { get; set; }

			// Token: 0x17000B09 RID: 2825
			// (get) Token: 0x06004A0E RID: 18958 RVA: 0x0018BDAD File Offset: 0x0018A1AD
			// (set) Token: 0x06004A0F RID: 18959 RVA: 0x0018BDB5 File Offset: 0x0018A1B5
			public long paid_timestamp { get; set; }
		}

		// Token: 0x0200098F RID: 2447
		public class StatusDetailTransaction
		{
			// Token: 0x17000B0A RID: 2826
			// (get) Token: 0x06004A11 RID: 18961 RVA: 0x0018BDC6 File Offset: 0x0018A1C6
			// (set) Token: 0x06004A12 RID: 18962 RVA: 0x0018BDCE File Offset: 0x0018A1CE
			public long create_time { get; set; }

			// Token: 0x17000B0B RID: 2827
			// (get) Token: 0x06004A13 RID: 18963 RVA: 0x0018BDD7 File Offset: 0x0018A1D7
			// (set) Token: 0x06004A14 RID: 18964 RVA: 0x0018BDDF File Offset: 0x0018A1DF
			public string payment_method { get; set; }

			// Token: 0x17000B0C RID: 2828
			// (get) Token: 0x06004A15 RID: 18965 RVA: 0x0018BDE8 File Offset: 0x0018A1E8
			// (set) Token: 0x06004A16 RID: 18966 RVA: 0x0018BDF0 File Offset: 0x0018A1F0
			public string status { get; set; }
		}

		// Token: 0x02000990 RID: 2448
		public class StatusDetail
		{
			// Token: 0x17000B0D RID: 2829
			// (get) Token: 0x06004A18 RID: 18968 RVA: 0x0018BE01 File Offset: 0x0018A201
			// (set) Token: 0x06004A19 RID: 18969 RVA: 0x0018BE09 File Offset: 0x0018A209
			public long date_next_charge { get; set; }

			// Token: 0x17000B0E RID: 2830
			// (get) Token: 0x06004A1A RID: 18970 RVA: 0x0018BE12 File Offset: 0x0018A212
			// (set) Token: 0x06004A1B RID: 18971 RVA: 0x0018BE1A File Offset: 0x0018A21A
			public IAPurchase.StatusDetailTransaction[] transactions { get; set; }

			// Token: 0x17000B0F RID: 2831
			// (get) Token: 0x06004A1C RID: 18972 RVA: 0x0018BE23 File Offset: 0x0018A223
			// (set) Token: 0x06004A1D RID: 18973 RVA: 0x0018BE2B File Offset: 0x0018A22B
			public string cancel_reason { get; set; }
		}

		// Token: 0x02000991 RID: 2449
		public class TimePeriod
		{
			// Token: 0x17000B10 RID: 2832
			// (get) Token: 0x06004A1F RID: 18975 RVA: 0x0018BE3C File Offset: 0x0018A23C
			// (set) Token: 0x06004A20 RID: 18976 RVA: 0x0018BE44 File Offset: 0x0018A244
			public string time_type { get; set; }

			// Token: 0x17000B11 RID: 2833
			// (get) Token: 0x06004A21 RID: 18977 RVA: 0x0018BE4D File Offset: 0x0018A24D
			// (set) Token: 0x06004A22 RID: 18978 RVA: 0x0018BE55 File Offset: 0x0018A255
			public int value { get; set; }
		}

		// Token: 0x02000992 RID: 2450
		public class Subscription
		{
			// Token: 0x17000B12 RID: 2834
			// (get) Token: 0x06004A24 RID: 18980 RVA: 0x0018BE66 File Offset: 0x0018A266
			// (set) Token: 0x06004A25 RID: 18981 RVA: 0x0018BE6E File Offset: 0x0018A26E
			public string app_id { get; set; }

			// Token: 0x17000B13 RID: 2835
			// (get) Token: 0x06004A26 RID: 18982 RVA: 0x0018BE77 File Offset: 0x0018A277
			// (set) Token: 0x06004A27 RID: 18983 RVA: 0x0018BE7F File Offset: 0x0018A27F
			public string order_id { get; set; }

			// Token: 0x17000B14 RID: 2836
			// (get) Token: 0x06004A28 RID: 18984 RVA: 0x0018BE88 File Offset: 0x0018A288
			// (set) Token: 0x06004A29 RID: 18985 RVA: 0x0018BE90 File Offset: 0x0018A290
			public string subscription_id { get; set; }

			// Token: 0x17000B15 RID: 2837
			// (get) Token: 0x06004A2A RID: 18986 RVA: 0x0018BE99 File Offset: 0x0018A299
			// (set) Token: 0x06004A2B RID: 18987 RVA: 0x0018BEA1 File Offset: 0x0018A2A1
			public string price { get; set; }

			// Token: 0x17000B16 RID: 2838
			// (get) Token: 0x06004A2C RID: 18988 RVA: 0x0018BEAA File Offset: 0x0018A2AA
			// (set) Token: 0x06004A2D RID: 18989 RVA: 0x0018BEB2 File Offset: 0x0018A2B2
			public string currency { get; set; }

			// Token: 0x17000B17 RID: 2839
			// (get) Token: 0x06004A2E RID: 18990 RVA: 0x0018BEBB File Offset: 0x0018A2BB
			// (set) Token: 0x06004A2F RID: 18991 RVA: 0x0018BEC3 File Offset: 0x0018A2C3
			public long subscribed_timestamp { get; set; }

			// Token: 0x17000B18 RID: 2840
			// (get) Token: 0x06004A30 RID: 18992 RVA: 0x0018BECC File Offset: 0x0018A2CC
			// (set) Token: 0x06004A31 RID: 18993 RVA: 0x0018BED4 File Offset: 0x0018A2D4
			public IAPurchase.TimePeriod free_trial_period { get; set; }

			// Token: 0x17000B19 RID: 2841
			// (get) Token: 0x06004A32 RID: 18994 RVA: 0x0018BEDD File Offset: 0x0018A2DD
			// (set) Token: 0x06004A33 RID: 18995 RVA: 0x0018BEE5 File Offset: 0x0018A2E5
			public IAPurchase.TimePeriod charge_period { get; set; }

			// Token: 0x17000B1A RID: 2842
			// (get) Token: 0x06004A34 RID: 18996 RVA: 0x0018BEEE File Offset: 0x0018A2EE
			// (set) Token: 0x06004A35 RID: 18997 RVA: 0x0018BEF6 File Offset: 0x0018A2F6
			public int number_of_charge_period { get; set; }

			// Token: 0x17000B1B RID: 2843
			// (get) Token: 0x06004A36 RID: 18998 RVA: 0x0018BEFF File Offset: 0x0018A2FF
			// (set) Token: 0x06004A37 RID: 18999 RVA: 0x0018BF07 File Offset: 0x0018A307
			public string plan_id { get; set; }

			// Token: 0x17000B1C RID: 2844
			// (get) Token: 0x06004A38 RID: 19000 RVA: 0x0018BF10 File Offset: 0x0018A310
			// (set) Token: 0x06004A39 RID: 19001 RVA: 0x0018BF18 File Offset: 0x0018A318
			public string plan_name { get; set; }

			// Token: 0x17000B1D RID: 2845
			// (get) Token: 0x06004A3A RID: 19002 RVA: 0x0018BF21 File Offset: 0x0018A321
			// (set) Token: 0x06004A3B RID: 19003 RVA: 0x0018BF29 File Offset: 0x0018A329
			public string status { get; set; }

			// Token: 0x17000B1E RID: 2846
			// (get) Token: 0x06004A3C RID: 19004 RVA: 0x0018BF32 File Offset: 0x0018A332
			// (set) Token: 0x06004A3D RID: 19005 RVA: 0x0018BF3A File Offset: 0x0018A33A
			public IAPurchase.StatusDetail status_detail { get; set; }
		}

		// Token: 0x02000993 RID: 2451
		public class QuerySubscritionResponse
		{
			// Token: 0x17000B1F RID: 2847
			// (get) Token: 0x06004A3F RID: 19007 RVA: 0x0018BF4B File Offset: 0x0018A34B
			// (set) Token: 0x06004A40 RID: 19008 RVA: 0x0018BF53 File Offset: 0x0018A353
			public int statusCode { get; set; }

			// Token: 0x17000B20 RID: 2848
			// (get) Token: 0x06004A41 RID: 19009 RVA: 0x0018BF5C File Offset: 0x0018A35C
			// (set) Token: 0x06004A42 RID: 19010 RVA: 0x0018BF64 File Offset: 0x0018A364
			public string message { get; set; }

			// Token: 0x17000B21 RID: 2849
			// (get) Token: 0x06004A43 RID: 19011 RVA: 0x0018BF6D File Offset: 0x0018A36D
			// (set) Token: 0x06004A44 RID: 19012 RVA: 0x0018BF75 File Offset: 0x0018A375
			public List<IAPurchase.Subscription> subscriptions { get; set; }
		}
	}
}
