﻿using CommunityToolkit.Mvvm.ComponentModel;
using Controls.UserDialogs.Maui;
using Deppo.Core.BaseModels;
using Deppo.Core.Services;
using Deppo.Mobile.Core.Models.SalesModels;
using Deppo.Mobile.Helpers.HttpClientHelpers;
using Deppo.Mobile.Helpers.MappingHelper;
using Deppo.Mobile.Helpers.MVVMHelper;
using DevExpress.Maui.Controls;
using System.Collections.ObjectModel;

namespace Deppo.Mobile.Modules.SalesModule.SalesPanel.ViewModels
{
    public partial class SalesPanelShippedProductListViewModel : BaseViewModel
    {
        private readonly IHttpClientService _httpClientService;
        private readonly ISalesPanelService _salesPanelService;
        private readonly IUserDialogs _userDialogs;

        [ObservableProperty]
        public Product selectedItem;

        public ObservableCollection<Product> Items { get; } = new();

        public ObservableCollection<SalesTransactionModel> ShippedOrders { get; } = new();

        public SalesPanelShippedProductListViewModel(IHttpClientService httpClientService, IUserDialogs userDialogs, ISalesPanelService salesPanelService)
        {
            _httpClientService = httpClientService;
            _salesPanelService = salesPanelService;
            _userDialogs = userDialogs;

            Title = "Sevkedilen Ürün Listesi";

            LoadItemsCommand = new Command(async () => await LoadItemsAsync());
            LoadMoreItemsCommand = new Command(async () => await LoadMoreItemsAsync());
            ItemTappedCommand = new Command<Product>(async (item) => await ItemTappedAsync(item));
            ShippedOrdersCloseCommand = new Command(async () => await ShippedOrdersCloseAsync());
            LoadMoreShippedOrdersCommand = new Command(async () => await LoadMoreShippedOrdersAsync());
            BackCommand = new Command(async () => await BackAsync());
        }

        public Command LoadItemsCommand { get; }
        public Command LoadMoreItemsCommand { get; }
        public Command ItemTappedCommand { get; }
        public Command NextViewCommand { get; }
        public Command ShippedOrdersCloseCommand { get; }
        public Command LoadMoreShippedOrdersCommand { get; }
        public Command BackCommand { get; }

        public Page CurrentPage { get; set; }

        private async Task LoadItemsAsync()
        {
            if (IsBusy)
                return;

            try
            {
                IsBusy = true;

                _userDialogs.ShowLoading("Loading...");
                Items.Clear();
                await Task.Delay(1000);
                var httpClient = _httpClientService.GetOrCreateHttpClient();
                var result = await _salesPanelService.GetShippedProducts(httpClient, _httpClientService.FirmNumber, _httpClientService.PeriodNumber, string.Empty, 0, 20);
                if (result.IsSuccess)
                {
                    if (result.Data is not null)
                    {
                        foreach (var item in result.Data)
                        {
                            var waitingOrder = Mapping.Mapper.Map<SalesTransactionModel>(item);
                            Items.Add(new Product
                            {
                                ReferenceId = waitingOrder.ProductReferenceId,
                                Code = waitingOrder.ProductCode,
                                Name = waitingOrder.ProductName,
                                StockQuantity = waitingOrder.Quantity,
                                UnitsetReferenceId = waitingOrder.UnitsetReferenceId,
                                UnitsetName = waitingOrder.UnitsetName,
                                UnitsetCode = waitingOrder.UnitsetCode,
                                SubUnitsetReferenceId = waitingOrder.SubUnitsetReferenceId,
                                SubUnitsetName = waitingOrder.SubUnitsetName,
                                SubUnitsetCode = waitingOrder.SubUnitsetCode,
                                LocTracking = waitingOrder.LocTracking,
                                TrackingType = waitingOrder.TrackingType,
                                IsVariant = waitingOrder.IsVariant,
                                VariantIcon = waitingOrder.VariantIcon,
                                LocTrackingIcon = waitingOrder.LocTrackingIcon,
                                TrackingTypeIcon = waitingOrder.TrackingTypeIcon,
                                Image = waitingOrder.Image,
                            });
                        }
                    }
                }

                _userDialogs.HideHud();
            }
            catch (Exception ex)
            {
                if (_userDialogs.IsHudShowing)
                    _userDialogs.HideHud();

                _userDialogs.Alert(ex.Message);
            }
            finally
            {
                IsBusy = false;
            }
        }

