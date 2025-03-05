namespace Loadings
{
	public class Department
	{
		public int DepartmentId { get; set; }
		public string DepartmentName { get; set; }

		// Navigation Property
		public virtual List<Employee> Employees { get; set; } = new List<Employee>();
	}

}
