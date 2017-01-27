using UnityEngine;
using System.Collections;

public class CarController : MonoBehaviour {

	public SpriteRenderer _sprite;

	[Header("Positions")]
	public Transform _initialPos;

	[Header("Parameters")]
	public float _acceleration = 1;
	public float _initialSpeed = 1;
	public float _moveSpeed{ get; set;}
	public float _maxSpeed = 6;
	public float _stopSpeed = 0.1f;
	public float _speedIncrement = 0.1f;
	public float _turnSpeed = 2f;
	public float _stopWaitTime = 4;
	public float _wrongWayTime = 1;
	float _wwTimer = 0;
	bool _wrongWay = false;



	[Header("Particles")]
	public ParticleSystem _rightSignal;
	public ParticleSystem _leftSignal;
	public ParticleSystem _accidentParticle;
	public ParticleSystem _stopParticle;


	[Header("Sounds")]
	public AudioSource _accidentSound;
	public AudioSource _turnSignalSound;

	[Header("Controllers")]
	[SerializeField]
	private GameController _gameController;

	[HideInInspector]
	public int _ultimaSeta = 0; //0 - nada, 1 - esquerda, -1 direita
	bool _playedStopParticle;
	public Lanes _currentLane{ get; protected set;}
	public bool _move{ get; set; }
	bool _stoppedCar;
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
		if (_moveSpeed < _stopSpeed) {
			_stoppedCar = true;
			if (!_playedStopParticle) {
				_stopParticle.Play ();
				_gameController.CrossStreet ();
				_playedStopParticle = true;
			}
		} else {
			_playedStopParticle = false;
		}

		/*
		if (_wrongWay) {
			_sprite.color = Color.red;
		} else {
			_sprite.color = Color.white;
		}
		*/

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
		StopTurnSignal ();
		_ultimaSeta = 0;
		_wwTimer = 0;

	}

	void NewSituation(){
		_gameController.Reset ();
		StopTurnSignal ();
		_stoppedCar = false;
		_wwTimer = 0;

	}



	public void Accident(){
		StopCoroutine ("Turn");
		StartCoroutine (AccidentRoutine ());
	}

	public IEnumerator AccidentRoutine(){
		_move = false;
		_accidentSound.Play ();
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
			_turnSignalSound.Stop ();
		}
		else{
			signal.Play();
			_turnSignalSound.Play ();
			_ultimaSeta = signal == _leftSignal ? 1 : -1;
		}
	}

	void StopTurnSignal(){
		_rightSignal.Stop ();
		_leftSignal.Stop ();
		_turnSignalSound.Stop ();
	}

	void CountWrongWay(bool canteiro){
		//Debug.Log ("ContraMao");
		_wrongWay = true;
		_wwTimer += Time.deltaTime;
		if (_wwTimer >= _wrongWayTime) {
			if (canteiro) {
				_gameController.Lose ("Art. 193: Transitar com veículo em calçadas, ciclovias ou canteiros.", 7, 880.41f);
			} else {
				_gameController.Lose ("Art. 186,I: Transitar pela contramão em via de mão dupla, salvo ultrapassagem.", 5, 195.23f);

			}
		}
	}

	protected void OnTriggerEnter2D(Collider2D other){
		if (other.CompareTag ("Exit")) {
			Teleporter tp = other.GetComponent<Teleporter> ();
			if(tp != null){
				if (tp._exitLane == _currentLane || _wrongWay) {
					_gameController.Lose ("Art. 186,I: Transitar pela contramão em via de mão dupla, salvo ultrapassagem.", 5, 195.23f);
				} else {
					transform.position = tp._tpPoint.position;
					transform.localRotation = tp._tpPoint.localRotation;
					_currentLane = tp._nextLane;
					NewSituation ();
				}
			}
		}
			
		if(other.CompareTag("Checker")){
			if (!_wrongWay) {
				Lanes l = other.GetComponent<Checker> ()._lane;
				_gameController.ChooseAction (l, _stoppedCar);
			} else {
				_gameController.Lose ("Art. 186,I: Transitar pela contramão em via de mão dupla, salvo ultrapassagem.", 5, 195.23f);
			}
		}

		if (other.CompareTag ("Faixa")) {
			if (other.transform.parent.GetComponent<FaixaPedestre> ()._crossStreet) {
				_gameController.Lose ("Art. 214,II: Não deixar pedestre concluir a travessia, mesmo com sinal verde.", 7, 293.47f);
			} else {
				_gameController.Lose ("Art. 214,I: Deixar de dar preferência a pedestre que se encontra na faixa.", 7, 293.47f);
			}
		}
	}

	void OnTriggerStay2D(Collider2D other){

		if (other.CompareTag ("FaixaParada")) {
			if (_moveSpeed <= _stopSpeed) {
				_gameController.Lose ("Art. 182,VI: Parar o veículo na faixa de pedestres.", 3, 88.38f);
			}
		}

		if (other.CompareTag ("Cruzamento")) {
			if (_moveSpeed <= _stopSpeed) {
				_gameController.Lose ("Art. 182,VII: Parar o veículo na área de cruzamento.", 4, 130.16f);
			}
			_wrongWay = false;
			_wwTimer = 0;
		} else {
			if (other.CompareTag ("Pista")) {
				if (Mathf.Abs(Quaternion.Angle (transform.localRotation, other.transform.localRotation)) > 60) {
					CountWrongWay (false);
				} else {
					_wrongWay = false;
					_wwTimer = 0;
				}
			}
		}

		if (other.CompareTag ("Canteiro")) {
			CountWrongWay (true);
		}

	
	}

	void OnTriggerExit2D(Collider2D other){
		if (other.CompareTag ("Pista")) {
			_wrongWay = false;
			_wwTimer = 0;
		}
	}

}
