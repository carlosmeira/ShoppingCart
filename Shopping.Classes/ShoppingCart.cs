using System;
using System.Collections.Generic;
using System.Linq;

namespace Shopping.Classes
{
    /// <summary>
    ///  Shopping cart Class
    /// </summary>
    public class ShoppingCart
    {
        /// <summary>
        /// Shooping cart constructor. The constructor is private since the customer can only have 1 active shopping cart. You must use the Create method to create or use an existing cart.
        /// </summary>
        private ShoppingCart()
        {
            shoppingCartItems = new List<ShoppingCartItem>();
            checkedOut = false;
        }

        /// <summary>
        /// Shooping cart constructor. The constructor is private since the customer can only have 1 active shopping cart. You must use the Create method to create or use an existing cart.
        /// </summary>
        /// <param name="_clientId">Client unique ID</param>
        private ShoppingCart(int _clientId) : this()
        {
            clientId = _clientId;
            shoppingCartId = _clientId;
        }

        private int shoppingCartId;

        private int clientId;

        private List<ShoppingCartItem> shoppingCartItems;

        /// <summary>
        /// Client unique ID
        /// </summary>
        public int ClientId
        {
            get { return clientId; }
        }

        /// <summary>
        /// Shopping Cart unique ID
        /// </summary>
        public int ShoppingCartId
        {
            get { return shoppingCartId; }
        }

        private Boolean checkedOut;

        /// <summary>
        /// Property to represent if the shopping cart has already been finalized
        /// </summary>
        public Boolean CheckedOut
        {
            get { return checkedOut; }
        }

        /// <summary>
        /// Create is a method to create a new shopping cart or to retrieve an existing one from the database, since the customer can only have 1 active cart.
        /// </summary>
        /// <param name="clientId">Client ID</param>
        /// <returns>Shopping Cart Instance.</returns>
        public static ShoppingCart Create(int clientId)
        {
            var cart = ShoppingCartDatabase.GetShoppingCart(clientId);
            if (cart == null)
            {
                cart = new ShoppingCart(clientId);
                ShoppingCartDatabase.AddShoppingCart(cart);
            }
            return cart;
        }

