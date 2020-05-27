using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ImageGray : UIGray {
	private Image img;
	private Material GrayMat = null;
	private Material MatImg = null;

	[Range(0,10)]
	public float GreyPow = 1.2f;

	void OnValidate() {
		if(null != GrayMat){
			GrayMat.SetFloat("_GreyPow", GreyPow);
		}
	}



	public override void SetUIGray(bool IsGray, float greypow = 1.0f) {

		Init();
		GreyPow = greypow;
		GrayMat.SetFloat("_GreyPow", GreyPow);


		if (null != img) {
			if (IsGray == true) {
				img.material = GrayMat;
				img.material.hideFlags = HideFlags.HideAndDontSave;
				img.material.color = Color.white;
			} else {
				img.material = MatImg;
			}
		}
	}

	private void Init(){
		if (m_IsInit == false) 
		{
			if (GrayMat == null) 
			{
				if (m_matKey == "") 
				{
					Shader s = ResourceManger.LoadShader("UI/UIGrayScale");
					if (s!= null) 
					{
						GrayMat = new Material(s);
					}			
				} 
				else 
				{
					GrayMat = ShareMaterialM.GetMaterial(m_matKey);
					if (GrayMat == null) 
					{
						Shader s = ResourceManger.LoadShader("UI/UIGrayScale");
						if (s!= null) 
						{
							GrayMat = new Material(s);
						}
						ShareMaterialM.AddMaterial(m_matKey, GrayMat);
					}
				}
			}
				
			img = gameObject.GetComponent<Image>();
			if (null != img) {
				MatImg = img.material == null ? null : img.material;
			}
			m_IsInit = true;
		}
	}
}
