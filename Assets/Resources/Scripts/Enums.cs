using UnityEngine;
using System.Collections;


public class Configs{
	public static string PLAYERPREFSKEY = "PlayerPrefsJogoPlaca";
	public static int COUNTDOWNNUMBER = 3;
	public static int MAXSCORE = 1000000;
}

public enum Lanes 
{
	TOP = 1,
	RIGHT,
	BOTTOM,
	LEFT
}

public enum Choices
{
	RIGHT,
	LEFT,
	STOP,
	NONE

}
