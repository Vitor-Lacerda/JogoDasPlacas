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

	public SimpleBlit _cameraBlit;

	bool _losing;


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

		if (!_losing) {
			StartCoroutine (DefeatRoutine ());
		}
		//_defeatScreen.SetActive (true);
	}

	public void ShowDefeat(string message){
		_defeatMessage.text = message;
		if (!_losing) {
			StartCoroutine (DefeatRoutine ());
		}
		//_defeatScreen.SetActive (true);
	}

	IEnumerator DefeatRoutine(){
		float f = 0;
		float t = 0;
		_losing = true;
		while (t < 0.5f) {
			f = Mathf.PingPong (Time.time*2, 0.5f);
			_cameraBlit.SetFade (f);
			t += Time.deltaTime;
			yield return null;
		}

		_cameraBlit.SetFade (0);
		_defeatScreen.transform.localScale = new Vector3 (0, 0, 0);
		_defeatScreen.SetActive (true);
		_losing = false;
		while (Mathf.Abs (_defeatScreen.transform.localScale.x - 1) > 0.1f) {
			_defeatScreen.transform.localScale = Vector3.Lerp (_defeatScreen.transform.localScale, new Vector3 (1, 1, 1), Time.deltaTime * 3);
			yield return null;
		}

		yield return null;
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
