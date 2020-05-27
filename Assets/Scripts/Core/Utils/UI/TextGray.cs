using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TextGray : UIGray {
	private Text txt;
	private Color TextColor;


	public override void SetUIGray(bool IsGray, float greypow = 1.0f) {

		Init();

		if (null != txt) {
			if (IsGray == true) {
				txt.color = Color.gray;
			} else {
				txt.color = TextColor;
			}
		}
	}

	private void Init(){
		if (m_IsInit == false) {
			txt = gameObject.GetComponent<Text>();
			if (null != txt) {
				TextColor = txt.color;
			}
			m_IsInit = true;
		}
	}
}
