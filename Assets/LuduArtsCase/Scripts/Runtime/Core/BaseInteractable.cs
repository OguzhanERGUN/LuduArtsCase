using UnityEngine;

namespace LuduArts.Interaction
{
	/// <summary>
	/// Etkileþimli nesneler için temel sýnýf. 
	/// Trigger kontrolü ve ses iþlemlerini yönetir.
	/// </summary>
	public abstract class BaseInteractable : MonoBehaviour, IInteractable
	{
		#region Serialized Fields

		[Header("Base Settings")]
		[Tooltip("Etkileþim sýrasýnda çalacak ses efekti")]
		[SerializeField] private AudioClip m_InteractionSound;

		[Tooltip("UI'da görünecek metin")]
		[SerializeField] private string m_PromptMessage = "Interact";

		#endregion

		#region Private Fields

		[SerializeField] private AudioSource m_AudioSource;
		private bool m_IsInRange;

		[Header("Highlight Settings")]
		[Tooltip("Q ile tarama yapýldýðýnda açýlacak görsel obje (World Canvas ikon vb.)")]
		[SerializeField] private GameObject m_HighlightVisual;

		#endregion

		#region Properties

		public bool IsInRange => m_IsInRange;

		#endregion

		#region Unity Methods

		protected virtual void Awake()
		{
			// AudioSource ayarlarý
			m_AudioSource.spatialBlend = 1.0f; // 3D Ses
		}

		// Trigger Mantýðý: Oyuncu alana girdi mi?
		private void OnTriggerEnter(Collider other)
		{
			if (other.CompareTag("Player"))
			{
				m_IsInRange = true;
			}
		}

		// Trigger Mantýðý: Oyuncu alandan çýktý mý?
		private void OnTriggerExit(Collider other)
		{
			if (other.CompareTag("Player"))
			{
				m_IsInRange = false;
			}
		}

		#endregion

		#region Public Methods

		public virtual void Interact()
		{
			// Güvenlik: Oyuncu Trigger alanýnda deðilse iþlem yapma (Raycast uzaktan vursa bile)
			if (!m_IsInRange) return;

			PlaySound();
			PerformAction();
		}

		public void SetHighlight(bool isActive)
		{
			if (m_HighlightVisual != null)
			{
				m_HighlightVisual.SetActive(isActive);
			}
		}

		public string GetInteractionPrompt()
		{
			return m_PromptMessage;
		}

		#endregion

		#region Protected Methods

		/// <summary>
		/// Her nesnenin kendi özel iþini yapacaðý soyut metot.
		/// Örn: Kapý animasyonu oynatýr, Sandýk kapaðý açar.
		/// </summary>
		protected abstract void PerformAction();

		private void PlaySound()
		{
			if (m_InteractionSound != null && m_AudioSource != null)
			{
				m_AudioSource.PlayOneShot(m_InteractionSound);
			}
		}

		#endregion
	}
}