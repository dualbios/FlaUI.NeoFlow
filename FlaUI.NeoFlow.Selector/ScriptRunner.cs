using FlaUI.Core;
using FlaUI.Core.Conditions;
using FlaUI.UIA3;
using MoonSharp.Interpreter;

namespace FlaUI.NeoFlow.Selector;

public class ScriptRunner(ConditionFactory cf, Application application, UIA3Automation automationBase) {
    public void Run(string script) {
        UserData.RegisterType<UiApi>();
        UserData.RegisterType<UiElement>();

        // ReSharper disable once UseObjectOrCollectionInitializer
        MoonSharp.Interpreter.Script lua = new (CoreModules.Preset_SoftSandbox);
        lua.Globals["UI"] = new UiApi(cf, application, automationBase);

        lua.DoString(script);
    }
}
