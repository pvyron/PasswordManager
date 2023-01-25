using Mediator;
using PasswordManager.Application.IServices;
using PasswordManager.Shared.ResponseModels;
using System.Runtime.CompilerServices;

namespace PasswordManager.Application.Queries.Images;

public sealed record GetPasswordLogosQuery : IStreamQuery<LogoImageResponseModel>;

public sealed class GetPasswordLogosQueryHandler : IStreamQueryHandler<GetPasswordLogosQuery, LogoImageResponseModel>
{
    private readonly IImagesService _imagesService;

    public GetPasswordLogosQueryHandler(IImagesService imagesService)
    {
        _imagesService = imagesService;
    }

    public async IAsyncEnumerable<LogoImageResponseModel> Handle(GetPasswordLogosQuery query, [EnumeratorCancellation] CancellationToken cancellationToken)
    {
        await foreach (var logoModel in _imagesService.GetAllPasswordLogos(cancellationToken))
        {
            yield return new LogoImageResponseModel
            {
                LogoImageId = logoModel.LogoId,
                Title = logoModel.Title,
                PublicUrl = logoModel.FileUrl
            };
        }
    }
}
