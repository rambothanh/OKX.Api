﻿using OKX.Api.Account.Converters;
using OKX.Api.Account.Enums;
using OKX.Api.Public.Converters;
using OKX.Api.Public.Enums;
using OKX.Api.Trade.Converters;
using OKX.Api.Trade.Enums;
using OKX.Api.Trade.Models;

namespace OKX.Api.Trade.Clients;

/// <summary>
/// OKX Rest Api Trade Client
/// </summary>
public class OkxTradingRestClient(OkxRestApiClient root) : OkxBaseRestClient(root)
{
    // Endpoints
    private const string v5TradeOrder = "api/v5/trade/order";
    private const string v5TradeBatchOrders = "api/v5/trade/batch-orders";
    private const string v5TradeCancelOrder = "api/v5/trade/cancel-order";
    private const string v5TradeCancelBatchOrders = "api/v5/trade/cancel-batch-orders";
    private const string v5TradeAmendOrder = "api/v5/trade/amend-order";
    private const string v5TradeAmendBatchOrders = "api/v5/trade/amend-batch-orders";
    private const string v5TradeClosePosition = "api/v5/trade/close-position";
    private const string v5TradeOrderGet = "api/v5/trade/order";
    private const string v5TradeOrdersPending = "api/v5/trade/orders-pending";
    private const string v5TradeOrdersHistory = "api/v5/trade/orders-history";
    private const string v5TradeOrdersHistoryArchive = "api/v5/trade/orders-history-archive";
    private const string v5TradeFills = "api/v5/trade/fills";
    private const string v5TradeFillsHistory = "api/v5/trade/fills-history";
    private const string v5TradeFillsArchive = "api/v5/trade/fills-archive";
    private const string v5TradeEasyConvertCurrencyList = "api/v5/trade/easy-convert-currency-list";
    private const string v5TradeEasyConvert = "api/v5/trade/easy-convert";
    private const string v5TradeEasyConvertHistory = "api/v5/trade/easy-convert-history";
    private const string v5TradeOneClickRepayCurrencyList = "api/v5/trade/one-click-repay-currency-list";
    private const string v5TradeOneClickRepay = "api/v5/trade/one-click-repay";
    private const string v5TradeOneClickRepayHistory = "api/v5/trade/one-click-repay-history";
    private const string v5TradeMassCancel = "api/v5/trade/mass-cancel";
    private const string v5TradeCancelAllAfter = "api/v5/trade/cancel-all-after";
    private const string v5TradeAccountRateLimit = "api/v5/trade/account-rate-limit";

