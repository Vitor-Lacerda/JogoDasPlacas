using UnityEngine;
using System.Collections;

public class NPCCar : CarController {

	public States _startState = States.DEFAULT;
	public SpriteRenderer _colorSprite;
	public bool _randomColor = true;
	protected override void Awake(){
		base.Awake ();
		_isPlayer = false;
		_currentState = _startState;
		if (_randomColor) {
			_colorSprite.color = Random.ColorHSV ();
		}


	}

	/*
	protected override void WaitStopTime(){
		_stopTime += Time.deltaTime;
		if (_stopTime >= _stopWaitTime) {
			ChangeState (States.DEFAULT);
			_stop = false;
		}
	}
	*/


	protected override void OnTriggerEnter2D(Collider2D other){

		base.OnTriggerEnter2D (other);
		if (other.CompareTag ("Exit")) {
			Destroy (this.gameObject);
		}



		/*
		if (other.CompareTag ("Exit")) {
			Destroy (this.gameObject);
		}

		if (other.CompareTag ("Stop")) {
			if (_currentState == States.STOP) {
				_stop = true;
				_stopTime = 0;
			}
		}

		if (other.CompareTag ("Curva")) {
			if (_currentState == States.RIGHT && !_turn) {
				_turn = true;
				_targetZ = transform.localEulerAngles.z -90;
				_spriteRenderer.sprite = _sprites [1];
			}
			if (_currentState == States.LEFT && !_turn) {
				_turn = true;
				_targetZ = transform.localEulerAngles.z + 90;
				_spriteRenderer.sprite = _sprites [2];

			}
		}

		if (other.CompareTag ("Checker")) {
			if (_turn) {
				transform.localEulerAngles = new Vector3 (0, 0, _targetZ);
				_turn = false;
				ChangeState (States.DEFAULT);

			}
		}
		*/
	}



}
