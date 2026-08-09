using FullstackProject.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace FullstackProject.Controllers
{
    public class ShoppingCartController : Controller
    {
        private readonly S22024Group2ProjectContext _context;

        public ShoppingCartController(S22024Group2ProjectContext context)
        {
            _context = context;
        }

        // GET: /ShoppingCart
        public IActionResult Index()
        {
            var cart = GetCart();
            return View(cart);
        }

        // GET: /ShoppingCart/Checkout
        [HttpGet]
        public IActionResult Checkout()
        {
            var cart = GetCart();

            if (!cart.Any())
            {
                TempData["Message"] = "Your cart is empty.";
                return RedirectToAction(nameof(Index));
            }

            ViewBag.Total = cart.Sum(item => item.Price * item.Quantity);

            return View(cart);
        }

        // POST: /ShoppingCart/Checkout
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Checkout(
            string customerName,
            string email,
            string paymentMethod)
        {
            var cart = GetCart();

            if (!cart.Any())
            {
                TempData["Message"] = "Your cart is empty.";
                return RedirectToAction(nameof(Index));
            }

            var total = cart.Sum(item => item.Price * item.Quantity);

            var purchase = new Purchase
            {
                PurchaseDate = DateOnly.FromDateTime(DateTime.Today),
                PricePaid = total,
                UserId = null
            };

            _context.Purchases.Add(purchase);
            await _context.SaveChangesAsync();

            var payment = new Payment
            {
                PurchaseId = purchase.PurchaseId,
                PaymentDate = DateOnly.FromDateTime(DateTime.Today),
                PaymentMethod = string.IsNullOrWhiteSpace(paymentMethod)
                    ? "Card"
                    : paymentMethod
            };

            _context.Payments.Add(payment);
            await _context.SaveChangesAsync();

            // Empty cart after successful purchase
            HttpContext.Session.Remove("Cart");

            TempData["Message"] =
                $"Checkout complete. Purchase #{purchase.PurchaseId} has been created.";

            return RedirectToAction(
                nameof(Confirmation),
                new { id = purchase.PurchaseId });
        }

        // GET: /ShoppingCart/Confirmation/5
        public IActionResult Confirmation(int id)
        {
            var purchase = _context.Purchases
                .Include(p => p.Payments)
                .FirstOrDefault(p => p.PurchaseId == id);

            if (purchase == null)
            {
                return NotFound();
            }

            return View(purchase);
        }

        // GET: /ShoppingCart/AddToCart/5
        public async Task<IActionResult> AddToCart(int id)
        {
            var game = await _context.Games.FindAsync(id);

            if (game == null)
            {
                return NotFound();
            }

            var cart = GetCart();

            var existingItem = cart.FirstOrDefault(
                item => item.GameId == id);

            if (existingItem == null)
            {
                cart.Add(new CartItem
                {
                    GameId = game.GameId,
                    Title = game.Title,
                    Price = game.Price ?? 0,
                    ImageFileName = game.ImageFileName,
                    Quantity = 1
                });
            }
            else
            {
                existingItem.Quantity++;
            }

            SaveCart(cart);

            TempData["Message"] =
                $"{game.Title} added to your cart.";

            return RedirectToAction(nameof(Index));
        }

        // GET: /ShoppingCart/BuyNow/5
        public async Task<IActionResult> BuyNow(int id)
        {
            var game = await _context.Games.FindAsync(id);

            if (game == null)
            {
                return NotFound();
            }

            var cart = new List<CartItem>
            {
                new CartItem
                {
                    GameId = game.GameId,
                    Title = game.Title,
                    Price = game.Price ?? 0,
                    ImageFileName = game.ImageFileName,
                    Quantity = 1
                }
            };

            SaveCart(cart);

            TempData["Message"] =
                $"{game.Title} is ready for checkout.";

            return RedirectToAction(nameof(Index));
        }

        // GET: /ShoppingCart/RemoveFromCart/5
        public IActionResult RemoveFromCart(int id)
        {
            var cart = GetCart();

            var item = cart.FirstOrDefault(
                x => x.GameId == id);

            if (item != null)
            {
                cart.Remove(item);
                SaveCart(cart);
            }

            return RedirectToAction(nameof(Index));
        }

        // GET: /ShoppingCart/ClearCart
        public IActionResult ClearCart()
        {
            HttpContext.Session.Remove("Cart");

            TempData["Message"] = "Your cart has been cleared.";

            return RedirectToAction(nameof(Index));
        }

        // Keep Clear working
        public IActionResult Clear()
        {
            return RedirectToAction(nameof(ClearCart));
        }

        // PayPal success / Thank You page
        public IActionResult Success()
        {
            return View("Thankyou");
        }

        private List<CartItem> GetCart()
        {
            var sessionCart = HttpContext.Session.GetString("Cart");

            if (string.IsNullOrEmpty(sessionCart))
            {
                return new List<CartItem>();
            }

            return JsonConvert.DeserializeObject<List<CartItem>>(sessionCart)
                   ?? new List<CartItem>();
        }

        private void SaveCart(List<CartItem> cart)
        {
            HttpContext.Session.SetString(
                "Cart",
                JsonConvert.SerializeObject(cart));
        }
    }

    public class CartItem
    {
        public int GameId { get; set; }

        public string Title { get; set; } = string.Empty;

        public decimal Price { get; set; }

        public int Quantity { get; set; }

        public string ImageFileName { get; set; } = string.Empty;
    }
}