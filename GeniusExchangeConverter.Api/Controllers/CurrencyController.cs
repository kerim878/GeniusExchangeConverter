using AutoMapper;
using FluentValidation;
using GeniusExchangeConverter.Api.Models.ConversionLog;
using GeniusExchangeConverter.Api.Models.CurrencyConversion;
using GeniusExchangeConverter.Application.CurrencyService;
using GeniusExchangeConverter.Domain.Models.CurrencyConversion;
using Microsoft.AspNetCore.Mvc;

namespace GeniusExchangeConverter.Api.Controllers;

[Route("api/currency")]
[ApiController]
public class CurrencyController : ControllerBase
{
    private readonly ICurrencyService _currencyService;
    private readonly IMapper _mapper;

    private readonly IValidator<CurrencyConversionRequestDto> _validator; // We can optimize by using a generic validator and using it for all DTOs

    public CurrencyController(ICurrencyService currencyService, IMapper mapper,
        IValidator<CurrencyConversionRequestDto> validator)
    {
        _currencyService = currencyService;
        _mapper = mapper;
        _validator = validator;
    }

    [HttpGet("conversion-logs")]
    public async Task<IActionResult> GetConversionLogs([FromQuery] ConversionLogRequestDto requestDto,
        CancellationToken cancellationToken)
    {
        var logs = await _currencyService.GetConversionLogs(
            requestDto.FromCurrency,
            requestDto.ToCurrency,
            requestDto.StartDate,
            requestDto.EndDate,
            cancellationToken);

        var logsDto = _mapper.Map<IReadOnlyCollection<ConversionLogResponseDto>>(logs);

        return Ok(logsDto);
    }


    [HttpPost("convert")]
    public async Task<IActionResult> ConvertCurrency([FromBody] CurrencyConversionRequestDto requestDto,
        CancellationToken cancellationToken)
    {
        var validationResult = await _validator.ValidateAsync(requestDto, cancellationToken);
        if (!validationResult.IsValid)
        {
            var errors = validationResult.Errors.Select(x => x.ErrorMessage);
            return BadRequest(errors);
        }

        var request = _mapper.Map<CurrencyConversionRequest>(requestDto);
        var response = await _currencyService.ConvertCurrency(request, cancellationToken);
        var responseDto = _mapper.Map<CurrencyConversionResponseDto>(response);

        return Ok(responseDto);
    }
}