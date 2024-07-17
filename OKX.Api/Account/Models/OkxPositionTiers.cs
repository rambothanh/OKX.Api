﻿using OKX.Api.Trading.Converters;
using OKX.Api.Trading.Enums;

namespace OKX.Api.Account.Models;

/// <summary>
/// OkxPosition
/// </summary>
public class OkxPositionTiers
{
    /// <summary>
    /// Underlying
    /// </summary>
    [JsonProperty("uly")]
    public string Underlying { get; set; }

    /// <summary>
    /// Instrument Family
    /// </summary>
    [JsonProperty("instFamily")]
    public string InstrumentFamily { get; set; }

    /// <summary>
    /// Maximum Positions
    /// </summary>
    [JsonProperty("maxSz")]
    public long MaximumPositions { get; set; }

    /// <summary>
    /// Position Type
    /// </summary>
    [JsonProperty("posType"), JsonConverter(typeof(OkxPositionTypeConverter))]
    public OkxPositionType PositionType { get; set; }
}