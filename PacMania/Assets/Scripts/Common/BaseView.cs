using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public abstract class BaseView : MonoBehaviour
{
	public void Hide()
	{
		GetComponent<SpriteRenderer>().enabled = false;
	}

	public void Show()
	{
		GetComponent<SpriteRenderer>().enabled = true;
	}
}
