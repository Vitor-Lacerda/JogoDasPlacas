using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class SituationGenerator : MonoBehaviour {

	public List<Situation> _situations;



	public Situation _currentSituation{ get; protected set; }

	public float _chanceFaixa = 20;


	public FaixaPedestre _topFaixa;
	public FaixaPedestre _rightFaixa;
	public FaixaPedestre _leftFaixa;
	public FaixaPedestre _botFaixa;
	FaixaPedestre _faixaAtiva;
	public bool _tinhaPedestre;

	public SpriteRenderer _topSign;
	public SpriteRenderer _rightSign;
	public SpriteRenderer _leftSign;
	public SpriteRenderer _botSign;

	public Transform _topPos;
	public Transform _rightPos;
	public Transform _botPos;
	public Transform _leftPos;

	List<GameObject> _extraObjects;

	void Awake(){
		_extraObjects = new List<GameObject> ();
	}

	public void Reset(){
		_topSign.sprite = null;
		_rightSign.sprite = null;
		_leftSign.sprite = null;
		_botSign.sprite = null;
		HideFaixas ();

		foreach (GameObject gc in _extraObjects) {
			Destroy (gc);
		}

		_extraObjects.Clear ();

	}

	public bool CheckAnswer(Choices choice){
		
		if (_currentSituation == null) {
			return false;
		}

		foreach (Choices c in _currentSituation._correctChoices) {
			if (c == choice) {
				return true;
			}
		}
		return false;
	}

	public void GenerateNew(CarController car){
		Lanes lane = car._currentLane;
		Situation s = _situations [UnityEngine.Random.Range (0, _situations.Count)];
		//Situation s = _situations [4];
		_currentSituation = s;
		if (!s._parallel && UnityEngine.Random.Range (0, 101) <= _chanceFaixa) {
			ShowFaixas (lane);
		}

		switch (lane) {

		case Lanes.BOTTOM:
			if (!s._opposite) {
				_botSign.sprite = s._signSprite;
				if (s._doubled) {
					_topSign.sprite = s._signSprite;
				}
			} else {
				_rightSign.sprite = s._signSprite;
				if (s._doubled) {
					_leftSign.sprite = s._signSprite;
				}
			}

			if (s._extraCar != null) {
				int r = UnityEngine.Random.Range (0, 100);
				if (r <= s._extraCarProbability) {
					Transform pos;
					if (s._parallel) {
						pos = _topPos;
					} else {
						if (s._direction == 0) {
							if (r % 2 == 0) {
								pos = _rightPos;
							} else {
								pos = _leftPos;
							}
						} else {
							pos = s._direction > 0 ? _leftPos : _rightPos;
						}
					}
					InstantiateExtraCar (s, pos.position, pos.localRotation, car);
				}
			}

			break;

		case Lanes.RIGHT:
			if (!s._opposite) {
				_rightSign.sprite = s._signSprite;
				if (s._doubled) {
					_leftSign.sprite = s._signSprite;
				}
			} else {
				_topSign.sprite = s._signSprite;
				if (s._doubled) {
					_botSign.sprite = s._signSprite;
				}
			}

			if (s._extraCar != null) {
				int r = UnityEngine.Random.Range (0, 100);
				if (r <= s._extraCarProbability) {
					Transform pos;
					if (s._parallel) {
						pos = _leftPos;
					} else {
						if (s._direction == 0) {
							if (r % 2 == 0) {
								pos = _topPos;
							} else {
								pos = _botPos;
							}
						} else {
							pos = s._direction > 0 ? _botPos : _topPos;
						}
					}
					InstantiateExtraCar (s, pos.position, pos.localRotation, car);
				}
			}
			break;

		case Lanes.LEFT:
			if (!s._opposite) {
				_leftSign.sprite = s._signSprite;
				if (s._doubled) {
					_rightSign.sprite = s._signSprite;
				}
			} else {
				_botSign.sprite = s._signSprite;
				if (s._doubled) {
					_topSign.sprite = s._signSprite;
				}
			}

			if (s._extraCar != null) {
				int r = UnityEngine.Random.Range (0, 100);
				if (r <= s._extraCarProbability) {
					Transform pos;
					if (s._parallel) {
						pos = _rightPos;
					} else {
						if (s._direction == 0) {
							if (r % 2 == 0) {
								pos = _topPos;
							} else {
								pos = _botPos;
							}
						} else {
							pos = s._direction > 0 ? _topPos : _botPos;
						}
					}
					InstantiateExtraCar (s, pos.position, pos.localRotation, car);
				}
			}

			break;

		case Lanes.TOP:
			if (!s._opposite) {
				_topSign.sprite = s._signSprite;
				if (s._doubled) {
					_botSign.sprite = s._signSprite;
				}
			} else {
				_leftSign.sprite = s._signSprite;
				if (s._doubled) {
					_rightSign.sprite = s._signSprite;
				}
			}
			if (s._extraCar != null) {
				int r = UnityEngine.Random.Range (0, 100);
				if (r <= s._extraCarProbability) {
					Transform pos;
					if (s._parallel) {
						pos = _botPos;
					} else {
						if (s._direction == 0) {
							if (r % 2 == 0) {
								pos = _rightPos;
							} else {
								pos = _leftPos;
							}
						} else {
							pos = s._direction > 0 ? _rightPos : _leftPos;
						}
					}
					InstantiateExtraCar (s, pos.position, pos.localRotation,car);
				}
			}

			break;


		default:
			break;
		}



	}

	public void CrossStreet(){
		if (_faixaAtiva != null) {
			_faixaAtiva.CrossTheStreet ();
		}
	}

	void ShowFaixas(Lanes l){
		_topFaixa.gameObject.SetActive (true);
		if (l == Lanes.TOP) {
			_tinhaPedestre = _topFaixa.Initialize ();
			_faixaAtiva = _topFaixa;
		}
		_rightFaixa.gameObject.SetActive (true);
		if (l == Lanes.RIGHT) {
			_tinhaPedestre = _rightFaixa.Initialize ();
			_faixaAtiva = _rightFaixa;
		}
		_leftFaixa.gameObject.SetActive (true);
		if (l == Lanes.LEFT) {
			_tinhaPedestre = _leftFaixa.Initialize ();
			_faixaAtiva = _leftFaixa;
		}
		_botFaixa.gameObject.SetActive (true);
		if (l == Lanes.BOTTOM) {
			_tinhaPedestre = _botFaixa.Initialize ();
			_faixaAtiva = _botFaixa;
		}

	}

	void HideFaixas(){
		_topFaixa.gameObject.SetActive (false);
		_rightFaixa.gameObject.SetActive (false);
		_leftFaixa.gameObject.SetActive (false);
		_botFaixa.gameObject.SetActive (false);

		_faixaAtiva = null;

	}

	void InstantiateExtraCar(Situation s, Vector2 position, Quaternion rotation, CarController car){
		int i = UnityEngine.Random.Range (0, s._extraCar.Length);
		GameObject ec = Instantiate (s._extraCar[i], position, rotation) as GameObject;
		NPCCar npc = ec.GetComponent<NPCCar> ();
		int r = UnityEngine.Random.Range (0, s._possibleStates.Length);
		npc.ChangeState (s._possibleStates [r]);
		if (!s._randomSpeed) {
			npc._moveSpeed = s._speed == 0 ? car._moveSpeed : s._speed;
		} else {
			npc._moveSpeed = UnityEngine.Random.Range (s._speedLimits.x, s._speedLimits.y);
		}
		npc._move = true;
		_extraObjects.Add (ec);

	}
}
