using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Xml.Linq;
using System.IO;
using System.Xml;
using System.Text.RegularExpressions;
using System.ComponentModel;
using System.Collections;
using System.Drawing;
using Baseline.ImTools;
using System.Data;
using System.Windows.Controls.Primitives;

namespace WpfApp5
{

    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {


        List<string> l1;
        string _xmlFile = "C:/Users/mayan/source/repos/WpfApp5/WpfApp5/XMLFile1.xml";
        string index_xml = "C:/Users/mayan/source/repos/WpfApp5/WpfApp5/XMLFile4.xml";
        int count;
        int max1;
        int b = 0;
        string input;
        DataTable _dataTable;
        public MainWindow()
        {

            InitializeComponent();

            l1 = new List<string>();

            App.Current.Properties["event_type"] = "none";
            App.Current.Properties["venue_selection"] = "none";
            App.Current.Properties["Order_Id"] = "none";
            int max1;
            XDocument xdoc1 = XDocument.Load("C:/Users/mayan/source/repos/WpfApp5/WpfApp5/XMLFile1.xml");
            var movieNames = xdoc1.Descendants("Events").Attributes("id").Select(g => g.Value);
            foreach (var name in movieNames)
            {
                {

                    var product_date = xdoc1.Descendants("Events")
                   .Where(g => g.Attribute("id").Value == name)
                   .Elements("Date")
                   .Single();

                    var last_name = xdoc1.Descendants("Events")
                   .Where(g => g.Attribute("id").Value == name)
                   .Elements("LName")
                   .Single();

                    string date_picker = product_date.Value.ToString().Split(' ').First();

                    max1 = 0;
                    count = 0;

                    input = date_picker + '\n' + "Customer Name:" + '\t' + last_name.Value;
                }

                lb3.Items.Add(input);
                ids.Items.Add(name);

            }
            sort_list_items();

            _dataTable = new DataTable();
          
            _dataTable.Columns.Add(new DataColumn("First_Name"));
            _dataTable.Columns.Add(new DataColumn("Last_Name"));
            _dataTable.Columns.Add(new DataColumn("Event_Date"));
            _dataTable.Columns.Add(new DataColumn("Occasion"));
            _dataTable.Columns.Add(new DataColumn("Days_Left"));
            dg_view.ColumnWidth = 100;
            dg_view.FontSize = 14;
            

            XDocument xdoc2 = XDocument.Load("C:/Users/mayan/source/repos/WpfApp5/WpfApp5/XMLFile1.xml");
            var values = xdoc1.Descendants("Events").Attributes("id").Select(g => g.Value);
            foreach (var name in values)
            {
                {
                    var product1 = xdoc1.Descendants("Events")
                   .Where(g => g.Attribute("id").Value == name)
                   .Elements("Days")
                   .Single();

                    var product_fname = xdoc1.Descendants("Events")
                   .Where(g => g.Attribute("id").Value == name)
                   .Elements("FName")
                   .Single();

                    var product_lname = xdoc1.Descendants("Events")
                   .Where(g => g.Attribute("id").Value == name)
                   .Elements("LName")
                   .Single();

                    var product_date = xdoc1.Descendants("Events")
                   .Where(g => g.Attribute("id").Value == name)
                   .Elements("Date")
                   .Single();

                    string date_picker = product_date.Value.ToString().Split(' ').First();

                    var product_event = xdoc1.Descendants("Events")
                   .Where(g => g.Attribute("id").Value == name)
                   .Elements("Event_Type")
                   .Single();

                    _dataTable.Rows.Add(
                        product_fname.Value,
                        product_lname.Value,
                        product_date.Value,
                        product_event.Value,
                        product1.Value);
                    dg_view.ItemsSource = _dataTable.DefaultView;
                }
            }

        }

        private void sort_list_items()
        {
            XDocument xdoc1 = XDocument.Load("C:/Users/mayan/source/repos/WpfApp5/WpfApp5/XMLFile1.xml");
            ArrayList list = new ArrayList();
            foreach (var item in lb3.Items)
            {
                list.Add(item);
            }
            list.Sort();
            lb3.Items.Clear();
            foreach (var item in list)
            {
                string days = item.ToString().Split('\n').First();
                string sel_val_name = item.ToString().ToString().Split('\t').Last();

                try
                {

                    var e_date = xdoc1.Descendants("Events")
                           .Where(g => g.Element("Date").Value == days)
                           .Elements("ID")
                           .SingleOrDefault();

                    App.Current.Properties["Order_Id_days"] = "one" + e_date.Value;
                }
                catch (Exception)
                {
                    var e_name = xdoc1.Descendants("Events")
                           .Where(g => g.Element("LName").Value == sel_val_name)
                           .Elements("ID")
                           .SingleOrDefault();

                    App.Current.Properties["Order_Id_days"] = "one" + e_name.Value;
                }

                string str1 = App.Current.Properties["Order_Id_days"].ToString();
                XDocument xdoc = XDocument.Load("C:/Users/mayan/source/repos/WpfApp5/WpfApp5/XMLFile1.xml");
                var Days_Left = xdoc.Descendants("Events")
                    .Where(g => g.Attribute("id").Value == str1)
                    .Elements("Days")
                    .Single();

                if (Convert.ToInt32(Days_Left.Value) < 5)
                {
                    lb3.Items.Add(new ListBoxItem { Content = item, Background = Brushes.Red, Foreground = Brushes.White }) ;
                }
                else if (Convert.ToInt32(Days_Left.Value) < 10 && Convert.ToInt32(Days_Left.Value) > 5)
                {
                    lb3.Items.Add(new ListBoxItem { Content = item, Background = Brushes.Blue, Foreground = Brushes.White });
                }
                else if (Convert.ToInt32(Days_Left.Value) > 10)
                {
                    lb3.Items.Add(new ListBoxItem { Content = item, Background = Brushes.Green, Foreground = Brushes.White });
                }
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            string selected_id = App.Current.Properties["Order_Id"].ToString();
            if (ids.Items.Contains(selected_id))
            {
                int result = 0;
                if ((Convert.ToDateTime(date.SelectedDate.ToString()) < Convert.ToDateTime(days_left.Content)))
                {
                    MessageBox.Show(" Cannot select a past date");
                }
                else
                {
                    for (int x = 0; x <= (Convert.ToDateTime(date.SelectedDate.ToString()) - Convert.ToDateTime(days_left.Content)).TotalDays; x++)
                    {
                        result = result + 1;
                    }

                    string t_event1 = Event_Type_Binding.Content.ToString();

                    App.Current.Properties["event_type"] = t_event1;

                    var date_picker = date.SelectedDate.Value.ToString("MM/dd/yyyy").Split(' ').First();

                    XDocument doc_i = XDocument.Load("C:/Users/mayan/source/repos/WpfApp5/WpfApp5/XMLFile1.xml");

                    var product_dr = doc_i.Descendants("Events").Single(p => p.Attribute("id").Value.Equals(selected_id));
                    product_dr.SetElementValue("FName", T1.Text);
                    product_dr.SetElementValue("LName", T2.Text);
                    
                    product_dr.SetElementValue("Date", date_picker);
                    product_dr.SetElementValue("Days", result.ToString());

                    doc_i.Save("C:/Users/mayan/source/repos/WpfApp5/WpfApp5/XMLFile1.xml");
                    MessageBox.Show("Saved!");
                }


            }
            else
            {
                if ((T1.Text == string.Empty) | (T2.Text == string.Empty) | (date.SelectedDate.ToString() == string.Empty) | (event_list.SelectedIndex == -1))
                {
                    if (T1.Text == string.Empty)
                    {
                        T1.BorderBrush = Brushes.Red;
                    }
                    if (T2.Text == string.Empty)
                    {
                        T2.BorderBrush = Brushes.Red;
                    }
                    if (date.SelectedDate.ToString() == string.Empty)
                    {
                        date.BorderBrush = Brushes.Red;
                    }
                    if (event_list.SelectedIndex == -1)
                    {
                        event_list.BorderBrush = Brushes.Red;
                    }

                    MessageBox.Show("Select all fields");

                }
                else
                {
                    XDocument xdoc = XDocument.Load("C:/Users/mayan/source/repos/WpfApp5/WpfApp5/XMLFile1.xml");
                    XDocument doc_i = XDocument.Load(index_xml);
                    var booking_id = doc_i.Descendants("Bookings").Elements("Booking_ID").Single();
                    int i = Convert.ToInt32(booking_id.Value);
                    int result = 0;
                    string j = "one" + i.ToString();
                    for (int x = 0; x <= (Convert.ToDateTime(date.SelectedDate.ToString()) - Convert.ToDateTime(days_left.Content)).TotalDays; x++)
                    {
                        result = result + 1;
                    }

                    string t_event1 = Event_Type_Binding.Content.ToString();

                    App.Current.Properties["event_type"] = t_event1;


                    var date_picker = date.SelectedDate.Value.ToString("MM/dd/yyyy");


                    XDocument doc = XDocument.Load(_xmlFile);
                    doc.Root.Add(new XElement("Events",
                        new XAttribute("id", j),
                        new XElement("ID", i),
                        new XElement("FName", T1.Text),
                        new XElement("LName", T2.Text),
                        new XElement("Date", date_picker.ToString().Split(' ').First()),
                        new XElement("Event_Type", t_event1),
                        new XElement("Venue", " "),
                        new XElement("Food", " "),
                        new XElement("Guest_No", " "),
                        new XElement("Venue_Cost", " "),
                        new XElement("Cake", " "),
                        new XElement("Days", result.ToString())

                    ));
                    doc.Save(_xmlFile);

                    XDocument doc_dr = XDocument.Load("C:/Users/mayan/source/repos/WpfApp5/WpfApp5/XMLFile3.xml");

                    doc_dr.Root.Add(new XElement("Drinks",
                        new XAttribute("id", j),
                        new XElement("A_drinks_count", 0),
                        new XElement("NA_drinks_count", 0)));
                    doc_dr.Save("C:/Users/mayan/source/repos/WpfApp5/WpfApp5/XMLFile3.xml");

                    XDocument doc_f = XDocument.Load("C:/Users/mayan/source/repos/WpfApp5/WpfApp5/XMLFile2.xml");

                    doc_f.Root.Add(new XElement("Food",
                        new XAttribute("id", j),
                        new XElement("V_food_count", 0),
                        new XElement("kid_v_food_count", 0),
                        new XElement("kid_nv_food_count", 0),
                        new XElement("des_count", 0),
                        new XElement("NV_food_count", 0)));
                    doc_f.Save("C:/Users/mayan/source/repos/WpfApp5/WpfApp5/XMLFile2.xml");

                    var results = doc.Descendants("Product").Select(x => new
                    {
                        ID = x.Element("ID").Value,
                        Name = x.Element("Name").Value,

                    });
                    i = i + 1;

                    j = "one" + i.ToString();
                    App.Current.Properties["Order_Id"] = j.ToString();
                    
                    event_list.BorderBrush = Brushes.Black;
                    T1.BorderBrush = Brushes.Black;
                    T2.BorderBrush = Brushes.Black;
                    date.BorderBrush = Brushes.Black;
                    ids.Items.Clear();
                    XDocument xdoc2 = XDocument.Load("C:/Users/mayan/source/repos/WpfApp5/WpfApp5/XMLFile1.xml");
                    var ev_id = xdoc2.Descendants("Events").Attributes("id").Select(g => g.Value);
                    foreach (var name in ev_id)
                    {
                        ids.Items.Add(name);
                    }
                    MessageBox.Show("Saved!");

                }

            }
            lb3.Items.Clear();
            string col;
            XDocument xdoc1 = XDocument.Load("C:/Users/mayan/source/repos/WpfApp5/WpfApp5/XMLFile1.xml");
            var movieNames = xdoc1.Descendants("Events").Attributes("id").Select(g => g.Value);
            Brush myBrush = Brushes.White;
            foreach (var name in movieNames)
            {
                {
                    var product_date = xdoc1.Descendants("Events")
                    .Where(g => g.Attribute("id").Value == name)
                    .Elements("Date")
                    .Single();

                    var last_name = xdoc1.Descendants("Events")
                   .Where(g => g.Attribute("id").Value == name)
                   .Elements("LName")
                   .Single();

                    var days = xdoc1.Descendants("Events")
                   .Where(g => g.Attribute("id").Value == name)
                   .Elements("Days")
                   .Single();

                    string date_picker = product_date.Value.ToString().Split(' ').First();

                    input = date_picker + '\n' + "Customer Name:" + '\t' + last_name.Value ;
                    

                    
                }
                if (lb3.Items.Contains(input))
                { }
                else
                {
                    lb3.Items.Add(input);

                }

            }
            sort_list_items();
            
            T1.Text = string.Empty;
            T2.Text = string.Empty;
            event_list.SelectedIndex = -1;
            date.SelectedDate = null;
            XDocument doc_b = XDocument.Load(index_xml);
            var booking_id1 = doc_b.Descendants("Bookings").Elements("Booking_ID").Single();
            int i1 = Convert.ToInt32(booking_id1.Value);
            int update_counter1 = i1 + 1;
            var counter1 = doc_b.Descendants("Bookings").Single();
            counter1.SetElementValue("Booking_ID", update_counter1);
            doc_b.Save("C:/Users/mayan/source/repos/WpfApp5/WpfApp5/XMLFile4.xml");

        }

        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            this.Hide();
            Window1 w = new Window1();
            w.ShowDialog();
        }

        private void Button_Click_4(object sender, RoutedEventArgs e)
        {
            
                ed.Visibility = Visibility.Visible;
                clear_selection.Visibility = Visibility.Visible;

                string sel_val_date = event_id.Content.ToString().Split('\n').First();
                string sel_val_name = event_id.Content.ToString().Split('\t').Last();

                try
                {
                    XDocument xdoc1 = XDocument.Load("C:/Users/mayan/source/repos/WpfApp5/WpfApp5/XMLFile1.xml");
                    var e_date = xdoc1.Descendants("Events")
                           .Where(g => g.Element("Date").Value == sel_val_date)
                           .Elements("ID")
                           .SingleOrDefault();

                    App.Current.Properties["Order_Id"] = "one" + e_date.Value;
                }
                catch (Exception)
                {
                    XDocument xdoc1 = XDocument.Load("C:/Users/mayan/source/repos/WpfApp5/WpfApp5/XMLFile1.xml");
                    var e_name = xdoc1.Descendants("Events")
                           .Where(g => g.Element("LName").Value == sel_val_name)
                           .Elements("ID")
                           .SingleOrDefault();

                    App.Current.Properties["Order_Id"] = "one" + e_name.Value;
                }
                

                string str1 = App.Current.Properties["Order_Id"].ToString();
                XDocument xdoc = XDocument.Load("C:/Users/mayan/source/repos/WpfApp5/WpfApp5/XMLFile1.xml");
                var FName = xdoc.Descendants("Events")
                    .Where(g => g.Attribute("id").Value == str1)
                    .Elements("FName")
                    .Single();
                T1.Text = FName.Value;
                string fname = FName.Value;

                var LName = xdoc.Descendants("Events")
                    .Where(g => g.Attribute("id").Value == str1)
                    .Elements("LName")
                    .Single();
                T2.Text = LName.Value;
                string lname = LName.Value;

                var Date = xdoc.Descendants("Events")
                    .Where(g => g.Attribute("id").Value == str1)
                    .Elements("Date")
                    .Single();
                string[] words = Date.Value.ToString().Split('/');
                
            string date_picker1 = words[1] + "/"+ words[0] + "/" + words[2];
            date.SelectedDate = Convert.ToDateTime(date_picker1);

            var t_event = xdoc.Descendants("Events")
                    .Where(g => g.Attribute("id").Value == str1)
                    .Elements("Event_Type")
                    .Single();
                string t_event1 = t_event.Value;

                if (t_event1 == "Birthday")
                {
                    event_list.SelectedIndex = event_list.Items.Count - 2;
                    event_list.Focus();
                }
                else
                {
                    event_list.SelectedIndex = event_list.Items.Count - 1;
                    event_list.Focus();
                }


                App.Current.Properties["event_type"] = t_event1;
            
            
        }

        private void Button_Click_5(object sender, RoutedEventArgs e)
        {
            XDocument xdoc1 = XDocument.Load("C:/Users/mayan/source/repos/WpfApp5/WpfApp5/XMLFile1.xml");
            var movieNames = xdoc1.Descendants("Events").Attributes("id").Select(g => g.Value);
            foreach (var name in movieNames)
            {
                {
                    var product1 = xdoc1.Descendants("Events")
                   .Where(g => g.Attribute("id").Value == name)
                   .Elements("Days")
                   .Single();
                    count = 0;
                    int str1 = Convert.ToInt32(product1.Value.ToString().Split(' ').Last());
                    while (str1 > 0)
                    {
                        str1 = str1 / 10;
                        count++;

                    }

                    if (count == 1)
                    {
                        input = "00" + product1.Value + " : " + name;

                    }
                    else if (count == 2)
                    {
                        input = "0" + product1.Value + " : " + name;

                    }
                    else if (count == 3)
                    {
                        input = product1.Value + " : " + name;

                    }
                    if (lb3.Items.Contains(input))
                    { }
                    else
                    {
                        lb3.Items.Add(input);
                    }

                }

            }

            ArrayList list = new ArrayList();
            foreach (var item in lb3.Items)
            {
                list.Add(item);
            }
            list.Sort();
            lb3.Items.Clear();
            foreach (var item in list)
            {
                lb3.Items.Add(item);
            }
        }

        private void Button_Click_7(object sender, RoutedEventArgs e)
        {
            string search_string = search_lb.Text.ToLower();
            int lb_count = lb3.Items.Count;
            for (int i = lb_count - 1; i >= 0; i--)
            {
                string filter = lb3.Items.GetItemAt(i).ToString().ToLower();
                if (filter.Contains(search_string))
                {
                    lb3.SelectedIndex = Convert.ToInt32(i);
                    lb3.Focus();
                }
            }
        }



        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            



        }
        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            
            string filter = tb1.Text;
            if(filter1.Content.ToString() == "ALL")
            {
                if (string.IsNullOrEmpty(filter))
                    _dataTable.DefaultView.RowFilter = null;
                else
                    _dataTable.DefaultView.RowFilter = string.Format("First_Name Like '%{0}%' OR Last_Name Like '%{0}%' OR Event_Date Like '%{0}%' OR Occasion Like '%{0}%' OR Days_Left Like '%{0}%'", filter);
                row_count.Visibility = Visibility.Visible;
                row_count.Text = "Total number of records: " + (dg_view.Items.Count - 1).ToString();
            }
            else if (filter1.Content.ToString() == "First Name")
            {
                if (string.IsNullOrEmpty(filter))
                    _dataTable.DefaultView.RowFilter = null;
                else
                    _dataTable.DefaultView.RowFilter = string.Format("First_Name Like '%{0}%'", filter);
                row_count.Visibility = Visibility.Visible;
                row_count.Text = "Total number of records: " + (dg_view.Items.Count - 1).ToString();
            }
            else if (filter1.Content.ToString() == "Last Name")
            {
                if (string.IsNullOrEmpty(filter))
                    _dataTable.DefaultView.RowFilter = null;
                else
                    _dataTable.DefaultView.RowFilter = string.Format("Last_Name Like '%{0}%'", filter);
                row_count.Visibility = Visibility.Visible;
                row_count.Text = "Total number of records: " + (dg_view.Items.Count - 1).ToString();
            }
            else if (filter1.Content.ToString() == "Event Date")
            {
                if (string.IsNullOrEmpty(filter))
                    _dataTable.DefaultView.RowFilter = null;
                else
                    _dataTable.DefaultView.RowFilter = string.Format("Event_Date Like '%{0}%'", filter);
                row_count.Visibility = Visibility.Visible;
                row_count.Text = "Total number of records: " + (dg_view.Items.Count - 1).ToString();
            }
            else if (filter1.Content.ToString() == "Occasion")
            {
                if (string.IsNullOrEmpty(filter))
                    _dataTable.DefaultView.RowFilter = null;
                else
                    _dataTable.DefaultView.RowFilter = string.Format("Occasion Like '%{0}%'", filter);
                row_count.Visibility = Visibility.Visible;
                row_count.Text = "Total number of records: " + (dg_view.Items.Count - 1).ToString();
            }
            else if (filter1.Content.ToString() == "Number of Guests")
            {
                if (string.IsNullOrEmpty(filter))
                    _dataTable.DefaultView.RowFilter = null;
                else
                    _dataTable.DefaultView.RowFilter = string.Format("Days_Left Like '%{0}%'", filter);
                row_count.Visibility = Visibility.Visible;
                row_count.Text = "Total number of records: " + (dg_view.Items.Count - 1).ToString();
            }

        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            string filter = "khare";
            if (string.IsNullOrEmpty(filter))
                _dataTable.DefaultView.RowFilter = null;
            else
                _dataTable.DefaultView.RowFilter = string.Format("Last_Name Like '%{0}%'", filter);

        }

        private void Button_Click_6(object sender, RoutedEventArgs e)
        {
            ICollectionView view = CollectionViewSource.GetDefaultView(dg_view.ItemsSource);
            view.SortDescriptions.Clear();
            SortDescription sd = new SortDescription("Days_Left", ListSortDirection.Ascending);
            view.SortDescriptions.Add(sd);

        }

        private void search_lb_TextChanged(object sender, TextChangedEventArgs e)
        {
            
            string search_string = search_lb.Text;
            int lb_count = lb3.Items.Count;
            if(cbb.Content.ToString() == "Name")
            {
                for (int i = lb_count - 1; i >= 0; i--)
                {
                   
                    string filter1 = lb3.Items.GetItemAt(i).ToString().ToLower().Split('\t').Last();

                    if (filter1.StartsWith(search_string))
                    {
                        lb3.SelectedIndex = Convert.ToInt32(i);
                        lb3.Focus();
                    }

                }
            }

            else if (cbb.Content.ToString() == "Date")
            {
                for (int i = lb_count - 1; i >= 0; i--)
                {

                    string filter1 = lb3.Items.GetItemAt(i).ToString().ToLower().Split('\n').First();

                    if (filter1.Contains(search_string))
                    {
                        lb3.SelectedIndex = Convert.ToInt32(i);
                        lb3.Focus();
                    }

                }
            }



        }
   

        private void clear_sel(object sender, MouseEventArgs e)
        {
            T1.Text = string.Empty;
            T2.Text = string.Empty;
            event_list.SelectedIndex = -1;
            date.SelectedDate = Convert.ToDateTime(days_left.Content);
            App.Current.Properties["Order_Id"] = string.Empty;
        }

        
    }
}


