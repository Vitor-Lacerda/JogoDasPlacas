using UnityEngine;
using System.Collections;

public class CarController : MonoBehaviour {



	public Transform _initialPos;
	public float _acceleration = 1;
	public float _initialSpeed = 1;
	public float _moveSpeed{ get; set;}
	public float _maxSpeed = 6;
	public float _speedIncrement = 0.1f;
	public float _turnSpeed = 2f;
	public float _stopWaitTime = 4;

	public ParticleSystem _rightSignal;
	public ParticleSystem _leftSignal;
	public SpriteRenderer _turnSpriteRenderer;

	public ParticleSystem _accidentParticle;

	public Sprite[] _wheelSprites;

	public Lanes _currentLane{ get; protected set;}
	public bool _move{ get; set; }
	bool _stoppedCar;


	[SerializeField]
	private GameController _gameController;

	protected float _stopTime = 0;





	// Use this for initialization
	protected virtual void Awake () {
		Reset ();
		_move = true;

	}
	
	// Update is called once per frame
	void Update () {
		if (!_move)
			return;
		MoveForward (_moveSpeed);
		GetInput ();
		if (_moveSpeed < 0.2f) {
			_stoppedCar = true;
		}
	}

	void GetInput(){
		transform.localEulerAngles += new Vector3 (0, 0, -Input.GetAxisRaw("Horizontal")  * _moveSpeed);

		if (Input.GetAxisRaw ("Vertical") < -0.1f) {
			_moveSpeed = Mathf.Lerp (_moveSpeed, 0, Time.deltaTime * _acceleration);
		} else {
			_moveSpeed = Mathf.Lerp (_moveSpeed, _initialSpeed, Time.deltaTime * _acceleration);
		}

		if(Input.GetKeyUp(KeyCode.Q)){
			ToggleTurnSignal (_leftSignal);
			_rightSignal.Stop ();
		}
		if(Input.GetKeyUp(KeyCode.E)){
			ToggleTurnSignal (_rightSignal);
			_leftSignal.Stop ();
		}
	}

	protected void MoveForward(float speed){
		transform.position += transform.up * speed * Time.deltaTime;
	}


	public void Reset(){
		if (_initialPos != null) {
			transform.position = _initialPos.position;
			transform.localRotation = _initialPos.localRotation;
		}
		_moveSpeed = _initialSpeed;
		_stoppedCar = false;
		_currentLane = Lanes.BOTTOM;
		if (_accidentParticle != null) {
			_accidentParticle.Stop ();
		}

	}



	public void Accident(){
		StopCoroutine ("Turn");
		StartCoroutine (AccidentRoutine ());
	}

	public IEnumerator AccidentRoutine(){
		_move = false;
		if (_accidentParticle != null) {
			_accidentParticle.Play ();
		}
		yield return new WaitForSeconds (0.2f);
		_gameController.Lose (true);
		yield return null;
	}

	void ToggleTurnSignal(ParticleSystem signal){
		if(signal.isPlaying){
			signal.Stop ();	
		}
		else{
			signal.Play();
		}
	}

	protected void OnTriggerEnter2D(Collider2D other){
		if (other.CompareTag ("Exit")) {
			transform.position = other.GetComponent<Teleporter> ()._tpPoint.position;
			transform.localRotation = other.GetComponent<Teleporter> ()._tpPoint.localRotation;
			_currentLane = other.GetComponent<Teleporter> ()._lane;
			_gameController.Reset ();
			_rightSignal.Stop ();
			_leftSignal.Stop ();
			_stoppedCar = false;
		}


		if(other.CompareTag("Checker")){
			if (!_stoppedCar) {
				Lanes l = other.GetComponent<Checker> ()._lane;
				_gameController.ChooseAction (l);
			} else {
				_gameController.ChooseAction (_currentLane);
			}

		}
	}

}
