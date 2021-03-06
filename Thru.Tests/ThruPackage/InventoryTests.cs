using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using NUnit.Framework;
using System;

namespace Thru.Tests.ThruPackage
{
    [TestFixture]
    public class InventoryTests
    {

        int[,] itemShape1, itemShape2;

    [OneTimeSetUp]
        public void OneTimeSetUp()
        {
           itemShape1 = new int[,]{
               { 0, 1, 0},
                { 1, 1, 1},
                { 0, 0, 0}
            };
            itemShape2 =  new int[,]{
               { 0, 1, 0},
                { 1, 1, 0},
                { 0, 1, 0}
            };

        }

        [OneTimeTearDown]
        public void OneTimeTearDown()
        {
            // Code here will be run once after all tests.
        }

        [SetUp]
        public void SetUp()
        {
            // Code here will be run once before every test.
        }

        [TearDown]
        public void TearDown()
        {
            // Code here will be run once after every test.
        }

   

      
        [Test]
        public void isValidMove_Success()
        {
            int[,] trueBoard = ThruLib.emptyBoard(5, 5);
           Assert.IsTrue(InventoryGameBoard.isValidMove(itemShape2, trueBoard, new Point(1,1), 5, 5));
        }
        [Test]
        public void rotate_Success()
        {
            Assert.AreEqual(itemShape1, ThruLib.rotate90DegClockwise<int>(itemShape2));
        }

        [Test]
        public void draggablesItemGroupTheSame()
        {
            MouseHandler mouseHandler = new MouseHandler();
            Item item = new Item(mouseHandler, null, Point.Zero, Point.Zero, false, 0f, 0f, 1f, itemShape2, ItemSlot.Misc1);
            

            Assert.AreEqual(itemShape2, item.DraggableGroup.ItemShape);
        }

        [Test]
        [Repeat(100)]
        public void FunctionBeingTested2_Condition_ExpectedResult()
        {
            // Tests can also be repeated N times.
            // Here, this test will be repeated 100 times.

            // Write some test code.
            // e.g. Assert.AreEqual(expectedValue, actualValue);
        }


     /*   public ThruGame setUpTestEnv()
        {
            ThruGame game = new ThruGame();
            GlobalState = game.state;
            setUpInventory(GlobalState, game.Services);
            return game;
        }

        public GlobalState setUpInventory(GlobalState GlobalState, IServiceProvider services)
        {
            SleepingBagImage = Content.Load<Texture2D>("ItemIcons/SleepingBag132x32");
            int[,] itemShape = new int[,]{
               { 0, 1, 0},
                { 1, 1, 1},
                { 0, 0, 0}
            };
            GlobalState.currentState = State.Game;
            GlobalState.gameView.stateMachine.currentState = GameState.Inventory;
            
            SleepingBag = new Item(GlobalState.MouseHandler, SleepingBagImage, new Point(600, 300), false, 4, 1.7f, 0, itemShape);
            int[,] trueBoard = ThruLib.emptyBoard(5, 5);
            return GlobalState;
        }*/
    }

}