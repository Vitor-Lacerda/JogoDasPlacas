using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Globalization;

public class GUIManager : MonoBehaviour {


	[Space(10)]


	[Header("Defeat Screen")]
	public GameObject _defeatScreen;
	public Text _defeatMessage;
	public Text _defeatScoreText;
	public Text _defeatInfractionText;

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
	public GameObject _scoreImage;
	public GameObject _highscoreImage;
	public Text _scoreText;
	public Text _highScoreText;

	[Header("Opening Screen")]
	public float _cameraSpeed = 0.2f;
	public float _cameraTargetSize = 5f;
	public GameObject _openScreen;
	public Animator _textAnim;
	public Animator _buttonAnim;



	[Header("Image Effects")]
	public Material _transitionMaterial;
	public SimpleBlit _cameraBlit;
	public Color _flashColor = Color.red;
	public Texture _flashTexture;
	public Color _transitionColor = Color.cyan;
	public Color _screenColor = Color.white;
	public Texture _transitionTex;
	public Texture _transitionTexReverse;
	[Range(0,1)]
	public float _transitionIncrement = 0.1f;
	public float _transitionWaitTime = 0.2f;

	bool _losing;

	NumberFormatInfo nfi;

	void Start(){
		nfi = new NumberFormatInfo ();
		nfi.NumberDecimalSeparator = ",";
		nfi.NumberGroupSeparator = ".";
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
			_defeatInfractionText.text = "";
		} else if (choice == Choices.STOP) {
			_defeatMessage.text = "Artigo 182,V: Parar o veículo na pista.";
			_defeatInfractionText.text = GetInfractionText (5, 195.53f);
		}
		else {
			_defeatMessage.text = situation._message;
			_defeatInfractionText.text = GetInfractionText (situation._points, situation._moneyValue);
		}

		if (!_losing) {
			StartCoroutine (DefeatRoutine ());
		}
		//_defeatScreen.SetActive (true);
	}

	public void ShowDefeat(string message, int points, float value){
		_defeatMessage.text = message;
		_defeatInfractionText.text = GetInfractionText (points, value);
		if (!_losing) {
			StartCoroutine (DefeatRoutine ());
		}
		//_defeatScreen.SetActive (true);
	}

	string GetInfractionText(int points, float value){
		return ("Infração " + GetSeverity (points) + ".\n" +
		points.ToString () + " pontos na CNH.\n" +
			"Valor: R$"+ value.ToString(nfi)); 
	}

	string GetSeverity(int points){
		string r = "Leve";

		switch (points) {
		case 7:
			r = "gravíssima";
			break;
		case 5:
			r = "grave";
			break;
		case 4:
			r = "média";
			break;
		default:
			r = "leve";
			break;
		}
		return r;
	}

	public IEnumerator OpeningRoutine(){
		float z = Camera.main.orthographicSize;
		//Close (_openScreen);
		_scoreImage.SetActive(true);
		_highscoreImage.SetActive (true);
		_textAnim.enabled = true;
		_buttonAnim.enabled = true;
		while (_cameraTargetSize - z > 0) {
			Camera.main.orthographicSize = z;
			z += _cameraSpeed;
			yield return null;
		}
		Camera.main.orthographicSize = _cameraTargetSize;
		yield return null;
	}

	IEnumerator DefeatRoutine(){
		
		float f = 0;
		float t = 0;
		_losing = true;
		_cameraBlit.SetFloat ("_DoTransition", 1);
		_cameraBlit.SetColor ("_MainColor", _flashColor);
		_cameraBlit.SetTexture ("_TransitionTex", _flashTexture);
		_cameraBlit.SetFloat ("_Cutoff", 1);

		while (t < 0.5f) {
			f = Mathf.PingPong (Time.time*2, 0.5f);
			_cameraBlit.SetFloat ("_Fade", f);
			t += Time.deltaTime;
			yield return null;
		}

		_cameraBlit.SetFloat ("_Fade", 0);
		_defeatScreen.transform.localScale = new Vector3 (0, 0, 0);
		_defeatScreen.SetActive (true);
		_losing = false;
		while (Mathf.Abs (_defeatScreen.transform.localScale.x - 1) > 0.1f) {
			_defeatScreen.transform.localScale = Vector3.Lerp (_defeatScreen.transform.localScale, new Vector3 (1, 1, 1), Time.deltaTime * 3);
			yield return null;
		}

		_cameraBlit.SetFloat ("_Cutoff", 0);
		_cameraBlit.SetFloat ("_DoTransition", 0);
		yield return null;
	}

	public IEnumerator TransitionForwardRoutine(bool loop){
		_cameraBlit.SetFloat ("_DoTransition", 1);
		_cameraBlit.SetColor ("_Color", _transitionColor);
		_cameraBlit.SetColor ("_MainColor", _screenColor);
		_cameraBlit.SetFloat ("_Fade", 1);
		_cameraBlit.SetFloat ("_Cutoff", 0);
		_cameraBlit.SetTexture ("_TransitionTex", _transitionTex);
		float c = 0;
		while (c < 1) {
			c += _transitionIncrement;
			_cameraBlit.SetFloat ("_Cutoff", c);
			yield return null;

		}
			
		yield return new WaitForSeconds (_transitionWaitTime);

		if (loop) {
			StartCoroutine (TransitionBackRoutine ());
		}

		_cameraBlit.SetFloat ("_DoTransition", 0);
		yield return null;
	}

	public IEnumerator TransitionBackRoutine(){
		_cameraBlit.SetFloat ("_DoTransition", 1);
		_cameraBlit.SetColor ("_Color", _transitionColor);
		_cameraBlit.SetColor ("_MainColor", _screenColor);
		_cameraBlit.SetFloat ("_Fade", 1);
		_cameraBlit.SetFloat ("_Cutoff", 1);
		_cameraBlit.SetTexture ("_TransitionTex", _transitionTexReverse);
		float c;
		c = 1;
		while (c > 0) {
			c -= _transitionIncrement;
			_cameraBlit.SetFloat ("_Cutoff", c);
			yield return null;

		}
		_cameraBlit.SetFloat ("_DoTransition", 0);
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
