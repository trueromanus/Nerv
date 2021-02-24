using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Nerv {

	/// <summary>
	/// Reactive context.
	/// </summary>
	public class ReactiveContext {

		private List<WeakReference<IReactiveProperty>> m_reactiveProperties = new List<WeakReference<IReactiveProperty>> ();

		private List<WeakReference<IReactiveCommand>> m_reactiveCommands = new List<WeakReference<IReactiveCommand>> ();

		/// <summary>
		/// Attach reactive property.
		/// </summary>
		/// <param name="reactiveProperty">Reactive property.</param>
		/// <exception cref="ArgumentNullException"></exception>
		public void Attach ( IReactiveProperty reactiveProperty ) {
			if ( reactiveProperty == null ) throw new ArgumentNullException ( nameof ( reactiveProperty ) );

			m_reactiveProperties.Add ( new WeakReference<IReactiveProperty> ( reactiveProperty ) );
		}

		/// <summary>
		/// Attach reactive command.
		/// </summary>
		/// <param name="reactiveCommand">Reactive command.</param>
		/// <exception cref="ArgumentNullException"></exception>
		public void Attach ( IReactiveCommand reactiveCommand ) {
			if ( reactiveCommand == null ) throw new ArgumentNullException ( nameof ( reactiveCommand ) );

			m_reactiveCommands.Add ( new WeakReference<IReactiveCommand> ( reactiveCommand ) );
		}

		/// <summary>
		/// Attach all reactive properties and commands from targetObject.
		/// </summary>
		/// <typeparam name="T">Target object type.</typeparam>
		/// <param name="targetObject">
		/// Usually it ViewModel class that have properties with types ReactiveProperty`1, ParameterlessCommand or ParameterfulCommand.
		/// All reactive properties and commands automatically will be added to reactive context.
		/// </param>
		/// <exception cref="ArgumentNullException"></exception>
		public void AttachAll<T> ( T targetObject ) {
			if ( targetObject == null ) throw new ArgumentNullException ( nameof ( targetObject ) );

			var properties = targetObject
				.GetType ()
				.GetTypeInfo ()
				.GetProperties ()
				.Where (
					a => a
						.PropertyType
						.GetInterfaces ()
						.Contains ( typeof ( IReactiveProperty ) ) ||
						a.PropertyType == typeof ( IReactiveProperty )
				);

			foreach ( var property in properties ) {
				var reactiveProperty = (IReactiveProperty) property.GetMethod?.Invoke ( targetObject , null );
				if ( reactiveProperty == null ) continue;

				Attach ( reactiveProperty );
			}

			var commands = targetObject
				.GetType ()
				.GetTypeInfo ()
				.GetProperties ()
				.Where (
					a => a
						.PropertyType
						.GetInterfaces ()
						.Contains ( typeof ( IReactiveCommand ) ) ||
						a.PropertyType == typeof ( IReactiveCommand )
				);

			foreach ( var command in commands ) {
				var reactiveCommand = (IReactiveCommand) command.GetMethod?.Invoke ( targetObject , null );
				if ( reactiveCommand == null ) continue;

				Attach ( reactiveCommand );
			}
		}

		/// <summary>
		/// Raise specified reactive property.
		/// </summary>
		/// <param name="reactiveProperty">Reactive property that will be raised.</param>
		/// <param name="raiseRelated">If true then after raising the property itself it finds related properties by Group property and raises it also.</param>
		public void RaiseProperty ( IReactiveProperty reactiveProperty , bool raiseRelated = true ) {
			reactiveProperty.RaiseProperty ();

			if ( !raiseRelated ) return;

			var groupProperties = m_reactiveProperties.Where ( a => CheckGroup ( a , reactiveProperty ) );
			foreach ( var groupProperty in groupProperties ) {
				if ( groupProperty.TryGetTarget ( out var groupPropertyTarget ) ) groupPropertyTarget.RaiseProperty ();
			}

			var commands = m_reactiveCommands.Where ( a => CheckCommandGroup ( reactiveProperty.Group , a ) );
			foreach ( var command in commands ) {
				if ( command.TryGetTarget ( out var commandTarget ) ) commandTarget.RaiseCanExecuteChanged ();
			}

			bool CheckGroup ( WeakReference<IReactiveProperty> weakReference , IReactiveProperty originReactiveProperty ) {
				if ( weakReference.TryGetTarget ( out var property ) ) {
					if ( property == originReactiveProperty ) return false;

					return property.Group == originReactiveProperty.Group;
				}

				return false;
			}

			bool CheckCommandGroup ( string group , WeakReference<IReactiveCommand> weakReference ) {
				if ( weakReference.TryGetTarget ( out var command ) ) return command.Group == group;

				return false;
			}

		}

		/// <summary>
		/// Raise all attached properties.
		/// </summary>
		public void RaiseAllProperties () {
			foreach ( var reactiveProperty in m_reactiveProperties ) {
				if ( reactiveProperty.TryGetTarget ( out var groupPropertyTarget ) ) groupPropertyTarget.RaiseProperty ();
			}
		}

		/// <summary>
		/// Raise canExecute for all commands.
		/// </summary>
		public void RaiseCanExecuteCommands () {
			foreach ( var reactiveCommand in m_reactiveCommands ) {
				if ( reactiveCommand.TryGetTarget ( out var command ) ) command.RaiseCanExecuteChanged ();
			}
		}

	}

}
