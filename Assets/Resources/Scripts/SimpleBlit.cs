using UnityEngine;
using System.Collections;

[ExecuteInEditMode]
public class SimpleBlit : MonoBehaviour
{

	public Material BlitMaterial;

	void Start(){
		SetFloat ("_Fade", 1);
		SetFloat ("_Cutoff", 0);
		SetFloat ("_DoTransition", 0);
	}

    void OnRenderImage(RenderTexture src, RenderTexture dst)
    {
       	if (BlitMaterial != null)
            Graphics.Blit(src, dst, BlitMaterial);
    }

	public void SetFloat(string name, float v){
		BlitMaterial.SetFloat (name, v);
	}

	public void SetColor(string name, Color c){
		BlitMaterial.SetColor (name, c);
	}

	public void SetTexture(string name, Texture t){
		BlitMaterial.SetTexture (name, t);
	}




}
