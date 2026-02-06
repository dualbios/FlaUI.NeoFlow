using System.Windows;

namespace FlaUI.NeoFlow.TestApp;

public partial class MainWindow : Window {
    public MainWindow() {
        InitializeComponent();
    }

    private void CheckboxOnClick(object sender, RoutedEventArgs e) {
        MainContent.Content = new CheckboxesView();
        ViewNameTextBox.Text = "Checkboxes View";
    }

    private void TextOnClick(object sender, RoutedEventArgs e) {
        MainContent.Content = new TextsView();
        ViewNameTextBox.Text = "Texts View";
    }

    private void ButtonOnClick(object sender, RoutedEventArgs e) {
        MainContent.Content = new ButtonsView();
        ViewNameTextBox.Text = "Buttons View";
    }
}
