namespace ProvaPub.Models
{
	public class RecordList<T>
	{
		public List<T> Records { get; set; }
		public int TotalCount { get; set; }
		public bool HasNext { get; set; }
	}
} 