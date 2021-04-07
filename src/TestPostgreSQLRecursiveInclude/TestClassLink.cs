using System;

namespace TestPostgreSQLRecursiveInclude
{
    public class TestClassLink
    {
        public Guid Id { get; set; }

        public Guid FromTestClassId { get; set; }

        public TestClass FromTestClass { get; set; }

        public Guid ToTestClassId { get; set; }

        public TestClass ToTestClass { get; set; }
    }
}
