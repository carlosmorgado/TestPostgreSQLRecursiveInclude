using System;
using System.Collections.Generic;

namespace TestPostgreSQLRecursiveInclude
{
    public class TestClass
    {
        public Guid Id { get; set; }

        public ICollection<TestClassLink> Parents { get; set; }

        public ICollection<TestClassLink> Children { get; set; }
    }
}
