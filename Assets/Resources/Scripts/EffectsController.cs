using UnityEngine;
using System.Collections;

public class EffectsController : MonoBehaviour {

	public ParticleSystem[] _roadParticles;

	public void PlayParticle(ParticleSystem p){
		p.Play ();
	}

	public void KillAllParticles(){
		foreach (ParticleSystem ps in _roadParticles) {
			ps.Stop ();
		}
	}


}
