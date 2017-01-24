using UnityEngine;
using UnityEditor;
using System.Collections;

public class ExtrasMenu : MonoBehaviour {

	[MenuItem ("PlayerPrefs/Clear")]
	static void ClearPlayerPrefs(){
		PlayerPrefs.DeleteAll ();
	}

	[MenuItem("PlayerPrefs/See")]
	static void SeePlayerPrefs(){
		Debug.Log(PlayerPrefs.GetInt(Configs.PLAYERPREFSKEY));
	}
}
