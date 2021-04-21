namespace Blazored.LocalStorage.Tests.TestAssets
{
    public class TestObject
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public TestObject() { }
        public TestObject(int id, string name)
        {
            Id = id;
            Name = name;
        }
    }
}
