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

			public ReactiveProperty<string> StringProperty { get; set; }

			public ReactiveProperty<int> IntegerProperty { get; set; }

			public ParameterlessCommand FirstCommand { get; set; }

			public ParameterfulCommand<int> SecondCommand { get; set; }

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

	}

}
