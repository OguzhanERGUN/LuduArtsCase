using UnityEngine;
using System.Collections.Generic;
using LuduArts.Interaction;

namespace LuduArts.Player
{
	/// <summary>
	/// Oyuncunun çevresindeki etkileþimli nesneleri taramasýný saðlar.
	/// Sphere Collider (Trigger) gerektirir.
	/// </summary>
	public class PlayerScannerController : MonoBehaviour
	{
		#region Private Fields

		// Menzildeki nesneleri tutan liste (Cache)
		// Her frame GetComponent yapmamak için listeliyoruz.
		private List<IInteractable> m_NearbyInteractables = new List<IInteractable>();

		private bool m_IsScanning = false;

		#endregion

		#region Unity Methods

		private void Update()
		{
			HandleInput();
		}

		private void OnTriggerEnter(Collider other)
		{
			// Sadece Interactable nesneleri listeye al
			if (other.TryGetComponent(out IInteractable interactable))
			{
				if (!m_NearbyInteractables.Contains(interactable))
				{
					m_NearbyInteractables.Add(interactable);

					// Eðer oyuncu þu an zaten Q'ya basýyorsa, yeni gireni de hemen yak
					if (m_IsScanning)
					{
						interactable.SetHighlight(true);
					}
				}
			}
		}

		private void OnTriggerExit(Collider other)
		{
			if (other.TryGetComponent(out IInteractable interactable))
			{
				if (m_NearbyInteractables.Contains(interactable))
				{
					// Listeden çýkarken mutlaka highlight'ý kapat
					interactable.SetHighlight(false);
					m_NearbyInteractables.Remove(interactable);
				}
			}
		}

		#endregion

		#region Private Methods

		private void HandleInput()
		{
			// Q tuþuna basýlý tutulduðu sürece göster (Hold)
			// Eðer toggle (bas-aç/bas-kapa) istersen GetKeyDown kullanabilirsin.
			bool isKeyDown = Input.GetKey(KeyCode.Q);

			if (isKeyDown != m_IsScanning)
			{
				m_IsScanning = isKeyDown;
				UpdateHighlights(m_IsScanning);
			}
		}

		/// <summary>
		/// Listedeki tüm nesnelerin highlight durumunu günceller.
		/// </summary>
		private void UpdateHighlights(bool state)
		{
			// Tersten döngü (Reverse for-loop) güvenlidir,
			// çünkü listeden silinmiþ (Destroy olmuþ) objeler hata verebilir.
			for (int i = m_NearbyInteractables.Count - 1; i >= 0; i--)
			{
				IInteractable item = m_NearbyInteractables[i];

				// Unity Object kontrolü (Destroy edilmiþ olabilir mi?)
				if (item as Object == null)
				{
					m_NearbyInteractables.RemoveAt(i);
					continue;
				}

				item.SetHighlight(state);
			}
		}

		#endregion
	}
}