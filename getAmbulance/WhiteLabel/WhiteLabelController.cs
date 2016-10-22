using getAmbulance.Models;
using getAmbulance.Reservation;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace getAmbulance.WhiteLabel
{
    public class WhiteLabelController : ApiController
    {
        private ApplicationIdentityContext _ctx;
        private ReservationService _reservationService;



        public WhiteLabelController()
        {
            _ctx = ApplicationIdentityContext.Create();
            _reservationService = new ReservationService();

        }

    }
}
