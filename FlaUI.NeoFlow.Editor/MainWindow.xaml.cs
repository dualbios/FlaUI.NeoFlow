using System.IO;
using System.Windows;
using FlaUI.Core.Conditions;
using FlaUI.NeoFlow.Selector;
using FlaUI.UIA3;
using Application = FlaUI.Core.Application;
using Window = System.Windows.Window;

namespace FlaUI.NeoFlow.Editor;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window {
    public MainWindow() {
        InitializeComponent();
    }

    private void ScriptTestOnClick(object sender, RoutedEventArgs e) {
        Application application = Application.Attach(int.Parse(ProcessIdTextBox.Text));
        UIA3Automation automationBase = new ();
        Core.AutomationElements.Window mainWindow = application.GetMainWindow(automationBase)
                                                    ?? throw new NullReferenceException("Main window not found.");

        ConditionFactory cf = new (new UIA3PropertyLibrary());

        ScriptRunner scriptRunner = new (cf, application, automationBase);
        string scriptPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "script.lua");
        string script = File.ReadAllText(scriptPath);
        scriptRunner.Run(script);
    }
}
