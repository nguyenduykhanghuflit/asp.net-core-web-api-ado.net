using System.Xml.Serialization;

namespace ApiDemo.Models
{
    public class StatisticalVM
    {
        public int OBJ_AUTOID { get; set; }
        public string? OBJ_NAME { get; set; }
        public Nullable<DateTime> DealDate { get; set; }
        public int DealTotal { get; set; }
        public dynamic? Deals { get; set; }
        public string? OBJ_PHONE { get; set; }
        public string? OBJ_ADDRESS { get; set; }
        public string? OBJ_EMAIL { get; set; }
        public bool OBJ_ISACTIVE { get; set; }
    }

    public class HallVM
    {
        public string? HallId { get; set; }
        public string? RET_DEFINEID { get; set; }
        public string? Total { get; set; }
    }

    public class TopHallVM
    {
        public HallVM[]? Hall { get; set; }
    }


    public class StatisticalDealStatusVM
    {
        public int Id { get; set; }
        public string? HomeTitle { get; set; }
        public string? Name { get; set; }
        public int Total { get; set; }
        public object? Deals { get; set; }
    }

    public class DealStatusVM
    {
        public int Id { get; set; }

        public string? CustName { get; set; }

        public string? Title { get; set; }

        public DateTime? DealDate { get; set; }

        public string? Notes { get; set; }

        public int? SaleMenId { get; set; }

        public string? SaleMenName { get; set; }

        public int? Status { get; set; }

        public string? StatusName { get; set; }

    }







}
