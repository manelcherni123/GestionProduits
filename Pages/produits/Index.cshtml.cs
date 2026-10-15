using GestionProduits.Data;
using GestionProduits.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace GestionProduits.Pages.Produits;

public class IndexModel : PageModel
{
    private readonly AppDbContext _context;
    public IndexModel(AppDbContext context) => _context = context;

    public List<Produit> Produits { get; set; } = new();

    [BindProperty(SupportsGet = true)]
    public string? SearchString { get; set; }

    public int TotalStock { get; set; }
    public decimal PrixMoyen { get; set; }

    public async Task OnGetAsync()
    {
        var requete = _context.Produits
            .Include(p => p.Categorie)
            .AsQueryable();

        if (!string.IsNullOrWhiteSpace(SearchString))
        {
            requete = requete.Where(p => p.Nom.Contains(SearchString));
        }

        Produits = await requete.OrderBy(p => p.Nom).ToListAsync();

        TotalStock = Produits.Sum(p => p.Stock);
        PrixMoyen = Produits.Any() ? Produits.Average(p => p.Prix) : 0;
    }
}