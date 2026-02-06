using FlaUI.Core.AutomationElements;
using FlaUI.NeoFlow.Script;

namespace FlaUI.NeoFlow.Selector;

public class UiElement {
    protected readonly SelectorEngine _engine;

    internal UiElement(AutomationElement element, SelectorEngine engine) {
        _engine = engine;
        Element = element;
    }

    public AutomationElement Element { get; private set; }

    public void Click() {
        Element.Click();
    }

    public void SetFocus() {
        Element.Focus();
    }

    public string GetText() {
        return Element.Name;
    }

    public bool IsEnabled() {
        return Element.IsEnabled;
    }

    public UiElement? Find(string selector) {
        Script.Selector parsed = new SelectorParser().Parse(selector);
        AutomationElement? element = _engine.Find(Element, parsed);

        return element == null ? null : new UiElement(element, _engine);
    }

    public IEnumerable<UiElement> FindAll(string selector) {
        Script.Selector parsed = new SelectorParser().Parse(selector);
        IEnumerable<AutomationElement> elements = _engine.FindAll(Element, parsed);
        return elements.Select(e => new UiElement(e, _engine)).ToList();
    }

    public UiButton AsButton() {
        return new UiButton(Element, _engine);
    }

    public UiCheckBox AsCheckBox() {
        return new UiCheckBox(Element, _engine);
    }
}
