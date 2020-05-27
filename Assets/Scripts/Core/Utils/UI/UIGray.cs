using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIGray : MonoBehaviour {

	public string m_AtlasName = "";
	protected bool m_IsInit = false;
	protected string m_matKey = "";


	public void Init(string Restype, string ResName){
		m_matKey = ShareMaterialM.GetKey(Restype, ResName, m_AtlasName);
	}

	public virtual void SetUIGray(bool IsGray, float greypow = 1.0f) {

	}
}
