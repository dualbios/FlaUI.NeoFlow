using FlaUI.Core;
using FlaUI.Core.AutomationElements;
using FlaUI.Core.Conditions;
using FlaUI.Core.Definitions;
using FlaUI.NeoFlow.Script;
using FlaUI.UIA3;

namespace FlaUI.NeoFlow.Selector;

public class SelectorEngine(ConditionFactory cf, Application application, UIA3Automation automation) {
    private readonly Application _application = application ?? throw new ArgumentNullException(nameof(application));

    public Window? GetMainWindow() {
        return _application.GetMainWindow(automation);
    }

    public AutomationElement? Find(AutomationElement element, Script.Selector selector) {
        IEnumerable<AutomationElement> elements = FindAll(element, selector);
        return elements.FirstOrDefault();
    }

    public IEnumerable<AutomationElement> FindAll(AutomationElement element, Script.Selector selector) {
        IEnumerable<AutomationElement> currentSet = [element];

        foreach (SelectorStep step in selector.Steps) {
            currentSet = currentSet.SelectMany(parent => {
                TreeScope scope = step.Scope == Scope.Children
                    ? TreeScope.Children
                    : TreeScope.Descendants;
                ConditionBase condition = BuildCondition(step);
                IEnumerable<AutomationElement> found = parent.FindAll(scope, condition)
                                                             .Where(e => StepFilterPredicate(e, step)); // contains / in-memory
                return found;
            });
        }

        return currentSet.ToList();
    }

    /// <summary>
    /// Build a UIA Condition for AutomationElement based on step filters
    /// </summary>
    private ConditionBase BuildCondition(SelectorStep step) {
        List<ConditionBase> conditions = [];

        // ControlType
        if (!string.IsNullOrEmpty(step.ControlType) && step.ControlType != "*") {
            conditions.Add(cf.ByControlType(ControlTypeFromString(step.ControlType)));
        }

        // Filters
        foreach (Filter filter in step.Filters.Where(f => f.Operator is Operator.Equals or Operator.NotEquals)) {
            ConditionBase? cond = filter.Name.ToLower() switch {
                "name" => cf.ByName(filter.Value),
                "automationid" => cf.ByAutomationId(filter.Value),
                "uid" => cf.ByAutomationId(filter.Value),
                "class" => cf.ByClassName(filter.Value),
                "processid" => cf.ByProcessId(int.Parse(filter.Value)),
                _ => null
            };

            if (cond != null) {
                if (filter.Operator == Operator.NotEquals) {
                    cond = new NotCondition(cond);
                }
                conditions.Add(cond);
            }
        }

        if (conditions.Count == 0) {
            return TrueCondition.Default;
        }

        if (conditions.Count == 1) {
            return conditions[0];
        }

        return new AndCondition(conditions.ToArray());
    }

    /// <summary>
    /// In-memory filter for contains operator (~=)
    /// </summary>
    private bool StepFilterPredicate(AutomationElement element, SelectorStep step) {
        foreach (Filter filter in step.Filters.Where(f => f.Operator == Operator.Contains)) {
            string value = filter.Name.ToLower() switch {
                "name" => element.Name ?? "",
                "automationid" => element.AutomationId ?? "",
                "uid" => element.AutomationId ?? "",
                "class" => element.ClassName ?? "",
                _ => ""
            };

            if (!value.Contains(filter.Value, StringComparison.OrdinalIgnoreCase)) {
                return false;
            }
        }

        return true;
    }

    /// <summary>
    /// Map string like "Button" to UIA ControlType
    /// </summary>
    private ControlType ControlTypeFromString(string controlType) {
        return controlType.ToLower() switch {
            "button" => ControlType.Button,
            "edit" => ControlType.Edit,
            "text" => ControlType.Text,
            "window" => ControlType.Window,
            "pane" => ControlType.Pane,
            "list" => ControlType.List,
            "listitem" => ControlType.ListItem,
            "checkbox" => ControlType.CheckBox,
            "element" => ControlType.Custom,
            _ => ControlType.Custom
        };
    }
}
