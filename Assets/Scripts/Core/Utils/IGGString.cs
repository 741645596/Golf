using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text;

public class IGGString  {

	private static StringBuilder g_Builder = null;
	public  static StringBuilder strBuilder{
		get { return g_Builder;}
	}
	// 初始化。
	public static void Init(){
		g_Builder = new StringBuilder(string.Empty);
	}

	// 拼接字符串
	public static string Strcat(string str1, string str2)
	{
		return string.Concat(str1, str2);
	}


	// 拼接字符串
	public static string Strcat(string str1, string str2, string str3)
	{
		return string.Concat(str1, str2, str3);
	}
		
	// 拼接字符串
	public static string Strcat(string str1, string str2, string str3, string str4)
	{
		return string.Concat(str1, str2, str3, str4);
	}
}
