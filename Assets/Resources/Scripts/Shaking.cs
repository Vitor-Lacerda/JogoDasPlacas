using UnityEngine;
using System.Collections;

public class Shaking : MonoBehaviour {

	public float _frequency;
	public float _amplitude;
	public Vector2 _multipliers;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		Vector2 temp = transform.localPosition;
		temp.x = _multipliers.x*_amplitude*Mathf.Sin (Mathf.PI*2*Time.time*_frequency);
		temp.y = _multipliers.y*_amplitude*Mathf.Sin (Mathf.PI*2*Time.time*_frequency + Mathf.PI/2);

		transform.localPosition = temp;
	}
}
