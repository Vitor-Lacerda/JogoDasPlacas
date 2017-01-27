using UnityEngine;
using System.Collections;

public class FaixaPedestre : MonoBehaviour {

	public Animator _person;
	public float _personSpeed = 2;
	public Transform[] _positions;
	public GameObject _collider;

	public bool _crossStreet{ get; set;}
	Vector3 _direction;

	public bool Initialize(){
		int r = Random.Range (0, 11);
		_person.transform.localPosition = _positions [0].localPosition;
		_person.SetBool ("Move", false);
		_person.gameObject.SetActive (r % 2 == 0);
		_collider.SetActive (r % 2 == 0);
		_crossStreet = false;
		return _person.gameObject.activeSelf;
	}

	public void CrossTheStreet(){
		if (_person.gameObject.activeSelf) {
			_direction = _positions [1].localPosition - _person.transform.localPosition; 
			_crossStreet = true;
			_person.SetBool ("Move", true);
			//StartCoroutine (MovePerson ());
		}
	}

	void Update(){

		if (!_crossStreet) {
			return;
		}

		_person.transform.localPosition += _direction.normalized*Time.deltaTime*_personSpeed;

		if(_person.transform.localPosition.x < _positions[1].localPosition.x){
			_collider.SetActive (false);
			//_person.gameObject.SetActive (false);
		}


	}

	/*
	IEnumerator MovePerson(){

		Vector3 direction = _positions [1].localPosition - _person.transform.localPosition;

		while(Mathf.Abs(_person.transform.localPosition.x - _positions[1].localPosition.x) > 0.1f){
			//_person.transform.position = Vector2.Lerp ((Vector2)_person.transform.position, (Vector2)_positions [1].position, Time.deltaTime * _personSpeed);
			_person.transform.localPosition += direction.normalized*Time.deltaTime*_personSpeed;
			yield return null;
		}

		_person.gameObject.SetActive (false);
		_collider.SetActive (false);
		yield return null;
	}
	*/
}
