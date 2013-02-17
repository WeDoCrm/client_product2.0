using System;
using System.Collections.Generic;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Client
{
    public partial class AddMemberForm : Form
    {
        public bool curListOnly = false;
        public bool multipleSelect = true;

        public AddMemberForm()
        {
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

        public AddMemberForm(bool curListOnly, string formKey, bool multipleSelect)
        {
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
        private void BtnAdd_Click(object sender, EventArgs e)
        {
            if (CurrInListBox.SelectedItems.Count != 0)
            {
                ArrayList list = new ArrayList();
                ListBox.SelectedObjectCollection collection=CurrInListBox.SelectedItems;

                if (!multipleSelect && AddListBox.Items.Count > 0) {
                    MessageBox.Show("사용자선택은 1명만 가능합니다.", "선택초과", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
                foreach (string member in collection)
                {
                    if (AddListBox.Items.Contains(member))
                    {
                        MessageBox.Show("이미 선택된 사용자 입니다.", "중복선택", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else
                    {
                        AddListBox.Items.Add(member);
                        list.Add(member);
                    }
                }
                foreach (object obj in list)
                {
                    string item = obj.ToString();
                    CurrInListBox.Items.Remove(item);
                }
            }
        }

        private void Btnback_Click(object sender, EventArgs e)
        {
            ArrayList list = new ArrayList();
            if (AddListBox.SelectedItems.Count != 0)
            {
                for (int i = 0; i < AddListBox.SelectedItems.Count; i++)
                {
                    list.Add(AddListBox.SelectedItems[i]);
                }
                foreach (object obj in list)
                {
                    string item = obj.ToString();
                    CurrInListBox.Items.Add(item);
                    AddListBox.Items.Remove(item);
                }
            }
        }

        private void BtnCancel_MouseClick(object sender, MouseEventArgs e)
        {
            Close();
        }

        #endregion

        public void SetAddListBox(TreeNodeCollection collection) {
            if (collection != null && collection.Count > 0 ) {
                foreach (TreeNode node in collection) {
                    if (node.Text.Length != 0) {
                        AddListBox.Items.Add(node.Text);
                    }
                }
            }
        }

        public void SetAddListBox(Object[] strArray) {
            if (strArray != null && strArray.Length > 0 ) {
                foreach (string str in strArray) {
                    if (str.Length > 2) {
                        AddListBox.Items.Add(str);
                    }
                }
            }
        }
        public void SetCurrInListBox(Hashtable table, Hashtable MemberInfoList) {
            if (table != null && table.Count != 0) {
                foreach (DictionaryEntry de in table) {
                    
                    if (de.Value != null)  {

                        string name = (string)MemberInfoList[de.Key.ToString()];
                        string item = name + "(" + de.Key.ToString() + ")";

                        if (!AddListBox.Items.Contains(item)) {
                            CurrInListBox.Items.Add(item);
                        }
                    }
                }
            }
        }
    }
}