    /// <summary>
    /// You can place an order only if you have sufficient funds.
    /// For leading contracts, this endpoint supports placement, but can't close positions.
    /// </summary>
    /// <param name="instrumentId">Instrument ID</param>
    /// <param name="tradeMode">Trade Mode</param>
    /// <param name="orderSide">Order Side</param>
    /// <param name="positionSide">Position Side</param>
    /// <param name="orderType">Order Type</param>
    /// <param name="size">Size</param>
    /// <param name="price">Price</param>
    /// <param name="currency">Currency</param>
    /// <param name="clientOrderId">Client Order ID</param>
    /// <param name="reduceOnly">Whether to reduce position only or not, true false, the default is false.</param>
    /// <param name="quantityType">Quantity Type</param>
    /// 
    /// <param name="quickMgnType">Quick Margin type. Only applicable to Quick Margin Mode of isolated margin. The default value is manual</param>
    /// <param name="banAmend">Whether to disallow the system from amending the size of the SPOT Market Order.</param>
    /// 
    /// <param name="tpTriggerPxType">Take-profit trigger price type. The Default is last</param>
    /// <param name="tpTriggerPx">Take-profit trigger price. If you fill in this parameter, you should fill in the take-profit order price as well.</param>
    /// <param name="tpOrdPx">Take-profit order price</param>
    /// 
    /// <param name="slTriggerPxType">Stop-loss trigger price type. The Default is last</param>
    /// <param name="slTriggerPx">Stop-loss trigger price. If you fill in this parameter, you should fill in the stop-loss order price.</param>
    /// <param name="slOrdPx">Stop-loss order price. If you fill in this parameter, you should fill in the stop-loss trigger price. If the price is -1, stop-loss will be executed at the market price.</param>
    /// 
    /// <param name="attachAlgoClientOrderOrderId">Client-supplied Algo ID when plaing order attaching TP/SL. A combination of case-sensitive alphanumerics, all numbers, or all letters of up to 32 characters. It will be posted to algoClOrdId when placing TP/SL order once the general order is filled completely.</param>
    /// 
    /// <param name="selfTradePreventionId">Self trade prevention ID. Orders from the same master account with the same ID will be prevented from self trade.</param>
    /// <param name="selfTradePreventionMode">Self trade prevention mode. Default to cancel maker. cancel_maker,cancel_taker, cancel_both. Cancel both does not support FOK.</param>
    /// 
    /// <param name="ct">Cancellation Token</param>
    /// <returns></returns>
    public Task<RestCallResult<OkxOrderPlaceResponse>> PlaceOrderAsync(
        string instrumentId,
        OkxTradeMode tradeMode,
        OkxOrderSide orderSide,
        OkxPositionSide positionSide,
        OkxOrderType orderType,
        decimal size,
        decimal? price = null,
        string currency = null,
        string clientOrderId = null,
        bool? reduceOnly = null,
        OkxQuantityType? quantityType = null,

        OkxQuickMarginType? quickMgnType = null,
        bool? banAmend = null,

        OkxAlgoPriceType? tpTriggerPxType = null,
        decimal? tpTriggerPx = null,
        decimal? tpOrdPx = null,

        OkxAlgoPriceType? slTriggerPxType = null,
        decimal? slTriggerPx = null,
        decimal? slOrdPx = null,

        string attachAlgoClientOrderOrderId = null,

        long? selfTradePreventionId = null,
        OkxSelfTradePreventionMode? selfTradePreventionMode = null,
        CancellationToken ct = default)
    {
        var parameters = new Dictionary<string, object> {
            {"tag", OkxConstants.BrokerId },
            {"instId", instrumentId },
            {"tdMode", JsonConvert.SerializeObject(tradeMode, new OkxTradeModeConverter(false)) },
            {"side", JsonConvert.SerializeObject(orderSide, new OkxOrderSideConverter(false)) },
            {"posSide", JsonConvert.SerializeObject(positionSide, new OkxPositionSideConverter(false)) },
            {"ordType", JsonConvert.SerializeObject(orderType, new OkxOrderTypeConverter(false)) },
            {"sz", size.ToOkxString() },
        };
        parameters.AddOptionalParameter("px", price?.ToOkxString());
        parameters.AddOptionalParameter("ccy", currency);
        parameters.AddOptionalParameter("clOrdId", clientOrderId);
        parameters.AddOptionalParameter("reduceOnly", reduceOnly);
        parameters.AddOptionalParameter("tgtCcy", JsonConvert.SerializeObject(quantityType, new OkxQuantityTypeConverter(false)));

        parameters.AddOptionalParameter("quickMgnType", JsonConvert.SerializeObject(quickMgnType, new OkxQuickMarginTypeConverter(false)));
        parameters.AddOptionalParameter("banAmend", banAmend);

        parameters.AddOptionalParameter("tpTriggerPxType", JsonConvert.SerializeObject(tpTriggerPxType, new OkxAlgoPriceTypeConverter(false)));
        parameters.AddOptionalParameter("tpTriggerPx", tpTriggerPx?.ToOkxString());
        parameters.AddOptionalParameter("tpOrdPx", tpOrdPx?.ToOkxString());

        parameters.AddOptionalParameter("slTriggerPxType", JsonConvert.SerializeObject(slTriggerPxType, new OkxAlgoPriceTypeConverter(false)));
        parameters.AddOptionalParameter("slTriggerPx", slTriggerPx?.ToOkxString());
        parameters.AddOptionalParameter("slOrdPx", slOrdPx?.ToOkxString());

        parameters.AddOptionalParameter("attachAlgoClOrdId", attachAlgoClientOrderOrderId);

        parameters.AddOptionalParameter("stpId", selfTradePreventionId?.ToOkxString());
        parameters.AddOptionalParameter("stpMode", JsonConvert.SerializeObject(selfTradePreventionMode, new OkxSelfTradePreventionModeConverter(false)));

        return ProcessOneRequestAsync<OkxOrderPlaceResponse>(GetUri(v5TradeOrder), HttpMethod.Post, ct, signed: true, bodyParameters: parameters);
    }

    /// <summary>
    /// Place orders in batches. Maximum 20 orders can be placed at a time. Request parameters should be passed in the form of an array.
    /// </summary>
    /// <param name="orders">Orders</param>
    /// <param name="ct">Cancellation Token</param>
    /// <returns></returns>
    public Task<RestCallResult<List<OkxOrderPlaceResponse>>> PlaceOrdersAsync(IEnumerable<OkxOrderPlaceRequest> orders, CancellationToken ct = default)
    {
        foreach (var order in orders) order.Tag = OkxConstants.BrokerId;
        var parameters = new Dictionary<string, object>
        {
            { Options.RequestBodyParameterKey, orders },
        };

        return ProcessListRequestAsync<OkxOrderPlaceResponse>(GetUri(v5TradeBatchOrders), HttpMethod.Post, ct, signed: true, bodyParameters: parameters);
    }

