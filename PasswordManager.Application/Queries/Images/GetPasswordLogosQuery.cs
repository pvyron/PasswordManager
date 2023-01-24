using Mediator;
using PasswordManager.Application.IServices;
using PasswordManager.Shared.ResponseModels;
using System.Runtime.CompilerServices;

namespace PasswordManager.Application.Queries.Images;

public sealed record GetPasswordLogosQuery : IStreamQuery<ImageLogoResponseModel>;

public sealed class GetPasswordLogosQueryHandler : IStreamQueryHandler<GetPasswordLogosQuery, ImageLogoResponseModel>
{
    private readonly IImagesService _imagesService;

    public GetPasswordLogosQueryHandler(IImagesService imagesService)
    {
        _imagesService = imagesService;
    }

    public async IAsyncEnumerable<ImageLogoResponseModel> Handle(GetPasswordLogosQuery query, [EnumeratorCancellation] CancellationToken cancellationToken)
    {
        await foreach (var logoModel in _imagesService.GetAllPasswordLogos(cancellationToken))
        {
            yield return new ImageLogoResponseModel
            {
                PublicUrl = logoModel.FileUrl,
                ThumbnailUrl = logoModel.ThumbnailUrl
            };
        }
    }
}
