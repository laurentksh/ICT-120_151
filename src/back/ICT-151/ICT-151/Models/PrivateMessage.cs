using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using ICT_151.Models.Dto;

namespace ICT_151.Models
{
    public class PrivateMessage
    {
        public Guid Id { get; set; }

        public string MessageContent { get; set; }

        public DateTime CreationDate { get; set; }


        public User Sender { get; set; }

        [ForeignKey(nameof(Sender))]
        public Guid SenderId { get; set; }

        public User Recipient { get; set; }

        [ForeignKey(nameof(Recipient))]
        public Guid RecipientId { get; set; }
    }

    public class PrivateMessageViewModel
    {
        public Guid Id { get; set; }

        public string MessageContent { get; set; }

        public DateTime CreationDateUtc { get; set; }


        public UserSummaryViewModel Sender { get; set; }

        public UserSummaryViewModel Recipient { get; set; }

        public static PrivateMessageViewModel FromPrivateMessage(PrivateMessage msg) => new PrivateMessageViewModel
        {
            Id = msg.Id,
            MessageContent = msg.MessageContent,
            CreationDateUtc = msg.CreationDate,
            Sender = UserSummaryViewModel.FromUser(msg.Sender),
            Recipient = UserSummaryViewModel.FromUser(msg.Recipient)
        };
    }
}
