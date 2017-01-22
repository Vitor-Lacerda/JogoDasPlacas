using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class GUIManager : MonoBehaviour {
	[Header("Buttons")]
	public GameObject[] _gameButtons;

	[Space(10)]
	[Header("Opening Screen")]
	public GameObject _openScreen;

	[Header("Defeat Screen")]
	public GameObject _defeatScreen;
	public Text _defeatMessage;
	public Text _defeatScoreText;

	[Header("Victory Screen")]
	public GameObject _victoryScreen;

	[Header("Tutorial")]
	[SerializeField]
	GameObject _tutorialStart;

	[Header("Countdown")]
	[SerializeField]
	GameObject _countdownPanel;
	public Image _trafficLightImage;
	public Sprite[] _trafficLightSprites;
	[Space(10)]


	[Header("Game GUI")]
	public Text _scoreText;
	public Text _highScoreText;



	void Start(){
		Open (_openScreen);
	}

	public void UpdateScore(int value){
		_scoreText.text = value.ToString ();
		_defeatScoreText.text = "Você fez: " + value.ToString () + " pontos.";
	}

	public void UpdateHighscore(int value){
		_highScoreText.text = value.ToString ();
	}

	public void ShowDefeat(Situation situation, Choices choice, bool accident){
		if (accident) {
			_defeatMessage.text = "Você causou um acidente!";
		} else if (choice == Choices.STOP) {
			_defeatMessage.text = "Você parou seu carro quando não devia. Isso pode causar acidentes!";
		}
		else {
			_defeatMessage.text = situation._message;
		}
			
		_defeatScreen.SetActive (true);
	}

	public void ShowDefeat(string message){
		_defeatMessage.text = message;
		_defeatScreen.SetActive (true);
	}

	public void ShowVictory(){
		Open (_victoryScreen);
	}

	public void Close(GameObject panel){
		panel.SetActive (false);
	}

	public void Open(GameObject panel){
		panel.SetActive (true);
	}

	public void OpenTutorial(){
		_tutorialStart.SetActive (true);
	}

	public void EnableButtons(bool b){
		foreach (GameObject btn in _gameButtons) {
			btn.SetActive (b);
		}
	}

	public void Countdown(int n){

		if (n > 0) {
			_trafficLightImage.sprite = _trafficLightSprites [n-1];
		}
		else{
			_countdownPanel.SetActive (false);
		}


	}

	public void ShowCountdown(){
		_countdownPanel.SetActive (true);
	}
}
