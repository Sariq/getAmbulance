using AspNet.Identity.MongoDB;
using getAmbulance.Enums;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;

namespace getAmbulance.Models
{
    public class ReservationEntity : DatabaseObject
    {
   
        public int WhiteLabel_ID { get; set; }
        public int Status { get; set; }
        public DateTime Date { get; set; }
        public ReservationFormEntity Reservation_Form { get; set; }


    }
    public class ReservationFormEntity
    {
        public string Place_Type { get; set; }
        public string From_Address { get; set; }
        public string To_Address { get; set; }
        public string Weight { get; set; }
        public string Patient_Status { get; set; }
        public string First_Name { get; set; }
        public string Last_Name { get; set; }
        public string Phone_Number { get; set; }
        public string Id_Number { get; set; }
    }

    public class WhiteLabelPriceOfferEntity
    {
        public string WhiteLabelName { get; set; }
        public string WhiteLabelId { get; set; }
        public string WhiteLabelLogo { get; set; }
        public string Price { get; set; }
    }
}