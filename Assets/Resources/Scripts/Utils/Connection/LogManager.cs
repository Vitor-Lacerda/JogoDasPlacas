using UnityEngine;
using System.Collections;
using System;

public class LogManager : MonoBehaviour {

	public void ShowLog(string userEmail){
		string s = PlayerPrefs.GetString ("VilaEmpreendedor" + userEmail, "");
		if (string.IsNullOrEmpty (s)) {
			s ="Log Vazio"; 
		} 
		//Application.ExternalCall ("mostrarLog", s);
		
		Application.OpenURL ("javascript:void(window.open('data:text/html," + WWW.EscapeURL (s) + "'))");
		
	}
	
	void AddToLog(string s, string userEmail){
		string prev = PlayerPrefs.GetString ("VilaEmpreendedor" + userEmail, "");
		string[] entries = prev.Split ('%');
		if (entries.Length > 80) {
			prev = "";
			for(int i = 1; i < entries.Length;i++){
				prev += entries[i] +"<br>%";
			}
		}
		prev += s;
		PlayerPrefs.SetString (Configs.PLAYERPREFSKEY + userEmail, prev);
	}
	
	public string BuildLogString(string sent, string received, string error, string userEmail){
		string s = "<br>";
		s += "Enviado em: " + DateTime.Now;
		s += " por: " + userEmail;
		s += "<br>" + sent;
		if (!string.IsNullOrEmpty (received)) {
			s += "<br>Resposta: <br>" + received;
		}
		if (!string.IsNullOrEmpty (error)) {
			s+= "<br>Erro:<br>" + error;
		}
		s += "<br>%";
		AddToLog (s, userEmail);
		return s;
		
	}
}
