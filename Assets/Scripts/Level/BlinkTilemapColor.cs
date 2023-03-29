using System.Collections;
using UnityEngine;
using UnityEngine.Tilemaps;

[RequireComponent(typeof(Tilemap))]
public class BlinkTilemapColor : MonoBehaviour
{
	public float Interval;

	public Color Color1;
	public Color Color2;

	private IEnumerator Start()
	{
		var tilemap = GetComponent<Tilemap>();
		tilemap.color = Color1;
		var isColor1 = true;

		var waitCoroutine = new WaitForSeconds(Interval);

		while (true)
		{
			yield return waitCoroutine;
			tilemap.color = isColor1 ? Color2 : Color1;
			isColor1 = !isColor1;
		}
	}
}
