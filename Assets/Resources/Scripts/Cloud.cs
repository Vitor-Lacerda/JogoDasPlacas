using UnityEngine;
using System.Collections;

public class Cloud : MonoBehaviour {

	public Transform[] _spawners;
	public Vector2 _yRange;
	public Vector2 _scaleRange;
	public Vector2 _speedRange;
	public Vector2 _delayRange;

	float _moveSpeed;
	float _delayTime;
	float _time;


	void Start () {
		ResetPosition ();
	}
	

	void Update () {

		if (Time.time - _time <= _delayTime) {
			return;
		}

		transform.position += Vector3.right * _moveSpeed * Time.deltaTime;
		Vector3 vpPos = Camera.main.WorldToViewportPoint (transform.position);
		if (vpPos.x <= -1f || vpPos.x >= 2f) {
			ResetPosition ();
		}
	}

	void ResetPosition(){
		Transform chosenSpawn = _spawners [Random.Range (0, _spawners.Length)];	
		transform.position = new Vector2 (chosenSpawn.position.x, Random.Range (_yRange.x, _yRange.y));
		int direction = chosenSpawn.position.x < 0 ? 1 : -1;
		_moveSpeed = Random.Range (_speedRange.x, _speedRange.y)*direction;
		transform.localScale = new Vector3 (Random.Range (_scaleRange.x, _scaleRange.y), Random.Range (_scaleRange.x, _scaleRange.y),1);
		_delayTime = Random.Range (_delayRange.x, _delayRange.y);
		_time = Time.time;
	}
}
