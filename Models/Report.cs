namespace FoodDeliveryManagementSystem.Models
{
    public class Report
    {
        public int ReportId { get; set; }        // Primary Key
        public string ReportType { get; set; }  // Type of report (e.g., "Sales", "Order Volume")
        public DateTime GeneratedDate { get; set; } // Date when the report was generated
        public string Data { get; set; }        // Serialized report data (e.g., JSON or XML)
        public int CreatedBy { get; set; }      // Foreign Key: Refers to the UserId of the report creator

        // Navigation Property
        public User User { get; set; }          // Navigation to the User who generated the report
    }

}
