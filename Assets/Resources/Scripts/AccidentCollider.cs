﻿using UnityEngine;
using System.Collections;

public class AccidentCollider : MonoBehaviour {

	public CarController parent;

	void OnTriggerEnter2D(Collider2D other){
		if (other.CompareTag ("Carro")) {
			parent.Accident ();
			other.transform.parent.GetComponent<NPCCar> ()._move = false;
		}
	}
}
