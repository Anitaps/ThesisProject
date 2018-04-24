using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Widget;
using Plugin.BLE;
using Plugin.BLE.Abstractions.Contracts;
using SQLite;
using System.Data.SqlClient;

namespace LatestApp
{
    [Activity(Label = "MainActivity")]
    public class MainActivity : Activity
    {
        string dbPath = Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal), "dbTest.db3");
        private const string devId = "b9407f30-f5f8-466e-aff9-25556b57fe6d";
        private Guid beaconId = Guid.Parse(devId);
        IDevice device;
        IBluetoothLE ble;
        Plugin.BLE.Abstractions.Contracts.IAdapter adapter;
        System.Collections.ObjectModel.ObservableCollection<IDevice> deviceList;
        ListView listView;
        private static string prevLocation;
        Contact mycontact;
        private static string clientId = UserLogin.User;
        private static SqlConnectionStringBuilder databaseAddress;
        private static SqlConnection connection;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.Main);

            databaseAddress = createAddress();
            connection = new SqlConnection(databaseAddress.ConnectionString);
            // Create your application here
            TextView scanview = FindViewById<TextView>(Resource.Id.scanview);
            TextView txt = FindViewById<TextView>(Resource.Id.txtdata);
            listView = FindViewById<ListView>(Resource.Id.listView1);
            txt.Text += clientId;

            ble = CrossBluetoothLE.Current;
            adapter = CrossBluetoothLE.Current.Adapter;
            deviceList = new ObservableCollection<IDevice>();


            Button Test = FindViewById<Button>(Resource.Id.btnTest);
            Test.Click += delegate
            {

                var state = ble.State;
                var info = ble.State.ToString();
                new AlertDialog.Builder(this)
                    .SetMessage(info)
                    .SetTitle("Bluetooth State")
                    .Show();

            };

            Button button = FindViewById<Button>(Resource.Id.dbbutton);
            button.Click += delegate
            {
                //var db = new SQLiteConnection(dbPath);
                ////set up table
                //db.CreateTable<Contact>();

                ////create new contact obj

                Contact myContact = new Contact(clientId, prevLocation);

                ////store obj into table
                //db.Insert(myContact);
                connection.Open();
                StringBuilder query = new StringBuilder();
                query.Append("INSERT INTO UsersTable(Username, Location)VALUES('" + myContact.Name + "', '" + myContact.Location + "')");
                string sqlquery = query.ToString();
                SqlCommand command = new SqlCommand(sqlquery, connection);
                command.ExecuteNonQuery();
                connection.Close();
            };
            Button showbutton = FindViewById<Button>(Resource.Id.showbtn);
            showbutton.Click += delegate
            {   //set up db connection
                var db = new SQLiteConnection(dbPath);

                //connect to table
                var table = db.Table<Contact>();

                foreach (var item in table)
                {
                    Contact myContact = new Contact(item.Name, item.Location);
                    txt.Text += myContact + "\n";
                }
            };


            Button BLEscan = FindViewById<Button>(Resource.Id.scanner);

            BLEscan.Click += async delegate

            {
                scanview.Text = "";

                deviceList.Clear();

                adapter.DeviceDiscovered += (s, a) =>
                {
                    deviceList.Add(a.Device);
                };
                await adapter.StartScanningForDevicesAsync();
                scanview.Text += "Device count:" + deviceList.Count;

                List<IDevice> x = deviceList
                .Where(device => device.Name != null) //&& device.Rssi > -30)
                .ToList();
                scanview.Text += "Var X:" + x.Count;

                CreateList(x);
                prevLocation = x.ElementAt(0).Name;
            };

        }


        void CreateList(List<IDevice> bleList)
        {
            IListAdapter listAdapter = new ArrayAdapter<IDevice>(this, Android.Resource.Layout.SimpleListItem1, bleList);
            listView.Adapter = listAdapter;
            listView.ItemClick += listView_itemClick;
        }


        void listView_itemClick(object sender, AdapterView.ItemClickEventArgs e)
        {
            var i = e.Position;
            var info = $"Name:{deviceList[i].Name}Id:{deviceList[i].Id}NativeDevice:{deviceList[i].NativeDevice}Rssi:{deviceList[i].Rssi}State:{deviceList[i].State}\n\n";
            Toast.MakeText(this, info, ToastLength.Long).Show();
        }

        void CreateContact(Contact mycontact, List<IDevice> bleList)
        {
            var db = new SQLite.SQLiteConnection(dbPath);
            //set up table
            db.CreateTable<Contact>();
            mycontact.Location = bleList.ElementAt(0).Name;
            mycontact.Name = "Anita";

            //store obj into table
            db.Insert(mycontact);
        }
        private SqlConnectionStringBuilder createAddress()
        {
            SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder();
            builder.DataSource = "Server=tcp:myappdb.database.windows.net";
            builder.UserID = "pushpendra";
            builder.Password = "vHfj8P4j";
            builder.InitialCatalog = "UsersDB";

            return builder;
        }
    }
   
}



