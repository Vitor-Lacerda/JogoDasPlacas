using UnityEngine;
using System.Collections; 

public class TesteCod : MonoBehaviour {

	

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public static void VerifyValues(float valA,float valB){

		Debug.LogWarning("Testando valores");

		if(valA > valB){

			Debug.LogWarning("Valor anormal");

			UserService uService = GameObject.Find("_Login").GetComponent<UserService>();

		}

	}
}
