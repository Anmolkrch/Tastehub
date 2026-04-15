using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DemoModel.ViewModel
{
   public class SubMainMenuViewModel
    {
        public int Id { get; set; }

        public int MainMenuId { get; set; }

        public string MenuName { get; set; }
        public string MenuText { get; set; }
        
        [Required(ErrorMessage = "Menu Description is required")]
        [Display(Name = "Menu Description*")]
        
        public string MenuDescription { get; set; }

        public bool IsActive { get; set; }



        public string CreatedBy { get; set; }

        public string ModifiedBy { get; set; }



        //public virtual tblMenu tblMenu { get; set; }
    }
}
