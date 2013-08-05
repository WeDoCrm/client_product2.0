using System;
using System.Collections.Generic;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Client.Common;

namespace Client
{
    public partial class AddMemberForm : Form
    {
        public bool curListOnly = false;
        public bool multipleSelect = true;

        public AddMemberForm() {
            InitializeComponent();
        }

        public AddMemberForm(bool curListOnly, string formKey) {
            
            InitializeComponent();
            this.curListOnly = curListOnly;
            this.formkey.Text = formKey;

            if (curListOnly) {
                this.radiobt_all.Enabled = false;
                this.radiobt_g.Enabled = false;
                this.combobox_team.Enabled = false;
                this.radiobt_con.Checked = true;
            }
        }

        public AddMemberForm(bool curListOnly, string formKey, bool multipleSelect) {
        
            InitializeComponent();
            this.curListOnly = curListOnly;
            this.formkey.Text = formKey;
            this.multipleSelect = multipleSelect;
        
            if (curListOnly) {
                this.radiobt_all.Enabled = false;
                this.radiobt_g.Enabled = false;
                this.combobox_team.Enabled = false;
                this.radiobt_con.Checked = true;
            }
        }

        #region 컨트롤이벤트처리
        private void BtnAdd_Click(object sender, EventArgs e) {
        
            if (CurrInListBox.SelectedItems.Count != 0) {
            
                ArrayList list = new ArrayList();
                ListBox.SelectedObjectCollection collection=CurrInListBox.SelectedItems;

                if (!multipleSelect && AddListBox.Items.Count > 0) {
                    MessageBox.Show("사용자선택은 1명만 가능합니다.", "선택초과", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
            
                foreach (ListBoxItem member in collection) {
                
                    if (ListBox.NoMatches != AddListBox.FindStringExact(member.Text)) {
                        MessageBox.Show("이미 선택된 사용자 입니다.", "중복선택", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    } else {
                        AddListBox.Items.Add(member);
                        list.Add(member);
                    }
                }

                foreach (ListBoxItem obj in list) {
                    CurrInListBox.Items.Remove(obj);
                }
            }
        }

        private void Btnback_Click(object sender, EventArgs e) {
        
            ArrayList list = new ArrayList();
            if (AddListBox.SelectedItems.Count != 0) {
            
                foreach (ListBoxItem item in AddListBox.SelectedItems) {
                    list.Add(item);
                }

                foreach (ListBoxItem obj in list) {
                    CurrInListBox.Items.Add(obj);
                    AddListBox.Items.Remove(obj);
                }
            }
        }

        private void BtnCancel_MouseClick(object sender, MouseEventArgs e) {
            
            Close();
        
        }

        #endregion

        public void SetAddListBox(TreeNodeCollection collection) {
            if (collection != null && collection.Count > 0 ) {
                foreach (TreeNode node in collection) {
                    UserObject userObj = (UserObject)node.Tag;
                    if (userObj == null || userObj.Id == "")
                        continue;
                    AddListBox.Items.Add(new ListBoxItem(userObj));
                }
            }
        }

        public void SetAddListBox(Object[] strArray, TreeNodeCollection collection) {

            if (strArray != null && strArray.Length > 0) {

                foreach (string str in strArray) {
                    if (str.Length > 0) {
                        string[] array = str.Split('(');
                        string[] array1 = array[1].Split(')');
                        string userId = array1[0];


                        UserObject userObj = ChatUtils.FindUserObjectTagFromTreeNodes(collection, userId);
                        if (userObj == null || userObj.Id == "")
                            continue;

                        ListBoxItem item = new ListBoxItem(userObj);
                        
                        AddListBox.Items.Add(item);
                    }
                }
            }
        }

        public void SetCurrInListBox(Hashtable table, TreeNodeCollection collection) {
            if (table != null && table.Count != 0) {
                foreach (DictionaryEntry de in table) {
                    
                    if (de.Value != null)  {
                        UserObject userObj = ChatUtils.FindUserObjectTagFromTreeNodes(collection, de.Key.ToString());
                        if (userObj == null || userObj.Id == "")
                            continue;

                        ListBoxItem item = new ListBoxItem(userObj);
                        if (ListBox.NoMatches == AddListBox.FindStringExact(item.Text)) {
                            CurrInListBox.Items.Add(item);
                        }
                    }
                }
            }
        }
    }
}