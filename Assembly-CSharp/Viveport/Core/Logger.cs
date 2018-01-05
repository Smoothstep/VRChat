using System;
using System.Reflection;

namespace Viveport.Core
{
	// Token: 0x0200097B RID: 2427
	public class Logger
	{
		// Token: 0x0600498E RID: 18830 RVA: 0x0018A24D File Offset: 0x0018864D
		public static void Log(string message)
		{
			if (!Logger._hasDetected || Logger._usingUnityLog)
			{
				Logger.UnityLog(message);
			}
			else
			{
				Logger.ConsoleLog(message);
			}
		}

		// Token: 0x0600498F RID: 18831 RVA: 0x0018A274 File Offset: 0x00188674
		private static void ConsoleLog(string message)
		{
			Console.WriteLine(message);
			Logger._hasDetected = true;
		}

		// Token: 0x06004990 RID: 18832 RVA: 0x0018A284 File Offset: 0x00188684
		private static void UnityLog(string message)
		{
			try
			{
				if (Logger._unityLogType == null)
				{
					Logger._unityLogType = Logger.GetType("UnityEngine.Debug");
				}
				MethodInfo method = Logger._unityLogType.GetMethod("Log", new Type[]
				{
					typeof(string)
				});
				method.Invoke(null, new object[]
				{
					message
				});
				Logger._usingUnityLog = true;
			}
			catch (Exception)
			{
				Logger.ConsoleLog(message);
				Logger._usingUnityLog = false;
			}
			Logger._hasDetected = true;
		}

		// Token: 0x06004991 RID: 18833 RVA: 0x0018A314 File Offset: 0x00188714
		private static Type GetType(string typeName)
		{
			Type type = Type.GetType(typeName);
			if (type != null)
			{
				return type;
			}
			foreach (Assembly assembly in AppDomain.CurrentDomain.GetAssemblies())
			{
				type = assembly.GetType(typeName);
				if (type != null)
				{
					return type;
				}
			}
			return null;
		}

		// Token: 0x040031E6 RID: 12774
		private const string LoggerTypeNameUnity = "UnityEngine.Debug";

		// Token: 0x040031E7 RID: 12775
		private static bool _hasDetected;

		// Token: 0x040031E8 RID: 12776
		private static bool _usingUnityLog = true;

		// Token: 0x040031E9 RID: 12777
		private static Type _unityLogType;
	}
}
