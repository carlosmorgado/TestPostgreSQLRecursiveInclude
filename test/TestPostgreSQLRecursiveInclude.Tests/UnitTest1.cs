using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Shouldly;
using Xunit;

namespace TestPostgreSQLRecursiveInclude.Tests
{
    public class UnitTest1
    {
        [Fact]
        public async System.Threading.Tasks.Task Test1Async()
        {
            // Arrange
            var ctx = new TestPostgreSQLDbConstext();

            var testClassParent = new TestClass
            {
                Id = Guid.NewGuid()
            };
            var testClass = new TestClass
            {
                Id = Guid.NewGuid()
            };
            var testClassChild = new TestClass
            {
                Id = Guid.NewGuid()
            };

            ctx.TestClasses.Add(testClassParent);
            ctx.TestClasses.Add(testClass);
            ctx.TestClasses.Add(testClassChild);
            await ctx.SaveChangesAsync();

            var parentLink = new TestClassLink
            {
                Id = Guid.NewGuid(),
                FromTestClass = testClassParent,
                ToTestClass = testClass
            };

            var childLink = new TestClassLink
            {
                Id = Guid.NewGuid(),
                FromTestClass = testClass,
                ToTestClass = testClassChild
            };

            testClassParent.Parents = new List<TestClassLink>();
            testClassParent.Children = new List<TestClassLink> { parentLink };
            testClass.Parents = new List<TestClassLink> { parentLink };
            testClass.Children = new List<TestClassLink> { childLink };
            testClassChild.Parents = new List<TestClassLink> { childLink };
            testClassChild.Children = new List<TestClassLink>();

            ctx.TestClassLinks.Add(parentLink);
            ctx.TestClassLinks.Add(childLink);
            await ctx.SaveChangesAsync();

            var expected = new List<TestClass> { testClassParent, testClass, testClassChild };
            var ids = expected.Select(tc => tc.Id);

            // Act
            var result = await ctx
                .TestClasses
                .Include(tc => tc.Parents)
                    .ThenInclude(tcl => tcl.FromTestClass)
                .Include(tc => tc.Children)
                    .ThenInclude(tcl => tcl.ToTestClass)
                .Where(tc => ids.Contains(tc.Id))
                .ToListAsync();

            // Assert
            result.ShouldBe(expected, ignoreOrder: true);

            // Clean up
            ctx.TestClasses.Remove(testClassParent);
            ctx.TestClasses.Remove(testClass);
            ctx.TestClasses.Remove(testClassChild);
            ctx.TestClassLinks.Remove(parentLink);
            ctx.TestClassLinks.Remove(childLink);
            await ctx.SaveChangesAsync();
        }
    }
}
