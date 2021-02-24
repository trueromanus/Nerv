using FakeItEasy;
using System;
using Xunit;

namespace Nerv.UnitTests {

	public class ReactiveContextUnitTests {

		[Fact]
		public void Attach_ReactiveProperty_Null () {
			//arrange
			var context = new ReactiveContext ();
			ReactiveProperty<bool> property = null;

			//assert
			Assert.Throws<ArgumentNullException> (
				() => {
					//action
					context.Attach ( property );
				}
			);
		}

		[Fact]
		public void Attach_ReactiveCommand_Null () {
			//arrange
			var context = new ReactiveContext ();
			ParameterlessCommand command = null;

			//assert
			Assert.Throws<ArgumentNullException> (
				() => {
					//action
					context.Attach ( command );
				}
			);
		}

		private class TestHost {

			public IReactiveProperty StringProperty { get; set; }

			public IReactiveProperty IntegerProperty { get; set; }

			public IReactiveCommand FirstCommand { get; set; }

			public IReactiveCommand SecondCommand { get; set; }

		}

		[Fact]
		public void AttachAll_Null () {
			//arrange
			var context = new ReactiveContext ();
			TestHost host = null;

			//assert
			Assert.Throws<ArgumentNullException> (
				() => {
					//action
					context.AttachAll ( host );
				}
			);
		}

		[Fact]
		public void AttachAll_HappyPath () {
			//arrange
			var stringProperty = false;
			var integerProperty = false;
			var firstCommand = false;
			var secondCommand = false;
			var context = new ReactiveContext ();
			TestHost host = new TestHost {
				StringProperty = A.Fake<IReactiveProperty> () ,
				IntegerProperty = A.Fake<IReactiveProperty> () ,
				FirstCommand = A.Fake<IReactiveCommand> () ,
				SecondCommand = A.Fake<IReactiveCommand> () ,
			};
			A.CallTo ( () => host.StringProperty.RaiseProperty () ).Invokes ( () => stringProperty = true );
			A.CallTo ( () => host.IntegerProperty.RaiseProperty () ).Invokes ( () => integerProperty = true );
			A.CallTo ( () => host.FirstCommand.RaiseCanExecuteChanged () ).Invokes ( () => firstCommand = true );
			A.CallTo ( () => host.SecondCommand.RaiseCanExecuteChanged () ).Invokes ( () => secondCommand = true );

			//action
			context.AttachAll ( host );

			//assert
			context.RaiseAllProperties ();
			context.RaiseCanExecuteCommands ();
			Assert.True ( stringProperty );
			Assert.True ( integerProperty );
			Assert.True ( firstCommand );
			Assert.True ( secondCommand );
		}

		[Fact]
		public void AttachAll_Properties_And_Commands_Null () {
			//arrange
			var context = new ReactiveContext ();
			TestHost host = new TestHost ();

			//action
			context.AttachAll ( host );

			//assert
			context.RaiseAllProperties ();
			context.RaiseCanExecuteCommands ();
			Assert.True ( true );
		}

		[Fact]
		public void RaiseAllProperties_HappyPath () {
			//arrange
			var stringProperty = false;
			var integerProperty = false;
			var context = new ReactiveContext ();
			TestHost host = new TestHost {
				StringProperty = A.Fake<IReactiveProperty> () ,
				IntegerProperty = A.Fake<IReactiveProperty> () ,
			};
			A.CallTo ( () => host.StringProperty.RaiseProperty () ).Invokes ( () => stringProperty = true );
			A.CallTo ( () => host.IntegerProperty.RaiseProperty () ).Invokes ( () => integerProperty = true );

			//action
			context.AttachAll ( host );

			//assert
			context.RaiseAllProperties ();
			context.RaiseCanExecuteCommands ();
			Assert.True ( stringProperty );
			Assert.True ( integerProperty );
		}

		[Fact]
		public void RaiseCanExecuteCommands_HappyPath () {
			//arrange
			var firstCommand = false;
			var secondCommand = false;
			var context = new ReactiveContext ();
			TestHost host = new TestHost {
				FirstCommand = A.Fake<IReactiveCommand> () ,
				SecondCommand = A.Fake<IReactiveCommand> ()
			};
			A.CallTo ( () => host.FirstCommand.RaiseCanExecuteChanged () ).Invokes ( () => firstCommand = true );
			A.CallTo ( () => host.SecondCommand.RaiseCanExecuteChanged () ).Invokes ( () => secondCommand = true );

			//action
			context.AttachAll ( host );

			//assert
			context.RaiseAllProperties ();
			context.RaiseCanExecuteCommands ();
			Assert.True ( firstCommand );
			Assert.True ( secondCommand );
		}

	}

}
