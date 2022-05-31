namespace GraduationProjectAPI.Utilities.General
{
	public class Pagination
	{
		public const int MaxPageSize = 10;
		public int CurrentPage { get; set; } = 1;
		public int LastPage { get; set; } = 1;
		public int PerPage { get; set; } = MaxPageSize; // Maximum number of elements per page
		public int Total { get; set; } // Number of elements in current page

		public Pagination()
		{

		}

		public Pagination(int currentPage)
		{
			CurrentPage = currentPage;
		}

		public Pagination(int currentPage, int totalCount, int totalInPage)
		{
			CurrentPage = currentPage;
			LastPage = (totalCount / MaxPageSize) + (totalCount % MaxPageSize == 0 ? 0 : 1);
			Total = totalInPage;

			if (LastPage <= 0)
				LastPage = 1;
		}
	}
}
