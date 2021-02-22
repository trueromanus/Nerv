using System;

namespace Nerv.UWP.Example {

	public class MainPageViewModel {

		public ReactiveProperty<string> MakedActions { get; private set; }

		public ReactiveProperty<string> OneWayText { get; private set; }

		public ParameterlessCommand ChangeOneWay { get; private set; }

		public ReactiveProperty<string> TwoWayText { get; private set; }

		public ParameterlessCommand ClearLog { get; private set; }

		public MainPageViewModel () {
			MakedActions = new ReactiveProperty<string> ( "", setCallback: (value) => ClearLog.RaiseCanExecuteChanged() );

			// example if you don't have ReactiveContext, raiseCallback = true means that after call SetValue raiseProperty will be fired.
			OneWayText = new ReactiveProperty<string> (
				"",
				//optional callback if you need handle SetValue situation and make additional logic
				setCallback: (value) => MakedActions.SetValue(
					(currentValue) => currentValue += "OneWayText changed: " + value + "\n"
				)
			);
			ChangeOneWay = new ParameterlessCommand ( changeOneWay );

			// example if you don't have ReactiveContext, raiseUiCallback = true means that after set value from user (TwoWayText.Value = ...) raiseProperty will be fired.
			TwoWayText = new ReactiveProperty<string> (
				"" ,
				//optional callback if you need handle Value = ... situation and make additional logic
				setUICallback: ( value ) => MakedActions.SetValue (
					( currentValue ) => currentValue += "TwoWayText changed: " + value + "\n"
				)
			);

			ClearLog = new ParameterlessCommand ( clearLog , canClearLog );
		}

		private bool canClearLog () => !string.IsNullOrEmpty ( MakedActions.Value );

		private void clearLog () => MakedActions.SetValue ( "" );

		private void changeOneWay () {
			OneWayText.SetValue ( "New text!!!" + DateTime.Now.Second ); // if you don't use ReactiveContext you can use this way for setting value
		}

	}

}
