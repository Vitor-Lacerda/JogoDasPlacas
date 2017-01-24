using UnityEngine;
using System.Collections;

public class NPCCar : MonoBehaviour {

	public enum States
	{
		STOP = 0,
		RIGHT,
		LEFT,
		DEFAULT
	}

	public Transform _initialPos;
	public float _initialSpeed = 1;
	public float _moveSpeed{ get; set;}
	public float _maxSpeed = 6;
	public float _speedIncrement = 0.1f;
	public float _turnSpeed = 2f;
	public float _leftTurnSpeed = 0.8f;
	public float _rightTurnSpeed = 2f;
	public float _stopWaitTime = 4;

	//[SerializeField]
	//private Transform _rightPivot;
	//[SerializeField]
	//private Transform _leftPivot;

	//[SerializeField]
	//private Pivots _pivots;

	public ParticleSystem _rightSignal;
	public ParticleSystem _leftSignal;
	public SpriteRenderer _turnSpriteRenderer;

	public ParticleSystem _accidentParticle;

	public Sprite[] _wheelSprites;

	public States _currentState { get; set; }
	public Lanes _currentLane{ get; protected set;}
	public bool _move{ get; set; }

	[SerializeField]
	private GameController _gameController;


	protected bool _stop = false;
	protected bool _turn = false;
	protected float _stopTime = 0;
	protected bool _isPlayer;


	public States _startState = States.DEFAULT;
	public SpriteRenderer _colorSprite;
	public bool _randomColor = true;

	protected void Awake(){
		_move = true;
		_currentState = _startState;
		if (_randomColor) {
			_colorSprite.color = Random.ColorHSV ();
		}
	}

	public void Reset(){
		if (_initialPos != null) {
			transform.position = _initialPos.position;
			transform.localRotation = _initialPos.localRotation;
		}
		_rightSignal.Stop ();
		_leftSignal.Stop ();
		_turn = false;
		_stop = false;
		_moveSpeed = _initialSpeed;
	}

	void Update(){
		if (!_move) {
			return;
		}

		switch (_currentState) {

		case States.STOP:
			if (_stop) {
				WaitStopTime ();
			} else {
				MoveForward (_moveSpeed);
			}
			break;

		case States.RIGHT:

			if (_turn) {
				MoveForward (_turnSpeed);
			} else {
				MoveForward (_moveSpeed);
			}



			break;

		case States.LEFT:

			if (_turn) {
				MoveForward (_turnSpeed);
			}
			else {
				MoveForward (_moveSpeed);
			}

			break;

		default:
			MoveForward (_moveSpeed);
			break;
		}

	}

	protected void MoveForward(float speed){
		transform.position += transform.up * speed * Time.deltaTime;
	}



	protected void WaitStopTime(){
		_stopTime += Time.deltaTime;
		if (_stopTime >= _stopWaitTime) {
			ChangeState (States.DEFAULT);
			_stop = false;
		}
	}

	public void ChangeState(States state){
		_currentState = state;
		if (_currentState == States.RIGHT) {
			_rightSignal.Play ();
			//_spriteRenderer.sprite = _sprites [1];

		} else if (_currentState == States.LEFT) {
			_leftSignal.Play ();
			//_spriteRenderer.sprite = _sprites [2];

		} else {
			_rightSignal.Stop ();
			_leftSignal.Stop ();
			_turnSpriteRenderer.sprite = _wheelSprites [0];

		}

	}

	IEnumerator Turn(int direction, float speed, float targetZ){
		float accumulator = 0;
		while (Mathf.Abs (transform.localEulerAngles.z - targetZ) > speed && accumulator < 90f) {
			accumulator += Mathf.Abs (direction * speed);
			transform.localEulerAngles += new Vector3 (0, 0, direction * speed);
			yield return null;
		}

		transform.localEulerAngles = new Vector3 (0, 0, targetZ);
		_turn = false;
		ChangeState (States.DEFAULT);
	}


	protected void OnTriggerEnter2D(Collider2D other){

		if (other.CompareTag ("Stop")) {
			if (_currentState == States.STOP) {
				_stop = true;
				_stopTime = 0;
			}
				
		}

		if (other.CompareTag ("Curva")) {
			if (_currentState == States.RIGHT && !_turn) {
				_turn = true;
				_turnSpriteRenderer.sprite = _wheelSprites [1];
				StartCoroutine(Turn(-1, _rightTurnSpeed, transform.localEulerAngles.z - 90));
			}
			if (_currentState == States.LEFT && !_turn) {
				_turn = true;
				_turnSpriteRenderer.sprite = _wheelSprites [2];
				StartCoroutine(Turn(1, _leftTurnSpeed, transform.localEulerAngles.z + 90));
			}
		}


		if (other.CompareTag ("Exit")) {
			//Destroy (this.gameObject);
		}


	}



}
