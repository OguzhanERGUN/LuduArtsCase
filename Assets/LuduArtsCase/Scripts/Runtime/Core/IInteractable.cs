namespace LuduArts.Interaction
{
	public interface IInteractable
	{
		/// <summary>
		/// Etkileþim tetiklendiðinde çalýþýr.
		/// </summary>
		void Interact();

		/// <summary>
		/// UI üzerinde gösterilecek metni döndürür.
		/// </summary>
		string GetInteractionPrompt();

		/// <summary>
		/// Oyuncunun etkileþim menzilinde (Trigger içinde) olup olmadýðýný kontrol eder.
		/// </summary>
		bool IsInRange { get; }
		void SetHighlight(bool isActive);
	}
}