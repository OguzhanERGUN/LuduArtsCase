using UnityEngine;
using LuduArts.Interaction;
using TMPro;

namespace LuduArts.Player
{
	public class InteractionController : MonoBehaviour
	{
		#region Serialized Fields

		[Header("Settings")]
		[Tooltip("Raycast menzili (Görsel seçim için)")]
		[SerializeField] private float m_RayDistance = 5f;

		[SerializeField] private LayerMask m_InteractionLayer;
		[SerializeField] private Transform m_CameraTransform;
		[SerializeField] private TextMeshProUGUI m_InteractionUI;

		#endregion

		private IInteractable m_CurrentInteractable;

		private void Update()
		{
			CheckForInteractable();

			if (Input.GetKeyDown(KeyCode.E) && m_CurrentInteractable != null)
			{
				// Trigger kontrolü BaseInteractable içinde de var ama
				// UI'ý hiç göstermemek için burada da kontrol edebiliriz.
				if (m_CurrentInteractable.IsInRange)
				{
					m_CurrentInteractable.Interact();
				}
			}
		}

		private void CheckForInteractable()
		{
			Ray ray = new Ray(m_CameraTransform.position, m_CameraTransform.forward);

			if (Physics.Raycast(ray, out RaycastHit hit, m_RayDistance, m_InteractionLayer))
			{
				IInteractable interactable = hit.collider.GetComponentInParent<IInteractable>();
				if (interactable != null)
				{
					// ÖNEMLÝ: Raycast objeyi görse bile, oyuncu Trigger içinde deðilse etkileþim yok.
					if (interactable.IsInRange)
					{
						m_CurrentInteractable = interactable;
						return;
					}
				}
			}

			m_CurrentInteractable = null;
			if (m_InteractionUI != null) m_InteractionUI.gameObject.SetActive(false);
		}
	}
}