namespace Loadings
{
	public class Employee
	{
		public int EmployeeId { get; set; }
		public string Name { get; set; }
		public string Position { get; set; }

		// Foreign Key
		public int DepartmentId { get; set; }

		// Navigation Property
		public virtual Department Department { get; set; }
	}

}
