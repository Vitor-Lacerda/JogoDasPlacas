using UnityEngine;
using System.Collections;

public class AccidentCollider : MonoBehaviour {

	public CarController parent;

	void OnTriggerEnter2D(Collider2D other){
		if (other.CompareTag ("Carro")) {
			if (other.transform.parent.GetComponent<CarController> ()._currentState != CarController.States.STOP) {
				parent.Accident ();
				other.transform.parent.GetComponent<CarController> ()._move = false;
			}
		}
	}
}