        private async Task LoadMoreItemsAsync()
        {
            if (IsBusy)
                return;

            try
            {
                IsBusy = true;

                _userDialogs.ShowLoading("Loading...");
                var httpClient = _httpClientService.GetOrCreateHttpClient();
                var result = await _salesPanelService.GetShippedProducts(httpClient, _httpClientService.FirmNumber, _httpClientService.PeriodNumber, string.Empty, Items.Count, 20);
                if (result.IsSuccess)
                {
                    if (result.Data is not null)
                    {
                        foreach (var item in result.Data)
                        {
                            var waitingOrder = Mapping.Mapper.Map<SalesTransactionModel>(item);
                            Items.Add(new Product
                            {
                                ReferenceId = waitingOrder.ProductReferenceId,
                                Code = waitingOrder.ProductCode,
                                Name = waitingOrder.ProductName,
                                StockQuantity = waitingOrder.Quantity,
                                UnitsetReferenceId = waitingOrder.UnitsetReferenceId,
                                UnitsetName = waitingOrder.UnitsetName,
                                UnitsetCode = waitingOrder.UnitsetCode,
                                SubUnitsetReferenceId = waitingOrder.SubUnitsetReferenceId,
                                SubUnitsetName = waitingOrder.SubUnitsetName,
                                SubUnitsetCode = waitingOrder.SubUnitsetCode,
                                LocTracking = waitingOrder.LocTracking,
                                TrackingType = waitingOrder.TrackingType,
                                IsVariant = waitingOrder.IsVariant,
                                VariantIcon = waitingOrder.VariantIcon,
                                LocTrackingIcon = waitingOrder.LocTrackingIcon,
                                TrackingTypeIcon = waitingOrder.TrackingTypeIcon,
                                Image = waitingOrder.Image,
                            });
                        }
                    }
                }

                _userDialogs.HideHud();
            }
            catch (Exception ex)
            {
                if (_userDialogs.IsHudShowing)
                    _userDialogs.HideHud();

                _userDialogs.Alert(ex.Message);
            }
            finally
            {
                IsBusy = false;
            }
        }

        private async Task ItemTappedAsync(Product product)
        {
            if (IsBusy)
                return;

            try
            {
                IsBusy = true;

                SelectedItem = product;

                await LoadShippedOrdersAsync(product);
                CurrentPage.FindByName<BottomSheet>("shippedOrdersBottomSheet").State = BottomSheetState.HalfExpanded;
            }
            catch (Exception ex)
            {
                if (_userDialogs.IsHudShowing)
                    _userDialogs.HideHud();

                _userDialogs.Alert(ex.Message, "Hata", "Tamam");
            }
            finally
            {
                IsBusy = false;
            }
        }

        private async Task ShippedOrdersCloseAsync()
        {
            await MainThread.InvokeOnMainThreadAsync(() =>
            {
                CurrentPage.FindByName<BottomSheet>("shippedOrdersBottomSheet").State = BottomSheetState.Hidden;
            });
        }

        private async Task LoadShippedOrdersAsync(Product product)
        {
            try
            {
                _userDialogs.ShowLoading("Yükleniyor...");
                await Task.Delay(1000);

                ShippedOrders.Clear();

                var httpClient = _httpClientService.GetOrCreateHttpClient();
                var result = await _salesPanelService.GetShippedOrders(httpClient, _httpClientService.FirmNumber, _httpClientService.PeriodNumber, product.ReferenceId, string.Empty, 0, 20);
                if (result.IsSuccess)
                {
                    if (result.Data is null)
                        return;

                    foreach (var item in result.Data)
                        ShippedOrders.Add(Mapping.Mapper.Map<SalesTransactionModel>(item));
                }

                _userDialogs.HideHud();
            }
            catch (Exception ex)
            {
                if (_userDialogs.IsHudShowing)
                    _userDialogs.HideHud();

                _userDialogs.Alert(ex.Message, "Hata", "Tamam");
            }
        }

        private async Task LoadMoreShippedOrdersAsync()
        {
            if (IsBusy)
                return;
            try
            {
                IsBusy = true;

                var httpClient = _httpClientService.GetOrCreateHttpClient();
                var result = await _salesPanelService.GetShippedOrders(httpClient, _httpClientService.FirmNumber, _httpClientService.PeriodNumber, SelectedItem.ReferenceId, string.Empty, ShippedOrders.Count, 20);
                if (result.IsSuccess)
                {
                    if (result.Data is null)
                        return;

                    foreach (var item in result.Data)
                        ShippedOrders.Add(Mapping.Mapper.Map<SalesTransactionModel>(item));
                }
            }
            catch (Exception ex)
            {
                await _userDialogs.AlertAsync(ex.Message, "Hata", "Tamam");
            }
            finally
            {
                IsBusy = false;
            }
        }

        private async Task BackAsync()
        {
            if (IsBusy)
                return;
            try
            {
                IsBusy = true;
                if (Items.Count > 0)
                {
                    Items.Clear();
                }
                await Shell.Current.GoToAsync("..");
            }
            catch (Exception ex)
            {
                if (_userDialogs.IsHudShowing)
                    _userDialogs.HideHud();

                await _userDialogs.AlertAsync(ex.Message, "Hata", "Tamam");
            }
            finally
            {
                IsBusy = false;
            }
        }
    }
}