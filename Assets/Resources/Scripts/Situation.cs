using UnityEngine;
using System.Collections;

public class Situation : MonoBehaviour {

	public Sprite _signSprite;
	public bool _opposite = false;
	public bool _doubled = false;
	public Choices[] _correctChoices;
	public GameObject[] _extraCar;
	public float _extraCarProbability = 50;
	[Tooltip("0 = random -- 1 = esquerda -- -1 = direita")]
	[Range(-1,1)]
	public int _direction = 0;
	[Tooltip ("0 = playerSpeed")]
	public float _speed = 0;
	public bool _randomSpeed = true;
	public Vector2 _speedLimits = new Vector2 (1, 4);
	public bool _parallel = false;
	public NPCCar.States[] _possibleStates;

	[TextArea]
	public string _message;
	public float _moneyValue;
	public int _points;



}