    /// <summary>
    /// Cancel an incomplete order.
    /// </summary>
    /// <param name="instrumentId">Instrument ID</param>
    /// <param name="orderId">Order ID</param>
    /// <param name="clientOrderId">Client Order ID</param>
    /// <param name="ct">Cancellation Token</param>
    /// <returns></returns>
    public Task<RestCallResult<OkxOrderCancelResponse>> CancelOrderAsync(string instrumentId, long? orderId = null, string clientOrderId = null, CancellationToken ct = default)
    {
        var parameters = new Dictionary<string, object> {
            {"instId", instrumentId },
        };
        parameters.AddOptionalParameter("ordId", orderId?.ToOkxString());
        parameters.AddOptionalParameter("clOrdId", clientOrderId);

        return ProcessOneRequestAsync<OkxOrderCancelResponse>(GetUri(v5TradeCancelOrder), HttpMethod.Post, ct, signed: true, bodyParameters: parameters);
    }

    /// <summary>
    /// Cancel incomplete orders in batches. Maximum 20 orders can be canceled at a time. Request parameters should be passed in the form of an array.
    /// </summary>
    /// <param name="orders">Orders</param>
    /// <param name="ct">Cancellation Token</param>
    /// <returns></returns>
    public Task<RestCallResult<List<OkxOrderCancelResponse>>> CancelOrdersAsync(IEnumerable<OkxOrderCancelRequest> orders, CancellationToken ct = default)
    {
        var parameters = new Dictionary<string, object> {
            { Options.RequestBodyParameterKey, orders },
        };

        return ProcessListRequestAsync<OkxOrderCancelResponse>(GetUri(v5TradeCancelBatchOrders), HttpMethod.Post, ct, signed: true, bodyParameters: parameters);
    }

    /// <summary>
    /// Amend an incomplete order.
    /// </summary>
    /// <param name="instrumentId">Instrument ID</param>
    /// <param name="orderId">Order ID</param>
    /// <param name="clientOrderId">Client Order ID</param>
    /// <param name="requestId">Request ID</param>
    /// <param name="cancelOnFail">Cancel On Fail</param>
    /// <param name="newQuantity">New Quantity</param>
    /// <param name="newPrice">New Price</param>
    /// 
    /// <param name="newTakeProfitTriggerPriceType">Take-profit trigger price. Either the take profit trigger price or order price is 0, it means that the take profit is deleted. Only applicable to Futures and Perpetual swap.</param>
    /// <param name="newTakeProfitTriggerPrice">Take-profit order price. If the price is -1, take-profit will be executed at the market price. Only applicable to Futures and Perpetual swap.</param>
    /// <param name="newTakeProfitOrderPrice">Take-profit order price. If the price is -1, take-profit will be executed at the market price. Only applicable to Futures and Perpetual swap.</param>
    ///
    /// <param name="newStopLossTriggerPriceType">Price</param>
    /// <param name="newStopLossTriggerPrice">Stop-loss trigger price. Either the stop loss trigger price or order price is 0, it means that the stop loss is deleted. Only applicable to Futures and Perpetual swap.</param>
    /// <param name="newStopLossOrderPrice">Stop-loss order price. If the price is -1, stop-loss will be executed at the market price. Only applicable to Futures and Perpetual swap.</param>
    /// 
    /// <param name="ct">Cancellation Token</param>
    /// <returns></returns>
    public Task<RestCallResult<OkxOrderAmendResponse>> AmendOrderAsync(
        string instrumentId,
        long? orderId = null,
        string clientOrderId = null,
        string requestId = null,
        bool? cancelOnFail = null,
        decimal? newQuantity = null,
        decimal? newPrice = null,

        OkxAlgoPriceType? newTakeProfitTriggerPriceType = null,
        decimal? newTakeProfitTriggerPrice = null,
        decimal? newTakeProfitOrderPrice = null,

        OkxAlgoPriceType? newStopLossTriggerPriceType = null,
        decimal? newStopLossTriggerPrice = null,
        decimal? newStopLossOrderPrice = null,
        CancellationToken ct = default)
    {
        var parameters = new Dictionary<string, object>
        {
            { "instId", instrumentId },
        };
        parameters.AddOptionalParameter("cxlOnFail", cancelOnFail);
        parameters.AddOptionalParameter("ordId", orderId?.ToOkxString());
        parameters.AddOptionalParameter("clOrdId", clientOrderId);
        parameters.AddOptionalParameter("reqId", requestId);
        parameters.AddOptionalParameter("newSz", newQuantity?.ToOkxString());
        parameters.AddOptionalParameter("newPx", newPrice?.ToOkxString());

        parameters.AddOptionalParameter("newTpTriggerPxType", JsonConvert.SerializeObject(newTakeProfitTriggerPriceType, new OkxAlgoPriceTypeConverter(false)));
        parameters.AddOptionalParameter("newTpTriggerPx", newTakeProfitTriggerPrice?.ToOkxString());
        parameters.AddOptionalParameter("newTpOrdPx", newTakeProfitOrderPrice?.ToOkxString());

        parameters.AddOptionalParameter("newSlTriggerPxType", JsonConvert.SerializeObject(newStopLossTriggerPriceType, new OkxAlgoPriceTypeConverter(false)));
        parameters.AddOptionalParameter("newSlTriggerPx", newStopLossTriggerPrice?.ToOkxString());
        parameters.AddOptionalParameter("newSlOrdPx", newStopLossOrderPrice?.ToOkxString());

        return ProcessOneRequestAsync<OkxOrderAmendResponse>(GetUri(v5TradeAmendOrder), HttpMethod.Post, ct, signed: true, bodyParameters: parameters);
    }

