using AutoMapper;
using GeniusExchangeConverter.Api.Models.ConversionLog;
using GeniusExchangeConverter.Api.Models.CurrencyConversion;
using GeniusExchangeConverter.Domain.Models.CurrencyConversion;

namespace GeniusExchangeConverter.Api.Models;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<CurrencyConversionRequestDto, CurrencyConversionRequest>();
        CreateMap<CurrencyConversionResponse, CurrencyConversionResponseDto>();

        CreateMap<Domain.Entities.ConversionLog, ConversionLogResponseDto>();
    }
}