        /// <summary>
        /// Method to clean the shopping cart. Only items are deleted.
        /// </summary>
        /// <returns>Boolean value representing the success or non-success of the operation</returns>
        public bool Reset()
        {
            try
            {
                if (CheckedOut) throw new Exception("Shopping Cart Finalized.");
                shoppingCartItems.Clear();
                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Method to return a cart item from the corresponding id.
        /// </summary>
        /// <param name="itemId">Item ID</param>
        /// <returns>ShoppingCartItem instance</returns>
        public ShoppingCartItem GetItem(int itemId)
        {
            return shoppingCartItems.FirstOrDefault(n => n.ItemId == itemId);
        }

        /// <summary>
        /// Method to add a number of items to the shopping cart.
        /// </summary>
        /// <param name="itemId">Item ID</param>
        /// <param name="quantity">Quantity of items to add to cart</param>
        /// <param name="unitValue">Unit value of the item</param>
        /// <returns>Boolean value representing the success or non-success of the operation</returns>
        public bool AddItem(int itemId, int quantity, decimal unitValue)
        {
            try
            {
                if (CheckedOut) throw new Exception("Shopping Cart Finalized.");
                ShoppingCartItem item = GetItem(itemId);

                if (item == null)
                {
                    if (quantity <= 0) throw new Exception("The minimum number of items allowed is 1.");
                    if (quantity > 10) throw new Exception("The maximum number of items allowed was exceeded (10 items).");

                    item = new ShoppingCartItem
                    {
                        ItemId = itemId,
                        Quantity = quantity,
                        UnitValue = unitValue
                    };
                    shoppingCartItems.Add(item);
                    return true;
                }
                else
                {
                    return UpdateItem(itemId, quantity + item.Quantity);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
        }

        /// <summary>
        /// Method to update the amount of items in the cart.
        /// </summary>
        /// <param name="itemId">Item ID</param>
        /// <param name="addQuantity">Quantity of items to add to cart</param>
        /// <returns>Boolean value representing the success or non-success of the operation</returns>
        public bool UpdateItem(int itemId, int quantity)
        {
            try
            {
                if (CheckedOut) throw new Exception("Shopping Cart Finalized.");
                var item = GetItem(itemId);
                if (item == null) throw new Exception($"Item \"{itemId}\" not found.");
                if (quantity <= 0) throw new Exception("The minimum number of items allowed is 1.");
                if (quantity > 10) throw new Exception("The maximum number of items allowed was exceeded (10 items).");
                item.Quantity = quantity;

                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }

        }

        /// <summary>
        /// Method to remove an item from the shopping cart.
        /// </summary>
        /// <param name="itemId">Item ID</param>
        /// <returns>Boolean value representing the success or non-success of the operation</returns>
        public bool RemoveItem(int itemId)
        {
            try
            {
                if (CheckedOut) throw new Exception("Shopping Cart Finalized.");
                var item = GetItem(itemId);
                if (item == null) throw new Exception($"Item \"{itemId}\" not found.");
                shoppingCartItems.Remove(item);
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
        }

        /// <summary>
        /// Metyhod to return a list of the items contained in the shopping cart.
        /// </summary>
        /// <returns>Dictionary of ShoppingCartItem with index as key.</returns>
        public Dictionary<int, ShoppingCartItem> ItemList()
        {
            return shoppingCartItems.ToList().Select((x, i) => new { x, i }).ToDictionary(a => a.i, a => a.x);
        }

        /// <summary>
        /// Method to verify the validity of the data informed in the shopping cart.
        /// </summary>
        /// <returns>Boolean value representing whether the shopping cart is valid and can be finalized.</returns>
        public bool IsValidForCheckout()
        {
            if (ItemCount == 0) return false;
            if (Total < 50m) return false;
            return true;
        }

        /// <summary>
        /// Checkout the shopping cart. After running this method it is no longer possible to change the items.
        /// </summary>
        /// <returns>Boolean value representing the success or non-success of the operation</returns>
        public bool Checkout()
        {
            if (IsValidForCheckout()) checkedOut = true;
            return checkedOut;
        }

        /// <summary>
        /// Property that returns the number of unique items in the shopping cart.
        /// </summary>
        public int ItemCount
        {
            get
            {
                return (shoppingCartItems == null) ? 0 : shoppingCartItems.Count;
            }
        }

        /// <summary>
        /// Property that returns total value of the cart
        /// </summary>
        public Decimal Total
        {
            get
            {
                return (shoppingCartItems == null) ? 0 : shoppingCartItems.Sum(n => n.Total);
            }
        }

    }

    /// <summary>
    /// This class represents the products in a shopping cart.
    /// </summary>
    public class ShoppingCartItem
    {
        private int itemId;

        private int quantity;

        private Decimal unitValue;

        /// <summary>
        /// Property that returns the item's unit value
        /// </summary>
        public Decimal UnitValue
        {
            get { return unitValue; }
            set { unitValue = value; }
        }

        /// <summary>
        /// Property that returns the item quantity
        /// </summary>
        public int Quantity
        {
            get { return quantity; }
            set { quantity = value; }
        }

        /// <summary>
        /// Item unique ID
        /// </summary>
        public int ItemId
        {
            get { return itemId; }
            set { itemId = value; }
        }

        /// <summary>
        /// Property that returns total value of the Item
        /// </summary>
        public Decimal Total
        {
            get
            {
                return Quantity * UnitValue;
            }
        }
    }


    /// <summary>
    /// Static class to simulate database.
    /// </summary>
    public static class ShoppingCartDatabase
    {
        public static List<ShoppingCart> ShoppingCartList;

        static ShoppingCartDatabase()
        {
            ShoppingCartList = new List<ShoppingCart>();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="clientId">Client unique ID</param>
        /// <returns>ShoppingCart instance or null</returns>
        public static ShoppingCart GetShoppingCart(int clientId)
        {
            return ShoppingCartList.FirstOrDefault(n => n.ClientId == clientId && n.CheckedOut == false);
        }

        /// <summary>
        /// Method to add a shopping cart to the database
        /// </summary>
        /// <param name="shoppingCart">Not Null ShoopingCart instance</param>
        /// <returns></returns>
        public static bool AddShoppingCart(ShoppingCart shoppingCart)
        {
            try
            {
                ShoppingCartList.Add(shoppingCart);
                return true;
            } catch
            {
                return false;
            }
        }
    }
}
