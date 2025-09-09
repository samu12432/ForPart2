using ForParts.DTOs.FEU;

namespace ForParts.IService.FEU
{
    public interface IFeuService
    {
        Task<FeuResponseDto> EmitirFacturaAsync(int id);

    }
}
