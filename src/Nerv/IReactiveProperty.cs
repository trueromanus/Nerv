namespace Nerv {

	/// <summary>
	/// React property.
	/// </summary>
	public interface IReactiveProperty {

		/// <summary>
		/// Group name.
		/// </summary>
		string Group {
			get;
		}

		/// <summary>
		/// Raise property.
		/// </summary>
		void RaiseProperty ();

	}

}
