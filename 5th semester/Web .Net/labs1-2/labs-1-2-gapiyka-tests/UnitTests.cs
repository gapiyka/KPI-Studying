using System;
using Xunit;
using CustomList;

namespace labs1_2_gapiyka_test
{
    public class UnitTests
    {
        [Fact]
        public void TestListConstructor()
        {
            //ARRANGE
            const int expectedCount = 0;

            //ACT
            var dinosaurs = new CustomList<string>();
            var initializeResult = dinosaurs.Count;
            var firstElement = dinosaurs.Get(0);

            //ASSERT    
            Assert.Equal(expectedCount, initializeResult);
            Assert.Null(firstElement);
        }

        [Fact]
        public void TestListConstructorWithIntParam()
        {
            //ARRANGE
            const int initCapacity = 5;
            const int expectedCount = 0;
            const int expectedFirstElement = 0;

            //ACT
            var list = new CustomList<int>(initCapacity);
            var initializeResult = list.Count;
            var firstElement = list.Get(0);

            //ASSERT    
            Assert.Equal(expectedCount, initializeResult);
            Assert.Equal(expectedFirstElement, firstElement);
        }

        [Fact]
        public void TestListConstructorWithIEParam()
        {
            //ARRANGE
            const int expectedCount = 3;
            bool expectedSecondElement = false;
            bool[] arr = new bool[] { false, false, true };

            //ACT
            var list = new CustomList<bool>(arr);
            var initializeResult = list.Count;
            var secondElement = list.Get(1);

            //ASSERT    
            Assert.Equal(expectedCount, initializeResult);
            Assert.Equal(expectedSecondElement, secondElement);
        }
        
        [Fact]
        public void TestListProperties()
        {
            //ARRANGE
            const int expectedInitializeCount = 0;
            const int expectedResultCount = 1;
            const bool expectedInitializeEmptyFlag = true;
            const bool expectedResultEmptyFlag = false;
            const bool expectedReadOnly = false;

            //ACT
            var list = new CustomList<string>();
            var initializeEmptyFlag = list.IsEmpty;
            var initializeCount = list.Count;
            list.Add("NEW_ONE");
            var resultEmptyFlag = list.IsEmpty;
            var resultCount = list.Count;
            var readOnly = list.IsReadOnly;

            //ASSERT    
            Assert.Equal(expectedInitializeCount, initializeCount);
            Assert.Equal(expectedResultCount, resultCount);
            Assert.Equal(expectedInitializeEmptyFlag, initializeEmptyFlag);
            Assert.Equal(expectedResultEmptyFlag, resultEmptyFlag);
            Assert.Equal(expectedReadOnly, readOnly);
        }

        [Fact]
        public void TestListAddLogic()
        {
            //ARRANGE
            string[] dinoPrefab = new string[] { "Tyrannosaurus", 
                "Amargasaurus", "Mamenchisaurus", 
                "Deinonychus", "Compsognathus" };
            int expectedCount = dinoPrefab.Length;

            //ACT
            var dinoList = new CustomList<string>();
            foreach (string dino in dinoPrefab)
            {
                dinoList.Add(dino);
            }
            var count = dinoList.Count;
            var firstElement = dinoList.Get(0);
            var secondElement = dinoList.Get(1);
            var lastElement = dinoList.Get(4);

            //ASSERT    
            Assert.Equal(expectedCount, count);
            Assert.Equal(dinoPrefab[0], firstElement);
            Assert.Equal(dinoPrefab[1], secondElement);
            Assert.Equal(dinoPrefab[4], lastElement);
        }

