namespace CRUDTests
{
    public class UnitTest1
    {
        [Fact]
        public void Test1()
        {
            //Arrange
            MyTest mm = new MyTest();
            int input1 = 10, input2 = 5;
            int expected = 15;
            //Act
            int actual = mm.Add(input1, input2);
            //Asset
            Assert.Equal(expected, actual);

        }
    }
}