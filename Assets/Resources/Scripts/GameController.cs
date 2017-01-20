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
		Highscore = PlayerPrefs.GetInt (Configs.PLAYERPREFSKEY, 0);
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


	public void ButtonClick(int lane){

		Lanes laneClicked = (Lanes)lane;

		Lanes carLane = _playerCar._currentLane;
		//Se clicar na mesma escolhe parar
		if (carLane == laneClicked) {
			ChooseStop ();
		} else {
			//Um monte de codigo so pra ver pra que lado a pessoa escolheu virar
			//Queria um jeito mais inteligente
			switch (carLane) {

			case Lanes.BOTTOM:
				if (laneClicked == Lanes.RIGHT) {
					ChooseRight ();
				}
				if (laneClicked == Lanes.LEFT) {
					ChooseLeft ();
				}
				break;
			
			case Lanes.RIGHT:
				if (laneClicked == Lanes.TOP) {
					ChooseRight ();
				}
				if (laneClicked == Lanes.BOTTOM) {
					ChooseLeft ();
				}
				break;

			case Lanes.TOP:
				if (laneClicked == Lanes.LEFT) {
					ChooseRight ();
				}
				if (laneClicked == Lanes.RIGHT) {
					ChooseLeft ();
				}
				break;

			case Lanes.LEFT:
				if (laneClicked == Lanes.BOTTOM) {
					ChooseRight ();
				}
				if (laneClicked == Lanes.TOP) {
					ChooseLeft ();
				}
				break;

			default:
				break;
			}

		}


	}

	public void Reset(){
		_guiManager.EnableButtons (true);
		_effectsController.KillAllParticles ();
		_situationGenerator.Reset ();
		_situationGenerator.GenerateNew (_playerCar);
		_chosenAnswer = Choices.NONE;
	}

	public void RestartGame(){
		Save ();
		Initialize ();
		Begin ();
		//SceneManager.LoadScene (0);

	}

	public void CheckAnswer(){
		bool _rightAnswer = _situationGenerator.CheckAnswer (_chosenAnswer);
		if (!_rightAnswer) {
			SendHighScore ();
			Lose (false);
		} else {
			_sessionScore += _scoreIncrement;
			_guiManager.UpdateScore (_sessionScore);
			if (_sessionScore > _highScore) {
				_highScore = _sessionScore;
				_guiManager.UpdateHighscore (_highScore);
			}
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
		Save ();
		_playerCar._move = false;
		_guiManager.ShowDefeat (_situationGenerator._currentSituation, _chosenAnswer, accident);
	}

	void ChooseRight(){
		_playerCar.ChangeState(CarController.States.RIGHT);
		_guiManager.EnableButtons (false);
		_chosenAnswer = Choices.RIGHT;

	}

	void ChooseLeft(){
		_playerCar.ChangeState(CarController.States.LEFT);
		_guiManager.EnableButtons (false);
		_chosenAnswer = Choices.LEFT;


	}

	void ChooseStop(){
		_playerCar.ChangeState(CarController.States.STOP);
		_guiManager.EnableButtons (false);
		_chosenAnswer = Choices.STOP;

	}

	public void ChooseNothing(){
		_guiManager.EnableButtons (false);
		_chosenAnswer = Choices.NONE;
	}



	public void SendHighScore(){
		_userService.SetHighScore (Highscore);
		_userService.CallSendScore ();
	}
	public void Save(){
		PlayerPrefs.SetInt (Configs.PLAYERPREFSKEY, _highScore);
	}


}
