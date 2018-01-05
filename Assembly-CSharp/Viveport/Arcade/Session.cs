using System;
using System.Runtime.InteropServices;
using LitJson;
using Viveport.Core;
using Viveport.Internal.Arcade;

namespace Viveport.Arcade
{
	// Token: 0x02000994 RID: 2452
	internal class Session
	{
        public static void IsReady(Session.SessionListener listener)
        {
            Viveport.Internal.Arcade.Session.IsReady(new Session.SessionHandler(listener).getIsReadyHandler());
        }

        public static void Start(Session.SessionListener listener)
        {
            Viveport.Internal.Arcade.Session.Start(new Session.SessionHandler(listener).getStartHandler());
        }

        public static void Stop(Session.SessionListener listener)
        {
            Viveport.Internal.Arcade.Session.Stop(new Session.SessionHandler(listener).getStopHandler());
        }


        // Token: 0x02000995 RID: 2453
        private class SessionHandler : Session.BaseHandler
		{
			// Token: 0x06004A49 RID: 19017 RVA: 0x0018BFEF File Offset: 0x0018A3EF
			public SessionHandler(Session.SessionListener cb)
			{
				Session.SessionHandler.listener = cb;
			}

			// Token: 0x06004A4A RID: 19018 RVA: 0x0018BFFD File Offset: 0x0018A3FD
			public SessionCallback getIsReadyHandler()
			{
				return new SessionCallback(this.IsReadyHandler);
			}

			// Token: 0x06004A4B RID: 19019 RVA: 0x0018C00C File Offset: 0x0018A40C
			protected override void IsReadyHandler(int code, [MarshalAs(UnmanagedType.LPStr)] string message)
			{
				Logger.Log(string.Concat(new object[]
				{
					"[Session IsReadyHandler] message=",
					message,
					",code=",
					code
				}));
				JsonData jsonData = JsonMapper.ToObject(message);
				int num = -1;
				string text = string.Empty;
				string text2 = string.Empty;
				if (code == 0)
				{
					try
					{
						num = (int)jsonData["statusCode"];
						text = (string)jsonData["message"];
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
						text
					}));
					if (num == 0)
					{
						try
						{
							text2 = (string)jsonData["appID"];
						}
						catch (Exception arg2)
						{
							Logger.Log("[IsReadyHandler] appID ex=" + arg2);
						}
						Logger.Log("[IsReadyHandler] appID=" + text2);
					}
				}
				if (Session.SessionHandler.listener != null)
				{
					if (code == 0)
					{
						if (num == 0)
						{
							Session.SessionHandler.listener.OnSuccess(text2);
						}
						else
						{
							Session.SessionHandler.listener.OnFailure(num, text);
						}
					}
					else
					{
						Session.SessionHandler.listener.OnFailure(code, message);
					}
				}
			}

			// Token: 0x06004A4C RID: 19020 RVA: 0x0018C170 File Offset: 0x0018A570
			public SessionCallback getStartHandler()
			{
				return new SessionCallback(this.StartHandler);
			}

			// Token: 0x06004A4D RID: 19021 RVA: 0x0018C180 File Offset: 0x0018A580
			protected override void StartHandler(int code, [MarshalAs(UnmanagedType.LPStr)] string message)
			{
				Logger.Log(string.Concat(new object[]
				{
					"[Session StartHandler] message=",
					message,
					",code=",
					code
				}));
				JsonData jsonData = JsonMapper.ToObject(message);
				int num = -1;
				string text = string.Empty;
				string text2 = string.Empty;
				string text3 = string.Empty;
				if (code == 0)
				{
					try
					{
						num = (int)jsonData["statusCode"];
						text = (string)jsonData["message"];
					}
					catch (Exception arg)
					{
						Logger.Log("[StartHandler] statusCode, message ex=" + arg);
					}
					Logger.Log(string.Concat(new object[]
					{
						"[StartHandler] statusCode =",
						num,
						",errMessage=",
						text
					}));
					if (num == 0)
					{
						try
						{
							text2 = (string)jsonData["appID"];
							text3 = (string)jsonData["Guid"];
						}
						catch (Exception arg2)
						{
							Logger.Log("[StartHandler] appID, Guid ex=" + arg2);
						}
						Logger.Log("[StartHandler] appID=" + text2 + ",Guid=" + text3);
					}
				}
				if (Session.SessionHandler.listener != null)
				{
					if (code == 0)
					{
						if (num == 0)
						{
							Session.SessionHandler.listener.OnStartSuccess(text2, text3);
						}
						else
						{
							Session.SessionHandler.listener.OnFailure(num, text);
						}
					}
					else
					{
						Session.SessionHandler.listener.OnFailure(code, message);
					}
				}
			}

