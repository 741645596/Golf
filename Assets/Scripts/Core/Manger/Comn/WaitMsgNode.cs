using UnityEngine;
using System.Collections;


namespace IGG.Core
{
	public delegate void taskFunc();
	public struct WaitMsgNode
	{
		private taskFunc success;
		private taskFunc fail;
		private float timeout;

		public WaitMsgNode(float timeout, taskFunc success, taskFunc fail)
		{
			this.timeout = timeout + Time.time;
			this.success = success;
			this.fail    = fail;
		}

		public bool IsExpired()
		{
			return timeout <= Time.time;
		}

		public void OnSuccess()
		{
			if (success != null)
				success();
		}

		public void OnFail()
		{
			if (fail != null)
				fail();
		}
	}
	
}

