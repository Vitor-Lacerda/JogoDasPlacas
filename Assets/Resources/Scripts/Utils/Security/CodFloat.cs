using System.Collections;
using System;
using UnityEngine;

[Serializable]
public class CodFloat {

	private float _valor;


	public float Valor {
		get {
			return _valor/Cod;
		}
		set {
			_valor = value * Cod;
		}
	}

	private float _cod = 0;

	float Cod {
		get {
			if (_cod == 0) {
				_cod = UnityEngine.Random.Range (100, 500);
			}
			return _cod;
		}
	}

	//Construtor
	public CodFloat(float v){
		Valor = v;
	}


	//Conversor float -> CodFloat;
	public static implicit operator CodFloat(float value){
		return new CodFloat (value);
	}


	//Operadores aritmeticos
	public static CodFloat operator +(CodFloat a, CodFloat b){
		return new CodFloat (a.Valor + b.Valor);
	}

	public static CodFloat operator -(CodFloat a, CodFloat b){
		return new CodFloat (a.Valor - b.Valor);
	}

	public static CodFloat operator *(CodFloat a, CodFloat b){
		return new CodFloat (a.Valor * b.Valor);
	}

	public static CodFloat operator /(CodFloat a, CodFloat b){
		return new CodFloat (a.Valor / b.Valor);
	}


	//Operadores relacionais
	public static bool operator >(CodFloat a, CodFloat b){
		float f1, f2;
		f1 = a.Valor;
		f2 = b.Valor;

		return (f1 > f2);
	}

	public static bool operator <(CodFloat a, CodFloat b){
		float f1, f2;
		f1 = a.Valor;
		f2 = b.Valor;

		return (f1 < f2);
	}

	public static bool operator >=(CodFloat a, CodFloat b){
		float f1, f2;
		f1 = a.Valor;
		f2 = b.Valor;

		return (f1 >= f2);
	}

	public static bool operator <=(CodFloat a, CodFloat b){
		float f1, f2;
		f1 = a.Valor;
		f2 = b.Valor;

		return (f1 <= f2);
	}

	//Metodos
	public override bool Equals (object obj)
	{
		return base.Equals (obj);
	}

	public override int GetHashCode ()
	{
		return base.GetHashCode ();
	}
		
	public override string ToString(){
		return Valor.ToString ();
	}

}
