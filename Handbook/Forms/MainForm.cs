using System.Runtime.InteropServices;
using System.Windows.Forms;
using Handbook.Models;
using static System.Reflection.Metadata.BlobBuilder;

namespace Handbook
{
    public partial class MainForm : Form
    {
        private PostalHandbook handbook;
        public MainForm()
        {
            InitializeComponent();

        }
        private void Form1_Load(object sender, EventArgs e)
        {

            handbook = PostalHandbook.LoadData();
            idNumericUpDown.Text = "";
            searchButton_Click(null, null);
        }

        private void searchButton_Click(object? sender, EventArgs? e)
        {
            int? id = null;
            if (idNumericUpDown.Text != "")
            {
                id = (int)idNumericUpDown.Value;
            }
            List<Person> results = handbook.Search(id, nameTextBox.Text,
            surnameTextBox.Text, countryTextBox.Text, regionTextBox.Text,
                districtTextBox.Text, settlementTextBox.Text, postcodeTextBox.Text);

            personBindingSource.DataSource = results;
            editButton.Enabled = deleteButton.Enabled = editToolStripMenuItem.Enabled = deleteToolStripMenuItem.Enabled = results.Count != 0;
            if (results.Count == 0 && sender != null)
            {
                MessageBox.Show("����� ������ �� ��������", "�����������", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void addButton_Click(object sender, EventArgs e)
        {
            PersonAddEditForm personAddForm = new PersonAddEditForm(handbook, null);
            personAddForm.ShowDialog();
            searchButton_Click(null, null);
        }

        private void editButton_Click(object sender, EventArgs e)
        {
            Person? person = GetSelectedPerson();
            if (person == null)
            {
                return;
            }
            PersonAddEditForm personEditForm = new PersonAddEditForm(handbook, person.Id);
            personEditForm.ShowDialog();
            searchButton_Click(null, null);
        }

        private Person? GetSelectedPerson()
        {
            if (resultsDataGridView.SelectedRows.Count == 0)
            {
                MessageBox.Show("������ �� ������", "�������", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return null;
            }
            return resultsDataGridView.CurrentRow.DataBoundItem as Person;
        }

        private void deleteButton_Click(object sender, EventArgs e)
        {
            Person? person = GetSelectedPerson();
            if (person == null)
            {
                return;
            }
            DialogResult result = MessageBox.Show($"�� �������, �� ������ �������� {person.Name} {person.Surname}?", "ϳ�����������", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result == DialogResult.Yes)
            {
                handbook.People.Remove(person);
                handbook.SaveData();
                searchButton_Click(null, null);
            }
        }

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("��������� ��������: ���� ����. \n�������� ���������� ��� ������ ������������ �����, ���� �� ������ �� ������.", "����������", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("�� ��������, �� ������ ����� � ��������?", "ϳ����������", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result == DialogResult.Yes)
            {
                Close();
            }
        }

        private void OnlyLetter_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (Char.IsDigit(e.KeyChar) || e.KeyChar == (char)Keys.Delete)
            {
                e.Handled = true;
            }
        }
    }
}
