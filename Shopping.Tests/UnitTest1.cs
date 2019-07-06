using System;
using Xunit;
using Shopping.Classes;

namespace Shopping.Tests
{
    public class ShoppingCartTests
    {
        [Fact]
        public void Task_SetupCart()
        {
            int clientId = 99;
            var cart = ShoppingCart.Create(clientId);

            Assert.NotNull(cart);
        }

        [Fact]
        public void Task_AddItem()
        {
            int clientId = 99;
            var cart = ShoppingCart.Create(clientId);
            cart.Reset();
            Assert.True(cart.AddItem(123, 9, 19.99m));
        }

        [Fact]
        public void Task_UpdateItem()
        {
            int clientId = 99;
            var cart = ShoppingCart.Create(clientId);
            cart.Reset();
            Assert.True(cart.AddItem(123, 7, 19.99m));

            var item = cart.GetItem(123);
            Assert.NotNull(item);
            Assert.Equal(7, item.Quantity);

            Assert.True(cart.UpdateItem(123, 3));
            Assert.Equal(3, item.Quantity);

            Assert.False(cart.UpdateItem(123, 11));
            Assert.NotEqual(11, item.Quantity);
            Assert.Equal(3, item.Quantity);
        }

        [Fact]
        public void Task_CountItems()
        {
            int clientId = 99;
            var cart = ShoppingCart.Create(clientId);
            cart.Reset();
            Assert.True(cart.AddItem(123, 9, 19.99m));
            Assert.True(cart.AddItem(456, 9, 9.99m));
            Assert.Equal(2, cart.ItemCount);

            Assert.True(cart.AddItem(123, 1, 19.99m));
            Assert.Equal(2, cart.ItemCount);
        }

        [Fact]
        public void Task_AddMax10Items()
        {
            int clientId = 99;
            var cart = ShoppingCart.Create(clientId);
            cart.Reset();
            Assert.True(cart.AddItem(123, 10, 19.99m));

            var item = cart.GetItem(123);
            Assert.NotNull(item);
            Assert.Equal(10, item.Quantity);

            // Try to add more than 10 items.
            Assert.False(cart.AddItem(123, 1, 19.99m));
            Assert.Equal(10, item.Quantity);
        }

        [Fact]
        public void Task_TotalItemsSum()
        {
            int clientId = 99;
            var cart = ShoppingCart.Create(clientId);
            cart.Reset();

            // Check Totals
            Assert.True(cart.AddItem(123, 4, 19.99m));
            Assert.Equal(79.96m, cart.Total);

            // Check Totals
            Assert.True(cart.AddItem(123, 5, 19.99m));
            Assert.Equal(179.91m, cart.Total);
        }

        [Fact]
        public void Task_GetItem()
        {
            int clientId = 99;
            var cart = ShoppingCart.Create(clientId);
            cart.Reset();
            Assert.True(cart.AddItem(123, 4, 19.99m));
            Assert.NotNull(cart.GetItem(123));
        }

        [Fact]
        public void Task_RemoveItem()
        {
            int clientId = 99;
            var cart = ShoppingCart.Create(clientId);
            cart.Reset();
            Assert.True(cart.AddItem(123, 4, 19.99m));
            Assert.Equal(1, cart.ItemCount);
            Assert.NotNull(cart.GetItem(123));

            Assert.True(cart.RemoveItem(123));
            Assert.Equal(0, cart.ItemCount);
            Assert.Null(cart.GetItem(123));
        }

        [Fact]
        public void Task_CartReset()
        {
            int clientId = 99;
            var cart = ShoppingCart.Create(clientId);
            cart.Reset();
            Assert.True(cart.AddItem(123, 4, 19.99m));
            Assert.Equal(1, cart.ItemCount);

            Assert.True(cart.Reset());
            Assert.Equal(0, cart.ItemCount);
            Assert.Null(cart.GetItem(123));
        }

        [Fact]
        public void Task_CartIsValidForCheckout()
        {
            int clientId = 99;
            var cart = ShoppingCart.Create(clientId);
            cart.Reset();

            // Can not be valid with 0 items
            Assert.False(cart.IsValidForCheckout());

            // Can not be valid with value less than 50
            Assert.True(cart.AddItem(123, 4, 9.99m));
            Assert.False(cart.IsValidForCheckout());

            cart.Reset();

            // Must be valid now
            Assert.True(cart.AddItem(123, 1, 50m));
            Assert.True(cart.IsValidForCheckout());
        }

        [Fact]
        public void Task_Checkout()
        {
            int clientId = 99;
            var cart = ShoppingCart.Create(clientId);
            cart.Reset();

            Assert.False(cart.CheckedOut);

            // Can not be valid with 0 items
            Assert.False(cart.Checkout());

            // Can not be valid with value less than 50
            Assert.True(cart.AddItem(123, 4, 9.99m));
            Assert.False(cart.Checkout());

            cart.Reset();

            // Must be valid now
            Assert.True(cart.AddItem(123, 1, 50m));
            Assert.True(cart.Checkout());
            Assert.True(cart.CheckedOut);
        }
    }
}
