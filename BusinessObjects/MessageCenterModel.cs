using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObjects
{
    [Serializable()]
    public class MessageCenterModel
    {
        public long ID { get; set; }
        public string FromUser { get; set; }
        public string ToUser { get; set; }
        public string Subject { get; set; }
        public string MessageBody { get; set; }
        public string Duration { get; set; }
        public string ProfileImage { get; set; }
        public bool IsRead { get; set; }
        public long Sender { get; set; }
        public long ListingDetailsID { get; set; }
        public string MessageStatus { get; set; }
        public bool IsArchived { get; set; }
    }

    [Serializable()]
    public class MessageCenterWrapper
    { 
        public List<MessageCenterModel> MessageAll { get; set; }
        public List<MessageCenterModel> MessageArchived { get; set; }
        public List<MessageCenterModel> MessageUnread { get; set; }

        public IPagedList<MessageCenterModel> PagedMessageAll { get; set; }
        public IPagedList<MessageCenterModel> PagedMessageArchived { get; set; }
        public IPagedList<MessageCenterModel> PagedMessageUnread { get; set; }
    }
}
