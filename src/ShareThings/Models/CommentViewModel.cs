using ShareThings.Domain;
using System;

namespace ShareThings.Models
{
    public class CommentViewModel
    {
        public CommentViewModel() { }

        public CommentViewModel(BorrowComment comment) : this() 
        {
            this.BorrowId = comment.BorrowId;
            this.CommentId = comment.CommentId;
            this.OwnerId = comment.OwnerId;
            this.OwnerUserIdentity = comment.Owner.UserIdentityId;
            this.Text = comment.Text;
            this.Creation = comment.Creation;
        }

        public int CommentId { get; set; }
        public string Text { get; set; }
        public int OwnerId { get; set; }
        public string OwnerUserIdentity { get; set; }
        public int BorrowId { get; set; }
        public string Owner { get; set; }
        public DateTime Creation { get; set; }

    }
}
