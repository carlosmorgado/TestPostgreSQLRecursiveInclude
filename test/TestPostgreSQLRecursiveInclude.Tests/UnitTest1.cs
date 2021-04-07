using System;
using System.Collections.Generic;
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

            testClassParent.Children = new List<TestClassLink> { parentLink };
            testClass.Parents = new List<TestClassLink> { parentLink };
            testClass.Children = new List<TestClassLink> { childLink };
            testClassChild.Parents = new List<TestClassLink> { childLink };

            ctx.TestClasses.Add(testClassParent);
            await ctx.SaveChangesAsync();

            // Act
            var result = await ctx
                .TestClasses
                .Include(tc => tc.Parents)
                    .ThenInclude(tcl => tcl.FromTestClass)
                .Include(tc => tc.Children)
                    .ThenInclude(tcl => tcl.ToTestClass)
                .FirstOrDefaultAsync();

            // Assert
            result.ShouldBeSameAs(testClassParent);

            // Clean up
        }
    }
}