    /// <summary>
    /// Amend incomplete orders in batches. Maximum 20 orders can be amended at a time. Request parameters should be passed in the form of an array.
    /// </summary>
    /// <param name="orders">Orders</param>
    /// <param name="ct">Cancellation Token</param>
    /// <returns></returns>
    public Task<RestCallResult<List<OkxOrderAmendResponse>>> AmendOrdersAsync(IEnumerable<OkxOrderAmendRequest> orders, CancellationToken ct = default)
    {
        var parameters = new Dictionary<string, object> {
            { Options.RequestBodyParameterKey, orders },
        };

        return ProcessListRequestAsync<OkxOrderAmendResponse>(GetUri(v5TradeAmendBatchOrders), HttpMethod.Post, ct, signed: true, bodyParameters: parameters);
    }

    /// <summary>
    /// Close all positions of an instrument via a market order.
    /// </summary>
    /// <param name="instrumentId">Instrument ID</param>
    /// <param name="marginMode">Margin Mode</param>
    /// <param name="positionSide">Position Side</param>
    /// <param name="currency">Currency</param>
    /// <param name="autoCxl">Whether any pending orders for closing out needs to be automatically canceled when close position via a market order.</param>
    /// <param name="clientOrderId">Client-supplied ID. A combination of case-sensitive alphanumerics, all numbers, or all letters of up to 32 characters.</param>
    /// <param name="ct">Cancellation Token</param>
    /// <returns></returns>
    public Task<RestCallResult<OkxClosePositionResponse>> ClosePositionAsync(
        string instrumentId,
        OkxAccountMarginMode marginMode,
        OkxPositionSide? positionSide = null,
        string currency = null,
        bool? autoCxl = null,
        string clientOrderId = null,
        CancellationToken ct = default)
    {
        var parameters = new Dictionary<string, object> {
            {"tag", OkxConstants.BrokerId },
            {"instId", instrumentId },
            {"mgnMode", JsonConvert.SerializeObject(marginMode, new OkxAccountMarginModeConverter(false)) },
        };
        parameters.AddOptionalParameter("posSide", JsonConvert.SerializeObject(positionSide, new OkxPositionSideConverter(false)));
        parameters.AddOptionalParameter("ccy", currency);
        parameters.AddOptionalParameter("autoCxl", autoCxl);
        parameters.AddOptionalParameter("clOrdId", clientOrderId);

        return ProcessOneRequestAsync<OkxClosePositionResponse>(GetUri(v5TradeClosePosition), HttpMethod.Post, ct, signed: true, bodyParameters: parameters);
    }

    /// <summary>
    /// Retrieve order details.
    /// </summary>
    /// <param name="instrumentId">Instrument ID</param>
    /// <param name="orderId">Order ID</param>
    /// <param name="clientOrderId">Client Order ID</param>
    /// <param name="ct">Cancellation Token</param>
    /// <returns></returns>
    public Task<RestCallResult<OkxOrder>> GetOrderAsync(
        string instrumentId,
        long? orderId = null,
        string clientOrderId = null,
        CancellationToken ct = default)
    {
        var parameters = new Dictionary<string, object> {
            {"instId", instrumentId },
        };
        parameters.AddOptionalParameter("ordId", orderId?.ToOkxString());
        parameters.AddOptionalParameter("clOrdId", clientOrderId);

        return ProcessOneRequestAsync<OkxOrder>(GetUri(v5TradeOrderGet), HttpMethod.Get, ct, signed: true, queryParameters: parameters);
    }

