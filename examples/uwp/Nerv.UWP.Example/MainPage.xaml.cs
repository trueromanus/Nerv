using System;
using Windows.UI.Xaml.Controls;

namespace Nerv.UWP.Example {

	public sealed partial class MainPage : Page {

		public MainPage () {
			InitializeComponent ();
			DataContext = new MainPageViewModel ();
		}

		private void Button_Click ( object sender , Windows.UI.Xaml.RoutedEventArgs e ) {
			Frame.Navigate ( typeof ( SecondPage ) );
		}

		private void Page_Unloaded ( object sender , Windows.UI.Xaml.RoutedEventArgs e ) {
			DataContext = null;
		}
	}

}
