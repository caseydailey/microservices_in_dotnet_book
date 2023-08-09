namespace    ShoppingCart.ShoppingCart
{
    using Microsoft.AspNetCore.Mvc;
    using ShoppingCart;

    [Route("/shoppingcart")]
    public class ShoppingCartController : ControllerBase
    {
        private readonly IShoppingCartStore shoppingCartStore;
        public ShoppingCartController(IShoppingCartStore shoppingCartStore)
        {
            this.shoppingCartStore = shoppingCartStore;
        }

        [HttpGet(" {userId:int} ")]
        public ShoppingCartController GetShoppingCartController(int userId) => 
            this.shoppingCartStore.Get(userId);

        [HttpPost("{userId:int/items}")]
        public async Task<ShoppingCart> Post(int userId, [FromBody] int[] productIds)
        {
            var shoppingCart = shoppingCartStore.Get(userId);
            var shoppingCartItems = await this.productCatalogClient.GetShoppingCartItems(productIds);
            shoppingcart.AddItems(shoppingCartItems, eventStore);
            shoppingCartStore.Save(shoppingCart);
            return shoppingCart;
        }

        [HttpDelete("{userId:int/items}")]
        public ShoppingCart Delete(int userId, [FromBody] int[] productIds)
        {
            var shoppingCart = this.shoppingCartStore.Get(userId);
            shoppingCart.RemoveItems(productIds, this.eventStore);
            this.shoppingCartStore.Save(shoppingCart);
            return shoppingCart;
        }  
    }
}