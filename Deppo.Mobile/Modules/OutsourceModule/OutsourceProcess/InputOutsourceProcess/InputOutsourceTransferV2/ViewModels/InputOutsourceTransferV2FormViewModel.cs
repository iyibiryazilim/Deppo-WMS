﻿using CommunityToolkit.Mvvm.ComponentModel;
using Controls.UserDialogs.Maui;
using Deppo.Mobile.Helpers.HttpClientHelpers;
using Deppo.Mobile.Helpers.MVVMHelper;
using DevExpress.Maui.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Deppo.Mobile.Modules.OutsourceModule.OutsourceProcess.InputOutsourceProcess.InputOutsourceTransferV2.ViewModels;

public partial class InputOutsourceTransferV2FormViewModel : BaseViewModel
{
	private readonly IHttpClientService _httpClientService;
	private readonly IUserDialogs _userDialogs;
	private readonly IServiceProvider _serviceProvider;


	[ObservableProperty]
	private DateTime ficheDate = DateTime.Now;

	[ObservableProperty]
	private string documentNumber = string.Empty;

	[ObservableProperty]
	private string documentTrackingNumber = string.Empty;

	[ObservableProperty]
	private string specialCode = string.Empty;

	[ObservableProperty]
	private string description = string.Empty;

	public InputOutsourceTransferV2FormViewModel(IHttpClientService httpClientService, IUserDialogs userDialogs, IServiceProvider serviceProvider)
	{
		_httpClientService = httpClientService;
		_userDialogs = userDialogs;
		_serviceProvider = serviceProvider;

		Title = "Fason Kabul Formu";

		LoadPageCommand = new Command(async () => await LoadPageAsync());
		ShowBasketItemCommand = new Command(async () => await ShowBasketItemAsync());
		SaveCommand = new Command(async () => await SaveAsync());
		BackCommand = new Command(async () => await BackAsync());
	}
	public Page CurrentPage { get; set; } = null!;
	public Command ShowBasketItemCommand { get; }
	public Command LoadPageCommand { get; }
	public Command SaveCommand { get; }
	public Command BackCommand { get; }

	private async Task LoadPageAsync()
	{
		if (IsBusy)
			return;
		try
		{
			IsBusy = true;

			//CurrentPage.FindByName<BottomSheet>("basketItemBottomSheet").State = BottomSheetState.HalfExpanded;
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

	private async Task ShowBasketItemAsync()
	{
		if (IsBusy)
			return;
		try
		{
			IsBusy = true;

			//CurrentPage.FindByName<BottomSheet>("basketItemBottomSheet").State = BottomSheetState.HalfExpanded;
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


	private async Task SaveAsync()
	{
		if (IsBusy)
			return;
		try
		{
			IsBusy = true;

			var httpClient = _httpClientService.GetOrCreateHttpClient();

		}
		catch (Exception ex)
		{
			if(_userDialogs.IsHudShowing)
				_userDialogs.HideHud();

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

	private async Task ClearDataAsync()
	{
		try
		{
			var warehouseListViewModel = _serviceProvider.GetRequiredService<InputOutsourceTransferV2WarehouseListViewModel>();
			var supplierListViewModel = _serviceProvider.GetRequiredService<InputOutsourceTransferV2SupplierListViewModel>();
			var productListViewModel = _serviceProvider.GetRequiredService<InputOutsourceTransferV2ProductListViewModel>();

			if(warehouseListViewModel is not null && warehouseListViewModel.SelectedWarehouseModel is not null)
			{
				warehouseListViewModel.SelectedWarehouseModel.IsSelected = false;
				warehouseListViewModel.SelectedWarehouseModel = null;
			}

			if(supplierListViewModel is not null && supplierListViewModel.SelectedOutsourceModel is not null)
			{
				supplierListViewModel.SelectedOutsourceModel.IsSelected = false;
				supplierListViewModel.SelectedOutsourceModel = null;
			}

			if(productListViewModel is not null && productListViewModel.SelectedOutsourceProductModel is not null)
			{
				productListViewModel.SelectedOutsourceProductModel.IsSelected = false;
				productListViewModel.SelectedOutsourceProductModel = null;
			}

		}
		catch (Exception ex)
		{

			throw;
		}
	}

	private async Task ClearFormAsync()
	{
		try
		{
			FicheDate = DateTime.Now;
			DocumentNumber = string.Empty;
			DocumentTrackingNumber = string.Empty;
			SpecialCode = string.Empty;
			Description = string.Empty;
		}
		catch (Exception ex)
		{

			throw;
		}
	}
}
