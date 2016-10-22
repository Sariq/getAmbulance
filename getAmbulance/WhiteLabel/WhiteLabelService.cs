using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using static getAmbulance.WhiteLabel.WhiteLabelModel;

namespace getAmbulance.WhiteLabel
{
    public class WhiteLabelService
    {
        private ApplicationIdentityContext _ctx;
        public WhiteLabelService()
        {
            _ctx = ApplicationIdentityContext.Create();
            
        }

        public List<WhiteLabelEntity> GetWhiteLabelsList()
        {
            var whiteLabelsList = _ctx.WhiteLabels.Aggregate().ToListAsync().Result;
            return whiteLabelsList;
        }
        public List<WhiteLabelEntity> GetWhiteLabelsListByStatus(bool status)
        {
            var builder = Builders<WhiteLabelEntity>.Filter;
            var filter = builder.Eq("isOnline", status);
            var whiteLabelsList = _ctx.WhiteLabels.Find(filter).ToListAsync().Result;
            return whiteLabelsList;
        }
    }
}