/* lb3.Items.Cast<string>().Any(s => Regex.IsMatch(s, search_string))
 * lb3.Items[i].ToString().ToLower().Contains(search_string.ToLower())
 for(int i=lb3.Items.Count-1; i >=0; i--)
var product_event = xdoc1.Descendants("Events")

private void vd_Copy_Click(object sender, RoutedEventArgs e)
            {
                string sel_val_date = lb3.SelectedItem.ToString().ToString().Split('\n').First();
                string sel_val_name = lb3.SelectedItem.ToString().ToString().Split('\t').Last();

                try
                {
                    XDocument xdoc1 = XDocument.Load("C:/Users/mayan/source/repos/WpfApp5/WpfApp5/XMLFile1.xml");
                    var e_date = xdoc1.Descendants("Events")
                           .Where(g => g.Element("Date").Value == sel_val_date)
                           .Elements("ID")
                           .SingleOrDefault();
                    MessageBox.Show("one" + e_date.Value);
                }
                catch (Exception)
                {
                    XDocument xdoc1 = XDocument.Load("C:/Users/mayan/source/repos/WpfApp5/WpfApp5/XMLFile1.xml");
                    var e_name = xdoc1.Descendants("Events")
                           .Where(g => g.Element("LName").Value == sel_val_name)
                           .Elements("ID")
                           .SingleOrDefault();
                    MessageBox.Show("one" + e_name.Value);
                }

            }


*/