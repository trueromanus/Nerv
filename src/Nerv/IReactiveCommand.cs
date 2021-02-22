namespace Nerv {

	/// <summary>
	/// Reactive command.
	/// </summary>
	public interface IReactiveCommand {

		/// <summary>
		/// Group name.
		/// </summary>
		string Group {
			get;
		}

		/// <summary>
		/// Compute new state for canExecute condition.
		/// </summary>
		void RaiseCanExecuteChanged ();

	}

}
