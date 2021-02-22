using System;
using System.Windows.Input;

namespace Nerv {

	/// <summary>
	/// Simple implementation for <see cref="ICommand"/> interface.
	/// You can use if you don't have command parameter.
	/// </summary>
	public sealed class ParameterlessCommand : IReactiveCommand, ICommand, IDisposable {

		private readonly WeakReference<Action> m_CommandAction;

		private readonly WeakReference<Func<bool>> m_CanExecute;

		private readonly string m_group;

		public event EventHandler CanExecuteChanged;

		public string Group => m_group;

		public ParameterlessCommand ( Action commandAction , Func<bool> canExecute = null , string group = "" ) {
			if ( commandAction == null ) throw new ArgumentNullException ( nameof ( commandAction ) );

			m_CommandAction = new WeakReference<Action> ( commandAction );

			if ( canExecute != null ) m_CanExecute = new WeakReference<Func<bool>> ( canExecute );

			m_group = group;
		}

		public void RaiseCanExecuteChanged () => CanExecuteChanged?.Invoke ( this , EventArgs.Empty );

		public bool CanExecute ( object parameter ) {
			if ( m_CanExecute == null ) return true;

			if ( m_CanExecute.TryGetTarget ( out var canExecute ) ) return canExecute ();

			return false;
		}

		public void Execute ( object parameter ) {
			if ( !CanExecute ( parameter ) ) return;

			if ( m_CommandAction.TryGetTarget ( out var execute ) ) execute ();
		}

		public void Dispose () {
			m_CommandAction.SetTarget ( null );
			m_CanExecute?.SetTarget ( null );
			CanExecuteChanged = null;
		}

	}

}