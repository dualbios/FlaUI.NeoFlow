using FlaUI.Core;
using FlaUI.Core.Conditions;
using FlaUI.UIA3;
using MoonSharp.Interpreter;
using AutomationElement = FlaUI.Core.AutomationElements.AutomationElement;

namespace FlaUI.NeoFlow.Selector;

[MoonSharpUserData]
public class UiApi(ConditionFactory cf, Application application, UIA3Automation automation) {
    private readonly SelectorEngine _engine = new (cf, application, automation);

    public UiWindow? GetMainWindow() {
        AutomationElement? element = _engine.GetMainWindow();

        return element == null ? null : new UiWindow(element, _engine);
    }

    // public UiElement Find(string selector)
    // {
    //     Script.Selector parsed = new SelectorParser().Parse(selector);
    //     AutomationElement? element = _engine.Find(parsed);
    //
    //     if (element == null)
    //         throw new ScriptRuntimeException($"Element not found: {selector}");
    //
    //     return new UiElement(element, _engine);
    // }
    //
    // public bool Exists(string selector)
    // {
    //     Script.Selector parsed = new SelectorParser().Parse(selector);
    //     return _engine.Find(parsed) != null;
    // }

    public void Wait(int milliseconds) {
        Thread.Sleep(milliseconds);
    }

    public void Log(string message) {
        Console.WriteLine("[Lua] " + message);
    }
}
