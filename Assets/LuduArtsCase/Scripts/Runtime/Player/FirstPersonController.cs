using UnityEngine;
using UnityEngine.InputSystem;

namespace LuduArts.Player
{
	/// <summary>
	/// New Input System kullanan temel FPS karakter kontrolcüsü.
	/// CharacterController bileþeni gerektirir.
	/// </summary>
	[RequireComponent(typeof(CharacterController))]
	public class FirstPersonController : MonoBehaviour
	{
		#region Serialized Fields

		[Header("Movement Settings")]
		[Tooltip("Karakterin yürüme hýzý")]
		[SerializeField] private float m_MoveSpeed = 5.0f;

		[Tooltip("Yerçekimi kuvveti")]
		[SerializeField] private float m_Gravity = -9.81f;

		[Header("Look Settings")]
		[Tooltip("Mouse hassasiyeti")]
		[SerializeField] private float m_LookSensitivity = 1.0f;

		[Tooltip("Kamera yukarý/aþaðý bakýþ açýsý limiti (Derece)")]
		[SerializeField] private float m_LookXLimit = 85.0f;

		[Header("References")]
		[Tooltip("FPS Kamerasý")]
		[SerializeField] private Transform m_CameraTransform;

		#endregion

		#region Private Fields

		// Component Referanslarý
		private CharacterController m_CharacterController;
		private IA_Player m_InputActions; // Generate edilen C# sýnýfý

		// Hareket Deðiþkenleri
		private Vector2 m_MoveInput;
		private Vector2 m_LookInput;
		private Vector3 m_Velocity;
		private float m_RotationX = 0f;

		#endregion

		#region Unity Methods

		private void Awake()
		{
			m_CharacterController = GetComponent<CharacterController>();

			// Input sýnýfýný oluþtur
			m_InputActions = new IA_Player();

			// Cursor'ý kilitle ve gizle
			Cursor.lockState = CursorLockMode.Locked;
			Cursor.visible = false;
		}

		private void OnEnable()
		{
			// Input'larý dinlemeye baþla
			m_InputActions.Enable();

			// Event'lere abone ol (Polling yerine Event tabanlý yaklaþým da seçilebilir,
			// ancak FPS hareketi için Update içinde okumak daha akýcýdýr)
		}

		private void OnDisable()
		{
			m_InputActions.Disable();
		}

		private void Update()
		{
			ReadInput();
			HandleLook();
			HandleMovement();
		}

		#endregion

		#region Private Methods

		/// <summary>
		/// Input System'den anlýk deðerleri okur.
		/// </summary>
		private void ReadInput()
		{
			m_MoveInput = m_InputActions.Player.Move.ReadValue<Vector2>();
			m_LookInput = m_InputActions.Player.Look.ReadValue<Vector2>();
		}

		/// <summary>
		/// Mouse giriþine göre kamera ve gövde rotasyonunu yönetir.
		/// </summary>
		private void HandleLook()
		{
			// Delta time ile çarpmýyoruz çünkü New Input System "Delta" modunda zaten frame baðýmsýz deðer verebilir.
			// Ancak hissiyatý yumuþatmak için Sensitivity çarpaný kullanýyoruz.

			// Y ekseni (Sað/Sol) -> Gövdeyi döndürür
			transform.Rotate(Vector3.up * (m_LookInput.x * m_LookSensitivity));

			// X ekseni (Yukarý/Aþaðý) -> Kamerayý döndürür (Tersi yönde)
			m_RotationX -= m_LookInput.y * m_LookSensitivity;

			// Bakýþ açýsýný kýsýtla (Clamp)
			m_RotationX = Mathf.Clamp(m_RotationX, -m_LookXLimit, m_LookXLimit);

			// Kameraya rotasyonu uygula
			if (m_CameraTransform != null)
			{
				m_CameraTransform.localRotation = Quaternion.Euler(m_RotationX, 0f, 0f);
			}
		}

		/// <summary>
		/// WASD giriþine göre karakter hareketini ve yerçekimini yönetir.
		/// </summary>
		private void HandleMovement()
		{
			// 1. Yer Yönünde Hareket
			// Karakterin baktýðý yöne göre hareket vektörü oluþtur
			Vector3 move = transform.right * m_MoveInput.x + transform.forward * m_MoveInput.y;

			// Character Controller ile hareket et
			m_CharacterController.Move(move * m_MoveSpeed * Time.deltaTime);

			// 2. Yerçekimi Uygula
			// Eðer yerdeysek düþme hýzýný sýfýrla (hafif bir baský ile yere yapýþýk tut)
			if (m_CharacterController.isGrounded && m_Velocity.y < 0)
			{
				m_Velocity.y = -2f;
			}

			m_Velocity.y += m_Gravity * Time.deltaTime;
			m_CharacterController.Move(m_Velocity * Time.deltaTime);
		}

		#endregion
	}
}