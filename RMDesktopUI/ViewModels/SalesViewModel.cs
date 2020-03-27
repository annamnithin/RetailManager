using Caliburn.Micro;
using RMDesktopUI.Library.Api;
using RMDesktopUI.Library.Helper;
using RMDesktopUI.Library.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RMDesktopUI.ViewModels
{
    public class SalesViewModel: Screen
    {
		IProductEndPoint _productEndPoint;
		IConfigHelper _configHelper;

		private BindingList<ProductModel> _products;

		public SalesViewModel(IProductEndPoint productEndPoint, IConfigHelper configHelper)
		{
			_productEndPoint = productEndPoint;
			_configHelper = configHelper;
			
		}

		protected override async void OnViewLoaded(object view)
		{
			base.OnViewLoaded(view);
			await LoadProducts();
		}

		public async Task LoadProducts()
		{
			var productList = await _productEndPoint.GetAll();
			Products = new BindingList<ProductModel>(productList);
		}
		public BindingList<ProductModel> Products
		{
			get { return _products; }
			set { 
				_products = value;
				NotifyOfPropertyChange(() => Products);
			}
		}

		private ProductModel _selectedProduct;

		public ProductModel SelectedProduct
		{
			get { return _selectedProduct; }
			set { 
				_selectedProduct = value;
				NotifyOfPropertyChange(() => SelectedProduct);
				NotifyOfPropertyChange(() => CanAddToCart);
			}
		}


		private BindingList<CartItemModel> _cart = new BindingList<CartItemModel>();
		public BindingList<CartItemModel> Cart
		{
			get { return _cart; }
			set
			{
				_cart = value;
				NotifyOfPropertyChange(() => Cart);
			}
		}

		public string SubTotal
		{
			get
			{
				return CaluclateSubTotal().ToString("C");
			}
		}

		public decimal CaluclateSubTotal()
		{
			decimal subTotal = 0;

			foreach (var item in Cart)
			{
				subTotal += (item.Product.RetailPrice * item.QuantityInCart);
			}

			return subTotal;
		}

		public string Tax
		{
			get
			{
				return CaluclateTax().ToString("C");
			}
		}

		private decimal CaluclateTax()
		{
			decimal taxAmount = 0;
			decimal taxRate = _configHelper.GetTaxRate()/100 ;

			taxAmount = Cart
						 .Where(x => x.Product.IsTaxable)
						.Sum(x => x.Product.RetailPrice * x.QuantityInCart * taxRate);

			//foreach (var item in Cart)
			//{
			//	if (item.Product.IsTaxable)
			//	{
			//		taxAmount += (item.Product.RetailPrice * item.QuantityInCart * _taxRate);
			//	}

			//}
			return taxAmount;
		}
		public string Total
		{
			get
			{
				decimal total =  CaluclateSubTotal() + CaluclateTax() ;
				return total.ToString("C");
			}
		}

		private int _itemQuantity = 1;

		public int ItemQuantity
		{
			get { return _itemQuantity; }
			set { 
				_itemQuantity = value;
				NotifyOfPropertyChange(() => ItemQuantity);
				NotifyOfPropertyChange(() => CanAddToCart);
			}
		}
		public bool CanAddToCart
		{
			get
			{
				bool output = false;

				if(ItemQuantity>0 && SelectedProduct?.QuantityInStock>= ItemQuantity)
				{
					output = true;
				}

				return output;
			}
		}
		public void AddToCart()
		{
			CartItemModel existingItem = Cart.FirstOrDefault(x => x.Product == SelectedProduct);
			if (existingItem != null)
			{
				existingItem.QuantityInCart += ItemQuantity;
				Cart.Remove(existingItem);
				Cart.Add(existingItem);
			}
			else
			{
				CartItemModel item = new CartItemModel
				{
					Product = SelectedProduct,
					QuantityInCart = ItemQuantity
				};
				Cart.Add(item);
			}
			
			SelectedProduct.QuantityInStock -= ItemQuantity;
			ItemQuantity = 1;
			NotifyOfPropertyChange(() => SubTotal);
			NotifyOfPropertyChange(() => Tax);
			NotifyOfPropertyChange(() => Total);

		}

		public bool CanRemoveFromCart
		{
			get
			{
				bool output = false;

				return output;
			}
		}
		public void ARemoveFromCart()
		{

		}

		public bool CanCheckOut
		{
			get
			{
				bool output = false;

				return output;
			}
		}
		public void CheckOut()
		{

		}

	}
}
