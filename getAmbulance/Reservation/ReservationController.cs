using getAmbulance.Hubs;
using getAmbulance.Models;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.OData;
using System.Web.Http.Cors;
using static getAmbulance.WhiteLabel.WhiteLabelModel;

namespace getAmbulance.Reservation
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    [System.Web.Http.Authorize]
    public class ReservationController :ApiController
    {
        private ApplicationIdentityContext _ctx;
        private ReservationService _reservationService;
     


        public ReservationController()
        {
            _ctx = ApplicationIdentityContext.Create();
            _reservationService = new ReservationService();
           

        }
        Lazy<IHubContext> hub = new Lazy<IHubContext>(
      () => GlobalHost.ConnectionManager.GetHubContext<ReservationHub>()
    );
        protected IHubContext Hub
        {
            get { return hub.Value; }
        }
        // POST: /Reservation/Add
        [HttpPost]
        [AllowAnonymous]
        public HttpResponseMessage AddReservation(ReservationEntity reservation)
        {
            HttpResponseMessage response;
            try
            {
                Hub.Clients.Group("1").addReservation(reservation);
               // Hub.Clients.All.addReservation(reservation);
                _reservationService.AddReservation(reservation);
                response = Request.CreateResponse(HttpStatusCode.OK);
            }
            catch (Exception ex)
            {
                response = Request.CreateErrorResponse(HttpStatusCode.BadRequest, "GetTYPAccountInfo Error");
            }
            return response;
        }

        // Post: /Reservation/GetReservationsListByWhiteLabelId
        [HttpPost]
        
        public HttpResponseMessage GetReservationsListByWhiteLabelId(JObject jsonData)
        {
            HttpResponseMessage response;
            int reservationStatus = 0;
            int whiteLabelId = 0;
            try
            {
                dynamic jsonObj = jsonData;
                if (jsonObj.status == null)
                {
                    reservationStatus = 0;
                }
                else
                {
                    Int32.TryParse(jsonObj.status.Value, out reservationStatus);
                }
                Int32.TryParse(jsonObj.whiteLabelId.Value, out whiteLabelId);

                List<ReservationEntity> reservationList = _reservationService.GetReservationsListByWhiteLabelId(whiteLabelId, reservationStatus);

                response = Request.CreateResponse(HttpStatusCode.OK, reservationList);
            }
            catch (Exception ex)
            {

                response = Request.CreateErrorResponse(HttpStatusCode.BadRequest, "GetReservationsListByWhiteLabelId Add Error");
            }
            return response;
        }
        // Get: /Reservation/GetReservationsList
        [AllowAnonymous]
        public HttpResponseMessage GetReservationsList()
        {
            HttpResponseMessage response;
            try
            {
                List<ReservationEntity> reservationList = _reservationService.GetReservationsList();
                response = Request.CreateResponse(HttpStatusCode.OK, reservationList);
            }
            catch (Exception ex)
            {
                response = Request.CreateErrorResponse(HttpStatusCode.BadRequest, "GetTYPAccountInfo Error");
            }
            return response;
        }
        // Post: /Reservation/GetAmbulanceOffersList
        [HttpPost]
        [AllowAnonymous]
        public HttpResponseMessage GetAmbulanceOffersList(JObject jsonData)
        {
            HttpResponseMessage response;
            dynamic jsonObj = jsonData;
            var currentPlace = jsonObj.currentPlace.Value;
            try
            {
                List<WhiteLabelOfferEntity> whiteLabelsOfferList=_reservationService.GetAmbulanceOffersList(jsonObj);
                response = Request.CreateResponse(HttpStatusCode.OK, whiteLabelsOfferList);
            }
            catch (Exception ex)
            {
                response = Request.CreateErrorResponse(HttpStatusCode.BadRequest, "GetAmbulanceOffersList Error");
            }
            return response;
        }
        
    }
}