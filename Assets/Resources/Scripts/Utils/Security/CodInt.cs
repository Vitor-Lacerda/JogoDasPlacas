using System.Collections;
using System;
using UnityEngine;

public class CodInt {

	private int _valor;


	public int Valor {
		get {
			return _valor/Cod;
		}
		set {
			_valor = value * Cod;
		}
	}

	private int _cod = 0;

	int Cod {
		get {
			if (_cod == 0) {
				_cod = UnityEngine.Random.Range (100, 500);
			}
			return _cod;
		}
	}

	//Construtor
	public CodInt(int v){
		Valor = v;
	}


	//Conversor int -> CodInt;
	public static implicit operator CodInt(int value){
		return new CodInt (value);
	}


	//Operadores aritmeticos
	public static CodInt operator +(CodInt a, CodInt b){
		return new CodInt (a.Valor + b.Valor);
	}

	public static CodInt operator -(CodInt a, CodInt b){
		return new CodInt (a.Valor - b.Valor);
	}

	public static CodInt operator *(CodInt a, CodInt b){
		return new CodInt (a.Valor * b.Valor);
	}

	public static CodInt operator /(CodInt a, CodInt b){
		return new CodInt (a.Valor / b.Valor);
	}


	//Operadores relacionais
	public static bool operator >(CodInt a, CodInt b){
		float f1, f2;
		f1 = a.Valor;
		f2 = b.Valor;

		return (f1 > f2);
	}

	public static bool operator <(CodInt a, CodInt b){
		float f1, f2;
		f1 = a.Valor;
		f2 = b.Valor;

		return (f1 < f2);
	}

	public static bool operator >=(CodInt a, CodInt b){
		float f1, f2;
		f1 = a.Valor;
		f2 = b.Valor;

		return (f1 >= f2);
	}

	public static bool operator <=(CodInt a, CodInt b){
		float f1, f2;
		f1 = a.Valor;
		f2 = b.Valor;

		return (f1 <= f2);
	}
/*
	public static bool operator ==(CodInt a, CodInt b){
		
		float f1, f2;
		f1 = a.Valor;
		f2 = b.Valor;

		return (f1 == f2);
	}

	public static bool operator !=(CodInt a, CodInt b){
		float f1, f2;
		f1 = a.Valor;
		f2 = b.Valor;

		return (f1 != f2);
	}
*/
	

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
