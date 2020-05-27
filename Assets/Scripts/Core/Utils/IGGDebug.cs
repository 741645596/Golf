using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class IGGDebug {
	// Conditional attribute ensures the method is only called on DEBUG mode.
	//[Conditional("IGG_DEBUG")]
	//[Conditional("IGG_RELEASE")]
	public static void Fail(string p_msg ) {
		Assert(false, p_msg, null);
	}

	//[Conditional("IGG_DEBUG")]
	//[Conditional("IGG_RELEASE")]
	/*public static void MethodNotImplemented() {
		var callerMethod = new StackFrame(1).GetMethod();
		string callerMethodName = callerMethod.DeclaringType + "." + callerMethod.Name;
		Fail(callerMethodName + "() was called but is not fully implemented yet.");
	}*/

	// Conditional attribute ensures the method is only called on DEBUG mode.
	// Conditional removes the call from the code. This way, the Assert expression
	// is not evaluated
	//[Conditional("IGG_DEBUG")]
	//[Conditional("IGG_RELEASE")]
	public static void Assert(bool p_exp, string p_msg, UnityEngine.Object pContext) {
		#if UNITY_EDITOR
		if (p_exp) {
			return;
		}

		UnityEngine.Debug.LogError(p_msg, pContext);
		#endif
	}


	// Conditional attribute ensures the method is only called on DEBUG mode.
	// Conditional removes the call from the code. This way, the Warn expression
	// is not evaluated
	//[Conditional("IGG_DEBUG")]
	//[Conditional("IGG_RELEASE")]
	public static void Warn(bool p_exp, string p_msg) {
		#if UNITY_EDITOR
		if (p_exp) {
			return;
		}

		UnityEngine.Debug.LogWarning(p_msg);
		#endif
	}

	//[Conditional("IGG_DEBUG")]
	public static void Log(object pMessage) {
		UnityEngine.Debug.Log(pMessage);
	}

	//[Conditional("IGG_DEBUG")]
	public static void Log(object pMessage, UnityEngine.Object pContext) {
		UnityEngine.Debug.Log(pMessage, pContext);
	}

	//[Conditional("IGG_DEBUG")]
	//[Conditional("IGG_RELEASE")]
	public static void LogWarning(object pMessage) {
		UnityEngine.Debug.LogWarning(pMessage);
	}

	//[Conditional("IGG_DEBUG")]
	//[Conditional("IGG_RELEASE")]
	public static void LogWarning(object pMessage, UnityEngine.Object pContext) {
		UnityEngine.Debug.LogWarning(pMessage, pContext);
	}

	//[Conditional("IGG_DEBUG")]
	//[Conditional("IGG_RELEASE")]
	public static void LogError(object pMessage) {
		UnityEngine.Debug.LogError(pMessage);
	}

	//[Conditional("IGG_DEBUG")]
	//[Conditional("IGG_RELEASE")]
	public static void LogError(object pMessage, UnityEngine.Object pContext) {
		UnityEngine.Debug.LogError(pMessage, pContext);
	}

	//[Conditional("IGG_DEBUG")]
	//[Conditional("IGG_RELEASE")]
	public static void LogException(Exception pException) {
		UnityEngine.Debug.LogException(pException);
	}

	//[Conditional("IGG_DEBUG")]
	//[Conditional("IGG_RELEASE")]
	public static void LogException(Exception pException, UnityEngine.Object pContext) {
		UnityEngine.Debug.LogException(pException, pContext);
	}
}
