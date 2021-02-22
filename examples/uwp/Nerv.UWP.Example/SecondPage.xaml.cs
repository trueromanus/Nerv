using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Nerv.UWP.Example {

	public sealed partial class SecondPage : Page {
		public SecondPage () {
			InitializeComponent ();
			DataContext = new SecondPageViewModel ();
		}

		private void Button_Click ( object sender , RoutedEventArgs e ) {
			Frame.Navigate ( typeof ( MainPage ) );
		}

		private void Page_Unloaded ( object sender , RoutedEventArgs e ) {
			DataContext = null;
		}
	}
}
