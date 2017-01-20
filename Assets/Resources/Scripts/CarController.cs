using UnityEngine;
using System.Collections;

public class CarController : MonoBehaviour {


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



	// Use this for initialization
	protected virtual void Awake () {
		_isPlayer = true;
		Reset ();
		_move = true;

	}
	
	// Update is called once per frame
	void Update () {
		if (!_move)
			return;
		//GetInput ();

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
		//Debug.Log (_turn);
	}

	void GetInput(){
		if (Input.GetKeyDown (KeyCode.S)) {
			_currentState = States.STOP;
		}
		if (Input.GetKeyDown (KeyCode.A)) {
			_currentState = States.LEFT;
		}

		if (Input.GetKeyDown (KeyCode.D)) {
			_currentState = States.RIGHT;
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


	void MoveForward(float speed){
		transform.position += transform.up * speed * Time.deltaTime;
	}

	/*
	void Turn(int direction, Transform pivot){
		_pivots.follow = false;
		transform.RotateAround (pivot.position, Vector3.forward, _turnSpeed*direction);
		if (Mathf.Abs (Vector2.Angle (transform.right, pivot.right) - 90f) < 1f){
		//if (Mathf.Abs (transform.localRotation.eulerAngles.z - _targetTurnAngle) < 0.1f) {
			_turn = false;
			ChangeState (States.DEFAULT);
			_pivots.follow = true;
		}
	}
	*/

	/*
	void Turn (int direction, float speed){


		_accumulator += Mathf.Abs (direction * speed);
		transform.localEulerAngles += new Vector3 (0, 0, direction * speed);
		//Quaternion targetRot = Quaternion.RotateTowards(transform.localRotation, Quaternion.Euler(new Vector3(0,0,_targetZ)),1000);
		//transform.localRotation = Quaternion.Slerp (transform.localRotation, targetRot, Time.deltaTime * speed);
		if (Mathf.Abs (transform.localEulerAngles.z - _targetZ) <= speed || _accumulator >= 90f) {
			transform.localEulerAngles = new Vector3 (0, 0, _targetZ);
			_turn = false;
			ChangeState (States.DEFAULT);
			_accumulator = 0;
		}

	}
	*/


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

		

	protected virtual void WaitStopTime(){
		_stopTime += Time.deltaTime;
		if (_stopTime >= _stopWaitTime || (Input.GetMouseButtonUp(0) && _isPlayer)) {
			ChangeState (States.DEFAULT);
			_stop = false;
		}
	}

	public void Reset(){
		if (_initialPos != null) {
			transform.position = _initialPos.position;
			transform.localRotation = _initialPos.localRotation;
		}
		_turn = false;
		_stop = false;
		ChangeState (States.DEFAULT);
		_moveSpeed = _initialSpeed;
		_currentLane = Lanes.BOTTOM;
		if (_accidentParticle != null) {
			_accidentParticle.Stop ();
		}

	}

	void NewSituation(){
		ChangeState (States.DEFAULT);
		//if (_moveSpeed < _maxSpeed) {
			_moveSpeed += 0.1f;
		//}
		_turn = false;
		_stop = false;
	}

	public void Accident(){
		StopCoroutine ("Turn");
		StartCoroutine (AccidentRoutine ());
	}

	public IEnumerator AccidentRoutine(){
		if (!_stop) {
			_move = false;
			if (_accidentParticle != null) {
				_accidentParticle.Play ();
			}
			yield return new WaitForSeconds (0.2f);
			_gameController.Lose (true);
		}
		yield return null;
	}

	protected virtual void OnTriggerEnter2D(Collider2D other){
		if (other.CompareTag ("Stop")) {
			if (_currentState == States.STOP) {
				_stop = true;
				_stopTime = 0;
			}

			if (_isPlayer && _currentState == States.DEFAULT) {
				_gameController.ChooseNothing ();
			}
		}

		if (other.CompareTag ("Curva")) {
			if (_currentState == States.RIGHT && !_turn) {
				_turn = true;
				_turnSpriteRenderer.sprite = _wheelSprites [1];
				StartCoroutine(Turn(-1, _rightTurnSpeed, transform.localEulerAngles.z -90));
			}
			if (_currentState == States.LEFT && !_turn) {
				_turn = true;
				_turnSpriteRenderer.sprite = _wheelSprites [2];
				StartCoroutine(Turn(1, _leftTurnSpeed, transform.localEulerAngles.z + 90));
			}
		}

		if (_isPlayer && other.CompareTag ("Exit")) {
			transform.position = other.GetComponent<Teleporter> ()._tpPoint.position;
			transform.localRotation = other.GetComponent<Teleporter> ()._tpPoint.localRotation;
			_currentLane = other.GetComponent<Teleporter> ()._lane;
			_gameController.Reset ();
			NewSituation ();

		}

		if (other.CompareTag ("Checker")) {
			if (_isPlayer) {
				_gameController.CheckAnswer ();
			}

			/*
			if (_turn) {
				transform.localEulerAngles = new Vector3 (0, 0, _targetZ);
				_turn = false;
				ChangeState (States.DEFAULT);

			}
			*/


		}



	}

}
