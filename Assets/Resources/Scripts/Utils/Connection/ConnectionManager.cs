using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ConnectionManager : MonoBehaviour {

	public float timerClick;
	
	public float timerTestConnection;
	
	float maxTimerTextConnection = 120;

	float maxTimer = 30;
	
	bool openPanel;
	
	[Space(10)]
	
	[Header("Visual assets")]
	
	public GameObject buttonsPanelObj;

	public Image conBtnImage;

	public Sprite nonConnectSpr;

	public Sprite connectedSpr;

	// Use this for initialization
	void Start () {
	
		DontDestroyOnLoad(gameObject);
	}
	
	// Update is called once per frame
	void Update () {
	
		timerClick += Time.deltaTime;
		
		timerTestConnection += Time.deltaTime;
		
		if(timerTestConnection > maxTimerTextConnection){
			
			timerTestConnection = 0;
			
			StartCoroutine("ConnectionTest");
		}
	}
	
	public void OpenConnectionPanel_Btn(){
		
		buttonsPanelObj.SetActive(openPanel = openPanel ? false : true);
	}

	public void SendScore_Btn(){

		if(timerClick >= maxTimer){

			timerClick = 0;

			UserService us = GameObject.Find("_Login").GetComponent<UserService>();

			if(us != null){

				us.StartCoroutine("sendScore");
			}
			else{

				Debug.LogWarning("_Login nao encontrado");
				
				conBtnImage.sprite = nonConnectSpr;
			}
		}
		
	}
	
	public void ShowLog_Btn(){
	
		GameObject login = GameObject.Find("_Login");
		
		if(login != null){
		
			login.GetComponent<LogManager>().ShowLog(login.GetComponent<UserService>().userEmail);
		}
		else{
		
			Debug.LogWarning("_Login nao encontrado");
		}
	
	}
		
	public void ConnectionReturn(WWW w){

		conBtnImage.sprite = (w.error == null) ?  connectedSpr : nonConnectSpr;

	}

	//2 min para testar a conexao
	public IEnumerator ConnectionTest(){

		UserService us = GameObject.Find("_Login").GetComponent<UserService>();
		
		if(us != null){
		
			WWW w = new WWW(us.urlServidor + "/rest/game/sendhighscore");
			
			while (!w.isDone) {
				
				yield return null;  
			}
			
			conBtnImage.sprite = (w.error == null) ? connectedSpr : nonConnectSpr;
		}
		else{
		
			Debug.LogWarning("_Login nao encontrado");
		
			conBtnImage.sprite = nonConnectSpr;
		}
	}

}
