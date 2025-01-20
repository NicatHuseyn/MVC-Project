namespace BackendProject.ViewModels.BasketViewModels
{
	public class BasketListVM
	{
		public BasketListVM()
		{
			BasketProducts = new List<BasketProductVM>();
		}

		public List<BasketProductVM> BasketProducts { get; set; }
	}
}
