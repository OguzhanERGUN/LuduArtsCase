using UnityEngine;

namespace LuduArts.Interaction.Interactables
{
	public class ChestInteractable : BaseInteractable
	{
		protected override void PerformAction()
		{
			// Buraya sandýk açýlma animasyonu veya mantýðý gelecek
			Debug.Log($"<color=green>{gameObject.name} opened!</color>");

			// Örnek: Animasyon tetikleme
			// GetComponent<Animator>().SetTrigger("Open");

			// Ýsterseniz etkileþimi tek seferlik yapmak için collider'ý kapatabilirsiniz
			// GetComponent<Collider>().enabled = false;
		}
	}
}