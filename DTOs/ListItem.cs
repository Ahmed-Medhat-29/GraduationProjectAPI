namespace GraduationProjectAPI.DTOs
{
	public class ListItem
	{
		public int Id { get; set; }
		public string Name { get; set; }

		public ListItem()
		{

		}

		public ListItem(int id, string name)
		{
			Id = id;
			Name = name;
		}
	}
}