			// Token: 0x06004A4E RID: 19022 RVA: 0x0018C308 File Offset: 0x0018A708
			public SessionCallback getStopHandler()
			{
				return new SessionCallback(this.StopHandler);
			}

			// Token: 0x06004A4F RID: 19023 RVA: 0x0018C318 File Offset: 0x0018A718
			protected override void StopHandler(int code, [MarshalAs(UnmanagedType.LPStr)] string message)
			{
				Logger.Log(string.Concat(new object[]
				{
					"[Session StopHandler] message=",
					message,
					",code=",
					code
				}));
				JsonData jsonData = JsonMapper.ToObject(message);
				int num = -1;
				string text = string.Empty;
				string text2 = string.Empty;
				string text3 = string.Empty;
				if (code == 0)
				{
					try
					{
						num = (int)jsonData["statusCode"];
						text = (string)jsonData["message"];
					}
					catch (Exception arg)
					{
						Logger.Log("[StopHandler] statusCode, message ex=" + arg);
					}
					Logger.Log(string.Concat(new object[]
					{
						"[StopHandler] statusCode =",
						num,
						",errMessage=",
						text
					}));
					if (num == 0)
					{
						try
						{
							text2 = (string)jsonData["appID"];
							text3 = (string)jsonData["Guid"];
						}
						catch (Exception arg2)
						{
							Logger.Log("[StopHandler] appID, Guid ex=" + arg2);
						}
						Logger.Log("[StopHandler] appID=" + text2 + ",Guid=" + text3);
					}
				}
				if (Session.SessionHandler.listener != null)
				{
					if (code == 0)
					{
						if (num == 0)
						{
							Session.SessionHandler.listener.OnStopSuccess(text2, text3);
						}
						else
						{
							Session.SessionHandler.listener.OnFailure(num, text);
						}
					}
					else
					{
						Session.SessionHandler.listener.OnFailure(code, message);
					}
				}
			}

			// Token: 0x04003230 RID: 12848
			private static Session.SessionListener listener;
		}

		// Token: 0x02000996 RID: 2454
		private abstract class BaseHandler
		{
			// Token: 0x06004A51 RID: 19025
			protected abstract void IsReadyHandler(int code, [MarshalAs(UnmanagedType.LPStr)] string message);

			// Token: 0x06004A52 RID: 19026
			protected abstract void StartHandler(int code, [MarshalAs(UnmanagedType.LPStr)] string message);

			// Token: 0x06004A53 RID: 19027
			protected abstract void StopHandler(int code, [MarshalAs(UnmanagedType.LPStr)] string message);
		}

		// Token: 0x02000997 RID: 2455
		public class SessionListener
		{
			// Token: 0x06004A55 RID: 19029 RVA: 0x00189732 File Offset: 0x00187B32
			public virtual void OnSuccess(string pchAppID)
			{
			}

			// Token: 0x06004A56 RID: 19030 RVA: 0x00189734 File Offset: 0x00187B34
			public virtual void OnStartSuccess(string pchAppID, string pchGuid)
			{
			}

			// Token: 0x06004A57 RID: 19031 RVA: 0x00189736 File Offset: 0x00187B36
			public virtual void OnStopSuccess(string pchAppID, string pchGuid)
			{
			}

			// Token: 0x06004A58 RID: 19032 RVA: 0x00189738 File Offset: 0x00187B38
			public virtual void OnFailure(int nCode, string pchMessage)
			{
			}
		}
	}
}
