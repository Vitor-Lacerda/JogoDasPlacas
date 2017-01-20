using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class QRCodePanelView : MonoBehaviour {

	public Text userDataInfoText;
	public GameObject qrCodePanelObj;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void setPanelView(bool active){
		qrCodePanelObj.SetActive(active);
	}

	public void buildUserInfoText(string userData){

		string[] dataInfo = userData.Split(';');
		string data = "";

		data += "Nome do jogo: " + dataInfo[0] + "\n";
		data += "Usuario: " + dataInfo[1] + "\n";
		data += "Email: " + dataInfo[2] + "\n";
		data += "Pontuaçao: " + dataInfo[3] + "\n";
		data += "Data: " + dataInfo[4];

		userDataInfoText.text = data;
	}
}
