using System;
using System.Collections.Generic;
using System.Text;

namespace LatinMedia.DataLayer.Entities
{
    public class UserRole
    {
        public int UR_ID { get; set; }

        public int UserID { get; set; }
        public int RoleID { get; set; }

        #region Relations

        public virtual User User{ get; set; }
        public Role Role { get; set; }

        #endregion
    }
}
