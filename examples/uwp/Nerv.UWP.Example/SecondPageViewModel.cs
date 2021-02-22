namespace Nerv.UWP.Example {

	public class SecondPageViewModel {

		public ReactiveProperty<string> Score { get; private set; }

		public ReactiveProperty<int> ScoreFirstPlayer { get; private set; }

		public ReactiveProperty<int> ScoreSecondPlayer { get; private set; }

		public ReactiveProperty<string> FirstPlayerName { get; private set; }

		public ReactiveProperty<string> SecondPlayerName { get; private set; }

		public ReactiveProperty<string> Result { get; private set; }

		public ParameterlessCommand StartGame { get; private set; }

		public ParameterlessCommand GoalToFirstPlayer { get; private set; }

		public ParameterlessCommand GoalToSecondPlayer { get; private set; }

		public ParameterlessCommand EndGame { get; private set; }

		private readonly ReactiveContext m_reactiveContext;

		public SecondPageViewModel () {
			// ReactiveContext allow you to do action for ReactiveProperty groups.
			m_reactiveContext = new ReactiveContext ();

			// group for displaying score, we have ScoreFirstPlayer and ScoreSecondPlayer numbers and Score for display it in format X : Y.
			// getCallback - callback if you need custom handle value before pass to ui, but passed value don't changed!
			Score = new ReactiveProperty<string> ( "" , group: "score_group" , getCallback: GenerateScore );
			ScoreFirstPlayer = new ReactiveProperty<int> ( 0 , group: "score_group" , raiseUiCallback: false );
			ScoreSecondPlayer = new ReactiveProperty<int> ( 0 , group: "score_group" , raiseUiCallback: false );
			
			Result = new ReactiveProperty<string> ( "" , getCallback: GetResult );

			FirstPlayerName = new ReactiveProperty<string> ( "First player" , getCallback: GetDisplayName );
			SecondPlayerName = new ReactiveProperty<string> ( "Last player" , getCallback: GetDisplayName );

			StartGame = new ParameterlessCommand ( startGame );
			// Command EndGame have group is score_group, which means that after we call m_reactiveContext.RaiseProperty for any property in this group, raise for canCommandExecute will be fired.
			EndGame = new ParameterlessCommand ( endGame, canEndGame, group: "score_group" );
			GoalToFirstPlayer = new ParameterlessCommand ( goalToFirstPlayer );
			GoalToSecondPlayer = new ParameterlessCommand ( goalToSecondPlayer );

			// this method attaches (or register if you will) all reactive properties and commands to m_reactiveContext, you don't need to do it yourself.
			m_reactiveContext.AttachAll ( this );
		}

		private bool canEndGame () {
			return ScoreFirstPlayer.Value > 0 && ScoreSecondPlayer.Value > 0;
		}

		private void endGame () {
			Result.SetValue ( ScoreFirstPlayer.Value > ScoreSecondPlayer.Value ? FirstPlayerName.Value : ( ScoreFirstPlayer.Value < ScoreSecondPlayer.Value ? SecondPlayerName.Value : "draw" ) );
		}

		private string GetResult ( string value ) {
			if ( value == "draw" ) return "Result is draw!";

			return $"Winner is {value}";
		}

		private void goalToFirstPlayer () {
			ScoreFirstPlayer.SetValue ( ( value ) => value + 1 );
			m_reactiveContext.RaiseProperty ( ScoreFirstPlayer );
		}

		private void goalToSecondPlayer () {
			ScoreSecondPlayer.SetValue ( ( value ) => value + 1 );
			m_reactiveContext.RaiseProperty ( ScoreSecondPlayer );
		}

		private string GenerateScore ( string value ) {
			return $"{ScoreFirstPlayer.Value} : {ScoreSecondPlayer.Value}";
		}

		private string GetDisplayName ( string value ) {
			return $"Player : {value}";
		}

		private void startGame () {
			Result.SetValue ( "" );

			// In this point we use Value = ... because we disable raiseUiCallback for these properties
			// it means that raiseProperty won't work automatically, need to call it manually
			// in this code using reactiveContext.RaiseAllProperties but it doesn't single one.
			ScoreFirstPlayer.Value = 0;
			ScoreSecondPlayer.Value = 0;

			// raise only property ScoreFirstPlayer, but because this property in same group as ScoreSecondPlayer and Score it will be raised too.
			m_reactiveContext.RaiseProperty ( ScoreFirstPlayer );
		}

	}

}
