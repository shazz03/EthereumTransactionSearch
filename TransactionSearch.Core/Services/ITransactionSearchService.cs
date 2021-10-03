using System.Threading.Tasks;
using TransactionSearch.Core.Models;

namespace TransactionSearch.Core.Services
{
    public interface ITransactionSearchService
    {
        Task<TransactionResponseDto> Search(SearchRequest request);
    }
}
