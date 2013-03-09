using System;
using System.Collections.Generic;
using System.Text;

namespace Client.Common {
    class UserDef {
    }

    public class UserObject {
        
        public UserObject(string id, string name, string team) {
            this.id = id;
            this.name = name;
            this.titleName = name;
            this.teamName = team;
            this.Status = MsgrUserStatus.LOGOUT;
        }

        private string id;
        public string Id { get { return id; } set { id = value; } }
        
        private string name;
        public string Name { get { return name; } set { name = value; titleName = value; } }

        private string titleName;
        public string TitleName { get { return titleName; } set { titleName = value; } }

        private string teamName;
        public string TeamName { get { return teamName; } set { teamName = value; } }
        
        private string status;
        public string Status {
            get { return status; }
            set {
                status = value;                ;
                switch (status) {
                    case MsgrUserStatus.AWAY:
                        titleName = string.Format("{0}({1})", name, "자리비움");
                        break;
                    case MsgrUserStatus.BUSY:
                        titleName = string.Format("{0}({1})", name, "통화중");
                        break;
                    case MsgrUserStatus.DND:
                        titleName = string.Format("{0}({1})", name, "다른용무중");
                        break;
                    case MsgrUserStatus.LOGOUT:
                    case MsgrUserStatus.ONLINE:
                        titleName = name;
                        break;
                }
            }
        }
    }

    public class ListBoxItem : Object {
        public virtual string Text { get; set; }
        public virtual object Tag { get; set; }
        public virtual string Name { get; set; }
        public virtual object Object { get; set; }
        /// <summary>
        /// Class Constructor
        /// </summary>
        public ListBoxItem() {
            this.Text = string.Empty;
            this.Tag = null;
            this.Name = string.Empty;
            this.Object = null;
        }

        /// <summary>
        /// Overloaded Class Constructor
        /// </summary>
        /// <param name="Text">Object Text</param>
        /// <param name="Name">Object Name</param>
        /// <param name="Tag">Object Tag</param>
        /// <param name="Object">Object</param>
        public ListBoxItem(string Text, string Name, object Tag) {
            this.Text = Text;
            this.Tag = Tag;
            this.Name = Name;
            this.Object = Tag;
        }

        /// <summary>
        /// Overloaded Class Constructor
        /// </summary>
        /// <param name="Object">Object</param>
        public ListBoxItem(UserObject userObj) {
            this.Text = string.Format("{0}({1})", userObj.Name, userObj.Id);
            this.Name = userObj.Id;
            this.Tag = userObj;
            this.Object = userObj;
        }

        /// <summary>
        /// Overridden ToString() Method
        /// </summary>
        /// <returns>Object Text</returns>
        public override string ToString() {
            return this.Text;
        }
    }
}
