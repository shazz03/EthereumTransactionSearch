using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TransactionSearch.Core.Models;
using TransactionSearch.Core.Services;

namespace TransactionSearch.Web.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ITransactionSearchService _transactionSearch;

        public IndexModel(ITransactionSearchService transactionSearch)
        {
            _transactionSearch = transactionSearch;
        }
        public IActionResult OnGet()
        {
            return Page();
        }
        [BindProperty]
        public SearchRequest SearchRequest { get; set; }

        [BindProperty]
        public TransactionResponseDto TransactionResponse { get; set; }
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            TransactionResponse = await _transactionSearch.Search(SearchRequest);

            return Page();
        }
    }
}
