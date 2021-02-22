using System;
using System.ComponentModel;

namespace Nerv {

	/// <summary>
	/// The class takes responsibility for the reactivity single property.
	/// </summary>
	/// <typeparam name="T">Type of property value.</typeparam>
	public class ReactiveProperty<T> : INotifyPropertyChanged, IReactiveProperty {

		private T m_value;

		private readonly string m_group;

		private readonly WeakReference<Action<T>> m_setUICallback;

		private readonly WeakReference<Action<T>> m_setCallback;

		private readonly WeakReference<Func<T , T>> m_getCallback;

		private readonly bool m_raiseUiCallback;

		private readonly bool m_raiseCallback;

		/// <summary>
		/// Create reactive property.
		/// </summary>
		/// <param name="initialValue">Initial value.</param>
		/// <param name="setUICallback">Callback fires after value changed from ui in two way binding.</param>
		/// <param name="setCallback">Callback fires after value changed from code.</param>
		/// <param name="raiseUiCallback">Raise changes after value changed from ui in two way binding.</param>
		/// <param name="raiseCallback">Raise changes after value changed from code.</param>
		public ReactiveProperty ( T initialValue , Action<T> setUICallback = null , Action<T> setCallback = null , bool raiseUiCallback = true , bool raiseCallback = true , string group = "" , Func<T , T> getCallback = null ) {
			m_setUICallback = new WeakReference<Action<T>> ( setUICallback );
			m_setCallback = new WeakReference<Action<T>> ( setCallback );
			m_getCallback = new WeakReference<Func<T , T>> ( getCallback );
			m_raiseUiCallback = raiseUiCallback;
			m_raiseCallback = raiseCallback;
			m_value = initialValue;
			m_group = group;
		}

		/// <summary>
		/// Property group.
		/// </summary>
		public string Group => m_group;

		/// <summary>
		/// Current value.
		/// </summary>
		public T Value {
			get {
				if ( m_getCallback == null ) return m_value;

				if ( m_getCallback.TryGetTarget ( out var callback ) ) return callback ( m_value );

				//if getCallback already collected do fallback to normal version
				return m_value;
			}
			set {
				if ( m_value.Equals ( value ) ) return;
				m_value = value;

				if ( m_setUICallback.TryGetTarget ( out var callback ) ) callback ( value );
				if ( m_raiseUiCallback ) RaiseProperty ();
			}
		}

		/// <summary>
		/// Set value and raise callback if m_raiseCallback was set to true.
		/// </summary>
		/// <param name="value"></param>
		public void SetValue ( T value ) {
			if ( m_value.Equals ( value ) ) return;

			m_value = value;

			if ( m_setCallback.TryGetTarget ( out var callback ) ) callback ( value );
			if ( m_raiseCallback ) RaiseProperty ();
		}

		/// <summary>
		/// Set value and raise callback if m_raiseCallback was set to true.
		/// Value can be compute from callback valueComputation.
		/// </summary>
		/// <param name="valueComputation">Function for compute value based on current value (increment e.g.).</param>
		public void SetValue ( Func<T , T> valueComputation ) {
			if ( valueComputation == null ) throw new ArgumentNullException ( nameof ( valueComputation ) );
			var newValue = valueComputation ( m_value );
			
			if ( m_value.Equals ( newValue ) ) return;

			m_value = newValue;

			if ( m_setCallback.TryGetTarget ( out var callback ) ) callback ( m_value );
			if ( m_raiseCallback ) RaiseProperty ();
		}

		/// <summary>
		/// Force raise property.
		/// </summary>
		public void RaiseProperty () => PropertyChanged?.Invoke ( this , new PropertyChangedEventArgs ( "Value" ) );

		public event PropertyChangedEventHandler PropertyChanged;
	}

}
