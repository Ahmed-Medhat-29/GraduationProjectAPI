namespace GraduationProjectAPI.Utilities.General
{
	public class Pagination
	{
		public const int MaxPageSize = 10;
		public int CurrentPage { get; set; }
		public int LastPage { get; set; }
		public int PerPage { get; set; } = MaxPageSize; // Maximum number of elements per page
		public int Total { get; set; } // Number of elements in current page

		public Pagination(int? currentPage)
		{
			CurrentPage = currentPage ?? 1;
			LastPage = 1;
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
