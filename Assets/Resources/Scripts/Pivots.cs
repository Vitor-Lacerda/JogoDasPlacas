using UnityEngine;
using System.Collections;

public class Pivots : MonoBehaviour {

	public GameObject _followTarget;
	public bool follow;

	// Use this for initialization
	void Start () {
		follow = true;
	}
	
	// Update is called once per frame
	void Update () {
		if (follow) {
			transform.position = _followTarget.transform.position;
			transform.localRotation = _followTarget.transform.localRotation;
		}
	}

}
