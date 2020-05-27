using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemGray : MonoBehaviour {

	public List<UIGray> listGray = new List<UIGray>();
	// 设置变灰
	public void SetGray(bool IsGray) {
		foreach (UIGray v in listGray) {
			if (v != null) {
				v.SetUIGray(IsGray);
			}
		}
	}

	public void Init(string Restype, string ResName){
		foreach (UIGray v in listGray) {
			if (v != null) {
				v.Init(Restype, ResName);
			}
		}
	}
}
