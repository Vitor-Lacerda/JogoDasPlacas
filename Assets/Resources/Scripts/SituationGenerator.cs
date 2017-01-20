using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class SituationGenerator : MonoBehaviour {

	public List<Situation> _situations;


	public Situation _currentSituation{ get; protected set; }


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
		//Situation s = _situations [UnityEngine.Random.Range (0, _situations.Count)];
		Situation s = _situations [7];
		_currentSituation = s;

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
						if (r % 2 == 0) {
							pos = _rightPos;
						} else {
							pos = _leftPos;
						}
					}
					InstantiateExtraCar (s, pos.position, pos.localRotation, car._moveSpeed);
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
						if (r % 2 == 0) {
							pos = _topPos;
						} else {
							pos = _botPos;
						}
					}
					InstantiateExtraCar (s, pos.position, pos.localRotation, car._moveSpeed);
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
						if (r % 2 == 0) {
							pos = _topPos;
						} else {
							pos = _botPos;
						}
					}
					InstantiateExtraCar (s, pos.position, pos.localRotation, car._moveSpeed);
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
						if (r % 2 == 0) {
							pos = _rightPos;
						} else {
							pos = _leftPos;
						}
					}
					InstantiateExtraCar (s, pos.position, pos.localRotation,car._moveSpeed);
				}
			}

			break;


		default:
			break;
		}



	}

	void InstantiateExtraCar(Situation s, Vector2 position, Quaternion rotation, float speed){

		GameObject ec = Instantiate (s._extraCar, position, rotation) as GameObject;
		NPCCar npc = ec.GetComponent<NPCCar> ();
		int r = UnityEngine.Random.Range (0, s._possibleStates.Length);
		npc.ChangeState (s._possibleStates [r]);
		npc._moveSpeed = speed;
		_extraObjects.Add (ec);

	}
}
