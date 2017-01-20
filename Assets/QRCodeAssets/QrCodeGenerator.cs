using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Text;
using ZXing;
using ZXing.QrCode;
using System.Security.Cryptography;
using System;

public class QrCodeGenerator : MonoBehaviour {

	public QRCodePanelView qrCodeView;
	public UserService uService;

	public RawImage qrImage;
	public Texture2D qrCodetexture;

	// Use this for initialization
	void Start () {
		uService = GameObject.Find("_Login").GetComponent<UserService>();
		//qrCodeGen();
	}

	public void qrCodeGen(){

		int width,height;
		width = height = 256;
		string data = dataToEncrypt();
		string encryptText = encryptData(data,"infosolo_detran2016");
		qrCodetexture = new Texture2D(width,height);

		var writer = new BarcodeWriter
		{
			Format = BarcodeFormat.QR_CODE,
			Options = new QrCodeEncodingOptions
			{
				Height = height,
				Width = width
			}
		};

		qrCodetexture.SetPixels32(writer.Write(encryptText));
		qrCodetexture.Apply();

		qrImage.texture = qrCodetexture;

		qrCodeView.setPanelView(true);
	}

	private string encryptData(string Message,string senha)
	{
		byte[] Results;
		System.Text.UTF8Encoding UTF8 = new System.Text.UTF8Encoding();
		MD5CryptoServiceProvider HashProvider = new MD5CryptoServiceProvider();
		byte[] TDESKey = HashProvider.ComputeHash(UTF8.GetBytes(senha));
		TripleDESCryptoServiceProvider TDESAlgorithm = new TripleDESCryptoServiceProvider();
		TDESAlgorithm.Key = TDESKey;
		TDESAlgorithm.Mode = CipherMode.ECB;
		TDESAlgorithm.Padding = PaddingMode.PKCS7;
		byte[] DataToEncrypt = UTF8.GetBytes(Message);
		try
		{
			ICryptoTransform Encryptor = TDESAlgorithm.CreateEncryptor();
			Results = Encryptor.TransformFinalBlock(DataToEncrypt, 0, DataToEncrypt.Length);
		}
		finally
		{
			TDESAlgorithm.Clear();
			HashProvider.Clear();
		}
		return Convert.ToBase64String(Results);
	}

	private string dataToEncrypt(){


		string data = "Nome do jogo" + ";" + uService.userName + ";" + uService.userEmail + ";" + 
			uService.highscore.ToString() + ";" + 
			DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");

		qrCodeView.buildUserInfoText(data);

		return data;
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