    /// <summary>
    /// Retrieve all incomplete orders under the current account.
    /// </summary>
    /// <param name="instrumentType">Instrument Type</param>
    /// <param name="instrumentId">Instrument ID</param>
    /// <param name="instFamily">Instrument family</param>
    /// <param name="underlying">Underlying</param>
    /// <param name="orderType">Order Type</param>
    /// <param name="state">State</param>
    /// <param name="after">Pagination of data to return records later than the requested ordId</param>
    /// <param name="before">Pagination of data to return records earlier than the requested ordId</param>
    /// <param name="limit">Number of results per request. The maximum is 100; the default is 100.</param>
    /// <param name="ct">Cancellation Token</param>
    /// <returns></returns>
    public Task<RestCallResult<List<OkxOrder>>> GetOpenOrdersAsync(
        OkxInstrumentType? instrumentType = null,
        string instrumentId = null,
        string instFamily = null,
        string underlying = null,
        OkxOrderType? orderType = null,
        OkxOrderState? state = null,
        long? after = null,
        long? before = null,
        int limit = 100,
        CancellationToken ct = default)
    {
        limit.ValidateIntBetween(nameof(limit), 1, 100);
        var parameters = new Dictionary<string, object>();
        parameters.AddOptionalParameter("instType", JsonConvert.SerializeObject(instrumentType, new OkxInstrumentTypeConverter(false)));
        parameters.AddOptionalParameter("uly", underlying);
        parameters.AddOptionalParameter("instFamily", instFamily);
        parameters.AddOptionalParameter("instId", instrumentId);
        parameters.AddOptionalParameter("ordType", JsonConvert.SerializeObject(orderType, new OkxOrderTypeConverter(false)));
        parameters.AddOptionalParameter("state", JsonConvert.SerializeObject(state, new OkxOrderStateConverter(false)));
        parameters.AddOptionalParameter("after", after?.ToOkxString());
        parameters.AddOptionalParameter("before", before?.ToOkxString());
        parameters.AddOptionalParameter("limit", limit.ToOkxString());

        return ProcessListRequestAsync<OkxOrder>(GetUri(v5TradeOrdersPending), HttpMethod.Get, ct, signed: true, queryParameters: parameters);
    }

    /// <summary>
    /// Retrieve the completed order data for the last 7 days, and the incomplete orders that have been cancelled are only reserved for 2 hours.
    /// </summary>
    /// <param name="instrumentType">Instrument Type</param>
    /// <param name="instrumentId">Instrument ID</param>
    /// <param name="instFamily">Instrument family</param>
    /// <param name="underlying">Underlying</param>
    /// <param name="orderType">Order Type</param>
    /// <param name="state">State</param>
    /// <param name="category">Category</param>
    /// <param name="after">Pagination of data to return records later than the requested order id</param>
    /// <param name="before">Pagination of data to return records earlier than the requested order id</param>
    /// <param name="begin">Filter with a begin timestamp. Unix timestamp format in milliseconds, e.g. 1597026383085</param>
    /// <param name="end">Filter with a begin timestamp. Unix timestamp format in milliseconds, e.g. 1597026383085</param>
    /// <param name="limit">Number of results per request. The maximum is 100; the default is 100.</param>
    /// <param name="ct">Cancellation Token</param>
    /// <returns></returns>
    public Task<RestCallResult<List<OkxOrder>>> GetOrderHistoryAsync(
        OkxInstrumentType instrumentType,
        string instrumentId = null,
        string instFamily = null,
        string underlying = null,
        OkxOrderType? orderType = null,
        OkxOrderState? state = null,
        OkxOrderCategory? category = null,
        long? after = null,
        long? before = null,
        long? begin = null,
        long? end = null,
        int limit = 100,
        CancellationToken ct = default)
    {
        limit.ValidateIntBetween(nameof(limit), 1, 100);
        var parameters = new Dictionary<string, object>
        {
            {"instType", JsonConvert.SerializeObject(instrumentType, new OkxInstrumentTypeConverter(false))}
        };
        parameters.AddOptionalParameter("uly", underlying);
        parameters.AddOptionalParameter("instFamily", instFamily);
        parameters.AddOptionalParameter("instId", instrumentId);
        parameters.AddOptionalParameter("ordType", JsonConvert.SerializeObject(orderType, new OkxOrderTypeConverter(false)));
        parameters.AddOptionalParameter("state", JsonConvert.SerializeObject(state, new OkxOrderStateConverter(false)));
        parameters.AddOptionalParameter("category", JsonConvert.SerializeObject(category, new OkxOrderCategoryConverter(false)));
        parameters.AddOptionalParameter("after", after?.ToOkxString());
        parameters.AddOptionalParameter("before", before?.ToOkxString());
        parameters.AddOptionalParameter("begin", begin?.ToOkxString());
        parameters.AddOptionalParameter("end", end?.ToOkxString());
        parameters.AddOptionalParameter("limit", limit.ToOkxString());

        return ProcessListRequestAsync<OkxOrder>(GetUri(v5TradeOrdersHistory), HttpMethod.Get, ct, signed: true, queryParameters: parameters);
    }

