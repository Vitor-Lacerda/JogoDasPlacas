using System.Collections.Generic;
using UnityEngine;
using System.Collections;
using System.Security.Cryptography;
using System;
using System.Text;
using UnityEngine.UI;

public class UserService : MonoBehaviour
{

	public string userName;
	public string userEmail;
	public string urlServidor;
	public CodInt highscore;
	public LogManager lManager;
	
	public string key;

	public void Awake ()
	{
		
		highscore = new CodInt(0);
		
    	CallSendScore();
    	
		//SetUserData("Teste;teste@viladoempreendedor.com.br;http://testes.desuni2014.com.br");
		#if UNITY_ANDROID
		HandleIntent();
		#else
		Application.ExternalCall ("enviarDadosJogo", "Login Service");
		#endif
	}

	void HandleIntent(){
		AndroidJavaClass up = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
		AndroidJavaObject ca = up.GetStatic<AndroidJavaObject>("currentActivity");

		AndroidJavaObject intent = ca.Call<AndroidJavaObject> ("getIntent");
		string info = intent.Call<string> ("getStringExtra", "dados");

		SetUserData (info);
	}


	public void SetUserData (string info)
	{
		string[] dados = info.Split (';');
		userName = dados [0];
		userEmail = dados [1];
		urlServidor = dados [2];
	}

	public void SetHighScore (int score)
	{
		
		if(highscore == null){
			highscore = new CodInt(0);			
		}
		if (score > highscore.Valor) {
			highscore.Valor = score;
		}
	}

	public void SetHighScore (CodInt score)
	{
		
		if(highscore == null){
			highscore = score;			
		}
		if (score.Valor > highscore.Valor) {
			highscore.Valor = score.Valor;
		}
	}

	public void CallSendScore ()
	{

		StartCoroutine ("sendScore");
	}

	public IEnumerator sendScore ()
	{

		string json = "{\"gameName\": \"nomejogo\", \"highscore\": " + highscore.Valor + ", \"metaData\": \"\", \"user\": {\"userName\": \"" + userName + "\", \"userEmail\": \"" + userEmail + "\"} }";
		Dictionary<string, string> hash = new Dictionary<string, string> ();
		hash ["Content-Type"] = "application/json";
		
		string jsonb64 = System.Convert.ToBase64String (Encoding.UTF8.GetBytes (json.ToCharArray ()));
		
		string b64 = System.Convert.ToBase64String (ToHMACSHA256 (json, key));
		
		byte[] pData = Encoding.UTF8.GetBytes(jsonb64 + "." + b64);

		WWW w = new WWW (urlServidor + "/rest/game/sendhighscore", pData, hash);

		while (!w.isDone) {

			yield return null;
		}

		//ConnectionManager cm = GameObject.Find ("ConnectionCanvas").GetComponent<ConnectionManager> ();
		/*
		if (cm != null) {

			cm.ConnectionReturn (w);
		}
		*/
		if (w.error != null || w.text != null) {

			lManager.BuildLogString (json, w.text, w.error, userEmail);
			//Application.ExternalCall("sendScoreCallback", w.error != null ? w.error : w.text);
		}



		Debug.Log (w.error);

	}
    
	public byte[] ToHMACSHA256 (string message, string key)
	{
		
		byte[] secretKey = Encoding.UTF8.GetBytes(key);
		
		HMACSHA256 hmac = new HMACSHA256(secretKey);
		
		hmac.Initialize();
		
		byte[] bytes = Encoding.UTF8.GetBytes(message);
		
		byte[] rawHmac = hmac.ComputeHash(bytes);
		
		return rawHmac;
	}
	
	public static string ByteToString (byte[] buff)
	{
		string sbinary = "";
		
		for (int i = 0; i < buff.Length; i++) {
			
			sbinary += buff [i].ToString ("X2"); // hex format
		}
		
		return (sbinary);
	}

}
