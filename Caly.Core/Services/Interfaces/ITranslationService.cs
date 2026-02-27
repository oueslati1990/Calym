using System.Threading;
using System.Threading.Tasks;

namespace Caly.Core.Services.Interfaces;

internal interface ITranslationService
{
    Task<string?> TranslateAsync(string word,
                    CancellationToken cancellationToken = default);
}