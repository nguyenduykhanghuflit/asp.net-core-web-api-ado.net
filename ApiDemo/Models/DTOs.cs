using System.ComponentModel.DataAnnotations;

namespace ApiDemo.Models
{
    public class StatusDTO
    {
        public int StatusId { get; set; }
        public int Year { get; set; }
    }



    public class Status
    {
        public int StatusId { get; set; }
    }


    public class DealDTO
    {
        [Required(ErrorMessage = "Id is required")]
        public int Id { get; set; }

        public int? Status { get; set; }

        public int? SaleMenId { get; set; }

        public string? Title { get; set; }

        public string? CustName { get; set; }

        public string? Phone { get; set; }

        public string? Email { get; set; }

        public string? Notes { get; set; }


    }

    public class CRM_Deal
    {
        public int Id { get; set; }

        public int? Status { get; set; }

        public int? SaleMenId { get; set; }

        public string? Title { get; set; }

        public string? CustName { get; set; }

        public string? Phone { get; set; }

        public string? Email { get; set; }


    }

    public class DealStatusDTO
    {
        public int dealId { get; set; }
        public int statusId { get; set; }
    }
    public class DealNoteDTO
    {
        public int dealId { get; set; }
        public string? notes { get; set; }
    }

}
