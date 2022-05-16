namespace GraduationProjectAPI.DTOs.Response
{
	public class ListItem
	{
		public int Id { get; set; }
		public string Name { get; set; }

		public ListItem(int id, string name)
		{
			Id = id;
			Name = name;
		}
	}
}
