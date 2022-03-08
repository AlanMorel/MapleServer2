using MapleServer2.Tools;
using NUnit.Framework;

namespace MapleServer2.Tests.Tools;

public sealed class ConditionHelperTests
{
    [Test]
    public void IsMatching_returns_true_for_exact_string_match()
    {
        Assert.That(ConditionHelper.IsMatching("1234", "1234"), Is.True);
    }

    [Test]
    public void IsMatching_returns_true_regardless_of_case()
    {
        Assert.That(ConditionHelper.IsMatching("foobar", "FoObAr"), Is.True);
    }

    [Test]
    public void IsMatching_returns_false_for_non_matching_string()
    {
        Assert.That(ConditionHelper.IsMatching("not", "a-match"), Is.False);
    }

    [Test]
    public void IsMatching_returns_true_for_exact_number_match()
    {
        Assert.That(ConditionHelper.IsMatching("1234", 1234), Is.True);
    }

    [Test]
    public void IsMatching_returns_true_for_condition_with_leading_zeroes()
    {
        Assert.That(ConditionHelper.IsMatching("000001234", 1234), Is.True);
    }

    [Test]
    public void IsMatching_returns_false_for_condition_with_trailing_zeroes()
    {
        Assert.That(ConditionHelper.IsMatching("123400000", 1234), Is.False);
    }

    private static IEnumerable<TestCaseData> List_test_cases()
    {
        yield return new TestCaseData("1,2,3", 1);
        yield return new TestCaseData("1,2,3", 2);
        yield return new TestCaseData("1,2,3", 3);
    }

    [TestCaseSource(nameof(List_test_cases))]
    public void IsMatching_returns_true_for_value_in_list(string conditionList, int match)
    {
        Assert.That(ConditionHelper.IsMatching(conditionList, match), Is.True);
    }

    [Test]
    public void IsMatching_returns_false_for_value_not_in_list()
    {
        Assert.That(ConditionHelper.IsMatching("1,2,3", 4), Is.False);
    }

    private static IEnumerable<TestCaseData> Range_test_cases()
    {
        yield return new TestCaseData("1-3", 1);
        yield return new TestCaseData("1-3", 2);
        yield return new TestCaseData("1-3", 3);
    }

    [TestCaseSource(nameof(Range_test_cases))]
    public void IsMatching_returns_true_for_value_in_range(string conditionRange, int match)
    {
        Assert.That(ConditionHelper.IsMatching(conditionRange, match), Is.True);
    }

    [Test]
    public void IsMatching_returns_false_for_value_outside_range()
    {
        Assert.That(ConditionHelper.IsMatching("1-3", 4), Is.False);
    }
}
