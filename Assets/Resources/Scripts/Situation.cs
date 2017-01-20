using UnityEngine;
using System.Collections;

public class Situation : MonoBehaviour {

	public Sprite _signSprite;
	public bool _opposite = false;
	public bool _doubled = false;
	public Choices[] _correctChoices;
	public GameObject _extraCar;
	public float _extraCarProbability = 50;
	public bool _parallel = false;
	public CarController.States[] _possibleStates;

	[TextArea]
	public string _message;
	public float _moneyValue;
	public int _points;



}
