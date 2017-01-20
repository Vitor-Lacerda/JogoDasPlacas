using UnityEngine;
using System.Collections;

public class SocialMedia: MonoBehaviour{

	public static void Facebook(float points){
		string AppID = "477640589044450";
		string caption = "Eu fiz " + points.ToString() + " pontos no jogo Vila do Empreededor!";
		string nome = "Vila do Empreededor";
		string img = "http://desafiouniversitarioempreendedor.sebrae.com.br/plataforma/img/plataforma/jogos/vilaempreendedor-thumb.jpg";
		string description = "Desafio Sebrae Universitario";
		string link = "http://desafiouniversitarioempreendedor.sebrae.com.br/plataforma";
		
		Application.OpenURL("javascript:void(window.open('http://www.facebook.com/dialog/feed?app_id=" + AppID + 
		                    "&picture=" + WWW.EscapeURL(img) +
		                    "&name=" + WWW.EscapeURL(nome) +
		                    "&caption=" + WWW.EscapeURL(caption) +
		                    "&description=" + WWW.EscapeURL(description) +
		                    "&link=" + WWW.EscapeURL(link) +
		                    "&redirect_uri=" + "http://www.facebook.com/" + "'))");

		string lastDate = PlayerPrefs.GetString ("SocialDate");


	}


	public static void Twitter(float points){

		//string cards = "http://desafiouniversitarioempreendedor.sebrae.com.br/plataforma/robotwitter.html";
		string cardsHomolog = "http://fabrica.infosolo.com.br/plataforma/twitterPlataforma.html";

		string tweet = "Eu fiz " + points.ToString () + " pontos no jogo Vila do Empreendedor!";
		Application.OpenURL("javascript:void(window.open('http://www.twitter.com/intent/tweet"+
		                    "?url=" + WWW.EscapeURL(cardsHomolog) +
		                    "&text=" + WWW.EscapeURL(tweet) +
		                    "&hashtags=" + "DesafioUniversitarioEmpreendedor" + "'))");




		//TweetPanel.SetActive (true);
	}


	/*GUIStyle s = new GUIStyle ();


		//Social Media buttons
		Rect FacebookRect = new Rect (SCREEN_EDGE_OFFSET, SCREEN_EDGE_OFFSET, 32, 32);
		if(GUI.Button(FacebookRect,FacebookTexture, s)){
			SocialMedia.Facebook(m_player.GetCurrentCapital());
		}

		Rect TwitterRect = new Rect (SCREEN_EDGE_OFFSET*4, SCREEN_EDGE_OFFSET, 32, 32);
		if(GUI.Button(TwitterRect,TwitterTexture,s)){
			SocialMedia.Twitter(m_player.GetCurrentCapital());
		}
		*/

}
