using FullStack_Shangri.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FullstackProject.Controllers;

public class ShoppingCartController : Controller
{
    private readonly S22024Group2ProjectContext _context;

    public ShoppingCartController(S22024Group2ProjectContext context)
    {
        _context = context;
    }

    public async Task<IActionResult> Index()
    {
        var cartItems = GetCartItems();

        decimal originalTotal = cartItems.Sum(item => (item.game.Price ?? 0) * item.Quantity);
        decimal finalTotal = originalTotal;
        decimal discountAmount = 0;

        ViewBag.MembershipName = "None";

        if (User.Identity.IsAuthenticated)
        {
            var userId = User.Identity.Name;
            var userMembership = await _context.UserMembership
                .FirstOrDefaultAsync(um => um.UserId == userId);

            if (userMembership != null && userMembership.MembershipExpiryDate > DateTime.Now)
            {
                var membership = await _context.Memberships.FindAsync(userMembership.MembershipId);
                if (membership != null)
                {
                    decimal discount = membership.Type switch
                    {
                        "Bronze" => 0.05m,
                        "Silver" => 0.10m,
                        "Gold" => 0.15m,
                        _ => 0
                    };

                    discountAmount = originalTotal * discount;
                    finalTotal = originalTotal - discountAmount;

                    ViewBag.MembershipName = membership.Type;
                    ViewBag.DiscountAmount = discountAmount;
                }
            }
        }

        ViewBag.OriginalTotal = originalTotal;
        ViewBag.FinalTotal = finalTotal;

        return View(cartItems);
    }

    public IActionResult AddToCart(int id)
    {
        var game = _context.Games.FirstOrDefault(p => p.GameId == id);
        if (game != null)
        {
            if (game.Stock > 0)
            {
                var cartItems = GetCartItems();
                var cartItem = cartItems.FirstOrDefault(item => item.game.GameId == id);

                if (cartItem != null)
                {
                    cartItem.Quantity++;
                }
                else
                {
                    cartItems.Add(new CartItem { game = game, Quantity = 1 });
                }

                SaveCartItems(cartItems);
                game.Stock--;
                _context.SaveChanges();
            }
            else
            {
                TempData["ErrorMessage"] = "Sorry, this game is out of stock!";
            }
        }
        return RedirectToAction("Index");
    }

    public IActionResult RemoveFromCart(int id)
    {
        var cartItems = GetCartItems();
        var cartItem = cartItems.FirstOrDefault(item => item.game.GameId == id);

        if (cartItem != null)
        {
            var game = _context.Games.FirstOrDefault(p => p.GameId == id);
            if (game != null)
            {
                game.Stock += cartItem.Quantity;
                _context.SaveChanges();
            }

            cartItems.Remove(cartItem);
            SaveCartItems(cartItems);
        }

        return RedirectToAction("Index");
    }

    public IActionResult ClearCart()
    {
        SaveCartItems(new List<CartItem>());
        return RedirectToAction("Index");
    }

    //Added a success page where it returns thank you view Karman Dwivedi 22/11/24
    public IActionResult Success()
    {
        return View("Thankyou");
    }

    private List<CartItem> GetCartItems()
    {
        var cartItems = HttpContext.Session.Get<List<CartItem>>("CartItems") ?? new List<CartItem>();
        return cartItems;
    }

    private void SaveCartItems(List<CartItem> cartItems)
    {
        HttpContext.Session.Set("CartItems", cartItems);
    }
}