    /// <summary>
    /// Retrieve the completed order data of the last 3 months, and the incomplete orders that have been canceled are only reserved for 2 hours.
    /// </summary>
    /// <param name="instrumentType">Instrument Type</param>
    /// <param name="instrumentId">Instrument ID</param>
    /// <param name="instFamily">Instrument family</param>
    /// <param name="underlying">Underlying</param>
    /// <param name="orderType">Order Type</param>
    /// <param name="state">State</param>
    /// <param name="category">Category</param>
    /// <param name="after">Pagination of data to return records later than the requested order id</param>
    /// <param name="before">Pagination of data to return records earlier than the requested order id</param>
    /// <param name="begin">Filter with a begin timestamp. Unix timestamp format in milliseconds, e.g. 1597026383085</param>
    /// <param name="end">Filter with a begin timestamp. Unix timestamp format in milliseconds, e.g. 1597026383085</param>
    /// <param name="limit">Number of results per request. The maximum is 100; the default is 100.</param>
    /// <param name="ct">Cancellation Token</param>
    /// <returns></returns>
    public Task<RestCallResult<List<OkxOrder>>> GetOrderArchiveAsync(
        OkxInstrumentType instrumentType,
        string instrumentId = null,
        string instFamily = null,
        string underlying = null,
        OkxOrderType? orderType = null,
        OkxOrderState? state = null,
        OkxOrderCategory? category = null,
        long? after = null,
        long? before = null,
        long? begin = null,
        long? end = null,
        int limit = 100,
        CancellationToken ct = default)
    {
        limit.ValidateIntBetween(nameof(limit), 1, 100);
        var parameters = new Dictionary<string, object>
        {
            {"instType", JsonConvert.SerializeObject(instrumentType, new OkxInstrumentTypeConverter(false))}
        };
        parameters.AddOptionalParameter("uly", underlying);
        parameters.AddOptionalParameter("instFamily", instFamily);
        parameters.AddOptionalParameter("instId", instrumentId);
        parameters.AddOptionalParameter("ordType", JsonConvert.SerializeObject(orderType, new OkxOrderTypeConverter(false)));
        parameters.AddOptionalParameter("state", JsonConvert.SerializeObject(state, new OkxOrderStateConverter(false)));
        parameters.AddOptionalParameter("category", JsonConvert.SerializeObject(category, new OkxOrderCategoryConverter(false)));
        parameters.AddOptionalParameter("after", after?.ToOkxString());
        parameters.AddOptionalParameter("before", before?.ToOkxString());
        parameters.AddOptionalParameter("begin", begin?.ToOkxString());
        parameters.AddOptionalParameter("end", end?.ToOkxString());
        parameters.AddOptionalParameter("limit", limit.ToOkxString());

        return ProcessListRequestAsync<OkxOrder>(GetUri(v5TradeOrdersHistoryArchive), HttpMethod.Get, ct, signed: true, queryParameters: parameters);
    }

    /// <summary>
    /// Retrieve recently-filled transaction details in the last 3 day.
    /// </summary>
    /// <param name="instrumentType">Instrument Type</param>
    /// <param name="instrumentId">Instrument ID</param>
    /// <param name="instFamily">Instrument family</param>
    /// <param name="underlying">Underlying</param>
    /// <param name="orderId">Order ID</param>
    /// <param name="after">Pagination of data to return records later than the requested <see cref="OkxTransaction.BillId"/></param>
    /// <param name="before">Pagination of data to return records earlier than the requested <see cref="OkxTransaction.BillId"/></param>
    /// <param name="begin">Filter with a begin timestamp. Unix timestamp format in milliseconds, e.g. 1597026383085</param>
    /// <param name="end">Filter with a begin timestamp. Unix timestamp format in milliseconds, e.g. 1597026383085</param>
    /// <param name="limit">Number of results per request. The maximum is 100; the default is 100.</param>
    /// <param name="ct">Cancellation Token</param>
    /// <returns></returns>
    public Task<RestCallResult<List<OkxTransaction>>> GetTradesAsync(
        OkxInstrumentType? instrumentType = null,
        string instrumentId = null,
        string instFamily = null,
        string underlying = null,
        long? orderId = null,
        long? after = null,
        long? before = null,
        long? begin = null,
        long? end = null,
        int limit = 100,
        CancellationToken ct = default)
    {
        limit.ValidateIntBetween(nameof(limit), 1, 100);
        var parameters = new Dictionary<string, object>();
        parameters.AddOptionalParameter("instType", JsonConvert.SerializeObject(instrumentType, new OkxInstrumentTypeConverter(false)));
        parameters.AddOptionalParameter("uly", underlying);
        parameters.AddOptionalParameter("instFamily", instFamily);
        parameters.AddOptionalParameter("instId", instrumentId);
        parameters.AddOptionalParameter("ordId", orderId?.ToOkxString());
        parameters.AddOptionalParameter("after", after?.ToOkxString());
        parameters.AddOptionalParameter("before", before?.ToOkxString());
        parameters.AddOptionalParameter("begin", begin?.ToOkxString());
        parameters.AddOptionalParameter("end", end?.ToOkxString());
        parameters.AddOptionalParameter("limit", limit.ToOkxString());

        return ProcessListRequestAsync<OkxTransaction>(GetUri(v5TradeFills), HttpMethod.Get, ct, signed: true, queryParameters: parameters);
    }

