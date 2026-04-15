using System.Collections.Generic;

namespace DemoModel.ViewModel
{
    public partial class UserTypeViewModel
    {
        public UserTypeViewModel()
        {
           // this.Users = new HashSet<UserViewModel>();
        }

        public long Id { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public System.DateTime CreatedOn { get; set; }
        public bool IsActive { get; set; }
        //public virtual ICollection<UserViewModel> Users { get; set; }
    }
}
