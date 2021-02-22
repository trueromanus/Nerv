using System;
using System.Windows.Input;

namespace Nerv {

	/// <summary>
	/// Simple implementation for <see cref="ICommand"/> interface with supporting parameter.
	/// You can use if you command have parameter.
	/// </summary>
	public class ParameterfulCommand<T> : IReactiveCommand, ICommand, IDisposable {

		private readonly WeakReference<Action<T>> m_CommandAction;

		private readonly WeakReference<Func<T , bool>> m_CanExecute;

		private readonly string m_group;

		public event EventHandler CanExecuteChanged;

		public string Group => m_group;

		public ParameterfulCommand ( Action<T> commandAction , Func<T , bool> canExecute = null, string group = "" ) {
			if ( commandAction == null ) throw new ArgumentNullException ( nameof ( commandAction ) );

			m_CommandAction = new WeakReference<Action<T>> ( commandAction );

			if ( canExecute != null ) m_CanExecute = new WeakReference<Func<T , bool>> ( canExecute );

			m_group = group;
		}

		/// <summary>
		/// Compute new state for canExecute condition.
		/// </summary>
		public void RaiseCanExecuteChanged () => CanExecuteChanged?.Invoke ( this , EventArgs.Empty );

		public bool CanExecute ( object parameter ) {
			if ( m_CanExecute == null ) return true;

			if ( m_CanExecute.TryGetTarget ( out var canExecute ) ) return canExecute ( (T) parameter );

			return false;
		}

		public virtual void Execute ( object parameter ) {
			if ( !CanExecute ( parameter ) ) return;

			if ( m_CommandAction.TryGetTarget ( out var execute ) ) execute ( (T) parameter );
		}

		public void Dispose () {
			m_CommandAction.SetTarget ( null );
			m_CanExecute?.SetTarget ( null );
			CanExecuteChanged = null;
		}

	}

}