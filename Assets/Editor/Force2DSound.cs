using UnityEngine;
using UnityEditor;

public class Force2DSound : AssetPostprocessor {

	void OnPreprocessAudio()
	{
		AudioImporter ai = assetImporter as AudioImporter;
		ai.threeD = false;
	}
}