    /// <summary>
    /// Retrieve recently-filled transaction details in the last 3 months.
    /// </summary>
    /// <param name="instrumentType">Instrument Type</param>
    /// <param name="instrumentId">Instrument ID</param>
    /// <param name="instFamily">Instrument family</param>
    /// <param name="underlying">Underlying</param>
    /// <param name="orderId">Order ID</param>
    /// <param name="after">Pagination of data to return records later than the requested <see cref="OkxTransaction.BillId"/></param>
    /// <param name="before">Pagination of data to return records earlier than the requested <see cref="OkxTransaction.BillId"/></param>
    /// <param name="begin">Filter with a begin timestamp. Unix timestamp format in milliseconds, e.g. 1597026383085</param>
    /// <param name="end">Filter with a begin timestamp. Unix timestamp format in milliseconds, e.g. 1597026383085</param>
    /// <param name="limit">Number of results per request. The maximum is 100; the default is 100.</param>
    /// <param name="ct">Cancellation Token</param>
    /// <returns>RestCallResult containing enumerable OkxTransaction list</returns>
    public Task<RestCallResult<List<OkxTransaction>>> GetTradesHistoryAsync(
        OkxInstrumentType instrumentType,
        string instrumentId = null,
        string instFamily = null,
        string underlying = null,
        long? orderId = null,
        long? after = null,
        long? before = null,
        long? begin = null,
        long? end = null,
        int limit = 100,
        CancellationToken ct = default)
    {
        limit.ValidateIntBetween(nameof(limit), 1, 100);
        var parameters = new Dictionary<string, object>();
        parameters.AddOptionalParameter("instType", JsonConvert.SerializeObject(instrumentType, new OkxInstrumentTypeConverter(false)));
        parameters.AddOptionalParameter("uly", underlying);
        parameters.AddOptionalParameter("instFamily", instFamily);
        parameters.AddOptionalParameter("instId", instrumentId);
        parameters.AddOptionalParameter("ordId", orderId?.ToOkxString());
        parameters.AddOptionalParameter("after", after?.ToOkxString());
        parameters.AddOptionalParameter("before", before?.ToOkxString());
        parameters.AddOptionalParameter("begin", begin?.ToOkxString());
        parameters.AddOptionalParameter("end", end?.ToOkxString());
        parameters.AddOptionalParameter("limit", limit.ToOkxString());

        return ProcessListRequestAsync<OkxTransaction>(GetUri(v5TradeFillsHistory), HttpMethod.Get, ct, signed: true, queryParameters: parameters);
    }

    /// <summary>
    /// Apply for recently-filled transaction details in the past 2 years except for last 3 months.
    /// </summary>
    /// <param name="year">4 digits year</param>
    /// <param name="quarter">Quarter, valid value is Q1, Q2, Q3, Q4</param>
    /// <param name="ct">Cancellation Token</param>
    /// <returns></returns>
    public Task<RestCallResult<OkxTimestamp>> ApplyTradesArchiveAsync(
        int year,
        OkxQuarter quarter,
        CancellationToken ct = default)
    {
        var parameters = new Dictionary<string, object> {
            {"year", year.ToOkxString() },
            {"quarter", JsonConvert.SerializeObject(quarter, new OkxQuarterConverter(false)) },
        };

        return ProcessOneRequestAsync<OkxTimestamp>(GetUri(v5TradeFillsArchive), HttpMethod.Post, ct, signed: true, bodyParameters: parameters);
    }

    /// <summary>
    /// Retrieve recently-filled transaction details in the past 2 years except for last 3 months.
    /// </summary>
    /// <param name="year">4 digits year</param>
    /// <param name="quarter">Quarter, valid value is Q1, Q2, Q3, Q4</param>
    /// <param name="ct">Cancellation Token</param>
    /// <returns></returns>
    public Task<RestCallResult<OkxDownloadLink>> GetTradesArchiveAsync(
        int year,
        OkxQuarter quarter,
        CancellationToken ct = default)
    {
        var parameters = new Dictionary<string, object> {
            {"year", year.ToOkxString() },
            {"quarter", JsonConvert.SerializeObject(quarter, new OkxQuarterConverter(false)) },
        };

        return ProcessOneRequestAsync<OkxDownloadLink>(GetUri(v5TradeFillsArchive), HttpMethod.Get, ct, signed: true, queryParameters: parameters);
    }

