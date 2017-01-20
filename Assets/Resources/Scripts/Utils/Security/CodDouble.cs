using System.Collections;
using System;
using UnityEngine;

public class CodDouble {

	private double _valor;


	public double Valor {
		get {
			return _valor/Cod;
		}
		set {
			_valor = value * Cod;
		}
	}

	private double _cod = 0;

	double Cod {
		get {
			if (_cod == 0) {
				_cod = UnityEngine.Random.Range (100, 500);
			}
			return _cod;
		}
	}

	//Construtor
	public CodDouble(double v){
		Valor = v;
	}


	//Conversor double -> CodDouble;
	public static implicit operator CodDouble(double value){
		return new CodDouble (value);
	}


	//Operadores aritmeticos
	public static CodDouble operator +(CodDouble a, CodDouble b){
		return new CodDouble (a.Valor + b.Valor);
	}

	public static CodDouble operator -(CodDouble a, CodDouble b){
		return new CodDouble (a.Valor - b.Valor);
	}

	public static CodDouble operator *(CodDouble a, CodDouble b){
		return new CodDouble (a.Valor * b.Valor);
	}

	public static CodDouble operator /(CodDouble a, CodDouble b){
		return new CodDouble (a.Valor / b.Valor);
	}


	//Operadores relacionais
	public static bool operator >(CodDouble a, CodDouble b){
		double f1, f2;
		f1 = a.Valor;
		f2 = b.Valor;

		return (f1 > f2);
	}

	public static bool operator <(CodDouble a, CodDouble b){
		double f1, f2;
		f1 = a.Valor;
		f2 = b.Valor;

		return (f1 < f2);
	}

	public static bool operator >=(CodDouble a, CodDouble b){
		double f1, f2;
		f1 = a.Valor;
		f2 = b.Valor;

		return (f1 >= f2);
	}

	public static bool operator <=(CodDouble a, CodDouble b){
		double f1, f2;
		f1 = a.Valor;
		f2 = b.Valor;

		return (f1 <= f2);
	}
	/*
	public static bool operator ==(CodDouble a, CodDouble b){
		double f1, f2;
		f1 = a.Valor;
		f2 = b.Valor;

		return (f1 == f2);
	}

	public static bool operator !=(CodDouble a, CodDouble b){
		double f1, f2;
		f1 = a.Valor;
		f2 = b.Valor;

		return (f1 != f2);
	}*/

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
