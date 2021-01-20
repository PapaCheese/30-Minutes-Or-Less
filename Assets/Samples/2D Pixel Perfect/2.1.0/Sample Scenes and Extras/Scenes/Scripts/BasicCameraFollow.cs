using UnityEngine;
using System.Collections;

public class BasicCameraFollow : MonoBehaviour 
{
	public GameObject followTarget;
	private Vector3 targetPos;
	public Camera cam;

	
	void FixedUpdate() 
	{
		targetPos = new Vector3(followTarget.transform.position.x, followTarget.transform.position.y, transform.position.z);
		Vector3 roundPos = new Vector3(RoundToNearestPixel(targetPos.x, cam), RoundToNearestPixel(targetPos.y, cam), -10);
		transform.position = roundPos;
	}

	public static float RoundToNearestPixel(float unityUnits, Camera viewingCamera)
	{
		float valueInPixels = (Screen.height / (viewingCamera.orthographicSize * 2)) * unityUnits;
		valueInPixels = Mathf.Round(valueInPixels);
		float adjustedUnityUnits = valueInPixels / (Screen.height / (viewingCamera.orthographicSize * 2));
		return adjustedUnityUnits;
	}

}
