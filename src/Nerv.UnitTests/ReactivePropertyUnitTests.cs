using System.ComponentModel;
using Xunit;

namespace Nerv.UnitTests {

	public class ReactivePropertyUnitTests {

		[Fact]
		public void Constructor_InitialValue () {
			//arrange
			//action
			var reactiveProperty = new ReactiveProperty<int> ( 200 );

			//assert
			Assert.Equal ( 200 , reactiveProperty.Value );
		}

		[Fact]
		public void Value_SetUICallback () {
			//arrange
			var result = false;
			var reactiveProperty = new ReactiveProperty<int> ( 200 , setUICallback: ( value ) => result = value == 300 );

			//action
			reactiveProperty.Value = 300;

			//assert
			Assert.True ( result );
		}

		[Fact]
		public void Value_RaiseUiCallback () {
			//arrange
			var result = false;
			var reactiveProperty = new ReactiveProperty<int> ( 200 , raiseUiCallback: true );
			reactiveProperty.PropertyChanged += ( object sender , PropertyChangedEventArgs e ) => {
				result = e.PropertyName == "Value";
			};

			//action
			reactiveProperty.Value = 300;

			//assert
			Assert.True ( result );
		}

		[Fact]
		public void Value_NotRaiseUiCallback () {
			//arrange
			var result = true;
			var reactiveProperty = new ReactiveProperty<int> ( 200 , raiseUiCallback: false );
			reactiveProperty.PropertyChanged += ( object sender , PropertyChangedEventArgs e ) => {
				result = false;
			};

			//action
			reactiveProperty.Value = 300;

			//assert
			Assert.True ( result );
		}

		[Fact]
		public void Value_SameValue () {
			//arrange
			var result = true;
			var reactiveProperty = new ReactiveProperty<int> ( 200 , raiseUiCallback: true );
			reactiveProperty.PropertyChanged += ( object sender , PropertyChangedEventArgs e ) => {
				result = false;
			};

			//action
			reactiveProperty.Value = 200;

			//assert
			Assert.True ( result );
		}

		[Fact]
		public void Value_GetCallback () {
			//arrange
			var reactiveProperty = new ReactiveProperty<int> ( 200 , getCallback: ( value ) => value + 5 );

			//action
			var value = reactiveProperty.Value;

			//assert
			Assert.Equal ( 205 , value );
		}

		[Fact]
		public void SetValue_SetCallback () {
			//arrange
			var result = false;
			var reactiveProperty = new ReactiveProperty<int> ( 200 , setCallback: ( value ) => result = value == 300 );

			//action
			reactiveProperty.SetValue ( 300 );

			//assert
			Assert.Equal ( 300 , reactiveProperty.Value );
			Assert.True ( result );
		}

		[Fact]
		public void SetValue_RaiseCallback () {
			//arrange
			var result = false;
			var reactiveProperty = new ReactiveProperty<int> ( 200 , raiseCallback: true );
			reactiveProperty.PropertyChanged += ( object sender , PropertyChangedEventArgs e ) => {
				result = e.PropertyName == "Value";
			};

			//action
			reactiveProperty.SetValue ( 300 );

			//assert
			Assert.True ( result );
		}

		[Fact]
		public void SetValue_SameValue () {
			//arrange
			var result = true;
			var reactiveProperty = new ReactiveProperty<int> ( 200 , raiseCallback: true );
			reactiveProperty.PropertyChanged += ( object sender , PropertyChangedEventArgs e ) => {
				result = false;
			};

			//action
			reactiveProperty.SetValue ( 200 );

			//assert
			Assert.True ( result );
		}

		[Fact]
		public void SetValueValueComputation_SetCallback () {
			//arrange
			var result = false;
			var reactiveProperty = new ReactiveProperty<int> ( 200 , setCallback: ( value ) => result = value == 300 );

			//action
			reactiveProperty.SetValue ( ( value ) => value + 100 );

			//assert
			Assert.Equal ( 300 , reactiveProperty.Value );
			Assert.True ( result );
		}

		[Fact]
		public void SetValueValueComputation_RaiseCallback () {
			//arrange
			var result = false;
			var reactiveProperty = new ReactiveProperty<int> ( 200 , raiseCallback: true );
			reactiveProperty.PropertyChanged += ( object sender , PropertyChangedEventArgs e ) => {
				result = e.PropertyName == "Value";
			};

			//action
			reactiveProperty.SetValue ( ( value ) => value + 100 );

			//assert
			Assert.True ( result );
		}

		[Fact]
		public void SetValueValueComputation_SameValue () {
			//arrange
			var result = true;
			var reactiveProperty = new ReactiveProperty<int> ( 200 , raiseCallback: true );
			reactiveProperty.PropertyChanged += ( object sender , PropertyChangedEventArgs e ) => {
				result = false;
			};

			//action
			reactiveProperty.SetValue ( 200 );

			//assert
			Assert.True ( result );
		}

		[Fact]
		public void RaiseProperty_HappyPath () {
			//arrange
			var result = false;
			var reactiveProperty = new ReactiveProperty<int> ( 0 );
			reactiveProperty.PropertyChanged += ( object sender , PropertyChangedEventArgs e ) => {
				result = e.PropertyName == "Value";
			};

			//action
			reactiveProperty.RaiseProperty ();

			//assert
			Assert.True ( result );
		}

	}

}
