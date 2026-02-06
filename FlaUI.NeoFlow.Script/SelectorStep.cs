namespace FlaUI.NeoFlow.Script;

public record SelectorStep(string ControlType, Scope Scope, IEnumerable<Filter> Filters);
