﻿using OKX.Api.Trade.Models;

namespace OKX.Api.Trade.Clients;

/// <summary>
/// OKX WebSocket Api Trade Client
/// </summary>
public class OkxTradingSocketClient(OKXWebSocketApiClient root)
{
    // Internal
    internal OKXWebSocketApiClient Root { get; } = root;
    internal OkxWebSocketApiOptions Options { get { return Root.Options; } }

    /// <summary>
    /// Retrieve order information. Data will not be pushed when first subscribed. Data will only be pushed when triggered by events such as placing/canceling order.
    /// </summary>
    /// <param name="onData">On Data Handler</param>
    /// <param name="instrumentType">Instrument Type</param>
    /// <param name="instrumentFamily">Instrument Family</param>
    /// <param name="instrumentId">Instrument ID</param>
    /// <param name="ct">Cancellation Token</param>
    /// <returns></returns>
    public async Task<CallResult<WebSocketUpdateSubscription>> SubscribeToOrderUpdatesAsync(
        Action<OkxOrder> onData,
        OkxInstrumentType instrumentType,
        string instrumentFamily = null,
        string instrumentId = null,
        CancellationToken ct = default)
        => await SubscribeToOrderUpdatesAsync(onData, [new(instrumentType, instrumentFamily, instrumentId)], ct).ConfigureAwait(false);

    /// <summary>
    /// Retrieve order information. Data will not be pushed when first subscribed. Data will only be pushed when there are order updates.
    /// </summary>
    /// <param name="onData">On Data Handler</param>
    /// <param name="symbols">Symbols to subscribe</param>
    /// <param name="ct">Cancellation Token</param>
    /// <returns></returns>
    public async Task<CallResult<WebSocketUpdateSubscription>> SubscribeToOrderUpdatesAsync(
        Action<OkxOrder> onData,
        IEnumerable<OkxSocketSymbolRequest> symbols,
        CancellationToken ct = default)
    {
        var internalHandler = new Action<WebSocketDataEvent<OkxSocketUpdateResponse<List<OkxOrder>>>>(data =>
        {
            foreach (var d in data.Data.Data)
                if (d is not null) onData(d);
        });

        var arguments = new List<OkxSocketRequestArgument>();
        foreach (var symbol in symbols) arguments.Add(new OkxSocketRequestArgument
        {
            Channel = "orders",
            InstrumentId = symbol.InstrumentId,
            InstrumentType = symbol.InstrumentType,
            InstrumentFamily = symbol.InstrumentFamily,
        });
        var request = new OkxSocketRequest(OkxSocketOperation.Subscribe, arguments);
        return await Root.RootSubscribeAsync(OkxSocketEndpoint.Private, request, null, true, internalHandler, ct).ConfigureAwait(false);
    }

    /// <summary>
    /// You can place an order only if you have sufficient funds.
    /// </summary>
    /// <param name="request">Request</param>
    /// <returns></returns>
    public async Task<CallResult<OkxOrderPlaceResponse>> PlaceOrderAsync(OkxOrderPlaceRequest request)
    {
        request.Tag = OkxConstants.BrokerId;
        var req = new OkxSocketRequest<OkxOrderPlaceRequest>(Root.RequestId().ToString(), OkxSocketOperation.Order, [request]);
        return await Root.RootQueryAsync<OkxOrderPlaceResponse>(OkxSocketEndpoint.Private, req, true).ConfigureAwait(false);
    }

    /// <summary>
    /// Place orders in a batch. Maximum 20 orders can be placed per request
    /// </summary>
    /// <param name="requests">Requests</param>
    /// <returns></returns>
    public async Task<CallResult<IEnumerable<OkxOrderPlaceResponse>>> PlaceOrdersAsync(IEnumerable<OkxOrderPlaceRequest> requests)
    {
        foreach (var order in requests) order.Tag = OkxConstants.BrokerId;
        var req = new OkxSocketRequest<OkxOrderPlaceRequest>(Root.RequestId().ToString(), OkxSocketOperation.BatchOrders, requests);
        return await Root.RootQueryAsync<IEnumerable<OkxOrderPlaceResponse>>(OkxSocketEndpoint.Private, req, true).ConfigureAwait(false);
    }

    /// <summary>
    /// Cancel an incomplete order
    /// </summary>
    /// <param name="request">Request</param>
    /// <returns></returns>
    public async Task<CallResult<OkxOrderCancelResponse>> CancelOrderAsync(OkxOrderCancelRequest request)
    {
        var req = new OkxSocketRequest<OkxOrderCancelRequest>(Root.RequestId().ToString(), OkxSocketOperation.CancelOrder, [request]);
        return await Root.RootQueryAsync<OkxOrderCancelResponse>(OkxSocketEndpoint.Private, req, true).ConfigureAwait(false);
    }

    /// <summary>
    /// Cancel incomplete orders in batches. Maximum 20 orders can be canceled per request.
    /// </summary>
    /// <param name="requests">Requests</param>
    /// <returns></returns>
    public async Task<CallResult<IEnumerable<OkxOrderCancelResponse>>> CancelOrdersAsync(IEnumerable<OkxOrderCancelRequest> requests)
    {
        var req = new OkxSocketRequest<OkxOrderCancelRequest>(Root.RequestId().ToString(), OkxSocketOperation.BatchAmendOrders, requests);
        return await Root.RootQueryAsync<IEnumerable<OkxOrderCancelResponse>>(OkxSocketEndpoint.Private, req, true).ConfigureAwait(false);
    }

    /// <summary>
    /// Amend an incomplete order.
    /// </summary>
    /// <param name="request">Request</param>
    /// <returns></returns>
    public async Task<CallResult<OkxOrderAmendResponse>> AmendOrderAsync(OkxOrderAmendRequest request)
    {
        var req = new OkxSocketRequest<OkxOrderAmendRequest>(Root.RequestId().ToString(), OkxSocketOperation.AmendOrder, [request]);
        return await Root.RootQueryAsync<OkxOrderAmendResponse>(OkxSocketEndpoint.Private, req, true).ConfigureAwait(false);
    }

    /// <summary>
    /// Amend incomplete orders in batches. Maximum 20 orders can be amended per request.
    /// </summary>
    /// <param name="requests">Requests</param>
    /// <returns></returns>
    public async Task<CallResult<IEnumerable<OkxOrderAmendResponse>>> AmendOrdersAsync(IEnumerable<OkxOrderAmendRequest> requests)
    {
        var req = new OkxSocketRequest<OkxOrderAmendRequest>(Root.RequestId().ToString(), OkxSocketOperation.BatchAmendOrders, requests);
        return await Root.RootQueryAsync<IEnumerable<OkxOrderAmendResponse>>(OkxSocketEndpoint.Private, req, true).ConfigureAwait(false);
    }

    /// <summary>
    /// Cancel all the MMP pending orders of an instrument family.
    /// Only applicable to Option in Portfolio Margin mode, and MMP privilege is required.
    /// </summary>
    /// <param name="request">Request</param>
    /// <returns></returns>
    public async Task<CallResult<OkxMassCancelResponse>> MassCancelAsync(OkxMassCancelRequest request)
    {
        var req = new OkxSocketRequest<OkxMassCancelRequest>(Root.RequestId().ToString(), OkxSocketOperation.MassCancel, [request]);
        return await Root.RootQueryAsync<OkxMassCancelResponse>(OkxSocketEndpoint.Private, req, true).ConfigureAwait(false);
    }

}