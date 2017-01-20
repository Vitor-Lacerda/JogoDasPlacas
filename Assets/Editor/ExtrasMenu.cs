using UnityEngine;
using UnityEditor;
using System.Collections;

public class ExtrasMenu : MonoBehaviour {

	[MenuItem ("PlayerPrefs/Clear")]
	static void ClearPlayerPrefs(){
		PlayerPrefs.DeleteAll ();
	}
}
