using getAmbulance.Models;
using getAmbulance.WhiteLabel;
using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using static getAmbulance.WhiteLabel.WhiteLabelModel;

namespace getAmbulance.Reservation
{
    public class ReservationService
    {
        private ApplicationIdentityContext _ctx;
        private WhiteLabelService _whiteLabelService;
        public ReservationService()
        {
            _ctx = ApplicationIdentityContext.Create();
            _whiteLabelService = new WhiteLabelService();
        }
        public void AddReservation(ReservationEntity reservation)
        {
            try
            {
                _ctx.Reservations.InsertOneAsync(reservation);
            }
            catch (Exception ex)
            {

            }
            //ReservationEntity r1 = new ReservationEntity { };

            //ReservationFormEntity rf = new ReservationFormEntity { };

            //rf.First_Name = "sari2";
            //rf.Last_Name = "qash2";
            //r1.Reservation_Form = rf;

            //r1.status = 1;
            //r1.WhiteLabel_ID = 1;

        }
        public List<ReservationEntity> GetReservationsList()
        {
            var reservationsList = _ctx.Reservations.Aggregate().ToListAsync().Result;
            return reservationsList;
        }

        public List<ReservationEntity> GetReservationsListByWhiteLabelId(int whiteLabel_ID, int reservationStatus = 0)
        {
            var builder = Builders<ReservationEntity>.Filter;
            var filter = builder.Eq("WhiteLabel_ID", whiteLabel_ID);
            if (reservationStatus != 0)
            {
                filter = filter & builder.Eq("status", reservationStatus);
            }
            var reservationsList = _ctx.Reservations.Find(filter).ToListAsync().Result;
          
                reservationsList = hideClientInformation(reservationsList);
            
            return reservationsList;
        }
        public List<ReservationEntity> hideClientInformation(List<ReservationEntity> reservationsList)
        {
            var temp_reservationsList = reservationsList;

            foreach (ReservationEntity reservation in temp_reservationsList)
            {
                if (reservation.Status == 1)
                {
                    reservation.Reservation_Form.First_Name = null;
                    reservation.Reservation_Form.Last_Name = null;
                    reservation.Reservation_Form.Phone_Number = null;
                    reservation.Reservation_Form.Id_Number = null;
                }
            }
            return temp_reservationsList;
        }
        public List<WhiteLabelOfferEntity> GetAmbulanceOffersList(dynamic jsonObj)
        {
            List<WhiteLabelOfferEntity> whiteLabelsOfferList= new List<WhiteLabelOfferEntity>();
            List <WhiteLabelEntity> whiteLabelsList= _whiteLabelService.GetWhiteLabelsListByStatus(true);
            foreach (WhiteLabelEntity whiteLabel in whiteLabelsList)
            {
                int distancePrice = getWhiteLabelDistancePriceByKM((BsonDocument)whiteLabel.prices["distance"], (int)jsonObj.address.distance.Value);
                int extraServicesPrice = getWhiteLabelExtraServicesPrice((BsonDocument)whiteLabel.prices,jsonObj);
                int finalPrice = distancePrice + extraServicesPrice;
                whiteLabelsOfferList.Add(new WhiteLabelOfferEntity(whiteLabel.whiteLabelid, whiteLabel.name, whiteLabel.logo, finalPrice));
            }
            return whiteLabelsOfferList;
        }
        public int getWhiteLabelDistancePriceByKM(BsonDocument distancePricesList,int reservationDistance)
        {
            foreach (var distance in distancePricesList)
            {
                if (reservationDistance <= Int32.Parse(distance.Name))
                {
                    //TODO: send dayOrNight
                    return (int)distance.Value["day"];
                }
            }
            return 0;
        }
        public int getWhiteLabelExtraServicesPrice(BsonDocument prices, dynamic reservationData)
        {

            var temp_prices = prices;
       

            foreach (var weightPrice in (BsonDocument)prices["weight"])
            {
                if ((int)reservationData["weight"] <= Int32.Parse(weightPrice.Name))
                {
                    Console.Write(weightPrice);
                    return (int)weightPrice.Value;
                }

            }
            return 0;

        }


    }
}