using System;
using System.Collections;
using System.Collections.Generic;
using IGG.Core;

// <summary>
// 优化enum 作为字典key 造成的gc alloc 
// 用于定义enum的几个几个结构接口
// </summary>
// <author>zhulin</author>





public struct msgtypeMsgTypeComparer: IEqualityComparer<eMsgTypes>{
	public bool Equals(eMsgTypes a, eMsgTypes b){
		return a == b;
	}

	public int GetHashCode(eMsgTypes obj){
		return (int)obj;
	}
}




public struct longComparer : IEqualityComparer<long> {
	public bool Equals(long a, long b) {
		return a == b;
	}

	public int GetHashCode(long obj) {
		return (int)obj;
	}
}

