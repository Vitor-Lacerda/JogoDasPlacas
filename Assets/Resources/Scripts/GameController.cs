using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour {

	[Header("Controllers")]
	public CarController _playerCar;
	public GUIManager _guiManager;
	public EffectsController _effectsController;
	public SituationGenerator _situationGenerator;

	[Space(10)]
	[Header("Configs")]
	public int _scoreIncrement = 20000;

	[Header("User Service")]
	public UserService _userService;




	bool _scored = false;
	bool _lost = false;
	Choices _chosenAnswer;
	int _highScore;
	int Highscore{
		get{
			return _highScore;
		}
		set{
			_highScore = value;
			_guiManager.UpdateHighscore (value);
		}
	}
	int _sessionScore;
	int SessionScore{
		get{
			return _sessionScore;
		}
		set{
			_sessionScore = value;
			_guiManager.UpdateScore (value);
			if (value > Highscore) {
				Highscore = value;
			}
		}

	}

	void Start(){
		Initialize ();
	}
		
	void Initialize () {
		_playerCar.Reset ();
		_situationGenerator.Reset ();
		_playerCar._move = false;
		SessionScore = 0;

	}

	public void Begin(){
		//Se tiver jogo salvo so comeca
		Debug.Log(PlayerPrefs.HasKey(Configs.PLAYERPREFSKEY));
		Highscore = PlayerPrefs.GetInt (Configs.PLAYERPREFSKEY,0);
		if(Highscore != 0){
			StartGame ();
		}
		//Se nao o botao do tutorial vai comecar
		else{
			Highscore = 0;
			_guiManager.OpenTutorial ();
		}
	}

	public void StartGame(){
		StartCoroutine (StartGameCoroutine ());
	}

	IEnumerator StartGameCoroutine(){

		_guiManager.ShowCountdown ();
		int i = Configs.COUNTDOWNNUMBER;
		while (i > 0) {
			_guiManager.Countdown (i);
			yield return new WaitForSeconds (1);
			i--;
		}

		_guiManager.Countdown (0);
		Reset ();
		_playerCar._move = true;
		//SendHighScore ();
	}


	public void ChooseAction(Lanes lane, bool stopped){

		//Lanes laneClicked = (Lanes)lane;

		Lanes carLane = _playerCar._currentLane;
			//Um monte de codigo so pra ver pra que lado a pessoa escolheu virar
			//Queria um jeito mais inteligente
		switch (carLane) {

			case Lanes.BOTTOM:
				if (lane == Lanes.RIGHT) {
					ChooseRight ();
				}
				if (lane == Lanes.LEFT) {
					ChooseLeft ();
				}
				break;
			
			case Lanes.RIGHT:
				if (lane == Lanes.TOP) {
					ChooseRight ();
				}
				if (lane == Lanes.BOTTOM) {
					ChooseLeft ();
				}
				break;

			case Lanes.TOP:
				if (lane == Lanes.LEFT) {
					ChooseRight ();
				}
				if (lane == Lanes.RIGHT) {
					ChooseLeft ();
				}
				break;

			case Lanes.LEFT:
				if (lane == Lanes.BOTTOM) {
					ChooseRight ();
				}
				if (lane == Lanes.TOP) {
					ChooseLeft ();
				}
				break;

			default:
				break;
		}

		if (!_lost) {
			if (stopped) {
				if (_situationGenerator._tinhaPedestre) {
					if (_situationGenerator.CheckAnswer (Choices.STOP)) {
						ChooseStop ();
					}
				} else {
					ChooseStop ();
				}
			}
			CheckAnswer ();

		}
	}

	public void Reset(){
		//_guiManager.EnableButtons (true);
		_effectsController.KillAllParticles ();
		_situationGenerator.Reset ();
		_situationGenerator.GenerateNew (_playerCar);
		_chosenAnswer = Choices.NONE;
		_lost = false;
		_scored = false;
	}

	public void RestartGame(){
		Save ();
		Initialize ();
		Begin ();
		//SceneManager.LoadScene (0);

	}

	public void CheckAnswer(){
		if (_scored)
			return;
		_scored = true;
		bool _rightAnswer = _situationGenerator.CheckAnswer (_chosenAnswer);
		if (!_rightAnswer) {
			SendHighScore ();
			Lose (false);
		} else {
			SessionScore += _scoreIncrement;
			/*
			if (_sessionScore >= Configs.MAXSCORE) {
				Win ();
			}
			*/
		}
	}

	public void Win(){
		Save ();
		_playerCar._move = false;
		_guiManager.ShowVictory ();
	}


	public void Lose(bool accident){
		_lost = true;
		Save ();
		_playerCar._move = false;
		foreach (NPCCar car in GameObject.FindObjectsOfType<NPCCar>()) 
		{
			car._move = false;	
		}
		_guiManager.ShowDefeat (_situationGenerator._currentSituation, _chosenAnswer, accident);
	}


	public void Lose(string message){
		_lost = true;
		Save ();
		_playerCar._move = false;
		foreach (NPCCar car in GameObject.FindObjectsOfType<NPCCar>()) 
		{
			car._move = false;	
		}
		_guiManager.ShowDefeat (message);

	}


	void ChooseRight(){
		_guiManager.EnableButtons (false);
		if (_playerCar._ultimaSeta != -1) {
			Lose ("Nao ligou a seta");
		}
		_chosenAnswer = Choices.RIGHT;

	}

	void ChooseLeft(){
		_guiManager.EnableButtons (false);
		if (_playerCar._ultimaSeta != 1) {
			Lose ("Nao ligou a seta");
		}
		_chosenAnswer = Choices.LEFT;


	}

	void ChooseStop(){
		_guiManager.EnableButtons (false);
		_chosenAnswer = Choices.STOP;

	}

	public void ChooseNothing(){
		_guiManager.EnableButtons (false);
		_chosenAnswer = Choices.NONE;
	}

	public void CrossStreet(){
		_situationGenerator.CrossStreet ();
	}

	public void SendHighScore(){
		_userService.SetHighScore (Highscore);
		_userService.CallSendScore ();
	}
	public void Save(){
		PlayerPrefs.SetInt (Configs.PLAYERPREFSKEY, Highscore);
		PlayerPrefs.Save ();
	}


}
