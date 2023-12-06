using UnityEngine;
using Object = UnityEngine.Object;

namespace SF
{
	public static class LoggingSystem 
	{
		public static bool DoLogging = true;

		[System.Diagnostics.Conditional("UNITY_EDITOR")]
		public static void LogMessage(string message, Object context, LogType logType = LogType.Log)
		{
#if UNITY_EDITOR
			if(!DoLogging) return;

			Debug.Log(message, context);
#endif
		}
	}
}