        [Fact]
        public void TestListInsertLogic()
        {
            //ARRANGE
            int[] arr = new int[] { 1, 9, 8 };
            const int expectedStartCount = 3;
            const int expectedSecondCount = 4;
            const int expectedResultCount = 5;
            int[] newElements = new int[] { 3, 0 };

            //ACT
            var list = new CustomList<int>();
            foreach (int i in arr)
            {
                list.Add(i);
            }
            var firstCount = list.Count;
            var firstElementByIndexZero = list.Get(0);
            var firstElementByIndexTwo = list.Get(2);
            list.Insert(2, newElements[0]);
            var secondCount = list.Count;
            var secondElementByIndexZero = list.Get(0);
            var secondElementByIndexTwo = list.Get(2);
            list.Insert(0, newElements[1]);
            var resultCount = list.Count;
            var resultElementByIndexZero = list.Get(0);
            var resultElementByIndexTwo = list.Get(2);


            //ASSERT    
            Assert.Equal(expectedStartCount, firstCount);
            Assert.Equal(expectedSecondCount, secondCount);
            Assert.Equal(expectedResultCount, resultCount);
            Assert.Equal(firstElementByIndexZero, arr[0]);
            Assert.Equal(firstElementByIndexTwo, arr[2]);
            Assert.Equal(secondElementByIndexZero, arr[0]);
            Assert.Equal(secondElementByIndexTwo, newElements[0]);
            Assert.Equal(resultElementByIndexZero, newElements[1]);
            Assert.Equal(resultElementByIndexTwo, arr[1]);
        }

        [Fact]
        public void TestListResizing()
        {
            //ARRANGE
            const int expectedInitializeCount = 5;
            const int expectedFOCount = 4; // expected count after First Operation
            const int expectedSOCount = 3; // expected count after Second Operation
            const int expectedClearCount = 0; // expected count after Clear Operation
            string[] arr = new string[expectedInitializeCount] { "", "a", "bb", "ccc", "abc" };

            //ACT
            var list = new CustomList<string>(arr);
            var initializeCount = list.Count;
            list.Remove(0);
            var fOCount = list.Count;
            list.Remove("abc");
            var sOCount = list.Count;
            list.Clear();
            var resultCount = list.Count;


            //ASSERT    
            Assert.Equal(expectedInitializeCount, initializeCount);
            Assert.Equal(expectedFOCount, fOCount);
            Assert.Equal(expectedSOCount, sOCount);
            Assert.Equal(expectedClearCount, resultCount);
        }

        [Fact]
        public void TestListContainsLogic()
        {
            //ARRANGE
            bool[] arr= new bool[] { false, false, false };
            const bool expectedFirstContainsResult = false;
            const bool expectedSecondContainsResult = true;

            //ACT
            var list = new CustomList<bool>(arr);
            var firstContainsMamenchisaurus = list.Contains(true);
            list.Set(true, 1);
            var secondContainsMamenchisaurus = list.Contains(true);

            //ASSERT    
            Assert.Equal(expectedFirstContainsResult, firstContainsMamenchisaurus);
            Assert.Equal(expectedSecondContainsResult, secondContainsMamenchisaurus);
        }

        [Fact]
        public void TestListFindLogic()
        {
            //ARRANGE
            string[] dinoPrefab = new string[] { "Tyrannosaurus",
                "Amargasaurus", "Mamenchisaurus",
                "Deinonychus", "Compsognathus" };
            const int expectedFirstFindResult = 2;
            const int expectedSecondFindResult = -1;

            //ACT
            var dinoList = new CustomList<string>(dinoPrefab);
            var firstFindMamenchisaurus = dinoList.Find("Mamenchisaurus");
            dinoList.Set("Bulbasaur", expectedFirstFindResult);
            var secondFindMamenchisaurus = dinoList.Find("Mamenchisaurus");

            //ASSERT    
            Assert.Equal(expectedFirstFindResult, firstFindMamenchisaurus);
            Assert.Equal(expectedSecondFindResult, secondFindMamenchisaurus);
        }
        
        [Fact]
        public void TestListCopyToLogic()
        {
            //ARRANGE
            string[] dinoPrefab = new string[] { "Tyrannosaurus",
                "Amargasaurus", "Mamenchisaurus",
                "Deinonychus", "Compsognathus" };
            const int expectedArrSize = 5;

            //ACT
            var dinoList = new CustomList<string>(dinoPrefab);
            string[] newArr = new string[expectedArrSize];
            dinoList.CopyTo(newArr, 0);
            var firstElement = dinoList.Get(0);
            var newFirstElement = newArr[0];
            var lastElement = dinoList.Get(4);
            var newLastElement = newArr[4];
            var newArrSize = newArr.Length;

            //ASSERT    
            Assert.Equal(dinoPrefab[0], firstElement);
            Assert.Equal(dinoPrefab[0], newFirstElement);
            Assert.Equal(dinoPrefab[4], lastElement);
            Assert.Equal(dinoPrefab[4], newLastElement);
            Assert.Equal(expectedArrSize, newArrSize);
        }
    }
}