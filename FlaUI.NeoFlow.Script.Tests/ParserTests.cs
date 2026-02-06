namespace FlaUI.NeoFlow.Script.Tests;

[TestClass]
public sealed class ParserTests {
    [TestMethod]
    public void WindowsContainsButtonAsChildTest() {
        SelectorParser parser = new ();

        Selector selector = parser.Parse("Window[name='Login'] > Button[text~='OK']");

        Assert.HasCount(2, selector.Steps);

        SelectorStep step0 = selector.Steps.ElementAt(0);
        Assert.AreEqual("Window", step0.ControlType);
        Assert.HasCount(1, step0.Filters);

        Filter step0Filter = step0.Filters.ElementAt(0);
        Assert.AreEqual("name", step0Filter.Name);
        Assert.AreEqual(Operator.Equals, step0Filter.Operator);
        Assert.AreEqual("Login", step0Filter.Value);
        Assert.AreEqual(Scope.Children, step0.Scope);

        SelectorStep step1 = selector.Steps.ElementAt(1);
        Assert.AreEqual("Button", step1.ControlType);
        Assert.HasCount(1, step1.Filters);

        Filter step1Filter = step1.Filters.ElementAt(0);
        Assert.AreEqual("text", step1Filter.Name);
        Assert.AreEqual(Operator.Contains, step1Filter.Operator);
        Assert.AreEqual("OK", step1Filter.Value);

    }

    [TestMethod]
    public void WindowsContainsButtonAsDescendantTest() {
        SelectorParser parser = new ();

        Selector selector = parser.Parse("Window[name='Login'] >> Button[text~='OK']");

        Assert.HasCount(2, selector.Steps);

        SelectorStep step0 = selector.Steps.ElementAt(0);
        Assert.AreEqual("Window", step0.ControlType);
        Assert.HasCount(1, step0.Filters);

        Filter step0Filter = step0.Filters.ElementAt(0);
        Assert.AreEqual("name", step0Filter.Name);
        Assert.AreEqual(Operator.Equals, step0Filter.Operator);
        Assert.AreEqual("Login", step0Filter.Value);
        Assert.AreEqual(Scope.Descendants, step0.Scope);

        SelectorStep step1 = selector.Steps.ElementAt(1);
        Assert.AreEqual("Button", step1.ControlType);
        Assert.HasCount(1, step1.Filters);

        Filter step1Filter = step1.Filters.ElementAt(0);
        Assert.AreEqual("text", step1Filter.Name);
        Assert.AreEqual(Operator.Contains, step1Filter.Operator);
        Assert.AreEqual("OK", step1Filter.Value);
    }

    [TestMethod]
    public void FilterOperatorEqualsEqualTest() {
        SelectorParser parser = new ();

        Selector selector = parser.Parse("Window[name='Login']");

        Assert.HasCount(1, selector.Steps);

        SelectorStep step0 = selector.Steps.ElementAt(0);
        Assert.HasCount(1, step0.Filters);

        Filter filter0 = step0.Filters.ElementAt(0);
        Assert.AreEqual("name", filter0.Name);
        Assert.AreEqual(Operator.Equals, filter0.Operator);
        Assert.AreEqual("Login", filter0.Value);
    }

    [TestMethod]
    public void FilterOperatorEqualsNotEqualsTest() {
        SelectorParser parser = new ();

        Selector selector = parser.Parse("Window[name!='Login']");

        Assert.HasCount(1, selector.Steps);

        SelectorStep step0 = selector.Steps.ElementAt(0);
        Assert.HasCount(1, step0.Filters);

        Filter filter0 = step0.Filters.ElementAt(0);
        Assert.AreEqual("name", filter0.Name);
        Assert.AreEqual(Operator.NotEquals, filter0.Operator);
        Assert.AreEqual("Login", filter0.Value);
    }

    [TestMethod]
    public void FilterOperatorEqualsContainsTest() {
        SelectorParser parser = new ();

        Selector selector = parser.Parse("Window[name~='Login']");

        Assert.HasCount(1, selector.Steps);

        SelectorStep step0 = selector.Steps.ElementAt(0);
        Assert.HasCount(1, step0.Filters);

        Filter filter0 = step0.Filters.ElementAt(0);
        Assert.AreEqual("name", filter0.Name);
        Assert.AreEqual(Operator.Contains, filter0.Operator);
        Assert.AreEqual("Login", filter0.Value);
    }

    [TestMethod]
    public void OperatorValueContainNumericTest() {
        SelectorParser parser = new ();

        Selector selector = parser.Parse("Window[index=3]");

        Assert.HasCount(1, selector.Steps);

        SelectorStep step0 = selector.Steps.ElementAt(0);
        Assert.HasCount(1, step0.Filters);

        Filter filter0 = step0.Filters.ElementAt(0);
        Assert.AreEqual("index", filter0.Name);
        Assert.AreEqual(Operator.Equals, filter0.Operator);
        Assert.AreEqual("3", filter0.Value);
    }
}
