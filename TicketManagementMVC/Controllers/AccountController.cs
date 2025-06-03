using Microsoft.AspNetCore.Mvc;
using Microsoft.Win32;
using System.Reflection;
using System.Text;
using System.Text.Json;
using TicketManagementMVC.DTOs;
using TicketManagementMVC.ViewModels;

public class AccountController : Controller
{
    private readonly IHttpClientFactory _httpClientFactory;

    public AccountController(IHttpClientFactory httpClientFactory)
    {
        _httpClientFactory = httpClientFactory;
    }

    [HttpGet]
    public IActionResult Login()
    {
        return View();
    }
    [HttpPost]
    public async Task<IActionResult> Login(LoginViewModel loginVM)
    {
        if (!ModelState.IsValid)
            return View(loginVM);

        var client = _httpClientFactory.CreateClient();
        var content = new StringContent(JsonSerializer.Serialize(loginVM), Encoding.UTF8, "application/json");
        var response = await client.PostAsync("https://localhost:7226/api/User/Login", content);

        if (response.IsSuccessStatusCode)
        {
            var result = await response.Content.ReadFromJsonAsync<JwtResponse>();
            HttpContext.Session.SetString("JWToken", result.Token);

            return RedirectToAction("Index", "Home");
            
        }

        ViewBag.Error = "Invalid credentials.";
        return View(loginVM);
        
    }

    [HttpGet]
    public IActionResult Register()
    {
        return View();
    }
    [HttpPost]
    public async Task<IActionResult> Register(RegisterViewModel registerVM)
    {
        if (!ModelState.IsValid)
            return View(registerVM);

        var client = _httpClientFactory.CreateClient();
        var content = new StringContent(JsonSerializer.Serialize(registerVM), Encoding.UTF8, "application/json");
        var response = await client.PostAsync("https://localhost:7226/api/User/Register", content);

        if (response.IsSuccessStatusCode)
        {
            return RedirectToAction("Login");
  
        }
        var error = await response.Content.ReadAsStringAsync();
        ModelState.AddModelError(string.Empty, error);
        return View(registerVM);
    }

    public IActionResult Logout()
    {
        HttpContext.Session.Clear();
        return RedirectToAction("Login");
    }
}
