using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

[Serializable]
public class ABResource  {
    public string ResourceVer;
	public List<ABResourceUnit> listABUnit = new List<ABResourceUnit>();

	public void Clear(){
		foreach (ABResourceUnit Info in listABUnit) {
			Info.Clear ();
		}
		listABUnit.Clear();
	}

	public void AddResourceUnit(ABResourceUnit Unit){
		if (Unit != null) {
			listABUnit.Add(Unit);
		}
	}

	public static string GetResourceFileName(){
		return "ABResource.json";
	}
}

[Serializable]
public class ABResourceUnit  {

	public string ABdir ;
	public List<ABFileUnitInfo> listAb = new List<ABFileUnitInfo>();
	public void Clear(){
		listAb.Clear ();
	}

	public void AddAbfile(ABFileUnitInfo abfile){
		listAb.Add(abfile);
	}
}

[Serializable]
public class ABFileUnitInfo{
	public string ABfileName = "";

	public ABFileUnitInfo(){
	}

	public ABFileUnitInfo(string ABfileName){
		this.ABfileName = ABfileName;
	}

}