    public Task<RestCallResult<OkxEasyConvertCurrencyList>> GetEasyConvertCurrenciesAsync(CancellationToken ct = default)
    {
        return ProcessOneRequestAsync<OkxEasyConvertCurrencyList>(GetUri(v5TradeEasyConvertCurrencyList), HttpMethod.Get, ct, signed: true);
    }

    public Task<RestCallResult<List<OkxEasyConvertOrder>>> PlaceEasyConvertOrderAsync(IEnumerable<string> fromCcy, string toCcy, CancellationToken ct = default)
    {
        var parameters = new Dictionary<string, object> {
            {"fromCcy", fromCcy },
            {"toCcy", toCcy },
        };

        return ProcessListRequestAsync<OkxEasyConvertOrder>(GetUri(v5TradeEasyConvert), HttpMethod.Post, ct, signed: true, bodyParameters: parameters);
    }

    public Task<RestCallResult<List<OkxEasyConvertOrderHistory>>> GetEasyConvertHistoryAsync(long? after = null, long? before = null, int limit = 100, CancellationToken ct = default)
    {
        limit.ValidateIntBetween(nameof(limit), 1, 100);
        var parameters = new Dictionary<string, object>();
        parameters.AddOptionalParameter("after", after?.ToOkxString());
        parameters.AddOptionalParameter("before", before?.ToOkxString());
        parameters.AddOptionalParameter("limit", limit.ToOkxString());

        return ProcessListRequestAsync<OkxEasyConvertOrderHistory>(GetUri(v5TradeEasyConvertHistory), HttpMethod.Get, ct, signed: true, queryParameters: parameters);
    }

    public Task<RestCallResult<List<OkxOneClickRepayCurrencyList>>> GetOneClickRepayCurrenciesAsync(OkxDebtType debtType, CancellationToken ct = default)
    {
        var parameters = new Dictionary<string, object>();
        parameters.AddOptionalParameter("debtType", JsonConvert.SerializeObject(debtType, new OkxDebtTypeConverter(false)));

        return ProcessListRequestAsync<OkxOneClickRepayCurrencyList>(GetUri(v5TradeOneClickRepayCurrencyList), HttpMethod.Get, ct, signed: true, parameters);
    }

    public Task<RestCallResult<List<OkxOneClickRepayOrder>>> PlaceOneClickRepayOrderAsync(IEnumerable<string> debtCcy, string repayCcy, CancellationToken ct = default)
    {
        var parameters = new Dictionary<string, object> {
            {"debtCcy", debtCcy },
            {"repayCcy", repayCcy },
        };

        return ProcessListRequestAsync<OkxOneClickRepayOrder>(GetUri(v5TradeOneClickRepay), HttpMethod.Post, ct, signed: true, bodyParameters: parameters);
    }

    public Task<RestCallResult<List<OkxOneClickRepayOrderHistory>>> GetOneClickRepayHistoryAsync(long? after = null, long? before = null, int limit = 100, CancellationToken ct = default)
    {
        limit.ValidateIntBetween(nameof(limit), 1, 100);
        var parameters = new Dictionary<string, object>();
        parameters.AddOptionalParameter("after", after?.ToOkxString());
        parameters.AddOptionalParameter("before", before?.ToOkxString());
        parameters.AddOptionalParameter("limit", limit.ToOkxString());

        return ProcessListRequestAsync<OkxOneClickRepayOrderHistory>(GetUri(v5TradeOneClickRepayHistory), HttpMethod.Get, ct, signed: true, queryParameters: parameters);
    }

    public Task<RestCallResult<OkxMassCancelResponse>> MassCancelAsync(OkxInstrumentType instrumentType, string instrumentFamily, CancellationToken ct = default)
    {
        var parameters = new Dictionary<string, object> {
            {"instType", JsonConvert.SerializeObject(instrumentType, new OkxInstrumentTypeConverter(false)) },
            {"instFamily", instrumentFamily },
        };

        return ProcessOneRequestAsync<OkxMassCancelResponse>(GetUri(v5TradeMassCancel), HttpMethod.Post, ct, signed: true, bodyParameters: parameters);
    }

    public Task<RestCallResult<OkxCancelAllAfterResponse>> CancelAllAfterAsync(int timeout, string tag, CancellationToken ct = default)
    {
        var parameters = new Dictionary<string, object> {
            {"timeOut", timeout.ToOkxString() },
        };
        parameters.AddOptionalParameter("tag", tag);

        return ProcessOneRequestAsync<OkxCancelAllAfterResponse>(GetUri(v5TradeCancelAllAfter), HttpMethod.Post, ct, signed: true, bodyParameters: parameters);
    }

    public Task<RestCallResult<OkxAccountRateLimit>> GetAccountRateLimitAsync(CancellationToken ct = default)
    {
        return ProcessOneRequestAsync<OkxAccountRateLimit>(GetUri(v5TradeAccountRateLimit), HttpMethod.Get, ct, signed: true);
    }

}