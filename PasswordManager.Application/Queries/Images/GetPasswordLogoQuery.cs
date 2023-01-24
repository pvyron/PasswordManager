using LanguageExt.Common;
using Mediator;
using PasswordManager.Application.IServices;

namespace PasswordManager.Application.Queries.Images;

public sealed record GetPasswordLogoQuery(Guid Guid) : IRequest<Result<byte[]>>;

public sealed class GetPasswordLogoQueryHandler : IRequestHandler<GetPasswordLogoQuery, Result<byte[]>>
{
    private readonly IImagesService _imagesService;

    public GetPasswordLogoQueryHandler(IImagesService imagesService)
    {
        _imagesService = imagesService;
    }

    public async ValueTask<Result<byte[]>> Handle(GetPasswordLogoQuery request, CancellationToken cancellationToken)
    {
        try
        {
            return await _imagesService.DownloadImage(request.Guid, cancellationToken);
        }
        catch (Exception ex)
        {
            return new Result<byte[]>(ex);
        }
    }
}
